using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FindSameThingCtl : BaseScene
{
    enum UpdateState
    {
        none,
        waiting_for_select,
        selecting,
        playing_correct,
        playing_error,

    }

    public static FindSameThingCtl instance;

    public SoundManager mSound;
    public RectTransform mRtranTime;
    public Text mTime, mCount;
    public Image mTimeImg, mCountImg, mMask, mOver;

    public List<ParticleSystem> mEffectStars = new List<ParticleSystem>();
    public List<ParticleSystem> mEffectCount = new List<ParticleSystem>();

    public List<FindSameThingStation> mStations = new List<FindSameThingStation>();//所有对象
    public List<FindSameThingLine> mLines = new List<FindSameThingLine>();//所有线

    public List<FindSameThingStation> mCurrentSelect_Station = new List<FindSameThingStation>();//当前选中对象
    public List<FindSameThingLine> mCurrentSelect_Line = new List<FindSameThingLine>();//当前选中线


    public int mdata_guanka = 1;
    public int mdata_time = 60;//第二关计时
    public int mdata_count = 0;//第二关计数
    public Dictionary<int, List<int>> mdata_dic_thing = new Dictionary<int, List<int>>();
    public Dictionary<int, string> mdata_dic_sound = new Dictionary<int, string>();
    public List<int> mdata_curent_type = new List<int>();
    public List<Vector3> mdata_pos = new List<Vector3>();

    UpdateState mState = UpdateState.none;
    bool isFirstTime = true;

    void Awake()
    {
        instance = this;
        mSound = gameObject.AddComponent<SoundManager>();
        mSound.SetAbName("findsamething_sound");

        mdata_dic_thing.Add(0, new List<int>() { 0, 1, 2, 3, 4 });
        mdata_dic_thing.Add(1, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 });
        mdata_dic_thing.Add(2, new List<int>() { 0, 1, 2, 3 });
        mdata_dic_thing.Add(3, new List<int>() { 0, 1, 2, 3 });
        mdata_dic_thing.Add(4, new List<int>() { 0, 1, 2, 3 });
        mdata_dic_thing.Add(5, new List<int>() { 0, 1 });

        mdata_dic_sound.Add(0, "白菜");
        mdata_dic_sound.Add(1, "菠菜");
        mdata_dic_sound.Add(2, "南瓜");
        mdata_dic_sound.Add(3, "胡萝卜");
        mdata_dic_sound.Add(4, "土豆");

        mdata_dic_sound.Add(10, "鞋子");
        mdata_dic_sound.Add(11, "帽子");
        mdata_dic_sound.Add(12, "鞋子");
        mdata_dic_sound.Add(13, "手套");
        mdata_dic_sound.Add(14, "裤子");
        mdata_dic_sound.Add(15, "衣服");
        mdata_dic_sound.Add(16, "裙子");
        mdata_dic_sound.Add(17, "鞋子");

        mdata_dic_sound.Add(20, "足球");
        mdata_dic_sound.Add(21, "跳绳");
        mdata_dic_sound.Add(22, "乒乓球拍");
        mdata_dic_sound.Add(23, "羽毛球");

        mdata_dic_sound.Add(30, "书");
        mdata_dic_sound.Add(31, "铅笔");
        mdata_dic_sound.Add(32, "钢笔");
        mdata_dic_sound.Add(33, "橡皮擦");

        mdata_dic_sound.Add(40, "葡萄");
        mdata_dic_sound.Add(41, "桃子");
        mdata_dic_sound.Add(42, "西瓜");
        mdata_dic_sound.Add(43, "香蕉");

        mdata_dic_sound.Add(50, "被子");
        mdata_dic_sound.Add(51, "枕头");


        foreach (Vector3 v in Common.PosSortByWidth(1200, 4, 140))
        {
            mdata_pos.Add(v);
        }
        foreach (Vector3 v in Common.PosSortByWidth(1000, 3, -30))
        {
            mdata_pos.Add(v);
        }
        foreach (Vector3 v in Common.PosSortByWidth(1200, 4, -220))
        {
            mdata_pos.Add(v);
        }


    }
    void Start()
    {
        mSceneType = SceneEnum.FindSameThing;
        CallLoadFinishEvent();

        Image bg = UguiMaker.newImage("bg", transform, "findsamething_sprite", "bg", false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);

        Reset();

    }

    

    void Update ()
    {
        switch(mState)
        {
            case UpdateState.none:
                break;
            case UpdateState.waiting_for_select:
                UpdateWaitingForSelect();
                break;
            case UpdateState.selecting:
                UpdateSelecting();
                break;
            case UpdateState.playing_correct:
                UpdatePlayingCorrect();
                break;
        }
	}
    void UpdateWaitingForSelect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FindSameThingStation com = null;
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach (RaycastHit h in hits)
            {
                com = h.collider.gameObject.GetComponent<FindSameThingStation>();
                if (null != com)
                {
                    break;
                }
            }

            if (null == com)
                return;

            //固定绳子起始位置
            FindSameThingLine.gSelect = CreateLine();
            FindSameThingLine.gSelect.gameObject.SetActive(true);
            FindSameThingLine.gSelect.mRtran.anchoredPosition = com.mRtran.anchoredPosition;
            mCurrentSelect_Line.Add(FindSameThingLine.gSelect);

            //定位
            com.ShowLock(true);
            com.SetBoxEnable(false);
            mCurrentSelect_Station.Add(com);

            FindSameThingLine.gSelect.UpdateLine(Common.getMouseLocalPos(transform));
            mState = UpdateState.selecting;

            mSound.PlayShort("按钮2");

        }
            
    }
    void UpdateSelecting()
    {
        if (Input.GetMouseButton(0))
        {
            FindSameThingStation com = null;
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach (RaycastHit h in hits)
            {
                com = h.collider.gameObject.GetComponent<FindSameThingStation>();
                if (null != com)
                {
                    break;
                }
            }
            
            
            
            if (null != com && mCurrentSelect_Station[0].mdata_type == com.mdata_type)
            {
                //同类连接
                com.ShowLock(true);
                com.SetBoxEnable(false);
                mCurrentSelect_Station.Add(com);

                //连接两个station
                FindSameThingLine.gSelect.UpdateLine(com.mRtran.anchoredPosition3D);
                FindSameThingLine.gSelect = null;

                mSound.PlayShort("按钮2");
                if (IsCorrect(mCurrentSelect_Station[0].mdata_type))
                {
                    //全部连完
                    mState = UpdateState.playing_correct;
                    StartCoroutine("TPlayCorrect");
                    return;
                }
                else
                {
                    //还没有连完
                    //固定绳子起始位置
                    FindSameThingLine.gSelect = CreateLine();
                    FindSameThingLine.gSelect.gameObject.SetActive(true);
                    FindSameThingLine.gSelect.mRtran.anchoredPosition = com.mRtran.anchoredPosition;
                    mCurrentSelect_Line.Add(FindSameThingLine.gSelect);

                }



            }
            else
            {
                FindSameThingLine.gSelect.UpdateLine(Common.getMouseLocalPos(transform));
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            for(int i = 0; i < mCurrentSelect_Station.Count; i++)
            {
                mCurrentSelect_Station[i].SetBoxEnable(true);
                mCurrentSelect_Station[i].ShowLock(false);
                mCurrentSelect_Station[i].Fly();
            }
            mCurrentSelect_Station.Clear();

            for(int i = 0; i < mCurrentSelect_Line.Count; i++)
            {
                mCurrentSelect_Line[i].ToError();
            }
            FindSameThingLine.gSelect = null;
            //mCurrentSelect_Station.Clear();

            mState = UpdateState.playing_error;
        }

    }
    void UpdatePlayingCorrect()
    {

    }



    public FindSameThingLine CreateLine()
    {
        FindSameThingLine result = null;

        //找闲置绳子
        for (int i = 0; i < mLines.Count; i++)
        {
            if (!mLines[i].gameObject.activeSelf)
            {
                result = mLines[i];
                break;
            }
        }
        //没找到，创建绳子
        if (null == result)
        {
            result = UguiMaker.newGameObject("line", transform).AddComponent<FindSameThingLine>();
            result.Init();
            mLines.Add(result);
        }

        return result;
    }
    public bool IsCorrect(int type)
    {
        int count = 0;
        for(int i = 0; i < mdata_curent_type.Count; i++)
        {
            if(mdata_curent_type[i] == type)
            {
                count++;
            }
        }

        bool result = mCurrentSelect_Station.Count == count;

        if(result)
        {
            for(int i = 0; i < mdata_curent_type.Count; i++)
            {
                if(mdata_curent_type[i] == type)
                {
                    mdata_curent_type.RemoveAt(i);
                    i--;
                }
            }
        }

        //if(result && 2 == mdata_guanka)
        //{
        //    mdata_count += count;
        //    mCount.text = mdata_count.ToString();
        //}

        return result;
    }
    public void PlayStar(Vector3 play_pos)
    {
        ParticleSystem ef = null;
        for (int i = 0; i < mEffectStars.Count; i++)
        {
            if(!mEffectStars[i].isPlaying)
            {
                ef = mEffectStars[i];
                break;
            }
        }
        if(null == ef)
        {
            ef = ResManager.GetPrefab("effect_star4", "effect_star4", transform).GetComponent<ParticleSystem>();
            mEffectStars.Add(ef);
        }
        ef.transform.localPosition = play_pos;
        ef.Play();
        mSound.PlayShortDefaultAb("09-爆2", 0.25f);

    }
    public void PlayCount(Vector3 play_pos)
    {
        StartCoroutine(TPlayCount(play_pos));

    }
    public void PlayTip()
    {
        /*
        List<string> sound_names = new List<string>();
        for (int i = 0; i < mCurrentSelect_Station.Count; i++)
        {
            int key = mCurrentSelect_Station[i].mdata_type * 10 + mCurrentSelect_Station[i].mdata_id;
            string value = mdata_dic_sound[key];
            if (!sound_names.Contains(value))
            {
                sound_names.Add(value);
            }
        }
        switch( mCurrentSelect_Station[0].mdata_type)
        {
            case 0:
                sound_names.Add("是蔬菜");
                break;
            case 1:
                sound_names.Add("是服饰");
                break;
            case 2:
                sound_names.Add("是体育用品");
                break;
            case 3:
                sound_names.Add("是文具");
                break;
            case 4:
                sound_names.Add("是水果");
                break;
            case 5:
                sound_names.Add("是床上用品");
                break;
        }

        List<float> sound_volume = new List<float>();
        for (int i = 0; i < sound_names.Count; i++)
            sound_volume.Add(1f);
        mSound.PlayTipListDefaultAb(sound_names, sound_volume, false);
            */


        switch (mCurrentSelect_Station[0].mdata_type)
        {
            case 0:
                mSound.PlayShortDefaultAb("是蔬菜");
                break;
            case 1:
                mSound.PlayShortDefaultAb("是服饰");
                break;
            case 2:
                mSound.PlayShortDefaultAb("是体育用品");
                break;
            case 3:
                mSound.PlayShortDefaultAb("是文具");
                break;
            case 4:
                mSound.PlayShortDefaultAb("是水果");
                break;
            case 5:
                mSound.PlayShortDefaultAb("是床上用品");
                break;
        }



    }
    public void OnClkOver()
    {
        GameOverCtl.GetInstance().Show(2, Reset);
    }
    
    public void callback_PlayError(FindSameThingLine line)
    {
        mCurrentSelect_Line.Remove(line);
        if(mCurrentSelect_Line.Count == 0)
        {
            mState = UpdateState.waiting_for_select;
        }
    }

    public void Reset()
    {
        mdata_curent_type.Clear();

        if (null != mOver)
            mOver.gameObject.SetActive(false);

        if (null != mTimeImg)
            mTimeImg.gameObject.SetActive(false);

        if (null != mMask)
            mMask.gameObject.SetActive(false);
        
        if(mStations.Count > 0)
        {
            for(int i = 0; i < mStations.Count; i++)
            {
                mStations[i].gameObject.SetActive(false);
            }
        }

        mdata_guanka = 1;
        StartCoroutine("TStartGuanka1");
        return;

        mdata_guanka = 2;
        StartCoroutine("TStartGuanka2");

    }
    IEnumerator TStartGuanka1()
    {
        mState = UpdateState.none;
        yield return new WaitForSeconds(0.5f);
        TopTitleCtl.instance.Reset();
        yield return new WaitForSeconds(1f);
        if (isFirstTime)
        {
            SoundManager.instance.PlayBg("bgmusic_loop1", "bgmusic_loop1", 0.2f);
            isFirstTime = false;
        }

        List<int> type_ids = new List<int>();//十位是type，个位是id
        List<int> types = Common.BreakRank<int>(new List<int>() { 0, 1, 2, 3, 4, 5 });
        List<int> types_use = new List<int>();
        int temp_index = -1;
        int temp_count = 0;
        while(temp_count < 11)
        {
            temp_index++;
            int type = types[temp_index];
            temp_count += mdata_dic_thing[type].Count;
            types_use.Add(type);
        }

        List<int> type_ids_unuse = new List<int>();//还没有使用的，加入随机序列
        for(int i = 0; i < types_use.Count; i++)
        {
            int type = types_use[i];
            mdata_dic_thing[type] = Common.BreakRank<int>(mdata_dic_thing[type]);
            type_ids.Add(type * 10 + mdata_dic_thing[type][0]);
            type_ids.Add(type * 10 + mdata_dic_thing[type][1]);
            for(int j = 2; j < mdata_dic_thing[type].Count; j++)
            {
                type_ids_unuse.Add(type * 10 + mdata_dic_thing[type][j]);
            }
        }
        type_ids_unuse = Common.BreakRank<int>(type_ids_unuse);
        int add_num = 11 - type_ids.Count;
        for(int i = 0; i < add_num; i++)
        {
            type_ids.Add(type_ids_unuse[i]);
        }



        mdata_curent_type.Clear();
        string msg = "";
        for(int i = 0; i < 11; i ++)
        {
            msg += type_ids[i].ToString() + " ";
            int type = type_ids[i] / 10;
            int id = type_ids[i] % 10;
            mdata_curent_type.Add(type);

            if(mStations.Count - 1 < i)
            {
                FindSameThingStation station = UguiMaker.newGameObject("station" + i.ToString(), transform).AddComponent<FindSameThingStation>();
                mStations.Add(station);
            }
            mStations[i].gameObject.SetActive(true);
            mStations[i].Init(type, id);
            mStations[i].mRtran.anchoredPosition3D = mdata_pos[i];
            mStations[i].SetResetPos( mdata_pos[i]);
            mStations[i].Show();


            yield return new WaitForSeconds(0.1f);

        }
        Debug.Log(msg);


        mState = UpdateState.waiting_for_select;

        yield return new WaitForSeconds(1);
        mSound.PlayTipDefaultAb("tip那些物品属于同一类把他们连起来吧");

        //yield return new WaitForSeconds(2);

    }
    IEnumerator TStartGuanka2()
    {
        Debug.Log("TStartGuanka2");
        mState = UpdateState.none;
        //while(mSound.is_playing_tip)
        //{
        //    yield return new WaitForSeconds(0.1f);
        //}
        yield return new WaitForSeconds(2.5f);


        if(null == mTimeImg)
        {
            mTimeImg = UguiMaker.newImage("mTimeImg", transform, "findsamething_sprite", "time", false);
            mCountImg = UguiMaker.newImage("mCountImg", mTimeImg.transform, "findsamething_sprite", "count", false);
            //mCountImg.type = Image.Type.Sliced;
            mCountImg.rectTransform.pivot = new Vector2(0, 0.5f);
            mCountImg.rectTransform.anchoredPosition = new Vector2(87.2f, 0);
            //mCountImg.rectTransform.sizeDelta = new Vector2(131.3f, 96);


            mTime = UguiMaker.newText("mTime", mTimeImg.transform);
            mCount = UguiMaker.newText("mCount", mCountImg.transform);

            mTime.rectTransform.sizeDelta = new Vector2(200, 200);
            mTime.rectTransform.anchoredPosition = new Vector2(0, -26.47f);
            mTime.alignment = TextAnchor.MiddleCenter;
            mTime.fontSize = 80;


            mCount.rectTransform.sizeDelta = new Vector2(200, 200);
            mCount.rectTransform.anchoredPosition = new Vector2(0, -26.47f);
            mCount.alignment = TextAnchor.MiddleCenter;
            mCount.fontSize = 80;

            //mCount.rectTransform.sizeDelta = new Vector2(200, 200);
            //mCount.rectTransform.anchoredPosition = new Vector2(-14.4f, 0);
            //mCount.alignment = TextAnchor.MiddleCenter;
            //mCount.fontSize = 60;

        }
        mdata_time = 60;
        mdata_count = 0;
        mTime.text = mdata_time.ToString();
        mCount.text = mdata_count.ToString();
        mTime.color = new Color(1, 1, 1, 1);

        mTimeImg.gameObject.SetActive(true);
        mTimeImg.rectTransform.anchoredPosition = Vector2.zero;
        mSound.PlayShortDefaultAb("m魔法-恢复-轻-1");
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mTimeImg.transform.localScale = Vector3.Lerp(new Vector3(3, 3, 1), new Vector3(0.6f, 0.6f, 1), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);
        mTimeImg.transform.localScale = new Vector3(0.6f, 0.6f, 1);
        for (float i = 0; i < 1f; i += 0.08f)
        {
            mTimeImg.rectTransform.anchoredPosition = Vector3.Lerp(Vector2.zero, new Vector3(-555.4f, 279.7f), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mTimeImg.rectTransform.anchoredPosition = new Vector2(-556.7f, 281.3f);


        List<int> temp = new List<int>() { 0, 1, 2, 3, 4, 10, 11, 12, 13, 14, 15, 16, 17, 20, 21, 22, 23, 30, 31, 32, 33, 40, 41, 42, 43, 50, 51 };


        mdata_curent_type.Clear();
        string msg = "";
        for (int i = 0; i < 11; i++)
        {
            //int type = Random.Range(0, 1000) % 6;
            //int random_index = Random.Range(0, 1000) % mdata_dic_thing[type].Count;
            //int id = mdata_dic_thing[type][random_index];


            int type_id = temp[Random.Range(0, 1000) % temp.Count];
            int type = type_id / 10;
            int id = type_id % 10;
            temp.Remove(type_id);

            msg += (type * 10 + id).ToString();

            mdata_curent_type.Add(type);

            if (mStations.Count - 1 < i)
            {
                FindSameThingStation station = UguiMaker.newGameObject("station" + i.ToString(), transform).AddComponent<FindSameThingStation>();
                mStations.Add(station);
            }

            mStations[i].gameObject.SetActive(true);
            mStations[i].Init(type, id);
            mStations[i].mRtran.anchoredPosition3D = mdata_pos[i];
            mStations[i].SetResetPos(mdata_pos[i]);
            mStations[i].Show();


            yield return new WaitForSeconds(0.1f);

        }
        Debug.Log(msg);
        //TopTitleCtl.instance.Reset();


        mState = UpdateState.waiting_for_select;


        while (true)
        {
            mTime.text = mdata_time.ToString();
            if(mdata_time == 10)
            {
                mSound.StopBg();
            }
            if(mdata_time <= 10)
            {
                mSound.PlayShortDefaultAb("09-倒数");
                mTime.color = new Color(1, 0, 0, 1);
            }
            else
            {
                mSound.PlayShortDefaultAb("秒针", 0.8f);
            }

            yield return new WaitForSeconds(1f);
            mdata_time--;
            if (-1 == mdata_time)
            {
                break;
            }
        }
        
        mState = UpdateState.none;
        for(int i = 0; i < mStations.Count; i++)
        {
            mStations[i].SetBoxEnable(false);
        }
        /*
        if(null == mMask)
        {
            mMask = UguiMaker.newImage("mMask", transform, "public", "white", false);
            mMask.color = new Color(0, 0, 0, 0);
            mMask.rectTransform.sizeDelta = new Vector2(1500, 810);
        }
        mMask.gameObject.SetActive(true);
        mMask.transform.SetAsLastSibling();
        mTimeImg.transform.SetAsLastSibling();
        Vector2 pos0 = mTimeImg.rectTransform.anchoredPosition;
        Vector3 scale0 = mTimeImg.rectTransform.localScale;
        for(float i = 0; i < 1f; i += 0.03f)
        {
            mTimeImg.rectTransform.anchoredPosition = Vector2.Lerp(pos0, Vector2.zero, Mathf.Sin(Mathf.PI * 0.5f * i));
            mTimeImg.rectTransform.localScale = Vector3.Lerp(scale0, new Vector3(1.5f, 1.5f, 1), i);
            mMask.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.5f), i);
            yield return new WaitForSeconds(0.01f);
        }
        mTimeImg.rectTransform.anchoredPosition = Vector2.zero;
        */
        

        //yield return new WaitForSeconds(3);
        for (int i = 0; i < mCurrentSelect_Station.Count; i++)
        {
            mCurrentSelect_Station[i].SetBoxEnable(true);
            mCurrentSelect_Station[i].ShowLock(false);
            mCurrentSelect_Station[i].Fly();
        }
        mCurrentSelect_Station.Clear();
        for (int i = 0; i < mCurrentSelect_Line.Count; i++)
        {
            mCurrentSelect_Line[i].ToError();
        }
        /*
        yield return new WaitForSeconds(2);

        for (float i = 0; i < 1f; i += 0.08f)
        {
            mTimeImg.rectTransform.anchoredPosition = Vector3.Lerp(Vector2.zero, new Vector3(-555.4f, 279.7f), Mathf.Sin(Mathf.PI * 0.5f * i));
            mTimeImg.rectTransform.localScale = Vector3.Lerp(new Vector3(1.5f, 1.5f, 1), scale0, i);
            mMask.color = Color.Lerp(new Color(0, 0, 0, 0.5f), new Color(0, 0, 0, 0), i);
            yield return new WaitForSeconds(0.01f);
        }
        mTimeImg.rectTransform.anchoredPosition = new Vector2(-556.7f, 281.3f);
        mTimeImg.rectTransform.localScale = scale0;
        mMask.color = new Color(0, 0, 0, 0);
        */
        OnClkOver();

    }
    IEnumerator TPlayCorrect()
    {

        float aspeed = 40;
        List<Vector3> speeds = new List<Vector3>();
        for(int i = mCurrentSelect_Station.Count - 1; i > 0; i--)
        {
            Vector3 speed = mCurrentSelect_Station[i - 1].mRtran.anchoredPosition3D - mCurrentSelect_Station[i].mRtran.anchoredPosition3D;
            speeds.Add(speed.normalized * aspeed);
        }

        if (2 == mdata_guanka)
        {
            PlayCount(mCurrentSelect_Station[mCurrentSelect_Station.Count - 1].mRtran.anchoredPosition3D);
        }
        for (int i = 0; i < speeds.Count; i++)
        {
            int mid_index = mCurrentSelect_Station.Count - i - 1;
            while (aspeed < Vector3.Distance(mCurrentSelect_Station[mid_index].mRtran.anchoredPosition3D, mCurrentSelect_Station[mid_index - 1].mRtran.anchoredPosition3D))
            {
                Vector3 pos = mCurrentSelect_Station[mid_index].mRtran.anchoredPosition3D + speeds[i];
                for(int j = mid_index; j < mCurrentSelect_Station.Count; j++)
                {
                    mCurrentSelect_Station[j].mRtran.anchoredPosition3D = pos;
                }

                mCurrentSelect_Line[mCurrentSelect_Line.Count - 1 - i].UpdateLine(pos);
                yield return new WaitForSeconds(0.01f);

            }

            for (int j = mid_index; j < mCurrentSelect_Station.Count; j++)
            {
                mCurrentSelect_Station[j].mRtran.anchoredPosition3D = mCurrentSelect_Station[mid_index - 1].mRtran.anchoredPosition3D;
            }


            mCurrentSelect_Line[mCurrentSelect_Line.Count - 1 - i].gameObject.SetActive(false);
            PlayStar(mCurrentSelect_Station[mid_index - 1].mRtran.anchoredPosition3D);
            if (2 == mdata_guanka)
            {
                PlayCount(mCurrentSelect_Station[mid_index - 1].mRtran.anchoredPosition3D);
            }

        }

        PlayTip();

        for (int i = 0; i < mCurrentSelect_Station.Count; i++)
        {
            mCurrentSelect_Station[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < mCurrentSelect_Line.Count; i++)
        {
            mCurrentSelect_Line[i].gameObject.SetActive(false);
        }
        mCurrentSelect_Line.Clear();
        mCurrentSelect_Station.Clear();

        if(1 == mdata_guanka)
        {
            bool isover = true;
            for(int i = 0; i < mStations.Count; i++)
            {
                if(mStations[i].gameObject.activeSelf)
                {
                    isover = false;
                    break;
                }
            }
            if(isover)
            {
                mdata_guanka = 2;
                StartCoroutine("TStartGuanka2");
            }
            else
            {
                mState = UpdateState.waiting_for_select;
            }

        }
        else if(2 == mdata_guanka)
        {

            List<int> temp = new List<int>() { 0, 1, 2, 3, 4, 10, 11, 12, 13, 14, 15, 16, 17, 20, 21, 22, 23, 30, 31, 32, 33, 40, 41, 42, 43, 50, 51 };
            for(int i = 0; i < mStations.Count; i++)
            {
                if(mStations[i].gameObject.activeSelf)
                {
                    int type_id = mStations[i].mdata_type * 10 + mStations[i].mdata_id;
                    if(temp.Contains(type_id))
                    {
                        temp.Remove(type_id);
                    }
                }

            }

            for (int i = 0; i < mStations.Count; i++)
            {
                if (!mStations[i].gameObject.activeSelf)
                {
                    int type_id = temp[Random.Range(0, 1000) % temp.Count];
                    int type = type_id / 10;
                    int id = type_id % 10;
                    temp.Remove(type_id);
                    //Debug.Log("type_id=" + type_id);
                    //int type = Random.Range(0, 1000) % 6;
                    //int random_index = Random.Range(0, 1000) % mdata_dic_thing[type].Count;
                    //int id = mdata_dic_thing[type][random_index];

                    mStations[i].gameObject.SetActive(true);
                    mStations[i].Init(type, id);
                    mStations[i].Show();

                    mdata_curent_type.Add(type);


                }
            }
            mState = UpdateState.waiting_for_select;


        }




    }
    IEnumerator TPlayCount(Vector3 play_pos)
    {
        ParticleSystem ef = null;
        for (int i = 0; i < mEffectCount.Count; i++)
        {
            if (!mEffectCount[i].gameObject.activeSelf)
            {
                ef = mEffectCount[i];
                break;
            }
        }
        if (null == ef)
        {
            ef = ResManager.GetPrefab("findsamething_prefab", "effect_count", transform).GetComponent<ParticleSystem>();
            mEffectCount.Add(ef);
        }
        ef.gameObject.SetActive(true);
        ef.transform.localPosition = play_pos;
        ef.Play();

        Vector3 pos_end = new Vector3(-467, 275, 0);
        while (35 < Vector3.Distance(ef.transform.localPosition, pos_end))
        {
            Vector3 speed = (pos_end - play_pos).normalized * 30;
            speed.z = 0;
            ef.transform.localPosition += speed;
            yield return new WaitForSeconds(0.01f);
        }
        ef.gameObject.SetActive(false);
        mdata_count++;
        mCount.text = mdata_count.ToString();

    }

}
