using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : GenericPoolableObject
{
    [SerializeField] protected float timeAlive;
    public override void PrepareToUse()
    {
        base.PrepareToUse();
        Invoke("ReturnToPool", timeAlive);
    }
}
