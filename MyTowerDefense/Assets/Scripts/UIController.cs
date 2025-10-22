using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text liveText;

    private void OnEnable()
    {
        Spawner.OnWaveChanged += Spawner_OnWaveChanged;
        GameManager.OnEnemyEndsAlive += GameManager_OnEnemyEndsAlive;
    }
 
    private void OnDisable()
    {
        Spawner.OnWaveChanged -= Spawner_OnWaveChanged;
        GameManager.OnEnemyEndsAlive -= GameManager_OnEnemyEndsAlive;
    }
    private void GameManager_OnEnemyEndsAlive(int life)
    {
        liveText.text = $"Live: {life}";
    }

    private void Spawner_OnWaveChanged(int currentWave) 
    {
        waveText.text = $"Wave: {currentWave}";
    }
}
