using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TvRoomCtl : BaseScene {
    public static TvRoomCtl instance = null;

    public TvRoomControl mControl { get; set; }
    public List<TvRoomTv> mTvs { get; set; } 
    //public TvRoomForm mForm { get; set; }
    public TvRoomData mGuanka { get; set; }
    public SoundManager mSound { get; set; }

    bool isFirstTime = true;

    void Awake()
    {
        instance = this;
        mGuanka = new TvRoomData();
    }
    void OnDestroy()
    {
        if (this == instance)
            instance = null;
    }
	void Start () {
        mSound = gameObject.AddComponent<SoundManager>();
        //mSound.PlayShort("tvroom_sound", "game-name2-2");

        StartCoroutine(TStart(1));
        CallLoadFinishEvent();

    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            //Callback_NextGuanka();
            StartCoroutine("TOver");
        }
    }
    IEnumerator TStart(int guanka)
    {

        TopTitleCtl.instance.Reset();
        mGuanka.Set(guanka);
        UguiMaker.newRawImage("bg", transform, "tvroom_texture", "bg", false).GetComponent<RectTransform>().sizeDelta = new Vector2(1423, 800);
        UguiMaker.newGameObjects(transform, "tv", "control", "form");

        Image light0 = UguiMaker.newImage("light", transform, "tvroom_sprite", "control_light", false);
        Image light1 = UguiMaker.newImage("light", transform, "tvroom_sprite", "control_light", false);
        light0.rectTransform.anchoredPosition = new Vector2(-484, 214);
        light1.rectTransform.anchoredPosition = new Vector2(484, 214);
        light0.rectTransform.localEulerAngles = new Vector3(0, 0, 34);
        light1.rectTransform.localEulerAngles = new Vector3(0, 0, -34);

        yield return new WaitForSeconds(0.5f);

        //控制版
        mControl = transform.Find("control").gameObject.AddComponent<TvRoomControl>();
        mControl.Init();
        mControl.ResetData();
        for (float i = 0; i < 1f; i += 0.04f)
        {
            mControl.mRtran.anchoredPosition = Vector2.Lerp(
                new Vector2(0, -400 - 124), new Vector2(0, -400), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

        //电视
        if (null == mTvs)
        {
            mTvs = new List<TvRoomTv>();
            Vector2[] reset_pos = new Vector2[] { new Vector2(-235, 186), new Vector2(235, 186), new Vector2(-235, -119.3f), new Vector2(235, -119.3f) };

            for (int i = 0; i < 4; i++)
            {
                mTvs.Add(UguiMaker.newGameObject(i.ToString(), transform.Find("tv")).AddComponent<TvRoomTv>());
                mTvs[i].Init(i);
                mTvs[i].mResetPos = reset_pos[i];
            }
        }
        for (int i = 0; i < 4; i++)
            mTvs[i].gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            mTvs[i].gameObject.SetActive(true);
            mTvs[i].SetData(mGuanka);
            if (i == 1)
                yield return new WaitForSeconds(0.2f);
        }

        mControl.PushInBg1();



        yield return new WaitForSeconds(1);
        mSound.PlayTip("tvroom_sound", "game-tips2-2-1", 1, true);
        yield return new WaitForSeconds(2);
        if(isFirstTime)
        {
            SoundManager.instance.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.05f);
            isFirstTime = false;
        }

    }
    

    public void RePlay()
    {
        TopTitleCtl.instance.Reset();
        //mForm.Reset();

        mGuanka.Set(1);
        mControl.PushInStop();
        MoveRandom();

    }
    public void MoveRandom()
    {
        mSound.PlayBg("tvroom_sound", "03-随机");
        for (int i = 0; i < mTvs.Count; i++)
        {
            mTvs[i].Move();
        }

    }
    public bool Callback_ClickOK()
    {
        bool match = true;
        for(int type = 1; type <= 5; type++)
        {
            bool is_select = mControl.GetTypeIsSelect(type);

            bool temp_is_same = true;//假设全部类型都一样
            int temp_type_kind = -1;//类型是属于那种种类
            for(int j = 0; j < mTvs.Count; j++)
            {
                int type_kind = mTvs[j].GetTypeKind(type);
                if (-1 == temp_type_kind)
                {
                    temp_type_kind = type_kind;
                }
                else
                {
                    if(temp_type_kind != type_kind)
                    {
                        temp_is_same = false;
                        break;
                    }
                }
            }
            if(is_select != temp_is_same)
            {
                //Debug.LogError("type=" + type + " is_select=" + is_select + " );
                match = false;
                break;
            }
        }

        if (match)
        {
            StartCoroutine( TMatchGame());
        }
        else
        {
            for(int i = 0; i < mTvs.Count; i++)
            {
                mTvs[i].PlayError();
            }
            //Debug.LogError("配对失败");
        }

        return match;
    }
    public void Callback_ClickStop()
    {
        for (int i = 0; i < mTvs.Count; i++)
        {
            mTvs[i].StopMove();
        }
        mSound.StopBg();
        
        TvRoomCtl.instance.mSound.PlayShort("tvroom_sound", "01-按钮按下");
        TvRoomCtl.instance.mSound.PlayShort("tvroom_sound", "05-点停止");

        mSound.PlayShort("tvroom_sound", "game-tips2-2-1");

    }
    public void Callback_NextGuanka()
    {
        //TopTitleCtl.instance.AddStar();
        if(mGuanka.guanka == 5)
        {
        }
        else
        {
            mGuanka.Set(mGuanka.guanka + 1);
            mControl.PushInStop();
            MoveRandom();
        }

    }


    int kbady_tall_over = 0;
    Button btnCancelKbady;
    public void OnClkCancelKbady()
    {
        btnCancelKbady.GetComponent<Image>().sprite = ResManager.GetSprite("public", "btn_skip_down");
        SoundManager.instance.PlayShort("button_down");
        Invoke("InvokeOnClkCancelKbady1", 1);
    }
    public void InvokeOnClkCancelKbady1()
    {
        KbadyCtl.instance.HideSpine();
        kbady_tall_over = 2;
        mSound.StopOnly();

        SoundManager.instance.PlayShort("button_up");
        btnCancelKbady.GetComponent<Image>().sprite = ResManager.GetSprite("public", "btn_skip_up");
        Invoke("InvokeOnClkCancelKbady2", 1);
    }
    public void InvokeOnClkCancelKbady2()
    {
        if (null != btnCancelKbady)
        {
            btnCancelKbady.gameObject.SetActive(false);

        }
    }
    IEnumerator TOver()
    {

        KbadyCtl.Init();
        RectTransform mask = KbadyCtl.instance.BgEffect1_Create(new Color32(0, 168, 191, 255));
        //mForm.mForm.transform.SetParent(KbadyCtl.instance.transform);

        //出现背景
        KbadyCtl.instance.BgEffect1_Play();
        yield return new WaitForSeconds(1);


        //创建跳过按钮
        btnCancelKbady = UguiMaker.newButton("mBtnCancelKbady", KbadyCtl.instance.transform, "public", "btn_skip_up");
        btnCancelKbady.onClick.AddListener(OnClkCancelKbady);
        //btnCancelKbady.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(GlobalParam.screen_width * -0.5f + 100, GlobalParam.screen_height * 0.5f - 200, 0);
        btnCancelKbady.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(GlobalParam.screen_width * 0.5f - 82, 256, 0);
        btnCancelKbady.transform.localScale = new Vector3(1, 1, 1);

        //出现K宝
        KbadyCtl.instance.mRtranSpine.anchoredPosition = new Vector2(-285, -45);// Vector2.zero;// new Vector2(-346, -252);
        KbadyCtl.instance.ShowSpine(new Vector3(0.6f, 0.6f));
        //KbadyCtl.instance.PlaySpine(kbady_enum.Encourage_1, true);
        KbadyCtl.instance.PlaySpineEncourage(true);

        //mSound.PlaySoundList(new List<string>() { "tvroom_sound"}, new List<string>() { "game-tips2-2-7" }, null);// ("tvroom_sound", "game-tips2-2-7");
        mSound.PlayOnly("tvroom_sound", "game-tips2-2-7");

        StartCoroutine("TShowOverHead");
        
        kbady_tall_over = 0;
        while (0 == kbady_tall_over)
        {
            yield return new WaitForSeconds(0.5f);
        }

        StopCoroutine("TShowOverHead");
        if (null != RootShowHead)
        {
            for(float i = 0; i < 1f; i += 0.08f)
            {
                RootShowHead.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
                yield return new WaitForSeconds(0.01f);
            }
            Destroy(RootShowHead.gameObject);
        }

        KbadyCtl.instance.BgEffect1_Stop();
        yield return new WaitForSeconds(1.5f);
        GameOverCtl.GetInstance().Show(5, RePlay);
        
        if(null != btnCancelKbady)
            Destroy(btnCancelKbady.gameObject);



        yield return new WaitForSeconds(1.5f);


    }
    IEnumerator TMatchGame()
    {
        yield return new WaitForSeconds(2f);
        TopTitleCtl.instance.AddStar();
        mControl.PushOutBg1();
        //mForm.Show(true);
        //Debug.LogError("配对成功");
        if (mGuanka.guanka == 5)
        {
            StartCoroutine(TOver());
        }
        else
        {
            yield return new WaitForSeconds(1f);
            TvRoomCtl.instance.Callback_NextGuanka();
        }

    }

    GameObject RootShowHead = null;
    IEnumerator TShowOverHead()
    {
        //展示动物头像
        Image[] head = new Image[6];
        RootShowHead = UguiMaker.newGameObject("RootShowHead", KbadyCtl.instance.transform);
        RootShowHead.transform.localPosition = new Vector3(275, -33.8f, 0);
        List<int> animal_ids = Common.GetMutexValue(0, 13, 2);
        for (int i = 0; i < 6; i++)
        {
            Image image = UguiMaker.newImage(i.ToString(), RootShowHead.transform, "tvroom_sprite", "animal" + animal_ids[0].ToString(), false);
            head[i] = image;
        }
        //颜色
        List<Vector3> poss0 = Common.PosSortByWidth(600, 6, 0);
        for (int i = 0; i < 6; i++)
        {
            head[i].rectTransform.anchoredPosition3D = poss0[i];
        }
        yield return new WaitForSeconds(3);
        //队形
        List<Vector3> poss1 = new List<Vector3>(){
            new Vector3( -5f, 125f, 0f),
            new Vector3( -3f, 16f, 0f),
            new Vector3( -67f, -82f, 0f),
            new Vector3( -179f, -123f, 0f),
            new Vector3( 82f, -72f, 0f),
            new Vector3( 185f, -130f, 0f),
        };
        for (float i = 0; i < 1f; i += 0.02f)
        {
            for (int j = 0; j < 6; j++)
            {
                head[j].rectTransform.anchoredPosition3D = Vector3.Lerp(poss0[j], poss1[j], i);
            }
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(2);
        //种类
        for (float i = 0; i < 1f; i += 0.05f)
        {
            for (int j = 0; j < 6; j++)
            {
                head[j].color = Color32.Lerp(Color.white, new Color(1, 1, 1, 0), i);
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int j = 0; j < 6; j++)
        {
            head[j].sprite = ResManager.GetSprite("tvroom_sprite", "animal" + animal_ids[1].ToString());
        }
        for (float i = 0; i < 1f; i += 0.05f)
        {
            for (int j = 0; j < 6; j++)
            {
                head[j].color = Color32.Lerp(new Color(1, 1, 1, 0), Color.white, i);
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int j = 0; j < 6; j++)
        {
            head[j].color = Color.white;
        }
        yield return new WaitForSeconds(2);
        //大小
        for (float i = 0; i < 1f; i += 0.04f)
        {
            RootShowHead.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.6f, 0.6f, 1), i);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(2);
        //复原
        for (float i = 0; i < 1f; i += 0.05f)
        {
            for (int j = 0; j < 6; j++)
            {
                head[j].rectTransform.anchoredPosition3D = Vector3.Lerp(poss1[j], poss0[j], i);
            }
            RootShowHead.transform.localScale = Vector3.Lerp(new Vector3(0.6f, 0.6f, 1), Vector3.one, i);
            yield return new WaitForSeconds(0.01f);

        }
        for (int j = 0; j < 6; j++)
        {
            head[j].rectTransform.anchoredPosition3D = poss0[j];
        }


    }

}
