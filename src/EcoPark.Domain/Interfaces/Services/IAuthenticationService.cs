using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace EcoPark.Domain.Interfaces.Services;

public interface IAuthenticationService
{
    string GenerateJwtToken(string email, EUserType role);
    string ComputeSha256Hash(string? password);

    string GetUserIpAddress(HttpContext httpContext);
}