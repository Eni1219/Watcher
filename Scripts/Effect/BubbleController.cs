using UnityEngine;
using TMPro;
using System.Collections;
public class BubbleController : MonoBehaviour
{
    public GameObject bubble;
    public Vector3 offset=new Vector3(0,2f,0);
    private TextMeshProUGUI bubbleText;
    void Start()
    {
        if(bubble == null)
        {
            this.enabled = false;
            return;
        }
        bubbleText=bubble.GetComponentInChildren<TextMeshProUGUI>();
        if(bubbleText == null)
        {
            this.enabled=false;
            return;
        }
        bubble.SetActive(false);
    }

    public void ShowThought(string message, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ThoughtSequence(message, duration));
    }

    private IEnumerator ThoughtSequence(string message, float duration)
    {
        bubbleText.text = message;
        bubble.SetActive(true); 
        yield return new WaitForSeconds(duration);
        bubble.SetActive(false); 
    }
}
