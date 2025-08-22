using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Email;

public sealed class SendEmailHandler(
    IServiceOrderRepository repository,
    IEmailTemplateProvider emailTemplateProvider,
    IEmailService emailService) : INotificationHandler<UpdateServiceOrderStatusNotification>
{
    public async Task Handle(UpdateServiceOrderStatusNotification notification, CancellationToken cancellationToken)
    {
        if (notification.ServiceOrder.Status != ServiceOrderStatus.WaitingApproval) return;

        var foundEntity = await repository.GetDetailedAsync(notification.ServiceOrder.Id, cancellationToken);
        if (foundEntity is null) return;

        string html = emailTemplateProvider.GetTemplate(foundEntity);
        await emailService.SendEmailAsync(foundEntity.Client.Email, "Envio de orçamento de serviço(s)", html);
    }
}
