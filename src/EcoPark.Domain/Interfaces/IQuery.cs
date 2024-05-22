using EcoPark.Domain.ValueObjects;

namespace EcoPark.Domain.Interfaces;

public interface IQuery
{
    public RequestUserInfoValueObject RequestUserInfo { get; }

    public void SetRequestUserInfo(RequestUserInfoValueObject information);
}