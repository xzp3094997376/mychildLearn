using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


//"Assets/ResSprite/catchfish_sprite",
//"Assets/ResPrefab/catchfish_prefab",
//"Assets/ResSound/catchfish_sound",
//"Assets/ResTexture/catchfish_texture",
//"Assets/ResSound/bgmusic_loop0",
//"Assets/ResSound/number_sound",
//"Assets/ResSound/checkgamebtn_sound",
//"Assets/ResPrefab/effect_okbtn",


/// <summary>
/// 分(抓)小鱼
/// </summary>
public class CatchFish : BaseScene
{

    public int nLevel = 1;
    public const int nLevels = 3;
    public bool bLvPass = false;

    private RawImage imgBG;
    private RawImage imgWater;

    private WaterMove mWaterMove;

    [HideInInspector]
    public PlaySoundController mSoundCtrl;

    [HideInInspector]
    public CFishLevel1 mLevel1 = null;
    [HideInInspector]
    public CFishLevel2 mLevel2 = null;
    [HideInInspector]
    public CFishLevel3 mLevel3 = null;

    /// <summary>
    /// 数字
    /// </summary>
    public int nNum = 6;

    //渔网
    [HideInInspector]
    public GameObject mFishNet;
    //down water
    [HideInInspector]
    public GameObject mFishDownParent;
    //top pics
    [HideInInspector]
    public CFishLinefishsObj mTopLinePic;
    //check btn
    [HideInInspector]
    public Button mCheckBtn;
    private Image imgCheckBtn;

    private ParticleSystem mCheckBtnFX;


    void Awake()
    {
        mSceneType = SceneEnum.CatchFish;
        CallLoadFinishEvent();

        imgBG = UguiMaker.newRawImage("bg", transform, "catchfish_texture", "catchfishbg", false);
        imgBG.rectTransform.sizeDelta = new Vector2(1423f, 800f);
        imgWater = UguiMaker.newRawImage("water", transform, "catchfish_texture", "waterdown", false);
        imgWater.rectTransform.sizeDelta = new Vector2(1418, 195f);
        imgWater.rectTransform.anchoredPosition = new Vector2(0f, -400f + 195 * 0.5f);
        //down water
        mFishDownParent = UguiMaker.newGameObject("downfishTrans", transform);
        mFishDownParent.transform.localPosition = new Vector3(0f, -400f + 195 * 0.5f, 0f);
        //check btn
        mCheckBtn = UguiMaker.newButton("mCheckBtn", transform, "catchfish_sprite", "btnup");
        mCheckBtn.transition = Selectable.Transition.None;
        EventTriggerListener.Get(mCheckBtn.gameObject).onDown = CheckBtnDown;
        mCheckBtn.gameObject.SetActive(false);
        imgCheckBtn = mCheckBtn.GetComponent<Image>();

        mLevel1 = UguiMaker.newGameObject("level1", transform).AddComponent<CFishLevel1>();
        mLevel2 = UguiMaker.newGameObject("level2", transform).AddComponent<CFishLevel2>();
        mLevel3 = UguiMaker.newGameObject("level3", transform).AddComponent<CFishLevel3>();
        mLevel1.InitAwake();
        mLevel2.InitAwake();
        mLevel3.InitAwake();


        mCheckBtn.transform.SetSiblingIndex(30);
    }

    // Use this for initialization
    void Start ()
    {
        mFishNet = CreateFishNet();

        mCheckBtnFX = ResManager.GetPrefab("effect_okbtn", "okbtn_effect").GetComponent<ParticleSystem>();
        mCheckBtnFX.transform.SetParent(mCheckBtn.transform);
        mCheckBtnFX.transform.localScale = Vector3.one;
        mCheckBtnFX.transform.localPosition = Vector3.zero;
        mCheckBtnFX.Pause();
        mCheckBtnFX.Stop();

        mWaterMove = UguiMaker.newGameObject("watermove", transform).AddComponent<WaterMove>();
        mWaterMove.strAB = "catchfish_sprite";
        mWaterMove.strResSprite = "water1";
        mWaterMove.transform.localPosition = imgWater.transform.localPosition + new Vector3(0f, -12f, 0f);
        mWaterMove.InitAwake(1423f, 170f,0.63f);
        mWaterMove.transform.SetSiblingIndex(4);

        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        mSoundCtrl.PlayBGSound("bgmusic_loop0", "bgmusic_loop0");

        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);

        int getNum = Random.Range(6, 8);
        nNum = getNum;

        nLevel = 1;
        InitLevelData();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitLevelData();
        }
    }


    public void ResetInfos()
    {
        bLvPass = false;
        CheckBtnActive(true);
        mCheckBtn.gameObject.SetActive(false);
        mLevel1.gameObject.SetActive(false);
        mLevel2.gameObject.SetActive(false);
        mLevel3.gameObject.SetActive(false);
    }

    /// <summary>
    /// 初始化关卡
    /// </summary>
    public void InitLevelData()
    {
        ResetInfos();

        if (nLevel == 1)
        {
            if (mTopLinePic != null)
            {
                if (mTopLinePic.gameObject != null)
                { GameObject.Destroy(mTopLinePic.gameObject); }
            }
            //create top line pics
            mTopLinePic = CreateLineFish(nNum, transform);
            mTopLinePic.transform.localPosition = new Vector3(0f, 320f, 0f);

            mLevel1.gameObject.SetActive(true);
            mLevel1.SetData(nNum);
        }
        else if (nLevel == 2)
        {
            mLevel2.gameObject.SetActive(true);
            mLevel2.SetData(nNum);
        }
        else
        {
            mLevel3.gameObject.SetActive(true);
            mLevel3.SetData(nNum);
        }

        SceneMove(true);

        PlayTipSound();
    }

    /// <summary>
    /// 进场/退场
    /// </summary>
    /// <param name="_in"></param>
    public void SceneMove(bool _in)
    {
        if (mLevel1.gameObject.activeSelf)
        {
            if (_in)
            {
                mTopLinePic.transform.localPosition = new Vector3(0f, 600f, 0f);
                mTopLinePic.transform.DOLocalMove(new Vector3(0f, 320f, 0f), 1f);
            }
            mLevel1.SceneMove(_in);
        }
        else if (mLevel2.gameObject.activeSelf)
        {          
            mLevel2.SceneMove(_in);
        }
        else
        {

        }

        if (!_in)
        {
            CheckBtnShow(false);
        }
    }

    /// <summary>
    /// 下一关
    /// </summary>
    public void LevelCheckNext()
    {
        CheckBtnActive(false);
        bLvPass = true;
        StartCoroutine(IETOver());
    }
    IEnumerator IETOver()
    {
        TopTitleCtl.instance.AddStar();
        yield return new WaitForSeconds(1f);

        //赞美语音
        bPlayOtherTip = true;
        StopTipSound();
        string strCP = "game-tips_suc2";
        if (Random.value > 0.5)
        { strCP = "game-tips_suc3"; }
        AudioClip cp = ResManager.GetClip("catchfish_sound", strCP);
        mSoundCtrl.PlaySound(cp, 1f);
        yield return new WaitForSeconds(cp.length);
        bPlayOtherTip = false;

        nLevel++;
        if (nLevel > nLevels)
        {
            //结算
            GameOverCtl.GetInstance().Show(3, RePlayGame);
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
        mLevel1.ResetInfos();
        mLevel2.ResetInfo();
        mLevel3.ResetInfos();
        InitLevelData();
    }



    /// <summary>
    /// create top line pics
    /// </summary>
    public CFishLinefishsObj CreateLineFish(int _id,Transform _trans)
    {
        GameObject mgo = ResManager.GetPrefab("catchfish_prefab", "linepic" + _id);
        mgo.transform.SetParent(_trans);
        mgo.transform.localPosition = new Vector3(0f, 340f, 0f);
        mgo.transform.localScale = Vector3.one;
        CFishLinefishsObj linectrl = mgo.AddComponent<CFishLinefishsObj>();
        linectrl.InitAwake(_id);
        return linectrl;
    }
    /// <summary>
    /// create fish obj
    /// </summary>
    public CFishFishObj CreateFishObj(int _type,Transform _trans,Vector3 _vStart)
    {
        GameObject mgo = UguiMaker.newGameObject("fishobj", _trans);
        mgo.transform.SetParent(_trans);
        mgo.transform.localPosition = _vStart;
        mgo.transform.localScale = Vector3.one;
        CFishFishObj fishctrl = mgo.AddComponent<CFishFishObj>();
        fishctrl.InitAwake(_type);
        return fishctrl;
    }
    /// <summary>
    /// 创建渔网
    /// </summary>
    /// <returns></returns>
    public GameObject CreateFishNet()
    {
        GameObject mgo = UguiMaker.newGameObject("mFishNet", transform);
        Image img = UguiMaker.newImage("img", mgo.transform, "catchfish_sprite", "fishnet", false);
        img.rectTransform.anchoredPosition = new Vector2(30f, 10f);
        mgo.transform.localPosition = new Vector3(850f, 0f, 0f);
        return mgo;
    }

    /// <summary>
    /// 是否在捕鱼状态
    /// </summary>
    public bool bCatchState = false;
    /// <summary>
    /// 捕鱼动作是否完成
    /// </summary>
    public bool bCatchFishOK = false;

    /// <summary>
    /// 用渔网捕鱼
    /// </summary>
    /// <param name="_stationT"></param>
    /// <param name="_fish"></param>
    public void CatchFishByNet(Transform _stationT ,CFishFishObj _fish)
    {
        bCatchState = true;
        bCatchFishOK = false;

        mFishNet.transform.SetParent(transform);
        mFishNet.transform.SetSiblingIndex(50);
        mFishNet.transform.localPosition = new Vector3(0f, 650f, 0f);

        mFishNet.transform.position = new Vector3(_fish.transform.position.x, mFishNet.transform.position.y, mFishNet.transform.position.z);
        mFishNet.transform.localEulerAngles = new Vector3(0f, 0f, 160f);

        AudioClip cp = ResManager.GetClip("catchfish_sound", "to_getfish");
        mSoundCtrl.PlaySortSound(cp);

        if (mCheckBtn.gameObject.activeSelf)
            CheckBtnShow(false);

        mFishNet.transform.DOMove(_fish.transform.position, 0.5f).OnComplete(()=> 
        {
            _fish.transform.SetParent(mFishNet.transform);
            _fish.transform.SetSiblingIndex(0);
            _fish.PlayAnimation("Struggle");

            float foldX = _fish.transform.localScale.x;
            if (foldX >= 0)
            { _fish.transform.localScale = Vector3.one; }
            else
            { _fish.transform.localScale = new Vector3(-1f, 1f, 1f); }

            mFishNet.transform.DORotate(Vector3.zero, 0.25f).OnComplete(()=> 
            {
                float fYY = mFishNet.transform.localPosition.y;
                mFishNet.transform.DOLocalMoveY(fYY + 60f, 0.2f).OnComplete(()=> 
                {
                    bCatchFishOK = true;
                });
            });
        });
    }

    /// <summary>
    /// 添加到鱼缸
    /// </summary>
    /// <param name="_fish"></param>
    /// <param name="_box"></param>
    public void AddToFishBox(CFishFishObj _fish, CFlishFishBoxObj _box)
    {
        AudioClip cp = ResManager.GetClip("catchfish_sound", "to_fishtobox");
        mSoundCtrl.PlaySortSound(cp);

        mLevel1.StopFishBoxClick();

        Vector3 vto = _box.transform.localPosition + new Vector3(0f, 250f, 0f);
        mFishNet.transform.DOLocalMove(vto, 0.3f).OnComplete(()=> 
        {
            mFishNet.transform.DOLocalRotate(new Vector3(0f, 0f, 160f), 0.25f).OnComplete(() => 
            {
                _box.AddFish(_fish, AddFishToFishBoxCallBack);
            });          
        });
    }
    /// <summary>
    /// 添加完成回调
    /// </summary>
    public void AddFishToFishBoxCallBack()
    {
        //收起渔网
        mFishNet.transform.DOLocalMoveY(650f, 0.25f).OnComplete(()=> 
        {
            bCatchState = false;
            bCatchFishOK = false;
            if (nLevel == 1)
            {
                mLevel1.CheckShowBtn();
            }
        });
    }

    /// <summary>
    /// 取得fish的station
    /// </summary>
    /// <param name="_fish"></param>
    /// <returns></returns>
    public Transform GetFishStation(CFishFishObj _fish)
    {
        if (_fish.nInStation == 0)
        { return mFishDownParent.transform; }
        else if (_fish.nInStation == 1)
        { return mLevel1.mLeftBox.transform; }
        else if (_fish.nInStation == 2)
        { return mLevel1.mRightBox.transform; }
        return null;
    }

    /// <summary>
    /// 点击按钮按下事件
    /// </summary>
    /// <param name="_go"></param>
    private void CheckBtnDown(GameObject _go)
    {
        if (bLvPass)
            return;

        AudioClip cp = ResManager.GetClip("checkgamebtn_sound", "checkgamebtn_down");
        mSoundCtrl.PlaySortSound(cp);

        SetCheckBtnSprite("btndown");
    }
    /// <summary>
    /// 设置按钮点击抬起事件
    /// </summary>
    /// <param name="_action"></param>
    public void SetCheckBtnUp(EventTriggerListener.VoidDelegate _action)
    {
        EventTriggerListener.Get(mCheckBtn.gameObject).onUp = _action;
    }

    /// <summary>
    /// 按钮事件active
    /// </summary>
    /// <param name="_active"></param>
    public void CheckBtnActive(bool _active)
    {
        imgCheckBtn.raycastTarget = _active;
    }

    /// <summary>
    /// 设置checkbtn sprite
    /// </summary>
    /// <param name="_spriteName"></param>
    public void SetCheckBtnSprite(string _spriteName)
    {
        imgCheckBtn.sprite = ResManager.GetSprite("catchfish_sprite", _spriteName);
    }

    /// <summary>
    /// 按钮显示/隐藏
    /// </summary>
    /// <param name="_show"></param>
    public void CheckBtnShow(bool _show)
    {
        if (_show)
        {
            mCheckBtn.transform.localScale = Vector3.one * 0.001f;
            mCheckBtn.gameObject.SetActive(true);
            mCheckBtn.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            SetCheckBtnSprite("btnup");
        }
        else
        {
            mCheckBtn.transform.DOScale(Vector3.one * 0.001f, 0.3f).OnComplete(()=> 
            {
                mCheckBtn.gameObject.SetActive(false);
            });
        }
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

        if (nLevel == 1)
        {
            SetTipSound(mLevel1.ieSoundTip());
        }
        else if (nLevel == 2)
        {
            SetTipSound(mLevel2.ieSoundTip());
        }
        else
        {
            SetTipSound(mLevel3.ieSoundTip());
        }

        StartTipSound();
    }

    public void StopTipSound()
    {
        if (ieTipSound != null)
            StopCoroutine(ieTipSound);
        mSoundCtrl.mKimiAudioSource.Stop();
    }
    public void StartTipSound()
    {
        if (ieTipSound != null)
            StartCoroutine(ieTipSound);
    }

    #endregion


    /// <summary>
    /// play checkBtn OK effect
    /// </summary>
    public void PlayCheckBtnEffect()
    {
        mCheckBtnFX.Play();
    }

}
