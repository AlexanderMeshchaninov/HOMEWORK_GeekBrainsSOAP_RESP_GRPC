using Grpc.Net.Client;
using ClinicServiceProtoFiles;
using static ClinicServiceProtoFiles.ClinicClientService;


AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
var channel = GrpcChannel.ForAddress("http://localhost:5001");

ClinicClientServiceClient client = new ClinicClientServiceClient(channel);

var createClientResponse = client.CreateClient(new CreateClientRequest
{
    Document = "1232 3243",
    FirstName = "Василиса",
    Surname = "Премудрая",
    Patronymic = "Премудровна"
});

Console.WriteLine($"Client ({createClientResponse.ClientId}) created successfully");

var getClientsResponse = client.GetAllClients(new GetAllClientRequest());

Console.WriteLine("Clients:");
Console.WriteLine("========\n");

foreach (var clientObj in getClientsResponse.Client)
{
    Console.WriteLine($"{clientObj.Document} - {clientObj.Surname} - {clientObj.FirstName} - {clientObj.Patronymic}");
}

var updateClientResponse = client.UpdateClient(new UpdateClientRequest
{
    ClientId = 6,
    Document = "1232 3243",
    FirstName = "Василий",
    Surname = "Пупкин",
    Patronymic = "Александрович"
});

Console.WriteLine($"Client ({updateClientResponse}) updated successfully");

var getClientsAfterUpdateResponse = client.GetAllClients(new GetAllClientRequest());

Console.WriteLine("Clients:");
Console.WriteLine("========\n");

foreach (var clientObj in getClientsAfterUpdateResponse.Client)
{
    Console.WriteLine($"{clientObj.Document} - {clientObj.Surname} - {clientObj.FirstName} - {clientObj.Patronymic}");
}

Console.ReadKey();