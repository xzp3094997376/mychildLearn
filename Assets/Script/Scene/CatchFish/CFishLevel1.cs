using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// 关卡1
/// </summary>
public class CFishLevel1 : MonoBehaviour
{
    private CatchFish mCtrl;
    private int nNum = 0;

    public int nCount = 0;
    public int nToCount = 0;
    public List<CFishFishObj> mFishObjList = new List<CFishFishObj>();

    private Button mCheckBtn;

    //left box
    public CFlishFishBoxObj mLeftBox;
    //right box
    public CFlishFishBoxObj mRightBox;

    private bool bCheckFishOK = false;

    /// <summary>
    /// 点击指引鱼
    /// </summary>
    public CFishFishObj mGuideFish = null;


    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as CatchFish;
        mCheckBtn = mCtrl.mCheckBtn;
        //box
        mLeftBox = UguiMaker.newGameObject("fishBoxLeft", transform).AddComponent<CFlishFishBoxObj>();
        mRightBox = UguiMaker.newGameObject("fishBoxRight", transform).AddComponent<CFlishFishBoxObj>();      
        mLeftBox.transform.localPosition = new Vector3(-300f, -30f, 0f);
        mRightBox.transform.localPosition = new Vector3(300f, -30f, 0f);
        mLeftBox.InitAwake(1);
        mRightBox.InitAwake(2);
    }

    public void ResetInfos()
    {
        for (int i = 0; i < mFishObjList.Count; i++)
        {
            if (mFishObjList[i].gameObject != null)
                GameObject.Destroy(mFishObjList[i].gameObject);
        }
        mFishObjList.Clear();

        mCheckBtn.gameObject.SetActive(false);

        nCount = 0;
        nToCount = 0;
        nNum = 0;
    }

    public void SetData(int _num)
    {
        ResetInfos();

        mCtrl.SetCheckBtnUp(CheckBtnUp);
        mCtrl.SetCheckBtnSprite("btnup");
        mCheckBtn.transform.localPosition = new Vector3(0f, -100f, 0f);
        mCheckBtn.gameObject.SetActive(false);

        nNum = _num;
        nCount = 0;
        nToCount = nNum - 1;

        //create fish
        CreateFish();
    }

    /// <summary>
    /// 在down water创建鱼
    /// </summary>
    public void CreateFish()
    {
        bCheckFishOK = false;

        mLeftBox.NumberTipReset();
        mRightBox.NumberTipReset();
        mLeftBox.NumberTipActive(true);
        mRightBox.NumberTipActive(true);

        Image img = mCheckBtn.GetComponent<Image>();
        img.sprite = ResManager.GetSprite("catchfish_sprite", "btnup");
        //mCheckBtn.gameObject.SetActive(false);
        mCtrl.CheckBtnShow(false);

        mFishObjList.Clear();
        mLeftBox.mFishList.Clear();
        mRightBox.mFishList.Clear();

        mCtrl.StartCoroutine(IECreateFish());
    }
    IEnumerator IECreateFish()
    {
        int fishtype = Random.Range(0, 4);
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < nNum; i++)
        {
            Vector3 vfishpos = new Vector3(Random.Range(-250f, 250f), Random.Range(-40f, 20f), 0f);
            CFishFishObj fishobj = mCtrl.CreateFishObj(fishtype, mCtrl.mFishDownParent.transform, vfishpos);
            fishobj.transform.localScale = Vector3.one * 0.001f;
            mFishObjList.Add(fishobj);
            fishobj.SceneMoveIn();
            yield return new WaitForSeconds(0.2f);
        }
        mCtrl.CheckBtnActive(true);
        if (bfirsttimes)
        {
            //鱼点击指引创建
            mGuideFish = mFishObjList[nNum - 1];
            mGuideFish.CreateGuideObj();
            //设置鱼缸指引创建 回调
            mGuideFish.SetNextGuideCallback(GuideFishBoxClick);
            bfirsttimes = false;
        }
    }
    bool bfirsttimes = true;
    


    void Update()
    {
        //成功捕鱼后不能再捕鱼
        if (!mCtrl.bCatchFishOK)
        {
            if (!mCtrl.bCatchState)
                CheckCatchFish();
        }
        else
        {
            CheckFishToFishBox();
        }
        
    }
    Vector3 temp_select_offset = Vector3.zero;
    CFishFishObj mSelect = null;
    //点击捕鱼
    private void CheckCatchFish()
    {
        if (Input.GetMouseButtonDown(0))
        {
            #region//one
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hits != null)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    CFishFishObj com = hits[i].collider.gameObject.GetComponent<CFishFishObj>();
                    if (com != null)
                    {
                        if (mSelect == null)
                        { mSelect = com; }
                        else
                        {
                            if (com.transform.GetSiblingIndex() > mSelect.transform.GetSiblingIndex())
                            {
                                mSelect = com;
                            }
                        }
                    }
                }
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//two
            if (mSelect != null)
            {
                //mSelect.transform.SetSiblingIndex(20);
                temp_select_offset = Common.getMouseLocalPos(transform) - mSelect.transform.localPosition;
                mSelect.StopMove();
                //捕鱼
                Transform transFishParent = mCtrl.GetFishStation(mSelect);
                mCtrl.CatchFishByNet(transFishParent, mSelect);

                if (mGuideFish != null)
                {
                    mGuideFish.StopGuideHand();
                }

                //如果鱼在鱼缸,移出鱼
                if (mSelect.nInStation != 0)
                {
                    Transform yugangT = mCtrl.GetFishStation(mSelect);
                    CFlishFishBoxObj _fishbox = yugangT.GetComponent<CFlishFishBoxObj>();
                    _fishbox.RemoveOutFish(mSelect);
                }
            }
            #endregion
        }
    }

    CFlishFishBoxObj mSelectFishBox = null;
    //点击鱼缸加鱼
    private void CheckFishToFishBox()
    {
        if (Input.GetMouseButtonDown(0) && mSelect != null)
        {
            #region//one
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hits != null)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    CFlishFishBoxObj com = hits[i].collider.gameObject.GetComponent<CFlishFishBoxObj>();
                    if (com != null)
                    {
                        mSelectFishBox = com; 
                    }
                }
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0) && mSelect != null)
        {
            #region//two
            if (mSelectFishBox != null)
            {
                //不能全在一个鱼缸里
                if (mSelectFishBox.mFishList.Count >= (nNum - 1))
                {
                    mSelectFishBox.ShakeObj();
                }
                else
                {
                    mCtrl.AddToFishBox(mSelect, mSelectFishBox);
                    mSelect = null;
                    mSelectFishBox = null;
                }
            }
            #endregion
        }
    }

    int theSameCountCheck = 0;
    private void CheckBtnUp(GameObject _go)
    {
        if (bCheckFishOK)
            return;

        int ncountL = mLeftBox.mFishList.Count;
        int ncountR = mRightBox.mFishList.Count;
        if (ncountL > 0 && ncountR > 0 && (ncountL + ncountR == nNum))
        {
            theSameCountCheck = 0;
            bool checkinok = mCtrl.mTopLinePic.ToCheckInFish(mLeftBox.mFishList, mRightBox.mFishList, out theSameCountCheck);
            if (checkinok)
            {
                mCtrl.PlayCheckBtnEffect();
                bCheckFishOK = true;
                mCtrl.CheckBtnActive(false);
                mCtrl.StartCoroutine(IEToNext());
            }
            else
            {
                AudioClip cp = ResManager.GetClip("checkgamebtn_sound", "checkgamebtn_up");
                mCtrl.mSoundCtrl.PlaySortSound(cp);
                mCtrl.SetCheckBtnSprite("btnup");
                //check faile sound tip
                mCtrl.StopTipSound();
                mCtrl.SetTipSound(ieSoundFaileTip());
                mCtrl.StartTipSound();
            }
        }
        else
        {
            //AudioClip cp = ResManager.GetClip("checkgamebtn_sound", "checkgamebtn_up");
            //mCtrl.mSoundCtrl.PlaySortSound(cp);
            //mCtrl.SetCheckBtnSprite("btnup");
            mCtrl.CheckBtnShow(false);
        }
    }
    IEnumerator IEToNext()
    {
        nCount++;

        mCtrl.bPlayOtherTip = true;
        mCtrl.StopTipSound();
        AudioClip cpGood = ResManager.GetClip("catchfish_sound", "game-tips_suc" + Random.Range(0, 2));
        mCtrl.mSoundCtrl.PlaySound(cpGood, 1f);
        yield return new WaitForSeconds(cpGood.length);
        mCtrl.bPlayOtherTip = false;

        if (nCount >= nToCount && nToCount > 0)
        {
            mLeftBox.NumberTipActive(false);
            mRightBox.NumberTipActive(false);
            mCtrl.LevelCheckNext();
        }
        else
        {
            CreateFish();
        }
    }

    /// <summary>
    /// 检测显示按钮
    /// </summary>
    public void CheckShowBtn()
    {
        int fishcount = mLeftBox.mFishList.Count + mRightBox.mFishList.Count;
        if (fishcount >= nNum)
        {
            //mCheckBtn.gameObject.SetActive(true);
            mCtrl.CheckBtnShow(true);
        }
    }

    public void SceneMove(bool _in)
    {
        //mLeftBox.transform.localPosition = new Vector3(-300f, -30f, 0f);
        //mRightBox.transform.localPosition = new Vector3(300f, -30f, 0f);
        if (_in)
        {
            mLeftBox.transform.localPosition = new Vector3(-1000f, -30f, 0f);
            mRightBox.transform.localPosition = new Vector3(1000f, -30f, 0f);
            mLeftBox.transform.DOLocalMove(new Vector3(-300f, -30f, 0f), 1f);
            mRightBox.transform.DOLocalMove(new Vector3(300f, -30f, 0f), 1f);
        }
        else
        {
            mLeftBox.transform.DOLocalMove(new Vector3(-1000f, -30f, 0f), 1f);
            mRightBox.transform.DOLocalMove(new Vector3(1000f, -30f, 0f), 1f);
        }
    }

    /// <summary>
    /// 玩法提示语音
    /// </summary>
    /// <returns></returns>
    public IEnumerator ieSoundTip()
    {
        yield return new WaitForSeconds(0.1f);
        List<AudioClip> cpList = new List<AudioClip>();
        cpList.Add(ResManager.GetClip("catchfish_sound", "game-tips_lv1_0"));
        cpList.Add(ResManager.GetClip("number_sound", nNum.ToString()));
        cpList.Add(ResManager.GetClip("catchfish_sound", "game-tips_lv1_1"));
        cpList.Add(ResManager.GetClip("catchfish_sound", "game-tips_lv1_2"));
        int ncout = nNum - 1;
        cpList.Add(ResManager.GetClip("number_sound", ncout.ToString()));
        cpList.Add(ResManager.GetClip("catchfish_sound", "game-tips_lv1_3"));
        for (int i = 0; i < cpList.Count; i++)
        {
            mCtrl.mSoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
    }

    /// <summary>
    /// 不匹配语音
    /// </summary>
    /// <returns></returns>
    public IEnumerator ieSoundFaileTip()
    {
        yield return new WaitForSeconds(0.1f);
        AudioClip cp = null;
        if (theSameCountCheck > 0)
        {
            cp = ResManager.GetClip("catchfish_sound", "game-tips_lv1_faile1");
        }
        else
        {
            cp = ResManager.GetClip("catchfish_sound", "game-tips_lv1_faile");
        }  
        mCtrl.mSoundCtrl.PlaySound(cp, 1f);
    }


    /// <summary>
    /// 鱼缸点击
    /// </summary>
    public void GuideFishBoxClick()
    {
        mFishBoxClickGuide = GuideHandCtl.Create(mLeftBox.transform);
        mFishBoxClickGuide.GuideTipClick(0.8f, 0.7f, true, true, "hand1");
        mFishBoxClickGuide.SetClickTipOffsetPos(new Vector3(8f, -25f, 0f));
    }
    GuideHandCtl mFishBoxClickGuide;
    /// <summary>
    /// 停止鱼缸点击指引
    /// </summary>
    public void StopFishBoxClick()
    {
        if (mFishBoxClickGuide != null)
        {
            mFishBoxClickGuide.StopClick();
            if (mFishBoxClickGuide.gameObject != null)
                GameObject.Destroy(mFishBoxClickGuide.gameObject);
            mFishBoxClickGuide = null;
        }
    }

}
