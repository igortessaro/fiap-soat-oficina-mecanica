using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.ServiceOrders;

[ExcludeFromCodeCoverage]
public record PatchServiceOrderRequest(ServiceOrderStatus Status);
