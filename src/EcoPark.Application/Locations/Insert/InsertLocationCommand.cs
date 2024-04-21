namespace EcoPark.Application.Locations.Insert;

public class InsertLocationCommand(string? name, string? address) : ICommand
{
    public string? Name { get; private set; } = name;
    public string? Address { get; private set; } = address;
}