namespace EcoPark.Application.Punctuations;

public class PunctuationCommand(Guid locationId, Guid clientId, double punctuation) : ICommand
{
    public Guid LocationId { get; private set; } = locationId;
    public Guid ClientId { get; private set; } = clientId;
    public double Punctuation { get; private set; } = punctuation;
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}