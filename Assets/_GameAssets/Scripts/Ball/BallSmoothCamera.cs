using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class BallSmoothCamera : MonoBehaviour
{
    [SerializeField]
    float _smoothChangeYaxis;
    MainCamController _mainCameraController;
    BallController _ballController;
    [SerializeField] SwipeInputHandler swipeInputHandler;

    
    private void Start()
    {
        _ballController = GetComponent<BallController>();
        _mainCameraController = GameManager.Instance._mainCameraController;
    }
 
    public void Smooth(Vector3 velocity, Transform ballTracker, float _distanceSpeedUp)
    {

        if (GameManager.Instance.GameMode != GameMode.Playing) return;

        if (_ballController.onReset) return;

        float AngleY = 0f;

        if (velocity.sqrMagnitude < 0.001f)
        {
            AngleY = 0f;
        }
        else
        {
            AngleY = Vector3.Angle(velocity, ballTracker.forward);
        }
        int ReversedirectionY;
        if (_ballController.IsGrounded)
        {
            ReversedirectionY = -1;
            if (velocity.y < 0)
                ReversedirectionY = 1;
        }
        else
        {
            ReversedirectionY = 1;
        }

        _smoothChangeYaxis = Mathf.MoveTowards(_smoothChangeYaxis, Mathf.Pow(Vector3.ClampMagnitude(velocity, 1f).y, 2f) * ReversedirectionY, 1);
        if (AngleY >= 4f)
        {
            _mainCameraController.ChangeYAxis(Mathf.Min(_smoothChangeYaxis, 1f));

        }
        else
        {
            _smoothChangeYaxis = 0f;
            _mainCameraController.ChangeYAxis(_smoothChangeYaxis);
        }

    }  

}
