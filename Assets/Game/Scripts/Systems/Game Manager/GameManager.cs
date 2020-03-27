using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    public float scrollSpeed;
    public GameObject wingedSpirit;
    public GameSetup gameSetup;
    public MenuClassifier gameOverMenu;


    public void StartGame()
    {
        // Master client tells all clients to setup their game
       if(NetworkManager.Instance.IsMasterClient())
       {
            NetworkManager.Instance.RaiseEventAll(null, gameSetup.setupGameSceneEvent);
       }
    }

    public void EndGame()
    {
        MenuManager.Instance.showMenu(gameOverMenu);
    }
}
