﻿namespace EcoPark.Application.Locations.Delete;

public record DeleteLocationCommand : DeleteEntityCommand, ICommand 
{
    public (string Email, string UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}