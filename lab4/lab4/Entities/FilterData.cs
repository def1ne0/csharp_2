namespace lab4.Entities;

public sealed class FilterData
{
    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }
    public int? MaxMileage { get; set; }
    public int? MinPower { get; set; }
    public bool? AutoGearBox { get; set; }
    
    public static bool IsSet(int? value) => value.HasValue && value.Value != 0;

    public override string ToString()
    {
        var parts = new List<string>();

        if (IsSet(MinYear))     parts.Add($"ГодВыпуска >= {MinYear!.Value}");
        if (IsSet(MaxYear))     parts.Add($"ГодВыпуска <= {MaxYear!.Value}");
        if (IsSet(MaxMileage))  parts.Add($"Пробег <= {MaxMileage!.Value} км");
        if (IsSet(MinPower))    parts.Add($"Мощность >= {MinPower!.Value} л.с.");
        if (AutoGearBox.HasValue)
            parts.Add($"АКПП = {(AutoGearBox.Value ? "да" : "нет")}");

        return parts.Count == 0
            ? "условия не заданы (выбираются все автомобили)"
            : string.Join(" И ", parts);
    }
}