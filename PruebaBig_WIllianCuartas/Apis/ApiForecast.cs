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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace PruebaBig_WIllianCuartas.Apis
{
    [ApiController]
    [Route("api/Forecast")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiForecast : Controller
    {
        Connectiondb cn = new Connectiondb();

        [HttpGet("{id}")]
        [AllowAnonymous]
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
        [Authorize(Roles = "Admin")]
        public async Task PostForecast([FromBody] MForecast parametros)
        {
            var identity = User.Identity as ClaimsIdentity;
            int IdUser = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);

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
                IdUserChange = IdUser
           }, commandType: CommandType.StoredProcedure);
           
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task PutForecast(int id,[FromBody] MForecast parametros)
        {
            var identity = User.Identity as ClaimsIdentity;
            int IdUser = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);

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
                IdUserChange = IdUser
            }, commandType: CommandType.StoredProcedure);

        }

        [HttpGet]
        [Route("GetByDate/{Date}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<MForecast>>> GetDateForecast(DateTime Date)
        {
            string _connectionString = cn.cadenaSQL();

            var db = new SqlConnection(_connectionString);

            var lista = await db.QueryAsync<MForecast>("SP_ListDateForecast", new
            {
                DateClima = Date

            }, commandType: CommandType.StoredProcedure);

            return lista.ToList();
        }

        [HttpGet]
        [Route("GetByCity/{id}")]
        public async Task<ActionResult<List<MForecast>>> GetCityForecast(int id)
        {
            string _connectionString = cn.cadenaSQL();

            var db = new SqlConnection(_connectionString);

            var lista = await db.QueryAsync<MForecast>("SP_ListCityForecast", new
            {
                IdCity = id

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