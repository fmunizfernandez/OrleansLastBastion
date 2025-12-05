using System;
using TMPro;
using UnityEngine;

public class TowerRemove : MonoBehaviour
{
    [SerializeField] private TMP_Text costText;

    public static event Action OnRemoveTower;

    private TowerData _data;

    private void OnEnable()
    {
        costText.text = "100";
    }

    public void RemoveTower()
    {
        OnRemoveTower?.Invoke();
    }
}
