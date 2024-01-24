using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation.Concrete;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Entities.DTOs;
using Core.Utilities.Business;
using Core.Utilities.Filtering;
using Core.Utilities.Filtering.DataTable;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Params;
using Entities.DTOs.Results;

namespace Business.Concrete
{
    public class PersonManager : BaseManager, IPersonService
    {
        private readonly IPersonDal _personDal;
        private readonly IPersonContactInfoDal _personContactInfoDal;
        private readonly IMapper _mapper;

        public PersonManager(IPersonDal personDal, IPersonContactInfoDal personContactInfoDal, IMapper mapper)
        {
            _personDal = personDal;
            _personContactInfoDal = personContactInfoDal;
            _mapper = mapper;
        }

        #region Business Rules
        private async Task<IResult> ValidatePerson(Guid? id, PersonForUpsertDTO person)
        {
            if (await _personDal.GetAsync(c => c.Id != id && c.FirstName == person.FirstName && c.LastName == person.LastName && c.Company == person.Company) != null)
                return new ErrorResult(Messages.PersonHasBeenUsed);

            return new SuccessResult();
        }
        #endregion

        public async Task<IDataResult<IList<PersonForViewDTO>>> ListAsync(DataTableOptions options)
        {
            return await _personDal.ListAsync(new Filter(options));
        }

        public async Task<IDataResult<PersonForPreviewDTO>> GetAsync(Guid id)
        {
            var dataItem = await _personDal.GetAsync(id);
            if (dataItem != null)
                return new SuccessDataResult<PersonForPreviewDTO>(dataItem);

            return new ErrorDataResult<PersonForPreviewDTO>(Messages.RecordNotFound);
        }

        [ValidationAspect(typeof(PersonForUpsertValidator), Priority = 1)]
        public async Task<IDataResult<Guid?>> AddAsync(PersonForUpsertDTO model)
        {
            var result = BusinessRules.Run(await ValidatePerson(null, model));
            if (result != null)
                return new ErrorDataResult<Guid?>(result.Message);

            var entityToAdd = _mapper.Map<Person>(model);
            
            await _personDal.AddAsync(entityToAdd);

            return new SuccessDataResult<Guid?>(entityToAdd.Id);
        }

        [ValidationAspect(typeof(PersonForUpsertValidator), Priority = 1)]
        public async Task<IResult> UpdateAsync(Guid id, PersonForUpsertDTO model)
        {
            var entity = await _personDal.GetAsync(f => f.Id == id);
            if (entity == null)
                return new ErrorResult(Messages.RecordNotFound);

            var result = BusinessRules.Run(await ValidatePerson(id, model));
            if (result != null)
                return new ErrorResult(result.Message);

            entity = _mapper.Map(model, entity);
            
            await _personDal.UpdateAsync(entity);

            return new SuccessResult();
        }

        [TransactionScopeAspect()]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = _personDal.Get(f => f.Id == id);
            if (entity == null)
                return new ErrorResult(Messages.RecordNotFound);

            await _personDal.DeleteAsync(entity);

            //PersonContactInfo
            var personContactInfos = await _personContactInfoDal.GetListAsync(c => c.PersonId == id);
            if (personContactInfos != null)
            {
                foreach (var personContactInfo in personContactInfos)
                    await _personContactInfoDal.DeleteAsync(personContactInfo);
            }

            return new SuccessResult();
        }


        public async Task<IDataResult<IList<PersonReportForViewDTO>>> ReportAsync()
        {
            var report = await _personDal.ReportAsync();
            if (report != null)
                return new SuccessDataResult<IList<PersonReportForViewDTO>>(report);

            return new ErrorDataResult<IList<PersonReportForViewDTO>>();
        }
    }
}
