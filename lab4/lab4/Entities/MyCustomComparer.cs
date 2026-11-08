using lab4.Interfaces;

namespace lab4.Entities;

public class MyCustomComparer<T> : IComparer<T>
    where T : INamed
{
    private readonly StringComparison _comparison;

    public MyCustomComparer(StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        _comparison = comparison;
    }

    public int Compare(T? x, T? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        return string.Compare(x.Name, y.Name, _comparison);
    }
}