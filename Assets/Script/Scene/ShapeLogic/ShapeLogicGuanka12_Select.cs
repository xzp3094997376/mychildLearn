using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka12_Select : MonoBehaviour {

    public RectTransform mRtran;
    public Image mTool, mSelect, mLineAnchor;
    public string mdata_thing;
    public int mdata_type;

    Vector3 mResetPos = Vector3.zero;
    Vector3 mFlyPos = Vector3.zero;

    public void Init(string thing, int type)
    {
        mRtran = GetComponent<RectTransform>();
        mdata_thing = thing;
        mdata_type = type;

        switch (thing)
        {
            case "bg0":
                { 
                    mSelect = UguiMaker.newImage("mSelect", transform, "shapelogic_sprite", "guanka12_stationbg1");
                    mTool = UguiMaker.newImage("mTool", transform, "shapelogic_sprite", "guanka12_step0", false);
                    switch (type)
                    {
                        case 0:
                            mSelect.color = new Color32(255, 255, 255, 0);
                            break;
                        case 1:
                            mSelect.color = new Color32(44, 216, 65, 255);
                            break;
                        case 2:
                            mSelect.color = new Color32(251, 150, 245, 255);
                            break;
                        case 3:
                            mSelect.color = new Color32(131, 168, 255, 255);
                            break;
                    }
                    mTool.rectTransform.anchoredPosition = new Vector2(0, -33.06f);
                    mSelect.gameObject.AddComponent<Button>().onClick.AddListener(ClkBg0);
                }
                break;
            case "bg1":
                {
                    mSelect = UguiMaker.newImage("mSelect", transform, "shapelogic_sprite", "guanka12_stationbg0");
                    mTool = UguiMaker.newImage("mTool", transform, "shapelogic_sprite", "guanka12_step2", false);
                    switch (type)
                    {
                        case 0:
                            mSelect.color = new Color32(255, 255, 255, 0);
                            break;
                        case 1:
                            mSelect.color = new Color32(255, 133, 133, 255);
                            break;
                        case 2:
                            mSelect.color = new Color32(158, 238, 167, 255);
                            break;
                        case 3:
                            mSelect.color = new Color32(255, 167, 250, 255);
                            break;
                    }
                    mSelect.rectTransform.pivot = new Vector2(0.07f, 0.95f);
                    mSelect.rectTransform.anchoredPosition = Vector2.zero;
                    mSelect.gameObject.AddComponent<Button>().onClick.AddListener(ClkBg1);

                }
                break;
            case "fruit":
                {
                    mTool = UguiMaker.newImage("mTool", transform, "public", "white", false);
                    mTool.type = Image.Type.Sliced;
                    mTool.rectTransform.sizeDelta = new Vector2(4, 165);
                    mTool.rectTransform.pivot = new Vector2(0.5f, 1);
                    mTool.rectTransform.anchoredPosition = Vector2.zero;

                    mLineAnchor = UguiMaker.newImage("mTool", mTool.transform, "shapelogic_sprite", "guanka12_step2", false);
                    mLineAnchor.rectTransform.anchorMax = new Vector2(0.5f, 0);
                    mLineAnchor.rectTransform.anchorMin = new Vector2(0.5f, 0);
                    mLineAnchor.rectTransform.anchoredPosition = Vector2.zero;

                    mSelect = UguiMaker.newImage("mSelect", mTool.transform, "shapelogic_sprite", "guanka12_fruit" + type);
                    mSelect.rectTransform.anchorMax = new Vector2(0.5f, 0);
                    mSelect.rectTransform.anchorMin = new Vector2(0.5f, 0);
                    mSelect.rectTransform.anchoredPosition = Vector2.zero;
                    mSelect.rectTransform.localEulerAngles = new Vector3(0, 0, Random.Range(-20, 20));
                    mSelect.gameObject.AddComponent<Button>().onClick.AddListener(ClkFruit);

                }
                break;
            case "top":
                {
                    mTool = UguiMaker.newImage("mTool", transform, "shapelogic_sprite", "guanka12_step1", false);
                    mSelect = UguiMaker.newImage("mSelect", transform, "shapelogic_sprite", "guanka12_type" + type);
                    mTool.rectTransform.localEulerAngles = new Vector3(0, 0, 90);
                    mSelect.gameObject.AddComponent<Button>().onClick.AddListener(ClkTop);
                }
                break;

        }





    }


    public void PlayFruit()
    {
        StartCoroutine("TPlayFruit");
    }
    IEnumerator TPlayFruit()
    {
        float p = 0;
        while (true)
        {
            mTool.rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(p) * 5);
            p += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }
    }


    public void PlayBg1()
    {
        StartCoroutine("TPlayBg1");
    }
    IEnumerator TPlayBg1()
    {
        float p = 0;
        while(true)
        {
            mSelect.rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(p) * 5 - 45);
            p += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void ClkInit()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("按钮0");
        ShapeLogicGuanka12_Station station = ShapeLogicCtl.instance.mGuankaCtl12.mStation[ShapeLogicCtl.instance.mGuankaCtl12.mdata_answer_index];
        mFlyPos = station.mRtran.anchoredPosition3D - mRtran.anchoredPosition3D;
    }
    public void ClkBg0()
    {
        ClkInit();
        //mSelect.raycastTarget = false;
        StartCoroutine("TFly");

    }
    public void ClkBg1()
    {
        ClkInit();
        //mSelect.raycastTarget = false;
        StopAllCoroutines();
        StartCoroutine("TFly");

    }
    public void ClkFruit()
    {
        ClkInit();
        //mSelect.raycastTarget = false;
        //StopAllCoroutines();
        StartCoroutine("TFly");

    }
    public void ClkTop()
    {
        ClkInit();
        mFlyPos.y += 92;
        //mSelect.raycastTarget = false;
        StartCoroutine("TFly");

    }
    IEnumerator TFly()
    {
        ShapeLogicCtl.instance.mGuankaCtl12.callback_SetStation(mdata_thing, mdata_type);
        mSelect.transform.SetParent(transform);
        Vector3 pos0 = mSelect.rectTransform.anchoredPosition3D;
        Vector3 angle = Common.Parse180( mSelect.rectTransform.localEulerAngles);
        //Debug.Log("pos0=" + pos0 + " fly_pos=" + mFlyPos);

        for(float i = 0; i < 1f; i += 0.05f)
        {
            mSelect.rectTransform.anchoredPosition3D = Vector3.Lerp(pos0, mFlyPos, i) + new Vector3(0, Mathf.Sin(Mathf.PI * i) * 200, 0);
            mSelect.rectTransform.localEulerAngles = Vector3.Lerp(angle, Vector3.zero, i);
            yield return new WaitForSeconds(0.01f);
        }
        mSelect.rectTransform.anchoredPosition3D = mFlyPos;
        mSelect.gameObject.SetActive(false);
        ShapeLogicCtl.instance.mGuankaCtl12.callback_FlushStation();
        //yield return new WaitForSeconds(0.2f);
        ShapeLogicCtl.instance.mGuankaCtl12.SetSelectClickEnable(mdata_thing, true);

    }


    public void ResetFly()
    {
        if(mSelect.gameObject.activeSelf)
        {
            return;
        }
        StopAllCoroutines();
        StartCoroutine("TResetFly");

    }
    IEnumerator TResetFly()
    {
        switch(mdata_thing)
        {
            case "bg0":
            case "bg1":
            case "top":
                {
                    mSelect.gameObject.SetActive(true);
                    mSelect.transform.SetParent(transform);
                    Vector3 pos0 = mSelect.rectTransform.anchoredPosition3D;
                    //Debug.Log("pos0=" + pos0 + " fly_pos=" + mFlyPos);
                    for (float i = 0; i < 1f; i += 0.05f)
                    {
                        mSelect.rectTransform.anchoredPosition3D = Vector3.Lerp(pos0, Vector3.zero, i) + new Vector3(0, Mathf.Sin(Mathf.PI * i) * 200, 0);
                        yield return new WaitForSeconds(0.01f);
                    }
                    mSelect.rectTransform.anchoredPosition3D = Vector3.zero;

                }
                break;
            case "fruit":
                {
                    mSelect.gameObject.SetActive(true);
                    mSelect.transform.SetParent(transform);
                    Vector3 pos0 = mSelect.rectTransform.anchoredPosition3D;
                    //Debug.Log("pos0=" + pos0 + " fly_pos=" + mFlyPos);
                    for (float i = 0; i < 1f; i += 0.05f)
                    {
                        Vector3 p = transform.worldToLocalMatrix.MultiplyPoint(mLineAnchor.rectTransform.position);
                        mSelect.rectTransform.anchoredPosition3D = Vector3.Lerp(pos0, p, i) + new Vector3(0, Mathf.Sin(Mathf.PI * i) * 200, 0);
                        yield return new WaitForSeconds(0.01f);
                    }
                    mSelect.transform.SetParent(mTool.transform);
                    mSelect.rectTransform.anchoredPosition3D = Vector3.zero;

                }
                break;
        }
        mSelect.raycastTarget = true;

        switch (mdata_thing)
        {
            case "bg0":
                break;
            case "bg1":
                PlayBg1();
                break;
            case "top":
                break;
            case "fruit":
                PlayFruit();
                break;
        }

    }


    public void Hide()
    {
        mSelect.raycastTarget = false;
        StopAllCoroutines();
        StartCoroutine(THide());
    }
    IEnumerator THide()
    {
        switch (mdata_thing)
        {
            case "bg0":
                {
                    yield return new WaitForSeconds(0.5f);
                    ShapeLogicCtl.instance.mSound.PlayShort("素材出去通用");
                    Vector3 pos0 = mRtran.anchoredPosition3D;
                    Vector3 pos1 = mRtran.anchoredPosition3D + new Vector3(0, 600, 0);
                    for (float i = 0; i < 1f; i += 0.05f)
                    {
                        mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i - Mathf.PI * 0.5f) + 1);
                        yield return new WaitForSeconds(0.01f);
                    }


                }
                break;
            case "bg1":
                {
                    Vector3 pos0 = mSelect.rectTransform.anchoredPosition3D;
                    Vector3 pos1 = mSelect.rectTransform.anchoredPosition3D + new Vector3(0, -350, 0);
                    for (float i = 0; i < 1f; i += 0.05f)
                    {
                        mSelect.rectTransform.anchoredPosition3D = Vector3.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i - Mathf.PI * 0.5f) + 1);
                        yield return new WaitForSeconds(0.01f);
                    }
                    for (float i = 0; i < 1f; i += 0.05f)
                    {
                        mTool.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, Mathf.Sin(Mathf.PI * 0.5f * i - Mathf.PI * 0.5f) + 1);
                        yield return new WaitForSeconds(0.01f);
                    }
                    mTool.rectTransform.localScale = Vector3.zero;

                }
                break;
            case "top":
                {
                    for (float i = 0; i < 1f; i += 0.05f)
                    {
                        mRtran.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 0, 45), i);
                        yield return new WaitForSeconds(0.01f);
                    }

                    ShapeLogicCtl.instance.mSound.PlayShort("素材出去通用");
                    Vector3 pos0 = mRtran.anchoredPosition3D;
                    Vector3 pos1 = mRtran.anchoredPosition3D + new Vector3(500, 500, 0);
                    for (float i = 0; i < 1f; i += 0.03f)
                    {
                        mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
                        yield return new WaitForSeconds(0.01f);
                    }

                }
                break;
            case "fruit":
                {
                    Vector3 pos0 = mTool.rectTransform.anchoredPosition3D;
                    Vector3 pos1 = mTool.rectTransform.anchoredPosition3D + new Vector3(0, 250, 0);
                    for (float i = 0; i < 1f; i += 0.05f)
                    {
                        mTool.rectTransform.anchoredPosition3D = Vector3.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
                        yield return new WaitForSeconds(0.01f);
                    }

                }
                break;
        }
    }

}
