using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour
{
    public GameObject chooseObject;
    public Image btnImage;
    public MapGame mapGame;
    public int step = -1;
    public int rowIndex;
    public int colIndex;
    // Start is called before the first frame update
    void Start()
    {
        mapGame = UIManager.Instance.canvas.transform.Find("UI_MapGame(Clone)").GetComponent<MapGame>();
        btnImage = gameObject.transform.Find("Button").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIndex(int x,int y)
    {
        rowIndex = x;
        colIndex = y;
    }

    public void ClickBtn()
    {
        if (chooseObject.activeInHierarchy || RunGame.Instance.stepCount >= RunGame.Instance.maxStepCount)
        {
            return;
        }
        PlayAniState(btnImage.gameObject);
        RunGame.Instance.stepCount += 1;
        UpdateChooseState(true);
        mapGame.UpdateState(gameObject);
        if(RunGame.Instance.gameCount > 2)
        {
            PlayLysisAni(0, true);
        }
        else
        {
            PlayLysisAni(0, false);
        }
    }

    public void SetIcon(string name)
    {
        if(btnImage == null)
        {
            btnImage = gameObject.transform.Find("Button").GetComponent<Image>();
        }
        UIManager.Instance.SetUIImage(btnImage, name);
    }

    public void PlayAinError()
    {
        Sequence mySequence = DOTween.Sequence();
        Vector3 max = new Vector3(0, 0, 30);
        Vector3 min = new Vector3(0, 0, -30);
        Tweener move1 = btnImage.transform.DORotate(max, 0.15f);
        Tweener move2 = btnImage.transform.DORotate(min, 0.15f);
        Tweener move3 = btnImage.transform.DORotate(new Vector3(0, 0, 0), 0.15f);
        mySequence.Append(move1);
        mySequence.Append(move2);
        mySequence.Append(move3);
        mySequence.SetLoops(3);
    }
    void IconFinishMove()
    {
        mapGame.FinishMove();
    }

    public void PlayLysisAni(int value,bool needCallBack)
    {
        Sequence mySequence = DOTween.Sequence();
        Material material;
        Image image = chooseObject.GetComponent<Image>();
        if (image.material.name.Contains("(Clone)"))
        {
            material = image.material;
        }
        else
        {
            material = GameObject.Instantiate(image.material);
            image.material = material;
        }
        material.DOKill();
        if (value == 1)
        {
            material.SetFloat("_DissolveAmount", 0);
        }
        else
        {
            material.SetFloat("_DissolveAmount", 1);
        }
        Tweener move1 = material.DOFloat(value, "_DissolveAmount",1f);
        mySequence.Append(move1);
        if (needCallBack)
        {
            mySequence.onComplete = IconFinishMove;
        }
    }

    public void UpdateChooseState(bool isTrue)
    {
        if (isTrue)
        {
            step = RunGame.Instance.stepCount;
        }
        else
        {
            step = -1;
            SetBtnState(false);
        }
        chooseObject.SetActive(isTrue);
    }

    public void SetBtnState(bool enable)
    {
        if (chooseObject.activeInHierarchy)
        {
            enable = true;
        }
        gameObject.transform.Find("Button").GetComponent<Button>().enabled = enable;
        Color currentColor = btnImage.color;
        currentColor.a = enable ? 1 : 0.5f;
        btnImage.color = currentColor;
        if (enable && !chooseObject.activeInHierarchy)
        {
            PlayAniState(btnImage.gameObject);
        }
    }

    public void PlayAniState(GameObject gameObject)
    {
        Sequence mySequence = DOTween.Sequence();
        Vector3 max = new Vector3(1.2f, 1.2f, 1.2f);
        Vector3 min = new Vector3(1, 1, 1);
        Tweener move1 = gameObject.transform.DOScale(max, 0.75f);
        Tweener move2 = gameObject.transform.DOScale(min, 0.75f);
        mySequence.Append(move1);
        mySequence.Append(move2);
    }
}
