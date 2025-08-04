using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

[ExcludeFromCodeCoverage]
public record ServiceOrderEventDto(Guid Id, ServiceOrderStatus Status, DateTime CreatedAt);
