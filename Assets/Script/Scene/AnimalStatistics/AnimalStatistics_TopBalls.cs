using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class AnimalStatistics_TopBalls : MonoBehaviour
{

    public AnimalStatistics_BallCtrl[] mBallCtrlList = null;
    private Vector3 vStart;

    //AnimalStatisticsCtrl mCtrl;
    public void InitAwake()
    {
        gameObject.SetActive(true);
        //mCtrl = SceneMgr.Instance.GetNowScene() as AnimalStatisticsCtrl;
        vStart = transform.localPosition;
        transform.localPosition = vStart + new Vector3(-1500f, 0f, 0f);

        mBallCtrlList = new AnimalStatistics_BallCtrl[5];
        for (int i = 0; i < 5; i++)
        {
            GameObject gogo = transform.Find("ballObj" + (i + 1)).gameObject;
            mBallCtrlList[i] = gogo.GetComponent<AnimalStatistics_BallCtrl>();
            if (mBallCtrlList[i] == null)
            {
                mBallCtrlList[i] = gogo.AddComponent<AnimalStatistics_BallCtrl>();
            }
            mBallCtrlList[i].InitAwake();
        }

        mDrapBallTip = GuideHandCtl.Create(transform);
    }


    public void SetData(List<int> numList)
    {
        List<int> ballTypeList = Common.GetIDList(0, 5, 5, -1);

        List<int> breakList = Common.BreakRank(numList);
        for (int i = 0; i < 5; i++)
        {
            mBallCtrlList[i].SetNumber(breakList[i]);
            mBallCtrlList[i].SetBallBG(ballTypeList[i]);
            mBallCtrlList[i].ResetInfos();
        }
    }

    public void SceneMove(bool _in)
    {
        if (_in)
        {
            transform.localPosition = vStart + new Vector3(0f, 400f, 0f);
            transform.DOLocalMove(vStart, 1f);
        }
        else
        {
            transform.DOLocalMove(vStart + new Vector3(0f, 400f, 0f), 1f);
        }
    }

    /// <summary>
    /// ball 指引
    /// </summary>
    private GuideHandCtl mDrapBallTip;
    public void ShowDrapTip(Vector3 _form,Vector3 _to)
    {
        mDrapBallTip.GuideTipDrag(_form, _to, -1,1f,"hand1");
        mDrapBallTip.SetDragDate(new Vector3(5f, -30f, 0f), 1f);
    }
    public void StopDrapTip()
    {
        mDrapBallTip.StopDrag();
    }

}
