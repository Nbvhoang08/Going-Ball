using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalFlatform : Elevator
{
    [SerializeField] float _timeDelay = 0f;
    Vector3 _checkPoint;
    protected override void Start()
    {
        _maxLocalDistance = _rbModel.transform.position.x + _maxDistance;
        _minLocalDistance = _rbModel.transform.position.x + _minDistance;
        _checkDirectionValue = _rbModel.position.x;
        _checkPoint = transform.position;
    }

    protected override void Move()
    {
        _checkPoint += transform.right * _moveSpeed * _directionMove * Time.fixedDeltaTime;
        _checkPoint.x = Mathf.Clamp(_checkPoint.x, _minLocalDistance, _maxLocalDistance);
        //_rbModel.MovePosition(_checkPoint);
        transform.position = _checkPoint;
        _checkDirectionValue = _rbModel.position.x;
    }
    private IEnumerator StopDelay()
    {
        yield return new WaitForSeconds(_timeDelay);
        delaying = false;
    }

    private void OnEnable()
    {
        if (_timeDelay > 0)
        {
            delaying = true;
            StartCoroutine(StopDelay());
        }
    }
}
