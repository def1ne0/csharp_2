namespace lab2.Entities;

public class Ticket : IEquatable<Ticket>
{
    public Tariff Tariff { get; set; }
    public DateTime TravelDate { get; set; }

    public Ticket(Tariff tariff, DateTime travelDate)
    {
        Tariff = tariff;
        TravelDate = travelDate;
    }

    public decimal GetPrice() => Tariff.Price;

    public bool Equals(Ticket? other)
    {
        if (other == null) return false;
        return Tariff.Equals(other.Tariff) && TravelDate == other.TravelDate;
    }
}