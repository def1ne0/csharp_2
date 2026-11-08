namespace lab4.Entities;

public sealed class Car : Interfaces.INamed
{
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Mileage { get; set; }
    public int Power { get; set; }
    public bool AutoGearBox { get; set; }
    public Car() {}

    public Car(string name, int year, int mileage, int power, bool autoGearBox)
    {
        Name = name;
        Year = year;
        Mileage = mileage;
        Power = power;
        AutoGearBox = autoGearBox;
    }

    public override string ToString()
    {
        return $"{Name,-24} | {Year} г. | пробег {Mileage,7} км | " +
               $"{Power,4} л.с. | КПП: {(AutoGearBox ? "автомат" : "механика")}";
    }
}
