using API.Controllers;
using Business.Abstract;
using Core.CrossCuttingConcerns.Logging;
using Core.Entities.DTOs;
using Core.Utilities.Filtering.DataTable;
using Entities.DTOs.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ReportApi.Tests
{
    public class ReportControllerTest
    {
        [Fact]
        public void List_Returns_OkResult_When_ReportList_Successful()
        {
            // Arrange
            var reportServiceMock = new Mock<IReportService>();
            var loggerMock = new Mock<ILogger>();

            var controller = new ReportsController(reportServiceMock.Object, loggerMock.Object);

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

            reportServiceMock.Setup(x => x.List(options)).Returns(successfulResult);

            // Act
            var result = controller.List(options);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public void List_Returns_BadRequestResult_When_ReportList_Fails()
        {
            // Arrange
            var reportServiceMock = new Mock<IReportService>();
            var loggerMock = new Mock<ILogger>();

            var controller = new ReportsController(reportServiceMock.Object, loggerMock.Object);

            var options = new DataTableOptions { };

            var errorResult = new ErrorDataResult<IList<ReportForViewDTO>>();
            reportServiceMock.Setup(x => x.List(options)).Returns(errorResult);

            // Act
            var result = controller.List(options);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

        [Fact]
        public void Get_Returns_OkResult_When_ReportFound()
        {
            // Arrange
            var reportServiceMock = new Mock<IReportService>();
            var loggerMock = new Mock<ILogger>();

            var controller = new ReportsController(reportServiceMock.Object, loggerMock.Object);

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

            reportServiceMock.Setup(x => x.Get(reportId)).Returns(successfulResult);

            // Act
            var result = controller.Get(reportId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            // Ensure that the returned object is the expected result
            Assert.Equal(successfulResult, okObjectResult.Value);
        }

        [Fact]
        public void Get_Returns_BadRequestResult_When_ReportNotFound()
        {
            // Arrange
            var reportServiceMock = new Mock<IReportService>();
            var loggerMock = new Mock<ILogger>();

            var controller = new ReportsController(reportServiceMock.Object, loggerMock.Object);

            var reportId = Guid.NewGuid();

            var errorResult = new ErrorDataResult<ReportForPreviewDTO>("Report not found");

            // Setup mock service to return the error result when called with the expected model
            reportServiceMock.Setup(x => x.Get(reportId)).Returns(errorResult);

            // Act
            var result = controller.Get(reportId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            // Ensure that the returned object is the expected error result
            Assert.Equal(errorResult, badRequestResult.Value);
        }

        [Fact]
        public async Task Create_Returns_OkResult_When_ReportCreatedSuccessfully()
        {
            // Arrange
            var reportServiceMock = new Mock<IReportService>();
            var loggerMock = new Mock<ILogger>();

            var controller = new ReportsController(reportServiceMock.Object, loggerMock.Object);

            var successfulResult = new SuccessDataResult<string>("Report created successfully");

            // Setup mock service to return the expected result
            reportServiceMock.Setup(x => x.Create()).ReturnsAsync(successfulResult);

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
            // Arrange
            var reportServiceMock = new Mock<IReportService>();
            var loggerMock = new Mock<ILogger>();

            var controller = new ReportsController(reportServiceMock.Object, loggerMock.Object);

            var errorResult = new ErrorDataResult<string>("Report creation failed");

            // Setup mock service to return the error result
            reportServiceMock.Setup(x => x.Create()).ReturnsAsync(errorResult);

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