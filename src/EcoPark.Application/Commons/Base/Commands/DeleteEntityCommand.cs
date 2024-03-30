namespace EcoPark.Application.Commons.Base.Commands;

public abstract record DeleteEntityCommand
{
    public Guid Id { get; set; }
}