using UnityEngine;
using UnityEngine.UI;

public class RealCameraController : MonoBehaviour
{
    public GameObject realMonitorPanel;
    public GameObject monitorFilter;
    private void Start()
    {
        if(realMonitorPanel != null)
        realMonitorPanel.SetActive(false);
    }
   public void OpenMonitor()
    {
        if (realMonitorPanel != null)
            realMonitorPanel.SetActive(true);
        if(monitorFilter != null) monitorFilter.SetActive(false);
    }
}
