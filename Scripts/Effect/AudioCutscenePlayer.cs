using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioCutscenePlayer : MonoBehaviour
{
    public AudioClip audioClip;
    public string nextSceneName = "Main";
    private AudioSource audioSource;
    void Start()
    {
     audioSource=GetComponent<AudioSource>();
        if(audioSource == null )
        {
            LoadNextScene();
                return;
        }
        audioSource.playOnAwake = false;
        audioSource.clip = audioClip;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.time > 0 && !audioSource.isPlaying)
            LoadNextScene();
    }
    void LoadNextScene()
    {
        this.enabled = false;
        SceneManager.LoadScene(nextSceneName);
    }
}
