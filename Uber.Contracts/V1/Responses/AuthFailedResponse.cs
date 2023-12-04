namespace Uber.Contract.V1.Responses;

public class AuthFailedResponse
{
    public IEnumerable<String> Errors { get; set; }
}