using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRewardRender : MonoBehaviour
{
    public Transform models;
    [SerializeField] BallModel _ballModel;

    [SerializeField] LayerMask RenderLayer;
    [SerializeField] LayerMask OriginLayer;
    public void OnShow(bool show, TypeBall TypeBall)
    {
        gameObject.SetActive(show);

        if (show)
        {
        
            if (_ballModel != null)
            {
                _ballModel.ResetLayer(OriginLayer);
                _ballModel = null;
            }
            _ballModel = Pool.Instance.Ball(TypeBall);
            _ballModel.transform.SetParent(models.transform);
            _ballModel.transform.localPosition = Vector3.zero;
            _ballModel.SetUpToRender(RenderLayer);
        }
        else 
        {
            if (_ballModel != null) 
                _ballModel.ResetLayer(OriginLayer);
            _ballModel = null;
        }
        
    }
}
