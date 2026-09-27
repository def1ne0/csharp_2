using System.Diagnostics;
using lab1.Collections;
using lab1.Entities;

namespace lab1;

internal static class Program
{
    public static void Main()
    {
        // Test Collection
        var list = new MyCustomCollection<int>(1, 2, 3);
        list.Add(4);
        Debug.Assert(list.Current() == 1);
        list.MoveNext();
        Debug.Assert(list.Current() == 2);
        list.Remove(2);
        Debug.Assert(list.Count == 3);
        var rem = list.RemoveCurrent();
        Debug.Assert(rem == 3);
        Debug.Assert(list.Current() == 4);
        Debug.Assert(list.Count == 2);
        list.Reset();
        Debug.Assert(list.Current() == 1);
        
        // Second part
        var station = new RailwayStation();
        var tariff1 = new Tariff("Moscow", 52.2m);
        var tariff2 = new Tariff("Minsk", 42.2m);
        station.AddTariff(tariff1);
        station.AddTariff(tariff2);
        var p1 = new Passenger("Vasya", "Noname", "1234");
        var p2 = new Passenger("Dima", "Anonim", "12345");
        station.RegisterPassengers(p1, p2);
        station.BuyTicket("1234", new Ticket(tariff1, new DateTime(2026, 9, 14)));
        station.BuyTicket("1234", new Ticket(tariff2, new DateTime(2026, 9, 15)));
        station.BuyTicket("12345", new Ticket(tariff2, new DateTime(2026, 9, 13)));
        var cost1 = station.CalculateTotalCost<decimal>("1234");
        Debug.Assert(cost1 == 94.4m);
        var cost2 = station.CalculateTotalCost<decimal>("12345");
        Debug.Assert(cost2 == 42.2m);
        var cost3 = station.CalculateTotalCost<int>("12345");
        Debug.Assert(cost3 == 42);

        string[] destionations = {"Moscow", "Minsk"};

        foreach (var destionation in destionations)
        {
            Console.WriteLine($"Destination: {destionation}");
            foreach (var pass in station.GetPassengersByDestination(destionation))
            {
                Console.WriteLine($"{pass.FirstName}, {pass.LastName} : {pass.PassportNumber}");
            }
        }
    }
}