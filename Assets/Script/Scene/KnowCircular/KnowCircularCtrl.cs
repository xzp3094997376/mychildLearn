using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class KnowCircularCtrl : BaseScene
{

    public int nLevel = 1;
    public const int nLevels = 3;
    public bool bLvPass = false;
    public bool bPlayOtherTip = false;

    private PlaySoundController mSoundCtrl;
    public PlaySoundController SoundCtrl { get { return mSoundCtrl; } }


    public KnowCirLevel1 mLevel1 = null;
    public KnowCirLevel2 mLevel2 = null;
    public KnowCirLevel3 mLevel3 = null;

    void Start()
    {
        mSceneType = SceneEnum.CombineGraphics;
        CallLoadFinishEvent();
        RawImage imgbg = UguiMaker.newRawImage("bg", transform, "knowcircular_texture", "beijing0", false);
        imgbg.rectTransform.sizeDelta = new Vector2(1280f, 800f);
        StartCoroutine(ieCreateSceneRes());
    }
    IEnumerator ieCreateSceneRes()
    {
        PreLoadResource();
        yield return new WaitForSeconds(1f);
        TopTitleCtl.instance.Reset();
        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);
        mSoundCtrl.PlayBGSound("bgmusic_loop2", "bgmusic_loop2");

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
        if (mLevel1 != null)
        {
            if (mLevel1.gameObject != null)
                GameObject.Destroy(mLevel1.gameObject);
            mLevel1 = null;
        }
        if (mLevel2 != null)
        {
            if (mLevel2.gameObject != null)
                GameObject.Destroy(mLevel2.gameObject);
            mLevel2 = null;
        }
        if (mLevel3 != null)
        {
            if (mLevel3.gameObject != null)
                GameObject.Destroy(mLevel3.gameObject);
            mLevel3 = null;
        }
    }
    public void SceneMove(bool _in)
    {
        if (mLevel1 != null)
        {
            mLevel1.transform.DOLocalMoveY(-800f, 1f);
        }
        if (mLevel2 != null)
        {
            mLevel2.transform.DOLocalMoveY(-800f, 1f);
        }
        if (mLevel3 != null)
        {
            mLevel3.transform.DOLocalMoveY(-800f, 1f);
        }
    }
    /// <summary>
    /// 初始化关卡
    /// </summary>
    public void InitLevelData()
    {
        ResetInfos();

        if (nLevel == 1)
        {
            mLevel1 = UguiMaker.newGameObject("level1", transform).AddComponent<KnowCirLevel1>();
            mLevel1.InitAwake(this);
        }
        else if (nLevel == 2)
        {
            mLevel2 = UguiMaker.newGameObject("level2", transform).AddComponent<KnowCirLevel2>();
            mLevel2.InitAwake(this);
            mLevel2.transform.localPosition = new Vector3(0f, -800f, 0f);
            mLevel2.transform.DOLocalMoveY(0f, 1f);
        }
        else
        {
            mLevel3 = UguiMaker.newGameObject("level3", transform).AddComponent<KnowCirLevel3>();
            mLevel3.InitAwake(this);
        }


    }
    /// <summary>
    /// 下一关
    /// </summary>
    public void LevelCheckNext()
    {
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
            yield return new WaitForSeconds(0.5f);
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
    /// <summary>
    /// 播放提示语音
    /// </summary>
    public void PlayTipSound()
    {
        if (bLvPass)
            return;
        if (bPlayOtherTip)
            return;
        SoundCtrl.StopTipSound();
        if (nLevel == 3)
        {
            AudioClip mcp = ResManager.GetClip("knowcircular_sound", "圆形藏起来了把它们点出来吧");
            SoundCtrl.PlaySound(mcp, 1f);
        }
        else
        {
            AudioClip mcp = ResManager.GetClip("knowcircular_sound", "把圆形找出来吧");
            SoundCtrl.PlaySound(mcp, 1f);
        }
    }

    public void PlayObjSound(int _type)
    {
        transform.DOLocalMove(Vector3.zero, 0.2f).OnComplete(()=> 
        {
            if (_type == 0)
            {
                SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "圆形"), 1f);
            }
            else if (_type == 1)
            { SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "正方形"), 1f); }
            else
            { SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "三角形"), 1f); }
        });
    }

    public void PlaySucSound()
    {
        string strCp = "哇哦做得很好";
        if (UnityEngine.Random.value > 0.5f)
        { strCp = "真是火眼金睛"; }
        SoundCtrl.PlaySound(ResManager.GetClip("knowcircular_sound", strCp), 1f);
    }



    #region//指引---
    private GuideHandCtl mGuideHand;
    /// <summary>
    /// 指引显示
    /// </summary>
    public void GuideShow(Vector3 _from,Vector3 _to)
    {
        if (mGuideHand == null)
        {
            mGuideHand = GuideHandCtl.Create(transform);
            mGuideHand.transform.SetSiblingIndex(5);
            mGuideHand.GuideTipDrag(_from, _to, -1, 1f, "hand1");
            mGuideHand.SetDragDate(new Vector3(0f, -26f, 0f), 1f);
        }
        else
        {
            mGuideHand.transform.SetSiblingIndex(5);
            mGuideHand.GuideTipDrag(_from, _to, -1, 1f, "hand1");
            mGuideHand.SetDragDate(new Vector3(0f, -26f, 0f), 1f);
        }
    }
    /// <summary>
    /// 关闭指引
    /// </summary>
    public void GuideStop()
    {
        if (mGuideHand != null)
        {
            mGuideHand.StopDrag();
        }
    }
    public void DesGuideHand()
    {
        if (mGuideHand != null)
        {
            GameObject.Destroy(mGuideHand.gameObject);
            mGuideHand = null;
        }
    }
    #endregion


    void PreLoadResource()
    {
        AbManager.GetAB(AbEnum.prefab, "knowcircular_prefab");
        cpSetBig = ResManager.GetClip("knowcircular_sound", "setbig");
        mobjtype0 = ResManager.GetPrefab("knowcircular_prefab", "spine0");
        mobjtype1 = ResManager.GetPrefab("knowcircular_prefab", "spine1");
        mobjtype2 = ResManager.GetPrefab("knowcircular_prefab", "spine2");
        mobjtype0.gameObject.SetActive(false);
        mobjtype1.gameObject.SetActive(false);
        mobjtype2.gameObject.SetActive(false);
    }
    AudioClip cpSetBig;
    public void PlaySetBigSound()
    {
        SoundCtrl.PlaySortSound(cpSetBig);
    }
    private GameObject mobjtype0;
    private GameObject mobjtype1;
    private GameObject mobjtype2;
    public GameObject GetObjType0 { get { return mobjtype0; } }
    public GameObject GetObjType1 { get { return mobjtype1; } }
    public GameObject GetObjType2 { get { return mobjtype2; } }
}
