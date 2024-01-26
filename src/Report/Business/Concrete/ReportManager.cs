using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Business.MessageContracts;
using Business.MessageContracts.Commands;
using Core.Aspects.Autofac.Transaction;
using Core.Entities.DTOs;
using Core.Utilities.Business;
using Core.Utilities.Filtering;
using Core.Utilities.Filtering.DataTable;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Constants;
using Entities.DTOs.Params;
using Entities.DTOs.Results;
using MassTransit;

namespace Business.Concrete
{
    public class ReportManager : BaseManager, IReportService
    {
        private readonly IReportDal _reportDal;
        private readonly IReportDetailDal _reportDetailDal;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IMapper _mapper;

        public ReportManager(IReportDal reportDal, IReportDetailDal reportDetailDal, ISendEndpointProvider sendEndpointProvider, IMapper mapper)
        {
            _reportDal = reportDal;
            _reportDetailDal = reportDetailDal;
            _sendEndpointProvider = sendEndpointProvider;
            _mapper = mapper;
        }

        #region Business Rules
        private IResult ValidateReport(Guid? id)
        {
            return new SuccessResult();
        }
        #endregion

        public async Task<IDataResult<IList<ReportForViewDTO>>> ListAsync(DataTableOptions options)
        {
            return await _reportDal.ListAsync(new Filter(options));
        }

        //[CacheAspect(minute: 1)]
        public async Task<IDataResult<ReportForPreviewDTO>> GetAsync(Guid id)
        {
            var dataItem = await _reportDal.GetAsync(id);
            if (dataItem != null)
                return new SuccessDataResult<ReportForPreviewDTO>(dataItem);

            return new ErrorDataResult<ReportForPreviewDTO>();
        }

        public async Task<IDataResult<Guid?>> CreateAsync()
        {
            var result = BusinessRules.Run(ValidateReport(null));
            if (result != null)
                return new ErrorDataResult<Guid?>(result.Message);

            var entityToAdd = new Report
            {
                Status = ReportStatus.Preparing,
            };

            await _reportDal.AddAsync(entityToAdd);

            //Send Quee
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQConstants.CreateReportQueueName}"));
            await sendEndpoint.Send<ICreateReportCommand>(new
            {
                ReportId = entityToAdd.Id,
            });

            return new SuccessDataResult<Guid?>(entityToAdd.Id);
        }

        [TransactionScopeAspectAsync()]
        public async Task<IResult> CreateDetailAsync(Guid reportId, IList<ReportDetailForUpsertDTO> data)
        {
            var reportEntity = await _reportDal.GetAsync(f => f.Id == reportId);
            if (reportEntity == null)
                return new ErrorResult(Messages.RecordNotFound);

            foreach (var item in data)
            {
                var detailToAdd = _mapper.Map<ReportDetail>(item);
                detailToAdd.ReportId = reportId;
                detailToAdd.CreatedBy = reportEntity.CreatedBy;

                await _reportDetailDal.AddAsync(detailToAdd);
            }

            reportEntity.Status = ReportStatus.Completed;

            await _reportDal.UpdateAsync(reportEntity);

            return new SuccessResult();
        }
    }
}
