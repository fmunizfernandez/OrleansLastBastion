using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text liveText;
    [SerializeField] private TMP_Text goldText;

    private void OnEnable()
    {
        Spawner.OnWaveChanged += Spawner_OnWaveChanged;
        GameManager.OnEnemyEndsAlive += GameManager_OnEnemyEndsAlive;
        GameManager.OnEnemyDead += GameManager_OnEnemyDead;
    }

    private void OnDisable()
    {
        Spawner.OnWaveChanged -= Spawner_OnWaveChanged;
        GameManager.OnEnemyEndsAlive -= GameManager_OnEnemyEndsAlive;
        GameManager.OnEnemyDead -= GameManager_OnEnemyDead;
    }

    private void GameManager_OnEnemyEndsAlive(int life)
    {
        liveText.text = $"Live: {life}";
    }

    private void GameManager_OnEnemyDead(int gold)
    {
        goldText.text = $"Gold: {gold}";
    }

    private void Spawner_OnWaveChanged(int currentWave) 
    {
        waveText.text = $"Wave: {currentWave}";
    }
}
