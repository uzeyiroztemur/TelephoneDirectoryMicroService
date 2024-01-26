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
        private async Task<IResult> ValidatePersonContactInfo(Guid personId, Guid? id, PersonContactInfoForUpsertDTO personContactInfo)
        {
            if (await _personContactInfoDal.GetAsync(c => c.Id != id && c.PersonId == personId && c.InfoType == personContactInfo.InfoType && c.InfoValue == personContactInfo.InfoValue) != null)
                return new ErrorResult(Messages.PersonContactInfoHasBeenUsed);

            return new SuccessResult();
        }
        #endregion

        [ValidationAspect(typeof(PersonContactInfoForUpsertValidator), Priority = 1)]
        public async Task<IDataResult<Guid?>> AddAsync(Guid personId, PersonContactInfoForUpsertDTO model)
        {
            var result = BusinessRules.Run(await ValidatePersonContactInfo(personId, null, model));
            if (result != null)
                return new ErrorDataResult<Guid?>(result.Message);

            var entityToAdd = _mapper.Map<PersonContactInfo>(model);
            entityToAdd.PersonId = personId;

            await _personContactInfoDal.AddAsync(entityToAdd);

            return new SuccessDataResult<Guid?>(entityToAdd.Id);
        }

        [ValidationAspect(typeof(PersonContactInfoForUpsertValidator), Priority = 1)]
        public async Task<IResult> UpdateAsync(Guid personId, Guid id, PersonContactInfoForUpsertDTO model)
        {
            var entity = await _personContactInfoDal.GetAsync(f => f.PersonId == personId && f.Id == id);
            if (entity == null)
                return new ErrorResult(Messages.RecordNotFound);

            var result = BusinessRules.Run(await ValidatePersonContactInfo(personId, id, model));
            if (result != null)
                return new ErrorResult(result.Message);

            entity = _mapper.Map(model, entity);

            await _personContactInfoDal.UpdateAsync(entity);

            return new SuccessResult();
        }

        public async Task<IResult> DeleteAsync(Guid personId, Guid id)
        {
            var entity = await _personContactInfoDal.GetAsync(f => f.PersonId == personId && f.Id == id);
            if (entity == null)
                return new ErrorResult(Messages.RecordNotFound);

            await _personContactInfoDal.DeleteAsync(entity);

            return new SuccessResult();
        }
    }
}
