using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Platform : MonoBehaviour
{
    public static event Action<Platform> OnPlatformClicked;
    public static event Action<Platform> OnPlatformRemoveClicked;
    public static event Action<TowerData> OnRemoveTower;

    [SerializeField] private LayerMask platformLayerMask;

    public bool HasTower => _towerCreated != null;

    private GameObject _towerCreated = null;
    private TowerData _dataActive = null;

    public TowerData DataActive => _dataActive;

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
        _dataActive = data;
    }

    public void RemoveTower()
    {
        Destroy(_towerCreated);
        OnRemoveTower?.Invoke(_dataActive);
        _dataActive = null;
    }
}
