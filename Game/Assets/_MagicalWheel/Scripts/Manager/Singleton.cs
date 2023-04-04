using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_instance;
    private static bool s_appIsQuit = false;

    public static T Instance
    {
        get
        {
            if (s_appIsQuit)
                return null;

            if (s_instance == null)
            {
                s_instance = FindObjectOfType<T>();
                if (s_instance == null)
                {
                    var singletonObj = new GameObject();
                    singletonObj.name = typeof(T).Name;
                    DontDestroyOnLoad(singletonObj);

                    s_instance = singletonObj.AddComponent<T>();
                }
            }

            return s_instance;
        }
    }

    protected virtual void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnApplicationQuit()
    {
        if (s_instance == this)
            s_instance = null;

        s_appIsQuit = true;
    }

    private void OnDestroy()
    {
        if (s_instance == this)
            s_instance = null;
    }
}