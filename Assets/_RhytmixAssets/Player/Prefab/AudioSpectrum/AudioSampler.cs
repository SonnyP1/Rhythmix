using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSampler : MonoBehaviour
{
    [SerializeField] AudioSource AudioSource;
    public float[] Samples= new float[512];

    private void Update()
    {
        GetSectrumAudioSource();
    }

    void GetSectrumAudioSource()
    {
        AudioSource.GetSpectrumData(Samples, 0,FFTWindow.Blackman);
    }

}
