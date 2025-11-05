using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text liveText;
    [SerializeField] private TMP_Text goldText;

    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject towerMenu;

    private void Awake()
    {
        levelMenu.SetActive(false);
        towerMenu.SetActive(false); 
    }

    private void OnEnable()
    {
        Spawner.OnWaveChanged += Spawner_OnWaveChanged;
        GameManager.OnEnemyEndsAlive += GameManager_OnEnemyEndsAlive;
        GameManager.OnGoldChange += GameManager_OnGoldChange;
        Platform.OnPlatformClicked += Platform_OnPlatformClicked;
    }

    private void OnDisable()
    {
        Spawner.OnWaveChanged -= Spawner_OnWaveChanged;
        GameManager.OnEnemyEndsAlive -= GameManager_OnEnemyEndsAlive;
        GameManager.OnGoldChange -= GameManager_OnGoldChange;
        Platform.OnPlatformClicked -= Platform_OnPlatformClicked;
    }

    private void GameManager_OnEnemyEndsAlive(int life)
    {
        liveText.text = $"Live: {life}";
    }
    private void GameManager_OnGoldChange(int gold)
    {
        goldText.text = $"Gold: {gold}";
    }

    private void Spawner_OnWaveChanged(int currentWave) 
    {
        waveText.text = $"Wave: {currentWave}";
    }

    private void Platform_OnPlatformClicked(Platform obj)
    {
        ShowTowerMenu();
    }

    public void ShowLevelMenu() 
    {
        levelMenu.SetActive(true);
        GameManager.Instance.SetTimeScale(0f);
    }

    public void HideLevelMenu() 
    {
        levelMenu.SetActive(false);
        GameManager.Instance.SetTimeScale(1f);
    }

    public void ShowTowerMenu()
    {
        towerMenu.SetActive(true);
    }

    public void HideTowerMenu()
    {
        towerMenu.SetActive(false);
    }
}
