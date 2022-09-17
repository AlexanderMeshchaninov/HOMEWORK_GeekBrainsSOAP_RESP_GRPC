using ClinicService.Data.Models;

namespace ClinicService.Requests;

public class CreateConsultationRequest
{
    public int ClientId { get; set; }
    public int PetId { get; set; }
    public string? Description { get; set; }
    public DateTime ConsultationDate { get; set; }
    public Client? Client { get; set; }
    public Pet? Pet { get; set; }
}