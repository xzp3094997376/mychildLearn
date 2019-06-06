using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class NumReasonLevel2 : NumReasonLevelBase
{
    private int nResCount = 6;
    private List<NumReasonStationLv2> mStationList = new List<NumReasonStationLv2>();


    public override void ResetInfos()
    {
        base.ResetInfos();
        Common.DestroyChilds(transform);
        mStationList.Clear();
    }

    public override void SetData()
    {
        ResetInfos();

        gameObject.SetActive(true);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
        //transform.DOLocalMove(Vector3.zero, 1f);

        if (imgBG == null)
        {
            imgBG = UguiMaker.newRawImage("bg", transform, "numberreasoning_texture", "numreasonimg_bj2", false);
            imgBG.rectTransform.sizeDelta = new Vector2(1280f, 800f);
        }

        nToCount = Random.Range(3, 6);
        nCount = 0;
        mCtrl.StartCoroutine(ieCreateRes());
    }
    IEnumerator ieCreateRes()
    {
        yield return new WaitForSeconds(0.1f);
        //animal pos       
        List<Vector3> animalPosList = AnimalPosList();
        //id
        List<int> animalIDs = Common.GetIDList(1, 14, nResCount, -1);
        //color
        List<Color> mColorList = ColorList();
        mColorList = Common.BreakRank(mColorList);
        //station pos
        List<Vector3> stationPosList = StationPosList();

        List<Vector2> datainfoList = GetData();

        //set lost list num index
        List<int> lostStationIndexList = Common.GetIDList(0, 5, nToCount, -1);

        for (int i = 0; i < nResCount; i++)
        {
            GameObject stGO = UguiMaker.newGameObject("station" + i, transform);
            stGO.transform.localPosition = stationPosList[i];
            NumReasonStationLv2 stCtrl = stGO.AddComponent<NumReasonStationLv2>();

            int nA = (int)datainfoList[i].x;
            int nB = (int)datainfoList[i].y;
            if (Random.value > 0.5f)
            { stCtrl.InitAwake(i, nA, nB); }
            else
            { stCtrl.InitAwake(i, nB, nA); }

            stCtrl.SetBgColor(mColorList[i]);
            stCtrl.SetStartPos(stationPosList[i]);
            //set lost num
            if (lostStationIndexList.Contains(i))
            {
                stCtrl.SetLostNumObj();
            }

            if (i > 1)
                stCtrl.CreateAnimalSpine(animalIDs[i], animalPosList[i], true);
            else
                stCtrl.CreateAnimalSpine(animalIDs[i], animalPosList[i]);

            mStationList.Add(stCtrl);
            yield return new WaitForSeconds(0.25f);
        }

        int nindex = 0;
        for (int i = 0; i < 3; i++)
        {
            mStationList[nindex].CardShowOut();
            mStationList[nindex + 1].CardShowOut();
            nindex += 2;
            yield return new WaitForSeconds(0.25f);
        }

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
        { return;}
        if (Input.GetMouseButtonUp(0))
        {
            mSelect = null;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                NumReasonNumObj com = hit.collider.gameObject.GetComponent<NumReasonNumObj>();
                if (com != null)
                {mSelect = com;}
            }
            if (mSelect != null)
            {
                mCtrl.MInputNumObj.transform.position = mSelect.transform.parent.position;
                mCtrl.MInputNumObj.ShowEffect();
                Vector3 vpos = mCtrl.MInputNumObj.transform.localPosition;
                vpos = vpos + new Vector3(260f, 0f, 0f);
                float fY = vpos.y;
                fY = Mathf.Clamp(fY, -245f, 190f);
                mCtrl.MInputNumObj.transform.localPosition = new Vector3(vpos.x, fY, 0f);
                mCtrl.MInputNumObj.SetInputNumberCallBack(InputNumbCallback);
                mCtrl.MInputNumObj.SetFinishInputCallBack(InputNumFinishCallback);
                mCtrl.MInputNumObj.SetClearNumberCallBack(InputClearCallback);

                mSelect.WenHaoActive(false);
                mCtrl.mInputTip.transform.position = mSelect.transform.position;
                mCtrl.mInputTip.gameObject.SetActive(true);
            }
        }
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
        {mCtrl.mInputTip.gameObject.SetActive(false);}
    }
    //确认输入
    public override void InputNumFinishCallback()
    {
        mCtrl.mInputTip.gameObject.SetActive(false);
        if (mCtrl.MInputNumObj.strInputNum.CompareTo("") == 0)
        {
            mSelect.WenHaoActive(true);
            mSelect.MiniNumberActive(false);
            return;
        }
        int thenum = mCtrl.MInputNumObj.nInputNum;
        mSelect.SetNumber(thenum);

        bool bcheckok = false;
        NumReasonStationLv2 mstation = mSelect.transform.parent.GetComponent<NumReasonStationLv2>();
        if (mSelect == mstation.mNumLeft)
        {
            if ((thenum + 1 == mstation.mNumRight.nNumber) || (thenum - 1 == mstation.mNumRight.nNumber))
            { bcheckok = true; }
        }
        if (mSelect == mstation.mNumRight)
        {
            if ((thenum + 1 == mstation.mNumLeft.nNumber) || (thenum - 1 == mstation.mNumLeft.nNumber))
            { bcheckok = true; }
        }

        if (bcheckok)
        {
            //Debug.Log("---ok---");
            nCount++;
            mSelect.Box2DActive(false);
            if (nCount >= nToCount && nToCount > 0)
            { mCtrl.StartCoroutine(ieToNextLevel()); }
            else
            {
                //动物欢呼
                mstation.PlayAnimation("face_sayyes", false);
                mCtrl.PlayAnimalSound(mstation.nAnimalID, true);
            }       
            //play sort sound
            mCtrl.SoundCtrl.PlaySortSound("numberreasoning_sound", "选择正确");
        }
        else
        {          
            mSelect.WenHaoActive(true);
            mSelect.MiniNumberActive(false);
            mSelect.ShakeObj();
            //play sort sound
            mCtrl.SoundCtrl.PlaySortSound("numberreasoning_sound", "inputnum_error");
            //动物失望
            mstation.PlayAnimation("face_sayno", false);
            mCtrl.PlayAnimalSound(mstation.nAnimalID, false);
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
        Debug.Log("level 2 pass");
        mCtrl.bPlayOtherTip = true;
        mCtrl.bLvPass = true;

        yield return new WaitForSeconds(0.1f);
        AudioClip cpHappy = ResManager.GetClip("numberreasoning_sound", "欢呼");
        mCtrl.SoundCtrl.PlaySortSound(cpHappy);
        for (int i = 0; i < mStationList.Count; i++)
        {
            mStationList[i].PlayAnimation("face_sayyes", true);
        }

        yield return new WaitForSeconds(cpHappy.length);
        for (int i = 0; i < mStationList.Count; i++)
        {
            mStationList[i].PlayAnimation("face_idle", true);
        }

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
            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;
        });
    }


    //玩法提示语音
    public override void PlayTipSound()
    {
        AudioClip cp = ResManager.GetClip("numberreasoning_sound", "s观察牌子上的数字规律");
        mCtrl.SoundCtrl.PlaySound(cp, 1f);
    }

    

    #region//data infos
    private List<Vector2> GetData()
    {
        List<Vector2> getInfos = new List<Vector2>();

        List<int> baseDataList = new List<int>();
        for (int i = 2; i <= 19; i++)
        { baseDataList.Add(i); }

        int hadGetCount = 0;
        while (hadGetCount < 6)
        {
            bool bCanGet = true;

            //随机取一个
            int indexA = Random.Range(0, baseDataList.Count);
            int getA = baseDataList[indexA];
            baseDataList.Remove(getA);
            //下取一个
            int getB = getA - 1;
            if (baseDataList.Contains(getB))
            {
                baseDataList.Remove(getB);
            }
            else//上取操作
            {
                getB = getA + 1;
                if (baseDataList.Contains(getB))
                {
                    baseDataList.Remove(getB);
                }
                else
                {
                    //Debug.Log("取不到了 B="+getB +", A=" + getA + "(已移除A,B)");
                    bCanGet = false;
                    baseDataList.Remove(getB);
                }
            }

            if (bCanGet)
            {
                //Debug.Log("已取得:" + getA + "," + getB);
                getInfos.Add(new Vector2(getA, getB));
                hadGetCount++;
            }
        }

        return getInfos;
    }
    private List<Color> ColorList()
    {
        List<Color> colorList = new List<Color>();
        colorList.Add(new Color(253f / 255, 215f / 255, 52f / 255, 1f));
        colorList.Add(new Color(252f / 255, 84f / 255, 145f / 255, 1f));
        colorList.Add(new Color(65f / 255, 208f / 255, 178f / 255, 1f));
        colorList.Add(new Color(243f / 255, 98f / 255, 67f / 255, 1f));
        colorList.Add(new Color(75f / 255, 182f / 255, 76f / 255, 1f));
        colorList.Add(new Color(120f / 255, 215f / 255, 247f / 255, 1f));
        return colorList;
    }
    private List<Vector3> StationPosList()
    {
        List<Vector3> posList = new List<Vector3>();
        posList.Add(new Vector3(-110f, 270f, 0f));
        posList.Add(new Vector3(110f, 270f, 0f));
        posList.Add(new Vector3(-110, 40f, 0f));
        posList.Add(new Vector3(110f, 40f, 0f));
        posList.Add(new Vector3(-110, -190f, 0f));
        posList.Add(new Vector3(110f, -190f, 0f));
        return posList;
    }
    private List<Vector3> AnimalPosList()
    {
        List<Vector3> posList = new List<Vector3>();
        posList.Add(new Vector3(-275f, 130f, 0f));
        posList.Add(new Vector3(275f, 130f, 0f));
        posList.Add(new Vector3(-260f, -113f, 0f));
        posList.Add(new Vector3(260f, -113f, 0f));
        posList.Add(new Vector3(-240f, -356f, 0f));
        posList.Add(new Vector3(240f, -356f, 0f));
        return posList;
    }
    #endregion

}
