using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 存放已经标记的头像
/// </summary>
public class KnowCalendarDayHeadStation : MonoBehaviour
{

    public int nDay = 1;
    public int nMonth = 1;

    public List<KnowCalendarDayHead> mHeadList = new List<KnowCalendarDayHead>();




    public void AddDayHead(KnowCalendarDayHead _head)
    {
        _head.transform.SetParent(transform);
        _head.transform.localPosition = Vector3.zero;
        _head.transform.localScale = Vector3.one * 0.5f;
        mHeadList.Add(_head);
    }

    private void SetHeadPos()
    {
        
    }

}
