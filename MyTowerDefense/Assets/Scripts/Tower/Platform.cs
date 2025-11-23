using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Platform : MonoBehaviour
{
    public static event Action<Platform> OnPlatformClicked;
    public static event Action<Platform> OnPlatformRemoveClicked;

    [SerializeField] private LayerMask platformLayerMask;

    private bool _hasTower = false;
    public bool HasTower => _hasTower;

    public static bool IsTowerPanelOpened { get; set; } = false;

    private void Update()
    {
        if (IsTowerPanelOpened)
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
        Instantiate(data.prefab, transform.position, Quaternion.identity, transform);
        _hasTower = true;
    }
}
