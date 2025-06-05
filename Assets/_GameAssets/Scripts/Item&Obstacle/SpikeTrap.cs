using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rbModel;
    [SerializeField] protected float _maxDistance;
    [SerializeField] protected float _minDistance;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _offsetReverseDirection;
    [SerializeField] protected float _timeSleep;
    [Range(-1, 1)]
    [SerializeField] protected int _directionMove = 1;
    protected float _maxLocalDistance;
    protected float _minLocalDistance;
    protected bool _sleep;
    protected float _timeSleeping;
    protected float _checkDirectionValue;
    Vector3 _checkPoint;
    private void Start()
    {
        _maxLocalDistance = _rbModel.transform.position.y + _maxDistance;
        _minLocalDistance = _rbModel.transform.position.y + _minDistance;
        _checkDirectionValue = _rbModel.position.y;
        _checkPoint = transform.position;
    }
    private void FixedUpdate()
    {
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

    private void Move()
    {
        _checkPoint += transform.up * _moveSpeed * _directionMove * Time.fixedDeltaTime;
        _checkPoint.y = Mathf.Clamp(_checkPoint.y, _minLocalDistance, _maxLocalDistance);
        _rbModel.MovePosition(_checkPoint);
        _checkDirectionValue = _rbModel.position.y;
    }
}
