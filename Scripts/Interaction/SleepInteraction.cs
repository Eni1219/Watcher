using UnityEngine;
using System.Collections;
public class SleepInteraction : Interactable
{
    public Transform sleepTarget;
    public GameObject player;
    public float delayBeforeReveal = 1f;
    public BubbleController characterBubble;
    private bool isPlayerInRange=false;
    private Animator characterAnimator;
    private MonitorPlayerController characterController;
    private bool isInteracting = false;
    void Start()
    {
        associatedTask = TaskManager.TaskState.AwaitingSleep;
        if (player!=null)
        {
            characterAnimator=player.GetComponent<Animator>();
            characterController=player.GetComponent<MonitorPlayerController>();
        }    
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject==player)
            isPlayerInRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject==player) 
            isPlayerInRange=false;
    }
    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isInteracting)

            OnInteract();
    }
    public override void OnInteract()
    {
        if (TaskManager.instance == null || TaskManager.instance.currentTask != associatedTask)
        {
            Debug.Log($"Current Task is : {TaskManager.instance?.currentTask}");
            if (characterBubble != null)
            {
                characterBubble.ShowThought("まだねたくない", 4f);
            }

            return;
        }
        if (sleepTarget==null||characterAnimator==null||characterController==null)
        return;
        isInteracting=true;
        StartCoroutine(GoToBedSequence());
    }
    private IEnumerator GoToBedSequence()
    {
        characterController.enabled = false;

        Transform characterTransform = player.transform;
        Vector3 startPos = characterTransform.position;
        Quaternion startRot = characterTransform.rotation;
        float timer = 0f;
        float moveToTargetTime = .5f; 

        while (timer < moveToTargetTime)
        {
            float t = timer / moveToTargetTime;
            characterTransform.position = Vector3.Lerp(startPos, sleepTarget.position, t);
            characterTransform.rotation = Quaternion.Slerp(startRot, sleepTarget.rotation, t);
            timer += Time.deltaTime;
            yield return null;
        }
        characterTransform.position = sleepTarget.position;
        characterTransform.rotation = sleepTarget.rotation;


        characterAnimator.SetBool("isLying", true);

        yield return new WaitForSeconds(delayBeforeReveal);

        TaskManager.instance.CompleteTask(TaskManager.TaskState.AwaitingSleep);

    }
}
