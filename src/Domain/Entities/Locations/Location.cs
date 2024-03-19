using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Locations;

public class Location
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}