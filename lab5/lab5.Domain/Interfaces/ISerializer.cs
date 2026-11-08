using lab5.Domain.Entities;

namespace lab5.Domain.Interfaces;

public interface ISerializer
{
    IEnumerable<Station> DeSerializeByLINQ(string fileName);
    IEnumerable<Station> DeSerializeXML(string fileName);
    IEnumerable<Station> DeSerializeJSON(string fileName);
    void SerializeByLINQ(IEnumerable<Station> dep, string fileName);
    void SerializeXML(IEnumerable<Station> dep, string fileName);
    void SerializeJSON(IEnumerable<Station> dep, string fileName);
}