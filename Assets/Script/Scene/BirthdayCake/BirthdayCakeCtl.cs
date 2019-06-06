using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BirthdayCakeCtl : BaseScene
{
    public static BirthdayCakeCtl instance;

    public Material mMat { get; set; }

    List<BirthdayCakePaopao> mPaopaos = new List<BirthdayCakePaopao>();//空中浮动的泡泡
    List<BirthdayCakeColor> mColors = new List<BirthdayCakeColor>();//颜色选择按钮3个
    List<GameObject> mLazhuHezis = new List<GameObject>();//蜡烛盒子点击按钮

    Transform mSceenParent { get; set; }
    GameObject mLight;//灯光父节点
    GameObject mLazhuSelectGoop { get; set; }//蜡烛盒子父节点
    GameObject mPaopaoParentBack { get; set; }//泡泡父节点
    public GameObject mPaopaoParentFront { get; set; }//泡泡父节点
    BirthdayCakeCake mCake { get; set; }//蛋糕
    
    public SoundManager mSound { get; set; }
    AudioSource mMusic { get; set; }
    RectTransform mBtnOK { get; set; }

    Guanka mGuanka { get; set; }
    public State mState = State.none;
    public int temp_select_color { get; set; }
    public int la_zhu_num = 12;
    bool temp_game_init = false;
    float temp_android_light_param = 1;//安卓灯光会比较暗，做调整

    ParticleSystem mEffectOkBtn { get; set; }


    void Awake()
    {
        instance = this;
        mSound = gameObject.AddComponent<SoundManager>();
        mGuanka = new Guanka();
        if(Application.platform == RuntimePlatform.Android)
        {
            temp_android_light_param = 1.5f;
        }
    }
    void OnDestroy()
    {
        if (instance == this)
            instance = this;

#if OPEN_MIC
        AudioMicrophone.Kill();
#endif

    }
    void Start()
    {
        mSceneType = SceneEnum.BirthdayCake;
        CallLoadFinishEvent();
        mSceenParent = transform.parent;
        StartCoroutine(TStart());

    }
    IEnumerator TStart()
    {

        SetState(State.none);
        mGuanka.Set(1);
        TopTitleCtl.instance.Reset();

        if (!temp_game_init)
        {
            mMat = new Material(Shader.Find("UI/Lit/Transparent"));

            mLight = ResManager.GetPrefab("birthdaycake_prefab", "light");
            UguiMaker.InitGameObj(mLight, transform, "light", Vector3.zero, Vector3.one);

            Image cake = UguiMaker.newImage("cake", transform, "birthdaycake_sprite", "bg");
            mCake = cake.gameObject.AddComponent<BirthdayCakeCake>();
            cake.rectTransform.sizeDelta = new Vector2(1426, 800);
            cake.material = mMat;

            BirthdayCakeColor color0 = UguiMaker.newGameObject("color0", transform).AddComponent<BirthdayCakeColor>();
            BirthdayCakeColor color1 = UguiMaker.newGameObject("color1", transform).AddComponent<BirthdayCakeColor>();
            BirthdayCakeColor color2 = UguiMaker.newGameObject("color2", transform).AddComponent<BirthdayCakeColor>();
            mColors.Add(color0);
            mColors.Add(color1);
            mColors.Add(color2);
            color0.Init(0);
            color1.Init(1);
            color2.Init(2);

        }

        //Light direct_light = transform.FindChild("light/Directional light").GetComponent<Light>();
        //direct_light.gameObject.SetActive(true);
        //direct_light.intensity = 0;

        //yield return new WaitForSeconds(0.2f);


        mCake.Init(mGuanka);


        int temp = 0;
        for (int i = 0; i < 3; i++)
        {
            if (mGuanka.flower_color_select.Contains(i))
            {
                mColors[i].gameObject.SetActive(true);
                mColors[i].Reset();
                mColors[i].transform.localPosition = new Vector3(GlobalParam.screen_width * -0.5f + 136 * 0.5f, GlobalParam.screen_height * -0.5f + 136 * (temp + 0.5f), 0);
                temp++;
            }
            else
            {
                mColors[i].gameObject.SetActive(false);
            }
        }



        Light direct_light = transform.Find("light/Directional light").GetComponent<Light>();
        direct_light.gameObject.SetActive(true);
        direct_light.intensity = 1;

        //for (float i = 0; i < 1f; i += 0.08f)
        //{
        //    direct_light.intensity = i;
        //    yield return new WaitForSeconds(0.01f);
        //}
        //direct_light.intensity = 1;


        temp_game_init = true;
        temp_select_color = 3;

        SetState(State.select_flower);

        yield return new WaitForSeconds(1f);
        mSound.PlayTip("birthdaycake_sound", "gametip_flower", 1, true);
        yield return new WaitForSeconds(1f);
        SoundManager.instance.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.1f);

    }



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            GameOverCtl.GetInstance().Show(2, Replay);
        }

        switch(mState)
        {

            case State.select_flower:
                UpdateFlower();
                break;

            case State.select_lazhu:
                UpdateSelectLazhu();
                break;

        }


    }
    float temp_click_time = 0;
    void UpdateSelectLazhu()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100);
            if (null != hits && 0 < hits.Length)
            {
                foreach (RaycastHit hit in hits)
                {
                    BirthdayCakePaopao.gSelect = hit.collider.gameObject.GetComponent<BirthdayCakePaopao>();
                    if (null != BirthdayCakePaopao.gSelect)
                    {
                        temp_click_time = Time.timeSinceLevelLoad;
                        BirthdayCakePaopao.gSelect.mIsControl = true;
                        break;
                    }

                    if (null == BirthdayCakeLazhu.gSelect)
                        BirthdayCakeLazhu.gSelect = hit.collider.gameObject.GetComponent<BirthdayCakeLazhu>();
                }

            }
        }
        else if(Input.GetMouseButton(0) && null != BirthdayCakePaopao.gSelect)
        {
            BirthdayCakePaopao.gSelect.transform.localPosition = Common.getMouseLocalPos(transform);

        }
        else if(Input.GetMouseButtonUp(0))
        {
            if(null != BirthdayCakePaopao.gSelect)
            {
                if(0.1f > Time.timeSinceLevelLoad - temp_click_time)
                {
                    BirthdayCakePaopao.gSelect.Kill();
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, 100);
                if (null != hits && 0 < hits.Length)
                {
                    foreach(RaycastHit hit in hits)
                    {
                        BirthdayCakeLazhu com = hit.collider.gameObject.GetComponent<BirthdayCakeLazhu>();
                        if (null != com)
                        {

                            com.PushInLazhu(BirthdayCakePaopao.gSelect.mColor);
                            BirthdayCakePaopao.gSelect.gameObject.SetActive(false);
                            if(mCake.AllLazhuInCake())
                            {
                                ShowOk(true, "lazhu");
                            }
                            mCake.FlushLazhuEffect();
                            break;
                        }

                    }
                }
                
                BirthdayCakePaopao.gSelect.mIsControl = false;

            }
            else if(null != BirthdayCakeLazhu.gSelect)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, 100);
                if (null != hits && 0 < hits.Length)
                {
                    foreach (RaycastHit hit in hits)
                    {
                        if(BirthdayCakeLazhu.gSelect == hit.collider.gameObject.GetComponent<BirthdayCakeLazhu>())
                        {
                            BirthdayCakeLazhu.gSelect.PushOutLazhu();
                            if (!mCake.AllLazhuInCake())
                            {
                                ShowOk(false, "lazhu");
                            }
                            mCake.FlushLazhuEffect();
                            break;
                        }
                    }

                }
            }

            BirthdayCakePaopao.gSelect = null;
            BirthdayCakeLazhu.gSelect = null;

        }

    }
    void UpdateFlower()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100);
            if (null != hits && 0 < hits.Length)
            {
                foreach (RaycastHit hit in hits)
                {
                    BirthdayCakeFlower flower = hit.collider.gameObject.GetComponent<BirthdayCakeFlower>();
                    if(null != flower)
                    {
                        flower.OnClick();
                        if(mCake.AllFlowerHaveColor())
                        {
                            ShowOk(true, "flower");
                        }
                        break;
                    }
                }
            }
        }

    }
    


    public void Replay()
    {
        SoundManager.instance.PlayBg();
        temp_click_replay = true;
    }
    public void SetState(State state)
    {
        mState = state;
    }
    public State GetState()
    {
        return mState;
    }



#region 回调函数
    public bool Callback_ClickFlower()
    {
        if( mCake.HaveRuleFlower())
        {
            mBtnOK.GetComponent<Button>().enabled = false;

            for (int i = 0; i < mColors.Count; i++)
                mColors[i].Stop();
            mCake.EnabelBtn_AllFlower(false);
            StartCoroutine(TStartSelectLazhu());
            return true;
        }
        else
        {
            mSound.PlayTip("birthdaycake_sound", "flower_error" + (Random.Range(0, 1000) % 2));
            return false;
        }

    }
    public void Callback_SelectColor(BirthdayCakeColor select_color)
    {
        for (int i = 0; i < mColors.Count; i++)
        {
            mColors[i].Stop();
        }
    }
    public void Callback_Mic(float volume)//当检测到有声音时返回
    {
        //Debug.LogError("volume=" + volume);
        if (10 > volume) return;

        List<BirthdayCakeLazhu> temp = new List<BirthdayCakeLazhu>();
        for(int i = 0; i < mCake.mLazhus.Count; i++)
        {
            if(mCake.mLazhus[i].mIsFire)
            {
                temp.Add(mCake.mLazhus[i]);
            }
        }
        if(temp.Count == 0)
        {
            temp_mic_volume = true;
        }
        else
        {
            int kill_lazhu_num = (int)(la_zhu_num * volume / 250);
            if (temp.Count < kill_lazhu_num)
            {
                kill_lazhu_num = temp.Count;
                temp_mic_volume = true;
            }
            temp = Common.BreakRank<BirthdayCakeLazhu>(temp);
            //ScreenDebug.Log(temp.Count.ToString() + "  volume=" + volume + "  kill=" + kill_lazhu_num);
            for(int i = 0; i < kill_lazhu_num; i++)
            {
                temp[i].ShowFire(false);
            }

        }

        
    }
#endregion



#region 游戏总进度控制
    float temp_scene_scale = 1.5f;
    bool temp_click_next_guanka = false;
    bool temp_click_replay = false;
    bool temp_mic_volume = false;
    IEnumerator TStartSelectLazhu()
    {
        mSound.PlayTip("birthdaycake_sound", "gametip_lazhu", 1, false);
        TopTitleCtl.instance.mSoundTipData.SetData(mSound, "birthdaycake_sound", "补充  additional-2");
        yield return new WaitForSeconds(1);


        SetState(State.none);
        mGuanka.Set(2);
        TopTitleCtl.instance.AddStar();

        //放大全图
        for(float i = 0; i < 1f; i += 0.08f)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.5f, 1.5f, 1), i);
            transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(33, -57.37f, 0), i);
            yield return new WaitForSeconds(0.01f);
        }

        //初始化蜡烛盒子
        if (null == mLazhuSelectGoop)
        {
            mPaopaoParentBack = UguiMaker.newGameObject("mPaopaoParent", transform);
            for (int i = 0; i < la_zhu_num; i++)
            {
                mPaopaos.Add(
                    UguiMaker.newGameObject("p" + i.ToString(), mPaopaoParentBack.transform).AddComponent<BirthdayCakePaopao>());
                mPaopaos[i].gameObject.SetActive(false);
            }
            

            mLazhuSelectGoop = UguiMaker.newGameObject("mLazhuSelectGoop", transform);
            Button lazhu_hezi0 = UguiMaker.newButton("lazhu_hezi0", mLazhuSelectGoop.transform, "birthdaycake_sprite", "lazhu_hezi0");
            Button lazhu_hezi1 = UguiMaker.newButton("lazhu_hezi1", mLazhuSelectGoop.transform, "birthdaycake_sprite", "lazhu_hezi1");
            Button lazhu_hezi2 = UguiMaker.newButton("lazhu_hezi2", mLazhuSelectGoop.transform, "birthdaycake_sprite", "lazhu_hezi2");
            lazhu_hezi0.onClick.AddListener(ClickLazhuHezi0);
            lazhu_hezi1.onClick.AddListener(ClickLazhuHezi1);
            lazhu_hezi2.onClick.AddListener(ClickLazhuHezi2);
            mLazhuHezis.Add(lazhu_hezi0.gameObject);
            mLazhuHezis.Add(lazhu_hezi1.gameObject);
            mLazhuHezis.Add(lazhu_hezi2.gameObject);

        }

        //出来蜡烛选项
        mLazhuSelectGoop.SetActive(true);
        List<Vector3> poss = Common.PosSortByWidth(80 * mGuanka.lazhu_select.Count, mGuanka.lazhu_select.Count, 0);
        int temp_index = 0;
        for (int i = 0; i < 3; i++)
        {
            if (mGuanka.lazhu_select.Contains(i))
            {
                mLazhuHezis[i].transform.localPosition = poss[temp_index];
                mLazhuHezis[i].SetActive(true);
                temp_index++;
            }
            else
            {
                mLazhuHezis[i].SetActive(false);
            }
        }
        Vector3 pos0 = new Vector3( -812, -183, 0);
        Vector3 pos1 = new Vector3(-285, -183, 0);
        for (float i = 0; i < 1f; i += 0.06f)
        {
            float p = Mathf.Sin(Mathf.PI * i) * 300;
            mLazhuSelectGoop.transform.localPosition = Vector3.Lerp( pos0, pos1, i) + new Vector3(p, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        mLazhuSelectGoop.transform.localPosition = pos1;


        mPaopaoParentFront = UguiMaker.newGameObject("mPaopaoParentFront", transform);
        mCake.FlushLazhuEffect();

        SetState(State.select_lazhu);

        //ShowOk(true, "lazhu");
    }
    IEnumerator TGameOver()
    {
        TopTitleCtl.instance.mSoundTipData.Clean();

        mEffectOkBtn.Play();
        mBtnOK.GetComponent<Button>().enabled = false;
        yield return new WaitForSeconds(1);

        ShowOk(false, "flower");
        mSound.PlayTip("birthdaycake_sound", "cake_beautiful" + (Random.Range(0, 1000) % 3));
        float p = 0;
        temp_click_next_guanka = false;
        SetState(State.none);
        mGuanka.Set(3);
        TopTitleCtl.instance.AddStar();

        //收起蜡烛盒子
        Vector3 pos0 = new Vector3(-862, -183, 0);
        Vector3 pos1 = new Vector3(-285, -183, 0);
        for (float i = 0; i < 1f; i += 0.09f)
        {
            mLazhuSelectGoop.transform.localPosition = Vector3.Lerp(pos1, pos0, i);
            yield return new WaitForSeconds(0.01f);
        }
        mLazhuSelectGoop.transform.localPosition = pos0;

        //炸了全部泡泡
        for(int i = 0; i < mPaopaos.Count; i++)
        {
            if(mPaopaos[i].gameObject.activeSelf)
            {
                mPaopaos[i].Kill();
            }
        }
        


        //缩小全图
        for (float i = 0; i < 1f; i += 0.08f)
        {
            transform.localScale = Vector3.Lerp(new Vector3(1.5f, 1.5f, 1), Vector3.one, i);
            transform.localPosition = Vector3.Lerp(new Vector3(33, -57.37f, 0), Vector3.zero, i);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = Vector3.one;
        yield return new WaitForSeconds(1);


#region 显示灯光
        //灯光
        Light light = mLight.transform.Find("Directional light").gameObject.GetComponent<Light>();
        mLight.transform.SetAsLastSibling();
        GameObject mask = mLight.transform.Find("mask").gameObject;
        mask.GetComponent<Image>().sprite = ResManager.GetSprite("birthdaycake_sprite", "light_mask");
        GameObject red = mLight.transform.Find("red").gameObject;
        red.GetComponent<Image>().sprite = ResManager.GetSprite("birthdaycake_sprite", "light_she");
        GameObject green = mLight.transform.Find("green").gameObject;
        green.GetComponent<Image>().sprite = ResManager.GetSprite("birthdaycake_sprite", "light_she");
        GameObject blue = mLight.transform.Find("blue").gameObject;
        blue.GetComponent<Image>().sprite = ResManager.GetSprite("birthdaycake_sprite", "light_she");
        while (light.intensity > 0)
        {
            light.intensity -= 0.04f;
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i < la_zhu_num; i++)
        {
            mCake.mLazhus[i].ShowFire(true);
            light.intensity = (i + 1f) / 16f;
            yield return new WaitForSeconds(0.1f);
        }
        light.intensity = 1;
        mask.SetActive(true);
        red.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        green.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        blue.SetActive(true);
        //yield return new WaitForSeconds(0.5f);
        //mCake.ShowAllFire(true);
#endregion

        GameObject music_obj = ResManager.GetPrefab("birthdaycake_prefab", "music");
        UguiMaker.InitGameObj(music_obj, transform, "music", Vector3.zero, Vector3.one);
        if(null == mMusic)
        {
            mMusic = gameObject.AddComponent<AudioSource>();
            mMusic.loop = false;
            mMusic.clip = ResManager.GetClip("birthdaycake_sound", "music");
        }
        mMusic.Play();
        SoundManager.instance.StopBg();
        yield return new WaitForSeconds(mMusic.clip.length);
        mSound.PlayTip("birthdaycake_sound", "chui_lazhu");
        yield return new WaitForSeconds(2.8f);


        #region 吹气
#if OPEN_MIC
        temp_mic_volume = false;
            AudioMicrophone.Init();
            AudioMicrophone.instance.BeginCheekVolume(Callback_Mic);
            while (!temp_mic_volume)
            {
                yield return new WaitForSeconds(0.01f);
            }
            AudioMicrophone.instance.StopCheekVolume();
            AudioMicrophone.Kill();
#endif
#endregion





        //显示下一关
        Button next_guanka = UguiMaker.newButton("button", transform, "public", "gameover_next");
        next_guanka.GetComponent<Image>().material = mMat;
        next_guanka.transition = Selectable.Transition.None;
        next_guanka.onClick.AddListener(OnClkNextGuanka);
        next_guanka.transform.localPosition = new Vector3(GlobalParam.screen_width * 0.5f - 95, GlobalParam.screen_height * -0.5f + 50, 0);
        for (float i = 0; i < 1f; i += 0.08f)
        {
            p = Mathf.Sin(Mathf.PI * i) * 0.2f;
            next_guanka.transform.localScale =
                Vector3.Lerp(new Vector3(0.5f, 0.5f, 1), Vector3.one, i) +
                new Vector3(p, p, 0);
            yield return new WaitForSeconds(0.01f);
        }
        next_guanka.transform.localScale = Vector3.one;

        TopTitleCtl.instance.AddStar();

        //等待点击下一关
        while (!temp_click_next_guanka)
            yield return new WaitForSeconds(0.02f);

        if(null != next_guanka)
        {
            //点击下一关按钮特效
            for (float i = 0; i < 1f; i += 0.15f)
            {
                p = 1 - 0.2f * Mathf.Sin(Mathf.PI * i);
                next_guanka.transform.localScale = new Vector3(p, p, 1);
                yield return new WaitForSeconds(0.01f);
            }
            next_guanka.transform.localScale = Vector3.one;
        }
        Destroy(music_obj.gameObject);
        Destroy(next_guanka.gameObject);
        mMusic.Stop();

        GameOverCtl.GetInstance().Show(3, Replay);
        //等待重新开始游戏
        temp_click_replay = false;
        while (!temp_click_replay)
            yield return new WaitForSeconds(0.02f);
        

#region 关灯
        mCake.ShowAllFire(false);
        while (light.intensity > 0)
        {
            light.intensity -= 0.04f;
            yield return new WaitForSeconds(0.03f);
        }
        mask.SetActive(false);
        red.SetActive(false);
        green.SetActive(false);
        blue.SetActive(false);
#endregion



        mCake.Reset();
        StartCoroutine(TStart());


    }
    void OnClkNextGuanka()
    {
        temp_click_next_guanka = true;
    }
#endregion



#region 盒子点击，和点击特效，创建泡泡
    public void ClickLazhuHezi0()
    {
        //Debug.LogError("ClickLazhuHezi0");
        StopCoroutine("TEffectLazhuHezi0");
        StartCoroutine("TEffectLazhuHezi0");
        ShootPaopao(0);
    }
    public void ClickLazhuHezi1()
    {
        //Debug.LogError("ClickLazhuHezi1");
        StopCoroutine("TEffectLazhuHezi1");
        StartCoroutine("TEffectLazhuHezi1");
        ShootPaopao(1);
    }
    public void ClickLazhuHezi2()
    {
        //Debug.LogError("ClickLazhuHezi2");
        StopCoroutine("TEffectLazhuHezi2");
        StartCoroutine("TEffectLazhuHezi2");
        ShootPaopao(2);
    }
    IEnumerator TEffectLazhuHezi0()
    {
        float p = 0;
        for (float i = 0; i < 1f; i += 0.15f)
        {
            p = 1 - 0.2f * Mathf.Sin(Mathf.PI * i);
            mLazhuHezis[0].transform.localScale = new Vector3(p, p, 1);
            yield return new WaitForSeconds(0.01f);
        }
        mLazhuHezis[0].transform.localScale = Vector3.one;

    }
    IEnumerator TEffectLazhuHezi1()
    {
        float p = 0;
        for (float i = 0; i < 1f; i += 0.15f)
        {
            p = 1 - 0.2f * Mathf.Sin(Mathf.PI * i);
            mLazhuHezis[1].transform.localScale = new Vector3(p, p, 1);
            yield return new WaitForSeconds(0.01f);
        }
        mLazhuHezis[1].transform.localScale = Vector3.one;

    }
    IEnumerator TEffectLazhuHezi2()
    {
        float p = 0;
        for (float i = 0; i < 1f; i += 0.15f)
        {
            p = 1 - 0.2f * Mathf.Sin(Mathf.PI * i);
            mLazhuHezis[2].transform.localScale = new Vector3(p, p, 1);
            yield return new WaitForSeconds(0.01f);
        }
        mLazhuHezis[2].transform.localScale = Vector3.one;

    }
    void ShootPaopao(int color)
    {
        BirthdayCakePaopao paopao = null;
        for (int i = 0; i < mPaopaos.Count; i++)
        {
            if (!mPaopaos[i].gameObject.activeSelf)
            {
                paopao = mPaopaos[i];
                break;
            }
        }
        if (null == paopao)
            return;
        paopao.transform.SetParent(mPaopaoParentBack.transform);
        paopao.gameObject.SetActive(true);
        paopao.mIsControl = false;
        paopao.Shoot(color, mLazhuHezis[color].transform.position);

        BirthdayCakeCtl.instance.mSound.PlayShort("birthdaycake_sound", "click_hezi");
    }
    #endregion



    #region 确定按钮
    string temp_select = "";
    bool temp_show = false;
    public void ShowOk(bool is_show, string select)
    {
        if (select.Equals(temp_select) && temp_show == is_show)
            return;
        temp_select = select;
        temp_show = is_show;

        Button btn = null;
        if(null == mBtnOK)
        {
            btn = UguiMaker.newButton("btn_ok", transform, "birthdaycake_sprite", "btn_ok_up");
            btn.transition = Selectable.Transition.SpriteSwap;

            SpriteState spState = new SpriteState();
            spState.disabledSprite = ResManager.GetSprite("birthdaycake_sprite", "btn_ok_up");
            spState.pressedSprite = ResManager.GetSprite("birthdaycake_sprite", "btn_ok_down");
            btn.spriteState = spState;


            mBtnOK = btn.GetComponent<RectTransform>();

            mEffectOkBtn = ResManager.GetPrefab("effect_okbtn", "okbtn_effect").GetComponent<ParticleSystem>();
            UguiMaker.InitGameObj(mEffectOkBtn.gameObject, btn.transform, "okbtn_effect", Vector3.zero, Vector3.one);
        }
        else
        {
            btn = mBtnOK.GetComponent<Button>();
        }

        btn.enabled = true;
        btn.onClick.RemoveAllListeners();
        switch (select)
        {
            case "flower":
                btn.onClick.AddListener(OnClkOK_Flower);
                mBtnOK.anchoredPosition3D = new Vector3(474, -306, 0);
                mBtnOK.localScale = Vector3.one;
                break;
            case "lazhu":
                btn.onClick.AddListener(OnClkOK_Lazhu);
                mBtnOK.anchoredPosition3D = new Vector3(377, -193, 0);
                mBtnOK.localScale = new Vector3( 1/ 1.48f, 1/1.48f, 1);
                break;
        }
        StartCoroutine(TShowOk(is_show));

    }
    public void OnClkOK_Flower()
    {
        mEffectOkBtn.Play();
        //Global.instance.PlayBtnClickAnimation(mBtnOK.transform);
        Callback_ClickFlower();
    }
    public void OnClkOK_Lazhu()
    {
        //Global.instance.PlayBtnClickAnimation(mBtnOK.transform);
        mEffectOkBtn.Play();
        if (mCake.HaveRuleLazhu())
        {
            StartCoroutine(TGameOver());
        }
        else
        {
            mSound.PlayTip("birthdaycake_sound", "lazhu_error" + (Random.Range(0, 1000) % 2));
        }
    }
    IEnumerator TShowOk(bool is_show)
    {
        Vector3 s0 = mBtnOK.localScale;
        Vector3 s1 = 0.5f * s0;
        if (is_show)
        {
            mBtnOK.gameObject.SetActive(true);
            for (float i = 0; i < 1f; i += 0.1f)
            {
                float p = Mathf.Sin(Mathf.PI * i) * s0.x * 0.3f;
                mBtnOK.localScale = Vector3.Lerp(s1, s0, i) + new Vector3(p, p, 0);
                yield return new WaitForSeconds(0.01f);
            }
            mBtnOK.localScale = s0;
        }
        else
        {
            for (float i = 0; i < 1f; i += 0.1f)
            {
                float p = Mathf.Sin(Mathf.PI * i) * s0.x * 0.3f;
                mBtnOK.localScale = Vector3.Lerp(s0, s1, i) + new Vector3(p, p, 0);
                yield return new WaitForSeconds(0.01f);
            }
            mBtnOK.gameObject.SetActive(false);

        }
    }

#endregion

    public class Guanka
    {
        public Vector3[] flower_poss { get; set; }
        public Vector3[] lazhu_box_poss { get; set; }
        public int guanka { get; set; }
        public List<int> flower_color_select = new List<int>();
        public List<int> lazhu_select = new List<int>();

        public Guanka()
        {
            guanka = 1;

            flower_poss = new Vector3[] {
                new Vector3(-300, 72, 0),
                new Vector3(-282, 126, 0),
                new Vector3(-244.6f, 176, 0),
                new Vector3(-189, 208, 0),
                new Vector3(-128.8f, 230.7f, 0),
                new Vector3(-67, 240.3f, 0),
                new Vector3(-1.9f, 244, 0),
                new Vector3(60.1f, 238.8f, 0),
                new Vector3(122, 224.5f, 0),
                new Vector3(178.3f, 197.9f, 0),
                new Vector3(227.4f, 161.2f, 0),
                new Vector3(259.7f, 116, 0),
                new Vector3(276, 60, 0),
                new Vector3(270, 2, 0),
                new Vector3(226, -46, 0),
                new Vector3(167, -82, 0),
                new Vector3(104, -102, 0),
                new Vector3(38, -110, 0),
                new Vector3(-30, -115, 0),
                new Vector3(-95, -113, 0),
                new Vector3(-158, -100, 0),
                new Vector3(-212, -76, 0),
                new Vector3(-269, -43, 0),
                new Vector3(-301, 14, 0),
            };

            
            lazhu_box_poss = new Vector3[]{
                new Vector3( -204f, 105f, 0f),
                new Vector3( -154f, 156f, 0f),
                new Vector3( -62f, 184f, 0f),
                new Vector3( 45.4f, 182.4f, 0f),
                new Vector3( 132.3f, 154.7f, 0f),
                new Vector3( 187.2f, 103.6f, 0f),
                new Vector3( 173.7f, 37.5f, 0f),
                new Vector3( 120.8f, -17.6f, 0f),
                new Vector3( 26f, -47f, 0f),
                new Vector3( -74.3f, -48.2f, 0f),
                new Vector3( -156.9f, -21.3f, 0f),
                new Vector3( -207.9f, 30.1f, 0f),
            };

            flower_color_select = new List<int>() { 0, 1, 2 };
            lazhu_select = new List<int>() { 0, 1, 2 };

        }
        public void Set(int _guanka)
        {
            guanka = _guanka;
            switch (guanka)
            {
                case 1:
                    {
                    }
                    break;
                case 2:
                    {
                    }
                    break;

            }


        }
    }
    public enum State
    {
        none,
        select_flower,
        select_lazhu,
    }


}
