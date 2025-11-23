using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text liveText;
    [SerializeField] private TMP_Text goldText;

    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject towerMenu;
    //[SerializeField] private GameObject towerRemoveMenu;

    [SerializeField] private Button sppedButtonx1;
    [SerializeField] private Button sppedButtonx2;

    private Platform _activePlatform;
    private Platform _activeRemovePlatform;


    private void Awake()
    {
        levelMenu.SetActive(false);
        towerMenu.SetActive(false);
        //towerRemoveMenu.SetActive(false);
    }

    private void OnEnable()
    {
        Spawner.OnWaveChanged += Spawner_OnWaveChanged;
        GameManager.OnEnemyEndsAlive += GameManager_OnEnemyEndsAlive;
        GameManager.OnGoldChange += GameManager_OnGoldChange;
        Platform.OnPlatformClicked += Platform_OnPlatformClicked;
        Platform.OnPlatformRemoveClicked += Platform_OnPlatformRemoveClicked;
        TowerSelection.OnLocateTower += TowerSelection_OnLocateTower;
    }

    private void OnDisable()
    {
        Spawner.OnWaveChanged -= Spawner_OnWaveChanged;
        GameManager.OnEnemyEndsAlive -= GameManager_OnEnemyEndsAlive;
        GameManager.OnGoldChange -= GameManager_OnGoldChange;
        Platform.OnPlatformClicked -= Platform_OnPlatformClicked;
        Platform.OnPlatformRemoveClicked -= Platform_OnPlatformRemoveClicked;
        TowerSelection.OnLocateTower -= TowerSelection_OnLocateTower;
    }

    private void Start()
    {
        sppedButtonx1.onClick.AddListener(() => SetGameSpeed(1f)); 
        sppedButtonx2.onClick.AddListener(() => SetGameSpeed(2f));
    }

    #region Events

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

    private void Platform_OnPlatformClicked(Platform platform)
    {
        _activePlatform = platform;
        ShowTowerMenu();
    }

    private void Platform_OnPlatformRemoveClicked(Platform platform)
    {
        //_activeRemovePlatform = platform;
        //ShowTowerRemoveMenu();
    }

    private void TowerSelection_OnLocateTower(TowerData data)
    {
        if (!_activePlatform.HasTower)
        {
            _activePlatform.LocateTower(data);
        }

        HideTowerMenu();
    }

    #endregion

    #region Level Menu

    public void ShowLevelMenu()
    {
        levelMenu.SetActive(true);
        GameManager.Instance.Pause();
    }

    public void HideLevelMenu()
    {
        levelMenu.SetActive(false);
        GameManager.Instance.Resume();
    }
    #endregion

    #region Tower Menu
    public void ShowTowerMenu()
    {
        towerMenu.SetActive(true);
        Platform.IsTowerPanelOpened = true;
    }

    public void HideTowerMenu()
    {
        towerMenu.SetActive(false);
        Platform.IsTowerPanelOpened = false;
    }

    #endregion

    #region Tower Menu
    //public void ShowTowerRemoveMenu()
    //{
    //    towerRemoveMenu.SetActive(true);
    //}

    //public void HideTowerRemoveMenu()
    //{
    //    towerRemoveMenu.SetActive(false);
    //}

    #endregion

    #region SpeedButtons

    private void SetGameSpeed(float speed)
    {
        GameManager.Instance.SetGameSpeed(speed);
    }

    #endregion
}
