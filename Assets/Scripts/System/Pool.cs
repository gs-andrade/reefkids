using System.Collections.Generic;
using UnityEngine;


public class Pool<T> where T : Component
{
    private T prefab;
    private Queue<T> pool;
    private Transform parent;

    public Pool(int initialSize, T prefab, Transform parent)
    {
        this.pool = new Queue<T>();
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            AddToPool(CreateInstance());
        }
    }

    public T GetInstance()
    {
        T instance = null;

        if (pool.Count > 0)
        {
            instance = pool.Dequeue();
        }
        else
        {
            instance = CreateInstance();
        }

        instance.gameObject.SetActive(true);
        return instance;
    }

    private T CreateInstance()
    {
        return GameObject.Instantiate(prefab, parent) as T;
    }

    public void AddToPool(T instance)
    {
        instance.gameObject.SetActive(false);
        pool.Enqueue(instance);
    }
}