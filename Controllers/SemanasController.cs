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
    public class SemanasController : ControllerBase
    {
        //para la conexion con .env
        private string _connection;
        public SemanasController()
        {
            DotEnv.Load();
            var envVar = DotEnv.Read();
            _connection = $"{envVar["StringConnetion"]}";
        }

        [HttpGet]
        public async Task<ActionResult<Semana>> GetUser([FromQuery] int noSemana, [FromQuery] string noPeriodo)
        {
            try
            {
                IEnumerable<Semana> result;
                using (var db = new MySqlConnection(_connection))
                {
                    var query = $"SELECT * FROM Semanas where No_Semana={noSemana} and periodo='{noPeriodo}'";
                    result = await db.QueryAsync<Semana>(query);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> InsertUser(Semana semana)
        {
            int result = 0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    // Consulta para verificar si ya existe un presupuesto para el usuario
                    var queryValidate = $"SELECT COUNT(*) FROM Semanas WHERE Fecha_Inicio='{semana.Fecha_Inicio}' and Fecha_Fin='{semana.Fecha_Fin}'";
                    int count = await db.ExecuteScalarAsync<int>(queryValidate);

                    // Verificar el resultado
                    if (count > 0)
                    {
                        // Ya existe un presupuesto para este usuario, puedes manejar el error o simplemente no hacer la inserción
                        return Unauthorized("Ya existe esa semana");
                    }

                    // Si no existe, realizar la inserción
                    var querySql = "INSERT INTO Semanas VALUES(0, @NoSemana, @FechaInicio, @FechaFin, @Periodo)";
                    result = await db.ExecuteAsync(querySql, semana);
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            int result;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    string querySql = $"DELETE FROM Semanas WHERE id={id}";
                    result = await db.ExecuteAsync(querySql);
                }
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, Semana user)
        {
            int result = 0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    string querySql = $"UPDATE Semanas SET No_Semana=@NoSemana, Fecha_Inicio=@Fecha_Inicio, Fecha_Fin=@Fecha_Fin, periodo=@Periodo WHERE id={id}";
                    result = await db.ExecuteAsync(querySql, user);
                }
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
