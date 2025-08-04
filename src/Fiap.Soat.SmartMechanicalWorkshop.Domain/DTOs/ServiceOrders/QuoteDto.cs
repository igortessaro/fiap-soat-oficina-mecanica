using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

[ExcludeFromCodeCoverage]
public record QuoteDto(Guid Id, decimal Total, QuoteStatus Status, Guid ServiceOrderId);
