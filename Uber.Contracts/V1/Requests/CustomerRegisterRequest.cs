namespace Uber.Contract.V1.Requests;

public class CustomerRegisterRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime RegistrationDate { get; set; } 
    public string? State { get; set; }
    public DateTime DateOfBirth { get; set; }
}