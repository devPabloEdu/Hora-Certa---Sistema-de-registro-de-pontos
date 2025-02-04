using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;   
using RegistroDePontosApi.Models;
using Microsoft.EntityFrameworkCore;
namespace RegistroDePontosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration _config;
        private readonly RegistroContext _context;
        public AuthenticationController(IConfiguration Configuration, RegistroContext context)
        {
            _config = Configuration;
            _context = context;
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel user)
        {
            if(user == null)
            {
                return BadRequest("Request do cliente inválido");
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == user.Username);

            var senha = await _context.Usuarios.FirstOrDefaultAsync(u => u.PasswordHash == user.PasswordHash);

            if (usuario != null && senha != null)
            {
                var _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var _issuer = _config["Jwt:Issuer"];
                var _audience = _config["Jwt:Audience"];

                var signinCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken
                (
                    issuer: _issuer,
                    audience: _audience,
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(2),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new {Token = tokenString});
            }
            else
            {
                return Unauthorized("Usuário não cadastrado ou não autorizado");
            }
        }

    }
}