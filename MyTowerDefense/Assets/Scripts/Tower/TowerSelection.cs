using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelection : MonoBehaviour
{
    [SerializeField] private Image towerImage;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TowerData data;

    public static event Action<TowerData> OnLocateTower;

    private void OnEnable()
    {
        towerImage.sprite = data.sprite;
        costText.text = data.initialCost.ToString();
    }

    public void LocateTower() 
    {
        if (LevelManager.Instance.Gold >= data.initialCost) 
        {
            OnLocateTower?.Invoke(data);
        }
    }
}
