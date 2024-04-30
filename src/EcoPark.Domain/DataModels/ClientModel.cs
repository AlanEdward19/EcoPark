using EcoPark.Domain.Aggregates.Client;

namespace EcoPark.Domain.DataModels;

public class ClientModel(Guid credentialsId)
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CredentialsId { get; set; } = credentialsId;

    [ForeignKey(nameof(CredentialsId))]
    public virtual CredentialsModel Credentials { get; set; }

    public virtual ICollection<CarModel> Cars { get; set; }

    public virtual ICollection<ReservationModel> Reservations { get; set; }

    public void SetCredentials(CredentialsModel credentials)
    {
        Credentials = credentials;
    }

    public void UpdateBasedOnAggregate(ClientAggregateRoot clientAggregateRoot)
    {
        Credentials.Email = clientAggregateRoot.Email;
        Credentials.Password = clientAggregateRoot.Password;
        Credentials.FirstName = clientAggregateRoot.FirstName;
        Credentials.LastName = clientAggregateRoot.LastName;
    }
}