namespace EcoPark.Application.Locations.Insert;

public class InsertLocationCommand(string name, string address)
{
    public string Name { get; private set; } = name;
    public string Address { get; private set; } = address;
}