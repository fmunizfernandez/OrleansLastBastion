using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelPanel;

    [SerializeField] private Button newGameButton; 
    [SerializeField] private Button continueButton;

    [SerializeField] private Button[] levelButtons;

    private int _unlockedLevel;

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        levelPanel.SetActive(false);
        ManageContinueVisibillity();
    }

    #region MainMenud

    public void NewGame() 
    {
        GameManager.Instance.ResetProgress();
        Continue();
    }
    
    public void Continue() 
    {
        //GenerateButtons();
        mainMenuPanel.SetActive(false);
        levelPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion

    #region Selection Level

    private void ManageContinueVisibillity() 
    {
        _unlockedLevel = GameManager.Instance.GetMaxUnlockedLevel();
        continueButton.gameObject.SetActive(_unlockedLevel > 1);
    }

    private void GenerateButtons()
    {
        foreach (var button in levelButtons) 
        {
            button.gameObject.SetActive(false);
        }

        var progress = GameManager.Instance.GetMaxUnlockedLevel();
        for (int i = 1; i <= progress; i++) 
        {
            var button=levelButtons.Where(p => p.gameObject.name.Contains(i.ToString())).FirstOrDefault();
            button.gameObject.SetActive(button != null);
        }
    }

    public void Back()
    {
        mainMenuPanel.SetActive(true);
        levelPanel.SetActive(false);

        ManageContinueVisibillity();
    }

    public void Level1()
    {
        SceneManager.LoadScene($"2.Level1");
    }

    public void Level2()
    {
        SceneManager.LoadScene($"3.Level2");
    }

    public void Level3()
    {
        SceneManager.LoadScene($"4.Level3");
    }

    public void Level4()
    {
        SceneManager.LoadScene($"5.Level4");
    }

    public void Level5()
    {
        SceneManager.LoadScene($"6.Level5");
    }

    #endregion


}
