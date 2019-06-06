using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka10_Station : MonoBehaviour
{
    public RectTransform mRtran { get; set; }
    public Image mFrame, mBg;
    public Image[] mLun;
    public Image[] mStar;
    public Image[] mWindow;
    public Image[] mYan;
    public BoxCollider mBox;
    public ParticleSystem mEffect { get; set; }

    Vector3 mScale = new Vector3(0.8f, 0.8f, 1);
    public bool mdata_is_over = false;

    public int mdata_bg { get; set; }
    public int mdata_lun { get; set; }
    public int mdata_star { get; set; }
    public int mdata_window { get; set; }
    public int mdata_yan { get; set; }

    public int temp_bg { get; set; }
    public int temp_lun { get; set; }
    public int temp_star { get; set; }
    public int temp_window { get; set; }
    public int temp_yan { get; set; }


    public void Init(int bg, int lun, int star, int window, int yan)
    {
        mRtran = gameObject.GetComponent<RectTransform>();
        mRtran.localScale = mScale;

        mdata_bg = bg;
        mdata_lun = lun;
        mdata_star = star;
        mdata_window = window;
        mdata_yan = yan;

        if (null == mBg)
        {

            mLun = new Image[4];
            mStar = new Image[3];
            mWindow = new Image[2];
            mYan = new Image[3];

            mLun[0] = UguiMaker.newImage("mLun[0]", transform, "shapelogic_sprite", "guanka10_station1");
            mLun[1] = UguiMaker.newImage("mLun[1]", transform, "shapelogic_sprite", "guanka10_station1");
            mLun[2] = UguiMaker.newImage("mLun[2]", transform, "shapelogic_sprite", "guanka10_station1");
            mLun[3] = UguiMaker.newImage("mLun[3]", transform, "shapelogic_sprite", "guanka10_station1");
            mBg = UguiMaker.newImage("mBg", transform, "shapelogic_sprite", "guanka10_station0");
            mYan[2] = UguiMaker.newImage("mYan[0]", mBg.transform, "shapelogic_sprite", "guanka10_station4");
            mYan[1] = UguiMaker.newImage("mYan[1]", mBg.transform, "shapelogic_sprite", "guanka10_station4");
            mYan[0] = UguiMaker.newImage("mYan[2]", mBg.transform, "shapelogic_sprite", "guanka10_station4");
           
            mStar[0] = UguiMaker.newImage("mStar[0]", mBg.transform, "shapelogic_sprite", "guanka10_station5");
            mStar[1] = UguiMaker.newImage("mStar[1]", mBg.transform, "shapelogic_sprite", "guanka10_station5");
            mStar[2] = UguiMaker.newImage("mStar[2]", mBg.transform, "shapelogic_sprite", "guanka10_station5");
            mWindow[0] = UguiMaker.newImage("mWindow[0]", mBg.transform, "shapelogic_sprite", "guanka10_station2");
            mWindow[1] = UguiMaker.newImage("mWindow[1]", mBg.transform, "shapelogic_sprite", "guanka10_station2");

            mWindow[0].rectTransform.anchoredPosition = new Vector2(28, 36);
            mWindow[1].rectTransform.anchoredPosition = new Vector2(83, 36);

        }

    }
    public void SetBoxEnable(bool _enable)
    {
        if (null == mBox)
        {
            mBox = gameObject.AddComponent<BoxCollider>();
            mBox.size = new Vector3(184, 276.3f, 1);
            mBox.center = new Vector3(0, 19, 0);
        }
        mBox.enabled = _enable;

    }
    public bool GetCorrect()
    {
        if (temp_bg != mdata_bg)
            return false;
        if (temp_lun != mdata_lun)
            return false;
        if (temp_star != mdata_star)
            return false;
        if (temp_window != mdata_window)
            return false;
        if (temp_yan != mdata_yan)
            return false;

        return true;
    }


    public void ReflushUI_Temp()
    {
        //车身
        mBg.enabled = temp_bg == 1;

        //窗户
        mWindow[0].enabled = temp_window == 2;
        mWindow[1].enabled = temp_window == 2;

        //轮子
        for (int i = 0; i < 4; i++)
        {
            if (temp_lun > i)
                mLun[i].enabled = true;
            else
                mLun[i].enabled = false;
        }
        Vector2 offset_lun = new Vector2(15, 0);
        switch (temp_lun)
        {
            case 2:
                mLun[0].rectTransform.anchoredPosition = new Vector2(70, -111) + offset_lun;
                mLun[1].rectTransform.anchoredPosition = new Vector2(-70, -111) + offset_lun;
                break;
            case 3:
                mLun[0].rectTransform.anchoredPosition = new Vector2(0, -111) + offset_lun;
                mLun[1].rectTransform.anchoredPosition = new Vector2(-70, -111) + offset_lun;
                mLun[2].rectTransform.anchoredPosition = new Vector2(70, -111) + offset_lun;
                break;
            case 4:
                mLun[0].rectTransform.anchoredPosition = new Vector2(-25, -111) + offset_lun;
                mLun[1].rectTransform.anchoredPosition = new Vector2(-75, -111) + offset_lun;
                mLun[2].rectTransform.anchoredPosition = new Vector2(25, -111) + offset_lun;
                mLun[3].rectTransform.anchoredPosition = new Vector2(75, -111) + offset_lun;
                break;
        }

        //星星
        for (int i = 0; i < 3; i++)
        {
            if (temp_star > i)
                mStar[i].enabled = true;
            else
                mStar[i].enabled = false;
        }
        switch (temp_star)
        {
            case 1:
                mStar[0].rectTransform.anchoredPosition = new Vector2(56, -44.2f);
                break;
            case 2:
                mStar[0].rectTransform.anchoredPosition = new Vector2(34.6f, -44.2f);
                mStar[1].rectTransform.anchoredPosition = new Vector2(77.6f, -44.2f);
                break;
            case 3:
                mStar[0].rectTransform.anchoredPosition = new Vector2(26.4f, -36.3f);
                mStar[1].rectTransform.anchoredPosition = new Vector2(86.4f, -36.3f);
                mStar[2].rectTransform.anchoredPosition = new Vector2(55.6f, -59f);
                break;
        }

        //烟囱
        for (int i = 0; i < 3; i++)
        {
            if (temp_yan > i)
                mYan[i].enabled = true;
            else
                mYan[i].enabled = false;
        }
        switch (temp_yan)
        {
            case 1:
                mYan[0].rectTransform.anchoredPosition = new Vector2(-68.7f, 18.4f);
                break;
            case 2:
                mYan[0].rectTransform.anchoredPosition = new Vector2(-68.7f, 18.4f);
                mYan[1].rectTransform.anchoredPosition = new Vector2(-68.7f, 18.4f + 35f);
                break;
            case 3:
                mYan[0].rectTransform.anchoredPosition = new Vector2(-68.7f, 18.4f);
                mYan[1].rectTransform.anchoredPosition = new Vector2(-68.7f, 18.4f + 35f);
                mYan[2].rectTransform.anchoredPosition = new Vector2(-68.7f, 18.4f + 70f);
                break;
        }

    }
    public void ReflushUI()
    {
        //车身
        mBg.gameObject.SetActive(mdata_bg == 1);

        //窗户
        mWindow[0].enabled = mdata_window == 2;
        mWindow[1].enabled = mdata_window == 2;

        //轮子
        for (int i = 0; i < 4; i++)
        {
            if (mdata_lun > i)
                mLun[i].enabled = true;
            else
                mLun[i].enabled = false;
        }
        Vector2 offset_lun = new Vector2(15, 0);
        switch (mdata_lun)
        {
            case 2:
                mLun[0].rectTransform.anchoredPosition = new Vector2(70, -111) + offset_lun;
                mLun[1].rectTransform.anchoredPosition = new Vector2(-70, -111) + offset_lun;
                break;
            case 3:
                mLun[0].rectTransform.anchoredPosition = new Vector2(0, -111) + offset_lun;
                mLun[1].rectTransform.anchoredPosition = new Vector2(-70, -111) + offset_lun;
                mLun[2].rectTransform.anchoredPosition = new Vector2(70, -111) + offset_lun;
                break;
            case 4:
                mLun[0].rectTransform.anchoredPosition = new Vector2(-25, -111) + offset_lun;
                mLun[1].rectTransform.anchoredPosition = new Vector2(-75, -111) + offset_lun;
                mLun[2].rectTransform.anchoredPosition = new Vector2(25, -111) + offset_lun;
                mLun[3].rectTransform.anchoredPosition = new Vector2(75, -111) + offset_lun;
                break;
        }

        //星星
        for (int i = 0; i < 3; i++)
        {
            if (mdata_star > i)
                mStar[i].enabled = true;
            else
                mStar[i].enabled = false;
        }
        switch (mdata_star)
        {
            case 1:
                mStar[0].rectTransform.anchoredPosition = new Vector2(56, -44.2f);
                break;
            case 2:
                mStar[0].rectTransform.anchoredPosition = new Vector2(34.6f, -44.2f);
                mStar[1].rectTransform.anchoredPosition = new Vector2(77.6f, -44.2f);
                break;
            case 3:
                mStar[0].rectTransform.anchoredPosition = new Vector2(26.4f, -36.3f);
                mStar[1].rectTransform.anchoredPosition = new Vector2(86.4f, -36.3f);
                mStar[2].rectTransform.anchoredPosition = new Vector2(55.6f, -59f);
                break;
        }

        //烟囱
        for (int i = 0; i < 3; i++)
        {
            if (mdata_yan > i)
                mYan[i].enabled = true;
            else
                mYan[i].enabled = false;
        }
        switch (mdata_yan)
        {
            case 1:
                mYan[0].rectTransform.anchoredPosition = new Vector2(-68.7f, 18.4f);
                break;
            case 2:
                mYan[0].rectTransform.anchoredPosition = new Vector2(-68.7f, 18.4f);
                mYan[1].rectTransform.anchoredPosition = new Vector2(-68.7f, 18.4f + 35f);
                break;
            case 3:
                mYan[0].rectTransform.anchoredPosition = new Vector2(-68.7f, 18.4f);
                mYan[1].rectTransform.anchoredPosition = new Vector2(-68.7f, 18.4f + 35f);
                mYan[2].rectTransform.anchoredPosition = new Vector2(-68.7f, 18.4f + 70f);
                break;
        }


    }

    public void BeginIn()
    {
        StartCoroutine("TBeginIn");
    }
    IEnumerator TBeginIn()
    {
        Play();
        Vector3 pos0 = mRtran.anchoredPosition3D;
        Vector3 pos1 = pos0 + new Vector3(800, 0);
        //Vector3 temp_pos = pos0;

        //if(null == mEffect)
        //{
        //    mEffect = ResManager.GetPrefab("effect_puke", "effect_puke0").GetComponent<ParticleSystem>();
        //    UguiMaker.InitGameObj(mEffect.gameObject, transform, "effect_puke", new Vector3(150, -50, 0), Vector3.one);
        //}
        //mEffect.Play();

        for (float i = 0; i < 1; i += 0.01f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos1, pos0, Mathf.Sin(Mathf.PI * 0.5f * i));
            //float angle = Vector3.Angle(Vector3.left, mRtran.anchoredPosition3D - temp_pos);
            //mRtran.localEulerAngles = new Vector3(0, 0, -angle);
            //temp_pos = mRtran.anchoredPosition3D;
            yield return new WaitForSeconds(0.01f);
        }
        //mRtran.localEulerAngles = Vector3.zero;
        mRtran.anchoredPosition3D = pos0;
    }
    
    public void Play()
    {
        StartCoroutine("TPlay");
    }
    IEnumerator TPlay()
    {
        float p = 0;
        Vector3 angle = Vector3.zero;
        while (true)
        {
            mBg.rectTransform.anchoredPosition = new Vector2(0, Mathf.Sin(p) * 3 - 5);
            mBg.rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Cos(p) * 1);
            yield return new WaitForSeconds(0.01f);
            p += 0.2f;

        }
    }


    public void InitAnswer()
    {
        StartCoroutine("TInitAnswer");
    }
    IEnumerator TInitAnswer()
    {
        mBg.enabled = false;
        mLun[0].enabled = false;
        mLun[1].enabled = false;
        mLun[2].enabled = false;
        mLun[3].enabled = false;

        mStar[0].enabled = false;
        mStar[1].enabled = false;
        mStar[2].enabled = false;

        mWindow[0].enabled = false;
        mWindow[1].enabled = false;

        mYan[0].enabled = false;
        mYan[1].enabled = false;
        mYan[2].enabled = false;


        yield return new WaitForSeconds(2.5f);
        mFrame = UguiMaker.newImage("mFrame", transform, "shapelogic_sprite", "guanka10_station3");
        mFrame.type = Image.Type.Sliced;
        mFrame.transform.SetAsFirstSibling();
        mFrame.rectTransform.anchoredPosition = new Vector2(0, -16.4f);

        for (float i = 0; i < 1f; i += 0.08f)
        {
            mFrame.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(0, 0), new Vector2(260, 260), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mFrame.rectTransform.sizeDelta = new Vector2(260, 260);

    }

    
    public void PlayError()
    {
        StartCoroutine("TPlayError");
    }
    IEnumerator TPlayError()
    {

        ShapeLogicCtl.instance.mSound.PlayShort("错误");
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * i) * 5);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.localEulerAngles = Vector3.zero;


    }


    public void PlayShoot()
    {
        mdata_is_over = true;
        transform.SetAsLastSibling();
        StartCoroutine("TPlayShoot");
    }
    IEnumerator TPlayShoot()
    {
        AudioSource sound = gameObject.AddComponent<AudioSource>();
        sound.clip = ResManager.GetClip("shapelogic_sound", "car_run");
        sound.loop = true;
        sound.Play();
        Vector3 pos0 = mRtran.anchoredPosition3D;
        Vector3 pos1 = pos0 - new Vector3(1423, 0, 0);
        for (float i = 0; i < 1f; i += 0.01f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i - Mathf.PI * 0.5f) + 1);
            yield return new WaitForSeconds(0.01f);
            if (mRtran.anchoredPosition3D.x < -831)
                break;
        }
        sound.Stop();

    }


}
