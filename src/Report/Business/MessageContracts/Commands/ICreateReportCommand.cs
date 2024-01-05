namespace Business.MessageContracts.Commands
{
    public interface ICreateReportCommand
    {
        Guid ReportId { get; }
    }
}
