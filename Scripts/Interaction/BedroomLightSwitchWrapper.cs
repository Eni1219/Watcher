using UnityEngine;

/// <summary>
/// 寝室の電灯スイッチのラッパークラス
/// タスク進行状態に応じて操作を制限する
/// </summary>
[RequireComponent(typeof(LightSwitch))] 
public class BedroomLightSwitchWrapper : Interactable 
{
    private LightSwitch genericLightSwitch;
    public BubbleController characterBubble;
    public bool defaultIsOn = true;
    
    void Awake()
    {
        genericLightSwitch = GetComponent<LightSwitch>();
        if (genericLightSwitch == null)
        {
            this.enabled = false;
        }
    }

    void Start()
    {
        associatedTask = TaskManager.TaskState.AwaitingOffTheBedroomLight;
        
        // リフレクションを使用して初期状態を設定
        if(genericLightSwitch != null)
        {
            var isOnField = typeof(LightSwitch).GetField("isOn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if(isOnField != null)
                isOnField.SetValue(genericLightSwitch, defaultIsOn);
        }
    }

    public override void OnInteract()
    {
        // タスクが正しい順序でない場合、吹き出しを表示
        if (TaskManager.instance == null || TaskManager.instance.currentTask != associatedTask)
        {
            if (characterBubble != null)
            {
                characterBubble.ShowThought("まだ寝ない", 4f);
            }
        }

        if (genericLightSwitch != null)
        {
            genericLightSwitch.OnInteract();
            TaskManager.instance.CompleteTask(TaskManager.TaskState.AwaitingOffTheBedroomLight);
        }
    }
}