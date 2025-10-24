using UnityEngine;
using System.Collections;
public class KeyDoor : Interactable
{
    public float openAngle = 90.0f;
    public float rotationSpeed = 2.0f;

    private bool isOpen = false;
    private Quaternion initialRotation;
    private Coroutine rotationCoroutine;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    public override void OnInteract()
    {

        if (isOpen || rotationCoroutine != null) return;

        if (CollectionManager.instance != null && CollectionManager.instance.hasKey)
        {
            UnlockAndOpenSequence();
            AudioManager.instance.Play("UnlockKeyDoor");
        }
        else
 
            SubtitleManager.instance.ShowSubtitle("ÊI§…§≥§À÷√§§§ø§√§±", 3f);
    }

    private void UnlockAndOpenSequence()
    {
        isOpen = true;

       
        rotationCoroutine = StartCoroutine(RotateDoor(initialRotation * Quaternion.Euler(0, openAngle, 0)));

        this.enabled = false;
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        float timer = 0f;
        Quaternion startRotation = transform.rotation;

        while (timer < 1f)
        {
            timer += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timer);
            yield return null;
        }

        transform.rotation = targetRotation;
        rotationCoroutine = null;
    }
}
