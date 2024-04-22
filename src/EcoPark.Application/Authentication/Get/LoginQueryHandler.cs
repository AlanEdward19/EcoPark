using EcoPark.Application.Authentication.Models;

namespace EcoPark.Application.Authentication.Get;

public class LoginQueryHandler(IRepository<UserModel> repository, IAuthenticationService authenticationService) : IHandler<LoginQuery, LoginViewModel?>
{
    public async Task<LoginViewModel?> HandleAsync(LoginQuery command, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(command, cancellationToken);

        if (user == null)
            return null;

        string token = authenticationService.GenerateJwtToken(command.Email,
            command.IsEmployee ? (user as EmployeeModel).UserType.ToString() : "Client");

        return new LoginViewModel(user.Email, token);
    }
}