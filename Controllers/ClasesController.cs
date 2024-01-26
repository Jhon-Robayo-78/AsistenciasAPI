using AsistenciasApi.Models;
using dotenv.net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authorization;

namespace AsistenciasApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClasesController : ControllerBase
    {
        //para la conexion con .env
        private string _connection;
        public ClasesController()
        {
            DotEnv.Load();
            var envVar = DotEnv.Read();
            _connection = $"{envVar["StringConnetion"]}";
        }

        [HttpGet("{NRC}")]
        public async Task<ActionResult<Clase>> GetClass(int NRC)
        {
            try
            {
                IEnumerable<Clase> result;
                using (var db = new MySqlConnection(_connection))
                {
                    var query = $"SELECT * FROM Clase where NRC='{NRC}'";
                    result = await db.QueryAsync<Clase>(query);
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> InsertClass(Clase newClase)
        {
            int result = 0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    // Consulta para verificar si ya existe un presupuesto para el usuario
                    var queryValidate = $"SELECT COUNT(*) FROM Clase WHERE NRC='{newClase.Nrc}'";
                    int count = await db.ExecuteScalarAsync<int>(queryValidate);

                    // Verificar el resultado
                    if (count > 0)
                    {
                        // Ya existe un presupuesto para este usuario, puedes manejar el error o simplemente no hacer la inserción
                        return Unauthorized($"Ya existe un la clase: {newClase.Nrc}");
                    }

                    // Si no existe, realizar la inserción
                    var querySql = "INSERT INTO Useruniversity VALUES(@Nrc, @No_Students, @Nombre, @Inicio, @Fin, @Tipo, @Materia, @P_Academico)";
                    result = await db.ExecuteAsync(querySql, newClase);
                }
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{NRC}")]
        public async Task<IActionResult> DeleteClass(int NRC)
        {
            int result;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {

                    string querySql = $"DELETE FROM Clase WHERE NRC='{NRC}'";
                    result = await db.ExecuteAsync(querySql);
                }
                return Ok(result);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "admin")]
        [HttpPut("{NRC}")]
        public async Task<IActionResult> UpdateClass(int NRC, Clase alterClass)
        {
            int result = 0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    var queryValidate = $"SELECT COUNT(*) FROM Clase WHERE NRC='{alterClass.Nrc}'";
                    int count = await db.ExecuteScalarAsync<int>(queryValidate);
                    if (count > 0)
                    {
                        return Unauthorized($"ya existe ese NRC:{alterClass.Nrc}");
                    }
                    string querySql = $"UPDATE Clase SET NRC=@Nrc,No_Students=@No_Students,Nombre=@Nombre,Inicio=@Inicio,Fin=@Fin,Tipo=@Tipo,materia=@Materia,P_Academico=@P_Academico WHERE NRC='{NRC}'";
                    result = await db.ExecuteAsync(querySql, alterClass);
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

