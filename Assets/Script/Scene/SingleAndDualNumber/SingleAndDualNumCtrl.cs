using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class SingleAndDualNumCtrl : BaseScene
{

    public int nLevel = 0;
    public int nLevels = 3;

    /// <summary>
    /// 跳转按钮是否激活
    /// </summary>
    public bool bButtonActive = true;

    private PlaySoundController mSoundCtrl;
    public PlaySoundController SoundCtrl { get { return mSoundCtrl; } }

    RawImage rawImage;

    public SingleDualNum_panel1New panel1;
    public SingleDualNum_panel2 panel2;
    public SingleDualNum_panel3 panel3;

    private Vector3 vBtns;
    private Transform btns;
    private Button btn1;
    private Button btn2;
    private Button btn3;

    private Image imgBtn1;
    private Image imgBtn2;
    private Image imgBtn3;
    private Image imgBtn1_down;
    private Image imgBtn2_down;
    private Image imgBtn3_down;

    private ParticleSystem parsys;

    private void Awake()
    {
        mSceneType = SceneEnum.SingleAndDualNumber;
        CallLoadFinishEvent();

        rawImage = transform.Find("RawImage").GetComponent<RawImage>();
        rawImage.enabled = false;
        //paopao
        GameObject mpaopao = ResManager.GetPrefab("effect_paopao", "paopao");
        mpaopao.transform.SetParent(rawImage.transform);
        mpaopao.transform.localScale = Vector3.one;
        mpaopao.transform.localPosition = new Vector3(0f, -300f, 0f);
        //imgbg
        Image imgBlue = UguiMaker.newGameObject("imgblue", rawImage.transform).AddComponent<Image>();
        imgBlue.rectTransform.sizeDelta = new Vector2(1280f, 800f);
        imgBlue.color = new Color(40f / 255, 144f / 255, 223f / 255, 1f);
        RawImage imgBG = UguiMaker.newRawImage("bg", rawImage.transform, "singledualnum_texture", "bg", false);
        imgBG.rectTransform.sizeDelta = new Vector2(1280f, 185f);
        imgBG.rectTransform.anchoredPosition = new Vector2(0f, -310f);

        panel1 = UguiMaker.newGameObject("Panel1", transform).AddComponent<SingleDualNum_panel1New>();       
        panel2 = LoadGameObj(transform, "Panel2").GetComponent<SingleDualNum_panel2>();        
        panel3 = LoadGameObj(transform, "Panel3").GetComponent<SingleDualNum_panel3>();
        
        panel1.transform.localPosition = new Vector3(0f, 1000f, 0f);
        panel2.transform.localPosition = new Vector3(0f, 1000f, 0f);
        panel3.transform.localPosition = new Vector3(0f, 1000f, 0f);

        btns = transform.Find("btns");
        btns.transform.SetSiblingIndex(10);
        vBtns = btns.transform.localPosition;
        btns.transform.localPosition = vBtns + new Vector3(0f, -400f, 0f);
    }
    GameObject LoadGameObj(Transform _transform, string _name)
    {
        GameObject mgo = ResManager.GetPrefab("singledualnum_prefab", _name);
        mgo.transform.SetParent(_transform);
        mgo.transform.localScale = Vector3.one;
        mgo.transform.localRotation = Quaternion.identity;
        mgo.transform.SetSiblingIndex(2);
        return mgo;
    }

    // Use this for initialization
    void Start ()
    {
        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        mSoundCtrl.InitAwake();
        StartCoroutine(IEWaite());
    }
    IEnumerator IEWaite()
    {      
        yield return new WaitForSeconds(0.1f);
        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);
        TopTitleCtl.instance.HideAllStar();

        GetClip("biut");
        PreLoadFishs();

        panel1.InitAwake();
        panel2.InitAwake();
        panel3.InitAwake();

        parsys = LoadGameObj(transform, "parsys").GetComponent<ParticleSystem>();
        parsys.Stop();
        parsys.Pause();

        btn1 = btns.Find("Button1").GetComponent<Button>();
        btn2 = btns.Find("Button2").GetComponent<Button>();
        btn3 = btns.Find("Button3").GetComponent<Button>();
        EventTriggerListener.Get(btn1.gameObject).onClick = ClickBtn;
        EventTriggerListener.Get(btn2.gameObject).onClick = ClickBtn;
        EventTriggerListener.Get(btn3.gameObject).onClick = ClickBtn;
        imgBtn1 = btn1.transform.GetComponent<Image>();
        imgBtn2 = btn2.transform.GetComponent<Image>();
        imgBtn3 = btn3.transform.GetComponent<Image>();
        imgBtn1.sprite = ResManager.GetSprite("singledualnum_sprite", "btn1");
        imgBtn2.sprite = ResManager.GetSprite("singledualnum_sprite", "btn2");
        imgBtn3.sprite = ResManager.GetSprite("singledualnum_sprite", "btn3");

        imgBtn1_down = imgBtn1.transform.Find("btn1_down").GetComponent<Image>();
        imgBtn2_down = imgBtn2.transform.Find("btn2_down").GetComponent<Image>();
        imgBtn3_down = imgBtn3.transform.Find("btn3_down").GetComponent<Image>();
        imgBtn1_down.sprite = ResManager.GetSprite("singledualnum_sprite", "btn1_down");
        imgBtn2_down.sprite = ResManager.GetSprite("singledualnum_sprite", "btn2_down");
        imgBtn3_down.sprite = ResManager.GetSprite("singledualnum_sprite", "btn3_down");

        InitLevelData();

        yield return new WaitForSeconds(0.2f);
        //螃蟹
        GameObject pangxie = ResManager.GetPrefab("singledualnum_prefab", "mlittle", rawImage.transform);
        pangxie.transform.localPosition = new Vector3(800f, -390f, 0f);
        pangxie.transform.DOLocalMoveX(-800f, 20f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        //海草
        yield return new WaitForSeconds(0.2f);
        GameObject haizao = ResManager.GetPrefab("singledualnum_prefab", "mplant", rawImage.transform);
        haizao.transform.localPosition = new Vector3(0f, -600f, 0f);
        haizao.transform.localScale = new Vector3(0.9f, 1f, 1f);
        haizao.transform.SetSiblingIndex(2);
        haizao.transform.DOLocalMoveY(-370f, 0.5f);

        mSoundCtrl.SetDelayLoadBGClip(4.5f);
        mSoundCtrl.PlayBGSound1("bgmusic_loop0", "bgmusic_loop0");
    }


    GameObject[] mPreLoadFishs = new GameObject[4];
    private void PreLoadFishs()
    {
        for (int i = 1; i <= 4; i++)
        {
            mPreLoadFishs[i-1] = ResManager.GetPrefab("aa_littlefish_prefab", "littlefish" + i);
            mPreLoadFishs[i - 1].SetActive(false);
        }
    }
    public GameObject GetFishResByID(int _id)
    {
        return mPreLoadFishs[_id];
    }



    public void ResetInfos()
    {
        bButtonActive = false;
        PanelsActive(false);
        imgBtn1_down.gameObject.SetActive(false);
        imgBtn2_down.gameObject.SetActive(false);
        imgBtn3_down.gameObject.SetActive(false);
    }
    public void InitLevelData()
    {
        ResetInfos();
        imgBtn1_down.gameObject.SetActive(true);

        nPanelID = 1;
        PanelMove(true, nPanelID, fmoveTime);
        //BtnsMove(true, fmoveTime);
    }


    /// <summary>
    /// 关卡pass检测
    /// </summary>
    public void MLevelPass()
    {
        //TopTitleCtl.instance.AddStar();
        AndroidDataCtl.DoAndroidFunc("onCompleteOneGame");
        //Debug.Log("安卓通信：发送打通游戏信号");
        AndroidDataCtl.DoAndroidFunc("completeOneLevel");

        StopTipSound();
        StartCoroutine(TOver());
    }
    IEnumerator TOver()
    {
        yield return new WaitForSeconds(0.5f);
        GameOverCtl.GetInstance().Show(1, () =>
        {
            TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);
            nLevel = 1;
            panel1.ResetInfos();
            panel2.ReplayReset();
            panel3.ReplayReset();
            InitLevelData();
            bButtonActive = true;
        });
    }
    public void SceneMove(bool _in)
    {
    }


    public int nPanelID = 1;
    /// <summary>
    /// 按钮点击
    /// </summary>
    private void ClickBtn(GameObject _go)
    {
        if (!bButtonActive)
            return;

        imgBtn1_down.gameObject.SetActive(false);
        imgBtn2_down.gameObject.SetActive(false);
        imgBtn3_down.gameObject.SetActive(false);

        int thepanelID = 0;
        if (_go == btn1.gameObject)
        {
            thepanelID = 1;
            imgBtn1_down.gameObject.SetActive(true);
        }
        else if (_go == btn2.gameObject)
        {
            thepanelID = 2;
            imgBtn2_down.gameObject.SetActive(true);
        }
        else
        {
            thepanelID = 3;
            imgBtn3_down.gameObject.SetActive(true);
        }

        if (thepanelID == nPanelID)
            return;

        StopTipSound();
        //切换页面
        ChangePanel(nPanelID, thepanelID);
    }

    private void PanelsActive(bool _active)
    {
        panel1.gameObject.SetActive(_active);
        panel2.gameObject.SetActive(_active);
        panel3.gameObject.SetActive(_active);
    }

    /// <summary>
    /// 切换页面
    /// </summary>
    public void ChangePanel(int nowID, int _nextID)
    {        
        panel1.StopZongjie();
        mSoundCtrl.StopTipSound();
        bPlayOtherTip = false;

        imgBtn1_down.gameObject.SetActive(false);
        imgBtn2_down.gameObject.SetActive(false);
        imgBtn3_down.gameObject.SetActive(false);
        if(_nextID == 1)
            imgBtn1_down.gameObject.SetActive(true);
        else if(_nextID == 2)
            imgBtn2_down.gameObject.SetActive(true);
        else
            imgBtn3_down.gameObject.SetActive(true);

        PanelMove(false, nowID, fmoveTime);
        nPanelID = _nextID;
        ChangePanelReset();
        BtnsMove(false, fmoveTime, () =>
        {
            PanelMove(true, nPanelID, fmoveTime);
        });

        GuideHide(); 
    }
    float fmoveTime = 0.5f;

    /// <summary>
    /// 面板进出changjing
    /// </summary>
    public void PanelMove(bool _in,int _id,float _time)
    {
        if (_id == 1)
        {
            panel1.SceneMove(_in, _time);
        }
        else if (_id == 2)
        {
            panel2.SceneMove(_in, _time);
        }
        else
        {
            panel3.SceneMove(_in, _time);
        }

        if (_in)
        {
            AudioClip cp = Resources.Load<AudioClip>("sound/素材出现通用音效");
            AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position);
        }
        else
        {
            AudioClip cp = Resources.Load<AudioClip>("sound/素材出去通用");
            AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position);
        }
    }


    /// <summary>
    /// 切换按钮进/出场
    /// </summary>
    public void BtnsMove(bool _in,float _time, System.Action _callback= null)
    {
        btnMoveCallback = _callback;
        if (_in)
        {
            btns.transform.DOLocalMove(vBtns, _time).OnComplete(() =>
            {
                if (btnMoveCallback != null)
                    btnMoveCallback();
            }); ;
        }
        else
        {
            btns.transform.DOLocalMove(vBtns + new Vector3(0f, -400f, 0f), _time).OnComplete(() =>
            {
                if (btnMoveCallback != null)
                    btnMoveCallback();
            });
        }
    }
    private System.Action btnMoveCallback = null;


    /// <summary>
    /// 粒子移动
    /// </summary>
    public void MovePraSys(Vector3 _worldForm, Vector3 _worldTo, float _time = 0.5f)
    {
        parsys.Stop();
        parsys.transform.position = _worldForm;
        parsys.Play();
        PlayTheSortSound("starmove");
        parsys.transform.DOMove(_worldTo, _time).OnComplete(()=> 
        {
            parsys.Stop();
        });
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
        //if (bLvPass)
        //    return;
        if (bPlayOtherTip)
            return;

        StopTipSound();

        if (nPanelID == 1)
        {
            AudioClip gamename1 = GetClip("连一连");//game-tips2-4-3
            PlaySound(gamename1, 1f);
        }
        else if (nPanelID == 2)
        {
            panel2.PlayTipSoundPanel2();
            StartTipSound();
        }
        else
        {
            panel3.mPlayTipSound3();
            if (!panel3.bPanelOK)
                StartTipSound();
        }       
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
        
    
    /// <summary>
    /// 播放语音
    /// </summary>
    public void PlaySound(AudioClip _clip, float _volume)
    {
        mSoundCtrl.PlaySound(_clip, _volume);
    }

    public AudioClip GetClip(string _clipName)
    {
        AudioClip clip1 = ResManager.GetClip("singledualnum_sound", _clipName);
        return clip1;
    }
    public AudioClip GetNumClip(int _num)
    {
        AudioClip clip1 = ResManager.GetClip("number_sound", _num.ToString());
        return clip1;
    }
    /// <summary>
    /// sort声音
    /// </summary>
    public void PlayTheSortSound(string _clipname)
    {
        AudioClip aclip = GetClip(_clipname);
        PlayTheSortSound(aclip);
    }
    public void PlayTheSortSound(AudioClip aclip)
    {
        mSoundCtrl.PlaySortSound(aclip);
    }
    public void BGSoundActive(bool active)
    {
        if (active)
        {
            SoundManager.instance.PlayBg();
        }
        else
        {
            SoundManager.instance.StopBg();
        }
    }
    #endregion

    /// <summary>
    /// 切换页面重置一些信息
    /// </summary>
    public void ChangePanelReset()
    {
        panel2.ChangePanelResetInfos();
        panel3.ChangePanelResetInfos();
        BGSoundActive(true);
        StopTipSound();
    }


    #region//Guide
    bool bOnceGuide = false;
    GuideHandCtl mGuideHand;
    /// <summary>
    /// 显示指引
    /// </summary>
    public void GuideShow(Transform _trans, Vector3 _vF, Vector3 _vT)
    {
        if (!bOnceGuide)
        {
            mGuideHand = GuideHandCtl.Create(_trans);
            mGuideHand.GuideTipDrag(_vF, _vT, -1, 1f, "hand1");
            mGuideHand.SetDragDate(new Vector3(10f, -25f, 0f), 1f);
            bOnceGuide = true;
        }
    }
    /// <summary>
    /// 删除指引
    /// </summary>
    public void GuideHide()
    {
        if (mGuideHand != null)
        {
            mGuideHand.StopDrag();
            GameObject.Destroy(mGuideHand.gameObject);
            mGuideHand = null;
        }
    }
    #endregion


}




