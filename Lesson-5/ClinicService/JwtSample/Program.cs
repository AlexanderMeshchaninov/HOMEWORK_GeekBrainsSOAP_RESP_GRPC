using JwtSample;

Console.WriteLine("Enter user name: ");
string userName = Console.ReadLine();

Console.WriteLine("Enter user password: ");
string userPassword = Console.ReadLine();

UserService userService = new UserService();
string token = userService.Authentificate(userName, userPassword);
Console.WriteLine(token);

Console.ReadKey(true);