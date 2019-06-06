using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class RegularOrderCtrl : BaseScene
{
    /// <summary>
    /// 行
    /// </summary>
    [HideInInspector]
    public int nRow = 8;
    /// <summary>
    /// 列
    /// </summary>
    [HideInInspector]
    public int nCol = 6;


    public int nLevel = 0;
    private const int nLevels = 3;

    public int nCount = 0;
    public int nToCount = 0;

    /// <summary>
    /// 当前类型
    /// </summary>
    public int nNowType = 0;

    /// <summary>
    /// fuck
    /// </summary>
    private bool bCanSetObj = false;
    private bool bLevelPass = false;

    private Image scenebg;
    private Image panel;
    private GridLayoutGroup grid;
    private GameObject blockobj;

    private int nremaidGet = 0;
    private RegularOrderDefine miniMap = new RegularOrderDefine();

    private RegularOrderBlockObj[,] mStations = null;

    private RegularOrderSelectObj mDropObj1;
    private RegularOrderSelectObj mDropObj2;
    private RegularOrderSelectObj mDropObj3;

    private RegularOrderSelectObj mSelect = null;

    private Sprite bed24;
    private Sprite bed48;

    private Image head1;
    private Image head2;
    private Image head3;

    particleSystest clickEffectCtrl;
    ParticleSystem finestar;

    private RectTransform getpicpanel;
    private Button yesbtn;
    private Button nobtn;
    private GetScreenPicController mGetScreenPicCtrl;

    /// <summary>
    /// 随机的动物
    /// </summary>
    public List<int> mTheAnimalType = new List<int>();

    private PlaySoundController mSoundCtrl;
    public PlaySoundController SoundCtrl { get { return mSoundCtrl; } }

    void Awake()
    {
        mSceneType = SceneEnum.RegularOrder;
        CallLoadFinishEvent();

        clickEffectCtrl = gameObject.AddComponent<particleSystest>();

        mStations = new RegularOrderBlockObj[nRow, nCol];

        scenebg = transform.Find("scenebg").GetComponent<Image>();
        panel = transform.Find("panel").GetComponent<Image>();
        grid = panel.transform.Find("grid").GetComponent<GridLayoutGroup>();
        grid.transform.localPosition = new Vector3(-162f, -10f, 0f);
        scenebg.rectTransform.sizeDelta = new Vector2(1280f, 800f);

        scenebg.sprite = CreateSpriteByTexture(ResManager.GetTexture("regularorder_texture", "cdgl_bj"));

        panel.transform.localPosition = new Vector3(-1500f, 0f, 0f);

        blockobj = transform.Find("blockobj").gameObject;
        blockobj.SetActive(false);

        finestar = transform.Find("finestar").GetComponent<ParticleSystem>();
        finestar.Pause();
        finestar.Stop();

        getpicpanel = transform.Find("getpicpanel") as RectTransform;
        yesbtn = getpicpanel.Find("yesbtn").GetComponent<Button>();
        nobtn = getpicpanel.Find("nobtn").GetComponent<Button>();
        EventTriggerListener.Get(yesbtn.gameObject).onClick = ClickGetTextureBtn;
        EventTriggerListener.Get(nobtn.gameObject).onClick = ClickGetTextureBtn;
        getpicpanel.gameObject.SetActive(false);
    }

    void Start ()
    {
        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        mSoundCtrl.InitAwake();
        mSoundCtrl.SetDelayLoadBGClip(1f);
        StartCoroutine(MMStart());
    }
    private IEnumerator MMStart()
    {
        yield return new WaitForSeconds(0.1f);
        mSoundCtrl.PlayBGSound1("bgmusic_loop0", "bgmusic_loop0");
        #region//scene res
        bed24 = CreateSpriteByTexture(ResManager.GetTexture("regularorder_texture", "cdgl_24"));
        bed48 = CreateSpriteByTexture(ResManager.GetTexture("regularorder_texture", "cdgl_48"));
        panel.sprite = bed24;
        panel.rectTransform.sizeDelta = new Vector2(855f, 698f);

        head1 = panel.transform.Find("head1").GetComponent<Image>();
        head2 = panel.transform.Find("head2").GetComponent<Image>();
        head3 = panel.transform.Find("head3").GetComponent<Image>();
        head1.sprite = ResManager.GetSprite("regularorder_sprite", "pic3");
        head2.sprite = ResManager.GetSprite("regularorder_sprite", "pic3");
        head3.sprite = ResManager.GetSprite("regularorder_sprite", "pic3");
        head1.SetNativeSize();
        head2.SetNativeSize();
        head3.SetNativeSize();


        getpicpanel.GetComponent<Image>().sprite = ResManager.GetSprite("regularorder_sprite", "cdgl_tan");
        yesbtn.GetComponent<Image>().sprite = ResManager.GetSprite("regularorder_sprite", "cdgl_bc");
        nobtn.GetComponent<Image>().sprite = ResManager.GetSprite("regularorder_sprite", "cdgl_qx");

        Image leftimg = UguiMaker.newImage("leftimg", transform, "regularorder_sprite", "cdgl_guolan_02", false);
        leftimg.transform.SetSiblingIndex(2);
        leftimg.rectTransform.localScale = Vector3.one;
        Vector3 vleftimg = new Vector3(-GlobalParam.screen_width * 0.5f, -GlobalParam.screen_height * 0.5f, 0);
        leftimg.rectTransform.anchoredPosition3D = vleftimg + new Vector3(leftimg.rectTransform.sizeDelta.x * 0.5f, leftimg.rectTransform.sizeDelta.y * 0.5f, 0f);

        mGetScreenPicCtrl = GetScreenPicController.Create(transform);
        mGetScreenPicCtrl.SetReadyAction(() =>
        {
            getpicpanel.gameObject.SetActive(false);
            clickEffectCtrl.ActiveEffect(false);
        });
        mGetScreenPicCtrl.SetFinishAction(() =>
        {
            clickEffectCtrl.ActiveEffect(true);
            GameOverCtl.GetInstance().Show(3, () =>
            {
                nLevel = 1;
                TopTitleCtl.instance.Reset();
                PlayStartSound();
                InitLevelData();
            });
        });
        #endregion

        nLevel = 1;

        miniMap.InitMapDataInfo(1);
        miniMap.InitAnserPos();

        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(LvStartSound);
        PlayStartSound();


        panel.transform.localScale = Vector3.one * GlobalParam.screen_width / 1423f;

        mDropObj1 = ResManager.GetPrefab("regularorder_prefab", "pig").AddComponent<RegularOrderSelectObj>();
        mDropObj1.InitAwake(1, panel.transform);
        mDropObj1.rectTransform.anchoredPosition3D = new Vector3(300f, 150f, 0f);

        mDropObj2 = ResManager.GetPrefab("regularorder_prefab", "flower").AddComponent<RegularOrderSelectObj>();
        mDropObj2.InitAwake(2, panel.transform);
        mDropObj2.rectTransform.anchoredPosition3D = new Vector3(300f, -185f, 0f);

        mDropObj3 = ResManager.GetPrefab("regularorder_prefab", "kong").AddComponent<RegularOrderSelectObj>();
        mDropObj3.InitAwake(3, panel.transform);
        mDropObj3.rectTransform.anchoredPosition3D = new Vector3(300f, 0f, 0f);

        //Debug.Log(this.ToString() + " res ready ok");      
        InitLevelData();
    }

    /// <summary>
    /// 创建blackObj
    /// </summary>
    public RegularOrderBlockObj CreateBlockCtrl(Transform _parent, string _name)
    {
        GameObject go = GameObject.Instantiate(blockobj) as GameObject;
        go.name = _name;
        go.SetActive(true);
        go.transform.SetParent(_parent.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        RegularOrderBlockObj blockCtrl = go.AddComponent<RegularOrderBlockObj>();
        blockCtrl.InitAwake();
        blockCtrl.SetSprites();
        return blockCtrl;
    }

    public Sprite CreateSpriteByTexture(Texture _texture)
    {
        Sprite _sp = null;
        Texture2D _t2d = _texture as Texture2D;
        _sp = Sprite.Create(_t2d, new Rect(0f, 0f, _t2d.width, _t2d.height), Vector2.zero);
        _sp.name = _texture.name;
        return _sp;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitLevelData();
        }
    }

    /// <summary>
    /// 初始化信息
    /// </summary>
    public void InitLevelData()
    {
        ResetInfos();
        bLevelPass = false;

        //兔子白色 和 底盘颜色相同 忽略11
        mTheAnimalType = Common.GetIDList(1, 14, 2, 11);
        mDropObj1.SetAnimalType(mTheAnimalType[0]);
        mDropObj3.SetAnimalType(mTheAnimalType[1]);

        #region//set data
        if (nLevel <= 2)
        {
            nRow = 8;
            nCol = 6;
            panel.sprite = bed48;
            panel.rectTransform.sizeDelta = new Vector2(875f, 698f);
            grid.transform.localPosition = new Vector3(-120f, 12f, 0f);
            grid.cellSize = new Vector2(95f, 82f);

            mDropObj3.gameObject.SetActive(false);
            mDropObj1.transform.localPosition = new Vector3(300f, 150f, 0f);
            mDropObj2.transform.localPosition = new Vector3(300f, -180f, 0f);

            head1.rectTransform.anchoredPosition = new Vector2(300f, 150f);
            head2.rectTransform.anchoredPosition = new Vector2(300f, -180f);
            head3.gameObject.SetActive(false);
        }
        else
        {
            nRow = 6;
            nCol = 4;
            panel.sprite = bed24;
            panel.rectTransform.sizeDelta = new Vector2(864.9f, 701.7f);
            grid.cellSize = new Vector2(142f, 110f);
            grid.transform.localPosition = new Vector3(-124.6f, 3.4f, 0f);

            mDropObj3.gameObject.SetActive(true);
            mDropObj1.transform.localPosition = new Vector3(300f, 250f, 0f);
            mDropObj2.transform.localPosition = new Vector3(300f, -240f, 0f);
            mDropObj3.transform.localPosition = new Vector3(300f, 0f, 0f);

            head1.rectTransform.anchoredPosition = new Vector2(300f, 250f);
            head2.rectTransform.anchoredPosition = new Vector2(300f, -240f);
            head3.gameObject.SetActive(true);

        }
        grid.constraintCount = nCol;
        #endregion

        mStations = new RegularOrderBlockObj[nRow, nCol];
        for (int i = 0; i < nRow; i++)
        {
            for (int j = 0; j < nCol; j++)
            {
                mStations[i, j] = CreateBlockCtrl(grid.transform, "blockObj" + i + "_" + j);
                mStations[i, j].SetType(0);
                mStations[i, j].bStation = true;
            }
        }


        if (nLevel <= 2)
        {
            SetLevelData_1_Or_2();
        }
        else
        {
            //全空
            SetLevelData_3();
        }

        SceneMove(true);
        bCanSetObj = true;
    }

    /// <summary>
    /// 关卡1 和 2 信息设置
    /// </summary>
    public void SetLevelData_1_Or_2()
    {
        int ngetCount = 0;
        if (nLevel == 1)
        { ngetCount = 4; }
        else if (nLevel == 2)
        { ngetCount = 5; }
        nToCount = ngetCount;

        //取得信息列表
        List<int> getList = Common.GetIDList(1, 6, 1, nremaidGet);
        int mapID = getList[0];
        //Debug.Log("map id:" + mapID);
        nremaidGet = mapID;
        miniMap.InitMapDataInfo(mapID);
        int[,] mapInfo = miniMap.myMap;
        //随机出空缺
        List<RegularOrderPoint> indexList = new List<RegularOrderPoint>();
        for (int i = 0; i < nRow; i++)
        {
            for (int j = 0; j < nCol; j++)
            {
                if (mapInfo[i, j] > 0 && mapInfo[i, j] != 3)
                {
                    indexList.Add(new RegularOrderPoint(i, j));
                }
            }
        }
        //--
        indexList = Common.BreakRank(indexList);
        miniMap.InitAnserPos();
        //--
        for (int i = 0; i < ngetCount; i++)
        {
            RegularOrderPoint thePoint = indexList[i];
            //替换类型
            mStations[thePoint.x, thePoint.y].bKong = true;
            mStations[thePoint.x, thePoint.y].bCanChange = true;
        }
        //set type
        for (int i = 0; i < nRow; i++)
        {
            for (int j = 0; j < nCol; j++)
            {
                mStations[i, j].SetType(mapInfo[i, j]);
            }
        }

        if (nLevel == 2)
        { LvStartSound(); }
    }
    //关卡3信息设置
    public void SetLevelData_3()
    {
        nToCount = nRow * nCol;
        //set type
        for (int i = 0; i < nRow; i++)
        {
            for (int j = 0; j < nCol; j++)
            {
                mStations[i, j].bKong = true;
                mStations[i, j].bCanChange = true;
                mStations[i, j].SetType(0);
            }
        }
        bCanSetIn = true;
        if (nLevel == 3)
        { LvStartSound(); }
    }



    /// <summary>
    /// 重置信息
    /// </summary>
    public void ResetInfos()
    {
        nCount = 0;
        nToCount = 0;
        mSelect = null;
        bCanSetObj = false;

        if (mStations != null)
        {
            foreach (RegularOrderBlockObj info in mStations)
            {
                if (info != null && info.gameObject != null)
                {
                    GameObject.Destroy(info.gameObject);
                }
            }
        }
        mStations = null;

        mDropObj1.ResetInfos();
        mDropObj2.ResetInfos();
        mDropObj3.ResetInfos();

        head1.gameObject.SetActive(true);
        head2.gameObject.SetActive(true);
    }

    /// <summary>
    /// 关卡完成
    /// </summary>
    public void MLevelPass()
    {
        //Debug.LogError("level pass");
        bCanSetObj = false;
        StartCoroutine(TOver());
    }
    IEnumerator TOver()
    {
        bLevelPass = true;
        StopIEStartSound();

        yield return new WaitForSeconds(1f);

        //成功声音
        AudioClip sucClip = GetClip("sound_suc" + Random.Range(1, 4));
        PlaySound(sucClip);

        List<RegularOrderBlockObj> mline1 = GetList1();
        for (int i=0;i<mline1.Count;i++)
        {
            if (nLevel <= 2)
            {
                if (mline1[i].nType != 3)
                {
                    mline1[i].DoBigEffect();
                    PlaySortSound("sound_big");
                    yield return new WaitForSeconds(0.3f);
                }
            }
            else
            {
                mline1[i].DoBigEffect();
                PlaySortSound("sound_big");
                yield return new WaitForSeconds(0.3f);
            }
        }
        List<RegularOrderBlockObj> mline2 = GetList2();
        for (int i = 0; i < mline2.Count; i++)
        {
            if (nLevel <= 2)
            {
                if (mline2[i].nType != 3)
                {
                    mline2[i].DoBigEffect();
                    PlaySortSound("sound_big");
                    yield return new WaitForSeconds(0.3f);
                }
            }
            else
            {
                mline2[i].DoBigEffect();
                PlaySortSound("sound_big");
                yield return new WaitForSeconds(0.3f);
            }
        }
        List<RegularOrderBlockObj> mline3 = GetList3();
        for (int i = 0; i < mline3.Count; i++)
        {
            if (nLevel <= 2)
            {
                if (mline3[i].nType != 3)
                {
                    mline3[i].DoBigEffect();
                    PlaySortSound("sound_big");
                    yield return new WaitForSeconds(0.3f);
                }
            }
        }
              
        //yield return new WaitForSeconds(sucClip.length + 0.1f);

        TopTitleCtl.instance.AddStar();
        nLevel++;
        if (nLevel > nLevels)
        {
            yield return new WaitForSeconds(1.3f);
            GameOverCtl.GetInstance().Show(3, () =>
            {
                nLevel = 1;
                TopTitleCtl.instance.Reset();
                TopTitleCtl.instance.mSoundTipData.SetData(LvStartSound);
                PlayStartSound();
                InitLevelData();
            });
        }
        else
        {
            yield return new WaitForSeconds(0.4f);
            SceneMove(false);
            yield return new WaitForSeconds(1.3f);
            InitLevelData();
        }
    }

    public void SceneMove(bool _in)
    {
        if (_in)
        {
            //AudioClip mclip = GetClip("sound_scenemove");
            //PlaySound(mASSort, mclip);
            PlaySortSound("sound_scenemove");

            mDropObj1.transform.localScale = Vector3.one * 0.001f;
            mDropObj2.transform.localScale = Vector3.one * 0.001f;
            mDropObj3.transform.localScale = Vector3.one * 0.001f;

            panel.transform.localPosition = new Vector3(-1500f, 0f, 0f);
            panel.transform.DOLocalMove(Vector3.zero, 1f).OnComplete(()=> 
            {
                mDropObj1.SceneMove(_in);
                mDropObj2.SceneMove(_in);
                mDropObj3.SceneMove(_in);
            });
        }
        else
        {
            panel.transform.DOLocalMove(new Vector3(1500f, 0f, 0f), 1f);
        }
    }

    /// <summary>
    /// 设置选中的obj
    /// </summary>
    public void SetSelectObj(RegularOrderBlockObj _obj)
    {
        //StopIEStartSound();
        if (!bCanSetObj)
            return;
        //station设置
        if (_obj.bStation)
        {
            if (mSelect != null && _obj.bCanChange)
            {               
                if (nLevel >= 3) //关卡3
                {
                    if (bCanSetIn)//能否设置station
                    {
                        PlaySortSound("sound_add");                     
                        _obj.bKong = false;
                        _obj.SetType(mSelect.nType);
                        _obj.OKSeting();
                        CheckLevel3();
                    }
                }
                else //关卡1,2
                {
                    if (_obj.StationDropInSet(mSelect.nType))
                    {
                        PlaySortSound("sound_add");
                        nCount++;
                        _obj.OKSeting();
                        if (nCount >= nToCount && nToCount > 0)
                        {
                            MLevelPass();
                        }
                    }
                    else
                    {
                        //Debug.LogError("匹配错误");
                        AudioClip failClip = GetClip("sound_fail" + Random.Range(1, 3));
                        PlaySound(failClip);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 点击选中
    /// </summary>
    public void SetSelectObj(RegularOrderSelectObj _obj)
    {
        //StopIEStartSound();
        if (!bCanSetObj)
            return;
        //设置选中的obj
        if (mSelect != _obj)
        {
            if (mSelect != null)
            {
                mSelect.DropReset();
            }
            mSelect = _obj;
            mSelect.DropSet();

            if (mSelect.nTheAnimalType == 0)
            {
                PlaySortSound("sound_obj" + _obj.nType);
            }
            else
            {
                string anmalname = MDefine.GetAnimalNameByID_CH(mSelect.nTheAnimalType);
                AudioClip cp0 = ResManager.GetClip("aa_animal_sound", anmalname + 0);
                AudioSource.PlayClipAtPoint(cp0, Camera.main.transform.position);
            }
        }
    }




    bool bCanSetIn = true;
    /// <summary>
    /// 关卡3检测通关
    /// </summary>
    private void CheckLevel3()
    {
        if (IsAllInStation())
        {
            int okID = 0;
            bool bCheckOk = CheckOrderIsOK(out okID);
            if (bCheckOk)
            {
                bCanSetIn = false;
                //Debug.Log("data ok");
                MLevelPass();
                if (theIESoundTip != null)
                {
                    StopCoroutine(theIESoundTip);
                }
            }
            else
            {
                if (theIESoundTip != null)
                {
                    StopCoroutine(theIESoundTip);
                }
                if (okID == 0) //正常的匹配失败
                {
                    //Debug.Log("匹配失败了");               
                    theIESoundTip = IEShowSoundTip();
                    StartCoroutine(theIESoundTip);
                }
                else if(okID == -1) //值全相同失败
                {
                    //Debug.Log("值全部相同, 不能这样子, 要至少用上2种来拼图");
                    theIESoundTip = IEShowSoundTip2();
                    StartCoroutine(theIESoundTip);
                }           
            }
        }
    }
    IEnumerator theIESoundTip = null;
    IEnumerator IEShowSoundTip()
    {
        yield return new WaitForSeconds(2f);
        bCanSetIn = false;
        BedShakeLR(() => { bCanSetIn = true; });
        //Debug.LogError("不符合哦,请在试试");
        AudioClip failClip = GetClip("sound_fail" + Random.Range(1, 3));
        PlaySound(failClip);    
    }
    IEnumerator IEShowSoundTip2()
    {
        yield return new WaitForSeconds(1f);
        bCanSetIn = false;
        BedShakeLR(() => { bCanSetIn = true; });
        //Debug.LogError("不符合哦,请在试试");
        AudioClip failClip = GetClip("至少要有2种图案");
        PlaySound(failClip);
    }



    /// <summary>
    /// 是否填了全部
    /// </summary>
    private bool IsAllInStation()
    {
        bool isAllIn = true;
        for (int i = 0; i < nRow; i++)
        {
            for (int j = 0; j < nCol; j++)
            {
                if (mStations[i, j].nType <= 0)
                {
                    return false;
                }
            }
        }
        return isAllIn;
    }

    #region//数据检测----

    private int[,] oldDataObj = null;
    /// <summary>
    /// 检测数据是否有规律排序
    /// </summary>
    /// <param name="_OKID">-1 值相同失败;  0 正常的匹配失败;  1 匹配成功</param>
    /// <returns></returns>
    public bool CheckOrderIsOK(out int _OKID)
    {
        bool checkOK = false;
        _OKID = 0;

        //station数据化
        oldDataObj = InitDatasByStations();

        //检测值是否全相同
        bool bSameID = CheckTheIDIsSame(oldDataObj);
        if (bSameID)
        {
            _OKID = -1;
            return false;
        }

        //
        for (int i = 1; i <= 3; i++)
        {
            //if (i == 3)
            //{
            //    continue;
            //}

            for (int j = 1; j <= 2; j++)
            {
                checkOK = testInitDateIsOk(i, j);
                if (checkOK)
                {
                    _OKID = 1;
                    return checkOK;
                }
            }
        }       
        return checkOK;
    }

    private bool testInitDateIsOk(int _row,int _col)
    {
        bool isOKok = false;

        int[,] newDataObj = null;
        newDataObj = new int[nRow, nCol];

        //根据行和列截取一个新的二维数组
        int[,] testDate = GetGroupByIndexRowAndCol(oldDataObj, _row, _col);

        int dataRow = testDate.GetLength(0);//行
        int dataCol = testDate.GetLength(1);//列

        int getRow = nRow / dataRow;
        int getCol = nCol / dataCol;
        for (int i = 0; i < getRow; i++)
        {          
            for (int j = 0; j < getCol; j++)
            {
                //testDate里的信息
                for (int xx = 0; xx < dataRow; xx++)
                {
                    for (int yy = 0; yy < dataCol; yy++)
                    {
                        int getValue = testDate[xx, yy];

                        newDataObj[i * dataRow + xx, j * dataCol + yy] = getValue;
                    }
                }
            }
        }

        isOKok = CheckTheSame(oldDataObj, newDataObj);
        return isOKok;
    }


    /// <summary>
    /// 根据行和列截取一个新的二维数组
    /// </summary>
    public int[,] GetGroupByIndexRowAndCol(int[,] _orlData, int _row, int _col)
    {
        int[,] _result = new int[_row, _col];
        for (int i=0;i<_row;i++)
        {
            string ddd = "";
            for (int j = 0; j < _col; j++)
            {
                _result[i, j] = _orlData[i, j];
                ddd = ddd + " " + _result[i, j];
            }
            //Debug.Log(ddd);
        }
        return _result;
    }

    /// <summary>
    /// station数据化
    /// </summary>
    /// <returns></returns>
    public int[,] InitDatasByStations()
    {
        int[,] _result = new int[nRow, nCol];
        for (int i = 0; i < nRow; i++)
        {
            for (int j = 0; j < nCol; j++)
            {
                _result[i, j] = mStations[i, j].nType;
            }
        }
        return _result;
    }

    /// <summary>
    /// 检测两个二维数组里的值是否相同
    /// </summary>
    public bool CheckTheSame(int[,] dataA, int[,] dataB)
    {
        bool isTheSame = true;
        for (int i = 0; i < nRow; i++)
        {
            for (int j = 0; j < nCol; j++)
            {
                if (dataA[i, j] != dataB[i, j])
                {
                    isTheSame = false;
                    return isTheSame;
                }
            }
        }
        return isTheSame;
    }

    /// <summary>
    /// 检测二维数组的值是否相同
    /// </summary>
    /// <param name="datas"></param>
    /// <returns></returns>
    public bool CheckTheIDIsSame(int[,] datas)
    {
        bool bIsAllSameID = true;
        int nfirstID = oldDataObj[0, 0];
        for (int i = 0; i < nRow; i++)
        {
            for (int j = 0; j < nCol; j++)
            {
                if (nfirstID != oldDataObj[i, j])
                {
                    bIsAllSameID = false;
                    return bIsAllSameID;
                }
            }
        }
        return bIsAllSameID;
    }
    #endregion


    /// <summary>
    /// 床shake
    /// </summary>
    /// <param name="_callback"></param>
    public void BedShakeLR(System.Action _callback = null)
    {
        Vector3 vOld = panel.transform.localPosition;
        panel.transform.DOLocalMove(vOld + new Vector3(20f, 0f, 0f), 0.15f).OnComplete(() =>
        {
            panel.transform.DOLocalMove(vOld + new Vector3(-20f, 0f, 0f), 0.3f).OnComplete(() =>
            {
                panel.transform.DOLocalMove(vOld, 0.3f).OnComplete(() =>
                {
                    if (_callback != null)
                    { _callback(); }
                });
            });
        });
    }



    #region//sound
    /// <summary>
    /// 播放声音
    /// </summary>
    public void PlaySound(AudioClip _clip)
    {
        mSoundCtrl.PlaySound(_clip, 1f);
    }

    public AudioClip GetClip(string _clipName)
    {
        AudioClip clip1 = ResManager.GetClip("regularorder_sound", _clipName);
        return clip1;
    }

    public void PlaySortSound(string _cpName)
    {
        AudioClip cp = GetClip(_cpName);
        mSoundCtrl.PlaySortSound(cp);
    }


    public void PlayStartSound()
    {
        StartCoroutine(IEPlayStartSound());
    }
    IEnumerator IEPlayStartSound()
    {
        yield return new WaitForSeconds(0.1f);
        bCanSetObj = true;
        LvStartSound();
    }
    public void LvStartSound()
    {
        if (bLevelPass)
            return;
        StopIEStartSound();
        theIEStartSound = IELvStartSound();
        StartCoroutine(theIEStartSound);
    }
    IEnumerator theIEStartSound = null;
    public void StopIEStartSound()
    {
        finestar.Stop();
        mSoundCtrl.StopTipSound();
        if (theIEStartSound != null)
        { StopCoroutine(theIEStartSound); }
        finestar.Stop();
    }
    IEnumerator IELvStartSound()
    {
        string soundname = "sound_tip1";
        if (nLevel == 3)
        {
            soundname = "sound_tip2";
        }

        AudioClip titleClip1 = GetClip(soundname);
        PlaySound(titleClip1);

        if (nLevel <= 2)
        {
            yield return new WaitForSeconds(2f);
            //pos
            Vector3 vpos0 = mStations[0, 0].transform.position;
            Vector3 vpos1 = mStations[0, nCol - 1].transform.position;
            Vector3 vpos2 = mStations[nRow - 1, 0].transform.position;
            Vector3 vpos3 = Vector3.zero;
            if (nLevel <= 2)
            { vpos3 = mStations[5, 5].transform.position; }
            else
            { vpos3 = mStations[3, 3].transform.position; }
            //sys move
            finestar.transform.position = vpos0;
            finestar.Play();
            finestar.transform.DOMove(vpos1, 1f).SetEase(Ease.Linear);
            PlaySortSound("sound_starmove");
            yield return new WaitForSeconds(1.3f);
            finestar.transform.position = vpos0;
            finestar.Play();
            finestar.transform.DOMove(vpos2, 1f).SetEase(Ease.Linear);
            PlaySortSound("sound_starmove");
            yield return new WaitForSeconds(1.3f);
            finestar.transform.position = vpos0;
            finestar.Play();
            finestar.transform.DOMove(vpos3, 1f).SetEase(Ease.Linear);
            PlaySortSound("sound_starmove");
            yield return new WaitForSeconds(1.3f);
            finestar.Stop();
            yield return new WaitForSeconds(titleClip1.length - 6f);
        }
        else
        {
            yield return new WaitForSeconds(10.8f);
            //pos
            Vector3 vpos0 = mStations[0, 0].transform.position;
            Vector3 vpos1 = mStations[0, nCol - 1].transform.position;
            Vector3 vpos2 = mStations[nRow - 1, 0].transform.position;
            Vector3 vpos3 = Vector3.zero;
            if (nLevel <= 2)
            { vpos3 = mStations[5, 5].transform.position; }
            else
            { vpos3 = mStations[3, 3].transform.position; }
            yield return new WaitForSeconds(1f);
            //sys move
            finestar.transform.position = vpos0;
            finestar.Play();
            finestar.transform.DOMove(vpos1, 1f).SetEase(Ease.Linear);
            PlaySortSound("sound_starmove");
            yield return new WaitForSeconds(1.3f);
            finestar.transform.position = vpos0;
            finestar.Play();
            finestar.transform.DOMove(vpos2, 1f).SetEase(Ease.Linear);
            PlaySortSound("sound_starmove");
            yield return new WaitForSeconds(1.3f);
            //finestar.transform.position = vpos0;
            //finestar.Play();
            //finestar.transform.DOMove(vpos3, 1f).SetEase(Ease.Linear);
            //PlaySortSound("sound_starmove");
            //yield return new WaitForSeconds(1.3f);
            finestar.Stop();
            yield return new WaitForSeconds(1f);
        }       
    }


    #endregion


    #region//截图功能
    private void ClickGetTextureBtn(GameObject _go)
    {
        if (_go.name.CompareTo(yesbtn.gameObject.name) == 0)
        {
            mGetScreenPicCtrl.OnClickShot();
        }
        else if (_go.name.CompareTo(nobtn.gameObject.name) == 0)
        {
            getpicpanel.DOScale(Vector3.one * 0.001f, 0.3f).OnComplete(()=> 
            {
                getpicpanel.gameObject.SetActive(false);
                GameOverCtl.GetInstance().Show(3, () =>
                {
                    nLevel = 1;
                    TopTitleCtl.instance.Reset();
                    PlayStartSound();
                    InitLevelData();
                });
            });          
        }
    }
    #endregion

    /// <summary>
    /// 第一行
    /// </summary>
    /// <returns></returns>
    public List<RegularOrderBlockObj> GetList1()
    {
        List<RegularOrderBlockObj> reget = new List<RegularOrderBlockObj>();
        for (int i = 0; i < nCol; i++)
        {
            reget.Add(mStations[0, i]);
        }
        return reget;
    }
    /// <summary>
    /// 第一列
    /// </summary>
    /// <returns></returns>
    public List<RegularOrderBlockObj> GetList2()
    {
        List<RegularOrderBlockObj> reget = new List<RegularOrderBlockObj>();
        for (int i = 0; i < nRow; i++)
        {
            reget.Add(mStations[i, 0]);
        }
        return reget;
    }
    /// <summary>
    /// 斜列
    /// </summary>
    /// <returns></returns>
    public List<RegularOrderBlockObj> GetList3()
    {
        List<RegularOrderBlockObj> reget = new List<RegularOrderBlockObj>();
        for (int i = 0; i < nCol; i++)
        {
            reget.Add(mStations[i, i]);
        }
        return reget;
    }


}
