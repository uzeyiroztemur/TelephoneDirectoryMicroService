using FluentValidation;

namespace Core.CrossCuttingConcerns.Validation.FluentValidation
{
    public static class ValidationTool
    {
        public static void ValidateIt(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity);
            var result = validator.Validate(context);
            if (!result.IsValid)
            {
                throw new System.Exception(string.Join("\r\n,", result.Errors));
                //throw new ValidationException(result.Errors);
            }
        }
    }
}
