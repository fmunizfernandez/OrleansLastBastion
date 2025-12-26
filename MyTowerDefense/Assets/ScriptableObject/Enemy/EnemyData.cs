using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int Live;
    public float Speed;
    public int Damage;
    public int GoldForDead;

    [Range(0f, 1f)] public float damageMultiplierBase = 0.10f;
    [Range(0f, 1f)] public float resistanceMultiplierBase = 0.10f;
    [Range(0f, 1f)] public float resistanceMultiplierPerWaveBase = 0.10f;
    [Range(0f, 1f)] public float speedMultiplierBase = 0.10f;

    public float GetDamageMultiplier(int level)
    {
        return 1f + damageMultiplierBase * Mathf.Log(level);
    }

    public float GetSpeedMultiplier(int level)
    {
        return 1f+ speedMultiplierBase * Mathf.Log(level);
    }

    public float GetResistanceMultiplier(int level,int wave)
    {
        var levelMult = 1f + ((level-1) * resistanceMultiplierBase);
        var waveMult = 1f+ Mathf.Log(wave) * resistanceMultiplierBase;

        return levelMult * waveMult;
    }
}
