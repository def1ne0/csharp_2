namespace lab2.Collections.Exceptions;

public class TargetNotFoundException : Exception
{
    public TargetNotFoundException() : base() {}
    public TargetNotFoundException(string msg) : base(msg) {}
    public TargetNotFoundException(string msg, Exception innerException) : base(msg, innerException) {}
}