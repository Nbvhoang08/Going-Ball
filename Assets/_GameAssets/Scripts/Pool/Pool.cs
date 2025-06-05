using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hapiga.Core.Runtime.Singleton;


public class Pool : Singleton<Pool>
{
    [SerializeField] ObstaclePool coin ,key;
    [SerializeField] ObstaclePool bigBall;
    [SerializeField] TextPool textEffect;
    [SerializeField] EffectPool resetEffect;
    [SerializeField] EffectPool ballCollisionRace;
    [SerializeField] EffectPool ballCollisionPipe;
    [SerializeField] EffectPool ClamCoin;
    [SerializeField] BallModelPool[] ballModel;
    [SerializeField] TrailFXPool[] trailFXPools;
    public override void Init()
    {

    }
    public Obstacle Coin { get { return coin.GetPrefabInstance(); } }
    public Obstacle Key { get { return key.GetPrefabInstance(); } }
    public Obstacle BigBall { get { return bigBall.GetPrefabInstance(); } }
    public TextEffect TextEffect { get { return textEffect.GetPrefabInstance(); } }
    public Effect ResetEffect { get { return resetEffect.GetPrefabInstance(); } }
    public Effect ColRaceEffect { get { return ballCollisionRace.GetPrefabInstance(); } }
    public Effect ColPipeEffect { get { return ballCollisionPipe.GetPrefabInstance(); } }
    public Effect ClamCoinEffect { get { return ClamCoin.GetPrefabInstance(); } }
    public BallModel Ball(TypeBall typeModel)
    {
        return ballModel[(int)typeModel].GetPrefabInstance();
    }
    public BallModel BallRender(TypeBall typeModel)
    {
        return ballModel[(int)typeModel].GetPrefabInstance();
    }

    public TrailFX GetTrailFX(TrailType typeTrail)
    {
        return trailFXPools[(int)typeTrail].GetPrefabInstance();
    }
}
