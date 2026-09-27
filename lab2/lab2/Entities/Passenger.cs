using System.Runtime.Remoting;
using lab2.Collections;

namespace lab2.Entities;

public class Passenger(string firstName, string lastName, string passportNumber) : IEquatable<Passenger>
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string PassportNumber { get; set; } = passportNumber;
    public MyCustomCollection<Ticket> Tickets = new();

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
        return $"[{firstName}, {lastName}, {passportNumber}]";
    }
}