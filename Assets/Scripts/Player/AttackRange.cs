using UnityEngine;

public class AttackRange : MonoBehaviour
{
    //Attack Range Variables
    private GameObject playerCharacter;

    private void Awake()
    {
        playerCharacter = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            Debug.Log(other.gameObject + " has entered the attack range");
        }
    }
}
