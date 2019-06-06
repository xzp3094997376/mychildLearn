using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 动物分类
/// </summary>
public class AnimalClassificationCtrl : BaseScene
{
    public int nLevel = 1;
    public const int nLevels = 3;

    public int nCounts = 0;
    public bool bCanDrop = false;

    /// <summary>
    /// 动物scale
    /// </summary>
    public float fScale = 0.9f;

    private bool bLevelPass = false;

    private AnimalClass_LvStation station_lv1;
    private AnimalClass_LvStation station_lv2;
    private AnimalClass_LvStation station_lv3;

    private RawImage mBG;
    private Image tree1;
    private Image tree2;

    //默认位置
    private List<Vector3> vLocalPos = new List<Vector3>();
    //动物列表
    private List<AnimalClass_Animal> mAnimalList = new List<AnimalClass_Animal>();
    //station
    public List<AnimalClass_Station> mStationList = new List<AnimalClass_Station>();


    private AnimalClassDefine mDefine = new AnimalClassDefine();
    //属性数量信息列表
    public Dictionary<int, List<int>> dicCheckList = new Dictionary<int, List<int>>();

    //分类完成返回结果列表
    private List<AnimalStationValue> theResultList = new List<AnimalStationValue>();

    private Transform mCenter;

    public TujianCtrl mTujian;

    void Awake()
    {
        fScale = 0.9f;
        //Debug.logger.logEnabled = false;
        mDefine.InitDicDataInfos();
        dicCheckList = mDefine.mDicDataInfos;

        float scale = 1f;//GlobalParam.screen_width / 1423f;
        mCenter = transform.Find("mCenter");
        mCenter.transform.localScale = Vector3.one * scale;

        mSceneType = SceneEnum.AnimalClassification;
        CallLoadFinishEvent();

        nLevel = 1;

        mBG = transform.Find("bg").GetComponent<RawImage>();
        mBG.texture = ResManager.GetTexture("animalclass_texture", "animalclass_bg");
        tree1 = transform.Find("tree1").GetComponent<Image>();
        tree2 = transform.Find("tree2").GetComponent<Image>();
        tree1.sprite = ResManager.GetSprite("animalclass_sprite", "tree1");
        tree2.sprite = ResManager.GetSprite("animalclass_sprite", "tree2");

        station_lv1 = CreateLvStation("station_lv1", 2);
        station_lv2 = CreateLvStation("station_lv2", 3);
        station_lv3 = CreateLvStation("station_lv3", 4);
    }
    AnimalClass_LvStation CreateLvStation(string _name, int _stations)
    {
        GameObject mgo = ResManager.GetPrefab("animalclass_prefab", _name);
        mgo.transform.SetParent(mCenter.transform);
        mgo.transform.localPosition = Vector3.zero;
        mgo.transform.localScale = Vector3.one;
        AnimalClass_LvStation lvstation = mgo.AddComponent<AnimalClass_LvStation>();
        lvstation.InitAwake(_stations);
        lvstation.gameObject.SetActive(false);
        return lvstation;
    }

    void Start ()
    {
        mTujian = TujianCtrl.Create(transform.parent);
        List<int> dataids = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        mTujian.SetData(dataids);

        PlayBGSound();

        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(ReplayTipSound);

        mASSort = gameObject.AddComponent<AudioSource>();
        mASSort.playOnAwake = false;
        mAudioSourceKimi = gameObject.AddComponent<AudioSource>();
        mAudioSourceKimi.playOnAwake = false;

        KbadyCtl.Init();
        //StartCoroutine(IEStart());
        //动物资源
        for (int i = 1; i <= 14; i++)
        {
            AnimalClass_Animal animalObj = CreateAnimal(i);
            mAnimalList.Add(animalObj);
            animalObj.gameObject.SetActive(false);
        }
        
        InitLevelData();
        PlayTitleSound();
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitLevelData();
            //mTujian.ShowTujian();
        }
        if (bCanDrop && !mTujian.bTujianState)
        {
            MUpdate();
        }
	}

    /// <summary>
    /// 初始化信息
    /// </summary>
    public void InitLevelData()
    {
        ResetInfos();
        bLevelPass = false;

        List<Transform> mposList = null;

        #region//station创建
        if (nLevel == 1)
        {
            station_lv1.gameObject.SetActive(true);
            mStationList = station_lv1.mStationList;
            mposList = station_lv1.animalposList;
        }
        else if (nLevel == 2)
        {
            station_lv2.gameObject.SetActive(true);
            mStationList = station_lv2.mStationList;
            mposList = station_lv2.animalposList;
        }
        else
        {
            station_lv3.gameObject.SetActive(true);
            mStationList = station_lv3.mStationList;
            mposList = station_lv3.animalposList;
            station_lv3.SetLv3TipObj();
        }
        #endregion

        #region//动物显示
        mAnimalList = Common.BreakRank(mAnimalList);
        for (int i = 0; i < mAnimalList.Count; i++)
        {
            mAnimalList[i].gameObject.SetActive(true);
            mAnimalList[i].transform.position = mposList[i].position;
            mAnimalList[i].SetRemaidPos(mAnimalList[i].transform.localPosition);
            mAnimalList[i].transform.localScale = Vector3.one * 0.001f;
        }
        #endregion

        for (int i = 0; i < mStationList.Count; i++)
        {
            mStationList[i].SetLan();
        }

        MSceneMove(true);
        OrderSetting();
        StartCoroutine(IESetHaha());

        if (nLevel != 1)
        {
            ReplayTipSound();
        }
    }
    IEnumerator IESetHaha()
    {
        yield return new WaitForSeconds(1.1f);   
        //for (int i = 0; i < mStationList.Count; i++)
        //{
        //    mStationList[i].SetHouse();
        //}
        //PlaySortSound("sound_houseout",0.8f);
        //yield return new WaitForSeconds(1.5f);      
        for (int i = 0; i < mAnimalList.Count; i++)
        {
            mAnimalList[i].SceneMove(true,0.25f);
            PlaySortSound("sound_animalout",0.3f);
            yield return new WaitForSeconds(0.2f);
        }
        bCanDrop = true;
    }

    /// <summary>
    /// 重置信息
    /// </summary>
    public void ResetInfos()
    {
        nCounts = 0;
        theResultList.Clear();
        bCanDrop = false;

        station_lv1.ResetInfos();
        station_lv2.ResetInfos();
        station_lv3.ResetInfos();
        station_lv1.gameObject.SetActive(false);
        station_lv2.gameObject.SetActive(false);
        station_lv3.gameObject.SetActive(false);

        for (int i = 0; i < mAnimalList.Count; i++)
        {
            mAnimalList[i].transform.SetParent(transform);
            mAnimalList[i].transform.localScale = Vector3.one * fScale;
            mAnimalList[i].gameObject.SetActive(false);
        }     
    }


    List<string> finishClipList = new List<string>();
    private void AddFinishClip(string _clipName)
    {
        if (_clipName.CompareTo("") == 0)
            return;
        if (!finishClipList.Contains(_clipName))
        {
            finishClipList.Add(_clipName);
        }
    }
    private void mOrderStationValue()
    {
        for (int i = 0; i < theResultList.Count - 1; i++)
        {
            for (int j = 1; j < theResultList.Count; j++)
            {
                int nA = (int)theResultList[i].mValueType;
                int nB = (int)theResultList[j].mValueType;
                if (nB < nA)
                {
                    AnimalStationValue thirdObj = theResultList[i];
                    theResultList[i] = theResultList[j];
                    theResultList[j] = thirdObj;
                }
            }
        }
    }

    private AnimalStationValue GetAnimalValueTypeByType(AnimalValueType _type)
    {
        AnimalStationValue getvalue = null;
        getvalue = theResultList.Find((x) => { return x.mValueType == _type; });
        return getvalue;
    }

    /// <summary>
    /// 关卡完成
    /// </summary>
    public void MLevelPass()
    {
        bCanDrop = false;

        finishClipList.Clear();
        mOrderStationValue();
        //鼓励声音
        AddFinishClip("sound_guli" + Random.Range(1, 4));

        //第一关5个结果中随机一个作显示
        int ramdomIndex = Random.Range(1, 6);     
        //Debug.LogError("-------分类完成-------");
        for (int i = 0; i < theResultList.Count; i++)
        {
            //string strDebug = theResultList[i].mStation.name + "有动物->" + theResultList[i].mValueType.ToString() + ":";
            //for (int j = 0; j < theResultList[i].mAnimalList.Count; j++)
            //{
            //    strDebug += theResultList[i].mAnimalList[j].gameObject.name + ", ";
            //}
            //Debug.Log(strDebug);  

            string soundname = theResultList[i].GetSoundName();           
            if (nLevel == 1)
            {
                // 第一关由于有多个相同结果,所以随机一个结果了
                if ((theResultList[i].mValueType == AnimalValueType.HaveWings) || (theResultList[i].mValueType == AnimalValueType.NoWings))
                {
                    string strTiptxt = "";
                    soundname = LevelOneRandomGet(ramdomIndex, theResultList[i].mValueType, out strTiptxt);
                    theResultList[i].strTipText = strTiptxt;
                }             
            }

            AddFinishClip(soundname);
        }

        //结束语("谢谢你...安全回家")
        AddFinishClip("sound_lvpass" + Random.Range(1, 3));

        //Debug.LogError("level pass");
        StartCoroutine(TOver());
    }
    IEnumerator TOver()
    {
        bLevelPass = true;
        yield return new WaitForSeconds(0.5f);
        //1
        //Debug.Log(finishClipList[0]);
        AudioClip mclip0 = GetClip(finishClipList[0]);
        mAudioSourceKimi.clip = mclip0;
        mAudioSourceKimi.Play();
        yield return new WaitForSeconds(mclip0.length + 0.1f);
        //2
        AudioClip mclip1 = GetClip(finishClipList[1]);
        mAudioSourceKimi.clip = mclip1;
        mAudioSourceKimi.Play();
        if (nLevel == 1)
        {
            yield return new WaitForSeconds(4f);
            if (theResultList.Count > 0)
                SetStationOKEffect(theResultList[0]);
            yield return new WaitForSeconds(1f);
            if (theResultList.Count > 1)
                SetStationOKEffect(theResultList[1]);
            yield return new WaitForSeconds(2f);
        }
        else if (nLevel == 2)
        {
            yield return new WaitForSeconds(3.5f);
            if (theResultList.Count > 0)
                SetStationOKEffect(theResultList[0]);
            yield return new WaitForSeconds(1.7f);
            if (theResultList.Count > 1)
                SetStationOKEffect(theResultList[1]);
            yield return new WaitForSeconds(2f);
            if (theResultList.Count > 2)
                SetStationOKEffect(theResultList[2]);
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return new WaitForSeconds(3f);
            AnimalStationValue mstv0 = GetAnimalValueTypeByType(AnimalValueType.Poultry);
            if (mstv0 != null)
                SetStationOKEffect(mstv0);
            yield return new WaitForSeconds(1f);
            AnimalStationValue mstv1 = GetAnimalValueTypeByType(AnimalValueType.Livestock);
            if (mstv1 != null)
                SetStationOKEffect(mstv1);
            yield return new WaitForSeconds(1f);
            AnimalStationValue mstv2 = GetAnimalValueTypeByType(AnimalValueType.Birds);
            if (mstv2 != null)
                SetStationOKEffect(mstv2);
            yield return new WaitForSeconds(1f);
            AnimalStationValue mstv3 = GetAnimalValueTypeByType(AnimalValueType.Beast);
            if (mstv3 != null)
                SetStationOKEffect(mstv3);
            yield return new WaitForSeconds(2f);
        }
        yield return new WaitForSeconds(0.5f);

        TopTitleCtl.instance.AddStar();

        //3
        AudioClip mclip2 = GetClip(finishClipList[2]);
        mAudioSourceKimi.clip = mclip2;
        mAudioSourceKimi.Play();
        yield return new WaitForSeconds(mclip2.length + 0.1f);

        yield return new WaitForSeconds(0.3f);

        nLevel++;
        if (nLevel > nLevels)
        {
            //结算
            //Debug.LogError("Game Run Over!");
            GameOverCtl.GetInstance().Show(3, () =>
            {
                nLevel = 1;
                TopTitleCtl.instance.Reset();
                InitLevelData();
                PlayTitleSound();
            });
        }
        else
        {          
            yield return new WaitForSeconds(0.5f);
            MSceneMove(false);
            yield return new WaitForSeconds(1.3f);
            InitLevelData();
        }
    }
    private void SetStationOKEffect(AnimalStationValue _value)
    {
        AnimalClass_Station st0 = _value.mStation;
        st0.ShowTipText(_value.strTipText);
        st0.SetBigEffect();
    }
    

    /// <summary>
    /// 场景资源 移入/移出
    /// </summary>
    public void MSceneMove(bool _in)
    {
        station_lv1.SceneMove(_in);
        station_lv2.SceneMove(_in);
        station_lv3.SceneMove(_in);
        if (!_in)
        {
            for (int i = 0; i < mAnimalList.Count; i++)
            {
                mAnimalList[i].SceneMove(_in);            
            }
        }
    }

    /// <summary>
    /// 全部拖完判断错误检测
    /// </summary>
    private void MShowTipsSound()
    {
        if (theIETip != null)
        { StopCoroutine(theIETip); }
        theIETip = IEShowSoundTip();
        StartCoroutine(theIETip);
    }
    IEnumerator theIETip = null;
    IEnumerator IEShowSoundTip()
    {
        yield return new WaitForSeconds(1f);
        //Debug.LogError("不对哦,请在试试");
        AudioClip faileClip = GetClip("sound_fail_" + Random.Range(1, 6));
        mAudioSourceKimi.Stop();
        mAudioSourceKimi.clip = faileClip;
        mAudioSourceKimi.Play();
    }

    /// <summary>
    /// 第一关由于有多个结果,所以随机一个结果了
    /// </summary>
    public string LevelOneRandomGet(int ramdomIndex, AnimalValueType oldValueType,out string _tipText)
    {
        string resultGet = oldValueType.ToString();
        _tipText = "";
        if (oldValueType == AnimalValueType.HaveWings)
        {
            #region// ai
            switch (ramdomIndex)
            {
                case 1:
                    resultGet = "sound_suc_chibang";
                    _tipText = "tag_1_2";//"有翅膀";
                break;
                case 2:
                    resultGet = "sound_suc_yumao";
                    _tipText = "tag_1_4";//"有羽毛";
                    break;
                case 3:
                    resultGet = "sound_suc_shengdan";
                    _tipText = "tag_1_3";//"dan";
                    break;
                case 4:
                    resultGet = "sound_suc_tui";
                    _tipText = "tag_1_1";//"两条腿";
                    break;
                case 5:
                    resultGet = "sound_suc_chibang";
                    _tipText = "tag_1_2"; //"有翅膀";
                    break;
                default:
                    break;
            }
            #endregion
        }
        else if (oldValueType == AnimalValueType.NoWings)
        {
            #region// ai1
            switch (ramdomIndex)
            {
                case 1:
                    resultGet = "sound_suc_chibang";
                    _tipText = "tag_2_2";//"无翅膀";
                    break;
                case 2:
                    resultGet = "sound_suc_yumao";
                    _tipText = "tag_2_4";//"无羽毛";
                    break;
                case 3:
                    resultGet = "sound_suc_shengdan";
                    _tipText = "tag_2_3";//"tai生";
                    break;
                case 4:
                    resultGet = "sound_suc_tui";
                    _tipText = "tag_2_1";//"四条腿";
                    break;
                case 5:
                    resultGet = "sound_suc_chibang";
                    _tipText = "tag_2_2";//"无翅膀";
                    break;
                default:
                    break;
            }
            #endregion
        }
        return resultGet;
    }

    /// <summary>
    /// 动物obj创建
    /// </summary>
    private AnimalClass_Animal CreateAnimal(int _type)
    {
        string strAnimal = "animal" + _type;
        GameObject mGo = ResManager.GetPrefab("animalclass_prefab", strAnimal);
        mGo.transform.SetParent(mCenter.transform);
        mGo.transform.localScale = Vector3.one * fScale;
        mGo.transform.localPosition = Vector3.zero;
        mGo.SetActive(true);

        AnimalClass_Animal animalCtrl = mGo.AddComponent<AnimalClass_Animal>();
        animalCtrl.InitAwake(_type);
        return animalCtrl;
    }

    Vector3 temp_select_offset = Vector3.zero;
    AnimalClass_Animal mSelect = null;
    private void MUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            #region//one
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hits != null)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    AnimalClass_Animal com = hits[i].collider.gameObject.GetComponent<AnimalClass_Animal>();
                    if (com != null)
                    {
                        if (mSelect == null)
                        {
                            mSelect = com;
                        }
                        else if (com.transform.GetSiblingIndex() > mSelect.transform.GetSiblingIndex())
                        {
                            mSelect = com;
                        }
                    }
                }
                if (mSelect != null)
                {
                    AnimalClass_Station parentStation = null;
                    parentStation = mSelect.transform.parent.GetComponent<AnimalClass_Station>();
                    if (parentStation != null)
                    {
                        parentStation.DropOut(mSelect);
                        nCounts--;
                        if (nCounts < 0)
                            nCounts = 0;
                    }
                    mSelect.transform.SetParent(transform);
                    mSelect.transform.SetSiblingIndex(50);

                    RectTransform retf = mSelect.transform as RectTransform;
                    temp_select_offset = Common.getMouseLocalPos(transform) - retf.anchoredPosition3D;

                    mSelect.DropSet();
                    mSelect.YingziActive(false);

                    //停止声音提示
                    if (theIETip != null)
                    { StopCoroutine(theIETip); }
                }
            }
            #endregion
        }
        else if (Input.GetMouseButton(0))
        {
            #region//动物位置设置
            if (mSelect != null)
            {
                //拖动值限制
                Vector3 vInput = Common.getMouseLocalPos(transform) - temp_select_offset;
                float fX = vInput.x;
                float fY = vInput.y;
                Vector3 vsize = mSelect.GetSize();

                float fOffsetHeight = mSelect.GetColliderOffset().y * fScale;

                fX = Mathf.Clamp(fX, -GlobalParam.screen_width * 0.5f + vsize.x * 0.5f, GlobalParam.screen_width * 0.5f - vsize.x * 0.5f);
                fY = Mathf.Clamp(fY, -GlobalParam.screen_height * 0.5f + vsize.y * 0.5f - fOffsetHeight, GlobalParam.screen_height * 0.5f - vsize.y * 0.5f - fOffsetHeight);
                mSelect.SetLocalPos(new Vector3(fX, fY, 0f));
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//two
            if (mSelect != null)
            {
                mSelect.DropReset();

                bool bLevelPass = false;
                AnimalClass_Station _station = null;
                GameObject outSiseHit = null;

                #region//射线检测
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hits != null)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].collider.gameObject.name.CompareTo("collideroutside") == 0)
                        {
                            outSiseHit = hits[i].collider.gameObject;
                        }
                        if (_station == null)
                        {
                            _station = hits[i].collider.transform.parent.GetComponent<AnimalClass_Station>();
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                #endregion

                #region//拖到staion
                if (_station != null)
                {
                    //限制数量12ge
                    if (_station.mAnimalList.Count >= _station.setposList.Count)
                    {
                        mSelect.BackToRemaidPos();
                    }
                    else
                    {
                        _station.DropIn(mSelect);
                        nCounts++;
                        if (nCounts >= 14)
                        {
                            bLevelPass = CheckLevelIsPass();
                        }
                    }
                }
                else
                {
                    if (outSiseHit != null)
                    {
                        mSelect.BackToRemaidPos();
                    }
                    else
                    {
                        mSelect.SetRemaidPos(mSelect.transform.localPosition);
                        mSelect.YingziActive(true);
                    }
                }
                #endregion

                if (bLevelPass)
                {
                    MLevelPass();
                }
                else
                {
                    if (nCounts >= 14)
                    {
                        MShowTipsSound();
                    }
                }

                OrderSetting();

                mSelect = null;
            }
            #endregion
        }
    }

    /// <summary>
    /// 检测关卡是否完成
    /// </summary>
    private bool CheckLevelIsPass()
    {
        bool bbok = false;
        int mmCheckCount = 2;
        theResultList.Clear();

        if (nLevel == 1)
        {
            mmCheckCount = 2;
            theResultList = GetResurtInfoByLevel1();       
        }
        else if (nLevel == 2)
        {
            mmCheckCount = 3;
            theResultList = GetResurtInfoByLevel2();
        }
        else
        {
            mmCheckCount = 4;
            theResultList = GetResurtInfoByLevel3();
            //检测是否与设定好的属性匹配
            for (int i = 0; i < theResultList.Count; i++)
            {
                int nvaluetype = (int)theResultList[i].mValueType;
                if (nvaluetype == 34)
                { theResultList[i].mValueType = AnimalValueType.Poultry; }
                else if (nvaluetype == 35)
                { theResultList[i].mValueType = AnimalValueType.Birds; }

                if (theResultList[i].mStation.theAnimalValueType != theResultList[i].mValueType)
                { return false; }
            }
        }

        if (theResultList.Count == mmCheckCount)
        {
            bbok = true;
        }

        return bbok;
    }

    private List<AnimalStationValue> GetResurtInfoByLevel1()
    {
        List<AnimalStationValue> getValue = new List<AnimalStationValue>();
        getValue = GetResurtInfosS(new int[] { 1, 2 });
        if (getValue.Count == 2)
        {
            return getValue;
        }
        getValue = GetResurtInfosS(new int[] { 3, 4 });
        if (getValue.Count == 2)
        {
            return getValue;
        }
        getValue = GetResurtInfosS(new int[] { 5, 6 });
        if (getValue.Count == 2)
        {
            return getValue;
        }
        getValue = GetResurtInfosS(new int[] { 7, 8 });
        if (getValue.Count == 2)
        {
            return getValue;
        }
        getValue = GetResurtInfosS(new int[] { 9, 10 });
        if (getValue.Count == 2)
        {
            return getValue;
        }
        return getValue;
    }
    private List<AnimalStationValue> GetResurtInfoByLevel2()
    {
        List<AnimalStationValue> getValue = new List<AnimalStationValue>();
        getValue = GetResurtInfosS(new int[] { 20, 21, 22 });
        if (getValue.Count == 3)
        {
            return getValue;
        }
        getValue = GetResurtInfosS(new int[] { 20, 23, 24 });
        if (getValue.Count == 3)
        {
            return getValue;
        }
        getValue = GetResurtInfosS(new int[] { 20, 25, 26 });
        if (getValue.Count == 3)
        {
            return getValue;
        }
        return getValue;
    }
    private List<AnimalStationValue> GetResurtInfoByLevel3()
    {
        List<AnimalStationValue> getValue = new List<AnimalStationValue>();
        getValue = GetResurtInfosS(new int[] { 30, 31, 32 ,33 });
        if (getValue.Count == 4)
        {
            return getValue;
        }
        getValue = GetResurtInfosS(new int[] { 34, 31, 35, 33 });
        if (getValue.Count == 4)
        {
            return getValue;
        }
        return getValue;
    }

    private List<AnimalStationValue> GetResurtInfosS(int[] _types)
    {
        List<AnimalStationValue> getValue = new List<AnimalStationValue>();
        List<int> valueList = new List<int>();

        for (int i = 0; i < mStationList.Count; i++)
        {
            for (int j = 0; j < _types.Length; j++)
            {
                int valuetype = _types[j];
                if (CheckStationOK(valuetype, mStationList[i]))
                {
                    if (!CheckHasSameStation(getValue, i))
                    {
                        AnimalStationValue maa = new AnimalStationValue();
                        maa.nID = i;
                        maa.mValueType = (AnimalValueType)valuetype;
                        maa.mStation = mStationList[i];
                        maa.mAnimalList = mStationList[i].mAnimalList;

                        getValue.Add(maa);
                        valueList.Add(valuetype);
                    }
                }
            }          
        }
        return getValue;
    }

    //检测是否有相同的station
    public bool CheckHasSameStation(List<AnimalStationValue> getValue,int _id)
    {
        AnimalStationValue hehe = getValue.Find((x) => { return x.nID == _id; });
        if (hehe != null)
        {
            //Debug.Log("有相同的station了");
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测station的动物 是否与属性 匹配
    /// </summary>
    /// <param name="_valueType"></param>
    /// <param name="_station"></param>
    /// <returns></returns>
    private bool CheckStationOK(int _valueType, AnimalClass_Station _station)
    {
        bool okok = true;

        //没有动物直接返回false
        if (_station.mAnimalList.Count <= 0)
            return false;

        //_valueType这个属性 中的动物列表
        List<int> theValueAnimals = dicCheckList[_valueType];

        for (int i = 0; i < theValueAnimals.Count; i++)
        {
            int theAnimalID = theValueAnimals[i];
            if (_station.CheckIsHaveAnimal(theAnimalID))
            { }
            else
            {
                okok = false;
                break;
            }
        }

        return okok;
    }

    /// <summary>
    /// station大小
    /// </summary>
    /// <returns></returns>
    public float GetStationScale()
    {
        if (nLevel == 2)
        {
            return 0.8f;
        }
        else
        {
            return 1f;
        }
    }

    /// <summary>
    /// 层设置
    /// </summary>
    public void OrderSetting()
    {
        int index = 0;
        for (int i = 0; i < mAnimalList.Count - 1; ++i)
        {
            index = i;
            for (int j = i + 1; j < mAnimalList.Count; ++j)
            {
                if (mAnimalList[j].transform.position.y > mAnimalList[index].transform.position.y)
                    index = j;
            }
            AnimalClass_Animal t = mAnimalList[index];
            mAnimalList[index] = mAnimalList[i];
            mAnimalList[i] = t;
            //Debug.Log(mAnimalList[i].gameObject.name);
            mAnimalList[i].transform.SetSiblingIndex(40);
        }
        mAnimalList[mAnimalList.Count - 1].transform.SetSiblingIndex(40);
    }


    
    #region//sound
    public void PlayBGSound()
    {
        SoundManager.instance.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.1f);
    }
    private AudioSource mAudioSourceKimi;
    private AudioSource mASSort;
    List<AudioClip> kimiClipList = new List<AudioClip>();
    private System.Action mPlaySoundCallback = null;
    IEnumerator theIEPlaySound = null;
    /// <summary>
    /// 播放Kimi语音
    /// </summary>
    public void PlayKimiSounds(List<AudioClip> _clipList, System.Action _callback = null)
    {
        if (theIEPlaySound != null)
            StopCoroutine(theIEPlaySound);

        kimiClipList.Clear();
        mPlaySoundCallback = _callback;
        kimiClipList = _clipList;

        theIEPlaySound = IEPlayKimiSounds();
        StartCoroutine(theIEPlaySound);
    }
    IEnumerator IEPlayKimiSounds()
    {
        for (int i = 0; i < kimiClipList.Count; i++)
        {
            float clipLegth = kimiClipList[i].length;
            mAudioSourceKimi.clip = kimiClipList[i];
            mAudioSourceKimi.Play();
            yield return new WaitForSeconds(clipLegth + 0.1f);
        }
        if (mPlaySoundCallback != null)
            mPlaySoundCallback();
    }
    /// <summary>
    /// 播放短暂的声音
    /// </summary>
    public void PlaySortSound(AudioClip _clip,float _volume = 1f)
    {
        mASSort.volume = _volume;
        mASSort.Stop();
        mASSort.clip = _clip;
        mASSort.Play();
    }

    public AudioClip GetClip(string _clipName)
    {
        AudioClip clip1 = ResManager.GetClip("animalclass_sound", _clipName);
        return clip1;
    }


    /// <summary>
    /// 开始语音播放
    /// </summary>
    public void PlayTitleSound()
    {
        List<AudioClip> startclipList = new List<AudioClip>();
        //AudioClip clip1 = GetClip("sound_title");
        //startclipList.Add(clip1);
        AudioClip clip2 = GetClip("game-tips1-2-1");
        startclipList.Add(clip2);
        PlayKimiSounds(startclipList);
    }

    /// <summary>
    /// 直接播放短暂的声音
    /// </summary>
    public void PlaySortSound(string _clipName, float _volume = 1f)
    {
        AudioClip clip1 = GetClip(_clipName);
        PlaySortSound(clip1, _volume);
    }
    #endregion

    /// <summary>
    /// 玩法提示语音重播
    /// </summary>
    public void ReplayTipSound()
    {
        if (bLevelPass)
            return;
        List<AudioClip> startclipList = new List<AudioClip>();
        string strcpname = "game-tips1-2-1";
        if (nLevel == 3)
        { strcpname = "sound_game"; }
        else if (nLevel == 2)
        { strcpname = "game-tips1-2-1A"; }
        AudioClip clip2 = GetClip(strcpname);
        startclipList.Add(clip2);
        PlayKimiSounds(startclipList);
    }

}
