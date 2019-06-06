using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class RegularMazeCtrl : BaseScene
{

    public class RM_ObjInfo
    {
        /// <summary>
        /// 顺序ID
        /// </summary>
        public int nOrderID = 0;
        /// <summary>
        /// 物品ID
        /// </summary>
        public int nObjID = 0;
        public RM_ObjInfo(int _order,int _objid)
        {
            nOrderID = _order;
            nObjID = _objid;
        }
    }

    public int nLevel = 1;
    public const int nLevels = 3;
    public bool bLvPass = false;

    public int nToCount = 0;
    public int nCount = 0;

    private RawImage imgBG;
    private PlaySoundController mSoundCtrl;
    public PlaySoundController SoundCtrl { get { return mSoundCtrl; } }

    private Image imgRectBG;
    private Image imgheye0;
    private Image imgheye1;
    private Image imgEnd;
    private Image imgStart;
    //青蛙
    private RM_QingWa mQingWa;
    //蝌蚪
    private RM_KeDou mKeDou;

    private GameObject mBlockParent;
    private int nRaw = 9;
    private RM_Block[,] mBlocks = new RM_Block[9, 9];

    /// <summary>
    /// 规律提示
    /// </summary>
    public List<RM_Block> mTipBlocks = new List<RM_Block>();
    private GameObject mTipBlockTrans;

    public int mmapid = 1;
    //写死地图
    private RM_map mMap = new RM_map();

    List<RM_ObjInfo> objInfoList = new List<RM_ObjInfo>();
    List<int> orderList = new List<int>();
    int nindex = 0;

    [HideInInspector]
    public List<RM_Block> roadList = new List<RM_Block>();


    private Image lineTip;
    private Image pointTip;
    private RM_Drop mDrop;

    bool bInit = false;
    bool bFirstTipShake = false;

    void Awake()
    {
        mSceneType = SceneEnum.RegularMaze;
        CallLoadFinishEvent();

        imgBG = UguiMaker.newRawImage("bg", transform, "regularmaze_texture", "texturebg", false);
        imgBG.rectTransform.sizeDelta = new Vector2(1280f, 800f);     
    }

    // Use this for initialization
    void Start ()
    {
        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        mSoundCtrl.InitAwake();
        mSoundCtrl.SetDelayLoadBGClip(1f);
        StartCoroutine(ieLoadRes());
    }
    IEnumerator ieLoadRes()
    {
        yield return new WaitForSeconds(0.1f);
        mSoundCtrl.PlayBGSound1("bgmusic_loop1", "bgmusic_loop1");

        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);

        mTipBlockTrans = UguiMaker.newGameObject("mTipBlockTrans", transform);
        mTipBlockTrans.transform.localPosition = new Vector3(-500f, 0f, 0f);

        imgRectBG = UguiMaker.newImage("imgRectBG", transform, "regularmaze_sprite", "rect3", false);
        imgRectBG.transform.localScale = Vector3.one * 0.97f;
        imgRectBG.transform.localPosition = new Vector3(0f, -32f, 0f);

        mBlockParent = UguiMaker.newGameObject("mBlockParent", imgRectBG.transform);

        linesObj = UguiMaker.newGameObject("linesObj", imgRectBG.transform);
        pointsObj = UguiMaker.newGameObject("pointsObj", imgRectBG.transform);

        imgEnd = UguiMaker.newImage("imgEnd", imgRectBG.transform, "regularmaze_sprite", "rect2", false);
        imgStart = UguiMaker.newImage("imgStart", imgRectBG.transform, "regularmaze_sprite", "rect2", false);
        imgEnd.rectTransform.sizeDelta = new Vector2(160f, 164f);
        imgStart.rectTransform.sizeDelta = new Vector2(164f, 160f);
        imgEnd.transform.localPosition = new Vector3(-280f, 282f, 0f);
        imgStart.transform.localPosition = new Vector3(282f, -280f, 0f);

        imgheye0 = UguiMaker.newImage("heye0", imgRectBG.transform, "regularmaze_sprite", "heye0", false);
        imgheye1 = UguiMaker.newImage("heye1", imgRectBG.transform, "regularmaze_sprite", "heye1", false);
        imgheye0.transform.localPosition = new Vector3(342f, -318f, 0f);
        imgheye1.transform.localPosition = new Vector3(-374f, 335f, 0f);

        mKeDou = UguiMaker.newGameObject("mKeDou", imgRectBG.transform).AddComponent<RM_KeDou>();
        mKeDou.InitAwake();
        mQingWa = UguiMaker.newGameObject("mQingWa", imgRectBG.transform).AddComponent<RM_QingWa>();
        mQingWa.InitAwake();


        lineTip = UguiMaker.newImage("lineTip", imgRectBG.transform, "regularmaze_sprite", "line");
        lineTip.rectTransform.sizeDelta = Vector2.one * 18f;
        pointTip = UguiMaker.newImage("pointTip", imgRectBG.transform, "regularmaze_sprite", "droppoint");
        mDrop = UguiMaker.newGameObject("mDrop", imgRectBG.transform).AddComponent<RM_Drop>();
        mDrop.InitAwake();
        mDrop.SetHitCall(HitCall);
        bInit = true;

        List<float> findexList = Common.GetOrderList(9, 80f);
        for (int y = 0; y < nRaw; y++)
        {
            for (int x = 0; x < nRaw; x++)
            {
                mBlocks[x, y] = CreateRMBlock(x, y, mBlockParent.transform);
                mBlocks[x, y].SetPos(new Vector3(findexList[x], findexList[y], 0f));
            }
        }

        nLevel = 1;
        InitLevelData();
    }


    /// <summary>
    /// 创建Block
    /// </summary>
    RM_Block CreateRMBlock(int _x,int _y, Transform _trans)
    {
        GameObject mgo = UguiMaker.newGameObject("block" + _x + "_" + _y, _trans);
        RM_Block rmBlock = mgo.AddComponent<RM_Block>();
        rmBlock.InitAwake(_x, _y);
        return rmBlock;
    }

    /// <summary>
    /// 重置信息
    /// </summary>
    public void ResetInfos()
    {
        bLvPass = false;
        bPlayOtherTip = false;

        mTipBlockTrans.transform.localPosition = new Vector3(-860f, 0f, 0f);
        imgRectBG.transform.localPosition = new Vector3(1100f, -32f, 0f);

        for (int y = 0; y < nRaw; y++)
        {
            for (int x = 0; x < nRaw; x++)
            {
                mBlocks[x, y].ResetInfos();
            }
        }
        roadList.Clear();

        mKeDou.ResetInfos();
        mQingWa.ResetInfos();

        for (int i = 0; i < mTipBlocks.Count; i++)
        {
            if (mTipBlocks[i].gameObject != null)
            { GameObject.Destroy(mTipBlocks[i].gameObject); }
        }
        mTipBlocks.Clear();
        objInfoList.Clear();

        lineTip.gameObject.SetActive(false);
        pointTip.gameObject.SetActive(false);
        mDrop.gameObject.SetActive(false);
        DestroyLines();

        Common.DestroyChilds(mTipBlockTrans.transform);
    }

    /// <summary>
    /// 初始化关卡
    /// </summary>
    public void InitLevelData()
    {
        ResetInfos();

        string strObjName = "animal";

        if (nLevel == 1)
        {
            #region//data level1
            strObjName = "animal";
            orderList = new List<int>() { 1, 2, 3 };
            nReCount = 2;
            //get map
            int[,] getMap = null;
            int nmapID = Random.Range(1, 8);
            getMap = mMap.GetObjMapLv1(nmapID);
            Debug.Log("mapiID:" + nmapID);
            //根据orderID绑定objID
            objInfoList = InitObjInfos();

            //填充
            for (int y = 0; y < nRaw; y++)
            {
                for (int x = 0; x < nRaw; x++)
                {
                    int orderID = getMap[y, x];
                    if (orderID != 0)//固定填充
                    {
                        mBlocks[x, y].CreateObj(orderID, strObjName);
                    }
                    else//随机填充
                    {
                        int randomIndex = Random.Range(0, orderList.Count);
                        orderID = orderList[randomIndex];
                        mBlocks[x, y].CreateObj(orderID, strObjName);
                    }
                }
            }
            //get road
            List<RM_RoadData> roadDataList = mMap.GetRoad();
            for (int i = 0; i < roadDataList.Count; i++)
            {
                int nx = roadDataList[i].nX;
                int ny = roadDataList[i].nY;
                roadList.Add(mBlocks[nx, ny]);
                mBlocks[nx, ny].SetColor(Color.black);
            }
            #endregion
        }
        else
        {
            if (nLevel == 2)
            {
                strObjName = "color";
                orderList = new List<int>() { 1, 1, 2 };
                nReCount = 2;
            }
            else
            {
                strObjName = "xing";
                orderList = new List<int>() { 1, 1, 2, 2 };
                nReCount = 3;
            }

            //根据orderID绑定objID
            objInfoList = InitObjInfos();
            DebugRoadInfos(strObjName);
        }


        //生成规律提示list
        List<float> ftipY = Common.GetOrderList(orderList.Count, 150f);
        for (int i = 0; i < orderList.Count; i++)
        {
            int tipOrderId = orderList[i];
            RM_Block tipBlock = CreateRMBlock(0, 0, mTipBlockTrans.transform);
            tipBlock.ButtonActive(false);
            tipBlock.CreateObj(tipOrderId, strObjName);
            tipBlock.transform.localPosition = new Vector3(0f, ftipY[orderList.Count - 1 - i], 0f);
            tipBlock.BoxActive(false);
            mTipBlocks.Add(tipBlock);
        }
        //箭头
        for (int i = 0; i < orderList.Count - 1; i++)
        {
            Image imgArrow = UguiMaker.newImage("arrow", mTipBlockTrans.transform, "regularmaze_sprite", "arrow", false);
            imgArrow.transform.localPosition = mTipBlocks[i].transform.localPosition + new Vector3(0f, -75f, 0f);
        }

        nToCount = roadList.Count;
        nCount = 0;

        SceneMove(true);

        PlayTipSound();
    }



    /// <summary>
    /// 进场/退场
    /// </summary>
    /// <param name="_in"></param>
    public void SceneMove(bool _in)
    {
        if (_in)
        {
            if (cpResIn == null)
            {
                cpResIn = Resources.Load<AudioClip>("sound/素材出现通用音效");
            }
            mSoundCtrl.PlaySortSound(cpResIn);
            mTipBlockTrans.transform.DOLocalMoveX(-450f, 1f);
            imgRectBG.transform.DOLocalMoveX(80f, 1f);
        }
        else
        {
            if (cpResOut == null)
            {
                cpResOut = Resources.Load<AudioClip>("sound/素材出去通用");
            }
            mSoundCtrl.PlaySortSound(cpResOut);
            mTipBlockTrans.transform.DOLocalMoveX(-860f, 1f);
            imgRectBG.transform.DOLocalMoveX(1100f, 1f);
        }
    }

    /// <summary>
    /// 下一关
    /// </summary>
    public void LevelCheckNext()
    {
        nCount++;
        if ((nCount +1 >= nToCount) && nToCount > 0)
        {
            Debug.Log("level pass");
            bLvPass = true;

            lineTip.gameObject.SetActive(false);
            mDrop.gameObject.SetActive(false);

            mSoundCtrl.StopTipSound();
            StartCoroutine(ieHideRoadBlock());
        }
    }
    IEnumerator ieHideRoadBlock()
    {
        bPlayingWrongSound = false;
        lineTip.gameObject.SetActive(false);
        mDrop.gameObject.SetActive(false);
        pointTip.gameObject.SetActive(false);

        LinesHide();
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < roadList.Count; i++)
        {
            roadList[i].ShowLine();
            roadList[i].HideByShake();
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1f);
        CreateWaterRoad();

        yield return new WaitForSeconds(1.5f);
        mKeDou.MoveToQingwa(() =>
        {
            mQingWa.PlayAnimation("Click", true);
            mQingWa.PlayJumpSound(true);
            StartCoroutine(IETOver());
        });
    }
    IEnumerator IETOver()
    {
        mSoundCtrl.StopTipSound();
        bPlayOtherTip = true;

        AudioClip sucCP = ResManager.GetClip("regularmaze_sound", "哇谢谢你小蝌蚪找到妈妈啦");
        mSoundCtrl.PlaySound(sucCP, 1f);

        yield return new WaitForSeconds(sucCP.length);
        mQingWa.PlayAnimation("Idle", true);
        mQingWa.PlayJumpSound(false);
        mKeDou.PlayAnimation("Idle", true);

        TopTitleCtl.instance.AddStar();
        yield return new WaitForSeconds(0.6f);

        nLevel++;
        if (nLevel > nLevels)
        {
            //结算
            GameOverCtl.GetInstance().Show(nLevels, RePlayGame);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            SceneMove(false);
            yield return new WaitForSeconds(1.1f);
            InitLevelData();
        }
    }
    /// <summary>
    /// 重玩
    /// </summary>
    private void RePlayGame()
    {
        nLevel = 1;
        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);
        InitLevelData();
    }

    #region//sound ctrl
    public bool bPlayOtherTip = false;
    public void PlayTipSound()
    {
        if (bLvPass)
            return;
        if (bPlayOtherTip)
            return;
        bPlayingWrongSound = false;
        mSoundCtrl.PlayTipSound(iePlayTipSound());
    }
    IEnumerator iePlayTipSound()
    {
        List<AudioClip> cpList = new List<AudioClip>();
        yield return new WaitForSeconds(0.1f);
        if (nLevel == 1)
        {
            AudioClip cp0 = ResManager.GetClip("regularmaze_sound", "小蝌蚪要按照");
            mSoundCtrl.PlaySound(cp0, 1f);
            yield return new WaitForSeconds(cp0.length);
            List<AudioClip> getObjCpList = GetObjsAudioClip();
            AudioClip cp1 = getObjCpList[0];
            mSoundCtrl.PlaySound(cp1, 1f);
            if (!bFirstTipShake)
            { mTipBlocks[0].HideByShake(false); }
            yield return new WaitForSeconds(cp1.length);
            AudioClip cp2 = getObjCpList[1];
            mSoundCtrl.PlaySound(cp2, 1f);
            if (!bFirstTipShake)
            { mTipBlocks[1].HideByShake(false); }
            yield return new WaitForSeconds(cp2.length);
            AudioClip cp3 = getObjCpList[2];
            mSoundCtrl.PlaySound(cp3, 1f);
            if (!bFirstTipShake)
            { mTipBlocks[2].HideByShake(false); }
            bFirstTipShake = true;
            yield return new WaitForSeconds(cp3.length);
            AudioClip cp4 = ResManager.GetClip("regularmaze_sound", "的顺序才能找到青蛙妈妈哦");
            mSoundCtrl.PlaySound(cp4, 1f);
            yield return new WaitForSeconds(cp4.length);
        }
        else if (nLevel == 2)
        {
            cpList.Add(ResManager.GetClip("regularmaze_sound", "仔细看好颜色的规律顺序"));
            for (int i = 0; i < cpList.Count; i++)
            {
                mSoundCtrl.PlaySound(cpList[i], 1f);
                yield return new WaitForSeconds(cpList[i].length + 0.4f);
            }
        }
        else
        {
            cpList.Add(ResManager.GetClip("regularmaze_sound", "这次是图形的规律哦"));
            for (int i = 0; i < cpList.Count; i++)
            {
                mSoundCtrl.PlaySound(cpList[i], 1f);
                yield return new WaitForSeconds(cpList[i].length + 0.4f);
            }
        }        
    }
    bool bPlayingWrongSound = false;
    IEnumerator iePlayWrongSound()
    {
        List<AudioClip> cpList = new List<AudioClip>();
        bPlayingWrongSound = true;
        yield return new WaitForSeconds(0.1f);
        //随机错误提示语音
        int nrandom = UnityEngine.Random.Range(1, 4);
        if (nrandom == 1)
        {
            cpList.Add(ResManager.GetClip("regularmaze_sound", "嗯错啦"));
        }
        else if (nrandom == 2)
        {
            cpList.Add(ResManager.GetClip("regularmaze_sound", "再仔细想想"));
        }
        else
        {
            cpList.Add(ResManager.GetClip("regularmaze_sound", "仔细看好小蝌蚪是按照"));
            List<AudioClip> getObjCpList = GetObjsAudioClip();
            cpList.AddRange(getObjCpList);
            cpList.Add(ResManager.GetClip("regularmaze_sound", "的顺序找到妈妈的"));
        }
        for (int i = 0; i < cpList.Count; i++)
        {
            mSoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length + 0.4f);
        }
        bPlayingWrongSound = false;
    }

    List<AudioClip> GetObjsAudioClip()
    {
        List<AudioClip> getList = new List<AudioClip>();
        for (int i = 0; i < mTipBlocks.Count; i++)
        {
            int objid = mTipBlocks[i].nObjID;
            AudioClip cpget = GetObjAudioClip(objid);
            if (cpget != null)
                getList.Add(cpget);
            else
                Debug.Log("level" + nLevel + " obj id = " + objid + ",get audioClip null");
        }
        return getList;
    }
    AudioClip GetObjAudioClip(int _objID)
    {
        AudioClip getCP = null;
        switch (_objID)
        {
            case 1:
                if (nLevel == 1)
                { getCP = ResManager.GetClip("regularmaze_sound", "熊"); }
                else if (nLevel == 2)
                { getCP = ResManager.GetClip("regularmaze_sound", "绿色"); }
                else
                { getCP = ResManager.GetClip("regularmaze_sound", "梯形"); }
                break;
            case 2:
                if (nLevel == 1)
                { getCP = ResManager.GetClip("regularmaze_sound", "鸭子"); }
                else if (nLevel == 2)
                { getCP = ResManager.GetClip("regularmaze_sound", "红色"); }
                else
                { getCP = ResManager.GetClip("regularmaze_sound", "正方形"); }
                break;
            case 3:
                if (nLevel == 1)
                { getCP = ResManager.GetClip("regularmaze_sound", "猪"); }
                else if (nLevel == 2)
                { getCP = ResManager.GetClip("regularmaze_sound", "黄色"); }
                else
                { getCP = ResManager.GetClip("regularmaze_sound", "五边形"); }
                break;
            case 4:
                if (nLevel == 1)
                { }
                else if (nLevel == 2)
                { getCP = ResManager.GetClip("regularmaze_sound", "蓝色"); }
                else
                { getCP = ResManager.GetClip("regularmaze_sound", "六边形"); }
                break;
            case 5:
                if (nLevel == 1)
                {  }
                else if (nLevel == 2)
                {  }
                else
                { getCP = ResManager.GetClip("regularmaze_sound", "三角形"); }
                break;
            default:
                break;
        }
        return getCP;
    }

    AudioClip cpResIn;
    AudioClip cpResOut;
    #endregion



    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    InitLevelData();
        //}
        MUpdate();
    }


    



    /// <summary>
    /// 初始化物品信息
    /// </summary>
    public List<RM_ObjInfo> InitObjInfos()
    {
        List<int> mmOrderList = new List<int>();
        List<int> mmObjList = new List<int>();

        if (nLevel == 1)
        {
            mmOrderList = new List<int>() { 1, 2, 3 };
            mmObjList = Common.GetIDList(1, 3, 3, -1);
        }
        else if (nLevel == 2)
        {
            mmOrderList = new List<int>() { 1, 2, 3 ,4};
            mmObjList = Common.GetIDList(1, 4, 4, -1);
        }
        else
        {
            mmOrderList = new List<int>() { 1, 2, 3, 4 ,5};
            mmObjList = Common.GetIDList(1, 5, 5, -1);
        }

        List<RM_ObjInfo> getList = new List<RM_ObjInfo>();
        for (int i = 0; i < mmOrderList.Count; i++)
        {
            RM_ObjInfo getinfo = new RM_ObjInfo(mmOrderList[i], mmObjList[i]);
            getList.Add(getinfo);
        }
        return getList;
    }
    /// <summary>
    /// 根据orderID取得物品ID
    /// </summary>
    public int GetObjIDByOrderID(int _orderID)
    {
        for (int i = 0; i < objInfoList.Count; i++)
        {
            if (_orderID == objInfoList[i].nOrderID)
            {
                return objInfoList[i].nObjID;
            }
        }
        return 0;
    }
    #region//随机生成路(第一关写死)
    //循环数
    int nReCount = 2;
    //打印出路信息
    private void DebugRoadInfos(string _strObjName)
    {
        nindex = 0;
        //取得路线
        GetRoad();
        //线路填充
        //string strLoad = "";
        for (int i = 0; i < roadList.Count; i++)
        {
            if (nindex > nReCount)
            {
                nindex = 0;
            }
            roadList[i].CreateObj(orderList[nindex], _strObjName);
            roadList[i].SetColor(Color.black);
            nindex++;
            //strLoad += roadList[i].nPosX + "," + roadList[i].nPosY + "|";
        }
        //Debug.Log(strLoad);
        //唯一路设置
        SetOnlyOneRoad();

        //string strMapData = "";
        //填充全部
        for (int y = 0; y < nRaw; y++)
        {
            //strMapData += "{";
            for (int x = 0; x < nRaw; x++)
            {
                mBlocks[x, y].CreateOnlyObj(_strObjName);
                //strMapData += mBlocks[x, y].nOrderID + ",";
            }
            //strMapData += "},";
        }
        //Debug.Log("map = new int[9, 9]{" + strMapData + "};");
    }

    private RM_Block startBlock = null;
    private RM_Block target = null;
    //起始点列表
    List<RM_Block> setStartList = new List<RM_Block>();

    public void GetRoad()
    {
        //设置起始点列表
        setStartList.Clear();
        setStartList.Add(mBlocks[0, 6]);
        setStartList.Add(mBlocks[1, 6]);
        setStartList.Add(mBlocks[2, 7]);
        setStartList.Add(mBlocks[2, 8]);
        //随机起始点
        startBlock = setStartList[Random.Range(0, 4)];
        setStartList.Remove(startBlock);

        for (int i = 0; i < setStartList.Count; i++)
        {
            setStartList[i].SetColor(Color.gray);
            setStartList[i].nOrderID = -1;
        }

        target = startBlock;
        target.nPassID = 1;
        roadList.Clear();
        roadList.Add(target);

        while (true)
        {
            RM_Block nextBlock = GetNextBlock(target);
            if (nextBlock != null)
            {
                roadList.Add(nextBlock);
                target = nextBlock;
                if (CheckLoadOK(target))
                {
                    break;
                }
            }
            else
            {
                //Debug.Log("can not get block");
                if (roadList.Contains(target))
                {                
                    if (target != startBlock)
                    {
                        target.nPassID = 1;
                        roadList.Remove(target);
                    }
                    if (roadList.Count > 0)
                    {
                        //Debug.Log("退回上一个block");
                        target = roadList[roadList.Count - 1];
                    }
                }
            }
        }    
    }
    private RM_Block GetNextBlock(RM_Block _now)
    {
        List<RM_Block> getsList = new List<RM_Block>();
        //忽略上方向取
        List<RM_Block> aroundBlocks = GetAroundBlock(_now, false, true, true, true);
        for (int i = 0; i < aroundBlocks.Count; i++)
        {
            RM_Block getB = aroundBlocks[i];
            if (getB.nPassID == 0 && !roadList.Contains(getB))
            {
                getsList.Add(getB);
            }
        }
        if (getsList.Count > 0)
        {
            getsList = Common.BreakRank(getsList);
            RM_Block theGet = getsList[0];

            //如果theGet的周围block,在roadList里>=2个,弃掉
            int nOut = 0;
            List<RM_Block> theGetArounds = GetAroundBlock(_now, true, true, true, true);
            for (int i = 0; i < theGetArounds.Count; i++)
            {
                if (roadList.Contains(theGetArounds[i]))
                { nOut++; }
            }
            if (nOut >= 2)
            { return null; }
            else
            {
                //作为起始点的也弃掉
                if (!setStartList.Contains(theGet))
                { return theGet; }
                else
                { return null; }
            }
        }
        else
        { return null; }
    }
    //检测是到达目标
    private bool CheckLoadOK(RM_Block _check)
    {
        if (_check == mBlocks[6, 0])
        { return true; }
        if (_check == mBlocks[6, 1])
        { return true; }
        if (_check == mBlocks[7, 2])
        { return true; }
        if (_check == mBlocks[8, 2])
        { return true; }
        return false;
    }
    /// <summary>
    /// 路的周边填充(确保唯一条)
    /// </summary>
    private void SetOnlyOneRoad()
    {
        List<RM_Block> theOnlyList = new List<RM_Block>();

        for (int i = 0; i < roadList.Count -1; i++)
        {
            RM_Block _now = roadList[i];
            RM_Block nextBlock = roadList[i + 1];

            List<RM_Block> aroundBlocks = GetAroundBlock(_now, true, true, true, true);
            for (int j = 0; j < aroundBlocks.Count; j++)
            {
                RM_Block getB = aroundBlocks[j];
                if (!roadList.Contains(getB) && !setStartList.Contains(getB))
                {
                    getB.SetColor(Color.gray);
                    getB.nOrderID = -1;
                }
            }
        }
    }
    /// <summary>
    /// 取得周围的blocks
    /// </summary>
    public List<RM_Block> GetAroundBlock(RM_Block _now, bool _getUp = true, bool _getDown = true, bool _getLeft = true, bool _getRight = true)
    {
        List<RM_Block> getsList = new List<RM_Block>();
        //上
        if (_getUp)
        {
            if (_now.nPosY + 1 <= 8)
            {
                RM_Block getB = mBlocks[_now.nPosX, _now.nPosY + 1];
                getsList.Add(getB);
            }
        }

        //下
        if (_getDown)
        {
            if (_now.nPosY - 1 >= 0)
            {
                RM_Block getB = mBlocks[_now.nPosX, _now.nPosY - 1];
                getsList.Add(getB);
            }
        }

        //左
        if (_getLeft)
        {
            if (_now.nPosX - 1 >= 0)
            {
                RM_Block getB = mBlocks[_now.nPosX - 1, _now.nPosY];
                getsList.Add(getB);
            }
        }

        //右
        if (_getRight)
        {
            if (_now.nPosX + 1 <= 8)
            {
                RM_Block getB = mBlocks[_now.nPosX + 1, _now.nPosY];
                getsList.Add(getB);
            }
        }
        return getsList;
    }

    #endregion




    RM_Block mSelect;
    Vector3 vInput;
    Vector3 vStartDrop;
    void MUpdate()
    {
        if (!bInit)
            return;
        if (bLvPass)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            mSelect = null;
            #region//stp1
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hits != null)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    RM_Block com = hits[i].collider.gameObject.GetComponent<RM_Block>();
                    if (com != null)
                    {
                        if (tweenReset != null)
                        { tweenReset.Pause(); }

                        mSelect = com;                       
                        lineTip.rectTransform.sizeDelta = new Vector2(0, lineTip.rectTransform.sizeDelta.y);
                        mDrop.transform.localPosition = com.transform.localPosition;
                        pointTip.transform.localPosition = com.transform.localPosition;

                        mDrop.Box2DActive(true);

                        lineTip.gameObject.SetActive(true);
                        mDrop.gameObject.SetActive(true);
                        pointTip.gameObject.SetActive(true);

                        RectTransform rt = mSelect.transform as RectTransform;
                        vStartDrop = rt.anchoredPosition3D;
                        mSoundCtrl.PlaySortSound("regularmaze_sound", "dropline");
                        break;
                    }
                }
            }
            #endregion
        }
        else if (Input.GetMouseButton(0))
        {
            #region//stp2
            if (mSelect != null)
            {
                vInput = Common.getMouseLocalPos(imgRectBG.transform);
                mDrop.transform.localPosition = vInput;
                LineCtrol();
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//stp3
            if (mSelect != null)
            {
                mDrop.Box2DActive(false);
                tweenReset = mDrop.transform.DOLocalMove(vStartDrop, 0.3f).OnUpdate(LineCtrol).OnComplete(() =>
                {
                    lineTip.gameObject.SetActive(false);
                    mDrop.gameObject.SetActive(false);
                    pointTip.gameObject.SetActive(false);
                });
                mSoundCtrl.PlaySortSound("regularmaze_sound", "lineback", 0.7f);
                mSelect = null;
            }
            #endregion          
        }
    }
    private void LineCtrol()
    {
        //长度
        float dis = Vector3.Distance(vStartDrop, mDrop.transform.localPosition);
        lineTip.rectTransform.sizeDelta = new Vector2(dis, lineTip.rectTransform.sizeDelta.y);
        lineTip.rectTransform.anchoredPosition3D = (vStartDrop + mDrop.transform.localPosition) * 0.5f;
        //旋转方向
        Vector3 dir = (mDrop.transform.localPosition - vStartDrop).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(new Vector3(0f, 0f, targetAngle));
        lineTip.rectTransform.localRotation = q;
    }
    Tween tweenReset = null;

    /// <summary>
    /// Hit call
    /// </summary>
    /// <param name="_go"></param>
    public void HitCall(GameObject _go)
    {
        bool bok = false;
        RM_Block toBlock = _go.GetComponent<RM_Block>();
        if (toBlock != null)
        {
            RM_Block nowBlock = roadList[nCount];
            if (nowBlock == mSelect && nowBlock != toBlock)
            {
                if (roadList[nCount + 1] == toBlock)
                {
                    bok = true;
                }
            }
            if (bok)
            {
                mSelect = toBlock;
                vStartDrop = toBlock.transform.localPosition;
                CreateLine(nowBlock.transform.localPosition, toBlock.transform.localPosition);
                pointTip.transform.localPosition = mSelect.transform.localPosition;

                nowBlock.BoxActive(false);

                nowBlock.BGImageActive(true);
                toBlock.BGImageActive(true);

                toBlock.SetSucBG();
                nowBlock.SetSucBG();

                CreateLinePoint(toBlock.transform.localPosition);
                if (nCount == 0)
                {
                    CreateLinePoint(nowBlock.transform.localPosition);
                }

                LevelCheckNext();
                mSoundCtrl.PlaySortSound("regularmaze_sound", "linesuc");
            }
            else
            {
                if (nowBlock != toBlock)
                {
                    toBlock.PlayAnimation("Click");
                    toBlock.SetFaileBG();

                    if (!bPlayingWrongSound)
                        mSoundCtrl.PlayTipSound(iePlayWrongSound());
                }
            }
        }
    }


    #region //drop lines / road tips
    private GameObject linesObj;
    private GameObject pointsObj;
    List<Image> mLineList = new List<Image>();
    List<Image> mLinePointList = new List<Image>();
    public void CreateLine(Vector3 _from, Vector3 _to)
    {
        Image mmline = UguiMaker.newImage("line", linesObj.transform, "regularmaze_sprite", "line");
        float dis = Vector3.Distance(_from, _to);
        mmline.rectTransform.sizeDelta = new Vector2(dis, lineTip.rectTransform.sizeDelta.y);
        mmline.rectTransform.anchoredPosition3D = (_from + _to) * 0.5f;
        //旋转方向
        Vector3 dir = (_to - _from).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(new Vector3(0f, 0f, targetAngle));
        mmline.rectTransform.localRotation = q;
        mLineList.Add(mmline);
    }
    public void CreateLinePoint(Vector3 _pos)
    {
        Image mmPoint = UguiMaker.newImage("point", pointsObj.transform, "regularmaze_sprite", "droppoint");
        mmPoint.transform.localPosition = _pos;
        mmPoint.rectTransform.sizeDelta = Vector2.one * 35f;
        mLinePointList.Add(mmPoint);
    }
    /// <summary>
    /// 删除连线和线点
    /// </summary>
    public void DestroyLines()
    {
        for (int i = 0; i < mLineList.Count; i++)
        {
            if (mLineList[i].gameObject != null)
            {
                GameObject.Destroy(mLineList[i].gameObject);
            }
        }
        mLineList.Clear();
        for (int i = 0; i < mLinePointList.Count; i++)
        {
            if (mLinePointList[i].gameObject != null)
            {
                GameObject.Destroy(mLinePointList[i].gameObject);
            }
        }
        mLinePointList.Clear();

        Common.DestroyChilds(linesObj.transform);
        mWaterRoadBlockList.Clear();
    }
    //隐藏连好的线
    private void LinesHide()
    {
        for (int i = 0; i < mLineList.Count; i++)
        {
            Image imgdd = mLineList[i];
            DOTween.To(() => imgdd.color, x => imgdd.color = x, new Color(1f, 1f, 1f, 0f), 1f);
        }
        for (int i = 0; i < mLinePointList.Count; i++)
        {
            Image imgdd = mLinePointList[i];
            DOTween.To(() => imgdd.color, x => imgdd.color = x, new Color(1f, 1f, 1f, 0f), 1f);
        }
    }


    List<Image> mWaterRoadBlockList = new List<Image>();
    /// <summary>
    /// 创建路tips
    /// </summary>
    public void CreateWaterRoad()
    {
        //缝隙填充(起始)
        if ((roadList[0] == mBlocks[0, 6]) || (roadList[0] == mBlocks[1, 6]))
        {
            Image imgroad = WaterRaodBlock();
            imgroad.transform.localPosition = roadList[0].transform.localPosition + new Vector3(0f, 80f,0f);
            mWaterRoadBlockList.Add(imgroad);
        }
        else
        {
            Image imgroad = WaterRaodBlock();
            imgroad.transform.localPosition = roadList[0].transform.localPosition + new Vector3(-80f, 0f,0f);
            mWaterRoadBlockList.Add(imgroad);
        }
        //填充路
        for (int i = 0; i < roadList.Count; i++)
        {
            Image imgroad = WaterRaodBlock();
            imgroad.transform.localPosition = roadList[i].transform.localPosition;
            mWaterRoadBlockList.Add(imgroad);
        }
        //缝隙填充(终点)
        if ((roadList[roadList.Count - 1] == mBlocks[6, 0]) || (roadList[roadList.Count - 1] == mBlocks[6, 1]))
        {
            Image imgroad = WaterRaodBlock();
            imgroad.transform.localPosition = roadList[roadList.Count -1].transform.localPosition + new Vector3(80f, 0f,0f);
            mWaterRoadBlockList.Add(imgroad);
        }
        else
        {
            Image imgroad = WaterRaodBlock();
            imgroad.transform.localPosition = roadList[roadList.Count - 1].transform.localPosition + new Vector3(0f, -80f,0f);
            mWaterRoadBlockList.Add(imgroad);
        }

        //渐现
        for (int i = 0; i < mWaterRoadBlockList.Count; i++)
        {
            Image imgdd = mWaterRoadBlockList[i];
            DOTween.To(() => imgdd.color, x => imgdd.color = x, new Color(147f / 255, 230f / 255, 126f / 255, 1f), 1f);
        }
    }
    private Image WaterRaodBlock()
    {
        Image imgroad = UguiMaker.newGameObject("waterRoad", linesObj.transform).AddComponent<Image>();
        imgroad.raycastTarget = false;
        imgroad.rectTransform.sizeDelta = Vector3.one * 80f;
        imgroad.color = new Color(147f/255, 230f/255, 126f/255, 0f);
        return imgroad;
    }

    #endregion

}
