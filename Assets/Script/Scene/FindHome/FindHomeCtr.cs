using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FindHomeCtr : MonoBehaviour {
    public static FindHomeCtr instance = null;
    private GameObject houseGO { get; set; }
    private GameObject lightGo { get; set; }
    private GameObject windows { get; set; }
    private Dictionary<int, WindowCtr> windowsDic = new Dictionary<int, WindowCtr>();
    public SoundManager mSound { get; set; }
    private InputNumObj inputobj { get; set; }
    private WindowCtr currWindow { get; set; }
    private string currType { get; set; }
    ///private Image role { get; set; }
    private GameObject roleGo { get; set; }
    private RoleSpine animalrole { get; set; }
    private bool misInPass { get; set; }
    private Image bg { get; set; }
    private int distance = 60;
    public static List<int> animalNames { get; set; }
    public Guanka mGuanka = new Guanka();

    public class Guanka
    {
        public int guanka { get; set; }
        public int guanka_last { get; set; }
        public List<int> indexs { get; set; }
        private Dictionary<int, Vector3> floors = new Dictionary<int, Vector3>();
        public float placeY { get; set; }
        public int currIndex { get; set; }
        public int okTimes { get; set; }

        public Guanka()
        {
            guanka_last = 2;
            //y坐标 索引起点 索引结束点
            floors.Add(0, new Vector3(800, 0, 11));
            floors.Add(1, new Vector3(608, 4, 15));
            floors.Add(2, new Vector3(359, 8, 19));
            floors.Add(3, new Vector3(130, 12, 23));
            floors.Add(4, new Vector3(-160, 16, 23));
        }

        public void Set(int _guanka)
        {
            guanka = _guanka;
            animalNames = Common.BreakRank(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 });
            switch (_guanka)
            {
                case 1:
                    break;
                case 2:
                    okTimes = 0;
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }
        public void setscondList(int index)
        {
            currIndex = index;
            placeY = floors[currIndex].x;
            int min = (int)floors[currIndex].y;
            int max = (int)floors[currIndex].z;
            indexs = Common.BreakRank(Common.GetMutexValue(min, max, 4));
        }
    }
    void Awake()
    {
        mSound = gameObject.AddComponent<SoundManager>();
    }
    // Use this for initialization
    void Start () {
        bg = UguiMaker.newImage("bg", transform, "findhome_sprite", "bg");
        bg.type = Image.Type.Sliced;
        bg.raycastTarget = false;
        bg.rectTransform.sizeDelta = new Vector2(1423, 1600);

        GameObject startGo = UguiMaker.newGameObject("startGo", bg.transform);
        //Image star = UguiMaker.newImage("star", startGo.transform, "findhome_sprite", "star");
        //star.rectTransform.localPosition = new Vector3(0, 250, 0);

        GameObject star = ResManager.GetPrefab("findhome_prefab", "star");
        star = UguiMaker.InitGameObj(star, transform, "star", Vector3.zero, Vector3.one);
        star.transform.parent = startGo.transform;

        Image moon = UguiMaker.newImage("moon", startGo.transform, "findhome_sprite", "moon");
        moon.rectTransform.localPosition = new Vector3(-556, 312, 0);

        houseGO = UguiMaker.newGameObject("houseSpace", transform);
        Canvas canvas = houseGO.AddComponent<Canvas>();
        houseGO.gameObject.layer = LayerMask.NameToLayer("UI");
        canvas.overrideSorting = true;
        canvas.sortingOrder = 2;
        houseGO.transform.localPosition = new Vector3(0, -850, 0);
        Image housetop = UguiMaker.newImage("housetop", houseGO.transform, "findhome_sprite", "house_top");
        housetop.rectTransform.localPosition = new Vector3(0, 0, 0);
        housetop.rectTransform.sizeDelta = new Vector2(1030, 880);
        Image housebotton = UguiMaker.newImage("housebotton", houseGO.transform, "findhome_sprite", "house_botton");
        housebotton.rectTransform.localPosition = new Vector3(0, -800, 0);

        windows = UguiMaker.newGameObject("windows", houseGO.transform);

        animalNames = Common.BreakRank(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 });
        StartCoroutine(TcreateView());
        //SoundManager.instance.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.1f);
        instance = this;
    }
	
	// Update is called once per frame
	void Update () {

        if (misInPass) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            if (null != hits && (null == inputobj || !inputobj.gameObject.activeSelf))//防止在计算器出现的时候，点击其他窗户的问号
            {
                foreach (RaycastHit hit in hits)
                {
                    GameObject com = hit.collider.gameObject;
                    if (null != com)
                    {
                        int select = 0;
                        if(com.name == "wenred")
                        {
                            showInput(com,"left", transform.InverseTransformPoint(com.GetComponent<Transform>().position) + new Vector3(200,-200,0));
                            select = -1;
                        }
                        if (com.name == "wenblack")
                        {
                            showInput(com, "right", transform.InverseTransformPoint(com.GetComponent<Transform>().position) + new Vector3(100, -200, 0));
                            select = 1;
                        }

                        if(0 != select)
                        {
                            Vector3 pos = transform.worldToLocalMatrix.MultiplyPoint(com.transform.parent.parent.position);
                            if(pos.y - 199 > -277)
                            {
                                inputobj.transform.localPosition = pos + new Vector3(0, -199, 0);
                            }
                            else
                            {
                                if(select == -1)
                                    inputobj.transform.localPosition = pos + new Vector3(-168, 0, 0);
                                else
                                    inputobj.transform.localPosition = pos + new Vector3(168, 0, 0);
                            }
                            timerun();
                            break;
                        }

                    }
                }
            }
        }

    }
    //设置游戏数据
    private void setGameData(int guanka)
    {
        mGuanka.Set(guanka);
        misInPass = false;

        if (timeGo.active)
        {
            timeGo.SetActive(false);
        }
        
        if (guanka == 1)
        {
            TopTitleCtl.instance.Reset();
            mGuanka.setscondList(0);
            StartCoroutine("THouseMove");
        }
        else
        {
            TopTitleCtl.instance.AddStar();
            randomGameTowData();
        }
    }
    private void randomGameTowData()
    {
        int curr = Common.GetRandValue(1, 4);
        while(curr == mGuanka.currIndex)
        {
            curr = Common.GetRandValue(1, 4);
        }
        mGuanka.setscondList(curr);
        StartCoroutine("THouseMove");
    }
    private void showInput(GameObject go,string type,Vector3 pos)
    {
        currWindow = go.transform.parent.parent.gameObject.GetComponent<WindowCtr>();
        currType = type;

        if (null == inputobj)
        {
            InputInfoData data = new InputInfoData();
            data.nConstraintCount = 3;
            data.fNumScale = 3f;
            data.fscale = 0.3f;
            data.vBgSize = new Vector2(665,575);
            data.vCellSize = new Vector2(200, 166);
            data.vSpacing = new Vector2(10, 10);
            data.bgcolor = new Color32(252, 229, 194, 255);
            data.color_blockBG = new Color32(179, 138, 89, 255);
            data.color_blockNum = new Color32(202, 183, 155, 255);
            data.color_blockSureBG = new Color32(172, 123, 66, 255);
            data.color_blockSureStart = new Color32(202, 183, 155, 255);

            inputobj = InputNumObj.Create(transform, data);
            Canvas canvas = inputobj.gameObject.AddComponent<Canvas>();
            inputobj.gameObject.layer = LayerMask.NameToLayer("UI");
            canvas.overrideSorting = true;
            canvas.sortingOrder = 11;
            inputobj.SetInputNumberCallBack(getNumfromInputNumObj);
            inputobj.SetClearNumberCallBack(CleanInputNum);
            inputobj.transform.localPosition = new Vector3((GlobalParam.screen_width) * 0.5f - 180, 0, 0);
            GraphicRaycaster gry = inputobj.gameObject.AddComponent<GraphicRaycaster>();
            gry.ignoreReversedGraphics = true;

        }
        else
        {
            inputobj.ShowEffect();
        }

    }
    //clean当前的输入
    private void CleanInputNum()
    {
        currWindow.setInpudata(inputobj.strInputNum, currType);
    }
    //获取选择的数字
    private void getNumfromInputNumObj()
    {
        currWindow.setInpudata(inputobj.strInputNum, currType);
        inputobj.HideEffect();
        finishInput();
    }
    
    public void playEffectSound(string soundName,float val = 1)
    {
        mSound.PlayTip("findhome_sound", soundName, val);
    }
    //关闭输入板回调
    private void finishInput()
    {
        bool boo = currWindow.checkFinish();
       
        if (boo)
        {
            currWindow.setWenBgCorrect();
            string animal_name = "";
            switch (currWindow.id_animal)
            {
                case 0:
                    animal_name = "熊";
                    break;
                case 1:
                    animal_name = "公鸡";
                    break;
                case 2:
                    animal_name = "牛";
                    break;
                case 3:
                    animal_name = "鸭子";
                    break;
                case 4:
                    animal_name = "鹅";
                    break;
                case 5:
                    animal_name = "马";
                    break;
                case 6:
                    animal_name = "豹子";
                    break;
                case 7:
                    animal_name = "猫头鹰";
                    break;
                case 8:
                    animal_name = "猪";
                    break;
                case 9:
                    animal_name = "鸽子";
                    break;
                case 10:
                    animal_name = "兔子";
                    break;
                case 11:
                    animal_name = "羊";
                    break;
                case 12:
                    animal_name = "燕子";
                    break;
                case 13:
                    animal_name = "老虎";
                    break;
            }
            //TODO 播放当前窗口牌号
            mSound.StopTip();
            mSound.PlayShort("aa_animal_sound", animal_name + "0",1);
            if(mGuanka.guanka == 1)
            {
                List<string> soundnames = new List<string>() { animal_name, "game-tips6-1-3", currWindow.mNumH.ToString(), "game-tips6-1-4", currWindow.mNumV.ToString(), "game-tips6-1-5" };
                List<string> soundab = new List<string>() { "aa_animal_name", "findhome_sound", "findhome_sound", "findhome_sound", "findhome_sound", "findhome_sound" };
                mSound.PlayTipList(soundab, soundnames);
            }
            currWindow.open();
            if (mGuanka.guanka == 2)
            {
                if (mGuanka.okTimes >= 2)
                {
                    //做延迟弹框
                    StopCoroutine("TDelay");
                    StopCoroutine("THouseMove");
                    misInPass = true;
                    StartCoroutine(TFinishGame());
                }
                else
                {
                    mGuanka.okTimes++;
                }
                return;
            }

            for (int i = 0;i < 4; i++)
            {
                WindowCtr window = windowsDic[mGuanka.indexs[i]];
                if (!window.mState)
                {
                    boo = false;
                    break;
                }
            }
            StartCoroutine(Tclosedoor(boo));
        }
        
    }
    IEnumerator TFinishGame()
    {
        if(mGuanka.guanka != 2)
        {
            yield return new WaitForSeconds(5f);
        }
        mSound.PlayTip("findhome_sound", "game-tips6-1-1" + (Common.GetRandValue(0,10) % 2), 1);

        yield return new WaitForSeconds(5f);
        TopTitleCtl.instance.AddStar();
        yield return new WaitForSeconds(1f);
        GameOverCtl.GetInstance().Show(2, reGame);
        cleanAfterQuestion();
    }
    public void playError()
    {
        mSound.PlayTip("findhome_sound", "game-tips6-1-9", 1);
    }
    IEnumerator Tclosedoor(bool boo)
    {
        yield return new WaitForSeconds(5);
        misInPass = false;

        if (boo)
        {
            mGuanka.guanka++;
            cleanAfterQuestion();
            setGameData(mGuanka.guanka);
        }
    }
    private void reGame()
    {
        setGameData(1);
    }
    private void cleanAfterQuestion()
    {
        for (int i = 0; i < 4; i++)
        {
            WindowCtr window = windowsDic[mGuanka.indexs[i]];
            window.close();
            window.closeDoor();
        }
    }
    private void setQuestion()
    {
        StartCoroutine(TsetQuestion());
    }
    private Vector2 getLightData(int index)
    {
        Dictionary<int, Vector2> arr = new Dictionary<int, Vector2>();
        arr.Add(16, new Vector2(16,1.4f));
        arr.Add(17, new Vector2(360, 1.6f));
        arr.Add(18, new Vector2(345, 2f));
        arr.Add(19, new Vector2(335, 2.3f));
        return arr[index];
    }

   

    //房子移动
    IEnumerator THouseMove()
    {
        if(null != inputobj) inputobj.HideEffect();

        mGuanka.okTimes = 0;
        playEffectSound("houth_move");
        closeTimeGo();
        Vector3 starPos = houseGO.transform.localPosition;
        Vector3 endPos = new Vector3(0, mGuanka.placeY, 0);
        Vector3 startbg = Vector3.zero;
        Vector3 endbg = new Vector3(0, 400, 0);
        for (float i = 0; i < 1f; i += 0.02f)
        {
            houseGO.transform.localPosition = Vector3.Lerp(starPos, endPos, i);
            bg.transform.localPosition = Vector3.Lerp(startbg, endbg, i);
            yield return new WaitForSeconds(0.01f);
        }
       
        houseGO.transform.localPosition = endPos;
        bg.transform.localPosition = endbg;
        if (mGuanka.guanka == 1)
        {
            StartCoroutine(TroleRun());//TLightMove
        }
        else
        {
            setQuestion();
        }
        
    }
    private void closeLight()
    {
        lightGo.SetActive(false);
    }

    IEnumerator TsetQuestion()
    {
        misInPass = true;
        for (int i = 0; i < 4; i++)
        {
            WindowCtr window = windowsDic[mGuanka.indexs[i]];
            if (null == window)
            {
                continue;
            }
            
            if (i == 0)
            {
                window.showNum(true, true, true);
                window.setState(true);
                window.open();
            }
            else
            {
                window.showNum(true, false, false);
                window.setState(false);
                window.setWenBgCorrectde();
            }
            playEffectSound("window_open");
            yield return new WaitForSeconds(0.5f);
            
        }
        mSound.PlayTip("findhome_sound", "game-tips6-1-1", 1, true);

        if(mGuanka.guanka == 2)
        {
            showTimeGo();
        }else
        {
            misInPass = false;
        }
        misInPass = false;
        timeIsRun = false;
        yield return new WaitForSeconds(4f);

        timerun();
    }
    private void timerun()
    {
        if (mGuanka.guanka == 2 && !timeIsRun)
        {
            StartCoroutine("TDelay");

        }
    }
    private bool timeIsRun = false;
    //间隔时间
    IEnumerator TDelay()
    {
        timeIsRun = true;

        for (int i = distance; i >= 0; i--)
        {
            setTimeNum(i);
            if(i <= 5)
            {
                AudioClip cp0 = Resources.Load<AudioClip>("sound/倒计时");
                acnew.PlaySpine("Click", false);
                if (cp0 != null)
                {
                    AudioSource.PlayClipAtPoint(cp0, Camera.main.transform.position);
                }
            }
            yield return new WaitForSeconds(1f);
        }

        timeIsRun = false;
        if (mGuanka.okTimes >= 2) yield return null;

        cleanAfterQuestion();
        randomGameTowData();

    }
    private void setTimeNum(int num)
    {
        if (!timeGo.active)
        {
            timeGo.SetActive(true);
        }
        txt.text = num.ToString();
        //timeImg.sprite = ResManager.GetSprite("findhome_sprite", "time_" + num);
    }
    private GameObject timeGo { get; set; }
    private RoleSpine acnew { get; set; }
    //private Image timeImg { get; set; }
    private Text txt { get; set; }
    IEnumerator TcreateView()
    {
        mSound.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.1f);
        //mSound.PlayShort("findhome_sound", "game-name6-1", 1);

        for (int i = 23; i >= 0; i--)//0
        {
            GameObject go = UguiMaker.newGameObject("window" + i, windows.transform);
            WindowCtr window = go.AddComponent<WindowCtr>();
            window.setData(i, (int)(i / 4) + 1, (i % 4) + 1);
            go.transform.localPosition = new Vector3(-310, -1024, 0) + new Vector3((i % 4) * 200, (int)(i / 4) * 250, 0);
            windowsDic.Add(i, window);
            window.closeDoor(false);
        }
        timeGo = UguiMaker.newGameObject("timeGo", transform);
        //Image tbg = UguiMaker.newImage("tbg", timeGo.transform, "findhome_sprite", "timebg");
        acnew = ResManager.GetPrefab("findhome_prefab", "clok").GetComponent<RoleSpine>();
        GameObject clok  = UguiMaker.InitGameObj(acnew.gameObject, timeGo.transform, "star", new Vector3(5,-80,0), Vector3.one);
        GameObject textGo = UguiMaker.newGameObject("mText", timeGo.transform);
        RectTransform rect = textGo.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(90, 90);
        txt = textGo.AddComponent<Text>();
        txt.alignment = TextAnchor.MiddleCenter;
        txt.text = distance.ToString();
        txt.fontSize = 60;
        txt.color = Color.red;
        txt.font = Font.CreateDynamicFontFromOSFont("FZSEJW", 60);
        timeGo.SetActive(false);
        // timeImg = UguiMaker.newImage("timeImg", timeGo.transform, "findhome_sprite", "time_10");
        timeGo.transform.localPosition = new Vector3(552, 0, 0);
        yield return new WaitForSeconds(3f);
        setGameData(1);
    }
    private void closeTimeGo()
    {
        misInPass = true;
        if (timeGo.active)
        {
            StartCoroutine(TMoveTimeGo(new Vector3(552, 0, 0), new Vector3(852, 0, 0), "out"));
        }
    }
    //出现倒计时
    private void showTimeGo()
    {
        if(null != timeGo)
        {
            timeGo.SetActive(true);
            StartCoroutine(TMoveTimeGo(new Vector3(852, 0, 0), new Vector3(552, 0, 0), "in"));
        }else
        {
            misInPass = false;
        }
    }
    IEnumerator TMoveTimeGo(Vector3 startPos, Vector3 endPos,string type)
    {
        float speed = 0.1f;
        if(type == "out")
        {
            speed = 0.2f;
        }else
        {
            txt.text = distance.ToString();
        }
        for (float i = 0; i < 1f; i += speed)
        {
            timeGo.transform.localPosition = Vector3.Lerp(startPos, endPos, i);
            yield return new WaitForSeconds(0.01f);
        }
        if(type == "out")
        {
            misInPass = true;
            timeGo.transform.localPosition = endPos - new Vector3(10, 0, 0);
            txt.text = distance.ToString();
        }
        else
        {
            timeGo.transform.localPosition = endPos + new Vector3(10, 0, 0);
        }
    }
    IEnumerator TroleRun()
    {
        if(null == animalrole)
        {
            animalrole = ResManager.GetPrefab("findhome_prefab", "role").GetComponent<RoleSpine>();
            roleGo = UguiMaker.InitGameObj(animalrole.gameObject, transform, "role", new Vector3(-600, -384, 0), Vector3.one);
            Canvas canvas = roleGo.AddComponent<Canvas>();
            roleGo.gameObject.layer = LayerMask.NameToLayer("UI");
            canvas.overrideSorting = true;
            canvas.sortingOrder = 2;
            roleGo.transform.localPosition = new Vector3(-1800, -370, 0);
            animalrole.PlaySpine("Go", true);
            yield return new WaitForSeconds(0.2f);
        }
        roleGo.SetActive(true);
        mSound.PlayShort("findhome_sound", "role_run",1.5f);
        Vector3 startPos = new Vector3(-660, -370, 0);
        Vector3 endPos = new Vector3(-437, -370, 0);
        for (float i = 0; i < 1f; i += 0.01f)
        {
            roleGo.transform.localPosition = Vector3.Lerp(startPos, endPos, i);
            yield return new WaitForSeconds(0.01f);
        }
        roleGo.transform.localPosition = endPos;
        setQuestion();
        animalrole.PlaySpine("Illumination",true);
        yield return new WaitForSeconds(5f);
        animalrole.PlaySpine("Go", true);
        yield return new WaitForSeconds(1f);
        mSound.PlayShort("findhome_sound", "role_run", 1.5f);
        roleGo.transform.Rotate(0, 180, 0);
        roleGo.transform.localPosition -= new Vector3(150, 0, 0);
        startPos = roleGo.transform.localPosition;
        endPos = new Vector3(-660 - 150, -370, 0);

        for (float i = 0; i < 1f; i += 0.01f)
        {
            roleGo.transform.localPosition = Vector3.Lerp(startPos, endPos, i);
            yield return new WaitForSeconds(0.01f);
        }
        roleGo.transform.localPosition = endPos;
        animalrole = null;
        GameObject.Destroy(roleGo);
    }
    /*
    IEnumerator TLightMove()
    {
        lightGo.SetActive(true);
        Vector2 ve2 = getLightData(16);
        for (int i = 0; i < 25; i += 1)
        {
            int index = i % 25;
            if(index == 0)
            {
                lightGo.transform.localScale = new Vector3(ve2.y, ve2.y, 0);
            }
            lightGo.transform.localEulerAngles = new Vector3(0,0,-index);//Vector3.Lerp(endeu,new Vector3(0,0,60), i);
            lightGo.transform.localScale += new Vector3(index / 400f, index / 380f,  0);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        lightGo.SetActive(false);
        setQuestion();
    }
    */
}
