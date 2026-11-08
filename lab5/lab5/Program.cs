using System.Text;
using Microsoft.Extensions.Configuration;
using lab5.Domain.Entities;
using lab5.Domain.Interfaces;
using SerializerLib.Entities;

namespace lab5;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var rawFileName = configuration.GetSection("FileName").Value;

        if (string.IsNullOrWhiteSpace(rawFileName))
        {
            throw new InvalidOperationException("В appsettings.json не найден параметр FileName.");
        }

        var baseFileName = Path.GetFileNameWithoutExtension(rawFileName.Trim());

        if (string.IsNullOrWhiteSpace(baseFileName))
        {
            throw new InvalidOperationException("Имя файла в appsettings.json указано некорректно.");
        }

        var linqFileName = Path.Combine(AppContext.BaseDirectory, $"{baseFileName}.linq.xml");
        var xmlFileName = Path.Combine(AppContext.BaseDirectory, $"{baseFileName}.xml");
        var jsonFileName = Path.Combine(AppContext.BaseDirectory, $"{baseFileName}.json");

        var stations = CreateStations();

        var serializer = new Serializer();

        Console.WriteLine("Выполняется сериализация...");
        Console.WriteLine();

        serializer.SerializeByLINQ(stations, linqFileName);
        serializer.SerializeXML(stations, xmlFileName);
        serializer.SerializeJSON(stations, jsonFileName);

        Console.WriteLine("Файлы созданы:");
        Console.WriteLine(linqFileName);
        Console.WriteLine(xmlFileName);
        Console.WriteLine(jsonFileName);
        Console.WriteLine();

        PrintFile(linqFileName);
        PrintFile(xmlFileName);
        PrintFile(jsonFileName);

        Console.WriteLine("Исходная коллекция:");
        PrintStations(stations);

        Console.WriteLine("Проверка десериализации:");

        CheckCollection("LINQ-to-XML", stations, serializer.DeSerializeByLINQ(linqFileName));
        CheckCollection("XmlSerializer", stations, serializer.DeSerializeXML(xmlFileName));
        CheckCollection("JSON", stations, serializer.DeSerializeJSON(jsonFileName));
    }

    private static List<Station> CreateStations()
    {
        return new List<Station>
        {
            new()
            {
                Name = "Казанский вокзал",
                City = "Москва",
                PlatformCount = 12,
                BaggageDepartment = new BaggageDepartment
                {
                    Name = "Багажное отделение № 1",
                    Capacity = 500,
                    HasSecurityCheck = true,
                    Manager = "Иванов Иван Иванович"
                }
            },
            new()
            {
                Name = "Ленинградский вокзал",
                City = "Москва",
                PlatformCount = 10,
                BaggageDepartment = new BaggageDepartment
                {
                    Name = "Багажное отделение № 2",
                    Capacity = 450,
                    HasSecurityCheck = true,
                    Manager = "Петров Петр Петрович"
                }
            },
            new()
            {
                Name = "Ярославский вокзал",
                City = "Москва",
                PlatformCount = 8,
                BaggageDepartment = new BaggageDepartment
                {
                    Name = "Багажное отделение № 3",
                    Capacity = 400,
                    HasSecurityCheck = false,
                    Manager = "Сидоров Сидор Сидорович"
                }
            },
            new()
            {
                Name = "Московский вокзал",
                City = "Санкт-Петербург",
                PlatformCount = 9,
                BaggageDepartment = new BaggageDepartment
                {
                    Name = "Багажное отделение Московское",
                    Capacity = 600,
                    HasSecurityCheck = true,
                    Manager = "Смирнов Алексей Сергеевич"
                }
            },
            new()
            {
                Name = "Финляндский вокзал",
                City = "Санкт-Петербург",
                PlatformCount = 7,
                BaggageDepartment = new BaggageDepartment
                {
                    Name = "Багажное отделение Финляндское",
                    Capacity = 350,
                    HasSecurityCheck = false,
                    Manager = "Кузнецов Олег Дмитриевич"
                }
            },
            new()
            {
                Name = "Нижегородский вокзал",
                City = "Нижний Новгород",
                PlatformCount = 6,
                BaggageDepartment = new BaggageDepartment
                {
                    Name = "Багажное отделение Нижегородское",
                    Capacity = 300,
                    HasSecurityCheck = true,
                    Manager = "Волкова Анна Павловна"
                }
            }
        };
    }

    private static void PrintStations(IEnumerable<Station> stations)
    {
        foreach (var station in stations)
        {
            Console.WriteLine(station);
        }

        Console.WriteLine();
    }

    private static void PrintFile(string fileName)
    {
        Console.WriteLine($"Содержимое файла {Path.GetFileName(fileName)}:");
        Console.WriteLine(File.ReadAllText(fileName));
        Console.WriteLine(new string('-', 80));
    }

    private static void CheckCollection(string methodName, IEnumerable<Station> expected, IEnumerable<Station> actual)
    {
        bool isSame = expected.SequenceEqual(actual);

        Console.WriteLine($"{methodName}: {(isSame ? "данные совпадают" : "данные НЕ совпадают")}");
    }
}