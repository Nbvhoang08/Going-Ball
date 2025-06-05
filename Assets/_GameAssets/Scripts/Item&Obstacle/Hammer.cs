using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] HingeJoint joint;
    [SerializeField] float _maxAngleRotate, _minAngleRotate;
    [SerializeField] float _timeSleep;
    [SerializeField] float _timeDelay;
    [SerializeField] int _directionMove = 1;
    Rigidbody _rb;
    bool _sleep;
    [SerializeField] bool _delaying;
    float _currentTime;
    void Start()
    {
        _rb = joint.transform.GetComponent<Rigidbody>();
        joint.axis = new Vector3(0f, 0f, _directionMove);
        if(_delaying)
            joint.useMotor = false;
    }
    void FixedUpdate()
    {
        if (_delaying)
            return;

        if (!_sleep)
        {
            float localZ = joint.transform.localEulerAngles.z;
            if (localZ > 180)
                localZ -= 360;
            if (localZ >= _maxAngleRotate && _directionMove == 1)
            {
                Vector3 claimEulAngle = joint.transform.localEulerAngles;
                claimEulAngle.z = _maxAngleRotate;
                joint.transform.localEulerAngles = claimEulAngle;

                _directionMove = -_directionMove;
                _rb.velocity = Vector3.zero;
                joint.axis = new Vector3(0f, 0f, _directionMove);
                _sleep = true;
            }
            else if (localZ <= _minAngleRotate && _directionMove == -1)
            {
                Vector3 claimEulAngle = joint.transform.localEulerAngles;
                claimEulAngle.z = _minAngleRotate;
                joint.transform.localEulerAngles = claimEulAngle;

                _directionMove = -_directionMove;
                _rb.velocity = Vector3.zero;
                joint.axis = new Vector3(0f, 0f, _directionMove);
                _sleep = true;
            }
        }
        else
        {
            _rb.velocity = Vector3.zero;
            _currentTime += Time.deltaTime;
            if (_currentTime >= _timeSleep)
            {
                _currentTime -= _timeSleep;
                _sleep = false;
            }
        }
    }
    IEnumerator StopDelay()
    {
        yield return new WaitForSeconds(_timeDelay);
        joint.useMotor = true;
        _delaying = false;
    }
    private void OnEnable()
    {
        if (_timeDelay > 0)
        {
            _delaying = true;

            StartCoroutine(StopDelay());
        }
    }
}

