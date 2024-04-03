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
}