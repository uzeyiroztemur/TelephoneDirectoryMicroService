using API.Controllers;
using Business.Abstract;
using Core.CrossCuttingConcerns.Logging;
using Core.Entities.DTOs;
using Entities.DTOs.Params;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AuthApi.Tests
{
    public class AccountAuthControllerTest
    {
        [Fact]
        public void ChangePassword_Returns_OkResult_When_ChangePassword_Succeeds()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger>();

            var accountController = new AccountController(authServiceMock.Object, loggerMock.Object);

            var passwordForChangeDTO = new PasswordForChangeDTO
            {
                OldPassword = "currentPassword",
                NewPassword = "newPassword",
                NewPasswordAgain = "newPassword"
            };

            var successfulResult = new SuccessResult("Password changed successfully");
            authServiceMock.Setup(x => x.ChangePassword(It.IsAny<PasswordForChangeDTO>())).Returns(successfulResult);

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
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger>();

            var accountController = new AccountController(authServiceMock.Object, loggerMock.Object);

            var passwordForChangeDTO = new PasswordForChangeDTO
            {
                OldPassword = "currentPassword",
                NewPassword = "newPassword",
                NewPasswordAgain = "newPasswordagain"
            };

            var errorResult = new ErrorResult("Failed to change password");
            authServiceMock.Setup(x => x.ChangePassword(It.IsAny<PasswordForChangeDTO>())).Returns(errorResult);

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