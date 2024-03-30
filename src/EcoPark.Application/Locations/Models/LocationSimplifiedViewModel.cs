namespace EcoPark.Application.Locations.Models;

public class LocationSimplifiedViewModel(string name, string address)
{
    public string Name { get; private set; } = name;
    public string Address { get; private set; } = address;
}