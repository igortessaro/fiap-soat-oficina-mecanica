using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Create;

[ExcludeFromCodeCoverage]
public record CreateSupplyCommand(string Name, int Quantity, decimal Price) : IRequest<Response<Supply>>;
