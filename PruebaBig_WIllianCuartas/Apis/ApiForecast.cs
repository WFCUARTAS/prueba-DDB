using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaBig_WIllianCuartas.Connection;
using PruebaBig_WIllianCuartas.Models;
using System.Diagnostics;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using System.Runtime.InteropServices;
using System;

namespace PruebaBig_WIllianCuartas.Apis
{
    [ApiController]
    [Route("api/Forecast")]
    public class ApiForecast : Controller
    {
        Connectiondb cn = new Connectiondb();

        [HttpGet("{id}")]
        public async Task<ActionResult<MForecast>> GetForecast(int id)
        {
            string _connectionString = cn.cadenaSQL();

            var db = new SqlConnection(_connectionString);

            var Result = await db.QuerySingleAsync<MForecast>("SP_GetForecast", new
            {
                Id = id

            }, commandType: CommandType.StoredProcedure);

            return Result;
        }

        [HttpPost]
        public async Task PostForecast([FromBody] MForecast parametros)
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
        public async Task PutForecast(int id,[FromBody] MForecast parametros)
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

        [HttpGet]
        [Route("GetByDate/{Date}")]
        public async Task<ActionResult<List<MForecast>>> GetCityForecast(DateTime Date)
        {
            string _connectionString = cn.cadenaSQL();

            var db = new SqlConnection(_connectionString);

            var lista = await db.QueryAsync<MForecast>("SP_ListDateForecast", new
            {
                DateClima = Date

            }, commandType: CommandType.StoredProcedure);

            return lista.ToList();
        }

        [HttpPost]
        [Route("GetByCityDate")]
        public async Task<ActionResult<MForecast>> GetCityDateForecast([FromBody] MForecast Forecast)
        {
            string _connectionString = cn.cadenaSQL();

            var db = new SqlConnection(_connectionString);

            try
            {
                var Result = await db.QuerySingleAsync<MForecast>("SP_ListCityDateForecast", new
                {
                    IdCity = Forecast.IdCity,
                    DateClima = Forecast.DateClima

                }, commandType: CommandType.StoredProcedure);

                return Result;

            } catch (InvalidOperationException ex) {

                return NoContent();
            }catch (Exception)
            {
                return NotFound();
            }
            
        }

    }
}