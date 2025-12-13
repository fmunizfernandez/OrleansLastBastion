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
    [Range(0f, 1f)] public float shootIntervalMultiplierBase = 0.10f;

    public Sprite sprite;
    public GameObject prefab;
    public GameObject elitePrefab;

    public int RemoveCost =>  Mathf.RoundToInt(initialCost * sellPercentage);
    public int ImproveCost => Mathf.RoundToInt(initialCost * (1+improvePercentage));

    //TODO: Mejorar el multiplier
    public float GetDamageMultiplier(int upgradeCount)
    {
        return damageMultiplierBase* Mathf.Log(upgradeCount+ 1);
    }

    public float GetRangeMultiplier(int upgradeCount)
    {
        return rangeMultiplierBase* Mathf.Log(upgradeCount + 1);
    }

    public float GetShootIntervalMultiplier(int upgradeCount)
    {
        return shootIntervalMultiplierBase * Mathf.Log(upgradeCount + 1);
    }
}
