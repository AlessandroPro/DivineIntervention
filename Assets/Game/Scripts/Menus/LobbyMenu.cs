using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenu : Menu
{
    public SceneReference sceneToLoad;
    public SceneReference sceneToUnload;
    public void OnClick_StartGame()
    {
        MenuManager.Instance.hideMenu(menuClassifier);
        SceneLoader.Instance.UnloadScene(sceneToUnload);
        SceneLoader.Instance.LoadScene(sceneToLoad);
    }
}
