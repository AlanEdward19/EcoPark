namespace EcoPark.Domain.DataModels;

public class ClientModel(string email, string password, string firstName, string lastName) : UserModel(email, password, firstName, lastName)
{
    public virtual ICollection<CarModel>? Cars { get; set; }
}