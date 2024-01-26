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
    public class MatriculasController : ControllerBase
    {
        //para la conexion con .env
        private string _connection;
        public MatriculasController()
        {
            DotEnv.Load();
            var envVar = DotEnv.Read();
            _connection = $"{envVar["StringConnetion"]}";
        }

        [HttpGet]
        public async Task<ActionResult<Matricula>> GetUser([FromQuery] string id, [FromQuery] string periodo)
        {
            try
            {
                IEnumerable<Matricula> result;
                using (var db = new MySqlConnection(_connection))
                {
                    var query = $"SELECT * FROM Matricula where codigoEstudiante='{id}' and periodo='{periodo}'";
                    result = await db.QueryAsync<Matricula>(query);
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "admin,stud")]
        [HttpPost]
        public async Task<IActionResult> InsertUser(Matricula clase)
        {
            int result = 0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    // Consulta para verificar si ya existe un presupuesto para el usuario
                    var queryValidate = $"SELECT COUNT(*) FROM Matricula WHERE codigoEstudiante='{clase.codigoEstudiante}' and nrc={clase.nrc} and periodo='{clase.periodo}'";
                    int count = await db.ExecuteScalarAsync<int>(queryValidate);

                    // Verificar el resultado
                    if (count > 0)
                    {
                        // Ya existe un presupuesto para este usuario, puedes manejar el error o simplemente no hacer la inserción
                        return Unauthorized($"Ya matriculo la materia con nrc: {clase.nrc}");
                    }

                    // Si no existe, realizar la inserción
                    var querySql = "INSERT INTO Matricula VALUES(@id_matricula, @codigoEstudiante, @id_Horario, @inscrito, @periodo, @nrc)";
                    result = await db.ExecuteAsync(querySql, clase);
                }
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "admin,stud")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            int result;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    string querySql = $"DELETE FROM Matricula WHERE id_matricula={id}";
                    result = await db.ExecuteAsync(querySql);
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);  
            }

        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, Matricula matricula)
        {
            int result = 0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    string querySql = $"UPDATE Matricul SET nrc=@nrc, inscrito=@inscrito id_horario=@id_horario WHERE id='{id}'";
                    result = await db.ExecuteAsync(querySql, matricula);
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}