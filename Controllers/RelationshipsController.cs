using AsistenciasApi.Models;
using dotenv.net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.AspNetCore.Authorization;
namespace AsistenciasApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RelationshipsController : ControllerBase
    {
        //para la conexion con .env
        private string _connection;
        public RelationshipsController()
        {
            DotEnv.Load();
            var envVar = DotEnv.Read();
            _connection = $"{envVar["StringConnetion"]}";
        }

        [HttpGet]
        public async Task<ActionResult<ProfessorWithMaterium>> GetUser([FromQuery] string idProfesor, [FromQuery] int nrc)
        {
            try
            {
                IEnumerable<ProfessorWithMaterium> result;
                using (var db = new MySqlConnection(_connection))
                {
                    var query = $"SELECT * FROM ProfessorWithMateria where Fk_Id='{idProfesor}' and NRC={nrc}";
                    result = await db.QueryAsync<ProfessorWithMaterium>(query);
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> InsertUser(ProfessorWithMaterium relation, [FromQuery] string periodo)
        {
            int result = 0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    // Consulta para verificar si ya existe un presupuesto para el usuario
                    var queryValidate = $"SELECT COUNT(*) FROM ProfessorWithMateria WHERE Fk_Id='{relation.Fk_Id}' and NRC={relation.Nrc} and P_Academico='{periodo}'";
                    int count = await db.ExecuteScalarAsync<int>(queryValidate);

                    // Verificar el resultado
                    if (count > 0)
                    {
                        // Ya existe un presupuesto para este usuario, puedes manejar el error o simplemente no hacer la inserción
                        return Unauthorized($"Ya el id: {relation.Fk_Id} ya esta relcionado con el nrc:{relation.Nrc} en periodo:{periodo}");
                    }

                    // Si no existe, realizar la inserción
                    var querySql = "INSERT INTO ProfessorWithMateria VALUES(0, @Fk_Id, @Nrc, @P_Academico)";
                    result = await db.ExecuteAsync(querySql, relation);
                }
                return Ok(result);
            }catch(Exception ex)
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
                    string querySql = $"DELETE FROM ProfessorWithMateria WHERE id={id}";
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
        public async Task<IActionResult> UpdateUser(string id, ProfessorWithMaterium user)
        {
            int result = 0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    string querySql = $"UPDATE ProfessorWithMateria SET Fk_Id=@Fk_Id,NRC=@NRC,P_Academico=@P_Academico WHERE id={id}";
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

