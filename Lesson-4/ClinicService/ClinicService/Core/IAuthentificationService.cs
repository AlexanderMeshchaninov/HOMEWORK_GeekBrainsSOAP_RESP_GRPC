using ClinicService.Models;
using ClinicService.Requests;
using ClinicService.Responses;

namespace ClinicService.Core
{
    public interface IAuthentificationService
    {
        AuthentificationResponse Login(AuthentificationRequest authenticationRequest);
        public SessionContext GetSessionInfo(string sessionToken);
    }
}
