using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Config _config;
    private PlayerProgress _levelProgress;

    private static readonly string configPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Config/config.json");
    private static readonly string progressPath = System.IO.Path.Combine(Application.streamingAssetsPath, "SaveDataGame/LevelProgress.json");

    public const string LEVEL_PREFIX = "Level";

    public int Gold => _config.gold;
    public int Lives => _config.lives;
    public float Speed => _config.speed;

    public string SceneName=> _config.sceneName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _config = LoadConfig();
        _levelProgress = LoadProgress();
    }

    #region Config

    private Config LoadConfig()
    {
        //If file does not exist, create it
        if (!File.Exists(configPath)) 
        {
            var config = new Config
            {
                gold = 100,
                lives = 20,
                speed = 1f,
                totalLevels = 5,
                sceneName = "Level"
            };

            SaveConfig(config);
        }

        var json = File.ReadAllText(configPath);
        
        //If after create or if file is empty, return default values
        if (string.IsNullOrEmpty(json))
            return new Config
            {
                gold = 100,
                lives = 20,
                speed = 1f,
                totalLevels=5,
                sceneName="Level"
            };

        return JsonUtility.FromJson<Config>(json);
    }

    private void SaveConfig(Config config) 
    {
        var json = JsonUtility.ToJson(config, true);
        File.WriteAllText(configPath, json);
    }

    #endregion

    #region Progress

    private void SaveProgress(PlayerProgress data)
    {
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(progressPath, json);
    }

    private PlayerProgress LoadProgress()
    {
        if (!File.Exists(progressPath)) 
        {
            var playerProgress = new PlayerProgress
            {
                Progress = new List<LevelProgress>() { new LevelProgress { Level=1, IsPassed=false } }
            };

            SaveProgress(playerProgress);
        }
        
        var json = File.ReadAllText(progressPath);
        return JsonUtility.FromJson<PlayerProgress>(json);
    }

    public void MarkLevelAsPassed(int levelNumber)
    {
        //Check if level is created. If it is not, will create new node. Otherwise change the value
        var existing = _levelProgress.Progress.FirstOrDefault(p => p.Level == levelNumber);
        if (existing == null)
        {
            _levelProgress.Progress.Add(new LevelProgress
            {
                Level = levelNumber,
                IsPassed = true
            });
        }
        else
        {
            existing.IsPassed = true;
        }

        //Create also the new node to Start Playing in tha level if the number of total Levels is 
        _levelProgress.Progress.Add(new LevelProgress
        {
            Level = levelNumber+1,
            IsPassed = false
        });

        //Save in json
        SaveProgress(_levelProgress);
    }

    public int GetMaxUnlockedLevel() 
    {
        var level = _levelProgress.Progress.Where(p => !p.IsPassed).FirstOrDefault();
        if (level == null)
            return 1;

        return level.Level;
    }

    public void ResetProgress() 
    {
        if (File.Exists(progressPath))
            File.Delete(progressPath);

        _levelProgress=LoadProgress();
    }

    #endregion
}

