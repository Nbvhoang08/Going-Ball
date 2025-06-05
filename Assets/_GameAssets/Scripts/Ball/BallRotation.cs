using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRotation : MonoBehaviour
{
    [SerializeField] private Transform rotateBall;
    public Transform forwardBall;
    public float rotationSpeed = 3;
    public float RotateXAxis = 0.5f; // This is the rotation speed for the X axis of the camer
    private float rotationAngleSpeed;
    public float maxRotationAngleSpeed = 40f;
    public float baseRotationAngleSpeed = 30f;
    private float velocityMagnitude;
    private BallController ballController;
    private void Awake()
    {
        ballController = GetComponent<BallController>();
    }

    public void UpdateInnerBallRotation()
    {
        if (GameManager.Instance.GameMode == GameMode.Playing)
        {
            Vector3 velocity = ballController.GetMovingVelocity();
            velocityMagnitude = velocity.magnitude;
            if (velocityMagnitude > 10)
            {
                float t = Mathf.InverseLerp(10f, 25f, velocityMagnitude);
                rotationAngleSpeed = Mathf.Lerp(baseRotationAngleSpeed, maxRotationAngleSpeed, t);
            }
            else
            {
                rotationAngleSpeed = baseRotationAngleSpeed;
            }

            if (velocityMagnitude > 0.01f)
            {
                float rotationAngle = velocityMagnitude * rotationAngleSpeed * Time.fixedDeltaTime;
                RollingInnerBall(rotationAngle);

            }
        }
    }
    public void UpdateBallRotation()
    {
        if (GameManager.Instance.GameMode == GameMode.Playing)
        {
            Vector3 velocity = ballController.GetMovingVelocity();
            velocityMagnitude = velocity.magnitude;
           

            if (velocityMagnitude > 0.01f)
            {
                forwardBall.forward =
                            Vector3.Slerp(forwardBall.forward, velocity.normalized, Time.fixedDeltaTime * rotationSpeed);


            }
        }
    }
    public void UpdateCameraRotation()
    {
        if (GameManager.Instance.GameMode == GameMode.Playing)
        {
            Vector3 velocity = ballController.GetMovingVelocity();
            velocityMagnitude = velocity.magnitude;
            Vector3 flatVelocity = new Vector3(velocity.x, 0f, velocity.z);
            if (flatVelocity.sqrMagnitude > 3.5f)
            {
                float targetYaw = Mathf.Atan2(flatVelocity.x, flatVelocity.z) * Mathf.Rad2Deg;

                if (targetYaw < 0f)
                    targetYaw += 360f;

                float currentYaw = ballController._mainCameraController.cinemachineFreeLook.m_XAxis.Value;


                float newYaw = Mathf.LerpAngle(currentYaw, targetYaw, Time.fixedDeltaTime * RotateXAxis);
                if (Mathf.Abs(Mathf.DeltaAngle(newYaw, targetYaw)) < 0.1f)
                    newYaw = targetYaw;
                ballController._mainCameraController.ChangeValueXAxis(newYaw);

            }
            
        }

    }


    public void RollingInnerBall(float rotationAngle)
    {
        rotateBall.Rotate(rotateBall.right, rotationAngle, Space.World);
    }
    public void ResetRotation()
    {
        rotateBall.localRotation = Quaternion.identity;
        forwardBall.localRotation = Quaternion.identity;
    }
}
