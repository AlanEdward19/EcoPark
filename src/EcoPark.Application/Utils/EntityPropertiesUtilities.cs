using System.Security.Claims;

namespace EcoPark.Application.Utils;

public static class EntityPropertiesUtilities
{
    public static IEnumerable<string> GetEntityPropertiesAndValueAsIEnumerable<T>(T entity) =>
        entity.GetType().GetProperties().Select(x => $"{x.Name} = {x.GetValue(entity)}");

    public static (string Email, string UserType) GetUserInfo(ClaimsPrincipal user)
    {
        var userEmail = user.FindFirst("userName")?.Value;
        var userType = user.Claims
            .FirstOrDefault(c => c.Type.Contains("role", StringComparison.InvariantCultureIgnoreCase))?.Value;

        return (userEmail, userType);
    }
}