﻿using API.Controllers;
using Business.Abstract;
using Core.Entities.DTOs;
using Core.Utilities.Filtering.DataTable;
using Core.Utilities.Test;
using Entities.DTOs.Params;
using Entities.DTOs.Results;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ContactApi.Tests
{
    public class PersonControllerTest : BaseServiceTest<IPersonService>
    {
        [Fact]
        public void List_Returns_OkResult_When_PersonList_Successful()
        {
            var controller = new PersonsController(serviceMock.Object, loggerMock.Object);

            var options = new DataTableOptions { };

            var successfulResult = new SuccessDataResult<IList<PersonForViewDTO>>
            {
                Data = new List<PersonForViewDTO>
                {
                    new PersonForViewDTO
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Üzeyir",
                        LastName = "Öztemür",
                        Company = "TEST",
                    }
                },
                Success = true,
                Count = 1,
                Message = null
            };

            serviceMock.Setup(x => x.List(options)).Returns(successfulResult);

            // Act
            var result = controller.List(options);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public void List_Returns_BadRequestResult_When_PersonList_Fails()
        {
            var controller = new PersonsController(serviceMock.Object, loggerMock.Object);

            var options = new DataTableOptions { };

            var errorResult = new ErrorDataResult<IList<PersonForViewDTO>>();
            serviceMock.Setup(x => x.List(options)).Returns(errorResult);

            // Act
            var result = controller.List(options);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

        [Fact]
        public void Get_Returns_OkResult_When_PersonFound()
        {
            var controller = new PersonsController(serviceMock.Object, loggerMock.Object);

            var personId = Guid.NewGuid();

            IDataResult<PersonForPreviewDTO> successfulResult = new SuccessDataResult<PersonForPreviewDTO>
            {
                Data = new PersonForPreviewDTO
                {
                    Id = personId,
                    FirstName = "Üzeyir",
                    LastName = "Öztemür",
                    Company = "TEST",
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = null,
                    ContactInfos = new List<PersonContactInfoForViewDTO>()
                },
                Success = true,
                Count = 1,
                Message = null
            };

            serviceMock.Setup(x => x.Get(personId)).Returns(successfulResult);

            // Act
            var result = controller.Get(personId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public void Get_Returns_BadRequestResult_When_PersonNotFound()
        {
            var controller = new PersonsController(serviceMock.Object, loggerMock.Object);

            var personId = Guid.NewGuid();

            var errorResult = new ErrorDataResult<PersonForPreviewDTO>("Person not found");

            // Setup mock service to return the error result when called with the expected model
            serviceMock.Setup(x => x.Get(personId)).Returns(errorResult);

            // Act
            var result = controller.Get(personId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected error result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

        [Fact]
        public void Add_Returns_OkResult_When_PersonAddedSuccessfully()
        {
            var controller = new PersonsController(serviceMock.Object, loggerMock.Object);

            var personForUpsertDTO = new PersonForUpsertDTO
            {
                FirstName = "Üzeyir",
                LastName = "Öztemür",
                Company = "TEST",
                IsActive = true,
            };

            var successfulResult = new SuccessDataResult<string>("Person added successfully");

            // Setup mock service to return the expected result when called with the expected model
            serviceMock.Setup(x => x.Add(personForUpsertDTO)).Returns(successfulResult);

            // Act
            var result = controller.Add(personForUpsertDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public void Add_Returns_BadRequestResult_When_PersonAddFails()
        {
            var controller = new PersonsController(serviceMock.Object, loggerMock.Object);

            var personForUpsertDTO = new PersonForUpsertDTO
            {
                FirstName = "Üzeyir",
                LastName = "Öztemür",
                Company = "TEST",
                IsActive = true,
            };

            var errorResult = new ErrorDataResult<string>("Person add failed");

            // Setup mock service to return the error result when called with the expected model
            serviceMock.Setup(x => x.Add(personForUpsertDTO)).Returns(errorResult);

            // Act
            var result = controller.Add(personForUpsertDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected error result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

        [Fact]
        public void Edit_Returns_OkResult_When_PersonUpdatedSuccessfully()
        {
            var controller = new PersonsController(serviceMock.Object, loggerMock.Object);

            var personId = Guid.NewGuid();
            var personForUpsertDTO = new PersonForUpsertDTO
            {
                FirstName = "Üzeyir",
                LastName = "Öztemür",
                Company = "TEST",
                IsActive = false,
            };

            var successfulResult = new SuccessDataResult<string>("Person updated successfully");

            // Setup mock service to return the expected result when called with the expected parameters
            serviceMock.Setup(x => x.Update(personId, personForUpsertDTO)).Returns(successfulResult);

            // Act
            var result = controller.Edit(personId, personForUpsertDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public void Edit_Returns_BadRequestResult_When_PersonUpdateFails()
        {
            var controller = new PersonsController(serviceMock.Object, loggerMock.Object);

            var personId = Guid.NewGuid();
            var personForUpsertDTO = new PersonForUpsertDTO
            {
                FirstName = "Üzeyir",
                LastName = "Öztemür",
                Company = "TEST",
                IsActive = false,
            };

            var errorResult = new ErrorDataResult<string>("Person update failed");

            // Setup mock service to return the error result when called with the expected parameters
            serviceMock.Setup(x => x.Update(personId, personForUpsertDTO)).Returns(errorResult);

            // Act
            var result = controller.Edit(personId, personForUpsertDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected error result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

        [Fact]
        public void Delete_Returns_OkResult_When_PersonDeletedSuccessfully()
        {
            var controller = new PersonsController(serviceMock.Object, loggerMock.Object);

            var personId = Guid.NewGuid();

            var successfulResult = new SuccessDataResult<string>("Person deleted successfully");

            // Setup mock service to return the expected result when called with the expected parameter
            serviceMock.Setup(x => x.Delete(personId)).Returns(successfulResult);

            // Act
            var result = controller.Delete(personId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public void Delete_Returns_BadRequestResult_When_PersonDeleteFails()
        {
            var controller = new PersonsController(serviceMock.Object, loggerMock.Object);

            var personId = Guid.NewGuid();

            var errorResult = new ErrorDataResult<string>("Person delete failed");

            // Setup mock service to return the error result when called with the expected parameter
            serviceMock.Setup(x => x.Delete(personId)).Returns(errorResult);

            // Act
            var result = controller.Delete(personId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected error result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

        [Fact]
        public void Report_Returns_OkResult_When_PersonReportSuccessful()
        {
            var controller = new PersonsController(serviceMock.Object, loggerMock.Object);

            var successfulResult = new SuccessDataResult<IList<PersonReportForViewDTO>>
            {
                Data = new List<PersonReportForViewDTO>
                {
                    new PersonReportForViewDTO
                    {
                        Location = "BURSA",
                        PersonCount = 2,
                        PhoneNumberCount = 1,
                    }
                },
                Success = true,
                Count = 1,
                Message = null
            };

            // Setup mock service to return the expected result
            serviceMock.Setup(x => x.Report()).Returns(successfulResult);

            // Act
            var result = controller.Report();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public void Report_Returns_BadRequestResult_When_PersonReportFails()
        {
            var controller = new PersonsController(serviceMock.Object, loggerMock.Object);

            var errorResult = new ErrorDataResult<IList<PersonReportForViewDTO>>();

            // Setup mock service to return the error result
            serviceMock.Setup(x => x.Report()).Returns(errorResult);

            // Act
            var result = controller.Report();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected error result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

    }
}