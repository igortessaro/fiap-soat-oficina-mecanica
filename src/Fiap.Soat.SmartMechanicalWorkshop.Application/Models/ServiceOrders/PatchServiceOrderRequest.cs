using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Models.ServiceOrders;

[ExcludeFromCodeCoverage]
public record PatchServiceOrderRequest(ServiceOrderStatus Status);
