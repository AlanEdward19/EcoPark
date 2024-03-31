using EcoPark.Application.Authentication.Models;

namespace EcoPark.Application.Authentication.Get;

public class LoginQueryHandler
(DatabaseDbContext databaseDbContext,
    IAuthenticationService authenticationService) : IHandler<LoginQuery, LoginViewModel>
{
    public async Task<LoginViewModel> HandleAsync(LoginQuery command, CancellationToken cancellationToken)
    {
        LoginViewModel result;

        string hashedPassword = authenticationService.ComputeSha256Hash(command.Password);

        if (command.IsEmployee)
        {
            EmployeeModel? employeeModel = await databaseDbContext.Employees
                .FirstOrDefaultAsync(e => e.Email == command.Email && e.Password == hashedPassword, cancellationToken);

            if (employeeModel == null)
                throw new Exception("User not found!");

            string token = authenticationService.GenerateJwtToken(command.Email, employeeModel.UserType.ToString());

            result = new LoginViewModel(employeeModel.Email, token);
        }

        else
        {
            ClientModel? clientModel = await databaseDbContext.Clients
                .FirstOrDefaultAsync(c => c.Email == command.Email && c.Password == hashedPassword, cancellationToken);

            if (clientModel == null)
                throw new Exception("User not found!");

            string token = authenticationService.GenerateJwtToken(command.Email, "Client");

            result = new LoginViewModel(clientModel.Email, token);
        }

        return result;
    }
}