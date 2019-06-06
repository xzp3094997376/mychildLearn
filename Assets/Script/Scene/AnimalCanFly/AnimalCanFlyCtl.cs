using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AnimalCanFlyCtl : BaseScene
{

    public static AnimalCanFlyCtl instance = null;


    public Image mBgSky { get; set; }
    public Image mBgGroundMid { get; set; }
    public Image mBgGroundUnder { get; set; }
    public Image mBgTreeLeft { get; set; }
    public Image mBgTreeRight { get; set; }
    
    public RectTransform mRtranSceen { get; set; }
    public RectTransform mRtranSky { get; set; }


    public SoundManager mSound { get; set; }
    public AnimalCanFlyBalance mBalanceRight { get; set; }
    public AnimalCanFlyBalance mBalanceLeft { get; set; }
    public AnimalCanFlyCalculate mCalculate { get; set; }
    public AnimalCanFlyBall mBallRight { get; set; }
    public AnimalCanFlyBall mBallLeft { get; set; }
    public List<AnimalCanFlyAnimal> mAnimals = new List<AnimalCanFlyAnimal>();//天空中的动物
    public List<ParticleSystem> mEffectShine = new List<ParticleSystem>();

    public AudioSource mAudioAnimal0, mAudioAnimal1, mAudioAnimal2;

    public int mdata_guanka = 0;
    public int mdata_guanka_all = 3;
    public int mdata_animal_id0 = 0;
    public int mdata_animal_id1 = 0;
    public int mdata_animal_id2 = 0;
    public int mdata_animal_weight0 = 0;
    public int mdata_animal_weight1 = 0;
    public int mdata_animal_weight2 = 0;
    public int mdata_animal_num0 = 0;//固定的
    public int mdata_animal_num1 = 0;//随用户操作变动的
    public List<int> mdata_guanka_types { get; set; }//记录每一关的配对模式，避免重复用列表预先存好
    public Dictionary<int, List<int>> mdata_animal_weight { get; set; }//数据，存储动物的体重




    //int temp_next_count = 0;

    void Awake()
    {
        instance = this;
        mSound = gameObject.AddComponent<SoundManager>();
        mSound.SetAbName("animalcanfly_sound");


    }
    void Start ()
    {

        mSceneType = SceneEnum.AnimalCanFly;
        CallLoadFinishEvent();
        Reset();



    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                #region 播放动物叫声
                if (hit.collider.gameObject.name.Equals("mSitRight"))
                {
                    switch (hit.collider.gameObject.transform.parent.parent.GetComponent<AnimalCanFlyBalance>().mdata_side)
                    {
                        case "right":
                            PlayAnimalSound(2);
                            mBalanceRight.PlayAnimal("right");
                            break;
                        case "left":
                            PlayAnimalSound(1);
                            mBalanceLeft.PlayAnimal("right");
                            break;
                    }


                }
                else if (hit.collider.gameObject.name.Equals("mSitLeft"))
                {
                    switch (hit.collider.gameObject.transform.parent.parent.GetComponent<AnimalCanFlyBalance>().mdata_side)
                    {
                        case "right":
                            PlayAnimalSound(1);
                            mBalanceRight.PlayAnimal("left");
                            break;
                        case "left":
                            PlayAnimalSound(0);
                            mBalanceLeft.PlayAnimal("left");
                            break;
                    }

                }

                #endregion
            }

        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast( ray, out hit, 100))
            {
                AnimalCanFlyBalance com = hit.collider.gameObject.GetComponent<AnimalCanFlyBalance>();
                if(null == com)
                {
                    if(null == AnimalCanFlyBalance.gSelect)
                    {

                    }
                    else
                    {
                        AnimalCanFlyBalance.gSelect.ResetShakeSub();
                        AnimalCanFlyBalance.gSelect = null;
                    }

                }
                else
                {
                    if (null == AnimalCanFlyBalance.gSelect)
                    {
                        AnimalCanFlyBalance.gSelect = com;
                        AnimalCanFlyBalance.gSelect.mdata_sub = 0.4f;
                        AnimalCanFlyBalance.gSelect.PlayEffect();
                    }
                    else
                    {
                        if(com == AnimalCanFlyBalance.gSelect)
                        {
                            AnimalCanFlyBalance.gSelect.mdata_sub = 0.4f;
                            AnimalCanFlyBalance.gSelect.PlayEffect();
                        }
                        else
                        {
                            AnimalCanFlyBalance.gSelect.ResetShakeSub();
                            AnimalCanFlyBalance.gSelect = com;
                            AnimalCanFlyBalance.gSelect.mdata_sub = 0.4f;
                            AnimalCanFlyBalance.gSelect.PlayEffect();

                        }
                    }
                }

            

            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(null != AnimalCanFlyBalance.gSelect)
            {
                AnimalCanFlyBalance.gSelect.ResetShakeSub();
                AnimalCanFlyBalance.gSelect = null;
            }
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach(RaycastHit h in hits)
            {
                AnimalCanFlyAnimal com = h.collider.gameObject.GetComponent<AnimalCanFlyAnimal>();
                if(null != com)
                {
                    if (mdata_animal_num1 > 0)
                        mdata_animal_num1--;
                    FallAnimal(com);
                    mCalculate.FlushBtnUpDown();
                    mCalculate.Flush(mdata_animal_num1);
                    ParticleSystem effect = PlayEffectShine();
                    effect.transform.localPosition = com.mRtran.anchoredPosition3D + new Vector3(0, 15, 0);
                    mSound.PlayShortDefaultAb("打开菜单");
                    break;
                }
            }
        }

    }
	
    public void Reset()
    {
        //TopTitleCtl.instance.ResetNotMoveIn();
        mdata_guanka_types = Common.GetMutexValue(0, 4, mdata_guanka_all);//new List<int>() { 0, 0, 0, 0 };// 
        mdata_guanka = 0;

        //FlushGuankaData();
        StartCoroutine(TStart());
        Invoke("InvokePlayTip1", 0.5f);
    }
    public void InvokePlayTip1()
    {
        mSound.PlayTipDefaultAb("tip规则", 1, true);

    }
    public void FlushGuankaData()
    {
        switch(mdata_guanka_types[mdata_guanka])
        {
            case 0://9-3-1
                mdata_animal_weight0 = 9;
                mdata_animal_weight1 = 3;
                mdata_animal_weight2 = 1;
                break;
            case 1://8-4-2
                mdata_animal_weight0 = 8;
                mdata_animal_weight1 = 4;
                mdata_animal_weight2 = 2;
                break;
            //case 2://8-4-1
            //    mdata_animal_weight0 = 8;
            //    mdata_animal_weight1 = 4;
            //    mdata_animal_weight2 = 1;
            //    break;
            //case 2://8-2-1
            //    mdata_animal_weight0 = 8;
            //    mdata_animal_weight1 = 2;
            //    mdata_animal_weight2 = 1;
            //    break;
            case 2://6-3-1
                mdata_animal_weight0 = 6;
                mdata_animal_weight1 = 3;
                mdata_animal_weight2 = 1;
                break;
            case 3://6-2-1
                mdata_animal_weight0 = 6;
                mdata_animal_weight1 = 2;
                mdata_animal_weight2 = 1;
                break;
            case 4://4-2-1
                mdata_animal_weight0 = 4;
                mdata_animal_weight1 = 2;
                mdata_animal_weight2 = 1;
                break;
        }
        mdata_animal_id0 = mdata_animal_weight[mdata_animal_weight0][Random.Range(0, 1000) % mdata_animal_weight[mdata_animal_weight0].Count];
        mdata_animal_id1 = mdata_animal_weight[mdata_animal_weight1][Random.Range(0, 1000) % mdata_animal_weight[mdata_animal_weight1].Count];
        mdata_animal_id2 = mdata_animal_weight[mdata_animal_weight2][Random.Range(0, 1000) % mdata_animal_weight[mdata_animal_weight2].Count];

        mdata_animal_num0 = Random.Range(0, 1000) % 2 + 1;
        mdata_animal_num1 = 0;

        if(0 < mAnimals.Count)
        {
            for(int i = 0; i < mAnimals.Count; i++)
            {
                Destroy(mAnimals[i].gameObject);
            }
            mAnimals.Clear();
        }

    }
    public void ShootAnimal(int animal_id, bool can_click, float delay_time)
    {
        AnimalCanFlyAnimal free = GetAnimal(animal_id);
        free.SetBoxEnable(true);
        free.Shoot();

    }
    public void FallAnimal(int animal_id)
    {
        AnimalCanFlyAnimal animal = null;
        for(int i = 0; i < mAnimals.Count; i++)
        {
            if(mAnimals[i].gameObject.activeSelf && mAnimals[i].mdata_animal_id == animal_id)
            {
                animal = mAnimals[i];
                break;
            }
        }
        if (null == animal)
            return;
        FallAnimal(animal);

    }
    public void FallAnimal(AnimalCanFlyAnimal animal)
    {
        if (null == animal)
            return;
        animal.mdata_animal_id = 0;
        animal.Fall();
        Global.instance.PlayBtnClickAnimation(animal.transform);
    }
    public void PlayAnimalSound(int index)
    {
        switch(index)
        {
            case 0:
                if(!mAudioAnimal0.isPlaying)
                {
                    mAudioAnimal0.Play();
                }
                break;
            case 1:
                if (!mAudioAnimal1.isPlaying)
                {
                    mAudioAnimal1.Play();
                }
                break;
            case 2:
                if (!mAudioAnimal2.isPlaying)
                {
                    mAudioAnimal2.Play();
                }
                break;
        }
    }
    public ParticleSystem PlayEffectShine()
    {
        ParticleSystem free = null;
        for(int i = 0; i < mEffectShine.Count; i++)
        {
            if(!mEffectShine[i].isPlaying)
            {
                free = mEffectShine[i];
                break;
            }
        }
        if(null == free)
        {
            Vector3 scale = new Vector3(0.7f, 0.7f, 0.7f);
            free = ResManager.GetPrefab("effect_shine0", "effect_shine0").GetComponent<ParticleSystem>();
            UguiMaker.InitGameObj(free.gameObject, mRtranSky, "effect_shine0", Vector3.zero, scale);
            free.transform.Find("1").localScale = scale;
            free.transform.Find("2").localScale = scale;


            mEffectShine.Add(free);
        }
        //free.transform.localPosition = Common.getMouseLocalPos(transform);
        free.Play();

        return free;
    }
    public AnimalCanFlyAnimal GetAnimal(int animal_id)
    {
        AnimalCanFlyAnimal free = null;
        for (int i = 0; i < mAnimals.Count; i++)
        {
            if (!mAnimals[i].gameObject.activeSelf)
            {
                free = mAnimals[i];
                free.gameObject.SetActive(true);
                break;
            }
        }
        if (null == free)
        {
            free = UguiMaker.newGameObject("animal", mRtranSceen).AddComponent<AnimalCanFlyAnimal>();
            mAnimals.Add(free);
        }
        free.SetData(animal_id);
        return free;
    }

    public int Callbalc_Up()
    {
        if (mdata_animal_num1 == 18)
            return mdata_animal_num1;
        mdata_animal_num1++;
        mCalculate.Flush(mdata_animal_num1);
        ShootAnimal(mdata_animal_id2, true, 0);
        return mdata_animal_num1;
    }
    public int Callback_Down()
    {
        if (mdata_animal_num1 == 0)
            return mdata_animal_num1;
        mdata_animal_num1--;
        mCalculate.Flush(mdata_animal_num1);
        FallAnimal(mdata_animal_id2);
        return mdata_animal_num1;
    }
    public void Callback_OK()
    {
        int count0 = mdata_animal_num0 * mdata_animal_weight0;
        int count2 = mdata_animal_num1 * mdata_animal_weight2;
        if(count0 == count2)
        {
            //Debug.Log("成功");
            StartCoroutine(TNext());
            mSound.PlayTipDefaultAb("tip答对了" + Random.Range(0, 1000) % 2);
            //mSound.PlayTipListDefaultAb(new List<string> { "tip答对了" + Random.Range(0, 1000) % 2, "animalcanfly_sound" }, new List<float>() { 1, 1 });
            mSound.PlayShortDefaultAb("04-星星-02");
        }
        else
        {
            //Debug.Log("失败");
            mCalculate.PlayError();
            if(2 >= Mathf.Abs(count0 - count2))
            {
                mSound.PlayTipDefaultAb("tip就差一点点继续加油");
            }
            else
            {
                mSound.PlayTipDefaultAb("tip数字不对" + Random.Range(0, 1000) % 2);
                
            }
            
        }

    }

    bool isFirstTime = true;
    IEnumerator TStart()
    {
        if(null == mBgSky)
        {
            //scene
            mBgSky = UguiMaker.newImage("mBgSky", transform, "animalcanfly_sprite", "bg_sky", false);
            mRtranSky = UguiMaker.newGameObject("mRtranSky", transform).GetComponent<RectTransform>();
            mBgGroundUnder = UguiMaker.newImage("mBgGroundUnder", transform, "animalcanfly_sprite", "bg_ground_under", false);
            mRtranSceen = UguiMaker.newGameObject("mRtranSceen", transform).GetComponent<RectTransform>();
            mBgGroundMid = UguiMaker.newImage("mBgGroundMid", transform, "animalcanfly_sprite", "bg_ground_mid", false);
            mBgTreeLeft = UguiMaker.newImage("mBgTreeLeft", transform, "animalcanfly_sprite", "bg_tree_l", false);
            mBgTreeRight = UguiMaker.newImage("mBgTreeRight", transform, "animalcanfly_sprite", "bg_tree_r", false);
            mBgSky.rectTransform.anchoredPosition = new Vector2(0, 123);
            mBgGroundUnder.rectTransform.anchoredPosition = new Vector2(0, -342);
            mBgGroundMid.rectTransform.anchoredPosition = new Vector2(0, -176);
            mBgTreeLeft.rectTransform.pivot = new Vector2(0, 1);
            mBgTreeRight.rectTransform.pivot = new Vector2(1, 1);
            mBgTreeLeft.rectTransform.anchoredPosition = new Vector2(Common.gWidth * -0.5f, Common.gHeight * 0.5f); ;
            mBgTreeRight.rectTransform.anchoredPosition = new Vector2(Common.gWidth * 0.5f, Common.gHeight * 0.5f); ;

            //balance
            mBalanceLeft = UguiMaker.newGameObject("mBalanceLeft", mBgGroundMid.transform).AddComponent<AnimalCanFlyBalance>();
            mBalanceRight = UguiMaker.newGameObject("mBalanceRight", mBgGroundMid.transform).AddComponent<AnimalCanFlyBalance>();
            mBalanceLeft.mdata_side = "left";
            mBalanceRight.mdata_side = "right";

            yield return new WaitForSeconds(0.2f);

            //ball
            mBallLeft = UguiMaker.newGameObject("mBallLeft", mRtranSky.transform).AddComponent<AnimalCanFlyBall>();
            mBallRight = UguiMaker.newGameObject("mBallRight", mRtranSky.transform).AddComponent<AnimalCanFlyBall>();
            mBallLeft.mdata_side = "left";
            mBallRight.mdata_side = "right";
            mBallLeft.Init();
            mBallRight.Init();


            mdata_animal_weight = new Dictionary<int, List<int>>();
            mdata_animal_weight.Add(1, new List<int>() { 13, 10 });//燕子，鸽子
                                                                   //mdata_animal_weight.Add(2, new List<int>() { 2, 4 });//鸡，鸭
            mdata_animal_weight.Add(2, new List<int>() { 4 });//鸭,由于头像问题排除了鸡
            mdata_animal_weight.Add(3, new List<int>() { 8, 5, 11 });//猫头鹰，鹅，兔
            mdata_animal_weight.Add(4, new List<int>() { 7, 12 });//豹，羊
            mdata_animal_weight.Add(6, new List<int>() { 14, 1, 9 });//老虎，熊，猪
            mdata_animal_weight.Add(8, new List<int>() { 6 });//马
            mdata_animal_weight.Add(9, new List<int>() { 3 });//牛

            mCalculate = UguiMaker.newGameObject("mCalculate", mBgGroundMid.transform).AddComponent<AnimalCanFlyCalculate>();
            mCalculate.Init();

            mAudioAnimal0 = gameObject.AddComponent<AudioSource>();
            mAudioAnimal1 = gameObject.AddComponent<AudioSource>();
            mAudioAnimal2 = gameObject.AddComponent<AudioSource>();

        }

        FlushGuankaData();

        mBallLeft.ResetData();
        mBallRight.ResetData();

        mCalculate.Flush(mdata_animal_num0, mdata_animal_id0, 0, mdata_animal_id2);
        mCalculate.Show();
        
        mBalanceLeft.Show(mdata_animal_id0, mdata_animal_id1, 1, mdata_animal_weight0 / mdata_animal_weight1);
        mBalanceRight.Show(mdata_animal_id1, mdata_animal_id2, 1, mdata_animal_weight1 / mdata_animal_weight2);

        mAudioAnimal0.clip = ResManager.GetClip("aa_animal_sound", MDefine.GetAnimalNameByID_CH(AnimalCanFlyCtl.instance.mdata_animal_id0) + "1");
        mAudioAnimal1.clip = ResManager.GetClip("aa_animal_sound", MDefine.GetAnimalNameByID_CH(AnimalCanFlyCtl.instance.mdata_animal_id1) + "1");
        mAudioAnimal2.clip = ResManager.GetClip("aa_animal_sound", MDefine.GetAnimalNameByID_CH(AnimalCanFlyCtl.instance.mdata_animal_id2) + "1");


        for (int i = 0; i < mdata_animal_num0; i++)
        {
            //ShootAnimal(mdata_animal_id0, false, Random.Range(0, 0.5f));
            AnimalCanFlyAnimal animal = GetAnimal(mdata_animal_id0);
            animal.SetBoxEnable(false);
            mBallLeft.PushAnimal(animal);
        }

        mBallLeft.Fly();
        mBallRight.Fly();

        mCalculate.SetLock(false);
        //temp_next_count = 0;

        yield return new WaitForSeconds(2);
        //TopTitleCtl.instance.MoveIn();
        if(0 == mdata_guanka)
        {
            TopTitleCtl.instance.Reset();
        }
        yield return new WaitForSeconds(2);
        if (isFirstTime)
        {
            SoundManager.instance.PlayBgAsync("bgmusic_loop2", "bgmusic_loop2", 0.2f);
            isFirstTime = false;
        }

    }
    IEnumerator TCorrect()
    {
        for(float i = 0; i < 1f; i += 0.05f)
        {

            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator TNext()
    {
        //TopTitleCtl.instance.AddStar(
        mCalculate.PlayCorrect();
        yield return new WaitForSeconds(1.5f);

        mCalculate.Hide();
        mBalanceLeft.Fly();
        mBalanceRight.Fly();
        mBallLeft.FlyOut();
        mBallRight.FlyOut();

        yield return new WaitForSeconds(3f);

        mdata_guanka++;
        if (mdata_guanka == FormManager.config_games[SceneEnum.AnimalCanFly].m_all_guanka)
        {
            TopTitleCtl.instance.AddStar();
            GameOverCtl.GetInstance().Show(FormManager.config_games[SceneEnum.AnimalCanFly].m_all_guanka, Reset);
        }
        else
        {
            //FlushGuankaData();
            StartCoroutine(TStart());
            TopTitleCtl.instance.AddStar();
        }

    }


}
