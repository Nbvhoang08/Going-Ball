using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPoint : MonoBehaviour
{
    [SerializeField] Vector3 _direction;
    [SerializeField] float _force;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Ball))
        {
            BallDetect ballDetect = other.GetComponent<BallDetect>();
            if (ballDetect != null)
            {
                GameManager.Instance.PlaySound(Constants.Jumpoint);
                GameManager.Instance.StopSound(Constants.RollingRace);
                ballDetect.AddForce(_direction * _force);
            }
          
        }
    }
}
