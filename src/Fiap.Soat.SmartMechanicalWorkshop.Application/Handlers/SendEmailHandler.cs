using Fiap.Soat.SmartMechanicalWorkshop.Application.Notifications;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services.Interfaces;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Handlers;

public sealed class SendEmailHandler(
    IServiceOrderRepository repository,
    IEmailTemplateProvider emailTemplateProvider,
    IEmailService emailService) : INotificationHandler<ServiceOrderChangeStatusNotification>
{
    public async Task Handle(ServiceOrderChangeStatusNotification notification, CancellationToken cancellationToken)
    {
        if (notification.ServiceOrder.Status != ServiceOrderStatus.WaitingApproval) return;

        var foundEntity = await repository.GetDetailedAsync(notification.ServiceOrder.Id, cancellationToken);
        if (foundEntity is null) return;

        string html = emailTemplateProvider.GetTemplate(foundEntity);
        await emailService.SendEmailAsync(foundEntity.Client.Email, "Envio de orçamento de serviço(s)", html);
    }
}
