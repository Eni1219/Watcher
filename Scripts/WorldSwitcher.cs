using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class WorldSwitcher : MonoBehaviour
{
    public Camera mainCam;
    public Transform playCamPos;
    public RawImage monitorImage;
    public Material monitorEffectMaterial;
    public TaskManager taskManagerA;

    public GameObject keyPromptsPanel;
    public TextMeshProUGUI[] keyPrompts;

    public Color promptNormalColor = new Color(1, 1, 1);
    public Color promptHighLightColor = Color.black;
    public float promptFlashDur = .2f;
    private Coroutine[] flashCoroutines;

    private bool isMonitoring = true;
    private bool isRealMonitoring = false;
    public GameObject pressIUI;
    public GameObject globalVolume;
    public Transform PosA;
    public Transform PosB;
    public float transitionDur = 3f;
    private Coroutine currentTransition;

    public CameraController monitorController;
    public PlayerController playerController;
    public MonitorPlayerController monitorPlayerController;
    void Start()
    {
        SetMonitoringMode(true);
        if(keyPrompts!=null)
            flashCoroutines=new Coroutine[keyPrompts.Length];
    }

    void Update()
    {
        if (!isMonitoring && playerController != null)
        {
            if (playerController.isLookingAtComputer)
                pressIUI.SetActive(true);
            else
                pressIUI.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            TrySwitchMode();

        }

        if (isMonitoring)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                monitorController.SwitchTo(0);
                FlashKey(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                monitorController.SwitchTo(1);
                FlashKey(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                monitorController.SwitchTo(2);
                FlashKey(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                monitorController.SwitchTo(3);
                FlashKey(3);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                monitorController.SwitchTo(4);
                FlashKey(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                monitorController.SwitchTo(5);
                FlashKey(5);
            }

        }

    }
    void FlashKey(int index)
    {
        if (keyPrompts == null || index < 0 || index >= keyPrompts.Length || keyPrompts[index]==null)
            return;
        if(flashCoroutines[index] != null)
            StopCoroutine(flashCoroutines[index]);

        flashCoroutines[index] = StartCoroutine(FlashKeyCoroutine(keyPrompts[index]));

    }
    private void TrySwitchMode()
    {
        if (isMonitoring)
        {
            if (taskManagerA != null && taskManagerA.currentTask == TaskManager.TaskState.AllTasksCompleted)
            {
              
                SetMonitoringMode(false);
            }
         
        }
        else
        {

           
            if(Input.GetKeyDown(KeyCode.I)&&playerController.isLookingAtComputer)
            { 
                SetMonitoringMode(true);
            }

        }
    }
    void SetMonitoringMode(bool v)
    {
        isMonitoring = v;
        monitorController.monitorCam.enabled = v;
        playerController.enabled=!v;
        monitorController.enabled = v;
        monitorPlayerController.enabled = v;
        if (monitorImage == null) return;
        if (v)
        {
            Debug.Log("Enter MoritoringMode");
            monitorImage.enabled = true;
            if ( monitorEffectMaterial != null)
                monitorImage.material = monitorEffectMaterial;
            globalVolume.SetActive(true);

            if (keyPromptsPanel != null)
                keyPromptsPanel.SetActive(true);
            if (keyPrompts != null)
            {
                foreach (TextMeshProUGUI prompt in keyPrompts)
                {
                    if (prompt != null) prompt.color = promptNormalColor;
                }
            }

        }
        else
        {
            Debug.Log("Back To Reality");
            mainCam.transform.SetPositionAndRotation(playCamPos.position+new Vector3(0,.5f,0), playCamPos.rotation);
            monitorImage.enabled = false;
            globalVolume.SetActive(false);
            pressIUI.SetActive(false);
            if (keyPromptsPanel != null) 
                keyPromptsPanel.SetActive(false);
            StartCoroutine(TransitionToReality());
        }
        }
    private IEnumerator TransitionToReality()
    {
       // monitor off
        if (monitorPlayerController != null) monitorPlayerController.enabled = false;
        if (monitorController != null) monitorController.enabled = false;

        Vector3 startPos = PosA.position;
        Vector3 endPos = PosB.position;

        float timer = 0f;
        while (timer < transitionDur)
        {

            float t = timer / transitionDur;
     
            t = t * t * (3f - 2f * t);
            mainCam.transform.position = Vector3.Lerp(startPos, endPos, t);

            timer += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.position=endPos;

        if (playerController != null) playerController.enabled = true;

        currentTransition = null;
    }
    private IEnumerator FlashKeyCoroutine(TextMeshProUGUI keyTest)
    {
        keyTest.color=promptHighLightColor;
        yield return new WaitForSeconds(promptFlashDur);
        keyTest.color = promptNormalColor;
    }
}
