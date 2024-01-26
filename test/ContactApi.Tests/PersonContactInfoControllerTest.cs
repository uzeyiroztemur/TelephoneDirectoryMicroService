using API.Controllers;
using Business.Abstract;
using Core.Entities.DTOs;
using Core.Utilities.Test;
using Entities.DTOs.Params;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ContactApi.Tests
{
    public class PersonContactInfoControllerTest : BaseServiceTest<IPersonContactInfoService>
    {
        [Fact]
        public async Task Add_Returns_OkResult_When_PersonContactInfoAddedSuccessfully()
        {
            var controller = new PersonContactInfosController(serviceMock.Object);

            var personId = Guid.NewGuid();
            var personContactInfoForUpsertDTO = new PersonContactInfoForUpsertDTO
            {
                InfoType = Entities.Constants.ContactInfoType.Email,
                InfoValue = "uzeyiroztemur@gmail.com",
                IsActive = true,
            };

            var successfulResult = new SuccessDataResult<string>("PersonContactInfo added successfully");

            // Setup mock service to return the expected result when called with the expected parameters
            serviceMock.Setup(x => x.AddAsync(personId, personContactInfoForUpsertDTO)).ReturnsAsync(successfulResult);

            // Act
            var result = await controller.Add(personId, personContactInfoForUpsertDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public async Task Add_Returns_BadRequestResult_When_PersonContactInfoAddFails()
        {
            var controller = new PersonContactInfosController(serviceMock.Object);

            var personId = Guid.NewGuid();
            var personContactInfoForUpsertDTO = new PersonContactInfoForUpsertDTO
            {
                InfoType = Entities.Constants.ContactInfoType.Email,
                InfoValue = "uzeyiroztemur@gmail.com",
                IsActive = false,
            };

            var errorResult = new ErrorDataResult<string>("PersonContactInfo add failed");

            // Setup mock service to return the error result when called with the expected parameters
            serviceMock.Setup(x => x.AddAsync(personId, personContactInfoForUpsertDTO)).ReturnsAsync(errorResult);

            // Act
            var result = await controller.Add(personId, personContactInfoForUpsertDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected error result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

        [Fact]
        public async Task Edit_Returns_OkResult_When_PersonContactInfoUpdatedSuccessfully()
        {
            var controller = new PersonContactInfosController(serviceMock.Object);

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
            serviceMock.Setup(x => x.UpdateAsync(personId, personContactInfoId, personContactInfoForUpsertDTO)).ReturnsAsync(successfulResult);

            // Act
            var result = await controller.Edit(personId, personContactInfoId, personContactInfoForUpsertDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public async Task Edit_Returns_BadRequestResult_When_PersonContactInfoUpdateFails()
        {
            var controller = new PersonContactInfosController(serviceMock.Object);

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
            serviceMock.Setup(x => x.UpdateAsync(personId, personContactInfoId, personContactInfoForUpsertDTO)).ReturnsAsync(errorResult);

            // Act
            var result = await controller.Edit(personId, personContactInfoId, personContactInfoForUpsertDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected error result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_Returns_OkResult_When_PersonContactInfoDeletedSuccessfully()
        {
            var controller = new PersonContactInfosController(serviceMock.Object);

            var personId = Guid.NewGuid();
            var personContactInfoId = Guid.NewGuid();

            var successfulResult = new SuccessDataResult<string>("PersonContactInfo deleted successfully");

            // Setup mock service to return the expected result when called with the expected parameters
            serviceMock.Setup(x => x.DeleteAsync(personId, personContactInfoId)).ReturnsAsync(successfulResult);

            // Act
            var result = await controller.Delete(personId, personContactInfoId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public async Task Delete_Returns_BadRequestResult_When_PersonContactInfoDeleteFails()
        {
            var controller = new PersonContactInfosController(serviceMock.Object);

            var personId = Guid.NewGuid();
            var personContactInfoId = Guid.NewGuid();

            var errorResult = new ErrorDataResult<string>("PersonContactInfo delete failed");

            // Setup mock service to return the error result when called with the expected parameters
            serviceMock.Setup(x => x.DeleteAsync(personId, personContactInfoId)).ReturnsAsync(errorResult);

            // Act
            var result = await controller.Delete(personId, personContactInfoId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected error result
            Assert.Equal(errorResult, badRequestResult.Value);
        }
    }
}