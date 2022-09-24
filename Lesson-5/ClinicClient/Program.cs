using Grpc.Net.Client;
using ClinicServiceProtoFiles;
using Grpc.Core;
using static ClinicServiceProtoFiles.ClinicClientService;
using static ClinicServiceProtoFiles.AuthService;

var channel = GrpcChannel.ForAddress("https://localhost:5001");

AuthServiceClient authenticateServiceClient = new AuthServiceClient(channel);

var authenticationResponse = authenticateServiceClient.Login(new AuthentificationRequest()
{
    Login = "vasia_pupkin@mail.ru",
    Password = "12345"
});

if (authenticationResponse.Status != 0)
{
    Console.WriteLine("Authentication error.");
    Console.ReadKey();
    return;
}

Console.WriteLine($"Session token: {authenticationResponse.SessionContext.SessionToken}");

var callCredentials = CallCredentials.FromInterceptor((c, m) =>
{
    m.Add("Authorization",
        $"Bearer {authenticationResponse.SessionContext.SessionToken}");
    return Task.CompletedTask;
});

var protectedChannel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
{
    Credentials = ChannelCredentials.Create(new SslCredentials(), callCredentials)
});

ClinicClientServiceClient client = new ClinicClientServiceClient(protectedChannel);

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