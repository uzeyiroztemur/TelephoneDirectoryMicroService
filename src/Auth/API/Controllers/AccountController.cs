using Business.Abstract;
using Entities.DTOs.Params;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordForChangeDTO model)
        {
            Log.Information("Change password");
            var result = await _authService.ChangePasswordAsync(model);
            Log.Information("Change password completed");

            return ActionResultInstance<string>(result);
        }
    }
}