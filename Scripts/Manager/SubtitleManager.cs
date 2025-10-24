using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager instance;
    public TextMeshProUGUI subtitleText;

    public float typingSpeed = 0.05f;
    public float lineDuration = 2f;
    private Coroutine currentSubtitleCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (subtitleText == null)
        {
            this.enabled = false;
            return;
        }
        subtitleText.gameObject.SetActive(false);
    }

    public void ShowSubtitles(string fullText)
    {
        if (subtitleText == null) return;

        if (currentSubtitleCoroutine != null)
        {
            StopCoroutine(currentSubtitleCoroutine);
        }

        currentSubtitleCoroutine = StartCoroutine(MultiLineSubtitleRoutine(fullText));
    }

    public void ShowSubtitle(string text, float duration)
    {
        if (subtitleText == null) return;

        if (currentSubtitleCoroutine != null)
        {
            StopCoroutine(currentSubtitleCoroutine);
        }

        currentSubtitleCoroutine = StartCoroutine(TypewriterRoutine(text, duration));
    }

    private IEnumerator MultiLineSubtitleRoutine(string fullText)
    {
        subtitleText.gameObject.SetActive(true);

        string[] lines = fullText.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
 
            subtitleText.text = "";

            foreach (char letter in line.ToCharArray())
            {
                subtitleText.text += letter;

                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(lineDuration);
        }

        subtitleText.gameObject.SetActive(false);
        currentSubtitleCoroutine = null;
    }

    private IEnumerator TypewriterRoutine(string text, float duration)
    {
        subtitleText.gameObject.SetActive(true);
        subtitleText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            subtitleText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(duration);
        subtitleText.gameObject.SetActive(false);
        currentSubtitleCoroutine = null;
    }
}