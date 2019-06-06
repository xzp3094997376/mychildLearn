using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SDN_Panel3Station : MonoBehaviour
{
    public int nCheck = 0;
    private Image img;
    private PolygonCollider2D mbox2d;

    private Transform possT;
    private Transform[] mposs = new Transform[10];
    public List<SDN_Panel3Zhenzhu> mZhenzhuOKList = new List<SDN_Panel3Zhenzhu>();
    public List<SDN_Panel3Zhenzhu> mZhenzhuFaileList = new List<SDN_Panel3Zhenzhu>();
    private List<int> mIndexIDList = new List<int>();

    SingleAndDualNumCtrl mCtrl;

    public void InitAwake(int _id)
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as SingleAndDualNumCtrl;

        img = gameObject.GetComponent<Image>();
        img.sprite = ResManager.GetSprite("singledualnum_sprite", "beikeopen" + _id);

        mbox2d = transform.Find("mcollider").GetComponent<PolygonCollider2D>();

        possT = transform.Find("poss");
        for (int i = 0; i < mposs.Length; i++)
        {
            mposs[i] = possT.Find("pos" + i);
        }

        posIndexList = Common.GetIDList(0, 9, 10, -1);
        posHadUsedIndexList.Clear();
    }


    public void ResetInfos()
    {
        nCheck = -1;
        mZhenzhuOKList.Clear();
        mZhenzhuFaileList.Clear();
        mIndexIDList.Clear();
        for (int i = 0; i < 20; i++)
        {
            mIndexIDList.Add(i);
        }

        posIndexList = Common.GetIDList(0, 9, 10, -1);
        posHadUsedIndexList.Clear();
    }



    /// <summary>
    /// 添加珍珠到station
    /// </summary>
    /// <param name="_zhenzhu"></param>
    public bool AddZhenzhuNumber(SDN_Panel3Zhenzhu _zhenzhu)
    {
        int theNumber = _zhenzhu.imgNumber.nNumber;

        _zhenzhu.StopScaleTween();
        _zhenzhu.transform.SetParent(transform);
        _zhenzhu.transform.localScale = Vector3.one * 0.4f;
        _zhenzhu.bInStation = true;

        Vector3 vget = Vector3.zero;
        if (posIndexList.Count <= 0)
            vget = Vector3.zero;
        else
        {
            int mmid = posIndexList[0];
            vget = mposs[mmid].transform.localPosition;
            _zhenzhu.transform.localPosition = vget;
            _zhenzhu.setInPointIndex = mmid;
            posHadUsedIndexList.Add(mmid);
            posIndexList.Remove(mmid);
        }

        if (theNumber % 2 == nCheck)
        {
            mZhenzhuOKList.Add(_zhenzhu);
            //Debug.Log("move in");
        }
        else
        {
            mZhenzhuFaileList.Add(_zhenzhu);
            //Debug.Log("move in");
        }

        mCtrl.PlayTheSortSound("偶数入贝壳");
        //Debug.Log(_zhenzhu.gameObject.name);
        return true;            
    }

    /// <summary>
    /// 移出一个zhenzhu
    /// </summary>
    /// <param name="_zhenzhu"></param>
    public void RemoveZhenzhu(SDN_Panel3Zhenzhu _zhenzhu)
    {
        if (mZhenzhuOKList.Contains(_zhenzhu))
        {
            mCtrl.panel3.nCount--;
            mZhenzhuOKList.Remove(_zhenzhu);
            mCtrl.panel3.CheckBtnActive(false);
            //Debug.Log("move out");
            int mmid = _zhenzhu.setInPointIndex;
            posIndexList.Add(mmid);
            posHadUsedIndexList.Remove(mmid);
        }
        if (mZhenzhuFaileList.Contains(_zhenzhu))
        {
            mCtrl.panel3.nCount--;
            mZhenzhuFaileList.Remove(_zhenzhu);
            mCtrl.panel3.CheckBtnActive(false);
            //Debug.Log("move out");
            int mmid = _zhenzhu.setInPointIndex;
            posIndexList.Add(mmid);
            posHadUsedIndexList.Remove(mmid);
        }
        
    }


    public List<int> posIndexList = new List<int>();
    public List<int> posHadUsedIndexList = new List<int>();


    /// <summary>
    /// 不匹配的弹回
    /// </summary>
    public void RemoveUnOKZhenzhu()
    {
        for (int i = 0; i < mZhenzhuFaileList.Count; i++)
        {
            SDN_Panel3Zhenzhu _zhenzhu = mZhenzhuFaileList[i];
            _zhenzhu.transform.SetParent(mCtrl.panel3.transform);
            _zhenzhu.FaileMoveBack();
            mCtrl.panel3.nCount--;
        }
        mZhenzhuFaileList.Clear();
    }



    /// <summary>
    /// 全部是否符合
    /// </summary>
    /// <returns></returns>
    public bool CheckStationIsOK()
    {
        return mZhenzhuFaileList.Count == 0;
    }


}
