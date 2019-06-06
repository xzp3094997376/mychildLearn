using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 节日头像
/// </summary>
public class KnowCalendarDayHead : MonoBehaviour
{
    public int nId = 0;
    public int nMonth = 0;
    public int nDay = 0;

    /// <summary>
    /// 是否完成标记
    /// </summary>
    public bool bIsOK = false;

    private Image imgHead;
    private BoxCollider2D mbox2D;

    private Vector3 vRemenber;

    public void InitAwake(int _month,int _day, int _festivalID)
    {
        nId = _festivalID;
        nMonth = _month;
        nDay = _day;

        imgHead = UguiMaker.newImage("imghead", transform, "knowcalendar_sprite", "kc_obj_" + nId, false);

        mbox2D = gameObject.AddComponent<BoxCollider2D>();
        mbox2D.size = imgHead.rectTransform.sizeDelta;
    }

    public void SetRemenberPos(Vector3 _vpos)
    {
        vRemenber = _vpos;
    }

    public Vector2 GetSize()
    {
        return mbox2D.size;
    }

    public void MoveToRemenberPos()
    {
        transform.DOLocalMove(vRemenber, 0.3f);
    }

    public void DoScale(float _scale)
    {
        transform.DOScale(Vector3.one * _scale, 0.23f);
    }

    public void BoxColliderActive(bool _active)
    {
        mbox2D.enabled = _active;
    }
}
