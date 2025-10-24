using UnityEngine;
using UnityEngine.UI; 

[RequireComponent(typeof(Collider))]
public class HiddenCameraInteract : Interactable
{
    public TaskManager_B.TaskState_B associatedDisableTask;

    public RawImage displayImage; 

    public ParticleSystem disableEffect;
    public string disableSoundName = "CameraDisable";

    private bool isDisabled = false;

    public override void OnInteract()
    {
        if (isDisabled || TaskManager_B.instance == null) return;

        bool canInteractNow = false;
        if (TaskManager_B.instance.currentTask == TaskManager_B.TaskState_B.FindCameras &&
            (associatedDisableTask == TaskManager_B.TaskState_B.DisableCamera1 || associatedDisableTask == TaskManager_B.TaskState_B.DisableCamera2))
        { canInteractNow = true; }
        else if (TaskManager_B.instance.currentTask == TaskManager_B.TaskState_B.FindFinalCamera &&
                 associatedDisableTask == TaskManager_B.TaskState_B.FinalConfrontation)
        { canInteractNow = true; }

        if (!canInteractNow)
        {
            Debug.Log($"Can't stop this camera,currentTask is : {TaskManager_B.instance.currentTask}");
            return;
        }

        isDisabled = true;
        Debug.Log(" Camera'" + gameObject.name + "' Has been Baned£¡");

        gameObject.SetActive(false);

        if (displayImage != null)
        {
            displayImage.texture = null; 
            displayImage.color = Color.black; 
        }

        TaskManager_B.instance.ReportTaskCompletion(associatedDisableTask);

        GetComponent<Collider>().enabled = false;
    }
}