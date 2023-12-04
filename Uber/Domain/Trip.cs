using System.ComponentModel.DataAnnotations.Schema;

namespace Uber.Domain;

public class Trip
{
    public Guid Id { get; set; }
    
    [ForeignKey("DriverId")]
    public Guid DriverId { get; set; }
    public Driver Driver { get; set; }
    [ForeignKey("CustomerId")]
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    
    public double PickupLatitude { get; set; }
    public double PickupLongitude { get; set; }
    public double DropLatitude { get; set; }
    public double DropLongitude { get; set; }
    public DateTime TripStart { get; set; }
    public DateTime TripEnd { get; set; }
    public double Fare { get; set; }
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed
    }
    //1-5
    public int Rating { get; set; }
    public string? Review { get; set; }
    public double Tip { get; set; }
}
