using UnityEngine;
using UnityEngine.Video;

public class TVController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public bool isPlaying=>videoPlayer.isPlaying;
    
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }
    
    public void PlayBroadCast()
    {
        if(videoPlayer!=null&&!videoPlayer.isPlaying)
            videoPlayer.Play();
    }
    
    public void StopBroadCast()
    {
        if(videoPlayer!=null&&videoPlayer.isPlaying)
            videoPlayer.Stop();
    }
}
