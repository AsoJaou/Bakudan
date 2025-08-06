using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private GameObject Enemy;
    private RectTransform ImageRect;

    private Vector3 healthOffset = new Vector3(0, 3.5f, 0);

    private void Awake()
    {
        Enemy = transform.parent.gameObject;
        ImageRect = transform.Find("HealthBarFill").GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (Enemy != null)
        {
            Destroy(gameObject);
            return;
        }
    }
}
