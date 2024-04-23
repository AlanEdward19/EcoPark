﻿namespace EcoPark.Application.Cars.Delete;

public record DeleteCarCommand : DeleteEntityCommand, ICommand
{
    [JsonIgnore]
    public (string Email, string UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}