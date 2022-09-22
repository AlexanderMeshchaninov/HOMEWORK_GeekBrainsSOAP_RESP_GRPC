using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JwtSample;

public class UserService
{
    private IDictionary<string, string> _users = new Dictionary<string, string>()
    {
        {"root1", "test"},
        {"root2", "test"},
        {"root3", "test"},
        {"root4", "test"}
    };

    private const string SecretKey = "vRguqNCvRxDGMFguqNCMz8w2DGMQ==";
    
    public string Authentificate(string user, string password)
    {
        if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
        {
            return string.Empty;
        }

        int i = 0;
        
        foreach (KeyValuePair<string, string> pair in _users)
        {
            if (string.CompareOrdinal(pair.Key, user) is 0 || 
                string.CompareOrdinal(pair.Value, password) is 0)
            {
                return GenerateJwtToken(i);
            }

            i++;
        }

        return null;
    }

    private string GenerateJwtToken(int id)
    {
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        byte[] key = Encoding.ASCII.GetBytes(SecretKey);
        
        SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor();
        securityTokenDescriptor.Expires = DateTime.UtcNow.AddMinutes(15);
        securityTokenDescriptor.Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString())
        });

        securityTokenDescriptor.SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        
        return jwtSecurityTokenHandler.WriteToken(securityToken);
    }
}