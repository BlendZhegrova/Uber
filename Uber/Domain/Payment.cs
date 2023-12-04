using System.ComponentModel.DataAnnotations.Schema;

namespace Uber.Domain;

public class Payment
{
    public Guid Id { get; set; }
    [ForeignKey("TripId")]
    public Guid TripId { get; set; }
    public Trip Trip { get; set; }
    public double Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public int CardId { get; set; }
}