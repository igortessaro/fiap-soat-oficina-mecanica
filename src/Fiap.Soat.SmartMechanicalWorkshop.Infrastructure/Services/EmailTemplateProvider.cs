using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services;

public class EmailTemplateProvider(IConfiguration configuration) : IEmailTemplateProvider
{
    public string GetTemplate(ServiceOrder serviceOrder)
    {
        string? emailBaseUrl = configuration["Email:BaseUrl"];
        var quoteId = serviceOrder.Quotes.FirstOrDefault(x => x.Status == QuoteStatus.Pending)?.Id ?? Guid.Empty;
        string approveUrl = $"{emailBaseUrl}/api/v1/serviceorders/{serviceOrder.Id}/quote/{quoteId}/approved";
        string rejectUrl = $"{emailBaseUrl}/api/v1/serviceorders/{serviceOrder.Id}/quote/{quoteId}/rejected";
        var servicesHtml = new StringBuilder();
        foreach (var service in serviceOrder.AvailableServices)
        {
            servicesHtml.AppendLine("<li>");
            servicesHtml.AppendLine($"<strong>{service.Name}</strong> - R$ {service.Price:F2}");

            if (service.AvailableServiceSupplies.Any())
            {
                servicesHtml.AppendLine("<ul>");
                foreach (var supply in service.AvailableServiceSupplies)
                {
                    servicesHtml.AppendLine($"<li>{supply.Supply.Name} (Qtd: {supply.Quantity}) - R$ {supply.Supply.Price:F2}</li>");
                }

                servicesHtml.AppendLine("</ul>");
            }

            servicesHtml.AppendLine("</li>");
        }

        string html = $@"
              <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #ccc; padding: 20px;'>
                <h2 style='color: #007BFF;'>Ordem de Serviço - Oficina Smart</h2>
                <p>Olá, {serviceOrder.Client.Fullname}!</p>
             <ul style='list-style: none; padding-left: 0;'>
                  <li><strong>Document:</strong> {serviceOrder.Client.Document}</li>
                  <li><strong>Email:</strong> {serviceOrder.Client.Email.Address}</li>
                  <li><strong>Phone:</strong> {serviceOrder.Client.Phone}</li>
                </ul>


                <p>Segue abaixo os detalhes da sua ordem de serviço:</p>

                <ul style='list-style: none; padding-left: 0;'>
                  <li><strong>ID:</strong> {serviceOrder.Id}</li>
                  <li><strong>Título:</strong> {serviceOrder.Title}</li>
                  <li><strong>Descrição:</strong> {serviceOrder.Description}</li>
                  <li><strong>Status Atual:</strong> {serviceOrder.Status}</li>

                </ul>

                <h4>Informações do veículo</h4>
                <ul style='list-style: none; padding-left: 0;'>
                  <li><strong>Marca:</strong> {serviceOrder.Vehicle.Brand}</li>
                  <li><strong>Modelo:</strong> {serviceOrder.Vehicle.Model}</li>
                  <li><strong>Ano:</strong> {serviceOrder.Vehicle.ManufactureYear}</li>
                  <li><strong>Placa:</strong> {serviceOrder.Vehicle.LicensePlate}</li>
                </ul>

            <br>
            <h2>Serviços solicitados:</h2>
            <ul>
               {servicesHtml}
            </ul>


            <br>
            <br>
            <h2>Valor final de orçamento:</h2>
            <ul>
               <strong>{00000}<strong>
            </ul>

                <div style='margin-top: 30px;'>
                  <a href='{approveUrl}' style='padding: 12px 20px; background-color: #28a745; color: white; text-decoration: none; border-radius: 5px; margin-right: 10px;'>Aprovar</a>
                  <a href='{rejectUrl}' style='padding: 12px 20px; background-color: #dc3545; color: white; text-decoration: none; border-radius: 5px;'>Reprovar</a>
                </div>

                <p style='margin-top: 40px;'>Obrigado por escolher a <strong>Oficina Smart</strong>! Em caso de dúvidas, entre em contato pelo WhatsApp: <a href='tel:+5500000000000'>+55 00 0000-0000</a>.</p>
              </div>";
        return html;
    }
}
