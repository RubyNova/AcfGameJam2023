using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerHealthController = Player.PlayerHealthController;

public class HUD_Health : MonoBehaviour
{

    [SerializeField] private Image _healthBarMask;

    private void HandleHealthChanged(int currentHealth)
    {
        _healthBarMask.fillAmount = (float)currentHealth / 100f;
    }
}