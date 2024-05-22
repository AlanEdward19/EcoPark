using System.Text.RegularExpressions;

namespace EcoPark.Application.Utils;

public static class ValidatorUtils
{
    public static bool ValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        var regex = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$");

        return regex.IsMatch(password);
    }

    public static bool ValidateLicensePlate(string licensePlate)
    {
        var regex = new Regex(@"^[A-Z]{3}-\d{4}$|^[A-Z]{3}-\d[A-Z]\d{2}$");
        return regex.IsMatch(licensePlate);
    }

    public static bool IsValidTypeAndFuelCombination(ECarType carType, EFuelType fuelType) => carType switch
    {
        ECarType.Electric => fuelType == EFuelType.Electric,
        ECarType.Hybrid => fuelType == EFuelType.Electric || fuelType == EFuelType.Gasoline ||
                           fuelType == EFuelType.Alcohol,
        ECarType.Combustion or ECarType.Pcd => fuelType == EFuelType.Gasoline || fuelType == EFuelType.Alcohol,
        _ => false,
    };

}