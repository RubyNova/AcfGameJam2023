using System;
using UnityEngine;
using TMPro;

namespace Player
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI _lightItemsText;
        [SerializeField] public TextMeshProUGUI _heavyItemsText;
        private int _lightItems;
        private int _heavyItems;

        public int LightItems
        {
            get => _lightItems;
            set
            {
                if (_lightItems ==  value)
                {
                    return;
                }

                _lightItems = value;

                if (_lightItems <= 0)
                {
                    _lightItems = 0;
                }

                LightItemsAmountChanged?.Invoke(_lightItems);
                LightItemsAmountChanged?.Invoke(UpdateText(_lightItems));
            }
        }

        public int HeavyItems 
        {
            get => _heavyItems;
            set 
            {
                if (_heavyItems == value)
                {
                    return;
                }

                _heavyItems = value;
                
                if (_heavyItems <= 0)
                {
                    _heavyItems = 0;
                }

                HeavyItemsAmountChanged?.Invoke(_heavyItems);
                HeavyItemsAmountChanged?.Invoke(UpdateText(_heavyItems));
            }
        }

        public event Action<int> LightItemsAmountChanged;
        public event Action<int> HeavyItemsAmountChanged;
        
        public void UpdateText(int _heavyItemsText, int _lightItemsText)
        {
            _lightItemsText = _lightItems;
            _heavyItemsText = _heavyItems;
        }
    }
}
