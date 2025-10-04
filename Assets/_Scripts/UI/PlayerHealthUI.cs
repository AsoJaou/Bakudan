using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBarSprite;

    private void Start()
    {
        if (healthBarSprite != null)
        {
            healthBarSprite.fillAmount = 1f;
        }
    }

    public void UpdateHealthBar(float healthPercentage)
    {
        Debug.Log("Updating health bar to: " + healthPercentage);
        if (healthBarSprite == null)
        {
            return;
        }

        healthBarSprite.fillAmount = Mathf.Clamp01(healthPercentage);
    }
}