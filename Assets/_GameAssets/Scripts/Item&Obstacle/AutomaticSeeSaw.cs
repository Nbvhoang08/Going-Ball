using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticSeeSaw : MonoBehaviour
{
    [SerializeField] Transform _model;
    [SerializeField] float _angleRotate;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _timeSleep;
    [SerializeField] int _direction = 1;
    float _maxAngle;
    private IEnumerator RotationModel()
    {
        WaitForSeconds timeSleep = new WaitForSeconds(_timeSleep);
        WaitForFixedUpdate timeMove = new WaitForFixedUpdate();
        bool sleep = false;
        while (true)
        {
            if (!sleep)
            {
                transform.localEulerAngles += new Vector3(0f, 0f, _moveSpeed * _direction * Time.deltaTime);

                float localZ = transform.localEulerAngles.z;
                if (localZ > 180)
                    localZ -= 360;
                if (localZ >= _maxAngle && _direction == 1 || localZ <= -_maxAngle && _direction == -1)
                {
                    _direction = -_direction;
                    sleep = true;
                }
                yield return timeMove;
            }
            else
            {
                yield return timeSleep;
                sleep = false;
            }
        }
    }
    private void OnEnable()
    {
        _maxAngle = _angleRotate / 2f;
        StartCoroutine(RotationModel());
    }
}

