using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PruebaBig_WIllianCuartas.Models;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace PruebaBig_WIllianCuartas.Controllers
{
    public class AutenticacionController : Controller
    {

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
            if (User.Email == "mail@mail.com" && User.Password == "12345")
            {
                List<Claim> claims= new List<Claim>() {
                    new Claim(ClaimTypes.Name, "willian"),
                    new Claim(ClaimTypes.Email, User.Email)
                };

                ClaimsIdentity ci = new (claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new ();

                properties.AllowRefresh = true;
                properties.ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), properties);
                HttpContext.Session.SetString("JwtToken", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJtYWlsQG1haWwuY29tIiwibmJmIjoxNjg5NDkwNDMyLCJleHAiOjE2ODk0OTQwMzIsImlhdCI6MTY4OTQ5MDQzMn0.Yv3bgXHlWSoP9fFr9Ik_ty0MhSjqijG41u_uJIN-cv0");

                return RedirectToAction("index", "Forecast");
            }
            return View();
        }

        public async Task<IActionResult> exit()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login", "Autenticacion");
        }

    }
}
