using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : Menu
{
    public MenuClassifier titleMenu;

    public SceneReference sceneToLoad;
    public SceneReference sceneToUnload;
    public void OnClick_Exit()
    {
        NetworkManager.Instance.Disconnect();

        MenuManager.Instance.hideMenu(this.menuClassifier);
        SceneLoader.Instance.onSceneLoadedEvent.AddListener(ShowTitleScreen);
        SceneLoader.Instance.UnloadScene(sceneToUnload);
        SceneLoader.Instance.LoadScene(sceneToLoad);
    }

    public void ShowTitleScreen(List<string> scenes)
    {
        MenuManager.Instance.showMenu(titleMenu);
        SceneLoader.Instance.onSceneLoadedEvent.RemoveListener(ShowTitleScreen);
    }
}
