using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class KnowCalendarLv2 : MonoBehaviour
{
    public int nToday = 0;
    public int nFinishCount = 0;

    private KnowCalendarCtrl mCtrl;
    private KnowCalendarMonth mMonthCtrl;
    private KnowCalendarQuesPanellv2 mQuesPanel;

    private InputNumObj mInputNumCtrl;
    private Image imgGuangbiao;
    private InputWeekObj mInputWeekCtrl;

    private Vector3 vMonth = new Vector3(-270f, -35f, 0f);
    private Vector3 vQuesPanel = new Vector3(350f, 50f, 0f);

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as KnowCalendarCtrl;
        mInputNumCtrl = mCtrl.mInputNumCtrl;
        imgGuangbiao = mCtrl.imgGuangbiao;
        mInputWeekCtrl = mCtrl.mInputWeekCtrl;
    }

    public void ResetInfos()
    {
        if (mMonthCtrl != null)
        {
            if (mMonthCtrl.gameObject != null)
                GameObject.Destroy(mMonthCtrl.gameObject);
        }
        mMonthCtrl = null;
        if (mQuesPanel != null)
        {
            if (mQuesPanel.gameObject != null)
                GameObject.Destroy(mQuesPanel.gameObject);
        }
        mQuesPanel = null;
        nFinishCount = 0;
    }

    public void SetData()
    {
        ResetInfos();

        int nowyear = System.DateTime.Now.Year;
        int nowmonth = System.DateTime.Now.Month;
        nToday = System.DateTime.Now.Day;

        mMonthCtrl = mCtrl.CreateMonth(transform, nowyear, nowmonth);
        mMonthCtrl.ChangeMonthBtnActive(false);
        mMonthCtrl.transform.localPosition = vMonth;
        //天数create
        mMonthCtrl.InitMonthData();

        int ndays = mMonthCtrl.mDayList.Count;
        for (int i = 0; i < ndays; i++)
        {
            mMonthCtrl.mDayList[i].ButtonActive(false);
            if (mMonthCtrl.mDayList[i].nDay == nToday)
            {
                mMonthCtrl.mDayList[i].ImgBGActive(true);
            }
        }

        mQuesPanel = UguiMaker.newGameObject("quspanel", transform).AddComponent<KnowCalendarQuesPanellv2>();
        mQuesPanel.InitAwake();
        mQuesPanel.transform.localPosition = vQuesPanel;
    }


    private KnowCalendarQueslv2 theQua = null;
    #region//日期点击检测-----
    /// <summary>
    /// 点击回调
    /// </summary>
    /// <param name="_qua"></param>
    public void ClickCheckDay(KnowCalendarQueslv2 _qua)
    {
        if (mInputNumCtrl.gameObject.activeSelf)
            return;
        if (_qua.bShakeFinish == false)
            return;
        mInputNumCtrl.ResetInfos();
        mInputNumCtrl.transform.position = _qua.transform.position;
        Vector3 vshow = mInputNumCtrl.transform.localPosition + new Vector3(0f, -170f, 0f); //new Vector3(-230f, 0f, 0f);
        mInputNumCtrl.transform.localPosition = vshow;
        mInputNumCtrl.ShowEffect();

        theQua = _qua;
        //光标位置
        imgGuangbiao.gameObject.SetActive(true);
        imgGuangbiao.transform.position = _qua.mNum0.transform.position;
        theQua.SetWenhao0Active(false);

        QuaInputNumCallBack();
    }
    /// <summary>
    /// 输入数字
    /// </summary>
    public void QuaInputNumCallBack()
    {
        if (theQua == null)
            return;
        int theGet = 0;
        string strGet = mInputNumCtrl.strInputNum;
        if (int.TryParse(strGet, out theGet))
        {
            theQua.mNum0.gameObject.SetActive(true);
            theQua.mNum0.SetNumber(theGet);
            //光标位置调整
            mCtrl.SetGuangbiaoPos(theQua.transform, strGet, theQua.mNum0);
        }
        else
        {
            theQua.mNum0.gameObject.SetActive(false);
            imgGuangbiao.transform.position = theQua.mNum0.transform.position;
        }
    }
    /// <summary>
    /// 输入数字完成
    /// </summary>
    public void QuaInputNumFinishCallBack()
    {
        if (theQua == null)
            return;
        theQua.mNum0.SetNumber(mInputNumCtrl.nInputNum);
        theQua.mNum0.gameObject.SetActive(true);
        imgGuangbiao.gameObject.SetActive(false);

        bool checkok = false;
        if (theQua.nQuaID == 0)//昨天
        {
            int yestoday = GetOldDay();
            if (yestoday == theQua.mNum0.nNumber)
            {
                checkok = true;
            }
        }
        else if (theQua.nQuaID == 1) //今天
        {
            if (nToday == theQua.mNum0.nNumber)
            {
                checkok = true;
            }
        }
        else //后天
        {
            int nextday = GetNextDay();
            if (nextday == theQua.mNum0.nNumber)
            {
                checkok = true;
            }
        }

        if (checkok)
        {
            theQua.Btn0Active(false);
            CheckIsPass();
        }
        else
        {
            theQua.ShakeObj0();
        }
    }
    /// <summary>
    /// 输入清除
    /// </summary>
    public void QuaInputNumClearCallBack()
    {
        if (theQua == null)
            return;
        int theGet = 0;
        string strGet = mInputNumCtrl.strInputNum;
        if (int.TryParse(strGet, out theGet))
        {
            theQua.mNum0.gameObject.SetActive(true);
            theQua.mNum0.SetNumber(theGet);
            //光标位置调整
            mCtrl.SetGuangbiaoPos(theQua.transform, strGet, theQua.mNum0);
        }
        else
        {
            theQua.mNum0.gameObject.SetActive(false);
            imgGuangbiao.transform.position = theQua.mNum0.transform.position;
        }
    }

    /// <summary>
    /// 获取今天
    /// </summary>
    public int GetToDay()
    {
        return nToday;
    }
    /// <summary>
    /// 获取昨天
    /// </summary>
    public int GetOldDay()
    {
        if (nToday == 1)
        {
            int year = mMonthCtrl.nYear;
            int month = mMonthCtrl.nMonth - 1;
            if (month < 1)
            {
                month = 12;
                year = mMonthCtrl.nYear - 1;
            }
            int ndays = System.DateTime.DaysInMonth(year, month);
            return ndays;
        }
        else
        {
            return nToday - 1;
        }
    }
    /// <summary>
    /// 获取后天
    /// </summary>
    public int GetNextDay()
    {
        int year = mMonthCtrl.nYear;
        int month = mMonthCtrl.nMonth;
        int ndays = System.DateTime.DaysInMonth(year, month);
        if (nToday == ndays)
            return 1;
        else
            return (nToday + 1);
    }
    #endregion

    #region//星期点击检测-----
    public void ClickCheckWeek(KnowCalendarQueslv2 _qua)
    {
        if (mInputWeekCtrl.gameObject.activeSelf)
            return;
        if (_qua.bShakeFinish == false)
            return;
        mInputWeekCtrl.strInputNum = "";
        mInputWeekCtrl.transform.position = _qua.mImg1.transform.position;
        Vector3 vshow = mInputWeekCtrl.transform.localPosition + new Vector3(-30f, -170f, 0f);
        mInputWeekCtrl.transform.localPosition = vshow;
        mInputWeekCtrl.ShowEffect();

        theQua = _qua;
        //光标位置
        imgGuangbiao.gameObject.SetActive(true);
        imgGuangbiao.transform.position = _qua.mImg1.transform.position;
        theQua.SetWenhao1Active(false);

        QuaInputWeekCallBack();
    }
    public void QuaInputWeekCallBack()
    {
        if (theQua == null)
            return;
        int theGet = 0;
        string strGet = mInputWeekCtrl.strInputNum;
        if (int.TryParse(strGet, out theGet))
        {
            theQua.mImg1.gameObject.SetActive(true);
            theQua.SetWeekSprite(theGet);
            //光标位置调整
            imgGuangbiao.transform.position = theQua.mImg1.transform.position;
            Vector3 vpos = imgGuangbiao.transform.localPosition + new Vector3(20f, 0f, 0f);
            imgGuangbiao.transform.localPosition = vpos;
        }
        else
        {
            theQua.mImg1.gameObject.SetActive(false);
            imgGuangbiao.transform.position = theQua.mImg1.transform.position;         
        }
    }
    public void QuaInputWeekFinishCallBack()
    {
        if (theQua == null)
            return;

        imgGuangbiao.gameObject.SetActive(false);

        if (mInputWeekCtrl.strInputNum.CompareTo("") == 0)
        {
            theQua.SetWenhao1Active(true);
            return;
        }

        theQua.SetWeekSprite(mInputWeekCtrl.nInputNum);
        theQua.mImg1.gameObject.SetActive(true);

        int nweek = 0;
        if (theQua.nQuaID == 0)//昨天
        {
            nweek = GetOldWeek(); 
        }
        else if (theQua.nQuaID == 1) //今天
        {
            nweek = GetNowWeek();
        }
        else //后天
        {
            nweek = GetNextWeek();
        }

        if (nweek == mInputWeekCtrl.nInputNum)
        {
            theQua.Btn1Active(false);
            CheckIsPass();
        }
        else
        {
            theQua.ShakeObj1();
        }
    }

    public int GetOldWeek()
    {
        int nowweek = (int)System.DateTime.Now.DayOfWeek;
        int oldweek = nowweek - 1;
        if (oldweek < 0)
            return 6;
        else
            return oldweek;
    }
    public int GetNowWeek()
    {
        int nowweek = (int)System.DateTime.Now.DayOfWeek;
        return nowweek;
    }
    public int GetNextWeek()
    {
        int nowweek = (int)System.DateTime.Now.DayOfWeek;
        int nextweek = nowweek + 1;
        if (nextweek > 6)
            nextweek = 0;
        return nextweek;
    }
    #endregion


    /// <summary>
    /// 检测到下一关
    /// </summary>
    public void CheckIsPass()
    {
        nFinishCount++;
        if (nFinishCount == 2)
        {
            mQuesPanel.ShowQua(0);
            mCtrl.SetTipSound(ieSoundTipLv2());
            mCtrl.StartTipSound();
        }
        if (nFinishCount == 4)
        {
            mQuesPanel.ShowQua(2);
            mCtrl.SetTipSound(ieSoundTipLv2());
            mCtrl.StartTipSound();
        }
        if (nFinishCount >= 6)
        {
            Debug.Log("lv2 pass");
            mCtrl.LevelCheckNext();
        }
    }



    public void SceneMove(bool _in)
    {
        if (_in)
        {
            mMonthCtrl.transform.localPosition = vMonth + new Vector3(-1000f, 0f, 0f);
            mQuesPanel.transform.localPosition = vQuesPanel + new Vector3(1000f, 0f, 0f);
            mMonthCtrl.transform.DOLocalMove(vMonth, 1f);
            mQuesPanel.transform.DOLocalMove(vQuesPanel, 1f);
        }
        else
        {
            mMonthCtrl.transform.DOLocalMove(vMonth + new Vector3(-1000f, 0f, 0f), 1f);
            mQuesPanel.transform.DOLocalMove(vQuesPanel + new Vector3(1000f, 0f, 0f), 1f);
        }
    }


    /// <summary>
    /// 提问语音
    /// </summary>
    /// <returns></returns>
    public IEnumerator ieSoundTipLv2()
    {
        yield return new WaitForSeconds(0.1f);
        AudioClip cp = null;
        if (nFinishCount < 2)
        {
            cp = ResManager.GetClip("knowcalendar_sound", "game-tips_lv2_0");
        }
        else if (nFinishCount >= 2 && nFinishCount < 4)
        {
            cp = ResManager.GetClip("knowcalendar_sound", "game-tips_lv2_1");
        }
        else
        {
            cp = ResManager.GetClip("knowcalendar_sound", "game-tips_lv2_2");
        }
        mCtrl.mSoundCtrl.PlaySound(cp, 1f);
    }


}
