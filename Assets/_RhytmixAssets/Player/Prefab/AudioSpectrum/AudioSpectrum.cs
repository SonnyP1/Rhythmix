using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    [SerializeField] GameObject _sampleGameObject;
    [SerializeField] float _maxScale;
    [SerializeField] AudioSampler _audioSampler;
    GameObject[] _sampleObjects = new GameObject[512];

    private void Start()
    {
        for (int i = 0; i < 512; i++)
        {
            GameObject instance = (GameObject)Instantiate(_sampleGameObject);
            instance.transform.position = this.transform.position;
            instance.transform.parent = this.transform;
            instance.name = "sampleObject " + i;
            //this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            instance.transform.position = new Vector3(instance.transform.position.x,0,-i);
            _sampleObjects[i] = instance;
        }
    }

    private void Update()
    {
        for (int i = 0; i < 512; i++)
        {
            if (_sampleObjects != null)
            {
                _sampleObjects[i].transform.localScale = new Vector3(1, (_audioSampler.Samples[i] * _maxScale)+2, 1);
            }
        }
    }
}
