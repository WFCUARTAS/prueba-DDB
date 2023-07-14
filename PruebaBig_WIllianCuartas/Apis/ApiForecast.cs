using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaBig_WIllianCuartas.Connection;
using PruebaBig_WIllianCuartas.Models;
using System.Diagnostics;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace PruebaBig_WIllianCuartas.Apis
{
    [ApiController]
    [Route("api/Forecast")]
    public class ApiForecast : Controller
    {
        Connectiondb cn = new Connectiondb();

        [HttpGet("{id}")]
        public async Task<ActionResult<Mforecast>> GetForecast(int id)
        {
            string _connectionString = cn.cadenaSQL();

            var db = new SqlConnection(_connectionString);

            var lista = await db.QuerySingleAsync<Mforecast>("SP_GetForecast", new
            {
                Id = id

            }, commandType: CommandType.StoredProcedure);

            return lista;
        }


    }
}