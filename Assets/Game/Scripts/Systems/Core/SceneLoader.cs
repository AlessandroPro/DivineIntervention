using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
	[System.Serializable]
	public class SceneLoadedEvent : UnityEvent<List<string>> { }
	[HideInInspector] public SceneLoadedEvent onSceneLoadedEvent = new SceneLoadedEvent();

	public float delayTime = 1.0f;
	public MenuClassifier loadingMenuClassifier;

	private List<string> loadedScenes = new List<string>();

	// When loading just add a flag for persistence. If true don't add to the loadedScenes
	// Only remove the scenes when you unload

	public void LoadScene(string scene, bool showLoadingScreen = true)
	{
		StartCoroutine(loadScene(scene, showLoadingScreen, true));
	}

	public void LoadScenes(List<string> scenes, bool showLoadingScreen = true)
	{
		StartCoroutine(loadScenes(scenes, showLoadingScreen));
	}

	IEnumerator loadScenes(List<string> scenes, bool showLoadingScreen)
	{
		if (showLoadingScreen)
		{
			MenuManager.Instance.showMenu(loadingMenuClassifier);
		}

		foreach (string scene in scenes)
		{
			yield return StartCoroutine(loadScene(scene, false, false));
		}

		if (showLoadingScreen)
		{
			MenuManager.Instance.hideMenu(loadingMenuClassifier);
		}

		loadedScenes.Clear();
		loadedScenes.AddRange(scenes);
		onSceneLoadedEvent.Invoke(loadedScenes);
	}

	IEnumerator loadScene(string scene, bool showLoadingScreen, bool raiseEvent)
	{
		if (SceneManager.GetSceneByPath(scene).isLoaded == false)
		{
			if (showLoadingScreen)
			{
				MenuManager.Instance.showMenu(loadingMenuClassifier);
			}

			yield return new WaitForSeconds(delayTime);

			AsyncOperation sync;

			Application.backgroundLoadingPriority = ThreadPriority.Low;

			sync = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
			while (sync.isDone == false) { yield return null; }

			Application.backgroundLoadingPriority = ThreadPriority.Normal;

			yield return new WaitForSeconds(delayTime);

			if (showLoadingScreen)
			{
				MenuManager.Instance.hideMenu(loadingMenuClassifier);
			}
		}

		if (raiseEvent)
		{
			loadedScenes.Clear();
			loadedScenes.Add(scene);
			onSceneLoadedEvent.Invoke(loadedScenes);
		}
	}

	// 4 Methods:
	//	- Unload single scene
	//	- Unload list of scenes
	//		- Support to unload multiple (Coroutine)
	//	- Actual Unload of scenes.

	public void UnloadScene(string scene)
	{
		StartCoroutine(unloadScene(scene));
	}

	public void UnloadScenes(List<string> scenes)
	{
		StartCoroutine(unloadScenes(scenes));
	}

	IEnumerator unloadScenes(List<string> scenes)
	{
		foreach(string scene in scenes)
		{
			yield return StartCoroutine(unloadScene(scene));
		}
	}

	IEnumerator unloadScene(string scene)
	{
		AsyncOperation sync = null;

		try
		{
			sync = SceneManager.UnloadSceneAsync(scene);	
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}

		if (sync != null)
		{
			while(sync.isDone == false) { yield return null; }
		}

		sync = Resources.UnloadUnusedAssets();
		while(sync.isDone == false) { yield return null; }
	}

	public void SetActiveScene(Scene scene)
	{
		SceneManager.SetActiveScene(scene);
	}

	public void SetActiveScene(SceneReference scene)
	{
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.SceneName));
	}

	public void UnloadActiveScene()
	{
		UnloadScene(SceneManager.GetActiveScene().name);
	}
}
