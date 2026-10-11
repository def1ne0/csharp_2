using System.Diagnostics;

namespace lab2;

using Collections;
using Collections.Exceptions;
using Entities;

internal static class Program
{
    public static void Main()
    {
        /*
         *  Task 1
         */
        var list = new MyCustomCollection<int>(1, 2);
        list.Remove(1);
        Debug.Assert(list.Count == 1);
        Debug.Assert(list[0] == 2);

        try
        {
            _ = list[1];
        }
        catch (Exception exception)
        {
            if (exception is IndexOutOfRangeException)
            {
                Console.WriteLine("Index out of range");
            }
        }
        
        try
        {
            list.Remove(0);
        }
        catch (Exception exception)
        {
            if (exception is TargetNotFoundException)
            {
                Console.WriteLine("TargetNotFoundException");
            }
        }
        
        /*
         * Task 2
         */

        var station = new RailwayStation("Sigma");
        var journal = new Journal();
        station.ListChangedEvent += journal.LogEvent;

        station.TicketPurchasedEvent += (sender, e) =>
        {
            var entityName = sender is RailwayStation ? station.Name : "unknown";
            Console.WriteLine($"[INFO] Entity: {entityName}, Action: {e.ActionDescription}");
        };

        var tariff1 = new Tariff("Moscow", 52.2m);
        var tariff2 = new Tariff("Minsk", 42.2m);
        station.AddTariff(tariff1);
        station.AddTariff(tariff2);
        station.RegisterPassengers(
            new Passenger("Anonim", "1", "123"),
            new Passenger("Anonim", "2", "1234")
        );
        
        station.BuyTicket("123", new Ticket(tariff1, new DateTime(2026, 9, 27)));
        station.BuyTicket("1234", new Ticket(tariff2, new DateTime(2026, 9, 28)));
        journal.PrintAllLogs();
    }
}