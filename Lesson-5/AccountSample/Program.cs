using ClinicService.Utilits;

var result = PasswordUtils.CreatePasswordHash("12345");
Console.WriteLine(result.passwordSalt);
Console.WriteLine(result.passwordHash);
Console.ReadKey();