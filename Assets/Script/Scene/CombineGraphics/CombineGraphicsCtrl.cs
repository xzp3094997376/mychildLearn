using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class CombineGraphicsCtrl : BaseScene
{

    public int nLevel = 1;
    public const int nLevels = 2;
    public bool bLvPass = false;
    public int nToCount = 0;
    public int inImgRightCount = 0;//在右框的block数
    private bool bCanDrop = false;
    /// <summary>
    /// 缩放系数
    /// </summary>
    public float fScale = 2f;
    private float fStartScale = 1.3f;
    /// <summary>
    /// 允许的检测范围
    /// </summary>
    public float fCheckMaxDis = 35f;
  
    private PlaySoundController mSoundCtrl;
    public PlaySoundController SoundCtrl { get { return mSoundCtrl; } }
    private CG_DataInfos mDataInfos = new CG_DataInfos();

    private RawImage imgBG;
    private Image imgbtn;
    private RawImage chuang;

    private GameObject mLeft;
    private Image imgLeft;
    private CG_TheObj mTheObjCtrl;//target Obj
    private Image imgRealPic;
    private Image imgRealRight;

    private GameObject mRight;
    private Image imgRight;
    private GameObject mBlockTrans;
    public List<CG_Block> mBlockList = new List<CG_Block>();//block碎片

    private ParticleSystem smoke1;
    private ParticleSystem smoke2;

    public int[] mObjIDs = new int[3];
    AudioClip sceneOutCP;

    void Awake()
    {
        mSceneType = SceneEnum.CombineGraphics;
        CallLoadFinishEvent();
        imgBG = UguiMaker.newRawImage("bg", transform, "combinegraphics_texture", "beijing", false);
        imgBG.rectTransform.sizeDelta = new Vector2(1280f, 800f);
        chuang = UguiMaker.newRawImage("chuang", transform, "combinegraphics_texture", "chuang", false);
        chuang.rectTransform.sizeDelta = new Vector2(1280f, 800f);
    }
    void Start ()
    {
        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        mSoundCtrl.PlayBGSound("bgmusic_loop0", "bgmusic_loop0", 0.2f);
        TopTitleCtl.instance.Reset();
        KbadyCtl.Init();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);
        //checkBtn
        imgbtn = UguiMaker.newImage("btnCheck", transform, "combinegraphics_sprite", "btnup");
        imgbtn.transform.localPosition = new Vector3(540f, -190f, 0f);
        Button btn = imgbtn.gameObject.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        EventTriggerListener.Get(imgbtn.gameObject).onDown = ClickDown;
        EventTriggerListener.Get(imgbtn.gameObject).onUp = ClickUp;
        //left
        mLeft = UguiMaker.newGameObject("mLeft", transform);
        mLeft.transform.localPosition = new Vector3(-300f, 45f, 0f);
        imgLeft = UguiMaker.newImage("imgLeft", mLeft.transform, "combinegraphics_sprite", "rect1", false);
        imgLeft.type = Image.Type.Sliced;
        imgLeft.rectTransform.sizeDelta = new Vector2(300f, 300f);
        imgRealPic = UguiMaker.newImage("realPic", mLeft.transform, "combinegraphics_sprite", "fish", false);
        //right
        mRight = UguiMaker.newGameObject("mRight", transform);
        imgRight = UguiMaker.newImage("imgRight", mRight.transform, "combinegraphics_sprite", "rect1", false);
        imgRight.type = Image.Type.Sliced;
        imgRight.rectTransform.sizeDelta = new Vector2(580f, 570f);
        imgRight.transform.localPosition = new Vector3(155f, 55f, 0f);
        BoxCollider2D box2DRight = imgRight.gameObject.AddComponent<BoxCollider2D>();
        box2DRight.size = imgRight.rectTransform.sizeDelta;
        mBlockTrans = UguiMaker.newGameObject("mBlockTrans", mRight.transform);

        StartCoroutine(ieLoadSmoke());
        StartCoroutine(InitSnow());

        List<int> objLists = Common.GetIDList(1, 15, 3, -1);
        mObjIDs = objLists.ToArray();

        fScale = 1.8f;
        nLevel = 1;
        InitLevelData();
    }

    IEnumerator ieLoadSmoke()
    {
        yield return new WaitForSeconds(0.5f);
        smoke1 = ResManager.GetPrefab("combinegraphics_prefab", "smoke").GetComponent<ParticleSystem>();
        smoke1.transform.SetParent(mLeft.transform);
        smoke1.transform.localPosition = Vector3.zero;
        smoke1.transform.localScale = Vector3.one;
        smoke2 = ResManager.GetPrefab("combinegraphics_prefab", "smoke").GetComponent<ParticleSystem>();
        smoke2.transform.SetParent(mRight.transform);
        smoke2.transform.localPosition = Vector3.zero;
        smoke2.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 重置信息
    /// </summary>
    public void ResetInfos()
    {
        bLvPass = false;
        bPlayOtherTip = false;
        bCanDrop = false;
        inImgRightCount = 0;
        if (mTheObjCtrl != null)
        {
            GameObject.Destroy(mTheObjCtrl.gameObject);
            mTheObjCtrl = null;
        }
        Common.DestroyChilds(mBlockTrans.transform);
        mBlockList.Clear();

        if (theChangeShow != null)
        {
            StopCoroutine(theChangeShow);
        }

        imgbtn.gameObject.SetActive(false);

        if (imgRealRight != null)
        { if (imgRealRight.gameObject != null)
            { GameObject.Destroy(imgRealRight.gameObject); }
        }
        imgRealRight = null;
    }


    public int ObjIDID = 1;
    public float fstartX = 500f;
    public float fXXhehe = 30f;
    /// <summary>
    /// 初始化关卡
    /// </summary>
    public void InitLevelData()
    {
        ResetInfos();

        bool bShowWrongBlock = false;
        int nWrongCount = 0;
        if (nLevel == 2)
        {
            bShowWrongBlock = true;
            nWrongCount = 1;//UnityEngine.Random.Range(1, 3);
        }

        //取得Obj信息
        int nobjid = mObjIDs[nLevel - 1];//UnityEngine.Random.Range(1, 16);
        mDataInfos.SetObjDate(nobjid);
        string strObjName = mDataInfos.strName;
        fCheckMaxDis = mDataInfos.fCheckMaxDis;
        List<int> mindexList = mDataInfos.mBlockIndexList;
        List<int> midList = mDataInfos.mBlockIDList;
        //真实图
        imgRealPic.sprite = ResManager.GetSprite("combinegraphics_sprite", strObjName);
        imgRealPic.SetNativeSize();
        imgRealPic.sprite = ResManager.GetSprite("combinegraphics_sprite", strObjName + "real");
        //imgRealPic.color = Color.gray;
        //创建target Obj
        GameObject mgo = ResManager.GetPrefab("combinegraphics_prefab", strObjName, mLeft.transform);
        mTheObjCtrl = mgo.AddComponent<CG_TheObj>();
        mTheObjCtrl.InitAwake(strObjName, mindexList, midList);
        //创建blocks
        for (int i = 0; i < mindexList.Count; i++)
        {
            CG_Block mmgoCtrl = CreateBlock(strObjName, mindexList[i], midList[i]);
            mmgoCtrl.transform.localScale = Vector3.one * fStartScale;
            mBlockList.Add(mmgoCtrl);
        }
        //创建wrongBlock
        if (bShowWrongBlock)
        {
            List<int> mwrongList = mDataInfos.mWrongIDList;
            for (int i = 0; i < nWrongCount; i++)
            {
                //id 为-1
                CG_Block mmgoCtrl = CreateBlock(strObjName + "wrong", mwrongList[i], -1);
                mmgoCtrl.transform.localScale = Vector3.one * fStartScale;
                mBlockList.Add(mmgoCtrl);
            }
        }
        //统一block位置设置
        List<CG_Block> nowBlockList = new List<CG_Block>();
        nowBlockList.AddRange(mBlockList);
        nowBlockList = Common.BreakRank(nowBlockList);

        fstartX = mDataInfos.fStartX;
        fXXhehe = mDataInfos.fJianju;

        for (int i = 0; i < nowBlockList.Count; i++)
        {
            float fsizeX = nowBlockList[i].GetBoxSize().x * fStartScale;
            fstartX -= fsizeX * 0.5f;
            nowBlockList[i].transform.localPosition = new Vector3(fstartX, -300f, 0f);
            nowBlockList[i].SetStartPos(new Vector3(fstartX, -300f, 0f));
            fstartX -= fsizeX * 0.5f + fXXhehe;
        }

        nToCount = mindexList.Count;

        //start state
        mTheObjCtrl.gameObject.SetActive(false);
        imgRealPic.gameObject.SetActive(false);
        mLeft.transform.localPosition = new Vector3(0f, 45f, 0f);
        mLeft.transform.localScale = Vector3.one * 0.001f;

        mRight.transform.localPosition = Vector3.zero;
        imgRight.gameObject.SetActive(false);
        imgRight.transform.localScale = Vector3.one * 0.001f;
        mBlockTrans.transform.localPosition = new Vector3(0f, -300f, 0f);

        StartCoroutine(ieSceneMove());
    }
    /// <summary>
    /// 进场/退场
    /// </summary>
    /// <param name="_in"></param>
    public void SceneMove(bool _in)
    {
        if (_in)
        {
        }
        else
        {
            if (sceneOutCP == null)
                sceneOutCP = Resources.Load<AudioClip>("sound/素材出去通用");
            mSoundCtrl.PlaySortSound(sceneOutCP);
            ShowCheckBtn(false);
            mLeft.transform.DOLocalMoveX(-1000f, 1f);
            mRight.transform.DOLocalMoveX(1300f, 1f);
        }
    }
    /// <summary>
    /// 下一关
    /// </summary>
    public void LevelCheckNext()
    {
        if (theChangeShow != null)
        { StopCoroutine(theChangeShow); }
        Debug.Log("level pass");
        bLvPass = true;
        StartCoroutine(IETOver());
    }
    IEnumerator IETOver()
    {
        mSoundCtrl.StopTipSound();
        bPlayOtherTip = true;

        //雾..
        mSoundCtrl.PlaySortSound(ResManager.GetClip("combinegraphics_sound", "fxsound"));
        smoke1.Play();
        smoke2.transform.localPosition = GetCenterPos();
        smoke2.Play();      
        mTheObjCtrl.gameObject.SetActive(true);
        imgRealPic.gameObject.SetActive(false);
        imgRealRight = UguiMaker.newImage("realRight", mBlockTrans.transform, "combinegraphics_sprite", mDataInfos.strName + "real", false);
        imgRealRight.transform.localPosition = GetCenterPos();
        imgRealRight.transform.localScale = Vector3.one * 1.1f;
        imgRealRight.color = new Color(1f, 1f, 1f, 0f);
        for (int i = 0; i < mBlockList.Count; i++)
        {
            mBlockList[i].HideByColor();
        }
        yield return new WaitForSeconds(1f);
        DOTween.To(() => imgRealRight.color, x => imgRealRight.color = x, new Color(1f, 1f, 1f, 1f), 1f);
        yield return new WaitForSeconds(1.5f);

        string cpName = "哇有点厉害哦";
        if (UnityEngine.Random.value > 0.5f)
        { cpName = "这么快就完成了组合图形真是不可思议"; }
        AudioClip cp = ResManager.GetClip("combinegraphics_sound", cpName);
        SoundCtrl.PlaySound(cp, 1f);

        yield return new WaitForSeconds(cp.length + 0.5f);
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

        AudioClip cp = ResManager.GetClip("combinegraphics_sound", "请你仔细观察左边的组合图形");
        SoundCtrl.PlaySound(cp, 1f);
    }

    public void PlayFaileSound(int _okCount)
    {
        string cpName = "要仔细观察左边的图片哦";
        if (_okCount >= 3)
        {
            if (UnityEngine.Random.value > 0.5f)
            {
                cpName = "就差一点点了加油";
            }
        }        
        AudioClip cp = ResManager.GetClip("combinegraphics_sound", cpName);
        SoundCtrl.PlaySound(cp, 1f);
    }
    #endregion



    IEnumerator ieSceneMove()
    {
        mLeft.transform.DOScale(Vector3.one * 1.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        mTheObjCtrl.gameObject.SetActive(true);
        bChangeSound = true;
        //实物交替出现
        StartChangeShow();
        yield return new WaitForSeconds(6f);
        //缩小靠右边
        mLeft.transform.DOScale(Vector3.one, 1f);
        mLeft.transform.DOLocalMoveX(-300f, 1f);
        //右边白色框显示
        imgRight.gameObject.SetActive(true);
        imgRight.transform.DOScale(Vector3.one, 1f);
        mBlockTrans.transform.DOLocalMove(Vector3.zero, 1f);
        yield return new WaitForSeconds(1);
        bChangeSound = false;

        GuideShow();

        bCanDrop = true;
        //播放语音...
        PlayTipSound();
    }

    void StartChangeShow()
    {
        theChangeShow = ieChangeShow();
        StartCoroutine(theChangeShow);
    }
    IEnumerator theChangeShow = null;
    IEnumerator ieChangeShow()
    {
        mTheObjCtrl.gameObject.SetActive(true);
        imgRealPic.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        if (bChangeSound)
            mSoundCtrl.PlaySortSound(ResManager.GetClip("combinegraphics_sound", "change"));
        mTheObjCtrl.gameObject.SetActive(false);
        imgRealPic.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        if (bChangeSound)
            mSoundCtrl.PlaySortSound(ResManager.GetClip("combinegraphics_sound", "change"));
        StartChangeShow();
    }
    bool bChangeSound = false;
    void StopChangeShow()
    {
        if (theChangeShow != null)
        { StopCoroutine(theChangeShow); }
        mTheObjCtrl.gameObject.SetActive(false);
        imgRealPic.gameObject.SetActive(true);
    }

    /// <summary>
    /// 按钮显示/隐藏
    /// </summary>
    /// <param name="_show"></param>
    public void ShowCheckBtn(bool _show)
    {
        if (_show)
        {
            imgbtn.sprite = ResManager.GetSprite("combinegraphics_sprite", "btnup");
            imgbtn.transform.localScale = Vector3.one * 0.001f;
            imgbtn.gameObject.SetActive(true);
            imgbtn.transform.DOScale(Vector3.one, 0.4f);
        }
        else
        {
            imgbtn.transform.DOScale(Vector3.one * 0.001f, 0.4f).OnComplete(()=> 
            {
                imgbtn.gameObject.SetActive(false);
            });
        }
    }
    private void ClickDown(GameObject _go)
    {
        if (bLvPass)
            return;
        if (button_down == null)
        { button_down = Resources.Load<AudioClip>("sound/button_down"); }
        mSoundCtrl.PlaySortSound(button_down);
        imgbtn.sprite = ResManager.GetSprite("combinegraphics_sprite", "btndown");
    }
    //点击检查通关
    private void ClickUp(GameObject _go)
    {
        if (bLvPass)
            return;
        int count = 0;
        Vector3 vblockBase = mBlockList[0].transform.localPosition;
        for (int i = 0; i < mTheObjCtrl.mStationList.Count; i++)
        {
            //mBlockList[i].transform.localScale = Vector3.one;

            CG_BlockStation cgSt = mTheObjCtrl.mStationList[i];
            Vector3 voffset = cgSt.transform.localPosition - mTheObjCtrl.mStationList[0].transform.localPosition;     
            Vector3 vReal = vblockBase + voffset * fScale;

            float frealDis = Vector3.Distance(cgSt.transform.localPosition, mTheObjCtrl.mStationList[0].transform.localPosition);
            frealDis = frealDis * fScale;

            CG_Block okBlock = CheckHaveSameIDIsOK(cgSt.nID, vReal);
            if (okBlock != null)
            {
                okBlock.SetColor(Color.green);
                okBlock.bOK = true;
                count++;
            }
            else
            {
                for (int j = 0; j < mBlockList.Count; j++)
                {
                    if (!mBlockList[j].bOK)
                    {
                        mBlockList[j].SetColor(Color.white);
                    }
                }
            }
        }
        if (count >= nToCount && nToCount > 0 && (nWrongInCount <= 0))
        {
            LevelCheckNext();
            mSoundCtrl.PlaySortSound(ResManager.GetClip("combinegraphics_sound", "click_suc"));
        }
        else
        {
            imgbtn.sprite = ResManager.GetSprite("combinegraphics_sprite", "btnup");
            PlayFaileSound(count);
            if (button_up == null)
            { button_up = Resources.Load<AudioClip>("sound/button_up"); }
            mSoundCtrl.PlaySortSound(button_up);
        }
    }
    AudioClip button_up;
    AudioClip button_down;
    /// <summary>
    /// 找到id相同的来检测距离是否在范围内
    /// </summary>
    /// <param name="_id">id</param>
    /// <param name="_real">0偏差显示的位置</param>
    /// <returns></returns>
    public CG_Block CheckHaveSameIDIsOK(int _id, Vector3 _real)
    {
        for (int i = 0; i < mBlockList.Count; i++)
        {
            if (mBlockList[i].nID == _id)
            {
                Vector3 vNow = mBlockList[i].transform.localPosition;
                float fDis = Vector3.Distance(_real, vNow);
                //Debug.Log(mBlockList[i].gameObject.name + ":" + fDis);
                if (fDis <= fCheckMaxDis)
                {
                    //Debug.Log("dis:" + mBlockList[i].gameObject.name +" " + fDis);
                    return mBlockList[i];
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 创建Block
    /// </summary>
    public CG_Block CreateBlock(string _name,int _indexID, int _id)
    {
        GameObject mmgo = UguiMaker.newGameObject(_name + _indexID.ToString(), mBlockTrans.transform);
        mmgo.transform.localScale = Vector3.one * fScale;
        CG_Block mmgoCtrl = mmgo.AddComponent<CG_Block>();
        mmgoCtrl.InitAwake(_name, _indexID, _id);
        return mmgoCtrl;
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitLevelData();
        }
        if (bCanDrop)
            MUpdate();
    }
    public int nWrongInCount = 0;
    CG_Block mSelect;
    Vector3 temp_select_offset;
    private void MUpdate()
    {
        if (bLvPass)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            mBlockTrans.transform.SetSiblingIndex(5);
            bool bHitRightRect = false;
            #region//one
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            for (int i = 0; i < hits.Length; i++)
            {
                CG_Block com = hits[i].collider.gameObject.GetComponent<CG_Block>();
                if (com != null)
                {
                    if (mSelect == null)
                    { mSelect = com; }
                    else if (com.transform.GetSiblingIndex() > mSelect.transform.GetSiblingIndex())
                    { mSelect = com; }
                }
                if (hits[i].collider.gameObject == imgRight.gameObject)
                {
                    bHitRightRect = true;
                }
            }
            if (mSelect != null)
            {
                mSelect.bOK = false;
                mSelect.SetColor(Color.white);
                RectTransform retf = mSelect.transform as RectTransform;
                temp_select_offset = Common.getMouseLocalPos(transform) - retf.anchoredPosition3D;
                mSelect.transform.SetSiblingIndex(50);

                for (int j = 0; j < mBlockList.Count; j++)
                {
                    mBlockList[j].bOK = false;
                }
                if (bHitRightRect)
                {
                    inImgRightCount--;
                    if (inImgRightCount < 0)
                        inImgRightCount = 0;

                    if (mSelect.nID == -1)
                    {
                        nWrongInCount--;
                    }
                }
                GuideHide();

                mSelect.transform.DOScale(Vector3.one * fScale, 0.15f);
                //mSelect.transform.localScale = Vector3.one * fScale;
                mSoundCtrl.PlaySortSound(ResManager.GetClip("combinegraphics_sound", "click_item"));
            }
            #endregion
        }
        else if (Input.GetMouseButton(0))
        {
            #region//动物位置设置
            if (mSelect != null)
            {
                //拖动值限制
                Vector3 vInput = Common.getMouseLocalPos(transform) - temp_select_offset;
                float fX = vInput.x;
                float fY = vInput.y;               
                Vector3 vsize = mSelect.GetBoxSize();
                fX = Mathf.Clamp(fX, -GlobalParam.screen_width * 0.5f + vsize.x * 0.5f, GlobalParam.screen_width * 0.5f - vsize.x * 0.5f);
                fY = Mathf.Clamp(fY, -GlobalParam.screen_height * 0.5f + vsize.y * 0.5f, GlobalParam.screen_height * 0.5f - vsize.y * 0.5f - 60f);
                mSelect.transform.localPosition = new Vector3(fX, fY, 0f);
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (mSelect != null)
            {
                mSelect.transform.SetSiblingIndex(mSelect.nIndex);
                if (mSelect.nID == -1)
                {
                    mSelect.transform.SetSiblingIndex(50);
                }

                bool bhitImgRight = false;
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                for (int i = 0; i < hits.Length; i++)
                {
                    GameObject hitObj = hits[i].collider.gameObject;
                    if (hitObj == imgRight.gameObject)
                    {
                        bhitImgRight = true;
                        break;
                    }
                }
                if (bhitImgRight)
                {
                    mSoundCtrl.PlaySortSound(ResManager.GetClip("combinegraphics_sound", "dropdown"));

                    inImgRightCount++;
                    if (inImgRightCount > mBlockList.Count)
                    { inImgRightCount = mBlockList.Count; }

                    if (mSelect.nID == -1)
                    {
                        nWrongInCount++;
                    }            
                }
                else
                {
                    mSelect.BlackToStart();
                }

                //按钮显示/隐藏
                if (inImgRightCount > 0)
                {
                    if (!imgbtn.gameObject.activeSelf)
                    {
                        ShowCheckBtn(true);
                    }
                }
                else
                {
                    if (imgbtn.gameObject.activeSelf)
                    {
                        ShowCheckBtn(false);
                    }
                }
            }
            mSelect = null;
        }
    }



    #region//指引---
    private GuideHandCtl mGuideHand;
    bool bOnceDropTip = false;
    /// <summary>
    /// 指引显示
    /// </summary>
    public void GuideShow()
    {
        if (bOnceDropTip)
            return;
        if (mGuideHand == null)
        {
            mGuideHand = GuideHandCtl.Create(transform);
            Vector3 vfrom = mBlockList[3].transform.position;
            Vector3 vto = imgRight.transform.position;
            mGuideHand.GuideTipDrag(vfrom, vto, -1, 1f, "hand1");
            mGuideHand.SetDragDate(new Vector3(0f, -26f, 0f), 1f);
            bOnceDropTip = true;
        }
    }
    /// <summary>
    /// 关闭指引
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

    /// <summary>
    /// 平均值点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCenterPos()
    {
        Vector3 vget = Vector3.zero;
        int ngetcount = 0;
        for (int i = 0; i < mBlockList.Count; i++)
        {
            if (mBlockList[i].nID != -1)
            {
                vget = vget + mBlockList[i].transform.localPosition;
                ngetcount++;
            }
        }
        vget = vget / ngetcount;
        return vget;
    }

    #region//snow sys
    private CG_Snow[] imgSnow = new CG_Snow[5];
    public IEnumerator InitSnow()
    {
        for (int i = 0; i < 5; i++)
        {
            imgSnow[i] = UguiMaker.newGameObject("snow", imgBG.transform).AddComponent<CG_Snow>();
            imgSnow[i].InitAwake();
            if (i >= 2)
            {
                imgSnow[i].isFarSnow = true;
            }
            imgSnow[i].PlaySnow();
            yield return new WaitForSeconds(UnityEngine.Random.Range(3.3f, 5.5f));
        }
    }

    #endregion


}
