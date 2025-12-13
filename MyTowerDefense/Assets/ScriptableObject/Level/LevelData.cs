 using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public int levelNumber;
    public int wavesNumber;
    public float increaseResources;
    public float increaseLifes;
    public int upgradeNo;
}
