using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class CFishLevel2 : MonoBehaviour
{

    private CatchFish mCtrl;

    private float fWidth = 0;
    public int nCount = 0;
    public int nToCount = 0;

    public GameObject mparent;
    private Vector3 vparent;
    private CFishNumObj mheadNumber;
    private Image imgfen;
    private Button mCheckBtn;

    public List<CFishNumObj> mNumStationList = new List<CFishNumObj>();
    private List<CFishNumObj> mNumDownList = new List<CFishNumObj>();

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as CatchFish;
        mCheckBtn = mCtrl.mCheckBtn;
    }

    public void ResetInfo()
    {
        for (int i = 0; i < mNumDownList.Count; i++)
        {
            if (mNumDownList[i].gameObject != null)
                GameObject.Destroy(mNumDownList[i].gameObject);
        }
        mNumDownList.Clear();

        fWidth = 0;

        if (mparent != null)
        { GameObject.Destroy(mparent); }
        mparent = null;
        mheadNumber = null;
        imgfen = null;
        mNumStationList.Clear();
    }

    public void SetData(int _num)
    {
        ResetInfo();

        //reset btn
        mCtrl.SetCheckBtnUp(CheckBtnUp);
        mCtrl.SetCheckBtnSprite("btnup");
        mCheckBtn.transform.localPosition = new Vector3(0f, -100f, 0f);
        mCheckBtn.gameObject.SetActive(false);

        //parent
        mparent = UguiMaker.newGameObject("mparent", transform);
        //head num
        mheadNumber = CreateNumber(0, mparent.transform, true);
        //fen hao
        imgfen = UguiMaker.newImage("imgfen", mparent.transform, "catchfish_sprite", "fen", false);


        nCount = 0;
        nToCount = (_num - 1) * 2;

        fWidth += mheadNumber.GetSize().x * 0.5f;
        fWidth += imgfen.rectTransform.sizeDelta.x * 0.5f;
        //set head num pos
        mheadNumber.transform.localPosition = Vector3.zero;
        imgfen.transform.localPosition = new Vector3(fWidth, 0f, 0f);
        fWidth += imgfen.rectTransform.sizeDelta.x * 0.5f;
        //set head num id
        mheadNumber.SetNumber(_num);
        //pos index
        List<float> getIndexX = Common.GetOrderList(_num - 1, 150f);
        fWidth += mheadNumber.GetSize().x * 0.5f;
        //create nums
        for (int i = 0; i < _num - 1; i++)
        {
            CFishNumObj numUp = CreateNumber(0, mparent.transform, true);
            CFishNumObj numDown = CreateNumber(0, mparent.transform, true);
            numUp.bUp = true;    
            numDown.bUp = false;
            mNumStationList.Add(numUp);
            mNumStationList.Add(numDown);
            //set pos
            numUp.transform.localPosition = new Vector3(fWidth, 53f, 0f);
            numDown.transform.localPosition = new Vector3(fWidth, -53f, 0f);
            //hite num
            numUp.NumberActive(false);
            numDown.NumberActive(false);
            //get indexX
            fWidth += numUp.GetSize().x;

            //create drops
            CFishNumObj numDrop = CreateNumber(i + 1,transform, false);
            numDrop.transform.localPosition = new Vector3(getIndexX[i], -300f, 0f);
            mNumDownList.Add(numDrop);
            numDrop.SetStartPos(new Vector3(getIndexX[i], -300f, 0f));
        }
        //set parent pos
        vparent = new Vector3((fWidth - mheadNumber.GetSize().x) * 0.5f * -1, 0f, 0f);
        mparent.transform.localPosition = vparent - new Vector3(60f, 0f, 0f);

        float fbtnx = fWidth + mheadNumber.GetSize().x * 0.5f + vparent.x - 30f;
        mCheckBtn.transform.localPosition = new Vector3(fbtnx, vparent.y, 0f);
    }

    /// <summary>
    /// 创建numObj
    /// </summary>
    private CFishNumObj CreateNumber(int _num,Transform _trans, bool _isStation = false)
    {
        CFishNumObj mmnumber = UguiMaker.newGameObject("Number" +_num, _trans).AddComponent<CFishNumObj>();
        mmnumber.InitAwake();
        mmnumber.bStation = _isStation;
        if (_num != 0)
            mmnumber.SetNumber(_num);
        return mmnumber;
    }

    void Update()
    {
        MUpdate();
    }
    Vector3 temp_select_offset = Vector3.zero;
    CFishNumObj mSelect = null;
    private void MUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            #region//one
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hits != null)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    CFishNumObj com = hits[i].collider.gameObject.GetComponent<CFishNumObj>();
                    if (com != null && !com.bStation)
                    {
                        mSelect = com;
                        mSelect.transform.SetSiblingIndex(50);
                        temp_select_offset = Common.getMouseLocalPos(transform) - com.transform.localPosition;
                        mSelect.SetScale(1.2f);

                        AudioClip cp = ResManager.GetClip("catchfish_sound", "to_setok");
                        mCtrl.mSoundCtrl.PlaySortSound(cp);
                    }
                }
            }
            #endregion
        }
        else if (Input.GetMouseButton(0))
        {
            #region//two
            if (mSelect != null)
            {
                //拖动值限制
                Vector3 vInput = Common.getMouseLocalPos(transform) - temp_select_offset;
                float fX = vInput.x;
                float fY = vInput.y;
                Vector2 vsize = mSelect.GetSize();
                fX = Mathf.Clamp(fX, -GlobalParam.screen_width * 0.5f + vsize.x * 0.5f, GlobalParam.screen_width * 0.5f - vsize.x * 0.5f);
                fY = Mathf.Clamp(fY, -GlobalParam.screen_height * 0.5f + vsize.y * 0.5f, GlobalParam.screen_height * 0.5f - vsize.y * 0.5f - 60f);
                mSelect.transform.localPosition = new Vector3(fX, fY, 0f);
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//two
            if (mSelect != null)
            {
                bool bMatch = false;
                CFishNumObj comstation = null;

                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hits != null)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        comstation = hits[i].collider.gameObject.GetComponent<CFishNumObj>();
                        if (comstation != null && comstation.bStation)
                        {
                            bMatch = true;
                            comstation.SetNumber(mSelect.nNum);
                            comstation.NumberActive(true);
                            //comstation.BoxActive(false);

                            mSelect.ReCreateNum();

                            DropInCheckShowBtn();

                            AudioClip cp = ResManager.GetClip("catchfish_sound", "to_dropin");
                            mCtrl.mSoundCtrl.PlaySortSound(cp);
                            break;
                        }
                    }
                }

                if (!bMatch)
                {
                    mSelect.ReCreateNum();
                }

                mSelect = null;
            }
            #endregion
        }
    }

    //计数显示按钮
    private void DropInCheckShowBtn()
    {
        //nCount++;
        //if (nCount >= nToCount && nToCount > 0)
        //{
        //    //mCheckBtn.gameObject.SetActive(true);
        //    mCtrl.CheckBtnShow(true);
        //}

        bool bShowBtn = true;
        for (int i = 0; i < mNumStationList.Count; i++)
        {
            if (mNumStationList[i].nNum == 0)
            {
                bShowBtn = false;
                break;
            }
        }
        if (bShowBtn)
        {
            //mCheckBtn.gameObject.SetActive(true);
            if (!mCheckBtn.gameObject.activeSelf)
                mCtrl.CheckBtnShow(true);
        }
    }

    //点击按钮抬起事件
    private void CheckBtnUp(GameObject _go)
    {
        List<CFishNumObj> unOKList = new List<CFishNumObj>();

        CFishLinefishsObj mTopPics = mCtrl.mTopLinePic;
        List<CFishFishpicObj> mFishPicObjList = mTopPics.mFishPicObjList;
        int nindex = 0;
        for (int i = 0; i < mFishPicObjList.Count; i++)
        {
            CFishFishpicObj thePicObj = mFishPicObjList[i];

            CFishNumObj upnum = mNumStationList[nindex];
            if (upnum.nNum != thePicObj.mLeftFishList.Count)
            {
                unOKList.Add(upnum);
            }

            nindex++;

            CFishNumObj downnum = mNumStationList[nindex];
            if (downnum.nNum != thePicObj.mRightFishList.Count)
            {
                unOKList.Add(downnum);
            }

            nindex++;
        }

        mCtrl.CheckBtnActive(false);

        if (unOKList.Count <= 0)
        {
            mCtrl.PlayCheckBtnEffect();
            //下一关
            mCtrl.LevelCheckNext();
        }
        else
        {          
            //有不匹配
            mCtrl.StartCoroutine(IeCheckFaile(unOKList));
            //faile sound
            mCtrl.StopTipSound();
            mCtrl.SetTipSound(ieSoundFaileTip());
            mCtrl.StartTipSound();
        }
    }

    IEnumerator IeCheckFaile(List<CFishNumObj> unOKList)
    {
        mCtrl.SetCheckBtnSprite("btnup");
        yield return new WaitForSeconds(0.3f);
        mCtrl.CheckBtnActive(true);
        //mCheckBtn.gameObject.SetActive(false);
        mCtrl.CheckBtnShow(false);
        //数字移出效果
        for (int i = 0; i < unOKList.Count; i++)
        {
            unOKList[i].RemoveOutEffect();
            nCount--;
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1f);
        for (int i = 0; i < unOKList.Count; i++)
        {
            unOKList[i].BoxActive(true);
        }
    }

    public void SceneMove(bool _in)
    {
        if (_in)
        {
            transform.localPosition = new Vector3(-1400f, 0f, 0f);
            transform.DOLocalMove(Vector3.zero, 1f);
        }
        else
        {
            //transform.DOLocalMove(new Vector3(1400f, 0f, 0f), 1f);
            for (int i = 0; i < mNumDownList.Count; i++)
            {
                mNumDownList[i].transform.DOLocalMoveY(-550f, 1f);
            }
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
        cpList.Add(ResManager.GetClip("catchfish_sound", "game-tips_lv2_0"));
        cpList.Add(ResManager.GetClip("number_sound", mCtrl.nNum.ToString()));
        cpList.Add(ResManager.GetClip("catchfish_sound", "game-tips_lv2_1"));
        cpList.Add(ResManager.GetClip("catchfish_sound", "game-tips_lv2_2"));
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
        AudioClip cp = ResManager.GetClip("catchfish_sound", "game-tips_lv2_faile1");
        mCtrl.mSoundCtrl.PlaySound(cp, 1f);
    }

}
