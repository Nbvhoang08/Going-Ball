using Cinemachine;
using Hapiga.Core.Runtime.EventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShopController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] Transform ballTarget;
    [SerializeField] Vector3 offsetAngle;
    Vector3 forward;
    public void ShowShop(bool show, Transform ballTrans)
    {
        if (show)
        {
            ballTarget.transform.position = ballTrans.position;
            ballTarget.transform.rotation = ballTrans.rotation * Quaternion.Euler(offsetAngle);
            forward = Vector3.Cross(ballTrans.forward, Vector3.up);
        }
        GameEvent.OnShowShopBallMethod(show);
        gameObject.SetActive(show);
    }
}
