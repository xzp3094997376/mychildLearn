using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class NumberReasoningCtrl : BaseScene
{
    public int nLevel = 1;
    public const int nLevels = 3;
    public bool bLvPass = false;

    private Image imgBG;
    
    private PlaySoundController mSoundCtrl;
    public PlaySoundController SoundCtrl { get { return mSoundCtrl; } }


    
    /// <summary>
    /// 计算机输入窗口
    /// </summary>
    /// <returns></returns>
    public InputNumObj MInputNumObj { get { return mInputNumObj; } }
    private InputNumObj mInputNumObj;
    /// <summary>
    /// 输入光标
    /// </summary>
    public Image mInputTip { get { return imgInputTip; } }
    private Image imgInputTip;

    private NumReasonLevel1 mlevel1 = null;
    private NumReasonLevel2 mlevel2 = null;
    private NumReasonLevel3 mlevel3 = null;

    void Awake()
    {
        mSceneType = SceneEnum.NumberReasoning;
        CallLoadFinishEvent();

        imgBG = UguiMaker.newGameObject("imgBG", transform).AddComponent<Image>();
        imgBG.raycastTarget = false;
        imgBG.rectTransform.sizeDelta = new Vector2(1280f, 800f);
        imgBG.color = new Color(146f / 255, 213f / 255, 251f / 255, 1f);
    }

    void Start ()
    {
        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        mSoundCtrl.PlayBGSound("bgmusic_loop2", "bgmusic_loop2", 0.2f);
        TopTitleCtl.instance.Reset();
        KbadyCtl.Init();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);

        mlevel1 = UguiMaker.newGameObject("mlevel1", transform).AddComponent<NumReasonLevel1>();
        mlevel2 = UguiMaker.newGameObject("mlevel2", transform).AddComponent<NumReasonLevel2>();
        mlevel3 = UguiMaker.newGameObject("mlevel3", transform).AddComponent<NumReasonLevel3>();
        mlevel1.InitAwake();
        mlevel2.InitAwake();
        mlevel3.InitAwake();

        mlevel2.transform.localPosition = new Vector3(-1400f, 0f, 0f);
        mlevel3.transform.localPosition = new Vector3(-1400f, 0f, 0f);

        SceneResInit();
        nLevel = 1;
        InitLevelData();

        PreLoadResource();
    }

    //输入框/提示光标创建
    private void SceneResInit()
    {
        if (mInputNumObj == null)
        {
            InputInfoData data = new InputInfoData();
            data.fNumScale = 0.75f;
            data.strAlatsName = "inputnumres_sprite";
            data.strCellPicFirstName = "";
            data.strPicBG = "bg";
            data.bgcolor = new Color32(252, 229, 194, 255);
            data.color_blockBG = new Color32(179, 138, 89, 255);
            data.color_blockNum = new Color32(202, 183, 155, 255);
            data.color_blockSureBG = new Color32(172, 123, 66, 255);
            data.color_blockSureStart = new Color32(202, 183, 155, 255);
            mInputNumObj = InputNumObj.Create(transform, data);
            mInputNumObj.SetInputNumberCallBack(null);
            mInputNumObj.SetClearNumberCallBack(null);
            mInputNumObj.gameObject.SetActive(false);
            mInputNumObj.nCountLimit = 2;
        }
        //输入光标
        if (imgInputTip == null)
        {
            imgInputTip = UguiMaker.newGameObject("imgInputTip", transform).AddComponent<Image>();
            imgInputTip.raycastTarget = false;
            imgInputTip.color = Color.black;
            imgInputTip.rectTransform.sizeDelta = new Vector2(5f, 50f);
            DOTween.To(() => imgInputTip.color, x => imgInputTip.color = x, new Color(0f, 0f, 0f, 0f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
        imgInputTip.gameObject.SetActive(false);

        mInputNumObj.transform.SetSiblingIndex(10);
        mInputTip.transform.SetSiblingIndex(11);
    }

	//void Update ()
 //   {
 //       if (Input.GetKeyDown(KeyCode.Alpha1))
 //       {
 //           InitLevelData();
 //       }
 //       if (Input.GetKeyDown(KeyCode.Alpha2))
 //       {
 //           RePlayGame();
 //       }
 //   }

    /// <summary>
    /// 重置信息
    /// </summary>
    public void ResetInfos()
    {
        bLvPass = false;
        bPlayOtherTip = false;
    }

    /// <summary>
    /// 初始化关卡
    /// </summary>
    public void InitLevelData()
    {
        ResetInfos();
        if (nLevel == 1)
        {
            mlevel3.MoveOutAndReset();
            mlevel1.transform.SetSiblingIndex(1);
            mlevel1.SetData();
        }
        else if (nLevel == 2)
        {
            mlevel1.MoveOutAndReset();
            mlevel2.transform.SetSiblingIndex(1);
            mlevel2.SetData();
        }
        else
        {
            mlevel2.MoveOutAndReset();
            mlevel3.transform.SetSiblingIndex(1);
            mlevel3.SetData();
        }
    }

    /// <summary>
    /// 进场/退场
    /// </summary>
    /// <param name="_in"></param>
    public void SceneMove(bool _in)
    {
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
        mSoundCtrl.StopTipSound();
        bPlayOtherTip = true;
        yield return new WaitForSeconds(0.5f);

        TopTitleCtl.instance.AddStar();
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
        SoundCtrl.StopTipSound();
        if (nLevel == 1)
        { mlevel1.PlayTipSound(); }
        else if (nLevel == 2)
        { mlevel2.PlayTipSound(); }
        else
        { mlevel3.PlayTipSound(); }
    }
    /// <summary>
    /// 播放动物声音
    /// </summary>
    /// <param name="_animalID"></param>
    public void PlayAnimalSound(int _animalID, bool _suc)
    {
        string strName = MDefine.GetAnimalNameByID_CH(_animalID);
        if (_suc)
        { mSoundCtrl.PlaySortSound("aa_animal_sound", strName + "0"); }
        else
        { mSoundCtrl.PlaySortSound("aa_animal_sound", strName + "1"); }
    }
    /// <summary>
    /// 播放鼓励语音
    /// </summary>
    public void PlayGoodGoodSound()
    {
        mSoundCtrl.StopTipSound();
        AudioClip cp = SoundCtrl.GetClip("aa_good_sound", "goodsound" + Random.Range(1, 5));
        SoundCtrl.PlaySound(cp, 1f);
    }
    #endregion





    /// <summary>
    /// 预加载一些语音
    /// </summary>
    public void PreLoadResource()
    {
        ResManager.GetClipAsync("numberreasoning_sound", "s观察牌子上的数字规律", null);

        ResManager.GetClipAsync("numberreasoning_sound", "s观察车子上面的数字规律", null);
        ResManager.GetClipAsync("numberreasoning_sound", "s你对数字很敏感", null);
        ResManager.GetClipAsync("numberreasoning_sound", "s有的车子是偶数", null);
        ResManager.GetClipAsync("numberreasoning_sound", "s有的车子是奇数", null);
        ResManager.GetClipAsync("numberreasoning_sound", "s每个数字之间都相差", null);
        ResManager.GetClipAsync("numberreasoning_sound", "s用后面的数字减去前面的", null);

        Texture bg2 = ResManager.GetTexture("numberreasoning_texture", "numreasonimg_bj2");

        StartCoroutine(ieLoadAnimas());
    }
    IEnumerator ieLoadAnimas()
    {
        AssetBundle ab = AbManager.GetAB(AbEnum.prefab, "aa_animal_person_prefab");
        AssetBundleRequest abReq = ab.LoadAllAssetsAsync();
        yield return abReq;
        
    }



}
