using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    public GameObject Menu;

    public void ShowControls()
    {
        Menu.SetActive(false);
    }

    public void HideControls()
    {
        Menu.SetActive(true);
    }
}
