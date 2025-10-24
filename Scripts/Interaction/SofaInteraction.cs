using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class SofaInteraction :Interactable
{
    public Transform sitTarget;
    public GameObject tvScreen;
    public GameObject characterToSit;
    private bool isPlayInRange = false;
    public BubbleController characterBubble;
    private Animator characterAnimator;
    public TVController tvController;
    private MonitorPlayerController characterController;
    public GameObject pressE;
    void Start()
    {
        pressE.SetActive(false);
        associatedTask = TaskManager.TaskState.AwaitingRest;
        if (characterToSit != null)
        {
            characterAnimator = characterToSit.GetComponent<Animator>();
            characterController=characterToSit.GetComponent<MonitorPlayerController>();
        }
        if(tvScreen != null)
            tvScreen.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject==characterToSit)
        {
            pressE.SetActive(true);
            isPlayInRange = true;
            Debug.Log("Can Sit");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == characterToSit)
        {
            pressE.SetActive(false);
            isPlayInRange = false;
            Debug.Log("Cannot Sit");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(isPlayInRange&&Input.GetKeyDown(KeyCode.E))
        {
            OnInteract();
        }
    }
    public override void OnInteract()
    {
        if (TaskManager.instance == null || TaskManager.instance.currentTask != associatedTask)
        {
            Debug.Log($"Current Task is: {TaskManager.instance?.currentTask}");
            if (characterBubble != null)
            {
                characterBubble.ShowThought("まだ恙りたくない", 4f);
            }

            return;
        }
        if (sitTarget == null || characterAnimator == null || characterController == null)
            return;
       
            StartCoroutine(SitDownSequence());
        
    }
    private IEnumerator SitDownSequence()
    {
        characterController.enabled = false;

        Transform characterTransform=characterToSit.transform;
        Vector3 startPos = characterTransform.position;
        Quaternion startRot= characterTransform.rotation;
        float timer = 0f;
        float moveToTargetTime = .5f;
        while(timer < moveToTargetTime)
        {
            float t = timer / moveToTargetTime;
            characterTransform.position=Vector3.Lerp(startPos,sitTarget.position, t);
            characterTransform.rotation=Quaternion.Slerp(startRot,sitTarget.rotation, t);
            timer += Time.deltaTime;
            yield return null;
        }
        characterTransform.position=sitTarget.position;
        characterTransform.rotation=sitTarget.rotation;
        characterAnimator.SetBool("isSitting", true);
        yield return new WaitForSeconds(2f);
       if(tvController!=null)
        {
                Debug.Log("Ready To Play");
            if(tvScreen!=null)
            {
                tvScreen.SetActive(true);

                Debug.Log("Already played");
            }
        }
            
            tvController.PlayBroadCast();
        float timeout = 3f; 
        while (!tvController.isPlaying && timeout > 0)
        {
            yield return null; 
            timeout -= Time.deltaTime;
        }

        if (tvController.isPlaying)
        {
            yield return new WaitUntil(() => !tvController.isPlaying);
          
        }
      

        tvScreen.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        characterAnimator.SetBool("isSitting", false);
        characterController.enabled = true;
        TaskManager.instance.CompleteTask(TaskManager.TaskState.AwaitingRest);
        this.enabled = false;
        
    }
}
