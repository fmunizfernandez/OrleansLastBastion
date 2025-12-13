using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Platform : MonoBehaviour
{
    public static event Action<Platform> OnPlatformClicked;
    public static event Action<Platform> OnPlatformRemoveClicked;

    [SerializeField] private LayerMask platformLayerMask;

    public bool HasTower => _towerCreated != null;

    private GameObject _towerCreated = null;
    private TowerData _activeTowerData = null;
    private bool _endUpgrades = false;
    private int _upgradeCount=0;

    public TowerData ActiveTowerData => _activeTowerData;
    public bool EndUpgrades=> _endUpgrades;
    public int Upgrade => _upgradeCount;

    public static bool IsTowerPanelOpened { get; set; } = false;
    public static bool IsTowerRemovePanelOpened { get; set; } = false;

    private void Update()
    {
        if (IsTowerPanelOpened || IsTowerRemovePanelOpened || Time.timeScale == 0f)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            var raycastHit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, platformLayerMask);
            if (raycastHit.collider != null)
            {
                var platform = raycastHit.collider.GetComponent<Platform>();
                if (platform != null)
                {
                    if (platform.HasTower)
                    {
                        OnPlatformRemoveClicked?.Invoke(platform);
                    }
                    else
                    {
                        OnPlatformClicked?.Invoke(platform);
                    }
                }
            }
        }
    }

    public void LocateTower(TowerData data)
    {
        _towerCreated = Instantiate(data.prefab, transform.position, Quaternion.identity, transform);
        _activeTowerData = data;
        _endUpgrades = _upgradeCount >= LevelManager.Instance.MaxUpgradeNo;

        var tower = _towerCreated.GetComponent<Tower>();
        tower.SetUpgrade(_upgradeCount);
    }

    public void RemoveTower()
    {
        Destroy();
        _upgradeCount = 0;
    }

    internal void ImproveTower(TowerData data)
    {
        Destroy();

        _towerCreated = Instantiate(data.elitePrefab, transform.position, Quaternion.identity, transform);
        _activeTowerData = data;
        _upgradeCount++;
        _endUpgrades = _upgradeCount>=LevelManager.Instance.MaxUpgradeNo;

        var tower = _towerCreated.GetComponent<Tower>();
        tower.SetUpgrade(_upgradeCount);
    }

    private void Destroy()
    {
        Destroy(_towerCreated);

        _towerCreated = null;
        _activeTowerData = null;
        _endUpgrades = false;
    }
}
