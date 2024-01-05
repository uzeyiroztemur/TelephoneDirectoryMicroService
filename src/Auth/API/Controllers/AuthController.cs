using Business.Abstract;
using Entities.DTOs.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.CrossCuttingConcerns.Logging;

namespace API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly Core.CrossCuttingConcerns.Logging.ILogger _logger;

        public AuthController(IAuthService authService, Core.CrossCuttingConcerns.Logging.ILogger logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserForLoginDTO model)
        {
            _logger.Info($"User login in {model.UserName}");
            var userToLogin = _authService.Login(model);
            _logger.HandleResult(userToLogin, $"User signed in {model.UserName}");

            if (!userToLogin.Success)
                return BadRequest(userToLogin);

            var result = _authService.CreateAccessToken(userToLogin.Data);

            _logger.HandleResult(result, $"Creating token {model.UserName}");

            if (result.Success)
                userToLogin.Data.Token = result.Data;
            else
                return ActionResultInstance<string>(result);

            return ActionResultInstance<string>(userToLogin);
        }
    }
}
