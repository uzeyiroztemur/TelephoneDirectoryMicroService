using Business.Constants;
using Business.ValidationRules.FluentValidation.Abstract;
using Entities.DTOs.Params;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation.Concrete
{
    public class PersonContactInfoForUpsertValidator : AbstractValidator<PersonContactInfoForUpsertDTO>, IEntityValidator
    {
        public PersonContactInfoForUpsertValidator()
        {
            RuleFor(p => p.InfoValue).NotEmpty().WithMessage(Messages.InfoValueRequired);
            RuleFor(p => p.InfoValue).Length(2, 250).WithMessage(Messages.InfoValueLength);
        }
    }
}
