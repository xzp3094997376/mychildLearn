using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class WFF_InputObj : MonoBehaviour
{

    public WFF_RocketObj targetRocket;

    private GameObject mparent;
    private Image[] imgBtns = new Image[5];
    private Image[] imgNum = new Image[5];

    private Image imgClose;

    Tween scaleTween = null;

    WhoFlyFastCtrl mCtrl;

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as WhoFlyFastCtrl;

        imgClose = UguiMaker.newGameObject("imgClose", transform).AddComponent<Image>();
        imgClose.rectTransform.sizeDelta = new Vector2(2600f, 1500f);
        imgClose.color = new Color(1f, 1f, 1f, 0f);
        EventTriggerListener.Get(imgClose.gameObject).onClick = CloseCall;

        mparent = UguiMaker.newGameObject("btns", transform);
        mparent.transform.localPosition = new Vector3(0f, -110f, 0f);

        Vector3[] vposs = new Vector3[5];
        vposs[0] = new Vector3(-86f, 11f, 0f);
        vposs[1] = new Vector3(-72f, -48f, 0f);
        vposs[2] = new Vector3(0f, -85f, 0f);
        vposs[3] = new Vector3(72f, -50f, 0f);
        vposs[4] = new Vector3(88f, 11f, 0f);
        for (int i = 0; i < 5; i++)
        {
            imgBtns[i] = UguiMaker.newImage("btn" + (i + 1), transform, "whoflyfast_sprite", "yuntype2_2");
            imgBtns[i].transform.localPosition = vposs[i];
            EventTriggerListener.Get(imgBtns[i].gameObject).onClick = ClickCall;
            imgNum[i] = UguiMaker.newImage("num" + (i + 1), imgBtns[i].transform, "number_slim", (i + 1).ToString(), false);
            imgNum[i].color = new Color(124f / 255, 27f / 255, 30f / 255, 1f);
            imgNum[i].transform.localScale = Vector3.one * 0.5f;

            imgBtns[i].transform.SetParent(mparent.transform);
        }
    }

    private void ClickCall(GameObject _go)
    {
        int getID = 0;
        if (_go.name.Contains("1"))
        { getID = 1; }
        else if (_go.name.Contains("2"))
        { getID = 2; }
        else if (_go.name.Contains("3"))
        { getID = 3; }
        else if (_go.name.Contains("4"))
        { getID = 4; }
        else if (_go.name.Contains("5"))
        { getID = 5; }

        if (scaleTween != null)
            scaleTween.Pause();
        _go.transform.localScale = Vector3.one;

        scaleTween = _go.transform.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(() =>
         {
             scaleTween = _go.transform.DOScale(Vector3.one, 0.15f).OnComplete(()=> 
             {
                 HideInputObj();
             });
         });

        if (targetRocket != null)
        {
            mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("whoflyfast_sound", "setok"));
            targetRocket.SetNumber(getID);
        }

        mCtrl.GuideStop();
        mCtrl.DesGuide();
    }


    public void ShowInputObj(WFF_RocketObj _target)
    {
        mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("whoflyfast_sound", "shownuminput"));
        targetRocket = _target;
        transform.position = _target.transform.position;
        transform.localPosition = transform.localPosition + new Vector3(-4f, -6f, 0f);
        mparent.transform.localScale = new Vector3(1f, 0.001f, 1f);
        gameObject.SetActive(true);
        mparent.transform.DOScaleY(1f, 0.2f).OnComplete(()=> 
        {
            _target.transform.SetSiblingIndex(10);
            int orderId = mCtrl.GetNowOrder(_target);
            Vector3 guidePos = imgBtns[orderId - 1].transform.position;
            mCtrl.GuideClick(guidePos, new Vector3(10f,-30f,0f));
        });
        _target.SetInputTipActive(true);
    }

    public void HideInputObj()
    {
        if (targetRocket != null)
        {
            targetRocket.SetInputTipActive(false);
        }
        targetRocket = null;
        mparent.transform.DOScaleY(0.001f, 0.2f).OnComplete(()=> 
        {
            gameObject.SetActive(false);
        });
        mCtrl.GuideStop();
    }

    public void CloseCall(GameObject _go)
    {
        HideInputObj();
    }

}
