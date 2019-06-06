using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AnimalStatisticsCtrl : BaseScene
{

    public int nLevel = 0;
    public int nLevels = 4;

    public int nCount = 0;
    /// <summary>
    /// 倍数
    /// </summary>
    public int nBeiShu = 1;

    private PlaySoundController mSoundCtrl;
    public PlaySoundController SoundCtrl { get { return mSoundCtrl; } }

    private Transform mCenter;
    private RawImage rawImage;
    public AnimalStatistics_Panel mPanel;
    public AnimalStatistics_TopBalls mTopballs;
    public AnimalStatistics_UFO mUFO;
    public AnimalStatistics_panel4 mPanel4;

    [HideInInspector]
    public bool bContrlFinish = true;
    [HideInInspector]
    public bool bGameReady = false;
    [HideInInspector]
    public bool bLevelPass = false;

    /// <summary>
    /// 方格是否要添加小花
    /// </summary>
    public bool bLittleFlower = false;

    private ParticleSystem flower_ok;

    float fScale;
    float fOffset;
    float ffSet;

    /// <summary>
    /// get res obj
    /// </summary>
    public GameObject CreateResObj(string _name, Transform _parent)
    {
        GameObject mgo = ResManager.GetPrefab("animalstatistics_prefab", _name);
        mgo.transform.SetParent(_parent);
        mgo.transform.localScale = Vector3.one;
        mgo.transform.localPosition = Vector3.zero;
        return mgo;
    }

    void Awake()
    {
        mSceneType = SceneEnum.AnimalStatistics;
        CallLoadFinishEvent();
    }
    void Start()
    {
        rawImage = transform.Find("RawImage").GetComponent<RawImage>();
        rawImage.texture = ResManager.GetTexture("animalstatistics_texture", "animalstatisticsbg");
        rawImage.rectTransform.sizeDelta = new Vector2(1280f, 800f);

        fScale = GlobalParam.screen_width / 1423f;
        fOffset = 400f * (1 - fScale);
        ffSet = fOffset * (1 - fScale);
        mCenter = transform.Find("mCenter");
        mCenter.transform.localScale = Vector3.one * fScale;

        mPanel = mCenter.Find("mPanel").gameObject.AddComponent<AnimalStatistics_Panel>();
        mTopballs = mCenter.Find("mTopballs").gameObject.AddComponent<AnimalStatistics_TopBalls>();
        mUFO = mCenter.Find("mUFO").gameObject.AddComponent<AnimalStatistics_UFO>();
        mPanel4 = mCenter.Find("mPanel4").gameObject.AddComponent<AnimalStatistics_panel4>();

        mPanel.gameObject.SetActive(false);
        mPanel4.gameObject.SetActive(false);
        mUFO.gameObject.SetActive(false);
        mTopballs.gameObject.SetActive(false);

        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        mSoundCtrl.PlayBGSound("bgmusic_loop0", "bgmusic_loop0", 0.1f);

        StartCoroutine(IEWaite());
    }  
    IEnumerator IEWaite()
    {
        yield return new WaitForSeconds(0.1f);
        TopTitleCtl.instance.Reset();      
        TopTitleCtl.instance.mSoundTipData.SetData(PlayLvStartSound);
        //mPanel 
        Vector3 vposPanel = new Vector3(0f, -100f, 0f);
        mPanel.transform.localPosition = vposPanel;
        mPanel.InitAwake();
        //mTopballs
        Vector3 vposTopBall = Vector3.zero;
        mTopballs.transform.localPosition = vposTopBall + new Vector3(0f, fOffset + ffSet, 0f);
        mTopballs.InitAwake();
        //flower_ok
        flower_ok = transform.Find("flower_ok").GetComponent<ParticleSystem>();
        flower_ok.transform.localPosition = new Vector3(0f, -300f, 0f);
        flower_ok.Pause();
        flower_ok.Stop();

        nLevel = 1;
        InitLevelData();
        PlayTitleSound();

        yield return new WaitForSeconds(3f);
        //mUFO
        Vector3 vposUFO = new Vector3(0f, 210f, 0f);
        mUFO.transform.localPosition = vposUFO + new Vector3(0f, fOffset + ffSet, 0f);
        mUFO.InitAwake();
        mUFO.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        //mPanel4
        mPanel4.InitAwake();
        mPanel4.gameObject.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitLevelData();
        }
    }

    public List<int> numsss = new List<int>();
    public void InitLevelData()
    {
        ResetInfos();

        if (nLevel == 1)
            nBeiShu = 1;
        else if (nLevel == 2)
            nBeiShu = 2;
        else if (nLevel == 3)
            nBeiShu = 3;
        else
            nBeiShu = 3;

        mPanel.SetTipText(nBeiShu);
        bLittleFlower = false;
        
        mUFO.gameObject.SetActive(false);
        mTopballs.gameObject.SetActive(false);

        List<int> animalType = Common.GetIDList(1, 14, 5, -1);
        List<int> hafValue = Common.GetIDList(1, 10, 5, -1);
        List<int> getNumList = new List<int>();

        if (nLevel <= 2)
        {
            for (int i = 0; i < hafValue.Count; i++)
            {
                getNumList.Add(hafValue[i] * nBeiShu);
            }
            mPanel.SetDataLv1To2(animalType, getNumList);
            mTopballs.gameObject.SetActive(true);
            mTopballs.SetData(getNumList);
            if (nLevel == 1)
            { mPanel.SetLineText(); }
        }
        else if (nLevel == 3)
        {
            getNumList = new List<int> { 3, 6, 9, 12, 15, 18 };
            getNumList = Common.BreakRank(getNumList);
            
            mPanel.SetDataLv3(animalType, getNumList);
            mUFO.SetBlockSprite("block_yellow");
            mUFO.gameObject.SetActive(true);
            mPanel.SetLineText();
        }
        else
        {
            getNumList = new List<int> { 3, 6, 9, 12, 15, 18 };
            getNumList = Common.BreakRank(getNumList);
            mPanel4.SetData(animalType, getNumList);
        }

        SceneMove(true);
        StartCoroutine(IEPreReady());

        if (nLevel != 1)
            PlayLvStartSound();
    }
    IEnumerator IEPreReady()
    {
        yield return new WaitForSeconds(1.2f);
        if (nLevel != 4)
            mPanel.DataObjMove(true);
        yield return new WaitForSeconds(2.65f);
        bGameReady = true;
    }

    public void ResetInfos()
    {
        nCount = 0;
        bGameReady = false;
        bLevelPass = false;
        mPanel.gameObject.SetActive(false);
        mPanel4.gameObject.SetActive(false);
        mUFO.SetBlockState(false);
        if (nLevel <= 3)
        {
            mPanel.gameObject.SetActive(true);
            if (nLevel == 1 || nLevel == 3)
            {
                mPanel.TextActive(true);
            }
            else
                mPanel.TextActive(false);
        }
        else
        {
            mUFO.gameObject.SetActive(false);         
            mPanel4.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 关卡pass检测
    /// </summary>
    public void MLevelPass()
    {
        nCount++;
        if (nCount >= 5)
        {
            if (nLevel <= 1 && mPanel.nNextQuaID < 2)//提问 那个最多/最少
            {
                mPanel.ToNextQua();
            }
            else if (nLevel == 2 && mPanel.nQuea2Count < 2)//提问xx比xx多几只,xx比xx少几只
            {
                mPanel.SetQuea2();
            }
            else if (nLevel == 4 && mPanel4.nQuea2Count < 1)//提问 哪些是奇数 / 偶数
            {
                mPanel4.SetQuea2();
            }
            else
            {
                Debug.Log("ppppppppppppppppppppp");
                bLevelPass = true;
                //Debug.LogError("level pass");
                TopTitleCtl.instance.AddStar();
                StartCoroutine(TOver());
            }
        }
    }
    IEnumerator TOver()
    {
        mSoundCtrl.StopTipSound();
        yield return new WaitForSeconds(0.5f);

        //关卡结束播放鼓励语音
        AudioClip cpgoodgood = GetClip("children_you_goodgood" + Random.Range(0, 5));
        mSoundCtrl.PlaySound(cpgoodgood,1f);
        yield return new WaitForSeconds(cpgoodgood.length + 0.2f);

        nLevel++;
        if (nLevel > nLevels)
        {
            //结算
            //Debug.LogError("Game Run Over!");
            yield return new WaitForSeconds(2f);
            GameOverCtl.GetInstance().Show(4, ReplayGame);
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            SceneMove(false);
            yield return new WaitForSeconds(1.3f);
            InitLevelData();
        }
    }

    /// <summary>
    /// 重玩
    /// </summary>
    public void ReplayGame()
    {
        nLevel = 1;
        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayLvStartSound);
        PlayTitleSound();
        InitLevelData();
    }

    public void SceneMove(bool _in)
    {
        if (mPanel.gameObject.activeSelf)
            mPanel.PanelMove(_in);
        if (mTopballs.gameObject.activeSelf)
            mTopballs.SceneMove(_in);
        if (mUFO.gameObject.activeSelf)
            mUFO.SceneMove(_in);
        if (mPanel4.gameObject.activeSelf)
            mPanel4.PanelMove(_in);
    }


    #region//sound
    public AudioClip GetClip(string _clipName)
    {
        AudioClip clip1 = ResManager.GetClip("animalstatistics_sound", _clipName);
        return clip1;
    }
    /// <summary>
    /// 直接播放音效
    /// </summary>
    public void PlaySortSound(string _clipname)
    {
        AudioClip clip1 = GetClip(_clipname);
        mSoundCtrl.PlaySortSound(clip1);
    }
    /// <summary>
    /// 刚开始播放语音(包含1关)
    /// </summary>
    public void PlayTitleSound()
    {
        PlayLvStartSound();
    }
    /// <summary>
    /// 每关播放语音
    /// </summary>
    public void PlayLvStartSound()
    {
        if (bLevelPass)
            return;
        if (nLevel == 1)
        { mSoundCtrl.PlayTipSound(mPanel.ieTipSoundLv1()); }
        else if (nLevel == 2)
        { mSoundCtrl.PlayTipSound(mPanel.ieTipSoundLv2()); }
        else if (nLevel == 3)
        {
            AudioClip cp =  GetClip("z_additional_gezibiaoshi" + nBeiShu);
            mSoundCtrl.PlaySound(cp, 1f);
        }
        else
        { mSoundCtrl.PlayTipSound(mPanel4.ieTipSoundLv4()); }
    }
    #endregion


    #region//guide hand
    /// <summary>
    /// 第一关指引显示
    /// </summary>
    public void GuideHandShowLv1()
    {
        AnimalStatistics_BallCtrl gball = mTopballs.mBallCtrlList[0];
        AnimalStatistics_DataObj gdata = mPanel.GetDataObjByNum(gball.nID);
        if (gdata != null)
        {
            mTopballs.ShowDrapTip(gball.BallImage.transform.position, gdata.tipNum.transform.position);
        }
    }
    public void GuideHandStopLv1()
    {
        mTopballs.StopDrapTip();
    }
    /// <summary>
    /// 第三关指引显示
    /// </summary>
    public void GuideHandShowLv3(Vector3 _worldPos)
    {
        mUFO.ShowClickTip(_worldPos);
    }
    public void GuideHandStopLv3()
    { mUFO.StopClickTip(); }
    [HideInInspector]
    public bool bguideLv3Finish = false;
    #endregion

    /// <summary>
    /// 播放flower_ok特效
    /// </summary>
    public void PlayFlowerOK_fx(Vector3 _vWorldPos)
    {
        flower_ok.transform.position = _vWorldPos;
        flower_ok.Play();
    }

}
