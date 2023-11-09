using UnityEngine;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Text startGameButton;
    [SerializeField] private Text settingsButton;
    [SerializeField] private Text creditsButton;
    [SerializeField] private Text leaderboardsButton;
    [SerializeField] private Text quitGameButton;
    [SerializeField] private Color activeColor = Color.yellow;
    [SerializeField] private Color unactiveColor = Color.white;
    [SerializeField] private int activeFontSize = 50;
    [SerializeField] private int unactiveFontSize = 40;
    private int menuIndex = 1;
    private int linkCount = 5;


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
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (menuIndex > 1)
            {
                menuIndex--;
                UpdateLinks();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (menuIndex < linkCount)
            {
                menuIndex++;
                UpdateLinks();
            }
        }
    }


    private void UpdateLinks()
    {
        RemoveHighlight();

        switch (menuIndex)
        {
            case 1:
                startGameButton.color = activeColor;
                startGameButton.fontSize = activeFontSize;
                break;
            case 2:
                settingsButton.color = activeColor;
                settingsButton.fontSize = activeFontSize;
                break;
            case 3:
                creditsButton.color = activeColor;
                creditsButton.fontSize = activeFontSize;
                break;
            case 4:
                leaderboardsButton.color = activeColor;
                leaderboardsButton.fontSize = activeFontSize;
                break;
            case 5:
                quitGameButton.color = activeColor;
                quitGameButton.fontSize = activeFontSize;
                break;
        }
    }


    private void ActivateLink()
    {
        switch (menuIndex)
        {
            case 1: startGame(); break;
            case 2: OpenSettings(); break;
            case 3: OpenCredits(); break;
            case 4: OpenLeaderboards(); break;
            case 5: QuitGame(); break;
        }
    }


    private void RemoveHighlight()
    {
        startGameButton.color = unactiveColor;
        settingsButton.color = unactiveColor;
        creditsButton.color = unactiveColor;
        leaderboardsButton.color = unactiveColor;
        quitGameButton.color = unactiveColor;

        startGameButton.fontSize = unactiveFontSize;
        settingsButton.fontSize = unactiveFontSize;
        creditsButton.fontSize = unactiveFontSize;
        leaderboardsButton.fontSize = unactiveFontSize;
        quitGameButton.fontSize = unactiveFontSize;
    }


    // Unity mouse over event
    public void HoverStartGameButtonEvent()
    {
        menuIndex = 1;
        UpdateLinks();
    }

    // Unity mouse over event
    public void HoverSettingsButtonEvent()
    {
        menuIndex = 2;
        UpdateLinks();
    }

    // Unity mouse over event
    public void HoverCreditsButtonEvent()
    {
        menuIndex = 3;
        UpdateLinks();
    }

    // Unity mouse over event
    public void HoverLeaderboardsButtonEvent()
    {
        menuIndex = 4;
        UpdateLinks();
    }

    // Unity mouse over event
    public void HoverQuitButtonEvent()
    {
        menuIndex = 5;
        UpdateLinks();
    }

    // Unity mouse click event
    public void startGame()
    {
        Debug.Log("start game");
    }

    // Unity mouse click event
    public void OpenSettings()
    {
        Debug.Log("Opened settings");
    }

    // Unity mouse click event
    public void OpenCredits()
    {
        Debug.Log("Opened credits");
    }

    // Unity mouse click event
    public void OpenLeaderboards()
    {
        Debug.Log("Opened leaderboards");
    }

    // Unity mouse click event
    public void QuitGame()
    {
        Debug.Log("Quit game");
    }
}
