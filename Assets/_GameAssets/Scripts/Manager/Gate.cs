using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    //[SerializeField] GameObject[] fences;
    public Transform EndPoint, Barrier;
    public Spawn spawn;
    public CinemachineVirtualCamera endCamera;
    private void OnEnable()
    {
        endCamera.gameObject.SetActive(false);
    }
    public void OnTouchedBall()
    {
        EnableEndCamera(true);

    }
    public void OnSetupNewLevel()
    {
        EnableEndCamera(false);
    }
    public void EnableBarrier(bool enable)
    {
        Barrier.gameObject.SetActive(enable);
        EndPoint.gameObject.SetActive(!enable);
    }
    private void EnableEndCamera(bool enable)
    {
        endCamera.gameObject.SetActive(enable);
    }
    //public void OnShowShopBall(bool show)
    //{
    //    foreach (GameObject GO in fences)
    //    {
    //        if (GO != null)
    //            GO.gameObject.SetActive(!show);
    //    }
    //}
}
