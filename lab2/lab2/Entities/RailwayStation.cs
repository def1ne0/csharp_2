using lab2.Contracts;
using lab2.Collections;
using System.Numerics;

namespace lab2.Entities;

public sealed class RailwayStation : IRailwayStation
{
    public event EventHandler<StationEventArgs>? ListChanged;
    public event EventHandler<StationEventArgs>? TicketPurchased;
    
    private void OnListChanged(StationEventArgs e)
    {
        ListChanged?.Invoke(this, e);
    }

    private void OnTicketPurchased(StationEventArgs e)
    {
        TicketPurchased?.Invoke(this, e);
    }
    
    private MyCustomCollection<Tariff> _tariffs = new();
    private MyCustomCollection<Passenger> _passengers = new();

    public string Name { get; set; }
    
    public RailwayStation(string name)
    {
        Name = name;
    }

    public RailwayStation()
    {
        Name = "Unknown Railway station";
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
        OnListChanged(new StationEventArgs($"Tariff added: {tariff}"));
    }
    
    public void RegisterPassenger(Passenger passenger)
    {
        _passengers.Add(passenger);
        OnListChanged(new StationEventArgs($"Passenger added: {passenger}"));
    }

    public void RegisterPassengers(params Span<Passenger> passengers)
    {
        foreach (var pass in passengers)
        {
            _passengers.Add(pass);
            OnListChanged(new StationEventArgs($"Passenger added: {pass}"));
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
        OnTicketPurchased(new StationEventArgs($"Passenger {passenger} purchased ticket: {ticket}"));
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
    