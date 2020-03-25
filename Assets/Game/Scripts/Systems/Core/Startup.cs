using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
	public bool loadScenes = true;
	public SceneReference UIScene;
	public SceneReference startupScene;

	private void Start()
	{
		// Set system parameters

		Scene scene = SceneManager.GetSceneByPath(startupScene);
		if (scene.isLoaded == false && loadScenes == true)
		{
			StartCoroutine(bootSequence());
		}
		else if (scene.buildIndex == -1)
		{
			Debug.LogError("Scene not found in build list: " + startupScene);
		}
		else
		{
			StartCoroutine(ignoreBootSequence());
		}
	}

	IEnumerator ignoreBootSequence()
	{
		yield return new WaitForSeconds(1);
		SceneLoadedCallback(null);
	}

	IEnumerator bootSequence()
	{
		yield return new WaitForSeconds(1);
		SceneLoader.Instance.onSceneLoadedEvent.AddListener(SceneLoadedCallback);
		SceneLoader.Instance.LoadScenes(new List<string>() { UIScene, startupScene }, true);
	}

	void SceneLoadedCallback(List<string> scenesLoaded)
	{
		SceneLoader.Instance.onSceneLoadedEvent.RemoveListener(SceneLoadedCallback);
		MenuManager.Instance.hideMenu(MenuManager.Instance.loadingMenuClassifier);
		SceneLoader.Instance.SetActiveScene(startupScene);
	}
}
