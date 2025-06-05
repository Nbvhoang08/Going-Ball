using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingBride : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _speed = 90f;
    [SerializeField] float _maxAngleRotate, _minAngleRotate;
    [SerializeField] float _timeSleep;
    [SerializeField] float _timeDelay;
    [SerializeField] int _directionMove = 1;

    bool _sleep;
    bool _delaying;
    float _currentTime;

    void FixedUpdate()
    {
        if (_sleep)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _timeSleep)
            {
                _currentTime = 0f;
                _sleep = false;
                _delaying = true;
            }
            return;
        }

        if (_delaying)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _timeDelay)
            {
                _currentTime = 0f;
                _delaying = false;
                _directionMove *= -1;
            }
            return;
        }

        float currentY = _target.localEulerAngles.y;
        if (currentY > 180f)
            currentY -= 360f;

        float delta = _speed * Time.deltaTime * _directionMove;
        float nextY = currentY + delta;

        if (_directionMove == 1 && nextY >= _maxAngleRotate)
        {
            nextY = _maxAngleRotate;
            _sleep = true;
        }
        else if (_directionMove == -1 && nextY <= _minAngleRotate)
        {
            nextY = _minAngleRotate;
            _sleep = true;
        }

        Vector3 newEuler = _target.localEulerAngles;
        newEuler.y = nextY;
        _target.localEulerAngles = newEuler;
    }
    IEnumerator StopDelay()
    {
        yield return new WaitForSeconds(_timeDelay);
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

