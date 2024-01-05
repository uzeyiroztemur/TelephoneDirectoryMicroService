using API.Controllers;
using Business.Abstract;
using Core.CrossCuttingConcerns.Logging;
using Core.Entities.DTOs;
using Core.Utilities.Security;
using Entities.DTOs.Params;
using Entities.DTOs.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AuthApi.Tests
{
    public class AuthControllerTest
    {
        [Fact]
        public void Login_Returns_BadRequest_When_Login_Fails()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger>();

            var authController = new AuthController(authServiceMock.Object, loggerMock.Object);

            var userForLoginDTO = new UserForLoginDTO
            {
                UserName = "testuser",
                Password = "invalidpassword"
            };

            var failedResult = new ErrorDataResult<UserForViewDTO>("Login failed");
            authServiceMock.Setup(x => x.Login(It.IsAny<UserForLoginDTO>())).Returns(failedResult);

            // Act
            var result = authController.Login(userForLoginDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.Equal(failedResult, badRequestResult.Value);
        }

        [Fact]
        public void Login_Returns_Token_When_Login_Succeeds()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger>();

            var authController = new AuthController(authServiceMock.Object, loggerMock.Object);

            var userForLoginDTO = new UserForLoginDTO
            {
                UserName = "testuser",
                Password = "validpassword"
            };

            var expectedToken = new AccessToken { Token = "testtoken" };

            var successfulLoginResult = new SuccessDataResult<UserForViewDTO>(new UserForViewDTO { Token = expectedToken });
            authServiceMock.Setup(x => x.Login(It.IsAny<UserForLoginDTO>())).Returns(successfulLoginResult);

            var successfulTokenResult = new SuccessDataResult<AccessToken>(expectedToken);
            authServiceMock.Setup(x => x.CreateAccessToken(It.IsAny<UserForViewDTO>())).Returns(successfulTokenResult);

            // Act
            var result = authController.Login(userForLoginDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected token
            Assert.Equal(expectedToken, ((SuccessDataResult<UserForViewDTO>)okObjectResult.Value).Data.Token);
        }
    }
}