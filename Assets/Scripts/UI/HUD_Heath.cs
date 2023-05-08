using Player;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Health : MonoBehaviour
{
    [SerializeField] private Image _healthBarMask;
    [SerializeField] private Image _healthBarMask2;
    [SerializeField] private Image _healthBarMask3;

    public void Start()
    {
        var controller = PlayerController.Instance.GetComponent<PlayerHealthController>();
        _healthBarMask.fillAmount = controller.CurrentHealth / 100f;
        controller.OnHealthChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(int currentHealth)
    {
        _healthBarMask.fillAmount = (float)currentHealth / 100f;
        _healthBarMask2.fillAmount = (float)currentHealth / 100f;
        _healthBarMask3.fillAmount = (float)currentHealth / 100f;
    }
}