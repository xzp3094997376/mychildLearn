using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka5_Station : MonoBehaviour
{
    public RectTransform mRtran { get; set; }
    Image[] mHorns = new Image[2];
    Image[] mHornTail = new Image[6];
    Image[] mColorAreaRight = new Image[4];
    Image[] mColorAreaLeft = new Image[4];
    Image mHornLink { get; set; }

    Button mBtn { get; set; }

    Color32 mdata_color0 = new Color32(161, 62, 223, 255);
    Color32 mdata_color1 = new Color32(245, 160, 200, 255);
    Vector2 mdata_link_size_min = new Vector2(27, 8.2f);
    Vector2 mdata_link_size_max = new Vector2(200, 8.2f);
    Vector3[] mdata_horn_angle_min = new Vector3[] { new Vector3(0, 0, -40), new Vector3(0, 0, 40) };
    Vector2 mdata_tail_top = new Vector2(0, 7.41f);
    Vector2 mdata_tail_down = new Vector2(0, 0);
    Vector2 mdata_area_size_min = new Vector2(95, 0);
    Vector2 mdata_area_size_max = new Vector2(95, 60);
    public List<int> mdata_colors;

    Vector3 mResetPos = Vector3.zero;

    public bool mdata_is_answer = false;
    public bool mdata_is_showing = false;
    public bool mdata_is_over = false;
    public void Init()
    {
        mdata_is_showing = false;
        mRtran = gameObject.GetComponent<RectTransform>();

        
        
        

        mHornLink = UguiMaker.newImage("link", transform, "public", "white", false);
        mHornLink.color = new Color32(181, 137, 18, 255);
        mHornLink.rectTransform.sizeDelta = mdata_link_size_min;

        mHorns[0] = UguiMaker.newImage("horn0", mHornLink.transform, "shapelogic_sprite", "guanka5_station1", false);
        mHorns[1] = UguiMaker.newImage("horn1", mHornLink.transform, "shapelogic_sprite", "guanka5_station1", false);
        mHorns[0].rectTransform.anchorMin = new Vector2(0, 0.5f);
        mHorns[0].rectTransform.anchorMax = new Vector2(0, 0.5f);
        mHorns[1].rectTransform.anchorMin = new Vector2(1, 0.5f);
        mHorns[1].rectTransform.anchorMax = new Vector2(1, 0.5f);
        mHorns[0].rectTransform.pivot = new Vector2(0.75f, 0.43f);
        mHorns[1].rectTransform.pivot = new Vector2(0.75f, 0.43f);
        mHorns[0].rectTransform.localEulerAngles = new Vector3(0, 0, -40);
        mHorns[1].rectTransform.localEulerAngles = new Vector3(0, 0, 40);
        mHorns[1].rectTransform.localScale = new Vector3(-1, 1, 1);

        Vector2[] tail_piovt = new Vector2[] {
            new Vector2(0.142f, 0.5f),
            new Vector2(0.2857f, 0.5f),
            new Vector2(0.4285f, 0.5f),
            new Vector2(0.5714f, 0.5f),
            new Vector2(0.7142f, 0.5f),
            new Vector2(0.8571f, 0.5f),
        };
        for (int i = 0; i < 6; i++)
        {
            mHornTail[i] = UguiMaker.newImage("tail" + i.ToString(), mHornLink.transform, "shapelogic_sprite", "guanka5_station0", false);
            mHornTail[i].rectTransform.pivot = new Vector2(0.52f, 0.77f);
            mHornTail[i].rectTransform.anchorMin = tail_piovt[i];
            mHornTail[i].rectTransform.anchorMax = tail_piovt[i];
            mHornTail[i].rectTransform.anchoredPosition = mdata_tail_top;
            mHornTail[i].rectTransform.sizeDelta = new Vector2(29, 29);
        }


        Vector2[] area_pos_left = new Vector2[]
        {
            new Vector2(0, -19.5f),
            new Vector2(0, -83.1f),
            new Vector2(0, -146.7f),
            new Vector2(0, -209.5f),
        };
        Vector2[] area_pos_right = new Vector2[]
         {
            new Vector2(0, -19.5f),
            new Vector2(0, -83.1f),
            new Vector2(0, -146.7f),
            new Vector2(0, -209.5f),
         };

        for (int i = 0; i < 4; i++)
        {
            mColorAreaLeft[i] = UguiMaker.newImage("area" + i.ToString(), mHornLink.transform, "public", "white", true);
            mColorAreaLeft[i].rectTransform.pivot = new Vector2(1, 1);
            mColorAreaLeft[i].rectTransform.anchoredPosition = area_pos_left[i];
            mColorAreaLeft[i].rectTransform.sizeDelta = mdata_area_size_min;

            Image frame = UguiMaker.newImage("frame", mColorAreaLeft[i].transform, "shapelogic_sprite", "guanka5_stationbg", false);
            frame.type = Image.Type.Sliced;
            frame.rectTransform.pivot = new Vector2(0.5f, 1f);
            frame.rectTransform.anchorMin = new Vector2(0, 0);
            frame.rectTransform.anchorMax = new Vector2(1, 1);
            frame.rectTransform.offsetMin = new Vector2(-5, -5);
            frame.rectTransform.offsetMax = new Vector2(5, 5);

            mColorAreaLeft[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < 4; i++)
        {
            mColorAreaRight[i] = UguiMaker.newImage("area" + (4 + i).ToString(), mHornLink.transform, "public", "white", true);
            mColorAreaRight[i].rectTransform.pivot = new Vector2(0, 1);
            mColorAreaRight[i].rectTransform.anchoredPosition = area_pos_right[i];
            mColorAreaRight[i].rectTransform.sizeDelta = mdata_area_size_min;

            Image frame = UguiMaker.newImage("frame", mColorAreaRight[i].transform, "shapelogic_sprite", "guanka5_stationbg", false);
            frame.type = Image.Type.Sliced;
            frame.rectTransform.pivot = new Vector2(0.5f, 1f);
            frame.rectTransform.anchorMin = new Vector2(0, 0);
            frame.rectTransform.anchorMax = new Vector2(1, 1);
            frame.rectTransform.offsetMin = new Vector2(-5, -5);
            frame.rectTransform.offsetMax = new Vector2(5, 5);

            mColorAreaRight[i].gameObject.SetActive(false);

        }

        mBtn = UguiMaker.newButton("btn", transform, "public", "white");
        mBtn.image.color = new Color(1, 1, 1, 0);
        mBtn.onClick.AddListener(OnClkBtn);
        mBtn.image.rectTransform.sizeDelta = new Vector2(100, 100);

    }
    public void InitColor(List<int> colors)
    {
        mdata_colors = new List<int>();
        for (int i = 0; i < 8; i++)
        {
            mdata_colors.Add(colors[i]);
        }

        for(int i = 0; i < 4; i++)
        {
            mColorAreaLeft[i].color = 0 == colors[i] ? mdata_color0 : mdata_color1;
            mColorAreaRight[i].color = 0 == colors[i + 4] ? mdata_color0 : mdata_color1;
            mdata_colors[i] = -1;
            mdata_colors[i + 4] = -1;
        }
    }
    public void SetAnswerObj()
    {
        mdata_is_answer = true;
        mBtn.gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            mColorAreaLeft[i].color = Color.white;
            mColorAreaRight[i].color = Color.white;
        }

        mColorAreaLeft[0].gameObject.AddComponent<Button>().onClick.AddListener(OnClkBtnArea0);
        mColorAreaLeft[1].gameObject.AddComponent<Button>().onClick.AddListener(OnClkBtnArea1);
        mColorAreaLeft[2].gameObject.AddComponent<Button>().onClick.AddListener(OnClkBtnArea2);
        mColorAreaLeft[3].gameObject.AddComponent<Button>().onClick.AddListener(OnClkBtnArea3);
        mColorAreaRight[0].gameObject.AddComponent<Button>().onClick.AddListener(OnClkBtnArea4);
        mColorAreaRight[1].gameObject.AddComponent<Button>().onClick.AddListener(OnClkBtnArea5);
        mColorAreaRight[2].gameObject.AddComponent<Button>().onClick.AddListener(OnClkBtnArea6);
        mColorAreaRight[3].gameObject.AddComponent<Button>().onClick.AddListener(OnClkBtnArea7);


    }
    public void SetQuestionObj()
    {
        mBtn.gameObject.SetActive(true);
        mdata_is_answer = false;
        for(int i = 0; i < 8; i++)
        {
            if(i < 4)
            {
                mColorAreaLeft[i].GetComponent<Button>().enabled = false;
            }
            else
            {
                mColorAreaRight[i - 4].GetComponent<Button>().enabled = false;
            }
        }
    }
    public void SetResetPos(Vector3 reset_pos)
    {
        mResetPos = reset_pos;
    }
    public void SetOver()
    {
        mdata_is_over = true;
    }

    public void Show()
    {
        if (mdata_is_showing)
            return;
        mdata_is_showing = true;
        StartCoroutine(TShow());
        mBtn.image.rectTransform.sizeDelta = new Vector2(199.11f, 274.41f);
        mBtn.image.rectTransform.anchoredPosition = new Vector2(0, -130.4f);

        //Debug.LogError("Show");

    }
    public void Hide()
    {
        if (!mdata_is_showing)
            return;
        mdata_is_showing = false;
        mBtn.image.rectTransform.sizeDelta = new Vector2(100, 100);
        mBtn.image.rectTransform.anchoredPosition = Vector2.zero;

        //Debug.LogError("Hide");
        StartCoroutine("THide");

    }
    public void OnClkBtn()
    {
        if (mdata_is_showing)
            Hide();
        else
            Show();


    }

    bool first_time_show = true;
    IEnumerator TShow()
    {
        if(!first_time_show)
            ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("开关切换驾驶舱1");
        first_time_show = false;

        //horn
        mBtn.gameObject.SetActive(false);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mHorns[0].transform.localEulerAngles = Vector3.Lerp(mdata_horn_angle_min[0], Vector3.zero, i);
            mHorns[1].transform.localEulerAngles = Vector3.Lerp(mdata_horn_angle_min[1], Vector3.zero, i);
            yield return new WaitForSeconds(0.01f);
        }
        mHorns[0].transform.localEulerAngles = Vector3.zero;
        mHorns[1].transform.localEulerAngles = Vector3.zero;

        //link
        for (float i = 0; i < 1; i += 0.1f)
        {
            mHornLink.rectTransform.sizeDelta = Vector2.Lerp(mdata_link_size_min, mdata_link_size_max, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mHornLink.rectTransform.sizeDelta = mdata_link_size_max;


        //tail
        for (float i = 0; i < 1f; i += 0.2f)
        {
            Vector2 pos = Vector2.Lerp(mdata_tail_top, mdata_tail_down, i);
            for (int j = 0; j < mHornTail.Length; j++)
            {
                mHornTail[j].rectTransform.anchoredPosition = pos;
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int j = 0; j < mHornTail.Length; j++)
        {
            mHornTail[j].rectTransform.anchoredPosition = mdata_tail_down;
        }


        //area  
        float offset_y = 0;
        for (int i = 0; i < mColorAreaRight.Length; i++)
        {
            mColorAreaLeft[i].gameObject.SetActive(true);
            mColorAreaRight[i].gameObject.SetActive(true);
            for (float j = 0; j < 1f; j += 0.2f)
            {
                Vector2 size = Vector2.Lerp(mdata_area_size_min, mdata_area_size_max, Mathf.Sin(Mathf.PI * 0.5f * j));
                mColorAreaLeft[i].rectTransform.sizeDelta = size;
                mColorAreaRight[i].rectTransform.sizeDelta = size;
                
                mRtran.anchoredPosition3D = mResetPos + new Vector3( 0, 0.5f * (offset_y + mColorAreaLeft[i].rectTransform.sizeDelta.y), 0);
                yield return new WaitForSeconds(0.01f);
            }
            mColorAreaLeft[i].rectTransform.sizeDelta = mdata_area_size_max;
            mColorAreaRight[i].rectTransform.sizeDelta = mdata_area_size_max;
            offset_y += mdata_area_size_max.y;

        }

        if(!mdata_is_answer)
            mBtn.gameObject.SetActive(true);
    }
    IEnumerator THide()
    {
        ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("开关切换驾驶舱1");
        //horn
        mBtn.gameObject.SetActive(false);


        //area  
        float offset_y = mdata_area_size_max.y * 4;
        for (int i = mColorAreaRight.Length - 1; i >= 0; i--)
        {
            offset_y -= mdata_area_size_max.y;
            for (float j = 0; j < 1f; j += 0.2f)
            {
                Vector2 size = Vector2.Lerp(mdata_area_size_max, mdata_area_size_min, Mathf.Sin(Mathf.PI * 0.5f * j));
                mColorAreaLeft[i].rectTransform.sizeDelta = size;
                mColorAreaRight[i].rectTransform.sizeDelta = size;

                mRtran.anchoredPosition3D = mResetPos + new Vector3(0, 0.5f * (offset_y + mColorAreaLeft[i].rectTransform.sizeDelta.y), 0);
                yield return new WaitForSeconds(0.01f);
            }
            mColorAreaLeft[i].rectTransform.sizeDelta = mdata_area_size_min;
            mColorAreaRight[i].rectTransform.sizeDelta = mdata_area_size_min;

            mColorAreaLeft[i].gameObject.SetActive(false);
            mColorAreaRight[i].gameObject.SetActive(false);

        }


        //tail
        for (float i = 0; i < 1f; i += 0.2f)
        {
            Vector2 pos = Vector2.Lerp(mdata_tail_down, mdata_tail_top, i);
            for (int j = 0; j < mHornTail.Length; j++)
            {
                mHornTail[j].rectTransform.anchoredPosition = pos;
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int j = 0; j < mHornTail.Length; j++)
        {
            mHornTail[j].rectTransform.anchoredPosition = mdata_tail_top;
        }

        //link
        for (float i = 0; i < 1; i += 0.1f)
        {
            mHornLink.rectTransform.sizeDelta = Vector2.Lerp(mdata_link_size_max, mdata_link_size_min, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mHornLink.rectTransform.sizeDelta = mdata_link_size_min;
        

        for (float i = 0; i < 1f; i += 0.1f)
        {
            mHorns[0].transform.localEulerAngles = Vector3.Lerp( Vector3.zero, mdata_horn_angle_min[0], i);
            mHorns[1].transform.localEulerAngles = Vector3.Lerp( Vector3.zero, mdata_horn_angle_min[1], i);
            yield return new WaitForSeconds(0.01f);
        }
        mHorns[0].transform.localEulerAngles = mdata_horn_angle_min[0];
        mHorns[1].transform.localEulerAngles = mdata_horn_angle_min[1];



        if (mdata_is_over)
        {
            StopCoroutine("TShake");
            StartCoroutine("TOver");
        }
        else
        {
            mBtn.gameObject.SetActive(true);
        }

    }
    IEnumerator TOver()
    {
        Vector3 angle0 = mRtran.localEulerAngles;
        Vector3 angle1 = angle0;
        angle1.z -= 500;
        for (float i = 0; i < 1f; i+= 0.08f) 
        {
            mHornLink.rectTransform.localEulerAngles = Vector3.Lerp(angle0, angle1, Mathf.Sin(Mathf.PI * 0.5f * i - Mathf.PI * 0.5f) + 1);
            mHornLink.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
            yield return new WaitForSeconds(0.01f);
        }
        mHornLink.rectTransform.localScale = Vector3.zero;


        ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("星星闪烁2");
        ParticleSystem effect = ResManager.GetPrefab("effect_star0", "effect_star2").GetComponent<ParticleSystem>();
        UguiMaker.InitGameObj(effect.gameObject, transform, "effect", Vector3.zero, Vector3.one);
        effect.Emit(1);
        yield return new WaitForSeconds(1f);
        ShapeLogicCtl.instance.mGuankaCtl5.callback_StationOver();

    }

    public void Shake()
    {
        StartCoroutine("TShake");
    }
    IEnumerator TShake()
    {
        float p = 0;
        while (true)
        {
            mHornLink.rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(p) * 3);
            p += 0.2f;
            yield return new WaitForSeconds(0.01f);
        }


    }

    public void PlayError()
    {
        StartCoroutine("TPlayError");
    }
    IEnumerator TPlayError()
    {
        for(float i = 0; i < 1f; i += 0.08f)
        {
            mRtran.localEulerAngles = new Vector3(0, 0, 3 * Mathf.Sin(Mathf.PI * 6 * i));
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.localEulerAngles = Vector3.zero;

    }

    public void OnClkBtnArea0()
    {
        OnClkBtnArea(0);
    }
    public void OnClkBtnArea1()
    {
        OnClkBtnArea(1);
    }
    public void OnClkBtnArea2()
    {
        OnClkBtnArea(2);
    }
    public void OnClkBtnArea3()
    {
        OnClkBtnArea(3);
    }
    public void OnClkBtnArea4()
    {
        OnClkBtnArea(4);
    }
    public void OnClkBtnArea5()
    {
        OnClkBtnArea(5);
    }
    public void OnClkBtnArea6()
    {
        OnClkBtnArea(6);
    }
    public void OnClkBtnArea7()
    {
        OnClkBtnArea(7);
    }
    public void OnClkBtnArea(int index)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("按钮2");
        if(index < 4)
        {
            mColorAreaLeft[index].color = 0 == ShapeLogicCtl.instance.mGuankaCtl5.mdata_select_color ? mdata_color0 : mdata_color1;
            mdata_colors[index] = ShapeLogicCtl.instance.mGuankaCtl5.mdata_select_color;
        }
        else
        {
            mColorAreaRight[index - 4].color = 0 == ShapeLogicCtl.instance.mGuankaCtl5.mdata_select_color ? mdata_color0 : mdata_color1;
            mdata_colors[index] = ShapeLogicCtl.instance.mGuankaCtl5.mdata_select_color;
        }
    }

}
