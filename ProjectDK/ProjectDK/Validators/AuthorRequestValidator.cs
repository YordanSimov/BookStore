using FluentValidation;
using ProjectDK.Models.Requests;

namespace ProjectDK.Validators
{
    public class AuthorRequestValidator : AbstractValidator<AuthorRequest>
    {
        public AuthorRequestValidator()
        {
            RuleFor(x => x.Age)
                .GreaterThan(0)
                .WithMessage("My custom message for Age zero")
                .LessThanOrEqualTo(120)
                .WithMessage("My custom message for Age 120");

            RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(10);

            When(x => !string.IsNullOrEmpty(x.Nickname), () =>
            {
                RuleFor(x => x.Nickname).MinimumLength(2).MaximumLength(10);
            });

            RuleFor(x => x.DateOfBirth).GreaterThan(DateTime.MinValue).LessThan(DateTime.MaxValue);
        }
    }
}