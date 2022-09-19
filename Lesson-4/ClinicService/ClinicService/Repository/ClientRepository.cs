using ClinicService.Core;
using ClinicService.Data;
using ClinicService.Data.Models;

namespace ClinicService.Repository;

public class ClientRepository : IClientRepository
{
    private readonly ClinicServiceDbContext _dbContext;
    private readonly ILogger<ClientRepository> _logger;

    public ClientRepository(
        ClinicServiceDbContext dbContext, 
        ILogger<ClientRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public int Add(Client item)
    {
        try
        {
            _dbContext.Clients.Add(item);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Add client successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
        }
        
        return item.ClientId;
    }
    public void Update(Client item)
    {
        try
        {
            if (item is null)
                throw new NullReferenceException();

            var client = GetById(item.ClientId);

            if (client is null)
                throw new KeyNotFoundException();

            client.Surname = item.Surname;
            client.FirstName = item.FirstName;
            client.Patronymic = item.Patronymic;
            client.Document = item.Document;

            _dbContext.Update(client);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Client updated successfully");
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
    public void Delete(Client item)
    {
        try
        {
            if (item is null)
                throw new NullReferenceException();

            Delete(item.ClientId);
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
            var client = GetById(id);
            
            if (client == null)
                throw new KeyNotFoundException();
            
            _dbContext.Remove(client);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Client with id: {client.ClientId} deleted successfully");
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
    public IList<Client> GetAll()
    {
        return _dbContext.Clients.ToList();
    }
    public Client? GetById(int id)
    {
        return _dbContext.Clients.FirstOrDefault(x => x.ClientId.Equals(id));
    }
}