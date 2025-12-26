 using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public int levelNumber;
    public int wavesNumber;
    [Range(0f, 1f)] public float increaseResources;
    [Range(0f, 1f)] public float increaseLifes;
    public int upgradeNo;

    public float GetResouceMultiplier()
    {
        var levelMultiplier = increaseResources * levelNumber;
        return (1f + levelMultiplier);
    }

    public float GetLifeMultiplier()
    {
        var levelMultiplier = increaseLifes * levelNumber;
        return (1f + levelMultiplier);
    }
}
