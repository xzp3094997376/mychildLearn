using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 天obj
/// </summary>
public class KnowCalendarDay : MonoBehaviour
{

    public int nYear;
    public int nMonth;
    public int nDay;

    private Image img0;
    private Button btn0;

    private Image img1;

    private GameObject imgNumberObj;
    public ImageNumber imgNumber;

    private BoxCollider2D mbox2D;

    private System.Action<KnowCalendarDay> mClickCallBack = null;

    public void InitAwake(int _year, int _month, int _day, bool _nullObj = false)
    {
        nYear = _year;
        nMonth = _month;
        nDay = _day;

        img0 = gameObject.GetComponent<Image>();
        btn0 = gameObject.AddComponent<Button>();
        btn0.transition = Selectable.Transition.None;
        EventTriggerListener.Get(btn0.gameObject).onClick = ClickBtn;

        img1 = UguiMaker.newImage("img1", transform, "knowcalendar_sprite", "kc_daylost", false);
        img1.rectTransform.sizeDelta = new Vector2(85f, 72f);
        img1.enabled = false;

        mbox2D = gameObject.AddComponent<BoxCollider2D>();
        mbox2D.size = new Vector2(85f, 72f);
        mbox2D.enabled = false;

        imgNumberObj = UguiMaker.newGameObject("imgNumber", transform);
        Image num1 = UguiMaker.newImage("num1", imgNumberObj.transform, "knowcalendar_sprite", "kc_num0", false);
        Image num2 = UguiMaker.newImage("num2", imgNumberObj.transform, "knowcalendar_sprite", "kc_num0", false);
        num1.SetNativeSize();
        num2.SetNativeSize();
        imgNumber = imgNumberObj.AddComponent<ImageNumber>();
        imgNumber.strABName = "knowcalendar_sprite";
        imgNumber.strFirstPicName = "kc_num";
        imgNumber.fIndex = 2;
        imgNumber.InitAwake();
        imgNumber.SetNumber(_day);
        imgNumber.SetNumColor(new Color(104f / 255, 13f / 255, 2f / 255, 1f));
        imgNumber.transform.localScale = Vector3.one * 1f;

        gameObject.name = "day" + _day;
        if (_nullObj)
        {
            imgNumber.gameObject.SetActive(false);
            gameObject.name = "nullday" + _day;
        }
    }


    public void SetLostDay()
    {
        img1.enabled = true;
        imgNumberObj.SetActive(false);
    }

    public void ShowTheDay()
    {
        img1.enabled = false;
        imgNumberObj.SetActive(true);
    }

    /// <summary>
    /// 碰撞体开/关
    /// </summary>
    /// <param name="_active"></param>
    public void BoxColliderActive(bool _active)
    {
        mbox2D.enabled = _active;
    }

    /// <summary>
    /// 背景开/关
    /// </summary>
    public void ImgBGActive(bool _active)
    {
        img1.enabled = _active;
    }

    /// <summary>
    /// 按钮事件开/关
    /// </summary>
    /// <param name="_active"></param>
    public void ButtonActive(bool _active)
    {
        img0.raycastTarget = _active;
    }

    /// <summary>
    /// 设置点击回调
    /// </summary>
    /// <param name="_callback"></param>
    public void SetClickCallBack(System.Action<KnowCalendarDay> _callback)
    {
        mClickCallBack = _callback;
    }

    private void ClickBtn(GameObject _go)
    {
        if (mClickCallBack != null)
        {
            mClickCallBack(this);
        }
    }


    public void ShakeObj(System.Action _call = null)
    {
        float ft = 0.14f;
        transform.DOLocalRotate(new Vector3(0f, 0f, -15f), ft).OnComplete(() => 
        {
            transform.DOLocalRotate(new Vector3(0f, 0f, 15f), ft * 2f).OnComplete(() => 
            {
                transform.DOLocalRotate(new Vector3(0f, 0f, 0f), ft).OnComplete(()=> 
                {
                    if (_call != null)
                        _call();
                });
            });
        });
    }

}
