using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaBig_WIllianCuartas.Connection;
using PruebaBig_WIllianCuartas.Models;
using System.Diagnostics;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using System.Runtime.InteropServices;

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

        [HttpPost]
        public async Task PostForecast([FromBody] Mforecast parametros)
        {

            string _connectionString = cn.cadenaSQL();
            var db = new SqlConnection(_connectionString);

            var res = await db.QueryAsync<int>("SP_PostForecast", new
            {
                Title=parametros.Title,
                DateClima=parametros.DateClima,
                MinTemperature =parametros.MinTemperature,
                MaxTemperature =parametros.MaxTemperature,
                RainProbability =parametros.RainProbability,
                Observation = parametros.Observation,
                IdCity =parametros.IdCity,
                IdUserChange =12
            }, commandType: CommandType.StoredProcedure);

        }

        [HttpPut("{id}")]
        public async Task PutForecast(int id,[FromBody] Mforecast parametros)
        {

            string _connectionString = cn.cadenaSQL();
            var db = new SqlConnection(_connectionString);

            var res = await db.QueryAsync<int>("SP_PutForecast", new
            {
                Id=id,
                Title = parametros.Title,
                DateClima = parametros.DateClima,
                MinTemperature = parametros.MinTemperature,
                MaxTemperature = parametros.MaxTemperature,
                RainProbability = parametros.RainProbability,
                Observation = parametros.Observation,
                IdCity = parametros.IdCity,
                IdUserChange = 12
            }, commandType: CommandType.StoredProcedure);

        }


    }
}