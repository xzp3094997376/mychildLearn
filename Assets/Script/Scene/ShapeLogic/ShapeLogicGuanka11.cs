using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka11 : MonoBehaviour
{
    Button mOK;
    Image mBg0;
    RectTransform mMask;
    //Image[] mImageSelect;
    //RectTransform[] mRtranSelect;
    ParticleSystem mEffectOK;

    public List<ShapeLogicGuanka11_Station> mStation;
    ShapeLogicGuanka11_Station mStationShow;

    int mdata_correct_door_type = 0;
    int mdata_correct_window_type = 0;
    int mdata_correct_top_side = 0;
    int mdata_correct_qizi_type = 0;

    public int mdata_answer_index;
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
                ShapeLogicGuanka11_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka11_Station>();
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
        mdata_correct_door_type = 0;
        mdata_correct_window_type = 0;
        mdata_correct_top_side = 0;
        mdata_correct_qizi_type = 0;

        //背景
        mBg0 = UguiMaker.newImage("mBg0", transform, "public", "white", false);
        mBg0.color = new Color32(176, 140, 255, 255);
        mBg0.rectTransform.sizeDelta = new Vector2(1423, 800);

        Image[] tiao = new Image[4];
        for(int i = 0; i < tiao.Length; i++)
        {
            tiao[i] = UguiMaker.newImage(i.ToString(), mBg0.transform, "public", "white", false);
            tiao[i].color = new Color32(174, 132, 255, 255);
            tiao[i].rectTransform.sizeDelta = new Vector2(187.51f, 1000);
        }
        tiao[0].rectTransform.anchoredPosition = new Vector2(-521, 3);
        tiao[0].rectTransform.localEulerAngles = new Vector3(0, 0, 3);

        tiao[1].rectTransform.anchoredPosition = new Vector2(-210, 42);
        tiao[1].rectTransform.localEulerAngles = new Vector3(0, 0, 356.5f);

        tiao[2].rectTransform.anchoredPosition = new Vector2(413, 76);
        tiao[2].rectTransform.localEulerAngles = new Vector3(0, 0, 356.5f);

        tiao[3].rectTransform.anchoredPosition = new Vector2(102, 37);
        tiao[3].rectTransform.localEulerAngles = new Vector3(0, 0, 3);



        mMask = UguiMaker.newImage("mMask", transform, "public", "white", false).GetComponent<RectTransform>();
        mMask.gameObject.AddComponent<Mask>().showMaskGraphic = false;
        mMask.sizeDelta = new Vector2(1423, 0);
        mMask.pivot = new Vector2(0.5f, 0);
        mMask.anchoredPosition = new Vector2(0, -400);


        //房子
        mdata_answer_index = 3;// Random.Range(0, 1000) % 9;
        mStation = new List<ShapeLogicGuanka11_Station>();
        int[,] temp_door_type = Common.GetArrayMuteId(3);
        int[,] temp_window_type = Common.GetArrayMuteId(3);
        int[,] temp_top_side = Common.GetArrayMuteId(3, -1);
        List<int> temp_qizi_type = new List<int>();// new List<int>() { 0, 1, 2 };根据top对应生成
        List<Vector3> poss = Common.PosSortByWidth(800, 3, 570);
        for (int j = 0; j < 3; j++)
        {

            //temp_door_type = Common.BreakRank<int>(temp_door_type);
            //temp_window_type = Common.BreakRank<int>(temp_window_type);
            //temp_top_side = Common.BreakRank<int>(temp_top_side);

            temp_qizi_type.Clear();
            for(int i = 0; i < 3; i++)
            {
                switch(temp_top_side[j,i])
                {
                    case -1:
                        temp_qizi_type.Add(0);
                        break;
                    case 0:
                        temp_qizi_type.Add(1);
                        break;
                    case 1:
                        temp_qizi_type.Add(2);
                        break;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                int index = j * 3 + i;
                
                mStation.Add(UguiMaker.newGameObject("station", mMask).AddComponent<ShapeLogicGuanka11_Station>());
                if (index == mdata_answer_index)
                {
                    mdata_correct_door_type = temp_door_type[j,i];
                    mdata_correct_window_type = temp_window_type[j,i];
                    mdata_correct_top_side = temp_top_side[j,i];
                    mdata_correct_qizi_type = temp_qizi_type[i];


                    mStation[index].Init(0, 0, 0, 0);
                    mStation[index].InitFrameBg();

                    mStationShow = UguiMaker.newGameObject("station_show", mMask).AddComponent<ShapeLogicGuanka11_Station>();
                    mStationShow.Init(0, 0, 0, 0);
                    mStationShow.InitAnswer();
                    mStationShow.mRtran.localScale = new Vector3(1.6f, 1.6f, 1);
                    mStationShow.mRtran.anchorMax = new Vector2(0.5f, 0);
                    mStationShow.mRtran.anchorMin = new Vector2(0.5f, 0);
                    mStationShow.mRtran.anchoredPosition = new Vector2(-453, 365.2f);
                    mStationShow.ReflushUI();
                }
                else
                {
                    mStation[index].Init(temp_door_type[j,i], temp_window_type[j,i], temp_top_side[j,i], temp_qizi_type[i]);

                }
                mStation[index].mRtran.anchorMax = new Vector2(0.5f, 0);
                mStation[index].mRtran.anchorMin = new Vector2(0.5f, 0);
                mStation[index].mRtran.anchoredPosition3D = poss[i] + new Vector3(120, j * -245, 0);
                mStation[index].ReflushUI();

            }

        }


        AudioSource sound = gameObject.AddComponent<AudioSource>();
        sound.clip = ResManager.GetClip("shapelogic_sound", "电流平均");
        sound.loop = true;
        sound.Play();


        Image green_line = UguiMaker.newImage("green_line", transform, "public", "white", false);
        green_line.color = new Color(0, 1, 0, 1);
        green_line.rectTransform.sizeDelta = new Vector2(1423, 4);
        for(float i = 0; i < 1f; i += 0.01f)
        {
            mMask.sizeDelta = new Vector2(1423, 800 * i);
            green_line.rectTransform.anchoredPosition = new Vector2(0, 800 * i - 400);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(green_line.gameObject);
        mMask.sizeDelta = new Vector2(1423, 800);

        sound.Stop();


        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        mOK = UguiMaker.newButton("mOK", transform, "shapelogic_sprite", "btn_up");
        mOK.onClick.AddListener(OnClkOK);
        mOK.transition = Selectable.Transition.None;
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mOK.image.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(-453, -460), new Vector2(-453, -296), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mOK.image.rectTransform.anchoredPosition = new Vector2(-453, -296);

        //ShapeLogicCtl.instance.CreateLine(708.6f, mBg0.transform, new List<Vector2>() { new Vector2(115.6f, 92.09f), new Vector2(115.6f, -159.3f) }, true);

        ShapeLogicCtl.instance.mSound.PlayTipListDefaultAb(
            new List<string>() { "11观察长方形房子的变化规律", "选择合适的小图案组成正确的大图案吧" },
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
            mStation[i].Play();
            //mStation[i].PlayCorrect();
        }

        //Vector3[] poss0 = new Vector3[] { new Vector3(-278, 273, 0), new Vector3(-499, 237, 0), new Vector3(-380, 158, 0), new Vector3(-380, 0, 0), new Vector3(-380, -198, 0) };
        //Vector3[] poss1 = new Vector3[] { new Vector3(-278 - 500, 273, 0), new Vector3(-499 - 500, 237, 0), new Vector3(-380 - 500, 158, 0), new Vector3(-380 - 500, 0, 0), new Vector3(-380 - 500, -198, 0) };
        //for (int j = 0; j < mRtranSelect.Length; j++)
        //{
        //    for (float i = 0; i < 1f; i += 0.1f)
        //    {
        //        mRtranSelect[j].anchoredPosition3D = Vector3.Lerp(poss0[j], poss1[j], Mathf.Sin(Mathf.PI * 0.5f * i - Mathf.PI * 0.5f) + 1);
        //        yield return new WaitForSeconds(0.01f);
        //    }
        //    mRtranSelect[j].anchoredPosition3D = poss1[j];
        //}


        yield return new WaitForSeconds(1);
        Vector3 pos0 = mStationShow.mRtran.anchoredPosition3D;
        Vector3 pos1 = pos0 + new Vector3(0, -800, 0);
        Vector3 pos2 = mOK.image.rectTransform.anchoredPosition3D;
        Vector3 pos3 = pos2 + new Vector3(0, -800, 0);


        ShapeLogicCtl.instance.mSound.PlayShort("素材出去通用");
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mStationShow.mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
            mOK.image.rectTransform.anchoredPosition3D = Vector3.Lerp(pos2, pos3, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

    }


    //public void OnClkWindow(int type)
    //{
    //    //Debug.Log(type);
    //    for (int i = 0; i < mStation[mdata_answer_index].mWindow.Length; i++)
    //    {
    //        mStation[mdata_answer_index].mWindow[i].enabled = false;
    //    }
    //    if (type == mdata_window)
    //    {
    //        mdata_window = 0;
    //        mImageSelect[0].gameObject.SetActive(false);

    //    }
    //    else
    //    {
    //        mdata_window = type;
    //        mImageSelect[0].gameObject.SetActive(true);
    //        mImageSelect[0].rectTransform.sizeDelta = new Vector2(142, 140);
    //        mImageSelect[0].rectTransform.anchoredPosition = new Vector2(-0.4f, 0);

    //        //for (int i = 0; i < mStation[mdata_answer_index].mWindow.Length; i++)
    //        //{
    //        //    mStation[mdata_answer_index].mWindow[i].enabled = true;
    //        //}
    //    }
    //    mStation[mdata_answer_index].temp_window = mdata_window;
    //    mStation[mdata_answer_index].ReflushUI_Temp();

    //}
    //public void OnClkYan(int num)
    //{
    //    for (int i = 0; i < mStation[mdata_answer_index].mYan.Length; i++)
    //    {
    //        mStation[mdata_answer_index].mYan[i].enabled = false;
    //    }
    //    if (num == mdata_yan)
    //    {
    //        mdata_yan = 0;
    //        mImageSelect[1].gameObject.SetActive(false);

    //    }
    //    else
    //    {
    //        mdata_yan = num;
    //        mImageSelect[1].gameObject.SetActive(true);
    //        switch (num)
    //        {
    //            case 1:
    //                mImageSelect[1].rectTransform.sizeDelta = new Vector2(70, 100);
    //                mImageSelect[1].rectTransform.anchoredPosition = new Vector2(-50.4f, 0.5f);
    //                break;
    //            case 2:
    //                mImageSelect[1].rectTransform.sizeDelta = new Vector2(70, 135.1f);
    //                mImageSelect[1].rectTransform.anchoredPosition = new Vector2(-0.4f, 18.1f);
    //                break;
    //            case 3:
    //                mImageSelect[1].rectTransform.sizeDelta = new Vector2(70, 169.8f);
    //                mImageSelect[1].rectTransform.anchoredPosition = new Vector2(49.88f, 35.5f);
    //                break;
    //        }
    //        //for (int i = 0; i < mStation[mdata_answer_index].mYan.Length; i++)
    //        //{
    //        //    mStation[mdata_answer_index].mYan[i].enabled = i < num;
    //        //}
    //    }
    //    mStation[mdata_answer_index].temp_yan = mdata_yan;
    //    mStation[mdata_answer_index].ReflushUI_Temp();

    //}
    //public void OnClkStar(int num)
    //{
    //    //Debug.Log(num);
    //    for (int i = 0; i < mStation[mdata_answer_index].mStar.Length; i++)
    //    {
    //        mStation[mdata_answer_index].mStar[i].enabled = false;
    //    }
    //    if (num == mdata_star)
    //    {
    //        mdata_star = 0;
    //        mImageSelect[2].gameObject.SetActive(false);

    //    }
    //    else
    //    {
    //        mdata_star = num;
    //        mImageSelect[2].gameObject.SetActive(true);
    //        switch (num)
    //        {
    //            case 1:
    //                mImageSelect[2].rectTransform.sizeDelta = new Vector2(150.6f, 111);
    //                mImageSelect[2].rectTransform.anchoredPosition = new Vector2(-138.16f, 0);
    //                //mStation[mdata_answer_index].mStar[0].enabled = true;
    //                break;
    //            case 2:
    //                mImageSelect[2].rectTransform.sizeDelta = new Vector2(150.6f, 111);
    //                mImageSelect[2].rectTransform.anchoredPosition = new Vector2(-0.4f, 0);
    //                //mStation[mdata_answer_index].mStar[0].enabled = true;
    //                //mStation[mdata_answer_index].mStar[1].enabled = true;
    //                break;
    //            case 3:
    //                mImageSelect[2].rectTransform.sizeDelta = new Vector2(150.6f, 111);
    //                mImageSelect[2].rectTransform.anchoredPosition = new Vector2(138.5f, -0.6f);
    //                //mStation[mdata_answer_index].mStar[0].enabled = true;
    //                //mStation[mdata_answer_index].mStar[1].enabled = true;
    //                //mStation[mdata_answer_index].mStar[2].enabled = true;
    //                break;
    //        }

    //    }
    //    mStation[mdata_answer_index].temp_star = mdata_star;
    //    mStation[mdata_answer_index].ReflushUI_Temp();

    //}
    //public void OnClkLun(int num)
    //{
    //    Debug.Log(num);
    //    for (int i = 0; i < mStation[mdata_answer_index].mLun.Length; i++)
    //    {
    //        mStation[mdata_answer_index].mLun[i].enabled = false;
    //    }
    //    if (num == mdata_lun)
    //    {
    //        mdata_lun = 0;
    //        mImageSelect[3].gameObject.SetActive(false);

    //    }
    //    else
    //    {
    //        mdata_lun = num;
    //        mImageSelect[3].gameObject.SetActive(true);
    //        switch (num)
    //        {
    //            case 2:
    //                mImageSelect[3].rectTransform.sizeDelta = new Vector2(267.6f, 96.98f);
    //                mImageSelect[3].rectTransform.anchoredPosition = new Vector2(0, 66.3f);
    //                //mStation[mdata_answer_index].mLun[0].enabled = true;
    //                //mStation[mdata_answer_index].mLun[1].enabled = true;
    //                break;
    //            case 3:
    //                mImageSelect[3].rectTransform.sizeDelta = new Vector2(267.6f, 96.98f);
    //                mImageSelect[3].rectTransform.anchoredPosition = new Vector2(0, -0.6f);
    //                //mStation[mdata_answer_index].mLun[0].enabled = true;
    //                //mStation[mdata_answer_index].mLun[1].enabled = true;
    //                //mStation[mdata_answer_index].mLun[2].enabled = true;
    //                break;
    //            case 4:
    //                mImageSelect[3].rectTransform.sizeDelta = new Vector2(267.6f, 96.98f);
    //                mImageSelect[3].rectTransform.anchoredPosition = new Vector2(0, -66.3f);
    //                //mStation[mdata_answer_index].mLun[0].enabled = true;
    //                //mStation[mdata_answer_index].mLun[1].enabled = true;
    //                //mStation[mdata_answer_index].mLun[2].enabled = true;
    //                //mStation[mdata_answer_index].mLun[3].enabled = true;
    //                break;
    //        }
    //    }
    //    mStation[mdata_answer_index].temp_lun = mdata_lun;
    //    mStation[mdata_answer_index].ReflushUI_Temp();


    //}
    //public void OnClkBg()
    //{
    //    //Debug.Log("bg");
    //    if (mdata_bg == 0)
    //    {
    //        mdata_bg = 1;
    //        mStation[mdata_answer_index].mBg.enabled = true;
    //        mImageSelect[4].gameObject.SetActive(true);
    //        mImageSelect[4].rectTransform.sizeDelta = new Vector2(295, 230);
    //    }
    //    else
    //    {
    //        mdata_bg = 0;
    //        mStation[mdata_answer_index].mBg.enabled = false;
    //        mImageSelect[4].gameObject.SetActive(false);

    //    }
    //    mStation[mdata_answer_index].temp_bg = mdata_bg;
    //    //mStation[mdata_answer_index].ReflushUI_Temp();

    //}

    public void OnClkOK()
    {

        if (0 != temp_updata_state)
            return;

        ShapeLogicCtl.instance.mSound.PlayShort("button_down");

        mOK.image.sprite = ResManager.GetSprite("shapelogic_sprite", "btn_down");
        mOK.enabled = false;

        bool correct = true;

        if (mStation[mdata_answer_index].mdata_door_type != mdata_correct_door_type ||
            mStation[mdata_answer_index].mdata_window_type != mdata_correct_window_type ||
            mStation[mdata_answer_index].mdata_top_side != mdata_correct_top_side ||
            mStation[mdata_answer_index].mdata_qizi_type != mdata_correct_qizi_type)
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

        //mStation[0].PlayShoot();

    }
    void InvokeOnClkOk()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("button_up");
        mOK.image.sprite = ResManager.GetSprite("shapelogic_sprite", "btn_up");
        mOK.enabled = true;

    }

}
