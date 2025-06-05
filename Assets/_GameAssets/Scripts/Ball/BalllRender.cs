using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalllRender : MonoBehaviour
{
    public Transform models;
    [SerializeField] BallModel _ballModel;

    [SerializeField] LayerMask RenderLayer;
    [SerializeField] LayerMask OriginLayer;
    private void OnEnable()
    {
        GameEvent.OnChangeBallSkin += InitBallRender;
    }
    private void OnDisable()
    {
        GameEvent.OnChangeBallSkin -= InitBallRender;
    }

    public void InitBallRender()
    {
        if (_ballModel != null)
        {
            _ballModel.ResetLayer(OriginLayer);
            _ballModel = null;
        }
        _ballModel = Pool.Instance.Ball(GameManager.Instance.GameData.BallUsing());
        _ballModel.transform.SetParent(models.transform);
        _ballModel.transform.localPosition = Vector3.zero;
        _ballModel.SetUpToRender(RenderLayer);
    }
   
}
