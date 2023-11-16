namespace Uber.Helpers.Exceptions.GeneralExceptions;

public class UnAuthorizedAccessException : Exception
{
    public UnAuthorizedAccessException(string message) : base(message)
    {
        
    }
}