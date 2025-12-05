using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// 吹き出し(思考バブル)の表示を制御するクラス
/// キャラクターの上に一定時間メッセージを表示する
/// </summary>
public class BubbleController : MonoBehaviour
{
    /// <summary>
    /// 吹き出しのGameObject
    /// </summary>
    public GameObject bubble;
    
    /// <summary>
    /// キャラクターからの吹き出しのオフセット位置
    /// デフォルトは上方向に2ユニット
    /// </summary>
    public Vector3 offset = new Vector3(0, 2f, 0);
    
    /// <summary>
    /// 吹き出し内のテキストコンポーネント
    /// </summary>
    private TextMeshProUGUI bubbleText;
    
    /// <summary>
    /// 初期化処理
    /// 吹き出しとテキストコンポーネントの存在確認を行う
    /// </summary>
    void Start()
    {
        // 吹き出しが設定されていない場合、スクリプトを無効化
        if(bubble == null)
        {
            this.enabled = false;
            return;
        }
        
        // 吹き出しの子要素からテキストコンポーネントを取得
        bubbleText = bubble.GetComponentInChildren<TextMeshProUGUI>();
        if(bubbleText == null)
        {
            this.enabled = false;
            return;
        }
        
        // 最初は吹き出しを非表示にする
        bubble.SetActive(false);
    }

    /// <summary>
    /// 思考メッセージを指定時間表示する
    /// </summary>
    /// <param name="message">表示するメッセージ</param>
    /// <param name="duration">表示時間(秒)</param>
    public void ShowThought(string message, float duration)
    {
        // 既存のコルーチンを停止
        StopAllCoroutines();
        StartCoroutine(ThoughtSequence(message, duration));
    }

    /// <summary>
    /// 思考表示のシーケンス
    /// メッセージを設定し、指定時間後に非表示にする
    /// </summary>
    /// <param name="message">表示するメッセージ</param>
    /// <param name="duration">表示時間(秒)</param>
    private IEnumerator ThoughtSequence(string message, float duration)
    {
        // テキストを設定して吹き出しを表示
        bubbleText.text = message;
        bubble.SetActive(true); 
        
        // 指定時間待機
        yield return new WaitForSeconds(duration);
        
        // 吹き出しを非表示にする
        bubble.SetActive(false); 
    }
}
