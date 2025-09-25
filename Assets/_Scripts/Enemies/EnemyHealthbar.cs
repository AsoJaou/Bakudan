using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] private Image healthBarSprite;

    public void UpdateHealthbar(float currentHealthPercentage)
    {
        healthBarSprite.fillAmount = currentHealthPercentage;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
    }
}