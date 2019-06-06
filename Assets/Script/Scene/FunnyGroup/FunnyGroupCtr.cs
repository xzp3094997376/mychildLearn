using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FunnyGroupCtr : MonoBehaviour {
    public static List<int> angelList { get; set; }
    private static string mMachine = "m2";
    public static FunnyGroupCtr instance = null;
    public static float mR = 250;
    public static float mD = 150;
    public static float mN = 160;
    public static float mS = 0.05f;
    private List<ItemSpineCtr> items = new List<ItemSpineCtr>();
    private List<Vector3> mPoss { get; set; }
    private string mType { get; set; }
    private GameObject rolesGo { get; set; }
    private GameObject answerButtonGo { get; set; }
    private bool iscommand { get; set; }
    private bool cmdState { get; set; }
    private List<AnswerButton> buttons = new List<AnswerButton>();
    private bool isStarted { get; set; }
    private GameObject micGo { get; set; }
    //private Image effect { get; set; }
    private Image mMicTime { get; set; }
    private RoundCtr roundctr { get; set; }
    public SoundManager mSound { get; set; }
    private bool inPass { get; set; }
    private bool isAoto = true;
    private GameObject networlkGO { get; set; }
    public Guanka mGuanka = new Guanka();
    
    public class Guanka
    {
        public int guanka { get; set; }
        public int guanka_last { get; set; }
        public int roleNum { get; set; }
        public int allRole { get; set; }
        public Dictionary<int, int> answers = new Dictionary<int, int>();
        public int que { get; set; }
        public bool opState { get; set; }
        //是不是自动出现选择按钮
        public bool isAuto { get; set; }
        public int errTimes { get; set; }
        public bool quePart { get; set; }
        public int answerTimes { get; set; }
        public List<Vector3> mpos { get; set; }
        public string askSound1 { get; set; }
        public string askSound2 { get; set; }
        public float delayShowAnswer { get; set; }
        public Guanka()
        {
            guanka_last = 2;
        }

        public void Set(int _guanka)
        {
            guanka = _guanka;
            answers.Clear();
            que = 0;
            opState = false;
            isAuto = true;
            errTimes = 0;
            quePart = false;
            answerTimes = 0;
            delayShowAnswer = 3;
            switch (_guanka)
            {
                case 1:
                    if(Common.GetRandValue(1,10) % 2 == 0)
                    {
                        roleNum = 2;
                        answers.Add(0, 5);//分为多少组
                        answers.Add(1, 2);//每组多少人
                        mpos = getPoss(5);
                        askSound1 = "game-tips2-4-4";
                        askSound2 = "game-tips2-4-3";
                    }
                    else
                    {
                        roleNum = 5;
                        answers.Add(0, 2);
                        answers.Add(1, 5);
                        mpos = getPoss(2);
                        askSound1 = "game-tips2-4-2";
                        askSound2 = "game-tips2-4-1";
                        
                    }
                    allRole = 10;
                    mR = 250;
                    mD = 150;
                    mN = 160;
                    mS = 0.018f;
                    break;
                case 2:
                    var rand = Common.GetRandValue(1, 10) % 4;
                    if (rand == 0)
                    {
                        roleNum = 2;
                        answers.Add(0, 6);
                        answers.Add(1, 2);
                        mpos = getPoss(6);
                        askSound1 = "game-tips2-4-8";
                        askSound2 = "game-tips2-4-7";
                    }
                    else if(rand == 1)
                    {
                        roleNum = 3;
                        answers.Add(0, 4);
                        answers.Add(1, 3);
                        mpos = getPoss(4);
                        askSound1 = "game-tips2-4-12";
                        askSound2 = "game-tips2-4-11";
                    }
                    else if (rand == 2)
                    {
                        roleNum = 4;
                        answers.Add(0, 3);
                        answers.Add(1, 4);
                        mpos = getPoss(3);
                        askSound1 = "game-tips2-4-10";
                        askSound2 = "game-tips2-4-9";
                    }
                    else
                    {
                        roleNum = 6;
                        answers.Add(0, 2);
                        answers.Add(1, 6);
                        mpos = getPoss(2); 
                        askSound1 = "game-tips2-4-6";
                        askSound2 = "game-tips2-4-5";
                    }
                    allRole = 12;

                    mR = 270;
                    mD = 150;
                    mN = 140;
                    mS = 0.015f;

                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;

            }
        }
        private List<Vector3> getPoss(int num)
        {
            List<Vector3> arr = new List<Vector3>();
            float startX = 0;
            float disX = 0;
            float disY = 280;
            float startY = 180;
            if(num >= 3)
            {
                disX = GlobalParam.screen_width / 4;
                if(num == 3)
                {
                    startY = 0;
                }
                if(num == 4)
                {
                    startY = 250;
                    disY = 350;
                }
            }else
            {
                disX = GlobalParam.screen_width / 3;
                startY = 50;
            }
            startX = -GlobalParam.screen_width * 0.5f + disX;
            Vector3 startPos = new Vector3(startX, startY, 0);
            
            
            for(int i = 0;i < num; i++)
            {
                Vector3 pos = startPos + new Vector3((i % 3) * disX, -(int)(i / 3) * disY, 0);
                arr.Add(pos);
            }
            return arr;
        }
        public string[] getNumstr(int num)
        {
            string[] str = null;
            switch (num)
            {
                case 0:
                    str = new string[] { "ling" };
                    break;
                case 1:
                    str = new string[] { "yi","yige","yigeren",};
                    break;
                case 2:
                    str = new string[] { "er", "liang", "erge", "ergeren", "liangge", "lianggeren", "erzu", "erzuren", "liangzu", "liangzuren", "liangzhu", "liangzhuren" };
                    break;
                case 3:
                    str = new string[] { "san", "shan", "sange", "sansangeren", "shange", "shangeren", "sanzu", "sanzuren", "shanzu", "shanzuren" };
                    break;
                case 4:
                    str = new string[] { "si", "shi", "sige", "sigeren", "shige", "shigeren", "sizhu", "sizhuren", "shizhu", "shizhuren", "sizhu", "sizhuren", "shizhu", "shizhuren" };
                    break;
                case 5:
                    str = new string[] { "wu", "wuge", "wugeren", "wuzu", "wuzuren", "wuzhu", "wuzhuren" };
                    break;
                case 6:
                    str = new string[] { "liu", "liuge", "liugeren", "liuzu", "liuzuren", "liuzhu", "liuzhuren" };
                    break;
                case 7:
                    str = new string[] { "qi", "qige" , "qigeren", "qi" };
                    break;
                case 8:
                    str = new string[] { "ba", "ba", "ba" };
                    break;
                case 9:
                    str = new string[] { "jiu" };
                    break;
                case 10:
                    str = new string[] { "shi","si"};
                    break;
            }
            return str;
        }
        
    }
    void Awake()
    {
        mSound = gameObject.AddComponent<SoundManager>();
    }
    // Use this for initialization
    bool issetFiwi = false;
    void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        RawImage bg = UguiMaker.newRawImage("bg", transform, "funnygroup_texture", "bg", false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
       
        StartCoroutine(Tinit());
    }
    IEnumerator Tinit()
    {
        yield return new WaitForSeconds(0.5f);


        if(mMachine == "m2")
        {
            initGame();
        }else
        {
            if (!isplayBgsound)
            {
                mSound.PlayBgAsync("bgmusic_loop3", "bgmusic_loop3", 0.1f);
                isplayBgsound = true;
            }

            startGame();
        }
    }
    private void initGame()
    {
        if (!checkNet())
        {
            showNetWorlkTips();
        }
        else
        {
            issetFiwi = true; 
            startGame();
        }
    }
    private void startGame()
    {
        
        if (mMachine == "m2")
        {
            NotificationCenter.GetInstance().addEventListerner(UIDefineEvent.AndroidCall, command);
        }
        instance = this;
        setGameData(1);
    }
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && !issetFiwi && mMachine == "m2")
        {
            issetFiwi = true;
            if(null != networlkGO)
            {
                networlkGO.SetActive(false);
                KbadyCtl.instance.HideSpine(true);
            }
            initGame();
        }
    }
    private bool checkNet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("网络不可用");
            
            return false;
        }
        else
        {
            Debug.Log("网络可用");
            return true;
        }
    }
    private void showNetWorlkTips()
    {
        inPass = false;
        issetFiwi = false;
        if (null == networlkGO)
        {
            networlkGO = UguiMaker.newGameObject("networlkGO", transform);
            Image networlkbg = UguiMaker.newImage("networlkbg", networlkGO.transform, "public", "white");
            Color color = Color.black;
            color.a = 0.6f;
            networlkbg.color = color;
            networlkbg.rectTransform.sizeDelta = new Vector2(1423, 800);
            Image networlktips = UguiMaker.newImage("networlktips", networlkGO.transform, "funnygroup_sprite", "qwfz_tan_03");

            Button connect = UguiMaker.newButton("connect", networlkGO.transform, "funnygroup_sprite", "qwfz_anniu_06");
            connect.transform.localPosition = new Vector3(-131, -51, 0);
            BoxCollider boxc = connect.gameObject.AddComponent<BoxCollider>();
            boxc.size = new Vector3(220, 67, 0);

            Button exit = UguiMaker.newButton("exit", networlkGO.transform, "funnygroup_sprite", "qwfz_anniu_08");
            exit.transform.localPosition = new Vector3(119, -51, 0);
            BoxCollider boxw = exit.gameObject.AddComponent<BoxCollider>();
            boxw.size = new Vector3(220,67,0);

            KbadyCtl.Init();

            KbadyCtl.instance.PlaySpine(kbady_enum.Idle, true);
            KbadyCtl.instance.ShowSpine(new Vector3(0.5f, 0.5f, 1));
            KbadyCtl.instance.mRtranSpine.anchoredPosition = new Vector2(290,-236);
            
        }
        mSound.PlayTip("funnygroup_sound", "additional-15", 1);
        networlkGO.SetActive(true);
    }
    // Update is called once per frame
    float time = 0;
	void Update () {

        if (null != answerButtonGo && answerButtonGo.active)
        {
            time += 0.1f;
            answerButtonGo.transform.localPosition = new Vector3(0, 5 * Mathf.Sin(time), 0);
        }else
        {
            if (null != micGo && micGo.active)
            {
                time += 0.1f;
                micGo.transform.localScale = Vector3.one + (Vector3.one * 0.05f * Mathf.Sin(time));
            }
        }
        

        if (inPass) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            if (null != hits)
            {
                AnswerButton.gSelect = null;
                foreach (RaycastHit hit in hits)
                {
                    GameObject go = hit.collider.gameObject;
                    if(null != go)
                    {
                        if(go.name == "connect")
                        {
                            Debug.Log("connectNet");
                            mSound.PlayTip("funnygroup_sound", "submit_down", 1);
                            AndroidDataCtl.DoAndroidFunc("setWifi");
                            inPass = true;
                            //Application.Quit();
                        }
                        else if(go.name == "exit")
                        {
                            mSound.PlayTip("funnygroup_sound", "submit_down", 1);
                            Debug.Log("exit");
                            Screen.sleepTimeout = SleepTimeout.SystemSetting;
                            Application.Quit();
                            inPass = true;
                        }
                    }

                    AnswerButton com = hit.collider.gameObject.GetComponent<AnswerButton>();
                    if(null != com)
                    {
                        mSound.PlayTip("funnygroup_sound", "submit_down", 1);
                        //Debug.Log("daan : " + mGuanka.answers[mGuanka.que] + "; chooseNum : " + com.mNum);
                        AnswerButton.gSelect = com;
                        com.transform.localScale = 0.8f * Vector3.one;
                    }
                }
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            if (null != hits)
            {
                foreach (RaycastHit hit in hits)
                {
                    AnswerButton com = hit.collider.gameObject.GetComponent<AnswerButton>();
                    if (null != com && null != AnswerButton.gSelect)
                    {
                        if (mGuanka.answers[mGuanka.que] == AnswerButton.gSelect.mNum)
                        {
                            mSound.StopTip();
                            for (int i = 0; i < 2; i++)
                            {
                                AnswerButton button = buttons[i];
                                if(button.mNum != AnswerButton.gSelect.mNum)
                                {
                                    button.fanguolai();
                                }
                            }
                            nextQuestion();
                        }
                        else
                        {
                            //TODO 发出所选择的答案错误，另外选一个答案
                            mSound.StopTip();
                            playErroeSound(false);
                            //AnswerButton.gSelect.transform.localScale = Vector3.one;
                            //Debug.LogError("answer err");
                        }
                        break;
                    }
                }
            }
            if (null != AnswerButton.gSelect)
            {
                AnswerButton.gSelect.transform.localScale = Vector3.one;
            }

            AnswerButton.gSelect = null;
            ClickBtnUp();
        }

        if (Input.GetKeyDown(KeyCode.A))//运行
        {
            test("zou");
        }
        if (Input.GetKeyDown(KeyCode.S))//暂停
        {
            test("ting");
        }
        if (Input.GetKeyDown(KeyCode.D))//组队6人
        {
            test("tingzou");
        }
        if (Input.GetKeyDown(KeyCode.W))//组队2人
        {
            test("zouting");
        }
        if (Input.GetKeyDown(KeyCode.C))//发出错误命令
        {
            test("err times : " + mGuanka.errTimes);
        }
        if (Input.GetKeyDown(KeyCode.Q))//答题命令
        {
            test("answer msg : " + mGuanka.answerTimes);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            test("er");
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            test("san");
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            test("si");
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            test("wu");
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            test("liu");
        }
    }

    private int cmdNum { get; set; }
    //接受指令
    private void command(M_Notification _notif = null)
    {
        string smd = (_notif.data as string).ToLower();
        test(smd);
    }
    //判断指令
    private void test(string smd)
    {
        if ((!cmdState || null == smd || smd == "") && mMachine == "m2") return;

        //ScreenDebug.Log("mGuanka.cmd index : " + cmdNum + ";cmd : " + smd);
        bool isDoStop = false;
        if (mGuanka.errTimes > 2 && mType == "moving" && mGuanka.isAuto)
        {
            mGuanka.isAuto = false;
            itemsGroup(mGuanka.roleNum);
            //TODO 提示用户角色已经开始停下来的逻辑（发送答题的问题）
            string succeName = "game-tips2-4-17";
            if (Common.GetRandValue(1, 10) % 2 == 0)
            {
                succeName = "game-tips2-4-18";
            }
            mSound.PlayTip("funnygroup_sound", succeName, 1);
            mGuanka.quePart = true;
            return;
        }
        if(null != txt)
        {
            txt.text = smd;
        }
        
        
        if (mType != "stop" && (smd.Contains("ing") || smd.Contains("stop") || smd.Contains("he")))
        {
            //mGuanka.quePart = true;
            isDoStop = true;
            itemsGroup(mGuanka.roleNum);
        }
        else if (mType != "moving" && (smd.Contains("ou") || smd.Contains("ao") || smd.Contains("run") || smd.Contains("shan") || smd.Contains("fen") || smd.Contains("kai")))
        {
           // mGuanka.quePart = false;
            itemsMove();
            closeanswerButton();

        }
        else
        {
            if(mGuanka.isAuto){
                //mGuanka.errTimes++;
                //ScreenDebug.Log("mGuanka.errTimes : " + mGuanka.errTimes + ";cmd : " + smd);
            }
            
        }

        if(mType == "stop" && !isDoStop && false)//取消语音答题
        {
            //Debug.Log("smd : " + smd + ";answer num : " + mGuanka.getNumstr(mGuanka.answers[mGuanka.que]) + " ; is ok : " + smd.Contains(mGuanka.getNumstr(mGuanka.answers[mGuanka.que])));
            if (isfit(mGuanka.getNumstr(mGuanka.answers[mGuanka.que]), smd)){
                //TODO next question
                mSound.StopTip();
                mGuanka.answerTimes = 0;
                nextQuestion();
                return;
            }else
            {
                mGuanka.answerTimes++;
                
                if (mGuanka.answerTimes > 2)
                {
                    //TODO 自动显示选择按钮,并发出提示用户选择答案的逻辑
                    //mGuanka.quePart = false;
                    //stopRecord();
                    if (Common.GetRandValue(0, 10) % 2 == 0)
                    {
                        mSound.PlayTip("funnygroup_sound", "game-tips2-4-17", 1);
                    }
                    else
                    {
                        mSound.PlayTip("funnygroup_sound", "game-tips2-4-18", 1);
                    }
                    closeMic();
                    mGuanka.delayShowAnswer = 6;
                    showAnswerButton();
                }
                else
                {
                    stopRecord();
                    playErroeSound(true);
                }
            }
        }
    }

    private void playErroeSound(bool start_record)
    {
        float delay_time = 0;
        if (Common.GetRandValue(0, 10) % 2 == 0)
        {
            mSound.PlayTipList(new List<string>() { "funnygroup_sound" }, new List<string>() { "game-encourage2-4-1" });
            delay_time = 4.8f;
        }
        else
        {
            mSound.PlayTipList(new List<string>() { "funnygroup_sound" }, new List<string>() { "game-encourage2-4-2" });
            delay_time = 3.5f;
        }

        if(start_record)
        {
            Invoke("startRecord", delay_time);
        }
        else
        {
            //CancelInvoke();
        }
    }
    private bool isfit(string[] arr,string cmd)
    {
        bool boo = false;
        for(int i = 0; i < arr.Length; i++)
        {
            string str = arr[i];
            if (cmd== str)
            {
                boo = true;
                break;
            }
        }

        return boo;
    }
    //发送第二道题目
    private void nextQuestion()
    {
        mGuanka.answerTimes = 0;
        stopRecord();
        mGuanka.que++;
        inPass = true;
        if (mGuanka.que > 1)
        {
            Debug.Log("过关");
            playSucce();
            NextGame();
        }
        else
        {
            Debug.Log("设置第二个问题");
            //TODO 发送问题 问问题
            StartCoroutine(askScondQuestion());
        }
    }
    private void playSucce()
    {
        string succeName = "game-encourage2-4-3";
        if (Common.GetRandValue(1, 10) % 2 == 0)
        {
            succeName = "game-encourage2-4-4";
        }
        mSound.PlayTip("funnygroup_sound", succeName, 1);
    }
    IEnumerator askScondQuestion()
    {
        playSucce();
        yield return new WaitForSeconds(5);
        closeanswerButton();
        StartCoroutine(TDelay(askQuestion, 0.1f));
        //askQuestion();
        //yield return new WaitForSeconds(8);
        inPass = false;
        //startRecord();
    }
    private void closeMic()
    {
        stopRecord();
        micGo.SetActive(false);
    }
    //开启语音识别
    private void startRecord()
    {
        if (null != answerButtonGo && answerButtonGo.active) return;

        if (mMachine == "m3") return;

        record = true;
        micGo.SetActive(true);
        StartCoroutine(Trecording());
    }
    //关闭语音识别
    private void stopRecord()
    {
        if(mMachine == "m2")
        {
            micGo.SetActive(false);
            record = false;
            StopCoroutine(Trecording());
            resetTime();
        }
    }

    //识别的时间间隔
    private bool record { get; set; }
    IEnumerator Trecording()
    {
        while(record)
        {
            cmdState = false;
            AndroidDataCtl.DoAndroidFunc("startListenAnswer", 0);
            //effect.gameObject.SetActive(true);
            for (float j = 0; j < 1f; j += 0.02f)
            {
                mMicTime.fillAmount = j;
                yield return new WaitForSeconds(0.01f);
            }
            cmdState = true;
            AndroidDataCtl.DoAndroidFunc("stopListenAnswer");
            resetTime();
            //effect.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.5f);
        }
    }

    //下一关
    private void NextGame()
    {
        stopRecord();
        StartCoroutine(TNextGame());
    }

    IEnumerator TNextGame()
    {
        yield return new WaitForSeconds(3f);
        mGuanka.guanka++;
        AnimalItem.cleanPosStat();
        if (mGuanka.guanka > mGuanka.guanka_last)
        {
            TopTitleCtl.instance.AddStar();
            yield return new WaitForSeconds(1f);
            GameOverCtl.GetInstance().Show(2, reGame);
        }
        else
        {
            setGameData(mGuanka.guanka);
        }
    }
    //重玩
    private void reGame()
    {
        setGameData(1);
    }
    //获取随机角度
    public static float randomAngel()
    {
        return Common.GetMutexValue(10, 350, 150)[0];
    }
    //获取区域坐标系列
    public List<Vector3> getScreenPoss(int _num)
    {
        List<Vector3> arr = new List<Vector3>();
        int vNum = 5;
        int hNum = 2;
        if(_num == 12)
        {
            vNum = 4;
            hNum = 3;
        }
        float disw = GlobalParam.screen_width / vNum;
        float dish = GlobalParam.screen_height / hNum;
        float startX = -GlobalParam.screen_width * 0.5f + disw * 0.5f;
        float startY = GlobalParam.screen_height * 0.5f - dish * 0.5f;
        for (int i = 0; i < vNum; i++)
        {
            float vx = startX + disw * i;

            for (int j = 0;j < hNum; j++)
            {
                float vy = startY - dish * j;
                arr.Add(new Vector3(vx, vy, 0));
            }
        }
        return arr;
    }
    //设置游戏数据
    private void  setGameData(int guanka)
    {
        cmdNum = 0;
        inPass = false;
        iscommand = false;
        mGuanka.Set(guanka);
        mGuanka.quePart = false;
        if (guanka == 1)
        {
            TopTitleCtl.instance.Reset();
        }
        else
        {
            TopTitleCtl.instance.AddStar();
        }

        if (null != answerButtonGo)
        {
            answerButtonGo.SetActive(false);
        }

        if (null == rolesGo)
        {
            rolesGo = UguiMaker.newGameObject("rolesGo", transform);
            rolesGo.transform.localPosition = new Vector3(0, -126, 0);
            roundctr = rolesGo.AddComponent<RoundCtr>();
        }
        else
        {
            for (int i = 0; i < rolesGo.transform.childCount; i++)
            {
                GameObject.Destroy(rolesGo.transform.GetChild(i).gameObject);
            }
        }

        if (null == mMicTime && mMachine == "m2")
        {
            micGo = UguiMaker.newGameObject("micGo", transform);
            //effect = UguiMaker.newImage("effect", micGo.transform, "funnygroup_sprite", "qwfz_guang_07");
            micGo.transform.localPosition = new Vector3(-550, -315, 0);
            Image mic = UguiMaker.newImage("mic", micGo.transform, "funnygroup_sprite", "qwfz_mai_07");
            mMicTime = UguiMaker.newImage("mictime", micGo.transform, "funnygroup_sprite", "qwfz_quan_07");
            mMicTime.type = Image.Type.Filled;
            mMicTime.fillMethod = Image.FillMethod.Radial360;
            mMicTime.fillOrigin = 2;
        }
        if(mMachine == "m2")
        {
            resetTime();
            closeMic();
        }
        

        if (null != txt)
        {
            GameObject textGo = UguiMaker.newGameObject("mText", transform);
            RectTransform rect = textGo.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(1000, 100);
            txt = textGo.AddComponent<Text>();
            txt.text = "text test";
            txt.fontSize = 30;
            txt.color = Color.red;
            txt.font = Font.CreateDynamicFontFromOSFont("FZSEJW", 30);
        }
        roundctr.stop();
        mType = "moving";
        StartCoroutine(TshootRole());
    }
    //人物打散
    public void itemsMove()
    {
        if (mType == "moving") return;
        mType = "moving";
        angelList = Common.GetMutexValue(10, 350, 150);
        for (int i = 0; i < items.Count; i++)
        {
            ItemSpineCtr item = items[i];
            item.setDefaultPos();
        }
        roundctr.round();

    }
    //组团
    /**
     * n表示每个组有多少个人物
     * */
    private void itemsGroup(int _n)
    {
        if (mType == "stop") return;

        mType = "stop";

        roundctr.stop();


        for (int i = 0; i < items.Count; i++)
        {
            ItemSpineCtr item = items[i];
            item.isFix = false;
        }

        int groupNum = _n;
        int arrIndex = 0;
        itemDic.Clear();
        items = Common.BreakRank(items);
        for (int i = 0; i < items.Count; i++)
        {
            ItemSpineCtr itemFirt = items[i];

            if (!itemFirt.isFix)
            {
                pushItem(arrIndex, itemFirt);
                for (int n = 0; n < groupNum -1; n++)
                {
                    ItemSpineCtr stempItem = null;
                    for (int j = i + 1; j < items.Count; j++)
                    {
                        ItemSpineCtr itemScond = items[j];
                        float dis = 10000;
                        float disc = Vector3.Distance(itemFirt.transform.localPosition, itemScond.transform.localPosition);
                        if (disc < dis && !itemScond.isFix)
                        {
                            dis = disc;
                            stempItem = itemScond;
                        }
                    }
                    pushItem(arrIndex, stempItem);
                }
                arrIndex++;
            }
            
        }
        List<Vector3> endPoss = Common.BreakRank(getScreenPoss(mGuanka.allRole));
        float ofsetY = 0;
        for (int k = 0; k < itemDic.Keys.Count; k++)
        {
            List<ItemSpineCtr> list = itemDic[k];
            Vector3 endPos = mGuanka.mpos[k];
            if(list.Count >= 4)
            {
                ofsetY = 150;
            }
            bool isDan = false;
            if (list.Count % 2 != 0)
            {
                isDan = true;
            }
            for (int j = 0; j < list.Count; j++)
            {
                ItemSpineCtr item = list[j];
                Vector3 pos = endPos + new Vector3(-100, ofsetY, 0) + new Vector3((j % 2) * 100, -(int)(j / 2) * 180, 0);
                if (isDan && j == (list.Count - 1))
                {
                    pos = pos + new Vector3(40, 0, 0);
                    if(list.Count < 4)
                    {
                        pos = pos + new Vector3(0, 20, 0);
                    }
                }
                item.MoveTo(pos);
            }
        }
        stopRecord();

        mSound.PlayTip("funnygroup_sound", "game-tips2-4-16", 1);

        if (mGuanka.isAuto)
        {
            mGuanka.isAuto = false;
            mGuanka.quePart = true;
            StartCoroutine(TDelay(askQuestion, 3));
            //askQuestion();
            // TODO 发出问题逻辑
        }else
        {
            StartCoroutine(TDelay(askQuestion, 3));
        }

    }

    //发问题
    private void askQuestion()
    {
        if (mGuanka.que == 0)
        {
            tipsName = mGuanka.askSound1;
        }
        else
        {
            tipsName = mGuanka.askSound2;
        }
        playTips();
        TopTitleCtl.instance.mSoundTipData.SetData(playTips);
    }
    private string tipsName { get; set; }
    private void playTips()
    {
        if (mType != "stop" || inPass) return;
        stopRecord();
        mSound.StopTip();
        mSound.PlayTip("funnygroup_sound", tipsName, 1, false);
        mGuanka.delayShowAnswer = 6;
        StopCoroutine("TdelayTips");
        StartCoroutine("TdelayTips");
    }
    private Dictionary<int, List<ItemSpineCtr>> itemDic = new Dictionary<int, List<ItemSpineCtr>>();
    private void pushItem(int index, ItemSpineCtr item)
    {
        if (itemDic.ContainsKey(index))
        {
            item.isFix = true;
            itemDic[index].Add(item);
        }else
        {
            List<ItemSpineCtr> arr = new List<ItemSpineCtr>();
            arr.Add(item);
            itemDic.Add(index, arr);
        }
    }
    private Text txt { get; set; }
    
    //显示选择按钮
    private void showAnswerButton()
    {
        if (null != answerButtonGo && answerButtonGo.active) return;

        if (null == answerButtonGo)
        {
            answerButtonGo = UguiMaker.newGameObject("answerButtonGo", transform);
            Vector3 startPos = new Vector3(325, -310,0);
            for(int i = 0;i < 2; i++)
            {
                Image button = UguiMaker.newImage("button" + i, answerButtonGo.transform, "funnygroup_sprite", "qwfz_ka_" + 0);
                AnswerButton ab = button.gameObject.AddComponent<AnswerButton>();
                button.gameObject.transform.localPosition = startPos + new Vector3(i * 200, 0, 0);
                BoxCollider box = button.gameObject.AddComponent<BoxCollider>();
                box.size = new Vector3(160,160,0);
                buttons.Add(ab);
            }
        }else
        {
            answerButtonGo.SetActive(true);
        }
        for (int i = 0; i < 2; i++)
        {
            AnswerButton button = buttons[i];
            button.setData(mGuanka.answers[i],i);
        }

    }
    
    private void closeanswerButton()
    {
        if (null == answerButtonGo) return;
        answerButtonGo.SetActive(false);
    }
  
    IEnumerator TdelayTips()
    {
        yield return new WaitForSeconds(mGuanka.delayShowAnswer);
        showAnswerButton();
        startRecord();
    }
    IEnumerator TDelay(System.Action _action,float time)
    {
        yield return new WaitForSeconds(time);

        _action();
    }
    IEnumerator TshootRole()
    {
        angelList = Common.GetMutexValue(10, 350, 150);
        mPoss = Common.BreakRank(getScreenPoss(mGuanka.allRole));
        items.Clear();
        ItemSpineCtr.cleanRoadData();
        for (int i = 0; i < mGuanka.allRole; i++)
        {
            string imgName = "r" + ((i % 4) + 1);
            string spinname = "";
            Vector3 pos = Vector3.zero;
            if(Common.GetMutexValue(1,10) % 2 == 0)
            {
                spinname = "boy";
            }else
            {
                spinname = "girl";
                pos = new Vector3(-5, 5, 0);
            }
            ItemSpineCtr animalCtrs = ResManager.GetPrefab("funnygroup_prefab", spinname).GetComponent<ItemSpineCtr>();
            GameObject go = UguiMaker.InitGameObj(animalCtrs.gameObject, rolesGo.GetComponent<RectTransform>(), "animal" + i, Vector3.zero,Vector3.zero * 0.7f);
            animalCtrs.PlaySpine("Click", true);
            items.Add(animalCtrs);
            //animalCtrs.ShootIn(i);
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < mGuanka.allRole; i++)
        {
            yield return new WaitForSeconds(0.30f);
            ItemSpineCtr animalCtrs = items[i];
            GameObject go = animalCtrs.gameObject;
            go.transform.localScale = Vector3.one * 0.7f;
            animalCtrs.ShootIn(i);
        }
        roundctr.round();
        //TODO 等待指令控制打散逻辑
        yield return new WaitForSeconds(2f);
        string tips = "game-tips2-4-15";
        if (mGuanka.guanka == 1)
        {
            tips = "game-tips2-4-13";
        }
        if (mMachine == "m3")
        {
            tips = "additional-127（趣味分组）";
        }
        mSound.PlayShort("funnygroup_sound", tips, 1);
        yield return new WaitForSeconds(4f);
        iscommand = true;
        mType = "moving";
        if (isAoto && mMachine == "m2")
        {
            startRecord();
        }else
        {
            showStopButton();
        }
        
    }
    private bool isplayBgsound { get; set; }
    private Image stopCommButton { get; set; }
    private bool isDownCommButton = false;
    private void showStopButton()
    {
        if(null == stopCommButton)
        {
            stopCommButton = UguiMaker.newImage("stopCommButton", transform, "funnygroup_sprite", "play1");
            stopCommButton.rectTransform.localPosition = new Vector3(-550, -342, 0);
            EventTriggerListener.Get(stopCommButton.gameObject).onDown = ClickBtnDown;
        }else
        {
            stopCommButton.sprite = ResManager.GetSprite("funnygroup_sprite", "play1");
        }
        
        stopCommButton.enabled = true;
        isDownCommButton = false;
        GuideClick(stopCommButton.transform.position, new Vector3(6, -26, 0));
    }
    private void closeStopButton()
    {
        stopCommButton.enabled = false;
        mClickGuide.StopClick();
    }

    private void ClickBtnDown(GameObject go)
    {
        if (isDownCommButton) return;
        mSound.PlayShort("button_down");
        isDownCommButton = true;
        stopCommButton.sprite = ResManager.GetSprite("funnygroup_sprite", "play2");
    }
    private void ClickBtnUp()
    {
        if (isDownCommButton)
        {
            test("ting");
            mSound.PlayShort("button_up");
            closeStopButton();
        }
    }
    GuideHandCtl mClickGuide;
    public void GuideClick(Vector3 _worldPos, Vector3 _localOffset)
    {
        if (mClickGuide == null)
        {
            mClickGuide = GuideHandCtl.Create(transform);
        }
        mClickGuide.transform.SetSiblingIndex(60);
        mClickGuide.GuideTipClick(0.8f, 0.7f, true, true, "hand1");
        mClickGuide.SetClickTipPos(_worldPos);
        mClickGuide.SetClickTipOffsetPos(_localOffset);
    }
    private void resetTime()
    {
        mMicTime.fillAmount = 0;
    }
    
}
