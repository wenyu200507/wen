using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component 
{
	static T _instance;
	static bool run = true;
	public static T Instance 
	{
		get {
			if( run && _instance == null ) 
			{
				string objName = typeof(T).Name;
				GameObject obj = new GameObject(objName);
				DontDestroyOnLoad(obj);
				_instance = obj.AddComponent(typeof(T)) as T;
			}
			return _instance;
		}
	}

	void OnApplicationQuit() {
		run = false;
	}
}

public class SceneSingleton<T> : MonoBehaviour where T : Component 
{
	static T _instance;
	static bool run = true;
	public static T Instance 
	{
		get {
			if( run && _instance == null ) 
			{
				string objName = typeof(T).Name;
				GameObject obj = new GameObject(objName);
				//DontDestroyOnLoad(obj);
				_instance = obj.AddComponent(typeof(T)) as T;
			}
			return _instance;
		}
	}

	void OnApplicationQuit() {
		run = false;
	}
}