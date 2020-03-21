using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : Menu
{
    public SceneReference sceneToLoad;
    public SceneReference sceneToUnload;
    public Text playerStatusText;
    public Button startGameButton;

    public override void Start()
    {
        base.Start();
        startGameButton.gameObject.SetActive(false);
        NetworkManager.Instance.punHandler.OnJoinedRoomEvent.AddListener(showPlayerStatus);
    }

    public void OnClick_StartGame()
    {
        MenuManager.Instance.hideMenu(menuClassifier);
        SceneLoader.Instance.UnloadScene(sceneToUnload);
        SceneLoader.Instance.LoadScene(sceneToLoad);
    }

    public void showPlayerStatus()
    {
        if (NetworkManager.Instance.IsMasterClient())
        {
            playerStatusText.text = "You are the Leader.";
            startGameButton.gameObject.SetActive(true);
        }
        else
        {
            playerStatusText.text = "Waiting for the leader to start.";
            startGameButton.gameObject.SetActive(false);
        }
    }
}
