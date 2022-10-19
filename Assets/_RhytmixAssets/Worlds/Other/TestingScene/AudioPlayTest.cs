using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayTest : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    public void PlayAudio()
    {
        audio.Play();
    }

    public void StopAudio()
    {
        audio.Stop();
    }
}
