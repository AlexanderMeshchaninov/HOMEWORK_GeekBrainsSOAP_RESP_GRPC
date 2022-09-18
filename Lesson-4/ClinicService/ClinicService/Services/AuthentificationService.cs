using ClinicService.Core;
using ClinicService.Data;
using ClinicService.Data.Models;
using ClinicService.Models;
using ClinicService.Requests;
using ClinicService.Responses;
using ClinicService.Utilits;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicService.Services
{
    public class AuthentificationService : IAuthentificationService
    {
        private const string SecretKey = "vRguqNCvRxDGMFguqNCMz8w2DGMQ==";
        private readonly Dictionary<string, SessionContext> _sessions;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AuthentificationService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _sessions = new Dictionary<string, SessionContext>();
        }

        public SessionContext GetSessionInfo(string sessionToken)
        {
            throw new NotImplementedException();
        }

        public AuthentificationResponse Login(AuthentificationRequest authenticationRequest)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ClinicServiceDbContext context = scope.ServiceProvider
                .GetRequiredService<ClinicServiceDbContext>();

            Account account = 
                !string.IsNullOrWhiteSpace(authenticationRequest.Login) ?
                FindAccountByLogin(context, authenticationRequest.Login) : null;

            if (account is null)
            {
                return new AuthentificationResponse{ Status = AuthentificationStatus.UserNotFound };
            }

            if (!PasswordUtils.VerifyPassword(
                authenticationRequest.Password, 
                account.PasswordSalt,
                account.PasswordHash))
            {
                return new AuthentificationResponse{ Status = AuthentificationStatus.InvalidPassword };
            }

            AccountSession session = new AccountSession
            {
                SessionToken = CreateSessionToken(account),
                AccountId = account.AccountId,
                TimeCreated = DateTime.Now,
                TimeLastRequest = DateTime.Now,
                IsClosed = false,
            };

            context.AccountSessions.Add(session);
            context.SaveChanges();

            SessionContext sessionContext = GetSessionContext(account, session);

            lock (_sessions)
            {
                _sessions[sessionContext.SessionToken] = sessionContext;
            }

            return new AuthentificationResponse
            {
                Status = AuthentificationStatus.Success,
                SessionContext = sessionContext
            };
        }

        private SessionContext GetSessionContext(Account account, AccountSession accountSession)
        {
            return new SessionContext
            {
                SessionId = accountSession.SessionId,
                SessionToken = accountSession.SessionToken,
                Account = new AccountDto
                {
                    AccountId = account.AccountId,
                    EMail = account.EMail,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    SecondName = account.SecondName,
                    Locked = account.Locked
                }
            };
        }
        private string CreateSessionToken(Account account)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(SecretKey);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                    new Claim(ClaimTypes.Name, account.EMail),
                }),
                
                Expires = DateTime.Now.AddMinutes(15),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }
        private Account FindAccountByLogin(ClinicServiceDbContext context, string login)
        {
            return context.Accounts.FirstOrDefault(account => account.EMail.Equals(login));
        }
    }
}
