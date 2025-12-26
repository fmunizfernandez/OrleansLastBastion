using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnemyPool
{
    public EnemyData enemyType;
    public ObjectPooler pool;
}

public class Spawner : MonoBehaviour
{
    public static event Action<int> OnWaveChanged;
    public static event Action OnVictory;

    public static event Action<float> OnWaveCountdown;
    public static event Action OnWaveCountdownFinished;

    public static event Action<int, int> OnWaveEnemyProgress;

    [SerializeField] private EnemyPool[] enemyPools;

    [SerializeField] private WaveData[] waves;

    private WaveData CurrentWave => waves[_currentWaveIndex];
    private int _currentWaveIndex = 0;
    private float _spawnTimer = 0;
    private int _waveCounter;
    private int _spawnCounter;
    private int _enemiesRemoved;
    private float _timeBetweenWaves = 7.5f;
    private float _wavecoolDown;
    private float _firstWavecoolDown;

    private int _currentGroupIndex;
    private int _spawnedInCurrentGroup;

    private bool _isBetweenWaves;
    private bool _isFirstWave;
    private bool _isVictory;

    private void Start()
    {
        _isFirstWave = true;
        _isBetweenWaves = false;
        _isVictory = false;
        _waveCounter = 0;

        _firstWavecoolDown = _timeBetweenWaves;
        OnWaveChanged?.Invoke(_waveCounter + 1);
    }

    private void OnEnable()
    {
        Enemy.OnEnemyReachEnd += Enemy_OnEnemyReachEnd;
        Enemy.OnEnemyDestroyed += Enemy_OnEnemyDestroyed;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyReachEnd -= Enemy_OnEnemyReachEnd;
        Enemy.OnEnemyDestroyed -= Enemy_OnEnemyDestroyed;
    }

    void Update()
    {
        if (_isVictory) return;

        if (_isFirstWave)
        {
            _firstWavecoolDown -= Time.deltaTime;

            OnWaveCountdown?.Invoke(_firstWavecoolDown);

            if (_firstWavecoolDown <= 0f)
            {
                NewWave();
                OnWaveCountdownFinished?.Invoke();
                _isFirstWave = false;
            }
            return;
        }

        if (_isBetweenWaves)
        {
            if (_waveCounter + 1 > LevelManager.Instance.Data.wavesNumber)
            {
                _isVictory = true;
                _isBetweenWaves = false;
                OnVictory?.Invoke();
                return;
            }

            _wavecoolDown -= Time.deltaTime;

            OnWaveCountdown?.Invoke(Mathf.Max(0f, _wavecoolDown));

            if (_wavecoolDown <= 0f)
            {
                NewWave();
                OnWaveCountdownFinished?.Invoke();
            }
        }
        else
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0 && _spawnCounter < CurrentWave.EnemiesPerWave)
                GetNewEnemyToSpawn();
            else if (_spawnCounter >= CurrentWave.EnemiesPerWave && _enemiesRemoved >= CurrentWave.EnemiesPerWave)
                EndWave();
        }
    }

    private void GetNewEnemyToSpawn()
    {
        if (_spawnedInCurrentGroup >= CurrentWave.EnemyGroupPerWave[_currentGroupIndex].count)
        {
            _currentGroupIndex++;
            _spawnedInCurrentGroup = 0;
        }

        SpawnEnemy();

        _spawnedInCurrentGroup++;
        _spawnCounter++;
        _spawnTimer = CurrentWave.SpawnInterval;
    }

    private void EndWave()
    {
        _isBetweenWaves = true;
        _wavecoolDown = _timeBetweenWaves;
    }

    private void NewWave()
    {
        _currentWaveIndex = (_isFirstWave) ? _waveCounter : _currentWaveIndex + 1;
        _waveCounter++;
        OnWaveChanged?.Invoke(_waveCounter);

        _spawnCounter = 0;
        _enemiesRemoved = 0;
        _spawnTimer = 0f;
        _currentGroupIndex = 0;
        _spawnedInCurrentGroup = 0;

        _isBetweenWaves = false;
        OnWaveEnemyProgress?.Invoke(_enemiesRemoved, CurrentWave.EnemiesPerWave);
    }

    private void SpawnEnemy()
    {
        var group = CurrentWave.EnemyGroupPerWave[_currentGroupIndex];

        var enemyPool = enemyPools.First(p => p.enemyType == group.enemyType).pool;
        var spawnedObject = enemyPool.GetPooledObject();
        spawnedObject.transform.position = transform.position;

        var enemy = spawnedObject.GetComponent<Enemy>();
        enemy.Initialize(LevelManager.Instance.Data.levelNumber, _waveCounter);
        spawnedObject.SetActive(true);
    }

    private void Enemy_OnEnemyReachEnd(int damage)
    {
        _enemiesRemoved++;
        OnWaveEnemyProgress?.Invoke(_enemiesRemoved, CurrentWave.EnemiesPerWave);
    }

    private void Enemy_OnEnemyDestroyed(Enemy enemy)
    {
        _enemiesRemoved++;
        OnWaveEnemyProgress?.Invoke(_enemiesRemoved, CurrentWave.EnemiesPerWave);
    }
}
