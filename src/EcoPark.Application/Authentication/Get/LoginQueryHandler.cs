using EcoPark.Application.Authentication.Models;

namespace EcoPark.Application.Authentication.Get;

public class LoginQueryHandler(IRepository<CredentialsModel> repository, IAuthenticationService authenticationService) : IHandler<LoginQuery, LoginViewModel?>
{
    public async Task<LoginViewModel?> HandleAsync(LoginQuery command, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(command, cancellationToken);

        if (user == null)
            return null;

        string token = authenticationService.GenerateJwtToken(command.Email, user.UserType);

        return new LoginViewModel(user.Email, token);
    }
}