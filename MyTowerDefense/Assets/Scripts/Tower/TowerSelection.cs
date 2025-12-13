using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelection : MonoBehaviour
{
    public static event Action<TowerData> OnLocateTower;

    [SerializeField] private Image towerImage;
    [SerializeField] private TMP_Text costText;

    private TowerData _towerData;

    public void Inizialite(TowerData data)
    {
        _towerData = data;

        towerImage.sprite = data.sprite;
        costText.text = data.initialCost.ToString();
    }

    public void LocateTower()
    {
        OnLocateTower?.Invoke(_towerData);
    }
}
