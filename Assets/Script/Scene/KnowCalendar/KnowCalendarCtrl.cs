using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class KnowCalendarCtrl : BaseScene
{
    public int nLevel = 1;
    public const int nLevels = 3;
    public bool bLvPass = false;

    private RawImage imgBG;
    public KnowCalendarLv1 mLevel1Ctrl;
    public KnowCalendarLv2 mLevel2Ctrl;
    public KnowCalendarLv3 mLevel3Ctrl;

    [HideInInspector]
    public PlaySoundController mSoundCtrl;
    [HideInInspector]
    public InputNumObj mInputNumCtrl;
    [HideInInspector]
    public Image imgGuangbiao;
    
    public InputWeekObj mInputWeekCtrl;

    private ParticleSystem mDropCheckFX;

    private void Awake()
    {
        mSceneType = SceneEnum.KnowCalendar;
        CallLoadFinishEvent();

        imgBG = UguiMaker.newRawImage("bg", transform, "knowcalendar_texture", "calendarbg", false);
        imgBG.rectTransform.sizeDelta = new Vector2(1280f, 800f);
        //数字输入框
        InputInfoData minputdata = new InputInfoData();
        minputdata.strAlatsName = "public";//"knowcalendar_sprite";
        minputdata.strCellPicFirstName = "input";//"kc_num";
        minputdata.strPicBG = "inputbg";//"kc_imgbg0";
        minputdata.fNumScale = 3.5f;
        minputdata.color_blockBG = new Color(179f / 255, 138f / 255, 89f / 255, 1f);
        minputdata.color_blockNum = new Color(202f / 255, 183f / 255, 155f / 255, 1f);
        minputdata.color_blockSureBG = new Color(172f / 255, 123f / 255, 66f / 255, 1f);
        minputdata.color_blockSureStart = new Color(202f / 255, 183f / 255, 155f / 255, 1f);
        minputdata.bgcolor = new Color(252f / 255, 229f / 255, 94f, 1f);
        mInputNumCtrl = InputNumObj.Create(transform, minputdata);
        mInputNumCtrl.gameObject.SetActive(false);
        mInputNumCtrl.transform.localScale = Vector3.one * 0.4f;
        mInputNumCtrl.nCountLimit = 2;
        //光标
        GameObject mgo = UguiMaker.newGameObject("guangbiao", transform);
        imgGuangbiao = mgo.AddComponent<Image>();
        imgGuangbiao.raycastTarget = false;
        imgGuangbiao.color = Color.black;
        imgGuangbiao.rectTransform.sizeDelta = new Vector2(3f, 43f);
        GuangbiaoFlash();
        imgGuangbiao.gameObject.SetActive(false);
        //星期输入框
        InputInfoData minputdataWeek = new InputInfoData();
        minputdataWeek.strAlatsName = "knowcalendar_sprite";
        minputdataWeek.strCellPicFirstName = "cn";
        minputdataWeek.strPicBG = "kc_imgbg0";
        minputdataWeek.color_blockBG = new Color(179f / 255, 138f / 255, 89f / 255, 1f);
        minputdataWeek.color_blockNum = new Color(202f / 255, 183f / 255, 155f / 255, 1f);
        minputdataWeek.color_blockSureBG = new Color(172f / 255, 123f / 255, 66f / 255, 1f);
        minputdataWeek.color_blockSureStart = new Color(202f / 255, 183f / 255, 155f / 255, 1f);
        minputdataWeek.bgcolor = new Color(252f / 255, 229f / 255, 94f, 1f);
        minputdataWeek.fscale = 1f;
        GameObject mgoweek = ResManager.GetPrefab("knowcalendar_prefab", "imputweekobj");
        mgoweek.transform.SetParent(transform);
        mInputWeekCtrl = mgoweek.AddComponent<InputWeekObj>();
        mInputWeekCtrl.mInputInfoData = minputdataWeek;
        mInputWeekCtrl.InitAwake();
        mInputWeekCtrl.gameObject.SetActive(false);


        mLevel1Ctrl = UguiMaker.newGameObject("mLevel1", transform).AddComponent<KnowCalendarLv1>();
        mLevel2Ctrl = UguiMaker.newGameObject("mLevel2", transform).AddComponent<KnowCalendarLv2>();
        mLevel3Ctrl = UguiMaker.newGameObject("mLevel3", transform).AddComponent<KnowCalendarLv3>();
        mLevel1Ctrl.InitAwake();
        mLevel2Ctrl.InitAwake();
        mLevel3Ctrl.InitAwake();


    }

    // Use this for initialization
    void Start ()
    {
        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        mSoundCtrl.PlayBGSound("bgmusic_loop0", "bgmusic_loop0");

        TopTitleCtl.instance.Reset();
        KbadyCtl.Init();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);

        mInputNumCtrl.transform.SetSiblingIndex(6);
        imgGuangbiao.transform.SetSiblingIndex(7);
        mInputWeekCtrl.transform.SetSiblingIndex(8);

        mDropCheckFX = ResManager.GetPrefab("effect_star2", "effect_star2").GetComponent<ParticleSystem>();
        mDropCheckFX.transform.SetParent(transform);
        mDropCheckFX.transform.localPosition = new Vector3(0f, 850f, 0f);
        mDropCheckFX.transform.localScale = Vector3.one;
        mDropCheckFX.Stop();

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
        mLevel1Ctrl.ResetInfos();
        mLevel2Ctrl.ResetInfos();
        mLevel3Ctrl.ResetInfos();
        mLevel1Ctrl.gameObject.SetActive(false);      
        mLevel2Ctrl.gameObject.SetActive(false);
        mLevel3Ctrl.gameObject.SetActive(false);
    }

    public void InitLevelData()
    {
        ResetInfos();
        bLvPass = false;

        if (nLevel == 1)
        {
            mLevel1Ctrl.gameObject.SetActive(true);
            mLevel1Ctrl.SetData();
        }
        else if (nLevel == 2)
        {
            //输入框回调设置
            mInputNumCtrl.SetInputNumberCallBack(mLevel2Ctrl.QuaInputNumCallBack);
            mInputNumCtrl.SetFinishInputCallBack(mLevel2Ctrl.QuaInputNumFinishCallBack);
            mInputNumCtrl.SetClearNumberCallBack(mLevel2Ctrl.QuaInputNumClearCallBack);
            //星期输入框回调设置
            mInputWeekCtrl.SetInputWeekCallBack(mLevel2Ctrl.QuaInputWeekCallBack);
            mInputWeekCtrl.SetFinishInputWeekCallBack(mLevel2Ctrl.QuaInputWeekFinishCallBack);

            mLevel2Ctrl.gameObject.SetActive(true);
            mLevel2Ctrl.SetData();
        }
        else
        {
            mLevel3Ctrl.gameObject.SetActive(true);
            mLevel3Ctrl.SetData();
        }

        SceneMove(true);

        PlayTipSound();
    }


    /// <summary>
    /// 下一关
    /// </summary>
    public void LevelCheckNext()
    {
        bLvPass = true;
        StartCoroutine(IETOver());
    }
    IEnumerator IETOver()
    {
        TopTitleCtl.instance.AddStar();
        yield return new WaitForSeconds(1f);

        //good sound
        AudioClip cp = ResManager.GetClip("knowcalendar_sound", "game-tips_good" + Random.Range(0, 2));
        mSoundCtrl.PlaySound(cp, 1f);
        yield return new WaitForSeconds(cp.length);

        nLevel++;
        if (nLevel > nLevels)
        {
            //结算
            GameOverCtl.GetInstance().Show(3, () =>
            {
                nLevel = 1;
                TopTitleCtl.instance.Reset();
                InitLevelData();
            });
        }
        else
        {
            yield return new WaitForSeconds(1f);
            SceneMove(false);
            yield return new WaitForSeconds(1.1f);
            InitLevelData();
        }
    }


    public void SceneMove(bool _in)
    {
        if (mLevel1Ctrl.gameObject.activeSelf)
            mLevel1Ctrl.SceneMove(_in);
        if (mLevel2Ctrl.gameObject.activeSelf)
            mLevel2Ctrl.SceneMove(_in);
        if (mLevel3Ctrl.gameObject.activeSelf)
            mLevel3Ctrl.SceneMove(_in);
    }


    /// <summary>
    /// 创建一个月日历
    /// </summary>
    public KnowCalendarMonth CreateMonth(Transform _trans, int _year, int _month)
    {
        Image imgmonth = UguiMaker.newImage("month", _trans, "knowcalendar_sprite", "kc_monthbg", false);
        KnowCalendarMonth monthctrl = imgmonth.gameObject.AddComponent<KnowCalendarMonth>();
        monthctrl.InitAwake(_year, _month);
        return monthctrl;
    }

    /// <summary>
    /// 光标闪动
    /// </summary>
    private void GuangbiaoFlash()
    {
        imgGuangbiao.transform.DOScale(Vector3.one, 0.6f).OnComplete(() =>
        {
            imgGuangbiao.enabled = false;
            imgGuangbiao.transform.DOScale(Vector3.one, 0.6f).OnComplete(() =>
            {
                imgGuangbiao.enabled = true;
                GuangbiaoFlash();
            });
        });
    }

    /// <summary>
    /// 光标位置调整
    /// </summary>
    public void SetGuangbiaoPos(Transform _target, string _strNum, ImageNumber _imgNumber, float _offset = 15f)
    {
        if (_target == null)
        {
            imgGuangbiao.gameObject.SetActive(false);
            return;
        }
        int theGet = 0;
        if (int.TryParse(_strNum, out theGet))
        {
            imgGuangbiao.transform.position = _imgNumber.transform.position;
            Vector2 vposs = imgGuangbiao.rectTransform.anchoredPosition;
            float num1Width = _imgNumber.GetNum1().rectTransform.sizeDelta.x;
            float num2Width = _imgNumber.GetNum2().rectTransform.sizeDelta.x;
            if (theGet >= 10)
            { imgGuangbiao.rectTransform.anchoredPosition = vposs + new Vector2((num1Width + num2Width) * 0.5f + _offset, 0f); }
            else
            { imgGuangbiao.rectTransform.anchoredPosition = vposs + new Vector2(num1Width * 0.5f + _offset, 0f); }
        }
    }

    /// <summary>
    /// 拖对特效
    /// </summary>
    /// <param name="_vWorldPos"></param>
    public void PlayDropOKFX(Vector3 _vWorldPos)
    {       
        mDropCheckFX.transform.position = _vWorldPos;
        mDropCheckFX.Play();
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
            SetTipSound(mLevel1Ctrl.iePlayTipSoune());
        }
        else if (nLevel == 2)
        {
            SetTipSound(mLevel2Ctrl.ieSoundTipLv2());
        }
        else
        {
            SetTipSound(mLevel3Ctrl.iePlayTipSound());
        }

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

    #endregion


}
