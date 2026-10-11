namespace lab3;
using Entities;

internal static class Program
{
    public static void Main()
    {
        var station = new RailwayStation("Sigma");
        var journal = new Journal();
        station.TariffChangedEvent += journal.LogEvent;

        station.TicketPurchasedEvent += (sender, e) =>
        {
            var entityName = sender is RailwayStation ? station.Name : "unknown";
            Console.WriteLine($"[INFO] Entity: {entityName}, Action: {e.ActionDescription}");
        };

        var tariff1 = new Tariff("Moscow", 52.2m);
        var tariff2 = new Tariff("Minsk", 42.2m);
        station.SetTariff("T1", tariff1);
        station.SetTariff("T2", tariff2);
        var p1 = new Passenger("Anonim1", "1", "123");
        var p2 = new Passenger("Anonim2", "2", "1234");
        station.RegisterPassengers(
            p1,
            p2
        );
        
        station.BuyTicket("123", new Ticket(tariff1, new DateTime(2026, 9, 27)));
        station.BuyTicket("1234", new Ticket(tariff2, new DateTime(2026, 9, 28)));
        station.BuyTicket("123", new Ticket(tariff2, new DateTime(2026, 9, 28)));
        journal.PrintAllLogs();
        
        Console.WriteLine("--- Passengers by destination: Minsk ---");
        
        foreach (var p in station.GetPassengersByDestination("Minsk"))
        {
            Console.WriteLine($"{p}");
        }
        
        Console.WriteLine("--- Tariff Names Sorted ---");
        
        foreach (var name in station.GetTariffNamesSorted())
        {
            Console.WriteLine($"Tariff name: {name}");
        }
        
        Console.WriteLine($"$Total revenue: {station.GetTotalRevenue()}");
        Console.WriteLine($"Passenger {p1} total cost: {station.GetPassengerTotal(p1)}");
        Console.WriteLine($"Name of passenger with max sum: {station.GetPassengerNameWithMaxCost()}");
        Console.WriteLine($"Count of passengers, that spend more of 40 dollars: {station.CountPassengersOver(40)}");
        Console.WriteLine($"--- List of sums with destination of {p1} ---");

        foreach (var (dest, sum) in station.GetCostByDestination(p1))
        {
            Console.WriteLine($"destination: {dest}, sum: {sum}");
        }

    }
}