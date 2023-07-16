using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaBig_WIllianCuartas.Connection;
using PruebaBig_WIllianCuartas.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PruebaBig_WIllianCuartas.Apis
{
    [ApiController]
    [Route("api/City")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiCity : ControllerBase
    {

        Connectiondb cn = new Connectiondb();

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<MCity>>> GetForecast()
        {
            string _connectionString = cn.cadenaSQL();

            var db = new SqlConnection(_connectionString);

            var lista = await db.QueryAsync<MCity>("select * from cities");

            return lista.ToList();
        }

        [HttpPost]

        [Authorize(Roles = "Admin")]
        public async Task PostForecast([FromBody] MCity parametros)
        {

            string _connectionString = cn.cadenaSQL();
            var db = new SqlConnection(_connectionString);

            var res = await db.QueryAsync<int>("SP_PostCity", new
            {
                Name = parametros.Name,
                Departament = parametros.Departament
            }, commandType: CommandType.StoredProcedure);

        }

    }
}
