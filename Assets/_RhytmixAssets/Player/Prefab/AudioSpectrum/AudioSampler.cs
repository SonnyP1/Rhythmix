using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSampler : MonoBehaviour
{
    private AudioSource _audioSource;
    public float[] GetSampler()
    {
        return _samples;
    }
    private float[] _samples= new float[512];
    public float[] GetSampleBuffer()
    {
        return _sampleBuffer;
    }
    private  float[] _sampleBuffer = new float[512];

    private float[] _sampleDecrease = new float[512];


    private void Start()
    {
        _audioSource = FindObjectOfType<CoreGameDataHolder>().GetMusic();
    }

    private void Update()
    {
        GetSectrumAudioSource();
        BandBuffer();
    }

    void GetSectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0,FFTWindow.Blackman);
    }

    void BandBuffer()
    {
        for(int g = 0; g < 512;++g)
        {
            if (_samples[g] > _sampleBuffer[g])
            {
                _sampleBuffer[g] = _samples[g];
                _sampleDecrease[g] = 0.0005f;
            }

            if (_samples[g] < _sampleBuffer[g])
            {
                _sampleBuffer[g] -= _sampleDecrease[g];
                _sampleDecrease[g] *= 1.2f;
            }

        }
    }
}
