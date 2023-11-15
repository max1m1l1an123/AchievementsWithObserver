using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // this is a singleton class
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null) { Destroy(this); }
        else { instance = this; }
    }
}
