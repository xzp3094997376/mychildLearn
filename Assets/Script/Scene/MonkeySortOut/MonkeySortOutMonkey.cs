using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Spine.Unity;

public class MonkeySortOutMonkey : MonoBehaviour {
    public static MonkeySortOutMonkey gSelect { get; set; }

    public int m_info_size = 0;//小，大
    public int m_info_color = 0;//桔色，棕色
    public int m_info_hat = 0;//不戴帽子，带帽子
    public int m_info_face_side = 0;//正面,侧面
    public int m_info_tail = 0;//尾巴长,短
    public int m_info_clothes = 0;//不穿马甲，穿马甲
    public int m_info_stand = 0;//坐着，站着
    public int m_info_food = 0;//没有桃子，有桃子
    public int m_info_mouth = 0;//闭上嘴巴，张开嘴巴

    public RectTransform mrtran { get; set; }
    public SkeletonGraphic mSpine { get; set; }
    public Image mShadow { get; set; }
    public BoxCollider mBox { get; set; }

    Vector3 m_scale_default = new Vector3(0.8f, 0.8f, 1);

    void Start ()
    {
        mrtran = GetComponent<RectTransform>();
        mSpine = transform.Find("spine").GetComponent<SkeletonGraphic>();
        mShadow = UguiMaker.newImage("shaodw", transform, "monkeysortout_sprite", "shadow");
        mShadow.transform.SetAsFirstSibling();
        if (0 == m_info_size)
        {
            mShadow.rectTransform.anchoredPosition = new Vector2(0, 26 * 0.7f);
            mShadow.transform.localScale = new Vector3(0.7f, 0.7f, 1);
        }
        else
        {
            mShadow.rectTransform.anchoredPosition = new Vector2(0, 18);
        }

        MonkeySortOutCtl.instance.CallbackStart();
        //ScreenDebug.Log("finish=" + gameObject.name);

    }
	

    public void PlaySpine(string name, bool isloop)
    {
        if (isloop)
        {
            mSpine.AnimationState.SetAnimation(1, name, isloop);
        }
        else
        {
            mSpine.AnimationState.ClearTracks();
            mSpine.AnimationState.AddAnimation(1, name, false, 0);
            mSpine.AnimationState.AddAnimation(1, "Idle", true, 0);

        }

    }
    public void SetRTran(Transform _parent, int sibling_index)
    {
        transform.SetParent(_parent);
        transform.SetSiblingIndex(sibling_index);
    }
    public void Select()
    {
        transform.SetParent(MonkeySortOutCtl.instance.mfont);
        transform.SetAsLastSibling();
        transform.localScale = m_scale_default;
        PlaySpine("Click", true);

        if(!temp_playing_selecting)
        {
            StartCoroutine("TSelecting");
        }

        mShadow.enabled = false;
    }
    public void SetBoxEnable(bool _enable)
    {
        if (null == mBox)
            mBox = GetComponent<BoxCollider>();

        mBox.enabled = _enable;
    }
    

    public void ThrowOutCar(Vector2 end_pos)
    {
        MonkeySortOutCtl.instance.mSoundMgr.PlayShort("monkeysortout_sound", "monkey_out_car", 0.4f);
        StartCoroutine(TThrowOutCar(end_pos));
    }
    IEnumerator TThrowOutCar(Vector2 end_pos)
    {
        mShadow.enabled = false;
        SetRTran(MonkeySortOutCtl.instance.mfont, MonkeySortOutCtl.instance.mfont.childCount);
        Vector2 pos = mrtran.anchoredPosition;
        Vector3 scale = transform.localScale;

        for (float i = 0; i < 1f; i += 0.03f)
        {
            mrtran.anchoredPosition = Vector2.Lerp(pos, end_pos, Mathf.Sin(Mathf.PI * 0.5f * i)) + new Vector2(0, Mathf.Sin(Mathf.PI * i) * 80);
            transform.localScale = Vector3.Lerp(scale, m_scale_default, i);

            yield return new WaitForSeconds(0.01f);
        }
        mrtran.anchoredPosition = end_pos;
        transform.localScale = m_scale_default;
        mShadow.enabled = true;
        SetBoxEnable(true);
    }
    public Vector3 mResetPos = Vector3.zero;
    public void ResetPos()
    {
        StopAllCoroutines();
        StartCoroutine("TResetPos");
    }
    IEnumerator TResetPos()
    {

        //yield return new WaitForSeconds(GlobalParam.drag_delay_time);
        Vector3 beg_scale = transform.localScale;
        Vector3 beg_pos = mrtran.anchoredPosition3D;
        Vector3 forward = (mResetPos - beg_pos).normalized;
        forward.z = 0;
        float distance = Vector3.Distance(mResetPos, transform.localPosition);
        float temp_dis = 0;
        float temp = 1;
        int count = 0;
        while (temp_dis < distance)
        {
            count++;
            temp_dis += temp;
            temp += GlobalParam.drag_reset_aspeed;
        }
        count -= 4;

        for (int i = 0; i < count; i++)
        {
            mrtran.anchoredPosition3D += forward * GlobalParam.drag_reset_aspeed * (i + 1);
            //transform.localScale = Vector3.Lerp(beg_scale, mResetScale, 1f * i / count);
            yield return new WaitForSeconds(0.01f);
        }
        mrtran.anchoredPosition3D = mResetPos;
        //transform.localScale = mResetScale;

        PlaySpine("Idle", true);
        mShadow.enabled = true;
        MonkeySortOutCtl.instance.SortFont();
    }

    bool temp_playing_selecting = false;
    public void StopTSelecting()
    {
        temp_playing_selecting = false;
        StopCoroutine("TSelecting");
        transform.localScale = m_scale_default;
    }
    IEnumerator TSelecting()
    {
        temp_playing_selecting = true;
        Vector3 scale_select = new Vector3(1, 1, 1);
        //int count = 2;
        while (true)
        {
            for(float i = 0; i < 1f; i += 0.1f)
            {
                float p = Mathf.Sin(Mathf.PI * i) * 0.1f;
                transform.localScale = scale_select + new Vector3(p, p, 0);
                yield return new WaitForSeconds(0.01f);
            }
            //count--;
        }
        //transform.localScale = m_scale_default;
    }


}
