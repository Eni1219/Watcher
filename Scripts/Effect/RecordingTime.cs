using UnityEngine;
using TMPro; 

public class RecordingTimer : MonoBehaviour
{
   
    public TextMeshProUGUI timerText;

    private float elapsedTime = 0f;
    private bool isFlashing = false;
    private float flashTimer = 0f;
    private float flashInterval = 0.5f;

    public float timeMultiplier = 100f;


    void Start()
    {
        if (timerText == null)
        {
            this.enabled = false;
        }

        elapsedTime = Random.Range(0f, 3600f);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime*timeMultiplier;

        System.TimeSpan time = System.TimeSpan.FromSeconds(elapsedTime);
        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",  time.Hours, time.Minutes, time.Seconds);

        flashTimer += Time.deltaTime;
        if (flashTimer >= flashInterval)
        {
            isFlashing = !isFlashing;
            flashTimer = 0f;
        }

        string recPrefix = "REC:";

        timerText.text = recPrefix + " " + formattedTime;
    }
}