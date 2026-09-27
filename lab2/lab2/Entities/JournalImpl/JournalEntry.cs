namespace lab2.Entities.JournalImpl;

public class JournalEntry
{
    public string EntityName { get; }
    public string Description { get; }
    public DateTime Time { get; }

    public JournalEntry(string entityName, string description, DateTime time)
    {
        EntityName = entityName;
        Description = description;
        Time = time;
    }
}
