using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EcoPark.Application.Authentication.Services;

public class AuthenticationService(IConfiguration configuration) : IAuthenticationService
{
    public string GenerateJwtToken(string email, EUserType role)
    {
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new ("userName", email),
            new (ClaimTypes.Role, role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            expires: role != EUserType.System ? DateTime.Now.AddMinutes(30) : DateTime.Now.AddYears(1),
            signingCredentials: credentials,
            claims: claims);

        var tokenHandler = new JwtSecurityTokenHandler();

        var stringToken = tokenHandler.WriteToken(token);

        return stringToken;
    }

    public string ComputeSha256Hash(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return "";

        using SHA256 sha256Hash = SHA256.Create();

        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

        StringBuilder builder = new StringBuilder();

        foreach (var t in bytes)
            builder.Append(t.ToString("x2"));

        return builder.ToString();
    }

    public string GetUserIpAddress(HttpContext httpContext)
    {
        // Verificar se o cabeçalho 'X-Forwarded-For' está presente
        string ipAddress = httpContext.Request.Headers["X-Forwarded-For"];

        // Se não houver 'X-Forwarded-For', obter o endereço IP do cliente diretamente
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = httpContext.Connection.RemoteIpAddress.ToString();
        }

        // Remover a porta, se presente
        int portIndex = ipAddress.IndexOf(':');
        if (portIndex != -1)
        {
            ipAddress = ipAddress.Substring(0, portIndex);
        }

        // Validar e retornar o endereço IP
        if (IsIPv4Address(ipAddress))
        {
            return ipAddress;
        }
        else
        {
            // Se o endereço IP não estiver no formato IPv4, retorne uma string vazia ou trate o erro conforme necessário
            return string.Empty;
        }
    }

    private bool IsIPv4Address(string ipAddress)
    {
        // Validação básica do endereço IPv4
        string[] parts = ipAddress.Split('.');
        if (parts.Length == 4)
        {
            foreach (string part in parts)
            {
                if (!byte.TryParse(part, out _))
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}