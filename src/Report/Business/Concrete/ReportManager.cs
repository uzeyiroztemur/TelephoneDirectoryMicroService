using AutoMapper;
using Business.Abstract;
using Business.Constants;
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

        public IDataResult<IList<ReportForViewDTO>> List(DataTableOptions options)
        {
            return _reportDal.List(new Filter(options));
        }

        public IDataResult<ReportForPreviewDTO> Get(Guid id)
        {
            var dataItem = _reportDal.Get(id);
            if (dataItem != null)
                return new SuccessDataResult<ReportForPreviewDTO>(dataItem);

            return new ErrorDataResult<ReportForPreviewDTO>();
        }

        public async Task<IResult> Create()
        {
            var result = BusinessRules.Run(ValidateReport(null));
            if (result != null)
                return new ErrorResult(result.Message);

            var entityToAdd = new Report
            {
                Status = ReportStatus.Preparing,
            };

            _reportDal.Add(entityToAdd);

            //SendQuee

            return new SuccessResult();
        }

        [TransactionScopeAspect()]
        public IResult CreateDetail(Guid reportId, IList<ReportDetailForUpsertDTO> data)
        {
            var reportEntity = _reportDal.Get(f => f.Id == reportId);
            if (reportEntity == null)
                return new ErrorResult(Messages.RecordNotFound);

            foreach (var item in data)
            {
                var detailToAdd = _mapper.Map<ReportDetail>(item);
                detailToAdd.ReportId = reportId;
                detailToAdd.CreatedBy = reportEntity.CreatedBy;
                _reportDetailDal.Add(detailToAdd);
            }

            reportEntity.Status = ReportStatus.Completed;
            _reportDal.Update(reportEntity);

            return new SuccessResult();
        }
    }
}
