using Cinemachine.Utility;
using UnityEngine;

public class CamTarget : MonoBehaviour
{
    public BallController ball;
    //public float rotationSpeed = 10f;
    public float velocityInputThreshold = 4.5f;
    [SerializeField] private float velocityThreshold = 1f;
    public float smoothTime = 0.3f;

    private Vector3 currentVelocity;
    private Vector3 smoothDampVelocity;
    private Vector3 targetForward;
    public bool isRotating;
    public float minRotationSpeed = 1.5f;
    public float maxRotationSpeed = 4f;
    public float maxAngle = 120f;
    private void OnEnable()
    {
        GameEvent.OnResetLevel += ResetInfo;
        GameEvent.OnDead += ResetInfo;
    }
    private void OnDisable()
    {
        GameEvent.OnResetLevel -= ResetInfo;
        GameEvent.OnDead -= ResetInfo;
    }
    private void FixedUpdate()
    {
        transform.position = ball.transform.position;
        Vector3 velocity = ball.Rigidbody.velocity .ProjectOntoPlane(ball.contactNormal);

        if (GameManager.Instance.GameMode != GameMode.Playing)
        {
            return;
        }
        if (GameManager.Instance.BallController.standOnMovingPlatform) 
        {
            velocityThreshold = 6;
        }
        else
        {
            velocityThreshold = velocityInputThreshold;
        }


        if (!isRotating)
        {
            isRotating = velocity.magnitude > velocityThreshold;
        }
        else
        {
            currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity, ref smoothDampVelocity, smoothTime);
            targetForward = currentVelocity.normalized;
            if (targetForward.sqrMagnitude > 0.001f)
            {
                // Làm phẳng hướng target để chỉ quay trên mặt phẳng ngang (XZ)
                Vector3 flatTargetForward = new Vector3(targetForward.x, 0f, targetForward.z).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(flatTargetForward, Vector3.up);

                // Tính góc và dynamicSpeed
                float angle = Quaternion.Angle(transform.rotation, targetRotation);
                float dynamicSpeed = Mathf.Lerp(minRotationSpeed, maxRotationSpeed, angle / maxAngle);

                // Giữ lại style Slerp nhưng khóa X và Z bằng cách tách Euler
                Quaternion slerpedRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * dynamicSpeed);

                // Tách Euler ra và giữ lại chỉ trục Y
                Vector3 euler = slerpedRotation.eulerAngles;
                transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
            }

            if (velocity.magnitude <= velocityThreshold &&
                (velocity.sqrMagnitude <= 0.001f || Vector3.Dot(velocity.normalized, transform.forward) >= 0.9999f))
            {
                isRotating = false;
                currentVelocity = Vector3.zero;
            }
        }
    }
    

    public void ResetInfo()
    {
        isRotating = false;
        currentVelocity = Vector3.zero;
        smoothDampVelocity = Vector3.zero;
        targetForward = ball.transform.forward;
        transform.forward = targetForward;
        transform.position = ball.transform.position;

    }
}
