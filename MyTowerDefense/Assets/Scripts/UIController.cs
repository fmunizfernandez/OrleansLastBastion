using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text liveText;
    [SerializeField] private TMP_Text goldText;

    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject towerMenu;
    [SerializeField] private GameObject towerRemoveMenu;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    [SerializeField] private Button speedButtonx1;
    [SerializeField] private Button speedButtonx2;

    [SerializeField] private Button muteButton;
    [SerializeField] private Button volumeButton;

    [SerializeField] private CanvasGroup gameplayGroup;

    [SerializeField] private GameObject towerMenuPrefab;
    [SerializeField] private Transform towerMenuContainer;
    [SerializeField] private GameObject towerMenuNoResourceText;

    [SerializeField] private Transform towerRemoveButtonContainer;
    [SerializeField] private GameObject towerRemoveDestroyButton;
    [SerializeField] private GameObject towerRemoveEliteButton;
    [SerializeField] private GameObject towerRemoveNoResourceText;

    [SerializeField] private TowerData[] towerMenuData;
    private List<GameObject> _activeSelectors =new List<GameObject>();

    private Platform _activePlatform;

    private const float SPEED_NORMAL = 1f;
    private const float SPEED_FAST = 2f;

    private void Awake()
    {
        levelMenu.SetActive(false);
        towerMenu.SetActive(false);
        towerRemoveMenu.SetActive(false);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        towerMenuNoResourceText.SetActive(false);
        towerRemoveNoResourceText.SetActive(false);
    }

    private void OnEnable()
    {
        Spawner.OnWaveChanged += Spawner_OnWaveChanged;
        Spawner.OnVictory += Spawner_OnVictory;
        LevelManager.OnEnemyEndsAlive += GameManager_OnEnemyEndsAlive;
        LevelManager.OnGoldChange += GameManager_OnGoldChange;
        Platform.OnPlatformClicked += Platform_OnPlatformClicked;
        Platform.OnPlatformRemoveClicked += Platform_OnPlatformRemoveClicked;
        TowerSelection.OnLocateTower += TowerSelection_OnLocateTower;
        TowerRemove.OnRemoveTower += TowerRemove_OnRemoveTower;
        TowerRemove.OnImproveTower += TowerRemove_OnImproveTower;
    }

    private void OnDisable()
    {
        Spawner.OnWaveChanged -= Spawner_OnWaveChanged;
        Spawner.OnVictory -= Spawner_OnVictory;
        LevelManager.OnEnemyEndsAlive -= GameManager_OnEnemyEndsAlive;
        LevelManager.OnGoldChange -= GameManager_OnGoldChange;
        Platform.OnPlatformClicked -= Platform_OnPlatformClicked;
        Platform.OnPlatformRemoveClicked -= Platform_OnPlatformRemoveClicked;
        TowerSelection.OnLocateTower -= TowerSelection_OnLocateTower;
        TowerRemove.OnRemoveTower -= TowerRemove_OnRemoveTower;
    }

    private void Start()
    {
        Volume();
        SpeedNormal();
    }

    #region Events

    private void GameManager_OnEnemyEndsAlive(int life)
    {
        liveText.text = $"Live: {life}";

        if (life <= 0) 
        {
            GameOver();
        }
    }

    private void GameManager_OnGoldChange(int gold)
    {
        goldText.text = $"Gold: {gold}";
    }

    private void Spawner_OnWaveChanged(int currentWave)
    {
        waveText.text = $"Wave: {currentWave}";
    }

    private void Spawner_OnVictory()
    {
        Victory();
    }

    private void Platform_OnPlatformClicked(Platform platform)
    {
        _activePlatform = platform;
        SelectTower();
    }

    private void Platform_OnPlatformRemoveClicked(Platform platform)
    {
        _activePlatform = platform;
        OpenRemoveMenu();
    }

    private void TowerSelection_OnLocateTower(TowerData data)
    {
        if (!_activePlatform.HasTower)
        {
            if (LevelManager.Instance.Gold >= data.initialCost)
            {
                _activePlatform.LocateTower(data);
                CancelSelection();
            }
            else 
            {
                StartCoroutine(ShowNoResourceMessage(towerMenuNoResourceText));
            }
        }
    }

    private void TowerRemove_OnRemoveTower(TowerData data)
    {
        if (_activePlatform.HasTower)
        {
            _activePlatform.RemoveTower();
        }

        CancelRemove();
    }

    private void TowerRemove_OnImproveTower(TowerData data)
    {
        if (_activePlatform.HasTower)
        {
            if (LevelManager.Instance.Gold >= data.ImproveCost)
            {
                _activePlatform.ImproveTower(data);
                CancelRemove();
            }
            else
            {
                StartCoroutine(ShowNoResourceMessage(towerRemoveNoResourceText));
            }
        }
    }
    #endregion

    #region Tower Menu

    private void ShowTowerMenu()
    {
        towerMenu.SetActive(true);
        PopulateTowerSelectionUnits();
    }

    private void HideTowerMenu()
    {
        towerMenu.SetActive(false);
    }

    private void SelectTower()
    {
        ShowTowerMenu();
        Platform.IsTowerPanelOpened = true;
    }

    public void CancelSelection()
    {
        HideTowerMenu();
        Platform.IsTowerPanelOpened = false;
    }

    private void PopulateTowerSelectionUnits() 
    {
        DestroyActiveSelectors();

        foreach (var towerData in towerMenuData) 
        {
            var gameObject = Instantiate(towerMenuPrefab,towerMenuContainer);

            var towerSelectorObj= gameObject.GetComponent<TowerSelection>();
            towerSelectorObj.Inizialite(towerData);

            _activeSelectors.Add(gameObject);
        }
    }

    private IEnumerator ShowNoResourceMessage(GameObject gObj)
    {
        gObj.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        gObj.SetActive(false);
    }

    #endregion

    #region Tower Remove Menu

    private void OpenRemoveMenu()
    {
        PopulateTowerRemoveButtons();
        ShowTowerRemoveMenu();
        Platform.IsTowerRemovePanelOpened = true;
    }

    public void CancelRemove()
    {
        HideTowerRemoveMenu();
        Platform.IsTowerRemovePanelOpened = false;
    }

    private void ShowTowerRemoveMenu()
    {
        towerRemoveMenu.SetActive(true);
    }

    private void HideTowerRemoveMenu()
    {
        towerRemoveMenu.SetActive(false);
    }

    private void PopulateTowerRemoveButtons()
    {
        var destroyButton = towerRemoveDestroyButton.GetComponent<TowerRemove>();
        destroyButton.InitRemove(_activePlatform.ActiveTowerData);

        var eliteButton= towerRemoveEliteButton.GetComponent<TowerRemove>();
        eliteButton.InitImprove(_activePlatform.ActiveTowerData);
        towerRemoveEliteButton.SetActive(!_activePlatform.EndUpgrades);
    }
    #endregion

    #region SpeedButtons

    public void SpeedNormal()
    {
        SetGameSpeed(SPEED_NORMAL);
    }
    
    public void SpeedDouble()
    {
        SetGameSpeed(SPEED_FAST);
    }

    private void SetGameSpeed(float speed)
    {
        LevelManager.Instance.SetGameSpeed(speed);
        UpdateSpeedButtons();
    }

    private void UpdateSpeedButtons()
    {
        TMP_Text textx1 = speedButtonx1.GetComponentInChildren<TMP_Text>();
        if (textx1 != null)
        {
            textx1.fontStyle = (LevelManager.Instance.GameSpeed == 1f) ? FontStyles.Bold : FontStyles.Normal;
        }

        TMP_Text textx2 = speedButtonx2.GetComponentInChildren<TMP_Text>();
        if (textx2 != null)
        {
            textx2.fontStyle = (LevelManager.Instance.GameSpeed == 2f) ? FontStyles.Bold : FontStyles.Normal;
        }
    }

    #endregion

    #region Volume

    public void Mute()
    {
        LevelManager.Instance.Mute();
        muteButton.gameObject.SetActive(false);
        volumeButton.gameObject.SetActive(true);
    }

    public void Volume()
    {
        LevelManager.Instance.Volume();
        muteButton.gameObject.SetActive(true);
        volumeButton.gameObject.SetActive(false);
    }

    #endregion

    #region Pause

    public void Pause()
    {
        if (Platform.IsTowerPanelOpened)
        {
            HideTowerMenu();
        }
        else if (Platform.IsTowerRemovePanelOpened)
        {
            HideTowerRemoveMenu();
        }

        ShowLevelMenu();
        LevelManager.Instance.Pause();
        ManageActionButtons(false);
    }

    public void Resume()
    {
        HideLevelMenu();
        LevelManager.Instance.Resume();
        ManageActionButtons(true);

        if (Platform.IsTowerPanelOpened)
        {
            ShowTowerMenu();
        }
        else if (Platform.IsTowerRemovePanelOpened)
        {
            ShowTowerRemoveMenu();
        }
    }

    public void ShowLevelMenu()
    {
        levelMenu.SetActive(true);
    }

    public void HideLevelMenu()
    {
        levelMenu.SetActive(false);
    }

    private void ManageActionButtons(bool active)
    {
        gameplayGroup.interactable = active;
        gameplayGroup.blocksRaycasts = active;
    }

    public void RestartLevel() 
    {
        LevelManager.Instance.SetGameSpeed(SPEED_NORMAL);
        
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }

    public void MainMenu() 
    {
        LevelManager.Instance.SetGameSpeed(SPEED_NORMAL);
        SceneManager.LoadScene("MainMenu");
    }

    #endregion

    #region Victory/Defeat

    private void GameOver() 
    {
        LevelManager.Instance.SetGameSpeed(0f);
        gameOverPanel.SetActive(true);
        ManageActionButtons(false);
    }

    private void Victory() 
    {
        LevelManager.Instance.SetGameSpeed(0f);
        victoryPanel.SetActive(true);
        ManageActionButtons(false);
    }
    
    public void NextLevel() 
    {
        SceneManager.LoadScene($"Level{GameManager.Instance.GetMaxUnlockedLevel()}");
    }

    #endregion

    private void DestroyActiveSelectors()
    {
        foreach (var selector in _activeSelectors)
        {
            Destroy(selector);
        }

        _activeSelectors.Clear();
    }
}
