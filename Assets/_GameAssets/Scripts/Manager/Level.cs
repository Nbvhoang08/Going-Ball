using GameAnalyticsSDK.Setup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] private Gate _gate;
    public SkyBoxTheme theme;
    public Transform[] spawnPoint;
    public CoinFactory coinFactory;
    public KeyFactory keyFactory;
    public int curSpawnPointIndex = 0;
    public int Index { get { return _index; } set { _index = value; } }
    public Transform currentSpawnPoint { get { return spawnPoint[curSpawnPointIndex]; } }
    private void Awake()
    {
        coinFactory = GetComponentInChildren<CoinFactory>();
        keyFactory = GetComponentInChildren<KeyFactory>();
    }
    public void OnSetupNewLevel()
    {
        if (_index.Equals(GameManager.Instance.Level))
            _gate.OnSetupNewLevel();
        GameEvent.OnChangeBallSkinMethod();
        OnShow();
    }
    public void OnShow()
    {
        if (coinFactory != null )
            coinFactory.InitCoins();
        if ( keyFactory != null ) 
            keyFactory.InitKeys();
    }


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        if (spawnPoint.Length == 0)
        {
          
            return;
        }
        
        foreach (Transform point in spawnPoint)
        {
            Vector3 origin = point.position;
            Vector3 direction = Vector3.down;
            float rayLength = Constants.offsetCamHeight;
            // Vẽ ray
            Gizmos.DrawRay(origin, direction * rayLength);
        }

    }
}

