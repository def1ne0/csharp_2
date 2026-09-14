using System.Runtime.Remoting;
using lab1.Collections;

namespace lab1.Entities;

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
}