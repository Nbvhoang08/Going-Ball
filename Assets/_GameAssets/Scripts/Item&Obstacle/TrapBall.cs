using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBall : MonoBehaviour
{
    [SerializeField, Range(0, 10000f)]
    float _force, _mass;
    [SerializeField, Range(0, 10f)]
    float _size;

    [SerializeField, Range(0, 5f)] float _timeSpawn, _timeDelay;
    [SerializeField] Transform _SpawnPoint;
    BigBall _bigBall;
    private IEnumerator SpawnBalls()
    {
        WaitForSeconds timeSpawn = new WaitForSeconds(_timeSpawn);
        yield return new WaitForSeconds(_timeDelay);

        while (true)
        {
            _bigBall = Pool.Instance.BigBall as BigBall;
            if (_bigBall != null)
            {
                _bigBall.transform.position = _SpawnPoint.position;
                _bigBall.OnSpawn(transform.forward * _force, _size, _mass);
                _bigBall = null;
            }
            yield return timeSpawn;
        }
    }
    private void OnEnable()
    {
        StartCoroutine(SpawnBalls());
    }
}

