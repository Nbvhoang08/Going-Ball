using System;
using UnityEngine;

[System.Serializable]
public class TrailFX : GenericPoolableObject
{
    [SerializeField] private Vector3 Offset;
    [SerializeField] private ParticleSystem VFX;
    public override void PrepareToUse()
    {
        base.PrepareToUse();
        VFX = GetComponent<ParticleSystem>();
        transform.localPosition = Offset;
        if(VFX != null)
        {
            VFX.Clear();
            VFX.Play();
        }
    }

}
