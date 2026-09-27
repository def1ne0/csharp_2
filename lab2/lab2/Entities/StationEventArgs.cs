namespace lab2.Entities;

public class StationEventArgs : EventArgs
{
    public string ActionDescription { get; }
    public DateTime TimeStamp { get; }

    public StationEventArgs(string actionDescription)
    {
        ActionDescription = actionDescription;
        TimeStamp = DateTime.Now;
    }
}