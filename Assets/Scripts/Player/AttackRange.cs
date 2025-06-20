using UnityEngine;

public class AttackRange : MonoBehaviour
{
    //Attack Range Variables
    private Renderer objectRenderer;
    private GameObject playerCharacter;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        playerCharacter = transform.parent.gameObject;
        SetOpacity(0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            playerCharacter.SendMessage("EnemyEnterAttackRange", other, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            playerCharacter.SendMessage("EnemyExitAttackRange", other, SendMessageOptions.DontRequireReceiver);
        }
    }

    void ShowAttackRange()
    {
        SetOpacity(60/225f);
    }
    
    void HideAttackRange()
    {
        SetOpacity(0f);
    }

    void SetOpacity(float alpha)
    {
        Color color = objectRenderer.material.color;
        color.a = alpha;
        objectRenderer.material.color = color;
    }
}
