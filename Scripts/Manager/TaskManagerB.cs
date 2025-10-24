using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TaskManager_B : MonoBehaviour
{
    public static TaskManager_B instance;

    public enum TaskState_B
    {
        Inactive,
        FindCameras,
        DisableCamera1,
        DisableCamera2,
        FindFinalCamera,
        FinalConfrontation,
        SequenceComplete
    }

    public TaskState_B currentTask = TaskState_B.Inactive;
    public Camera mainCam;
    public HiddenCameraInteract hiddenCamera1;
    public HiddenCameraInteract hiddenCamera2;
    public HiddenCameraInteract finalHiddenCamera;

    private int camerasDisabledCount = 0;
    public string endSceneName = "EndScene";
    public PlayerController playerController;

    

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void ActivateSequence()
    {
        if (currentTask == TaskState_B.Inactive)
        {
            currentTask = TaskState_B.FindCameras;
            camerasDisabledCount = 0;
            Debug.Log("TaskManager_B: -> " + currentTask);
            if (SubtitleManager.instance != null)
            {
                SubtitleManager.instance.ShowSubtitle("これは…俺の部屋？！ちいちゃんのじゃなく？誰かに見られているのか？あの目を潰さないと…", 5f);
            }
        }
    }

    public void ReportTaskCompletion(TaskState_B completedDisableTask)
    {
        if (currentTask == TaskState_B.FindCameras)
        {
            if (completedDisableTask == TaskState_B.DisableCamera1 || completedDisableTask == TaskState_B.DisableCamera2)
            {
                camerasDisabledCount++;
                if (camerasDisabledCount >= 2)
                {
                    currentTask = TaskState_B.FindFinalCamera;
                    Debug.Log("TaskManager_B:  -> " + currentTask);
                    if (SubtitleManager.instance != null)
                    {
                        SubtitleManager.instance.ShowSubtitle("最後の一つだ…階段の上にあるはず！", 4f);
                    }
                }
                else
                {
                    if (SubtitleManager.instance != null)
                    {
                        SubtitleManager.instance.ShowSubtitle("オフにしなきゃ…", 3f);
                    }
                }
            }
        }
        else if (currentTask == TaskState_B.FindFinalCamera)
        {
            currentTask= TaskState_B.FinalConfrontation;
            if (finalHiddenCamera != null)
                StartCoroutine(FinalConfrontationSequence(finalHiddenCamera.transform));

        }
    }
    private IEnumerator FinalConfrontationSequence(Transform targetCam)
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        float zoomDuration = 4f;
        float timer = 0f;
        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;
        Vector3 targetPos = targetCam.position + targetCam.forward * .5f;
        Quaternion targetRot = Quaternion.LookRotation(targetCam.position - targetPos);
        if (SubtitleManager.instance != null)
        {
            SubtitleManager.instance.ShowSubtitle("見つかった,ビッグブラザー", 1f);
        }
        while (timer < zoomDuration)
        {
            float t = timer / zoomDuration;
            t = t * t * (3f - 2f * t); 

            mainCam.transform.SetPositionAndRotation(
                Vector3.Lerp(startPos, targetPos, t),
                Quaternion.Slerp(startRot, targetRot, t)
            );

            timer += Time.deltaTime;
            yield return null;

        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        currentTask = TaskState_B.SequenceComplete;
        SceneManager.LoadScene(endSceneName);
    }
}