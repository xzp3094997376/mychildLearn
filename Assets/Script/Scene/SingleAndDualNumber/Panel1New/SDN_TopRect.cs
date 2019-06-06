using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class SDN_TopRect : MonoBehaviour
{

    public int nNum = 0;
    private SDN_LinesParent mLinesParent;

    private Image imgBG;
    private Image imgNum;

    //private SingleAndDualNumCtrl mCtrl;



    public void InitAwake(int _num)
    {
        //mCtrl = SceneMgr.Instance.GetNowScene() as SingleAndDualNumCtrl;
        nNum = _num;

        imgBG = UguiMaker.newImage("rect", transform, "singledualnum_sprite", "rect", false);

        imgNum = UguiMaker.newImage("num", transform, "singledualnum_sprite", _num.ToString(), false);
        imgNum.color = new Color(247f / 255, 226f / 255, 61f / 255);

        imgNum.transform.localPosition = new Vector3(0f, 25f, 0f);
        imgNum.transform.localScale = Vector3.one * 0.2f;

        ResetInfos();
    }


    public void ResetInfos()
    {
        imgNum.gameObject.SetActive(false);
        if (mLinesParent != null)
        {
            GameObject.Destroy(mLinesParent.gameObject);
            mLinesParent = null;
        }

    }



    public void SetLinesParent(SDN_LinesParent _linesPar)
    {
        if (nNum == _linesPar.nNumber)
        {
            imgNum.gameObject.SetActive(true);

            mLinesParent = _linesPar;
            _linesPar.NumberActive(false);
            _linesPar.transform.SetParent(transform);
            _linesPar.transform.DOLocalMove(new Vector3(0f, -14f, 0f), 0.3f);
            _linesPar.transform.DOScale(Vector3.one * 0.13f, 0.3f);
            for (int i = 0; i < _linesPar.mLinelineObjList.Count; i++)
            {
                _linesPar.mLinelineObjList[i].StopAnimation();
            }
        }
        else
        {
            Debug.Log("id 不相同");
        }
    }


    public void SetLinesParentOne(SDN_LinesParent _linesPar)
    {
        if (nNum == _linesPar.nNumber)
        {
            imgNum.gameObject.SetActive(true);
            mLinesParent = _linesPar;
            _linesPar.NumberActive(false);
            _linesPar.transform.SetParent(transform);
        }
        else
        {
            Debug.Log("id 不相同");
        }
    }

}
