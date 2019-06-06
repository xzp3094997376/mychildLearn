using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KnowOneAndTen : BaseScene
{

    public int nLevel = 1;
    public const int nLevels = 3;
    public bool bLvPass = false;
    public bool bPlayOtherTip = false;

    private RawImage rawImage;

    private PlaySoundController mSoundCtrl;
    public PlaySoundController SoundCtrl { get { return mSoundCtrl; } }

    public KOAT_FirstShow mFirstShow = null;
    public KOAT_Level1 mLevel1 = null;
    public KOAT_Level2 mLevel2 = null;
    public KOAT_Level3 mLevel3 = null;

    public KOAT_RectBox mRectBox = null;

    void Awake()
    {
        mSceneType = SceneEnum.KnowOneAndTen;
        CallLoadFinishEvent();

        //rawImage = UguiMaker.newGameObject("bg", transform).AddComponent<RawImage>();
        ////rawImage.texture = ResManager.GetTexture("knowoneandten_texture", "bg");
        //rawImage.rectTransform.sizeDelta = new Vector2(1280f, 800f);
        //rawImage.raycastTarget = false;

        mRectBox = transform.Find("mRectBox").gameObject.AddComponent<KOAT_RectBox>();
        mRectBox.InitAwake();
    }

    // Use this for initialization
    void Start ()
    {
        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        mSoundCtrl.PlayBGSound("bgmusic_loop3", "bgmusic_loop3", 0.2f);

        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);

        mFirstShow = UguiMaker.newGameObject("mFirstShow", transform).AddComponent<KOAT_FirstShow>();
        mLevel1 = UguiMaker.newGameObject("mLevel1", transform).AddComponent<KOAT_Level1>();
        mLevel2 = UguiMaker.newGameObject("mLevel2", transform).AddComponent<KOAT_Level2>();
        mLevel3 = UguiMaker.newGameObject("mLevel3", transform).AddComponent<KOAT_Level3>();

        //mRectBox.transform.SetSiblingIndex(8);

        StartCoroutine(ieStart());
    }
    IEnumerator ieStart()
    {
        yield return new WaitForSeconds(0.1f);
        nLevel = 3;
        //mFirstShow.SetFinishCall(InitLevelData);
        //mFirstShow.InitData();
        InitLevelData();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitLevelData();
        }
    }

    public void InitLevelData()
    {
        Debug.Log("init level");
        ResetInfos();

        if (nLevel == 1)
        {
            mLevel1.gameObject.SetActive(true);
            mLevel1.InitData();
        }
        else if (nLevel == 2)
        {
            mLevel2.gameObject.SetActive(true);
            mLevel2.InitData();
        }
        else
        {
            mLevel3.gameObject.SetActive(true);
            mLevel3.InitData();
        }
    }


    public void ResetInfos()
    {
        mFirstShow.ResetInfos();
        mLevel1.ResetInfos();
        mLevel2.ResetInfos();
        mLevel3.ResetInfos();

        mFirstShow.gameObject.SetActive(false);
        mLevel1.gameObject.SetActive(false);
        mLevel2.gameObject.SetActive(false);
        mLevel3.gameObject.SetActive(false);
    }

    /// <summary>
    /// 关卡pass检测
    /// </summary>
    public void MLevelPass()
    {
        bLvPass = true;
        Debug.Log("level pass");    
        StartCoroutine(TOver());
    }
    IEnumerator TOver()
    {
        mSoundCtrl.StopTipSound();
        yield return new WaitForSeconds(0.5f);
        TopTitleCtl.instance.AddStar();
        nLevel++;
        yield return new WaitForSeconds(0.5f);
        if (nLevel > nLevels)
        {
            //结算
            //Debug.LogError("Game Run Over!");
            yield return new WaitForSeconds(2f);
            GameOverCtl.GetInstance().Show(nLevels, ReplayGame);
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
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);
        InitLevelData();
    }

    public void SceneMove(bool _in)
    {
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
            mLevel1.PlaySoundTip();
        }
        else if (nLevel == 2)
        {
            mLevel2.PlaySoundTip();
        }
        else
        { }
    }

}
