using System;
using System.Collections.Generic;
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

    [SerializeField] private EnemyPool[] enemyPools;

    [SerializeField] private WaveData[] waves;
    [SerializeField] private ObjectPooler regularPool;
    [SerializeField] private ObjectPooler fastPool;
    [SerializeField] private ObjectPooler blastPool;

    private WaveData CurrentWave => waves[_currentWaveIndex];
    private int _currentWaveIndex = 0;
    private float _spawnTimer = 0;
    private int _waveCounter = 0;
    private int _spawnCounter;
    private int _enemiesRemoved;
    private Dictionary<EnemyType, ObjectPooler> _poolDictionary;
    private float _timeBetweenWaves = 7.5f;
    private float _wavecoolDown;
    private float _firstWavecoolDown;
    private bool _isBetweenWaves = false;
    private bool _isFirstWave = true;

    private int _currentGroupIndex;
    private int _spawnedInCurrentGroup;

    private bool _isVictory = false;

    private void Awake()
    {
        _poolDictionary = new Dictionary<EnemyType, ObjectPooler>()
        {
            { EnemyType.Regular,regularPool },
            { EnemyType.Fast,fastPool },
            { EnemyType.Blast,blastPool }
        };
    }

    private void Start()
    {
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
        if (_isFirstWave)
        {
            _firstWavecoolDown -= Time.deltaTime;
            if (_firstWavecoolDown <= 0f)
            {
                NewWave();
                _isFirstWave = false;
            }
            return;
        }

        if (_isBetweenWaves)
        {
            _wavecoolDown -= Time.deltaTime;
            if (_wavecoolDown <= 0f)
                NewWave();
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
        if (_waveCounter + 1 > LevelManager.Instance.Data.wavesNumber)
        {
            if (!_isVictory)
            {
                _isVictory = true;
                OnVictory?.Invoke();
                return;
            }
        }

        _currentWaveIndex = (_isFirstWave) ? _waveCounter : (_currentWaveIndex + 1) % waves.Length;
        _waveCounter++;
        OnWaveChanged?.Invoke(_waveCounter);
        
        _spawnCounter = 0;
        _enemiesRemoved = 0;
        _spawnTimer = 0f;
        _currentGroupIndex = 0;
        _spawnedInCurrentGroup = 0;

        _isBetweenWaves = false;
    }

    private void SpawnEnemy()
    {
        var group = CurrentWave.EnemyGroupPerWave[_currentGroupIndex];

        var pool = enemyPools.First(p => p.enemyType == group.enemyType).pool;
        GameObject spawnedObject = pool.GetPooledObject();
        spawnedObject.transform.position = transform.position;

        var healthMultiplier = 1f + (_waveCounter * 0.1f);
        var enemy = spawnedObject.GetComponent<Enemy>();   
        enemy.Initialize(healthMultiplier);
        spawnedObject.SetActive(true);
    }

    private void Enemy_OnEnemyReachEnd(EnemyData data)
    {
        _enemiesRemoved++;
    }

    private void Enemy_OnEnemyDestroyed(Enemy enemy)
    {
        _enemiesRemoved++;
    }
}
