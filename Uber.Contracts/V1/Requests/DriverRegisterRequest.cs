namespace Uber.Contract.V1.Requests;

public class DriverRegisterRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string State { get; set; }
    public string StateCode { get; set; }
    //Social Security number
    public string? Ssn { get; set; }

    //Driver License Number
    public string? Dln { get; set; }
    public string? LicensePlate { get; set; }
    public bool Status { get; set; }
}