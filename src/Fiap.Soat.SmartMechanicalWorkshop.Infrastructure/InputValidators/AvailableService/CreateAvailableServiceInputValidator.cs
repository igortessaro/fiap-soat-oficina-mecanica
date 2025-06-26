using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using FluentValidation;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.InputValidators.AvailableService;

public class CreateAvailableServiceInputValidator : AbstractValidator<CreateAvailableServiceRequest>
{
    public CreateAvailableServiceInputValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}
