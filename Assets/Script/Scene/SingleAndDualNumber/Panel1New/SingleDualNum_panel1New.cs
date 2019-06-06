using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class SingleDualNum_panel1New : MonoBehaviour
{

    public int nNowIndex = 0;

    private List<int> nIDList = new List<int>();
    public List<SDN_TopRect> mTopRectList = new List<SDN_TopRect>();
    private List<SDN_LinesParent> mLineParentList = new List<SDN_LinesParent>();

    private SingleAndDualNumCtrl mCtrl;

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as SingleAndDualNumCtrl;

        nIDList = new List<int>() { 1, 3, 5, 7, 9, 2, 4, 6, 8 , 10};

        List<float> findexList = Common.GetOrderList(10, 120f);
        for (int i = 0; i < nIDList.Count; i++)
        {
            int id = nIDList[i];
            SDN_TopRect rectTop = CreateTopRectObj(id, transform);
            rectTop.transform.localPosition = new Vector3(findexList[i], 290f, 0f);
            mTopRectList.Add(rectTop);
        }        
    }


    public void ResetInfos()
    {
        nNowIndex = 0;
        for (int i = 0; i < mTopRectList.Count; i++)
        {
            mTopRectList[i].ResetInfos();
        }
        for (int i = 0; i < mLineParentList.Count; i++)
        {
            if (mLineParentList[i].gameObject != null)
                GameObject.Destroy(mLineParentList[i].gameObject);
        }
        mLineParentList.Clear();
    }


    /// <summary>
    /// 开始
    /// </summary>
    public void PanelStart()
    {
        if (nNowIndex == 0 ||nNowIndex > 9)
        {
            //Debug.Log("start");
            mCtrl.StartCoroutine(iePanelStart());
        }
    }
    IEnumerator iePanelStart()
    {
        yield return new WaitForSeconds(1.4f);
        //1出现
        SDN_LinesParent linesParent0 = CreateLinesParentObj(1, transform);
        linesParent0.bFinish = true;
        linesParent0.LinesParentBoxHide();
        mLineParentList.Add(linesParent0);

        linesParent0.transform.localPosition = new Vector3(1000f, 0f, 0f);
        linesParent0.transform.DOLocalMove(Vector3.zero, 0.5f);
        AudioClip cp0 = mCtrl.GetClip("biut");
        mCtrl.PlayTheSortSound(cp0);

        yield return new WaitForSeconds(1f);   
        LineOKCall(linesParent0);

        yield return new WaitForSeconds(2f);
        mCtrl.BtnsMove(true, 0.5f);
    }


    /// <summary>
    /// 连线完成call
    /// </summary>
    public void LineOKCall(SDN_LinesParent _linepar)
    {
        //Debug.Log("Line Line Finish");
        mCtrl.bButtonActive = false;
        mCtrl.StartCoroutine(iePlayNumSound(_linepar));
    }
    //播放奇偶数声音
    IEnumerator iePlayNumSound(SDN_LinesParent _linepar)
    {
        yield return new WaitForSeconds(1f);
        int nid = _linepar.nNumber; ;
        List<AudioClip> aclist = new List<AudioClip>();
        aclist.Add(mCtrl.GetNumClip(nid));
        if (IsSingleNumber(nid))
        {
            aclist.Add(mCtrl.GetClip("是奇数"));//game-tips2-4-1
        }
        else
        {
            aclist.Add(mCtrl.GetClip("是偶数"));//game-tips2-4-2
        }
        for (int i = 0; i < aclist.Count; i++)
        {
            float actime = aclist[i].length;
            mCtrl.PlaySound(aclist[i], 1f);
            yield return new WaitForSeconds(actime + 0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        //飞到top
        SDN_TopRect topRect = GetTopRectByID(nid);
        topRect.SetLinesParent(_linepar);
        AudioClip cp1 = mCtrl.GetClip("totop");
        mCtrl.PlayTheSortSound(cp1);
        yield return new WaitForSeconds(0.2f);
        mCtrl.bButtonActive = true;
        ToNextNumber();
    }

    /// <summary>
    /// 下一个数字
    /// </summary>
    public void ToNextNumber()
    {
        nNowIndex++;
        if (nNowIndex > 9)
        {
            //to panel2
            Zongjie();
            return;
        }
       
        SDN_LinesParent linesParent = CreateLinesParentObj(nIDList[nNowIndex], transform);
        mLineParentList.Add(linesParent);
        linesParent.transform.localPosition = new Vector3(1000f, 0f, 0f);
        linesParent.transform.DOLocalMove(Vector3.zero, 0.5f).OnComplete(()=> 
        {
            linesParent.SetGuideHand();
        });      
    }

    //总结
    private void Zongjie()
    {
        mCtrl.bPlayOtherTip = true;
        //mCtrl.bButtonActive = false;
        StopZongjie();
        ieZongjie = ieToPanel2();
        mCtrl.StartCoroutine(ieZongjie);
    }

    IEnumerator ieZongjie = null;
    public void StopZongjie()
    {
        if (ieZongjie != null)
            mCtrl.StopCoroutine(ieZongjie);
    }

    IEnumerator ieToPanel2()
    {
        //你发现了吗？奇数最后一个总是没有朋友。偶数都是成双成对的朋友
        List<AudioClip> cpList = new List<AudioClip>();
        cpList.Add(ResManager.GetClip("singledualnum_sound", "你发现了没"));
        cpList.Add(ResManager.GetClip("singledualnum_sound", "有一条小鱼没朋友"));
        cpList.Add(ResManager.GetClip("singledualnum_sound", "所以"));
        cpList.Add(ResManager.GetClip("singledualnum_sound", "是奇数"));
        cpList.Add(ResManager.GetClip("singledualnum_sound", "每条小鱼都有朋友"));
        cpList.Add(ResManager.GetClip("singledualnum_sound", "所以"));
        cpList.Add(ResManager.GetClip("singledualnum_sound", "是偶数"));
        for (int i = 0; i < cpList.Count; i++)
        {
            mCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
        yield return new WaitForSeconds(0.2f);
        mCtrl.bPlayOtherTip = false;
        mCtrl.ChangePanel(1, 2);
    }



    public void SceneMove(bool _in, float _time)
    {
        if (_in)
        {
            gameObject.SetActive(true);
            transform.localPosition = new Vector3(0f, 900f, 0f);
            transform.DOLocalMove(Vector3.zero, _time);
          
            if ((nNowIndex >= 1 && nNowIndex <= 9))
            {
                mCtrl.BtnsMove(true, 0.5f);
            }
            else
            {
                ResetInfos();
                PanelStart();
            }
        }
        else
        {
            transform.DOLocalMove(new Vector3(0f, 900f, 0f), _time).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }



    public SDN_TopRect GetTopRectByID(int _id)
    {
        for (int i = 0; i < mTopRectList.Count; i++)
        {
            if (mTopRectList[i].nNum == _id)
                return mTopRectList[i];
        }
        return null;
    }


    private bool IsSingleNumber(int _num)
    {
        return _num % 2 == 1;
    }



    public SDN_LinesParent CreateLinesParentObj(int _num, Transform _trans)
    {
        GameObject mgo = UguiMaker.newGameObject("linesParent" + _num, _trans);
        SDN_LinesParent lineParentCtrl = mgo.AddComponent<SDN_LinesParent>();
        lineParentCtrl.InitAwake(_num);
        lineParentCtrl.SetLineOKCallBack(LineOKCall);

        AudioClip cp0 = mCtrl.GetClip("biut");
        mCtrl.PlayTheSortSound(cp0);
        return lineParentCtrl;
    }
    public SDN_TopRect CreateTopRectObj(int _num, Transform _trans)
    {
        GameObject mgo = UguiMaker.newGameObject("topRect" + _num, _trans);
        SDN_TopRect toprectCtrl = mgo.AddComponent<SDN_TopRect>();
        toprectCtrl.InitAwake(_num);
        return toprectCtrl;
    }

}

/// <summary>
/// 连线完成委托
/// </summary>
public delegate void SDN_LineOK_Delegate(SDN_LinesParent _linePar);
