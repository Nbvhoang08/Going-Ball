using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : Elevator
{
    [SerializeField] float _timeDelay = 0f;
    Vector3 _checkPoint;
    protected override void Start()
    {
        _maxLocalDistance = _rbModel.transform.position.y + _maxDistance;
        _minLocalDistance = _rbModel.transform.position.y + _minDistance;
        _checkDirectionValue = _rbModel.position.y;
        _checkPoint = transform.position;
    }

    protected override void Move()
    {
        _checkPoint += transform.up * _moveSpeed * _directionMove * Time.fixedDeltaTime;
        _checkPoint.y = Mathf.Clamp(_checkPoint.y, _minLocalDistance, _maxLocalDistance);
        transform.position = _checkPoint;
        _checkDirectionValue = _rbModel.position.y;
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
