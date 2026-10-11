namespace lab3.Entities;

public sealed class StationEventArgs : EventArgs
{
    public string ActionDescription { get; }
    public DateTime TimeStamp { get; }

    public StationEventArgs(string actionDescription)
    {
        ActionDescription = actionDescription;
        TimeStamp = DateTime.Now;
    }
}