namespace lab5.Domain.Entities;

public class BaggageDepartment : IEquatable<BaggageDepartment>
{
    public string Name { get; init; } = string.Empty;
    public int Capacity { get; init; }
    public bool HasSecurityCheck { get; init; }
    public string Manager { get; init; } = string.Empty;

    public bool Equals(BaggageDepartment? other)
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
               Capacity == other.Capacity &&
               HasSecurityCheck == other.HasSecurityCheck &&
               Manager == other.Manager;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as BaggageDepartment);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Capacity, HasSecurityCheck, Manager);
    }

    public override string ToString()
    {
        return $"{Name}, вместимость: {Capacity}, досмотр: {(HasSecurityCheck ? "есть" : "нет")}, ответственный: {Manager}";
    }
}