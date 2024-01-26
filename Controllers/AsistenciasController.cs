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
    public class AsistenciasController : ControllerBase
    {
        //para la conexion con .env
        private string _connection;
        public AsistenciasController()
        {
            DotEnv.Load();
            var envVar = DotEnv.Read();
            _connection = $"{envVar["StringConnetion"]}";
        }

        [HttpGet]
        public async Task<ActionResult<Asistencium>> GetUser([FromQuery] string id, [FromQuery] string nrc)
        {
            try
            {
                IEnumerable<Asistencium> result;
                using (var db = new MySqlConnection(_connection))
                {
                    var query = $"SELECT * FROM asistencia WHERE id_User='{id}' and nrc={nrc}";
                    result = await db.QueryAsync<Asistencium>(query);
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "prof,stud")]
        [HttpPost]
        public async Task<IActionResult> InsertUser(Asistencium data)
        {
            int result = 0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    // Consulta para verificar si ya existe un presupuesto para el usuario
                    var queryValidate = $"SELECT COUNT(*) FROM Asistencia WHERE id_User='{data.Id}' and id_horario={data.id_Horario}";
                    int count = await db.ExecuteScalarAsync<int>(queryValidate);

                    // Verificar el resultado
                    if (count > 0)
                    {
                        // Ya existe un presupuesto para este usuario, puedes manejar el error o simplemente no hacer la inserción
                        return Unauthorized("Ya marco su asistencia");
                    }

                    // Si no existe, realizar la inserción
                    var querySql = "INSERT INTO Asistencia VALUES(@id_User, @id_Horario, @Id, @No_Semana, @Dato, @nrc)";
                    result = await db.ExecuteAsync(querySql, data);
                }
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "admin,prof")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromQuery] string id,[FromQuery] string idUser)
        {
            int result;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    string querySql = $"DELETE FROM Asistencia WHERE id_horario='{id}' and id_User='{idUser}'";
                    result = await db.ExecuteAsync(querySql);
                }
                return Ok(result);
            } catch(Exception ex) { 
                return BadRequest(ex.Message);
            }

        }
        [Authorize(Roles = "admin,prof")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromQuery] string id, Asistencium user)
        {
            int result = 0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    string querySql = $"UPDATE Asistencia SET Dato='{user.Dato}'  WHERE id='{id}'";
                    result = await db.ExecuteAsync(querySql, user);
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

