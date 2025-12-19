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
}
