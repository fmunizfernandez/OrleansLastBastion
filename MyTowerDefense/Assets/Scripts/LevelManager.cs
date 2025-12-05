using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public static event Action<int> OnEnemyEndsAlive;
    public static event Action<int> OnGoldChange;

    private int _lives;
    private int _initialGold;
    private int _gold = 0;
    private float _gameSpeed;

    [SerializeField] private LevelData data;

    public LevelData Data => data;

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
    }

    private void OnEnable()
    {
        Enemy.OnEnemyReachEnd += Enemy_OnEnemyReachEnd;
        Enemy.OnEnemyDestroyed += Enemy_OnEnemyDestroyed;
        TowerSelection.OnLocateTower += TowerSelection_OnLocateTower;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        Spawner.OnVictory += Spawner_OnVictory;
        Platform.OnRemoveTower += Platform_OnRemoveTower;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyReachEnd -= Enemy_OnEnemyReachEnd;
        Enemy.OnEnemyDestroyed -= Enemy_OnEnemyDestroyed;
        TowerSelection.OnLocateTower -= TowerSelection_OnLocateTower;
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        Spawner.OnVictory -= Spawner_OnVictory;
        Platform.OnRemoveTower -= Platform_OnRemoveTower;
    }

    private void Start()
    {
        _gold = _initialGold;
        OnEnemyEndsAlive?.Invoke(_lives);
        OnGoldChange?.Invoke(_gold);
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

    private void Platform_OnRemoveTower(TowerData data)
    {
        AddGold(Mathf.RoundToInt(data.removeCost));
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        _initialGold = Mathf.RoundToInt(GameManager.Instance.Gold * (1 + Data.increaseResources));
        _lives = Mathf.RoundToInt(GameManager.Instance.Lives * (1 + Data.increaseLifes));
        _gameSpeed = GameManager.Instance.Speed;
    }

    private void Spawner_OnVictory()
    {
        GameManager.Instance.MarkLevelAsPassed(data.levelNumber);
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
        var audiosource = Camera.main.GetComponent<AudioSource>();
        audiosource.mute = true;
    }

    public void Volume()
    {
        var audiosource = Camera.main.GetComponent<AudioSource>();
        audiosource.mute = false;

    }
}
