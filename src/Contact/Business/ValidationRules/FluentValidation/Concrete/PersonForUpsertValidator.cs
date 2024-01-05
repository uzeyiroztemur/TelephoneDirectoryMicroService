using Business.Constants;
using Business.ValidationRules.FluentValidation.Abstract;
using Entities.DTOs.Params;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation.Concrete
{
    public class PersonForUpsertValidator : AbstractValidator<PersonForUpsertDTO>, IEntityValidator
    {
        public PersonForUpsertValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().WithMessage(Messages.FirstNameRequired);
            RuleFor(p => p.FirstName).Length(2, 100).WithMessage(Messages.FirstNameLength);

            RuleFor(p => p.LastName).NotEmpty().WithMessage(Messages.LastNameRequired);
            RuleFor(p => p.LastName).Length(2, 200).WithMessage(Messages.LastNameLength);

            RuleFor(p => p.Company).Length(2, 250).WithMessage(Messages.CompanyLength);
        }
    }
}
