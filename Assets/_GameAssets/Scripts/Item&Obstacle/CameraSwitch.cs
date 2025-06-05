using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    Vector3 spawn;
    Quaternion spawnRotation;
    private void Awake()
    {
        if (virtualCamera != null)
        {
            virtualCamera.enabled = false; 
        }
        spawn = virtualCamera.transform.position; // Store the initial position of the camera
        spawnRotation = virtualCamera.transform.rotation; // Store the initial rotation of the camera
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.transform.position.y > transform.position.y)
        {

            if (virtualCamera != null && !virtualCamera.enabled)
            {
                virtualCamera.enabled = true;
                virtualCamera.Follow = GameManager.Instance.target; // Assuming GameManager.Instance.target is the player transform
                virtualCamera.LookAt = GameManager.Instance.target;
                virtualCamera.PreviousStateIsValid = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Optionally disable the camera when the player exits the trigger
            if (virtualCamera != null && virtualCamera.enabled)
            {
               
                virtualCamera.PreviousStateIsValid = false;
                virtualCamera.enabled = false;
                StartCoroutine(ResetCameraPosition()); // Reset the camera position after a delay
                GameManager.Instance._mainCameraController.ChangeValueXAxis(0);
            }
        }
    }
    IEnumerator ResetCameraPosition()
    {
        yield return new WaitForSeconds(1.2f); // Wait for a short duration to ensure the camera is reset
        if (virtualCamera != null)
        {
            virtualCamera.Follow = null;
            virtualCamera.LookAt = null;
            virtualCamera.transform.position =spawn;
            virtualCamera.transform.rotation = spawnRotation; // Reset the camera to its initial position and rotation

        }
    }
}
