using ClinicService.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.Data;

public class ClinicServiceDbContext : DbContext
{
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Pet> Pets { get; set; }
    public virtual DbSet<Consultation> Consultations { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountSession> AccountSessions { get; set; }

    public ClinicServiceDbContext(DbContextOptions options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Consultation>()
            .HasOne(p => p.Pet)
            .WithMany(b => b.Consultations)
            .HasForeignKey(p => p.PetId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}