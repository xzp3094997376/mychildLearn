using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// 鱼s照片
/// </summary>
public class CFishFishpicObj : MonoBehaviour
{
    /// <summary>
    /// 是否已经完成
    /// </summary>
    public bool bOK = false;

    public List<CFishFishObj> mLeftFishList = new List<CFishFishObj>();
    public List<CFishFishObj> mRightFishList = new List<CFishFishObj>();

    private Image imgpic;
    private Image whiteline;

    private GridLayoutGroup gridL;
    private GridLayoutGroup gridC;
    private GridLayoutGroup gridR;

    private CatchFish mCtrl;

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as CatchFish;

        GameObject mgo = ResManager.GetPrefab("catchfish_prefab", "picobj");
        mgo.transform.SetParent(transform);
        mgo.transform.localPosition = Vector3.zero;
        mgo.transform.localScale = Vector3.one;

        imgpic = mgo.transform.GetComponent<Image>();
        imgpic.sprite = ResManager.GetSprite("catchfish_sprite", "picshow");
        whiteline = imgpic.transform.Find("whiteline").GetComponent<Image>();

        gridL = imgpic.transform.Find("gridL").GetComponent<GridLayoutGroup>();
        gridC = imgpic.transform.Find("gridC").GetComponent<GridLayoutGroup>();
        gridR = imgpic.transform.Find("gridR").GetComponent<GridLayoutGroup>();

        whiteline.enabled = false;
    }

    /// <summary>
    /// 先=>左操作
    /// </summary>
    /// <param name="_fishList"></param>
    public void AddLeftFishToPic(List<CFishFishObj> _fishList)
    {
        for (int i = 0; i < _fishList.Count; i++)
        {
            CFishFishObj _fishobj = _fishList[i];
            _fishobj.StopMove();
            _fishobj.BoxActive(false);
            _fishobj.StopPlayAnimal();

            mLeftFishList.Add(_fishobj);
            StartCoroutine(IEMoveToLeft());
        }
    }
    IEnumerator IEMoveToLeft()
    {
        for (int i = 0; i < mLeftFishList.Count; i++)
        {
            CFishFishObj _fishobj = mLeftFishList[i];
            Transform getParent = gridL.transform;
            if (i <= 3)
            {
                getParent = gridL.transform;
                //_fishobj.transform.SetParent(gridL.transform);
            }
            else
            {
                getParent = gridC.transform;
                //_fishobj.transform.SetParent(gridC.transform);
            }

            AudioClip cp = ResManager.GetClip("catchfish_sound", "to_fishtopic");
            mCtrl.mSoundCtrl.PlaySortSound(cp, 0.08f);

            _fishobj.transform.DOScale(getParent.transform.localScale, 0.2f);
            _fishobj.transform.DOMove(getParent.transform.position, 0.2f).OnComplete(() =>
            {
                _fishobj.transform.SetParent(getParent);
                _fishobj.transform.localScale = Vector3.one;
            });
            yield return new WaitForSeconds(0.22f);
        }
    }

    /// <summary>
    /// 后=>右操作
    /// </summary>
    /// <param name="_fishList"></param>
    public void AddRightFishToPic(List<CFishFishObj> _fishList)
    {
        for (int i = 0; i < _fishList.Count; i++)
        {
            CFishFishObj _fishobj = _fishList[i];
            _fishobj.StopMove();
            _fishobj.BoxActive(false);
            _fishobj.StopPlayAnimal();

            mRightFishList.Add(_fishobj);
            StartCoroutine(IEMoveToRight());
        }

        ShowWhiteLinePos();
    }
    IEnumerator IEMoveToRight()
    {
        for (int i = 0; i < mRightFishList.Count; i++)
        {
            CFishFishObj _fishobj = mRightFishList[i];
            Transform getParent = gridR.transform;
            if (mLeftFishList.Count <= 4)
            {
                if (mRightFishList.Count <= 4)
                {
                    getParent = gridR.transform;
                    //_fishobj.transform.SetParent(gridR.transform);
                }
                else
                {
                    if (i <= 3)
                    {
                        getParent = gridC.transform;
                        //_fishobj.transform.SetParent(gridC.transform);
                    }
                    else
                    {
                        getParent = gridR.transform;
                        _fishobj.transform.SetParent(gridR.transform);
                    }
                }
            }
            else
            {
                getParent = gridR.transform;
                //_fishobj.transform.SetParent(gridR.transform);
            }

            AudioClip cp = ResManager.GetClip("catchfish_sound", "to_fishtopic");
            mCtrl.mSoundCtrl.PlaySortSound(cp, 0.08f);

            _fishobj.transform.DOScale(getParent.transform.localScale, 0.2f);
            _fishobj.transform.DOMove(getParent.transform.position, 0.2f).OnComplete(()=> 
            {
                _fishobj.transform.SetParent(getParent);
                _fishobj.transform.localScale = Vector3.one;
            });
          
            yield return new WaitForSeconds(0.22f);
        }
    }





    /// <summary>
    /// 分隔白线显示
    /// </summary>
    public void ShowWhiteLinePos()
    {
        whiteline.enabled = true;
        if (mLeftFishList.Count <= 4 && mRightFishList.Count <= 4)
        {
            Vector3 vpos = (gridL.transform.localPosition + gridR.transform.localPosition) * 0.5f;
            whiteline.transform.localPosition = vpos;
        }
        else if (mLeftFishList.Count <= 4 && mRightFishList.Count > 4)
        {
            Vector3 vpos = (gridC.transform.localPosition + gridL.transform.localPosition) * 0.5f;
            whiteline.transform.localPosition = vpos;
        }
        else if (mLeftFishList.Count > 4 && mRightFishList.Count <= 4)
        {
            Vector3 vpos = (gridC.transform.localPosition + gridR.transform.localPosition) * 0.5f;
            whiteline.transform.localPosition = vpos;
        }
    }


}
