using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance;
    public Material monitorEffectMaterial;
    public GameObject pressIUI;
    public enum TaskState
    {
        AwaitingLightSwitch,
        AwaitingRest,
        AwaitingShower,
        AwaitingOffTheBedroomLight,
        AwaitingSleep,
        AllTasksCompleted
    
    }
    public TaskState currentTask;
    public WorldSwitcher worldSwitcher;

    public BubbleController characterBubble;
    private void Awake()
    {
        if(instance==null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        pressIUI.SetActive(false);
        currentTask=TaskState.AwaitingLightSwitch;
        if (characterBubble != null)
        {
            characterBubble.ShowThought("To black", 4f);
        }
        Debug.Log("Task Manager: now task -> " + currentTask);
    }
    public void CompleteTask(TaskState completedTask)
    {
        if (completedTask != currentTask) return;
        switch(completedTask)
        {
            case TaskState.AwaitingLightSwitch:
                currentTask = TaskState.AwaitingRest;
                Debug.Log("Task Manager: now task -> " + currentTask);
                if (characterBubble != null)
                {
                    characterBubble.ShowThought("疲れたな、ソファで少し座るか", 4f);
                }
                break;
                case TaskState.AwaitingRest:
                currentTask=TaskState.AwaitingShower;
                Debug.Log("Task Manager: now task -> " + currentTask);
                if (characterBubble != null)
                {
                    characterBubble.ShowThought("シャワーしようか", 4f);
                }
                break;
                case TaskState.AwaitingShower:
                currentTask=TaskState.AwaitingOffTheBedroomLight;
                Debug.Log("Task Manager: now task -> " + currentTask);
                if (characterBubble != null)
                {
                    characterBubble.ShowThought("寝る前に部屋の電気消さな", 4f);
                }
                break; 
                case TaskState.AwaitingOffTheBedroomLight:
                currentTask=TaskState.AwaitingSleep;
                Debug.Log("Task Manager: now task -> " + currentTask);
                if (characterBubble != null)
                {
                    characterBubble.ShowThought("そろそろねるわ", 4f);
                }
                break;
                case TaskState.AwaitingSleep:
                currentTask=TaskState.AllTasksCompleted;
                pressIUI.SetActive(true);
                break;


        }
    }
    void Update()
    {
        
    }
}
