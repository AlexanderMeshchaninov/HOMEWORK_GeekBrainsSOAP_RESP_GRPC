using ClinicService.Core;
using ClinicService.Data.Models;
using ClinicServiceProtoFiles;
using Grpc.Core;
using static ClinicServiceProtoFiles.ClinicClientService;

namespace ClinicService.Services;

public class ClinicClientService : ClinicClientServiceBase
{
    private readonly IClientRepository _clientRepository;
    private readonly ILogger<ClinicClientService> _logger;

    public ClinicClientService(
        IClientRepository clientRepository,
        ILogger<ClinicClientService> logger
        )
    {
        _clientRepository = clientRepository;
        _logger = logger;
    }

    public override Task<CreateClientResponse> CreateClient(CreateClientRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Create client");

        int clientId = _clientRepository.Add(new Client
        {
            FirstName = request.FirstName,
            Patronymic = request.Patronymic,
            Document = request.Document,
            Surname = request.Surname,
        });

        var response = new CreateClientResponse
        {
            ClientId = clientId
        };

        return Task.FromResult(response);
    }

    public override Task<UpdateClientResponse> UpdateClient(UpdateClientRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Update client");

        _clientRepository.Update(new Client
        {
            ClientId = request.ClientId,
            Surname = request.Surname,
            FirstName = request.FirstName,
            Patronymic = request.Patronymic,
            Document = request.Document,
        });

        var response = new UpdateClientResponse();

        return Task.FromResult(response);
    }

    public override Task<DeleteClientResponse> DeleteClient(DeleteClientRequest request, ServerCallContext context)
    {
        _clientRepository.Delete(request.ClientId);

        var response = new DeleteClientResponse();

        return Task.FromResult(response);
    }

    public override Task<ClientResponse> GetClientById(GetClientByIdRequest request, ServerCallContext context)
    {
        var client = _clientRepository.GetById(request.ClientId);

        var response = new ClientResponse()
        {
            ClientId = client.ClientId,
            Document = client.Document,
            FirstName = client.FirstName,
            Patronymic = client.Patronymic,
            Surname = client.Surname
        };

        return Task.FromResult(response);
    }

    public override Task<GetAllClientResponse> GetAllClients(GetAllClientRequest request, ServerCallContext context)
    {
        var response = new GetAllClientResponse();

        response.Client.AddRange(_clientRepository.GetAll().Select(x => new ClientResponse 
        {
            ClientId = x.ClientId,
            Document = x.Document,
            FirstName = x.FirstName,
            Patronymic = x.Patronymic,
            Surname = x.Surname
        }).ToList());

        return Task.FromResult(response);
    }
}