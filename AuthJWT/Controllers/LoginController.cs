using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthJWT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        [HttpPost]
        public IActionResult Login(string user, string pass, string rol)
        {

            bool log = false;
            if (user == "and" && pass == "123")
            {

                var tokenReturned = this.BuildToken(new UserInfo() { User = user, Password = pass }, rol);

                return Ok(tokenReturned.Token);
            }
            else
            {
                return BadRequest("Invalid User");
            }



        }


        private UserToken BuildToken(UserInfo data, string rol)
        {

            var claimsData = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.UniqueName, data.User),
                new Claim("Custom", "CustomData"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, rol)
            };



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LLAVE-DE-SEGURIDAD123456789012345679012lsklsksiwkwkwkisksksiwlwlodckd34569874525241252524414525215655000"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var encryptingCredentials = new EncryptingCredentials(key, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);


            var tiempoExpiracion = DateTime.UtcNow.AddMinutes(30);

            var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
                issuer: null,
                audience: null,
                subject: new ClaimsIdentity(claimsData),
                notBefore: null,
                expires: tiempoExpiracion,
                issuedAt: null,
                signingCredentials: creds,
                encryptingCredentials: encryptingCredentials
                );

            return new UserToken()
            {
                ExpirationToken = tiempoExpiracion,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

        }

    }


}