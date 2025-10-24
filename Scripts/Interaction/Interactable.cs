using UnityEngine;

public class Interactable : MonoBehaviour
{
    public TaskManager.TaskState associatedTask;
    public virtual void OnFocus()
    {

    }
    public virtual void OnLoseFocus()
    {

    }
    public virtual void OnInteract()
    {

    }
}
