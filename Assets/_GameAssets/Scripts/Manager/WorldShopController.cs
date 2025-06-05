using Cinemachine;
using UnityEngine;

public class WorldShopController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] Transform worldTarget;
    [SerializeField] Vector3 offsetAngle;
    [SerializeField, Range(0f, 100f)] float rotateSpeed;
    bool _Showing;
    public void ShowShop(bool show, Transform ballTrans)
    {
        if (show)
        {
            worldTarget.transform.position = ballTrans.position;
            worldTarget.transform.rotation = ballTrans.rotation * Quaternion.Euler(offsetAngle);
        }
        //GameEvent.OnShowWorldShopMethod(show);
        _Showing = show;
        gameObject.SetActive(show);
    }
    private void FixedUpdate()
    {
        if (_Showing)
        {
            worldTarget.eulerAngles += new Vector3(0f, rotateSpeed * Time.deltaTime, 0f);
        }
    }
}
