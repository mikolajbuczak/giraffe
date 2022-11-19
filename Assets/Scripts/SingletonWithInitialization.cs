using UnityEngine;
/// <summary>
/// This is lazy implementation of Singleton Design Pattern.
/// Instance is created when someone call Instance property.
/// Additionally, with this implementation you have same instance when moving to different scene.
/// </summary>
public class SingletonWithInitialization<T> : MonoBehaviour where T : MonoBehaviour, IAwakeInitializationSubject
{
    // Flag used to mark singleton destruction.
    private static bool singletonDestroyed = false;
    // Reference to our singular instance.
    private static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }
    /// <summary>
    /// Unity method called just after object creation - like constructor.
    /// </summary>
    protected virtual void Awake()
    {
        // If we don't have reference to instance and we didn't destroy instance yet than this object will take control
        if (instance == null && !singletonDestroyed)
        {
            instance = this as T;
            instance.OnSingletonCreated();
            DontDestroyOnLoad(instance);
        }
        else if (instance != this) // Else this is other instance and we should destroy it!
        {
            Destroy(this);
        }
    }
    /// <summary>
    /// Unity method called before object destruction.
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (instance != this) // Skip if instance is other than this object.
        {
            return;
        }
        singletonDestroyed = true;
        instance = null;
    }
}