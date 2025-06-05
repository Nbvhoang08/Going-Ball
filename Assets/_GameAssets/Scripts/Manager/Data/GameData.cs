using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Data")]
public class GameData : ScriptableObject
{
    public Level[] levelPrefabs;
    public BallInfor[] BallInfors;
    public TrailFXInfor[] trailPrefabs;
    public SkyBoxInfors[] skyBoxInfors;
    public Sprite[] ticketSprites;
    public Sprite[] keySprites;
    public Sprite[] coinSprites;
    public Sprite treasureSprite;

    public void InitDatas(ref UserData userData, bool firstTime)
    {
        InitWorlds(ref userData, firstTime);
        InitBalls(ref userData, firstTime);
    }
    public void LoadData(UserData userData)
    {
        //load Skyboxs
        for (int i = 0; i < skyBoxInfors.Length; i++)
        {
            skyBoxInfors[i].name = skyBoxInfors[i].material.name;
            for (int j = 0; j < userData.SkyboxDatas.Length; j++)
            {
                if (skyBoxInfors[i].name.Equals(userData.SkyboxDatas[j].name))
                {
                    skyBoxInfors[i].state = userData.SkyboxDatas[j].state;
                }
            }
        }

        // load balls
        for (int i = 0; i < BallInfors.Length; i++)
        {
            for (int j = 0; j < userData.BallDatas.Length; j++)
            {
                if (BallInfors[i].typeBall.Equals(userData.BallDatas[j].type))
                    BallInfors[i].state = userData.BallDatas[j].state;
            }
        }
    }
    public void SaveData(ref UserData userData)
    {
        //save skyboxs
        for (int i = 0; i < skyBoxInfors.Length; i++)
        {
            for (int j = 0; j < userData.SkyboxDatas.Length; j++)
            {
                if (skyBoxInfors[i].name.Equals(userData.SkyboxDatas[j].name))
                {
                    userData.SkyboxDatas[j].state = skyBoxInfors[i].state;
                }
            }
        }

        //save ball
        for (int i = 0; i < BallInfors.Length; i++)
        {
            for (int j = 0; j < userData.BallDatas.Length; j++)
            {
                if (BallInfors[i].typeBall.Equals(userData.BallDatas[j].type))
                {
                    userData.BallDatas[j].state = BallInfors[i].state;
                }
            }
        }
    }

    void InitWorlds(ref UserData userData, bool firstTime)
    {
        for (int i = 0; i < skyBoxInfors.Length; i++)
        {
            skyBoxInfors[i].name = skyBoxInfors[i].material.name;
            if (firstTime)
            {
                //set default if first time
                if (i == 0)
                    skyBoxInfors[i].state = ItemState.used;
                else
                    skyBoxInfors[i].state = ItemState.unlock;
            }
        }
        userData.SkyboxDatas = new SkyboxData[skyBoxInfors.Length];

        for (int i = 0; i < userData.SkyboxDatas.Length; i++)
        {
            userData.SkyboxDatas[i] = new SkyboxData(skyBoxInfors[i].name, skyBoxInfors[i].state);
        }
    }
    void InitBalls(ref UserData userData, bool firstTime)
    {
        if (firstTime)
        {
            //set default if first time
            for (int i = 0; i < BallInfors.Length; i++)
            {
                if (i == 0)
                    BallInfors[i].state = ItemState.used;
                else
                    BallInfors[i].state = ItemState.unlock;
            }
        }
        userData.BallDatas = new BallData[BallInfors.Length];
        for (int i = 0; i < userData.BallDatas.Length; i++)
        {
            userData.BallDatas[i] = new BallData(BallInfors[i].typeBall, BallInfors[i].state);
        }
    }



    // Ball
    public int totalBall => BallInfors.Length;
    public int BallIndexUsing => (int)BallUsing();
    public TypeBall BallUsing()
    {
        for (int i = 0; i < BallInfors.Length; i++)
        {
            if (BallInfors[i].state.Equals(ItemState.used))
                return BallInfors[i].typeBall;
        }

        return BallInfors[0].typeBall;
    }
    public BallInfor GetBallInfor(int index)
    {
        return BallInfors[index];
    }
    public void SelectBall(int index)
    {
        for (int i = 0; i < BallInfors.Length; i++)
        {
            if (BallInfors[i].state.Equals(ItemState.used))
                BallInfors[i].state = ItemState.unlocked;
        }
        BallInfors[index].state = ItemState.used;
    }
    public void UnlockBall(int index)
    {
        BallInfors[index].state = ItemState.unlocked;
    }
    public void UnlockBall(TypeBall typeBall)
    {
        BallInfor ballInfor = BallInfors.FirstOrDefault(x => x.typeBall == typeBall);
        ballInfor.state = ItemState.unlocked;
    }
        
    // Trail 
    public int TotalTrail => trailPrefabs.Length;
    public int TrailIndexUsing => (int)TrailUsing();
    public TrailType TrailUsing()
    {
        for (int i = 0; i < trailPrefabs.Length; i++)
        {
            if (trailPrefabs[i].state.Equals(ItemState.used))
                return trailPrefabs[i].type;
        }
        return trailPrefabs[0].type;
    }
    public void SelectTrail(int index)
    {
        for (int i = 0; i < trailPrefabs.Length; i++)
        {
            if (trailPrefabs[i].state.Equals(ItemState.used))
                trailPrefabs[i].state = ItemState.unlocked;
        }
        trailPrefabs[index].state = ItemState.used;
    }


    // World
    public int TotalSkyBox => skyBoxInfors.Length;
    public int IndexWorldUsing()
    {
        for (int i = 0; i < skyBoxInfors.Length; i++)
        {
            if (skyBoxInfors[i].state.Equals(ItemState.used))
                return i;
        }

        return 0;
    }
    public SkyBoxInfors WorldUsing()
    {
        for (int i = 0; i < skyBoxInfors.Length; i++)
        {
            if (skyBoxInfors[i].state.Equals(ItemState.used))
                return skyBoxInfors[i];
        }

        return skyBoxInfors[0];
    }
    public SkyBoxInfors GetWorldInfor(int index)
    {
        return skyBoxInfors[index];
    }
    public void SelectWorld(int index)
    {
        for (int i = 0; i < skyBoxInfors.Length; i++)
        {
            if (skyBoxInfors[i].state.Equals(ItemState.used))
                skyBoxInfors[i].state = ItemState.unlocked;
        }
        skyBoxInfors[index].state = ItemState.used;
    }
    public void UnlockWorld(int index)
    {
        skyBoxInfors[index].state = ItemState.unlocked;
    }
}

[System.Serializable]
public class BallInfor
{
    public TypeBall typeBall;
    public ItemState state;
    public int price;
}
[System.Serializable]
public class SkyBoxInfors
{
    public Material material;
    public Color fogColor;
    public string name;
    public ItemState state;
    public int price;
}

[System.Serializable]
public class TrailFXInfor
{
    public TrailType type;
    public ItemState state;
    public int price;
}
