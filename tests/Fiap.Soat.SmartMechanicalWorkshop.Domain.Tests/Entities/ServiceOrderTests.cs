using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.States.ServiceOrder;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.Entities;

public sealed class ServiceOrderTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange
        const string title = "Test Title";
        const string description = "Test Description";
        var vehicleId = Guid.NewGuid();
        var clientId = Guid.NewGuid();

        // Act
        var order = new ServiceOrder(title, description, vehicleId, clientId);

        // Assert
        order.Title.Should().Be(title);
        order.Description.Should().Be(description);
        order.VehicleId.Should().Be(vehicleId);
        order.ClientId.Should().Be(clientId);
        order.AvailableServices.Should().BeEmpty();
        order.Events.Should().BeEmpty();
        order.Quotes.Should().BeEmpty();
    }

    [Fact]
    public void AddAvailableService_ShouldAdd_WhenCanBeUpdated()
    {
        // Arrange
        var order = new ServiceOrder("t", "d", Guid.NewGuid(), Guid.NewGuid());
        order.SetState(new ReceivedState());
        var service = new AvailableService("Service", 10m);

        // Act
        order.AddAvailableService(service);

        // Assert
        order.AvailableServices.Should().Contain(service);
    }

    [Fact]
    public void AddAvailableService_ShouldThrow_WhenCannotBeUpdated()
    {
        // Arrange
        var order = new ServiceOrder("t", "d", Guid.NewGuid(), Guid.NewGuid());
        order.SetState(new CompletedState());
        var service = new AvailableService("Service", 10m);

        // Act
        Action act = () => order.AddAvailableService(service);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Update_ShouldUpdateFieldsAndServices_WhenCanBeUpdated()
    {
        // Arrange
        var order = new ServiceOrder("t", "d", Guid.NewGuid(), Guid.NewGuid());
        order.SetState(new ReceivedState());
        var newServices = new List<AvailableService>
        {
            new("S1", 1m),
            new("S2", 2m)
        };

        // Act
        order.Update("new title", "new desc", newServices);

        // Assert
        order.Title.Should().Be("new title");
        order.Description.Should().Be("new desc");
        order.AvailableServices.Should().BeEquivalentTo(newServices);
    }

    [Fact]
    public void Update_ShouldNotUpdateFields_WhenNullOrEmpty()
    {
        // Arrange
        var order = new ServiceOrder("t", "d", Guid.NewGuid(), Guid.NewGuid());
        order.SetState(new ReceivedState());
        string origTitle = order.Title;
        string origDesc = order.Description;

        // Act
        order.Update(string.Empty, string.Empty, new List<AvailableService>());

        // Assert
        order.Title.Should().Be(origTitle);
        order.Description.Should().Be(origDesc);
    }

    [Fact]
    public void Update_ShouldThrow_WhenCannotBeUpdated()
    {
        // Arrange
        var order = new ServiceOrder("t", "d", Guid.NewGuid(), Guid.NewGuid());
        order.SetState(new CompletedState());

        // Act
        Action act = () => order.Update("x", "y", new List<AvailableService>());

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void SetState_ShouldSetStateAndStatus()
    {
        // Arrange
        var order = new ServiceOrder("t", "d", Guid.NewGuid(), Guid.NewGuid());
        var state = new ReceivedState();

        // Act
        order.SetState(state);

        // Assert
        order.Status.Should().Be(ServiceOrderStatus.Received);
    }

    [Fact]
    public void ChangeStatus_ShouldCallStateChangeStatus()
    {
        // Arrange
        var order = new ServiceOrder("t", "d", Guid.NewGuid(), Guid.NewGuid());
        var mockState = new Mock<ServiceOrderState>();
        order.SetState(mockState.Object);

        // Act
        order.ChangeStatus(ServiceOrderStatus.Completed);

        // Assert
        mockState.Verify(s => s.ChangeStatus(order, ServiceOrderStatus.Completed), Times.Once);
    }

    // [Theory]
    // [InlineData(ServiceOrderStatus.Received, typeof(ReceivedState))]
    // [InlineData(ServiceOrderStatus.UnderDiagnosis, typeof(UnderDiagnosisState))]
    // [InlineData(ServiceOrderStatus.WaitingApproval, typeof(WaitingApprovalState))]
    // [InlineData(ServiceOrderStatus.InProgress, typeof(InProgressState))]
    // [InlineData(ServiceOrderStatus.Completed, typeof(CompletedState))]
    // [InlineData(ServiceOrderStatus.Delivered, typeof(DeliveredState))]
    // [InlineData(ServiceOrderStatus.Cancelled, typeof(CancelledState))]
    // [InlineData(ServiceOrderStatus.Rejected, typeof(RejectedState))]
    // public void ChangeStatus_ShouldSetCorrectState(ServiceOrderStatus status, Type expectedType)
    // {
    //     // Arrange
    //     var order = new ServiceOrder("t", "d", Guid.NewGuid(), Guid.NewGuid());
    //     order.SetState(new ReceivedState());
    //     typeof(ServiceOrder)
    //         .GetProperty("Status", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
    //         ?.SetValue(order, status);
    //
    //     // Act
    //     order.ChangeStatus(expectedType);
    //
    //     // Assert
    //     order.GetType().GetField("_state", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    //         ?.GetValue(order)
    //         .Should().BeOfType(expectedType);
    // }

    [Theory]
    [InlineData(ServiceOrderStatus.Received, ServiceOrderStatus.UnderDiagnosis)]
    [InlineData(ServiceOrderStatus.UnderDiagnosis, ServiceOrderStatus.WaitingApproval)]
    [InlineData(ServiceOrderStatus.WaitingApproval, ServiceOrderStatus.InProgress)]
    [InlineData(ServiceOrderStatus.WaitingApproval, ServiceOrderStatus.Rejected)]
    [InlineData(ServiceOrderStatus.WaitingApproval, ServiceOrderStatus.Cancelled)]
    [InlineData(ServiceOrderStatus.InProgress, ServiceOrderStatus.Completed)]
    [InlineData(ServiceOrderStatus.Completed, ServiceOrderStatus.Delivered)]
    // [InlineData(ServiceOrderStatus.Delivered, typeof(DeliveredState))]
    [InlineData(ServiceOrderStatus.Cancelled, ServiceOrderStatus.Delivered)]
    [InlineData(ServiceOrderStatus.Rejected, ServiceOrderStatus.WaitingApproval)]
    [InlineData(ServiceOrderStatus.Delivered, ServiceOrderStatus.Delivered)]
    public void ChangeStatus_ShouldSetCorrectState(ServiceOrderStatus initialStatus, ServiceOrderStatus nextStatus)
    {
        // Arrange
        var order = new ServiceOrder("t", "d", Guid.NewGuid(), Guid.NewGuid());
        order.SyncState();
        typeof(ServiceOrder)
            .GetProperty("Status", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
            ?.SetValue(order, initialStatus);
        order.SyncState();

        // Act
        order.ChangeStatus(nextStatus);

        // Assert
        order.Status.Should().Be(nextStatus);
    }
    // Received, UnderDiagnosis, WaitingApproval, InProgress, Completed, Delivered, Cancelled, Rejected
    [Theory]
    [InlineData(ServiceOrderStatus.Received, ServiceOrderStatus.WaitingApproval)]
    [InlineData(ServiceOrderStatus.Received, ServiceOrderStatus.InProgress)]
    [InlineData(ServiceOrderStatus.Received, ServiceOrderStatus.Completed)]
    [InlineData(ServiceOrderStatus.Received, ServiceOrderStatus.Delivered)]
    [InlineData(ServiceOrderStatus.Received, ServiceOrderStatus.Rejected)]
    [InlineData(ServiceOrderStatus.UnderDiagnosis, ServiceOrderStatus.Received)]
    [InlineData(ServiceOrderStatus.UnderDiagnosis, ServiceOrderStatus.InProgress)]
    [InlineData(ServiceOrderStatus.UnderDiagnosis, ServiceOrderStatus.Completed)]
    [InlineData(ServiceOrderStatus.UnderDiagnosis, ServiceOrderStatus.Delivered)]
    [InlineData(ServiceOrderStatus.UnderDiagnosis, ServiceOrderStatus.Rejected)]
    [InlineData(ServiceOrderStatus.WaitingApproval, ServiceOrderStatus.Received)]
    [InlineData(ServiceOrderStatus.WaitingApproval, ServiceOrderStatus.UnderDiagnosis)]
    [InlineData(ServiceOrderStatus.WaitingApproval, ServiceOrderStatus.Completed)]
    [InlineData(ServiceOrderStatus.WaitingApproval, ServiceOrderStatus.Delivered)]
    [InlineData(ServiceOrderStatus.InProgress, ServiceOrderStatus.Received)]
    [InlineData(ServiceOrderStatus.InProgress, ServiceOrderStatus.UnderDiagnosis)]
    [InlineData(ServiceOrderStatus.InProgress, ServiceOrderStatus.WaitingApproval)]
    [InlineData(ServiceOrderStatus.InProgress, ServiceOrderStatus.Delivered)]
    [InlineData(ServiceOrderStatus.InProgress, ServiceOrderStatus.Rejected)]
    [InlineData(ServiceOrderStatus.Completed, ServiceOrderStatus.Received)]
    [InlineData(ServiceOrderStatus.Completed, ServiceOrderStatus.UnderDiagnosis)]
    [InlineData(ServiceOrderStatus.Completed, ServiceOrderStatus.WaitingApproval)]
    [InlineData(ServiceOrderStatus.Completed, ServiceOrderStatus.InProgress)]
    [InlineData(ServiceOrderStatus.Completed, ServiceOrderStatus.Rejected)]
    [InlineData(ServiceOrderStatus.Delivered, ServiceOrderStatus.Received)]
    [InlineData(ServiceOrderStatus.Delivered, ServiceOrderStatus.UnderDiagnosis)]
    [InlineData(ServiceOrderStatus.Delivered, ServiceOrderStatus.WaitingApproval)]
    [InlineData(ServiceOrderStatus.Delivered, ServiceOrderStatus.InProgress)]
    [InlineData(ServiceOrderStatus.Delivered, ServiceOrderStatus.Completed)]
    [InlineData(ServiceOrderStatus.Delivered, ServiceOrderStatus.Rejected)]
    [InlineData(ServiceOrderStatus.Cancelled, ServiceOrderStatus.Received)]
    [InlineData(ServiceOrderStatus.Cancelled, ServiceOrderStatus.UnderDiagnosis)]
    [InlineData(ServiceOrderStatus.Cancelled, ServiceOrderStatus.WaitingApproval)]
    [InlineData(ServiceOrderStatus.Cancelled, ServiceOrderStatus.InProgress)]
    [InlineData(ServiceOrderStatus.Cancelled, ServiceOrderStatus.Completed)]
    [InlineData(ServiceOrderStatus.Cancelled, ServiceOrderStatus.Rejected)]
    [InlineData(ServiceOrderStatus.Rejected, ServiceOrderStatus.Received)]
    [InlineData(ServiceOrderStatus.Rejected, ServiceOrderStatus.UnderDiagnosis)]
    [InlineData(ServiceOrderStatus.Rejected, ServiceOrderStatus.InProgress)]
    [InlineData(ServiceOrderStatus.Rejected, ServiceOrderStatus.Completed)]
    [InlineData(ServiceOrderStatus.Rejected, ServiceOrderStatus.Rejected)]
    [InlineData(ServiceOrderStatus.Rejected, ServiceOrderStatus.Cancelled)]
    public void ChangeStatus_ShouldReturnDomainException(ServiceOrderStatus initialStatus, ServiceOrderStatus nextStatus)
    {
        // Arrange
        var order = new ServiceOrder("t", "d", Guid.NewGuid(), Guid.NewGuid());
        order.SyncState();
        typeof(ServiceOrder)
            .GetProperty("Status", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
            ?.SetValue(order, initialStatus);
        order.SyncState();

        // Act
        Action act = () => order.ChangeStatus(nextStatus);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(QuoteStatus.Pending, ServiceOrderStatus.UnderDiagnosis)]
    [InlineData(QuoteStatus.Approved, ServiceOrderStatus.InProgress)]
    [InlineData(QuoteStatus.Rejected, ServiceOrderStatus.Rejected)]
    public void GetNextStatus_ShouldReturnExpected(QuoteStatus quoteStatus, ServiceOrderStatus expected)
    {
        ServiceOrder.GetNextStatus(quoteStatus).Should().Be(expected);
    }

    [Fact]
    public void GetNextStatus_ShouldThrow_OnUnknownStatus()
    {
        Action act = () => ServiceOrder.GetNextStatus((QuoteStatus) 999);

        act.Should().Throw<DomainException>();
    }
}
