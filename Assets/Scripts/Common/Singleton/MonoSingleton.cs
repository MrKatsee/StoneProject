using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    T prefab = Resources.Load<T>("Prefabs/Singleton/" + typeof(T).ToString());
                    if (prefab != null)
                    {
                        Debug.Log("Prefab Singleton Created");
                        instance = Instantiate(prefab) as T;
                        instance.name = typeof(T).ToString();
                    }
                    else
                    {
                        Debug.Log("New Singleton Created");
                        instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    }
                }
            }

            return instance;
        }
    }
    protected static T instance = null;

}
