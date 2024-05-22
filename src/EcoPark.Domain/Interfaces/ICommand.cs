using EcoPark.Domain.ValueObjects;

namespace EcoPark.Domain.Interfaces;

public interface ICommand
{
    public RequestUserInfoValueObject? RequestUserInfo { get;}

    public void SetRequestUserInfo(RequestUserInfoValueObject information);
}