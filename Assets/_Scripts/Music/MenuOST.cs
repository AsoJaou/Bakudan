using UnityEngine;

public class MenuOST : MonoBehaviour
{
    public AudioSource music;
    public float loopEndTime;

    void Update()
    {
        if (music.time >= loopEndTime)
            music.time = 0f;
    }
}
