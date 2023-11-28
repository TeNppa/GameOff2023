using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
    private enum MenuOption { StartGame, Settings, Credits, Leaderboards, QuitGame }
    private MenuOption menuIndex = MenuOption.StartGame;
    [SerializeField] private MainMenuCamera mainMenuCamera;
    [SerializeField] private Text cancelCreditsText;
    [SerializeField] private Text[] menuButtons;
    [SerializeField] private Color activeColor = Color.yellow;
    [SerializeField] private Color unactiveColor = Color.white;
    [SerializeField] private int activeFontSize = 50;
    [SerializeField] private int unactiveFontSize = 40;


    private void Start()
    {
        UpdateLinks();
    }


    private void Update()
    {
        CheckActivationKeys();
        CheckNavigationKeys();
    }


    private void CheckActivationKeys()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            ActivateLink();
        }
    }


    private void CheckNavigationKeys()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && menuIndex > MenuOption.StartGame)
        {
            menuIndex--;
            UpdateLinks();
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && menuIndex < MenuOption.QuitGame)
        {
            menuIndex++;
            UpdateLinks();
        }
    }


    private void UpdateLinks()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            menuButtons[i].color = (MenuOption)i == menuIndex ? activeColor : unactiveColor;
            menuButtons[i].fontSize = (MenuOption)i == menuIndex ? activeFontSize : unactiveFontSize;
        }
    }


    private void ActivateLink()
    {
        switch (menuIndex)
        {
            case MenuOption.StartGame: StartGame(); break;
            case MenuOption.Settings: OpenSettings(); break;
            case MenuOption.Credits: OpenCredits(); break;
            case MenuOption.Leaderboards: OpenLeaderboards(); break;
            case MenuOption.QuitGame: QuitGame(); break;
        }
    }


    // Unity mouse over event
    public void HoverStartGameButtonEvent()
    {
        menuIndex = MenuOption.StartGame;
        UpdateLinks();
    }

    // Unity mouse over event
    public void HoverSettingsButtonEvent()
    {
        menuIndex = MenuOption.Settings;
        UpdateLinks();
    }

    // Unity mouse over event
    public void HoverCreditsButtonEvent()
    {
        menuIndex = MenuOption.Credits;
        UpdateLinks();
    }

    // Unity mouse over event
    public void HoverLeaderboardsButtonEvent()
    {
        menuIndex = MenuOption.Leaderboards;
        UpdateLinks();
    }

    // Unity mouse over event
    public void HoverQuitButtonEvent()
    {
        menuIndex = MenuOption.QuitGame;
        UpdateLinks();
    }

    // Unity mouse click event
    public void MouseEnterCancelCredits()
    {
        cancelCreditsText.color = activeColor;
        cancelCreditsText.fontSize = activeFontSize;
    }

    // Unity mouse click event
    public void MouseExitCancelCredits()
    {
        cancelCreditsText.color = unactiveColor;
        cancelCreditsText.fontSize = unactiveFontSize;
    }

    // Unity mouse click event
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    // Unity mouse click event
    public void OpenSettings()
    {
        Debug.Log("Opened settings");
    }

    // Unity mouse click event
    public void OpenCredits()
    {
        mainMenuCamera.MoveToCreditsPosition();
    }

    // Unity mouse click event
    public void OpenLeaderboards()
    {
        mainMenuCamera.MoveToLeaderboardPosition();
    }

    // Unity mouse click event
    public void QuitGame()
    {
        Debug.Log("Quit game");
    }

    // Unity mouse click event
    public void CancelCredits()
    {
        mainMenuCamera.MoveToDefaultPosition();
    }
}
