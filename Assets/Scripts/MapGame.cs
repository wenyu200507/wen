using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class MapGame : MonoBehaviour
{
    public Toggle toggle1;
    public Toggle toggle2;
    public Toggle toggle3;
    public GameObject language_Group;
    private Image soundImage;
    public Text infoText;
    public GameObject NextObj;
    public GameObject infoObj;
    public GameObject item_group;
    private JArray answers;
    private JArray array;
    private int[,] myArray = new int[4, 4];
    public List<GameObject> itemList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        toggle1.onValueChanged.AddListener(OnToggleLanguageeChanged);
        toggle2.onValueChanged.AddListener(OnToggleLanguageeChanged);
        toggle3.onValueChanged.AddListener(OnToggleSoundChanged);
        soundImage = toggle3.transform.Find("Background/Checkmark").GetComponent<Image>();
        ShowUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowUI()
    {
        for (int i = 0; i < item_group.transform.childCount; i++)
        {
            Destroy(item_group.transform.GetChild(i).gameObject);
        }
        //加载资源
        TextAsset obj = Resources.Load<TextAsset>("Json/data");
        if (obj != null)
        {
            Debug.Log(obj);
            JArray jArray = JArray.Parse(obj.text);

            string p1 = jArray[0].ToString();

            JObject data = JObject.Parse(p1);
   
            string coordinates = data["Activity"]["Questions"].ToString();
            array = JArray.Parse(coordinates);
            UpdateUI(true);
        }
    }

    public void UpdateItemBtnState(string name)
    {
        ItemIcon item = item_group.transform.Find(name).gameObject.GetComponent<ItemIcon>();
        int rowIndex = item.colIndex;
        int colIndex = item.rowIndex;
        for (int i = 0; i < myArray.GetLength(0); i++)
        {
            for (int j = 0; j < myArray.GetLength(1); j++)
            {
                ItemIcon itemIcon = item_group.transform.Find(myArray[i, j].ToString()).gameObject.GetComponent<ItemIcon>();
                if(i == colIndex && (j == rowIndex - 1 || j == rowIndex + 1) && infoObj.activeInHierarchy) {
                    itemIcon.SetBtnState(true);
                }
                else if (j == rowIndex && (i == colIndex - 1 || i == colIndex + 1) && infoObj.activeInHierarchy)
                {
                    itemIcon.SetBtnState(true);
                }
                else if (j == rowIndex && i == colIndex)
                {
                    itemIcon.SetBtnState(true);
                }
                else
                {
                    itemIcon.SetBtnState(false);
                }
                
            }
        }
    }

    public void UpdateState(GameObject gameObject)
    {
        int index = int.Parse(gameObject.name);
        infoText.text = RunGame.Instance.stepCount + "/" + RunGame.Instance.maxStepCount;
        if ((int)answers[RunGame.Instance.rightSetp] == index)
        {
            RunGame.Instance.rightSetp += 1;
        }
        if (RunGame.Instance.stepCount >= RunGame.Instance.maxStepCount)
        {
            NextObj.SetActive(true);
            infoObj.SetActive(false);
        }
        itemList.Add(gameObject);
        UpdateItemBtnState(gameObject.name);
        Debug.Log(myArray);
    }

    public void UpdateUI(bool clean = false)
    {
        string p2 = array[RunGame.Instance.mapLv].ToString();
        JObject data2 = JObject.Parse(p2);
        answers = JArray.Parse(data2["Body"]["answers"][0].ToString());
        AudioClip audioClip = ResManager.Instance.GetLocalSound(data2["stimulusOfQuestion"]["Body"]["item"]["audio"]["sha1"].ToString());
        RunGame.Instance.audioSource.clip = audioClip;
        RunGame.Instance.audioSource.Play();
        RunGame.Instance.maxMapLv = array.Count;
        RunGame.Instance.maxStepCount = answers.Count;
        Debug.Log(array[0].ToString());
        JArray itemList = JArray.Parse(data2["Body"]["options"].ToString());
        GameObject obj1 = ResManager.Instance.GetUIObj("UI_ItemIcon");
        for (int i = 0; i < itemList.Count; i++)
        {
            if (obj1 != null)
            {
                GameObject mo;
                if (clean)
                {
                    mo = Instantiate<GameObject>(obj1, item_group.transform);
                }
                else
                {
                    mo = item_group.transform.Find(i.ToString()).gameObject;
                }
                mo.name = i.ToString();
                mo.GetComponent<ItemIcon>().SetIcon(itemList[i]["image"]["sha1"].ToString());
                mo.GetComponent<ItemIcon>().UpdateChooseState(false);
                mo.GetComponent<ItemIcon>().SetIndex((int)itemList[i]["rowIndex"] - 1,(int)itemList[i]["colIndex"] - 1);
                myArray[(int)itemList[i]["rowIndex"] -1, (int)itemList[i]["colIndex"] -1] = i;
            }
        }
        UpdateSetp();
    }

    public void UpdateSetp()
    {
        for (int i = 0; i < answers.Count; i++)
        {
            if(i < RunGame.Instance.rightSetp)
            {
                GameObject mo  = item_group.transform.Find(answers[i].ToString()).gameObject;
                mo.GetComponent<ItemIcon>().UpdateChooseState(true);
            }
        }
        infoText.text = RunGame.Instance.stepCount + "/" + RunGame.Instance.maxStepCount;
        UpdateItemBtnState(answers[RunGame.Instance.rightSetp - 1].ToString());
    }

    public void CloseBtn()
    {
        UIManager.Instance.ShowUI("UI_Confirm");
    }

    public void FindBtn()
    {
        UIManager.Instance.ShowTips("功能暂未开启");
    }

    public void UpdateBtn()
    {
        RunGame.Instance.rightSetp = 1;
        RunGame.Instance.stepCount = 1;
        NextObj.SetActive(false);
        infoObj.SetActive(true);
        PlayAni();
    }

    public void NextBtn()
    {
        if (RunGame.Instance.mapLv >= RunGame.Instance.maxMapLv - 1 && RunGame.Instance.rightSetp == RunGame.Instance.maxStepCount)
        {
            UIManager.Instance.ShowTips("恭喜已经全部通关");
        }
        else if (RunGame.Instance.gameCount > 2)
        {
            NextObj.SetActive(true);
            infoObj.SetActive(false);
            PlayAni();
        }
        else if(RunGame.Instance.rightSetp < answers.Count)
        {
            RunGame.Instance.gameCount += 1;
            RunGame.Instance.stepCount = RunGame.Instance.rightSetp;
            NextObj.SetActive(false);
            infoObj.SetActive(true);
            for (int i = 0; i < itemList.Count; i++)
            {
                if(RunGame.Instance.rightSetp - 2< i)
                {
                    var obj = itemList[i];
                    obj.GetComponent<ItemIcon>().PlayAinError();
                }
            }
            PlayAni();
        }
        else
        {
            RunGame.Instance.rightSetp = 1;
            RunGame.Instance.gameCount = 1;
            RunGame.Instance.stepCount = 1;
            RunGame.Instance.mapLv += 1;
            NextObj.SetActive(false);
            infoObj.SetActive(true);
            itemList.Clear();
            UpdateUI();
        }
        
    }

    void PlayAni()
    {
         var obj = itemList[itemList.Count-1];
         obj.GetComponent<ItemIcon>().PlayLysisAni(1,true);
    }

    public void FinishMove()
    {
        if (RunGame.Instance.rightSetp >= itemList.Count)
        {
            itemList.RemoveAt(itemList.Count - 1);
            if (RunGame.Instance.gameCount <= 2)
            {
                UpdateUI();
            }
            else if(RunGame.Instance.rightSetp == RunGame.Instance.maxStepCount)
            {
                RunGame.Instance.gameCount = 1;
                UpdateUI();
            }
            else
            {
                GameObject obj = item_group.transform.Find(answers[RunGame.Instance.rightSetp].ToString()).gameObject;
                obj.GetComponent<ItemIcon>().ClickBtn();
            }
        }
        else
        {
            itemList.RemoveAt(itemList.Count - 1);
            PlayAni();
        }
       
    }

    void OnToggleLanguageeChanged(bool isOn)
    {
        Debug.Log("Toggle is " + (isOn ? "ON" : "OFF"));
        // 根据Toggle的状态执行相应的操作
        if (isOn)
        {
            UIManager.Instance.ShowTips("暂无中文语言切换");
        }
        else
        {
            UIManager.Instance.ShowTips("暂无英文语言开启");
        }
    }

    void OnToggleSoundChanged(bool isOn)
    {
        Debug.Log("Toggle is " + (isOn ? "ON" : "OFF"));
        // 根据Toggle的状态执行相应的操作
        if (isOn)
        {
            // 开启功能
            UIManager.Instance.SetUIImage(soundImage, "icon_volumep_3016");
            RunGame.Instance.audioSource.Play();
        }
        else
        {
            // 关闭功能
            UIManager.Instance.SetUIImage(soundImage, "icon_mute_3017");
            RunGame.Instance.audioSource.Stop();
        }
    }
}
