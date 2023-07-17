using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PruebaBig_WIllianCuartas.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace PruebaBig_WIllianCuartas.Controllers
{
    public class AutenticationController : Controller
    {
        private readonly string ApiUrl;
        private readonly HttpClient httpClient;
        public AutenticationController(IConfiguration config)
        {
            ApiUrl = config.GetSection("ApiUrl").Value;
            httpClient = new HttpClient();
        }

        [HttpGet]
        public IActionResult login()
        {
            ClaimsPrincipal c = HttpContext.User;

            if(c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                {
                    return RedirectToAction("index", "Forecast");
                }
            }

            return View(new MUser());
        }

        [HttpPost]
        public async Task<IActionResult> login(MUser User)
        {
            /////inicia la sesion con los datos ingresados por el usuario solicita el token al api rest
            var parametros = new
            {
                Email = User.Email,
                Password = User.Password
            };

            var json = JsonConvert.SerializeObject(parametros);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");


            HttpResponseMessage responseToken = await httpClient.PostAsync(ApiUrl + "api/Autentication/Validate/", contenido);
            
            if (responseToken.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                TempData["Message"] = "Datos de acceso incorrectos";
                TempData["Message_type"] = "danger";
                return View(new MUser());
            }else{
                //capturar el togen de la respuesta de la API
                string jsonContentUser = await responseToken.Content.ReadAsStringAsync();
                JObject jsonObject = JObject.Parse(jsonContentUser);
                string token = (string)jsonObject["token"];

                //Consultar datos del usuario que inicio sesion
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseUser = await httpClient.GetAsync(ApiUrl + "api/User/" + User.Email);
                string jsonContentForecast = await responseUser.Content.ReadAsStringAsync();
                var DataUser = JsonConvert.DeserializeObject<MUser>(jsonContentForecast);

                //asignar los valores del Usuario a la sesion
                List<Claim> claims= new List<Claim>() {
                    new Claim(ClaimTypes.Name, DataUser.FullName),
                    new Claim(ClaimTypes.Email, User.Email),
                    new Claim(ClaimTypes.Role, DataUser.UserRol),
                    new Claim("token", token)
                };

                ClaimsIdentity ci = new (claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new ();

                properties.AllowRefresh = true;
                properties.ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), properties);

                return RedirectToAction("index", "Forecast");
            }
            
        }

        public async Task<IActionResult> exit()
        {
            ///finalixa la sesion.
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index", "Forecast");
        }

    }
}
