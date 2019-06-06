using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Global : MonoBehaviour
{
    public static Global instance = null;

    public MachineType m_machine_type;
    public Font mFontHwakang;
    public UguiSpriteControl mPublic;

    public Transform mCanvasTop;
    public Transform mCanvasScene;

    public bool mScreenDebug = false;
    public bool mPhoneApk = false;
    public Transform mDebugChildPos;
    public Transform mDebugChildBoxSize;
    public Transform mScriptRoot;

    void Awake()
    {
        Common.Init();

        /*
        */
        //请到 http://www.umeng.com/analytics 获取app key
        Umeng.GA.StartWithAppKeyAndChannelId("5a607363f29d985d1d0000ab", "Kimikis M2");
        //调试时开启日志 发布时设置为false
        Umeng.GA.SetLogEnabled(false);
        

#if UNITY_EDITOR
#elif UNITY_ANDROID
        
        if(!mPhoneApk)
        {
            if(1 == AndroidDataCtl.GetDataFromAndroid<int>("checkModel", "M1"))
            {
                //Debug.LogError("Global Awake() M1");
                m_machine_type = MachineType.M1;
            }
            else if(1 == AndroidDataCtl.GetDataFromAndroid<int>("checkModel", "M2"))
            {
                //Debug.LogError("Global Awake() M2");
                m_machine_type = MachineType.M2;
            }
            else if(1 == AndroidDataCtl.GetDataFromAndroid<int>("checkModel", "M3"))
            {
                //Debug.LogError("Global Awake() M3");
                m_machine_type = MachineType.M3;
            }
            else if(1 == AndroidDataCtl.GetDataFromAndroid<int>("checkModel", "M4"))
            {
                //Debug.LogError("Global Awake() M4");
                m_machine_type = MachineType.M4;
            }
            else
            {
                m_machine_type = MachineType.None;
                Application.Quit();
            }
        }
        /**/
#endif

        instance = this;
        Application.runInBackground = true;

        if (mScreenDebug)
        {
            (new GameObject()).AddComponent<ScreenDebug>();
        }

        mPublic.Init();
        ResManager.AddUguiSpriteControl("public", mPublic);
        gameObject.AddComponent<SoundManager>();
        
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        SetFrameRateHigh();
        //SceneMgr.Instance.OpenScene(ApkInfo.g_begin_scene);

        mCanvasScene.Find("scene").GetChild(0).gameObject.SetActive(true);

        Invoke("InvokeHideSplash", 0.1f);

    }
    void InvokeHideSplash()
    {
        AndroidDataCtl.DoAndroidFunc("HideSplash");
    }

	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            if(null != mDebugChildPos)
            {
                string str = "new List<Vector3>(){\n";
                foreach(Transform t in mDebugChildPos)
                {
                    str += "new Vector3( " + t.localPosition.x + "f, " + t.localPosition.y + "f, " + t.localPosition.z + "f),\n";
                }
                str += "};";
                Debug.LogError(str);
            }
            if (null != mDebugChildBoxSize)
            {
                string str = "";
                foreach (Transform t in mDebugChildBoxSize)
                {
                    str += "new Vector3( " + t.gameObject.GetComponent<BoxCollider>().size.x + "f, " + t.gameObject.GetComponent<BoxCollider>().size.y + "f, " + t.gameObject.GetComponent<BoxCollider>().size.z + "f);\n";
                }
                Debug.LogError(str);
            }
        }
        else if(Input.GetKeyDown(KeyCode.F))
        {
            if(null != mScriptRoot)
            {
                string str = "";
                

            }

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ScreenCapture.CaptureScreenshot(ApkInfo.g_begin_scene.ToString() + ".png", 0);
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            GameOverCtl.GetInstance().Show(10, null);
        }

    }


    public void SetFrameRateHigh()
    {
        Application.targetFrameRate = 36;
    }

    public void SetFrameRateLow()
    {
        Application.targetFrameRate = 20;
    }

    public void onAnswerResult(string command)
    {
        NotificationCenter.GetInstance().dispatchEvent(UIDefineEvent.AndroidCall, new M_Notification(null, "AnswerResult", command));
    }



    #region 播放按钮特效

    public void PlayBtnClickAnimation(Transform btn_tran)
    {
        StartCoroutine(TPlayBtnClickAnimation(btn_tran));
    }
    public void PlayBtnClickAnimation(Transform btn_tran, Vector3 scale)
    {
        StartCoroutine(TPlayBtnClickAnimation(btn_tran, scale));
    }
    IEnumerator TPlayBtnClickAnimation(Transform btn_tran)
    {
        Vector3 s = Vector3.one;// btn_tran.localScale;
        float sx = btn_tran.localScale.x;
        float p = 1;
        for(float i = 0; i < 1f; i += 0.2f)
        {
            if(null != btn_tran)
            {
                p = 1 - Mathf.Sin(Mathf.PI * i) * sx * 0.2f;
                btn_tran.localScale = new Vector3(p, p, 1);
            }
            yield return new WaitForSeconds(0.01f);
        }
        if(btn_tran != null)
        {
            btn_tran.localScale = s;
        }
    }
    IEnumerator TPlayBtnClickAnimation(Transform btn_tran, Vector3 scale)
    {
        for (float i = 0; i < 1f; i += 0.2f)
        {
            if (null != btn_tran)
            {
                float p = Mathf.Sin(Mathf.PI * i) * scale.x * 0.8f;
                btn_tran.localScale = new Vector3(p, p, p);
            }
            yield return new WaitForSeconds(0.01f);
        }
        if (btn_tran != null)
        {
            btn_tran.localScale = scale;
        }
    }

    public void PlayAngleAnimation(Transform tran, Vector3 default_angle, int time = 2, float speed = 0.1f, float offset = 10)
    {
        StartCoroutine(TPlayAngleAnimation(tran, default_angle, time, speed, offset));
    }
    IEnumerator TPlayAngleAnimation(Transform btn_tran, Vector3 default_angle, int time, float speed, float offset)
    {
        for (float i = 0; i < 1f; i += speed)
        {
            if (null != btn_tran)
            {
                btn_tran.localEulerAngles = default_angle + new Vector3(0, 0, Mathf.Sin(Mathf.PI * 2 * time * i) * offset);
            }
            yield return new WaitForSeconds(0.01f);
        }
        if (btn_tran != null)
        {
            btn_tran.localEulerAngles = default_angle;
        }
    }

    #endregion


    #region 智能生成创建对象代码
    string newScrcipt(Transform tran)
    {
        string str = "";

        return str;
    }
    #endregion 
}
