using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using PruebaBig_WIllianCuartas.Models;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace PruebaBig_WIllianCuartas.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ForecastController : Controller
    {

        private readonly string ApiUrl;
        private readonly HttpClient httpClient;
        String token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJtYWlsQG1haWwuY29tIiwibmJmIjoxNjg5NDk0ODYxLCJleHAiOjE2ODk0OTg0NjEsImlhdCI6MTY4OTQ5NDg2MX0.AkXEvCu8rOLtjRkLS9t4LL9paeM945ZpbybNUrjUl10";
        public ForecastController(IConfiguration config)
        {
            ApiUrl = config.GetSection("ApiUrl").Value;
            httpClient = new HttpClient();
        }

        // GET: ForecastController
        public async Task<ActionResult> Index()
        {
            // Agregar el token de autorización al encabezado
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Enviar solicitud GET al API y recibir la respuesta
            HttpResponseMessage responseForecast = await httpClient.GetAsync(ApiUrl+ "api/Forecast/GetByDate/"+ DateTime.Now.ToString("yyyy-MM-dd"));
            // Leer el contenido de la respuesta como una cadena JSON
            string jsonContentForecast = await responseForecast.Content.ReadAsStringAsync();
            // Deserializar la cadena JSON a objetos C#
            var DataForecast = JsonConvert.DeserializeObject<List<MForecast>>(jsonContentForecast);


            // Enviar solicitud GET al API y recibir la respuesta
            HttpResponseMessage responseCity = await httpClient.GetAsync(ApiUrl + "api/City");
            // Leer el contenido de la respuesta como una cadena JSON
            string jsonContentCity = await responseCity.Content.ReadAsStringAsync();
            // Deserializar la cadena JSON a objetos C#
            var DataCity = JsonConvert.DeserializeObject<List<MCity>>(jsonContentCity);


            ViewBag.DateNow = DateTime.Now;
            ViewBag.City = DataCity;
            return View(DataForecast);
        }

        // GET: ForecastController
        public async Task<ActionResult> Edit(int id)
        {

            // Enviar solicitud GET al API y recibir la respuesta
            HttpResponseMessage responseForecast = await httpClient.GetAsync(ApiUrl + "api/Forecast/" + id);
            // Leer el contenido de la respuesta como una cadena JSON
            string jsonContentForecast = await responseForecast.Content.ReadAsStringAsync();
            // Deserializar la cadena JSON a objetos C#
            var DataForecast = JsonConvert.DeserializeObject<MForecast>(jsonContentForecast);


            ViewBag.DateNow = DateTime.Now;
 
            return View("Register",DataForecast);
        }

        [HttpPost]
        // POST: ForecastController/Register
        public async Task<ActionResult> Register(MForecast Forecast)
        {
            // Crear un objeto con los parámetros en formato JSON
            var parametros = new
            {
                IdCity = Forecast.IdCity.ToString(),
                DateClima = Forecast.DateClima.ToString("yyyy-MM-dd")
            };
            // Serializar el objeto a JSON
            var json = JsonConvert.SerializeObject(parametros);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            // Enviar solicitud GET al API y recibir la respuesta
            HttpResponseMessage responseForecast = await httpClient.PostAsync(ApiUrl + "api/Forecast/GetByCityDate/", contenido);
            // Leer el contenido de la respuesta como una cadena JSON
            string jsonContentForecast = await responseForecast.Content.ReadAsStringAsync();
            // Deserializar la cadena JSON a objetos C#
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
         public async Task<ActionResult> Save(MForecast Forecast)
        {

            // Serializar el objeto a JSON
            var json = JsonConvert.SerializeObject(Forecast);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage responseForecast;
            // validar el id: si es nullo se envia post (insertar) si no es nullo enviar PUT
            if (Forecast.Id == null)
            {
                responseForecast = await httpClient.PostAsync(ApiUrl + "api/Forecast/", contenido);
            }
            else
            {
                responseForecast = await httpClient.PutAsync(ApiUrl + "api/Forecast/"+ Forecast.Id, contenido);
            }
            
            // Validar que si funciono el codigo
            if (responseForecast.IsSuccessStatusCode)
            {
                TempData["Message"] = "Registrado con exito";
                TempData["Message_type"] = "success";
                return RedirectToAction(nameof(Index));
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
