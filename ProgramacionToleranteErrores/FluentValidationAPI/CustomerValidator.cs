using FluentValidation;

namespace FluentValidationAPI
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");

            RuleFor(customer => customer.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(customer => customer.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("Balance must be greater than or equal to 0.");
        }
    }
}
