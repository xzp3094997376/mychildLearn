using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class NumReasonLevel1 : NumReasonLevelBase
{

    public NumReasonBigWheel mBigWheel;

    public override void ResetInfos()
    {
        base.ResetInfos();
        if (mBigWheel != null)
        {
            if (mBigWheel.gameObject != null)
                GameObject.Destroy(mBigWheel.gameObject);
            mBigWheel = null;
        }
        bFirstFaile = false;
        bFirstSuc = false;
    }

    public override void SetData()
    {
        ResetInfos();

        gameObject.SetActive(true);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
        //transform.DOLocalMove(Vector3.zero, 1f);

        nToCount = Random.Range(3, 6);
        nCount = 0;

        if (imgBG == null)
        {
            imgBG = UguiMaker.newRawImage("bg", transform, "numberreasoning_texture", "numreasonimg_bj1", false);
            imgBG.rectTransform.sizeDelta = new Vector2(1280f, 800f);
        }

        GameObject bigWheelObj = ResManager.GetPrefab("numberreasoning_prefab", "bigwheel", transform);
        mBigWheel = bigWheelObj.AddComponent<NumReasonBigWheel>();
        mBigWheel.InitAwake();
        //空的数字
        mBigWheel.SetLostNumObj(nToCount);

        mCtrl.PlayTipSound();
    }

    NumReasonNumObj mSelect;
    private void Update()
    {
        if (!bInit)
            return;
        if (mCtrl.bLvPass)
            return;
        if (mCtrl.MInputNumObj.gameObject.activeSelf)
        {
            return;
        }
        if (mBigWheel == null)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            mSelect = null;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                NumReasonNumObj com = hit.collider.gameObject.GetComponent<NumReasonNumObj>();
                if (com != null)
                {
                    mSelect = com; 
                }
            }
            if (mSelect != null)
            {
                mBigWheel.SetBigWheelRun(false);
                mCtrl.MInputNumObj.transform.position = mSelect.transform.parent.position;
                mCtrl.MInputNumObj.ShowEffect();

                SetInputObjPos(mCtrl.MInputNumObj.transform.localPosition);

                mCtrl.MInputNumObj.SetInputNumberCallBack(InputNumbCallback);
                mCtrl.MInputNumObj.SetFinishInputCallBack(InputNumFinishCallback);
                mCtrl.MInputNumObj.SetClearNumberCallBack(InputClearCallback);

                mSelect.WenHaoActive(false);
                mCtrl.mInputTip.transform.position = mSelect.transform.position;
                mCtrl.mInputTip.gameObject.SetActive(true);
            }
        }
    }

    //设置输入框位置
    private void SetInputObjPos(Vector3 _vbase)
    {
        Vector3 vget = _vbase + new Vector3(240f, 0f, 0f);

        float fY = vget.y;
        fY = Mathf.Clamp(fY, -245f, 190f);

        float fX = vget.x;
        if (fX > 480f)
        {
            fX = _vbase.x - 240f;
        }
        mCtrl.MInputNumObj.transform.localPosition = new Vector3(fX, fY, 0f);
    }


    //输入数字
    public override void InputNumbCallback()
    {
        mSelect.WenHaoActive(false);
        mSelect.MiniNumberActive(true);
        int nid = 0;
        if (int.TryParse(mCtrl.MInputNumObj.strInputNum, out nid))
        { }
        else { Debug.LogError("不能转成 int 类型:" + mCtrl.MInputNumObj.strInputNum); }
        mSelect.SetNumber(nid);
        //光标位置设置
        if (nid < 10)
        {
            mCtrl.mInputTip.gameObject.SetActive(true);
            Image imgTar = mSelect.GetMiniNumber.GetMaxRight();
            if (imgTar != null)
            {
                mCtrl.mInputTip.transform.position = imgTar.transform.position;
                mCtrl.mInputTip.transform.localPosition = mCtrl.mInputTip.transform.localPosition + new Vector3(21.1f, 0f, 0f);
            }
            else
            { mCtrl.mInputTip.transform.position = mSelect.transform.position; }
        }
        else
        {
            mCtrl.mInputTip.gameObject.SetActive(false);
        }
    }
    //确认输入
    public override void InputNumFinishCallback()
    {
        mCtrl.mInputTip.gameObject.SetActive(false);
        if (mCtrl.MInputNumObj.strInputNum.CompareTo("") == 0)
        {
            mSelect.WenHaoActive(true);
            mSelect.MiniNumberActive(false);
            mBigWheel.SetBigWheelRun(true);
            return;
        }
        mSelect.SetNumber(mCtrl.MInputNumObj.nInputNum);
        if (mSelect.CheckIsOK())
        {
            //Debug.Log("---ok---");
            nCount++;
            if (nCount >= nToCount && nToCount > 0)
            {
                mBigWheel.SetBigWheelRun(false);
                mCtrl.StartCoroutine(ieToNextLevel());
            }
            else
            {
                mBigWheel.SetBigWheelRun(true);
            }
            PlayFirstSucSound();
        }
        else
        {
            mBigWheel.SetBigWheelRun(true);
            mSelect.WenHaoActive(true);
            mSelect.MiniNumberActive(false);
            PlayFirstFaileSound();
        }   
    }
    //清除数字
    public override void InputClearCallback()
    {
        mCtrl.mInputTip.gameObject.SetActive(true);
        mCtrl.mInputTip.transform.position = mSelect.transform.position;
        //mSelect.WenHaoActive(true);
        mSelect.MiniNumberActive(false);
    }

    //关卡1完成
    IEnumerator ieToNextLevel()
    {
        Debug.Log("level 1 pass");
        mCtrl.bPlayOtherTip = true;
        mCtrl.PlayGoodGoodSound();
        yield return new WaitForSeconds(2.5f);
        mCtrl.LevelCheckNext();
    }

    /// <summary>
    /// 移出界面并清除信息
    /// </summary>
    public override void MoveOutAndReset()
    {
        transform.DOScale(Vector3.one * 0.01f, 1f);
        transform.DORotate(new Vector3(0f, 0f, -180f), 1f);
        transform.DOLocalMove(new Vector3(1400f, 800f, 0f), 1f).OnComplete(() =>
        {
            ResetInfos();
            gameObject.SetActive(false);
            transform.localPosition = new Vector3(-1400f, 0f, 0f);
        });
    }

    #region//语音---
    //玩法提示语音
    public override void PlayTipSound()
    {
        AudioClip  cp = mCtrl.SoundCtrl.GetClip("numberreasoning_sound", "s观察摩天轮上的数字规律");
        mCtrl.SoundCtrl.PlaySound(cp, 1f);
    }
    //播放初次失败声音
    bool bFirstFaile = false;
    public void PlayFirstFaileSound()
    {
        //play sort sound
        mCtrl.SoundCtrl.PlaySortSound("numberreasoning_sound", "inputnum_error");

        if (!bFirstFaile)
        {
            mCtrl.SoundCtrl.StopTipSound();
            AudioClip cp = mCtrl.SoundCtrl.GetClip("numberreasoning_sound", "s上面两个数字加起来是几");
            mCtrl.SoundCtrl.PlaySound(cp, 1f);

            mCtrl.SoundCtrl.PlayTipSound(iePlayFirstFaileSound());
            bFirstFaile = true;
        }
    }
    IEnumerator iePlayFirstFaileSound()
    {
        yield return new WaitForSeconds(0.02f);
        List<AudioClip> cpList = new List<AudioClip>();
        cpList.Add(ResManager.GetClip("numberreasoning_sound", "s上面两个数字加起来是几"));
        cpList.Add(ResManager.GetClip("numberreasoning_sound", "s再看看下面的数字"));
        cpList.Add(ResManager.GetClip("numberreasoning_sound", "s你发现了什么"));
        for (int i = 0; i < cpList.Count; i++)
        {
            AudioClip cp = cpList[i];
            mCtrl.SoundCtrl.PlaySound(cp, 1f);
            yield return new WaitForSeconds(cp.length);
        }
    }

    //播放初次成功声音
    bool bFirstSuc = false;
    public void PlayFirstSucSound()
    {
        //play sort sound
        mCtrl.SoundCtrl.PlaySortSound("numberreasoning_sound", "选择正确");

        if (!bFirstSuc)
        {
            mCtrl.SoundCtrl.StopTipSound();
            AudioClip cp = mCtrl.SoundCtrl.GetClip("numberreasoning_sound", "s嗯嗯有点厉害");
            mCtrl.SoundCtrl.PlaySound(cp, 1f);
            bFirstSuc = true;
        }
    }
    #endregion

}
