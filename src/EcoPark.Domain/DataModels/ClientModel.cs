using EcoPark.Domain.Aggregates.Client;

namespace EcoPark.Domain.DataModels;

public class ClientModel(string email, string password, string firstName, string lastName) : UserModel(email, password, firstName, lastName)
{
    public virtual ICollection<CarModel>? Cars { get; set; }

    public virtual ReservationModel? Reservation { get; set; }

    public void UpdateBasedOnAggregate(ClientAggregateRoot clientAggregateRoot)
    {
        Id = clientAggregateRoot.Id;
        Email = clientAggregateRoot.Email;
        Password = clientAggregateRoot.Password;
        FirstName = clientAggregateRoot.FirstName;
        LastName = clientAggregateRoot.LastName;
    }
}