using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerHealthController = Player.PlayerHealthController;

public class HUD_Health : MonoBehaviour
{

    [SerializeField] private Image _healthBarMask;
    [SerializeField] private Image _healthBarMask2;
    [SerializeField] private Image _healthBarMask3;
    
    [SerializeField] private PlayerHealthController _playerHealthController;
    
    public void Start()
    {
        _healthBarMask.fillAmount = (float)_playerHealthController.CurrentHealth / 100f;
        _playerHealthController.OnHealthChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(int currentHealth)
    {
        _healthBarMask.fillAmount = (float)currentHealth / 100f;
        _healthBarMask2.fillAmount = (float)currentHealth / 100f;
        _healthBarMask3.fillAmount = (float)currentHealth / 100f;
    }
}