using DeliveryProjectAzureApi.Helpers;
using DeliveryProjectAzureApi.Repositories;
using DeliveryProjectNuget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DeliveryProjectAzureApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryDelivery repo;
        private HelperOAuthToken helper;

        public AuthController(RepositoryDelivery repo, HelperOAuthToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            User user = await this.repo.LoginUserAsync(model.UserName, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);
                string jsonEmpleado = JsonConvert.SerializeObject(user);
                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonEmpleado)
                };
                JwtSecurityToken token =
                    new JwtSecurityToken(
                        claims: informacion,
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                    );
                return Ok(new
                {
                    response = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            await this.repo.RegisterUser(model.Email, model.Name, model.Password, model.Rol, model.dateAdd, model.Image);
            return Ok();
        }

        [HttpGet]
        [Route("[action]/{username}")]
        public async Task<ActionResult<User>> FindUser(string username)
        {
            User user = await this.repo.FindUserAsync(username);
            return user;
        }
    }
}
