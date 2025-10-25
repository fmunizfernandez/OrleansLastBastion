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
    public float incrementCost;
}
