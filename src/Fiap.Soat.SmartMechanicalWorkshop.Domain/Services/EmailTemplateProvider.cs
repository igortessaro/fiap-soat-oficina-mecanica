using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
public class EmailTemplateProvider(IConfiguration configuration) : IEmailTemplateProvider
{

    public string GetTemplate(ServiceOrder serviceOrder)
    {
        string? emailBaseUrl = configuration["Email:BaseUrl"];
        string approveUrl = $"{emailBaseUrl}/api/v1/serviceorders/{serviceOrder.Id}/approve";
        string rejectUrl = $"{emailBaseUrl}/api/v1/serviceorders/{serviceOrder.Id}/reject";

        string html = $@"
  <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #ccc; padding: 20px;'>
    <h2 style='color: #007BFF;'>Ordem de Serviço - Oficina Smart</h2>
    <p>Olá, {serviceOrder.Client.Fullname}!</p>

    <p>Segue abaixo os detalhes da sua ordem de serviço:</p>

    <ul style='list-style: none; padding-left: 0;'>
      <li><strong>ID:</strong> {serviceOrder.Id}</li>
      <li><strong>Título:</strong> {serviceOrder.Title}</li>
      <li><strong>Descrição:</strong> {serviceOrder.Description}</li>
      <li><strong>Status Atual:</strong> {serviceOrder.Status}</li>
      <li><strong>Data Entrada:</strong> {serviceOrder.VehicleCheckInDate:dd/MM/yyyy}</li>
      <li><strong>Data Saída:</strong> {(serviceOrder.VehicleCheckOutDate.HasValue ? serviceOrder.VehicleCheckOutDate.Value.ToString("dd/MM/yyyy") : "—")}</li>
    </ul>

    <h4>Informações do veículo</h4>
    <ul style='list-style: none; padding-left: 0;'>
      <li><strong>Marca:</strong> {serviceOrder.Vehicle.Brand}</li>
      <li><strong>Modelo:</strong> {serviceOrder.Vehicle.Model}</li>
      <li><strong>Ano:</strong> {serviceOrder.Vehicle.ManufactureYear}</li>
      <li><strong>Placa:</strong> {serviceOrder.Vehicle.LicensePlate}</li>
    </ul>

    <h4>Serviços solicitados:</h4>
    <ul>
      {string.Join("", serviceOrder.ServiceOrderAvailableServices.Select(s => $"<li>{s.AvailableServiceId}</li>"))}
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
