using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : Menu
{
    public MenuClassifier lobbyMenuClassifier;

    public void OnClick_Connect()
    {
        NetworkManager.Instance.Connect();
        MenuManager.Instance.hideMenu(menuClassifier);
        MenuManager.Instance.showMenu(lobbyMenuClassifier);
    }
}
