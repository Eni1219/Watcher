using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 電灯スイッチを管理するクラス
/// ライトのオン/オフ、ナイトビジョンモード、パスワード番号の表示を制御する
/// </summary>
public class LightSwitch : Interactable
{
    public Light[] targetLights;
    public Volume globalVolume;
    private ColorAdjustments colorAdjustments;
    public GameObject[] passwordNumbers;
    public Material monitorMaterial;
    private bool isOn = false; 

    void Start()
    {
        if (globalVolume != null && globalVolume.profile.TryGet(out colorAdjustments))
            UpdateLightAndMonitor();
        
        // パスワード番号を最初は非表示にする
        if(passwordNumbers!=null)
        {
            foreach(GameObject number in passwordNumbers)
            {
                if(number != null) 
                    number.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ライトのオン/オフを切り替え、タスクを完了させる
    /// </summary>
    public override void OnInteract()
    {
        isOn = !isOn;
        UpdateLightAndMonitor();
        TaskManager.instance.CompleteTask(TaskManager.TaskState.AwaitingLightSwitch);
    }

    /// <summary>
    /// ライト、モニター、視覚効果を更新する
    /// オン時は通常モード、オフ時はナイトビジョンモードに切り替える
    /// </summary>
    void UpdateLightAndMonitor()
    {
        if (targetLights == null || monitorMaterial == null) return;

        foreach (Light light in targetLights)
        {
            if (light != null)
                light.enabled = isOn;
        }

        if (isOn)
        {
            // ライトオン時: 通常モードに戻す
            AudioManager.instance.Play("LightSwitchOn");
            monitorMaterial.DisableKeyword("_NIGHT_VISION_ON");
            if (colorAdjustments != null)
                colorAdjustments.saturation.value = 0f;
            Debug.Log("NormalMode");
            
            if (passwordNumbers != null)
            {
                foreach (GameObject number in passwordNumbers)
                {
                    if (number != null)
                        number.SetActive(false);
                }
            }
        }
        else
        {
            // ライトオフ時: ナイトビジョンモードに切り替え
            AudioManager.instance.Play("LightSwitchOff");
            monitorMaterial.EnableKeyword("_NIGHT_VISION_ON");
            if (colorAdjustments != null)
                colorAdjustments.saturation.value = -100f;
            Debug.Log("MonoMode");
            
            // パスワード番号を表示
            if (passwordNumbers != null)
            {
                foreach (GameObject number in passwordNumbers)
                {
                    if (number != null)
                        number.SetActive(true);
                }
            }
        }
    }
}

