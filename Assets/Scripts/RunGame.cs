using UnityEngine;
using System;

public class RunGame : MonoBehaviour {
    public int targetFrameRate = 60;
    public float deltaTime = 0.0f;
    private float m_FPS = 0.0f;

    public float fpsMeasuringDelta = 2.0f;
    public int TargetFrame = 30;

    private float timePassed;
    private int m_FrameCount = 0;

    public int stepCount = 1;
    public int mapLv = 0;
    public int maxStepCount = 1;//关卡步骤数量
    public int maxMapLv = 1;//关卡中小关数量
    public int rightSetp = 1;//正确步骤
    public int gameCount = 1;
    public AudioClip audioClip;
    public AudioSource audioSource;

    // Use this for initialization
    public static RunGame Instance { get; set; }//单例

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    void Start()
    {

        audioClip = ResManager.Instance.GetLocalSound("17cd124bc2.mp3");
        UIManager.Instance.ShowUI("UI_MapGame");
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
	{
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            //游戏调用自身的退出对话框，点击确定后，调用QuickSDK的exit()方法
            UIManager.Instance.ShowUI("UI_Confirm");
        }

        m_FrameCount = m_FrameCount + 1;
        timePassed = timePassed + Time.deltaTime;

        if (timePassed > fpsMeasuringDelta)
        {
            m_FPS = m_FrameCount / timePassed;

            timePassed = 0.0f;
            m_FrameCount = 0;
        }
    }

    void OnGUI()
    {
        /*if (GUI.Button(new Rect(0, 300, 100, 50), "ShowGM"))
        {
            if (UIManager.Instance.GetUI("NewBattleUI"))
            {
                if (cubu == null){cubu = GameObject.Find("cubu"); }
                cubu.SetActive(!cubu.activeSelf);
            }
            else
            {
                UIManager.Instance.Push("UI_GM");
            }
        }
        GUIStyle bb = new GUIStyle();
        bb.normal.background = null;
        bb.normal.textColor = new Color(1.0f, 0.5f, 0.0f);
        bb.fontSize = 32;*/

        //居中显示FPS
        //GUI.Label(new Rect(0, 0, 200, 200), "FPS: " + m_FPS + "FR:" + Application.targetFrameRate + "Rnd:" + OnDemandRendering.renderFrameInterval, bb);
    }

    public void exit()
    {
        Application.Quit();
    }
}
