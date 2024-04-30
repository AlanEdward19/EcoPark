namespace EcoPark.Domain.Interfaces.Services;

public interface IAuthenticationService
{
    string GenerateJwtToken(string email, EUserType role);
    string ComputeSha256Hash(string password);
}