using Hapiga.Core.Runtime.EventManager;
using Hapiga.Core.Runtime.Singleton;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] Transform _levelContainer;
    [SerializeField] public int indexLevelToShows;
   
    public Level[] LevelShowings;
    public Level levelPlaying
    {
        get
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("❌ GameManager.Instance is NULL when accessing levelPlaying!");
                return null;
            }

            return LevelShowings
                   .Where(x => x.Index == GameManager.Instance.Level)
                   .FirstOrDefault();
        }
    }

    public override void Init()
    {
        
      

    }
    private void ShowLevels()
    {
        int currentLevel = GameManager.Instance.Level;
        int totalLevels = GameManager.Instance.GameData.levelPrefabs.Length;

        if (currentLevel >= totalLevels)
        {
            currentLevel = 0;
            GameManager.Instance.Level = 0;
            
        }

        indexLevelToShows = currentLevel;

        List<Level> existingLevels = _levelContainer.GetComponentsInChildren<Level>().ToList();
        foreach (Level level in existingLevels)
        {
            if (level.Index != currentLevel)
            {
                Destroy(level.gameObject);
            }
        }

        // Kiểm tra nếu level hiện tại chưa được tạo thì tạo mới
        bool hasCurrentLevel = existingLevels.Any(l => l.Index == currentLevel);
        if (!hasCurrentLevel)
        {
            Instantiate(GameManager.Instance.GameData.levelPrefabs[currentLevel], _levelContainer);
        }

        // Cập nhật lại danh sách LevelShowings
        LevelShowings = _levelContainer.GetComponentsInChildren<Level>();
    }


    void OnSetupNewLevel()
    {
        ShowLevels();
    }
    private void OnEnable()
    {
        GameEvent.OnSetUpNewLevel += OnSetupNewLevel;

    }
    private void OnDisable()
    {
        GameEvent.OnSetUpNewLevel -= OnSetupNewLevel;
    }
}
