namespace Uber.Helpers.Exceptions.GeneralExceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    { }
}