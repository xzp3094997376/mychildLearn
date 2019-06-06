using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka10 : MonoBehaviour
{
    Button mOK;
    Image mBg0;
    Image[] mImageSelect;
    RectTransform[] mRtranSelect; 
    ParticleSystem mEffectOK;

    List<ShapeLogicGuanka10_Station> mStation;

    int mdata_answer_index;
    int mdata_window = 0;
    int mdata_yan = 0;
    int mdata_star = 0;
    int mdata_lun = 0;
    int mdata_bg = 0;
    int temp_updata_state = 0;



    void Update()
    {
        switch (temp_updata_state)
        {
            case 1:
                UpdateOver();
                break;

        }

    }
    void UpdateOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach (RaycastHit h in hits)
            {
                ShapeLogicGuanka10_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka10_Station>();
                if (null != com)
                {
                    com.SetBoxEnable(false);
                    com.PlayShoot();
                    bool is_over = true;
                    for (int i = 0; i < mStation.Count; i++)
                    {
                        if (!mStation[i].mdata_is_over)
                        {
                            is_over = false;
                            break;
                        }
                    }
                    if (is_over)
                    {
                        Invoke("EndGame", 1f);
                    }
                    break;
                }
            }

        }

    }

    public void BeginGame()
    {
        gameObject.SetActive(true);
        StartCoroutine(TBeginGame());
    }
    public void EndGame()
    {
        StartCoroutine(TEndGame());
    }
    IEnumerator TBeginGame()
    {
        temp_updata_state = 0;
        mdata_window = 0;
        mdata_yan = 0;
        mdata_star = 0;
        mdata_lun = 0;
        mdata_bg = 0;

        //背景
        mBg0 = UguiMaker.newImage("mBg0", transform, "public", "white", false);
        mBg0.color = new Color32(255, 160, 207, 255);
        mBg0.rectTransform.sizeDelta = new Vector2(1423, 800);
        //ShapeLogicCtl.instance.CreateLine(674.4f, mBg0.transform, new List<Vector2>() { new Vector2(235.7f, 91.3f), new Vector2(235.7f, -158.1f) });

        //车
        ShapeLogicCtl.instance.mSound.PlaySoundList(new List<string>() { "shapelogic_sound", "shapelogic_sound" }, new List<string>() { "car_run", "car_run" });
        mdata_answer_index = Random.Range(0, 1000) % 9;
        mStation = new List<ShapeLogicGuanka10_Station>();
        //List<int> temp_lun = new List<int>() { 2, 3, 4 };
        //List<int> temp_star = new List<int>() { 1, 2, 3 };
        List<int> temp_window = new List<int>() { 0, 2, 2 };
        //List<int> temp_yan = new List<int>() { 1, 2, 3 };
        int[,] temp_lun = Common.GetArrayMuteId(3, 2);
        int[,] temp_star = Common.GetArrayMuteId(3, 1);
        int[,] temp_yan = Common.GetArrayMuteId(3, 1);


        List<Vector3> poss = Common.PosSortByWidth(700, 3, 220);
        for (int j = 0; j < 3; j++)
        {

            //temp_lun = Common.BreakRank<int>(temp_lun);
            //temp_star = Common.BreakRank<int>(temp_star);
            temp_window = Common.BreakRank<int>(temp_window);
            //temp_yan = Common.BreakRank<int>(temp_yan);
            for (int i = 0; i < 3; i++)
            {
                int index = j * 3 + i;
                

                mStation.Add(UguiMaker.newGameObject("station", transform).AddComponent<ShapeLogicGuanka10_Station>());
                mStation[index].Init(1, temp_lun[i, j], temp_star[i, j], temp_window[i], temp_yan[i, j]);
                mStation[index].mRtran.anchoredPosition3D = poss[i] + new Vector3(230, j * -240, 0);
                mStation[index].ReflushUI();
                if (index == mdata_answer_index)
                {
                    mStation[index].InitAnswer();
                }
                else
                {
                    mStation[index].BeginIn();

                    yield return new WaitForSeconds(0.1f);

                }
            }

        }

        mImageSelect = new Image[5];
        mRtranSelect = new RectTransform[5];
        string[] names = new string[] { "rtran_window", "rtran_yan", "rtran_star", "rtran_lun", "rtran_bg" };
        for(int i = 0; i < mRtranSelect.Length; i++)
        {
            mRtranSelect[i] = UguiMaker.newGameObject(names[i], transform).GetComponent<RectTransform>();

            mImageSelect[i] = UguiMaker.newImage("mSelect", mRtranSelect[i], "shapelogic_sprite", "guanka8_select", false);
            mImageSelect[i].type = Image.Type.Sliced;
            mImageSelect[i].gameObject.SetActive(false);

        }

        //window
        Image img0 = UguiMaker.newImage("window", mRtranSelect[0], "shapelogic_sprite", "guanka10_station2", false);
        Image img1 = UguiMaker.newImage("window", mRtranSelect[0], "shapelogic_sprite", "guanka10_station2", false);
        img0.rectTransform.anchoredPosition = new Vector2(-27.5f, 0);
        img1.rectTransform.anchoredPosition = new Vector2(27.5f, 0);
        Button btn0 = UguiMaker.newButton("btn", mRtranSelect[0], "public", "white");
        btn0.image.color = new Color(1, 1, 1, 0);
        btn0.image.rectTransform.sizeDelta = new Vector2(100, 100);
        btn0.onClick.AddListener(delegate() { OnClkWindow(2); });

        //yan
        Vector3[] vec30 = new Vector3[] { new Vector2(0, 0), new Vector2(0, 35f), new Vector2(0, 70f) };
        Vector3[] vec31 = new Vector3[] { new Vector2(-50, 0), new Vector2(0, 0), new Vector2(50, 0) };
        for (int i = 0; i < 3; i++)
        {
            RectTransform rtran = UguiMaker.newGameObject("rtran" + i.ToString(), mRtranSelect[1]).GetComponent<RectTransform>();
            rtran.anchoredPosition3D = vec31[i];
            
            btn0 = UguiMaker.newButton("btn", rtran, "public", "white");
            btn0.image.color = new Color(1, 1, 1, 0);
            btn0.image.rectTransform.sizeDelta = new Vector2(35, 40 * (i + 1));
            btn0.image.rectTransform.anchoredPosition = new Vector2(0, (i + 1) * 35 * 0.5f - 18);

            for (int j = 0; j < i + 1; j++)
            {
                Image img = UguiMaker.newImage(i.ToString(), rtran, "shapelogic_sprite", "guanka10_station4", false);
                img.rectTransform.anchoredPosition3D = vec30[j];
            }

        }
        mRtranSelect[1].transform.Find("rtran0/btn").GetComponent<Button>().onClick.AddListener(delegate () { OnClkYan(1); });
        mRtranSelect[1].transform.Find("rtran1/btn").GetComponent<Button>().onClick.AddListener(delegate () { OnClkYan(2); });
        mRtranSelect[1].transform.Find("rtran2/btn").GetComponent<Button>().onClick.AddListener(delegate () { OnClkYan(3); });

        //star
        vec31 = new Vector3[] { new Vector2(-140, 0), new Vector2(0, 0), new Vector2(140, 0) };
        for (int i = 0; i < 3; i++)
        {
            RectTransform rtran = UguiMaker.newGameObject("rtran" + i.ToString(), mRtranSelect[2]).GetComponent<RectTransform>();
            rtran.anchoredPosition3D = vec31[i];

            btn0 = UguiMaker.newButton("btn", rtran, "public", "white");
            btn0.image.color = new Color(1, 1, 1, 0);
            btn0.image.rectTransform.sizeDelta = new Vector2(100, 100);
            btn0.image.rectTransform.anchoredPosition = new Vector2(0, 0);

            switch (i + 1)
            {
                case 1:
                    vec30 = new Vector3[] { new Vector2(0, 0) };
                    break;
                case 2:
                    vec30 = new Vector3[] { new Vector2(-21.5f, 0), new Vector2(21.5f, 0) };
                    break;
                case 3:
                    vec30 = new Vector3[] { new Vector2(-31.4f, 11.35f), new Vector2(31.4f, 11.35f), new Vector2(0, -11.35f) };
                    break;
            }

            for (int j = 0; j < i + 1; j++)
            {
                Image img = UguiMaker.newImage(i.ToString(), rtran, "shapelogic_sprite", "guanka10_station5", false);
                img.rectTransform.anchoredPosition3D = vec30[j];
            }

        }
        mRtranSelect[2].transform.Find("rtran0/btn").GetComponent<Button>().onClick.AddListener(delegate () { OnClkStar(1); });
        mRtranSelect[2].transform.Find("rtran1/btn").GetComponent<Button>().onClick.AddListener(delegate () { OnClkStar(2); });
        mRtranSelect[2].transform.Find("rtran2/btn").GetComponent<Button>().onClick.AddListener(delegate () { OnClkStar(3); });

        //lun
        vec31 = new Vector3[] { new Vector2(0, 65), new Vector2(0, 0), new Vector2(0, -65) };
        int[] temp_lun_nums = new int[] { 2, 3, 4};
        for (int i = 0; i < 3; i++)
        {
            RectTransform rtran = UguiMaker.newGameObject("rtran" + i.ToString(), mRtranSelect[3]).GetComponent<RectTransform>();
            rtran.anchoredPosition3D = vec31[i];

            btn0 = UguiMaker.newButton("btn", rtran, "public", "white");
            btn0.image.color = new Color(1, 1, 1, 0);
            btn0.image.rectTransform.sizeDelta = new Vector2(55 * 4, 55);
            btn0.image.rectTransform.anchoredPosition = new Vector2(0, 0);

            switch (temp_lun_nums[i])
            {
                case 2:
                    vec30 = new Vector3[] { new Vector2(-70, 0), new Vector3(70, 0)};
                    break;
                case 3:
                    vec30 = new Vector3[] { new Vector2(-70, 0), new Vector3(0, 0), new Vector3(70, 0) };
                    break;
                case 4:
                    vec30 = new Vector3[] { new Vector2(-25, 0), new Vector2(-75, 0), new Vector2(25, 0), new Vector3(75, 0)};
                    break;
            }

            for (int j = 0; j < temp_lun_nums[i]; j++)
            {
                Image img = UguiMaker.newImage(i.ToString(), rtran, "shapelogic_sprite", "guanka10_station1", false);
                img.rectTransform.anchoredPosition3D = vec30[j];
            }

        }
        mRtranSelect[3].transform.Find("rtran0/btn").GetComponent<Button>().onClick.AddListener(delegate () { OnClkLun(2); });
        mRtranSelect[3].transform.Find("rtran1/btn").GetComponent<Button>().onClick.AddListener(delegate () { OnClkLun(3); });
        mRtranSelect[3].transform.Find("rtran2/btn").GetComponent<Button>().onClick.AddListener(delegate () { OnClkLun(4); });

        //bg
        img0 = UguiMaker.newImage("bg", mRtranSelect[4], "shapelogic_sprite", "guanka10_station0", false);
        btn0 = UguiMaker.newButton("btn", mRtranSelect[4], "public", "white");
        btn0.image.color = new Color(1, 1, 1, 0);
        btn0.image.rectTransform.sizeDelta = img0.rectTransform.sizeDelta;
        btn0.image.rectTransform.anchoredPosition = new Vector2(0, 0);
        btn0.onClick.AddListener(OnClkBg);

        Vector3[] poss0 = new Vector3[] { new Vector3(-278, 273, 0), new Vector3(-499, 237, 0) , new Vector3(-380, 158, 0) , new Vector3(-380, 0, 0), new Vector3(-380, -198, 0) };
        Vector3[] poss1 = new Vector3[] { new Vector3(-278 - 500, 273, 0), new Vector3(-499 - 500, 237, 0), new Vector3(-380-500, 158, 0), new Vector3(-380-500, 0, 0), new Vector3(-380-500, -198, 0) };
        for (int j = 0; j < mRtranSelect.Length; j++)
        {
            mRtranSelect[j].anchoredPosition3D = poss1[j];
        }
        for (int j = 0; j < mRtranSelect.Length; j++)
        {
            ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
            for (float i = 0; i < 1f; i += 0.1f)
            {
                mRtranSelect[j].anchoredPosition3D = Vector3.Lerp(poss1[j], poss0[j], Mathf.Sin(Mathf.PI * 0.5f * i));
                yield return new WaitForSeconds(0.01f);
            }
            mRtranSelect[j].anchoredPosition3D = poss0[j];
        }


        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        mOK = UguiMaker.newButton("mOK", transform, "shapelogic_sprite", "btn_up");
        mOK.onClick.AddListener(OnClkOK);
        mOK.transition = Selectable.Transition.None;
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mOK.image.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(-370, -460), new Vector2(-370, -350), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mOK.image.rectTransform.anchoredPosition = new Vector2(-370, -350);


        ShapeLogicCtl.instance.mSound.PlayTipListDefaultAb(
            new List<string>() { "10观察图形特征和排列顺序", "选择合适的小图案组成正确的大图案吧" },
            new List<float>() { 1, 1 }, true);

    }
    IEnumerator TEndGame()
    {
        yield return new WaitForSeconds(2f);
        ShapeLogicCtl.instance.GameNext();
        Color bg_color0 = mBg0.color;
        Color bg_color1 = mBg0.color;
        bg_color1.a = 0;

        for (float j = 0; j < 1f; j += 0.03f)
        {
            Color cor = Color.Lerp(new Color(1, 1, 1, 0.5f), new Color(1, 1, 1, 0), j);
            mBg0.color = Color.Lerp(bg_color0, bg_color1, j);
            yield return new WaitForSeconds(0.01f);
        }

        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }


        gameObject.SetActive(false);

    }
    IEnumerator TOver()
    {
        //ShapeLogicCtl.instance.DestroyLine();
        yield return new WaitForSeconds(0.8f);
        ShapeLogicCtl.instance.mSound.PlayShort("胜利通关音乐", 1);
        temp_updata_state = 1;
        for (int i = 0; i < mStation.Count; i++)
        {
            mStation[i].SetBoxEnable(true);
            //mStation[i].PlayCorrect();
        }
        mStation[mdata_answer_index].Play();
        Destroy(mStation[mdata_answer_index].mFrame.gameObject);

        Vector3[] poss0 = new Vector3[] { new Vector3(-278, 273, 0), new Vector3(-499, 237, 0), new Vector3(-380, 158, 0), new Vector3(-380, 0, 0), new Vector3(-380, -198, 0) };
        Vector3[] poss1 = new Vector3[] { new Vector3(-278 - 500, 273, 0), new Vector3(-499 - 500, 237, 0), new Vector3(-380 - 500, 158, 0), new Vector3(-380 - 500, 0, 0), new Vector3(-380 - 500, -198, 0) };
        for (int j = 0; j < mRtranSelect.Length; j++)
        {
            for (float i = 0; i < 1f; i += 0.1f)
            {
                mRtranSelect[j].anchoredPosition3D = Vector3.Lerp( poss0[j], poss1[j], Mathf.Sin(Mathf.PI * 0.5f * i - Mathf.PI * 0.5f) + 1);
                yield return new WaitForSeconds(0.01f);
            }
            mRtranSelect[j].anchoredPosition3D = poss1[j];
        }

        

        for (float i = 0; i < 1f; i += 0.08f)
        {
            mOK.image.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(-370, -350), new Vector2(-370, -460), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

    }


    public void OnClkWindow(int type)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        //Debug.Log(type);
        for (int i = 0; i < mStation[mdata_answer_index].mWindow.Length; i++)
        {
            mStation[mdata_answer_index].mWindow[i].enabled = false;
        }
        if(type == mdata_window)
        {
            mdata_window = 0;
            mImageSelect[0].gameObject.SetActive(false);

        }
        else
        {
            mdata_window = type;
            mImageSelect[0].gameObject.SetActive(true);
            mImageSelect[0].rectTransform.sizeDelta = new Vector2(142, 140);
            mImageSelect[0].rectTransform.anchoredPosition = new Vector2(-0.4f, 0);

            //for (int i = 0; i < mStation[mdata_answer_index].mWindow.Length; i++)
            //{
            //    mStation[mdata_answer_index].mWindow[i].enabled = true;
            //}
        }
        mStation[mdata_answer_index].temp_window = mdata_window;
        mStation[mdata_answer_index].ReflushUI_Temp();

    }
    public void OnClkYan(int num)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        for (int i = 0; i < mStation[mdata_answer_index].mYan.Length; i++)
        {
            mStation[mdata_answer_index].mYan[i].enabled = false;
        }
        if (num == mdata_yan)
        {
            mdata_yan = 0;
            mImageSelect[1].gameObject.SetActive(false);

        }
        else
        {
            mdata_yan = num;
            mImageSelect[1].gameObject.SetActive(true);
            switch(num)
            {
                case 1:
                    mImageSelect[1].rectTransform.sizeDelta = new Vector2(70, 100);
                    mImageSelect[1].rectTransform.anchoredPosition = new Vector2(-50.4f, 0.5f);
                    break;
                case 2:
                    mImageSelect[1].rectTransform.sizeDelta = new Vector2(70, 135.1f);
                    mImageSelect[1].rectTransform.anchoredPosition = new Vector2(-0.4f, 18.1f);
                    break;
                case 3:
                    mImageSelect[1].rectTransform.sizeDelta = new Vector2(70, 169.8f);
                    mImageSelect[1].rectTransform.anchoredPosition = new Vector2(49.88f, 35.5f);
                    break;
            }
            //for (int i = 0; i < mStation[mdata_answer_index].mYan.Length; i++)
            //{
            //    mStation[mdata_answer_index].mYan[i].enabled = i < num;
            //}
        }
        mStation[mdata_answer_index].temp_yan = mdata_yan;
        mStation[mdata_answer_index].ReflushUI_Temp();

    }
    public void OnClkStar(int num)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        //Debug.Log(num);
        for (int i = 0; i < mStation[mdata_answer_index].mStar.Length; i++)
        {
            mStation[mdata_answer_index].mStar[i].enabled = false;
        }
        if (num == mdata_star)
        {
            mdata_star = 0;
            mImageSelect[2].gameObject.SetActive(false);

        }
        else
        {
            mdata_star = num;
            mImageSelect[2].gameObject.SetActive(true);
            switch (num)
            {
                case 1:
                    mImageSelect[2].rectTransform.sizeDelta = new Vector2(150.6f, 111);
                    mImageSelect[2].rectTransform.anchoredPosition = new Vector2(-138.16f, 0);
                    //mStation[mdata_answer_index].mStar[0].enabled = true;
                    break;
                case 2:
                    mImageSelect[2].rectTransform.sizeDelta = new Vector2(150.6f, 111);
                    mImageSelect[2].rectTransform.anchoredPosition = new Vector2(-0.4f, 0);
                    //mStation[mdata_answer_index].mStar[0].enabled = true;
                    //mStation[mdata_answer_index].mStar[1].enabled = true;
                    break;
                case 3:
                    mImageSelect[2].rectTransform.sizeDelta = new Vector2(150.6f, 111);
                    mImageSelect[2].rectTransform.anchoredPosition = new Vector2(138.5f, -0.6f);
                    //mStation[mdata_answer_index].mStar[0].enabled = true;
                    //mStation[mdata_answer_index].mStar[1].enabled = true;
                    //mStation[mdata_answer_index].mStar[2].enabled = true;
                    break;
            }

        }
        mStation[mdata_answer_index].temp_star = mdata_star;
        mStation[mdata_answer_index].ReflushUI_Temp();

    }
    public void OnClkLun(int num)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        Debug.Log(num);  
        for (int i = 0; i < mStation[mdata_answer_index].mLun.Length; i++)
        {
            mStation[mdata_answer_index].mLun[i].enabled = false;
        }
        if (num == mdata_lun)
        {
            mdata_lun = 0;
            mImageSelect[3].gameObject.SetActive(false);

        }
        else
        {
            mdata_lun = num;
            mImageSelect[3].gameObject.SetActive(true);
            switch (num)
            {
                case 2:
                    mImageSelect[3].rectTransform.sizeDelta = new Vector2(267.6f, 96.98f);
                    mImageSelect[3].rectTransform.anchoredPosition = new Vector2(0, 66.3f);
                    //mStation[mdata_answer_index].mLun[0].enabled = true;
                    //mStation[mdata_answer_index].mLun[1].enabled = true;
                    break;
                case 3:
                    mImageSelect[3].rectTransform.sizeDelta = new Vector2(267.6f, 96.98f);
                    mImageSelect[3].rectTransform.anchoredPosition = new Vector2(0, -0.6f);
                    //mStation[mdata_answer_index].mLun[0].enabled = true;
                    //mStation[mdata_answer_index].mLun[1].enabled = true;
                    //mStation[mdata_answer_index].mLun[2].enabled = true;
                    break;
                case 4:
                    mImageSelect[3].rectTransform.sizeDelta = new Vector2(267.6f, 96.98f);
                    mImageSelect[3].rectTransform.anchoredPosition = new Vector2(0, -66.3f);
                    //mStation[mdata_answer_index].mLun[0].enabled = true;
                    //mStation[mdata_answer_index].mLun[1].enabled = true;
                    //mStation[mdata_answer_index].mLun[2].enabled = true;
                    //mStation[mdata_answer_index].mLun[3].enabled = true;
                    break;
            }
        }
        mStation[mdata_answer_index].temp_lun = mdata_lun;
        mStation[mdata_answer_index].ReflushUI_Temp();


    }
    public void OnClkBg()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        //Debug.Log("bg");
        if (mdata_bg == 0)
        {
            mdata_bg = 1;
            mStation[mdata_answer_index].mBg.enabled = true;
            mImageSelect[4].gameObject.SetActive(true);
            mImageSelect[4].rectTransform.sizeDelta = new Vector2(295, 230);
        }
        else
        {
            mdata_bg = 0;
            mStation[mdata_answer_index].mBg.enabled = false;
            mImageSelect[4].gameObject.SetActive(false);

        }
        mStation[mdata_answer_index].temp_bg = mdata_bg;
        //mStation[mdata_answer_index].ReflushUI_Temp();

    }

    public void OnClkOK()
    {
        if (0 != temp_updata_state)
            return;

        ShapeLogicCtl.instance.mSound.PlayShort("button_down");

        mOK.image.sprite = ResManager.GetSprite("shapelogic_sprite", "btn_down");
        mOK.enabled = false;

        bool correct = true;

        if (!mStation[mdata_answer_index].GetCorrect())
        {
            correct = false;
        }



        if (correct)
        {
            ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("04-星星-02");
            Debug.Log("回答正确!");
            mEffectOK = ResManager.GetPrefab("effect_star0", "effect_star1").GetComponent<ParticleSystem>();
            UguiMaker.InitGameObj(mEffectOK.gameObject, transform, "effect_ok", mOK.image.rectTransform.anchoredPosition3D, Vector3.one);
            mEffectOK.Play();
            StartCoroutine("TOver");

        }
        else
        {
            mStation[mdata_answer_index].PlayError();
            Debug.Log("回答错误!");
            Invoke("InvokeOnClkOk", 0.5f);
        }
    }
    void InvokeOnClkOk()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("button_up");
        mOK.image.sprite = ResManager.GetSprite("shapelogic_sprite", "btn_up");
        mOK.enabled = true;

    }


}
