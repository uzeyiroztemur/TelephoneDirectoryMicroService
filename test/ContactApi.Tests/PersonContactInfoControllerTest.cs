using API.Controllers;
using Business.Abstract;
using Core.CrossCuttingConcerns.Logging;
using Core.Entities.DTOs;
using Entities.DTOs.Params;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ContactApi.Tests
{
    public class PersonContactInfoControllerTest
    {
        [Fact]
        public void Add_Returns_OkResult_When_PersonContactInfoAddedSuccessfully()
        {
            // Arrange
            var personContactInfoServiceMock = new Mock<IPersonContactInfoService>();
            var loggerMock = new Mock<ILogger>();

            var controller = new PersonContactInfosController(personContactInfoServiceMock.Object, loggerMock.Object);

            var personId = Guid.NewGuid();
            var personContactInfoForUpsertDTO = new PersonContactInfoForUpsertDTO
            {
                InfoType = Entities.Constants.ContactInfoType.Email,
                InfoValue = "uzeyiroztemur@gmail.com",
                IsActive = true,
            };

            var successfulResult = new SuccessDataResult<string>("PersonContactInfo added successfully");

            // Setup mock service to return the expected result when called with the expected parameters
            personContactInfoServiceMock.Setup(x => x.Add(personId, personContactInfoForUpsertDTO)).Returns(successfulResult);

            // Act
            var result = controller.Add(personId, personContactInfoForUpsertDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public void Add_Returns_BadRequestResult_When_PersonContactInfoAddFails()
        {
            // Arrange
            var personContactInfoServiceMock = new Mock<IPersonContactInfoService>();
            var loggerMock = new Mock<ILogger>();

            var controller = new PersonContactInfosController(personContactInfoServiceMock.Object, loggerMock.Object);

            var personId = Guid.NewGuid();
            var personContactInfoForUpsertDTO = new PersonContactInfoForUpsertDTO
            {
                InfoType = Entities.Constants.ContactInfoType.Email,
                InfoValue = "uzeyiroztemur@gmail.com",
                IsActive = false,
            };

            var errorResult = new ErrorDataResult<string>("PersonContactInfo add failed");

            // Setup mock service to return the error result when called with the expected parameters
            personContactInfoServiceMock.Setup(x => x.Add(personId, personContactInfoForUpsertDTO)).Returns(errorResult);

            // Act
            var result = controller.Add(personId, personContactInfoForUpsertDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected error result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

        [Fact]
        public void Edit_Returns_OkResult_When_PersonContactInfoUpdatedSuccessfully()
        {
            // Arrange
            var personContactInfoServiceMock = new Mock<IPersonContactInfoService>();
            var loggerMock = new Mock<ILogger>();

            var controller = new PersonContactInfosController(personContactInfoServiceMock.Object, loggerMock.Object);

            var personId = Guid.NewGuid();
            var personContactInfoId = Guid.NewGuid();
            var personContactInfoForUpsertDTO = new PersonContactInfoForUpsertDTO
            {
                InfoType = Entities.Constants.ContactInfoType.Email,
                InfoValue = "uzeyiroztemur@hotmail.com",
                IsActive = true,
            };

            var successfulResult = new SuccessDataResult<string>("PersonContactInfo updated successfully");

            // Setup mock service to return the expected result when called with the expected parameters
            personContactInfoServiceMock.Setup(x => x.Update(personId, personContactInfoId, personContactInfoForUpsertDTO)).Returns(successfulResult);

            // Act
            var result = controller.Edit(personId, personContactInfoId, personContactInfoForUpsertDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public void Edit_Returns_BadRequestResult_When_PersonContactInfoUpdateFails()
        {
            // Arrange
            var personContactInfoServiceMock = new Mock<IPersonContactInfoService>();
            var loggerMock = new Mock<ILogger>();

            var controller = new PersonContactInfosController(personContactInfoServiceMock.Object, loggerMock.Object);

            var personId = Guid.NewGuid();
            var personContactInfoId = Guid.NewGuid();
            var personContactInfoForUpsertDTO = new PersonContactInfoForUpsertDTO
            {
                InfoType = Entities.Constants.ContactInfoType.Email,
                InfoValue = "uzeyiroztemur@hotmail.com",
                IsActive = false,
            };

            var errorResult = new ErrorDataResult<string>("PersonContactInfo update failed");

            // Setup mock service to return the error result when called with the expected parameters
            personContactInfoServiceMock.Setup(x => x.Update(personId, personContactInfoId, personContactInfoForUpsertDTO)).Returns(errorResult);

            // Act
            var result = controller.Edit(personId, personContactInfoId, personContactInfoForUpsertDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected error result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

        [Fact]
        public void Delete_Returns_OkResult_When_PersonContactInfoDeletedSuccessfully()
        {
            // Arrange
            var personContactInfoServiceMock = new Mock<IPersonContactInfoService>();
            var loggerMock = new Mock<ILogger>();

            var controller = new PersonContactInfosController(personContactInfoServiceMock.Object, loggerMock.Object);

            var personId = Guid.NewGuid();
            var personContactInfoId = Guid.NewGuid();

            var successfulResult = new SuccessDataResult<string>("PersonContactInfo deleted successfully");

            // Setup mock service to return the expected result when called with the expected parameters
            personContactInfoServiceMock.Setup(x => x.Delete(personId, personContactInfoId)).Returns(successfulResult);

            // Act
            var result = controller.Delete(personId, personContactInfoId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public void Delete_Returns_BadRequestResult_When_PersonContactInfoDeleteFails()
        {
            // Arrange
            var personContactInfoServiceMock = new Mock<IPersonContactInfoService>();
            var loggerMock = new Mock<ILogger>();

            var controller = new PersonContactInfosController(personContactInfoServiceMock.Object, loggerMock.Object);

            var personId = Guid.NewGuid();
            var personContactInfoId = Guid.NewGuid();

            var errorResult = new ErrorDataResult<string>("PersonContactInfo delete failed");

            // Setup mock service to return the error result when called with the expected parameters
            personContactInfoServiceMock.Setup(x => x.Delete(personId, personContactInfoId)).Returns(errorResult);

            // Act
            var result = controller.Delete(personId, personContactInfoId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected error result
            Assert.Equal(errorResult, badRequestResult.Value);
        }
    }
}