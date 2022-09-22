using System.Net.Http.Headers;
using ClinicService.Core;
using ClinicService.Models;
using ClinicService.Models.Requests;
using ClinicService.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace ClinicService.Controllers;

[Authorize]
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthentificationService _authentificationService;

    public AuthController(IAuthentificationService authentificationService)
    {
        _authentificationService = authentificationService;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterClientRequest registrationRequest)
    {
        bool isRegister = _authentificationService.Register(registrationRequest);
        if (isRegister)
        {
            return Ok("Пользователь успешно зарегистрирован");
        }
        
        return BadRequest("Зарегистрировать пользователя не удалось");
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<AuthentificationResponse> Login([FromBody] AuthentificationRequest authenticationRequest)
    {
        AuthentificationResponse authenticationResponse = _authentificationService.Login(authenticationRequest);
        if (authenticationResponse.Status == AuthentificationStatus.Success)
        {
            Response.Headers.Add("X-Session-Token", authenticationResponse.SessionContext.SessionToken);
        }
        
        return Ok(authenticationResponse);
    }

    [HttpGet("session")]
    public ActionResult<SessionContext> GetSession()
    {
        var authorization = Request.Headers[HeaderNames.Authorization];
        if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
        {
            var scheme = headerValue.Scheme;
            var sessionToken = headerValue.Parameter;

            if (string.IsNullOrEmpty(sessionToken))
                return Unauthorized();

            SessionContext sessionContext = _authentificationService.GetSessionInfo(sessionToken);

            if (sessionContext is null)
            {
                return Unauthorized();
            }
            
            return Ok(sessionContext);
        }
        
        return Unauthorized();
    }
}