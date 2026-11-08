using System.Text;

namespace lab4;

using Entities;

internal static class Program
{
    private const string Surname = "Finevich";
    private static readonly string[] Extensions = { ".txt", ".rtf", ".dat", ".inf" };

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var random = new Random();

        try
        {
            var folderPath = PrepareFolder(Surname + "_Lab4");
            
            CreateEmptyFiles(folderPath, 10, random);
            PrintFolderContent(folderPath);
            
            var cars = GenerateCars(50, random);
            
            var fileService = new FileService();
            var sourceFile = Path.Combine(folderPath, "cars.dat");

            Console.WriteLine("\n=== Запись коллекции в бинарный файл ===");
            fileService.SaveData(cars, sourceFile);
            Console.WriteLine($"Записано объектов: {cars.Count}. " +
                              $"Размер файла: {new FileInfo(sourceFile).Length} байт.");
            
            fileService.SaveData(cars, sourceFile);
            
            var renamedFile = Path.Combine(folderPath, "cars_renamed.dat");
            
            if (File.Exists(renamedFile))
            {
                File.Delete(renamedFile);
            }
            
            File.Move(sourceFile, renamedFile);
            Console.WriteLine($"Файл переименован: '{Path.GetFileName(sourceFile)}' -> " +
                              $"'{Path.GetFileName(renamedFile)}'");
            
            var loadedCars = new List<Car>();
            loadedCars.AddRange(fileService.ReadFile(renamedFile));
            Console.WriteLine($"Прочитано объектов из файла: {loadedCars.Count}");
            
            var sortedByName = loadedCars
                .OrderBy(car => car, new MyCustomComparer<Car>())
                .ToList();
            
            PrintCars("ИСХОДНАЯ КОЛЛЕКЦИЯ (создана в памяти, 50 объектов)", cars);
            PrintCars("КОЛЛЕКЦИЯ ИЗ ФАЙЛА, ОТСОРТИРОВАННАЯ ПО Name (MyCustomComparer<Car>)",
                      sortedByName);
            
            var sortedByPower = loadedCars
                .OrderBy(car => car,
                         Comparer<Car>.Create((a, b) => b.Power.CompareTo(a.Power))) 
                .ThenBy(car => car.Mileage)
                .ToList();

            PrintCars("СОРТИРОВКА ПО МОЩНОСТИ (убыв.), КОМПАРАТОР — ЛЯМБДА-ВЫРАЖЕНИЕ",
                      sortedByPower);
            
            var filters = new List<FilterData>
            {
                new FilterData { MinPower = 250, AutoGearBox = true },
                new FilterData { MinYear = 2018, MaxMileage = 60000 },
                new FilterData { MaxYear = 2005, AutoGearBox = false },
                new FilterData { MinYear = 2010, MaxYear = 2020,
                                 MinPower = 150, MaxMileage = 150000, AutoGearBox = true },
                new FilterData { MinYear = 0, MaxMileage = null }
            };

            Console.WriteLine("\n================ ФИЛЬТРАЦИЯ КОЛЛЕКЦИИ (QueryBuilder) ================");
            var n = 1;
            
            foreach (var filter in filters)
            {
                var predicate = QueryBuilder.Build(filter);

                Console.WriteLine($"\n--- Фильтр {n++} ---");
                Console.WriteLine($"Условия      : {filter}");
                Console.WriteLine($"Выражение    : {predicate}");
                
                var result = loadedCars
                    .AsQueryable()
                    .Where(predicate)
                    .ToList();

                PrintCars($"РЕЗУЛЬТАТ ФИЛЬТРА {n - 1}", result);
            }
            
            Console.WriteLine("\n=== Демонстрация обработки исключений ===");
            var badFile = Path.Combine(folderPath, "no_such_file.dat");
            
            try
            {
                _ = fileService.ReadFile(badFile).ToList();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Ожидаемое исключение: {ex.GetType().Name}: {ex.Message}");
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ошибка ввода-вывода: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Нет доступа к файлу/папке: {ex.Message}");
        }
    }
    
    private static string PrepareFolder(string folderName)
    {
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        if (Directory.Exists(folderPath))
        {
            var dir = new DirectoryInfo(folderPath);

            foreach (var file in dir.GetFiles())
                file.Delete();

            foreach (var sub in dir.GetDirectories())
                sub.Delete(true);

            Console.WriteLine($"Папка '{folderName}' уже существует — " +
                              $"всё её содержимое удалено ({folderPath}).");
        }
        else
        {
            Directory.CreateDirectory(folderPath);
            Console.WriteLine($"Создана папка '{folderName}': {folderPath}");
        }

        return folderPath;
    }
    
    private static void CreateEmptyFiles(string folderPath, int count, Random random)
    {
        Console.WriteLine($"\n=== Создание {count} пустых файлов ===");

        for (int i = 0; i < count; i++)
        {
            string ext = Extensions[random.Next(Extensions.Length)];
            string fileName;
            string fullPath;
            
            do
            {
                fileName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
                fullPath = Path.Combine(folderPath, fileName);
            } while (File.Exists(fullPath));

            using (File.Create(fullPath)) {}

            Console.WriteLine($"   создан пустой файл: {fileName}");
        }
    }

    private static void PrintFolderContent(string folderPath)
    {
        Console.WriteLine("\n=== Содержимое папки ===");

        foreach (string file in Directory.GetFiles(folderPath))
        {
            Console.WriteLine($"Файл: {Path.GetFileName(file)} " +
                              $"имеет расширение {Path.GetExtension(file)}");
        }
    }
    
    private static List<Car> GenerateCars(int count, Random random)
    {
        string[][] catalog =
        {
            new[] { "Toyota", "Camry" },      new[] { "Toyota", "Corolla" },
            new[] { "BMW", "X5" },            new[] { "BMW", "320i" },
            new[] { "Audi", "A4" },           new[] { "Audi", "Q7" },
            new[] { "Ford", "Focus" },        new[] { "Ford", "Mondeo" },
            new[] { "Kia", "Rio" },           new[] { "Kia", "Sportage" },
            new[] { "Hyundai", "Solaris" },   new[] { "Hyundai", "Tucson" },
            new[] { "Skoda", "Octavia" },     new[] { "Volkswagen", "Passat" },
            new[] { "Mazda", "CX-5" },        new[] { "Lada", "Vesta" },
            new[] { "Renault", "Duster" },    new[] { "Nissan", "Qashqai" },
            new[] { "Honda", "Civic" },       new[] { "Mercedes", "E200" }
        };

        var cars = new List<Car>(count);

        for (var i = 0; i < count; i++)
        {
            var item = catalog[random.Next(catalog.Length)];

            cars.Add(new Car(
                name: $"{item[0]} {item[1]}",
                year: random.Next(1995, 2027),
                mileage: random.Next(0, 351) * 1000,
                power: random.Next(60, 501),
                autoGearBox: random.Next(2) == 1));
        }

        return cars;
    }

    private static void PrintCars(string title, IEnumerable<Car> cars)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 100));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 100));
        Console.WriteLine($"{"№",4}  {"Автомобиль",-24} {"Год",6} {"Пробег,км",12} {"Мощн.,л.с.",12} {"КПП",-10}");
        Console.WriteLine(new string('-', 100));

        var i = 1;
        
        foreach (var car in cars)
        {
            Console.WriteLine($"{i++,4}  {car.Name,-24} {car.Year,6} {car.Mileage,12} " +
                              $"{car.Power,12} {(car.AutoGearBox ? "автомат" : "механика"),-10}");
        }

        Console.WriteLine(new string('-', 100));
        Console.WriteLine($"Всего объектов: {i - 1}");
    }
}
