using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Mirrors the player's health values onto the HUD bar and text readout.
public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBarSprite;
    [SerializeField] private TMP_Text healthBarText;

    private void Start()
    {
        // Initialize the display at full health when gameplay begins.
        if (healthBarSprite != null)
        {
            healthBarSprite.fillAmount = 1f;
        }
        if (healthBarText != null)
        {
            healthBarText.text = $"{PlayerStats.Instance.Health} / {PlayerStats.Instance.Health}";
        }
    }

    public void UpdateHealthBar(float healthPercentage)
    {
        healthBarSprite.fillAmount = Mathf.Clamp01(healthPercentage);
        healthBarText.text = $"{PlayerStats.Instance.CurrentHealth} / {PlayerStats.Instance.Health}";

        if (PlayerStats.Instance.CurrentHealth <= 300 && PlayerStats.Instance.CurrentHealth > 100)
        {
            // Yellow tint warns the player they are mid-health.
            healthBarSprite.color = Color.orange;
        }
        else if (PlayerStats.Instance.CurrentHealth <= 100)
        {
            healthBarSprite.color = Color.red;
        }

        if (PlayerStats.Instance.CurrentHealth <= 0)
        {
            healthBarText.text = "0 / 0";
        }
    }
}