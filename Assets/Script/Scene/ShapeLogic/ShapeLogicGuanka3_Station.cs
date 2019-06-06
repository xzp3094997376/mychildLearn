using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka3_Station : MonoBehaviour {
    public static ShapeLogicGuanka3_Station gSelect { get; set; }

    public List<int> color_ids = new List<int>();
    List<Image> mFlowers = new List<Image>();
    List<ShapeLogicGuanka3_Flower> mJumpEatFlower = new List<ShapeLogicGuanka3_Flower>();
    List<Vector3> mdata_flower_poss = new List<Vector3>();

    public RectTransform mRtran { get; set; }

    Image mBg { get; set; }
    Image mGan { get; set; }
    Image[] mYezi { get; set; }
    BoxCollider mBox { get; set; }
    Vector3 mResetPos = Vector3.zero;

    public bool mdata_is_question = true;
    public bool mdata_is_over = false;


    public void InitQuestion(List<int> _color_ids)
    {
        mdata_is_question = true;
        color_ids = _color_ids;
        mdata_flower_poss = new List<Vector3>()
        {
            new Vector3(-50, -145, 0),
            new Vector3(50, -145, 0),
            new Vector3(50, -220, 0),
            new Vector3(-50, -220, 0),
        };

        mRtran = gameObject.GetComponent<RectTransform>();
        mBg = UguiMaker.newImage("mBg", transform, "shapelogic_sprite", "guanka3_stationbg", false);
        mBg.type = Image.Type.Sliced;
        mBg.rectTransform.pivot = new Vector2(0.5f, 0.95f);
        mBg.rectTransform.anchoredPosition3D = Vector3.zero;

        mBox = gameObject.AddComponent<BoxCollider>();
        mBox.size = new Vector3(214.15f, 318.7f, 1);
        mBox.center = new Vector3(0, -139.85f, 0);

    }
    public void InitAnswer(List<int> _color_ids)
    {
        InitQuestion(_color_ids);
        mBg.rectTransform.sizeDelta = new Vector2(221, 125);
        mdata_is_question = false;

    }
    public void SetBoxEnable(bool _enable)
    {
        mBox.enabled = _enable;
    }
    public void Select()
    {
        transform.SetAsLastSibling();
        StartCoroutine("TShake");

    }
    public void SetResetPos(Vector3 reset_pos)
    {
        mResetPos = reset_pos;
    }


    void Update () {
	
	}

    public void AddJumpEatFlower(ShapeLogicGuanka3_Flower flower)
    {
        mJumpEatFlower.Add(flower);
    }
    public void Jump(RectTransform jump_before, RectTransform jump_mid, RectTransform jump_after, Vector3 endpos, bool show_flower)
    {

        StartCoroutine(TJump(jump_before, jump_mid, jump_after, endpos, show_flower));

    }
    IEnumerator TJump(RectTransform jump_before, RectTransform jump_mid, RectTransform jump_after, Vector3 endpos, bool show_flower)
    {
        ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("科学幻想小说菜单输入");


        mRtran.SetParent(jump_before);
        mBg.rectTransform.sizeDelta = new Vector2(221, 125);
        for (float i = 0;  i < 1f; i += 0.05f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(new Vector3(endpos.x, -430, 0), new Vector3(endpos.x, 200, 0), i);
            //mBg.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(221, 125), new Vector2(221, 300), i);
            yield return new WaitForSeconds(0.01f);
        }
        for (float i = 0; i < 1f; i += 0.1f)
        {
            //mRtran.anchoredPosition3D = Vector3.Lerp(new Vector3(endpos.x, -430, 0), new Vector3(endpos.x, 200, 0), i);
            mBg.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(221, 125), new Vector2(221, 300), i);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.8f);
        mRtran.SetParent(jump_mid);
        Vector3 temp_pos = mRtran.anchoredPosition3D;
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(temp_pos, new Vector3(endpos.x, -100, 0), i);
            yield return new WaitForSeconds(0.01f);
        }

        //吃
        ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("吃");
        bool is_eat = false;
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mBg.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(221, 300), new Vector2(221, 125), i);
            if(!is_eat && 0.5f < i)
            {
                is_eat = true;
                for (int j = 0; j < mJumpEatFlower.Count; j++)
                {
                    mJumpEatFlower[j].EatFlower();
                }
            }
            yield return new WaitForSeconds(0.01f);
        }

        temp_pos = mRtran.anchoredPosition3D;
        
        //
        for(int i = 0; i < 4; i++)
        {
            Image flower = UguiMaker.newImage("flower" + i.ToString(), transform, "shapelogic_sprite", "guanka3_color" + color_ids[i].ToString(), false);
            flower.rectTransform.anchoredPosition3D = mdata_flower_poss[i];
            flower.transform.localScale = Vector3.zero;
            mFlowers.Add(flower);

        }
        
        //上移
        for (float i = 0; i < 1f; i += 0.04f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(temp_pos, endpos, i);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition3D = endpos;
        mBg.rectTransform.sizeDelta = new Vector2(221, 300);

        //张嘴
        Vector2 size_bg = new Vector2(221, 310);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mBg.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(221, 125), size_bg, i);
            yield return new WaitForSeconds(0.01f);
        }
        mBg.rectTransform.sizeDelta = size_bg;


        //展示花
        if (show_flower)
        {
            StartCoroutine("TShowFlower");

        }


    }
    IEnumerator TShowFlower()
    {
        Vector3 size_flower = new Vector3(0.85f, 0.85f, 1);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            Vector3 scale = Vector3.Lerp(Vector3.zero, size_flower, i + Mathf.Sin(Mathf.PI * i) * 0.5f);
            for (int j = 0; j < 4; j++)
            {
                mFlowers[j].transform.localScale = scale;
                mFlowers[j].transform.localEulerAngles += new Vector3(0, 0, 10);
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int j = 0; j < 4; j++)
        {
            mFlowers[j].transform.localScale = size_flower;
        }

    }

    public void OpenMouse_ShowFlower()
    {
        StartCoroutine("TOpenMouse_ShowFlower");
    }
    IEnumerator TOpenMouse_ShowFlower()
    {
        for (int i = 0; i < 4; i++)
        {
            Image flower = UguiMaker.newImage("flower" + i.ToString(), transform, "shapelogic_sprite", "guanka3_color" + color_ids[i].ToString(), false);
            flower.rectTransform.anchoredPosition3D = mdata_flower_poss[i];
            flower.transform.localScale = Vector3.zero;
            mFlowers.Add(flower);

        }

        Vector2 size_bg = new Vector2(221, 310);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mBg.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(221, 125), size_bg, i);
            yield return new WaitForSeconds(0.01f);
        }
        mBg.rectTransform.sizeDelta = size_bg;

        Vector3 size_flower = new Vector3(0.85f, 0.85f, 1);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            Vector3 scale = Vector3.Lerp(Vector3.zero, size_flower, i + Mathf.Sin(Mathf.PI * i) * 0.5f);
            for (int j = 0; j < 4; j++)
            {
                mFlowers[j].transform.localScale = scale;
                mFlowers[j].transform.localEulerAngles += new Vector3(0, 0, 10);
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int j = 0; j < 4; j++)
        {
            mFlowers[j].transform.localScale = size_flower;
        }

        SetBoxEnable(true);

    }

    IEnumerator TShake()
    {
        float p = 0;
        while (true)
        {
            mRtran.localEulerAngles = new Vector3(0, 0, Mathf.Sin(p) * 3);
            p += 0.2f;
            if(null != mFlowers)
            {
                for(int i = 0; i < mFlowers.Count; i++)
                {
                    mFlowers[i].rectTransform.localEulerAngles += new Vector3(0, 0, 5);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }


    }

    public void CorrectShowFlower()
    {
        StartCoroutine("TShowFlower");
    }
    public void PlayError()
    {
        StopCoroutine("TShake");
        mRtran.localEulerAngles = Vector3.zero;
        StartCoroutine("TPlayError");
    }
    IEnumerator TPlayError()
    {
        SetBoxEnable(false);
        //收花
        Vector3 size_flower = new Vector3(0.85f, 0.85f, 1);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            Vector3 scale = Vector3.Lerp(size_flower, Vector3.zero, i);
            for (int j = 0; j < 4; j++)
            {
                mFlowers[j].transform.localScale = scale;
                mFlowers[j].transform.localEulerAngles -= new Vector3(0, 0, 10);
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int j = 0; j < 4; j++)
        {
            mFlowers[j].transform.localScale = Vector3.zero;
        }
        //闭嘴
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mBg.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(221, 300), new Vector2(221, 125), i);
            yield return new WaitForSeconds(0.01f);
        }
        //复位
        while (40 < Vector3.Distance(mResetPos, mRtran.anchoredPosition3D))
        {
            Vector3 dir = (mResetPos - mRtran.anchoredPosition3D).normalized * 40;
            dir.z = 0;
            mRtran.anchoredPosition3D += dir;
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition3D = mResetPos;
        //张嘴
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mBg.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(221, 125), new Vector2(221, 300), i);
            yield return new WaitForSeconds(0.01f);
        }
        mBg.rectTransform.sizeDelta = new Vector2(221, 300);

        StartCoroutine("TShowFlower");

        SetBoxEnable(true);

    }

    public void ResetPos()
    {
        StopCoroutine("TShake");
        mRtran.localEulerAngles = Vector3.zero;
        StartCoroutine("TResetPos");

    }
    IEnumerator TResetPos()
    {
        SetBoxEnable(false);

        while (40 < Vector3.Distance(mResetPos, mRtran.anchoredPosition3D))
        {
            Vector3 dir = (mResetPos - mRtran.anchoredPosition3D).normalized * 40;
            dir.z = 0;
            mRtran.anchoredPosition3D += dir;

            yield return new WaitForSeconds(0.01f);

        }

        mRtran.anchoredPosition3D = mResetPos;
        SetBoxEnable(true);

    }
    
    public void Falldown()
    {
        StartCoroutine("TFalldown");
    }
    IEnumerator TFalldown()
    {
        mBox.size = new Vector3(254, 318, 1);
        mBox.center = new Vector3(0, 258, 0);

        for (int i = 0; i < mFlowers.Count; i++)
        {
            mFlowers[i].transform.SetParent(mBg.transform);
        }
        Vector3 pos0 = mRtran.anchoredPosition3D;
        Vector3 pos1 = new Vector3(mRtran.anchoredPosition3D.x, -350, 0);
        for(float i = 0; i < 1f; i += 0.08f)
        {
            mBg.rectTransform.localEulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, 180), i);
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos1, i);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition3D = pos1;
        mBg.rectTransform.localEulerAngles = new Vector3(0, 0, 180);

        mGan = UguiMaker.newImage("mGan", transform, "public", "white0", false);
        mGan.transform.SetAsFirstSibling();
        mGan.type = Image.Type.Sliced;
        mGan.rectTransform.localEulerAngles = new Vector3(0, 0, 90);
        mGan.rectTransform.pivot = new Vector2(0, 0.5f);
        mGan.color = new Color32(0, 126, 6, 255);

        Vector2 size0 = new Vector3(0, 8);
        Vector2 size1 = new Vector2(60, 8);
        Vector3 size2 = new Vector3(110, 8);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mGan.rectTransform.sizeDelta = Vector2.Lerp(size0, size1, i);
            mBg.rectTransform.anchoredPosition3D = new Vector3(0, mGan.rectTransform.sizeDelta.x, 0);
            yield return new WaitForSeconds(0.01f);
        }
        mGan.rectTransform.sizeDelta = size1;


        mYezi = new Image[2];
        mYezi[0] = UguiMaker.newImage("yezi", transform, "shapelogic_sprite", "guanka3_yezi", false);
        mYezi[1] = UguiMaker.newImage("yezi", transform, "shapelogic_sprite", "guanka3_yezi", false);
        mYezi[0].transform.SetSiblingIndex(1);
        mYezi[1].transform.SetSiblingIndex(2);
        mYezi[1].rectTransform.localScale = new Vector3(-1, 1, 1);
        mYezi[0].rectTransform.pivot = new Vector2(0, 0);
        mYezi[1].rectTransform.pivot = new Vector2(0, 0);
        mYezi[0].rectTransform.anchoredPosition3D = new Vector3(0, size1.x);
        mYezi[1].rectTransform.anchoredPosition3D = new Vector3(0, size1.x);
        mYezi[0].transform.localEulerAngles = new Vector3(0, 0, 0);
        mYezi[1].transform.localEulerAngles = new Vector3(0, 0, 0);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            Vector3 scale0 = Vector3.Lerp(Vector3.zero, Vector3.one, i + Mathf.Sin(Mathf.PI * i) * 0.5f);
            Vector3 scale1 = new Vector3(scale0.x * -1, scale0.y, scale0.z);
            mYezi[0].transform.localScale = scale0;
            mYezi[1].transform.localScale = scale1;
            yield return new WaitForSeconds(0.01f);

        }


        for (float i = 0; i < 1f; i += 0.05f)
        {
            mGan.rectTransform.sizeDelta = Vector2.Lerp(size1, size2, i);
            mBg.rectTransform.anchoredPosition3D = new Vector3(0, mGan.rectTransform.sizeDelta.x, 0);
            yield return new WaitForSeconds(0.01f);
        }
        mGan.rectTransform.sizeDelta = size2;
        mBg.rectTransform.anchoredPosition3D = new Vector3(0, mGan.rectTransform.sizeDelta.x, 0);

        SetBoxEnable(true);

        while (true)
        {
            for(float i = 0; i < 1f; i += 0.01f)
            {
                mBg.rectTransform.localEulerAngles = new Vector3(0, 0, 180 + Mathf.Sin(Mathf.PI * 2 * i) * 5);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    public void OnClkOver()
    {

        mdata_is_over = true;
        //StopAllCoroutines();
        StartCoroutine("TOnClkOver");
    }
    IEnumerator TOnClkOver()
    {
        for(int i = 0; i < mFlowers.Count; i++)
        {
            mFlowers[i].transform.SetParent(transform);
        }
        StartCoroutine("TFlyFlower");
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mBg.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(221, 300), new Vector2(221, 125), i);
            yield return new WaitForSeconds(0.01f);
        }
        mBg.rectTransform.sizeDelta = new Vector2(221, 125);


    }
    IEnumerator TFlyFlower()
    {
        ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("剪断");
        Vector3[] dir = new Vector3[4];
        dir[0] = (mFlowers[0].rectTransform.anchoredPosition3D - mFlowers[2].rectTransform.anchoredPosition3D).normalized;
        dir[1] = (mFlowers[1].rectTransform.anchoredPosition3D - mFlowers[3].rectTransform.anchoredPosition3D).normalized;
        dir[2] = (mFlowers[2].rectTransform.anchoredPosition3D - mFlowers[0].rectTransform.anchoredPosition3D).normalized;
        dir[3] = (mFlowers[3].rectTransform.anchoredPosition3D - mFlowers[1].rectTransform.anchoredPosition3D).normalized;
        while(true)
        {
            for(int i = 0; i < mFlowers.Count; i++)
            {
                mFlowers[i].rectTransform.anchoredPosition3D += dir[i] * 30;
                mFlowers[i].rectTransform.localEulerAngles += new Vector3(0, 0, 10);
            }
            yield return new WaitForSeconds(0.01f);
        }


    }



}
