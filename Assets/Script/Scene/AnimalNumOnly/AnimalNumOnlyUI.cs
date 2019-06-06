using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AnimalNumOnlyUI : MonoBehaviour
{
    Image mBg { get; set; }
    Image mYun0, mYun1, mYun2;
    Image mControlShow { get; set; }
    Button mControl { get; set; }
    Button mReplay { get; set; }
    Button mTip { get; set; }
    Image[] mTime = new Image[5];
    //Text mTips { get; set; }
    Image mTipsNum { get; set; }
    public AnimalNumOnlySelectPlane mSelectPlane { get; set; }
    
    RectTransform mUIRoot { get; set; }
    int m_tip_count = 0;
    public bool m_can_click_replay = false;

    public void Init ()
    {
        if (null != mBg)
            return;

        mBg = UguiMaker.newImage("bg", transform, "public", "white", false);
        mBg.rectTransform.sizeDelta = new Vector2(1423, 800);
        mBg.color = new Color32(58, 112, 209, 255);

        mYun1 = UguiMaker.newImage("yun1", transform, "animalnumonly_sprite", "yun1", false);
        mYun0 = UguiMaker.newImage("yun0", transform, "animalnumonly_sprite", "yun0", false);
        mYun2 = UguiMaker.newImage("yun2", transform, "animalnumonly_sprite", "yun2", false);
        mYun0.rectTransform.pivot = new Vector2(0.5f, 0);
        mYun1.rectTransform.pivot = new Vector2(0.5f, 0);
        mYun2.rectTransform.pivot = new Vector2(0.5f, 1);
        mYun0.rectTransform.anchoredPosition = new Vector2(0, -400);
        mYun1.rectTransform.anchoredPosition = new Vector2(0, -400);
        mYun2.rectTransform.anchoredPosition = new Vector2(0, 400);

        mSelectPlane = UguiMaker.newGameObject("mSelectPlane", transform).AddComponent<AnimalNumOnlySelectPlane>();

        mUIRoot = UguiMaker.newGameObject("mUIRoot", transform).GetComponent<RectTransform>();
        mUIRoot.sizeDelta = Vector2.zero;
        mUIRoot.anchoredPosition3D = new Vector3(336, 1100, 0);

        Image line0 = UguiMaker.newImage("line0", mUIRoot, "animalnumonly_sprite", "ui_line");
        Image line1 = UguiMaker.newImage("line1", mUIRoot, "animalnumonly_sprite", "ui_line");
        line0.rectTransform.pivot = new Vector2(0.5f, 1);
        line1.rectTransform.pivot = new Vector2(0.5f, 1);
        line0.rectTransform.anchoredPosition3D = new Vector3(30, 0, 0);
        line1.rectTransform.anchoredPosition3D = new Vector3(-30, 0, 0);

        Image time0 = UguiMaker.newImage("time0", mUIRoot, "animalnumonly_sprite", "ui_bg0", false);
        Image time01 = UguiMaker.newImage("img", time0.transform, "animalnumonly_sprite", "ui_time", false);
        time0.rectTransform.anchoredPosition3D = new Vector3(0, -156.65f);

        Image time1 = UguiMaker.newImage("time1", mUIRoot, "animalnumonly_sprite", "ui_bg3", false);
        //mTime = UguiMaker.newText("mTime", time1.transform);
        time1.rectTransform.anchoredPosition3D = new Vector3(0, -259.08f, 0);
        //mTime.rectTransform.sizeDelta = new Vector2(258.5f, 109.1f);
        //mTime.fontSize = 50;
        //mTime.alignment = TextAnchor.MiddleCenter;
        //mTime.color = new Color32(33, 241, 82, 255);
        //mTime.raycastTarget = false;

        mTime[0] = UguiMaker.newImage("mTime0", time1.transform, "public", "default0", false);
        mTime[1] = UguiMaker.newImage("mTime1", time1.transform, "public", "default0", false);
        mTime[2] = UguiMaker.newImage("mTime2", time1.transform, "public", "default_maohao", false);
        mTime[3] = UguiMaker.newImage("mTime3", time1.transform, "public", "default0", false);
        mTime[4] = UguiMaker.newImage("mTime4", time1.transform, "public", "default0", false);
        for (int i = 0; i < 5; i++)
        {
            mTime[i].color = new Color32(33, 241, 82, 255);

        }
        mTime[0].rectTransform.anchoredPosition = new Vector2(-60, 0);
        mTime[1].rectTransform.anchoredPosition = new Vector2(-30, 0);
        mTime[3].rectTransform.anchoredPosition = new Vector2(30, 0);
        mTime[4].rectTransform.anchoredPosition = new Vector2(60, 0);
        mTime[0].rectTransform.sizeDelta = new Vector2(30, 40);
        mTime[1].rectTransform.sizeDelta = new Vector2(30, 40);
        mTime[2].rectTransform.sizeDelta = new Vector2(17, 40);
        mTime[3].rectTransform.sizeDelta = new Vector2(30, 40);
        mTime[4].rectTransform.sizeDelta = new Vector2(30, 40);



        mReplay = UguiMaker.newButton("replay", mUIRoot, "animalnumonly_sprite", "ui_bg1");
        Image replay1 = UguiMaker.newImage("img", mReplay.transform, "animalnumonly_sprite", "ui_replay", false);
        mReplay.image.rectTransform.anchoredPosition = new Vector2(0, -362.86f);
        mReplay.onClick.AddListener(OnClkReplay);
        mReplay.transition = Selectable.Transition.None;


        
        mTip = UguiMaker.newButton("tip1", mUIRoot, "animalnumonly_sprite", "ui_bg4");
        //mTips = UguiMaker.newText("mTime", mTip.transform);
        mTip.image.rectTransform.anchoredPosition = new Vector2(0, -465.7f); //new Vector2(0, -569.1f);
        //mTips.rectTransform.sizeDelta = new Vector2(258.5f, 109.1f);
        //mTips.rectTransform.anchoredPosition = new Vector2(53.1f, 0);
        //mTips.fontSize = 42;
        //mTips.alignment = TextAnchor.MiddleCenter;
        //mTips.color = Color.white;
        //mTips.raycastTarget = false;
        mTip.onClick.AddListener(OnClkTip);
        //Shadow shadow = mTips.gameObject.AddComponent<Shadow>();
        //shadow.effectColor = new Color32(153, 34, 17, 255);
        //shadow.effectDistance = new Vector2(1.61f, -1.31f);

        mTipsNum = UguiMaker.newImage("mTipsNum", mTip.transform, "public", "default0", false);
        mTipsNum.rectTransform.sizeDelta = new Vector2(25, 35);
        mTipsNum.rectTransform.anchoredPosition = new Vector2(53.1f, 1);

        Image tip01 = UguiMaker.newImage("img", mTip.transform, "animalnumonly_sprite", "ui_tip", false);
        tip01.rectTransform.anchoredPosition3D = new Vector3(-17.9f, 0);


        StartCoroutine("TMoveIn");
        StartCoroutine("TBg");
    }

    public void SetTip(int tip_num)
    {
        //mTips.text = tip_num.ToString();
        mTipsNum.sprite = ResManager.GetSprite("public", "default" + tip_num);


    }
    //public void SetTime(string time_str)
    //{
    //    mTime.text = time_str;
    //}
    public void SetYun0Last()
    {
        if (null == mControl)
        {
            Canvas canvas = mYun0.gameObject.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 5;
            mYun0.gameObject.layer = LayerMask.NameToLayer("UI");
            mYun0.transform.SetAsLastSibling();
            mYun0.gameObject.AddComponent<GraphicRaycaster>();

            mControl = UguiMaker.newButton("contol", mYun0.transform, "animalnumonly_sprite", "place_frame");
            mControl.onClick.AddListener(OnClkControl);
            mControl.transition = Selectable.Transition.None;
            mControl.image.rectTransform.sizeDelta = new Vector2(80, 80);
            mControl.image.rectTransform.anchoredPosition = new Vector2(Common.gWidth * 0.5f - 40, -44.61f);
            mControl.gameObject.layer = LayerMask.NameToLayer("UI");
            mControl.image.color = new Color(1, 1, 1, 0);
            mControl.gameObject.SetActive(false);

            mControlShow = UguiMaker.newImage("image", mControl.transform, "animalnumonly_sprite", "control1", false);

            mSelectPlane.mControlType = 1;

        }

    }
    public void Reset()
    {
        m_tip_count = AnimalNumOnlyCtl.instance.data_tip_num;
        //mTips.text = m_tip_count.ToString();
        mTipsNum.sprite = ResManager.GetSprite("public", "default" + m_tip_count);
        TimeBegin();
    }
    public void EnableUI(bool _enable)
    {
        mReplay.enabled = _enable;
        mTip.enabled = _enable;
        Debug.LogError("ui enable = " + _enable);
    }

    public void OnClkReplay()
    {
        SoundManager.instance.PlayShort("按钮0");
        //AnimalNumOnlyCtl.instance.mSound.PlayTipDefaultAb("重玩");
        Global.instance.PlayBtnClickAnimation(mReplay.transform);

        if (AnimalNumOnlyCtl.instance.mPlace.mLock)
            return;

        if (!m_can_click_replay)
            return;
        m_can_click_replay = false;
        AnimalNumOnlyCtl.instance.ReplayGuanka();
    }
    public void OnClkTip()
    {
        SoundManager.instance.PlayShort("按钮0");
        //AnimalNumOnlyCtl.instance.mSound.PlayTipDefaultAb("提示");
        Global.instance.PlayBtnClickAnimation(mTip.transform);

        if (AnimalNumOnlyCtl.instance.mPlace.mLock)
            return;

        if (m_tip_count <= 0)
            return;

        m_tip_count--;
        //mTips.text = m_tip_count.ToString();
        mTipsNum.sprite = ResManager.GetSprite("public", "default" + m_tip_count);
        AnimalNumOnlyCtl.instance.mPlace.Tip();

    }
    public void OnClkControl()
    {
        SoundManager.instance.PlayShort("按钮2");
        Global.instance.PlayBtnClickAnimation(mControl.transform);
        AnimalNumOnlyCell.gSelect = null;
        AnimalNumOnlyBtn.gSelect = null;
        if (mSelectPlane.mControlType == 0)
        {
            mSelectPlane.mControlType = 1;
            mControlShow.sprite = ResManager.GetSprite("animalnumonly_sprite", "control1");
            mSelectPlane.SetType(1);

        }
        else
        {
            mSelectPlane.Hide1();
            mSelectPlane.mControlType = 0;
            mControlShow.sprite = ResManager.GetSprite("animalnumonly_sprite", "control0");
            mSelectPlane.SetType(0);

        }

    }


    public void MoveIn()
    {
        StartCoroutine("TMoveIn");
    }
    IEnumerator TMoveIn()
    {
        for(float i = 0; i < 1f; i += 0.03f)
        {
            mUIRoot.anchoredPosition = Vector2.Lerp(new Vector2(336, 1100), new Vector2(336, 400), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mUIRoot.anchoredPosition = new Vector2(336, 400);

    }

    public void MoveOut()
    {
        StartCoroutine("TMoveOut");
    }
    IEnumerator TMoveOut()
    {
        for (float i = 0; i < 1f; i += 0.03f)
        {
            mUIRoot.anchoredPosition = Vector2.Lerp(new Vector2(336, 400), new Vector2(336, 1100), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mUIRoot.anchoredPosition = new Vector2(336, 1100);

    }
    
    public int time { get; set; }
    public int fen { get; set; }
    public int miao { get; set; }
    public void TimeBegin()
    {
        time = 0;
        fen = 0;
        miao = 0;
        StopCoroutine("TTime");
        StartCoroutine("TTime");
    }
    public void TimeStop()
    {
        StopCoroutine("TTime");
    }
    IEnumerator TTime()
    {
        Sprite[] numbers = new Sprite[11];
        for(int i = 0; i < 10; i++)
        {
            numbers[i] = ResManager.GetSprite("public", "default" + i.ToString());
        }
        numbers[10] = ResManager.GetSprite("public", "default_maohao");


        while (true)
        {
            fen = time / 60;
            miao = time % 60;

            if (10 > fen && 10 > miao)
            {
                //mTime.text = "0" + fen.ToString() + ":0" + miao.ToString();
                mTime[0].sprite = numbers[0];
                mTime[1].sprite = numbers[fen];
                mTime[3].sprite = numbers[0];
                mTime[4].sprite = numbers[miao];
            }
            else if(10 > fen && 10 <= miao)
            {
                //mTime.text = "0" + fen.ToString() + ":" + miao.ToString();
                mTime[0].sprite = numbers[0];
                mTime[1].sprite = numbers[fen];
                mTime[3].sprite = numbers[(int)(miao / 10)];
                mTime[4].sprite = numbers[miao % 10];

            }
            else if(10 <= fen && 10 > miao)
            {
                //mTime.text = fen.ToString() + ":0" + miao.ToString();
                mTime[0].sprite = numbers[(int)(fen / 10)];
                mTime[1].sprite = numbers[fen % 10];
                mTime[3].sprite = numbers[0];
                mTime[4].sprite = numbers[miao % 10];
            }
            else if(10 <= fen && 10 <= miao)
            {
                //mTime.text = fen.ToString() + ":" + miao.ToString();
                mTime[0].sprite = numbers[(int)(fen / 10)];
                mTime[1].sprite = numbers[fen % 10];
                mTime[3].sprite = numbers[(int)(miao / 10)];
                mTime[4].sprite = numbers[miao % 10];

            }
            yield return new WaitForSeconds(1f);
            time++;

        }
    }

    IEnumerator TBg()
    {
        float p1 = 0;
        float p2 = 0;
        float p3 = 0;
        while (true)
        {
            mYun0.rectTransform.anchoredPosition = new Vector2(0, Mathf.Sin(p1) * 5 - 5 -425);
            mYun1.rectTransform.anchoredPosition = new Vector2(0, Mathf.Sin(p2) * 5 - 5 - 400);
            mYun2.rectTransform.anchoredPosition = new Vector2(0, Mathf.Sin(p2) * 5 + 5 + 400);
            p1 += 0.08f;
            p2 += 0.04f;
            p3 += 0.02f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    
}
