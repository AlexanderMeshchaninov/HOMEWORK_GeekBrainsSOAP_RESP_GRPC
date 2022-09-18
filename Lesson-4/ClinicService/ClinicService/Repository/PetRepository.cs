using ClinicService.Core;
using ClinicService.Data;
using ClinicService.Data.Models;

namespace ClinicService.Repository;

public class PetRepository : IPetRepository
{
    private readonly ClinicServiceDbContext _dbContext;
    private readonly ILogger<PetRepository> _logger;

    public PetRepository(
        ClinicServiceDbContext dbContext,
        ILogger<PetRepository> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    public int Add(Pet item)
    {
        try
        {
            _dbContext.Pets.Add(item);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Add pet successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
        }
        
        return item.PetId;
    }
    public void Update(Pet item)
    {
        try
        {
            if (item is null)
                throw new NullReferenceException();

            var pet = GetById(item.PetId);

            if (pet is null)
                throw new KeyNotFoundException();

            pet.Name = item.Name;
            pet.Birthday = item.Birthday;

            _dbContext.Update(pet);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Pet info updated successfully");
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
    public void Delete(Pet item)
    {
        try
        {
            if (item is null)
                throw new NullReferenceException();

            Delete(item.PetId);
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
            var pet = GetById(id);
            
            if (pet is null)
                throw new KeyNotFoundException();
            
            _dbContext.Remove(pet);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Pet with id: {pet.PetId} deleted successfully");
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
    public IList<Pet> GetAll()
    {
        return _dbContext.Pets.ToList();
    }
    public Pet? GetById(int id)
    {
        return _dbContext.Pets.FirstOrDefault(x => x.PetId.Equals(id));
    }
}