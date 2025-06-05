using UnityEngine;
using Hapiga.Core.Runtime.Singleton;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Hapiga.Core.Runtime.EventManager;
using System.Linq;
using Sirenix.OdinInspector;
using Dreamteck.Splines.Primitives;
using GameAnalyticsSDK.Setup;
using Hapiga.Tracking;


public class GameManager : Singleton<GameManager>
{
    UserData _userData;

    [SerializeField] GameData _gameData;
    [SerializeField] LevelManager _levelCreater;
    public GameData GameData => _gameData;
    public int Level { get { return _userData.Level; } set { _userData.Level = value; } }
    [SerializeField] private GameMode _gameMode;
    public GameMode GameMode => _gameMode;
    public Gate currentGate;
    public Transform target;
    public MainCamController _mainCameraController;
    [SerializeField] private ParticleSystem ContifestBlast;
    [SerializeField] private BallController _ballController;
    [SerializeField] private BallSkin _ballSkin;
    [SerializeField] private BallShopController _ballShopController;
    [SerializeField] private WorldShopController _worldController;
    [SerializeField] private WorldShopController _TrailController;
    [SerializeField] private BallRewardRender _ballRewardRender;
    public int Key { get { return _userData.Key; } set { _userData.Key = value; GameEvent.OnChangeCurrencyMethod(TypeCurrency.Key); } }
    public float Coin { get { return _userData.Coin; } set { _userData.Coin = value; GameEvent.OnChangeCurrencyMethod(TypeCurrency.Coin); } }
    public float Gift { get { return _userData.Gift; } set { _userData.Gift = value; } }
    public int AdsTicket
    {
        get
        {
            if (_userData == null)
                return 0;
            return _userData.AdsTicket;
        }
        set
        {
            if (_userData == null)
                return;
            _userData.AdsTicket = value; GameEvent.OnChangeCurrencyMethod(TypeCurrency.AdsTickets);
        }
    }
    [HideInInspector] public bool MusicOn { get { return _userData.MusicOn; } set { _userData.MusicOn = value; } }
    [HideInInspector] public bool SoundOn { get { return _userData.SoundOn; } set { _userData.SoundOn = value; } }
    [HideInInspector] public bool HapticOn { get { return _userData.HapticOn; } set { _userData.HapticOn = value; } }
    bool PaymentCoin(float coin)
    {
        if (Coin >= coin)
        {
            Coin -= coin;
            return true;
        }
        return false;
    }
    public bool PaymentTicket(int adsTicket)
    {
        if (AdsTicket >= adsTicket)
        {
            AdsTicket -= adsTicket;
            return true;
        }
        return false;
    }

    public bool IsCurrentVersion
    {
        get
        {
            if (_userData == null)
                return false;
            return _userData.userVersion.Equals(Application.version);
        }
    }
    public string UserVersion
    {
        get
        {
            if (_userData == null)
                return string.Empty;
            return _userData.userVersion;
        }
    }

    public bool IsTesting = false;
    public int CoinCollected;
    public int LeveltoTest = 0;
    public int CurrLife
    {
        get { return _userData.Life; }
        set
        {
            _userData.Life = value;
            if (_userData.Life > Constants.maxBalls)
                _userData.Life = Constants.maxBalls;
            if (_userData.Life <= 0)
                _userData.Life = 0;
            //GameEvent.OnUpdateUIMethod();
        }
    }
    public BallController BallController => _ballController;
    public override void Init()
    {

        UserData data = SaveSystem.LoadData();
        if (data != null)
        {
            _userData = data;
            _gameData.LoadData(_userData);
        }
        else
        {
            _userData = new UserData();
            _userData.userVersion = Application.version;
            //firstSeasion = true;
            InitData(ref _userData, true);
        }
        if (IsTesting)
        {
            Level = LeveltoTest - 1;
        }
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {

    }
    void Start()
    {
        //_currLife = 1;
        NewLevel();
        SoundManager.Instance.PlayBGMusic();
    }
    public void Resume()
    {
        _gameMode = GameMode.Playing;
        Time.timeScale = 1;
    }

    #region Level
    public void NewLevel()
    {
        _gameMode = GameMode.Starting;
        UIManager.Instance.CloseUIDirectly<InGameUI>();
        UIManager.Instance.OpenUI<StatUI>();
        GameEvent.OnSetUpNewLevelMethod();
        Level level = _levelCreater.levelPlaying;
        LevelManager.Instance.levelPlaying.curSpawnPointIndex = 0;
        _ballController.transform.rotation = Quaternion.identity;
        

        if (level != null)
        {
            level.OnSetupNewLevel();

            Vector3 spawnPos = level.spawnPoint[0].position;
            spawnPos.y -= 2.5f;
            _ballController.ResetTransform(spawnPos, level.spawnPoint[0].rotation);
        }
       
        if (currentGate != null)
        {
            currentGate.OnSetupNewLevel();
            currentGate.EnableBarrier(true);
            currentGate = null;
        }
        Vector3 pos = level.spawnPoint[0].position;
        pos.z -= Constants.offsetDisstance;
        pos.y += 1f;
        _mainCameraController.ChangeTheme(level.theme);
        _mainCameraController.SetTransform(pos, level.spawnPoint[0].eulerAngles.y);
        Debug.Log(level.spawnPoint[0].eulerAngles.y);
    }
    
    public void StartGame()
    {
        UIManager.Instance.OpenUI<InGameUI>();
        GameEvent.OnGameStartMethod();
        _mainCameraController.StartFollow(target);
        _gameMode = GameMode.Playing;
        GameEvent.OnUpdateUIMethod();
    }
    public void SelectLV(int LevelSelected ) 
    {
        Level = LevelSelected;
        NewLevel();
        UIManager.Instance.CloseUIDirectly<TestSetting>();

    }
    public void OnReset()
    {
        GameEvent.OnDeadMethod();
        _mainCameraController.SetTransform(LevelManager.Instance.levelPlaying.currentSpawnPoint.position, LevelManager.Instance.levelPlaying.currentSpawnPoint.eulerAngles.y);
        _mainCameraController.StartFollow(target);
    }

    public void ResetCurrentLevel()
    {
        NewLevel();
        _mainCameraController.SetTransform(LevelManager.Instance.levelPlaying.spawnPoint[0].position, LevelManager.Instance.levelPlaying.spawnPoint[0].eulerAngles.y);
        _mainCameraController.StartFollow(target);
        CurrLife = 1;
        GameEvent.OnUpdateUIMethod();
    }
    public void RewardReFill()
    {
        NewLevel();
        _mainCameraController.SetTransform(LevelManager.Instance.levelPlaying.spawnPoint[0].position, LevelManager.Instance.levelPlaying.spawnPoint[0].eulerAngles.y);
        _mainCameraController.StartFollow(target);
        CurrLife += Constants.maxBalls;
        GameEvent.OnUpdateUIMethod();

    }
    
    public void ChangeGameMode(GameMode gameMode) 
    {
        _gameMode = gameMode;
    }

    public void FinishLevel(Gate gate, bool win)
    {
        if (_gameMode.Equals(GameMode.Win))
            return;

        ContifestBlast.Play();
        _gameMode = GameMode.Win;


        if (win)
        {
            if (Level < GameData.levelPrefabs.Length - 1)
            {
                Level++;
            }
            else
            {
                Level = 0;
            }

            SaveData();
            
            //SaveCurrentLevel(Level);
        }
        currentGate = gate;


    }
    #endregion

    #region BallShop
    public void ShowNextBall(ref int Index)
    {
        _ballSkin.ShowNextSkin(ref Index);
    }
    public void ShowPreviousBall(ref int Index)
    {
        _ballSkin.ShowPreviousSkin(ref Index);
    }
    public void SelectBall(int index)
    {
        _gameData.SelectBall(index);
        GameEvent.OnChangeBallSkinMethod();
        _ballController.SetUsingBall(_gameData.BallUsing());
        ShowBallShop(false);

    }
    public void SelectBall(TypeBall typeBall)
    {
        _gameData.SelectBall((int)typeBall);
        GameEvent.OnChangeBallSkinMethod();
        _ballController.SetUsingBall(_gameData.BallUsing());
    }
    public void ShowRenderBall(bool show, TypeBall typeBall = TypeBall.Base)
    {
        _ballRewardRender.OnShow(show, typeBall);

    }
    public void UnlockBallOnShop(int index, bool ads = false)
    {
        BallInfor ballInfor = _gameData.BallInfors[index];
        if (ads || PaymentCoin(ballInfor.price))
        {
            _gameData.UnlockBall(index);
        }
    }
    public void UnlockBall(TypeBall typeBall)
    {
        _gameData.UnlockBall(typeBall);
    }
    public void UnlockWorld(int index, bool ads = false)
    {
        SkyBoxInfors skyboxInfor = _gameData.skyBoxInfors[index];
        if (ads || PaymentCoin(skyboxInfor.price))
        {
            _gameData.UnlockWorld(index);
        }
    }

    public void ShowBallShop(bool show)
    {
        _ballShopController.gameObject.SetActive(show);

        if (show)
        {
            UIManager.Instance.OpenUI<ChangeBallUI>();
        }
        else
        {
            UIManager.Instance.CloseUIDirectly<ChangeBallUI>();
        }

        _ballController.ShowShopSkin(show);
        _ballShopController.ShowShop(show, _ballController.transform);
    }

    #endregion

    #region TrailShop
    public void ShowNextTrails(ref int Index)
    {
        _ballSkin.ShowNextTrails(ref Index);
    }
    public void ShowPreviousTrails(ref int Index)
    {
        _ballSkin.ShowPreviousTrails(ref Index);
    }
    public void SelectTrails(int index)
    {
        _gameData.SelectTrail(index);
        _ballController.SetUsingTrail(_gameData.TrailUsing());
        ShowTrailsShop(false);
    }

    public void ShowTrailsShop(bool show)
    {
        UIManager.Instance.OpenUI<ChangeTrailUI>();
        _ballController.ShowShopTrails(show);
        _TrailController.gameObject.SetActive(show);
        _TrailController.ShowShop(show, _ballController.transform);
    }


    #endregion


    #region WorldShop
    public void ShowNextWorld (ref int Index)
    {
        _mainCameraController.ShowNextWorld(ref Index);
    }
    public void ShowPreviousWorld(ref int Index)
    {
        _mainCameraController.ShowPreviousWorld(ref Index);
    }
    public void SelectWolrd(int index)
    {
        _gameData.SelectWorld(index);
        _mainCameraController.ChangeSkyBox(_gameData.GetWorldInfor(index));
        ShowWorldShop(false);
    }
    public void HideWolrdShop() 
    {
        _mainCameraController.ChangeSkyBox(_gameData.WorldUsing());
    }
    public void ShowWorldShop(bool show)
    {
        if (!show) HideWolrdShop();
        UIManager.Instance.OpenUI<ChangeWorldUI>();
        _worldController.gameObject.SetActive(show);
        _worldController.ShowShop(show, _ballController.transform);
    }

    #endregion

    #region Setting
    public bool ChangeSetting(TypeSetting typeSetting)
    {
        switch (typeSetting)
        {
            case TypeSetting.Music:
                MusicOn = !MusicOn;
                if (MusicOn)
                    SoundManager.Instance.PlayBGMusic();
                else
                    SoundManager.Instance.MuteMusic();
                return MusicOn;
            case TypeSetting.Sound:
                SoundOn = !SoundOn;
                return SoundOn;
            case TypeSetting.Haptic:
                HapticOn = !HapticOn;
                return HapticOn;
        }
        return false;
    }
    public bool GetStateSetting(TypeSetting typeSetting)
    {
        switch (typeSetting)
        {
            case TypeSetting.Music:
                return MusicOn;
            case TypeSetting.Sound:
                return SoundOn;
            case TypeSetting.Haptic:
                return HapticOn;
        }
        return false;
    }
    #endregion

    #region Sound
    public void PlaySound(string name)
    {
        if (!SoundOn)
            return;

        SoundManager.Instance.Play(name);
    }
    public void PlaySoundInUpdate(string name)
    {
        if (!SoundOn)
            return;

        SoundManager.Instance.PlayInUpdate(name);
    }
    public void CustomSound(string name, float volume)
    {
        if (!SoundOn)
            return;
        SoundManager.Instance.CustomVolum(name, volume);
    }
    public void StopSound(string name)
    {
        if (!SoundOn)
            return;

        SoundManager.Instance.Stop(name);
    }
    #endregion

    #region SomeThing
    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == 5)
            {
                return true;
            }
        }

        return false;
    }
    public void ChangeLife(int amount)
    {
        CurrLife += amount;
        if(CurrLife <= 0)
        {
            UIManager.Instance.OpenUI<LoseUI>();
            _gameMode = GameMode.Lose;
        }
        else 
        {
            if(amount <0)
                OnReset();
        }
        GameEvent.OnUpdateUIMethod();
    }
    public void PlayHaptic(HapticType type)
    {
        if (!HapticOn)
            return;
        HapticManager.PlayHaptic(type);
    }
    public void SpawnEffectLookAtCamera(Effect colliderFX)
    {
        Transform cam = Camera.main.transform;
        Vector3 directionToCamera = cam.position - colliderFX.transform.position;
        colliderFX.transform.rotation = Quaternion.LookRotation(directionToCamera);
        colliderFX.transform.rotation *= Quaternion.Euler(0, 0f, 0f);
    }
    public void PauseGame()
    {
        if (!_gameMode.Equals(GameMode.Paused))
        {
            _gameMode = GameMode.Paused;
            Time.timeScale = 0;
        }
    }
    public void ResumeGame()
    {
        if (!_gameMode.Equals(GameMode.Playing))
        {
            _gameMode = GameMode.Playing;
            Time.timeScale = 1;
        }
    }
    public bool UseKey(int totalKey)
    {
        if (Key >= totalKey)
        {
            Key -= totalKey;
            return true;
        }
        else
            return false;
    }

    #endregion

    #region Data

    void InitData(ref UserData userData, bool firstTime)
    {
        _gameData.InitDatas(ref userData, firstTime);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
    }
    private void OnApplicationQuit()
    {
        SaveData();
    }
    void SaveData()
    {
        _gameData.SaveData(ref _userData);
        SaveSystem.SavePlayer(_userData);
    }
    [Button]
    public void SetLevel(int level)
    {
        Level = level;
        SaveData();
    }
    #endregion

    #region BallReward
    public BallInfor CaculationRewardBallOnTreasure(bool firstTime = false)
    {
        if (firstTime)
        {
            return _gameData.BallInfors[(int)TypeBall.Purffer];
        }

        List<BallInfor> ballInfors = new List<BallInfor>();
        for (int i = 0; i < _gameData.BallInfors.Length; i++)
        {
            if (_gameData.BallInfors[i].state.Equals(ItemState.unlock))
                ballInfors.Add(_gameData.BallInfors[i]);
        }
        ballInfors = ballInfors.OrderBy(x => x.price).ToList();

        int totalBall = ballInfors.Count > 3 ? 3 : ballInfors.Count; // 3 = number low price ball to random
        if (totalBall < 1)
            return _gameData.BallInfors[0];

        return ballInfors[Random.Range(0, totalBall)];
    }

    public BallInfor CaculationRewardBallOnGiftBox()
    {
        List<BallInfor> ballInfors = new List<BallInfor>();
        for (int i = 0; i < _gameData.BallInfors.Length; i++)
        {
            if (_gameData.BallInfors[i].state.Equals(ItemState.unlock) && _gameData.BallInfors[i].price > Coin)
                ballInfors.Add(_gameData.BallInfors[i]);
        }
        ballInfors = ballInfors.OrderBy(x => x.price).ToList();
        int totalBall = ballInfors.Count > 3 ? 3 : ballInfors.Count; // 3 = number low price ball to random
        if (totalBall < 1)
            return _gameData.BallInfors[0];
        return ballInfors[Random.Range(0, totalBall)];
    }
    #endregion
}