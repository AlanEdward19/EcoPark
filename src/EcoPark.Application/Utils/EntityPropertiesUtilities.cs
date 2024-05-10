using System.Security.Claims;

namespace EcoPark.Application.Utils;

public static class EntityPropertiesUtilities
{
    public static IEnumerable<string> GetEntityPropertiesAndValueAsIEnumerable<T>(T entity) =>
        entity.GetType().GetProperties().Select(x => $"{x.Name} = {x.GetValue(entity)}");

    public static (string Email, EUserType UserType) GetUserInfo(ClaimsPrincipal user)
    {
        var userEmail = user.FindFirst("userName")?.Value;
        var userType = user.FindFirst("role")?.Value;

        EUserType parsedUserType = Enum.Parse<EUserType>(userType, true);

        return (userEmail, parsedUserType);
    }
}