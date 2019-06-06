using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AnimalSortCtr : MonoBehaviour {
    public static Transform mTransform { get; set; }
    public static Vector3 spine_scale = new Vector3(0.16f, 0.16f, 1);//0.17f
    public static bool opState { get; set; }
    private List<SpineCtr> answerList = new List<SpineCtr>();
    private Dictionary<SpineCtr, bool> answerDic = new Dictionary<SpineCtr, bool>();
    private GameObject gridsGo { get; set; }
    private GameObject kuangGo { get; set; }
    private GameObject answersanimalGo { get; set; }
    private GameObject starGo { get; set; }
    public Guanka mGuanka = new Guanka();
    private Image mroom { get; set; }
    private Image mraod { get; set; }
    private Image mbutton { get; set; }
    private ParticleSystem mtopStartEffect { get; set; }
    private AudioClip clip = null;
    private AudioSource audio = null;
    ParticleSystem mOKBtn_Effect;
    //第二关
    private Dictionary<int,GameObject> chooseList = new Dictionary<int, GameObject>();
    //第三关
    private List<SpineCtr> sChooseList = new List<SpineCtr>();

    private GameObject componentContain { get; set; }

    PlaySoundController mmPSCtrl;

    public class Guanka
    {
        public int guanka { get; set; }
        public int index { get; set; }
        public int map { get; set; }
        public int guanka_last { get; set; }
        public List<string> animals { get; set; }
        public List<string> animalgrids { get; set; }
        public string indexstr { get; set; }
        public string type { get; set; }
        public int startf { get; set; }
        public int starts { get; set; }
        public bool result { get; set; }
        public int questionTimes { get; set; }
        public int currQue = 1;
        public int dis { get; set; }
        public Guanka()
        {
            guanka_last = 3;
        }

        public void Set(int _guanka)
        {
            index++;
            questionTimes = 1;
            dis = 1;
            guanka = _guanka;
            animals = Common.BreakRank(new List<string>() { "circle", "square", "triangle" });

            switch (_guanka)
            {
                case 1:
                    indexstr = "second";
                    type = "first_same_second_up";
                    startf = 1;
                    starts = 0;
                    if (map == 3)
                    {
                        if (Common.GetRandValue(0, 10) % 2 == 0)
                        {
                            dis = 2;
                            starts = -1;
                        }
                    }
                    break;
                case 2:
                    indexstr = "second";
                    type = "first_same_second_down";
                    startf = 1;
                    starts = 5;
                    if (map == 3)
                    {
                        if (Common.GetRandValue(0, 10) % 2 == 0)
                        {
                            dis = 2;
                            starts = 9;
                        }
                    }
                    break;
                case 3:
                    indexstr = "first";
                    type = "first_down_second_same";
                    startf = 5;
                    starts = 1;
                    if (map == 3)
                    {
                        if (Common.GetRandValue(0, 10) % 2 == 0)
                        {
                            dis = 2;
                            startf = 9;
                        }
                    }
                    break;
                case 4:
                    indexstr = "first";
                    type = "first_up_second_same";
                    startf = 0;
                    starts = 1;
                    if (map == 3)
                    {
                        if (Common.GetRandValue(0, 10) % 2 == 0)
                        {
                            dis = 2;
                            startf = -1;
                        }
                    }
                    break;
                case 5:
                    indexstr = "both";
                    type = "first_up_second_down";
                    startf = 0;
                    starts = 5;
                    break;
                case 6:
                    indexstr = "both";
                    type = "first_down_second_down";
                    startf = 5;
                    starts = 5;
                    break;
                case 7:
                    indexstr = "both";
                    type = "first_down_second_up";
                    startf = 5;
                    starts = 0;
                    break;
                case 8:
                    indexstr = "both";
                    type = "first_up_second_up";
                    startf = 0;
                    starts = 0;
                    break;

            }
        }

        public bool isMax(List<int> lis, int _num)
        {
            bool boo = true;
            for (int i = 0; i < lis.Count; i++)
            {
                int num = lis[i];
                if (num > _num)
                {
                    boo = false;
                    break;
                }
            }
            return boo;
        }

        public bool isMin(List<int> lis, int _num)
        {
            bool boo = true;
            for (int i = 0; i < lis.Count; i++)
            {
                int num = lis[i];
                if (num < _num)
                {
                    boo = false;
                    break;
                }
            }
            return boo;
        }

        public bool isSame(List<int> lis, int _num)
        {
            bool boo = false;
            int sameNum = 0;
            for (int i = 0; i < lis.Count; i++)
            {
                int num = lis[i];
                if (num == _num)
                {
                    sameNum++;
                }
            }
            if (sameNum > 1)
            {
                boo = true;
            }
            return boo;
        }
    }
    public SoundManager mSound { get; set; }

    class GridData
    {
        public int mNumf { get; set; }
        public int mNums { get; set; }
        public string mAnimalNamef { get; set; }
        public string mAnimalNames { get; set; }

        public GridData(AnimalSortCtr.Guanka guanka, int _numf, int _nums)
        {
            mNumf = _numf;
            mNums = _nums;
            mAnimalNamef = guanka.animals[1];
            mAnimalNames = guanka.animals[2];

        }
    }
    void Awake()
    {
        mSound = gameObject.AddComponent<SoundManager>();

        mmPSCtrl = gameObject.AddComponent<PlaySoundController>();
    }
    private bool isPlayBgSoud = false;
    void Start () {
        RawImage bg = UguiMaker.newRawImage("bg", transform, "animalsort_texture", "bg", false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        componentContain = UguiMaker.newGameObject("componentContain", transform);
        starGo = UguiMaker.newGameObject("starGo", componentContain.transform);
        //mmPSCtrl.PlayBGSound1("bgmusic_loop0", "bgmusic_loop0", 0.1f);
        //StartCoroutine(TplayInit());
        StartCoroutine(TdelayInit());
    }
    IEnumerator TdelayInit()
    {
        yield return new WaitForSeconds(0.5f);
        Image boat = UguiMaker.newImage("boat", starGo.GetComponent<RectTransform>(), "animalsort_sprite", "boat");
        boat.transform.localScale = Vector3.one * 3f;
        Vector3 startPos = new Vector3(-GlobalParam.screen_width * 0.5f, 0, 0);
        boat.transform.localPosition = startPos;
        Vector3 endPos = new Vector3(GlobalParam.screen_width * 0.5f + 200, 0, 0);
        for (float j = 0; j < 1f; j += 0.01f)
        {
            boat.rectTransform.localPosition = Vector3.Lerp(startPos, endPos, j) + new Vector3(0, 50 * Mathf.Sin(j * 8), 0);
            yield return new WaitForSeconds(0.01f);
        }
        boat.rectTransform.localPosition = endPos;
        GameObject.Destroy(boat.gameObject);
        //
        //yield return new WaitForSeconds(1f);
        mTransform = transform;
        Image outRoom = UguiMaker.newImage("outRoom", componentContain.transform, "animalsort_sprite", "room");
        outRoom.rectTransform.localPosition = new Vector3(-390, 246, 0);
        outRoom.enabled = false;

        mroom = UguiMaker.newImage("room", componentContain.transform, "animalsort_sprite", "room");
        mroom.rectTransform.localPosition = new Vector3(77, -110, 0);
        mroom.enabled = false;
        BoxCollider roomBox = mroom.gameObject.AddComponent<BoxCollider>();
        roomBox.size = new Vector3(550, 138);
        audio = gameObject.AddComponent<AudioSource>();
        //yield return new WaitForSeconds(2.5f);
        //KbadyCtl.Init();
        setGemaData(1);
        //初始化数据InitData()
        //创建管道
        //管道进来

        //创建进来小人
        //小人进来

        //进来第1个答案

        //进来第2个答案

        //进来第3个答案

        StartCoroutine("TplayStar");

        yield return new WaitForSeconds(1);
        TopTitleCtl.instance.SetBgColor(new Color(86 / 256f, 170 / 256f, 188 / 256f, 0.5f));
        yield return new WaitForSeconds(1);
        //mSound.PlayBgAsync("animalsort_sound", "bgmusic_loop0", 0.1f);
    }
    IEnumerator TplayInit()
    {
        yield return new WaitForSeconds(0.01f);
        Image boat = UguiMaker.newImage("boat", starGo.GetComponent<RectTransform>(), "animalsort_sprite", "boat");
        boat.transform.localScale = Vector3.one * 3f;
        Vector3 startPos = new Vector3(-GlobalParam.screen_width * 0.5f, 0, 0);
        boat.transform.localPosition = startPos;
        Vector3 endPos = new Vector3(GlobalParam.screen_width * 0.5f + 200, 0, 0);
        for (float j = 0; j < 1f; j += 0.01f)
        {
            boat.rectTransform.localPosition = Vector3.Lerp(startPos, endPos, j) + new Vector3(0, 50 * Mathf.Sin(j * 8), 0);
            yield return new WaitForSeconds(0.01f);
        }
        boat.rectTransform.localPosition = endPos;
        GameObject.Destroy(boat.gameObject);
    }
    private SpineCtr getCurrentAnswer()
    {
        SpineCtr sctr = null;
        bool boo = false;
        for(int i = 0;i < answerList.Count; i++)
        {
            sctr = answerList[i];
            if (!answerDic[sctr])
            {
                boo = true;
                break;
            }
        }
        if (!boo)
        {
            sctr = null;
        }
        return sctr;
    }
    //刷新画面 UPDATA
    Vector3 temp_select_offset = Vector3.zero;
    bool temp_can_update = true;
    bool isDownSumibt = false;
    // Update is called once per frame
    void Update()
    {
        if (!opState) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            if (null != hits)
            {
                SpineCtr.gSelect = null;
                AnimalGrids.gSelect = null;
                SpineCtr.gCSelect = null;
                isDownSumibt = false;
                foreach (RaycastHit hit in hits)
                {
                    AnimalGrids com = hit.collider.gameObject.GetComponent<AnimalGrids>();
                    int randsound = Common.GetRandValue(1, 3);
                    if (null != com)//&& com.isAnswer
                    {
                        AnimalGrids.gSelect = com;
                        AnimalGrids.gSelect.gameObject.transform.SetSiblingIndex(5);

                        AnimalGrids.gSelect.playSpine("Click", true);
                        audio.Stop();
                        mSound.PlayTip("animalsort_sound", "Click_" + randsound, 1);
                        AnimalGrids.gSelect.toBiger();
                       temp_select_offset = com.gameObject.GetComponent<RectTransform>().anchoredPosition3D - Common.getMouseLocalPos(transform);
                    }
                    SpineCtr ac = hit.collider.gameObject.GetComponent<SpineCtr>();
                    if (null != ac && ac.isEvent)
                    {
                        SpineCtr acnew = ResManager.GetPrefab("animalsort_prefab", ac.mAnimalName).GetComponent<SpineCtr>();
                        acnew.setData(ac.mAnimalName,true,"answer");
                        GameObject go = UguiMaker.InitGameObj(acnew.gameObject, transform, "moveAnimal", ac.transform.localPosition, spine_scale + new Vector3(0.2f, 0.2f, 0));// + new Vector3(0.1f, 0.1f, 0)
                        acnew.setDefaultPosData(ac.transform.localPosition);
                        acnew.PlaySpine("Click", true);
                        audio.Stop();
                        mSound.PlayTip("animalsort_sound", "Click_" + randsound, 1);
                        SpineCtr.gSelect = acnew;
                        SpineCtr.gCSelect = ac;
                        SpineCtr.gCSelect.gameObject.SetActive(false);
                        temp_select_offset = ac.gameObject.GetComponent<RectTransform>().anchoredPosition3D - Common.getMouseLocalPos(transform);
                       
                    }
                    
                    if(mGuanka.map == 3)
                    {
                        GameObject button = hit.collider.gameObject;
                        if(button.name == "submitbutton")
                        {
                            audio.Stop();
                            mSound.PlayTip("animalsort_sound", "submit_up", 1);
                            mbutton.sprite = ResManager.GetSprite("animalsort_sprite", "submit_down");
                            isDownSumibt = true;
                            break;
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (null != SpineCtr.gSelect)
            {
                SpineCtr.gSelect.gameObject.GetComponent<RectTransform>().anchoredPosition3D = Common.getMouseLocalPos(transform) + temp_select_offset;
                if(null != SpineCtr.gCSelect && SpineCtr.gCSelect.type == "answer"){
                    //Debug.Log("remove id : " + SpineCtr.gCSelect.id);
                    chooseList.Remove(SpineCtr.gCSelect.id);
                    SpineCtr.gCSelect.Select();
                    SpineCtr.gCSelect = null;
                }
            }
            if (null != AnimalGrids.gSelect)
            {
                AnimalGrids.gSelect.gameObject.GetComponent<RectTransform>().anchoredPosition3D = Common.getMouseLocalPos(transform) + temp_select_offset;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);

            if (null != SpineCtr.gSelect)
            {
                //SpineCtr.gSelect.gameObject.transform.localScale = spine_scale;
                SpineCtr.gSelect.PlaySpine("Click", false);
                int randsound = Common.GetRandValue(1, 2);
                //mSound.PlayShort("animalsort_sound", "Defeated_" + randsound, 1);
                if (null != hits)
                {
                    bool boo = false;
                    foreach (RaycastHit hit in hits)
                    {
                        GameObject hitGo = hit.collider.gameObject;
                        SpineCtr partowSpine = hit.collider.gameObject.GetComponent<SpineCtr>();
                        if (hitGo.name == "room")
                        {
                            if(mGuanka.map == 3)
                            {
                                //chooseList.Add(partowSpine.id, SpineCtr.gSelect.gameObject);
                                if (!mbutton.gameObject.active)
                                {
                                    mbutton.sprite = ResManager.GetSprite("animalsort_sprite", "submit_up");
                                    mbutton.gameObject.SetActive(true);
                                }
                                if(gridsGo.transform.childCount < 23)
                                {
                                    boo = true;
                                    SpineCtr.gSelect.gameObject.transform.parent = gridsGo.transform;
                                    pushToRaod(SpineCtr.gSelect.gameObject);
                                    sChooseList.Add(SpineCtr.gSelect);
                                    SpineCtr.gCSelect.gameObject.SetActive(true);//选择按钮
                                }
                            }

                        }
                        if (mGuanka.map == 2)
                        {
                            /*
                            SpineCtr curr = getCurrentAnswer();
                            if (null != curr && curr.mAnimalName == SpineCtr.gSelect.mAnimalName)
                            {
                                answerDic[curr] = true;
                                SpineCtr.gSelect.transform.parent = gridsGo.GetComponent<RectTransform>();
                                pushToRaod(SpineCtr.gSelect.gameObject);
                                SpineCtr.gCSelect.gameObject.SetActive(true);
                                boo = true;
                                checkFinish();
                                break;
                            }
                            */
                            if (null != partowSpine && partowSpine.type == "line")
                            {

                                if (chooseList.ContainsKey(partowSpine.id))
                                {
                                    GameObject.Destroy(chooseList[partowSpine.id]);
                                    chooseList.Remove(partowSpine.id);
                                }
                                SpineCtr.gSelect.PlaySpine("Succeed", false);
                                chooseList.Add(partowSpine.id, SpineCtr.gSelect.gameObject);
                                if(null != SpineCtr.gCSelect && SpineCtr.gCSelect.type != "answer")
                                {
                                    SpineCtr.gCSelect.gameObject.SetActive(true);//选择按钮
                                }
                                
                                boo = true;
                                checkScondPartFinish();
                                SpineCtr.gSelect.transform.localPosition = partowSpine.transform.localPosition + new Vector3(0, 18, 0);
                                SpineCtr.gSelect.gameObject.transform.localScale = new Vector3(0.17f, 0.17f, 1);
                                SpineCtr.gSelect.id = partowSpine.id;
                                mSound.PlayTip("animalsort_sound", "shoot", 1);
                                break;
                            }
                        }

                    }
                    if (!boo)
                    {
                        opState = false;
                        SpineCtr.gSelect.PlaySpine("Idle_" + randsound, false);
                        mSound.PlayTip("animalsort_sound", "Idle_3_" + randsound, 1);
                        SpineCtr.gSelect.setDefaultPos();
                    }
                }else
                {
                    opState = false;
                    SpineCtr.gSelect.PlaySpine("Idle_" + randsound, false);
                    SpineCtr.gSelect.setDefaultPos();
                }
            }
            if(null != AnimalGrids.gSelect)
            {
                bool boo = false;
                if (null != hits)
                {
                    bool ishit = false;
                    foreach (RaycastHit hit in hits)
                    {
                        GameObject hitGo = hit.collider.gameObject;
                        if (hitGo.name == "room")// && 
                        {
                            ishit = true;
                            if (AnimalGrids.gSelect.isAnswer)
                            {
                                boo = true;
                               
                            }
                            break;
                        }
                    }
                    opState = false;
                    if (ishit)
                    {
                        StartCoroutine("TtPush", AnimalGrids.gSelect);
                        AnimalGrids.gSelect.Close();
                        if (boo)
                        {
                            mGuanka.result = true;
                        }else
                        {
                            mGuanka.result = false;
                        }
                    }else
                    {
                        int randsound = Common.GetRandValue(1, 2);
                        mSound.PlayTip("animalsort_sound", "Idle_3_" + randsound, 1);//Defeated_
                        AnimalGrids.gSelect.playSpine("Idle_" + randsound, false);
                        AnimalGrids.gSelect.backToDefaultPos();
                    }
                    AnimalGrids.gSelect = null;
                }
                
            }

            if (isDownSumibt)
            {
                bool boo = checkThirdFinish();
                StartCoroutine("TdelayUp", boo);
            }

        }
    }
    IEnumerator TdelayUp(bool boo)
    {
        yield return new WaitForSeconds(1f);
        if (!boo)
        {
            mbutton.sprite = ResManager.GetSprite("animalsort_sprite", "submit_up");
            mSound.PlayTip("animalsort_sound", "submit_up", 1);
        }
        isDownSumibt = false;
    }
    //检测第三关是否成功
    private bool checkThirdFinish()
    {
        bool boo = true;
        if (answerList.Count != sChooseList.Count){
            boo = false;
        }else
        {
            for (int i = 0; i < answerList.Count; i++)
            {
                SpineCtr spineLine = answerList[i];
                SpineCtr chooseSpine = sChooseList[i];
                if (spineLine.mAnimalName != chooseSpine.mAnimalName)
                {
                    boo = false;
                    break;
                }
            }
        }
        
        if (boo)
        {
            opState = false;
            mSound.PlayShort("animalsort_sound", "04-星星-02");
            mOKBtn_Effect.Play();
            Invoke("popOUt", 2);
            return boo;
        }else
        {
            //mSound.PlayShort("animalsort_sound", "shoot", 1);

            if (sChooseList.Count <= 0) return boo;

            StartCoroutine(TerrorRoad(sChooseList.Count));
        }
        return boo;
    }
    //检测第二关是否结束
    private void checkScondPartFinish()
    {
        bool boo = true;
        for(int i = 0; i < answerList.Count; i++)
        {
            SpineCtr spineLine = answerList[i];
            if (!chooseList.ContainsKey(i))
            {
                boo = false;
                break;
            }

        }
        if (boo)
        {
            bool isOk = true;

            for (int i = 0; i < answerList.Count; i++)
            {
                SpineCtr spineLine = answerList[i];
                SpineCtr chooseSpine = chooseList[i].GetComponent<SpineCtr>();
                if (spineLine.mAnimalName != chooseSpine.mAnimalName)
                {
                    isOk = false;
                    break;
                }
            }
            if (isOk)
            {
                mGuanka.result = true;
            }else
            {
                mGuanka.result = false;;
            }
            opState = false;
           StartCoroutine(TpushScondChoose());
        }
    }
    //检测第一关是否拖拽结束
    private void checkFinish()
    {
        bool boo = true;
        for (int i = 0; i < answerList.Count; i++)
        {
            if (!answerDic[answerList[i]])
            {
                boo = false;
                break;
            }
        }

        if (boo)
        {
            StartCoroutine("TfinishMapTow");
        }
    }
    //重玩
    private void reGame()
    {
        setGemaData(1);
    }
    //设置拖拽画面第二关
    private void setDragView(GameObject gridsGo, List<GridData> griddatas, float dis, Vector3 startPos)
    {
        List<GameObject> items = new List<GameObject>();
        for (int i = 0; i < griddatas.Count; i++)
        {
            GridData griddatac = griddatas[i];
            for (int j = 0; j < griddatac.mNumf; j++)
            {
                SpineCtr animalCtrs = ResManager.GetPrefab("animalsort_prefab", griddatac.mAnimalNamef).GetComponent<SpineCtr>();
                GameObject go = UguiMaker.InitGameObj(animalCtrs.gameObject, gridsGo.GetComponent<RectTransform>(), "animal" + j, Vector3.zero, spine_scale);
                animalCtrs.PlaySpine("Idle_2", false);
                animalCtrs.setData(griddatac.mAnimalNamef);
                go.SetActive(false);
                items.Add(go);
            }
            for (int n = 0; n < griddatac.mNums; n++)
            {
                SpineCtr animalCtrs = ResManager.GetPrefab("animalsort_prefab", griddatac.mAnimalNames).GetComponent<SpineCtr>();
                GameObject go = UguiMaker.InitGameObj(animalCtrs.gameObject, gridsGo.GetComponent<RectTransform>(), "animal" + n, Vector3.zero, spine_scale);
                animalCtrs.PlaySpine("Idle_2", false);
                animalCtrs.setData(griddatac.mAnimalNames);
                go.SetActive(false);
                items.Add(go);
            }
        }
        answerList.Clear();
        if (null == kuangGo)
        {
            kuangGo = UguiMaker.newGameObject("xuxiankuang", transform);
        }
        else
        {
            kuangGo.transform.DetachChildren();
        }
        ///* 设置虚线
        GridData griddata = null;
        if (mGuanka.type == "first_same_second_up")
        {
            //mGuanka.startf += mGuanka.dis;
            mGuanka.starts += mGuanka.dis;
            griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
        }
        if (mGuanka.type == "first_same_second_down")
        {
            //mGuanka.startf += mGuanka.dis;
            mGuanka.starts -= mGuanka.dis;
            griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
        }

        if (mGuanka.type == "first_down_second_same")
        {
            mGuanka.startf -= mGuanka.dis;
            //mGuanka.starts -= mGuanka.dis;
            griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
        }
        if (mGuanka.type == "first_up_second_same")
        {
            mGuanka.startf += mGuanka.dis;
            //mGuanka.starts -= mGuanka.dis;
            griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
        }

        if (mGuanka.type == "first_up_second_down")
        {
            mGuanka.startf += mGuanka.dis;
            mGuanka.starts -= mGuanka.dis;
            griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
        }
        if (mGuanka.type == "first_up_second_up")
        {
            mGuanka.startf += mGuanka.dis;
            mGuanka.starts += mGuanka.dis;
            griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
        }
        if (mGuanka.type == "first_down_second_down")
        {
            mGuanka.startf -= mGuanka.dis;
            mGuanka.starts -= mGuanka.dis;
            griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
        }
        if (mGuanka.type == "first_down_second_up")
        {
            mGuanka.startf -= mGuanka.dis;
            mGuanka.starts += mGuanka.dis;
            griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
        }

        int index = 0;
        for (int j = 0; j < griddata.mNumf; j++)
        {
            SpineCtr animalCtrs = ResManager.GetPrefab("animalsort_prefab", griddata.mAnimalNamef).GetComponent<SpineCtr>();
            animalCtrs.setData(griddata.mAnimalNamef, false, "line", index);
            GameObject go = UguiMaker.InitGameObj(animalCtrs.gameObject, kuangGo.transform, "animal" + j, Vector3.zero, spine_scale + new Vector3(0.1f, 0.1f, 0));
            go.SetActive(false);
            animalCtrs.PlaySpine("Idle_2", false);
            animalCtrs.closeAnimal();
            answerDic.Add(animalCtrs, false);
            answerList.Add(animalCtrs);
            index++;

        }

        for (int n = 0; n < griddata.mNums; n++)
        {
            SpineCtr animalCtrs = ResManager.GetPrefab("animalsort_prefab", griddata.mAnimalNames).GetComponent<SpineCtr>();
            animalCtrs.setData(griddata.mAnimalNames, false, "line", index);
            GameObject go = UguiMaker.InitGameObj(animalCtrs.gameObject, kuangGo.transform, "animal" + n, Vector3.zero, spine_scale + new Vector3(0.1f, 0.1f, 0));
            go.SetActive(false);
            animalCtrs.PlaySpine("Idle_2", false);
            animalCtrs.closeAnimal();
            answerDic.Add(animalCtrs, false);
            answerList.Add(animalCtrs);
            index++;
        }
        if (mGuanka.map == 2)
        {
            Vector3 startPoskuang = new Vector3(109, -175, 0);
            for (int i = 0; i < answerList.Count; i++)
            {
                SpineCtr animalCtrs = answerList[i];
                animalCtrs.transform.localPosition = startPoskuang + new Vector3(i * 100, 0, 0);
                //animalCtrs.gameObject.SetActive(true);
            }
            BoxCollider roomBox = mroom.gameObject.GetComponent<BoxCollider>();
            roomBox.enabled = false;
        }
        else
        {
            BoxCollider roomBox = mroom.gameObject.GetComponent<BoxCollider>();
            roomBox.enabled = true;

            if(null == mbutton)
            {
                mbutton = UguiMaker.newImage("submitbutton", transform, "animalsort_sprite", "submit_up");
                mbutton.transform.localPosition = new Vector3(438, -111, 0);
                mOKBtn_Effect = ResManager.GetPrefab("animalsort_prefab", "fxbb").GetComponent<ParticleSystem>();
                //GameObject effect = ResManager.GetPrefab("animalsort_prefab", "effect");
                UguiMaker.InitGameObj(mOKBtn_Effect.gameObject, mbutton.transform, "effect", Vector3.zero, Vector3.one);
                //effect.GetComponent<ParticleSystem>().startColor = new Color32(255, 0, 216, 255);
                //effect.gameObject.SetActive(false);
                BoxCollider buttonBox = mbutton.gameObject.AddComponent<BoxCollider>();
                buttonBox.size = new Vector3(162, 81, 0);
            }
            sChooseList.Clear();
            mbutton.gameObject.SetActive(false);
        }

        ///* 设置虚线
        ///
        StartCoroutine(TstartPush(items));

        for (int i = 0; i < 3; i++)
        {
            SpineCtr animalCtrs = ResManager.GetPrefab("animalsort_prefab", mGuanka.animals[i]).GetComponent<SpineCtr>();
            animalCtrs.setData(mGuanka.animals[i], true);
            GameObject go = UguiMaker.InitGameObj(animalCtrs.gameObject, answersanimalGo.transform, "animal" + i, Vector3.zero, spine_scale + new Vector3(0.1f,0.1f,0));
            animalCtrs.PlaySpine("Idle_2", false);
            go.transform.localPosition = startPos + new Vector3(i * dis, 0, 0);
        }

        //answersanimalGo.SetActive(true);
    }
    private void closeXuXian()
    {
        for (int i = 0; i < answerList.Count; i++)
        {
            SpineCtr animalCtrs = answerList[i];
            animalCtrs.gameObject.SetActive(false);
        }
    }
    private void showXuXian()
    {
        for (int i = 0; i < answerList.Count; i++)
        {
            SpineCtr animalCtrs = answerList[i];
            animalCtrs.gameObject.SetActive(true);
        }
    }
    //设置选择题画面第一关
    private void setChooseView(GameObject gridsGo, List<GridData> griddatas, float dis, Vector3 startPos)
    {
        
        List<GameObject> items = new List<GameObject>();
        for (int i = 0; i < griddatas.Count; i++)
        {
            GridData griddata = griddatas[i];
            for (int j = 0; j < griddata.mNumf; j++)
            {
                SpineCtr animalCtrs = ResManager.GetPrefab("animalsort_prefab", griddata.mAnimalNamef).GetComponent<SpineCtr>();
                GameObject go = UguiMaker.InitGameObj(animalCtrs.gameObject, gridsGo.GetComponent<RectTransform>(), "animal" + j, Vector3.zero, spine_scale);
                animalCtrs.PlaySpine("Idle_2", false);
                animalCtrs.setData(griddata.mAnimalNamef);
                go.SetActive(false);
                items.Add(go);
            }
            for (int n = 0; n < griddata.mNums; n++)
            {
                SpineCtr animalCtrs = ResManager.GetPrefab("animalsort_prefab", griddata.mAnimalNames).GetComponent<SpineCtr>();
                GameObject go = UguiMaker.InitGameObj(animalCtrs.gameObject, gridsGo.GetComponent<RectTransform>(), "animal" + n, Vector3.zero, spine_scale);
                animalCtrs.PlaySpine("Idle_2", false);
                animalCtrs.setData(griddata.mAnimalNames);
                go.SetActive(false);
                items.Add(go);
            }
        }
        BoxCollider roomBox = mroom.gameObject.GetComponent<BoxCollider>();
        roomBox.enabled = true;
        createButtons(dis, startPos);
        StartCoroutine(TstartPush(items));
    }

    private void createButtons(float dis,Vector3 startPos)
    {
        List<GameObject> arr = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            GameObject answer = UguiMaker.newGameObject("answerButton" + i, answersanimalGo.transform);
            AnimalGrids ag = answer.AddComponent<AnimalGrids>();
            bool isAnswer = false;
            if (i == 0)
            {
                isAnswer = true;
            }
            if (mGuanka.type == "first_same_second_up")
            {
                //mGuanka.startf += mGuanka.dis;

                if (mGuanka.starts >= 4)
                {
                    mGuanka.starts -= 3;
                }
                mGuanka.starts += mGuanka.dis;
                ag.setData(mGuanka, mGuanka.startf, mGuanka.starts, isAnswer, i);
            }
            if (mGuanka.type == "first_same_second_down")
            {
                //mGuanka.startf += mGuanka.dis;
                if (mGuanka.starts < 1)
                {
                    mGuanka.startf += 3;
                }
                mGuanka.starts -= mGuanka.dis;
                ag.setData(mGuanka, mGuanka.startf, mGuanka.starts, isAnswer, i);
            }

            if (mGuanka.type == "first_down_second_same")
            {

                if (mGuanka.startf < 1)
                {
                    mGuanka.startf += 3;
                }
                mGuanka.startf -= mGuanka.dis;
                //mGuanka.starts += mGuanka.dis;
                ag.setData(mGuanka, mGuanka.startf, mGuanka.starts, isAnswer, i);
            }
            if (mGuanka.type == "first_up_second_same")
            {
                if (mGuanka.startf > 4)
                {
                    mGuanka.startf -= 3;
                }
                mGuanka.startf += mGuanka.dis;
                //mGuanka.starts += mGuanka.dis;
                ag.setData(mGuanka, mGuanka.startf, mGuanka.starts, isAnswer, i);
            }

            if (mGuanka.type == "first_up_second_down")
            {
                if (mGuanka.startf > 4)
                {
                    mGuanka.startf -= 3;
                }
                if (mGuanka.starts < 1)
                {
                    mGuanka.starts += 3;
                }
                mGuanka.startf += mGuanka.dis;
                mGuanka.starts -= mGuanka.dis;
                ag.setData(mGuanka, mGuanka.startf, mGuanka.starts, isAnswer, i);
            }
            if (mGuanka.type == "first_up_second_up")
            {
                if (mGuanka.starts > 4)
                {
                    mGuanka.starts -= 3;
                }
                if (mGuanka.startf < 1)
                {
                    mGuanka.startf += 3;
                }
                mGuanka.startf -= mGuanka.dis;
                mGuanka.starts += mGuanka.dis;
                //griddata = new GridData(mGuanka, mGuanka.startf++, mGuanka.starts++);
                ag.setData(mGuanka, mGuanka.startf, mGuanka.starts, isAnswer, i);
            }
            if (mGuanka.type == "first_down_second_down")
            {
                if (mGuanka.starts <= 1)
                {
                    mGuanka.starts += 2;
                }
                if (mGuanka.startf <= 1)
                {
                    mGuanka.startf += 3;
                }
                mGuanka.startf -= mGuanka.dis;
                mGuanka.starts -= mGuanka.dis;

                //griddata = new GridData(mGuanka, mGuanka.startf--, mGuanka.starts--);
                ag.setData(mGuanka, mGuanka.startf, mGuanka.starts, isAnswer, i);
            }
            if (mGuanka.type == "first_down_second_up")
            {
                if (mGuanka.startf <= 1)
                {
                    mGuanka.startf += 2;
                }
                if (mGuanka.starts >= 4)
                {
                    mGuanka.starts -= 4;
                }
                mGuanka.startf -= mGuanka.dis;
                mGuanka.starts += mGuanka.dis;
                ag.setData(mGuanka, mGuanka.startf, mGuanka.starts, isAnswer, i);
                //griddata = new GridData(mGuanka, mGuanka.startf--, mGuanka.starts++);
            }
            arr.Add(answer);
        }
        arr = Common.BreakRank(arr);
        for (int i = 0; i < arr.Count; i++)
        {
            GameObject answer = arr[i];
            AnimalGrids ag = answer.GetComponent<AnimalGrids>();
            Vector3 pos = startPos + new Vector3(i * dis, 0, 0);
            ag.setDefaultPos(pos);
        }
    }

    private void resetButton()
    {
        for (int i = 0; i < answersanimalGo.transform.childCount; i++)
        {
            GameObject go = answersanimalGo.transform.GetChild(i).gameObject;
            go.transform.localScale = Vector3.one;
            go.SetActive(true);
        }
    }

    public void showAnswerButton()
    {
        StartCoroutine(TshowAnswerButton());
    }

    private void closeAnswerButton()
    {
        answersanimalGo.SetActive(false);
    }

    private List<GameObject> raodGos = new List<GameObject>();
    //把单个动画放入管道
    private int inSoundIndex { get; set; }
    private int okSoundIndex { get; set; }
    private int enterIndex { get; set; }
    private void pushToRaod(GameObject go)
    {
        Vector3 startPos = new Vector3(219, -133, 0);
        go.transform.localPosition = startPos;
        SpineCtr animalCtrs = go.GetComponent<SpineCtr>();
        go.transform.localScale = spine_scale;
        animalCtrs.isEvent = false;
        animalCtrs.PlaySpine("Idle_2", false);
        raodGos.Add(go);
        
        //mSound.PlayShort("animalsort_sound", "shoot", 1);

        enterIndex += 1;
        string entersname = "enter_1";
        if(enterIndex % 2 == 0)
        {
            entersname = "enter_2";
        }
        mSound.PlayShort("animalsort_sound", entersname, 1);//"animal_in_to_road"

        animalCtrs.pushToRaod();
    }

    public void popOUt()
    {
        //TODO 规律演示
        showRole();
    }
    //TODO 规律演示
    private void showRole()
    {
        Dictionary<string, GameObject> dic = new Dictionary<string, GameObject>();
        Dictionary<int, List<GameObject>> arr = new Dictionary<int, List<GameObject>>();
        int index = -1;
        for (int i = 0; i < raodGos.Count; i++)
        {
            GameObject go = raodGos[i];
            if (go)
            {
                SpineCtr animalCtrs = go.GetComponent<SpineCtr>();
                if (!dic.ContainsKey(animalCtrs.mAnimalName + "_" + index))
                {
                    index++;
                    arr[index] = new List<GameObject>();
                    dic[animalCtrs.mAnimalName + "_" + index] = go;
                    arr[index].Add(go);
                }
                else
                {
                    arr[index].Add(go);
                }
            }
            
        }
        StartCoroutine(TshowRole(arr));

    }

    //设置游戏数据
    private void setGemaData(int map)
    {
        int gk = 1;
        mGuanka.map = map;
        if (map < 3)
        {
            gk = Common.GetMutexValue(1, 7);
        }
        else
        {
            mGuanka.index = 0;
            gk = Common.GetMutexValue(1, 8);
        }
        mGuanka.Set(gk);
        if (map == 1)
        {
            TopTitleCtl.instance.Reset();
            TopTitleCtl.instance.mSoundTipData.SetData(replayHowToDo);
        }
        else
        {
            TopTitleCtl.instance.AddStar();
        }
        enterIndex = 0;
        inSoundIndex = Common.GetRandValue(1, 4);
        okSoundIndex = Common.GetRandValue(1, 4);
        while(inSoundIndex == okSoundIndex) okSoundIndex = Common.GetRandValue(1, 4);
        opState = true;
        List<GridData> griddatas = new List<GridData>();
        for (int i = 0; i < 3; i++)
        {
            GridData griddata = null;
            if (mGuanka.type == "first_same_second_up")
            {
                //mGuanka.startf += mGuanka.dis;
               
                mGuanka.starts += mGuanka.dis;
                griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
            }
            if (mGuanka.type == "first_same_second_down")
            {
                //mGuanka.startf += mGuanka.dis;
                mGuanka.starts -= mGuanka.dis;
                griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
            }

            if (mGuanka.type == "first_down_second_same")
            {
                mGuanka.startf -= mGuanka.dis;
                //mGuanka.starts -= mGuanka.dis;
                griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
            }
            if (mGuanka.type == "first_up_second_same")
            {
                mGuanka.startf += mGuanka.dis;
                //mGuanka.starts -= mGuanka.dis;
                griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
            }

            if (mGuanka.type == "first_up_second_down")
            {
                mGuanka.startf += mGuanka.dis;
                mGuanka.starts -= mGuanka.dis;
                griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
            }
            if (mGuanka.type == "first_up_second_up")
            {
                mGuanka.startf += mGuanka.dis;
                mGuanka.starts += mGuanka.dis;
                griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
            }
            if (mGuanka.type == "first_down_second_down")
            {
                mGuanka.startf -= mGuanka.dis;
                mGuanka.starts -= mGuanka.dis;
                griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
            }
            if (mGuanka.type == "first_down_second_up")
            {
                mGuanka.startf -= mGuanka.dis;
                mGuanka.starts += mGuanka.dis;
                griddata = new GridData(mGuanka, mGuanka.startf, mGuanka.starts);
            }
            griddatas.Add(griddata);
        }

        float dis = GlobalParam.screen_width / 4;
        Vector3 startPos = new Vector3(-GlobalParam.screen_width * 0.5f + dis, -GlobalParam.screen_height * 0.5f + 50, 0);
        if (null == gridsGo)
        {
            gridsGo = UguiMaker.newGameObject("gridGo", componentContain.transform);
        }
        else
        {
            for (int i = 0; i < gridsGo.transform.childCount; i++)
            {
                GameObject go = gridsGo.transform.GetChild(i).gameObject;
                GameObject.Destroy(go);
            }
        }
        
        RectTransform rect = gridsGo.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(1000, 100);

        if (null == mraod)
        {
            GameObject obj = ResManager.GetPrefab("animalsort_prefab", "AnimalSortParticleSystem");
            obj.transform.parent = componentContain.transform;
            ParticleSystem mStartEffect = obj.GetComponent<ParticleSystem>();
            mStartEffect.transform.localPosition = new Vector3(0, -108, 0);
            mStartEffect.transform.localScale = Vector3.one;
            mStartEffect.Play();

            GameObject obj1 = ResManager.GetPrefab("animalsort_prefab", "AnimalSortParticleSystem");
            obj1.name = "topParticel";
            obj1.transform.parent = componentContain.transform;
            mtopStartEffect = obj1.GetComponent<ParticleSystem>();
            mtopStartEffect.transform.localPosition = new Vector3(-350, 239, 0);
            mtopStartEffect.transform.localScale = Vector3.one;
            mtopStartEffect.Play();

            mraod = UguiMaker.newImage("road", componentContain.transform, "animalsort_sprite", "road");
            mraod.rectTransform.localPosition = new Vector3(0, 70, 0);
        }
        closeTopParticle();

        if(null != answersanimalGo)
        {
            Destroy(answersanimalGo.gameObject);
        }
        answersanimalGo = UguiMaker.newGameObject("answersAnimalGo", transform);
       

        mGuanka.map = map;

        if (map == 1)
        {
            if (kuangGo)
            {
                kuangGo.SetActive(false);
                mbutton.gameObject.SetActive(false);
            }
            setChooseView(gridsGo, griddatas, dis, startPos);
        }
        else
        {
            if (kuangGo)
            {
                kuangGo.SetActive(true);
            }
            setDragView(gridsGo, griddatas, dis, startPos);
        }
        closeAnswerButton();

        audio.clip = ResManager.GetClip("animalsort_sound", "game_how_play_" + mGuanka.index);

        showAnswerButton();


    }
    private void replayHowToDo()
    {
        if (!audio.isPlaying) return;
        audio.Play();
    }
    public void closeTopParticle()
    {
        mtopStartEffect.Stop();
    }
    public void playTopParticle()
    {
        mtopStartEffect.Play();
    }
    //在管道中报废
    IEnumerator TerrorRoad(int leng)
    {
        opState = false;
        int ran = Common.GetRandValue(1, 2);
        yield return new WaitForSeconds(2.5f);
        mSound.PlayShort("animalsort_sound", "game_error_" + ran, 1.2f);
        int startindex = raodGos.Count - leng;// resultList.Count;
        int len = raodGos.Count;
        for (int i = startindex; i < len; i++)
        {
            GameObject go = raodGos[i];
            SpineCtr animalCtrs = go.GetComponent<SpineCtr>();
            animalCtrs.PlaySpine("Defeated",false);
            mSound.PlayShort("animalsort_sound", "Defeated_" + ((i % 3) + 1), 0.6f);
            for (float j = 0; j < 1f; j += 0.1f)
            {
                mraod.color = Color.Lerp(new Color(1, 1, 1), new Color(0.5f, 0.5f, 1), j);
                //go.transform.localScale = Vector3.Lerp(AnimalSortCtr.spine_scale, Vector3.zero, j);
                yield return new WaitForSeconds(0.001f);
            }
            animalCtrs.RemovePosState();
            GameObject.Destroy(go);
            yield return new WaitForSeconds(0.05f);
        }
        mraod.color = new Color(1, 1, 1);
        opState = true;
        if (null != sChooseList)
        {
            sChooseList.Clear();
        }
        //resetButton();
    }

    //第二关答案输入管道
    IEnumerator TpushScondChoose()
    {
        List<GameObject> resultList = new List<GameObject>();
        for (int i = 0; i < answerList.Count; i++)
        {
            chooseList[i].transform.parent = gridsGo.transform;
            //yield return new WaitForSeconds(0.01f);
            answerList[i].gameObject.SetActive(false);
            for (float j = 0; j < 1f; j += 0.2f)
            {
                mSound.PlayShort("animalsort_sound", "shoot", 1);
                chooseList[i].transform.localPosition = Vector3.Lerp(chooseList[i].transform.localPosition, new Vector3(77, -150, 0), j);
                yield return new WaitForSeconds(0.0001f);
            }
            resultList.Add(chooseList[i]);
            pushToRaod(chooseList[i]);
        }
        if (mGuanka.result)//正确答案
        {
            yield return new WaitForSeconds(2f);
            chooseList.Clear();
            sChooseList.Clear();
            popOUt();
        }
        else
        {
            int ran = Common.GetRandValue(1, 2);
            yield return new WaitForSeconds(2.5f);
            mSound.PlayShort("animalsort_sound", "game_error_" + ran, 1);
            int startindex = raodGos.Count - resultList.Count;
            int len = raodGos.Count;
            
            for (int i = startindex; i < len; i++)
            {
                GameObject go = raodGos[i];
                SpineCtr animalCtrs = go.GetComponent<SpineCtr>();
                animalCtrs.RemovePosState();
                GameObject.Destroy(go);
                for (float j = 0; j < 1f; j += 0.1f)
                {
                    mraod.color = Color.Lerp(new Color(1, 1, 1), new Color(0.5f, 0.5f, 1), j);
                    yield return new WaitForSeconds(0.001f);
                }
                yield return new WaitForSeconds(0.05f);
            }
            mraod.color = new Color(1, 1, 1);
            chooseList.Clear();
            for (int i = 0; i < answerList.Count; i++)
            {
                answerList[i].gameObject.SetActive(true);
            }
            opState = true;
        }
    }

    //演示规律动画
    IEnumerator TshowRole(Dictionary<int,List<GameObject>> dic)
    {
        float dis = 0.3f;
        string[] strs =  mGuanka.type.Split('_');
        string fType = "";
        string sType = "";
        if (strs[1] == "same")
        {
            fType = "shoot";
        }else
        {
            fType = strs[1];
        }

        if (strs[3] == "same")
        {
            sType = "shoot";
        }
        else
        {
            sType = strs[3];
        }
        int index = 0;
        for (int j = 0; j < 8; j++)
        {
            if(j % 2 == 0)
            {
                List<GameObject> arr = dic[j];

                for (int i = 0; i < arr.Count; i++)
                {
                    GameObject go = arr[i];
                    SpineCtr animalCtrs = go.GetComponent<SpineCtr>();
                    animalCtrs.PlaySpine("Succeed", true);
                    animalCtrs.Scale();
                }
                if(fType == "shoot")
                {
                    mSound.PlayShort("animalsort_sound", "up_1", 0.6f);
                }
                else
                {
                    mSound.PlayShort("animalsort_sound", fType + "_" + (index + 1), 0.6f);
                }
                
                index++;
            }
            yield return new WaitForSeconds(dis);
        }

        index = 0;
        for (int j = 0; j < 8; j++)
        {
            if (j % 2 == 1)
            {
                List<GameObject> arr = dic[j];
                for (int i = 0; i < arr.Count; i++)
                {
                    GameObject go = arr[i];
                    SpineCtr animalCtrs = go.GetComponent<SpineCtr>();
                    animalCtrs.PlaySpine("Succeed", true);
                    animalCtrs.Scale();
                }
                if (sType == "shoot")
                {
                    mSound.PlayShort("animalsort_sound", "down_1", 0.6f);
                }
                else
                {
                    mSound.PlayShort("animalsort_sound", sType + "_" + (index + 1), 0.6f);
                }
                index++;
            }
            yield return new WaitForSeconds(dis);
        }
        int soundindex = Common.GetRandValue(1, 3);
        mSound.PlayTip("animalsort_sound", "game_succe_" + soundindex, 1);
        yield return new WaitForSeconds(1f);
        StartCoroutine(TpopOut());
    }

    //动画出管道
    IEnumerator TpopOut()
    {
        playTopParticle();
        yield return new WaitForSeconds(1f);
        int ran = Common.GetRandValue(1, 3);
        mSound.PlayShort("animalsort_sound", "Succeed_" + ran, 1);
        for (int i = 0;i < raodGos.Count; i++)
        {
            GameObject go = raodGos[i];
            if (go)
            {
                SpineCtr animalCtrs = go.GetComponent<SpineCtr>();
                animalCtrs.PlaySpine("Idle_3", true);
                animalCtrs.PopOut();
            }
           
        }
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(TnextGuanKa());
    }

    //把答案推进管道
    IEnumerator TtPush(AnimalGrids grid)
    {
        List<GameObject> resultList = new List<GameObject>();
        for (int i = 0;i < grid.animals.Count; i++)
        {
            SpineCtr animalCtrs = grid.animals[i];
            SpineCtr acnew = ResManager.GetPrefab("animalsort_prefab", animalCtrs.mAnimalName).GetComponent<SpineCtr>();
            acnew.setData(animalCtrs.mAnimalName);
            GameObject go = UguiMaker.InitGameObj(acnew.gameObject, gridsGo.GetComponent<RectTransform>(), "moveAnimal", animalCtrs.transform.localPosition, spine_scale);
            acnew.PlaySpine("Click", true);
            go.SetActive(false);
            yield return new WaitForSeconds(0.15f);
            go.SetActive(true);
            pushToRaod(go);
            resultList.Add(go);
           
        }
        
        if (mGuanka.result)//正确答案
        {
            yield return new WaitForSeconds(2f);
            popOUt();
        }
        else
        {
            StartCoroutine(TerrorRoad(resultList.Count));
        }
    }
    
    //结束第二关
    IEnumerator TfinishMapTow()
    {
        yield return new WaitForSeconds(1.5f);
        popOUt();
    }

    //结束游戏
    IEnumerator TcompleteGame()
    {
        TopTitleCtl.instance.AddStar();
        yield return new WaitForSeconds(1.5f);
        GameOverCtl.GetInstance().Show(mGuanka.guanka_last, reGame);
    } 
    //下一关
   IEnumerator TnextGuanKa()
    {
        yield return new WaitForSeconds(1);
        if(mGuanka.currQue >= mGuanka.questionTimes)
        {
            if (mGuanka.map >= mGuanka.guanka_last)
            {
                StartCoroutine("TcompleteGame");
            }else
            {
                mGuanka.map++;
                mGuanka.currQue = 1;
                setGemaData(mGuanka.map);

            }
        }else
        {
            mGuanka.currQue++;
            setGemaData(mGuanka.map);
        }
        /*
        if(mGuanka.map >= mGuanka.guanka_last)
        {
            mGuanka.currQue = 0;
            StartCoroutine("TcompleteGame");
        }
        else
        {
            mGuanka.currQue++;
            Debug.Log("mGuanka.currQue : " + mGuanka.currQue);
            if (mGuanka.currQue < mGuanka.questionTimes)
            {
                setGemaData(mGuanka.map);
            }else
            {
                mGuanka.map++;
                setGemaData(mGuanka.map);
            }
        }
        */
        
    }
    //开始推进管道
    IEnumerator TstartPush(List<GameObject> items)
    {
        raodGos.Clear();
        SpineCtr.cleanRoadData();
        for (int i = 0; i < items.Count; i++)//items.Count
        {
            GameObject go = items[i];
            yield return new WaitForSeconds(0.5f);
            go.SetActive(true);
            pushToRaod(go);
        }
    }

    //播放背景后面的飞船、星星灯物件
    IEnumerator TplayStar()
    {
        yield return new WaitForSeconds(0.5f);
        List<string> stempList = Common.BreakRank(new List<string>() { "boat", "star" });//"soket",
        int dis = (int)GlobalParam.screen_height / 130;
        List<int> numYs = Common.GetMutexValue(1, dis, 3);
        for (int i = 0; i < stempList.Count; i++)
        {
            Image starItem = UguiMaker.newImage(stempList[i], starGo.GetComponent<RectTransform>(), "animalsort_sprite", stempList[i]);
            StarCtr star = starItem.gameObject.AddComponent<StarCtr>();
            starItem.rectTransform.localPosition = new Vector3(-GlobalParam.screen_width * 0.5f, GlobalParam.screen_height * 0.5f - 130 * numYs[i], 0);
            star.begian();
        }
        yield return new WaitForSeconds(30f);
        StartCoroutine("TplayStar");
    }
    //显示选择按钮
    IEnumerator TshowAnswerButton()
    {
        float dt = 10;
        if(mGuanka.map == 1)
        {
            dt = 6;
        }

        yield return new WaitForSeconds(dt);
       
        audio.Play();
        ///*
        answersanimalGo.SetActive(true);
        for (float j = 0; j < 1f; j += 0.1f)
        {
            answersanimalGo.transform.localPosition = Vector3.Lerp(new Vector3(0,-300,0), new Vector3(0f, 0,0), j);
            yield return new WaitForSeconds(0.01f);
        }
        answersanimalGo.transform.localPosition = new Vector3(0f, 0, 0);
        //*/
        
        if (mGuanka.map == 2)
        {
            showXuXian();
        }
        yield return new WaitForSeconds(3f);
        if (!isPlayBgSoud)
        {
            mmPSCtrl.PlayBGSound1("bgmusic_loop0", "bgmusic_loop0", 0.1f);
            isPlayBgSoud = true;
        }
    }

}
