using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Obstacle
{
    [SerializeField] private Collider _collider;
    [SerializeField] private float jumpPower = 1f;
    [SerializeField] private float duration = 0.4f;
    private void Start()
    {
        _collider = GetComponent<Collider>();
    }
    public override void PrepareToUse()
    {
        base.PrepareToUse();

        if (_collider != null)
        {
            _collider.enabled = true;
        }
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
    }


    public override void OnTouchBall(BallDetect ballDetect)
    {
        base.OnTouchBall(ballDetect);
        ballDetect.OnTouchedCoin(Constants.ValuePerCoin);
        _collider.enabled = false;
        GameManager.Instance.PlaySound(Constants.CoinSound);
        Effect colliderFX = Pool.Instance.ClamCoinEffect;
        colliderFX.transform.position = transform.position;
        GameManager.Instance.SpawnEffectLookAtCamera(colliderFX);

        UIManager.Instance.ShowText($"+{Constants.ValuePerCoin}");
        Vector3 endValue = ballDetect.transform.position;
        endValue.z -= 2f;
        transform.DOJump(endValue, jumpPower, 1, duration).OnComplete(() =>
        {
            ReturnToPool();
 
        });
    }

}
