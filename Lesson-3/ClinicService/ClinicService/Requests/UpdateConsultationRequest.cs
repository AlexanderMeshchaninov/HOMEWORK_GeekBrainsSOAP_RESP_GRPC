namespace ClinicService.Requests;

public class UpdateConsultationRequest
{
    public int ConsultationId { get; set; }
    public string? Description { get; set; }
}