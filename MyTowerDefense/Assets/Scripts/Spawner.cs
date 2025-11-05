using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static event Action<int> OnWaveChanged;

    [SerializeField] 
    private WaveData[] waves;
    [SerializeField] 
    private ObjectPooler regularPool;
    [SerializeField] 
    private ObjectPooler fastPool;
    [SerializeField] 
    private ObjectPooler blastPool;
    
    private WaveData CurrentWave => waves[_currentWaveIndex];
    private int _currentWaveIndex = 0;
    private float _spawnTimer=0;
    private int _waveCounter = 0;
    private int _spawnCounter;
    private int _enemiesRemoved;
    private Dictionary<EnemyType, ObjectPooler> _poolDictionary;
    private float _timeBetweenWaves = 1f;
    private float _wavecoolDown;
    private bool _isBetweenWaves = false;

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
        _waveCounter++;
        OnWaveChanged?.Invoke(_waveCounter);
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
            else if(_spawnCounter >= CurrentWave.EnemiesPerWave && _enemiesRemoved >= CurrentWave.EnemiesPerWave)
                EndWave();
        }
    }

    private void GetNewEnemyToSpawn() 
    {
        _spawnTimer = CurrentWave.SpawnInterval;
        SpawnEnemy();
        _spawnCounter++;
    }

    private void EndWave() 
    {
        _isBetweenWaves = true;
        _wavecoolDown = _timeBetweenWaves;
    }

    private void NewWave() 
    {
        _currentWaveIndex = (_currentWaveIndex + 1) % waves.Length;
        _waveCounter++;
        OnWaveChanged?.Invoke(_waveCounter);
        _spawnCounter = 0;
        _enemiesRemoved = 0;
        _spawnTimer = 0f;

        _isBetweenWaves = false;
    }

    private void SpawnEnemy()
    {
        if (!_poolDictionary.TryGetValue(CurrentWave.EnemyType, out var pool))
            return;

        var spawnedObject = pool.GetPooledObject();
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
