using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : Menu
{
    public MenuClassifier lobbyMenuClassifier;
    public Button connectButton;
    public Text connectText;

    public SceneReference titleScene;

    public override void Start()
    {
        base.Start();
        onShowMenu(null);
    }

    public override void onShowMenu(string options)
    {
        base.onShowMenu(options);
        connectText.gameObject.SetActive(false);
        connectButton.interactable = true;
        NetworkManager.Instance.punHandler.OnConnectedToMasterEvent.AddListener(showLobbyMenu);
        NetworkManager.Instance.punHandler.OnDisconnectedEvent.AddListener(backToTitleMenu);
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

    public void backToTitleMenu()
    {

        MenuManager.Instance.hideAllMenus();

        NetworkManager.Instance.punHandler.resetAll();
        SceneLoader.Instance.onSceneLoadedEvent.RemoveAllListeners();
        SceneLoader.Instance.onSceneLoadedEvent.AddListener(showTitleMenu);

        SceneLoader.Instance.UnloadActiveScene();
        SceneLoader.Instance.LoadScene(titleScene);
    }

    public void showTitleMenu(List<string> scenes)
    {
        MenuManager.Instance.hideAllMenus();
        MenuManager.Instance.showMenu(this.menuClassifier);
        SceneLoader.Instance.onSceneLoadedEvent.RemoveListener(showTitleMenu);
    }
}
