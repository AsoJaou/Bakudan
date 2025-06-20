using UnityEngine;

public class AttackRange : MonoBehaviour
{
    //Attack Range Variables
    private GameObject playerCharacter;

    private void Start()
    {
        playerCharacter = transform.parent.gameObject;
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
}
