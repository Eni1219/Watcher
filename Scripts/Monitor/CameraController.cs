using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera monitorCam;
    public Transform[] camPos;
    private int currentCamIndex = 0;

    private void Awake()
    {
        if (camPos != null)
            SwitchTo(0);
    }
    public void SwitchTo(int index)
    {
        if (index < 0 || index >= camPos.Length) return;
        currentCamIndex = index;

        Transform target = camPos[index];
        monitorCam.transform.SetPositionAndRotation(target.position, target.rotation);
    }
}
