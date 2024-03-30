namespace EcoPark.Domain.Commons.Base;

public class BaseDataModel
{
    [Key]
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public BaseDataModel()
    {
        DateTime now = DateTime.Now;
        Id = Guid.NewGuid();

        CreatedAt = now;
        UpdatedAt = now;
    }
}