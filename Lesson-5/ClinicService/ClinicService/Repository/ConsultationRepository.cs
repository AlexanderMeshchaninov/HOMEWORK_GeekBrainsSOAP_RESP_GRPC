using ClinicService.Core;
using ClinicService.Data;
using ClinicService.Data.Models;

namespace ClinicService.Repository;

public class ConsultationRepository : IConsultationRepository
{
    private readonly ClinicServiceDbContext _dbContext;
    private readonly ILogger<ConsultationRepository> _logger;

    public ConsultationRepository(
        ClinicServiceDbContext dbContext, 
        ILogger<ConsultationRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public int Add(Consultation item)
    {
        try
        {
            _dbContext.Consultations.Add(item);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Add consultation successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
        }
        
        return item.ConsultationId;
    }
    public void Update(Consultation item)
    {
        try
        {
            if (item is null)
                throw new NullReferenceException();

            var consultation = GetById(item.ConsultationId);

            if (consultation is null)
                throw new KeyNotFoundException();

            consultation.ConsultationDate = item.ConsultationDate;
            consultation.Description = item.Description;
            consultation.Pet = item.Pet;

            _dbContext.Update(consultation);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Consultation info updated successfully");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError($"{ex}");
        }
        catch (NullReferenceException ex)
        {
            _logger.LogError($"{ex}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
        }
    }
    public void Delete(Consultation item)
    {
        try
        {
            if (item is null)
                throw new NullReferenceException();

            Delete(item.ConsultationId);
        }
        catch (NullReferenceException ex)
        {
            _logger.LogError($"{ex}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
        }
    }
    public void Delete(int id)
    {
        try
        {
            var consultation = GetById(id);
            
            if (consultation is null)
                throw new KeyNotFoundException();
            
            _dbContext.Remove(consultation);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Consultation with id: {consultation.ConsultationId} deleted successfully");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError($"{ex}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
        }
    }
    public IList<Consultation> GetAll()
    {
        return _dbContext.Consultations.ToList();
    }
    public Consultation? GetById(int id)
    {
        return _dbContext.Consultations.FirstOrDefault(x => x.ConsultationId.Equals(id));
    }
}