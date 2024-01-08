using API.Controllers;
using Business.Abstract;
using Core.Entities.DTOs;
using Core.Utilities.Security;
using Core.Utilities.Test;
using Entities.DTOs.Params;
using Entities.DTOs.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AuthApi.Tests
{
    public class AuthControllerTest : BaseServiceTest<IAuthService>
    {
        [Fact]
        public void Login_Returns_BadRequest_When_Login_Fails()
        {
            var authController = new AuthController(serviceMock.Object, loggerMock.Object);

            var userForLoginDTO = new UserForLoginDTO
            {
                UserName = "testuser",
                Password = "invalidpassword"
            };

            var failedResult = new ErrorDataResult<UserForViewDTO>("Login failed");
            serviceMock.Setup(x => x.Login(It.IsAny<UserForLoginDTO>())).Returns(failedResult);

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
            var authController = new AuthController(serviceMock.Object, loggerMock.Object);

            var userForLoginDTO = new UserForLoginDTO
            {
                UserName = "testuser",
                Password = "validpassword"
            };

            var expectedToken = new AccessToken { Token = "testtoken" };

            var successfulLoginResult = new SuccessDataResult<UserForViewDTO>(new UserForViewDTO { Token = expectedToken });
            serviceMock.Setup(x => x.Login(It.IsAny<UserForLoginDTO>())).Returns(successfulLoginResult);

            var successfulTokenResult = new SuccessDataResult<AccessToken>(expectedToken);
            serviceMock.Setup(x => x.CreateAccessToken(It.IsAny<UserForViewDTO>())).Returns(successfulTokenResult);

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