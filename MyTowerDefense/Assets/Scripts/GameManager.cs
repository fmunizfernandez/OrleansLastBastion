using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<int> OnEnemyEndsAlive;

    private int _lives = 20;

    private void OnEnable()
    {
        Enemy.OnEnemyReachEnd += Enemy_OnEnemyReachEnd;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyReachEnd -= Enemy_OnEnemyReachEnd;
    }

    private void Start()
    {
        OnEnemyEndsAlive?.Invoke(_lives);
    }

    private void Enemy_OnEnemyReachEnd(EnemyData data)
    {
        _lives = Mathf.Max(0, _lives - data.Damage);
        OnEnemyEndsAlive?.Invoke(_lives);
    }
}
