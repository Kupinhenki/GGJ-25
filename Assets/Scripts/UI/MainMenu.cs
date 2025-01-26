using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadMainMenu()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.bgMusic);
        SceneManager.LoadScene("MainMenu");
    }

    public void SelectObject(Selectable obj)
    {
        obj.Select();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
