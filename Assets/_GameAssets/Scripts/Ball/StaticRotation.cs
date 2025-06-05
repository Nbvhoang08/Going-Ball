using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticRotation : MonoBehaviour
{
    BallController ballController;
    BallRotation ballRotation;
    public bool IsForward;
    public void OnPrepare()
    {
        if(IsForward)
        {
            ballRotation = GetComponentInParent<BallRotation>();
        }
        else
        {
            ballController = GetComponentInParent<BallController>();
        }
     
    }
    private void LateUpdate()
    {
        RotationModel();
    }
    private void RotationModel()
    {
      
        if (!IsForward)
        {
            if(ballController != null)
                transform.forward = ballController.ballTracker.forward;
        }
        else
        {
            if (ballRotation != null)
                transform.forward = ballRotation.forwardBall.forward;
        }
    }
}
