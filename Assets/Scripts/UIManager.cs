using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : Singleton<UIManager> 
{
    
	public Dictionary<string, GameObject> UIs = new Dictionary<string, GameObject>();
	private List<string> uiQueue = new List<string>();
	private Dictionary<string, GameObject> UIRecycle = new Dictionary<string, GameObject>();
	//for guide
	public List<GameObject> AnotherUIs = new List<GameObject>();
	private Transform uiCollider;
	private float uiColliderTime = 0;
	public Canvas canvas = null;
	public Texture2D screenshot = null;

	void OnApplicationFocus(bool focusStatus) {
		if (focusStatus) {
			#if UNITY_ANDROID && !UNITY_EDITOR
		#endif
			int targetFrameRate = PlayerPrefs.GetInt("targetFrameRate");
			Application.targetFrameRate = targetFrameRate * 30;
			Time.fixedDeltaTime = 0.025f;
		} else {

			#if !UNITY_EDITOR
			Application.targetFrameRate = 1;
			Time.fixedDeltaTime = 0.1f;
			#endif
		}
	}

	void Start() 
	{
		
	}


	public List<string> tips = new List<string>();
	public  void ShowTips(string tip)
	{
       
		GameObject obj = Instantiate(ResManager.Instance.GetUIObj("UI_ErrorTips"));
		obj.transform.SetParent(canvas.transform, false);
		obj.GetComponent<TempTip>().ShowTips(tip, canvas.transform);
		
	}

	public void CloseUI(string name, bool isDestroy){
		CloseUI(name, isDestroy, null);
	}

	public void CloseUI(string name, bool isDestroy, object onFinish){
		
		if (UIs.ContainsKey (name)) {
			if (isDestroy)
			{
				DestroyUI(name);
			}
			else
			{
				HideUI(name);
			}
			if (uiQueue.Count > 0)
			{
				string ui_name = uiQueue[uiQueue.Count - 1];
				if (name != ui_name && UIs.ContainsKey(ui_name) && UIs[ui_name].activeInHierarchy == false)
				{
					ShowUI(ui_name);
				}
			}
		}
		if (name == "NewUserUI") {
			if(isDestroy){
				DestroyUI(name);
			}else {
				HideUI(name);
			}
		}
	}

	void HideUI(string name){
		if(UIRecycle.ContainsKey (name)) {
			UIRecycle[name].SetActive(false);
			if(UIs.ContainsKey(name)){
				UIs[name] = UIRecycle[name];
			}else{
				UIs.Add(name, UIRecycle[name]);
			}

			UIRecycle.Remove(name);
		}else if(UIs.ContainsKey(name)){
			UIs[name].SetActive(false);
		}
	}
	
	void DestroyUI(string name){

		if (UIRecycle.ContainsKey (name)) {
			UIRecycle [name].SetActive (false);
			Destroy (UIRecycle [name]);
			UIRecycle.Remove (name);
		} else if (UIs.ContainsKey (name)) {
			UIs [name].SetActive (false);
			Destroy (UIs [name]);
			UIs.Remove (name);
		}
	}

	public void ShowUI(string name){
		ShowUI (name, null);
	}

	public void ShowUIUnique(string name, object onFinish){
		DestroyUI (name);
		ShowUI (name, onFinish);
	}

	public void ShowUI(string name, object onFinish){
		if (UIs.ContainsKey (name)) {
			UIs[name].SetActive(true);
		}else {
            GameObject go = InitUI(name);
			if(go == null){
				return;
			}
			go.SetActive(true);
		}
	}

	public GameObject InitUI(string name)
	{
        if (canvas == null)
        {
			GameObject rootObj = GameObject.Find("Canvas");
			canvas = rootObj.GetComponent<Canvas>();
		}
		var obj = ResManager.Instance.GetUIObj(name);
		if (obj != null)
		{
			GameObject mo;
			mo = Instantiate<GameObject>(obj, canvas.transform);

			if (name != "ConfirmUI")
			{
				if (UIs.ContainsKey(name))
				{
					Destroy(mo);
					return null;
				}
				else
				{
					UIs.Add(name, mo);
				}
			}

			return mo;
		}
		else
		{
			return null;
		}
	}

	public void SetUIImage(Image texUI, string iconName , object Done = null)
	{
        if (iconName == null)
            return;
		iconName = iconName.Replace(".png", "");


		if (texUI != null)
		{
			texUI.sprite = ResManager.Instance.GetIcon(iconName);
		}
	}

	public bool IsOpen (string name) {
		return UIs.ContainsKey (name) && UIs[name].activeSelf;
	}

	
	public void TextDOTween(GameObject obj,string str,float num = 6.0f)
    {
		Text text = obj.GetComponent<Text>();
		TextDOTween(text, str, num);
	}
	public void TextDOTween(Text text, string str, float num = 6.0f,object tweenCallback = null)
	{
        if (tweenCallback == null)
        {
			text.DOText(str, num);
		}
        else
        {
			
		}
	}
	public void TextDOFade(Text text, float num =1,float time =1, object tweenCallback = null)
    {
		if (tweenCallback == null)
		{
			text.DOFade(num, time);
        }
        else
        {
			
		}

	}
	public void TextDOFade(Text text, float initial = 1, float num = 1, float time = 1, object tweenCallback = null)
	{

		text.color = new Color(text.color.r, text.color.g, text.color.b, initial);

		if (tweenCallback == null)
		{
			text.DOFade(num, time);
		}
		else
		{
			
		}

	}
	/// <summary>
	/// Image图片渐隐渐显
	/// </summary>
	/// <param 图片="image"></param>
	/// <param 初始透明度="initial"></param>
	/// <param 目标透明度="num"></param>
	/// <param 动画时间="time"></param>
	/// <param 回调事件="tweenCallback"></param>
	public void ImageDOFade(Image image, float initial = 1 , float num = 1, float time = 1, object tweenCallback = null)
	{
		image.color = new Color(image.color.r, image.color.g, image.color.b, initial);
		if (tweenCallback == null)
		{
			image.DOFade(num, time);
		}
		else
		{
			
		}

	}
	/// <summary>
	/// UICanvas渐隐渐显
	/// </summary>
	/// <param name="canvasGroup"></param>
	/// <param name="initial">初始透明度</param>
	/// <param name="num">目标透明度</param>
	/// <param name="time">动画时间</param>
	/// <param name="tweenCallback">回调事件</param>
	public void CanvasGroupDOFade(CanvasGroup canvasGroup, float initial = 1, float num = 1, float time = 1, object tweenCallback = null)
    {
		canvasGroup.alpha = initial;
        if (tweenCallback == null)
		{
			canvasGroup.DOFade(num, time);

        }
        else
        {
			
		}

	}

	
	// 设置材质球float值
	public  void DoMatFloat(Material gameMat, string floatStr, float startVal = 0, float endVal = 1, float doTime = 2, object tweenCallback = null)
	{
		if (gameMat == null || floatStr == null)
		{
			return;
		}
		gameMat.DOKill();
		gameMat.SetFloat(floatStr, startVal);
		if (tweenCallback == null)
		{
			gameMat.DOFloat(endVal, floatStr, doTime).SetEase(Ease.Linear);
		}
		else
		{
			
		}
	}

}
