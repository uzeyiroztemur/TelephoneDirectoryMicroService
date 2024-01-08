using API.Controllers;
using Business.Abstract;
using Core.Entities.DTOs;
using Core.Utilities.Test;
using Entities.DTOs.Params;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AuthApi.Tests
{
    public class AccountAuthControllerTest : BaseServiceTest<IAuthService>
    {
        [Fact]
        public void ChangePassword_Returns_OkResult_When_ChangePassword_Succeeds()
        {
            var accountController = new AccountController(serviceMock.Object, loggerMock.Object);

            var passwordForChangeDTO = new PasswordForChangeDTO
            {
                OldPassword = "currentPassword",
                NewPassword = "newPassword",
                NewPasswordAgain = "newPassword"
            };

            var successfulResult = new SuccessResult("Password changed successfully");
            serviceMock.Setup(x => x.ChangePassword(It.IsAny<PasswordForChangeDTO>())).Returns(successfulResult);

            // Act
            var result = accountController.ChangePassword(passwordForChangeDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public void ChangePassword_Returns_BadRequestResult_When_ChangePassword_Fails()
        {
            var accountController = new AccountController(serviceMock.Object, loggerMock.Object);

            var passwordForChangeDTO = new PasswordForChangeDTO
            {
                OldPassword = "currentPassword",
                NewPassword = "newPassword",
                NewPasswordAgain = "newPasswordagain"
            };

            var errorResult = new ErrorResult("Failed to change password");
            serviceMock.Setup(x => x.ChangePassword(It.IsAny<PasswordForChangeDTO>())).Returns(errorResult);

            // Act
            var result = accountController.ChangePassword(passwordForChangeDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

    }
}