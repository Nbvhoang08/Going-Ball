using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] protected Transform _rbModel;
    [SerializeField] protected float _maxDistance;
    [SerializeField] protected float _minDistance;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _offsetReverseDirection;
    [SerializeField] protected float _timeSleep;
    [Range(-1, 1)]
    [SerializeField] protected int _directionMove = 1;
    [SerializeField] protected bool delaying;
    [SerializeField] protected float _maxLocalDistance;
    [SerializeField] protected float _minLocalDistance;
    [SerializeField] protected bool _sleep;
    protected float _timeSleeping;
    protected float _checkDirectionValue;
    void Awake()
    {
        //_rbModel = GetComponentInChildren<Rigidbody>();
    }
    protected virtual void Start()
    {

    }
    private void FixedUpdate()
    {
        if (delaying)
            return;
        if (_sleep)
        {
            _timeSleeping += Time.fixedDeltaTime;
            if (_timeSleeping >= _timeSleep)
            {
                _timeSleeping -= _timeSleep;
                _sleep = false;
            }
        }
        else
        {
            Move();
        }
    }
    private void LateUpdate()
    {
        if (_checkDirectionValue >= _maxLocalDistance && _directionMove == 1 || _checkDirectionValue <= _minLocalDistance && _directionMove == -1)
        {
            _sleep = true;
            _directionMove = -_directionMove;
        }
      

        
    }

    protected virtual void Move()
    {
    }
}
