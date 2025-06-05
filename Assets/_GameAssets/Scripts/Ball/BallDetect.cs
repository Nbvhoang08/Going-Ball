using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDetect : MonoBehaviour
{
    BallController _ball;
    [SerializeField] private float maxTimeSpamEfffect;
    public void AddForce(Vector3 force)
    {
        _ball.OnDetected(force);
    }
    public void OnTouchedCoin(float value)
    {
        GameManager.Instance.CoinCollected ++;
        GameManager.Instance.Coin += value;

    }
    public void OnTouchedKey(int value)
    {
        GameManager.Instance.Key += value;
        UIManager.Instance.ShowText("KEY FOUND");
        GameManager.Instance.PlaySound(Constants.KeySound);
    }
    private void Start()
    {
        _ball = GetComponent<BallController>();
    }
   
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.Ground))
        {
            if (_ball.BreakGround)
            {
                Effect colliderFX = Pool.Instance.ColRaceEffect;
                colliderFX.transform.position = collision.GetContact(0).point;
                _ball.BreakGround = false;
                GameManager.Instance.PlayHaptic(HapticType.Heavy);
                if (_ball.GetGroundType() == GroundType.Ground)
                {
                    GameManager.Instance.PlaySound(Constants.CollisionRace);
                }
                else
                {
                    GameManager.Instance.PlaySoundInUpdate(Constants.CollisionPipe);
                }   
            }
        }
        if (collision.gameObject.CompareTag(Constants.Fences))
        {
            Effect colliderFX = Pool.Instance.ColPipeEffect;
            colliderFX.transform.position = collision.GetContact(0).point;
            GameManager.Instance.PlayHaptic(HapticType.Heavy);
            GameManager.Instance.PlaySoundInUpdate(Constants.CollisionPipe);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.Ground))
        {

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.FinishGate))
        {
            _ball.OnTouchGate(other.GetComponentInParent<Gate>());
            GameManager.Instance.PlayHaptic(HapticType.Success);
        }
        if (other.CompareTag(Constants.Obstacles))
        {
            if (other.GetComponent<Obstacle>() != null)
            {
                 other.GetComponent<Obstacle>().OnTouchBall(this);
            }
        }
        if (other.transform.CompareTag(Constants.DeadZone))
        {
            if(_ball.IsDead) return;
            
            Effect resetFX = Pool.Instance.ResetEffect;
            resetFX.transform.position = other.ClosestPoint(transform.position);
            GameManager.Instance.PlaySound(Constants.Respawn);
            GameManager.Instance.SpawnEffectLookAtCamera(resetFX);
            resetFX.transform.localScale = Vector3.one;
            _ball.Rigidbody.isKinematic = true;
            _ball.OnDead();

            DOVirtual.DelayedCall(0.5f,
               () => GameManager.Instance.ChangeLife(-1));

        }
        if (other.transform.CompareTag(Constants.CheckPoint))
        {
            LevelManager.Instance.levelPlaying.curSpawnPointIndex++;
            other.enabled = false;
            other.GetComponent<SpawnPoint>().GetSpawnPoint();
            GameManager.Instance.PlaySound(Constants.Checkpoint);
            UIManager.Instance.ShowText("Ball Found");
            GameManager.Instance.ChangeLife(1);
        }
        
    }
}

