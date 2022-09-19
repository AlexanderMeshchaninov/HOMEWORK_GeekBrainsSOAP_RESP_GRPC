using ClinicService.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace ClinicService.Responses
{
    public class AuthentificationResponse
    {
        public AuthentificationStatus Status { get; set; }
        public SessionContext SessionContext { get; set; }
    }
}
