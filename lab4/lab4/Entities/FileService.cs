namespace lab4.Entities;
using Interfaces;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public sealed class FileService : IFileService<Car>
{
    public IEnumerable<Car> ReadFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Не задано имя файла.", nameof(fileName));
        
        using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        using (var reader = new BinaryReader(fs, Encoding.UTF8))
        {
            while (true)
            {
                Car car;
                
                try
                {
                    if (reader.PeekChar() == -1)
                        break;

                    car = ReadCar(reader);
                }
                catch (EndOfStreamException ex)
                {
                    Console.WriteLine($"[ReadFile] Неожиданный конец файла " +
                                      $"'{Path.GetFileName(fileName)}': {ex.Message}");
                    yield break;
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"[ReadFile] Ошибка ввода-вывода при чтении " +
                                      $"'{Path.GetFileName(fileName)}': {ex.Message}");
                    yield break;
                }
                catch (ObjectDisposedException ex)
                {
                    Console.WriteLine($"[ReadFile] Поток уже закрыт: {ex.Message}");
                    yield break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"[ReadFile] Некорректные данные в файле " +
                                      $"'{Path.GetFileName(fileName)}': {ex.Message}");
                    yield break;
                }

                yield return car;
            }
        }
    }
    
    public void SaveData(IEnumerable<Car> data, string fileName)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Не задано имя файла.", nameof(fileName));
        
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
            Console.WriteLine($"Файл '{Path.GetFileName(fileName)}' уже существовал и был удалён.");
        }

        try
        {
            using (var fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
            using (var writer = new BinaryWriter(fs, Encoding.UTF8))
            {
                foreach (Car car in data)
                    WriteCar(writer, car);

                writer.Flush();
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"[SaveData] Ошибка ввода-вывода при записи " +
                              $"'{Path.GetFileName(fileName)}': {ex.Message}");
        }
        catch (ObjectDisposedException ex)
        {
            Console.WriteLine($"[SaveData] Поток уже закрыт: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"[SaveData] Некорректные данные: {ex.Message}");
        }
    }

    private static void WriteCar(BinaryWriter writer, Car car)
    {
        writer.Write(car.Name ?? string.Empty);
        writer.Write(car.Year);
        writer.Write(car.Mileage);
        writer.Write(car.Power);
        writer.Write(car.AutoGearBox);
    }

    private static Car ReadCar(BinaryReader reader)
    {
        Car car = new Car();
        car.Name        = reader.ReadString();
        car.Year        = reader.ReadInt32();
        car.Mileage     = reader.ReadInt32();
        car.Power       = reader.ReadInt32();
        car.AutoGearBox = reader.ReadBoolean();
        return car;
    }
}
    