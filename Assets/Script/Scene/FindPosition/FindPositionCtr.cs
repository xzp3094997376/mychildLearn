using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class FindPositionCtr : MonoBehaviour {

    public Guanka mGuanka = new Guanka();
    public SoundManager mSound { get; set; }
    public PlaySoundController mmPSCtrl { get; set; }
    private RawImage bg { get; set; }


    private GameObject positionGo { get; set; }

    private GameObject lianziGo { get; set; }
    private Image lianzi1 { get; set; }
    private Image lianzi2 { get; set; }

    private GameObject demoGo { get; set; }
    private GameObject demoPhoto { get; set; }
    private Image demopositionbg { get; set; }
    private Image demopositionH { get; set; }
    private Image demopositionV { get; set; }

    private bool inpass { get; set; }
    private bool isop { get; set; }

    private int distance = 60;
    private GameObject timeGo { get; set; }
    private clockSpine acnew { get; set; }
    private Text txt { get; set; }

    private Image lineh { get; set; }
    private Image linev { get; set; }

    private FindPositionSpine effectSpine { get; set; }

    public class Guanka
    {
        public int guanka { get; set; }
        public int guanka_last { get; set; }
        public List<Vector2> positions = new List<Vector2>();
        public int photoNum { get; set; }
        public List<PositionCtr> pcs = new List<PositionCtr>();

        public int currtimes { get; set; }
        public int alltimes { get; set; }

        private List<int> vs { get; set; }
        private List<int> hs { get; set; }

        public List<GameObject> demos = new List<GameObject>();
        public Vector3 demoStartPos { get; set; }

        public List<int> animalIndexs { get; set; }

        public int currH { get; set; }
        public int currV { get; set; }

        public List<float> angles = new List<float>() { 72, 77, 82, 87, 90, 94, 98, 104 };

        public List<string> demosSpineName = new List<string>();

        public FindPositionSpine guanka1Demo { get; set; }

        public Guanka()
        {
            guanka_last = 2;
        }
        public void setRandomData()
        {
            vs = Common.BreakRank(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 });
            hs = Common.BreakRank(new List<int>() { 1, 2, 3, 4, 5 });

            animalIndexs = Common.BreakRank(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 });
        }
        public void resetCurrTime()
        {
            currtimes = 0;
        }
        public void Set(int _guanka)
        {
            guanka = _guanka;
            positions.Clear();
            demosSpineName.Clear();
            switch (_guanka)
            {
                case 1:
                    alltimes = 3;
                    demoStartPos = new Vector3(0,0,0);
                    photoNum = 1;
                    positions.Add(new Vector2(hs[0], vs[0]));
                    positions.Add(new Vector2(hs[1], vs[1]));
                    positions.Add(new Vector2(hs[2], vs[2]));
                    break;
                case 2:
                    alltimes = 3;
                    setRandomData();
                    photoNum = 3;
                    demoStartPos = new Vector3(-314, 0, 0);
                    positions.Add(new Vector2(hs[0], vs[0]));
                    positions.Add(new Vector2(hs[1], vs[1]));
                    positions.Add(new Vector2(hs[2], vs[2]));
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }
    }
    void Awake()
    {
        mSound = gameObject.AddComponent<SoundManager>();
        mmPSCtrl = gameObject.AddComponent<PlaySoundController>();
    }
    // Use this for initialization
    bool isPlayedBg = false;
    void Start () {

        bg = UguiMaker.newRawImage("bg", transform, "findposition_texture", "bg", false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        //mSound.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.1f);
        StartCoroutine(TInit());
    }
    IEnumerator TInit()
    {
        yield return new WaitForSeconds(0.2f);
        setGameData(1, true);
    }
    // Update is called once per frame
    Vector3 temp_select_offset = Vector3.zero;
    bool temp_can_update = true;
    private float scaleTime = 0;
    void Update () {

        if (isop)
        {
            scaleTime += 0.1f;
            for (int i = 0; i < mGuanka.demos.Count; i++)
            {
                Image demobg = mGuanka.demos[i].transform.Find(mGuanka.demosSpineName[i] + "/rolebg").gameObject.GetComponent<Image>();
                demobg.rectTransform.localScale = Vector3.one * (1f + 0.1f * Mathf.Sin(scaleTime));
            }
        }

        if (inpass) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            if (null != hits)
            {
                FindPositionSpine.gSelect = null;
                PositionCtr.gSelect = null;
                foreach (RaycastHit hit in hits)
                {
                    FindPositionSpine com = hit.collider.gameObject.GetComponent<FindPositionSpine>();
                    if (null != com)
                    {
                        FindPositionSpine.gSelect = com;
                        Canvas canvas = FindPositionSpine.gSelect.transform.gameObject.GetComponent<Canvas>();
                        //FindPositionSpine.gSelect.transform.gameObject.layer = LayerMask.NameToLayer("UI");
                        //canvas.overrideSorting = true;
                        canvas.sortingOrder = 5;

                        Canvas canvasch = FindPositionSpine.gSelect.transform.Find("rolebg").transform.gameObject.GetComponent<Canvas>();
                        canvasch.sortingOrder = 4;

                        temp_select_offset = com.gameObject.GetComponent<RectTransform>().anchoredPosition3D - Common.getMouseLocalPos(transform);
                        mSound.StopTip();
                        mSound.PlayShort("aa_animal_effect_sound", MDefine.GetAnimalEffectSoundNameByID_CH(FindPositionSpine.gSelect.mid,"yes"));
                        break;
                    }
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            if(null != FindPositionSpine.gSelect)
            {
                FindPositionSpine.gSelect.gameObject.GetComponent<RectTransform>().anchoredPosition3D = Common.getMouseLocalPos(transform) + temp_select_offset;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (null != FindPositionSpine.gSelect)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray);
                if (null != hits)
                {
                    foreach (RaycastHit hit in hits)
                    {
                        PositionCtr com = hit.collider.gameObject.GetComponent<PositionCtr>();
                        if(null != com)
                        {
                            PositionCtr.gSelect = com;
                            break;
                        }
                    }   
                }

                //判断拖拽是否正确
                if (null != PositionCtr.gSelect && null != FindPositionSpine.gSelect)
                {
                    string[] arr = PositionCtr.gSelect.name.Split('_');
                    if(FindPositionSpine.gSelect.mh == int.Parse(arr[1]) && FindPositionSpine.gSelect.mv == int.Parse(arr[2]))
                    {
                        //TODO 处理正确的逻辑
                        mGuanka.currH = FindPositionSpine.gSelect.mh;
                        mGuanka.currV = FindPositionSpine.gSelect.mv;
                        StopCoroutine("TPlayOkEffect");
                        StopCoroutine("TCloseLine");
                        closeLine(true);
                        StartCoroutine("TPlayOkEffect");
                        
                    }
                    else
                    {
                        mSound.PlayShort("findposition_sound", "橡皮反弹的声音");
                        playErrSound();
                        FindPositionSpine.gSelect.setDefPos();
                    }
                }
                else
                {
                    if(null != FindPositionSpine.gSelect)
                    {
                        mSound.PlayShort("findposition_sound", "橡皮反弹的声音");

                        FindPositionSpine.gSelect.setDefPos();
                    }
                }
                    
            }
        }
    }
    private void playErrSound()
    {
        StartCoroutine(TplayErrSound());
    }
    private void checkFinish()
    {
        bool boo = true;
        for(int i = 0;i < mGuanka.photoNum; i++)
        {
            GameObject go = mGuanka.demos[i];
            int animalIndex = mGuanka.animalIndexs[i];
            string prefabName = MDefine.GetAnimalHeadResNameByID(animalIndex) + "_" + mGuanka.positions[i].x + "_" + mGuanka.positions[i].y;
            if (null != go.transform.Find(prefabName) && !go.transform.Find(prefabName).gameObject.GetComponent<FindPositionSpine>().isok)
            {
                boo = false;
                break;
            }
        }
        if (boo)
        {
            closeTimeGo();
            FindPositionSpine.gSelect = null;
            StartCoroutine(TCloceLianzi());
        }else
        {
            inpass = false;
        }
    }
    private void reGame()
    {
        mGuanka.guanka = 1;
        mGuanka.resetCurrTime();
        setGameData(1,true);
    }
    //设置游戏数据
    private void setGameData(int guanka,bool isNextguanka = false)
    {
        if (guanka == 1)
        {
            if (mGuanka.currtimes == 0)
            {
                mGuanka.setRandomData();
                TopTitleCtl.instance.Reset();
            }
        }
        else
        {
            if(isNextguanka)
            {
                TopTitleCtl.instance.AddStar();
            }
        }
        mGuanka.Set(guanka);
        inpass = false;
        isop = false;
        //Debug.Log("mGuanka.guanka : " + mGuanka.guanka + ";mGuanka.currtimes : " + mGuanka.currtimes + ";isNextguanka : " + isNextguanka);
        //创建元素
        if (null == positionGo)
        {
            positionGo = UguiMaker.newGameObject("positionGo", transform);
            //设置题目样本
            demoGo = UguiMaker.newGameObject("demoGo", positionGo.transform);
            demoGo.transform.localPosition = new Vector3(0, 262, 0);
            
            if (null == lianziGo)
            {
                lianziGo = UguiMaker.newGameObject("lianziGo", transform);
                Canvas canvasl = lianziGo.AddComponent<Canvas>();
                lianziGo.layer = LayerMask.NameToLayer("UI");
                canvasl.overrideSorting = true;
                canvasl.sortingOrder = 2;
                //TODO 动物头像和座位标题
                lianzi1 = UguiMaker.newImage("lianzi_1", lianziGo.transform, "findposition_sprite", "lianzi_bg");
                lianzi1.rectTransform.localPosition = new Vector3(-1015, 285, 0);
                lianzi2 = UguiMaker.newImage("lianzi_2", lianziGo.transform, "findposition_sprite", "lianzi_bg");
                lianzi2.rectTransform.localPosition = new Vector3(1015, 285, 0);

                Image lianzitop = UguiMaker.newImage("lianzi_top", lianziGo.transform, "findposition_sprite", "lianzi_top");
                lianzitop.rectTransform.localPosition = new Vector3(0, 373, 0);
            }

            timeGo = UguiMaker.newGameObject("timeGo", transform);
            Canvas canvas = timeGo.AddComponent<Canvas>();
            timeGo.layer = LayerMask.NameToLayer("UI");
            canvas.overrideSorting = true;
            canvas.sortingOrder = 3;

            acnew = ResManager.GetPrefab("findposition_prefab", "positionClok").GetComponent<clockSpine>();
            GameObject clok = UguiMaker.InitGameObj(acnew.gameObject, timeGo.transform, "timebg", new Vector3(5, -80, 0), Vector3.one);
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
            timeGo.transform.localPosition = new Vector3(539, 237, 0);

            StartCoroutine(setSeat());
            StartCoroutine(TInitCloseLianzi(isNextguanka));
        }//第二次进入
        else 
        {
            if (isNextguanka)
            {
                for(int i = 0;i < mGuanka.pcs.Count; i++)
                {
                    PositionCtr pc = mGuanka.pcs[i];
                    pc.cleanAnimal();
                }
            }else
            {
                if(mGuanka.guanka == 2)
                {
                    for (int i = 0; i < mGuanka.pcs.Count; i++)
                    {
                        PositionCtr pc = mGuanka.pcs[i];
                        pc.cleanAnimal();
                    }
                }
            }
            for (int i = 0; i < mGuanka.demos.Count; i++)
            {
                GameObject.Destroy(mGuanka.demos[i]);
            }
            mGuanka.demos.Clear();
            //yield return new WaitForSeconds(0.5f);
            StartCoroutine(TSetCommData(isNextguanka));
        }
        
    }
    //设置游戏数据元素
    IEnumerator TSetCommData(bool isNextguanka)
    {
        for (int i = 0; i < mGuanka.photoNum; i++)
        {
            int animalIndex = i;
            if (mGuanka.guanka == 1)
            {
                animalIndex = mGuanka.currtimes;
            }

            GameObject demo = getDemoGo(mGuanka.animalIndexs[animalIndex], (int)mGuanka.positions[animalIndex].x, (int)mGuanka.positions[animalIndex].y);
            float ofsetY = 0;
            if (i % 2 == 0 && mGuanka.photoNum > 1)
            {
                ofsetY = 13;
            }
            demo.transform.localPosition = mGuanka.demoStartPos + new Vector3(i * 300, ofsetY, 0);
            mGuanka.demos.Add(demo);
        }

        if (null == lineh)
        {
            lineh = UguiMaker.newImage("lineh", transform, "findposition_sprite", "rwaline");
            lineh.rectTransform.sizeDelta = new Vector2(28, 50);
            lineh.rectTransform.pivot = new Vector2(0, 0.5f);
            lineh.type = Image.Type.Tiled;
            Image headh = UguiMaker.newImage("head", lineh.transform, "findposition_sprite", "rwalinehead");

            linev = UguiMaker.newImage("linev", transform, "findposition_sprite", "rwaline");
            linev.rectTransform.sizeDelta = new Vector2(28, 50);
            linev.type = Image.Type.Tiled;
            linev.transform.localEulerAngles = new Vector3(0, 0, 90);
            linev.rectTransform.pivot = new Vector2(0, 0.5f);
            Image headv = UguiMaker.newImage("head", linev.transform, "findposition_sprite", "rwalinehead");
            headv.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        closeLine(true);
        StartCoroutine(TOpenLianzi(isNextguanka));
        yield return new WaitForSeconds(0.1f);
    }
    //设置座位
    IEnumerator setSeat()
    {
        Vector3 startPos = new Vector3(-314, 111, 0);
        for (int i = 0; i < 5; i++)
        {
            PlayComOutSound();
            for (int j = 0; j < 8; j++)
            {
                GameObject go = UguiMaker.newGameObject("position_" + (i + 1) + "_" + (j + 1), positionGo.transform);
                PositionCtr pc = go.AddComponent<PositionCtr>();
                go.transform.localPosition = startPos - new Vector3(i * 10 * 4, 0, 0) + new Vector3((int)j * (105 + i * 10), -(int)i * 100, 0);
                if (i == 4)
                {
                    pc.showVNum(j + 1);
                }
                if (j == 0)
                {
                    pc.showHNum(i + 1);
                }
                pc.setData(i, j);
                BoxCollider spineBox = pc.gameObject.AddComponent<BoxCollider>();
                spineBox.size = new Vector3(90, 90, 0);
                spineBox.center = new Vector3(0, 10, 0);
                mGuanka.pcs.Add(pc);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    public void PlayComOutSound()
    {
        mSound.PlayShort("素材出现通用音效");
    }
    //隐藏线
    private void closeLine(bool isinit = false)
    {
        if (isinit)
        {
            Image headh = lineh.transform.Find("head").gameObject.GetComponent<Image>();
            headh.enabled = false;
            Image headv = linev.transform.Find("head").gameObject.GetComponent<Image>();
            headv.enabled = false;
            lineh.enabled = false;
            linev.enabled = false;
        }
        else
        {
            StartCoroutine("TCloseLine");
        }
    }
    //隐藏钟表
    private void closeTimeGo()
    {
        if (timeGo.active)
        {
            StopCoroutine("TDelay");
            StartCoroutine(TMoveTimeGo(new Vector3(539, 237, 0), new Vector3(852, 237, 0), "out"));
        }
    }
    //出现倒计时
    private void showTimeGo()
    {
        if (null != timeGo)
        {
            timeGo.SetActive(true);
            StartCoroutine(TMoveTimeGo(new Vector3(852, 237, 0), new Vector3(552, 237, 0), "in"));
        }
        
    }
    private void timerun()
    {
        if (mGuanka.guanka == 2 && !timeIsRun)
        {
            timeGo.SetActive(true);
            StartCoroutine("TDelay");

        }
    }
    private bool timeIsRun = false;
    IEnumerator TCloseLine()
    {
        Color startColor = lineh.color;
        startColor.a = 1;
        Color endColor = lineh.color;
        endColor.a = 0;
        Image headh = lineh.transform.Find("head").gameObject.GetComponent<Image>();
        Image headv = linev.transform.Find("head").gameObject.GetComponent<Image>();
        for (float i = 0; i < 1f; i += 0.01f)
        {
            lineh.color = Color.Lerp(startColor, endColor, i);
            headh.color = Color.Lerp(startColor, endColor, i);
            linev.color = Color.Lerp(startColor, endColor, i);
            headv.color = Color.Lerp(startColor, endColor, i);
            yield return new WaitForSeconds(0.01f);
        }
        lineh.color = endColor;
        linev.color = endColor;
        headh.color = endColor;
        headv.color = endColor;

    }
    IEnumerator TplayErrSound()
    {
        yield return new WaitForSeconds(0.2f);
        if(Common.GetRandValue(0,10) % 2 == 0)
        {
            mSound.PlayShort("findposition_sound", "game-tips3-6-11-6");
        }else
        {
            mSound.PlayShort("findposition_sound", "game-tips3-6-11-7");
        }
    }
    //间隔时间
    IEnumerator TDelay()
    {
        timeIsRun = true;

        for (int i = distance; i >= 0; i--)
        {
            setTimeNum(i);
            if (i <= 5)
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
        closeTimeGo();
        //重置游戏数据
        StartCoroutine(TCloceLianzi(false));

    }
    //设置倒计时时间
    private void setTimeNum(int num)
    {
        if (!timeGo.active)
        {
            timeGo.SetActive(true);
        }
        txt.text = num.ToString();
        //timeImg.sprite = ResManager.GetSprite("findhome_sprite", "time_" + num);
    }
    //设置窗帘背后的题目元素
    private GameObject getDemoGo(int index,int h,int v)
    {
        GameObject demo = UguiMaker.newGameObject("demo_" + index, demoGo.transform);
        
        demopositionbg = UguiMaker.newImage("demopositionbg", demo.transform, "findposition_sprite", "photopositionbg");
        demopositionbg.transform.localPosition = new Vector3(60, 0, 0);
        Image danwei = UguiMaker.newImage("danwei", demo.transform, "findposition_sprite", "photoposition");
        danwei.transform.localPosition = new Vector3(71, 0, 0);
        demopositionH = UguiMaker.newImage("demopositionH", demo.transform, "findposition_sprite", "num_" + h);
        demopositionH.transform.localPosition = new Vector3(20, 0, 0);
        demopositionH.color = Color.black;
        demopositionV = UguiMaker.newImage("demopositionV", demo.transform, "findposition_sprite", "num_" + v);
        demopositionV.transform.localPosition = new Vector3(71, 0, 0);
        demopositionV.color = Color.black;

        FindPositionSpine spin = ResManager.GetPrefab("animalhead_prefab", MDefine.GetAnimalHeadResNameByID(index)).AddComponent<FindPositionSpine>();
        string namestr = MDefine.GetAnimalHeadResNameByID(index) + "_" + h + "_" + v;
        demoPhoto = UguiMaker.InitGameObj(spin.gameObject, demo.transform, namestr, Vector3.zero, Vector3.one * 0.3f);
        spin.transform.localPosition = new Vector3(-60, -45, 0);
        spin.PlaySpine("Idle", true);
        BoxCollider spineBox = spin.gameObject.AddComponent<BoxCollider>();
        spineBox.size = new Vector3(200, 250, 0);
        spineBox.center = new Vector3(0, 150, 0);
        spin.setData(h, v, MDefine.GetAnimalHeadResNameByID(index), index);
        mGuanka.demosSpineName.Add(namestr);

        mGuanka.guanka1Demo = spin;
        return demo;
    }
    //设置下一个出题
    private void nextTimes()
    {
        mGuanka.currtimes++;
        if(mGuanka.currtimes >= mGuanka.alltimes)
        {
            nextGame();
        }else
        {
            setGameData(mGuanka.guanka);
        }
    }
    //设置下一关
    private void nextGame()
    {
        mGuanka.guanka++;
        if (mGuanka.guanka > mGuanka.guanka_last)
        {
            TopTitleCtl.instance.AddStar();
            StopCoroutine("TDelay");
            closeTimeGo();
            
            StartCoroutine(TNextGame());
        }
        else
        {
            setGameData(mGuanka.guanka,true);
        }
    }
    IEnumerator TNextGame()
    {
        inpass = true;
        yield return new WaitForSeconds(2f);
        GameOverCtl.GetInstance().Show(mGuanka.guanka_last, reGame);
    }
    IEnumerator TMoveTimeGo(Vector3 startPos, Vector3 endPos, string type)
    {
        float speed = 0.1f;
        if (type == "out")
        {
            speed = 0.2f;
        }
        else
        {
            txt.text = distance.ToString();
        }
        for (float i = 0; i < 1f; i += speed)
        {
            timeGo.transform.localPosition = Vector3.Lerp(startPos, endPos, i);
            yield return new WaitForSeconds(0.01f);
        }
        if (type == "out")
        {
            inpass = true;
            timeGo.transform.localPosition = endPos - new Vector3(10, 0, 0);
            txt.text = distance.ToString();
            timeGo.active = false;
        }
        else
        {
            timeGo.transform.localPosition = endPos + new Vector3(10, 0, 0);

            timerun();
        }
    }

    IEnumerator TPlayOkEffect()
    {
        FindPositionSpine.gSelect.isok = true;
        FindPositionSpine.gSelect.gameObject.active = false;
        if(mGuanka.guanka == 1)
        {
            inpass = true;
        }
        effectSpine = FindPositionSpine.gSelect;
        Vector3 posh = positionGo.transform.Find("position_" + mGuanka.currH + "_1").localPosition + new Vector3(-83, 10, 0);

        Vector3 posv = positionGo.transform.Find("position_5_" + mGuanka.currV).localPosition + new Vector3(0, -70, 0);

        Vector3 endPos = positionGo.transform.Find("position_" + mGuanka.currH + "_" + mGuanka.currV).localPosition;
        //设置座位的动物
        PositionCtr.gSelect.setAnimal(effectSpine.mAnimalName);//TODO 播放座位号
        mSound.PlayTipList(new List<string>() { "findposition_sound","aa_animal_name" , "findposition_sound"},new List<string>() { "game-tips3-6-11-8",MDefine.GetAnimalNameByID_CH(effectSpine.mid), "game-tips3-6-11-9" });
        lineh.transform.localPosition = posh;
        float widthH = endPos.x - posh.x;
        
        linev.transform.localPosition = posv;
        linev.transform.localEulerAngles = new Vector3(0, 0, mGuanka.angles[mGuanka.currV - 1]);
        float widthV = 28 + endPos.y - posv.y;
        float stamp_height = 50;
        Vector2 starthsize = new Vector2(0, stamp_height);
        Vector2 endhsize = new Vector2(widthH, stamp_height);

        Vector2 startvsize = new Vector2(0, stamp_height);
        Vector2 endvsize = new Vector2(widthV, stamp_height);
        Color color = lineh.color;
        color.a = 1;
        lineh.color = color;
        linev.color = color;
        lineh.enabled = true;
        linev.enabled = true;
        mSound.PlayShort("findposition_sound", "闪光连线");
        Image headh = lineh.transform.Find("head").gameObject.GetComponent<Image>();
        headh.enabled = true;
        Image headv = linev.transform.Find("head").gameObject.GetComponent<Image>();
        headv.enabled = true;
        headh.color = color;
        headv.color = color;
        for (float i = 0; i < 1f; i += 0.02f)
        {
            lineh.rectTransform.sizeDelta = Vector2.Lerp(starthsize, endhsize, i);
            headh.transform.localPosition = new Vector3(lineh.rectTransform.sizeDelta.x +3, 0, 0);
            linev.rectTransform.sizeDelta = Vector2.Lerp(startvsize, endvsize, i);
            headv.transform.localPosition = new Vector3(linev.rectTransform.sizeDelta.x, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        lineh.rectTransform.sizeDelta = new Vector2(widthH, stamp_height);
        linev.rectTransform.sizeDelta = new Vector2(widthV - 20, stamp_height);
        if(null != PositionCtr.gSelect)
        {
            PositionCtr.gSelect.playStar();
        }
        
        closeLine();
        mSound.PlayShort("findposition_sound", "胜利冒星星");
        closeDemoNum(effectSpine);
        inpass = false;
        //FindPositionSpine.gSelect = null;
        //yield return new WaitForSeconds(1f);
        checkFinish();
    }
    private void closeDemoNum(FindPositionSpine spine)
    {
        if (null == spine) return;
        spine.transform.parent.gameObject.active = false;
    }
    //开窗帘
    IEnumerator TOpenLianzi(bool isNext = false)
    {
        yield return new WaitForSeconds(1f);
        mSound.PlayShort("findposition_sound", "拉幕布",0.6f);
        Vector3 startPos1 = new Vector3(-372, 285, 0);
        Vector3 startPos2 = new Vector3(372, 285, 0);
        Vector3 endPos1 = new Vector3(-850, 285, 0);
        Vector3 endPos2 = new Vector3(850, 285, 0);
        for (float i = 0; i < 1f; i += 0.02f)
        {
            lianzi1.transform.localPosition = Vector3.Lerp(startPos1, endPos1, i);
            lianzi2.transform.localPosition = Vector3.Lerp(startPos2, endPos2, i);
            yield return new WaitForSeconds(0.01f);
        }
        lianzi1.transform.localPosition = endPos1;
        lianzi2.transform.localPosition = endPos2;

        if (mGuanka.guanka == 2)
        {
            timeIsRun = false;
            showTimeGo();
        }
        else
        {
            closeTimeGo();
        }
        isop = true;
        if (mGuanka.guanka == 2)
        {
            if (isNext)
            {
                mSound.PlayTip("findposition_sound", "game-tips3-6-11-10", 1, true);
                yield return new WaitForSeconds(4f);
                isop = true;
            }else
            {
                isop = true;
            }
        }
        else
        {
            mSound.PlayTipList(new List<string>() { "aa_animal_name", "findposition_sound", "findposition_sound", "findposition_sound", "findposition_sound", "findposition_sound" },
                   new List<string>() { MDefine.GetAnimalNameByID_CH(mGuanka.guanka1Demo.mid), "game-tips3-6-11-1", mGuanka.guanka1Demo.mh.ToString(), "game-tips3-6-11-2", mGuanka.guanka1Demo.mv.ToString(), "game-tips3-6-11-3" },true);
            yield return new WaitForSeconds(6f);
            isop = true;
        }
       
    }
    IEnumerator TInitCloseLianzi(bool isNextguanka)
    {
        Vector3 startPos1 = new Vector3(-850, 285, 0);
        Vector3 startPos2 = new Vector3(850, 285, 0);
        Vector3 endPos1 = new Vector3(-372, 285, 0);
        Vector3 endPos2 = new Vector3(372, 285, 0);
        mSound.PlayShort("findposition_sound", "关幕布", 0.6f);
        for (float i = 0; i < 1f; i += 0.02f)
        {
            lianzi1.transform.localPosition = Vector3.Lerp(startPos1, endPos1, i);
            lianzi2.transform.localPosition = Vector3.Lerp(startPos2, endPos2, i);
            yield return new WaitForSeconds(0.01f);
        }
        lianzi1.transform.localPosition = endPos1;
        lianzi2.transform.localPosition = endPos2;
        yield return new WaitForSeconds(1f);
        ///*
        if (!isPlayedBg)
        {
            mSound.PlayBgAsync("bgmusic_loop4", "bgmusic_loop4", 0.1f);
            isPlayedBg = true;
        }
        //*/
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(TSetCommData(isNextguanka));
    }
    //关窗帘
    IEnumerator TCloceLianzi(bool isPass = true)
    {
        yield return new WaitForSeconds(1f);
        Vector3 startPos1 = new Vector3(-850, 285, 0);
        Vector3 startPos2 = new Vector3(850, 285, 0);
        Vector3 endPos1 = new Vector3(-372, 285, 0);
        Vector3 endPos2 = new Vector3(372, 285, 0);
        mSound.PlayShort("findposition_sound", "关幕布",0.6f);
        for (float i = 0; i < 1f; i += 0.02f)
        {
            lianzi1.transform.localPosition = Vector3.Lerp(startPos1, endPos1, i);
            lianzi2.transform.localPosition = Vector3.Lerp(startPos2, endPos2, i);
            yield return new WaitForSeconds(0.01f);
        }
        lianzi1.transform.localPosition = endPos1;
        lianzi2.transform.localPosition = endPos2;
        if (isPass)
        {
            mSound.PlayShort("findposition_sound", "愉悦的弹跳");
            nextTimes();
        }else
        {
            setGameData(mGuanka.guanka, false);
        }
        
    }
}
