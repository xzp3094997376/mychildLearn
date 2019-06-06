using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class CFishLevel3 : MonoBehaviour
{
    private CatchFish mCtrl;
    private Button mCheckBtn;
    private Image imgarrow;
    private GameObject mStart;
    private GameObject mEnd;

    public int nNum = 0;
    public List<CFishNumGroup> mNumGroupList = new List<CFishNumGroup>();

    public GameObject mparent;

    public bool blvpass = false;

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as CatchFish;
        mCheckBtn = mCtrl.mCheckBtn;

        imgarrow = UguiMaker.newImage("imgarrow", transform, "catchfish_sprite", "arrow", false);
        imgarrow.type = Image.Type.Sliced;
        imgarrow.gameObject.SetActive(false);

        mStart = UguiMaker.newGameObject("mstart", transform);
        mEnd = UguiMaker.newGameObject("mend", transform);
    }


    public void ResetInfos()
    {
        nNum = 0;
        if (mparent != null)
            GameObject.Destroy(mparent);
        mparent = null;
        mNumGroupList.Clear();
        blvpass = false;
    }

    public void SetData(int _num)
    {
        mCtrl.SetCheckBtnUp(this.CheckBtnUp);
        mCheckBtn.transform.localPosition = new Vector3(0f, -300f, 0f);
        mCtrl.CheckBtnShow(true);
        //mCheckBtn.gameObject.SetActive(true);
        mCtrl.SetCheckBtnSprite("btnup");
        mCtrl.CheckBtnActive(true);

        nNum = _num;
        //拿lv2的数据
        mparent = mCtrl.mLevel2.mparent;
        mparent.transform.SetParent(transform);
        List<CFishNumObj> mNumStationList = mCtrl.mLevel2.mNumStationList;

        int nindex = 0;
        //create numgroup
        for (int i = 0; i < _num - 1; i++)
        {
            CFishNumGroup numgroup = UguiMaker.newGameObject("numgroup"+i, mparent.transform).AddComponent<CFishNumGroup>();

            CFishNumObj leftnum = mNumStationList[nindex];
            nindex++;
            CFishNumObj rightnum = mNumStationList[nindex];

            numgroup.InitAwake(leftnum, rightnum);          
            mNumGroupList.Add(numgroup);

            nindex++;
        }

        imgarrow.transform.SetSiblingIndex(20);
    }


    CFishNumGroup mSelect;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            mCtrl.StartCoroutine(ieCheckLv3(true));
        }
        if (blvpass)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit2D.collider != null)
            {
                mSelect = hit2D.collider.gameObject.GetComponent<CFishNumGroup>();
                if (mSelect != null)
                {
                    if (endPosResetTween != null)
                        endPosResetTween.Pause();
                    imgarrow.gameObject.SetActive(true);
                    mStart.transform.position = mSelect.transform.position;
                    mEnd.transform.position = mSelect.transform.position;
                    SetArrowLine();
                    mSelect.ImgRectActive(true);
                    mSelect.transform.SetSiblingIndex(20);

                    AudioClip cp = ResManager.GetClip("catchfish_sound", "to_dropline");
                    mCtrl.mSoundCtrl.PlaySortSound(cp);
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (mSelect != null)
            {
                Vector3 vInput = Common.getMouseLocalPos(transform);
                mEnd.transform.localPosition = vInput;
                SetArrowLine();
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (mSelect != null)
            {
                bool bMatch = false;
                CFishNumGroup mstation = null;
                RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit2D.collider != null)
                {
                    mstation = hit2D.collider.gameObject.GetComponent<CFishNumGroup>();
                    if (mstation != null && mstation != mSelect)
                    {
                        bMatch = true;
                        mEnd.transform.position = mstation.transform.position;
                        SetArrowLine();
                        mstation.ImgRectActive(true);
                        imgarrow.gameObject.SetActive(false);
                        mstation.transform.SetSiblingIndex(20);
                        //change pos
                        ChangePos(mSelect, mstation);

                        AudioClip cp = ResManager.GetClip("catchfish_sound", "to_dropin");
                        mCtrl.mSoundCtrl.PlaySortSound(cp);
                    }
                }

                if (!bMatch)
                {
                    mSelect.ImgRectActive(false);
                    ResetEndPos();
                }

                mSelect = null;
            }
        }
    }

    Tween endPosResetTween = null;
    //line set
    private void SetArrowLine()
    {
        Vector3 vdir = mEnd.transform.localPosition - mStart.transform.localPosition;
        imgarrow.transform.localPosition = (mEnd.transform.localPosition + mStart.transform.localPosition) * 0.5f;

        vdir = vdir.normalized;
        imgarrow.transform.right = vdir;

        float flenght = Vector3.Distance(mEnd.transform.localPosition, mStart.transform.localPosition);
        imgarrow.rectTransform.sizeDelta = new Vector2(flenght, imgarrow.rectTransform.sizeDelta.y);
    } 
    //line reset
    private void ResetEndPos()
    {
        AudioClip cp = ResManager.GetClip("catchfish_sound", "to_lineback");
        mCtrl.mSoundCtrl.PlaySortSound(cp);

        endPosResetTween = mEnd.transform.DOLocalMove(mStart.transform.localPosition,0.25f).OnUpdate(() => 
        {
            SetArrowLine();
        });
    }


    /// <summary>
    /// 交换位置
    /// </summary>
    public void ChangePos(CFishNumGroup _numgroup1, CFishNumGroup _numgroup2)
    {
        //位置交换
        Vector3 vnum1 = _numgroup1.transform.localPosition;
        Vector3 vnum2 = _numgroup2.transform.localPosition;
        _numgroup1.ChangeToPos(vnum2);
        _numgroup2.ChangeToPos(vnum1);

        //列表index位置交换
        int nindex1 = -1;
        int nindex2 = -1;
        for (int i = 0; i < mNumGroupList.Count; i++)
        {
            if (mNumGroupList[i] == _numgroup1)
            {
                nindex1 = i;
            }
            if (mNumGroupList[i] == _numgroup2)
            {
                nindex2 = i;
            }
        }

        if (nindex1 >= 0 && nindex2 >= 0)
        {
            mNumGroupList[nindex1] = _numgroup2;
            mNumGroupList[nindex2] = _numgroup1;
            //Debug.Log("change ok");
        }
        else
        {
            //Debug.Log("change faile");
        }
    }


    //点击按钮抬起事件
    private void CheckBtnUp(GameObject _go)
    {
        bool checkOK = true;

        int ncheckID = 1;
        for (int i = 0; i < mNumGroupList.Count; i++)
        {
            if (mNumGroupList[i].mLeftNum.nNum == ncheckID)
            {
                ncheckID++;
            }
            else
            {
                checkOK = false;
                break;
            }
        }
        blvpass = checkOK;
        if (blvpass)
        {
            mCtrl.CheckBtnActive(false);
            mCtrl.PlayCheckBtnEffect();
        }
        mCtrl.StartCoroutine(ieCheckLv3(checkOK));
    }
    IEnumerator ieCheckLv3(bool _ok)
    {
        if (_ok)
        {
            mCtrl.bPlayOtherTip = true;
            List<AudioClip> cpList = new List<AudioClip>();
            cpList.Add(ResManager.GetClip("catchfish_sound", "game-tips_end0"));
            cpList.Add(ResManager.GetClip("catchfish_sound", "game-tips_end1"));
            cpList.Add(ResManager.GetClip("catchfish_sound", "game-tips_end2"));
            cpList.Add(ResManager.GetClip("catchfish_sound", "game-tips_end3"));
            cpList.Add(ResManager.GetClip("catchfish_sound", "game-tips_end4"));
            cpList.Add(ResManager.GetClip("catchfish_sound", "game-tips_end2"));
            cpList.Add(ResManager.GetClip("catchfish_sound", "game-tips_end5"));

            cpList.Add(ResManager.GetClip("catchfish_sound", "上边和下边的数字合起来都是"));
            cpList.Add(mCtrl.mSoundCtrl.GetNumClip(nNum));

            for (int i = 0; i < cpList.Count; i++)
            {
                mCtrl.mSoundCtrl.PlaySound(cpList[i], 1f);
                //数字scale效果
                if (cpList[i].name.CompareTo("game-tips_end3") == 0)
                {
                    PlayScaleEffect(true);
                }
                else if (cpList[i].name.CompareTo("game-tips_end5") == 0)
                {
                    PlayScaleEffect(false);
                }
                yield return new WaitForSeconds(cpList[i].length);
            }


            mCtrl.bPlayOtherTip = false;
            mCtrl.LevelCheckNext();
        }
        else
        {
            mCtrl.CheckBtnActive(false);
            yield return new WaitForSeconds(0.3f);
            //Debug.Log("检测失败");
            Image img = mCheckBtn.GetComponent<Image>();
            img.sprite = ResManager.GetSprite("catchfish_sprite", "btnup");
            AudioClip cp = ResManager.GetClip("checkgamebtn_sound", "checkgamebtn_up");
            mCtrl.mSoundCtrl.PlaySortSound(cp);
            //check faile sound
            mCtrl.StopTipSound();
            mCtrl.SetTipSound(ieSoundFaileTip());
            mCtrl.StartTipSound();

            mCtrl.CheckBtnActive(true);
        }
    }


    /// <summary>
    /// 玩法提示语音
    /// </summary>
    /// <returns></returns>
    public IEnumerator ieSoundTip()
    {
        yield return new WaitForSeconds(0.1f);
        AudioClip cp = ResManager.GetClip("catchfish_sound", "game-tips_lv3_1");
        mCtrl.mSoundCtrl.PlaySound(cp, 1f);
    }

    /// <summary>
    /// 不匹配语音
    /// </summary>
    /// <returns></returns>
    public IEnumerator ieSoundFaileTip()
    {
        yield return new WaitForSeconds(0.1f);
        AudioClip cp = ResManager.GetClip("catchfish_sound", "game-tips_lv3_0");
        mCtrl.mSoundCtrl.PlaySound(cp, 1f);
    }


    public void PlayScaleEffect(bool _up)
    {
        StartCoroutine(iePlayScaleEffect(_up));
    }
    IEnumerator iePlayScaleEffect(bool _up)
    {
        for (int i = 0; i < mNumGroupList.Count; i++)
        {
            mNumGroupList[i].transform.SetSiblingIndex(6);
            if (_up)
            {
                mNumGroupList[i].mLeftNum.DoScaleEffect();
            }
            else
            { mNumGroupList[i].mRightNum.DoScaleEffect(); }
            yield return new WaitForSeconds(0.4f);
        }
    }

}
