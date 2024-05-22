namespace EcoPark.Application.Commons.Models;

public class DatabaseOperationResponseViewModel(EOperationStatus status, string message)
{
    public string Status { get; set; } = status.ToString();
    public string Message { get; set; } = message;
}