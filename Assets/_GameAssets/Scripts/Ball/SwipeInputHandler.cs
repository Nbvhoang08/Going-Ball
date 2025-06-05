using Cinemachine;
using UnityEngine;

public class SwipeInputHandler : MonoBehaviour
{
    [SerializeField] private float swipeThreshold = 30f;
    [SerializeField] private BallController _ballController;
    private Vector2 startPosition;
    private Vector2 endPosition;
    public bool isInputDown = false;
    public bool CanMove => inputLength >= swipeThreshold/Screen.dpi ;
    public Vector2 inputDirection;
    public float inputLength;
    private Vector2 cumulativeDelta = Vector2.zero;
    [SerializeField] private float forceMultiplier = 1.5f;
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#endif
       
    }




    private void HandleTouchInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                BeginInput(touch.position);
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                UpdateInput(touch.position);
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                EndInput(touch.position);
                break;
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BeginInput(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            UpdateInput(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndInput(Input.mousePosition);
        }
    }

    private void BeginInput(Vector2 position)
    {
        startPosition = position;
        endPosition = position;
        isInputDown = true;
        cumulativeDelta = Vector2.zero;
    }

    
    private void UpdateInput(Vector2 position)
    {
        if (!isInputDown) return;

        endPosition = position;

        float distance = Vector2.Distance(startPosition, endPosition);
        if (distance >= swipeThreshold)
        {
            CalculateSwipeForce();
            startPosition = endPosition;

        }

    }

    private void EndInput(Vector2 position)
    {
        isInputDown = false;
        inputDirection = Vector2.zero;
        inputLength = 0f;
    }

    private void CalculateSwipeForce()
    {
        inputDirection = endPosition - startPosition;
        inputLength = inputDirection.magnitude * forceMultiplier / Screen.dpi;

        if (_ballController == null)
        {
            return;
        }
        _ballController.AddForceSpeedUp(inputDirection, inputLength);

    }


   
}
