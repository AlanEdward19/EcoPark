namespace EcoPark.Domain.ValueObjects;

public class EntityNotFoundValueObject(string message)
{
    public string Message { get; set; } = message;
}