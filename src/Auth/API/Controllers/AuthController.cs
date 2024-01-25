using Business.Abstract;
using Entities.DTOs.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDTO model)
        {
            Log.Information($"User login in {model.UserName}");
            var userToLogin = await _authService.LoginAsync(model);
            Log.Information($"User signed in {model.UserName}");

            if (!userToLogin.Success)
                return BadRequest(userToLogin);

            var result = await _authService.CreateAccessTokenAsync(userToLogin.Data);

            Log.Information($"Creating token {model.UserName}");

            if (result.Success)
                userToLogin.Data.Token = result.Data;
            else
                return ActionResultInstance<string>(result);

            return ActionResultInstance<string>(userToLogin);
        }
    }
}
