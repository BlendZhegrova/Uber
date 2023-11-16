using System.ComponentModel.DataAnnotations;

namespace Uber.Domain;

public class Driver : User
{
    [Key]
    public string? DriverId { get; set; }
    //Social Security number
    public string? Ssn { get; set; }
    //Driver License Number
    public string? Dln { get; set; }
    public string? LicensePlate { get; set; }
}