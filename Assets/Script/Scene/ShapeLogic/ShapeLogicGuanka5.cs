using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka5 : MonoBehaviour
{
    Image mBg;
    Image[] mBgTiao = null;
    Image[] mColorSelect;
    Image mAnswerBg;
    Button mOK;
    ParticleSystem mEffectOK;

    List<ShapeLogicGuanka5_Station> mStations;
    List<List<int>> mdata_question_colors;
    Dictionary<int, List<int>> mdata_dic_question_color;

    int mdata_answer_index = 0;
    public int mdata_select_color = 0;


    void Start () {
	
	}

    Vector3 temp_speed0 = new Vector3(1, 0, 0);
    Vector3 temp_speed1 = new Vector3(-1, 0, 0);
    void Update () {
        if(null != mBgTiao)
        {
            mBgTiao[0].rectTransform.anchoredPosition3D += temp_speed0;
            mBgTiao[1].rectTransform.anchoredPosition3D += temp_speed1;
            mBgTiao[2].rectTransform.anchoredPosition3D += temp_speed0;
            mBgTiao[3].rectTransform.anchoredPosition3D += temp_speed1;
            mBgTiao[4].rectTransform.anchoredPosition3D += temp_speed0;
            if (mBgTiao[0].rectTransform.anchoredPosition3D.x > 1020)
                mBgTiao[0].rectTransform.anchoredPosition3D = new Vector3(0, mBgTiao[0].rectTransform.anchoredPosition3D.y, 0);
            if (mBgTiao[2].rectTransform.anchoredPosition3D.x > 1020)
                mBgTiao[2].rectTransform.anchoredPosition3D = new Vector3(0, mBgTiao[2].rectTransform.anchoredPosition3D.y, 0);
            if (mBgTiao[4].rectTransform.anchoredPosition3D.x > 1020)
                mBgTiao[4].rectTransform.anchoredPosition3D = new Vector3(0, mBgTiao[4].rectTransform.anchoredPosition3D.y, 0);
            if (mBgTiao[1].rectTransform.anchoredPosition3D.x < -1020)
                mBgTiao[1].rectTransform.anchoredPosition3D = new Vector3(0, mBgTiao[1].rectTransform.anchoredPosition3D.y, 0);
            if (mBgTiao[3].rectTransform.anchoredPosition3D.x < -1020)
                mBgTiao[3].rectTransform.anchoredPosition3D = new Vector3(0, mBgTiao[3].rectTransform.anchoredPosition3D.y, 0);

        }

    }


    public void GameOverReset()
    {
        StartCoroutine("TGameOverReset");
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
        temp_over_count = 0;
        mdata_select_color = 0;
        //每组的颜色数据
        mdata_question_colors = new List<List<int>>();
        mdata_dic_question_color = new Dictionary<int, List<int>>();
        mdata_dic_question_color.Add(0, new List<int>() { 0, 0, 0, 0, 1, 1, 1, 1 });
        mdata_dic_question_color.Add(1, new List<int>() { 1, 0, 0, 0, 0, 1, 1, 1});
        mdata_dic_question_color.Add(2, new List<int>() { 1, 1, 0, 0, 0, 0, 1, 1});
        mdata_dic_question_color.Add(3, new List<int>() { 1, 1, 1, 0, 0, 0, 0, 1});
        mdata_dic_question_color.Add(4, new List<int>() { 1, 1, 1, 1, 0, 0, 0, 0});
        mdata_dic_question_color.Add(5, new List<int>() { 0, 1, 1, 1, 1, 0, 0, 0});
        mdata_dic_question_color.Add(6, new List<int>() { 0, 0, 1, 1, 1, 1, 0, 0});
        mdata_dic_question_color.Add(7, new List<int>() { 0, 0, 0, 1, 1, 1, 1, 0});
        int temp_rand_index = Random.Range(0, 1000) % 8;
        for (int i = 0; i < 8; i++)
        {
            int key = temp_rand_index + i;
            if (key >= 8)
                key -= 8;
            mdata_question_colors.Add(mdata_dic_question_color[key]);
        }

        mdata_answer_index = Random.Range(0, 1000) % 7 + 1;//不能是第一个

        mBg = UguiMaker.newImage("mBg0", transform, "public", "white", false);
        mBg.color = new Color32(118, 233, 255, 255);
        mBg.rectTransform.sizeDelta = new Vector2(1423, 800);

        List<Vector3> poss = Common.PosSortByHeight(850, 5, 0);
        mBgTiao = new Image[5];
        for (int i = 0; i < 5; i++)
        {
            Image bg = UguiMaker.newImage("mBg" + i, mBg.transform, "shapelogic_sprite", "guanka2_bg", false);
            bg.rectTransform.localEulerAngles = new Vector3(0, 0, 0);
            bg.rectTransform.anchoredPosition3D = poss[i];
            bg.rectTransform.sizeDelta = new Vector2(4000, 131);
            bg.type = Image.Type.Tiled;
            bg.color = new Color32(255, 255, 255, 255);

            mBgTiao[i] = bg;

        }

        mStations = new List<ShapeLogicGuanka5_Station>();
        List<Vector3> poss0 = Common.PosSortByWidth(1100, 4, 180);
        List<Vector3> poss1 = Common.PosSortByWidth(1100, 4, -133);

        ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("开关切换驾驶舱1");
        for (int i = 0; i < 8; i++)
        {
            ShapeLogicGuanka5_Station station = UguiMaker.newGameObject("station" + i.ToString(), transform).AddComponent<ShapeLogicGuanka5_Station>();
            station.Init();
            station.InitColor(mdata_question_colors[i]);
            if(mdata_answer_index == i)
            {
                station.SetAnswerObj();
            }
            if (i < 4 )
            {
                station.mRtran.anchoredPosition3D = poss0[i];
                station.SetResetPos(poss0[i]);
            }
            else
            {
                station.mRtran.anchoredPosition3D = poss1[i - 4];
                station.SetResetPos(poss1[i - 4]);
            }
            if (mdata_answer_index == i)
            {
                mAnswerBg = UguiMaker.newImage("mAnswerBg", transform, "shapelogic_sprite", "guanka1_bg1", false);
                mAnswerBg.transform.SetSiblingIndex(1);
                mAnswerBg.type = Image.Type.Sliced;
                mAnswerBg.rectTransform.sizeDelta = new Vector2(265, 308);
                mAnswerBg.rectTransform.anchoredPosition3D = station.mRtran.anchoredPosition3D + new Vector3(0, -15);

            }
            station.Show();
            mStations.Add(station);

        }

        ShapeLogicCtl.instance.mSound.PlayTipListDefaultAb(
            new List<string>() { "5观察长方形里面颜色的变化规律", "选择合适的小图案组成正确的大图案吧" },
            new List<float>() { 1, 1 }, true);

        yield return new WaitForSeconds(1.8f);
        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        mOK = UguiMaker.newButton("mOK", transform, "shapelogic_sprite", "btn_up");
        mOK.onClick.AddListener(OnClkOK);


        mColorSelect = new Image[3];
        mColorSelect[2] = UguiMaker.newImage("mColorSelectFrame", transform, "shapelogic_sprite", "guanka5_color_frame", false);
        mColorSelect[0] = UguiMaker.newImage("mColorSelect0", transform, "shapelogic_sprite", "guanka5_color0", true);
        mColorSelect[1] = UguiMaker.newImage("mColorSelect1", transform, "shapelogic_sprite", "guanka5_color1", true);
        mColorSelect[0].gameObject.AddComponent<Button>().onClick.AddListener(OnClkColor0);
        mColorSelect[1].gameObject.AddComponent<Button>().onClick.AddListener(OnClkColor1);
        mColorSelect[0].rectTransform.anchoredPosition3D = new Vector3(-80, -350);
        mColorSelect[1].rectTransform.anchoredPosition3D = new Vector3(80, -350);
        mColorSelect[2].rectTransform.anchoredPosition3D = mColorSelect[0].rectTransform.anchoredPosition3D;
        mdata_select_color = 0;
        mColorSelect[2].gameObject.SetActive(false);
        for(float i = 0; i < 1f; i += 0.05f)
        {
            float p = Mathf.Sin(Mathf.PI * i) * 0.5f + 1;
            Vector3 scale = new Vector3(p, p, 1);
            mColorSelect[0].rectTransform.localScale = scale;
            mColorSelect[1].rectTransform.localScale = scale;
            mOK.image.rectTransform.anchoredPosition3D = Vector3.Lerp( new Vector3(218, -460, 0), new Vector3(218, -344, 0), i);


            yield return new WaitForSeconds(0.01f);
        }
        mColorSelect[0].rectTransform.localScale = Vector3.one;
        mColorSelect[1].rectTransform.localScale = Vector3.one;
        mColorSelect[2].gameObject.SetActive(true);
        mOK.image.rectTransform.anchoredPosition3D = new Vector3(218, -344, 0);
        mOK.transition = Selectable.Transition.None;


    }
    IEnumerator TEndGame()
    {
        ShapeLogicCtl.instance.GameNext();
        yield return new WaitForSeconds(1f);
        GameOverCtl.GetInstance().Show(3, GameOverReset);
        yield break;

        //Color bg_color0 = mBg.color;
        //Color bg_color1 = mBg.color;
        
        //bg_color1.a = 0;
        //for (float j = 0; j < 1f; j += 0.03f)
        //{
        //    mBg.color = Color.Lerp(bg_color0, bg_color1, j);

        //    Color cor = Color.Lerp(Color.white, new Color(1, 1, 1, 0), j);
        //    for(int i = 0; i < mBgTiao.Length; i++)
        //    {
        //        mBgTiao[i].color = cor;
        //    }
        //    yield return new WaitForSeconds(0.01f);
        //}


        //foreach (Transform t in transform)
        //{
        //    Destroy(t.gameObject);
        //}
        //gameObject.SetActive(false);

        //mBg = null;
        //mBgTiao = null;
        //mColorSelect = null;
        //mAnswerBg = null;
        //mOK = null;
        //mEffectOK = null;
        //mStations.Clear();
        //mdata_question_colors.Clear();
        //mdata_dic_question_color.Clear();


        yield break;
    }
    IEnumerator TOver()
    {
        //ShapeLogicCtl.instance.mSound.PlayShort("胜利通关音乐", 1);
        yield return new WaitForSeconds(1.5f);
        Vector3 size0 = mAnswerBg.rectTransform.sizeDelta;
        Vector3 size1 = size0;
        size1.y = 0;
        mColorSelect[2].gameObject.SetActive(false);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            Vector3 scale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
            mColorSelect[0].rectTransform.localScale = scale;
            mColorSelect[1].rectTransform.localScale = scale;
            mOK.image.rectTransform.anchoredPosition3D = Vector3.Lerp( new Vector3(218, -344, 0), new Vector3(218, -460, 0), i);
            mAnswerBg.rectTransform.sizeDelta = Vector2.Lerp(size0, size1, i);

            yield return new WaitForSeconds(0.01f);
        }
        mColorSelect[0].rectTransform.localScale = Vector3.zero;
        mColorSelect[1].rectTransform.localScale = Vector3.zero;
        mOK.image.rectTransform.anchoredPosition3D = new Vector3(218, -460, 0);
        mAnswerBg.rectTransform.sizeDelta = size1;

        mStations[mdata_answer_index].SetQuestionObj();
        for(int i = 0; i < mStations.Count; i++)
        {
            mStations[i].Show();
            mStations[i].Shake();
            mStations[i].SetOver();
        }

    }
    IEnumerator TGameOverReset()
    {
        Color bg_color0 = mBg.color;
        Color bg_color1 = mBg.color;

        ShapeLogicCtl.instance.Reset();

        bg_color1.a = 0;
        for (float j = 0; j < 1f; j += 0.03f)
        {
            mBg.color = Color.Lerp(bg_color0, bg_color1, j);

            Color cor = Color.Lerp(Color.white, new Color(1, 1, 1, 0), j);
            for (int i = 0; i < mBgTiao.Length; i++)
            {
                mBgTiao[i].color = cor;
            }
            yield return new WaitForSeconds(0.01f);
        }


        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        gameObject.SetActive(false);

        mBg = null;
        mBgTiao = null;
        mColorSelect = null;
        mAnswerBg = null;
        mOK = null;
        mEffectOK = null;
        mStations.Clear();
        mdata_question_colors.Clear();
        mdata_dic_question_color.Clear();

        gameObject.SetActive(false);

    }

    public void OnClkColor0()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        mColorSelect[2].rectTransform.anchoredPosition3D = mColorSelect[0].rectTransform.anchoredPosition3D;
        mdata_select_color = 0;
    }
    public void OnClkColor1()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        mColorSelect[2].rectTransform.anchoredPosition3D = mColorSelect[1].rectTransform.anchoredPosition3D;
        mdata_select_color = 1;
    }
    public void OnClkOK()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("button_down");
        mOK.image.sprite = ResManager.GetSprite("shapelogic_sprite", "btn_down");
        mOK.enabled = false;
        bool correct = true;
        for(int i = 0; i < mStations[mdata_answer_index].mdata_colors.Count; i++)
        {
            //Debug.Log(mStations[mdata_answer_index].mdata_colors[i] + "  " + mdata_question_colors[mdata_answer_index][i]);
            if(mStations[mdata_answer_index].mdata_colors[i] != mdata_question_colors[mdata_answer_index][i])
            {
                correct = false;
                break;
            }
        }
        if(correct)
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
            Debug.Log("回答错误!");
            Invoke("InvokeOnClkOk", 0.5f);
            mStations[mdata_answer_index].PlayError();
        }
    }
    void InvokeOnClkOk()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("button_up");
        mOK.image.sprite = ResManager.GetSprite("shapelogic_sprite", "btn_up");
        mOK.enabled = true;

    }

    int temp_over_count = 0;
    public void callback_StationOver()
    {
        temp_over_count++;
        if(8 == temp_over_count)
        {
            EndGame();
        }
    }

}
