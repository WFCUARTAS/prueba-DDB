using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaBig_WIllianCuartas.Connection;
using PruebaBig_WIllianCuartas.Models;
using System.Data;
using System.Data.SqlClient;

namespace PruebaBig_WIllianCuartas.Apis
{
    [ApiController]
    [Route("api/User")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiUser : Controller
    {
        Connectiondb cn = new Connectiondb();

        [HttpGet("{email}")]
        public async Task<ActionResult<MUser>> GetUser(string email)
        {
            string _connectionString = cn.cadenaSQL();

            var db = new SqlConnection(_connectionString);

            var Result = await db.QuerySingleAsync<MUser>("SP_GetUser", new
            {
                Email = email

            }, commandType: CommandType.StoredProcedure);

            return Result;
        }
    }
}
