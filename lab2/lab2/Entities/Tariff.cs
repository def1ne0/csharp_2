namespace lab2.Entities;

public class Tariff : IEquatable<Tariff>
{
    public string Destination { get; set; } 
    public decimal Price { get; set; }    

    public Tariff(string destination, decimal price)
    {
        Destination = destination;
        Price = price;
    }
    
    public bool Equals(Tariff? other)
    {
        if (other == null) return false;
        return Destination == other.Destination && Price == other.Price;
    }

    public override string ToString()
    {
        return $"[Destination: {Destination}, Price: {Price}]";
    }
}