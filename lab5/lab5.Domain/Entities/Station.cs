namespace lab5.Domain.Entities;

public class Station : IEquatable<Station>
{
    public string Name { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public int PlatformCount { get; init; }
    public BaggageDepartment? BaggageDepartment { get; init; }

    public bool Equals(Station? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Name == other.Name &&
               City == other.City &&
               PlatformCount == other.PlatformCount &&
               Equals(BaggageDepartment, other.BaggageDepartment);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Station);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, City, PlatformCount, BaggageDepartment);
    }

    public override string ToString()
    {
        return $"Вокзал: {Name}, город: {City}, платформ: {PlatformCount}; багажное отделение: {BaggageDepartment}";
    }
}