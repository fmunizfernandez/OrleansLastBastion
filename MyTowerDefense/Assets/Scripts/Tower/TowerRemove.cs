using System;
using TMPro;
using UnityEngine;

public class TowerRemove : MonoBehaviour
{
    [SerializeField] private TMP_Text costText;

    public static event Action<TowerData> OnRemoveTower;
    
    private void OnEnable()
    {
        //costText.text = data.initialCost.ToString();
    }

    public void RemoveTower()
    {
        //OnRemoveTower?.Invoke(data);
    }
}
