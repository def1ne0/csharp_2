namespace lab3.Entities;

public class Passenger(string firstName, string lastName, string passportNumber) : IEquatable<Passenger>
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string PassportNumber { get; set; } = passportNumber;
    public List<Ticket> Tickets = new();

    void BuyTicket(Ticket ticket)
    {
        Tickets.Add(ticket);
    }

    public bool Equals(Passenger? other)
    {
        if (other == null)
        {
            return false;
        }

        return FirstName == other.FirstName && LastName == other.LastName && PassportNumber == other.PassportNumber;
    }

    public override string ToString()
    {
        return $"[{FirstName}, {LastName}, {PassportNumber}]";
    }
}