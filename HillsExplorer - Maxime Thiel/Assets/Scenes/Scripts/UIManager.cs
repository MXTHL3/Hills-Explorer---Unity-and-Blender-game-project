using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject hillsExplorerLogo;
    public GameObject play;
    public GameObject parameters;
    public GameObject quit;
    public GameObject exitToMenu;
    public GameObject hearts;
    public GameObject killedMonsters;
    public GameObject collectedCoins;
    public TextMeshProUGUI coinsNumber;
    public TextMeshProUGUI monstersNumber;
    public GameObject deadScreen;
    public GameObject victoryScreen;
    public TextMeshProUGUI author;

    public void HandleMenuUI()
    {
        hillsExplorerLogo.SetActive(true);
        play.SetActive(true);
        parameters.SetActive(true);
        quit.SetActive(true);
        exitToMenu.SetActive(false);
        hearts.SetActive(false);
        killedMonsters.SetActive(false);
        collectedCoins.SetActive(false);
        coinsNumber.enabled = false;
        monstersNumber.enabled = false;
        author.enabled = true;
    }

    public void HandleInGameUI()
    {
        hillsExplorerLogo.SetActive(false);
        play.SetActive(false);
        parameters.SetActive(false);
        quit.SetActive(false);
        exitToMenu.SetActive(true);
        hearts.SetActive(true);
        killedMonsters.SetActive(true);
        collectedCoins.SetActive(true);
        coinsNumber.enabled = true;
        monstersNumber.enabled = true;
        author.enabled = false;
    }

    public void HandleDeadScreen()
    {
        hillsExplorerLogo.SetActive(false);
        play.SetActive(false);
        parameters.SetActive(false);
        quit.SetActive(false);
        exitToMenu.SetActive(false);
        hearts.SetActive(false);
        killedMonsters.SetActive(false);
        collectedCoins.SetActive(false);
        coinsNumber.enabled = false;
        monstersNumber.enabled = false;
        author.enabled = false;
        deadScreen.SetActive(true);
    }

    public void HandleVictoryScreen()
    {
        hillsExplorerLogo.SetActive(false);
        play.SetActive(false);
        parameters.SetActive(false);
        quit.SetActive(false);
        exitToMenu.SetActive(false);
        hearts.SetActive(false);
        killedMonsters.SetActive(false);
        collectedCoins.SetActive(false);
        coinsNumber.enabled = false;
        monstersNumber.enabled = false;
        author.enabled = false;
        victoryScreen.SetActive(true);
    }

    public void UpdateHearts(GameObject newHeartState)
    {
        foreach (Transform child in hearts.transform)
        {
            child.gameObject.SetActive(false); 
        }
        newHeartState.SetActive(true); 
    }
}
