using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [Header("Video Clips")]
    [SerializeField] VideoClip clipLoop;
    private VideoPlayer _videoPlayer;
    private bool isSwitch = false;

    [Header("Childs")]
    [SerializeField] bool bHasChild = false;
    [SerializeField] GameObject[] childerens;

    public void StartVideo()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.Play();
        _videoPlayer.loopPointReached += SwitchVideo;

        if (bHasChild)
        {
            foreach (GameObject obj in childerens)
            {
                obj.SetActive(false);
            }
        }
    }


    private void SwitchVideo(VideoPlayer source)
    {
        if(isSwitch == false)
        {
            _videoPlayer.clip = clipLoop;
            isSwitch = true;
            _videoPlayer.Play();
            if(bHasChild)
            {
                foreach(GameObject obj in childerens)
                {
                    obj.SetActive(true);
                }
            }
        }
    }

}
