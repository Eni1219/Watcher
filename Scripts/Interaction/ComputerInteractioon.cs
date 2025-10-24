using UnityEngine;

public class ComputerInteractioon : Interactable
{
    public RealCameraController realCamController;

    public override void OnInteract()
    {
       if(CollectionManager.instance.hasUSB)
        {
            if (realCamController!= null)
              realCamController.OpenMonitor();
              this.enabled = false;
            if (TaskManager_B.instance != null)
            {
                TaskManager_B.instance.ActivateSequence();
            }
            this.enabled = false;
        }
       else
        {
            Debug.Log("Need Usb");
        }
    }
}
