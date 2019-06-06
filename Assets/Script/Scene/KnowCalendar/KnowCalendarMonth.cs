using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 月obj
/// </summary>
public class KnowCalendarMonth : MonoBehaviour
{
    public int nYear = 2018;
    public int nMonth = 1;

    private List<KnowCalendarDay> mOldDayList = new List<KnowCalendarDay>();
    public List<KnowCalendarDay> mDayList = new List<KnowCalendarDay>();

    private Image imgMonthTip;
    private Button btnLeft;
    private Button btnRight;
    private GridLayoutGroup mdays;

    private System.Action<KnowCalendarDay> mClickDayCallBack = null;
    /// <summary>
    /// 设置点击回调
    /// </summary>
    /// <param name="_callback"></param>
    public void SetClickCallBack(System.Action<KnowCalendarDay> _callback)
    {
        mClickDayCallBack = _callback;
    }

    private System.Action mChangeMonthCallBack = null;
    public void SetChangeMonthCallBack(System.Action _callback)
    {
        mChangeMonthCallBack = _callback;
    }

    private System.Action mChangeMonthBeforeCallBack = null;
    public void SetChangeMonthBeforeCallBack(System.Action _callback)
    {
        mChangeMonthBeforeCallBack = _callback;
    }


    /// <summary>
    /// 初始化信息
    /// </summary>
    public void InitAwake(int _year,int _month)
    {
        nYear = _year;
        nMonth = _month;

        imgMonthTip = UguiMaker.newImage("monthtip", transform, "knowcalendar_sprite", "yue1", false);
        imgMonthTip.transform.localPosition = new Vector3(-20f, 280f, 0f);

        btnLeft = UguiMaker.newButton("btnLeft", transform, "knowcalendar_sprite", "kc_img0");
        btnLeft.transition = Selectable.Transition.None;
        btnLeft.transform.localPosition = imgMonthTip.transform.localPosition + new Vector3(-135f, 0f, 0f);
        EventTriggerListener.Get(btnLeft.gameObject).onDown = ClickChangMonthDown;
        EventTriggerListener.Get(btnLeft.gameObject).onUp = ClickChangeMonthUp;

        btnRight = UguiMaker.newButton("btnRight", transform, "knowcalendar_sprite", "kc_img0");
        btnRight.transition = Selectable.Transition.None;
        btnRight.transform.localPosition = imgMonthTip.transform.localPosition + new Vector3(145f, 0f, 0f);
        btnRight.transform.localScale = new Vector3(-1f, 1f, 1f);
        EventTriggerListener.Get(btnRight.gameObject).onDown = ClickChangMonthDown;
        EventTriggerListener.Get(btnRight.gameObject).onUp = ClickChangeMonthUp;

        GameObject mdaysobj = UguiMaker.newGameObject("mdays", transform);
        mdays = mdaysobj.AddComponent<GridLayoutGroup>();
        mdays.transform.localPosition = new Vector3(1f, 102.5f, 0f);
        mdays.cellSize = new Vector2(85f, 72f);
        mdays.spacing = new Vector2(2f, 2f);
        mdays.startCorner = GridLayoutGroup.Corner.UpperLeft;
        mdays.startAxis = GridLayoutGroup.Axis.Horizontal;
        mdays.childAlignment = TextAnchor.UpperCenter;
        mdays.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        mdays.constraintCount = 7;
    }

    /// <summary>
    /// 清除日期s
    /// </summary>
    public void ClearDays()
    {
        for (int i = 0; i < mOldDayList.Count; i++)
        {
            if (mOldDayList[i].gameObject != null)
                GameObject.Destroy(mOldDayList[i].gameObject);
        }
        mOldDayList.Clear();

        for (int i = 0; i < mDayList.Count; i++)
        {
            if (mDayList[i].gameObject != null)
                GameObject.Destroy(mDayList[i].gameObject);
        }
        mDayList.Clear();
    }

    /// <summary>
    /// 重新创建日历
    /// </summary>
    /// <param name="_year">年</param>
    /// <param name="_month">月</param>
    public void ReCreateMonth(int _year, int _month)
    {
        nYear = _year;
        nMonth = _month;

        ClearDays();

        InitMonthData();
    }


    /// <summary>
    /// 创建days of month
    /// </summary>
    public void InitMonthData()
    {
        int _year = nYear;
        int _month = nMonth;
        int ndays = System.DateTime.DaysInMonth(_year, _month);

        //月份标题
        imgMonthTip.sprite = ResManager.GetSprite("knowcalendar_sprite", "yue" + _month);
        imgMonthTip.SetNativeSize();

        //判断第一天是星期几
        System.DateTime firstDayOfMonth = new System.DateTime(_year, _month, 1);
        int week = (int)firstDayOfMonth.DayOfWeek;

        //上一个月有多少天
        int olaDays = 30;
        int oldMonth = _month - 1;
        if (oldMonth <= 0)
        {
            //上一年
            oldMonth = 12;
            olaDays = System.DateTime.DaysInMonth(_year - 1, oldMonth);
        }
        else
        {
            olaDays = System.DateTime.DaysInMonth(_year, oldMonth);
        }

        //补填
        for (int i = olaDays - week + 1; i <= olaDays; i++)
        {
            KnowCalendarDay dayGO = CreateDay(_year, _month, i, true);
            mOldDayList.Add(dayGO);
        }

        for (int i = 1; i <= ndays; i++)
        {
            KnowCalendarDay dayGO = CreateDay(_year, _month, i);
            mDayList.Add(dayGO);
        }
    }
    /// <summary>
    /// 创建day
    /// </summary>
    private KnowCalendarDay CreateDay(int _year, int _month, int _day,bool _isNull = false)
    {
        GameObject mgo = UguiMaker.newGameObject("day", mdays.transform);
        Image imgday = mgo.AddComponent<Image>();
        imgday.color = new Color(0.4f, 0.4f, 0.4f, 0f);
        KnowCalendarDay dayctrl = mgo.AddComponent<KnowCalendarDay>();
        dayctrl.InitAwake(_year, _month, _day, _isNull);
        dayctrl.SetClickCallBack(mClickDayCallBack);
        return dayctrl;
    }

    /// <summary>
    /// 切换月份按钮显示/隐藏
    /// </summary>
    public void ChangeMonthBtnActive(bool _active)
    {
        btnLeft.gameObject.SetActive(_active);
        btnRight.gameObject.SetActive(_active);
    }

    /// <summary>
    /// 日期boxcollider 开/关
    /// </summary>
    /// <param name="_active"></param>
    public void DaysBoxColliderActive(bool _active)
    {
        for (int i = 0; i < mDayList.Count; i++)
        { mDayList[i].BoxColliderActive(_active); }
    }

    //click donw
    private void ClickChangMonthDown(GameObject _go)
    {
        _go.GetComponent<Image>().sprite = ResManager.GetSprite("knowcalendar_sprite", "kc_img1");
    }
    //click up
    private void ClickChangeMonthUp(GameObject _go)
    {
        _go.GetComponent<Image>().sprite = ResManager.GetSprite("knowcalendar_sprite", "kc_img0");
        int newmonth = nMonth;
        if (_go == btnLeft.gameObject)
        {
            newmonth--;
            if (newmonth < 1)
                newmonth = 1;
        }
        else
        {
            newmonth++;
            if (newmonth > 12)
                newmonth = 12;
        }

        if (newmonth == nMonth)
            return;

        //切换前回调
        if (mChangeMonthBeforeCallBack != null)
        {
            mChangeMonthBeforeCallBack();
        }

        nMonth = newmonth;
        ReCreateMonth(nYear, nMonth);
        DaysBoxColliderActive(true);

        //切换后回调
        if (mChangeMonthCallBack != null)
        {
            mChangeMonthCallBack();
        }
    }


    public void ChangeMonthBtnClickActive(bool _active)
    {
        btnLeft.GetComponent<Image>().raycastTarget = _active;
        btnRight.GetComponent<Image>().raycastTarget = _active;
    }


    public KnowCalendarDay GetDayByID(int _id)
    {
        for (int i = 0; i < mDayList.Count; i++)
        {
            if (mDayList[i].nDay == _id)
                return mDayList[i];
        }
        return null;
    }

}
