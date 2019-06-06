using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class WhoFlyFastCtrl : BaseScene
{

    public int nLevel = 1;
    public const int nLevels = 2;
    public bool bLvPass = false;
    public bool bPlayOtherTip = false;

    private PlaySoundController mSoundCtrl;
    public PlaySoundController SoundCtrl { get { return mSoundCtrl; } }

    private Image imgbtnClick;//停止按钮
    public Image mBtnClick { get { return imgbtnClick; } }
    private Image imgbtnCheck;//提交按钮
    public Image mBtnCheck { get { return imgbtnCheck; } }

    private GameObject mPanels;//云层
    private GameObject mRockets;

    private WFF_SceneObjMove[] panels = new WFF_SceneObjMove[5];
    public WFF_SceneObjMove[] mScenePanels { get { return panels; } }

    public WFF_RocketObj[] rocketObjs = new WFF_RocketObj[5];
    public WFF_RocketObj[] mSceneRockets { get { return rocketObjs; } }

    private WFF_InputObj mInputCtrl = null;
    public WFF_InputObj mSceneInputCtrl { get { return mInputCtrl; } }

    /// <summary>
    /// 航线
    /// </summary>
    public List<float> fLineYs = new List<float>();


    private Wff_level1Ctrl mLevel1Ctrl = null;
    private Wff_level2Ctrl mLevel2Ctrl = null;
    [HideInInspector]
    public AudioSource mAudioRocketfly;

    void Awake()
    {

        mSceneType = SceneEnum.CombineGraphics;
        CallLoadFinishEvent();

        fLineYs.Add(295f);
        fLineYs.Add(140f);
        fLineYs.Add(15f);
        fLineYs.Add(-105f);
        fLineYs.Add(-229f);
    }
    void Start()
    {
        RawImage imgbg = UguiMaker.newRawImage("bg", transform, "whoflyfast_texture", "beijing", false);
        imgbg.rectTransform.sizeDelta = new Vector2(1280f, 800f);
        StartCoroutine(ieCreateSceneRes());    
    }
    IEnumerator ieCreateSceneRes()
    {
        PreLoadSound();
        yield return new WaitForSeconds(1f);

        TopTitleCtl.instance.Reset();
        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);

        //checkBtn
        imgbtnClick = UguiMaker.newImage("btnPlay", transform, "whoflyfast_sprite", "play1");
        imgbtnClick.transform.localPosition = new Vector3(500f, -310f, 0f);
        imgbtnCheck = UguiMaker.newImage("btnCheck", transform, "whoflyfast_sprite", "yes1");
        imgbtnCheck.transform.localPosition = new Vector3(500f, -310f, 0f);
        imgbtnClick.gameObject.SetActive(false);
        imgbtnCheck.gameObject.SetActive(false);

        mLevel1Ctrl = UguiMaker.newGameObject("mlevel1", transform).AddComponent<Wff_level1Ctrl>();
        mLevel2Ctrl = UguiMaker.newGameObject("mlevel2", transform).AddComponent<Wff_level2Ctrl>();
        mLevel1Ctrl.InitAwake();
        mLevel2Ctrl.InitAwake();

        mPanels = UguiMaker.newGameObject("mPanels", transform);
        mRockets = UguiMaker.newGameObject("mRockets", transform);

        mInputCtrl = UguiMaker.newGameObject("mInputObj", transform).AddComponent<WFF_InputObj>();
        mInputCtrl.InitAwake();
        mInputCtrl.gameObject.SetActive(false);

        mAudioRocketfly = gameObject.AddComponent<AudioSource>();
        mAudioRocketfly.loop = true;
        mAudioRocketfly.volume = 0.6f;
        mAudioRocketfly.Stop();
        mAudioRocketfly.clip = ResManager.GetClip("whoflyfast_sound", "rocketfly");

        for (int i = 0; i < 5; i++)
        {
            GameObject mmpanel = transform.Find("panel" + i).gameObject;
            CreatePlane(mmpanel);
            yield return null;
            GameObject mmrocket = ResManager.GetPrefab("whoflyfast_prefab", "rocketobj");
            CreateRockets(mmrocket);
            yield return new WaitForSeconds(0.5f);
        }

        mSoundCtrl.PlayBGSound("bgmusic_loop0", "bgmusic_loop0");
        mClickGuide = GuideHandCtl.Create(transform);   

        StartGame();

    }

    private void StartGame()
    {
        nLevel = 1;
        InitLevelData();
    }

    /// <summary>
    /// 重置信息
    /// </summary>
    public void ResetInfos()
    {
        bLvPass = false;
        bPlayOtherTip = false;

        mInputCtrl.gameObject.SetActive(false);

        imgbtnClick.gameObject.SetActive(false);
        imgbtnClick.sprite = ResManager.GetSprite("whoflyfast_sprite", "play1");
        imgbtnClick.transform.SetSiblingIndex(50);
        imgbtnCheck.gameObject.SetActive(false);    
        imgbtnCheck.sprite = ResManager.GetSprite("whoflyfast_sprite", "yes1");
        imgbtnCheck.transform.SetSiblingIndex(50);
        mLevel1Ctrl.gameObject.SetActive(false);
        mLevel2Ctrl.gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化关卡
    /// </summary>
    public void InitLevelData()
    {
        ResetInfos();
        if (nLevel == 1)
        {
            for (int i = 0; i < 5; i++)
            {
                panels[i].SetState(true);
                rocketObjs[i].ResetInfo();
            }
            mLevel1Ctrl.gameObject.SetActive(true);
            mLevel1Ctrl.SetData();
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                panels[i].SetState(true);
                rocketObjs[i].InitStartData();
            }
            mLevel2Ctrl.gameObject.SetActive(true);
            mLevel2Ctrl.SetData();
        }
    }




    /// <summary>
    /// 下一关
    /// </summary>
    public void LevelCheckNext()
    {
        //for (int i = 0; i < 5; i++)
        //{
        //    mScenePanels[i].bMove = false;
        //}

        Debug.Log("level pass");
        bLvPass = true;
        StartCoroutine(IETOver());
    }
    IEnumerator IETOver()
    {
        mSoundCtrl.StopTipSound();
        bPlayOtherTip = true;

        yield return new WaitForSeconds(1f);
        TopTitleCtl.instance.AddStar();
        yield return new WaitForSeconds(0.5f);
        nLevel++;
        if (nLevel > nLevels)
        {
            //结算
            GameOverCtl.GetInstance().Show(nLevels, RePlayGame);
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
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
        mAudioRocketfly.Stop();
        InitLevelData();
    }

    /// <summary>
    /// 播放提示语音
    /// </summary>
    public void PlayTipSound()
    {
        if (bLvPass)
            return;
        if (bPlayOtherTip)
            return;
        if (nLevel == 1)
        {
            SoundCtrl.PlayTipSound(mLevel1Ctrl.iePlayTipSoundLv1());
        }
        else
        {
            mLevel2Ctrl.PlayTipSound();
        }
    }


    /// <summary>
    /// 检测是否全部填了数字
    /// </summary>
    public bool CheckIsAllSetNumber()
    {
        for (int i = 0; i < rocketObjs.Length; i++)
        {
            if (rocketObjs[i].nNum <= 0)
            { return false; }
        }
        return true;
    }
    /// <summary>
    /// 检测排序是否匹配
    /// </summary>
    public bool CheckIsOrderOK()
    {
        SetOrder();

        int countOK = 0;
        List<int> orderList = new List<int>() { 5, 4, 3, 2, 1 };
        for (int i = 0; i < orderList.Count; i++)
        {
            if (rocketObjs[i].nNum == orderList[i])
            {
                countOK++;
            }
            else
            {
                rocketObjs[i].SetWrong();
            }
        }
        if (countOK >= 5)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 判断rocket是第几名
    /// </summary>
    public int GetNowOrder(WFF_RocketObj _tarRocket)
    {
        int nOrder = 1;
        for (int i = 0; i < rocketObjs.Length; i++)
        {
            if (_tarRocket != rocketObjs[i])
            {
                float fG = rocketObjs[i].transform.localPosition.x;
                if (fG < _tarRocket.transform.localPosition.x)
                {
                    nOrder++;
                }
            }
        }
        return nOrder;
    }

    public void SetOrder()
    {
        for (int i = 0; i < rocketObjs.Length; i++)
        {
            rocketObjs[i].StopPingpong();
        }
        for (int i = 0; i < rocketObjs.Length; i++)
        {
            for (int j = 0; j < rocketObjs.Length - 1; j++)
            {
                if (rocketObjs[i].transform.localPosition.x > rocketObjs[j].transform.localPosition.x)
                {
                    WFF_RocketObj third = rocketObjs[i];
                    rocketObjs[i] = rocketObjs[j];
                    rocketObjs[j] = third;
                }
            }
        }
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetDirffientFast();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitLevelData();
        }
    }


    #region//create res obj
    public void InstantiateObj(string _name, System.Action<GameObject> _call)
    {
        StartCoroutine(ieInstantiateObj(_name, _call));
    }
    IEnumerator ieInstantiateObj(string _name, System.Action<GameObject> _call)
    {
        yield return null;
        GameObject mgo = ResManager.GetPrefab("whoflyfast_prefab", _name);
        if (mgo != null)
        {
            if (_call != null)
            {
                _call(mgo);
            }
        }
    }
    //场景创建
    public void CreatePlane(GameObject _panelObj)
    {
        _panelObj.transform.SetParent(mPanels.transform);
        _panelObj.transform.localScale = Vector3.one;
        _panelObj.transform.localPosition = Vector3.zero;

        if (!_panelObj.name.Contains("0"))
        {
            _panelObj.transform.localPosition = Vector3.zero - new Vector3(1200f, 0f, 0f);
            _panelObj.transform.DOLocalMove(Vector3.zero, 0.4f);
            mSoundCtrl.PlaySortSound(ResManager.GetClip("whoflyfast_sound", "resourcein"));
        }

        WFF_SceneObjMove panel = _panelObj.AddComponent<WFF_SceneObjMove>();
        if (_panelObj.name.Contains("0"))
        {
            panels[0] = panel;
            panels[0].InitAwake(0);
            panels[0].SetSpeed(20f);
        }
        if (_panelObj.name.Contains("1"))
        {
            panels[1] = panel;
            panels[1].InitAwake(1);
            panels[1].SetSpeed(40f);
        }
        if (_panelObj.name.Contains("2"))
        {
            panels[2] = panel;
            panels[2].InitAwake(2);
            panels[2].SetSpeed(60f);
        }
        if (_panelObj.name.Contains("3"))
        {
            panels[3] = panel;
            panels[3].InitAwake(3);
            panels[3].SetSpeed(200f);
        }
        if (_panelObj.name.Contains("4"))
        {
            panels[4] = panel;
            panels[4].InitAwake(4);
            panels[4].SetSpeed(200f);
        }
        panel.SetState(true);
    }
    //private int nResPanelCount = 0;
    //火箭创建
    public void CreateRockets(GameObject _Obj)
    {
        _Obj.transform.SetParent(mRockets.transform);
        _Obj.transform.localScale = Vector3.one;
        _Obj.transform.localPosition = Vector3.zero + new Vector3(1000f, 0f, 0f);
        WFF_RocketObj rocket = _Obj.AddComponent<WFF_RocketObj>();
        for (int i = 0; i < rocketObjs.Length; i++)
        {
            if (rocketObjs[i] == null)
            {
                rocketObjs[i] = rocket;
                rocketObjs[i].InitAwake(i);
                rocketObjs[i].gameObject.name = "rocket" + i;
                break;
            }
        }
        //nResPanelCount++;
        //if (nResPanelCount >= 5)
        //{
        //    StartGame();
        //}
        rocket.SetState(true);
    }
    #endregion

    #region//Guide Tip
    GuideHandCtl mClickGuide;
    /// <summary>
    /// 显示guide click
    /// </summary>
    public void GuideClick(Vector3 _worldPos,Vector3 _localOffset)
    {
        if (mClickGuide != null)
        {
            mClickGuide.transform.SetSiblingIndex(60);
            mClickGuide.GuideTipClick(0.8f, 0.7f, true, true, "hand1");
            mClickGuide.SetClickTipPos(_worldPos);
            mClickGuide.SetClickTipOffsetPos(_localOffset);
        }
    }
    /// <summary>
    /// 停止guide click
    /// </summary>
    public void GuideStop()
    {
        if (mClickGuide != null)
        {
            mClickGuide.StopClick();
        }
    }
    public void DesGuide()
    {
        if (mClickGuide != null)
        {
            if (mClickGuide.gameObject != null)
                GameObject.Destroy(mClickGuide.gameObject);
            mClickGuide = null;
        }
    }
    #endregion

    private void PreLoadSound()
    {
        AbManager.GetAB(AbEnum.sound, "whoflyfast_sound");
    }

    /// <summary>
    /// 至少错开local 50单位
    /// </summary>
    public void SetDirffientFast()
    {
        SetOrder();

        List<WFF_RocketObj> rockList = new List<WFF_RocketObj>();
        for (int i = 4; i >= 0; i--)
        {
            rockList.Add(rocketObjs[i]);
            rocketObjs[i].fSetX = rocketObjs[i].transform.localPosition.x;
        }

        for (int i = 0; i < rockList.Count - 1; i++)
        {
            WFF_RocketObj rock1 = rockList[i];
            WFF_RocketObj rock2 = rockList[i + 1];
            if (rock2.fSetX - rock1.fSetX < 50f)
            {
                rock2.fSetX = rock1.fSetX + 50f;
                rock2.DoLocalMoveX(rock2.fSetX);
            }
        }

    }


}
