using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using MySql.Data.MySqlClient;
using AsistenciasApi.Models;
using dotenv.net;
using Microsoft.AspNetCore.Authorization;


namespace AsistenciasApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //para la conexion con .env
        private string _connection;
        public UsersController()
        {
            
             DotEnv.Load();
                var envVar = DotEnv.Read();
                _connection = $"{envVar["StringConnetion"]}";
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Useruniversity>> GetUser(string id) 
        {
            try
            {
                IEnumerable<Useruniversity> result;
                using (var db = new MySqlConnection(_connection))
                {
                    var query = $"SELECT * FROM useruniversity where id='{id}'";
                    result = await db.QueryAsync<Useruniversity>(query);
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertUser(Useruniversity user)
        {
            int result=0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    // Consulta para verificar si ya existe un presupuesto para el usuario
                    var queryValidate = $"SELECT COUNT(*) FROM Useruniversity WHERE id='{user.Id}'";
                    int count = await db.ExecuteScalarAsync<int>(queryValidate);

                    // Verificar el resultado
                    if (count > 0)
                    {
                        // Ya existe un presupuesto para este usuario, puedes manejar el error o simplemente no hacer la inserción
                        return Unauthorized("Ya existe un presupuesto para este usuario");
                    }

                    // Si no existe, realizar la inserción
                    var querySql = "INSERT INTO Useruniversity VALUES(@Id, @Tipo_doc, @No_doc, @FirstName, @LastName, @Email, @EmailAcademico, @PhoneNumber, @Rol, @StatusUser)";
                    result = await db.ExecuteAsync(querySql, user);
                }
                return Ok(result);

            }catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "admin")]
        //[Authorize(Policy = "AdminRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            int result;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    string querySql = $"DELETE FROM Useruniversity WHERE id='{id}'";
                    result = await db.ExecuteAsync(querySql);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, Useruniversity user)
        {
            int result=0;
            try
            {
                using (var db = new MySqlConnection(_connection))
                {
                    string querySql = $"UPDATE Useruniversity SET Tipo_doc=@Tipo_doc,FirstName=@FirstName,LastName=@LastName,email=@Email,emailAcademico=@EmailAcademico,phoneNumber=@PhoneNumber,Rol=@Rol,statusUser=@StatusUser WHERE id='{id}'";
                    result = await db.ExecuteAsync(querySql, user);
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
             
        }
    }
}/*id='{T000}',
       Tipo_doc='@Tipo_doc',No_doc='@No_doc',FirstName='@fisrtName',LastName='@lastName',email='@email',emailAcademico='@emailAcademico',phoneNumber='@phoneNumber',Rol='@Rol',statusUser='@statusUser'

  */
