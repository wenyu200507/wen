using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TempTip : MonoBehaviour {

    void Awake()
    {
        transform.localPosition = new Vector3(0,0,0);
    }
    public void ShowTips(string tip,Transform uiRoot)
    {
        UIManager.Instance.tips.Add(tag);
        int count = UIManager.Instance.tips.Count * 5;
		GetStringLength (tip);

		Sequence mySequence = DOTween.Sequence();
		Vector3 max = new Vector3(1.2f, 1.2f, 1.2f);
		Vector3 min = new Vector3(1, 1, 1);
		Tweener move1 = transform.DOScale(max, 0.15f );
		Tweener move2 = transform.DOScale(min, 0.25f );
		mySequence.Append(move1);
		mySequence.Append(move2);
		mySequence.onComplete = ShowNext;

	}
    void FinishMove()
    {
        UIManager.Instance.tips.Remove(tag);
        transform.gameObject.SetActive(false);
        Destroy(transform.gameObject);
    }

	void ShowNext()
	{
		Invoke("FinishMove", 0.6f);
	}

	void GetStringLength(string tip)
	{
		int len = 0;		
		for (int i = 0; i < tip.Length; i++)
		{
			if ((int)tip [i] > 127)
				len += 2;
			else
				len += 1;
		}
		int slen = Convert.ToInt32(Math.Ceiling (len / 28.0));
        transform.Find("Image_Bg").GetComponent<RectTransform>().sizeDelta = new Vector2(700, 50 + slen * 30);
        transform.Find("Text").GetComponent<RectTransform>().sizeDelta = new Vector2(700, 50 + slen * 30);
        transform.Find("Text").GetComponent<Text>().text = tip;
	}
}
