using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FindSameThingStation : MonoBehaviour {
    public static FindSameThingStation gSelect;

    public int mdata_type;
    public int mdata_id;

    public Vector3 mResetPos = Vector3.zero;
    public RectTransform mRtran;
    Image mThing, mAnchor, mLock;//, mMask, mColor;
    BoxCollider mBox { get; set; }

    public void Init(int type, int id)
    {
        //Debug.Log("type=" + type + "id=" + id);
        mdata_type = type;
        mdata_id = id;

        string sprite_name = type + "-" + id;
        if(null == mRtran)
        {
            mRtran = gameObject.GetComponent<RectTransform>();
            
            mThing = UguiMaker.newImage("mThing", transform, "findsamething_sprite", sprite_name, false);
            mAnchor = UguiMaker.newImage("mAnchor", transform, "findsamething_sprite", "anchor", false);
            mLock = UguiMaker.newImage("mLock", transform, "findsamething_sprite", "anchor_lock", false);

            mBox = gameObject.AddComponent<BoxCollider>();
            mBox.size = new Vector3(120, 120, 1);
            mAnchor.color = new Color(1, 1, 1, 0);

        }

        
        mThing.sprite = ResManager.GetSprite("findsamething_sprite", sprite_name);
        mThing.SetNativeSize();

        ShowLock(false);
        SetBoxEnable(true);

    }
    public void SetResetPos(Vector3 reset_pos)
    {
        //Debug.Log("SetResetPos = " + reset_pos);
        mResetPos = reset_pos;
    }
    public void SetBoxEnable(bool _enable)
    {
        mBox.enabled = _enable;
    }
    public void ShowLock(bool show)
    {
        mLock.enabled = show;
        if(show)
        {
            //Debug.Log(show);
            StopAllCoroutines();
            mThing.rectTransform.anchoredPosition3D = Vector3.zero;
            StartCoroutine("TLock");
            //FindSameThingCtl.instance.mSound.PlayShort("findsamething_sound", FindSameThingCtl.instance.mdata_dic_sound[mdata_type * 10 + mdata_id]);
            //FindSameThingCtl.instance.mSound.PlayOnly("findsamething_sound", FindSameThingCtl.instance.mdata_dic_sound[mdata_type * 10 + mdata_id]);

        }
        else
        {
            StopCoroutine("TLock");
        }
    }

    public void Show()
    {
        StartCoroutine("TShow");
    }
    IEnumerator TShow()
    {
        FindSameThingCtl.instance.mSound.PlayShort("素材出现通用音效");
        mRtran.anchoredPosition3D = mResetPos;
        mThing.rectTransform.localEulerAngles = Vector3.zero;
        for(float i = 0; i < 1f; i += 0.05f)
        {
            float p = Mathf.Sin(Mathf.PI * i) * 0.7f;
            mRtran.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i) + new Vector3(p, p, 0);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.localScale = Vector3.one;
        Fly();
    }

    public void Fly()
    {
        StopAllCoroutines();
        StartCoroutine("TFly");
    }
    IEnumerator TFly()
    {
        float p = 0;
        while(true)
        {
            //mThing.rectTransform.localEulerAngles += new Vector3(0, 0, -5);
            mRtran.anchoredPosition3D = new Vector3(Mathf.Cos(p) * 3, Mathf.Sin(p) * 3, 0) + mResetPos;
            yield return new WaitForSeconds(0.01f);
            p += 0.3f;
        }

    }

    IEnumerator TLock()
    {
        while(true)
        {
            mLock.rectTransform.localEulerAngles += new Vector3(0, 0, -10);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
