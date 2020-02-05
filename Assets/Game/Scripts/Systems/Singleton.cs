using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

    /**
       Returns the instance of this singleton.
    */
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name, typeof(T));
                    instance = go.GetComponent<T>();
                }
            }

            return instance;
        }
    }
}
