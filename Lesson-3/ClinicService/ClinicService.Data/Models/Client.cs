using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicService.Data.Models;

[Table("Clients")]
public class Client
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ClientId { get; set; }

    [Column]
    [StringLength(50)]
    public string? Document { get; set; }

    [Column]
    [StringLength(255)]
    public string? Surname { get; set; }

    [Column]
    [StringLength(255)]
    public string? FirstName { get; set; }

    [Column]
    [StringLength(255)]
    public string? Patronymic { get; set; }

    [InverseProperty(nameof(Pet.Client))]
    public virtual ICollection<Pet> Pets { get; set; } = new HashSet<Pet>();

    [InverseProperty(nameof(Consultation.Client))]
    public virtual ICollection<Consultation> Consultations { get; set; } = new HashSet<Consultation>();
}