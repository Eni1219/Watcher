using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サウンドデータを保持するシリアライズ可能なクラス
/// </summary>
[System.Serializable]
public class Sound
{
    /// <summary>
    /// サウンドの識別名
    /// </summary>
    public string name;
    
    /// <summary>
    /// 再生するオーディオクリップ
    /// </summary>
    public AudioClip clip;
    
    /// <summary>
    /// 音量(0.0 ~ 1.0)
    /// </summary>
    [Range(0f, 1f)] public float volume=1f;
    
    /// <summary>
    /// ピッチ(0.0 ~ 1.0)
    /// </summary>
    [Range (0f, 1f)]public float pitch = 1f;
    
    /// <summary>
    /// ループ再生するかどうか
    /// </summary>
    public bool loop=false;
    
    /// <summary>
    /// 実際に音を再生するAudioSourceコンポーネント
    /// インスペクターには表示しない
    /// </summary>
    [HideInInspector]public AudioSource source;

}

/// <summary>
/// ゲーム内のすべてのオーディオを管理するシングルトンクラス
/// シーン間で破棄されない
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// シングルトンインスタンス
    /// </summary>
    public static AudioManager instance;
    
    /// <summary>
    /// 管理するサウンドのリスト
    /// </summary>
    public List<Sound> sounds;
    
    /// <summary>
    /// 初期化処理
    /// シングルトンパターンを実装し、各サウンドのAudioSourceを設定
    /// </summary>
    private void Awake()
    {
        // シングルトンの設定
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移時に破棄しない
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // 各サウンドにAudioSourceコンポーネントを追加して設定
        foreach(Sound s in sounds)
        {
            s.source=gameObject.AddComponent<AudioSource>();
            s.source.clip=s.clip;
            s.source.volume=s.volume;
            s.source.pitch=s.pitch;
            s.source.loop=s.loop;
        }
    }
    
    /// <summary>
    /// 指定した名前のサウンドを再生する
    /// </summary>
    /// <param name="name">再生するサウンドの名前</param>
    public void Play(string name)
    {
        Sound s=sounds.Find(sound=>sound.name==name);
        if(s==null)return;
        s.source.Play();
    }
    
    /// <summary>
    /// 指定した名前のサウンドを停止する
    /// </summary>
    /// <param name="name">停止するサウンドの名前</param>
    public void Stop(string name)
    {
        Sound s=sounds.Find (sound=>sound.name==name);
        if(s==null)return;
        s.source.Stop();
    }
    
    /// <summary>
    /// 指定した名前のサウンドが再生中かどうかを確認する
    /// </summary>
    /// <param name="name">確認するサウンドの名前</param>
    /// <returns>再生中ならtrue、それ以外はfalse</returns>
    public bool isPlaying(string name)
    {
        Sound s=sounds.Find(sounds=>sounds.name==name);
        return s!=null&&s.source.isPlaying;
    }
}
