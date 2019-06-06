using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 小动物转转转
/// </summary>
public class AnimalRotateCtrl : BaseScene
{

    public int nLevel = 1;
    public const int nLevels = 3;

    public bool bLvPass = false;

    [HideInInspector]
    public PlaySoundController mSoundCtrl;
    private RawImage sceneBG;

    public AnimalRotateLevel1 mLevel1 = null;
    public AnimalRotateLevel2 mLevel2 = null;
    public AnimalRotateLevel3 mLevel3 = null;

    private ParticleSystem mMoveStarFX;

    private Image imgKbaby;

    private void Awake()
    {
        mSceneType = SceneEnum.AnimalRotate;
        CallLoadFinishEvent();

        sceneBG = transform.Find("bg").GetComponent<RawImage>();
        sceneBG.texture = ResManager.GetTexture("animalrotate_texture", "animalrotatebg");
    }
    // Use this for initialization
    void Start ()
    {
        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        mSoundCtrl.PlayBGSound("bgmusic_loop0", "bgmusic_loop0");

        StartCoroutine(ieLoadStart());
    }

    IEnumerator ieLoadStart()
    {
        yield return new WaitForSeconds(0.1f);
        mLevel1 = UguiMaker.newGameObject("mLevel1", transform).AddComponent<AnimalRotateLevel1>();
        mLevel1.InitAwake();
        mLevel2 = UguiMaker.newGameObject("mLevel2", transform).AddComponent<AnimalRotateLevel2>();
        mLevel2.InitAwake();
        mLevel3 = UguiMaker.newGameObject("mLevel3", transform).AddComponent<AnimalRotateLevel3>();
        mLevel3.InitAwake();

        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);

        mMoveStarFX = ResManager.GetPrefab("fx_movestar", "fx_movestar").GetComponent<ParticleSystem>();
        mMoveStarFX.transform.SetParent(transform);
        mMoveStarFX.transform.localPosition = Vector3.zero;
        mMoveStarFX.transform.localScale = Vector3.one;
        mMoveStarFX.Pause();
        mMoveStarFX.Stop();

        GameObject mkbabyimg = new GameObject("imgKbaby");
        mkbabyimg.transform.SetParent(transform);
        mkbabyimg.transform.localPosition = Vector3.zero;
        mkbabyimg.transform.localScale = Vector3.one;
        imgKbaby = mkbabyimg.AddComponent<Image>();
        imgKbaby.raycastTarget = false;
        imgKbaby.color = new Color(0.6f, 0.6f, 0.6f, 0f);
        imgKbaby.rectTransform.sizeDelta = new Vector2(1280f, 800f);

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
        mLevel1.gameObject.SetActive(false);
        mLevel2.gameObject.SetActive(false);
        mLevel3.gameObject.SetActive(false);
        mLevel1.transform.localPosition = new Vector3(-1200f, 0f, 0f);
        mLevel2.transform.localPosition = new Vector3(-1200f, 0f, 0f);
        mLevel3.transform.localPosition = new Vector3(-1200f, 0f, 0f);
        imgKbaby.enabled = false;
    }

    public void InitLevelData()
    {
        ResetInfos();
        bLvPass = false;
        bPlayOtherTip = false;

        if (nLevel == 1)
        {
            mLevel1.gameObject.SetActive(true);
            mLevel1.SetData();
        }
        else if (nLevel == 2)
        {
            mLevel2.gameObject.SetActive(true);
            mLevel2.SetData();
        }
        else
        {
            mLevel3.gameObject.SetActive(true);
            mLevel3.SetData();
        }

        SceneMove(true);

        PlayTipSound();
        //StartCoroutine(IENaturePlayTipSound());
    }
    //IEnumerator IENaturePlayTipSound()
    //{
    //    yield return new WaitForSeconds(2f);
    //    PlayTipSound();
    //}

    private void SceneMove(bool _in)
    {
        if (mLevel1.gameObject.activeSelf)
        { mLevel1.SceneMove(_in); }
        if (mLevel2.gameObject.activeSelf)
        { mLevel2.SceneMove(_in); }
        if (mLevel3.gameObject.activeSelf)
        { mLevel3.SceneMove(_in); }

        if (_in)
        {
            AudioClip cp = Resources.Load<AudioClip>("sound/素材出现通用音效");
            mSoundCtrl.PlaySortSound(cp);
        }
        else
        {
            AudioClip cp = Resources.Load<AudioClip>("sound/素材出去通用");
            mSoundCtrl.PlaySortSound(cp);
        }
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
        yield return new WaitForSeconds(0.5f);

        nLevel++;
        if (nLevel > nLevels)
        {
            //结算
            GameOverCtl.GetInstance().Show(3, () =>
            {
                nLevel = 1;
                TopTitleCtl.instance.Reset();
                TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);
                InitLevelData();             
            });
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            SceneMove(false);
            yield return new WaitForSeconds(1f);
            InitLevelData();
        }
    }

    /// <summary>
    /// k宝显示
    /// </summary>
    /// <param name="_callback"></param>
    public void KBabyShow()
    {
        imgKbaby.color = new Color(0.6f, 0.6f, 0.6f, 0f);
        imgKbaby.enabled = true;
        DOTween.To(() => imgKbaby.color, x => imgKbaby.color = x, new Color(0.6f, 0.6f, 0.6f, 0.6f), 0.5f);
        KbadyCtl.instance.transform.localPosition = new Vector3(0f, -30f, 0f);
        KbadyCtl.instance.ShowSpine(Vector3.one * 0.5f);
        KbadyCtl.instance.PlaySpine(kbady_enum.Encourage_1, true);
    }
    /// <summary>
    /// k bao hide
    /// </summary>
    public void KBabyHide()
    {
        KbadyCtl.instance.HideSpine();
        DOTween.To(() => imgKbaby.color, x => imgKbaby.color = x, new Color(0.6f, 0.6f, 0.6f, 0f), 1f);
    }



    /// <summary>
    /// 创建一个animalrotateCell
    /// </summary>
    public AnimalRotateCell CreateAnimalRotateCell(int _id, Transform _transform)
    {
        AnimalRotateCell mcell = null;
        GameObject mgo = UguiMaker.newGameObject("cell" + _id, _transform);
        mcell = mgo.AddComponent<AnimalRotateCell>();
        mcell.InitAwake(_id);
        return mcell;
    }
    /// <summary>
    /// 创建一个animalrotateStation
    /// </summary>
    public AnimalRotateStation CreateAnimalRotateStation(int _type, int[] _cellIDs, Transform _transform)
    {
        AnimalRotateStation mstation = null;
        GameObject mgo = ResManager.GetPrefab("animalrotate_prefab", "type" + _type);
        mgo.transform.SetParent(_transform);
        mgo.transform.localScale = Vector3.one;
        mgo.transform.localPosition = Vector3.zero;
        mstation = mgo.AddComponent<AnimalRotateStation>();
        mstation.InitAwake(_type, _cellIDs);
        return mstation;
    }

    /// <summary>
    /// 检测两个station 的 rotate id是否相同
    /// </summary>
    public bool CheckStationRotationIsSame(AnimalRotateStation _station1, AnimalRotateStation _station2)
    {
        if (_station1.nRotateID == _station2.nRotateID)
        {
            for (int i = 0; i < 4; i++)
            {
                AnimalRotateCell cell0 = _station1.mCells[i];
                AnimalRotateCell cell1 = _station2.mCells[i];
                if (cell0.nRotateID != cell1.nRotateID)
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 检测两个station的动物是否一样
    /// </summary>
    public bool CheckStationCellsIsSame(AnimalRotateStation _station1, AnimalRotateStation _station2)
    {
        for (int i = 0; i < 4; i++)
        {
            AnimalRotateCell cell0 = _station1.mCells[i];
            AnimalRotateCell cell1 = _station2.mCells[i];
            if (cell0.nID != cell1.nID)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 粒子移动
    /// </summary>
    public void MovePraSys(Vector3 _worldForm, Vector3 _worldTo, float _time = 0.5f)
    {
        mMoveStarFX.Stop();
        mMoveStarFX.transform.position = _worldForm;
        mMoveStarFX.Play();
        mSoundCtrl.PlaySortSound("animalrotate_sound", "starmove");
        mMoveStarFX.transform.DOMove(_worldTo, _time).OnComplete(() =>
        {
            mMoveStarFX.Stop();
        });
    }

    /// <summary>
    /// 第3 关的检测
    /// </summary>
    public bool CheckStationRotationIsSameByAngle(AnimalRotateLv3Station _station1, AnimalRotateLv3Station _station2)
    {
        float frota1 = _station1.transform.localEulerAngles.z;
        float frota2 = _station2.transform.localEulerAngles.z;
        float findex = Mathf.Abs((frota1 - frota2));
        if (findex > 3f)
        {
            return false;
        }



        for (int i = 0; i < 4; i++)
        {
            AnimalRotateCell cell0 = _station1.mCellStations[i].mcell;
            AnimalRotateCell cell1 = _station2.mCellStations[i].mcell;
            if (cell0.nID != cell1.nID)
            {
                return false;
            }
            float fcell0 = cell0.transform.localEulerAngles.z;
            float fcell1 = cell1.transform.localEulerAngles.z;
            float fcellIndex = Mathf.Abs((fcell0 - fcell1));
            if (fcellIndex > 3)
            {
                return false;
            }
        }

        return true;
    }



    IEnumerator ieTipSound = null;
    public bool bPlayOtherTip = false;
    public void PlayTipSound()
    {
        if (bLvPass)
            return;
        if (bPlayOtherTip)
            return;

        if (ieTipSound != null)
            StopCoroutine(ieTipSound);

        if (nLevel == 1)
        { SetTipSound(mLevel1.iePlayTipSoundLv1()); }
        else if (nLevel == 2)
        { SetTipSound(mLevel2.iePlayTipSoundLv2()); }
        else
        { SetTipSound(mLevel3.iePlayTipSoundLv3()); }

        if (ieTipSound != null)
            StartCoroutine(ieTipSound);
    }
    public void SetTipSound(IEnumerator _ieTipSound)
    {
        ieTipSound = _ieTipSound;
    }

}
