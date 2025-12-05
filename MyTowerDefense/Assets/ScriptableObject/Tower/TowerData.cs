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
    public Sprite sprite;

    public GameObject prefab;

    public int removeCost =>  Mathf.RoundToInt(initialCost * sellPercentage);
}
