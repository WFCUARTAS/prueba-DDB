using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaBig_WIllianCuartas.Connection;
using PruebaBig_WIllianCuartas.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace PruebaBig_WIllianCuartas.Apis
{
    [ApiController]
    [Route("api/City")]
    public class ApiCity : ControllerBase
    {

        Connectiondb cn = new Connectiondb();

        [HttpGet]
        public async Task<ActionResult<List<MCity>>> GetForecast(int id)
        {
            string _connectionString = cn.cadenaSQL();

            var db = new SqlConnection(_connectionString);

            var lista = await db.QueryAsync<MCity>("select * from cities");

            return lista.ToList();
        }

        [HttpPost]
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
