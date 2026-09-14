using lab1.Contracts;
using lab1.Collections;
using System.Numerics;

namespace lab1.Entities;

public class RailwayStation : IRailwayStation
{
    private MyCustomCollection<Tariff> _tariffs = new();
    private MyCustomCollection<Passenger> _passengers = new();
    
    public RailwayStation(string firstName, string lastName, string passportNumber)
    {
        _passengers.Add(new Passenger(firstName, lastName, passportNumber));
    }

    public RailwayStation()
    {
        _tariffs = new();
        _passengers = new();
    }

    public Passenger FindPassenger(string passport)
    {
        foreach (var pass in _passengers)
        {
            if (pass.PassportNumber == passport)
            {
                return pass;
            }
        }

        throw new InvalidDataException("There is no such passenger");
    }
    
    public void AddTariff(Tariff tariff)
    {
        _tariffs.Add(tariff);
    }
    
    public void RegisterPassenger(Passenger passenger)
    {
        _passengers.Add(passenger);
    }

    public void RegisterPassengers(params Span<Passenger> passengers)
    {
        foreach (var pass in passengers)
        {
            _passengers.Add(pass);
        }
    }
    
    public static void BuyTicket(Passenger passenger, Ticket ticket)
    {
        passenger.Tickets.Add(ticket);
    }

    public void BuyTicket(string passport, Ticket ticket)
    {
        var passenger = FindPassenger(passport);
        passenger.Tickets.Add(ticket);
    }
    
    
    public static AmountT CalculateTotalCost<AmountT>(Passenger passenger) where AmountT : INumber<AmountT>
    {
        var total = AmountT.Zero;
        
        foreach (var ticket in passenger.Tickets)
        {
            var price = ticket.GetPrice();
            total += AmountT.CreateChecked(price);
        }

        return total;
    }
    
    public AmountT CalculateTotalCost<AmountT>(string passport) where AmountT : INumber<AmountT>
    {
        var passenger = FindPassenger(passport);
        return CalculateTotalCost<AmountT>(passenger);
    }
    
    public MyCustomCollection<Passenger> GetPassengersByDestination(string destination)
    {
        var result = new MyCustomCollection<Passenger>();

        foreach (var passenger in _passengers)
        {
            foreach (var ticket in passenger.Tickets)
            {
                if (ticket.Tariff.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(passenger);
                    break; 
                }
            }
        }

        return result;
    }
}
    