using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelPanel;

    private void Awake()
    {
        mainMenuPanel.SetActive(true);
        levelPanel.SetActive(false);
    }

    public void NewGame() 
    {
        mainMenuPanel.SetActive(false);
        levelPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back() 
    {
        mainMenuPanel.SetActive(true);
        levelPanel.SetActive(false);
    }

    public void Level1() 
    {
        SceneManager.LoadScene("Level1");
    }

    public void Level2() 
    {
        SceneManager.LoadScene("Level2");
    }
}
