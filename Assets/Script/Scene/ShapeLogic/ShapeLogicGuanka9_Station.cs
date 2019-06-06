using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka9_Station : MonoBehaviour {
    public RectTransform mRtran { get; set; }
    Image mBg, mHead, mBody;
    Image mTail, mTailType;
    Image[] mTailPlay;
    BoxCollider mBox;

    Vector3 mScale = new Vector3(0.8f, 0.8f, 1);
    public bool mdata_is_over = false;
    
    public int mdata_head { get; set; }
    public int mdata_body { get; set; }
    public int mdata_tail { get; set; }
    public int mdata_tail_type { get; set; }
    
    public int temp_head { get; set; }
    public int temp_body { get; set; }
    public int temp_tail { get; set; }
    public int temp_tail_type { get; set; }


    public void Init( int head, int body, int tail, int tail_type)
    {
        mRtran = gameObject.GetComponent<RectTransform>();
        mRtran.localScale = mScale;

        mdata_head = head;
        mdata_body = body;
        mdata_tail = tail;
        mdata_tail_type = tail_type;

        if (null == mHead)
        {
            mHead = UguiMaker.newImage("mHead", transform, "shapelogic_sprite", "guanka9_head" + mdata_head);
            mBody = UguiMaker.newImage("mBody", transform, "shapelogic_sprite", "guanka9_body" + mdata_body);
            mTail = UguiMaker.newImage("mTail", transform, "shapelogic_sprite", "guanka9_tail_f" + tail);
            mTailType = UguiMaker.newImage("mTailType", mTail.transform, "shapelogic_sprite", "guanka9_tail" + mdata_tail_type);
            mTailPlay = new Image[3];
            mTailPlay[0] = UguiMaker.newImage("mTailPlay", transform, "public", "white");
            mTailPlay[1] = UguiMaker.newImage("mTailPlay", transform, "public", "white");
            mTailPlay[2] = UguiMaker.newImage("mTailPlay", transform, "public", "white");

            
            mHead.rectTransform.pivot = new Vector2(0.5f, 1);
            mHead.rectTransform.anchoredPosition = new Vector2(0, 150);

            for(int i = 0; i < 3; i++)
            {
                mTailPlay[i].color = new Color32(0, 0, 0, 255);
                mTailPlay[i].rectTransform.pivot = new Vector2(0.5f, 1);
                mTailPlay[i].rectTransform.sizeDelta = new Vector2(4, 20);
                if (i == 0)
                {
                    mTailPlay[i].rectTransform.SetParent(transform);
                    mTailPlay[i].rectTransform.anchoredPosition = new Vector2(0, -69.3f);
                }
                else
                {
                    mTailPlay[i].rectTransform.SetParent(mTailPlay[i - 1].rectTransform);
                    mTailPlay[i].rectTransform.anchoredPosition = new Vector2(0, -8);
                }

            }

        }

    }
    public void InitAnswer()
    {
        StartCoroutine("TInitAnswer");
    }
    public void SetHead(int type)
    {
        mHead.gameObject.SetActive(true);
        temp_head = type;
        ReflushUI_Temp();

    }
    public void SetBody(int type)
    {
        mBody.gameObject.SetActive(true);
        temp_body = type;
        ReflushUI_Temp();
        //mBody.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka9_body" + type);

    }
    public void SetTail(int type)
    {
        mTail.enabled = true;
        temp_tail = type;
        ReflushUI_Temp();

    }
    public void SetTailType(int type)
    {
        mTailType.enabled = true;
        temp_tail_type = type;
        ReflushUI_Temp();
    }
    public void SetBoxEnable(bool _enable)
    {
        if(null == mBox)
        {
            mBox = gameObject.AddComponent<BoxCollider>();
            mBox.size = new Vector3(184, 276.3f, 1);
            mBox.center = new Vector3(0, 19, 0);
        }
        mBox.enabled = _enable;

    }
    public bool GetCorrect()
    {
        if (temp_head != mdata_head)
            return false;
        if (temp_body != mdata_body)
            return false;
        if (temp_tail != mdata_tail)
            return false;
        if (temp_tail_type != mdata_tail_type)
            return false;
        return true;
    }


    public void ReflushUI_Temp()
    {
        mBody.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka9_body" + temp_body);
        mHead.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka9_head" + temp_head);
        
        mTail.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka9_tail_f" + temp_tail);
        switch (temp_tail)
        {
            case 0:
                mTail.rectTransform.anchoredPosition = new Vector2(-35.22f, -80.94f);
                mTailType.rectTransform.anchoredPosition = new Vector2(-43, -11.04f);
                break;
            case 1:
                mTail.rectTransform.anchoredPosition = new Vector2(35.22f, -80.94f);
                mTailType.rectTransform.anchoredPosition = new Vector2(43, -11.04f);
                break;
            case 2:
                mTail.rectTransform.anchoredPosition = new Vector2(0, -80.94f);
                mTailType.rectTransform.anchoredPosition = new Vector2(1.6f, -11.04f);
                break;
        }

        mTailType.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka9_tail" + temp_tail_type);

      

    }
    public void ReflushUI()
    {
        mBody.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka9_body" + mdata_body);
        mHead.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka9_head" + mdata_head);
        mTail.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka9_tail_f" + mdata_tail);
        switch (mdata_tail)
        {
            case 0:
                mTail.rectTransform.anchoredPosition = new Vector2(-35.22f, -80.94f);
                mTailType.rectTransform.anchoredPosition = new Vector2(-43, -11.04f);
                break;
            case 1:
                mTail.rectTransform.anchoredPosition = new Vector2(35.22f, -80.94f);
                mTailType.rectTransform.anchoredPosition = new Vector2(43, -11.04f);
                break;
            case 2:
                mTail.rectTransform.anchoredPosition = new Vector2(0, -80.94f);
                mTailType.rectTransform.anchoredPosition = new Vector2(1.6f, -11.04f);
                break;
        }

        mTailType.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka9_tail" + mdata_tail_type);
   

    }
    public void ReflushUI_DefaultTail()
    {
        if (null != mTailPlay)
        {
            mTail.gameObject.SetActive(true);
            mTailPlay[0].gameObject.SetActive(false);
        }
        mTailType.rectTransform.SetParent(mTail.rectTransform);
        //mTailType.rectTransform.anchoredPosition = new Vector2(-30, -11.04f);
        //mTailType.transform.localScale = Vector3.one;
        ReflushUI();
        StopCoroutine("TPlayTail");
    }
    public void ReflushUI_PlayTail(bool paly)
    {
        if (null != mTail)
        {
            mTail.gameObject.SetActive(false);
            mTailPlay[0].gameObject.SetActive(true);
        }
        ReflushUI();
        mTailType.rectTransform.SetParent(mTailPlay[2].rectTransform);
        mTailType.rectTransform.anchoredPosition = new Vector2(0, -8);
        if (paly)
            StartCoroutine("TPlayTail");
        else
        {
            mTailPlay[0].rectTransform.localEulerAngles = Vector3.zero;
            mTailPlay[1].rectTransform.localEulerAngles = Vector3.zero;
            mTailPlay[2].rectTransform.localEulerAngles = Vector3.zero;
            mTailType.rectTransform.localEulerAngles = Vector3.zero;
            StopCoroutine("TPlayTail");
        }

    }
    public void Jump(bool from_right)
    {
        StartCoroutine(TJump(from_right));
    }

    IEnumerator TPlayTail()
    {
        float p = 0;
        Vector3 angle = Vector3.zero;
        while(true)
        {
            angle.z = Mathf.Sin(p) * 30;
            mTailPlay[0].rectTransform.localEulerAngles = angle;
            mTailPlay[1].rectTransform.localEulerAngles = angle;
            mTailPlay[2].rectTransform.localEulerAngles = angle;

            yield return new WaitForSeconds(0.01f);
            p += 0.2f;
            
        }
    }
    IEnumerator TJump(bool from_right)
    {

        AudioSource sound = gameObject.AddComponent<AudioSource>();
        sound.clip = ResManager.GetClip("shapelogic_sound", "弹簧");
        sound.volume = 0.2f;


        mRtran.localScale = new Vector3(0.65f, 0.65f, 1);
        Vector3 pos0 = Vector3.zero;
        Vector3 pos1 = mRtran.anchoredPosition3D;
        if(from_right)
        {
            pos0 = pos1 + new Vector3(1423, 0);
        }
        else
        {
            pos0 = pos1 - new Vector3(1423, 0);
        }

        bool temp_sound = true;
        float side = Mathf.PI * 0.5f;// * 3f * 0.5f;
        for (float i = 0; i < 1f; i += 0.006f)
        {
            float p = Mathf.PI * 8 * i;
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos1, i) + new Vector3(0, Mathf.Abs( Mathf.Sin(p)) * 100, 0);

            while (p > Mathf.PI)
                p -= Mathf.PI;


            if (temp_sound)
            {
                if(p > side)
                {
                    sound.Play();
                    temp_sound = false;
                }
            }
            else
            {
                if (p < side)
                {
                    temp_sound = true;
                }
            }

            
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition3D = pos1;

        mRtran.localScale = mScale;
        ReflushUI_DefaultTail();

        //float i = 0;
        //Color a = Color.Lerp(Color.white, new Color(1, 1, 1, 0), i) + new Color(0, 0, 0, -1 * Mathf.Abs(Mathf.Sin(Mathf.PI * 2 * i)) * 0.2f);


        //Destroy(sound);

    }
    IEnumerator TInitAnswer()
    {
        mHead.gameObject.SetActive(false);
        mBody.gameObject.SetActive(false);
        mTail.enabled = false;
        mTailType.enabled = false;
        mTailPlay[0].gameObject.SetActive(false);

        yield return new WaitForSeconds(5);
        mBg = UguiMaker.newImage("mBg", transform, "shapelogic_sprite", "guanka9_frame");
        mBg.type = Image.Type.Sliced;
        mBg.transform.SetAsFirstSibling();
        mBg.rectTransform.anchoredPosition = new Vector2(3.84f, 29.4f);

        for (float i = 0; i < 1f; i += 0.08f)
        {
            mBg.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(0, 0), new Vector2(220, 300), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mBg.rectTransform.sizeDelta = new Vector2(220, 290);

    }


    public void PlayCorrect()
    {
        if (null != mBg)
            Destroy(mBg.gameObject);
        ReflushUI_PlayTail(true);
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
        StartCoroutine("TPlayShoot");
    }
    IEnumerator TPlayShoot()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("素材出去通用");
        Vector2 pos0 = mRtran.anchoredPosition;
        Vector2 pos_old = mRtran.anchoredPosition;
        float h = (400 - mRtran.anchoredPosition.y) * 0.7f;// Random.Range(200, 400 - mRtran.anchoredPosition.y);
        float w = Random.Range(0, 1000) % 2 == 0 ? 1300: -1000;
        for(float i = 0; i < 1f; i += 0.01f)
        {
            mRtran.anchoredPosition = pos0 + new Vector2(w * i, Mathf.Sin(Mathf.PI * 0.5f * i) * h);
            float angle = Vector2.Angle(mRtran.anchoredPosition - pos_old, Vector2.up);
            if(w < 0)
            {
                mRtran.localEulerAngles = new Vector3(0, 0, angle);
            }
            else
            {
                mRtran.localEulerAngles = new Vector3(0, 0, -angle);
            }
            pos_old = mRtran.anchoredPosition;
            yield return new WaitForSeconds(0.01f);
        }
        

    }


}
