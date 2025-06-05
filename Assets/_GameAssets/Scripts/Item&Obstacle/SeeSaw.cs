using Hapiga.Core.Runtime.EventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeSaw : MonoBehaviour
{
    [SerializeField] bool auto;
    [SerializeField] float _maxAngle;
    [SerializeField] float _minAngle;
    [SerializeField]HingeJoint _HingeJoint;
    private void Start()
    {
        _HingeJoint = GetComponentInChildren<HingeJoint>();
        JointLimits jointLimits = _HingeJoint.limits;
        jointLimits.min = _minAngle;
        jointLimits.max = _maxAngle;
        _HingeJoint.limits = jointLimits;
    }
    void OnChangeLife()
    {
        if(_HingeJoint == null) 
        {
            _HingeJoint = GetComponentInChildren<HingeJoint>();
        }
        Rigidbody rb = _HingeJoint.transform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        _HingeJoint.transform.localEulerAngles = Vector3.zero;
    }

    private void OnEnable()
    {
        GameEvent.OnUpdateUI += OnChangeLife;
    }
    private void OnDisable()
    {
        GameEvent.OnUpdateUI -= OnChangeLife;
    }
}
