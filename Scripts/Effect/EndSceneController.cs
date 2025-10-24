using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro; 
using UnityEngine.UI; 

public class EndSceneController : MonoBehaviour
{
    public TextMeshProUGUI thanksText;

    public Button backButton; 

    public float initialBlackScreenTime = 15f;
    public float thanksFadeInDuration = 2f;
    public float delayBeforeButton = 2f;
    public float buttonFadeInDuration = 1f;

    public string titleSceneName = "TitleScene";


    private Image buttonImage;
    private TextMeshProUGUI buttonText;

    void Start()
    {
        if (thanksText != null)
        {
            thanksText.color = new Color(thanksText.color.r, thanksText.color.g, thanksText.color.b, 0);
        }
        else
        {
            Debug.LogError("thanksText instance£¡");
        }

        if (backButton != null)
        {
            backButton.interactable = false; 

            buttonImage = backButton.GetComponent<Image>();
            buttonText = backButton.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonImage != null)
                buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0);
            if (buttonText != null)
                buttonText.color = new Color(buttonText.color.r, buttonText.color.g, buttonText.color.b, 0);
        }
        else
        {
            Debug.LogError("backButton instance£¡");
        }

        StartCoroutine(FinalCreditsSequence());
    }

    private IEnumerator FinalCreditsSequence()
    {
        yield return new WaitForSeconds(initialBlackScreenTime);

        yield return StartCoroutine(FadeText(thanksText, 1f, thanksFadeInDuration));

        yield return new WaitForSeconds(delayBeforeButton);

        StartCoroutine(FadeImage(buttonImage, 1f, buttonFadeInDuration)); 
        yield return StartCoroutine(FadeText(buttonText, 1f, buttonFadeInDuration));

        if (backButton != null)
        {
            backButton.interactable = true;
        }
    }

    private IEnumerator FadeText(TextMeshProUGUI text, float targetAlpha, float duration)
    {
        if (text == null) yield break;

        Color startColor = text.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            text.color = Color.Lerp(startColor, endColor, timer / duration);
            yield return null;
        }
        text.color = endColor;
    }
-
    private IEnumerator FadeImage(Image image, float targetAlpha, float duration)
    {
        if (image == null) yield break;

        Color startColor = image.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            image.color = Color.Lerp(startColor, endColor, timer / duration);
            yield return null;
        }
        image.color = endColor;
    }
    //on click
    public void GoToTitleScene()
    {
        SceneManager.LoadScene(titleSceneName);
    }
}