using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

/// <summary>
/// タイトル画面を制御するクラス
/// </summary>
public class TitleSceneController : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public string firstGameSceneName = "Main";
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(firstGameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}