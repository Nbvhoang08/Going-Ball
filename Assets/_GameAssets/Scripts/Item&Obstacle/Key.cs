using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Obstacle
{
    [SerializeField] int value;


    public override void OnTouchBall(BallDetect ballDetect)
    {
        base.OnTouchBall(ballDetect);
        ballDetect.OnTouchedKey(value);
        transform.DOLocalMoveY(transform.localPosition.y + 5f, 0.5f).OnComplete(() =>
        {
            ReturnToPool();
        });
    }
}
