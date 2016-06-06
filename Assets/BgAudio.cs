using UnityEngine;
using System.Collections;

public class BgAudio : MonoBehaviour
{
    private static BgAudio instance = null;
    public static BgAudio Instance
    {
        get
        {
            return instance;
        }
    }
    private AudioSource bgAudio;
    //public AudioClip bg;
    public void Play()
    {
        bgAudio.Play();
    }
    public void Stop()
    {
        bgAudio.Stop();
    }
    void Awake()
    {
        bgAudio = GetComponent<AudioSource>();
        instance = this;
    }
}
