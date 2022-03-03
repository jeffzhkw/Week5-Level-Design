using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public AudioSource Audio;

    public AudioClip Die;

    public static SfxManager sfxInstance;

    private void Awake()
    {
        sfxInstance = this;
        if (sfxInstance != null && sfxInstance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }
}
