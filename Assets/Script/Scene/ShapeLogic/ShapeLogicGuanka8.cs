using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka8 : MonoBehaviour
{
    Button mOK;
    Image mBg0, mBg1;
    Image mBgAnswer;
    RectTransform mRtranLeft, mRtranMid, mRtranDown;
    Image mSelectLeft, mSelectMid, mSelectDown;
    ParticleSystem mEffectOK;


    //Image[] mBgTiao { get; set; }

    List<ShapeLogicGuanka8_Station> mStation;

    int temp_updata_state = 0;
    int mdata_question_index = 0;
    int mdata_current_select_left = -1;
    int mdata_current_select_mid = -1;
    int mdata_current_select_down = -1;

    // Use this for initialization
    void Start () {
        //int[,] s = Common.GetArrayMuteId(8);
        //string msg = "";
        //for(int i = 0; i < 8; i++)
        //{
        //    for(int j = 0; j < 8; j++)
        //    {
        //        msg += s[i, j].ToString() + ", ";
        //    }
        //    msg += "\n";
        //}
        //Debug.Log(msg);
	}

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
                ShapeLogicGuanka8_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka8_Station>();
                if (null != com)
                {
                    com.SetBoxEnable(false);
                    com.ShootOver();
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
    void CreateStationMid(int type, Transform mRootMid)
    {
        Image[] mMid = new Image[6];
        //mMid[0] = UguiMaker.newImage("mMid0", mRootMid, "shapelogic_sprite", "guanka8_station0-0", false);
        mMid[1] = UguiMaker.newImage("mMid1", mRootMid, "shapelogic_sprite", "guanka8_station0-0", false);
        mMid[2] = UguiMaker.newImage("mMid2", mRootMid, "shapelogic_sprite", "guanka8_station0-0", false);
        mMid[3] = UguiMaker.newImage("mMid3", mRootMid, "shapelogic_sprite", "guanka8_station0-0", false);
        mMid[4] = UguiMaker.newImage("mMid4", mRootMid, "shapelogic_sprite", "guanka8_station0-0", false);
        mMid[5] = UguiMaker.newImage("mMid5", mRootMid, "shapelogic_sprite", "guanka8_station0-0", false);
        switch (type)
        {
            case 0:
                mMid[1].gameObject.SetActive(true);
                mMid[1].sprite = ResManager.GetSprite("shapelogic_sprite", "guanka8_station3-1");
                mMid[1].type = Image.Type.Sliced;
                mMid[1].color = Color.white;
                mMid[1].rectTransform.anchoredPosition = new Vector2(-73.4f, -44f);
                mMid[1].rectTransform.sizeDelta = new Vector2(68.6f, 55f);
                for (int i = 2; i < 6; i++)
                {
                    mMid[i].gameObject.SetActive(true);
                    mMid[i].sprite = ResManager.GetSprite("public", "white");
                    mMid[i].type = Image.Type.Sliced;
                    mMid[i].color = new Color32(116, 25, 174, 255);
                }
                mMid[2].rectTransform.sizeDelta = new Vector2(4, 140f);
                mMid[3].rectTransform.sizeDelta = new Vector2(4, 140f);
                mMid[2].rectTransform.anchoredPosition = new Vector2(-40, 0);
                mMid[3].rectTransform.anchoredPosition = new Vector2(40, 0);
                mMid[4].rectTransform.sizeDelta = new Vector2(65.56f, 4);
                mMid[5].rectTransform.sizeDelta = new Vector2(65.56f, 4);
                mMid[4].rectTransform.anchoredPosition = new Vector2(73, -17);
                mMid[5].rectTransform.anchoredPosition = new Vector2(-73, -17);

                break;
            case 1:
                mMid[1].gameObject.SetActive(true);
                mMid[1].sprite = ResManager.GetSprite("shapelogic_sprite", "guanka8_station3-0");
                mMid[1].type = Image.Type.Simple;
                mMid[1].color = Color.white;
                mMid[1].SetNativeSize();
                mMid[1].rectTransform.anchoredPosition = new Vector2(-0, -28.2f);

                for (int i = 2; i < 6; i++)
                {
                    mMid[i].gameObject.SetActive(false);

                }
                break;
            case 2:
                for (int i = 1; i < 5; i++)
                {
                    mMid[i].gameObject.SetActive(true);
                    mMid[i].sprite = ResManager.GetSprite("public", "white");
                    mMid[i].type = Image.Type.Sliced;
                    mMid[i].color = new Color32(116, 25, 174, 255);
                }
                mMid[5].gameObject.SetActive(false);
                mMid[1].rectTransform.sizeDelta = new Vector2(210, 4);
                mMid[2].rectTransform.sizeDelta = new Vector2(210, 4);
                mMid[1].rectTransform.anchoredPosition = new Vector2(0, 30);
                mMid[2].rectTransform.anchoredPosition = new Vector2(0, -30);
                mMid[3].rectTransform.sizeDelta = new Vector2(4, 60);
                mMid[4].rectTransform.sizeDelta = new Vector2(4, 60);
                mMid[3].rectTransform.anchoredPosition = new Vector2(-40, 0);
                mMid[4].rectTransform.anchoredPosition = new Vector2(40, 0);

                break;
        }
    }
    void CreateStationDown(int type, Transform mRootDown)
    {
        Image[] mDown = new Image[4];
        mDown[0] = UguiMaker.newImage("mDown0", mRootDown, "shapelogic_sprite", "guanka8_station1-0", false);
        mDown[1] = UguiMaker.newImage("mDown1", mRootDown, "shapelogic_sprite", "guanka8_station1-0", false);
        mDown[2] = UguiMaker.newImage("mDown2", mRootDown, "shapelogic_sprite", "guanka8_station1-0", false);
        mDown[3] = UguiMaker.newImage("mDown3", mRootDown, "shapelogic_sprite", "guanka8_station1-0", false);
        List<Vector2> poss = new List<Vector2>();
        switch (type)
        {
            case 0:
                poss.Add(new Vector2(-27, 0));
                poss.Add(new Vector2(27, 0));
                break;
            case 1:
                poss.Add(new Vector2(-54, 0));
                poss.Add(new Vector2(0, 0));
                poss.Add(new Vector2(54, 0));
                break;
            case 2:
                poss.Add(new Vector2(-27, 0));
                poss.Add(new Vector2(27, 0));
                poss.Add(new Vector2(-81, 0));
                poss.Add(new Vector2(81, 0));
                break;
        }
        for (int i = 0; i < 4; i++)
        {
            if (i < poss.Count)
            {
                mDown[i].gameObject.SetActive(true);
                mDown[i].rectTransform.anchoredPosition = poss[i];

            }
            else
            {
                mDown[i].gameObject.SetActive(false);
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
        mdata_question_index = 0;
        mdata_current_select_left = -1;
        mdata_current_select_mid = -1;
        mdata_current_select_down = -1;

        //背景
        mBg0 = UguiMaker.newImage("mBg0", transform, "public", "white", false);
        mBg0.color = new Color32(255, 160, 207, 255);
        mBg0.rectTransform.sizeDelta = new Vector2(1423, 800);

        //ShapeLogicCtl.instance.CreateLine(890, mBg0.transform, new List<Vector2>() { new Vector2(116.8f, 82.4f), new Vector2(116.8f, -164) });

        //List<int> temp_left_id = new List<int>() { 0, 1, 2 };
        //List<int> temp_mid_id = new List<int>() { 0, 1, 2 };
        //List<int> temp_down_id = new List<int>() { 0, 1, 2 };
        int[,] temp_left_id = Common.GetArrayMuteId(3);
        int[,] temp_mid_id = Common.GetArrayMuteId(3);
        int[,] temp_down_id = Common.GetArrayMuteId(3);

        List<Vector3> poss = Common.PosSortByWidth(900, 3, 230);
        List<int> init_station = new List<int>() { 6, 7, 8, 3, 4, 5, 1, 2, 0 };
        List<float> delays = new List<float>() { 0, 0.2f, 0.4f, 0, 0.2f, 0.4f, 0, 0.2f, 2f };
        mStation = new List<ShapeLogicGuanka8_Station>();


        ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("car_run");
        for(int i = 0; i < init_station.Count; i++)
        {
            int index = init_station[i] % 3;
            int x = init_station[i] / 3;
            int y = init_station[i] % 3;
            //if (0 == i % 3)
            //{
            //    temp_left_id = Common.BreakRank(temp_left_id);
            //    temp_mid_id = Common.BreakRank(temp_mid_id);
            //    temp_down_id = Common.BreakRank(temp_down_id);
            //}
            if(0 == init_station[i])
            {
                mStation.Add(UguiMaker.newGameObject("mStation" + init_station[i].ToString(), transform).AddComponent<ShapeLogicGuanka8_Station>());
                mStation[i].Init(temp_left_id[x, y], temp_mid_id[x, y], temp_down_id[x, y]);
                //mStation[i].Run();
                Vector3 pos = poss[index] + new Vector3(150, init_station[i] / 3 * -248, 0);
                mStation[i].mRtran.anchoredPosition3D = pos;// + new Vector3(1000, 0, 0);
                //mStation[i].GotoPos(pos);
                mStation[i].ShowQuestionBg(delays[i]);
                mdata_question_index = i;
            }
            else
            {
                mStation.Add(UguiMaker.newGameObject("mStation" + init_station[i].ToString(), transform).AddComponent<ShapeLogicGuanka8_Station>());
                mStation[i].Init(temp_left_id[x, y], temp_mid_id[x, y], temp_down_id[x, y]);
                mStation[i].Run();
                Vector3 pos = poss[index] + new Vector3(150, init_station[i] / 3 * -248, 0);
                mStation[i].mRtran.anchoredPosition3D = pos + new Vector3(1000, 0, 0);
                mStation[i].GotoPos(pos, delays[i]);

            }
            //yield return new WaitForSeconds(0.2f);

        }

        yield return new WaitForSeconds(2f);

        //答案选择生成
        mRtranLeft = UguiMaker.newGameObject("mRtranLeft", transform).GetComponent<RectTransform>();
        mRtranMid = UguiMaker.newGameObject("mRtranMid", transform).GetComponent<RectTransform>();
        mRtranDown = UguiMaker.newGameObject("mRtranDown", transform).GetComponent<RectTransform>();

        Button btn_left0 = UguiMaker.newButton("left0", mRtranLeft, "shapelogic_sprite", "guanka8_station0-0");
        Button btn_left1 = UguiMaker.newButton("left1", mRtranLeft, "shapelogic_sprite", "guanka8_station0-1");
        Button btn_left2 = UguiMaker.newButton("left2", mRtranLeft, "shapelogic_sprite", "guanka8_station0-2");
        btn_left0.onClick.AddListener(OnClkLeft0);
        btn_left1.onClick.AddListener(OnClkLeft1);
        btn_left2.onClick.AddListener(OnClkLeft2);
        btn_left0.image.rectTransform.anchoredPosition = new Vector2(-100, 0);
        btn_left1.image.rectTransform.anchoredPosition = new Vector2(0, 0);
        btn_left2.image.rectTransform.anchoredPosition = new Vector2(100, 0);
        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        for(float i = 0; i < 1f; i += 0.05f)
        {
            mRtranLeft.anchoredPosition = Vector2.Lerp(new Vector2(-850, 300), new Vector2(-490, 300), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

        Button btn_mid0 = UguiMaker.newButton("mid0", mRtranMid, "shapelogic_sprite", "guanka8_station2-0");
        Button btn_mid1 = UguiMaker.newButton("mid1", mRtranMid, "shapelogic_sprite", "guanka8_station2-0");
        Button btn_mid2 = UguiMaker.newButton("mid2", mRtranMid, "shapelogic_sprite", "guanka8_station2-0");
        btn_mid0.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        btn_mid1.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        btn_mid2.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        btn_mid0.onClick.AddListener(OnClkMid0);
        btn_mid1.onClick.AddListener(OnClkMid1);
        btn_mid2.onClick.AddListener(OnClkMid2);
        btn_mid0.image.rectTransform.anchoredPosition = new Vector2(0, 130);
        btn_mid1.image.rectTransform.anchoredPosition = new Vector2(0, 0);
        btn_mid2.image.rectTransform.anchoredPosition = new Vector2(0, -130);
        btn_mid0.image.type = Image.Type.Sliced;
        btn_mid1.image.type = Image.Type.Sliced;
        btn_mid2.image.type = Image.Type.Sliced;
        btn_mid0.image.rectTransform.sizeDelta = new Vector2(216, 146);
        btn_mid1.image.rectTransform.sizeDelta = new Vector2(216, 146);
        btn_mid2.image.rectTransform.sizeDelta = new Vector2(216, 146);
        CreateStationMid(0, btn_mid0.image.rectTransform);
        CreateStationMid(1, btn_mid1.image.rectTransform);
        CreateStationMid(2, btn_mid2.image.rectTransform);
        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtranMid.anchoredPosition = Vector2.Lerp(new Vector2(-850, 62), new Vector2(-490, 62), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

        Button btn_down0 = UguiMaker.newButton("down0", mRtranDown, "shapelogic_sprite", "guanka8_station2-0");
        Button btn_down1 = UguiMaker.newButton("down1", mRtranDown, "shapelogic_sprite", "guanka8_station2-0");
        Button btn_down2 = UguiMaker.newButton("down2", mRtranDown, "shapelogic_sprite", "guanka8_station2-0");
        btn_down0.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        btn_down1.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        btn_down2.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        btn_down0.onClick.AddListener(OnClkDown0);
        btn_down1.onClick.AddListener(OnClkDown1);
        btn_down2.onClick.AddListener(OnClkDown2);
        btn_down0.image.color = new Color(1, 1, 1,  0);
        btn_down1.image.color = new Color(1, 1, 1, 0);
        btn_down2.image.color = new Color(1, 1, 1, 0);
        btn_down0.image.rectTransform.anchoredPosition = new Vector2(0, 55);
        btn_down1.image.rectTransform.anchoredPosition = new Vector2(0, 0);
        btn_down2.image.rectTransform.anchoredPosition = new Vector2(0, -55);
        btn_down0.image.type = Image.Type.Sliced;
        btn_down1.image.type = Image.Type.Sliced;
        btn_down2.image.type = Image.Type.Sliced;
        btn_down0.image.rectTransform.sizeDelta = new Vector2(216, 55);
        btn_down1.image.rectTransform.sizeDelta = new Vector2(216, 55);
        btn_down2.image.rectTransform.sizeDelta = new Vector2(216, 55);
        CreateStationDown(0, btn_down0.image.rectTransform);
        CreateStationDown(1, btn_down1.image.rectTransform);
        CreateStationDown(2, btn_down2.image.rectTransform);
        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtranDown.anchoredPosition = Vector2.Lerp(new Vector2(-850, -223.2f), new Vector2(-490, -223.2f), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

        mSelectLeft = UguiMaker.newImage("mSelectLeft", mRtranLeft, "shapelogic_sprite", "guanka8_select", false);
        mSelectMid = UguiMaker.newImage("mSelectMid", mRtranMid, "shapelogic_sprite", "guanka8_select", false);
        mSelectDown = UguiMaker.newImage("mSelectDown", mRtranDown, "shapelogic_sprite", "guanka8_select", false);
        mSelectLeft.type = Image.Type.Sliced;
        mSelectMid.type = Image.Type.Sliced;
        mSelectDown.type = Image.Type.Sliced;
        mSelectLeft.gameObject.SetActive(false);
        mSelectMid.gameObject.SetActive(false);
        mSelectDown.gameObject.SetActive(false);

        mOK = UguiMaker.newButton("mOK", transform, "shapelogic_sprite", "btn_up");
        mOK.onClick.AddListener(OnClkOK);
        mOK.transition = Selectable.Transition.None;
        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        for (float i = 0; i < 1f; i += 0.08f)
        {
            mOK.image.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(-490f, -460), new Vector2(-490, -350), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mOK.image.rectTransform.anchoredPosition = new Vector2(-490, -350);


        ShapeLogicCtl.instance.mSound.PlayTipListDefaultAb(
            new List<string>() { "8观察小推车的特征和变化规律", "选择合适的小图案组成正确的大图案吧" },
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
            //for (int i = 0; i < mBgTiao.Length; i++)
            //{
            //    mBgTiao[i].color = cor;
            //}
            mBg0.color = Color.Lerp(bg_color0, bg_color1, j);
            yield return new WaitForSeconds(0.01f);
        }

        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        mOK = null;
        mBg0 = null;
        mBg1 = null;
        mBgAnswer = null;
        mRtranLeft = null;
        mRtranMid = null;
        mRtranDown = null;
        mSelectLeft = null;
        mSelectMid = null;
        mSelectDown = null;
        mEffectOK = null;

        gameObject.SetActive(false);
    }
    IEnumerator TOver()
    {
        //ShapeLogicCtl.instance.DestroyLine();
        yield return new WaitForSeconds(0.8f);
        ShapeLogicCtl.instance.mSound.PlayShort("胜利通关音乐", 1);
        temp_updata_state = 1;
        for(int i = 0; i < mStation.Count; i++)
        {
            mStation[i].SetBoxEnable(true);
            mStation[i].Run();
        }
        mStation[mdata_question_index].HideQuestionBg();

        Vector3 pos_left0 = mRtranLeft.anchoredPosition3D;
        Vector3 pos_mid0 = mRtranMid.anchoredPosition3D;
        Vector3 pos_down0 = mRtranDown.anchoredPosition3D;
        Vector3 pos_left1 = pos_left0 + new Vector3(-600, 0, 0);
        Vector3 pos_mid1 = pos_mid0 + new Vector3(-600, 0, 0);
        Vector3 pos_down1 = pos_down0 + new Vector3(-600, 0, 0);

        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtranLeft.anchoredPosition = Vector2.Lerp(pos_left0, pos_left1, Mathf.Sin(Mathf.PI * 0.5f * i));
            mRtranMid.anchoredPosition = Vector2.Lerp(pos_mid0, pos_mid1, Mathf.Sin(Mathf.PI * 0.5f * i));
            mRtranDown.anchoredPosition = Vector2.Lerp(pos_down0, pos_down1, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1f);

        for (float i = 0; i < 1f; i += 0.08f)
        {
            mOK.image.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(-490, -350), new Vector2(-490f, -460), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

    }

    public void OnClkOK()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("button_down");
        mOK.image.sprite = ResManager.GetSprite("shapelogic_sprite", "btn_down");
        mOK.enabled = false;

        bool correct = true;
        if (mdata_current_select_left != mStation[mdata_question_index].mdata_left)
            correct = false;
        if (mdata_current_select_mid != mStation[mdata_question_index].mdata_mid)
            correct = false;
        if (mdata_current_select_down != mStation[mdata_question_index].mdata_down)
            correct = false;

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
            Debug.Log("回答错误!");
            Invoke("InvokeOnClkOk", 0.5f);
            mStation[mdata_question_index].PlayError();
        }
    }
    void InvokeOnClkOk()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("button_up");
        mOK.image.sprite = ResManager.GetSprite("shapelogic_sprite", "btn_up");
        mOK.enabled = true;

    }

    public void OnClkLeft0()
    {
        OnClkLeft(0);
    }
    public void OnClkLeft1()
    {
        OnClkLeft(1);
    }
    public void OnClkLeft2()
    {
        OnClkLeft(2);
    }
    public void OnClkLeft(int type)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        if (type == mdata_current_select_left)
        {
            return;
            mdata_current_select_left = -1;
            mSelectLeft.gameObject.SetActive(false);
        }
        else
        {
            mSelectLeft.gameObject.SetActive(true);
            mdata_current_select_left = type;
            mStation[mdata_question_index].ShowLeft(type);
        }

        switch(type)
        {
            case 0:
                mSelectLeft.rectTransform.anchoredPosition = new Vector2(-96.03f, 2.4f);
                mSelectLeft.rectTransform.sizeDelta = new Vector2(111.1f, 94.5f);

                break;

            case 1:
                mSelectLeft.rectTransform.anchoredPosition = new Vector2(3.6f, 2.4f);
                mSelectLeft.rectTransform.sizeDelta = new Vector2(111.1f, 94.5f);
                break;

            case 2:
                mSelectLeft.rectTransform.anchoredPosition = new Vector2(100.3f, 2.4f);
                mSelectLeft.rectTransform.sizeDelta = new Vector2(111.1f, 94.5f);
                break;
        }
    }


    public void OnClkMid0()
    {
        OnClkMid(0);

    }
    public void OnClkMid1()
    {
        OnClkMid(1);

    }
    public void OnClkMid2()
    {
        OnClkMid(2);
    }
    public void OnClkMid(int type)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        if (type == mdata_current_select_mid)
        {
            return;
            mdata_current_select_mid = -1;
            mSelectMid.gameObject.SetActive(false);
        }
        else
        {
            mSelectMid.gameObject.SetActive(true);
            mdata_current_select_mid = type;
            mStation[mdata_question_index].ShowMid(type);
        }

        mSelectMid.rectTransform.sizeDelta = new Vector2(222.2f, 167.3f);
        switch (type)
        {
            case 0:
                mSelectMid.rectTransform.anchoredPosition = new Vector2(0, 130.82f);

                break;

            case 1:
                mSelectMid.rectTransform.anchoredPosition = new Vector2(0, 0);
                break;

            case 2:
                mSelectMid.rectTransform.anchoredPosition = new Vector2(0, -130.82f);
                break;
        }

    }

    public void OnClkDown0()
    {
        OnClkDown(0);

    }
    public void OnClkDown1()
    {
        OnClkDown(1);

    }
    public void OnClkDown2()
    {
        OnClkDown(2);
    }
    public void OnClkDown(int type)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        if (type == mdata_current_select_down)
        {
            return;
            mdata_current_select_down = -1;
            mSelectDown.gameObject.SetActive(false);
        }
        else
        {
            mSelectDown.gameObject.SetActive(true);
            mdata_current_select_down = type;
            mStation[mdata_question_index].ShowDown(type);
        }

        switch (type)
        {
            case 0:
                mSelectDown.rectTransform.anchoredPosition = new Vector2(0, 55f);
                mSelectDown.rectTransform.sizeDelta = new Vector2(140.2f, 90.2f);

                break;

            case 1:
                mSelectDown.rectTransform.anchoredPosition = new Vector2(0, 0.8f);
                mSelectDown.rectTransform.sizeDelta = new Vector2(179.4f, 90.2f);
                break;

            case 2:
                mSelectDown.rectTransform.anchoredPosition = new Vector2(0, -53.73f);
                mSelectDown.rectTransform.sizeDelta = new Vector2(221.21f, 90.2f);
                break;
        }

    }
}
