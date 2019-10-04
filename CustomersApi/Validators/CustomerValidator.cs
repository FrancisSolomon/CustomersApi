using System.Threading.Tasks;

using CustomersApi.V1.Models;

using FluentValidation;

namespace CustomersApi.Validators
{
    public class CustomerValidator : AbstractValidator<DtoCustomer>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.Name)
                .MustAsync((n, c) => Task.FromResult(!string.IsNullOrWhiteSpace(n)))
                .WithMessage((m, c) => "Name must have a value.");
        }
    }
}
