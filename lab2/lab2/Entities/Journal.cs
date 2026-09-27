namespace lab2.Entities;
using JournalImpl;

public class Journal
{
    private readonly List<JournalEntry> _entries = new();
    
    public void LogEvent(object? sender, StationEventArgs e)
    {
        var entityName = sender is RailwayStation station ? station.Name : "Unknown Entity";
            
        _entries.Add(new JournalEntry(entityName, e.ActionDescription, e.TimeStamp));
    }
    
    public void PrintAllLogs()
    {
        Console.WriteLine("\n--- RAILWAY STATION LOGS ---");
        if (_entries.Count == 0)
        {
            Console.WriteLine("Journal is empty");
            return;
        }

        foreach (var entry in _entries)
        {
            Console.WriteLine($"[{entry.Time:HH:mm:ss}] Entity: {entry.EntityName} | Action: {entry.Description}");
        }
        Console.WriteLine("-------------------------------\n");
    }
}