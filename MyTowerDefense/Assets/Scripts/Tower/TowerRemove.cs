using System;
using TMPro;
using UnityEngine;

public class TowerRemove : MonoBehaviour
{
    [SerializeField] private TMP_Text costText;

    public static event Action<TowerData> OnRemoveTower;
    public static event Action<TowerData> OnImproveTower;

    private TowerData _data;
    
    public void RemoveTower()
    {
        OnRemoveTower?.Invoke(_data);
    }

    public void ImproveTower() 
    {
        OnImproveTower?.Invoke(_data);
    }

    public void InitRemove(TowerData data)
    {
        _data = data;
        costText.text = data.RemoveCost.ToString();
    }

    public void InitImprove(TowerData data)
    {
        _data = data;
        costText.text = data.ImproveCost.ToString();
    }
}
