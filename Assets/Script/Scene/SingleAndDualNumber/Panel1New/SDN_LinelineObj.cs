using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using Spine.Unity;

public class SDN_LinelineObj : MonoBehaviour
{
    /// <summary>
    /// 编号
    /// </summary>
    public int nIndex = 0;
    public bool bLineOK = false;

    private List<int> mUnLineUpList = new List<int>();

    private Image imgRang;
    private Image imgFlash;
    private BoxCollider2D mBox2D;

    //private bool bInit = false;
    private SkeletonGraphic fishSpine;

    private SingleAndDualNumCtrl mCtrl;

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as SingleAndDualNumCtrl;

        imgRang = UguiMaker.newImage("imgRangUp", transform, "singledualnum_sprite", "rangwhite", false);
        imgRang.rectTransform.sizeDelta = Vector2.one * 120f;

        mBox2D = gameObject.AddComponent<BoxCollider2D>();
        mBox2D.size = imgRang.rectTransform.sizeDelta;

        int nfishID = UnityEngine.Random.Range(0, 4);
        imgFlash = UguiMaker.newImage("flash", imgRang.transform, "singledualnum_sprite", "flash" + nfishID, false);
        imgFlash.rectTransform.localScale = Vector3.one * 0.5f;
        imgFlash.enabled = false;

        GameObject mgofish = GameObject.Instantiate(mCtrl.GetFishResByID(nfishID)) as GameObject;
        mgofish.SetActive(true);
        fishSpine = mgofish.GetComponent<SkeletonGraphic>();
        fishSpine.transform.SetParent(imgRang.transform);
        fishSpine.transform.localScale = Vector3.one * 0.6f;
        fishSpine.transform.localPosition = new Vector3(-4f, -45f, 0f);
        fishSpine.AnimationState.SetAnimation(1, "Idle", true);
        //bInit = true;
    }


    public void StopAnimation()
    {
        fishSpine.AnimationState.TimeScale = 0f;
    }

    public void BoxActive(bool _active)
    {
        mBox2D.enabled = _active;
    }


    /// <summary>
    /// 设置不能连起的编号(不能相隔直线连线, 单数最后一个谁都可以连)
    /// </summary>
    public void SetUnLineUpIndexs(bool _single)
    {
        if (!_single)
        {
            switch (nIndex)
            {
                case 0:
                    mUnLineUpList = new List<int>() { 4, 6, 8 };
                    break;
                case 1:
                    mUnLineUpList = new List<int>() { 5, 7, 9 };
                    break;
                case 2:
                    mUnLineUpList = new List<int>() { 6, 8 };
                    break;
                case 3:
                    mUnLineUpList = new List<int>() { 7, 9 };
                    break;
                case 4:
                    mUnLineUpList = new List<int>() { 0, 8 };
                    break;
                case 5:
                    mUnLineUpList = new List<int>() { 1, 9 };
                    break;
                case 6:
                    mUnLineUpList = new List<int>() { 0, 2 };
                    break;
                case 7:
                    mUnLineUpList = new List<int>() { 1, 3 };
                    break;
                case 8:
                    mUnLineUpList = new List<int>() { 0, 2, 4 };
                    break;
                case 9:
                    mUnLineUpList = new List<int>() { 1, 3, 5 };
                    break;
            }
        }
        else
        {
            mUnLineUpList = new List<int>();
        }
    }

    /// <summary>
    /// 检测是否能连起
    /// </summary>
    /// <param name="_Index"></param>
    /// <returns></returns>
    public bool CheckCanLineUp(int _Index)
    {
        if (mUnLineUpList.Contains(_Index))
        {
            Debug.Log("这个点不能连起");
            return false;
        }
        else
        {
            return true;
        }
    }

}
