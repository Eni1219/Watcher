using UnityEngine;

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
        if(genericLightSwitch != null)
        {
            var isOnField = typeof(LightSwitch).GetField("isOn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if(isOnField != null)
                isOnField.SetValue(genericLightSwitch,defaultIsOn);
          
        }
    }

    public override void OnInteract()
    {
        if (TaskManager.instance == null || TaskManager.instance.currentTask != associatedTask)
        {

            if (characterBubble != null)
            {
                characterBubble.ShowThought("まだねない", 4f);
            }
        }

        if (genericLightSwitch != null)
        {
            genericLightSwitch.OnInteract();
            TaskManager.instance.CompleteTask(TaskManager.TaskState.AwaitingOffTheBedroomLight);
        }
    }
}