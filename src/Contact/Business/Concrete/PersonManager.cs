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
        private IResult ValidatePerson(Guid? id, PersonForUpsertDTO person)
        {
            if (_personDal.Get(c => !c.IsDeleted && c.Id != id && c.FirstName == person.FirstName && c.LastName == person.LastName && c.Company == person.Company) != null)
                return new ErrorResult(Messages.PersonHasBeenUsed);

            return new SuccessResult();
        }
        #endregion

        public IDataResult<IList<PersonForViewDTO>> List(DataTableOptions options)
        {
            return _personDal.List(new Filter(options));
        }

        public IDataResult<PersonForPreviewDTO> Get(Guid id)
        {
            var dataItem = _personDal.Get(id);
            if (dataItem != null)
                return new SuccessDataResult<PersonForPreviewDTO>(dataItem);

            return new ErrorDataResult<PersonForPreviewDTO>();
        }

        [ValidationAspect(typeof(PersonForUpsertValidator), Priority = 1)]
        public IResult Add(PersonForUpsertDTO model)
        {
            var result = BusinessRules.Run(ValidatePerson(null, model));
            if (result != null)
                return new ErrorResult(result.Message);

            var entityToAdd = _mapper.Map<Person>(model);
            _personDal.Add(entityToAdd);

            return new SuccessResult();
        }

        [ValidationAspect(typeof(PersonForUpsertValidator), Priority = 1)]
        public IResult Update(Guid id, PersonForUpsertDTO model)
        {
            var entity = _personDal.Get(f => f.Id == id && !f.IsDeleted);
            if (entity == null)
                return new ErrorResult(Messages.RecordNotFound);

            var result = BusinessRules.Run(ValidatePerson(id, model));
            if (result != null)
                return new ErrorResult(result.Message);

            entity = _mapper.Map(model, entity);
            _personDal.Update(entity);

            return new SuccessResult();
        }

        [TransactionScopeAspect()]
        public IResult Delete(Guid id)
        {
            var entity = _personDal.Get(f => f.Id == id);
            if (entity == null)
                return new ErrorResult(Messages.RecordNotFound);

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = UserId;

            _personDal.Update(entity);

            //PersonContactInfo
            var personContactInfos = _personContactInfoDal.GetList(c => !c.IsDeleted && c.PersonId == id);
            if (personContactInfos != null)
            {
                foreach (var personContactInfo in personContactInfos)
                {
                    personContactInfo.IsDeleted = true;
                    personContactInfo.DeletedOn = DateTime.UtcNow;
                    personContactInfo.DeletedBy = UserId;
                    _personContactInfoDal.Update(personContactInfo);
                }
            }

            return new SuccessResult();
        }


        public IDataResult<IList<PersonReportForViewDTO>> Report()
        {
            var report = _personDal.Report();
            if (report != null)
                return new SuccessDataResult<IList<PersonReportForViewDTO>>(report);

            return new ErrorDataResult<IList<PersonReportForViewDTO>>();
        }
    }
}
