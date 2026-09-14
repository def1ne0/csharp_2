namespace lab1.Interfaces;

public interface ICustomCollection<T>
{
    T this[int index] { get; set; }
    
    void Reset();
    
    void MoveNext();
    
    T Current();
    
    int Count { get; }
    void Add(T item);
    
    void Remove(T item);
    
    T RemoveCurrent();
}