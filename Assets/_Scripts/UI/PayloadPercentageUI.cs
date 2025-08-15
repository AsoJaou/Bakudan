using UnityEngine;
using UnityEngine.UI;

public class PayloadPercentageUI : MonoBehaviour
{

    [SerializeField] private Image payloadBarSprite;

    public void UpdatePayloadBar(float currentPayloadPercentage)
    {
        payloadBarSprite.fillAmount = currentPayloadPercentage;
    }
}
