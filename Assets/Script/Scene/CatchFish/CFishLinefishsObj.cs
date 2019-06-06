using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 上部line
/// </summary>
public class CFishLinefishsObj : MonoBehaviour
{
    public class FishCheckData
    {
        public int nLeft = 0;
        public int nRight = 0;
        public FishCheckData(int _left, int _right)
        {
            nLeft = _left;
            nRight = _right;
        }            
    }



    public int nID = 0;

    public List<CFishFishpicObj> mFishPicObjList = new List<CFishFishpicObj>();
    private List<FishCheckData> mCheckFishDataList = new List<FishCheckData>();
    /// <summary>
    /// 完成加入
    /// </summary>
    private List<FishCheckData> mOKDataList = new List<FishCheckData>();

    private Image imgline;
    private Transform pics;




    public void InitAwake(int _id)
    {
        nID = _id;
        imgline = gameObject.GetComponent<Image>();
        imgline.sprite = ResManager.GetSprite("catchfish_sprite", "line");

        pics = transform.Find("pics");
        for (int i = 0; i < nID -1; i++)
        {
            GameObject mgo = pics.Find("pic" + i).gameObject;
            CFishFishpicObj picCtrl = mgo.AddComponent<CFishFishpicObj>();
            picCtrl.InitAwake();

            mFishPicObjList.Add(picCtrl);
        }

        SetCheckData();
    }

    private void SetCheckData()
    {
        for (int i = 1; i < nID; i++)
        {
            FishCheckData mdata = new FishCheckData(i, nID - i);
            mCheckFishDataList.Add(mdata);
            //Debug.Log("(" + i + "," + (nID - i) + ")");
        }
    }

    /// <summary>
    /// 由左ID取得checkdata forme old list
    /// </summary>
    public FishCheckData GetCheckDataByLeft(int _left)
    {
        for (int i = 0; i < mCheckFishDataList.Count; i++)
        {
            if (mCheckFishDataList[i].nLeft == _left)
            {
                return mCheckFishDataList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 由左ID取得checkdata forme old list
    /// </summary>
    public FishCheckData GetTheSameDataByLeft(int _left)
    {
        for (int i = 0; i < mOKDataList.Count; i++)
        {
            if (mOKDataList[i].nLeft == _left)
            {
                if (!CheckIsSameRightCount(_left))
                    return mOKDataList[i];
            }
        }
        return null;
    }

    //检测是否与右数量相同
    bool CheckIsSameRightCount(int _left)
    {
        for (int i = 0; i < mOKDataList.Count; i++)
        {
            if (mOKDataList[i].nRight == _left)
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 检测是否有数据匹配
    /// </summary>
    private bool CheckIsOK(List<CFishFishObj> _leftFish, List<CFishFishObj> _rightFish)
    {
        int _leftcount = _leftFish.Count;

        FishCheckData checkdata = GetCheckDataByLeft(_leftcount);
        if (checkdata != null)
        {
            if (checkdata.nRight == _rightFish.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            //Debug.LogError("get check data null");
            return false;
        }
    }

    /// <summary>
    /// 检测是否匹配成功
    /// </summary>
    public bool ToCheckInFish(List<CFishFishObj> _leftFish, List<CFishFishObj> _rightFish,out int _leftCount)
    {
        _leftCount = 0;
        bool bOK = CheckIsOK(_leftFish, _rightFish);
        if (bOK)
        {
            CFishFishpicObj getUnSetPic = GetUnSetOKPic();
            if (getUnSetPic != null)
            {
                getUnSetPic.AddLeftFishToPic(_leftFish);
                getUnSetPic.AddRightFishToPic(_rightFish);
                getUnSetPic.bOK = true;
                //删除一个信息
                FishCheckData checkdata = GetCheckDataByLeft(_leftFish.Count);
                mOKDataList.Add(checkdata);
                if (mCheckFishDataList.Contains(checkdata))
                    mCheckFishDataList.Remove(checkdata);

                ////检测是否全部完成
                //if (mCheckFishDataList.Count <= 0)
                //{
                //    Debug.Log("lv1 pass");
                //}            
            }
            else
            {
                Debug.LogError("取不到没有设置的fish pic obj");
            }
        }
        else
        {
            
            if (_leftFish.Count != _rightFish.Count)
            {
                FishCheckData theSameData = GetTheSameDataByLeft(_leftFish.Count);
                if (theSameData != null)
                {
                    _leftCount = theSameData.nLeft;
                }
            }
        }

        return bOK;
    }

    /// <summary>
    /// 取得没有设置的picobj
    /// </summary>
    /// <returns></returns>
    private CFishFishpicObj GetUnSetOKPic()
    {
        for (int i=0;i< mFishPicObjList.Count;i++)
        {
            if (!mFishPicObjList[i].bOK)
            {
                return mFishPicObjList[i];
            }
        }
        return null;
    }

}
