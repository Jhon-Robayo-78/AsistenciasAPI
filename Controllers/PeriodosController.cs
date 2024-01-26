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
    public class PeriodosController : ControllerBase
    {
        //para la conexion con .env
        private string _connection;
        public PeriodosController()
        {
            DotEnv.Load();
            var envVar = DotEnv.Read();
            _connection = $"{envVar["StringConnetion"]}";
        }

        [HttpGet("{P_Academico}")]
        public async Task<ActionResult<Periodo>> Getperiodo(string P_Academico)
        {
            try
            {
                IEnumerable<Periodo> result;
                using (var db = new MySqlConnection(_connection))
                {
                    var query = $"SELECT * FROM periodo where P_Academico='{P_Academico}'";
                    result = await db.QueryAsync<Periodo>(query);
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> InsertPeriodo(Periodo newPeriodo)
        {
            int result = 0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    // Consulta para verificar si ya existe un presupuesto para el usuario
                    var queryValidate = $"SELECT COUNT(*) FROM periodo WHERE P_Academico='{newPeriodo.P_Academico}'";
                    int count = await db.ExecuteScalarAsync<int>(queryValidate);

                    // Verificar el resultado
                    if (count > 0)
                    {
                        // Ya existe un presupuesto para este usuario, puedes manejar el error o simplemente no hacer la inserción
                        return Unauthorized($"Ya existe {newPeriodo.P_Academico}");
                    }

                    // Si no existe, realizar la inserción
                    var querySql = "INSERT INTO periodo VALUES(@P_Academico)";
                    result = await db.ExecuteAsync(querySql, newPeriodo);
                }
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{P_Academico}")]
        public async Task<IActionResult> DeletePeriodo(string P_Academico)
        {
            int result;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    string querySql = $"DELETE FROM periodo WHERE P_Academico='{P_Academico}'";
                    result = await db.ExecuteAsync(querySql);
                }
                return Ok(result);
            }catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "admin")]
        [HttpPut("{P_Academico}")]
        public async Task<IActionResult> UpdatePeriodo(string P_Academico, Periodo update)
        {
            int result = 0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    string querySql = $"UPDATE periodo SET P_Academico=@P_Academico WHERE P_Academico='{P_Academico}'";
                    result = await db.ExecuteAsync(querySql, update);
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }

        }
    }
}

