using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PruebaBig_WIllianCuartas.Connection;
using PruebaBig_WIllianCuartas.Models;
using System.Data.SqlClient;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dapper;

namespace PruebaBig_WIllianCuartas.Apis
{
    [Route("api/Autentication")]
    [ApiController]
    public class ApiAutentication : Controller
    {
        private readonly string secretkey;
        Connectiondb cn = new Connectiondb();

        public ApiAutentication(IConfiguration config)
        {
            secretkey = config.GetSection("settings").GetSection("secretkey").ToString();
        }

        [HttpPost]
        [Route("Validate")]
        public async Task<IActionResult> ValidateAsync([FromBody] MUser User)
        {
            string _connectionString = cn.cadenaSQL();
            var db = new SqlConnection(_connectionString);

            try
            {
                var Result = await db.QuerySingleAsync<MUser>("SP_ValidateUser", new
                {
                    Email = User.Email,
                    Password = User.Password
                }, commandType: CommandType.StoredProcedure);

                var keyBytes = Encoding.ASCII.GetBytes(secretkey);
                var claims = new ClaimsIdentity();

                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, User.Email));
                claims.AddClaim(new Claim(ClaimTypes.Role, Result.UserRol));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string tokenCreado = tokenHandler.WriteToken(tokenConfig);

                return StatusCode(StatusCodes.Status200OK, new { token = tokenCreado });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }

        }

    }
}
