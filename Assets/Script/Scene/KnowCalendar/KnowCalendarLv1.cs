using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 关卡1
/// </summary>
public class KnowCalendarLv1 : MonoBehaviour
{

    public int nCount = 0;
    private int nToCount = 3;

    private KnowCalendarMonth mMonthCtrl;
    private KnowCalendarCtrl mCtrl;

    private Vector3 vMonth = new Vector3(-270f, -35f, 0f);

    public List<KnowCalendarLostDay> mLostDayList = new List<KnowCalendarLostDay>();

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as KnowCalendarCtrl;

        InitPosList();
    }

    public void ResetInfos()
    {
        nCount = 0;
        if (mMonthCtrl != null)
        {
            if (mMonthCtrl.gameObject != null)
                GameObject.Destroy(mMonthCtrl.gameObject);
        }
        mMonthCtrl = null;

        for (int i = 0; i < mLostDayList.Count; i++)
        {
            if (mLostDayList[i].gameObject != null)
                GameObject.Destroy(mLostDayList[i].gameObject);
        }
        mLostDayList.Clear();
    }

    public void SetData()
    {
        ResetInfos();

        int nowyear = System.DateTime.Now.Year;
        int randomMonth = Random.Range(1, 13);
        mMonthCtrl = mCtrl.CreateMonth(transform, nowyear, randomMonth);
        mMonthCtrl.ChangeMonthBtnActive(false);
        mMonthCtrl.transform.localPosition = vMonth;
        //天数create
        mMonthCtrl.InitMonthData();

        mCtrl.StartCoroutine(IESetLostDay());
    }
    IEnumerator IESetLostDay()
    {
        int ndays = mMonthCtrl.mDayList.Count;
        for (int i = 0; i < ndays; i++)
        {
            mMonthCtrl.mDayList[i].ButtonActive(false);
            mMonthCtrl.mDayList[i].BoxColliderActive(true);
        }

        yield return new WaitForSeconds(1.5f);

        //lost days
        List<int> mlostList = Common.GetIDList(0, ndays - 1, nToCount, -1);
        vPosList = Common.BreakRank(vPosList);
        for (int i = 0; i < mlostList.Count; i++)
        {
            int nindex = mlostList[i];
            KnowCalendarDay mday = mMonthCtrl.mDayList[nindex];
            mday.SetLostDay();
            mday.ButtonActive(true);

            KnowCalendarLostDay lostday = CreateLostDay(mday.nDay);
            lostday.transform.position = mday.transform.position;
            lostday.SetRemenberPos(vPosList[i]);
            mLostDayList.Add(lostday);
            lostday.transform.DOLocalMove(vPosList[i], 0.5f);
            mCtrl.mSoundCtrl.PlaySortSound("knowcalendar_sound", "numbertoright");
            yield return new WaitForSeconds(0.5f);
        }
    }



    void Update()
    {
        MUpdate();
    }

    Vector3 temp_select_offset = Vector3.zero;
    KnowCalendarLostDay mSelect = null;
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
                    KnowCalendarLostDay com = hits[i].collider.gameObject.GetComponent<KnowCalendarLostDay>();
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
                    mSelect.DoScale(1.3f,0.3f);
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
                            if (mSelect.nDay == comDay.nDay)
                            {
                                bMatch = true;
                                CheckIsOK(comDay, mSelect);
                            }
                            else
                            {
                                bHitFaile = true;
                            }
                            break;
                        }
                    }
                }

                if (!bMatch)
                {
                    mSelect.DoScale(1f, 0.2f);
                    if (bHitFaile)
                    {
                        mSelect.MoveToRemenberPos();
                        mCtrl.mSoundCtrl.PlaySortSound("knowcalendar_sound", "numberblackto");
                    }
                    else
                    {
                        float fNowX = mSelect.transform.localPosition.x;
                        if (fNowX < 100f)
                        {
                            fNowX = 100f;
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

    /// <summary>
    /// 匹配成功
    /// </summary>
    /// <param name="_inputnum"></param>
    public void CheckIsOK(KnowCalendarDay _day, KnowCalendarLostDay _lostday)
    {
        PlayTheDaySound(_day);

        mCtrl.mSoundCtrl.PlaySortSound("knowcalendar_sound", "numbersetok");
        nCount++;
        _day.ShowTheDay();
        _lostday.gameObject.SetActive(false);
        mCtrl.PlayDropOKFX(_day.transform.position);
        if (nCount >= nToCount)
        {
            mCtrl.StartCoroutine(IEToNextLevel());
        }
    }
    IEnumerator IEToNextLevel()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("lv1 pass");
        mCtrl.LevelCheckNext();
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


    KnowCalendarLostDay CreateLostDay(int _day)
    {
        GameObject mgo = UguiMaker.newGameObject("lostday", transform);
        KnowCalendarLostDay lostday = mgo.AddComponent<KnowCalendarLostDay>();
        lostday.InitAwake(_day);
        return lostday;
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

    /// <summary>
    /// 玩法提示语音
    /// </summary>
    /// <returns></returns>
    public IEnumerator iePlayTipSoune()
    {
        yield return new WaitForSeconds(0.1f);
        AudioClip cp = ResManager.GetClip("knowcalendar_sound", "game-tips_lv1");
        mCtrl.mSoundCtrl.PlaySound(cp, 1f);
    }
 
    /// <summary>
    /// 播放月/日/星期
    /// </summary>
    public void PlayTheDaySound(KnowCalendarDay _day)
    {
        int nyear = _day.nYear;
        int nyue = _day.nMonth;
        int nday = _day.nDay;
        System.DateTime datetime = new System.DateTime(nyear, nyue, nday);
        int nxingqi = (int)datetime.DayOfWeek;

        mCtrl.StopTipSound();
        mCtrl.SetTipSound(iePlayTheDaySound(nyue, nday, nxingqi));
        mCtrl.StartTipSound();
    }
    public IEnumerator iePlayTheDaySound(int _month,int _day,int _week)
    {
        yield return new WaitForSeconds(0.1f);
        List<AudioClip> cpList = new List<AudioClip>();
        AudioClip cp0 = ResManager.GetClip("number_sound", _month.ToString());
        cpList.Add(cp0);
        AudioClip cp1 = ResManager.GetClip("knowcalendar_sound", "month");
        cpList.Add(cp1);
        AudioClip cp2 = ResManager.GetClip("number_sound", _day.ToString());
        cpList.Add(cp2);
        AudioClip cp3 = ResManager.GetClip("knowcalendar_sound", "day");
        cpList.Add(cp3);
        AudioClip cp4 = ResManager.GetClip("knowcalendar_sound", "xingqi" + _week);
        cpList.Add(cp4);
        for (int i = 0; i < cpList.Count; i++)
        {
            mCtrl.mSoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
    }
}
