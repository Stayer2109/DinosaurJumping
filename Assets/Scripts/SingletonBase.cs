using UnityEditor;
using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    [SerializeField] private bool dontDestroyOnLoad = true;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
