namespace EcoPark.Application.Utils;

public static class EntityPropertiesUtilities
{
    public static IEnumerable<string> GetEntityPropertiesAndValueAsIEnumerable<T>(T entity) =>
        entity.GetType().GetProperties().Select(x => $"{x.Name} = {x.GetValue(entity)}");
}