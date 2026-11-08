using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

using lab5.Domain.Entities;
using lab5.Domain.Interfaces;

namespace SerializerLib.Entities;

public class Serializer : ISerializer
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public void SerializeByLINQ(IEnumerable<Station> xxx, string fileName)
    {
        var document = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("Stations",
                xxx.Select(station =>
                {
                    XElement? baggageElement = null;

                    if (station.BaggageDepartment is not null)
                    {
                        baggageElement = new XElement(
                            "BaggageDepartment",
                            new XElement("Name", station.BaggageDepartment.Name),
                            new XElement("Capacity", station.BaggageDepartment.Capacity),
                            new XElement("HasSecurityCheck", station.BaggageDepartment.HasSecurityCheck),
                            new XElement("Manager", station.BaggageDepartment.Manager)
                        );
                    }

                    return new XElement(
                        "Station",
                        new XElement("Name", station.Name),
                        new XElement("City", station.City),
                        new XElement("PlatformCount", station.PlatformCount),
                        baggageElement
                    );
                })
            )
        );

        document.Save(fileName);
    }

    public IEnumerable<Station> DeSerializeByLINQ(string fileName)
    {
        var document = XDocument.Load(fileName);

        if (document.Root is null)
        {
            return Enumerable.Empty<Station>();
        }

        return document.Root
            .Elements("Station")
            .Select(element =>
            {
                var baggageElement = element.Element("BaggageDepartment");

                return new Station
                {
                    Name = (string?)element.Element("Name") ?? string.Empty,
                    City = (string?)element.Element("City") ?? string.Empty,
                    PlatformCount = (int?)element.Element("PlatformCount") ?? 0,
                    BaggageDepartment = baggageElement is null
                        ? null
                        : new BaggageDepartment
                        {
                            Name = (string?)baggageElement.Element("Name") ?? string.Empty,
                            Capacity = (int?)baggageElement.Element("Capacity") ?? 0,
                            HasSecurityCheck = (bool?)baggageElement.Element("HasSecurityCheck") ?? false,
                            Manager = (string?)baggageElement.Element("Manager") ?? string.Empty
                        }
                };
            })
            .ToList();
    }

    public void SerializeXML(IEnumerable<Station> xxx, string fileName)
    {
        var serializer = new XmlSerializer(typeof(List<Station>));

        using var writer = new StreamWriter(fileName, false, Encoding.UTF8);

        serializer.Serialize(writer, xxx.ToList());
    }

    public IEnumerable<Station> DeSerializeXML(string fileName)
    {
        var serializer = new XmlSerializer(typeof(List<Station>));

        using var reader = new StreamReader(fileName, Encoding.UTF8);

        return (List<Station>?)serializer.Deserialize(reader) ?? new List<Station>();
    }

    public void SerializeJSON(IEnumerable<Station> xxx, string fileName)
    {
        var json = JsonSerializer.Serialize(xxx.ToList(), JsonOptions);

        File.WriteAllText(fileName, json, Encoding.UTF8);
    }

    public IEnumerable<Station> DeSerializeJSON(string fileName)
    {
        var json = File.ReadAllText(fileName, Encoding.UTF8);

        return JsonSerializer.Deserialize<List<Station>>(json, JsonOptions) ?? new List<Station>();
    }
}