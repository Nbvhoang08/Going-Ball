using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform models;
    BallModel _ballModel;
    [SerializeField] private ParticleSystem GetSpawnPFX;
    [SerializeField] private ParticleSystem FX;
    Collider _collider;
    private void OnEnable()
    {
        GameEvent.OnChangeBallSkin += InitBallSkin;
    }
    private void OnDisable()
    {
        GameEvent.OnChangeBallSkin -= InitBallSkin;
    }
    
    public void InitBallSkin()
    {
        if (_ballModel != null)
        {
            _ballModel.ReturnToPool();
            _ballModel = null;
        }
        _ballModel = Pool.Instance.Ball(GameManager.Instance.GameData.BallUsing());
        _ballModel.transform.SetParent(models.transform);
        _ballModel.transform.localPosition = Vector3.zero;
        _ballModel.transform.localRotation = Quaternion.identity;
        gameObject.GetComponent<Collider>().enabled = true;
        if (!FX.gameObject.activeSelf) 
        {
            FX.gameObject.SetActive(true);
        }
        return;
    }
    public void GetSpawnPoint()
    {
        if (FX.gameObject.activeSelf)
        {
            FX.gameObject.SetActive(false);
        }
        GetSpawnPFX.Play();
        _ballModel.ReturnToPool();
        _ballModel = null;

    }



}
