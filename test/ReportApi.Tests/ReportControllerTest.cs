using API.Controllers;
using Business.Abstract;
using Core.Entities.DTOs;
using Core.Utilities.Filtering.DataTable;
using Core.Utilities.Test;
using Entities.DTOs.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ReportApi.Tests
{
    public class ReportControllerTest : BaseServiceTest<IReportService>
    {
        [Fact]
        public async Task List_Returns_OkResult_When_ReportList_Successful()
        {
            var controller = new ReportsController(serviceMock.Object);

            var options = new DataTableOptions { };

            var successfulResult = new SuccessDataResult<IList<ReportForViewDTO>>
            {
                Data = new List<ReportForViewDTO>
                {
                    new ReportForViewDTO
                    {
                        Id = Guid.NewGuid(),
                        CreatedOn = DateTime.UtcNow,
                        Status = Entities.Constants.ReportStatus.Preparing,
                    }
                },
                Success = true,
                Count = 1,
                Message = null
            };

            serviceMock.Setup(x => x.ListAsync(options)).ReturnsAsync(successfulResult);

            // Act
            var result = await controller.List(options);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public async Task List_Returns_BadRequestResult_When_ReportList_Fails()
        {
            var controller = new ReportsController(serviceMock.Object);

            var options = new DataTableOptions { };

            var errorResult = new ErrorDataResult<IList<ReportForViewDTO>>();
            serviceMock.Setup(x => x.ListAsync(options)).ReturnsAsync(errorResult);

            // Act
            var result = await controller.List(options);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

        [Fact]
        public async Task Get_Returns_OkResult_When_ReportFound()
        {
            var controller = new ReportsController(serviceMock.Object);

            var reportId = Guid.NewGuid();

            IDataResult<ReportForPreviewDTO> successfulResult = new SuccessDataResult<ReportForPreviewDTO>
            {
                Data = new ReportForPreviewDTO
                {
                    Id = reportId,
                    CreatedOn = DateTime.UtcNow,
                    Status = Entities.Constants.ReportStatus.Completed,
                    Details = new List<ReportDetailForViewDTO>
                    {
                         new ReportDetailForViewDTO
                         {
                             Id = Guid.NewGuid(),
                             CreatedOn = DateTime.UtcNow,
                             Location = "BURSA",
                             PersonCount = 2,
                             PhoneNumberCount = 1,
                         }
                    }
                },
                Success = true,
                Count = 1,
                Message = null
            };

            serviceMock.Setup(x => x.GetAsync(reportId)).ReturnsAsync(successfulResult);

            // Act
            var result = await controller.Get(reportId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public async Task Get_Returns_BadRequestResult_When_ReportNotFound()
        {
            var controller = new ReportsController(serviceMock.Object);

            var reportId = Guid.NewGuid();

            var errorResult = new ErrorDataResult<ReportForPreviewDTO>("Report not found");

            // Setup mock service to return the error result when called with the expected model
            serviceMock.Setup(x => x.GetAsync(reportId)).ReturnsAsync(errorResult);

            // Act
            var result = await controller.Get(reportId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected error result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

        [Fact]
        public async Task Create_Returns_OkResult_When_ReportCreatedSuccessfully()
        {
            var controller = new ReportsController(serviceMock.Object);

            var successfulResult = new SuccessDataResult<Guid?>("Report created successfully");

            // Setup mock service to return the expected result
            serviceMock.Setup(x => x.CreateAsync()).ReturnsAsync(successfulResult);

            // Act
            var result = await controller.Create();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public async Task Create_Returns_BadRequestResult_When_ReportCreateFails()
        {
            var controller = new ReportsController(serviceMock.Object);

            var errorResult = new ErrorDataResult<Guid?>("Report creation failed");

            // Setup mock service to return the error result
            serviceMock.Setup(x => x.CreateAsync()).ReturnsAsync(errorResult);

            // Act
            var result = await controller.Create();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected error result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

    }
}