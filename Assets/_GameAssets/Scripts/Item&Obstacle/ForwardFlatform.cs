using System.Collections;
using UnityEngine;


public class ForwardFlatform : Elevator
{
    [SerializeField] float _timeDelay = 0f;
    Vector3 _checkPoint;
    protected override void Start()
    {
        _maxLocalDistance = _rbModel.transform.position.z + _maxDistance;
        _minLocalDistance = _rbModel.transform.position.z + _minDistance;
        _checkDirectionValue = _rbModel.position.z;
        _checkPoint = transform.position;
    }

    protected override void Move()
    {
        _checkPoint += transform.forward * _moveSpeed * _directionMove * Time.fixedDeltaTime;
        _checkPoint.z = Mathf.Clamp(_checkPoint.z, _minLocalDistance, _maxLocalDistance);
        transform.position = _checkPoint;
        _checkDirectionValue = _rbModel.position.z;
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
