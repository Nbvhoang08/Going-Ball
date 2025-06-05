using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBall : Obstacle
{
    [SerializeField] Rigidbody _rb;

    bool _onCable;
    float _distancePerFrame;
    int _totalFrame;
    int _currentFrame;

    Vector3 _startPoint, _direction;
    public void OnSpawn(Vector3 force, float _size, float _max)
    {
        _rb.mass = _max;
        _rb.AddForce(force, ForceMode.Impulse);
        transform.localScale = Vector3.one * _size;
    }

    private void FixedUpdate()
    {
        if (_onCable)
        {
            _rb.MovePosition(_startPoint + _direction * _distancePerFrame * _currentFrame);
            _currentFrame++;
            if (_currentFrame >= _totalFrame)
            {
                _onCable = false;
                _rb.isKinematic = false;
                ReturnToPool();
            }
        }
    }
    private void LateUpdate()
    {
        if (transform.position.y <= -200f)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            ReturnToPool();
        }
    }
    public void OnCable(Vector3 startPoint, Vector3 endPoint, float duration)
    {
        _totalFrame = (int)(duration / Time.fixedDeltaTime);
        _onCable = true;
        _currentFrame = 0;
        _distancePerFrame = Vector3.Distance(startPoint, endPoint) / _totalFrame;
        _rb.position = _startPoint = startPoint;
        _direction = (endPoint - startPoint).normalized;
        _rb.isKinematic = true;
    }
}

