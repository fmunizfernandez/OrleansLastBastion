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
    private float _spawnTimer;
    private int _spawnCounter;
    private int _enemiesRemoved;
    private Dictionary<EnemyType, ObjectPooler> _poolDictionary;
    private float _timeBetweenWaves = 2f;
    private float _wavecoolDown;
    private bool _isBetweenWaves = false;

    private void Awake()
    {
        _poolDictionary = new Dictionary<EnemyType, ObjectPooler>()
        {
            { EnemyType.Orc,regularPool },
            {EnemyType.Dragon,fastPool },
            {EnemyType.Kaiju,blastPool }
        };
    }

    private void Start()
    {
        OnWaveChanged?.Invoke(_currentWaveIndex + 1);
        
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
            {
                _currentWaveIndex = (_currentWaveIndex + 1) % waves.Length;
                OnWaveChanged?.Invoke(_currentWaveIndex + 1);
                _spawnCounter = 0;
                _enemiesRemoved = 0;
                _spawnTimer = 0f;
                _isBetweenWaves = false;
            }
        }
        else 
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0 && _spawnCounter < CurrentWave.EnemiesPerWave)
            {
                _spawnTimer = CurrentWave.SpawnInterval;
                SpawnEnemy();
                _spawnCounter++;
            }
            else if(_spawnCounter >= CurrentWave.EnemiesPerWave && _enemiesRemoved >= CurrentWave.EnemiesPerWave)
            {
                _isBetweenWaves = true;
                _wavecoolDown = _timeBetweenWaves;

            }
        }
    }

    private void SpawnEnemy()
    {
        if (!_poolDictionary.TryGetValue(CurrentWave.EnemyType, out var pool))
            return;

        var spawnedObject = pool.GetPooledObject();
        spawnedObject.transform.position = transform.position;
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
