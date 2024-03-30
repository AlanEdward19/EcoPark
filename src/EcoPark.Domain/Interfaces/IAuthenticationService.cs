namespace EcoPark.Domain.Interfaces;

public interface IAuthenticationService
{
    string GenerateJwtToken(string email, string role);
    string ComputeSha256Hash(string password);
}