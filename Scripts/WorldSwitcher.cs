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

    // キープロンプトUI
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
    
    // トランジション設定
    public Transform PosA;
    public Transform PosB;
    public float transitionDur = 3f;
    private Coroutine currentTransition;

    // コントローラー参照
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
        // 現実モード時のUI表示制御
        if (!isMonitoring && playerController != null)
        {
            if (playerController.isLookingAtComputer)
                pressIUI.SetActive(true);
            else
                pressIUI.SetActive(false);
        }
        
        // モード切り替え
        if (Input.GetKeyDown(KeyCode.I))
        {
            TrySwitchMode();
        }

        // モニターモード時のカメラ切り替え(1-6キー)
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
    
    /// <summary>
    /// キープロンプトを点滅させる
    /// </summary>
    void FlashKey(int index)
    {
        if (keyPrompts == null || index < 0 || index >= keyPrompts.Length || keyPrompts[index]==null)
            return;
        if(flashCoroutines[index] != null)
            StopCoroutine(flashCoroutines[index]);

        flashCoroutines[index] = StartCoroutine(FlashKeyCoroutine(keyPrompts[index]));
    }
    
    /// <summary>
    /// モード切り替えを試みる
    /// タスク完了状態とプレイヤー位置をチェック
    /// </summary>
    private void TrySwitchMode()
    {
        if (isMonitoring)
        {
            // すべてのタスクが完了している場合のみ現実モードへ
            if (taskManagerA != null && taskManagerA.currentTask == TaskManager.TaskState.AllTasksCompleted)
            {
                SetMonitoringMode(false);
            }
        }
        else
        {
            // コンピューターを見ている場合のみモニターモードへ
            if(Input.GetKeyDown(KeyCode.I)&&playerController.isLookingAtComputer)
            { 
                SetMonitoringMode(true);
            }
        }
    }
    
    /// <summary>
    /// モニターモードまたは現実モードに設定する
    /// カメラ、コントローラー、UIの状態を切り替える
    /// </summary>
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
            // モニターモードに入る
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
            // 現実モードに戻る
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
    
    /// <summary>
    /// 現実モードへのスムーズなカメラトランジション
    /// </summary>
    private IEnumerator TransitionToReality()
    {
        if (monitorPlayerController != null) monitorPlayerController.enabled = false;
        if (monitorController != null) monitorController.enabled = false;

        Vector3 startPos = PosA.position;
        Vector3 endPos = PosB.position;

        float timer = 0f;
        while (timer < transitionDur)
        {
            float t = timer / transitionDur;
            // スムーズステップ補間
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
