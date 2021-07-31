using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pools a GameObject Prefab. Depool() replaces Instantiate(), and Enpool() replaces Destroy().
/// </summary>
public class ObjectPool 
{
    public ICloneable blueprint;

    [SerializeField]
    [Min(0)]
    private int startingAmount = 6;

    private int maxAmount = 10;
    public int MaxAmount { get => maxAmount; }

    //runtime data
    private Stack<ICloneable> pool; //stack has better locality than queue
    private IList<ICloneable> manifest;

    /// <summary>
    /// Every GameObject Managed by this pool, non- and active alike.
    /// </summary>
    public IList<ICloneable> Manifest { get => manifest; }

    /// <summary>
    /// Total items this Pool tracks.
    /// </summary>
    public int PopulationCount { get => manifest.Count; }

    /// <summary>
    /// How many items are ready to be deployed.
    /// </summary>
    public int ReadyCount { get => pool.Count; }

    /// <summary>
    /// Number of items currently depooled.
    /// </summary>
    public int InUseCount { get => manifest.Count - pool.Count; }

    #region Constructors

    public ObjectPool(ICloneable blueprint, int startingAmount, int maxAmount)
    {
        this.blueprint = blueprint;
        this.startingAmount = startingAmount;
        this.maxAmount = maxAmount;
        InitPool();
    }

    #endregion

    private ICloneable CreatePoolable()
    {
        if (maxAmount >= 0 && PopulationCount >= maxAmount)
        {
            Debug.Log("[ObjectPool] Pool is exhausted. "
                + "Count: " + ConstStrings.GetCachedString(maxAmount)
                + ". Consider increasing 'maxAmount' or setting 'createWhenEmpty'.");

            return null; //at max capacity
        }

        var newObj = (ICloneable)blueprint.Clone();

        manifest.Add(newObj);//track

        return newObj;
    }

    public void AddItems(int amount = 1)
    {
        for (var i = amount - 1; i >= 0; --i)
        {
            var obj = CreatePoolable();
            if (obj != null)
                Enpool(obj);
        }
    }

    /// <summary>
    /// Take an item out of the pool.
    /// </summary>
    /// <returns>Newly de-pool object.</returns>
    public ICloneable Depool()
    {
        ICloneable depooledItem = null;

        if (pool.Count > 0)
            depooledItem = pool.Pop();
        else
            depooledItem = CreatePoolable(); //or not

        return depooledItem;
    }

    /// <summary>
    /// Take an item out of the pool and GetComponent{T} on it.
    /// </summary>
    /// <returns>Newly de-pool object or null if the Component wasn't found.</returns>
    public T Depool<T>() where T : class
    {
        var obj = Depool();
        return obj as T;
    }

    /// <summary>
    /// Add an item back into the pool. This replaces Destroy().
    /// </summary>
    /// <param name="poolable"></param>
    public void Enpool(ICloneable poolable)
    {
        if (poolable == null) return;

        if (!pool.Contains(poolable))//guard against multiple entries
        {
            pool.Push(poolable);
        }
    }

    /// <summary>
    /// Create entire pool
    /// </summary>
    public void InitPool()
    {
        maxAmount = maxAmount < startingAmount ? startingAmount : maxAmount;
        pool = new Stack<ICloneable>(maxAmount);

        //preload pool
        for (var i = startingAmount; i > 0; --i)
        {
            var newP = CreatePoolable();

            pool.Push(newP);
        }
    }

    public void ReturnAllToPool()
    {
        var count = manifest.Count;
        for (var i = 0; i < count; ++i)
            Enpool(manifest[i]);
    }
}
