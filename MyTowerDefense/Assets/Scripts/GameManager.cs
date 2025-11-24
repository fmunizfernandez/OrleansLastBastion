using System;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action<int> OnEnemyEndsAlive;
    public static event Action<int> OnGoldChange;

    private int _lives = 20;
    private int _initialGold = 100;
    private int _gold = 0;
    private float _gameSpeed = 1f;

    public float GameSpeed => _gameSpeed;

    public int Gold => _gold;

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

        LoadConfig();
    }
    private void OnEnable()
    {
        Enemy.OnEnemyReachEnd += Enemy_OnEnemyReachEnd;
        Enemy.OnEnemyDestroyed += Enemy_OnEnemyDestroyed;
        TowerSelection.OnLocateTower += TowerSelection_OnLocateTower;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyReachEnd -= Enemy_OnEnemyReachEnd;
        Enemy.OnEnemyDestroyed -= Enemy_OnEnemyDestroyed;
        TowerSelection.OnLocateTower -= TowerSelection_OnLocateTower;
    }

    private void Start()
    {
        _gold = _initialGold;
        OnEnemyEndsAlive?.Invoke(_lives);
        OnGoldChange?.Invoke(_gold);
    }

    private void LoadConfig() 
    {
        var path = System.IO.Path.Combine(Application.streamingAssetsPath, "Config/config.json");

        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            if (!string.IsNullOrEmpty(json))
            {
                var configGame = JsonUtility.FromJson<Config>(json);
                _lives = configGame.game.lives;
                _initialGold = configGame.game.gold;
            }
        }
    }
   
    private void Enemy_OnEnemyReachEnd(EnemyData data)
    {
        _lives = Mathf.Max(0, _lives - data.Damage);
        OnEnemyEndsAlive?.Invoke(_lives);
    }

    private void Enemy_OnEnemyDestroyed(Enemy enemy)
    {
        AddGold(Mathf.RoundToInt(enemy.Data.GoldForDead));
    }

    private void TowerSelection_OnLocateTower(TowerData data)
    {
        if (_gold >= data.initialCost)
        {
            SubstractGold(data.initialCost);
        }
    }

    private void AddGold(int amount)
    {
        _gold += amount;
        OnGoldChange?.Invoke(_gold);
    }

    private void SubstractGold(int amount)
    {
        _gold -= amount;
        OnGoldChange?.Invoke(_gold);
    }

    private void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void SetGameSpeed(float timeSpeed) 
    {
        _gameSpeed = timeSpeed;
        SetTimeScale(_gameSpeed);
    }

    public void Pause()
    {
        SetTimeScale(0f);
    }

    public void Resume()
    {
        SetTimeScale(_gameSpeed);
    }

    public void Mute() 
    {
        var audiosource=Camera.main.GetComponent<AudioSource>();
        audiosource.mute = true;
    }

    public void Volume() 
    {
        var audiosource = Camera.main.GetComponent<AudioSource>();
        audiosource.mute = false;

    }
}
