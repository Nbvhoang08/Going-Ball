using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public float Coin;
    public float Gift;
    public int Key;
    public int AdsTicket;
    public int Level;
    public int Life;
    public bool NoAds;
    public bool LoadedConfigData;
    public string userVersion;
    public float interval;
    public SkyboxData[] SkyboxDatas;
    public BallData[] BallDatas;
    public bool SoundOn = true, MusicOn = true, HapticOn = true;
    public UserData()
    {
        AdsTicket = 1;
        interval = 40f;
    }

}
[System.Serializable]
public struct SkyboxData
{
    public string name;
    public ItemState state;
    public SkyboxData(string name, ItemState state)
    {
        this.name = name;
        this.state = state;
    }
}

[System.Serializable]
public struct BallData
{
    public TypeBall type;
    public ItemState state;
    public BallData(TypeBall type, ItemState state)
    {
        this.type = type;
        this.state = state;
    }
}
