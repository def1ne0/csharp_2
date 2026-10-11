using System.Numerics;

namespace lab3.Entities;
using Contracts;

public sealed class RailwayStation : IRailwayStation
{
    public event EventHandler<StationEventArgs>? TariffChangedEvent;
    public event EventHandler<StationEventArgs>? TicketPurchasedEvent;
    
    private void OnTariffChanged(StationEventArgs e)
    {
        TariffChangedEvent?.Invoke(this, e);
    }

    private void OnTicketPurchased(StationEventArgs e)
    {
        TicketPurchasedEvent?.Invoke(this, e);
    }
    
    private Dictionary<string, Tariff> _tariffs = new();
    private List<Passenger> _passengers = new();

    public string Name { get; set; }
    
    public RailwayStation(string name)
    {
        Name = name;
        _tariffs = new();
        _passengers = new();
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

    public void SetTariff(string name, Tariff tariff)
    {
        if (_tariffs.TryGetValue(name, out var old))
        {
            _tariffs[name] = tariff;
            OnTariffChanged(new StationEventArgs($"Tariff with name: {name} changed: {tariff}"));
            
        }
        else
        {
            _tariffs[name] = tariff;
            OnTariffChanged(new StationEventArgs($"Tariff with name: {name} added: {tariff}"));

        }
    }
    
    public void RegisterPassenger(Passenger passenger)
    {
        _passengers.Add(passenger);
        OnTariffChanged(new StationEventArgs($"Passenger added: {passenger}"));
    }

    public void RegisterPassengers(params Span<Passenger> passengers)
    {
        foreach (var pass in passengers)
        {
            _passengers.Add(pass);
            OnTariffChanged(new StationEventArgs($"Passenger added: {pass}"));
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
    
    public List<Passenger> GetPassengersByDestination(string destination)
    {
        var result = new List<Passenger>();

        foreach (var passenger in _passengers)
        {
            foreach (var ticket in passenger.Tickets)
            {
                if (ticket.Tariff.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(passenger);
                }
            }
        }

        return result;
    }
    
    

    public List<string> GetTariffNamesSorted()
    {
        return _tariffs
            .OrderBy(kv => kv.Value.Price)
            .Select(kv => kv.Key)
            .ToList();
    }

    public decimal GetTotalRevenue()
    {
        return _passengers
            .SelectMany(p => p.Tickets)
            .Sum(t => t.Tariff.Price);
    }

    public decimal GetPassengerTotal(Passenger p)
    {
        return p.Tickets
            .Sum(t => t.Tariff.Price);
    }

    public string GetPassengerNameWithMaxCost()
    {
        if (_passengers.Count == 0)
        {
            throw new InvalidOperationException("Passenger list is empty");
        }

        return _passengers
            .Aggregate((best, p) => GetPassengerTotal(p) > GetPassengerTotal(best) ? p : best)
            .FirstName;
    }

    public int CountPassengersOver(decimal target)
    {
        return _passengers
            .Aggregate(
                 0,
                (count, p) => count + (GetPassengerTotal(p) > target ? 1 : 0)
            );
    }

    public List<(string destionation, decimal total)> GetCostByDestination(Passenger p)
    {
        return p.Tickets
            .GroupBy(t => t.Tariff.Destination)
            .Select(g => (g.Key, g.Sum(t => t.Tariff.Price)))
            .ToList();
    }
    
        
}
    