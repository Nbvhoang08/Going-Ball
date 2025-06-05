using System;
using System.Security.Cryptography;
using Cinemachine.Utility;
using DG.Tweening;
using Hapiga.Core.Runtime.Process;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


public class BallController : MonoBehaviour
{
    #region Variable
    //[SerializeField] BallInputHandle _ballInput;
    [HideInInspector] public SwipeInputHandler _swipeInputHandler;
    [HideInInspector] public BallRotation _ballRotation;
    [HideInInspector] public BallSkin _ballSkin;
    [HideInInspector] public MainCamController _mainCameraController;
    BallSmoothCamera _smoothCamera;
    [SerializeField] CamTarget _ballCameraTarget;
    [SerializeField] float editorMoveSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] float onAirForceMultiplier = 0.3f;
    [SerializeField] float startForce;
    [SerializeField, Range(0f, 1f)] float _checkGroundRange;
    [SerializeField] ForceMode forceMode = ForceMode.Acceleration;
    [SerializeField] LayerMask _groundLayer;
    public Transform ballTracker;
    float targetDrag;
    Rigidbody _rb;
    public Rigidbody Rigidbody
    {
        get { return _rb; }
    }
    public bool IsDead = false;
    public bool OnMoveToGate { get; set; } = false;
    public bool OnHeightFlatform;
    [HideInInspector] public Vector3 contactNormal;
    public Rigidbody standOnMovingPlatform;
    [HideInInspector] public bool IsGrounded;
    [HideInInspector] public bool onReset;
    [HideInInspector] public bool BreakGround;
    [HideInInspector] public bool soundOn;
    [SerializeField] private float _raySpacing = 0.4f;
    [SerializeField] private float _rayLength = 0.5f;
    public float velocityMakeSound;
    [HideInInspector] public Vector3 tempForce;
    #endregion

    #region Process Method
    private void Awake()
    {
#if UNITY_EDITOR

#else
    
         Application.targetFrameRate = 60;
#endif

    }
    private void OnEnable()
    {
        GameEvent.OnResetLevel += ResetStartPoint;
        GameEvent.OnDead += ResetSpawnPoint;
        GameEvent.OnGameStart += OnGameStart;
    }
    private void OnDisable()
    {
        GameEvent.OnResetLevel -= ResetStartPoint;
        GameEvent.OnGameStart -= OnGameStart;
        GameEvent.OnDead -= ResetSpawnPoint;
    }
    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _swipeInputHandler = GetComponent<SwipeInputHandler>();
        _ballRotation = GetComponent<BallRotation>();
        _ballSkin = GetComponent<BallSkin>();
        _smoothCamera = GetComponent<BallSmoothCamera>();
        _mainCameraController = GameManager.Instance._mainCameraController;
        OnHeightFlatform = false;
        IsDead = false;
    }

    private void Update()
    {
        if (GameManager.Instance.GameMode != GameMode.Playing)
        {
          
            if (tempForce.magnitude > 0.001)
            {
                tempForce = Vector3.zero;
            }
            return;
        }
        IsGrounded = TouchedGround();
        
        


    }
    void FixedUpdate()
    {
        if (GameManager.Instance.GameMode != GameMode.Playing)
        {
            GameManager.Instance.StopSound(Constants.RollingRace);
            return;
        }
        
        HandleDrag();
        ApplyForce();
        UpdateContactNormal();
        _ballRotation.UpdateCameraRotation();
        _ballRotation.UpdateBallRotation();
        _ballRotation.UpdateInnerBallRotation();

        //if (IsGrounded && GetMovingVelocity().magnitude > velocityMakeSound)
        //{
        //    GameManager.Instance.PlaySoundInUpdate(Constants.RollingRace);
        //    float speed = GetMovingVelocity().magnitude;
        //    float t = Mathf.InverseLerp(0f, 20f, speed);
        //    customeVolume = Mathf.Lerp(0f, 0.2f, t);
        //    GameManager.Instance.CustomSound(Constants.RollingRace,customeVolume);
        //}
        //else
        //    GameManager.Instance.StopSound(Constants.RollingRace);

    }
    private void LateUpdate()
    {
        _smoothCamera.Smooth(_rb.velocity, ballTracker, _swipeInputHandler.inputLength);
        
    }
    #endregion

    #region Physic Controller
    public void ApplyForce()
    {
        if (tempForce.magnitude > 0.1f)
        {
            if (onReset)
            {
                onReset = false;
            }

            Rigidbody.AddForce(tempForce, forceMode);
            tempForce = Vector3.zero;

        }
    }
    
    public void AddForceSpeedUp(Vector2 inputDirection, float inputLength)
    {
        if (inputLength <= 0)
        {
            return;
        }
        float _moveSpeed = Application.isEditor ? editorMoveSpeed : moveSpeed;
        float length = IsGrounded ? inputLength : inputLength * onAirForceMultiplier;
        if (GetMovingVelocity().magnitude >= 5f && inputLength > 0.05) _moveSpeed *= 2f;


        Vector3 velocity = _rb.velocity;
        Vector3 moveDir = _mainCameraController.transform.TransformDirection(new Vector3(inputDirection.x, 0, inputDirection.y)).normalized;

        float dot = Vector3.Dot(velocity.normalized, moveDir);
        if (dot < 0f && GetMovingVelocity().magnitude < 15f && inputLength > 0.3f)
        {
            _moveSpeed *= 4;
        }

        Vector3 force = _mainCameraController.transform
                            .TransformDirection(new Vector3(inputDirection.x, 0, inputDirection.y)).normalized *
                        (length * _moveSpeed);
        force = force.ProjectOntoPlane(contactNormal);

        tempForce += force;
    }
  
    public void UpdateContactNormal()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 10,
                LayerMask.GetMask("Ground")))
        {
            contactNormal = hit.normal;
        }
        else
        {
            contactNormal = Vector3.up;
        }
    }
    private void HandleDrag()
    {
        float velocity = _rb.velocity.magnitude;


        if (_swipeInputHandler.inputLength > 0)
        {
            if (velocity <= 10)
            {
                float t = Mathf.InverseLerp(0f, 10f, velocity);
                _rb.drag = Mathf.Lerp(0f, 0.5f, t);
            }
            else if (velocity > 10 && velocity <= 25f)
            {
                _rb.drag = 0f;
            }
            else if (velocity > 25f && velocity < 30f)
            {
                float t = Mathf.InverseLerp(25f, 30f, velocity);
                _rb.drag = Mathf.Lerp(0f, 0.2f, t);
            }
            else
            {
                _rb.drag = 0.5f;
            }
        }
        else
        {
            targetDrag = 0.3f;
            _rb.drag = Mathf.MoveTowards(_rb.drag, targetDrag, Time.deltaTime * 0.2f);
        }
    }

    public Vector3 GetMovingVelocity()
    {
        if (standOnMovingPlatform)
        {
            return _rb.velocity - standOnMovingPlatform.velocity;
        }

        return _rb.velocity;
    }
    #endregion

    #region Game Event
    public void ResetSpawnPoint()
    {
        _rb.isKinematic = false;
        onReset = true;
        Vector3 spawnPos = LevelManager.Instance.levelPlaying.currentSpawnPoint.position;
        Quaternion newRotation = LevelManager.Instance.levelPlaying.currentSpawnPoint.rotation;
        spawnPos.y -= 2f;
        ResetTransform(spawnPos, newRotation);
    }
    public void ResetStartPoint()
    {
        _rb.isKinematic = false;
        onReset = true;
        Vector3 spawnPos = LevelManager.Instance.levelPlaying.spawnPoint[0].position;
        Quaternion newRotation = LevelManager.Instance.levelPlaying.spawnPoint[0].rotation;
        spawnPos.y -= 2f;
        ResetTransform(spawnPos, newRotation);
    }
    public void ResetTransform(Transform newTransform, UnityAction callback)
    {
        Vector3 position = new Vector3(newTransform.position.x, newTransform.position.y, newTransform.position.z);
        ResetTransform(position, newTransform.rotation);
        standOnMovingPlatform = null;
        if (callback != null)
            callback();
    }
    public void ResetTransform(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation * Quaternion.Euler(0f, 0f, 0f);
        _ballRotation.ResetRotation();
        _ballCameraTarget.ResetInfo();
        _ballSkin.Appear();
        _rb.isKinematic = false;
        _rb.velocity = Vector3.zero;
        IsDead = false;

    }
    public void OnDead()
    {
        _ballSkin.Disappear();
        IsDead = true;
    }
    private void MoveToGate(Gate gate)
    {
        Vector3 StartPoint = gate.spawn.SpawnPoint.position;
        StartPoint.y = transform.position.y;
        Vector3 direction = (StartPoint - _rb.position).normalized.ProjectOntoPlane(Vector3.up);
        //_rb.velocity = Vector3.zero;
        _rb.transform.forward = direction;
        _rb.isKinematic = true;
        _ballRotation.ResetRotation();
        float dt = 0;
        _rb.DOMove(StartPoint, Constants.timeDelayEnd).SetEase(Ease.OutQuad).SetUpdate(UpdateType.Fixed).OnUpdate(() =>
        {
            dt += Time.fixedDeltaTime;
            float speed = Mathf.Lerp(1000f, 0, dt / Constants.timeDelayEnd);
            _ballRotation.RollingInnerBall(speed * Time.smoothDeltaTime);
        }).OnComplete(() =>
        {
            transform.DORotateQuaternion(gate.spawn.SpawnPoint.rotation, 0.3f).SetEase(Ease.InCubic);
            _ballCameraTarget.ResetInfo();
            OnMoveToGate = false;
            tempForce = Vector3.zero;
            UIManager.Instance.OpenUI<WheelBonusUI>();

        });
    }
    private void OnGameStart()
    {
        _ballCameraTarget.ResetInfo();
        _rb.velocity = Vector3.zero;
        standOnMovingPlatform = null;
        _rb.AddForce(transform.forward * startForce, ForceMode.VelocityChange);
        onReset = false;
    }
    #endregion

    #region Checking Method
    private bool TouchedGround()
    {
        bool TouchedGround = Physics.CheckSphere(transform.position, _checkGroundRange, _groundLayer);
        if (!TouchedGround && !BreakGround)
        {
            BreakGround = !BreakGround;
        }
        return TouchedGround;
    } 
    public GroundType GetGroundType()
    {
        Vector3 origin = transform.position;
        Vector3 right = transform.right;
        Vector3 leftOrigin = origin - right * _raySpacing;
        Vector3 midOrigin = origin;
        Vector3 rightOrigin = origin + right * _raySpacing;

        int hitCount = 0;

        if (Physics.Raycast(leftOrigin, Vector3.down, _rayLength, _groundLayer)) hitCount++;
        if (Physics.Raycast(midOrigin, Vector3.down, _rayLength, _groundLayer)) hitCount++;
        if (Physics.Raycast(rightOrigin, Vector3.down, _rayLength, _groundLayer)) hitCount++;

        if (hitCount == 3) return GroundType.Ground;
        if (hitCount > 0 && hitCount < 3) return GroundType.Pipe;
        return GroundType.None;
    }
    public void OnDetected(Vector3 force)
    {
        _rb.velocity += force;
    }
    public void OnTouchGate(Gate gate)
    {
        gate.OnTouchedBall();
        GameManager.Instance.PlaySound(Constants.Win);
        MoveToGate(gate);
        GameManager.Instance.FinishLevel(gate, true);
    }
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Vector3 right = transform.right;

        Vector3 leftOrigin = origin - right * _raySpacing;
        Vector3 midOrigin = origin;
        Vector3 rightOrigin = origin + right * _raySpacing;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(leftOrigin, leftOrigin + Vector3.down * _rayLength);
        Gizmos.DrawLine(midOrigin, midOrigin + Vector3.down * _rayLength);
        Gizmos.DrawLine(rightOrigin, rightOrigin + Vector3.down * _rayLength);
    }
    public Transform DeafaultParent;
    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag(Constants.DynamicsGround))
    //    {
    //        standOnMovingPlatform = collision.gameObject.GetComponent<Rigidbody>();
    //    }
    //}
   

    //void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag(Constants.DynamicsGround))
    //    {
    //        standOnMovingPlatform = null;
    //        OnHeightFlatform = false;
    //    }
    //}
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(Constants.DynamicsGround))
        {
            transform.parent.SetParent(collision.transform);
            Debug.Log($"{collision.name}");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.DynamicsGround))
        {
            if(transform.parent.parent== null) 
            {
                transform.parent.SetParent(other.transform);
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag(Constants.DynamicsGround))
        {
            transform.parent.SetParent(null);

        }
    }

    #endregion

    #region Shop

    // Skin
    public void SetUsingBall(TypeBall typeBall)
    {
        _ballSkin.InitBallSkin(typeBall);
    }
    public void ShowShopSkin(bool show, UnityAction callback = null)
    {
        if (show)
        {
            Vector3 direction = Vector3.Cross(transform.forward, Vector3.up);
            _ballSkin.ShowAllSkin(direction);
        }
        else
        {
            _ballSkin.HideShowSkin(callback);
        }
    }

    //Trail
    public void SetUsingTrail(TrailType type)
    {
        _ballSkin.InitTrail(type);
    }

    public void ShowShopTrails(bool show, UnityAction callback = null)
    {
        if (show)
        {
            _ballSkin.ShowAllTrails();
        }
        else
        {
            _ballSkin.HideShowTrail(callback);
        }
    }





    #endregion
}

