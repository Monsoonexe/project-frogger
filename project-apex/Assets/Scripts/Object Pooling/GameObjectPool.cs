using System.Collections.Generic;
using UnityEngine;

//I admit that I imported this script from an existing package that I own and maintain.
//pooling is too important and re-writing this would be a pain, and I know this works (cuz I wrote it).

public delegate void GameObjectMethod(GameObject poolable);
//TODO: Reclaim when empty strategy. Like bullet holes in FPS games.
//TODO: ability to pre-spawn items in the Editor.

/// <summary>
/// Pools a GameObject Prefab. 
/// Depool() replaces Instantiate(), and Enpool() replaces Destroy().
/// </summary>
public class GameObjectPool : ApexMonoBehaviour
{
    public enum InitStrategy
    {
        /// <summary>
        /// Init right away when item created.
        /// </summary>
        OnCreate = 0,

        /// <summary>
        /// Init each time item is depooled.
        /// </summary>
        OnDepool = 1
    }

    [Header("---Resources---")]
    public GameObject objectPrefab;

    [Header("---Settings---")]
    public bool initOnAwake = true;

    [SerializeField]
    protected InitStrategy initStragety = InitStrategy.OnCreate;

    [SerializeField]
    [Min(0)]
    private int startingAmount = 6;

    [SerializeField]
    [Tooltip("less than 0 means 'no limit'.")]
    [Min(-1)]
    private int maxAmount = 10;
    public int MaxAmount { get => maxAmount; }

    [Tooltip("[Optional]")]
    public Transform poolParent = null;

    /// <summary>
    /// [Optional] Something special to be performed on new entries.
    /// </summary>
    public GameObjectMethod InitPoolableMethod = (p) => p = null;//no-op so never null

    //runtime data
    private Stack<GameObject> pool; //stack has better locality than queue
    private List<GameObject> manifest;
    /// <summary>
    /// Every GameObject Managed by this pool, non- and active alike.
    /// </summary>
    public IList<GameObject> Manifest { get => manifest; }

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

    private void Reset()
    {
        SetDevDescription("I replace Instantiate with a pool of objects. " +
            "I'm doing my part to reduce garbage generation.");
        poolParent = GetComponent<Transform>();
    }

    protected void Awake()
    {
        var poolSize = Mathf.Max(startingAmount, maxAmount);
        manifest = new List<GameObject>(poolSize);
        if (initOnAwake)
            InitPool();
    }

    private GameObject CreatePoolable()
    {
        GameObject newGameObject = null;//return value
        if (maxAmount >= 0 && PopulationCount >= maxAmount)
        {
            //Debug.Log("[" + name + "] Pool is exhausted. "
            //    + "Count: " + ConstStrings.GetCachedString(maxAmount)
            //    + ". Consider increasing 'maxAmount' or setting 'createWhenEmpty'."
            //    , this);
        }
        else
        {
            newGameObject = Instantiate(objectPrefab, poolParent);
            manifest.Add(newGameObject);//track
        }

        return newGameObject;
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
    public GameObject Depool()
    {
        GameObject depooledItem = null;

        if (pool.Count > 0)
            depooledItem = pool.Pop();
        else
            depooledItem = CreatePoolable(); //or not

        if (depooledItem != null)
        {
            if (initStragety == InitStrategy.OnDepool)
                InitPoolableMethod(depooledItem);
            depooledItem.SetActive(true);//behaves like Instantiate();
        }

        return depooledItem;
    }

    /// <summary>
    /// Take an item out of the pool and place at world space with rotation.
    /// </summary>
    /// <returns>Newly de-pool object.</returns>
    public GameObject Depool(Transform handle)
    {
        var obj = Depool();
        if (obj != null)
        {
            var trans = obj.GetComponent<Transform>();
            trans.position = handle.position;
            trans.rotation = handle.rotation;
            trans.parent = handle;

            //maybe this is faster?
            //trans.parent = handle;
            //trans.position = Vector3.zero;
            //trans.rotation = Quaternion.identity;
        }
        return obj;
    }

    /// <summary>
    /// Take an item out of the pool and place at world space with rotation.
    /// </summary>
    /// <returns>Newly de-pool object.</returns>
    public GameObject Depool(Transform handle, bool setParent)
    {
        GameObject obj = null;
        if (setParent)
            obj = Depool(handle);
        else
            obj = Depool(handle.position, handle.rotation);
        return obj;
    }

    /// <summary>
    /// Take an item out of the pool and place at world space.
    /// </summary>
    /// <returns>Newly de-pool object.</returns>
    public GameObject Depool(in Vector3 position)
    {
        var obj = Depool();
        if (obj != null)
        {
            var trans = obj.GetComponent<Transform>();
            trans.position = position;
        }
        return obj;
    }

    /// <summary>
    /// Take an item out of the pool and set at world space with orientation.
    /// </summary>
    /// <returns>Newly de-pool object.</returns>
    public GameObject Depool(in Vector3 position, in Quaternion rotation)
    {
        var obj = Depool();
        if (obj != null)
        {
            var trans = obj.GetComponent<Transform>();
            trans.position = position;
            trans.rotation = rotation;
        }
        return obj;
    }

    /// <summary>
    /// Take an item out of the pool and GetComponent{T} on it.
    /// </summary>
    /// <returns>Newly de-pool object or null if the Component wasn't found.</returns>
    public T Depool<T>() where T : Component
    {
        T component = null;
        var obj = Depool();
        if (obj)
            component = obj.GetComponent<T>();
        return component;
    }

    /// <summary>
    /// Useful for delegates/events.
    /// lol. doodee pool.
    /// </summary>
    public void DoDepool() => Depool();

    /// <summary>
    /// Useful for delegates/events.
    /// lol. doodee pool.
    /// </summary>
    public void DoDepool(Transform handle) => Depool(handle);

    /// <summary>
    /// Useful for delegates/events.
    /// lol. doodee pool.
    /// </summary>
    public void DoDepool(in Vector3 point) => Depool(point);

    public void Enpool(MonoBehaviour poolable)
        => Enpool(poolable.gameObject);

    /// <summary>
    /// Add an item back into the pool. This replaces Destroy().
    /// </summary>
    /// <param name="poolable"></param>
    public void Enpool(GameObject poolable)
    {
        Debug.Assert(poolable != null,
            "[GameObjectPool] Trying to Enpool null!", this);

        Debug.AssertFormat(manifest.Contains(poolable),
            "[GameObjectPool] This item is not included on this Pool's manifest. " +
            "This item probably belongs to another Pool. " +
            "pendingPoolable: {0}. Pool: {1}. manifest[0] {2}.",
            poolable, gameObject, manifest[0]);

        if (!pool.Contains(poolable))//guard against multiple entries
        {
            pool.Push(poolable);
            poolable.SetActive(false);
        }
    }

    /// <summary>
    /// Create entire pool
    /// </summary>
    public void InitPool()
    {
        maxAmount = maxAmount < startingAmount ? startingAmount : maxAmount;
        pool = new Stack<GameObject>(maxAmount);

        //preload pool
        for (var i = startingAmount; i > 0; --i)
        {
            var newP = CreatePoolable();

            if (initStragety == InitStrategy.OnCreate)
                InitPoolableMethod(newP);

            pool.Push(newP);
            newP.SetActive(false);
        }
    }

    /// <summary>
    /// Resize the pool.
    /// </summary>
    /// <param name="newCapacity"></param>
    public void Resize(int newCapacity)
    {
        maxAmount = Mathf.Min(newCapacity, ReadyCount);
        while (pool.Count > maxAmount) //shrink
        {
            var poolable = pool.Pop();//trim excess
            manifest.Remove(poolable);
            Destroy(poolable.gameObject);
        }

        manifest.TrimExcess();
        pool.TrimExcess();
    }

    public void ReturnAllToPool()
    {
        var count = manifest.Count;
        for (var i = 0; i < count; ++i)
            Enpool(manifest[i]);
    }
}
