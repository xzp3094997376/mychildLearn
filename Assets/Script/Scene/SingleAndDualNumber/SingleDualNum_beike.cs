using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class SingleDualNum_beike : MonoBehaviour
{
    public int nNum = 0;
    public bool bFinish = false;

    private Image img;
    private Image imgbeike;
    private Image imgnum;
    private Image img3;

    Tween nowBeikeTween = null;

    public void InitStart (int _num)
    {
        nNum = _num;

        img = gameObject.GetComponent<Image>();
        imgbeike = UguiMaker.newImage("imgbeike", transform, "singledualnum_sprite", "beike1_1", false);

        imgnum = UguiMaker.newImage("imgnum", transform, "singledualnum_sprite", _num.ToString(), false);
        imgnum.transform.localScale = Vector3.one * 0.75f;

        img3 = UguiMaker.newImage("img3", transform, "singledualnum_sprite", "beike1", false);
        img3.rectTransform.anchoredPosition = new Vector2(0f, 4f);

        if (_num % 2 == 0)
        {
            imgbeike.sprite = ResManager.GetSprite("singledualnum_sprite", "beike2_11");
            imgnum.color = new Color(51f / 255, 130f / 255, 16f / 255);
        }
        else
        {
            imgbeike.sprite = ResManager.GetSprite("singledualnum_sprite", "beike1_11");
            imgnum.color = new Color(171f / 255, 91f / 255, 6f / 255);
        }
        imgnum.rectTransform.anchoredPosition = new Vector2(0f, 5f);
        imgnum.SetNativeSize();

        gameObject.AddComponent<Button>();
        EventTriggerListener.Get(gameObject).onClick = BtnClick;
    }

    /// <summary>
    /// 设置贝壳状态 1未完成  2正在进行  3已完成
    /// </summary>
    /// <param name="_state"></param>
    public void SetState(int _state)
    {     
        if (nowBeikeTween != null)
        {
            nowBeikeTween.Pause();
            transform.localScale = Vector3.one;         
        }

        if (_state == 1)
        {
            if (nNum % 2 == 0)
            { imgbeike.sprite = ResManager.GetSprite("singledualnum_sprite", "beike2_11"); }
            else
            { imgbeike.sprite = ResManager.GetSprite("singledualnum_sprite", "beike1_11"); }
            img3.enabled = true;
        }
        else if (_state == 2)
        {
            if (nNum % 2 == 0)
            { imgbeike.sprite = ResManager.GetSprite("singledualnum_sprite", "beike2_1"); }
            else
            { imgbeike.sprite = ResManager.GetSprite("singledualnum_sprite", "beike1_1"); }
            nowBeikeTween = transform.DOScale(Vector3.one * 0.75f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            img3.enabled = false;
        }
        else
        {
            if (nNum % 2 == 0)
            { imgbeike.sprite = ResManager.GetSprite("singledualnum_sprite", "beike2_11"); }
            else
            { imgbeike.sprite = ResManager.GetSprite("singledualnum_sprite", "beike1_11"); }
            img3.enabled = false;
            bFinish = true;
        }
    }

    SingleAndDualNumCtrl mctrl;
    private void BtnClick(GameObject _go)
    {
        if (bFinish)
            return;
        if (mctrl == null)
            mctrl = SceneMgr.Instance.GetNowScene() as SingleAndDualNumCtrl;
        //if (mctrl.panel1.bGuide)
        //    return;
        //if (mctrl.panel1.bPlayNumSound)
        //    return;
        //if (nNum == mctrl.panel1.nNum)
        //    return;
        //mctrl.panel1.SetNumber(nNum);
    }

    public void ResetInfos()
    {
        if (bFinish)
        {
            SetState(3);
        }
        else
        {
            SetState(1);
        }
    }
}
