using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class RegularOrderBlockObj : MonoBehaviour
{
    /// <summary>
    /// 1随机动物 2花 3随机动物
    /// </summary>
    public int nType = 0;
    public bool bStation = false;
    public bool bKong = false;
    /// <summary>
    /// 是否可以改变图标
    /// </summary>
    public bool bCanChange = false;

    [HideInInspector]
    public RectTransform rectTransform;

    private Image picbg2;
    private Image picbg1;
    private Image pic2;
    private Image pic1;

    private Button btn;

    public void InitAwake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();

        pic1 = transform.Find("pic1").GetComponent<Image>();
        pic2 = transform.Find("pic2").GetComponent<Image>();
        picbg1 = transform.Find("picbg1").GetComponent<Image>();
        picbg2 = transform.Find("picbg2").GetComponent<Image>();

        btn = gameObject.GetComponent<Button>();
        EventTriggerListener.Get(gameObject).onClick = BtnClick;
    }

    /// <summary>
    /// sprite设置
    /// </summary>
    public void SetSprites()
    {       
        pic2.sprite = ResManager.GetSprite("regularorder_sprite", "pic2");        
        picbg2.sprite = ResManager.GetSprite("regularorder_sprite", "picbg2");

        RegularOrderCtrl mctrl = SceneMgr.Instance.GetNowScene() as RegularOrderCtrl;

        List<int> animalList = mctrl.mTheAnimalType;
        pic1.sprite = ResManager.GetSprite("regularorder_sprite", "ani" + animalList[0]);
        picbg1.sprite = ResManager.GetSprite("regularorder_sprite", "ani" + animalList[1]);
       
        if (mctrl.nLevel <= 2)
        {
            picbg1.rectTransform.sizeDelta = new Vector2(94f, 82f);
            picbg2.rectTransform.sizeDelta = new Vector2(94f, 82f);
            pic1.rectTransform.sizeDelta = new Vector2(78f, 78f);
            pic2.rectTransform.sizeDelta = new Vector2(78f, 78f);
        }
        else
        {
            picbg1.rectTransform.sizeDelta = new Vector2(105, 105);
            picbg2.rectTransform.sizeDelta = new Vector2(140f, 110f);
            pic1.rectTransform.sizeDelta = new Vector2(105f, 105f);
            pic2.rectTransform.sizeDelta = new Vector2(105f, 105f);
        }
    }

    /// <summary>
    /// 设置类型
    /// </summary>
    /// <param name="_type"></param>
    public void SetType(int _type)
    {
        HidePics();
        nType = _type;
        switch (_type)
        {
            case 0:
                picbg2.gameObject.SetActive(true);
                break;
            case 1:
                pic1.gameObject.SetActive(true);
                break;
            case 2:
                pic2.gameObject.SetActive(true);
                break;
            case 3:
                if (!bStation)
                {
                    picbg1.gameObject.SetActive(true);
                }
                RegularOrderCtrl mctrl = SceneMgr.Instance.GetNowScene() as RegularOrderCtrl;
                if (mctrl.nLevel >= 3)
                {
                    picbg1.gameObject.SetActive(true);
                }
                break;
            default:
                break;
        }

        if (bKong)
        {
            HidePics();
            picbg2.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 隐藏blockObj所有元素
    /// </summary>
    public void HidePics()
    {
        pic1.gameObject.SetActive(false);
        pic2.gameObject.SetActive(false);
        picbg1.gameObject.SetActive(false);
        picbg2.gameObject.SetActive(false);
    }

    /// <summary>
    /// 拖动设置
    /// </summary>
    public void DropSet()
    {
        transform.DOScale(Vector3.one * 1.5f, 0.2f); 
    }

    /// <summary>
    /// 拖动重置
    /// </summary>
    public void DropReset()
    {
        transform.DOScale(Vector3.one, 0.2f);
    }

    /// <summary>
    /// 左右震荡
    /// </summary>
    /// <param name="_callback"></param>
    public void DoShakeLR(System.Action _callback = null)
    {
        Vector3 vOld = transform.localPosition;
        transform.DOLocalMove(vOld + new Vector3(15f, 0f, 0f), 0.15f).OnComplete(() => 
        {
            transform.DOLocalMove(vOld + new Vector3(-15f, 0f, 0f), 0.3f).OnComplete(() => 
            {
                transform.DOLocalMove(vOld, 0.3f).OnComplete(() => 
                {
                    if (_callback != null)
                    { _callback(); }
                });
            });
        });
    }

    /// <summary>
    /// 拖进 station set Lv1,Lv2
    /// </summary>
    /// <param name="_drop"></param>
    public bool StationDropInSet(int _mSelectType)
    {
        bool bokok = false;
        bKong = false;
        bCanChange = false;

        int oldType = nType;
        SetType(_mSelectType);

        if (oldType == _mSelectType)
        {        
            bokok = true;
            transform.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(()=> 
            {
                transform.DOScale(Vector3.one, 0.2f);
            });
        }

        if (!bokok)
        {
            DoShakeLR(() => 
            {
                bKong = true;
                bCanChange = true;
                SetType(oldType);            
            });
        }

        return bokok;
    }


    public void OKSeting()
    {
        transform.localScale = Vector3.one * 0.001f;
        transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 变大效果
    /// </summary>
    public void DoBigEffect()
    {
        transform.DOScale(Vector3.one * 1.3f, 0.25f).OnComplete(()=> 
        {
            transform.DOScale(Vector3.one, 0.25f);
        });
    }

    public void SceneMove(bool _in)
    {
        if (_in)
        {
            transform.localScale = Vector3.one * 0.001f;
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        }
    }



    private void BtnClick(GameObject go)
    {
        RegularOrderCtrl mctrl = SceneMgr.Instance.GetNowScene() as RegularOrderCtrl;
        if (mctrl != null)
        {
            mctrl.SetSelectObj(this);
        }
    }


    public void ResetInfos()
    {
        SetType(0);
        bKong = false;
        bCanChange = false;
    }


}
