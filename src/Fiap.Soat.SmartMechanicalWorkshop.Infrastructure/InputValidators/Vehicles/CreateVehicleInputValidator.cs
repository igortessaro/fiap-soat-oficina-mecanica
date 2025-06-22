using Fiap.Soat.SmartMechanicalWorkshop.Api.DTOs.Vehicles;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.InputValidators.Vehicles
{
    public class CreateVehicleInputValidator : AbstractValidator<CreateNewVehicleRequest>
    {
        public CreateVehicleInputValidator()
        {
             RuleFor(x => x.LicensePlate)
                .NotEmpty().WithMessage("A placa é obrigatória.")
                .Must(IsValidLicensePlate).WithMessage("Formato de placa inválido.");
        }

        private bool IsValidLicensePlate(string placa)
        {
            Regex regex1 = new(@"^[A-Z]{3}-?[0-9]{4}$");
            Regex regex2 = new(@"^[A-Z]{3}[0-9][A-Z][0-9]{2}$");
            return regex1.IsMatch(placa) || regex2.IsMatch(placa);
        }
    }
}
