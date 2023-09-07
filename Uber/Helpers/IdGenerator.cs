using System.Text;

namespace Uber.Helpers;

public class IdGenerator
{
    private static string GenerateRandomCode()
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var randomString = new StringBuilder();
        var random = new Random();

        for (var i = 0; i < 16; i++)
        {
            var index = random.Next(characters.Length);
            randomString.Append(characters[index]);
        }

        var result = randomString.ToString().ToUpper();
        return result;
    }
    
    public string GenerateIdForUser(string state)
    {
        var randomCode = GenerateRandomCode();
        return $"U-{state}-{randomCode}";
    }

    public string GenerateIdForDriver(string state)
    {
        var randomCode = GenerateRandomCode();
        return $"D-{state}-{randomCode}";
    }

    public string GenerateIdForVehicle(string state)
    {
        var randomCode = GenerateRandomCode();
        return $"V-{state}-{randomCode}";
    }

    public string GenerateIdForLicense(string state)
    {
        var randomCode = GenerateRandomCode();
        return $"L-{state}-{randomCode}";
    }
    
    
    
}