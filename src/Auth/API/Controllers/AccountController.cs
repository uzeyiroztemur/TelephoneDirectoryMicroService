using Business.Abstract;
using Core.CrossCuttingConcerns.Logging;
using Entities.DTOs.Params;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly Core.CrossCuttingConcerns.Logging.ILogger _logger;

        public AccountController(IAuthService authService, Core.CrossCuttingConcerns.Logging.ILogger logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordForChangeDTO model)
        {
            _logger.Info("Change password");
            var result = await _authService.ChangePasswordAsync(model);
            _logger.HandleResult(model, "Change password");

            return ActionResultInstance<string>(result);
        }
    }
}