
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.Rendering;


public class ResManager : Singleton<ResManager>
{
	public Dictionary<string, GameObject> _uiObjs = new Dictionary<string, GameObject>();
	public Dictionary<string, Sprite> _icons = new Dictionary<string, Sprite>();


	public AudioClip GetLocalSound(string soundName)
	{
		AudioClip obj = Resources.Load<AudioClip>("Sound/" + soundName);
		return obj;
	}
	public GameObject GetUIObj(string name)
	{
		if (_uiObjs.ContainsKey(name))
		{
			return _uiObjs[name];
		}
		GetLocalUIObj(name);
		if (_uiObjs.ContainsKey(name))
		{
			return _uiObjs[name];
		}
		else
		{
			return null;
		}
	}
	public void GetLocalUIObj(string uiName)
	{
		GameObject obj = Resources.Load("UI/" + uiName) as GameObject;
		if (_uiObjs.ContainsKey(uiName))
		{
			_uiObjs.Remove(uiName);
		}
		_uiObjs.Add(uiName, obj);
	}

	public Sprite GetIcon(string name)
	{
		if (_icons.ContainsKey(name))
		{
			return _icons[name];
		}
		GetLocalIcon(name);
		if (_icons.ContainsKey(name))
		{
			return _icons[name];
		}
		else
		{
			return null;
		}
	}
	public void GetLocalIcon(string iconName)
	{
		Sprite texture = Resources.Load("Icon/" + iconName, typeof(Sprite)) as Sprite;
		if (_icons.ContainsKey(iconName))
		{
			_icons.Remove(iconName);
		}

		_icons.Add(iconName, texture);
	}

}