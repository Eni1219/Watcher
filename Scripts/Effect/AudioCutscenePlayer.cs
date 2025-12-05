using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume=1f;
    [Range (0f, 1f)]public float pitch = 1f;
    public bool loop=false;
    [HideInInspector]public AudioSource source;
}

/// <summary>
/// ゲーム内のすべてのオーディオを管理するシングルトンクラス
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<Sound> sounds;
    
    private void Awake()
    {
        // シングルトンの設定
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // 各サウンドにAudioSourceコンポーネントを追加
        foreach(Sound s in sounds)
        {
            s.source=gameObject.AddComponent<AudioSource>();
            s.source.clip=s.clip;
            s.source.volume=s.volume;
            s.source.pitch=s.pitch;
            s.source.loop=s.loop;
        }
    }
    
    public void Play(string name)
    {
        Sound s=sounds.Find(sound=>sound.name==name);
        if(s==null)return;
        s.source.Play();
    }
    
    public void Stop(string name)
    {
        Sound s=sounds.Find (sound=>sound.name==name);
        if(s==null)return;
        s.source.Stop();
    }
    
    public bool isPlaying(string name)
    {
        Sound s=sounds.Find(sounds=>sounds.name==name);
        return s!=null&&s.source.isPlaying;
    }
}
