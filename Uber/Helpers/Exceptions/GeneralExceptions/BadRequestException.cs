namespace Uber.Helpers.Exceptions.GeneralExceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
        
    }
}