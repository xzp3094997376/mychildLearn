using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

        //"Assets/ResSprite/animalshome_sprite",
        //"Assets/ResPrefab/animalshome_prefab",
        //"Assets/ResPrefab/animalhead_prefab",
        //"Assets/ResPrefab/effect_okbtn",
        //"Assets/ResSound/animalshome_sound",
        //"Assets/ResSound/bgmusic_loop0",
        //"Assets/ResSound/aa_animal_sound",
        //"Assets/ResSound/aa_animal_name",
        //"Assets/ResSound/number_sound",
        //"Assets/ResTexture/animalshome_texture",

public class AnimalsHomeCtrl : BaseScene
{

    public int nLevel = 1;
    public const int nLevels = 2;
    public bool bLvPass = false;

    public int nCount = 0;
    private const int nToCount = 3;

    /// <summary>
    /// 定位放大状态
    /// </summary>
    public bool bBigState = false;
    [HideInInspector]
    public bool bStartClick = false;

    private RawImage imgBG;
    private PlaySoundController mSoundCtrl;
    private InputNumObj mInputNumObj;
    public PlaySoundController MSoundCtrl { get { return mSoundCtrl; } }
    public InputNumObj MInputNumObj { get { return mInputNumObj; } }
    //line start of left 
    private List<Vector2> vLineList = new List<Vector2>();
    //pos offset
    private float fToRight = 172f;
    //windows all
    public List<AnimalsHomeWindow> mWindowList = new List<AnimalsHomeWindow>();
    //windows select
    public List<AnimalsHomeWindow> mSelectWindowList = new List<AnimalsHomeWindow>();
    //鸟
    private AnimalsHomeBird[] mBirds = new AnimalsHomeBird[3];

    //private AudioClip chipShowOut;
    //private AudioClip chipShowIn;

    //window位置index
    public List<int> mIndexIds = new List<int>();

    public AnimalsHomeClock mClock;
    public GameObject mTargetZero;
    public Image imgBlackPanel;

    void Awake()
    {
        mSceneType = SceneEnum.AnimalsHome;
        CallLoadFinishEvent();

        imgBG = UguiMaker.newRawImage("bg", transform, "animalshome_texture", "bg", false);
        imgBG.rectTransform.sizeDelta = new Vector2(1280f, 800f);
    }

    void Start()
    {
        mTargetZero = UguiMaker.newGameObject("zeroPos", transform);

        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        mSoundCtrl.InitAwake();
        
        StartCoroutine(ieLoadRes());
    }
    IEnumerator ieLoadRes()
    {
        yield return new WaitForSeconds(0.1f);

        InputInfoData data = new InputInfoData();
        data.nConstraintCount = 3;
        data.fNumScale = 3f;
        data.fscale = 0.3f;
        data.vBgSize = new Vector2(665, 575);
        data.vCellSize = new Vector2(200, 166);
        data.vSpacing = new Vector2(10, 10);
        data.bgcolor = new Color32(252, 229, 194, 255);
        data.color_blockBG = new Color32(179, 138, 89, 255);
        data.color_blockNum = new Color32(202, 183, 155, 255);
        data.color_blockSureBG = new Color32(172, 123, 66, 255);
        data.color_blockSureStart = new Color32(202, 183, 155, 255);
        mInputNumObj = InputNumObj.Create(transform, data);
        mInputNumObj.SetInputNumberCallBack(null);
        mInputNumObj.SetClearNumberCallBack(null);
        mInputNumObj.gameObject.SetActive(false);

        mClock = UguiMaker.newGameObject("mClock", transform).AddComponent<AnimalsHomeClock>();
        mClock.InitAwake();
        mClock.gameObject.SetActive(false);

        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);

        //create birds
        for (int i = 0; i < mBirds.Length; i++)
        {
            mBirds[i] = ResManager.GetPrefab("animalshome_prefab", "mbird").AddComponent<AnimalsHomeBird>();
            mBirds[i].InitAwake();
            mBirds[i].transform.SetParent(transform);
            mBirds[i].transform.localScale = Vector3.one * 0.8f;
            mBirds[i].gameObject.SetActive(false);
        }

        bBigState = false;

        mIndexIds = Common.GetIDList(0, 14, 15, -1);
        mIndexIds = Common.BreakRank(mIndexIds);

        nCount = 0;
        nLevel = 1;
        CreateWindows();

        //雪花
        GameObject snowObj = ResManager.GetPrefab("animalshome_prefab", "snow");
        snowObj.transform.SetParent(transform);
        snowObj.transform.localScale = Vector3.one;
        snowObj.transform.localPosition = Vector3.zero;

        GameObject mgopanel = UguiMaker.newGameObject("blackPanel", transform);
        imgBlackPanel = mgopanel.AddComponent<Image>();
        imgBlackPanel.color = new Color(0f, 0f, 0f, 0.65f);
        imgBlackPanel.rectTransform.sizeDelta = new Vector2(2000f, 2000f);
        Button btnpanel = imgBlackPanel.gameObject.AddComponent<Button>();
        btnpanel.transition = Selectable.Transition.None;
        EventTriggerListener.Get(btnpanel.gameObject).onClick = ClickBlackPanel;
        imgBlackPanel.gameObject.SetActive(false);
    }



    /// <summary>
    /// 当前的window door
    /// </summary>
    public AnimalsHomeDoor mNowSelectDoor = null;
    /// <summary>
    /// 点击黑面板重置门大小
    /// </summary>
    /// <param name="_go"></param>
    private void ClickBlackPanel(GameObject _go)
    {
        //Debug.Log(_go.name);
        if (mNowSelectDoor != null)
        {
            AudioClip cp = ResManager.GetClip("animalshome_sound", "despoint");
            if (cp != null)
            { MSoundCtrl.PlaySortSound(cp); }
            mNowSelectDoor.SetDoorResetBig();
            mNowSelectDoor = null;
        }
        imgBlackPanel.gameObject.SetActive(false);
        StopGuideHand();
    }


    /// <summary>
    /// 重置信息
    /// </summary>
    public void ResetInfos()
    {
        for (int i = 0; i < mWindowList.Count; i++)
        {
            mWindowList[i].ResetInfos();
        }
        for (int i = 0; i < mBirds.Length; i++)
        {
            mBirds[i].gameObject.SetActive(false);
            mBirds[i].transform.SetSiblingIndex(30);
        }
        mSelectWindowList.Clear();
        bLvPass = false;
        bStartClick = false;

        mClock.SetTime();
    }

    /// <summary>
    /// 初始化关卡
    /// </summary>
    public void InitLevelData()
    {
        ResetInfos();

        List<int> mAnimalIDs = Common.GetIDList(1, 14, 4, -1);
        List<int> getIndexIds = GetWindowRangList();
        
        //set select window
        for (int i = 0; i < getIndexIds.Count; i++)
        {
            int theIndex = getIndexIds[i];
            mSelectWindowList.Add(mWindowList[theIndex]);
            //第一个作为提示窗口
            if (i == 0)
            {
                mWindowList[theIndex].PreSetPoint();
                mWindowList[theIndex].DoorBtnActive(false);
            }
            else
            {
                mWindowList[theIndex].DoorBtnActive(true);
            }
            mSelectWindowList[i].CreateAnimalHead(mAnimalIDs[i]);
        }

        BirdFlyToOtherWindow();

        if (nLevel == 1)
        {
            ShowClock(false);
        }
        else
        {
            ShowClock(true);
        }
    }
    bool bplayBMG = false;


    /// <summary>
    /// 小鸟飞到window
    /// </summary>
    public void BirdFlyToOtherWindow()
    {
        StartCoroutine(ieBirdFlyToOtherWindow());
    }
    IEnumerator ieBirdFlyToOtherWindow()
    {      
        mSelectWindowList[0].OpenWindow();
        yield return new WaitForSeconds(0.2f);
        mSelectWindowList[0].ShowAnimalHead(true);

        for (int i = 0; i < mBirds.Length; i++)
        { 
            mBirds[i].transform.position = mSelectWindowList[0].BirdStartFlyPos.transform.position;
            mBirds[i].gameObject.SetActive(true);
            
            AnimalsHomeWindow win = mSelectWindowList[i + 1];
            if (win.BirdPos.transform.position.x > mSelectWindowList[0].BirdStartFlyPos.transform.position.x)
            {
                mBirds[i].transform.localScale = new Vector3(-1f, 1f, 1f) * 0.001f;
                mBirds[i].transform.DOScale(new Vector3(-1f, 1f, 1f) * 0.8f, 0.2f);
            }
            else
            {
                mBirds[i].transform.localScale = Vector3.one * 0.001f;
                mBirds[i].transform.DOScale(Vector3.one * 0.8f, 0.2f);
            }
            yield return new WaitForSeconds(0.21f);
            
            win.Bird = mBirds[i];
            float fmovetime = Vector3.Distance(mBirds[i].transform.position, win.BirdPos.transform.position) * 2f;
            mBirds[i].MoveToPosWorld(win.BirdPos.transform.position, fmovetime);
            yield return new WaitForSeconds(fmovetime);
        }

        yield return new WaitForSeconds(0.1f);
        bStartClick = true;

        //设置指引
        SetGuide();

        //关卡2计时
        if (nLevel == 2)
        {
            mClock.SetTime();
            mClock.StartTimeRun(TimeRunOverCall);
        }
        //播放玩法语音
        PlayTipSound();

        if (!bplayBMG)
        {
            mSoundCtrl.SetDelayLoadBGClip(1f);
            mSoundCtrl.PlayBGSound1("bgmusic_loop0", "bgmusic_loop0", 0.1f);
            bplayBMG = true;
        }
    }

    /// <summary>
    /// 进场/退场
    /// </summary>
    /// <param name="_in"></param>
    public void SceneMove(bool _in)
    {
        if (!_in)
        {
            for (int i = 0; i < mSelectWindowList.Count; i++)
            {
                mSelectWindowList[i].ShowAnimalHead(_in);
                mSelectWindowList[i].CloseWindow();
            }
        }
    }

    /// <summary>
    /// 下一关
    /// </summary>
    public void LevelCheckNext()
    {
        if (nCount >= nToCount)
        {
            //Debug.Log("level pass");
            bLvPass = true;
            StartCoroutine(IETOver());
        }
    }
    IEnumerator IETOver()
    {
        yield return new WaitForSeconds(5.5f);
        TopTitleCtl.instance.AddStar();
        PlayWelcomToHomeSound();
        yield return new WaitForSeconds(3f);
        nCount = 0;
        nLevel++;
        if (nLevel > nLevels)
        {
            //结算
            GameOverCtl.GetInstance().Show(2, RePlayGame);
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
        bJustPlayOnce = false;
        bHadShow = false;
        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);
        InitLevelData();
    }



    /// <summary>
    /// 移除已经完成过的
    /// </summary>
    /// <param name="_index"></param>
    public void RemoveFinishWindowIndex(int _index)
    {
        if (mIndexIds.Contains(_index))
            mIndexIds.Remove(_index);
    }

    #region//sound ctrl
    IEnumerator ieTipSound = null;
    public void SetTipSound(IEnumerator _ieTipSound)
    {
        ieTipSound = _ieTipSound;
    }
    public bool bPlayOtherTip = false;
    public void PlayTipSound()
    {
        if (bLvPass)
            return;
        if (bPlayOtherTip)
            return;
        StopTipSound();
        SetTipSound(iePlayTipSound());
        StartTipSound();
    }
    public void StopTipSound()
    {
        mSoundCtrl.mKimiAudioSource.Stop();
        if (ieTipSound != null)
            StopCoroutine(ieTipSound);
    }
    public void StartTipSound()
    {
        if (ieTipSound != null)
            StartCoroutine(ieTipSound);
    }

    //玩法提示语音
    IEnumerator iePlayTipSound()
    {
        yield return new WaitForSeconds(0.1f);
        List<AudioClip> cpList = new List<AudioClip>();
        cpList.Add(MSoundCtrl.GetClip("animalshome_sound", "有小鸟的窗户是"));
        cpList.Add(MSoundCtrl.GetClip("animalshome_sound", "第几层就在黄色"));
        cpList.Add(MSoundCtrl.GetClip("animalshome_sound", "第几号就在粉色"));
        //cpList.Add(MSoundCtrl.GetClip("animalshome_sound", "点击小圆点就会消失"));
        for (int i = 0; i < cpList.Count; i++)
        {
            MSoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
    }
    bool bJustPlayOnce = false;
    /// <summary>
    /// 播放 点击小圆点就会消失 语音
    /// </summary>
    public void PlayDesPointTipSound()
    {
        if (!bJustPlayOnce)
        {
            StopTipSound();
            AudioClip cp = MSoundCtrl.GetClip("animalshome_sound", "点击小圆点就会消失");
            MSoundCtrl.PlaySound(cp, 1f);
            bJustPlayOnce = true;
        }
    }
    /// <summary>
    /// 播放 欢迎来家做客 语音
    /// </summary>
    public void PlayWelcomToHomeSound()
    {
        StopTipSound();
        AudioClip cp = MSoundCtrl.GetClip("animalshome_sound", "欢迎到小动物的家做客");
        MSoundCtrl.PlaySound(cp, 1f);
    }

    AnimalsHomeWindow theWinToPlaySound = null;
    /// <summary>
    /// 播放动物几层几号
    /// </summary>
    /// <param name="_win"></param>
    public void PlayWindowFinishSound(AnimalsHomeWindow _win)
    {
        theWinToPlaySound = _win;
        StopTipSound();
        SetTipSound(iePlayWindowFinishSound());
        StartTipSound();
    }
    IEnumerator iePlayWindowFinishSound()
    {
        string strCp = MDefine.GetAnimalNameByID_CH(theWinToPlaySound.nAnimalID);
        yield return new WaitForSeconds(0.1f);
        List<AudioClip> cpList = new List<AudioClip>();
        cpList.Add(ResManager.GetClip("aa_animal_name", strCp));
        cpList.Add(ResManager.GetClip("animalshome_sound", "住在"));
        cpList.Add(ResManager.GetClip("number_sound", theWinToPlaySound.nPosY.ToString()));
        cpList.Add(ResManager.GetClip("animalshome_sound", "层"));
        cpList.Add(ResManager.GetClip("number_sound", theWinToPlaySound.nPosX.ToString()));
        cpList.Add(ResManager.GetClip("animalshome_sound", "号"));
        for (int i = 0; i < cpList.Count; i++)
        {
            MSoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
    }
    #endregion

    #region//windows create
    /// <summary>
    /// 创建windows
    /// </summary>
    public void CreateWindows()
    {
        StartCoroutine(ieCreateWindows());
    }
    IEnumerator ieCreateWindows()
    { 
        vLineList.Add(new Vector2(-302f, -230f));
        vLineList.Add(new Vector2(-302f, -100f));
        vLineList.Add(new Vector2(-302f, 38f));
        vLineList.Add(new Vector2(-302f, 172f));
        int index = 0;
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (y == 0 && x == 2)
                {
                    
                }
                else
                {
                    Vector2 vpos = vLineList[y] + new Vector2(fToRight * x, 0f);
                    AnimalsHomeWindow winCtrl = CreateWindow(x + 1, y + 1, index);
                    mWindowList.Add(winCtrl);
                    winCtrl.transform.localPosition = new Vector3(vpos.x, vpos.y, 0f);
                    index++;
                    AudioClip cp = ResManager.GetClip("animalshome_sound", "windowout");
                    if (cp != null)
                    { MSoundCtrl.PlaySortSound(cp, 0.1f); }
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        InitLevelData();
    }
    AnimalsHomeWindow CreateWindow(int _x, int _y, int _index)
    {
        GameObject mgo = ResManager.GetPrefab("animalshome_prefab", "window");
        mgo.transform.SetParent(transform);
        mgo.transform.localScale = Vector3.one * 0.85f;
        AnimalsHomeWindow winCtrl = mgo.AddComponent<AnimalsHomeWindow>();
        winCtrl.InitAwake(_x, _y);
        winCtrl.nIndex = _index;
        return winCtrl;
    }
    #endregion




    int indexStart = 0;
    int getCount = 4;
    /// <summary>
    /// 取得显示的window的index
    /// </summary>
    /// <returns></returns>
    public List<int> GetWindowRangList()
    {
        int theCounts = mIndexIds.Count;
        if (theCounts < getCount)
        {
            Debug.LogError("mIndexIds 不够个数");
            return null;
        }
        List<int> getList = new List<int>();
        for (int i = 0; i < getCount; i++)
        {
            int getID = 0;
            if (indexStart > (theCounts - 1))
            {
                indexStart = 0;
                getID = mIndexIds[indexStart];
                getList.Add(getID);
                indexStart++;
            }
            else
            {
                getID = mIndexIds[indexStart];
                getList.Add(getID);
                indexStart++;
            }
            //Debug.Log(getID);
        }
        return getList;
    }


    /// <summary>
    /// Clock显示/隐藏
    /// </summary>
    /// <param name="_show"></param>
    public void ShowClock(bool _show)
    {
        if (_show)
        {
            if (!mClock.gameObject.activeSelf)
            {
                mClock.gameObject.SetActive(true);
                mClock.transform.localScale = Vector3.one * 0.001f;
                mClock.transform.DOScale(Vector3.one, 1f);
            }
        }
        else
        {
            if (mClock.gameObject.activeSelf)
            {
                mClock.transform.localScale = Vector3.one * 0.001f;
                mClock.transform.DOScale(Vector3.one * 0.001f, 1f).OnComplete(()=> 
                {
                    mClock.gameObject.SetActive(false);
                });
            }
        }
    }

    /// <summary>
    /// 倒计时完成call
    /// </summary>
    public void TimeRunOverCall()
    {
        StopTipSound();
        //清0
        nCount = 0;
        InitLevelData();
    }



    #region//guideTips
    bool bHadShow = false;
    GuideHandCtl mGuideCtrl;
    /// <summary>
    /// 创建指引
    /// </summary>
    public void CreateGuideObj(Transform _tran)
    {
        if (!bHadShow)
        {
            mGuideCtrl = GuideHandCtl.Create(_tran);
            mGuideCtrl.GuideTipClick(0.8f, 0.7f, true, true, "hand1");
            mGuideCtrl.SetClickTipOffsetPos(new Vector3(8f, -25f, 0f));
            bHadShow = true;
        }
    }
    /// <summary>
    /// 删除指引
    /// </summary>
    public void StopGuideHand()
    {
        if (mGuideCtrl != null)
        {
            if (mGuideCtrl.gameObject != null)
                GameObject.Destroy(mGuideCtrl.gameObject);
            if (mnextGuideCallback != null)
                mnextGuideCallback();
            mGuideCtrl = null;
        }
    }
    System.Action mnextGuideCallback = null;
    public void SetNextGuideCallback(System.Action _callback)
    {
        mnextGuideCallback = _callback;
    }

    /// <summary>
    /// 指引设置
    /// </summary>
    public void SetGuide()
    {
        if (mSelectWindowList.Count >= 2)
        {
            AnimalsHomeWindow guideWin = mSelectWindowList[1];
            AnimalsHomeDoor door0 = guideWin.GetDoor(0);
            if (door0 != null)
            {
                CreateGuideObj(door0.transform);
            }
        }
    }
    public void SetGuideScale(float _scale)
    {
        if (mGuideCtrl != null)
            mGuideCtrl.transform.localScale = Vector3.one * _scale;
    }

    public void SetGuideParent(Transform _tran)
    {
        if (mGuideCtrl != null)
        {
            mGuideCtrl.transform.parent = _tran;
            mGuideCtrl.transform.localPosition = Vector3.zero;
        }
    }
    #endregion




    

}
