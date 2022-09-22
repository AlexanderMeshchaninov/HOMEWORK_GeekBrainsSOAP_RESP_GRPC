namespace ClinicService.Models.Requests;

public class UpdatePetRequest
{
    public int PetId { get; set; }
    public string? Name { get; set; }
    public DateTime Birthday { get; set; }
}