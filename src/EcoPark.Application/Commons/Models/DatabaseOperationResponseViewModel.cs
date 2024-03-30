namespace EcoPark.Application.Commons.Models;

public class DatabaseOperationResponseViewModel(string operation, EOperationStatus status, string message)
{
    public string Operation { get; set; } = operation;
    public string Status { get; set; } = status.ToString();
    public string Message { get; set; } = message;
}