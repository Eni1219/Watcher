using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightSwitch : Interactable
{
    public Light[] targetLights;
    public Volume globalVolume;
    private ColorAdjustments colorAdjustments;

    public GameObject[] passwordNumbers;
    public Material monitorMaterial;

    private bool isOn = false; 

    void Start()
    {
        if (globalVolume != null && globalVolume.profile.TryGet(out colorAdjustments))
            UpdateLightAndMonitor();
        if(passwordNumbers!=null)
        {
            foreach(GameObject number in passwordNumbers)
            {
                if(number != null) 
                    number.SetActive(false);
            }
        }
    }

    public override void OnInteract()
    {
        isOn = !isOn;
        UpdateLightAndMonitor();
        TaskManager.instance.CompleteTask(TaskManager.TaskState.AwaitingLightSwitch);
    }

    void UpdateLightAndMonitor()
    {
        if (targetLights == null || monitorMaterial == null) return;

        foreach (Light light in targetLights)
        {
            if (light != null)
                light.enabled = isOn;
        }

        // Shader
        if (isOn)
        {
            AudioManager.instance.Play("LightSwitchOn");
            monitorMaterial.DisableKeyword("_NIGHT_VISION_ON");
            if (colorAdjustments != null)
                colorAdjustments.saturation.value = 0f;
            Debug.Log("NormalMode");
            if (passwordNumbers != null)
            {
                foreach (GameObject number in passwordNumbers)
                {
                    if (number != null)
                        number.SetActive(false);
                }
            }
        }
        else
        {
            AudioManager.instance.Play("LightSwitchOff");
            monitorMaterial.EnableKeyword("_NIGHT_VISION_ON");
            if (colorAdjustments != null)
                colorAdjustments.saturation.value = -100f;
            Debug.Log("MonoMode");
            if (passwordNumbers != null)
            {
                foreach (GameObject number in passwordNumbers)
                {
                    if (number != null)
                        number.SetActive(true);
                }
            }
        }
    }
}

