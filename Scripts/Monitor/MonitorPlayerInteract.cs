using UnityEngine;

public class MonitorPlayerInteract : MonoBehaviour
{
    public float interactDis = 1.5f;
    public LayerMask interactableMask;
    public KeyCode interactKey = KeyCode.E;
    public Transform rayOrigin;
    public GameObject pressE;

    private Interactable currentTarget;

    private void Start()
    {
        if (pressE != null)
            pressE.SetActive(false);
    }

    void Update()
    {
        if (rayOrigin != null)
            rayOrigin.rotation = transform.rotation;

        DetectInteractable();

        if (currentTarget != null && Input.GetKeyDown(interactKey))
        {
            currentTarget.OnInteract();
        }

        if (rayOrigin != null)
        {
            Debug.DrawLine(rayOrigin.position, rayOrigin.position + rayOrigin.forward * interactDis, Color.green);
        }
    }

    void DetectInteractable()
    {
        if (rayOrigin == null) return;

        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        Interactable previousTarget = currentTarget;

        if (Physics.Raycast(ray, out RaycastHit hit, interactDis, interactableMask))
        {
            Interactable target = hit.collider.GetComponent<Interactable>();

            if (target != null)
            {
                if (target != currentTarget)
                {
                    currentTarget?.OnLoseFocus();

                    currentTarget = target;
                    currentTarget.OnFocus();
                }

                if (pressE != null)
                    pressE.SetActive(true);
            }
            else
            {
                ClearCurrentTarget();
            }
        }
        else
        {

            ClearCurrentTarget();
        }
    }

    private void ClearCurrentTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.OnLoseFocus();
            currentTarget = null;
        }

        if (pressE != null)
            pressE.SetActive(false);
    }

    private void OnDisable()
    {
        ClearCurrentTarget();
    }
}