using FluentValidation;

namespace EpicMedia.Web.ViewModels.Validation
{
    public class RegisterValidation:AbstractValidator<RegisterVm>
    {
        public RegisterValidation()
        {
            RuleFor(a => a.Username).NotEmpty();
            RuleFor(a => a.Email).NotEmpty().EmailAddress();
            RuleFor(a => a.Password).NotEmpty().WithMessage("Your password cannot be empty")
                    .MinimumLength(6).WithMessage("Your password length must be at least 6.")
                    .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                    .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                    .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                    .Matches(@"[\@\!\?\*\.]+").WithMessage("Your password must contain at least one (@!? *.).");

        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<RegisterVm>.CreateWithOptions((RegisterVm)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
