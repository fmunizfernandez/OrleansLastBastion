using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Platform : MonoBehaviour
{
    public static event Action<Platform> OnPlatformClicked;

    [SerializeField] private LayerMask platformLayerMask;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) 
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            var raycastHit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, platformLayerMask);
            if (raycastHit.collider != null) 
            {
                var platform = raycastHit.collider.GetComponent<Platform>();
                if (platform != null) 
                {
                    OnPlatformClicked?.Invoke(platform);
                }
            }
        }
    }
}
