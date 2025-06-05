using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTracker : MonoBehaviour
{
    public Vector3 Velocity { get; private set; }

    private Vector3 _lastPosition;

    private void Start()
    {
        _lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Velocity = (transform.position - _lastPosition) / Time.fixedDeltaTime;
        _lastPosition = transform.position;
    }
}
