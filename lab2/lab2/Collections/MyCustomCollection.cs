using System.Collections;
using System.Diagnostics;
using lab2.Collections.Exceptions;

namespace lab2.Collections;

using Interfaces; // ICustomCollection;

public class MyCustomCollection<T>: 
    ICustomCollection<T>,
    IEnumerable<T> 
    where T: IEquatable<T>
{
    private Node<T>? _head;
    private Node<T>? _curr;
    private int _size;
    
    public MyCustomCollection(IEnumerable<T> range)
    {
        _size = 0;
        
        foreach (var item in range)
        {
            Add(item);
        }
    }
    
    public MyCustomCollection(params ReadOnlySpan<T> args)
    {
        _size = 0;
        
        foreach (var item in args)
        {
            Add(item);
        }
    }

    public MyCustomCollection()
    {
        _head = null;
        _curr = null;
        _size = 0;
    }

    public int Count
    {
        get => _size;
    }

    public void Add(T item)
    {
        var newNode = new Node<T>(item);

        if (_head == null)
        {
            _head = newNode;
            _curr = _head;
        }
        else
        {
            var last = _head;

            while (last.Next != null)
            {
                last = last.Next;
            }

            last.Next = newNode;
        }

        ++_size;
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _size || _head == null) throw new IndexOutOfRangeException();
            
            var curr = _head;

            for (var i = 0; i < index; ++i)
            {
                Debug.Assert(curr != null);
                curr = curr.Next;
            }

            Debug.Assert(curr != null);
            return curr.Data;
        }
        set
        {
            if (index < 0 || index >= _size || _head == null) throw new IndexOutOfRangeException();
            
            var curr = _head;

            for (var i = 0; i < index; ++i)
            {
                Debug.Assert(curr != null);
                curr = curr.Next;
            }

            Debug.Assert(curr != null);
            curr.Data = value;
        }
    }

    public void Reset()
    {
        _curr = _head;
    }

    public void MoveNext()
    {
        if (_curr != null && _curr.Next != null)
        {
            _curr = _curr.Next;
        }
    }

    public T Current()
    {
        if (_curr == null)
        {
            throw new InvalidOperationException();
        }

        return _curr.Data;
    }

    public T RemoveCurrent()
    {
        if (_curr == null)
        {
            throw new InvalidOperationException();
        }

        var removedData = _curr.Data;
        Remove(removedData);

        return removedData;
    }

    public void Remove(T target)
    {
        if (_head == null)
        {
            throw new TargetNotFoundException();
        }

        if (_head.Data.Equals(target))
        {
            if (_curr == _head)
            {
                _curr = _head.Next;
            }

            _head = _head.Next;
            --_size;
            return;
        }

        var temp = _head;
        
        while (temp.Next != null && !temp.Next.Data.Equals(target))
        {
            temp = temp.Next;
        }

        if (temp.Next != null)
        {
            if (_curr == temp.Next)
            {
                _curr = temp.Next.Next;
            }

            temp.Next = temp.Next.Next;
            --_size;
        }
        else
        {
            throw new TargetNotFoundException();
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < _size; ++i)
        {
            yield return this[i];
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}