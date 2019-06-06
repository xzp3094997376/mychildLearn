using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;


//"Assets/ResSprite/lineupctrl_sprite",
//"Assets/ResPrefab/lineupctrl_prefab",
//"Assets/ResPrefab/aa_animal_person_prefab",
//"Assets/ResSound/lineupctrl_sound",
//"Assets/ResTexture/lineupctrl_texture",
//"Assets/ResSound/bgmusic_loop0",
//"Assets/ResSound/aa_animal_name",
//"Assets/ResSound/aa_good_sound",
//"Assets/ResSound/aa_animal_sound",
//"Assets/ResSprite/number_slim",


/// <summary>
/// 排队来做操
/// </summary>
public class LineUpCtrl : BaseScene
{

    public int nLevel = 1;
    public const int nLevels = 2;
    public bool bLvPass = false;

    public int nAnimalCount = 0;

    private RawImage imgBG;
    [HideInInspector]
    public PlaySoundController mSoundCtrl;
    [HideInInspector]
    public InputNumObj mInputNumObj;

    private GameObject mAnimalParent;
    public List<LineUp_AnimalObj> mAnimalObjList = new List<LineUp_AnimalObj>();
    public List<Vector3> mposListSet = new List<Vector3>();


    private Vector3 vDwon = new Vector3(0f, -280f, 0f);
    private LineUp_level1 mLevel1;
    private LineUp_level2 mLevel2;

    private AudioClip resShowOutCP;
    private AudioClip resShowInCP;
    private AudioClip resHuanhuCP;

    void Awake()
    { 
        mSceneType = SceneEnum.LineUpCtrl;
        CallLoadFinishEvent();

        imgBG = UguiMaker.newRawImage("bg", transform, "lineupctrl_texture", "xs_bj", false);
        imgBG.rectTransform.sizeDelta = new Vector2(1280f, 800f);

        mAnimalParent = UguiMaker.newGameObject("animals", transform);
        mAnimalParent.transform.localPosition = new Vector3(0f, -80f, 0f);       
    }

    void Start()
    {
        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        mSoundCtrl.InitAwake();
        StartCoroutine(ieLoadRes());
    }
    IEnumerator ieLoadRes()
    {    
        yield return new WaitForSeconds(0.1f);
        resHuanhuCP = ResManager.GetClip("lineupctrl_sound", "欢呼0");

        mLevel1 = UguiMaker.newGameObject("level1", transform).AddComponent<LineUp_level1>();
        mLevel2 = UguiMaker.newGameObject("level2", transform).AddComponent<LineUp_level2>();
        mLevel1.InitAwake(this);
        mLevel2.InitAwake(this);
        mLevel1.transform.localPosition = vDwon;
        mLevel2.transform.localPosition = vDwon;

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
      
        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);
        
        resShowInCP = Resources.Load<AudioClip>("sound/素材出现通用音效");
        resShowOutCP = Resources.Load<AudioClip>("sound/素材出去通用");

        nLevel = 1;
        InitLevelData();

        yield return new WaitForSeconds(1f);
        mSoundCtrl.SetDelayLoadBGClip(2f);
        mSoundCtrl.PlayBGSound("bgmusic_loop0", "bgmusic_loop0", 0.1f);
    }




    public void ResetInfos()
    {
        bLvPass = false;

        mLevel1.gameObject.SetActive(false);
        mLevel2.gameObject.SetActive(false);
    }

    /// <summary>
    /// 初始化关卡
    /// </summary>
    public void InitLevelData()
    {
        ResetInfos();
     
        if (nLevel == 1)
        {
            //清除数据
            Common.DestroyChilds(mAnimalParent.transform);
            mAnimalObjList.Clear();

            //animal count
            nAnimalCount = Random.Range(7, 10);
            if (nAnimalCount == 10)
                mAnimalParent.transform.localScale = Vector3.one * 0.93f;
            else
                mAnimalParent.transform.localScale = Vector3.one;

            //pos index
            List<float> findexpos = Common.GetOrderList(nAnimalCount, 130f);
            List<Vector3> mposList1 = new List<Vector3>();
            List<Vector3> mposList2 = new List<Vector3>();

            //hafl animal count
            int nOffsetcount = nAnimalCount / 2;
            //animal y offset
            float findex = 30f;
            //set animal y offset
            float fLeftIndex = -findex * nOffsetcount;
            for (int i = 0; i < nOffsetcount; i++)
            {
                float fX1 = findexpos[i];
                Vector3 vold1 = new Vector3(fX1, fLeftIndex, 0f);
                mposList1.Add(vold1);

                float fX2 = findexpos[findexpos.Count - i - 1];
                Vector3 vold2 = new Vector3(fX2, fLeftIndex, 0f);
                mposList2.Add(vold2);

                fLeftIndex += findex;
            }
            //位置pos
            mposListSet = new List<Vector3>();
            mposListSet.AddRange(mposList1);
            if (nAnimalCount % 2 == 1)
            {
                mposListSet.Add(Vector3.zero);
            }
            for (int i = mposList2.Count - 1; i >= 0; i--)
            {
                mposListSet.Add(mposList2[i]);
            }

            //动物出现效果
            StartCoroutine(ieAnimalSceneInEffect());
        }
        else
        {
            PlayActions();
        }
    }

    IEnumerator ieAnimalSceneInEffect()
    {
        //animal id list
        List<int> animalids = Common.GetIDList(1, 14, nAnimalCount, -1);
        //create animals
        for (int i = 0; i < nAnimalCount; i++)
        {
            int mid = animalids[i];
            LineUp_AnimalObj animalobj = CreateAnimalObj(mid, mAnimalParent.transform);
            animalobj.transform.localPosition = mposListSet[i] + new Vector3(0f, 600f, 0f);
            animalobj.nIndexID = i;
            mAnimalObjList.Add(animalobj);

            animalobj.transform.DOLocalMove(mposListSet[i], 0.2f);
            mSoundCtrl.PlaySortSound(resShowInCP, 0.5f);
            yield return new WaitForSeconds(0.21f);
        }

        PlayActions();
    }

    //集体播放动作
    public void PlayActions()
    {
        for (int i = 0; i < mAnimalObjList.Count; i++)
        {
            mAnimalObjList[i].PlayAnimation("face_idle", true);
        }
        StartCoroutine(iePlayActions());
    }
    IEnumerator iePlayActions()
    {
        yield return new WaitForSeconds(0.1f);
        if (nLevel == 1)
        {
            mLevel1.gameObject.SetActive(true);
            mLevel1.SetData();
        }
        else
        {
            mLevel2.gameObject.SetActive(true);
            mLevel2.SetData(mLevel1.targetID);
        }
        SceneMove(true);
    }


    /// <summary>
    /// 集体播放动作 欢呼
    /// </summary>
    public void PlayAllAnimalActions()
    {
        if (resHuanhuCP != null)
            mSoundCtrl.PlaySortSound(resHuanhuCP);

        for (int i = 0; i < mAnimalObjList.Count; i++)
        {
            mAnimalObjList[i].BoxActive(false);
            mAnimalObjList[i].PlayAnimation("face_sayyes", true);
        }
    }

    /// <summary>
    /// 集体播放动作 idle
    /// </summary>
    public void PlayAllAnimalIdle()
    {
        for (int i = 0; i < mAnimalObjList.Count; i++)
        {
            mAnimalObjList[i].PlayAnimation("face_idle", true);
            mAnimalObjList[i].BoxActive(true);
        }
    }


    /// <summary>
    /// 进场/退场
    /// </summary>
    /// <param name="_in"></param>
    public void SceneMove(bool _in)
    {
        GameObject mgo = mLevel1.gameObject;
        if (mLevel1.gameObject.activeSelf)
        { mgo = mLevel1.gameObject; }
        else if(mLevel2.gameObject.activeSelf)
        { mgo = mLevel2.gameObject; }

        if (_in)
        {
            mgo.transform.localPosition = vDwon + new Vector3(0f, -600f, 0f);
            mgo.transform.DOLocalMove(vDwon, 1f);
            mSoundCtrl.PlaySortSound(resShowInCP, 0.5f);
        }
        else
        {
            mgo.transform.DOLocalMove(vDwon + new Vector3(0f, -600f, 0f), 1f);
            mSoundCtrl.PlaySortSound(resShowOutCP);
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
        yield return new WaitForSeconds(1f);

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
        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);
        InitLevelData();
    }





    /// <summary>
    /// 创建一个动物obj
    /// </summary>
    /// <param name="_id">animal id</param>
    /// <returns></returns>
    public LineUp_AnimalObj CreateAnimalObj(int _id,Transform _trans)
    {
        GameObject mgo = UguiMaker.newGameObject(_id.ToString(), _trans);
        LineUp_AnimalObj objctrl = mgo.AddComponent<LineUp_AnimalObj>();
        objctrl.InitAwake(_id);
        return objctrl;
    }

    public LineUp_Sprite CreateSpriteObj(string _res, bool _isQue, Transform _trans)
    {
        GameObject mgo = UguiMaker.newGameObject(_res, _trans);
        LineUp_Sprite objctrl = mgo.AddComponent<LineUp_Sprite>();
        objctrl.InitAwake(_res, _isQue);
        return objctrl;
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
            SetTipSound(mLevel1.iePlaySoundTip());
        }
        else
        {
            SetTipSound(mLevel2.iePlaySoundTip());
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
