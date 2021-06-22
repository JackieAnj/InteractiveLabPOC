using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public string url;
    VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.url = url;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("t")) {
            Play();
        }
    }

    void Play() {
        videoPlayer.Play();
        videoPlayer.isLooping = true;
    }
}
