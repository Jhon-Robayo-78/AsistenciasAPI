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
    public class HorariosController : ControllerBase
    {
        //para la conexion con .env
        private string _connection;
        public HorariosController()
        {
            DotEnv.Load();
            var envVar = DotEnv.Read();
            _connection = $"{envVar["StringConnetion"]}";
        }

        [HttpGet("{NRC}")]
        public async Task<ActionResult<Horario>> GetHorario(int NRC)
        {
            try
            {
                IEnumerable<Horario> result;
                using (var db = new MySqlConnection(_connection))
                {
                    var query = $"SELECT * FROM Horario where NRC={NRC}";
                    result = await db.QueryAsync<Horario>(query);
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> InsertHorario(Horario newHorario)
        {
            int result = 0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    // Consulta para verificar si ya existe un presupuesto para el usuario
                    var queryValidate = $"SELECT COUNT(*) FROM Clase WHERE id={newHorario.Id}";
                    int count = await db.ExecuteScalarAsync<int>(queryValidate);

                    // Verificar el resultado
                    if (count == 0)
                    {
                        // Ya existe un presupuesto para este usuario, puedes manejar el error o simplemente no hacer la inserción
                        return Unauthorized($"No se puede crear horario si no existe la clase: {newHorario.Nrc}");
                    }

                    // Si no existe, realizar la inserción
                    var querySql = "INSERT INTO Horario VALUES(0, @Edf, @Salon, @Nrc, @hora_inicio, @hora_fin)";
                    result = await db.ExecuteAsync(querySql, newHorario);
                }
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHorario(string id)
        {
            int result;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    string querySql = $"DELETE FROM Horario WHERE id='{id}'";
                    result = await db.ExecuteAsync(querySql);
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHorario(string id, Horario horario)
        {
            int result = 0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    string querySql = $"UPDATE Horario SET Edf=@Edf,Salon=@Salon,NRC=@Nrc,hora_inicio=@hora_inicio,hora_fin=@hora_fin WHERE id='{id}'";
                    result = await db.ExecuteAsync(querySql, horario);
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
