using ClinicService.Models;
using ClinicService.Models.Requests;
using ClinicService.Responses;

namespace ClinicService.Core
{
    public interface IAuthentificationService
    {
        bool Register(RegisterClientRequest registerRequest);
        AuthentificationResponse Login(AuthentificationRequest authentificationRequest);
        public SessionContext GetSessionInfo(string sessionToken);
    }
}
