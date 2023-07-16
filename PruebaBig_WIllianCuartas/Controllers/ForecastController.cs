using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using PruebaBig_WIllianCuartas.Models;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
using static Dapper.SqlMapper;

namespace PruebaBig_WIllianCuartas.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ForecastController : Controller
    {

        private readonly string ApiUrl;
        private readonly HttpClient httpClient;
        private string token;


        public ForecastController(IConfiguration config)
        {
            ApiUrl = config.GetSection("ApiUrl").Value;
            httpClient = new HttpClient();

        }

        // GET: ForecastController
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            Boolean IsAdmin = false;
            //validar si se inicio secion 
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                {
                    var identity = User.Identity as ClaimsIdentity;
                    if (identity.FindFirst(ClaimTypes.Role)?.Value=="Admin")
                    {
                        IsAdmin = true;
                    }
                }
            }
             

            HttpResponseMessage responseForecast = await httpClient.GetAsync(ApiUrl+ "api/Forecast/GetByDate/"+ DateTime.Now.ToString("yyyy-MM-dd"));
            if (responseForecast.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("exit", "Autentication");
            }

            string jsonContentForecast = await responseForecast.Content.ReadAsStringAsync();
            var DataForecast = JsonConvert.DeserializeObject<List<MForecast>>(jsonContentForecast);


            // llamar API para obtener las ciudades
            HttpResponseMessage responseCity = await httpClient.GetAsync(ApiUrl + "api/City");
            if (responseCity.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("exit", "Autentication");
            }
            
            string jsonContentCity = await responseCity.Content.ReadAsStringAsync();
            var DataCity = JsonConvert.DeserializeObject<List<MCity>>(jsonContentCity);


            ViewBag.DateNow = DateTime.Now;
            ViewBag.IsAdmin = IsAdmin;
            ViewBag.City = DataCity;
            return View(DataForecast);
        }

        // GET: ForecastController
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int id)
        {
            var identity = User.Identity as ClaimsIdentity;
            token = identity.FindFirst("token")?.Value;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage responseForecast = await httpClient.GetAsync(ApiUrl + "api/Forecast/" + id);
            if (responseForecast.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("exit", "Autentication");
            }

            string jsonContentForecast = await responseForecast.Content.ReadAsStringAsync();
            var DataForecast = JsonConvert.DeserializeObject<MForecast>(jsonContentForecast);


            ViewBag.DateNow = DateTime.Now;
 
            return View("Register",DataForecast);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        // POST: ForecastController/Register
        public async Task<ActionResult> Register(MForecast Forecast)
        {
            var identity = User.Identity as ClaimsIdentity;
            token = identity.FindFirst("token")?.Value;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var parametros = new
            {
                IdCity = Forecast.IdCity.ToString(),
                DateClima = Forecast.DateClima.ToString("yyyy-MM-dd")
            };
            
            var json = JsonConvert.SerializeObject(parametros);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            
            HttpResponseMessage responseForecast = await httpClient.PostAsync(ApiUrl + "api/Forecast/GetByCityDate/", contenido);
            if (responseForecast.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("exit", "Autentication");
            }

            string jsonContentForecast = await responseForecast.Content.ReadAsStringAsync();
            var DataForecast = JsonConvert.DeserializeObject<MForecast>(jsonContentForecast);
            if (DataForecast == null)
            {
                DataForecast = new MForecast();
                DataForecast.IdCity = Forecast.IdCity;
                DataForecast.DateClima = Forecast.DateClima;
            }


            ViewBag.DateNow = DateTime.Now;
            
            return View(DataForecast);
        }



        // POST: ForecastController/Save
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Save(MForecast Forecast)
        {
            var identity = User.Identity as ClaimsIdentity;
            token = identity.FindFirst("token")?.Value;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Serializar el objeto a JSON
            var json = JsonConvert.SerializeObject(Forecast);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage responseForecast;
            
            if (Forecast.Id == null)
            {
                responseForecast = await httpClient.PostAsync(ApiUrl + "api/Forecast/", contenido);
            }
            else
            {
                responseForecast = await httpClient.PutAsync(ApiUrl + "api/Forecast/"+ Forecast.Id, contenido);
            }
            
            
            if (responseForecast.IsSuccessStatusCode)
            {
                TempData["Message"] = "Registrado con exito";
                TempData["Message_type"] = "success";
                return RedirectToAction(nameof(Index));
            }
            else if (responseForecast.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("exit", "Autentication");
            }
            else
            {
                TempData["Message"] = responseForecast.ReasonPhrase;
                TempData["Message_type"] = "danger";
                return RedirectToAction(nameof(Index));
            }
        }


    }
}
