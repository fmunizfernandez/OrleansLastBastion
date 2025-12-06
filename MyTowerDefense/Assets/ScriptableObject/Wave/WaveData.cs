using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnemyGroup
{
    public EnemyData enemyType;
    public int count;            
}

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    public EnemyGroup[] EnemyGroupPerWave;
    public float SpawnInterval;

    public int EnemiesPerWave
    {
        get
        {
            return EnemyGroupPerWave.Sum(s => s.count);
        }
    }
}
