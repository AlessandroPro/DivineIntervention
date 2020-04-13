using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;
	private bool initialized;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (T)FindObjectOfType(typeof(T));
				if (instance == null)
				{
					GameObject go = new GameObject(typeof(T).Name, typeof(T));
					instance = go.GetComponent<T>();
				}
			}
			return instance;
		}
	}

	public static bool isValidSingleton()
	{
		return (instance != null);
	}

	public static void destroy()
	{
		Destroy(instance.gameObject);
		instance = null;
	}
}
