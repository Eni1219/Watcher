using UnityEngine;
using NavKeypad;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 3f;
    public float mouseSensitivity = 100f;

    public float bobAmount = 0.05f;
    public float bobFrequency = 10f;
    public float bobSpeedThreshold = 2f;

    [Header("Component reference")]
    public Transform camPos;
    public KeypadInteractionFPV keypadInteractionScript;

    public float interactionDistance = 3f;
    public GameObject pressEUI;

    private Rigidbody rb;
    private float xRotation = 0f;
    private bool isInteractingWithKeypad = false;
    private Interactable currentInteractable;
    public bool isLookingAtComputer { get; private set; }

    private Vector3 camOriginalPos;
    private float bobTimer = 0f;
    private bool isMoving;

    void Start()
    {
        pressEUI.SetActive(false);
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (camPos != null)
            camOriginalPos = camPos.localPosition;

        ExitKeypadInteraction();
    }

    void Update()
    {
        HandleInteraction();

        if (!isInteractingWithKeypad)
        {
            Move();
            Look();
            ApplyCameraBob();
            HandleFootSteps();
        }
    }

    void ApplyCameraBob()
    {
        if (camPos == null) return;

        float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;

        float speedRatio = Mathf.Clamp01(horizontalSpeed / bobSpeedThreshold);

        if (horizontalSpeed > 0.1f)
        {
            bobTimer += Time.deltaTime * bobFrequency;

            float bobOffsetY = Mathf.Sin(bobTimer) * bobAmount * speedRatio;

            float bobOffsetX = Mathf.Cos(bobTimer * 0.5f) * bobAmount * 0.5f * speedRatio;

            camPos.localPosition = camOriginalPos + new Vector3(bobOffsetX, bobOffsetY, 0);
        }
        else
        {
            bobTimer = 0f;
            camPos.localPosition = Vector3.Lerp(camPos.localPosition, camOriginalPos, Time.deltaTime * 5f);
        }
    }

    void HandleInteraction()
    {
        Interactable previousInteractable = currentInteractable;

        currentInteractable = null;
        Keypad currentKeypad = null;
        isLookingAtComputer = false;

        Ray ray = new Ray(camPos.position, camPos.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Computer"))
                isLookingAtComputer = true;

            currentKeypad = hit.collider.GetComponentInParent<Keypad>();
            if (currentKeypad != null)
            {
                currentInteractable = currentKeypad.GetComponent<Interactable>();
            }
            else
            {
                currentInteractable = hit.collider.GetComponentInParent<Interactable>();
            }
        }

        bool shouldShowPressE = !isInteractingWithKeypad && (currentKeypad != null || currentInteractable != null);
        pressEUI.SetActive(shouldShowPressE);
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentKeypad != null)
            {
                isInteractingWithKeypad = !isInteractingWithKeypad;
                if (isInteractingWithKeypad)
                    EnterKeypadInteraction();
                else
                    ExitKeypadInteraction();
            }
            else if (currentInteractable != null)
            {
                currentInteractable.OnInteract();
            }
        }
    }

    private void EnterKeypadInteraction()
    {
        isInteractingWithKeypad = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (keypadInteractionScript != null)
            keypadInteractionScript.enabled = true;

        pressEUI.SetActive(false);

        if (camPos != null)
            camPos.localPosition = camOriginalPos;
    }

    private void ExitKeypadInteraction()
    {
        isInteractingWithKeypad = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (keypadInteractionScript != null)
            keypadInteractionScript.enabled = false;
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moveDir = h * transform.right + v * transform.forward;
        Vector3 velocity = moveDir.normalized * moveSpeed;
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
      
       
    }

     void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        camPos.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    void HandleFootSteps()
    {
        float horizontalSpeed=new Vector3(rb.linearVelocity.x,0,rb.linearVelocity.z).magnitude;
        bool isMoving=horizontalSpeed > .1f;
        if (isMoving && !AudioManager.instance.isPlaying("FootStep"))
            AudioManager.instance.Play("FootStep");
        else if (!isMoving && AudioManager.instance.isPlaying("FootStep"))
            AudioManager.instance.Stop("FootStep");
    }
}