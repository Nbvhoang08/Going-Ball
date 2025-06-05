using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallModel : GenericPoolableObject
{
    public Renderer[] renderers;
    public void OnSetModel()
    {
        renderers = GetComponentsInChildren<Renderer>();
        StaticRotation[] hamsterRotaion = GetComponentsInChildren<StaticRotation>();
        if (hamsterRotaion != null)
        {
            foreach (StaticRotation rotation in hamsterRotaion)
            {
                rotation.OnPrepare();
            }

        }
    }
    public void SetUpToRender(LayerMask RenderLayer)
    {
        int layer = GetLayerFromMask(RenderLayer);
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = layer;
        }


    }
    int GetLayerFromMask(LayerMask mask)
    {
        int value = mask.value;
        for (int i = 0; i < 32; i++)
        {
            if ((value & (1 << i)) != 0)
                return i;
        }
        return 0; // fallback layer nếu không có bit nào bật
    }
    public void ResetLayer(LayerMask OriginLayer)
    {

        int layer = GetLayerFromMask(OriginLayer);
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = layer;
        }
        ReturnToPool();

    }
    


}
