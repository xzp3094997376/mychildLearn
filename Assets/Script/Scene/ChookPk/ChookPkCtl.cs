using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChookPkCtl : BaseScene
{
    public static ChookPkCtl instance = null;

    List<ChookPkChook> mChooks = new List<ChookPkChook>();

    public bool sssss = false;
    public SoundManager mSound { get; set; }
    public ChookPkPulley mPulley { get; set; }
    public ChookPkCar mCarLeft { get; set; }
    public ChookPkCar mCarRight { get; set; }
    //Image mNumber = null;
    
    
    RectTransform mRtran { get; set; }
    GameObject mScene;
    Image mMathNum;
    GameObject mUnderGround;
    public Guanka mGuanka { get; set; }
    private State mstate = State.show;
    public State mState
    {
        get
        {
            return mstate;
        }
        set
        {
            //Debug.LogError("mstate=" + value);
            mstate = value;
        }
    }


    
    public int temp_start_count { get; set; }
    string temp_click_name = "";
    float temp_click_time = 0;

    bool isFirstTime = true;

    void Awake()
    {
        instance = this;
        mGuanka = new Guanka();
        ChookPkChook.gEffectFly0 = new List<ParticleSystem>();
        ChookPkChook.gEffectFly1 = new List<ParticleSystem>();
        ChookPkChook.gEffectFly2 = new List<ParticleSystem>();
        ChookPkChook.gEffectTanxi = new List<ParticleSystem>();

    }
    void Start () {
        

        mRtran = gameObject.GetComponent<RectTransform>();

        CallLoadFinishEvent();
        StartCoroutine(TStart());

        mSound = gameObject.AddComponent<SoundManager>();
        
    }
    void OnDestroy()
    {
        ChookPkChook.gEffectFly0.Clear();
        ChookPkChook.gEffectFly1.Clear();
        ChookPkChook.gEffectFly2.Clear();
        ChookPkChook.gEffectTanxi.Clear();
        ChookPkChook.gEffectFly2 = null;
        ChookPkChook.gEffectFly0 = null;
        ChookPkChook.gEffectFly1 = null;

        if (this == instance)
            instance = null;

    }
	void Update ()
    {
        switch(mState)
        {
            case State.select_weight:
                UpdateSelect();
                break;
            case State.match_weight:
                UpdateMatch();
                break;
        }
	
        if(Input.GetKeyDown(KeyCode.A))
        {
            FlushUnderGroundChook();
        }

	}
    void UpdateMatch()
    {

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100);
            ChookPkChook chook = null;
            foreach (RaycastHit hit in hits)
            {
                ChookPkChook temp_chook = hit.collider.gameObject.transform.parent.gameObject.GetComponent<ChookPkChook>();
                if(null != temp_chook)
                {
                    if (null == chook && temp_chook)
                    {
                        chook = temp_chook;
                        temp_click_name = hit.collider.gameObject.name;
                        temp_click_time = Time.timeSinceLevelLoad;
                    }
                    else if (chook.mType < temp_chook.mType)
                    {
                        chook = temp_chook;
                        temp_click_name = hit.collider.gameObject.name;
                        temp_click_time = Time.timeSinceLevelLoad;
                    }
                }
                else
                {
                    temp_click_name = hit.collider.gameObject.name;
                    temp_click_time = Time.timeSinceLevelLoad;
                }
            }

            if (null != chook)
            {

                if (mState == State.match_weight)
                {
                    if (chook.IsUnderGround())
                    {
                        //小鸡跑
                        chook.RunInUnderGround();
                    }
                    else if (chook.transform.parent.parent.gameObject.name.Contains("shenzi"))
                    {
                        if (chook.mCanFly)
                        {
                            //小鸡消失
                            chook.Fly();
                            mPulley.PushOut(chook);
                            //Debug.LogError("chook.isright=" + chook.IsRight());
                            CreateChookInUnderGround(!chook.IsRight(), chook.mType, true, true);
                        }
                        else
                        {
                            chook.PlayTanxi();
                        }

                    }


                }
            }
            
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100))
            {
                if(mState == State.match_weight && Time.timeSinceLevelLoad - temp_click_time < 0.2f && hit.collider.gameObject.name.Equals(temp_click_name)&& temp_click_name.Equals("mOK1"))
                {
                    mState = State.match_loading;
                    mPulley.PlayAnimationCenter(Callback_MatchPulleyCenter_Match);
                    mPulley.PlayOKBtnAnimation();

                }

            }

        }
    }
    void UpdateSelect()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.transform.parent.parent.gameObject.name.Contains("shenzi_"))
                {
                    temp_click_name = hit.collider.gameObject.name;
                    temp_click_time = Time.timeSinceLevelLoad;
                    break;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100);
            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.gameObject.name.Equals(temp_click_name) && Time.timeSinceLevelLoad - temp_click_time < 0.2f)
                {
                    if(hit.collider.gameObject.transform.parent.parent.gameObject.name.Contains("left"))
                    {
                        mPulley.OnClkLeft();
                    }
                    else if(hit.collider.gameObject.transform.parent.parent.gameObject.name.Contains("right"))
                    {
                        mPulley.OnClkRight();
                    }
                }
             
            }
        }
    }


    public Vector2 GetUnderGroundPos(string side, int chook_weight)
    {
        switch(side)
        {
            case "left":
                if(chook_weight < 2)
                    return new Vector2(Random.Range(-354, -200), Random.Range(-400, -375));
                else
                    return new Vector2(Random.Range(-354, -200), Random.Range(-375, -351));
            case "right":
                if (chook_weight < 2)
                    return new Vector2(Random.Range(200, 354), Random.Range(-400, -375));
                else
                    return new Vector2(Random.Range(200, 354), Random.Range(-375, -351));
        }
        return new Vector2( Random.Range(-354, 354), Random.Range(-400, -351));
    }
    public ChookPkChook GetFreeChook()
    {
        for(int i = 0; i < mChooks.Count; i++)
        {
            if(!mChooks[i].gameObject.activeSelf && !mChooks[i].IsBusy())
            {
                mChooks[i].transform.localScale = Vector3.one;
                mChooks[i].SetBusy(true);
                return mChooks[i];
            }
        }
        return null;
    }
    public ChookPkChook CreateChook(bool is_right, int type, bool is_under_ground, bool can_fly)
    {
        ChookPkChook chook = GetFreeChook();
        if(is_right)
        {
            chook.SetForward("right");
        }
        else
        {
            chook.SetForward("left");
        }
        chook.SetData(type, is_under_ground, can_fly);
        return chook;
    }
    public ChookPkChook CreateChookInUnderGround(bool is_right, int type, bool is_under_ground, bool can_fly)//创建一直小鸡
    {
        ChookPkChook chook = CreateChook(is_right, type, true, true);
        chook.transform.SetParent(mUnderGround.transform);
        chook.PlaySpine(ChookPkChook.Animation.Squat, true);
        if (is_right)
        {
            chook.mRtran.anchoredPosition = GetUnderGroundPos("left", chook.mWeight);
        }
        else
        {
            chook.mRtran.anchoredPosition = GetUnderGroundPos("right", chook.mWeight);
        }
        chook.gameObject.SetActive(true);
        Global.instance.PlayBtnClickAnimation(chook.transform);
        FlushUnderGroundSortFont();
        return chook;
    }
    public void SetCookFree(ChookPkChook chook)
    {
        chook.SetBusy(false);
        chook.gameObject.SetActive(false);
    }
    public void Reset()
    {
        //TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.ResetNotMoveIn();
        mGuanka.Set(1);
        StartCoroutine(TReset());
        mPulley.SetOKBtnType(false);
        mPulley.FlushOKSprite();

    }
    public void FlyPulleychooks()//滑轮上的鸡都飞起来
    {
        for(int i = 0; i < mPulley.mLeftChooks.Count; i++)
        {
            mPulley.mLeftChooks[i].Fly();
        }
        for (int i = 0; i < mPulley.mRightChooks.Count; i++)
        {
            mPulley.mRightChooks[i].Fly();
        }
        mPulley.CleanChook_L();
        mPulley.CleanChook_R();

    }
    public void FlushPulleyChook()//重新生成滑轮上的鸡
    {
        mPulley.CleanChook_L();
        mPulley.CleanChook_R();
        mGuanka.FlushData(mState);
        mSound.PlayShort("chookpk_sound", "03-鸡出现");
        //Debug.LogError("heavy_counts_l=" + mGuanka.heavy_counts_l.Count);

        for(int i = 0; i < mGuanka.heavy_counts_l.Count; i++)
        {
            ChookPkChook chook = CreateChook(false, mGuanka.heavy_counts_l[i], false, false);
            chook.transform.SetParent(mPulley.mLanziLeft);
            chook.mRtran.anchoredPosition = mPulley.GetStandPos("left", chook);// mGuanka.heavy_counts_l[i]);//poss[i];
            chook.gameObject.SetActive(true);
            chook.PlaySpine(ChookPkChook.Animation.Squat, true);
            chook.transform.localEulerAngles = Vector3.zero;
            Global.instance.PlayBtnClickAnimation(chook.transform);
            chook.FlushSibling_WhenInLanzi();
            mPulley.PushIn_L(chook);
            //Debug.LogError(chook.gameObject.name);
        }
        
        for (int i = 0; i < mGuanka.heavy_counts_r.Count; i++)
        {
            ChookPkChook chook = CreateChook(true, mGuanka.heavy_counts_r[i], false, false);
            chook.transform.SetParent(mPulley.mLanziRight);
            chook.mRtran.anchoredPosition = mPulley.GetStandPos("right", chook);// mGuanka.heavy_counts_r[i]);// poss[i];
            chook.gameObject.SetActive(true);
            chook.PlaySpine(ChookPkChook.Animation.Squat, true);
            chook.transform.localEulerAngles = Vector3.zero;
            Global.instance.PlayBtnClickAnimation(chook.transform);
            chook.FlushSibling_WhenInLanzi();
            mPulley.PushIn_R(chook);
            //Debug.LogError("right=" + chook.gameObject.name);
        }

        mPulley.Shake_L();
        mPulley.Shake_R();

    }
    public void FlushSelect()//刷新比轻重的问题
    {
        FlushPulleyChook();
        Invoke("Inovke_SetState_SelectWeight", 0.2f);
        mSound.PlayTip("chookpk_sound", "game-tips7-1-那边的鸡更重" + Random.Range(0, 1000) % 2, 1, true);

    }
    public void FlushMatch()//刷新配对的问题
    {
        mState = State.match_weight;
        FlushPulleyChook();
        ChookPkCtl.instance.mSound.PlayTip("chookpk_sound", "game-tips7-1-两边的鸡不一样重" + Random.Range(0, 1000) % 3);
    }
    public void FlushUnderGroundChook()//创建刷新地下鸡的数量
    {
        //统计滑轮上有多少鸡
        int num_heavy_l = 3;
        int num_heavy_r = 3;
        int num_soft_l = 5;
        int num_soft_r = 5;
        for(int i = 0; i < mPulley.mLeftChooks.Count; i++)
        {
            if (2 == mPulley.mLeftChooks[i].mType)
            {
                num_soft_l--;
            }
            else
            {
                num_heavy_l--;
            }
        }
        for (int i = 0; i < mPulley.mRightChooks.Count; i++)
        {
            if (2 == mPulley.mRightChooks[i].mType)
            {
                num_soft_r--;
            }
            else
            {
                num_heavy_r--;
            }
        }
        Debug.LogError("num_soft_l=" + num_soft_l + " num_heavy_l=" + num_heavy_l + " num_soft_r=" + num_soft_r + " num_heavy_r=" + num_heavy_r);
        //减去地下现有多少鸡
        ChookPkChook[] chooks = mUnderGround.transform.GetComponentsInChildren<ChookPkChook>();
        for(int i = 0; i < chooks.Length; i++)
        {
            if(!chooks[i].gameObject.activeSelf || !chooks[i].IsUnderGround())
            {
                continue;
            }
            //地下鸡方向交换
            if(chooks[i].IsRight())
            {
                if(2 == chooks[i].mType)
                {
                    num_soft_l--;
                }
                else
                {
                    num_heavy_l--;
                }
            }
            else
            {
                if (2 == chooks[i].mType)
                {
                    num_soft_r--;
                }
                else
                {
                    num_heavy_r--;
                }
            } 
        }

        Debug.LogError("num_soft_l=" + num_soft_l + " num_heavy_l=" + num_heavy_l + " num_soft_r=" + num_soft_r + " num_heavy_r=" + num_heavy_r);


        //注意地下左边和滑轮上的方向相反
        //创建鸡,左边
        for (int i = 0; i < num_heavy_l; i++)
        {
            ChookPkChook chook = CreateChook(true, mGuanka.select_chook[0], true, true);
            chook.transform.SetParent(mUnderGround.transform);
            chook.mRtran.anchoredPosition = GetUnderGroundPos("left", chook.mWeight);
            chook.gameObject.SetActive(true);
            chook.PlaySpine(ChookPkChook.Animation.Squat, true);
            Global.instance.PlayBtnClickAnimation(chook.transform);
        }
        for (int i = 0; i < num_soft_l; i++)
        {
            ChookPkChook chook = CreateChook(true, mGuanka.select_chook[1], true, true);
            chook.transform.SetParent(mUnderGround.transform);
            chook.mRtran.anchoredPosition = GetUnderGroundPos("left", chook.mWeight);
            chook.gameObject.SetActive(true);
            chook.PlaySpine(ChookPkChook.Animation.Squat, true);
            Global.instance.PlayBtnClickAnimation(chook.transform);
        }
        //创建鸡,右边
        for (int i = 0; i < num_heavy_r; i++)
        {
            ChookPkChook chook = CreateChook(false, mGuanka.select_chook[0], true, true);
            chook.transform.SetParent(mUnderGround.transform);
            chook.mRtran.anchoredPosition = GetUnderGroundPos("right", chook.mWeight);
            chook.gameObject.SetActive(true);
            chook.PlaySpine(ChookPkChook.Animation.Squat, true);
            Global.instance.PlayBtnClickAnimation(chook.transform);
        }
        for (int i = 0; i < num_soft_r; i++)
        {
            ChookPkChook chook = CreateChook(false, mGuanka.select_chook[1], true, true);
            chook.transform.SetParent(mUnderGround.transform);
            chook.mRtran.anchoredPosition = GetUnderGroundPos("right", chook.mWeight);
            chook.gameObject.SetActive(true);
            chook.PlaySpine(ChookPkChook.Animation.Squat, true);
            Global.instance.PlayBtnClickAnimation(chook.transform);
        }

        mSound.PlayShort("chookpk_sound", "03-鸡出现");
        FlushUnderGroundSortFont();

    }
    public void FlushUnderGroundSortFont()
    {
        if (mUnderGround.transform.childCount <= 0)
            return;

        ChookPkChook[] child = mUnderGround.GetComponentsInChildren<ChookPkChook>();
        List<ChookPkChook> temp = new List<ChookPkChook>();
        for (int i = 0; i < child.Length; i++)
        {
            temp.Add(child[i]);
        }
        temp.Sort(
            delegate (ChookPkChook x, ChookPkChook y)
            {
                if (x.mRtran.anchoredPosition.y > y.mRtran.anchoredPosition.y)
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


    void Inovke_SetState_SelectWeight()
    {
        mState = State.select_weight;
    }

    public void Callback_Select(bool is_correct, string heavy_side)//比轻重，选中答案后回调
    {
        mState = State.select_loading;
        if(is_correct)
        {
            //答对了
            Debug.LogError("鸡落地");
            mSound.PlayTip("chookpk_sound", "game-tips7-1-鸡落地" + Random.Range(0, 1000) % 2);
            mGuanka.select_count++;
            if (mGuanka.select_count == mGuanka.select_play_count)
            {
                mPulley.SetOKBtnType(true);
                TopTitleCtl.instance.AddStar();
            }
            switch (heavy_side)
            {
                case "left":
                    mPulley.PlayAnimation_L(Callback_Select_AnimationEnd);
                    break;
                case "right":
                    mPulley.PlayAnimation_R(Callback_Select_AnimationEnd);
                    break;
            }


        }
        else
        {
            //打错了
            mSound.PlayTip("chookpk_sound", "game-tips7-1-这边更轻");
            FlyPulleychooks();
            Invoke("FlushSelect", 2.5f);
        }

    }
    public void Callback_Select_AnimationEnd(string side)
    {
        if(mGuanka.select_count < mGuanka.select_play_count)
        {
            //刷新比轻重
            mPulley.PlayAnimationCenter(FlushSelect);
            //FlushSelect();
        }
        else
        {
            //下一关
            StartCoroutine(TStartMatch());
        }
    }
    public void Callback_MatchPulleyCenter()//归位中间
    {
        FlushMatch();
        FlushUnderGroundChook();

    }
    public void Callback_MatchPulleyCenter_Match()
    {
        StartCoroutine(TShowMatchResult());
    }
    public void Callback_MathcPulley_Error()
    {
        for(int i = 0; i < mChooks.Count; i++)
        {
            if (!mChooks[i].gameObject.activeSelf)
                continue;
            if (!mChooks[i].IsBusy())
                continue;
            if (mChooks[i].IsUnderGround())
                continue;
            if (!mChooks[i].mCanFly)
                continue;
            mPulley.PushOut(mChooks[i]);
            mChooks[i].Fly();
        }
        FlushUnderGroundChook();
        Debug.LogError("回答错误");
        mState = State.match_weight;

    }



    IEnumerator TStart()
    {

        //背景
        RawImage bg0 = UguiMaker.newRawImage("bg0", transform, "chookpk_texture", "chookpk_bg", false);
        bg0.rectTransform.sizeDelta = new Vector2(1423, 800);

        //公式
        mMathNum = UguiMaker.newImage("math_num", transform, "chookpk_sprite", "gongsi0");//UguiMaker.newGameObject("math_num", transform);
        mMathNum.transform.localPosition = new Vector3(0, -93, 0);
        mMathNum.color = new Color(1, 1, 1, 0);


        //场景节点
        mScene = UguiMaker.newGameObject("scene", transform);

        //滑轮工具
        mPulley = UguiMaker.newGameObject("lunhua", mScene.transform).AddComponent<ChookPkPulley>();
        UguiMaker.InitGameObj(mPulley.gameObject, mScene.transform, "tool", new Vector3(0, 320), Vector3.one);

        //升降台
        mCarLeft = UguiMaker.newGameObject("car_left", mScene.transform).AddComponent<ChookPkCar>();
        mCarRight = UguiMaker.newGameObject("car_right", mScene.transform).AddComponent<ChookPkCar>();
        mCarLeft.mFlag = "left";
        mCarRight.mFlag = "right";

        //升降台-背景
        Image bg2_0 = UguiMaker.newImage("bg2_0", transform, "chookpk_sprite", "bg2", false);
        bg2_0.rectTransform.anchoredPosition = new Vector2(-561, -19);
        bg2_0.transform.localScale = new Vector3(-1, 1, 1);
        Image bg2_1 = UguiMaker.newImage("bg2_1", transform, "chookpk_sprite", "bg2", false);
        bg2_1.rectTransform.anchoredPosition = new Vector2(561, -19);

        //地下
        mUnderGround = UguiMaker.newGameObject("under_ground", transform);


        //前景
        Image bg3 = UguiMaker.newImage("bg3", transform, "chookpk_sprite", "bg3", false);
        bg3.rectTransform.anchoredPosition = new Vector2(0, -176);
        
        for (int i = 0; i < 25; i++)
        {
            ChookPkChook com = UguiMaker.newGameObject("ji" + i.ToString(), mScene.transform).AddComponent<ChookPkChook>();
            mChooks.Add(com);
        }

        temp_start_count = 20 + 1 + 2;
        while (temp_start_count > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }

        Reset();


    }
    IEnumerator TReset()
    {
        mState = State.show;
        for (int i = 0; i < mChooks.Count; i++)
            mChooks[i].gameObject.SetActive(false);
        
        for(float i = 0; i < 1f; i += 0.08f)
        {
            mRtran.anchoredPosition = Vector2.Lerp(Vector2.zero, new Vector2(0, -116.9f), i);
            transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.3f, 1.3f, 1), i);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition = new Vector2(0, -116.9f);
        transform.localScale = new Vector3(1.3f, 1.3f, 1);
        yield return new WaitForSeconds(0.01f);


        int num0 = 0, num1 = 0;
        if(0 == mGuanka.select_chook[0] && 1 == mGuanka.select_chook[1])
        {
            num0 = 3;
            num1 = 4;
        }
        else if(0 == mGuanka.select_chook[0] && 2 == mGuanka.select_chook[1])
        {
            num0 = 1;
            num1 = 4;
        }
        else if(1 == mGuanka.select_chook[0] && 2 == mGuanka.select_chook[1])
        {
            num0 = 1;
            num1 = 3;
        }
        else
        {
            Debug.LogError("[0]=" + mGuanka.select_chook[0] + " [1]=" + mGuanka.select_chook[1]);
        }

        //显示公式
        //mSound.PlayTipList(
        //  new List<string>() { "number_sound", "chookpk_sound", "chookpk_sound", "chookpk_sound", "number_sound", "chookpk_sound", "chookpk_sound" },
        //  new List<string>() { "1", "只", mGuanka.select_chook[0] == 0 ? "game-tips7-1-公鸡" : "game-tips7-1-母鸡", "game-tips7-1-等于", mGuanka.select_chook[0] == 0 ? "4" : "3", "只", "game-tips7-1-小鸡" }
        //  );

        List<ChookPkChook> chooks = new List<ChookPkChook>();
        List<Vector3> chooks_poss = new List<Vector3>();
        List<Vector3> poss = Common.PosSortByWidth(280, num0, -119);
       
        
        for (int i = 0; i < num0; i++)
        {
            ChookPkChook chook = CreateChook(false, mGuanka.select_chook[0], false, false);
            chook.transform.SetParent(mPulley.mLanziLeft);
            chook.mRtran.anchoredPosition = poss[i];
            chooks.Add(chook);
            
            chook.gameObject.SetActive(true);
            chook.PlaySpine(ChookPkChook.Animation.Idle, true);
            Global.instance.PlayBtnClickAnimation(chook.transform);

            mPulley.PushIn_L(chook);
            mSound.PlayShort("chookpk_sound", "03-鸡出现");
            yield return new WaitForSeconds(0.1f);
        }
        mPulley.Shake_L();

        //yield return new WaitForSeconds(0.3f);
        poss = Common.PosSortByWidth(280, num1, -119);
        for (int i = 0; i < num1; i++)
        {
            ChookPkChook chook = CreateChook(true, mGuanka.select_chook[1], false, false);
            chook.transform.SetParent(mPulley.mLanziRight);
            chook.mRtran.anchoredPosition = poss[i];
            chooks.Add(chook);
            //
            chook.gameObject.SetActive(true);
            chook.PlaySpine(ChookPkChook.Animation.Idle, true);
            Global.instance.PlayBtnClickAnimation(chook.transform);

            mPulley.PushIn_R(chook);
            mSound.PlayShort("chookpk_sound", "03-鸡出现");
            yield return new WaitForSeconds(0.1f);
        }
        mPulley.Shake_R();
        
        yield return new WaitForSeconds(1);
        
        //鸡进去公式
        mSound.PlayTipList(
          new List<string>() { "number_sound", "chookpk_sound", "chookpk_sound", "chookpk_sound", "number_sound", "chookpk_sound", "chookpk_sound" },
          new List<string>() { "1", "只", mGuanka.select_chook[0] == 0 ? "game-tips7-1-公鸡" : "game-tips7-1-母鸡", "game-tips7-1-等于", mGuanka.select_chook[0] == 0 ? "4" : "3", "只", "game-tips7-1-小鸡" }
          );
        yield return new WaitForSeconds(0.6f);
        mSound.PlayShort("chookpk_sound", "02-公式");

        TopTitleCtl.instance.MoveIn();

        for (int i = 0; i < chooks.Count; i++)
        {
            chooks[i].transform.SetParent(mMathNum.transform);
            chooks_poss.Add(chooks[i].mRtran.anchoredPosition);
        }
        for (float i = 0; i < 1f; i += 0.05f)
        {
            for(int j = 0; j < chooks.Count; j++)
            {
                chooks[j].mRtran.anchoredPosition = Vector2.Lerp(chooks_poss[j], Vector2.zero, i) + new Vector2(0, Mathf.Sin(Mathf.PI * i) * 100);
                chooks[j].transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.5f, 0.5f, 0), i);
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < chooks.Count; i++)
        {
            chooks[i].SetBusy(false);
        }

        mMathNum.color = new Color(1, 1, 1, 1);
        mMathNum.sprite = ResManager.GetSprite("chookpk_sprite", "gongsi" + mGuanka.select_chook[0]);


        for (float i = 0; i < 1f; i += 0.05f)
        {
            mMathNum.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), i);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(5.3f);

        //进入第三关测试
        //StartCoroutine(TStartMatch());
        //yield break;

        //那边重
        mState = State.select_loading;
        FlushSelect();

        yield return new WaitForSeconds(1f);

        if (isFirstTime)
        {
            SoundManager.instance.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.1f);
            isFirstTime = false;
        }

    }
    IEnumerator TStartMatch()
    {
        mGuanka.Set(2);
        mState = State.match_loading;
        mPulley.FlushOKSprite();

        for (float i = 0; i < 1f; i += 0.08f)
        {
            mRtran.anchoredPosition = Vector2.Lerp(new Vector2(0, -116.9f), Vector2.zero, i);
            transform.localScale = Vector3.Lerp(new Vector3(1.3f, 1.3f, 1), Vector3.one, i);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition = Vector2.zero;
        transform.localScale = Vector3.one;

        mPulley.PlayAnimationCenter(Callback_MatchPulleyCenter);



    }
    IEnumerator TShowMatchResult()
    {
        //if(null == mNumber)
        //{
        //    mNumber = UguiMaker.newImage("number_color", transform, "number_color", "3", false);
        //}
        //mNumber.gameObject.SetActive(true);
        //for (int i = 3; i > 0; i--)
        //{
        //    mSound.PlayShort("chookpk_sound", "09-倒数");
        //    mNumber.sprite = ResManager.GetSprite("number_color", i.ToString());
        //    mNumber.transform.localScale = Vector3.one;
        //    for(float j = 0; j < 1f; j += 0.05f)
        //    {
        //        mNumber.transform.localScale = Vector3.Lerp(new Vector3(3, 3, 1), new Vector3(1, 1, 1), j);
        //        yield return new WaitForSeconds(0.01f);
        //    }
        //    yield return new WaitForSeconds(0.2f);
        //}
        //mNumber.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);

        int w_right = mPulley.GetWeight_R();
        int w_left = mPulley.GetWeight_L();

        Debug.LogError("所有鸡飞起来: w_left=" + w_left + " w_right=" + w_right);

        if (w_right == w_left)
        {

            mSound.PlayTip("chookpk_sound", "game-tips7-1-两边一样重");

            mPulley.SetAllChookFree();
            mPulley.PlayMatchEffect();
            for (int i = 0; i < mChooks.Count; i++)
            {
                if (mChooks[i].gameObject.activeSelf)
                {
                    mChooks[i].Fly();
                }
            }
            yield return new WaitForSeconds(2);

            mGuanka.match_count++;
            if(mGuanka.match_count >= mGuanka.match_play_count)
            {
                TopTitleCtl.instance.AddStar();
                GameOverCtl.GetInstance().Show(2, Reset);
            }
            else
            {
                mPulley.PlayAnimationCenter(Callback_MatchPulleyCenter);
            }
        }
        else if(w_right > w_left)
        {
            mPulley.PlayAnimationCenter("right", Callback_MathcPulley_Error);
        }
        else
        {
            mPulley.PlayAnimationCenter("left", Callback_MathcPulley_Error);
        }
        
        //
        //mPulley.DengClose();

    }




    public class Guanka
    {
        public List<int> select_chook;
        public int guanka = 1;

        
        public int[] weights = new int[] { 4, 3, 1};//体重
        public int max_weight = 0;
        public List<int> heavy_counts_l = new List<int>();//左边选择什么重量的鸡,存的是重量
        public List<int> heavy_counts_r = new List<int>();//左边选择什么重量的鸡,存的是重量


        public int select_count = 0;
        public int match_count = 0;

        public int select_play_count = 3;
        public int match_play_count = 6;



        public Guanka()
        {
            

        }

        public void Set(int _guanka)
        {
            guanka = _guanka;
            if(1 == _guanka)
            {
                int random = System.DateTime.Now.Second % 2; //Random.Range(0, 1000) % 2;

                select_chook = new List<int>();
                switch (random)
                {
                    case 0:
                        select_chook = new List<int>() { 1, 2 };
                        max_weight = 14;
                        break;
                    case 1:
                        select_chook = new List<int>() { 0, 2 };
                        max_weight = 17;
                        break;
                }

                select_count = 0;
                match_count = 0;

            }
            
        }
        public int GetHeavyWeight()
        {
            return weights[select_chook[0]];
        }
        public int GetSoftWeight()
        {
            return weights[select_chook[1]];
        }
        public List<int> GetChoose()
        {
            List<int> result = new List<int>();

            result.Add(select_chook[0]);
            result.Add(select_chook[0]);
            result.Add(select_chook[0]);

            result.Add(select_chook[1]);
            result.Add(select_chook[1]);
            result.Add(select_chook[1]);
            result.Add(select_chook[1]);
            result.Add(select_chook[1]);

            return result;

        }

        public void FlushData_change(State state)
        {
            heavy_counts_l.Clear();
            heavy_counts_r.Clear();
            List<int> w = new List<int>();
            switch (state)
            {
                case State.match_weight:
                case State.match_loading:
                    w = Common.GetMutexValue(1, (int)(max_weight * 0.6f), 2);// new List<int>() { 9, 5 }; //
                    break;
                case State.select_weight:
                case State.select_loading:
                    w = Common.GetMutexValue(1, (int)(max_weight), 2);// new List<int>() { 9, 5 }; //
                    break;
            }
            Debug.LogError(state);

            int heavy_num_l = 0;
            int heavy_num_r = 0;
            int soft_num_l = 0;
            int soft_num_r = 0;

            int weight_l = 0;
            int weight_r = 0;
            

            //必定有大鸡
            if (GetHeavyWeight() * 2 + 5 < w[0])
            {
                heavy_num_l = 3;
                weight_l = GetHeavyWeight() * 3;
                heavy_counts_l = new List<int>() { select_chook[0], select_chook[0], select_chook[0] };
            }
            else if (GetHeavyWeight() + 5 < w[0])
            {
                heavy_num_l = 2;
                weight_l = GetHeavyWeight() * 2;
                heavy_counts_l = new List<int>() { select_chook[0], select_chook[0] };
            }
            else if (5 < w[0])
            {
                heavy_num_l = 1;
                weight_l = GetHeavyWeight();
                heavy_counts_l = new List<int>() {  select_chook[0] };
            }

            if (GetHeavyWeight() * 2 + 5 <= w[1])
            {
                heavy_num_r = 3;
                weight_r = GetHeavyWeight() * 3;
                heavy_counts_r = new List<int>() { select_chook[0], select_chook[0], select_chook[0] };
            }
            else if (GetHeavyWeight() + 5 < w[1])
            {
                heavy_num_r = 2;
                weight_r = GetHeavyWeight() * 2;
                heavy_counts_r = new List<int>() { select_chook[0], select_chook[0] };
            }
            else if (5 < w[1])
            {
                heavy_num_r = 1;
                weight_r = GetHeavyWeight();
                heavy_counts_r = new List<int>() { select_chook[0] };
            }




            //左边
            while (weight_l != w[0])
            {
                int h = w[0] - weight_l;


                bool can_select_heavy = true;
                bool can_select_soft = true;
                if (heavy_num_l >= 3)
                    can_select_heavy = false;
                if (soft_num_l >= 5)
                    can_select_soft = false;
                if (h < 3)
                    can_select_heavy = false;


                if (can_select_heavy && can_select_soft)
                {
                    int temp = Random.Range(0, 1000) % 3;
                    if (temp == 0)
                    {
                        heavy_num_l++;
                        weight_l += GetHeavyWeight();
                        heavy_counts_l.Add(select_chook[0]);
                    }
                    else
                    {
                        soft_num_l++;
                        weight_l += GetSoftWeight();
                        heavy_counts_l.Add(select_chook[1]);
                    }
                }
                else if (can_select_heavy)
                {
                    heavy_num_l++;
                    weight_l += GetHeavyWeight();
                    heavy_counts_l.Add(select_chook[0]);
                }
                else if (can_select_soft)
                {
                    soft_num_l++;
                    weight_l += GetSoftWeight();
                    heavy_counts_l.Add(select_chook[1]);
                }
                else
                {
                    Debug.LogError("Error Left:\nheavy_num_l=" + heavy_num_l + "  soft_num_l=" + soft_num_l + "  weight_l=" + weight_l + " w[0]=" + w[0]);
                    break;
                }

            }

            //右边
            while (weight_r != w[1])
            {
                int h = w[1] - weight_r;


                bool can_select_heavy = true;
                bool can_select_soft = true;
                if (heavy_num_r >= 3)
                    can_select_heavy = false;
                if (soft_num_r >= 5)
                    can_select_soft = false;
                if (h < 3)
                    can_select_heavy = false;


                if (can_select_heavy && can_select_soft)
                {
                    int temp = Random.Range(0, 1000) % 3;
                    if (temp == 0)
                    {
                        heavy_num_r++;
                        weight_r += GetHeavyWeight();
                        heavy_counts_r.Add(select_chook[0]);
                    }
                    else
                    {
                        soft_num_r++;
                        weight_r += GetSoftWeight();
                        heavy_counts_r.Add(select_chook[1]);
                    }
                }
                else if (can_select_heavy)
                {
                    heavy_num_r++;
                    weight_r += GetHeavyWeight();
                    heavy_counts_r.Add(select_chook[0]);
                }
                else if (can_select_soft)
                {
                    soft_num_r++;
                    weight_r += GetSoftWeight();
                    heavy_counts_r.Add(select_chook[1]);
                }
                else
                {
                    Debug.LogError("Error Right:\nheavy_num_r=" + heavy_num_r + "  soft_num_r=" + soft_num_r + "  weight_r=" + weight_r + " w[1]=" + w[1]);
                    break;
                }

            }

            Debug.LogError("w[0]=" + w[0] + " w[1]=" + w[1]);
            Debug.LogError("heavy_num_l=" + heavy_num_l + "  soft_num_l=" + soft_num_l);
            Debug.LogError("heavy_num_r=" + heavy_num_r + "  soft_num_r=" + soft_num_r);
            string msg = "heavy_counts_l=";
            for (int i = 0; i < heavy_counts_l.Count; i++)
                msg += heavy_counts_l[i] + ",";
            Debug.LogError(msg);
            msg = "heavy_counts_r=";
            for (int i = 0; i < heavy_counts_r.Count; i++)
                msg += heavy_counts_r[i] + ",";
            Debug.LogError(msg);
            Debug.LogError("-------------");

            heavy_counts_l = Common.BreakRank<int>(heavy_counts_l);
            heavy_counts_r = Common.BreakRank<int>(heavy_counts_r);
        }
        public void FlushData(State state)
        {
            heavy_counts_l.Clear();
            heavy_counts_r.Clear();



            int heavy_num_l = 0;
            int heavy_num_r = 0;
            int soft_num_l = 0;
            int soft_num_r = 0;

            int weight_l = 0;
            int weight_r = 0;




            switch (state)
            {
                case State.match_weight:
                case State.match_loading:
                    {
                        int random = Random.Range(0, 1000) % 3;
                        if (0 == random)
                            Debug.LogError("只拖小鸡");
                        else if (1 == random)
                            Debug.LogError("只拖大鸡");
                        else
                            Debug.LogError("大鸡小鸡都要拖");

                        //在两边相等的时候减去 temp_kill_chook_num只小鸡 保证两边在屏幕显示出来的鸡是不想等的
                        int temp_kill_chook_num = Random.Range(0, 1000) % GetHeavyWeight();
                        if (0 == temp_kill_chook_num)
                            temp_kill_chook_num = 1;

                        switch (random)
                        {
                            case 0://只拖小鸡
                            case 1://只拖大鸡
                                {
                                    int random1 = Random.Range(0, 1000) % 6;
                                    Debug.LogError("random1=" + random1);
                                    switch (random1)
                                    {
                                        case 0:
                                            heavy_num_l = 0;
                                            heavy_num_r = 1;
                                            soft_num_r = 1;
                                            soft_num_l = 1 * GetHeavyWeight() + soft_num_r;
                                            if (0 == random)
                                            {
                                                //只拖小鸡
                                                soft_num_l -= temp_kill_chook_num;

                                            }
                                            else
                                            {
                                                //只拖大鸡
                                                heavy_num_r--;
                                            }
                                            break;
                                        case 1:
                                            heavy_num_l = 1;
                                            heavy_num_r = 0;
                                            soft_num_l = 1;// Random.Range(0, 1000) % 2;
                                            soft_num_r = 1 * GetHeavyWeight() + soft_num_l;
                                            if (0 == random)
                                            {
                                                //只拖小鸡
                                                soft_num_r -= temp_kill_chook_num;
                                            }
                                            else
                                            {
                                                //只拖大鸡
                                                heavy_num_l--;
                                            }
                                            break;
                                        case 2:
                                            heavy_num_l = 1;
                                            heavy_num_r = 2;
                                            soft_num_r = Random.Range(0, 1000) % 2;
                                            soft_num_l = 1 * GetHeavyWeight() + soft_num_r;
                                            if (0 == random)
                                            {
                                                //只拖小鸡
                                                soft_num_l -= temp_kill_chook_num;
                                            }
                                            else
                                            {
                                                //只拖大鸡
                                                heavy_num_r--;
                                            }
                                            break;
                                        case 3:
                                            heavy_num_l = 2;
                                            heavy_num_r = 1;
                                            soft_num_l = Random.Range(0, 1000) % 2;
                                            soft_num_r = 1 * GetHeavyWeight() + soft_num_l;
                                            if (0 == random)
                                            {
                                                //只拖小鸡
                                                soft_num_r -= temp_kill_chook_num;
                                            }
                                            else
                                            {
                                                //只拖大鸡
                                                heavy_num_l--;
                                            }
                                            break;
                                        case 4:
                                            heavy_num_l = 2;
                                            heavy_num_r = 3;
                                            soft_num_r = Random.Range(0, 1000) % 2;
                                            soft_num_l = 1 * GetHeavyWeight() + soft_num_r;
                                            if (0 == random)
                                            {
                                                //只拖小鸡
                                                soft_num_l -= temp_kill_chook_num;
                                            }
                                            else
                                            {
                                                //只拖大鸡
                                                heavy_num_r -= (Random.Range(0, 1000) % 2 + 1);
                                            }
                                            break;
                                        case 5:
                                            heavy_num_l = 3;
                                            heavy_num_r = 2;
                                            soft_num_l = Random.Range(0, 1000) % 2;
                                            soft_num_r = 1 * GetHeavyWeight() + soft_num_l;
                                            if (0 == random)
                                            {
                                                //只拖小鸡
                                                soft_num_r -= temp_kill_chook_num;
                                            }
                                            else
                                            {
                                                //只拖大鸡
                                                heavy_num_l -= (Random.Range(0, 1000) % 2 + 1);
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 2://大鸡小鸡都要拖
                                {
                                    switch (Random.Range(0, 1000) % 2)
                                    {
                                        case 0:
                                            {
                                                heavy_num_l = 3;
                                                heavy_num_r = 1;
                                                soft_num_l = 0;
                                                soft_num_r = Random.Range(0, 1000) % GetHeavyWeight() + 1;
                                            }
                                            break;
                                        case 1:
                                            {
                                                heavy_num_r = 3;
                                                heavy_num_l = 1;
                                                soft_num_r = 0;
                                                soft_num_l = Random.Range(0, 1000) % GetHeavyWeight() + 1;
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case State.select_weight:
                case State.select_loading:
                    {
                        while (true)
                        {
                            switch (Random.Range(0, 1000) % 6)
                            {
                                case 0:
                                    heavy_num_l = 0;
                                    heavy_num_r = 1;
                                    soft_num_l = Random.Range(0, 1000) % 3 + 3;
                                    soft_num_r = Random.Range(0, 1000) % 3;
                                    break;
                                case 1:
                                    heavy_num_l = 1;
                                    heavy_num_r = 0;
                                    soft_num_l = Random.Range(0, 1000) % 3;
                                    soft_num_r = Random.Range(0, 1000) % 3 + 3;
                                    break;
                                case 2:
                                    heavy_num_l = 1;
                                    heavy_num_r = 2;
                                    soft_num_l = Random.Range(0, 1000) % 3 + 3;
                                    soft_num_r = Random.Range(0, 1000) % 3;
                                    break;
                                case 3:
                                    heavy_num_l = 2;
                                    heavy_num_r = 1;
                                    soft_num_l = Random.Range(0, 1000) % 3;
                                    soft_num_r = Random.Range(0, 1000) % 3 + 3;
                                    break;
                                case 4:
                                    heavy_num_l = 2;
                                    heavy_num_r = 3;
                                    soft_num_l = Random.Range(0, 1000) % 2 + 4;
                                    soft_num_r = Random.Range(0, 1000) % 2;
                                    break;
                                case 5:
                                    heavy_num_l = 3;
                                    heavy_num_r = 2;
                                    soft_num_l = Random.Range(0, 1000) % 2;
                                    soft_num_r = Random.Range(0, 1000) % 2 + 4;
                                    break;
                            }
                            weight_l = GetHeavyWeight() * heavy_num_l + GetSoftWeight() * soft_num_l;
                            weight_r = GetHeavyWeight() * heavy_num_r + GetSoftWeight() * soft_num_r;
                            if (weight_l != weight_r)
                                break;
                        }

                    }
                    break;
            }

            for (int i = 0; i < heavy_num_l; i++)
                heavy_counts_l.Add(select_chook[0]);
            for (int i = 0; i < heavy_num_r; i++)
                heavy_counts_r.Add(select_chook[0]);
            for (int i = 0; i < soft_num_l; i++)
                heavy_counts_l.Add(select_chook[1]);
            for (int i = 0; i < soft_num_r; i++)
                heavy_counts_r.Add(select_chook[1]);

            heavy_counts_l = Common.BreakRank<int>(heavy_counts_l);
            heavy_counts_r = Common.BreakRank<int>(heavy_counts_r);


            return;
            List<int> w = new List<int>();
            switch (state)
            {
                case State.match_weight:
                case State.match_loading:
                    w = Common.GetMutexValue(1, (int)(max_weight * 0.6f), 2);// new List<int>() { 9, 5 }; //
                    break;
                case State.select_weight:
                case State.select_loading:
                    w = Common.GetMutexValue(1, (int)(max_weight), 2);// new List<int>() { 9, 5 }; //
                    break;
            }
            Debug.LogError(state);

            //int heavy_num_l = 0;
            //int heavy_num_r = 0;
            //int soft_num_l = 0;
            //int soft_num_r = 0;

            //int weight_l = 0;
            //int weight_r = 0;


            //必定有大鸡
            if (GetHeavyWeight() * 2 + 5 < w[0])
            {
                heavy_num_l = 3;
                weight_l = GetHeavyWeight() * 3;
                heavy_counts_l = new List<int>() { select_chook[0], select_chook[0], select_chook[0] };
            }
            else if (GetHeavyWeight() + 5 < w[0])
            {
                heavy_num_l = 2;
                weight_l = GetHeavyWeight() * 2;
                heavy_counts_l = new List<int>() { select_chook[0], select_chook[0] };
            }
            else if (5 < w[0])
            {
                heavy_num_l = 1;
                weight_l = GetHeavyWeight();
                heavy_counts_l = new List<int>() { select_chook[0] };
            }

            if (GetHeavyWeight() * 2 + 5 <= w[1])
            {
                heavy_num_r = 3;
                weight_r = GetHeavyWeight() * 3;
                heavy_counts_r = new List<int>() { select_chook[0], select_chook[0], select_chook[0] };
            }
            else if (GetHeavyWeight() + 5 < w[1])
            {
                heavy_num_r = 2;
                weight_r = GetHeavyWeight() * 2;
                heavy_counts_r = new List<int>() { select_chook[0], select_chook[0] };
            }
            else if (5 < w[1])
            {
                heavy_num_r = 1;
                weight_r = GetHeavyWeight();
                heavy_counts_r = new List<int>() { select_chook[0] };
            }




            //左边
            while (weight_l != w[0])
            {
                int h = w[0] - weight_l;


                bool can_select_heavy = true;
                bool can_select_soft = true;
                if (heavy_num_l >= 3)
                    can_select_heavy = false;
                if (soft_num_l >= 5)
                    can_select_soft = false;
                if (h < 3)
                    can_select_heavy = false;


                if (can_select_heavy && can_select_soft)
                {
                    int temp = Random.Range(0, 1000) % 3;
                    if (temp == 0)
                    {
                        heavy_num_l++;
                        weight_l += GetHeavyWeight();
                        heavy_counts_l.Add(select_chook[0]);
                    }
                    else
                    {
                        soft_num_l++;
                        weight_l += GetSoftWeight();
                        heavy_counts_l.Add(select_chook[1]);
                    }
                }
                else if (can_select_heavy)
                {
                    heavy_num_l++;
                    weight_l += GetHeavyWeight();
                    heavy_counts_l.Add(select_chook[0]);
                }
                else if (can_select_soft)
                {
                    soft_num_l++;
                    weight_l += GetSoftWeight();
                    heavy_counts_l.Add(select_chook[1]);
                }
                else
                {
                    Debug.LogError("Error Left:\nheavy_num_l=" + heavy_num_l + "  soft_num_l=" + soft_num_l + "  weight_l=" + weight_l + " w[0]=" + w[0]);
                    break;
                }

            }

            //右边
            while (weight_r != w[1])
            {
                int h = w[1] - weight_r;


                bool can_select_heavy = true;
                bool can_select_soft = true;
                if (heavy_num_r >= 3)
                    can_select_heavy = false;
                if (soft_num_r >= 5)
                    can_select_soft = false;
                if (h < 3)
                    can_select_heavy = false;


                if (can_select_heavy && can_select_soft)
                {
                    int temp = Random.Range(0, 1000) % 3;
                    if (temp == 0)
                    {
                        heavy_num_r++;
                        weight_r += GetHeavyWeight();
                        heavy_counts_r.Add(select_chook[0]);
                    }
                    else
                    {
                        soft_num_r++;
                        weight_r += GetSoftWeight();
                        heavy_counts_r.Add(select_chook[1]);
                    }
                }
                else if (can_select_heavy)
                {
                    heavy_num_r++;
                    weight_r += GetHeavyWeight();
                    heavy_counts_r.Add(select_chook[0]);
                }
                else if (can_select_soft)
                {
                    soft_num_r++;
                    weight_r += GetSoftWeight();
                    heavy_counts_r.Add(select_chook[1]);
                }
                else
                {
                    Debug.LogError("Error Right:\nheavy_num_r=" + heavy_num_r + "  soft_num_r=" + soft_num_r + "  weight_r=" + weight_r + " w[1]=" + w[1]);
                    break;
                }

            }

            Debug.LogError("w[0]=" + w[0] + " w[1]=" + w[1]);
            Debug.LogError("heavy_num_l=" + heavy_num_l + "  soft_num_l=" + soft_num_l);
            Debug.LogError("heavy_num_r=" + heavy_num_r + "  soft_num_r=" + soft_num_r);
            string msg = "heavy_counts_l=";
            for (int i = 0; i < heavy_counts_l.Count; i++)
                msg += heavy_counts_l[i] + ",";
            Debug.LogError(msg);
            msg = "heavy_counts_r=";
            for (int i = 0; i < heavy_counts_r.Count; i++)
                msg += heavy_counts_r[i] + ",";
            Debug.LogError(msg);
            Debug.LogError("-------------");

            heavy_counts_l = Common.BreakRank<int>(heavy_counts_l);
            heavy_counts_r = Common.BreakRank<int>(heavy_counts_r);
        }
        
    }
    public enum State
    {
        show,//展示规则

        select_weight,//选择那边重
        select_loading,//生成鸡中

        match_weight,//分配鸡
        match_loading//分配鸡中

    }


}
