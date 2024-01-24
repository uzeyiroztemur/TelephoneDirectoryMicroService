using Business.Abstract;
using Business.MessageContracts.Commands;
using Core.Entities.DTOs;
using Core.Extensions;
using Entities.DTOs.Params;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Business.Consumers
{
    public class CreateReportCommandConsumer : IConsumer<ICreateReportCommand>
    {
        private readonly IConfiguration _configuration;
        private readonly IReportService _reportService;

        public CreateReportCommandConsumer(IConfiguration configuration, IReportService reportService)
        {
            _configuration = configuration;
            _reportService = reportService;
        }

        public async Task Consume(ConsumeContext<ICreateReportCommand> context)
        {
            var message = context.Message;

            // Burada kurgu gerçekleşsin diye bilerek bekleme yapıldı.
            await Task.Delay(10 * 1000);

            var request = new HttpRequest
            {
                Url = $"{_configuration["ContactApiOptions:Uri"]}/contact/persons/report",
                Method = HttpRequestMethod.Get,
                Headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("X-Api-Key", _configuration["ContactApiOptions:ApiKey"])
                }
            };

            var responseString = await request.GetData();
            if (responseString.NotEmpty())
            {
                try
                {
                    var response = JsonConvert.DeserializeObject<DataResult<List<ReportDetailForUpsertDTO>>>(responseString);
                    if (response.Success)
                    {
                        var createResponse = await _reportService.CreateDetailAsync(message.ReportId, response.Data);
                        if (!createResponse.Success)
                            throw new Exception(createResponse.Message);

                        // Notification microservisine bildirim gönderilebilir. (Raporunuz hazırlandı...)
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
