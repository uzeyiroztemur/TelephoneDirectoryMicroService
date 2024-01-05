using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation.Concrete;
using Core.Aspects.Autofac.Validation;
using Core.Entities.DTOs;
using Core.Utilities.Business;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Params;

namespace Business.Concrete
{
    public class PersonContactInfoManager : BaseManager, IPersonContactInfoService
    {
        private readonly IPersonContactInfoDal _personContactInfoDal;
        private readonly IMapper _mapper;

        public PersonContactInfoManager(IPersonContactInfoDal personContactInfoDal, IMapper mapper)
        {
            _personContactInfoDal = personContactInfoDal;
            _mapper = mapper;
        }

        #region Business Rules
        private IResult ValidatePersonContactInfo(Guid personId, Guid? id, PersonContactInfoForUpsertDTO personContactInfo)
        {
            if (_personContactInfoDal.Get(c => !c.IsDeleted && c.Id != id && c.PersonId == personId && c.InfoType == personContactInfo.InfoType && c.InfoValue == personContactInfo.InfoValue) != null)
                return new ErrorResult(Messages.PersonContactInfoHasBeenUsed);

            return new SuccessResult();
        }
        #endregion

        [ValidationAspect(typeof(PersonContactInfoForUpsertValidator), Priority = 1)]
        public IResult Add(Guid personId, PersonContactInfoForUpsertDTO model)
        {
            var result = BusinessRules.Run(ValidatePersonContactInfo(personId, null, model));
            if (result != null)
                return new ErrorResult(result.Message);

            var entityToAdd = _mapper.Map<PersonContactInfo>(model);
            entityToAdd.PersonId = personId;
            _personContactInfoDal.Add(entityToAdd);

            return new SuccessResult();
        }

        [ValidationAspect(typeof(PersonContactInfoForUpsertValidator), Priority = 1)]
        public IResult Update(Guid personId, Guid id, PersonContactInfoForUpsertDTO model)
        {
            var entity = _personContactInfoDal.Get(f => f.PersonId == personId && f.Id == id && !f.IsDeleted);
            if (entity == null)
                return new ErrorResult(Messages.RecordNotFound);

            var result = BusinessRules.Run(ValidatePersonContactInfo(personId, id, model));
            if (result != null)
                return new ErrorResult(result.Message);

            entity = _mapper.Map(model, entity);
            _personContactInfoDal.Update(entity);

            return new SuccessResult();
        }

        public IResult Delete(Guid personId, Guid id)
        {
            var entity = _personContactInfoDal.Get(f => f.PersonId == personId && f.Id == id && !f.IsDeleted);
            if (entity == null)
                return new ErrorResult(Messages.RecordNotFound);

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = UserId;

            _personContactInfoDal.Update(entity);

            return new SuccessResult();
        }
    }
}
