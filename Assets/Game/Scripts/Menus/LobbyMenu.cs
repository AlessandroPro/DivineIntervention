﻿using Photon.Pun;
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
    public MenuClassifier inGameUIClassifier;

    public int gameLoadedCount = 0;
    private int numClientsRequired = 0;

    public override void Start()
    {
        base.Start();
    }

    public override void onShowMenu(string options)
    {
        base.onShowMenu(options);
        gameLoadedCount = 0;
        startGameButton.gameObject.SetActive(false);
        playerStatusText.text = "Joining...";
        NetworkManager.Instance.punHandler.OnJoinedRoomEvent.AddListener(showPlayerStatus);
        NetworkManager.Instance.punHandler.OnRaiseEvent.AddListener(LoadGameScene);
        NetworkManager.Instance.punHandler.OnRaiseEvent.AddListener(ReceiveGameSceneLoaded);
    }


    public void OnClick_StartGame()
    {
        MenuManager.Instance.hideMenu(menuClassifier);

        // Tell all clients to load the game scene
        NetworkManager.Instance.RaiseEventAll(null, NetworkManager.EventCode.LoadGameSceneEvent);
        numClientsRequired = NetworkManager.Instance.GetNumPlayersInRoom();
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

    public void LoadGameScene(byte eventCode, object[] data)
    {
        if(eventCode != (byte)NetworkManager.EventCode.LoadGameSceneEvent)
        {
            return;
        }

        MenuManager.Instance.hideMenu(menuClassifier);

        SceneLoader.Instance.onSceneLoadedEvent.AddListener(NotifyGameSceneLoaded);
        SceneLoader.Instance.UnloadScene(sceneToUnload);
        SceneLoader.Instance.LoadScene(sceneToLoad);
    }

    public void NotifyGameSceneLoaded(List<string> scenes)
    {
        foreach(string sceneName in scenes)
        {
            if(sceneName == sceneToLoad.ScenePath)
            {
                // Tell the master client that the game scene has been loaded on this client
                NetworkManager.Instance.RaiseEventAll(null, NetworkManager.EventCode.GameSceneLoadedEvent);
            }
        }
        MenuManager.Instance.showMenu(inGameUIClassifier);
        SceneLoader.Instance.onSceneLoadedEvent.RemoveListener(NotifyGameSceneLoaded);
    }

    // This callback is called and processed on the master client every time a client loads the game scene
    // It start the game once the game scene has been loaded on all clients
    public void ReceiveGameSceneLoaded(byte eventCode, object[] data)
    {

        if (eventCode != (byte)NetworkManager.EventCode.GameSceneLoadedEvent)
        {
            return;
        }

        if (NetworkManager.Instance.IsMasterClient())
        {
            gameLoadedCount++;

            // Start the game once the GameScene is loaded on all clients in the current room
            if(gameLoadedCount == numClientsRequired)
            {
                GameManager.Instance.StartGame();
                gameLoadedCount = 0;
            }
        }
    }
}
