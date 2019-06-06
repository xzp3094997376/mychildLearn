using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

public class ChookPkChook : MonoBehaviour {
    public static List<ParticleSystem> gEffectFly0;
    public static List<ParticleSystem> gEffectFly1;
    public static List<ParticleSystem> gEffectFly2;
    public static List<ParticleSystem> gEffectTanxi;
    //ParticleSystem mEffectTanxi;

    // 什么鸡,0-公鸡，1-母鸡，2-小鸡
    public int mFlag { get; set; }
    //状态0-下面，1-上升中，2-木板上
    public int mState { get; set; }

    SkeletonGraphic mSpine { get; set; }
    public RectTransform mRtran { get; set; }
    GameObject mImage { get; set; }
    BoxCollider mBox { get; set; }
    public int mType { get; set; }
    public int mWeight { get; set; }
    public int mPosInPulleyIndex { get; set; }
    bool mIsBusy { get; set; }
    bool mIsUnderGround { get; set; }
    public bool mCanFly { get; set; }//能否点击飞
    public bool mIsRuning { get; set; }


	public void Start ()
    {
        mRtran = gameObject.GetComponent<RectTransform>();
        mRtran.sizeDelta = Vector2.zero;

        mImage = UguiMaker.newGameObject("mImage", transform);
        //mImage = UguiMaker.newImage("Image", transform, "chookpk_sprite", "ji0", false);
        //mImage.rectTransform.anchorMin = new Vector2(0.5f, 0);
        //mImage.rectTransform.anchorMax = new Vector2(0.5F, 0);
        //mImage.rectTransform.pivot = new Vector2(0.5f, 0);
        //mImage.rectTransform.anchoredPosition = Vector2.zero;

        //Button btn = mImage.gameObject.AddComponent<Button>();
        //btn.transition = Selectable.Transition.None;
        //btn.onClick.AddListener(OnClk);

        mBox = mImage.gameObject.AddComponent<BoxCollider>();

        gameObject.SetActive(false);

        ChookPkCtl.instance.temp_start_count--;

    }
	
    //在pushin函数调用前调用才正确
    public void FlushSibling_WhenInLanzi()
    {
        if(mType == 2)
        {
            transform.SetAsLastSibling();
        }
        else
        {
            int index = ChookPkCtl.instance.mPulley.GetPosIndex(IsRight() ? "right" : "left", mType);
            transform.SetSiblingIndex(1 + index);
        }
        
    }
    public void SetData(int chook_type, bool is_under_ground, bool can_fly)
    {
        //Debug.LogError("ji" + chook_type);
        transform.localEulerAngles = Vector3.zero;
        //mImage.sprite = ResManager.GetSprite("chookpk_sprite", "ji" + chook_type);
        //mImage.color = Color.white;
        //mImage.rectTransform.anchoredPosition = Vector2.zero;

        mType = chook_type;
        mIsUnderGround = is_under_ground;
        mCanFly = can_fly;
        mWeight = ChookPkCtl.instance.mGuanka.weights[mType];

        mBox.enabled = true;
        mIsRuning = false;

        switch (chook_type)
        {
            case 0:
                mBox.size = new Vector3(90, 144, 1);
                mBox.center = new Vector3(2, 72, 0);
                break;
            case 1:
                mBox.size = new Vector3(100, 111, 2);
                mBox.center = new Vector3(0, 59, 0);
                break;
            case 2:
                mBox.size = new Vector3(62, 62, 5);
                mBox.center = new Vector3(0, 35, 0);
                break;
        }

        if(null != mSpine)
        {
            if(!mSpine.gameObject.name.Contains(mType.ToString()))
            {
                Destroy(mSpine.gameObject);
                mSpine = null;
            }
        }
        if(null == mSpine)
        {
            Vector3[] poss = new Vector3[] { new Vector3(32, 5, 0), new Vector3(4, 0, 0), new Vector3(0, 5, 0) };
            mSpine = ResManager.GetPrefab("chookpk_prefab", "chook" + mType).GetComponent<SkeletonGraphic>();
            UguiMaker.InitGameObj(mSpine.gameObject, mImage.transform, "spine" + mType.ToString(), poss[mType], new Vector3(0.5f, 0.5f, 1));
        }

    }
    public void SetRandomForward()
    {
        //Debug.LogError("SetRandomForward");
        mImage.transform.localScale = new Vector3(Random.Range(0, 1000) % 2 == 0 ? 1 : -1, 1, 1);
    }
    public void SetForward(string forward)
    {
        switch(forward)
        {
            case "left":
                mImage.transform.localScale = new Vector3(-1, 1, 1);
                break;
            case "right":
                mImage.transform.localScale = new Vector3(1, 1, 1);
                break;
        }
    }
    public void SetBusy(bool is_busy)
    {
        mIsBusy = is_busy;
        if(!is_busy)
            gameObject.SetActive(false);
    }
    public bool IsBusy()
    {
        return mIsBusy;
    }
    public bool IsRight()
    {
        if(mImage.transform.localScale.x > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public bool IsUnderGround()
    {
        return mIsUnderGround;
    }
    



    public void PushInCar(ChookPkCar car)
    {
        StartCoroutine(TPushInCar(car));
    }
    IEnumerator TPushInCar(ChookPkCar car)
    {
        transform.SetParent(car.transform);
        Vector3 pos0 = mRtran.anchoredPosition;
        Vector3 pos1 = new Vector3(Random.Range( 50, 172), 15);
        
        for(float i = 0; i < 1f; i += 0.1f)
        {
            transform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 1), Vector3.one, i);
            mRtran.anchoredPosition = Vector2.Lerp(pos0, pos1, i) + new Vector2(0, Mathf.Sin(Mathf.PI * i) * 50);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition = pos1;

    }

    public void PlayTanxi()
    {
        ParticleSystem effect = null;
        for(int i = 0; i < gEffectTanxi.Count; i++)
        {
            if(!gEffectTanxi[i].isPlaying)
            {
                effect = gEffectTanxi[i];
                break;
            }
        }
        if (null == effect)
        {
            effect = ResManager.GetPrefab("chookpk_prefab", "chook_tanxi").GetComponent<ParticleSystem>();
            gEffectTanxi.Add(effect);
        }
        UguiMaker.InitGameObj(effect.gameObject, mImage.transform, "effect_chook_tanxi", new Vector3(mBox.size.x * -0.5f, mBox.size.y, 0), Vector3.one);
        effect.Play();

    }
    public void PlayJimao()
    {
        ParticleSystem effect_fly = null;
        switch(mType)
        {
            case 0:
                for (int i = 0; i < gEffectFly0.Count; i++)
                {
                    if (!gEffectFly0[i].isPlaying)
                    {
                        effect_fly = gEffectFly0[i];
                    }
                }
                if (null == effect_fly)
                {
                    effect_fly = ResManager.GetPrefab("chookpk_prefab", "jimao0").GetComponent<ParticleSystem>();
                    UguiMaker.InitGameObj(effect_fly.gameObject, ChookPkCtl.instance.transform, "effect_fly0", Vector3.zero, Vector3.one);
                    gEffectFly0.Add(effect_fly);
                }
                break;
            case 1:
                for (int i = 0; i < gEffectFly1.Count; i++)
                {
                    if (!gEffectFly1[i].isPlaying)
                    {
                        effect_fly = gEffectFly1[i];
                    }
                }
                if (null == effect_fly)
                {
                    effect_fly = ResManager.GetPrefab("chookpk_prefab", "jimao1").GetComponent<ParticleSystem>();
                    UguiMaker.InitGameObj(effect_fly.gameObject, ChookPkCtl.instance.transform, "effect_fly1", Vector3.zero, Vector3.one);
                    gEffectFly1.Add(effect_fly);
                }
                break;
            case 2:
                for (int i = 0; i < gEffectFly2.Count; i++)
                {
                    if (!gEffectFly2[i].isPlaying)
                    {
                        effect_fly = gEffectFly2[i];
                    }
                }
                if (null == effect_fly)
                {
                    effect_fly = ResManager.GetPrefab("chookpk_prefab", "jimao2").GetComponent<ParticleSystem>();
                    UguiMaker.InitGameObj(effect_fly.gameObject, ChookPkCtl.instance.transform, "effect_fly2", Vector3.zero, Vector3.one);
                    gEffectFly2.Add(effect_fly);
                }
                break;
        }
        effect_fly.transform.position = transform.position;
        effect_fly.transform.localPosition += new Vector3(0, 30, 0);
        effect_fly.Play();

    }
    public void Fly()
    {
        ChookPkCtl.instance.mSound.PlayShort("chookpk_sound", "09-死掉", 0.2f);
        transform.SetParent(ChookPkCtl.instance.transform);
        gameObject.SetActive(false);
        SetBusy(false);
        PlayJimao();

        //StartCoroutine(TFly());
    }
    //IEnumerator TFly()
    //{
    //    transform.SetParent(ChookPkCtl.instance.transform);
    //    Vector2 pos0 = mRtran.anchoredPosition;
    //    Vector2 pos1 = pos0 + new Vector2(0, 100);
    //    for (float i =0; i <1f; i += 0.05f)
    //    {
    //        mImage.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), i);
    //        mRtran.anchoredPosition = Vector2.Lerp(pos0, pos1, i);
    //        yield return new WaitForSeconds(0.01f);
    //    }
    //    gameObject.SetActive(false);
    //    SetBusy(false);
    //}
    


    public void RunOut(float end_x)
    {
        StartCoroutine(TRunOut(end_x));
    }
    IEnumerator TRunOut(float end_x)
    {
        PlaySpine(Animation.Run, true);
        transform.SetParent(ChookPkCtl.instance.transform);
        transform.localEulerAngles = Vector3.zero;
        float speed = 10;
        switch(mType)
        {
            case 0:
                speed = 12;
                break;
            case 1:
                speed = 11;
                break;
            case 2:
                speed = 10;
                break;
        }
        if (end_x > mRtran.anchoredPosition.x)
        {
            while(end_x > mRtran.anchoredPosition.x)
            {
                mRtran.anchoredPosition += new Vector2(speed, 0);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            while (end_x < mRtran.anchoredPosition.x)
            {
                mRtran.anchoredPosition -= new Vector2(speed, 0);
                yield return new WaitForSeconds(0.01f);
            }
        }
        SetBusy(false);

    }


    public void AddChookInCar()
    { 
        //预先登记，
        if (IsRight())
        {
            ChookPkCtl.instance.mCarLeft.AddChook(this);
            //ChookPkCtl.instance.mPulley.PushIn_R(this);
        }
        else if (!IsRight())
        {
            ChookPkCtl.instance.mCarRight.AddChook(this);
            //ChookPkCtl.instance.mPulley.PushIn_L(this);
        }
    }
    public void RunInUnderGround()
    {
        if (!mIsUnderGround)
        {
            return;
        }

        AddChookInCar();
        if (Mathf.Abs(mRtran.anchoredPosition.x) > 354)
        {
            JumpInCar();
        }
        else
        {
            StopCoroutine("TRunInUnderGround");
            StartCoroutine("TRunInUnderGround");
        }

    }
    IEnumerator TRunInUnderGround()
    {
        ChookPkCtl.instance.mSound.PlayShort("chookpk_sound", "04-鸡叫");
        ChookPkCtl.instance.mSound.PlayBg2("chookpk_sound", "05-鸡走");
        PlaySpine(Animation.Run, true);
        mIsRuning = true;
        mBox.enabled = false;
        float speed = 7;
        bool isRight = IsRight();
        while(true)
        {
            if (Mathf.Abs(mRtran.anchoredPosition.x) > 354)
            {
                break;
            }

            if(isRight)
            {
                mRtran.anchoredPosition -= new Vector2(speed, 0);
            }
            else
            {
                mRtran.anchoredPosition += new Vector2(speed, 0);
            }
            yield return new WaitForSeconds(0.01f);
        }
        ChookPkCtl.instance.mSound.StopBg2();

        JumpInCar();
        mBox.enabled = true;
    }

    Vector3 temp_jump_scale = new Vector3(0.7f, 0.7f, 1);
    public void JumpInCar()
    {
        if (mIsUnderGround && Mathf.Abs(mRtran.anchoredPosition.x) > 354)
        {
            //车在上升中,方向因为鸡在下面的方向是反的
            if (IsRight() && ChookPkCtl.instance.mCarLeft.uping)
            {
                mIsRuning = false;
                PlaySpine(Animation.Squat, true);
                return;
            }
            else if (!IsRight() && ChookPkCtl.instance.mCarRight.uping)
            {
                mIsRuning = false;
                PlaySpine(Animation.Squat, true);
                return;
            }

            AddChookInCar();
            StartCoroutine(TJumpInCar());
        }
    }
    IEnumerator TJumpInCar()
    {
        PlaySpine(Animation.Run, true);
        mIsUnderGround = false;
        mIsRuning = true;
        Vector2 pos0 = Vector2.zero;
        Vector2 pos1 = Vector2.zero;
        if (IsRight())
        {
            transform.SetParent(ChookPkCtl.instance.mCarLeft.mCar.transform);
            pos0 = mRtran.anchoredPosition;
            pos1 = ChookPkCtl.instance.mCarLeft.GetRandomPos();
            ChookPkCtl.instance.mCarLeft.AddChook(this);
        }
        else
        {
            transform.SetParent(ChookPkCtl.instance.mCarRight.mCar.transform);
            pos0 = mRtran.anchoredPosition;
            pos1 = ChookPkCtl.instance.mCarRight.GetRandomPos();
            ChookPkCtl.instance.mCarRight.AddChook(this);

        }

        for (float i = 0; i < 1f; i += 0.08f)
        {
            mRtran.anchoredPosition = Vector2.Lerp(pos0, pos1, i) + new Vector2(0, Mathf.Sin(Mathf.PI * i) * 50);
            transform.localScale = Vector3.Lerp(Vector3.one, temp_jump_scale, i);
            yield return new WaitForSeconds(0.01f);
        }

        mIsRuning = false;
        if (IsRight())
        {
            SetForward("left");
            //transform.SetParent(ChookPkCtl.instance.mCarLeft.mCar.transform);
            ChookPkCtl.instance.mCarLeft.CarUp();
        }
        else
        {
            SetForward("right");
            //transform.SetParent(ChookPkCtl.instance.mCarRight.mCar.transform);
            ChookPkCtl.instance.mCarRight.CarUp();
        }


    }

    
    public void JumpInPulley()
    {
        StartCoroutine(TJumpInPulley());
    }
    IEnumerator TJumpInPulley()
    {
        //Transform tran_lanzi = null;
        PlaySpine(Animation.Fly, true);
        Vector3 random_pulley_pos = Vector3.zero;

        if (IsRight())
        {
            transform.SetParent(ChookPkCtl.instance.mPulley.mLanziRight.transform);
            //tran_lanzi = ChookPkCtl.instance.mPulley.mLanziRight;
            random_pulley_pos = ChookPkCtl.instance.mPulley.GetStandPos("right", this);
            FlushSibling_WhenInLanzi();
            ChookPkCtl.instance.mPulley.PushIn_R(this);
        }
        else
        {
            transform.SetParent(ChookPkCtl.instance.mPulley.mLanziLeft.transform);
            //tran_lanzi = ChookPkCtl.instance.mPulley.mLanziLeft;
            random_pulley_pos = ChookPkCtl.instance.mPulley.GetStandPos("left", this);
            FlushSibling_WhenInLanzi();
            ChookPkCtl.instance.mPulley.PushIn_L(this);
        }
        transform.localScale = Vector3.one;
        transform.localEulerAngles = Vector3.zero;

        Vector3 pos0 = transform.localPosition;
        float h = Random.Range(150, 250);// Mathf.Abs(random_pulley_pos.y - pos0.y);
        for (float i = 0; i < 1f; i += 0.025f)
        {
            transform.localPosition = Vector3.Lerp(pos0, random_pulley_pos, i) +
                new Vector3(0, Mathf.Sin(Mathf.PI * i) * h, 0);
            transform.localScale = Vector3.Lerp( temp_jump_scale, Vector3.one, i);

            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = random_pulley_pos;
        transform.localScale = Vector3.one;

        PlaySpine(Animation.Idle, true);

    }


    public void PlaySpine(Animation anim, bool is_loop)
    {
        if (null == mSpine)
            return;
        mSpine.AnimationState.SetAnimation(0, anim.ToString(), is_loop);
    }
    public enum Animation
    {
        Fly,
        Idle,
        Run,
        Squat,
    }

}
