using Business.Constants;
using Business.ValidationRules.FluentValidation.Abstract;
using Entities.DTOs.Params;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Business.ValidationRules.FluentValidation.Concrete
{
    public class UserForLoginValidator : AbstractValidator<UserForLoginDTO>, IEntityValidator
    {
        public UserForLoginValidator()
        {
            RuleFor(p => p.UserName).NotEmpty().WithMessage(Messages.UserNameRequired);
            RuleFor(p => p.Password).NotEmpty().WithMessage(Messages.PasswordRequired);
            RuleFor(p => p.Password).Length(2, 64).WithMessage(Messages.PasswordLength);
        }
    }
}
