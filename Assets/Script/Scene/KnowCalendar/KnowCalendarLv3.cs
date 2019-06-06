using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class KnowCalendarLv3 : MonoBehaviour
{
    private int nToCount = 3;
    public int nCount = 0;

    private bool bInit = false;
    private KnowCalendarCtrl mCtrl;
    private KnowCalendarMonth mMonthCtrl;

    private Vector3 vMonth = new Vector3(-270f, -35f, 0f);

    /// <summary>
    /// 节日数据
    /// </summary>
    public class FestivalData
    {
        public int nMonth = 1;
        public int nDay = 1;
        public int nID = 1;
        public FestivalData(int _mon,int _day,int _id)
        {
            nMonth = _mon;
            nDay = _day;
            nID = _id;
        }
    }

    private List<FestivalData> mFestivalDataList = new List<FestivalData>();
    /// <summary>
    /// 头像列表
    /// </summary>
    public List<KnowCalendarDayHead> mDayHeadList = new List<KnowCalendarDayHead>();
    /// <summary>
    /// 头像station列表
    /// </summary>
    public List<KnowCalendarDayHeadStation> mDHStationList = new List<KnowCalendarDayHeadStation>();

    /// <summary>
    /// init
    /// </summary>
    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as KnowCalendarCtrl;

        InitFestivalDataList();
        InitPosList();

        bInit = true;
    }

    /// <summary>
    /// reset
    /// </summary>
    public void ResetInfos()
    {
        nCount = 0;
        nToCount = 7;
        for (int i = 0; i < mDayHeadList.Count; i++)
        {
            if (mDayHeadList[i].gameObject != null)
                GameObject.Destroy(mDayHeadList[i].gameObject);
        }
        mDayHeadList.Clear();

        if (mMonthCtrl != null)
        {
            if (mMonthCtrl.gameObject != null)
                GameObject.Destroy(mMonthCtrl.gameObject);
        }
        mMonthCtrl = null;

        for (int i = 0; i < mDHStationList.Count; i++)
        {
            if (mDHStationList[i].gameObject != null)
                GameObject.Destroy(mDHStationList[i].gameObject);
        }
        mDHStationList.Clear();
    }

    /// <summary>
    /// set data
    /// </summary>
    public void SetData()
    {
        ResetInfos();

        int nowyear = System.DateTime.Now.Year;

        mMonthCtrl = mCtrl.CreateMonth(transform, nowyear, 1);
        mMonthCtrl.transform.localPosition = vMonth;
        //天数create
        mMonthCtrl.InitMonthData();
        //开启碰撞体
        mMonthCtrl.DaysBoxColliderActive(true);
        //设置切换前回调
        mMonthCtrl.SetChangeMonthBeforeCallBack(ChangeMonthBeforeCallBack);
        //设置切换后回调
        mMonthCtrl.SetChangeMonthCallBack(ChangeMonthCallBack);

        nToCount = 3;
        mFestivalDataList = Common.BreakRank(mFestivalDataList);
        for (int i = 0; i < nToCount; i++)
        {
            KnowCalendarDayHead dayhead = CreateDayHead(mFestivalDataList[i]);
            dayhead.transform.localPosition = vPosList[i];
            dayhead.SetRemenberPos(vPosList[i]);
            mDayHeadList.Add(dayhead);

            dayhead.transform.localScale = Vector3.one * 0.001f;
            dayhead.gameObject.SetActive(false);
        }

        //显示头像
        ShowDayHeadObj(nCount);
    }

    /// <summary>
    /// 显示头像
    /// </summary>
    /// <param name="_index"></param>
    public void ShowDayHeadObj(int _index)
    {
        Vector2 vbase = new Vector2(360f, 0f) + Random.insideUnitCircle * 50f;
        mDayHeadList[_index].transform.localPosition = vbase;
        mDayHeadList[_index].gameObject.SetActive(true);
        mDayHeadList[_index].DoScale(1f);
        mCtrl.PlayTipSound();
    }


    /// <summary>
    /// 节日头像创建
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    private KnowCalendarDayHead CreateDayHead(FestivalData _data)
    {
        int _month = _data.nMonth;
        int _day = _data.nDay;
        int _festivalID = _data.nID;
        GameObject mgo = UguiMaker.newGameObject("festival" + _festivalID, transform);
        KnowCalendarDayHead dayhead = mgo.AddComponent<KnowCalendarDayHead>();
        dayhead.InitAwake(_month, _day, _festivalID);
        return dayhead;
    }

    /// <summary>
    /// 节日数据初始化
    /// </summary>
    private void InitFestivalDataList()
    {
        mFestivalDataList.Add(new FestivalData(5, 1, 51));
        mFestivalDataList.Add(new FestivalData(6, 1, 61));
        mFestivalDataList.Add(new FestivalData(7, 1, 71));
        mFestivalDataList.Add(new FestivalData(8, 1, 81));
        mFestivalDataList.Add(new FestivalData(10, 1, 101));
        //父生日
        mFestivalDataList.Add(new FestivalData(20, 1, 201));
        //母生日
        mFestivalDataList.Add(new FestivalData(20, 2, 202));
    }


    List<Vector3> vPosList = new List<Vector3>();
    public void InitPosList()
    {
        vPosList.Add(new Vector3(308f, 245f, 0f));
        vPosList.Add(new Vector3(508f, 220f, 0f));
        vPosList.Add(new Vector3(181f, 158f, 0f));
        vPosList.Add(new Vector3(367f, 70f, 0f));
        vPosList.Add(new Vector3(205f, -22f, 0f));
        vPosList.Add(new Vector3(520f, -16f, 0f));
        vPosList.Add(new Vector3(372f, -116f, 0f));
        vPosList.Add(new Vector3(193f, -234f, 0f));
        vPosList.Add(new Vector3(530f, -248f, 0f));
        vPosList.Add(new Vector3(369f, -294f, 0f));
    }

    void Update()
    {
        if (!bInit)
            return;
        MUpdate();
    }

    Vector3 temp_select_offset = Vector3.zero;
    KnowCalendarDayHead mSelect = null;
    private void MUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            #region//one
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hits != null)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    KnowCalendarDayHead com = hits[i].collider.gameObject.GetComponent<KnowCalendarDayHead>();
                    if (com != null)
                    {
                        if (mSelect == null)
                        { mSelect = com; }
                        else
                        {
                            if (com.transform.GetSiblingIndex() > mSelect.transform.GetSiblingIndex())
                            {
                                mSelect = com;
                            }
                        }                 
                    }
                }

                if (mSelect != null)
                {
                    mSelect.transform.SetSiblingIndex(20);
                    temp_select_offset = Common.getMouseLocalPos(transform) - mSelect.transform.localPosition;
                    mSelect.DoScale(1.3f);
                    mCtrl.mSoundCtrl.PlaySortSound("knowcalendar_sound", "numberstartdrop");
                }
            }
            #endregion
        }
        else if (Input.GetMouseButton(0))
        {
            #region//two
            if (mSelect != null)
            {
                //拖动值限制
                Vector3 vInput = Common.getMouseLocalPos(transform) - temp_select_offset;
                float fX = vInput.x;
                float fY = vInput.y;
                Vector2 vsize = mSelect.GetSize();
                fX = Mathf.Clamp(fX, -GlobalParam.screen_width * 0.5f + vsize.x * 0.5f, GlobalParam.screen_width * 0.5f - vsize.x * 0.5f);
                fY = Mathf.Clamp(fY, -GlobalParam.screen_height * 0.5f + vsize.y * 0.5f, GlobalParam.screen_height * 0.5f - vsize.y * 0.5f - 60f);
                mSelect.transform.localPosition = new Vector3(fX, fY, 0f);
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//two
            if (mSelect != null)
            {
                bool bMatch = false;
                bool bHitFaile = false;
                KnowCalendarDay comDay = null;

                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hits != null)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        comDay = hits[i].collider.gameObject.GetComponent<KnowCalendarDay>();
                        if (comDay != null)
                        {                       
                            if (mSelect.nMonth >= 20)//生日全可以
                            {
                                bMatch = true;
                                SetDayHeadIn(comDay, mSelect);
                            }
                            else //其他固定节日
                            {
                                if (mSelect.nMonth == comDay.nMonth && mSelect.nDay == comDay.nDay)
                                {
                                    bMatch = true;
                                    SetDayHeadIn(comDay, mSelect);
                                }
                                else
                                {
                                    bHitFaile = true;
                                }
                            }
                            break;
                        }
                    }
                }

                if (!bMatch)
                {
                    mSelect.DoScale(1f);
                    if (bHitFaile)
                    {
                        mSelect.MoveToRemenberPos();
                        mCtrl.mSoundCtrl.PlaySortSound("knowcalendar_sound", "numberblackto");
                    }
                    else
                    {
                        float fNowX = mSelect.transform.localPosition.x;
                        if (fNowX < 95f)
                        {
                            fNowX = 95f;
                            mSelect.SetRemenberPos(new Vector3(fNowX, mSelect.transform.localPosition.y, 0f));
                            mSelect.MoveToRemenberPos();
                        }
                        else
                            mSelect.SetRemenberPos(mSelect.transform.localPosition);
                    }
                }

                mSelect = null;
            }
            #endregion
        }
    }


    private void SetDayHeadIn(KnowCalendarDay _day, KnowCalendarDayHead _dayhead)
    {
        //_day.BoxColliderActive(false);
        _dayhead.BoxColliderActive(false);
        _dayhead.bIsOK = true;
        _dayhead.transform.position = _day.transform.position;
        _dayhead.transform.localScale = Vector3.one;
        //生日设置为该日期
        if (_dayhead.nMonth >= 20)
        {
            _dayhead.nMonth = _day.nMonth;
            _dayhead.nDay = _day.nDay;
        }
        //将head分配到station
        SetDayHeadToStation(_dayhead, _day);

        mCtrl.PlayDropOKFX(_day.transform.position);

        mCtrl.mSoundCtrl.PlaySortSound("knowcalendar_sound", "numbersetok");
        //CheckFinish();
        PlayFestivalSound(_dayhead, CheckFinish);
    }

    /// <summary>
    /// 将head分配到station
    /// </summary>
    /// <param name="_head"></param>
    public void SetDayHeadToStation(KnowCalendarDayHead _head, KnowCalendarDay _day)
    {
        KnowCalendarDayHeadStation st = null;
        for (int i = 0; i < mDHStationList.Count; i++)
        {
            if (_head.nMonth == mDHStationList[i].nMonth && _head.nDay == mDHStationList[i].nDay)
            {
                st = mDHStationList[i];
            }
        }
        //station空就创建一个station
        if (st == null)
        {
            st = CreateDayHeadStation(_head.nMonth, _head.nDay);
            st.transform.position = _day.transform.position;
            //st.transform.SetParent(_day.transform);
            //st.transform.SetSiblingIndex(0);
        }
        st.AddDayHead(_head);
    }

    /// <summary>
    /// 切换月份前回调(将station parent 设置为transform)
    /// </summary>
    public void ChangeMonthBeforeCallBack()
    {
        for (int i = 0; i < mDHStationList.Count; i++)
        {
            mDHStationList[i].transform.SetParent(transform);
        }
    }

    /// <summary>
    /// 切换月份后回调(检测显示已经标好的icon)
    /// </summary>
    public void ChangeMonthCallBack()
    {
        for (int i = 0; i < mDHStationList.Count; i++)
        {
            mDHStationList[i].gameObject.SetActive(false);
            if (mDHStationList[i].nMonth == mMonthCtrl.nMonth)
            {
                KnowCalendarDay _day = mMonthCtrl.GetDayByID(mDHStationList[i].nDay);
                //mDHStationList[i].transform.SetParent(_day.transform);
                //mDHStationList[i].transform.localPosition = Vector3.zero;
                //mDHStationList[i].transform.SetSiblingIndex(0);
                mDHStationList[i].gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 创建一个头像station
    /// </summary>
    public KnowCalendarDayHeadStation CreateDayHeadStation(int _month,int _day)
    {
        GameObject mgo = UguiMaker.newGameObject("headStation" + _month + "_" + _day, transform);
        KnowCalendarDayHeadStation st = mgo.AddComponent<KnowCalendarDayHeadStation>();
        st.nMonth = _month;
        st.nDay = _day;
        mDHStationList.Add(st);
        return st;
    }

    /// <summary>
    /// 通关检测
    /// </summary>
    private void CheckFinish()
    {
        nCount++;
        if (nCount < nToCount)
        {
            ShowDayHeadObj(nCount);
        }
        if (nCount >= nToCount)
        {
            mMonthCtrl.ChangeMonthBtnClickActive(false);
            mCtrl.LevelCheckNext();
        }
    }

    public void SceneMove(bool _in)
    {
        if (_in)
        {
            mMonthCtrl.transform.localPosition = vMonth + new Vector3(-1000f, 0f, 0f);
            mMonthCtrl.transform.DOLocalMove(vMonth, 1f);
        }
        else
        {
            mMonthCtrl.transform.DOLocalMove(vMonth + new Vector3(-1000f, 0f, 0f), 1f);
        }
    }


    /// <summary>
    /// 提示语音
    /// </summary>
    /// <returns></returns>
    public IEnumerator iePlayTipSound()
    {
        yield return new WaitForSeconds(0.1f);
        KnowCalendarDayHead head = mDayHeadList[nCount];

        if (head.nId > 200)
        {
            AudioClip cp = ResManager.GetClip("knowcalendar_sound", "obj" + head.nId);
            mCtrl.mSoundCtrl.PlaySound(cp, 1);
        }
        else
        {
            List<AudioClip> cpList = new List<AudioClip>();
            cpList.Add(ResManager.GetClip("knowcalendar_sound", "qingnijiang"));
            cpList.Add(ResManager.GetClip("knowcalendar_sound", "obj" + head.nId));
            cpList.Add(ResManager.GetClip("knowcalendar_sound", "puto" + head.nId));
            for (int i = 0; i < cpList.Count; i++)
            {
                mCtrl.mSoundCtrl.PlaySound(cpList[i], 1);
                yield return new WaitForSeconds(cpList[i].length);
            }
        }
    }

    /// <summary>
    /// 播放节日语音
    /// </summary>
    /// <param name="_head"></param>
    public void PlayFestivalSound(KnowCalendarDayHead _head,System.Action _callback = null)
    {
        mtheCallback = _callback;      
        mCtrl.StopTipSound();
        mCtrl.SetTipSound(iePlayFestivalSound(_head));
        mCtrl.bPlayOtherTip = true;
        mCtrl.StartTipSound();
    }
    System.Action mtheCallback = null;
    private IEnumerator iePlayFestivalSound(KnowCalendarDayHead head)
    {
        yield return new WaitForSeconds(0.1f);
        if (head.nId > 200)
        {        
            List<AudioClip> cpList = new List<AudioClip>();
            cpList.Add(ResManager.GetClip("knowcalendar_sound", "festival" + head.nId + "_0"));
            cpList.Add(ResManager.GetClip("number_sound", head.nMonth.ToString()));
            cpList.Add(ResManager.GetClip("knowcalendar_sound", "month"));
            cpList.Add(ResManager.GetClip("number_sound", head.nDay.ToString()));
            cpList.Add(ResManager.GetClip("knowcalendar_sound", "day"));
            cpList.Add(ResManager.GetClip("knowcalendar_sound", "festival" + head.nId + "_1"));
            for (int i = 0; i < cpList.Count; i++)
            {
                mCtrl.mSoundCtrl.PlaySound(cpList[i], 1);
                yield return new WaitForSeconds(cpList[i].length);
            }
        }
        else
        {
            AudioClip cp = ResManager.GetClip("knowcalendar_sound", "festival" + head.nId);
            mCtrl.mSoundCtrl.PlaySound(cp, 1);
            yield return new WaitForSeconds(cp.length);
        }
        mCtrl.bPlayOtherTip = false;

        if (mtheCallback != null)
        { mtheCallback(); }
    }

}
