using ClinicService.Core;
using ClinicServiceProtoFiles;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using static ClinicServiceProtoFiles.AuthService;
using System.Net.Http.Headers;

namespace ClinicService.Services
{
    public class AuthService : AuthServiceBase
    {
        private readonly IAuthentificationService _authenticateService;

        public AuthService(IAuthentificationService authentificationService)
        {
            _authenticateService = authentificationService;
        }

        [AllowAnonymous]
        public override Task<RegisterClientResponse> Register(RegisterClientRequest request, ServerCallContext context)
        {
            var isRegister = _authenticateService.Register(new Models.Requests.RegisterClientRequest 
            {
                EMail = request.Email,
                Password = request.Password,
                FirstName = request.Firstname,
                SecondName = request.Secondname,
                LastName = request.Lastname
            });

            if (isRegister)
            {
                return Task.FromResult(new RegisterClientResponse());
            }

            return Task.FromResult(new RegisterClientResponse());
        }

        [AllowAnonymous]
        public override Task<AuthentificationResponse> Login(AuthentificationRequest request, ServerCallContext context)
        {
            var response = _authenticateService.Login(new Models.Requests.AuthentificationRequest
            {
                Login = request.Login,
                Password = request.Password
            });

            if (response.Status == Models.AuthentificationStatus.Success)
            {
                context.ResponseTrailers.Add("X-Session-Token", response.SessionContext.SessionToken);
            }

            return Task.FromResult(new AuthentificationResponse
            {
                Status = (int)response.Status,
                SessionContext = new SessionContext
                {
                    SessionId = response.SessionContext.SessionId,
                    SessionToken = response.SessionContext.SessionToken,

                    Account = new AccountDto
                    {
                        Accountid = response.SessionContext.Account.AccountId,
                        Email = response.SessionContext.Account.EMail,
                        Firstname = response.SessionContext.Account.FirstName,
                        Lastname = response.SessionContext.Account.LastName,
                        Secondname = response.SessionContext.Account.SecondName,
                        Locked = response.SessionContext.Account.Locked
                    }
                }
            });
        }

        public override Task<GetSessionResponse> GetSessionInfo(GetSessionRequest request, ServerCallContext context)
        {
            var authorizationHeader = context.RequestHeaders.FirstOrDefault(header => header.Key.Equals("Authorization"));

            if (AuthenticationHeaderValue.TryParse(authorizationHeader.Value, out var headerValue))
            {
                var scheme = headerValue.Scheme; // "Bearer"
                var sessionToken = headerValue.Parameter; // Token
                if (string.IsNullOrEmpty(sessionToken))
                {
                    return Task.FromResult(new GetSessionResponse());
                }

                Models.SessionContext sessionContext = _authenticateService.GetSessionInfo(sessionToken);
                if (sessionContext == null)
                {
                    return Task.FromResult(new GetSessionResponse());
                }

                return Task.FromResult(new GetSessionResponse
                {
                    SessionContext = new SessionContext
                    {
                        SessionId = sessionContext.SessionId,
                        SessionToken = sessionContext.SessionToken,
                        Account = new AccountDto
                        {
                            Accountid = sessionContext.Account.AccountId,
                            Email = sessionContext.Account.EMail,
                            Firstname = sessionContext.Account.FirstName,
                            Lastname = sessionContext.Account.LastName,
                            Secondname = sessionContext.Account.SecondName,
                            Locked = sessionContext.Account.Locked
                        }
                    }
                });
            }

            return Task.FromResult(new GetSessionResponse());
        }
    }
}
