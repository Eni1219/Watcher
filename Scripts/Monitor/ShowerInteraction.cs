using System.Collections;
using UnityEngine;

public class ShowerInteraction :Interactable
{
    public Transform showerTarget;
    public Transform showerExitTarget;

    public GameObject characterToInteract;
    public float showerDuration = 15f;
    private bool isPlayerInRange = false;
    public BubbleController characterBubble;
    public ParticleSystem showerSteamFX;
    private MonitorPlayerController characterController;
    
    void Start()
    {
        associatedTask = TaskManager.TaskState.AwaitingShower;
        if (characterToInteract != null)
            characterController=characterToInteract.GetComponent<MonitorPlayerController>();
        if(showerSteamFX!= null)
            showerSteamFX.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject==characterToInteract)
          isPlayerInRange = true;
        Debug.Log("Can Shower");
          OnInteract();
    }
   
   
    public override void OnInteract()
    {
        if (TaskManager.instance == null || TaskManager.instance.currentTask != associatedTask)
        {
            Debug.Log($"CurrentTask is: {TaskManager.instance?.currentTask}");
            if (characterBubble != null)
            {
                characterBubble.ShowThought("¤Þ¤À¥·¥ã¥ï©`¤·¤¿¤¯¤Ê¤¤", 4f);
            }

            return; 
        }
            if (characterToInteract == null) return;
            GetComponent<Collider>().enabled=false;
        StartCoroutine(ShowerSequence());
    }
    private IEnumerator ShowerSequence()
    {
        characterController.enabled = false;
        Transform characterTransform=characterToInteract.transform;
        Vector3 startPos = characterTransform.position;
        Quaternion startRot=characterTransform.rotation;
        float timer = 0f;
        float moveToTargetTime = 2f;
        while (timer < moveToTargetTime)
        {
            float t=timer/moveToTargetTime;
            characterTransform.position=Vector3.Lerp(startPos, showerTarget.position, t);
            characterTransform.rotation=Quaternion.Slerp(startRot,showerTarget.rotation, t);
            timer += Time.deltaTime;
            yield return null;
        }
        characterTransform.position = showerTarget.position;
        characterTransform.rotation = showerTarget.rotation;

        AudioManager.instance.Play("Shower");
        if(showerSteamFX != null)
            showerSteamFX.Play();
        yield return new WaitForSeconds(showerDuration);
        AudioManager.instance.Stop("Shower");
        if(showerSteamFX!=null)
            showerSteamFX.Stop();

        yield return new WaitForSeconds(3f);
        Debug.Log("Finished Shower");
        startPos=characterTransform.position;
        startRot=characterTransform.rotation;
        timer = 0f;
        moveToTargetTime = 2f;
        while (timer < moveToTargetTime)
        {
            float t = timer / moveToTargetTime;
            characterTransform.position = Vector3.Lerp(startPos, showerExitTarget.position, t);
            characterTransform.rotation = Quaternion.Slerp(startRot, showerExitTarget.rotation, t);
            timer += Time.deltaTime;
            yield return null;
        }
        characterTransform.position=showerExitTarget.position;
        characterTransform.rotation=showerExitTarget.rotation;
        TaskManager.instance.CompleteTask(TaskManager.TaskState.AwaitingShower);
        characterController.enabled = true;
    }
}
