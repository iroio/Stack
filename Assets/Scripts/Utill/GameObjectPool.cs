using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : Component
{
    Func<T> _createFunction;
    Queue<T> _pool;
    int _count;

    public GameObjectPool(int count, Func<T> createFunction)
    {
        _pool = new Queue<T>(count);
        _createFunction = createFunction;
        _count = count;

        for (int i = 0; i < _count; i++)
        {
            _pool.Enqueue(_createFunction());
        }
    }

    public T Get()
    {
        if (_pool.Count > 0)
            return _pool.Dequeue();

        return _createFunction();
    }

    public void Set(T obj)
    {
        _pool.Enqueue(obj);
    }
}