using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Uber.Domain;

public class Driver : User
{
    //Social Security number
    public string? Ssn { get; set; }
    //Driver License Number
    public string? Dln { get; set; }
    public string? LicensePlate { get; set; }
    public bool Status { get; set; }
}