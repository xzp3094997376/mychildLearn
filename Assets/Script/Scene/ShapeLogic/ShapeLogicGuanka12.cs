using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka12 : MonoBehaviour
{
    Button mOK;
    Image mBg0;
    ParticleSystem mEffectOK;

    public List<ShapeLogicGuanka12_Station> mStation;
    public List<ShapeLogicGuanka12_Select> mSelect;

    int mdata_correct_bg0 = 0;
    int mdata_correct_bg1 = 0;
    int mdata_correct_fruit = 0;
    int mdata_correct_top = 0;

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
                ShapeLogicGuanka12_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka12_Station>();
                if (null != com)
                {
                    ShapeLogicCtl.instance.mSound.PlayShort("素材出去通用");
                    com.SetBoxEnable(false);
                    com.Hide();
                    com.mdata_is_over = true;
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
                        Invoke("ShowGameOver", 1.5f);
                    }
                    break;
                }
            }

        }

    }

    public void ShowGameOver()
    {
        TopTitleCtl.instance.AddStar();
        GameOverCtl.GetInstance().Show(5, EndGame);
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
        mdata_correct_bg0 = 0;
        mdata_correct_bg1 = 0;
        mdata_correct_fruit = 0;
        mdata_correct_top = 0;

        //背景
        mBg0 = UguiMaker.newImage("mBg0", transform, "public", "white", false);
        mBg0.color = new Color32(0, 255, 255, 255);// new Color32(244, 213, 72, 255);
        mBg0.rectTransform.sizeDelta = new Vector2(1423, 800);
        //ShapeLogicCtl.instance.CreateLine(650, mBg0.transform, new List<Vector2>() { new Vector2(-249.4f, 104), new Vector2(-249.4f, -146.47f) });
        

        //相框
        mdata_answer_index = 4;// Random.Range(0, 1000) % 9;
        mStation = new List<ShapeLogicGuanka12_Station>();
        int[,] temp_bg0 = Common.GetArrayMuteId(3, 1);
        int[,] temp_bg1 = Common.GetArrayMuteId(3, 1);
        int[,] temp_fruit = Common.GetArrayMuteId(3, 1);
        int[,] temp_top = Common.GetArrayMuteId(3, 1);

        List<Vector3> poss = Common.PosSortByWidth(700, 3, 200);
        for (int j = 0; j < 3; j++)
        {

            //temp_bg0 = Common.BreakRank<int>(temp_bg0);
            //temp_bg1 = Common.BreakRank<int>(temp_bg1);
            //temp_fruit = Common.BreakRank<int>(temp_fruit);
            //temp_top = Common.BreakRank<int>(temp_top);
            
            for (int i = 0; i < 3; i++)
            {
                int index = j * 3 + i;

                mStation.Add(UguiMaker.newGameObject("station", transform).AddComponent<ShapeLogicGuanka12_Station>());
                if (index == mdata_answer_index)
                {
                    mdata_correct_bg0 = temp_bg0[j,i];
                    mdata_correct_bg1 = temp_bg1[j, i];
                    mdata_correct_fruit = temp_fruit[j, i];
                    mdata_correct_top = temp_top[j, i];


                    mStation[index].Init(0, 0, 0, 0);
                    mStation[index].mRtran.anchoredPosition3D = poss[i] + new Vector3(-250, j * -240, 0);
                    mStation[index].InitFrameBg();

                }
                else
                {
                    mStation[index].Init(temp_bg0[j, i], temp_bg1[j, i], temp_fruit[j, i], temp_top[j, i]);
                    mStation[index].mRtran.anchoredPosition3D = poss[i] + new Vector3(-250, j * -240, 0);
                    mStation[index].BeginIn();

                }
                mStation[index].ReflushUI();

            }

        }

        //道具
        mSelect = new List<ShapeLogicGuanka12_Select>();
        for(int i = 0; i < 12; i++)
        {
            mSelect.Add(UguiMaker.newGameObject("select" + i.ToString(), transform).AddComponent<ShapeLogicGuanka12_Select>());
        }
        float subw = 180;
        List<Vector2> poss0 = new List<Vector2>() { new Vector2(350 - subw, 406), new Vector2(350, 406), new Vector2(350 + subw, 406) };
        for(int i = 0; i < 3; i++)
        {
            mSelect[i].Init("fruit", i + 1);
            mSelect[i].mRtran.anchoredPosition = poss0[i];
            mSelect[i].PlayFruit();
        }
        subw = 150;
        poss0 = new List<Vector2>() { new Vector2(350 - subw, 125), new Vector2(350, 125), new Vector2(350 + subw, 125) };
        for (int i = 0; i < 3; i++)
        {
            int index = 3 + i;
            mSelect[index].Init("top", i + 1);
            mSelect[index].mRtran.anchoredPosition = poss0[i];
        }
        subw = 160;
        poss0 = new List<Vector2>() { new Vector2(350 - subw, 0), new Vector2(350, 0), new Vector2(350 + subw, 0) };
        for (int i = 0; i < 3; i++)
        {
            int index = 6 + i;
            mSelect[index].Init("bg0", i + 1);
            mSelect[index].mRtran.anchoredPosition = poss0[i];
        }
        subw = 120;
        poss0 = new List<Vector2>() { new Vector2(360 - subw, -112.63f), new Vector2(360, -112.63f), new Vector2(360 + subw, -112.63f) };
        for (int i = 0; i < 3; i++)
        {
            int index = 9 + i;
            mSelect[index].Init("bg1", i + 1);
            mSelect[index].mRtran.anchoredPosition = poss0[i];
            mSelect[index].PlayBg1();
        }





        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        mOK = UguiMaker.newButton("mOK", transform, "shapelogic_sprite", "btn_up");
        mOK.onClick.AddListener(OnClkOK);
        mOK.transition = Selectable.Transition.None;
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mOK.image.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(573.5f, -460), new Vector2(573.5f, -328.9f), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mOK.image.rectTransform.anchoredPosition = new Vector2(573.5f, -328.9f);


        ShapeLogicCtl.instance.mSound.PlayTipListDefaultAb(
            new List<string>() { "12观察小相框的排列顺序", "选择合适的小图案组成正确的大图案吧" },
            new List<float>() { 1, 1 }, true);

    }
    IEnumerator TEndGame()
    {
        yield return new WaitForSeconds(1f);
        ShapeLogicCtl.instance.Reset();
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

        temp_updata_state = 1;
        for (int i = 0; i < mStation.Count; i++)
        {
            mStation[i].Play();
        }


        ShapeLogicCtl.instance.mSound.PlayShort("素材出去通用");
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mOK.image.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(573.5f, -328.9f), new Vector2(573.5f, -460), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }


        ShapeLogicCtl.instance.mSound.PlayShort("素材出去通用");
        for (int i = 0; i < mSelect.Count; i++)
        {
            mSelect[i].Hide();
        }
        for (int i = 0; i < mStation.Count; i++)
        {
            mStation[i].SetBoxEnable(true);
        }

        //yield return new WaitForSeconds(1);
        //TopTitleCtl.instance.AddStar();
        //GameOverCtl.GetInstance().Show(5, EndGame);

    }

    public void callback_FlushStation()
    {
        mStation[mdata_answer_index].ReflushUI();
    }
    public void callback_SetStation(string thing, int type)
    {
        SetSelectClickEnable(thing, false);
        switch (thing)
        {
            case "bg0":
                {
                    int index = mStation[mdata_answer_index].mdata_bg0 - 1 + 6;
                    if (0 != mStation[mdata_answer_index].mdata_bg0)
                    {
                        mSelect[index].ResetFly();
                    }
                    mStation[mdata_answer_index].mdata_bg0 = 0;
                    mStation[mdata_answer_index].ReflushUI();
                    mStation[mdata_answer_index].mdata_bg0 = type;
                }
                break;
            case "bg1":
                {
                    int index = mStation[mdata_answer_index].mdata_bg1 - 1 + 9;
                    if (0 != mStation[mdata_answer_index].mdata_bg1)
                    {
                        mSelect[index].ResetFly();
                    }
                    mStation[mdata_answer_index].mdata_bg1 = 0;
                    mStation[mdata_answer_index].ReflushUI();
                    mStation[mdata_answer_index].mdata_bg1 = type;

                }
                break;
            case "top":
                {
                    int index = mStation[mdata_answer_index].mdata_top - 1 + 3;
                    if (0 != mStation[mdata_answer_index].mdata_top)
                    {
                        mSelect[index].ResetFly();
                    }
                    mStation[mdata_answer_index].mdata_top = 0;
                    mStation[mdata_answer_index].ReflushUI();
                    mStation[mdata_answer_index].mdata_top = type;

                }
                break;
            case "fruit":
                {
                    int index = mStation[mdata_answer_index].mdata_fruit - 1;
                    if (0 != mStation[mdata_answer_index].mdata_fruit)
                    {
                        mSelect[index].ResetFly();
                    }
                    mStation[mdata_answer_index].mdata_fruit = 0;
                    mStation[mdata_answer_index].ReflushUI();
                    mStation[mdata_answer_index].mdata_fruit = type;

                }
                break;
        }



    }
    public void SetSelectClickEnable(string thing, bool _enable)
    {
        switch (thing)
        {
            case "bg0":
                {
                    mSelect[6].mSelect.raycastTarget = _enable & mSelect[6].gameObject.activeSelf;
                    mSelect[7].mSelect.raycastTarget = _enable & mSelect[7].gameObject.activeSelf;
                    mSelect[8].mSelect.raycastTarget = _enable & mSelect[8].gameObject.activeSelf;
                }
                break;
            case "bg1":
                {
                    mSelect[9].mSelect.raycastTarget = _enable & mSelect[9].gameObject.activeSelf;
                    mSelect[10].mSelect.raycastTarget = _enable & mSelect[10].gameObject.activeSelf;
                    mSelect[11].mSelect.raycastTarget = _enable & mSelect[11].gameObject.activeSelf;

                }
                break;
            case "top":
                {
                    mSelect[3].mSelect.raycastTarget = _enable & mSelect[3].gameObject.activeSelf;
                    mSelect[4].mSelect.raycastTarget = _enable & mSelect[4].gameObject.activeSelf;
                    mSelect[5].mSelect.raycastTarget = _enable & mSelect[5].gameObject.activeSelf;

                }
                break;
            case "fruit":
                {
                    mSelect[0].mSelect.raycastTarget = _enable & mSelect[0].gameObject.activeSelf;
                    mSelect[1].mSelect.raycastTarget = _enable & mSelect[1].gameObject.activeSelf;
                    mSelect[2].mSelect.raycastTarget = _enable & mSelect[2].gameObject.activeSelf;

                }

                break;
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
        //for (int i = 0; i < mSelect.Count; i++)
        //{
        //    mSelect[i].Hide();
        //}


        if (0 != temp_updata_state)
            return;

        ShapeLogicCtl.instance.mSound.PlayShort("button_down");
        mOK.image.sprite = ResManager.GetSprite("shapelogic_sprite", "btn_down");
        mOK.enabled = false;

        bool correct = true;

        if (mStation[mdata_answer_index].mdata_bg0 != mdata_correct_bg0 ||
            mStation[mdata_answer_index].mdata_bg1 != mdata_correct_bg1 ||
            mStation[mdata_answer_index].mdata_top != mdata_correct_top ||
            mStation[mdata_answer_index].mdata_fruit != mdata_correct_fruit)
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
