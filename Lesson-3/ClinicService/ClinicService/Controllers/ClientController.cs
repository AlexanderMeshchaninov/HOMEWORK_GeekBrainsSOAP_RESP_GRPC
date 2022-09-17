using ClinicService.Core;
using ClinicService.Data.Models;
using ClinicService.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly IClientRepository _clientRepository;
    private readonly ILogger<ClientController> _logger;
    
    public ClientController(
        IClientRepository clientRepository,
        ILogger<ClientController> logger)
    {
        _logger = logger;
        _clientRepository = clientRepository;
    }
    
    [HttpPost("create")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public IActionResult Create([FromBody] CreateClientRequest createRequest)
    {
        _logger.LogInformation("Create client");
        
        int clientId = _clientRepository.Add(new Client
        {
            FirstName = createRequest.FirstName,
            Patronymic = createRequest.Patronymic,
            Document = createRequest.Document,
            Surname = createRequest.Surname,
        });
        
        return Ok(clientId);
    }
    
    [HttpPut("update")]
    public IActionResult Update([FromBody] UpdateClientRequest updateRequest)
    {
        _logger.LogInformation("Update client");
        
        _clientRepository.Update(new Client
        {
            ClientId = updateRequest.ClientId,
            Surname = updateRequest.Surname,
            FirstName = updateRequest.FirstName,
            Patronymic = updateRequest.Patronymic,
            Document = updateRequest.Document,
        });
        
        return Ok();
    }

    [HttpDelete("delete")]
    public IActionResult Delete([FromQuery] int clientId)
    {
        _clientRepository.Delete(clientId);
        return Ok();
    }

    [HttpGet("get-all")]
    [ProducesResponseType(typeof(IList<Client>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var foundClient = _clientRepository.GetAll();
        return Ok(foundClient);
    }

    [HttpGet("get/{id}")]
    [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
    public IActionResult GetById([FromQuery] int clientId)
    {
        var foundClient = _clientRepository.GetById(clientId);
        return Ok(foundClient);
    }
}