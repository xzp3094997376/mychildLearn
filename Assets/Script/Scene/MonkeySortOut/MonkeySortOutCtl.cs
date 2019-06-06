using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class MonkeySortOutCtl : BaseScene {
    public static MonkeySortOutCtl instance = null;

    public Guanka mGuanka { get; set; }
    public RectTransform mback { get; set; }
    public RectTransform mfont { get; set; }
    //public RectTransform mreward { get; set; }

    Image mbg1, mbg2;//mbg0,
    ParticleSystem mBanana { get; set; }
    ParticleSystem mReward { get; set; }

    List<MonkeySortOutMonkey> mMkys = new List<MonkeySortOutMonkey>();
    //List<Image> mRewards = new List<Image>();
    MonkeySortOutCar mCarLeft { get; set; }
    MonkeySortOutCar mCarRight { get; set; }
    //GridLayoutGroup mLayout { get; set; }
    public SoundManager mSoundMgr { get; set; }

    //0-大小，1-颜色，2-帽子，3-正面，4-尾巴，5-马甲，6-坐着，7-桃子, 8-嘴巴
    bool[] m_have_reward = new bool[] { false, false, false, false, false, false, false, false, false };
    public Vector2 m_game_scene_offset = new Vector2(0, -60);
    public int m_temp_sort_type = -1;//当前是按什么分组的

    void OnDestroy()
    {
        if (instance == this)
            instance = null;

        if (null != Camera.main)
            Camera.main.clearFlags = CameraClearFlags.Depth;

        TopTitleCtl.instance.ResetSetStartLayout();

    }
    void Awake()
    {
        instance = this;
        mGuanka = new Guanka();
        mSoundMgr = gameObject.AddComponent<SoundManager>();

        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.backgroundColor = new Color32(20, 92, 97, 255);



    }
    void Start()
    {
        mSceneType = SceneEnum.MonkeySortOut;
        CallLoadFinishEvent();

        StartCoroutine(TStart());

        


    }
    IEnumerator TStart()
    {

        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        mbg1 = UguiMaker.newImage("mbg1", transform, "monkeysortout_sprite", "bg1");
        UguiMaker.setRTran(mbg1.rectTransform, UguiAnchorEnum.up);
        mbg1.rectTransform.pivot = new Vector2(0.5f, 1);
        mbg1.rectTransform.anchoredPosition = Vector2.zero;

        yield return new WaitForSeconds(0.2f);
        SoundManager.instance.PlayTipList(new List<string>() { "monkeysortout_sound" }, new List<string>() { "game_start_tip" }, false);

        TopTitleCtl.instance.SetSpriteData("monkeysortout_sprite", "reward_bing", "reward_bing", "reward0");
        TopTitleCtl.instance.mSoundTipData.SetData(SoundManager.instance, "monkeysortout_sound", "game_start_tip");
        TopTitleCtl.instance.SetStartLayout(new Vector2(50, 50), new Vector2(3.94f, 0), new Vector3(329, -2, 0));



        mback = UguiMaker.newGameObject("mback", transform).GetComponent<RectTransform>();
        mbg2 = UguiMaker.newImage("mbg2", transform, "monkeysortout_sprite", "bg2");
        mfont = UguiMaker.newGameObject("mfont", transform).GetComponent<RectTransform>();


        //出来舞台
        UguiMaker.setRTran(mbg2.rectTransform, UguiAnchorEnum.bottom);
        mbg2.rectTransform.pivot = new Vector2(0.5f, 0);
        mbg2.rectTransform.anchoredPosition = new Vector2(0, -mbg2.rectTransform.sizeDelta.y);
        Vector2 pos0 = mbg2.rectTransform.anchoredPosition;
        Vector3 pos1 = new Vector3(0, 0);
        for (float i = 0; i < 1f; i += 0.03f)
        {
            mbg2.rectTransform.anchoredPosition = Vector2.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mbg2.rectTransform.anchoredPosition = pos1;
        
        RewardClean();
        RewardInit();


        SoundManager.instance.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.1f);
        for (int i = 1; i <= 10; i++)
        {
            GameObject obj = ResManager.GetPrefab("monkeysortout_prefab", "monkey" + i.ToString());
            mMkys.Add(UguiMaker.InitGameObj(obj, transform, "monkey" + i.ToString(), new Vector3(-1000, 0, 0), Vector3.one).GetComponent<MonkeySortOutMonkey>());
        }
        mCarLeft = UguiMaker.InitGameObj(new GameObject(), mfont.transform, "car_left", Vector3.zero, Vector3.one).AddComponent<MonkeySortOutCar>();
        yield return new WaitForSeconds(3);
        mCarRight = UguiMaker.InitGameObj(new GameObject(), mfont.transform, "car_right", Vector3.zero, Vector3.one).AddComponent<MonkeySortOutCar>();


        
        //yield return new WaitForSeconds(2.5f);


        temp_can_update = true;

    }
    /*
    IEnumerator TCallbackAssetbundleMonkey()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AssetBundleRequest request = ResManager.GetPrefabAllAsync("monkeysortout_prefab");
            while (request.isDone == false)
                yield return 2;

            for (int i = 0; i < request.allAssets.Length; i++)
            {
                if (request.allAssets[i].name.Contains("monkey"))
                {
                    GameObject obj = GameObject.Instantiate(request.allAssets[i]) as GameObject;
                    mMkys.Add(UguiMaker.InitGameObj(obj, transform, request.allAssets[i].name, new Vector3(-1000, 0, 0), Vector3.one).GetComponent<MonkeySortOutMonkey>());
                    yield return 1;
                }
            }

        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            for (int i = 1; i <= 10; i++)
            {
                mMkys.Add(UguiMaker.InitGameObj(ResManager.GetPrefab("monkeysortout_prefab", "monkey" + i), transform, "monkey" + i, new Vector3(-1000, 0, 0), Vector3.one).GetComponent<MonkeySortOutMonkey>());
            }
        }

        mCarLeft = UguiMaker.InitGameObj(new GameObject(), mfont.transform, "car_left", Vector3.zero, Vector3.one).AddComponent<MonkeySortOutCar>();
        yield return new WaitForSeconds(3);
        mCarRight = UguiMaker.InitGameObj(new GameObject(), mfont.transform, "car_right", Vector3.zero, Vector3.one).AddComponent<MonkeySortOutCar>();


    }
    */
    Vector3 temp_select_offset = Vector3.zero;
    bool temp_can_update = false;
    void Update()
    {
        if (!temp_can_update)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            MonkeySortOutMonkey.gSelect = null;
            MonkeySortOutCar.gSelect = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, 100);
            if (null != hits)
            {
                foreach (RaycastHit hit in hits)
                {
                    MonkeySortOutMonkey com = hit.collider.gameObject.GetComponent<MonkeySortOutMonkey>();
                    MonkeySortOutCar.gSelect = hit.collider.gameObject.GetComponent<MonkeySortOutCar>();
                    if (null != com)
                    {
                        if (null == MonkeySortOutMonkey.gSelect)
                        {
                            MonkeySortOutMonkey.gSelect = com;
                        }
                        else if (com.transform.GetSiblingIndex() > MonkeySortOutMonkey.gSelect.transform.GetSiblingIndex())
                        {
                            MonkeySortOutMonkey.gSelect = com;
                        }
                    }
                }
            }
            if (null != MonkeySortOutMonkey.gSelect)
            {
                MonkeySortOutMonkey.gSelect.Select();
                mCarLeft.PopMky(MonkeySortOutMonkey.gSelect);
                mCarRight.PopMky(MonkeySortOutMonkey.gSelect);

                temp_select_offset = MonkeySortOutMonkey.gSelect.mrtran.anchoredPosition3D - Common.getMouseLocalPos(transform);
                temp_select_offset.z = 0;

                mSoundMgr.PlayOnly("monkeysortout_sound", "select_monkey" + (Random.Range(0, 1000) % 14), 1, true);

            }
            else if (null != MonkeySortOutCar.gSelect)
            {
                mSoundMgr.PlayShort("monkeysortout_sound", "click_car");
                MonkeySortOutCar.gSelect.Shake(10);
            }
            else
            {
                BananaPlay(2);
                mSoundMgr.PlayOnly("monkeysortout_sound", "banana", 1, true);
            }
        }

        else if (Input.GetMouseButton(0))
        {
            if (null != MonkeySortOutMonkey.gSelect)
            {
                MonkeySortOutMonkey.gSelect.mrtran.anchoredPosition3D = Common.getMouseLocalPos(transform) + temp_select_offset;
            }
            else if (null == MonkeySortOutCar.gSelect)
            {
                //if (Time.frameCount % 2 == 0)
                BananaPlay(1);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mSoundMgr.StopOnly();
            if (null != MonkeySortOutMonkey.gSelect)
            {
                MonkeySortOutCar select_car = null;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray, 100);
                if (null != hits)
                {
                    foreach (RaycastHit hit in hits)
                    {
                        select_car = hit.collider.gameObject.GetComponent<MonkeySortOutCar>();
                        if (null != select_car)
                        {
                            //成功放到车里
                            MonkeySortOutMonkey.gSelect.mShadow.enabled = true;
                            MonkeySortOutMonkey.gSelect.PlaySpine("Click", false);
                            select_car.PushInMky(MonkeySortOutMonkey.gSelect);
                            mSoundMgr.PlayOnly("monkeysortout_sound", "monkey_jumpin_car", 1);
                            GetResult();
                            break;
                        }
                    }
                }

                if (null == select_car)
                {
                    bool is_out_wutai = true;
                    RaycastHit2D[] hit2ds = Physics2D.RaycastAll(MonkeySortOutMonkey.gSelect.transform.position, Vector2.up, 100);
                    foreach (RaycastHit2D hit2d in hit2ds)
                    {
                        if (hit2d.collider.gameObject.name.Equals("collider"))
                        {
                            is_out_wutai = false;
                            break;
                        }
                    }

                    if (is_out_wutai)
                    {
                        //没有放在舞台上，要归位
                        MonkeySortOutMonkey.gSelect.mResetPos = new Vector3(MonkeySortOutMonkey.gSelect.mrtran.anchoredPosition.x, -178, 0);
                        MonkeySortOutMonkey.gSelect.ResetPos();
                        mSoundMgr.PlayOnly("monkeysortout_sound", "monkey_fall_down", 1);
                    }
                    else
                    {
                        //放到舞台上
                        MonkeySortOutMonkey.gSelect.PlaySpine("Idle", true);
                        MonkeySortOutMonkey.gSelect.mShadow.enabled = true;
                        mSoundMgr.PlayOnly("monkeysortout_sound", "monkey_fall_down", 1);
                        SortFont();
                    }

                }


                MonkeySortOutMonkey.gSelect.StopTSelecting();
                MonkeySortOutMonkey.gSelect = null;

            }



        }

    }

    public void BananaPlay(int num)
    {
        mBanana.transform.localPosition = Common.getMouseLocalPos(transform);
        mBanana.Emit(num);
    }
    public IEnumerator RewardPlay(int sort_type)
    {

        //mLayout.enabled = false;
        if (null == mReward)
        {
            mReward = ResManager.GetPrefab("monkeysortout_prefab", "reward_effect").GetComponent<ParticleSystem>();
            UguiMaker.InitGameObj(mReward.gameObject, transform, "reward_effect", new Vector3(0, GlobalParam.screen_height * 0.5f + 82), Vector3.one);
        }

        //RectTransform rtran = mRewards[sort_type].GetComponent<RectTransform>();


        mSoundMgr.PlayShort("monkeysortout_sound", "show_reward" + (Random.Range(0, 1000) % 2));
        mReward.transform.position = TopTitleCtl.instance.GetStarPos("reward" + m_temp_sort_type);
        mReward.Play();
        yield return new WaitForSeconds(1.5f);
        RewardInit();

    }
    public void Reset()
    {
        temp_start_count = 1;
        mGuanka.guanka = 0;
        RewardClean();
        RewardInit();
        CallbackStart();
    }
    public void SortFont()
    {
        if (mfont.childCount <= 0)
            return;

        MonkeySortOutMonkey[] child = mfont.GetComponentsInChildren<MonkeySortOutMonkey>();
        List<MonkeySortOutMonkey> temp = new List<MonkeySortOutMonkey>();
        for (int i = 0; i < child.Length; i++)
        {
            temp.Add(child[i]);
        }
        temp.Sort(
            delegate (MonkeySortOutMonkey x, MonkeySortOutMonkey y)
            {
                if (x.mrtran.anchoredPosition.y > y.mrtran.anchoredPosition.y)
                    return -1;
                else
                    return 1;
            }
            );


        for (int i = 0; i < temp.Count; i++)
        {
            temp[i].transform.SetSiblingIndex(i);
        }

    }
    public void RewardInit()
    {
        string value = PlayerPrefs.GetString("MonkeySortOutCtl:Reward");
        string[] values = value.Split('-');
        if (null != values && values.Length >= 8)
        {
            for (int i = 0; i < 8; i++)
            {
                if (values[i].Contains("True") || values[i].Contains("true"))
                {
                    m_have_reward[i] = true;
                }
                else
                {
                    m_have_reward[i] = false;
                }
            }
        }

        if (0 > m_temp_sort_type || 8 < m_temp_sort_type)
        {
            return;
        }
        List<string> star_names = TopTitleCtl.instance.GetStarNames();
        string reward_name = "reward" + m_temp_sort_type;
        if (!star_names.Contains(reward_name))
        {
            TopTitleCtl.instance.SetSpriteData("monkeysortout_sprite", "reward_bing", "reward_bing", reward_name);
            TopTitleCtl.instance.AddStar();
        }

        ScreenDebug.Log(reward_name);


    }
    public void RewardSave()
    {
        string value = m_have_reward[0].ToString();
        for (int i = 1; i < m_have_reward.Length; i++)
        {
            value += "-" + m_have_reward[i].ToString();
        }
        PlayerPrefs.SetString("MonkeySortOutCtl:Reward", value);
    }
    public void RewardClean()
    {
        PlayerPrefs.DeleteKey("MonkeySortOutCtl:Reward");
        for(int i = 0; i < m_have_reward.Length; i++)
        {
            m_have_reward[i] = false;
        }
    }
    public void GetResult()
    {

        if (mfont.childCount > 0)
            return;

        List<int> type_left = mCarLeft.GetMkyType();
        List<int> type_right = mCarRight.GetMkyType();
        bool correct = false;
        bool can_play_reward_effect = false;
        m_temp_sort_type = -1;
        for (int i = 0; i < type_left.Count; i++)
        {
            if(type_right.Contains(type_left[i]))
            {
                correct = true;
                if (!m_have_reward[type_left[i]])
                    can_play_reward_effect = true;
                m_have_reward[type_left[i]] = true;
                RewardSave();
                m_temp_sort_type = type_left[i];
                break;
            }
        }
        

        if (correct)
        {
            SoundManager.instance.StopTip();
            temp_can_update = false;
            StartCoroutine(TPlayKbady(can_play_reward_effect));
        }
        else
        {
            //分组失败
            mCarLeft.Error();
            mCarRight.Error();

            SoundManager.instance.PlayShort("monkeysortout_sound", "anwer_error");
            SoundManager.instance.PlayTip("monkeysortout_sound", "sort_error" + (Random.Range(0, 1000) % 2));

        }

    }

    int temp_start_count = 12 - 1;//-1表示不等最后一辆车初始化就可以出来
    public void CallbackStart()
    {
        temp_start_count--;
        if (0 != temp_start_count)
            return;

        mGuanka.Set(mGuanka.guanka + 1);
        if (mGuanka.guanka == 1)
            TopTitleCtl.instance.Reset();
        else
        {
            //TopTitleCtl.instance.SetSpriteData("monkeysortout_sprite", "reward_bing", "reward_bing", "reward" + m_temp_sort_type);
            //TopTitleCtl.instance.AddStar();
        }

        StartCoroutine(TCallbackStart());
    }
    IEnumerator TCallbackStart()
    {
        
        mMkys = Common.BreakRank(mMkys);

        for (int i = 0; i < mMkys.Count; i++)
        {
            mCarLeft.PushInMky(mMkys[i], false);
            mMkys[i].SetBoxEnable(false);
        }
        yield return 1;
        mCarLeft.Run_down_left_to_right();
        yield return 1;
        temp_can_update = true;

        KbadyCtl.Init();
        yield return 1;

        //香蕉
        mBanana = ResManager.GetPrefab("monkeysortout_prefab", "banana_fall").GetComponent<ParticleSystem>();
        UguiMaker.InitGameObj(mBanana.gameObject, transform, "banana_fall", new Vector3(0, GlobalParam.screen_height * 0.5f + 82), Vector3.one);
        mBanana.Play();
        yield return 1;
        
        yield return 1;

    }

    public void CallbackEnd()
    {
        Debug.Log("temp_start_count=" + temp_start_count);
        temp_start_count--;
        if (0 < temp_start_count)
        {
            return;
        }


        bool is_pass_game = true;
        for(int i = 0; i < m_have_reward.Length; i++)
        {
            if(!m_have_reward[i])
            {
                is_pass_game = false;
                break;
            }
        }
        
        if(is_pass_game)
        {
            GameOverCtl.GetInstance().Show(m_have_reward.Length, Reset);
        }
        else
        {
            temp_start_count = 1;
            mSoundMgr.PlayShort("monkeysortout_sound", "sort_correct_find_again" + (Random.Range(0, 1000) % 2));
            CallbackStart();
        }


    }
    public void Callback_Run_down_left_to_right()
    {
        MonkeySortOutCtl.instance.mSoundMgr.PlayShort("monkeysortout_sound", "car_run");
        mCarLeft.Run_up_in(true);
        mCarRight.Run_up_in(false);

    }
    public void Callback_Sound1()
    {
        switch (m_temp_sort_type)
        {
            case 0:
                if (mCarLeft.GetMkyByIndex(0).m_info_size == 0)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("1只小猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("1只小猴子");
                }
                break;
            case 1:
                if (mCarLeft.GetMkyByIndex(0).m_info_color == 0)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("8只橘色的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("8只橘色的猴子");
                }
                break;
            case 2:
                if (mCarLeft.GetMkyByIndex(0).m_info_hat == 1)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("3只戴帽子的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("3只戴帽子的猴子");
                }
                break;
            case 3:
                if (mCarLeft.GetMkyByIndex(0).m_info_face_side == 0)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("4只正面的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("4只正面的猴子");
                }
                break;
            case 4:
                if (mCarLeft.GetMkyByIndex(0).m_info_tail == 0)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("5只尾巴长的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("5只尾巴长的猴子");
                }
                break;
            case 5:
                if (mCarLeft.GetMkyByIndex(0).m_info_clothes == 1)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("5只穿马甲的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("5只穿马甲的猴子");
                }
                break;
            case 6:
                if (mCarLeft.GetMkyByIndex(0).m_info_stand == 1)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("7只站着的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("7只站着的猴子");
                }
                break;
            case 7:
                if (mCarLeft.GetMkyByIndex(0).m_info_food == 1)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("2只拿桃子的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("2只拿桃子的猴子");
                }
                break;
            case 8:
                if (mCarLeft.GetMkyByIndex(0).m_info_mouth == 1)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("6只张嘴的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("6只张嘴的猴子");
                }
                break;
        }
       

    }
    public void Callback_Sound2()
    {
        switch (m_temp_sort_type)
        {
            case 0:
                if (mCarLeft.GetMkyByIndex(0).m_info_size == 1)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("9只大猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("9只大猴子");
                }
                break;
            case 1:
                if (mCarLeft.GetMkyByIndex(0).m_info_color == 1)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("2只棕色的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("2只棕色的猴子");
                }
                break;
            case 2:
                if (mCarLeft.GetMkyByIndex(0).m_info_hat == 0)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("7只不戴帽子的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("7只不戴帽子的猴子");
                }
                break;
            case 3:
                if (mCarLeft.GetMkyByIndex(0).m_info_face_side == 1)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("6只侧面的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("6只侧面的猴子");
                }
                break;
            case 4:
                if (mCarLeft.GetMkyByIndex(0).m_info_tail == 1)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("5只尾巴短的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("5只尾巴短的猴子");
                }
                break;
            case 5:
                if (mCarLeft.GetMkyByIndex(0).m_info_clothes == 0)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("5只不穿马甲的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("5只不穿马甲的猴子");
                }
                break;
            case 6:
                if (mCarLeft.GetMkyByIndex(0).m_info_stand == 0)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("3只坐着的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("3只坐着的猴子");
                }
                break;
            case 7:
                if (mCarLeft.GetMkyByIndex(0).m_info_food == 0)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("8只不拿桃子的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("8只不拿桃子的猴子");
                }
                break;
            case 8:
                if (mCarLeft.GetMkyByIndex(0).m_info_mouth == 0)
                {
                    mCarLeft.BigBigBig();
                    mCarLeft.ShowText("4只闭嘴的猴子");
                }
                else
                {
                    mCarRight.BigBigBig();
                    mCarRight.ShowText("4只闭嘴的猴子");
                }
                break;
        }
      
    }
    public void Callback_Sound3()
    {
        //SoundManager.instance.StopSoundList();
        KbadyCtl.instance.HideSpine();
        kbady_tall_over = 1;

    }
    public void OnClkCancelKbady()
    {

        SoundManager.instance.PlayShort("button_down");
        btnCancelKbady.GetComponent<Image>().sprite = ResManager.GetSprite("public", "btn_skip_down");
        Invoke("InvokeOnClkCancelKbady1", 0.5f);

    }
    public void InvokeOnClkCancelKbady1()
    {
        SoundManager.instance.StopSoundList();
        KbadyCtl.instance.HideSpine();
        kbady_tall_over = 2;

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

    public void Callback_GoodGood()
    {
        KbadyCtl.instance.PlaySpine(kbady_enum.Narrate, true);
    }

    int kbady_tall_over = 0;
    Button btnCancelKbady;
    IEnumerator TPlayKbady(bool can_play_reward_effect)
    {
        kbady_tall_over = 0;
        if (can_play_reward_effect)
            StartCoroutine(RewardPlay(m_temp_sort_type));

        yield return new WaitForSeconds(1f);

        //分组成功
        for (int i = 0; i < mMkys.Count; i++)
        {
            mMkys[i].SetBoxEnable(false);
        }

        int reward_count = 0;//统计一共找到了多少种方法
        for (int i = 0; i < m_have_reward.Length; i++)
        {
            if (m_have_reward[i])
                reward_count++;
        }
        

        Vector2 pos_car_left0 = mCarLeft.mrtran.anchoredPosition;
        Vector2 pos_car_right0 = mCarRight.mrtran.anchoredPosition;
        Vector2 pos_car_left1 = pos_car_left0 + new Vector2(0, 25);
        Vector2 pos_car_right1 = pos_car_right0 + new Vector2(0, 25);

        //提起车
        for (float i = 0; i < 1f; i += 0.08f)
        {
            mCarLeft.mrtran.anchoredPosition = Vector2.Lerp(pos_car_left0, pos_car_left1, i);
            mCarRight.mrtran.anchoredPosition = Vector2.Lerp(pos_car_right0, pos_car_right1, i);
            yield return new WaitForSeconds(0.01f);
        }
        mCarLeft.mrtran.anchoredPosition = pos_car_left1;
        mCarRight.mrtran.anchoredPosition = pos_car_right1;

        //设置层
        RectTransform mask = KbadyCtl.instance.BgEffect1_Create(new Color32(0, 168, 191, 255));
        mCarLeft.transform.SetParent(KbadyCtl.instance.transform);
        mCarRight.transform.SetParent(KbadyCtl.instance.transform);

        //下降车
        pos_car_left0 = mCarLeft.mrtran.anchoredPosition;
        pos_car_right0 = mCarRight.mrtran.anchoredPosition;
        pos_car_left1 = new Vector2(pos_car_left0.x, mCarLeft.m_pos_down_left.y);
        pos_car_right1 = new Vector2(pos_car_right0.x, mCarLeft.m_pos_down_left.y);
        for (float i = 0; i < 1f; i += 0.06f)
        {
            mCarLeft.mrtran.anchoredPosition = Vector2.Lerp(pos_car_left0, pos_car_left1, i);
            mCarRight.mrtran.anchoredPosition = Vector2.Lerp(pos_car_right0, pos_car_right1, i);
            yield return new WaitForSeconds(0.01f);
        }
        mCarLeft.mrtran.anchoredPosition = pos_car_left1;
        mCarRight.mrtran.anchoredPosition = pos_car_right1;

        yield return new WaitForSeconds(1);

        //出现背景
        KbadyCtl.instance.BgEffect1_Play();
        yield return new WaitForSeconds(1);

        //创建跳过按钮
        btnCancelKbady = UguiMaker.newButton("mBtnCancelKbady", KbadyCtl.instance.transform, "public", "btn_skip_up");
        btnCancelKbady.onClick.AddListener(OnClkCancelKbady);
        btnCancelKbady.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(GlobalParam.screen_width * 0.5f - 82, GlobalParam.screen_height * 0.5f - 150, 0);
        btnCancelKbady.transform.localScale = new Vector3(1, 1, 1);

        //出现K宝
        //KbadyCtl.instance.PlaySpineEncourage(true);
        KbadyCtl.instance.PlaySpine(kbady_enum.Narrate, true);
        KbadyCtl.instance.mRtranSpine.anchoredPosition = new Vector2(0, 150);
        KbadyCtl.instance.ShowSpine(new Vector3(0.6f, 0.6f));
        

        string temp_sound = "rule" + m_temp_sort_type;
        if (3 == reward_count)
            SoundManager.instance.PlaySoundList("monkeysortout_sound", new List<string>()
                {  "children_you_goodgood" + (Random.Range(0, 1000) % 5), temp_sound + "-0", temp_sound + "-1", temp_sound + "-2", "you_find_3" }, new List<System.Action>() { Callback_GoodGood, Callback_Sound1, Callback_Sound2, Callback_Sound3 });
        else if (6 == reward_count)
            SoundManager.instance.PlaySoundList("monkeysortout_sound", new List<string>()
                {  "children_you_goodgood" + (Random.Range(0, 1000) % 5), temp_sound + "-0", temp_sound + "-1", temp_sound + "-2", "you_find_6" }, new List<System.Action>() { Callback_GoodGood, Callback_Sound1, Callback_Sound2, Callback_Sound3 });
        else if (9 == reward_count)
            SoundManager.instance.PlaySoundList("monkeysortout_sound", new List<string>()
                {  "children_you_goodgood" + (Random.Range(0, 1000) % 5), temp_sound + "-0", temp_sound + "-1", temp_sound + "-2", "you_find_all" }, new List<System.Action>() { Callback_GoodGood, Callback_Sound1, Callback_Sound2, Callback_Sound3 });
        else
            SoundManager.instance.PlaySoundList("monkeysortout_sound", new List<string>()
                {"children_you_goodgood" + (Random.Range(0, 1000) % 5), temp_sound + "-0", temp_sound + "-1", temp_sound + "-2"}, new List<System.Action>() { Callback_GoodGood, Callback_Sound1, Callback_Sound2, Callback_Sound3 });



        while (0 == kbady_tall_over)
        {
            yield return new WaitForSeconds(0.5f);
        }

        if (1 == kbady_tall_over)
            yield return new WaitForSeconds(2);

        if(3 == reward_count || 6 == reward_count || 9 == reward_count)
            yield return new WaitForSeconds(1.1f);


        KbadyCtl.instance.BgEffect1_Stop();
        mCarLeft.HideText();
        mCarRight.HideText();
        if(null != btnCancelKbady)
            Destroy(btnCancelKbady.gameObject);
        yield return new WaitForSeconds(1);


        MonkeySortOutCtl.instance.mSoundMgr.PlayShort("monkeysortout_sound", "car_run");

        temp_start_count = 2;
        mCarLeft.Run_down_out(true);
        mCarRight.Run_down_out(false);
        

    }

    public class Guanka
    {
        public int guanka { get; set; }
        public Vector2[] m_mky_pos = new Vector2[10];
        public Guanka()
        {
            float scale = GlobalParam.screen_width / 1423f;

            m_mky_pos[0] = new Vector2(scale  * - 490, -283 - 138);
            m_mky_pos[1] = new Vector2(scale * -446, -66 - 138);
            m_mky_pos[2] = new Vector2(scale * -292, -230 - 138);
            m_mky_pos[3] = new Vector2(scale * -221, 14 - 138);
            m_mky_pos[4] = new Vector2(scale * -70, -277 - 138);
            m_mky_pos[5] = new Vector2(scale * 18, -105 - 138);
            m_mky_pos[6] = new Vector2(scale * 155, 53 - 138);
            m_mky_pos[7] = new Vector2(scale * 243, -173 - 138);
            m_mky_pos[8] = new Vector2(scale * 415, -283 - 138);
            m_mky_pos[9] = new Vector2(scale * 468, -32 - 138);
        }

        public void Set(int _guanka)
        {
            guanka = _guanka;
        }
    }


}
