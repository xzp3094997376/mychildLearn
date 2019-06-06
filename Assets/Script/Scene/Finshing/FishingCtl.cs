using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FishingCtl : BaseScene
{
    public static FishingCtl instance = null;
    public RectTransform mWater0, mWater1, mWater2, mScene;
    Image mCar0, mCarLun0, mCarLun1;
    Image mSelectFlag { get; set; }
    

    FishingAnimal[] mAnimals = new FishingAnimal[4];
    public SoundManager mSound { get; set; }
    AudioSource mCarSound { get; set; }


    int mdata_guanka = 1;
    int mdata_curent_answer_index = 0;
    List<int> mdata_complex = new List<int>();//对应每条线有多少段
    List<int> mdata_animal_id = new List<int>() { 0, 1, 2, 3};
    List<int> mdata_answer_id = new List<int>() { 0, 1, 2, 3 };//轮流回答的索引顺序
    List<int> mdata_fishing_id = new List<int>();
    List<Vector3> mdata_animal_pos = null;//动物站立的位置
    List<Vector3> mdata_fishing_pos = null;//鱼的位置
    



    void Awake()
    {
        instance = this;
        mSound = gameObject.AddComponent<SoundManager>();
        mSound.SetAbName("fishing_sound");

    }
	void Start () {

        mSceneType = SceneEnum.Fishing;
        CallLoadFinishEvent();

        Reset();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Reset()
    {
        TopTitleCtl.instance.Reset();
        SetGuanka(1);
        StartCoroutine(TStart());

    }
    public void AddGuanka()
    {

    }
    public void SetGuanka(int guanka)
    {
        mdata_guanka = guanka;
        switch (guanka)
        {
            case 1:
                mdata_complex = new List<int>() { 5, 5, 6, 5};
                break;

            case 2:
                mdata_complex = new List<int>() { 9, 9, 9, 9 };
                break;

            case 3:
                mdata_complex = new List<int>() { 9, 9, 9, 9 };
                break;

        }
        if(null == mdata_animal_pos)
        {
            mdata_animal_pos = Common.PosSortByWidth(1150, 4, 40);
            mdata_fishing_pos = Common.PosSortByWidth(1150, 4, -300);
            for(int i = 0; i < mdata_animal_pos.Count; i++)
            {
                mdata_animal_pos[i] += new Vector3(-80, 0, 0);
            }
        }

        mdata_animal_pos = Common.BreakRank<Vector3>(mdata_animal_pos);
        mdata_fishing_pos = Common.BreakRank<Vector3>(mdata_fishing_pos);
        mdata_animal_id = Common.BreakRank<int>(mdata_animal_id);
        mdata_answer_id = Common.BreakRank<int>(mdata_answer_id);
        mdata_fishing_id = Common.GetMutexValue(5, 8, 4);
        mdata_curent_answer_index = 0;



    }
    public void PlayGameTip()
    {
        StopCoroutine("TSelectFlag");
        if(mdata_curent_answer_index < mdata_answer_id.Count)
        {
            if(null == mSelectFlag)
            {
                mSelectFlag = UguiMaker.newImage("mSelectFlag", transform, "fishing_sprite", "select_flag", false);
            }
            mSelectFlag.rectTransform.anchoredPosition3D = mAnimals[mdata_answer_id[mdata_curent_answer_index]].mSpine.rectTransform.anchoredPosition3D;
            mSelectFlag.gameObject.SetActive(true);
            StartCoroutine("TSelectFlag");

            switch (mdata_answer_id[mdata_curent_answer_index])
            {
                case 0:
                    Debug.LogError("鸡钓什么？");
                    mSound.PlayTipList(new List<string>() { "aa_animal_name", "fishing_sound" }, new List<string>() { "鸡", "tip钓到的是那条鱼" }, true);
                    break;
                case 1:
                    Debug.LogError("鹅钓什么？");
                    mSound.PlayTipList(new List<string>() { "aa_animal_name", "fishing_sound" }, new List<string>() { "鹅", "tip钓到的是那条鱼" }, true);
                    break;
                case 2:
                    Debug.LogError("猪钓什么？");
                    mSound.PlayTipList(new List<string>() { "aa_animal_name", "fishing_sound" }, new List<string>() { "猪", "tip钓到的是那条鱼" }, true);
                    break;
                default:
                    Debug.LogError("鸽子钓什么");
                    mSound.PlayTipList(new List<string>() { "aa_animal_name", "fishing_sound" }, new List<string>() { "鸽子", "tip钓到的是那条鱼" }, true);
                    break;

            }

        }
        else
        {
            if(null != mSelectFlag)
            {
                mSelectFlag.gameObject.SetActive(false);
            }
        }
    }

    public void callbackOnClick(FishingAnimal animal)
    {
        if(mdata_answer_id[mdata_curent_answer_index] == animal.mdata_index)
        {
            mdata_curent_answer_index++;
            //Debug.LogError("回答正确");
            animal.GetFish();
            mSound.PlayTipDefaultAb("tip嗯嗯就是这条");
            mSound.PlayShort("按钮点击正确");
            Invoke("PlayGameTip", 3);
            //PlayGameTip();

        }
        else
        {
            //Debug.LogError("回答错误");
            animal.PlayError();
            mSound.PlayTipDefaultAb("tip线条有点乱");
            mSound.PlayShort("按钮点击错误");
        }
    }
    public void callback_Finish_PutFish()
    {
        bool over = true;
        for(int i = 0; i < 4; i++)
        {
            if (!mAnimals[i].mdata_isover)
            {
                return;
            }
        }
        StartCoroutine(TOver());
    }
    

    bool isFirstTime = true;
    IEnumerator TStart()
    {
        if(isFirstTime)
        {
            Image bg = UguiMaker.newImage("bg", transform, "fishing_sprite", "bg", false);
            bg.type = Image.Type.Sliced;
            bg.rectTransform.sizeDelta = new Vector2(1423, 800);

            GameObject paopao = ResManager.GetPrefab("effect_paopao", "paopao");
            UguiMaker.InitGameObj(paopao, transform, "effect_paopao", new Vector3(0, -410, 0), Vector3.one);

            mWater0 = UguiMaker.newGameObject("mWater0", transform).GetComponent<RectTransform>();
            mWater1 = UguiMaker.newGameObject("mWater1", transform).GetComponent<RectTransform>();
            mScene = UguiMaker.newGameObject("mScene", transform).GetComponent<RectTransform>();
            mWater2 = UguiMaker.newGameObject("mWater2", transform).GetComponent<RectTransform>();


            Image lang0 = UguiMaker.newImage("lang2", mWater0, "fishing_sprite", "lang3", false);
            Image lang1 = UguiMaker.newImage("lang1", mWater1, "fishing_sprite", "lang2", false);
            Image lang2 = UguiMaker.newImage("lang0", mWater2, "fishing_sprite", "lang1", false);
            lang2.color = new Color32(255, 255, 255, 100);
            Image lang_color1 = UguiMaker.newImage("lang_color1", mWater1, "public", "white", false);
            lang_color1.color = new Color32(0, 179, 255, 255);
            lang_color1.rectTransform.sizeDelta = new Vector2(1423, 400);
            lang_color1.rectTransform.anchoredPosition = new Vector2(0, -227.5f);
            Image lang_color2 = UguiMaker.newImage("lang_color2", mWater2, "public", "white", false);
            lang_color2.color = new Color32(0, 153, 219, 100);
            lang_color2.rectTransform.sizeDelta = new Vector2(1423, 400);
            lang_color2.rectTransform.anchoredPosition = new Vector2(0, -227.5f);


            RectTransform sea_bg = UguiMaker.newGameObject("sea_bg", mScene).GetComponent<RectTransform>();
            Image gass0 = UguiMaker.newImage("gass0", sea_bg, "fishing_sprite", "gass0", false);
            Image gass1 = UguiMaker.newImage("gass1", sea_bg, "fishing_sprite", "gass1", false);
            Image gass2 = UguiMaker.newImage("gass2", sea_bg, "fishing_sprite", "gass2", false);
            Image gass3 = UguiMaker.newImage("gass3", sea_bg, "fishing_sprite", "gass3", false);
            gass0.rectTransform.pivot = new Vector2(0.5f, 0);
            gass1.rectTransform.pivot = new Vector2(0.5f, 0);
            gass2.rectTransform.pivot = new Vector2(0.5f, 0);
            gass3.rectTransform.pivot = new Vector2(0.5f, 0);
            gass0.rectTransform.anchoredPosition = new Vector2(-575, -400);
            gass1.rectTransform.anchoredPosition = new Vector2(-311, -400);
            gass2.rectTransform.anchoredPosition = new Vector2(539, -400);
            gass3.rectTransform.anchoredPosition = new Vector2(126, -400);


            StartCoroutine(IWater());
            List<Vector3> poss = Common.PosSortByWidth(1200, 4, 40);
            for(int i = 0; i < 4; i++)
            {
                mAnimals[i] = UguiMaker.newGameObject("animal" + i, mScene).AddComponent<FishingAnimal>();
            }

            yield return new WaitForSeconds(0.2f);
            mSound.PlayTipListDefaultAb(new List<string>() { "tip小动物们在钓鱼", "tip请点一点吧" }, new List<float>() { 1, 1 });

            mCar0 = UguiMaker.newImage("mCar0", mScene, "fishing_sprite", "car0", false);
            mCarLun0 = UguiMaker.newImage("mCarLun0", mCar0.transform, "fishing_sprite", "car1", false);
            mCarLun1 = UguiMaker.newImage("mCarLun1", mCar0.transform, "fishing_sprite", "car1", false);
            mCarLun0.rectTransform.anchoredPosition = new Vector2(-137, -114.9f);
            mCarLun1.rectTransform.anchoredPosition = new Vector2(130.1f, -114.9f);
            mCarSound = mCar0.gameObject.AddComponent<AudioSource>();
            mCarSound.clip = ResManager.GetClip("fishing_sound", "car_run");
            mCarSound.loop = true;
            mCarSound.Stop();


            //yield return new WaitForSeconds(1);

        }
        
        //统计有多少个横竖坐标
        int shu_num = 0;
        int heng_num = 0;
        for(int i = 0; i < mdata_complex.Count; i++)
        {
            shu_num += (mdata_complex[i] - 2) / 2;
            heng_num += (mdata_complex[i] - 2) / 2;
        }



        //随机生成横竖坐标
        List<int> animal_line_begin_x = new List<int>();
        for(int i =0; i < 4; i++)
        {
            animal_line_begin_x.Add((int)(mdata_animal_pos[i].x + 180));
        }

        List<float> shu_poss = new List<float>();
        List<float> heng_poss = new List<float>();
        float line_dis = 15;
        int temp_pos = 0;
        while(shu_poss.Count != shu_num)
        {
            //temp_pos = (int)(Random.Range(-600 / line_dis, 600 / line_dis) * line_dis);
            temp_pos = (int)(-600 + Random.Range(0, 10000) % 80 * line_dis);// (int)(Random.Range(-217 / line_dis, -63 / line_dis) * line_dis);

            //判断是否和第一根线的固定x坐标重叠
            bool can_use = true;
            for(int i = 0; i < animal_line_begin_x.Count; i++)
            {
                if(line_dis > Mathf.Abs(animal_line_begin_x[i] - temp_pos))
                {
                    can_use = false;
                    break;
                }
            }
            if (!can_use)
                continue;

            if (!shu_poss.Contains(temp_pos))
            {
                //Debug.Log("x=" + temp_pos);
                shu_poss.Add(temp_pos);
            }
        }
        while (heng_poss.Count != heng_num)
        {
            temp_pos = (int)(-218 + Random.Range(0, 1000) % 12 * line_dis);// (int)(Random.Range(-217 / line_dis, -63 / line_dis) * line_dis);
            if (!heng_poss.Contains(temp_pos))
            {
                heng_poss.Add(temp_pos);
                //Debug.Log(temp);
            }
        }


        int heng_index = 0;
        int shu_index = 0;
        for (int i = 0; i < 4; i++)
        {
            //FishingFish fish = mFishLines[i].mFish;
            //生成每条线包含的坐标点
            List<Vector3> anchors = new List<Vector3>();
            anchors.Add(mdata_animal_pos[i] + new Vector3(180, 190, 0));

            int time = (mdata_complex[i] - 2) / 2;
            for (int j = 0; j < time; j++)
            {
                anchors.Add(new Vector3(anchors[anchors.Count - 1].x, heng_poss[heng_index], 0));
                if (j == time - 1)
                {
                    anchors.Add(new Vector3(mdata_fishing_pos[i].x - 30, anchors[anchors.Count - 1].y, 0));
                }
                else
                {
                    anchors.Add(new Vector3(shu_poss[shu_index], anchors[anchors.Count - 1].y, 0));
                }
                heng_index++;
                shu_index++;
            }

            anchors.Add(mdata_fishing_pos[i] + new Vector3(-30, 0, 0));
            //anchors.Add(new Vector3(anchors[anchors.Count - 1].x + 30 - line_end_offset[fish.mdata_id], anchors[anchors.Count - 1].y, 0));//line_end_offset中的索引=鱼id
            anchors.Add(mdata_fishing_pos[i]);

            mAnimals[i].Init(i, mdata_fishing_id[i], mdata_animal_pos[i], mdata_fishing_pos[i], anchors);
            mAnimals[i].gameObject.SetActive(false);

        }

        //车
        mCar0.gameObject.SetActive(true);
        mCar0.transform.SetAsLastSibling();
        mCar0.transform.localScale = Vector3.one;
        mCarSound.Play();
        for (float i = 0; i < 1f; i += 0.01f)
        {
            mCar0.rectTransform.anchoredPosition = Vector3.Lerp(new Vector3(-951, 184, 0), new Vector3(951, 184, 0), i);
            mCarLun0.transform.localEulerAngles += new Vector3(0, 0, -5);
            mCarLun1.transform.localEulerAngles += new Vector3(0, 0, -5);
    
            for(int j =0; j < 4; j++)
            {
                if (mAnimals[j].gameObject.activeSelf)
                    continue;
                if(10 > Mathf.Abs(mCar0.rectTransform.anchoredPosition.x - mAnimals[j].mSpine.rectTransform.anchoredPosition.x))
                {
                    mAnimals[j].gameObject.SetActive(true);
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
        mCar0.gameObject.SetActive(false);
        mCarSound.Stop();


        mAnimals[0].Shoot();
        mAnimals[1].Shoot();
        mAnimals[2].Shoot();
        mAnimals[3].Shoot();


        //TopTitleCtl.instance
        yield return new WaitForSeconds(1.5f);
        PlayGameTip();

        yield return new WaitForSeconds(1);

        yield return new WaitForSeconds(1);
        if (isFirstTime)
        {
            SoundManager.instance.PlayBgAsync("bgmusic_loop3", "bgmusic_loop3", 0.5f);
            isFirstTime = false;
        }

    }
    IEnumerator IWater()
    {


        float p0 = 0;
        float p1 = 0;
        float p2 = 0;
        float speed0 = 0.05f;
        float speed1 = 0.08f;
        float speed2 = 0.11f;
        Vector2 pos0 = new Vector2(0, -6);
        Vector2 pos1 = new Vector2(0, -18);
        Vector2 pos2 = new Vector2(0, -30);
        Vector2 _pos0 = new Vector2(0, -6 - 800);
        Vector2 _pos1 = new Vector2(0, -18 - 800);
        Vector2 _pos2 = new Vector2(0, -30 - 800);

        mWater0.anchoredPosition = _pos0;
        mWater1.anchoredPosition = _pos1;
        mWater2.anchoredPosition = _pos2;

        //yield return new WaitForSeconds(1);
        for (float i = 0; i < 1f; i += 0.01f)
        {
            float p = Mathf.Sin(Mathf.PI * 0.5f * i);
            mWater0.anchoredPosition = Vector2.Lerp(_pos0, pos0, p);
            mWater1.anchoredPosition = Vector2.Lerp(_pos1, pos1, p);
            mWater2.anchoredPosition = Vector2.Lerp(_pos2, pos2, p);

            yield return new WaitForSeconds(0.01f);
        }

        while (true)
        {
            mWater0.anchoredPosition = pos0 + new Vector2(0, Mathf.Sin(p0) * 3);
            //mWater1.anchoredPosition = pos1 + new Vector2(0, Mathf.Sin(p1) * 3);
            mWater2.anchoredPosition = pos2 + new Vector2(0, Mathf.Sin(p2) * 3);
            p0 += speed0;
            p1 += speed1;
            p2 += speed2;

            yield return new WaitForSeconds(0.01f);

        }

    }
    IEnumerator TOver()
    {
        mSound.PlayShort("胜利通关音乐");

        //车
        mCar0.gameObject.SetActive(true);
        mCar0.transform.SetAsLastSibling();
        mCar0.transform.localScale = new Vector3(-1, 1, 1);
        mCarSound.Play();
        for (float i = 0; i < 1f; i += 0.01f)
        {
            mCar0.rectTransform.anchoredPosition = Vector3.Lerp( new Vector3(951, 184, 0), new Vector3(-951, 184, 0), i);
            mCarLun0.transform.localEulerAngles += new Vector3(0, 0, 5);
            mCarLun1.transform.localEulerAngles += new Vector3(0, 0, 5);

            for (int j = 0; j < 4; j++)
            {
                if (!mAnimals[j].gameObject.activeSelf)
                    continue;
                if (10 > Mathf.Abs(mCar0.rectTransform.anchoredPosition.x - mAnimals[j].mSpine.rectTransform.anchoredPosition.x))
                {
                    mAnimals[j].gameObject.SetActive(false);
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
        mCarSound.Stop();
        mCar0.gameObject.SetActive(false);

        TopTitleCtl.instance.AddStar();
        yield return new WaitForSeconds(1f);

        mdata_guanka++;
        if(mdata_guanka > FormManager.config_games[SceneEnum.Fishing].m_all_guanka)
        {
            GameOverCtl.GetInstance().Show(FormManager.config_games[SceneEnum.Fishing].m_all_guanka, Reset);
        }
        else
        {
            SetGuanka(mdata_guanka);
            StartCoroutine(TStart());
        }

    }
    IEnumerator TSelectFlag()
    {
        Vector3 pos0 = mSelectFlag.rectTransform.anchoredPosition3D + new Vector3(0, 200, 0) ;
        while(true)
        {
            for(float i = 0; i < 1f; i += 0.05f)
            {
                mSelectFlag.rectTransform.anchoredPosition3D = pos0 + new Vector3(0, Mathf.Sin(Mathf.PI * i) * 10, 0);
                yield return new WaitForSeconds(0.01f);
            }

        }
    }

}
