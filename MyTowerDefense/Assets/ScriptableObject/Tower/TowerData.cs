using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
public class TowerData : ScriptableObject
{
    public float damage;
    public float range;
    public float shootInterval;
    public float projectilSpeed;
    public float projectilDuration;
    public int initialCost;

    [Range(0f, 1f)] public float sellPercentage = 0.7f;
    [Range(0f, 1f)] public float improvePercentage = 0.15f;
    [Range(0f, 1f)] public float damageMultiplierBase = 0.10f;
    [Range(0f, 1f)] public float rangeMultiplierBase = 0.10f;
    [Range(0f, 1f)] public float shootIntervalMultiplierBase = 0.50f;

    public TowerType towerType;
    public Sprite sprite;
    public GameObject prefab;
    public GameObject elitePrefab;

    public int RemoveCost => Mathf.RoundToInt(initialCost * sellPercentage);
    public int ImproveCost => Mathf.RoundToInt(initialCost * (1 + improvePercentage));

    public float GetDamageMultiplier(int levelFactor, int upgradeCount)
    {
        var levelMultiplier = damageMultiplierBase * Mathf.Log(levelFactor);
        var improveMultiplier = damageMultiplierBase * upgradeCount;

        return (1f + levelMultiplier) * (1f + improveMultiplier);
    }

    public float GetRangeMultiplier(int levelFactor, int upgradeCount)
    {
        var levelMultiplier = rangeMultiplierBase * Mathf.Log(levelFactor);
        var improveMultiplier = rangeMultiplierBase * upgradeCount;

        return (1f + levelMultiplier) * (1f + improveMultiplier);
    }

    public float GetShootIntervalMultiplier(int levelFactor, int upgradeCount)
    {
        var levelMultiplier = shootIntervalMultiplierBase * Mathf.Log(levelFactor);
        var improveMultiplier = shootIntervalMultiplierBase * upgradeCount;

        return (1f + levelMultiplier) * (1f + improveMultiplier);
    }
}
