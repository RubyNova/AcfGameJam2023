using UnityEngine;
using UnityEngine.Events;
using Player;
using TMPro;

public class HUDInventoryDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _lightItemsText;
    [SerializeField] private TMPro.TextMeshProUGUI _heavyItemsText;
    
    [SerializeField] private Inventory _inventory;
    
    private void Start()
    {
        _lightItemsText.text = "0";
        _heavyItemsText.text = "0";
        _inventory.LightItemsAmountChanged += UpdateLightItems;
        _inventory.HeavyItemsAmountChanged += UpdateHeavyItems;
    }

    private void UpdateLightItems(int amount)
    {
        _lightItemsText.text = amount.ToString();
    }
    
    private void UpdateHeavyItems(int amount)
    {
        _heavyItemsText.text = amount.ToString();
    }
    
}
