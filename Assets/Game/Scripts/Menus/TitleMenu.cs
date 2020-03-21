using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : Menu
{
    public MenuClassifier lobbyMenuClassifier;
    public Button connectButton;
    public Text connectText;

    public override void Start()
    {
        base.Start();
        connectText.gameObject.SetActive(false);
        NetworkManager.Instance.punHandler.OnConnectedToMasterEvent.AddListener(showLobbyMenu);
        NetworkManager.Instance.punHandler.OnDisconnectedEvent.AddListener(enableConnect);
    }

    public void onClick_Connect()
    {
        NetworkManager.Instance.Connect();
        connectButton.interactable = false;
        connectText.gameObject.SetActive(true);
    }

    public void showLobbyMenu()
    {
        MenuManager.Instance.hideMenu(menuClassifier);
        MenuManager.Instance.showMenu(lobbyMenuClassifier);
        //NetworkManager.Instance.JoinLobby();
        NetworkManager.Instance.JoinRoom();
    }

    public void enableConnect()
    {
        connectButton.interactable = true;
        connectText.gameObject.SetActive(false);
    }
}
