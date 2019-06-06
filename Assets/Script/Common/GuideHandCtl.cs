using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// 指引提示
/// </summary>
public class GuideHandCtl : MonoBehaviour
{

    public static GuideHandCtl Create(Transform _parent)
    {
        GameObject obj = UguiMaker.newGameObject("guide_hand", _parent);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        GuideHandCtl result = obj.AddComponent<GuideHandCtl>();
        return result;
    }

    bool mIsPlaying = false;

    //void Awake()
    //{
    //}



    #region 拖拽提醒
    GameObject tip_drag_parent = null;
    public Image tip_drag_sprite = null;
    float tip_drag_frequencyTime = 1f;

    /// <summary>
    /// 拖拽提示
    /// </summary>
    /// <param name="begin_pos">开始坐标(世界坐标)</param>
    /// <param name="end_pos">结束坐标(世界坐标)</param>
    /// <param name="round_number">播放次数, -1:无限循环播放 </param>
    /// <param name="speed_scale">1-正常速度 </param>
    public void GuideTipDrag( Vector3 begin_pos, Vector3 end_pos, int round_number, float speed_scale = 1,string _handName = "hand")
    {
        if (null == tip_drag_parent)
        {
            tip_drag_parent =  UguiMaker.newGameObject("drag_hand", transform);
            tip_drag_parent.layer = 5;
            tip_drag_parent.transform.localPosition = Vector3.zero;
            tip_drag_sprite = UguiMaker.newImage("handImage", tip_drag_parent.transform, "public", _handName, false);
            tip_drag_sprite.color = new Color(1, 1, 1, 0);
        }
        tip_drag_parent.gameObject.SetActive(true);
        mIsPlaying = true;

        StartCoroutine(TDragTip(begin_pos, end_pos, round_number, speed_scale));
        
    }
    IEnumerator TDragTip(Vector3 begin_pos, Vector3 end_pos, int round_number, float speed_scale = 1)
    {
        while (true)
        {
            if (round_number == 0) break;
            round_number--;

            tip_drag_parent.transform.position = begin_pos;

            for (float i = 0; i < 1f; i += 0.03f * speed_scale)
            {
                tip_drag_sprite.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), i);
                yield return new WaitForSeconds(0.01f);
            }
            tip_drag_sprite.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.2f);

            for (float i = 0; i < 1f; i += 0.02f * speed_scale)
            {
                tip_drag_parent.transform.position = Vector3.Lerp(begin_pos, end_pos, i);
                yield return new WaitForSeconds(0.01f);
            }
            tip_drag_parent.transform.position = end_pos;
            yield return new WaitForSeconds(0.2f);

            for (float i = 0; i < 1f; i += 0.03f * speed_scale)
            {
                tip_drag_sprite.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), i);
                yield return new WaitForSeconds(0.01f);
            }
            tip_drag_sprite.color = new Color(1, 1, 1, 0);

            yield return new WaitForSeconds(tip_drag_frequencyTime);
        }
    }

    public void StopDrag()
    {
        if (!mIsPlaying)
            return;
        StopAllCoroutines();
        if (null != tip_drag_parent)
        {
            tip_drag_parent.gameObject.SetActive(false);
        }
        mIsPlaying = false;
    }

    /// <summary>
    /// 频率时间设置
    /// </summary>
    public void SetDragFrequencyTime(float _time)
    {
        tip_drag_frequencyTime = _time;
    }

    /// <summary>
    /// drop信息调整
    /// </summary>
    public void SetDragDate(Vector3 voffset,float fscale)
    {
        tip_drag_sprite.transform.localPosition = voffset;
        tip_drag_sprite.transform.localScale = Vector3.one * fscale;
    }

    #endregion
    
    #region 点击提醒
    GameObject tip_click_parent = null;
    public Image tip_click_sprite = null;
    Tween scaleTween = null;
    ParticleSystem mClickFx;
    /// <summary>
    /// 点击提示
    /// </summary>
    /// <param name="_scale">缩放系数</param>
    /// <param name="_time">频率</param>
    public void GuideTipClick(float _scale, float _time,bool _showFx = false,bool _fxLoop = true, string _imageName = "hand")
    {
        if (null == tip_click_parent)
        {
            tip_click_parent = UguiMaker.newGameObject("click_hand", transform);
            tip_click_parent.layer = 5;
            tip_click_sprite = UguiMaker.newImage("handImage", tip_click_parent.transform, "public", _imageName, false);
            tip_click_sprite.transform.localPosition = Vector3.zero;
            if (_showFx)
            {
                if (mClickFx == null)
                {
                    GameObject mLoad = Resources.Load("prefab/clickFX/clickFX") as GameObject;
                    GameObject mgo = GameObject.Instantiate(mLoad);
                    mClickFx = mgo.GetComponent<ParticleSystem>();
                }              
            }
        }
        tip_click_parent.gameObject.SetActive(true);
        if (_showFx)
        {
            mClickFx.loop = _fxLoop;
            mClickFx.transform.Find("rang1").GetComponent<ParticleSystem>().loop = _fxLoop;
            mClickFx.transform.SetParent(tip_click_parent.transform);
            mClickFx.transform.localScale = Vector3.one;
            mClickFx.transform.localPosition = Vector3.zero;
            mClickFx.Play();
        }
        scaleTween = tip_click_parent.transform.DOScale(Vector3.one * _scale, _time).SetLoops(-1, LoopType.Yoyo);
    }
    private void StopScale()
    {
        if (scaleTween != null)
        {
            scaleTween.Pause();
            tip_click_parent.transform.localScale = Vector3.one;
            tip_click_parent.gameObject.SetActive(false);
        }
    }
    public void StopClick()
    {
        StopScale();
    }
    /// <summary>
    /// 点击提示位置设置
    /// </summary>
    /// <param name="_pos">world pos</param>
    public void SetClickTipPos(Vector3 _pos)
    {
        tip_click_parent.transform.position = _pos;
    }

    /// <summary>
    /// 偏移调整
    /// </summary>
    /// <param name="_pos">local pos</param>
    public void SetClickTipOffsetPos(Vector3 _pos,float _scale = 1f)
    {
        tip_click_sprite.transform.localPosition = _pos;
        tip_click_sprite.transform.localScale = Vector3.one * _scale;
    }

    #endregion
    
    #region 左右椭圆打圈
    GameObject tip_ellipse_parent = null;
    Image tip_ellipse_sprite = null;

    public void StopEllipse()
    {
        if (!mIsPlaying)
            return;
        StopAllCoroutines();
        if (null != tip_ellipse_parent)
        {
            tip_ellipse_parent.gameObject.SetActive(false);
        }
        mIsPlaying = false;
    }

    public void GuideTipEllipse(Vector3 local_pos, float scale_x, float scale_y)
    {
        if (null == tip_ellipse_parent)
        {
            tip_ellipse_parent = UguiMaker.newGameObject("ellipse_hand", transform);
            tip_ellipse_sprite = UguiMaker.newImage("handImage", tip_drag_parent.transform, "public", "hand", false);
        }
        tip_ellipse_parent.gameObject.SetActive(true);
        mIsPlaying = true;
        StartCoroutine(TEllipseTip(local_pos, scale_x, scale_y));
    }
    IEnumerator TEllipseTip(Vector3 local_pos, float scale_x, float scale_y)
    {
        float param = 0;
        while (true)
        {
            float x = Mathf.Sin(param) * scale_x;
            float y = Mathf.Cos(param) * scale_y;
            tip_ellipse_parent.transform.localPosition = local_pos + new Vector3(x, y, 0);

            param += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    #endregion

    #region 划半圆提醒
    GameObject tip_half_parent = null;
    Image tip_half_sprite = null;
    /// <summary>
    /// 划半圆提醒
    /// </summary>
    /// <param name="begin_pos"></param>
    /// <param name="end_pos"></param>
    /// <param name="clockwise">是否顺时针</param>
    /// <param name="sprite_depth"></param>
    /// <param name="speed_scale"></param>
    public void GuideTipHalf(Vector3 begin_pos, Vector3 end_pos, bool clockwise, float speed_scale = 1, float scale_sprite = 1f)
    {
        if (null == tip_half_parent)
        {
            tip_half_parent = UguiMaker.newGameObject("half_hand", transform);
            tip_half_sprite = UguiMaker.newImage("handImage", tip_drag_parent.transform, "public", "hand", false);
        }
        tip_half_parent.gameObject.SetActive(true);
        mIsPlaying = true;
        StartCoroutine(THalfTip(begin_pos, end_pos, clockwise, speed_scale));
    }
    IEnumerator THalfTip(Vector3 begin_pos, Vector3 end_pos, bool clockwise, float speed_scale)
    {
        Vector3 center_pos = (begin_pos + end_pos) * 0.5f;
        Vector3 dir_beg = begin_pos - center_pos;
        float radius = Vector3.Distance(begin_pos, end_pos) * 0.5f;

        float angle_beg = Vector3.Angle(dir_beg, Vector3.up) * (Vector3.Cross(Vector3.up, dir_beg).z > 0 ? -1 : 1);
        float angle_end = clockwise ? angle_beg - 180 : angle_beg + 180;
        Vector3 angle_beg_vec = new Vector3(0, 0, angle_beg);
        Vector3 angle_end_vec = new Vector3(0, 0, angle_end);

        while (true)
        {
            tip_half_parent.transform.position = begin_pos;

            for (float i = 0; i < 1f; i += 0.03f * speed_scale)
            {
                tip_half_sprite.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), i);
                yield return new WaitForSeconds(0.01f);
            }
            tip_half_sprite.color = new Color(1, 1, 1, 1);

            yield return new WaitForSeconds(0.2f);
            
            for(float i = 0; i < 1f; i += 0.03f * speed_scale)
            {
                Vector3 angle = Vector3.Lerp(angle_beg_vec, angle_end_vec, i);
                float pi = angle.z / 180f * Mathf.PI;
                tip_half_parent.transform.position = center_pos + new Vector3(Mathf.Sin(pi) * radius, Mathf.Cos(pi) * radius, 0);
                yield return new WaitForSeconds(0.01f);

            }

            yield return new WaitForSeconds(0.2f);

            for (float i = 0; i < 1f; i += 0.03f * speed_scale)
            {
                tip_half_sprite.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), i);
                yield return new WaitForSeconds(0.01f);
            }
            tip_half_sprite.color = new Color(1, 1, 1, 0);
        }
    }

    public void StopHalf()
    {
        if (!mIsPlaying)
            return;
        StopAllCoroutines();
        if (null != tip_half_parent)
        {
            tip_half_parent.gameObject.SetActive(false);
        }
        mIsPlaying = false;
    }
    #endregion


}
