using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MonkeySortOutCar : MonoBehaviour {
    public static MonkeySortOutCar gSelect = null;
    [HideInInspector]
    public Vector2 m_pos_up_left_in = new Vector2( GlobalParam.screen_width * -0.25f, 112);
    [HideInInspector]
    public Vector2 m_pos_up_right_in = new Vector2(GlobalParam.screen_width * 0.25f, 112);
    [HideInInspector]
    public Vector2 m_pos_up_left_out = new Vector2(-950, 112);
    [HideInInspector]
    public Vector2 m_pos_up_right_out = new Vector2(950, 112);
    [HideInInspector]
    public Vector2 m_pos_down_left = new Vector2(-950, -268);
    [HideInInspector]
    public Vector2 m_pos_down_right = new Vector2(950, -268);
    [HideInInspector]

    Image mback, mfront, mlun0, mlun1, mlun2, mlun3;
    public RectTransform mrtran { get; set; }
    RectTransform mSit { get; set; }
    BoxCollider mBox { get; set; }
    GridLayoutGroup m_layout_sit { get; set; }


    List<MonkeySortOutMonkey> mMkys = new List<MonkeySortOutMonkey>();

	void Start ()
    {
        
        mrtran = GetComponent<RectTransform>();
        mrtran.anchoredPosition = m_pos_down_left;

        mback = UguiMaker.newImage("mback", transform, "monkeysortout_sprite", "car0");
        mSit = UguiMaker.newGameObject("sit", transform).GetComponent<RectTransform>();
        mfront = UguiMaker.newImage("mfront", transform, "monkeysortout_sprite", "car1");
        mlun0 = UguiMaker.newImage("mlun0", transform, "monkeysortout_sprite", "car2");
        mlun1 = UguiMaker.newImage("mlun1", transform, "monkeysortout_sprite", "car3");
        mlun2 = UguiMaker.newImage("mlun2", transform, "monkeysortout_sprite", "car2");
        mlun3 = UguiMaker.newImage("mlun3", transform, "monkeysortout_sprite", "car3");
        mlun0.rectTransform.localPosition = new Vector3(-137, -80, 0);
        mlun1.rectTransform.localPosition = new Vector3(-137, -85, 0);
        mlun2.rectTransform.localPosition = new Vector3(146, -80, 0);
        mlun3.rectTransform.localPosition = new Vector3(146, -85, 0);

        mSit.transform.localScale = new Vector3(0.45f, 0.45f, 0);
        m_layout_sit = mSit.gameObject.AddComponent<GridLayoutGroup>();
        m_layout_sit.padding.top = -26;
        m_layout_sit.spacing = new Vector2(51.36f, 63.7f);
        m_layout_sit.startCorner = GridLayoutGroup.Corner.UpperLeft;
        m_layout_sit.startAxis = GridLayoutGroup.Axis.Horizontal;
        m_layout_sit.childAlignment = TextAnchor.MiddleCenter;
        m_layout_sit.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        m_layout_sit.constraintCount = 5;

        mBox = gameObject.AddComponent<BoxCollider>();
        mBox.size = new Vector3(431.5f, 200.7f, 1);
        mBox.center = new Vector3(0, 44, 0);
        mBox.enabled = false;

        MonkeySortOutCtl.instance.CallbackStart();
        ScreenDebug.Log("finish=" + gameObject.name);
        
        //StartCoroutine(TStart());
    }
    //IEnumerator TStart()
    //{
    //    mrtran = GetComponent<RectTransform>();
    //    mrtran.anchoredPosition = m_pos_down_left;

    //    mback = UguiMaker.newImage("mback", transform, "monkeysortout_sprite", "car0");
    //    mSit = UguiMaker.newGameObject("sit", transform).GetComponent<RectTransform>();
    //    mfront = UguiMaker.newImage("mfront", transform, "monkeysortout_sprite", "car1");
    //    mlun0 = UguiMaker.newImage("mlun0", transform, "monkeysortout_sprite", "car2");
    //    mlun1 = UguiMaker.newImage("mlun1", transform, "monkeysortout_sprite", "car3");
    //    mlun2 = UguiMaker.newImage("mlun2", transform, "monkeysortout_sprite", "car2");
    //    mlun3 = UguiMaker.newImage("mlun3", transform, "monkeysortout_sprite", "car3");
    //    mlun0.rectTransform.localPosition = new Vector3(-137, -80, 0);
    //    mlun1.rectTransform.localPosition = new Vector3(-137, -85, 0);
    //    mlun2.rectTransform.localPosition = new Vector3(146, -80, 0);
    //    mlun3.rectTransform.localPosition = new Vector3(146, -85, 0);
    //    yield return 1;

    //    mSit.transform.localScale = new Vector3(0.45f, 0.45f, 0);
    //    m_layout_sit = mSit.gameObject.AddComponent<GridLayoutGroup>();
    //    m_layout_sit.padding.top = -26;
    //    m_layout_sit.spacing = new Vector2(51.36f, 63.7f);
    //    m_layout_sit.startCorner = GridLayoutGroup.Corner.UpperLeft;
    //    m_layout_sit.startAxis = GridLayoutGroup.Axis.Horizontal;
    //    m_layout_sit.childAlignment = TextAnchor.MiddleCenter;
    //    m_layout_sit.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
    //    m_layout_sit.constraintCount = 5;
    //    ScreenDebug.Log("finish car 9=" + gameObject.name);
    //    yield return 1;

    //    mBox = gameObject.AddComponent<BoxCollider>();
    //    mBox.size = new Vector3(431.5f, 200.7f, 1);
    //    mBox.center = new Vector3(0, 44, 0);
    //    mBox.enabled = false;
    //    ScreenDebug.Log("finish car 10=" + gameObject.name);
    //    yield return 1;

    //    MonkeySortOutCtl.instance.CallbackStart();

    //    ScreenDebug.Log("finish=" + gameObject.name);

    //}

    public void SetRTran(Transform _parent, int sibling_index)
    {
        transform.SetParent(_parent);
        transform.SetSiblingIndex(sibling_index);
    }

    //加入猴子
    public void PushInMky(MonkeySortOutMonkey mky, bool play_shake = true)
    {
        if(!mMkys.Contains(mky))
        {
            mMkys.Add(mky);
        }
        mky.SetRTran(mSit, transform.childCount);
        mky.transform.localScale = Vector3.one;
        mky.mShadow.enabled = true;
        m_layout_sit.enabled = true;
        if(play_shake)
        {
            Shake();
        }
    }
    public void PopMky(MonkeySortOutMonkey mky)
    {
        if(mMkys.Contains(mky))
        {
            mMkys.Remove(mky);
            MonkeySortOutCtl.instance.mSoundMgr.PlayOnly("monkeysortout_sound", "monkey_out_car", 0.4f);
        }
    }

    public List<int> GetMkyType()
    {
        List<int> result = new List<int>();

        int[] t = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0};
        for(int i = 0; i < mMkys.Count; i++)
        {
            t[0] += mMkys[i].m_info_size;
            t[1] += mMkys[i].m_info_color;
            t[2] += mMkys[i].m_info_hat;
            t[3] += mMkys[i].m_info_face_side;
            t[4] += mMkys[i].m_info_tail;
            t[5] += mMkys[i].m_info_clothes;
            t[6] += mMkys[i].m_info_stand;
            t[7] += mMkys[i].m_info_food;
            t[8] += mMkys[i].m_info_mouth;
        }

        for(int i = 0; i < t.Length; i++)
        {
            if (t[i] == 0 || t[i] == mMkys.Count)
                result.Add(i);
        }

        return result;
    }
    public int GetMkyNum()
    {
        return mMkys.Count;
    }
    public MonkeySortOutMonkey GetMkyByIndex(int index)
    {
        if (null != mMkys && index < mMkys.Count)
            return mMkys[index];
        return null;
    }

    //从底部走过
    public void Run_down_left_to_right()
    {
        MonkeySortOutCtl.instance.mSoundMgr.PlaySoundList("monkeysortout_sound", new List<string>() { "car_run", "car_run" });
        StartCoroutine("TRun_down_left_to_right");
    }
    IEnumerator TRun_down_left_to_right()
    {
        SetRTran(MonkeySortOutCtl.instance.mfont, 0);
        mBox.enabled = false;

        for (int i = 0; i < mMkys.Count; i++)
        {
            mMkys[i].PlaySpine("Idle", true);
        }
        
        int index = 0;

        for(float i = 0; i < 1f; i += 0.009f)
        {
            mrtran.anchoredPosition = Vector2.Lerp(m_pos_down_left, m_pos_down_right, i);
            mlun1.rectTransform.localEulerAngles -= new Vector3(0, 0, 8);
            mlun3.rectTransform.localEulerAngles -= new Vector3(0, 0, 8);

            if(mMkys.Count > index)
            {
                if(mrtran.anchoredPosition.x > MonkeySortOutCtl.instance.mGuanka.m_mky_pos[index].x + 200)
                {
                    m_layout_sit.enabled = false;
                    mMkys[index].ThrowOutCar(MonkeySortOutCtl.instance.mGuanka.m_mky_pos[index]);
                    index++;
                }
            }


            yield return new WaitForSeconds(0.01f);
        }

        for(; index < mMkys.Count; index++)
        {
            mMkys[index].ThrowOutCar(MonkeySortOutCtl.instance.mGuanka.m_mky_pos[index]);
        }

        mrtran.anchoredPosition = m_pos_down_right;
        
        MonkeySortOutCtl.instance.Callback_Run_down_left_to_right();


    }

    //车在进出
    public void Run_up_in(bool is_left)
    {
        mMkys.Clear();
        SetRTran(MonkeySortOutCtl.instance.mback, 0);
        if (is_left)
            StartCoroutine(TRun_up_in(m_pos_up_left_out, m_pos_up_left_in));
        else
            StartCoroutine(TRun_up_in(m_pos_up_right_out, m_pos_up_right_in));
    }
    public void Run_up_out(bool is_left)
    {
        mMkys.Clear();
        if (is_left)
            StartCoroutine(TRun_up_out(m_pos_up_left_in, m_pos_up_left_out));
        else
            StartCoroutine(TRun_up_out(m_pos_up_right_in, m_pos_up_right_out));
    }
    public void Run_down_out(bool is_left)
    {
        mMkys.Clear();
        if (is_left)
            StartCoroutine(TRun_down_out(m_pos_down_left));
        else
            StartCoroutine(TRun_down_out(m_pos_down_right));
    }
    IEnumerator TRun_up_in(Vector2 begin, Vector2 end)
    {
        for (float i = 0; i < 1f; i += 0.02f)
        {
            mrtran.anchoredPosition = Vector2.Lerp(begin, end, Mathf.Sin(Mathf.PI * 0.5f * i));
            if(begin.x < end.x)
            {
                mlun1.rectTransform.localEulerAngles -= new Vector3(0, 0, 8);
                mlun3.rectTransform.localEulerAngles -= new Vector3(0, 0, 8);
            }
            else
            {
                mlun1.rectTransform.localEulerAngles += new Vector3(0, 0, 8);
                mlun3.rectTransform.localEulerAngles += new Vector3(0, 0, 8);
            }

            yield return new WaitForSeconds(0.01f);
        }
        mrtran.anchoredPosition = end;
        mBox.enabled = true;

    }
    IEnumerator TRun_up_out(Vector2 begin, Vector2 end)
    {
        mBox.enabled = false;
        for (float i = 0; i < 1f; i += 0.013f)
        {
            mrtran.anchoredPosition = Vector2.Lerp(begin, end, Mathf.Sin(Mathf.PI * 0.5f * i - Mathf.PI * 0.5f) + 1);
            if (begin.x < end.x)
            {
                mlun1.rectTransform.localEulerAngles += new Vector3(0, 0, 8);
                mlun3.rectTransform.localEulerAngles += new Vector3(0, 0, 8);
            }
            else
            {
                mlun1.rectTransform.localEulerAngles -= new Vector3(0, 0, 8);
                mlun3.rectTransform.localEulerAngles -= new Vector3(0, 0, 8);
            }

            yield return new WaitForSeconds(0.01f);
        }
        mrtran.anchoredPosition = end;
        MonkeySortOutCtl.instance.CallbackEnd();
    }
    IEnumerator TRun_down_out(Vector2 end)
    {
        mBox.enabled = false;
        Vector3 begin = mrtran.anchoredPosition;
        for (float i = 0; i < 1f; i += 0.013f)
        {
            mrtran.anchoredPosition = Vector2.Lerp(begin, end, Mathf.Sin(Mathf.PI * 0.5f * i - Mathf.PI * 0.5f) + 1);
            if (begin.x < end.x)
            {
                mlun1.rectTransform.localEulerAngles += new Vector3(0, 0, 8);
                mlun3.rectTransform.localEulerAngles += new Vector3(0, 0, 8);
            }
            else
            {
                mlun1.rectTransform.localEulerAngles -= new Vector3(0, 0, 8);
                mlun3.rectTransform.localEulerAngles -= new Vector3(0, 0, 8);
            }

            yield return new WaitForSeconds(0.01f);
        }
        mrtran.anchoredPosition = end;
        MonkeySortOutCtl.instance.CallbackEnd();
    }

    //回答特效
    float shake_param = 1;
    public void Shake(float _shake_param = 1)
    {
        shake_param = _shake_param;
        StopCoroutine("TShake");
        StartCoroutine("TShake");

        //MonkeySortOutCtl.instance.Test();
    }
    IEnumerator TShake()
    {
        Vector3 pos = Vector3.zero;
        float lunzi_param = 1;
        if (mrtran.anchoredPosition.x > 0)
        {
            pos = m_pos_up_right_in;
            for (float i = 0; i < 1f; i += 0.05f)
            {
                mrtran.anchoredPosition = pos + new Vector3(Mathf.Sin(Mathf.PI * 2 * i) * 2 * (1 - i) * shake_param, 0, 0);
                mlun1.rectTransform.localEulerAngles += new Vector3(0, 0, Mathf.Sin(Mathf.PI * 2 * i) * lunzi_param * (1 - i) * shake_param);
                mlun3.rectTransform.localEulerAngles += new Vector3(0, 0, Mathf.Sin(Mathf.PI * 2 * i) * lunzi_param * (1 - i) * shake_param);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            pos = m_pos_up_left_in;
            for (float i = 0; i < 1f; i += 0.05f)
            {
                mrtran.anchoredPosition = pos - new Vector3(Mathf.Sin(Mathf.PI * 2 * i) * 3 * (1 - i) * shake_param, 0, 0);
                mlun1.rectTransform.localEulerAngles -= new Vector3(0, 0, Mathf.Sin(Mathf.PI * 2 * i) * lunzi_param * (1 - i) * shake_param);
                mlun3.rectTransform.localEulerAngles -= new Vector3(0, 0, Mathf.Sin(Mathf.PI * 2 * i) * lunzi_param * (1 - i) * shake_param);
                yield return new WaitForSeconds(0.01f);
            }
        }
        mrtran.anchoredPosition = pos;

    }

    //回答错误
    public void Error()
    {
        StopCoroutine("TError");
        StartCoroutine("TError");
    }
    IEnumerator TError()
    {
        Vector3 pos = Vector3.zero;
        if(mrtran.anchoredPosition.x > 0)
        {
            pos = m_pos_up_right_in;
            for (float i = 0; i < 1f; i += 0.03f)
            {
                mrtran.anchoredPosition = pos + new Vector3(Mathf.Sin(Mathf.PI * 2 * i) * 50  * (1 - i), 0, 0);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            pos = m_pos_up_left_in;
            for (float i = 0; i < 1f; i += 0.03f)
            {
                mrtran.anchoredPosition = pos - new Vector3(Mathf.Sin(Mathf.PI * 2 * i) * 50 * (1 - i), 0, 0);
                yield return new WaitForSeconds(0.01f);
            }
        }
        mrtran.anchoredPosition = pos;

    }

    //放大缩小
    public void BigBigBig()
    {
        StopCoroutine("TBigBigBig");
        StartCoroutine("TBigBigBig");
    }
    IEnumerator TBigBigBig()
    {
        Vector3 scale1 = transform.localScale;

        for(float i = 0; i < 1f; i += 0.03f)
        {
            float s = Mathf.Abs(Mathf.Sin(Mathf.PI * 2 * i)) * 0.3f;
            transform.localScale = scale1 + new Vector3(s, s, 1);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = scale1;
    }

    Text mText = null;
    public void ShowText(string text)
    {
        if(null == mText)
        {
            mText = UguiMaker.newText("text", transform);
            mText.rectTransform.anchoredPosition3D = new Vector3(0, 158.3f, 0);
            mText.rectTransform.sizeDelta = new Vector2(450, 62);
            mText.fontSize = 49;
            mText.alignment = TextAnchor.MiddleCenter;
            mText.color = Color.white;
        }
        mText.gameObject.SetActive(true);
        mText.text = text;
        StartCoroutine("TShowText");

    }
    public void HideText()
    {
        if(null != mText)
        {
            mText.gameObject.SetActive(false);
        }
    }
    IEnumerator TShowText()
    {
        for (float i = 0; i < 1f; i += 0.03f)
        {
            float s = Mathf.Abs(Mathf.Sin(Mathf.PI * 2 * i)) * 0.3f;
            transform.localScale = Vector3.one + new Vector3(s, s, 1);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = Vector3.one;
    }

}
