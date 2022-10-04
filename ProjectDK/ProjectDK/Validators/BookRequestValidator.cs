using FluentValidation;
using ProjectDK.Models.Requests;

namespace ProjectDK.Validators
{
    public class BookRequestValidator : AbstractValidator<BookRequest>
    {
        public BookRequestValidator()
        {
            When(x => !string.IsNullOrEmpty(x.Title) && !x.Title.Any(char.IsDigit), () =>
            {
                RuleFor(x => x.Title).MinimumLength(3).MaximumLength(15).WithMessage("Title not in range");
            });

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
