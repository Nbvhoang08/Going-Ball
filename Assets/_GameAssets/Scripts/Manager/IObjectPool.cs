using System.Collections.Generic;
using UnityEngine;
using System;

public interface IObjectPool
{
    void ReturnToPool(object instance);
}
public interface IObjectPool<T> : IObjectPool where T : IPoolable
{
    T GetPrefabInstance();
    void ReturnToPool(T instance);
}
public interface IPoolable
{
    IObjectPool Orgin { get; set; }
    void PrepareToUse();
    void ReturnToPool();
}
public class GenericObjectPool<T> : MonoBehaviour, IObjectPool<T> where T : MonoBehaviour, IPoolable
{
    // Reference to prefab.
    [SerializeField]
    private T prefab;
    // References to reusable instances
    private Queue<T> reusableInstances = new Queue<T>();
    /// <summary>
    /// Returns instance of prefab.
    /// </summary>
    /// <returns>Instance of prefab.</returns>
    public T GetPrefabInstance()
    {
        T inst;
        // if we have object in our pool we can use them
        if (reusableInstances.Count > 0)
        {
            // get object from pool
            inst = reusableInstances.Dequeue();
            // remove parent
            inst.transform.SetParent(null);
            // reset position
            inst.transform.localPosition = Vector3.zero;
            inst.transform.localScale = Vector3.one;
            inst.transform.localEulerAngles = Vector3.zero;
            // activate object
            inst.gameObject.SetActive(true);
        }
        // otherwise create new instance of prefab
        else
        {
            inst = Instantiate(prefab);
        }
        // set reference to pool
        inst.Orgin = this;
        // and prepare instance for use
        inst.PrepareToUse();
        return inst;
    }
    /// <summary>
    /// Returns instance to the pool.
    /// </summary>
    /// <param name="instance">Prefab instance.</param>
    public void ReturnToPool(T instance)
    {
        // disable object
        instance.gameObject.SetActive(false);
        // set parent as this object
        instance.transform.SetParent(transform);
        // reset position
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localScale = Vector3.one;
        instance.transform.localEulerAngles = Vector3.zero;
        // add to pool
        reusableInstances.Enqueue(instance);
    }
    /// <summary>
    /// Returns instance to the pool.
    /// Additional check is this is correct type.
    /// </summary>
    /// <param name="instance">Instance.</param>
    public void ReturnToPool(object instance)
    {
        // if instance is of our generic type we can return it to our pool
        if (instance is T)
        {
            ReturnToPool(instance as T);
        }
    }
}
public class GenericPoolableObject : MonoBehaviour, IPoolable
{
    // Pool to return object
    public IObjectPool Orgin { get; set; }
    /// <summary>
    /// Prepares instance to use.
    /// </summary>
    public virtual void PrepareToUse()
    {
        // prepare object for use
        // you can add additional code here if you want to.
    }
    /// <summary>
    /// Returns instance to pool.
    /// </summary>
    public virtual void ReturnToPool()
    {
        // prepare object for return.
        // you can add additional code here if you want to.
        if (Orgin != null)
        {
            Orgin.ReturnToPool(this);
        }
        gameObject.SetActive(false);

    }
}