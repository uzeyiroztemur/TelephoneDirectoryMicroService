using Business.Constants;
using Business.ValidationRules.FluentValidation.Abstract;
using Entities.DTOs.Params;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation.Concrete
{
    public class PasswordForChangeValidator : AbstractValidator<PasswordForChangeDTO>, IEntityValidator
    {
        public PasswordForChangeValidator()
        {
            RuleFor(p => p.OldPassword).NotEmpty().WithMessage(Messages.PasswordRequired);
            RuleFor(p => p.OldPassword).Length(2, 64).WithMessage(Messages.PasswordLength);

            RuleFor(p => p.NewPassword).NotEmpty().WithMessage(Messages.PasswordRequired);
            RuleFor(p => p.NewPassword).Length(8, 64).WithMessage(Messages.PasswordLength);

            RuleFor(p => p.NewPasswordAgain).Equal(p => p.NewPassword).WithMessage(Messages.PasswordMatch);
        }
    }
}
