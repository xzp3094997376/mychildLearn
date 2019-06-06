using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class ShapeLogicGuanka12_Station : MonoBehaviour
{
    public RectTransform mRtran { get; set; }
    public Image mFrame;
    public Image mBg0, mBg1, mFruit, mTop;
    public BoxCollider mBox;
    //public ParticleSystem mEffect { get; set; }

    Vector3 mScale = new Vector3(0.9f, 0.9f, 1);
    public bool mdata_is_over = false;

    public int mdata_bg0 = 0;
    public int mdata_bg1 = 0;
    public int mdata_fruit = 0;
    public int mdata_top = 0;
    

    public void Init(int bg0, int bg1, int fruit, int top)
    {
        mRtran = gameObject.GetComponent<RectTransform>();
        mRtran.localScale = mScale;

        mdata_bg0 = bg0;
        mdata_bg1 = bg1;
        mdata_fruit = fruit;
        mdata_top = top;

        if (null == mBg0)
        {

            mBg0 = UguiMaker.newImage("mBg0", transform, "shapelogic_sprite", "guanka12_stationbg1", false);
            mBg1 = UguiMaker.newImage("mBg1", transform, "shapelogic_sprite", "guanka12_stationbg0", false);
            mTop = UguiMaker.newImage("mTop", transform, "shapelogic_sprite", "guanka12_type1", false);
            mFruit = UguiMaker.newImage("mFruit", transform, "shapelogic_sprite", "guanka12_fruit1", false);
            mTop.rectTransform.pivot = new Vector2(0.5f, 0);
            mTop.rectTransform.anchoredPosition = new Vector2(0, 92);
        }

    }
    public void InitFrameBg()
    {
        StartCoroutine("TInitFrameBg");
    }
    IEnumerator TInitFrameBg()
    {
        yield return new WaitForSeconds(2f);
        mFrame = UguiMaker.newImage("mFrame", transform, "shapelogic_sprite", "guanka11_frame", false);
        mFrame.transform.SetAsFirstSibling();
        mFrame.type = Image.Type.Sliced;
        mFrame.rectTransform.sizeDelta = new Vector2(190, 265);
        mFrame.rectTransform.anchoredPosition = new Vector2(0, 18);
        for(float i = 0; i < 1f; i += 0.1f)
        {
            mFrame.rectTransform.sizeDelta = Vector2.Lerp(Vector2.zero, new Vector2(190, 265), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f); ;
        }
    }
    public void BeginIn()
    {
        StartCoroutine("TBeginIn");
    }
    IEnumerator TBeginIn()
    {
        mFruit.enabled = false;
        yield return new WaitForSeconds(0.5f + Random.Range(0.01f, 1f));
        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效", 0.5f);
        mFruit.enabled = true;
        mFruit.transform.SetParent(transform.parent);
        mFruit.transform.SetAsLastSibling();
        Vector3 pos0 = mFruit.rectTransform.anchoredPosition3D; 
        Vector3 pos1 = mFruit.rectTransform.anchoredPosition3D;
        pos0.y = -450;
        pos0.x = pos0.x + Random.Range(-800, 800);
        //mFruit.rectTransform.anchoredPosition3D = pos0;
        float a = pos1.y + 400;
        for (float i = 0; i < 1; i += 0.03f)
        {
            mFruit.rectTransform.anchoredPosition3D = Vector3.Lerp(pos0, pos1, i) + new Vector3(0, Mathf.Sin(Mathf.PI * i) * a * 0.5f, 0);
            mFruit.rectTransform.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 360), Vector3.zero, i);
            yield return new WaitForSeconds(0.01f);
        }
        mFruit.rectTransform.anchoredPosition3D = pos1;
        mFruit.rectTransform.localEulerAngles = Vector3.zero;
        mFruit.transform.SetParent(transform);


        yield break;
    }


    public void SetBoxEnable(bool _enable)
    {
        if (null == mBox)
        {
            mBox = gameObject.AddComponent<BoxCollider>();
            mBox.size = new Vector3(173, 200, 1);
            mBox.center = new Vector3(0, 0, 0);
        }
        mBox.enabled = _enable;

    }


    public void ReflushUI()
    {
        switch (mdata_bg0)
        {
            case 0:
                mBg0.color = new Color32(255, 255, 255, 0);
                break;
            case 1:
                mBg0.color = new Color32(44, 216, 65, 255);
                break;
            case 2:
                mBg0.color = new Color32(251, 150, 245, 255);
                break;
            case 3:
                mBg0.color = new Color32(131, 168, 255, 255);
                break;

        }
        switch (mdata_bg1)
        {
            case 0:
                mBg1.color = new Color32(255, 255, 255, 0);
                break;
            case 1:
                mBg1.color = new Color32(255, 133, 133, 255);
                break;
            case 2:
                mBg1.color = new Color32(158, 238, 167, 255);
                break;
            case 3:
                mBg1.color = new Color32(255, 167, 250, 255);
                break;
        }
        if(0 == mdata_fruit)
        {
            mFruit.color = new Color(255, 255, 255, 0);
        }
        else
        {
            mFruit.color = new Color(255, 255, 255, 255);
            mFruit.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka12_fruit" + mdata_fruit.ToString());
            mFruit.SetNativeSize();
        }
        if (0 == mdata_top)
        {
            mTop.color = new Color(255, 255, 255, 0);
        }
        else
        {
            mTop.color = new Color(255, 255, 255, 255);
            mTop.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka12_type" + mdata_top.ToString());
            mTop.SetNativeSize();
        }


    }
    public void Play()
    {
        StartCoroutine("TPlay");
    }
    IEnumerator TPlay()
    {
        if (null != mFrame)
        {
            Destroy(mFrame.gameObject);
        }
        Vector3 pos0 = mFruit.rectTransform.anchoredPosition3D;
        float p = 0;
        while (true)
        {
            mFruit.rectTransform.anchoredPosition3D = pos0 + new Vector3(0, Mathf.Abs(Mathf.Sin(p)) * -5, 0);
            p += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }

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

        yield break;

    }


    public void Hide()
    {
        StartCoroutine(THide());
    }
    IEnumerator THide()
    {
        Vector3 pos0 = mRtran.anchoredPosition3D;
        Vector3 pos1 = mRtran.anchoredPosition3D + new Vector3(0, 600, 0);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, Mathf.Sin(Mathf.PI * 0.5f * i - Mathf.PI * 0.5f) + 1);
            mRtran.localEulerAngles += new Vector3(0, 0, -5);
            yield return new WaitForSeconds(0.01f);
        }
        gameObject.SetActive(false);

    }

}
