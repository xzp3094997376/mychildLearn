using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AnimalNumOnlyCtl : BaseScene
{
    public static AnimalNumOnlyCtl instance = null;

    public RectTransform mRtran { get; set; }
    public AnimalNumOnlyPlace mPlace { get; set; }
    public AnimalNumOnlyUI mUI { get; set; }
    public SoundManager mSound { get; set; }
    
    bool isFirstTime = true;

    public int data_guanka = 1;
    public int data_answer_num = 0;
    public int data_tip_num = 0;
    public Vector3 data_place_pos = new Vector3(-240, -10);
    public List<int> data_animal_types { get; set; }//允许出现的动物类型
    //public List<int> data_animal_finish = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};//每一行，每一列，要达成的目标，索引是动物id, 第一个id是没用的
    public Dictionary<int, int> data_animal_color = new Dictionary<int, int>();//key动物id，value颜色
    public Dictionary<int, AnimalNumOnlyAnimal> mAnimal = new Dictionary<int, AnimalNumOnlyAnimal>();
    public ParticleSystem mEffect { get; set; }
    
    void Awake()
    {
        instance = this;
        mSound = gameObject.AddComponent<SoundManager>();
        mSound.SetAbName("animalnumonly_sound");
    }
    void Start () {

        mRtran = gameObject.GetComponent<RectTransform>();

        mSceneType = SceneEnum.AnimalNumOnly;
        CallLoadFinishEvent();

        Reset();

    }
	void Update () {
	
	}

    public void Reset()
    {
        data_guanka = 1;
        ResetTopTitle();
        StartCoroutine("TStart");

    }
    public void ResetTopTitle()
    {
        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.MoveIn();

    }
    public void ReplayGuanka()
    {
        ClearTip();
        StartCoroutine("TReplayGuanka");
    }
    public void SetGuanka(int guanka)
    {
        data_animal_color.Clear();
        data_guanka = guanka;
        List<int> temp_colors = Common.BreakRank<int>(new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 });
        switch(guanka)
        {
            case 1:
                {
                    data_answer_num = 6;
                    data_tip_num = 3;
                    data_animal_types = Common.GetMutexValue(1, 14, 2);
                }
                break;
            case 2:
                {
                    data_answer_num = 15;
                    data_tip_num = 3;
                    data_animal_types = Common.GetMutexValue(1, 14, 6);
                }
                break;
            case 3:
                {
                    data_answer_num = 6;
                    data_tip_num = 3;
                    data_animal_types = Common.GetMutexValue(1, 14, 4);
                }
                break;

        }
        for (int i = 0; i < data_animal_types.Count; i++)
        {
            int animal_id = data_animal_types[i];
            data_animal_color.Add(animal_id, temp_colors[0]);
            temp_colors.RemoveAt(0);
        }


    }
    public void NextGuanka()
    {
        if(++data_guanka > 3)
        {
            TopTitleCtl.instance.AddStar();
            GameOverCtl.GetInstance().Show(3, Reset);
        }
        else
        {
            TopTitleCtl.instance.AddStar();
            StartCoroutine("TStart");
        }


    }
    public void JumpAnimal()
    {
        //StartCoroutine("TJumpAnimal");
        for(int i = 0; i < data_animal_types.Count; i++)
        {
            int id = data_animal_types[i];
            if(!mAnimal.ContainsKey(id))
            {
                GameObject obj = ResManager.GetPrefab("aa_animal_env_prefab", MDefine.GetAnimalNameByID_EN(id));
                AnimalNumOnlyAnimal com = obj.AddComponent<AnimalNumOnlyAnimal>();
                UguiMaker.InitGameObj(obj, transform, MDefine.GetAnimalNameByID_EN(id), new Vector3(0, -2999, 0), Vector3.one);
                mAnimal.Add(id, com);
                com.Init(id);
                com.Play("Idle");
            }
            mAnimal[id].Jump();

        }
        //mUI.SetYun0Last();
        if(null == mEffect)
        {
            mEffect = ResManager.GetPrefab("effect_yun0", "yun0").GetComponent<ParticleSystem>();
            UguiMaker.InitGameObj(mEffect.gameObject, transform, "effect_yun", new Vector3(0, -400, 0), Vector3.one);

        }
        mEffect.Play();
        Invoke("NextGuanka", 4);

    }
    public void PlayTip()
    {
        float volume = 0.5f;
        if(6 == data_animal_types.Count)
        {
            mSound.PlayTipList(
                new List<string>() { "animalnumonly_sound", "animalnumonly_sound", "animalnumonly_sound" },
                new List<string>() { "玩法", "每一行每一列都要有", "6种小动物"},
                new List<float>() { volume, volume, volume },
                true);

        }
        else if(4 == data_animal_types.Count)
        {
            mSound.PlayTipList(
                new List<string>() { "animalnumonly_sound", "animalnumonly_sound", "animalnumonly_sound", "aa_animal_name", "animalnumonly_sound", "aa_animal_name", "animalnumonly_sound", "aa_animal_name", "animalnumonly_sound", "aa_animal_name" },
                new List<string>() { "玩法", "每一行每一列都要有", "一只", MDefine.GetAnimalNameByID_CH(data_animal_types[0]), "一只", MDefine.GetAnimalNameByID_CH(data_animal_types[1]), "一只", MDefine.GetAnimalNameByID_CH(data_animal_types[2]), "一只", MDefine.GetAnimalNameByID_CH(data_animal_types[3])},
                new List<float>() { volume, volume, volume, 1f, 1f, 1f, 1f, 1f, 1f, 1f }, true);

        }
        else
        {
            mSound.PlayTipList(
                new List<string>() { "animalnumonly_sound", "animalnumonly_sound", "animalnumonly_sound", "aa_animal_name", "animalnumonly_sound", "aa_animal_name" },
                new List<string>() { "玩法", "每一行每一列都要有", "一只", MDefine.GetAnimalNameByID_CH(data_animal_types[0]), "一只", MDefine.GetAnimalNameByID_CH(data_animal_types[1]) },
                new List<float>() { volume, volume, volume, 1f, 1f, 1f, 1f }, true);
        }
    }
    public void ClearTip()
    {
        mSound.StopTip();
        TopTitleCtl.instance.mSoundTipData.Clean();
    }


    public void Callback_Correct()
    {
        mPlace.SetCellEnable(false);
        mPlace.PlayCorrect();
        mUI.TimeStop();

        bool fast = true;
        switch(data_guanka)
        {
            case 1:
                if (mUI.fen < 3)
                    fast = true;
                else
                    fast = false;
                break;
            case 2:
                if (mUI.fen < 8)
                    fast = true;
                else
                    fast = false;
                break;
            default:
                if (mUI.fen < 15)
                    fast = true;
                else
                    fast = false;
                break;
        }

        mSound.PlayShortDefaultAb("恭喜");
        if(fast)
        {
            if (mUI.fen == 0)
            {
                mSound.PlayTipList(
                    new List<string>() { "animalnumonly_sound", "animalnumonly_sound", "number_sound", "animalnumonly_sound" },
                    new List<string>() { "好厉害", "你只用了", mUI.miao.ToString(), "秒" },
                    new List<float>() { 1, 1, 1, 1 });
            }
            else 
            {
                if (mUI.miao == 0)
                {
                    mSound.PlayTipList(
                        new List<string>() { "animalnumonly_sound", "animalnumonly_sound", "number_sound", "animalnumonly_sound" },
                        new List<string>() { "好厉害", "你只用了", mUI.fen.ToString(), "分" },
                        new List<float>() { 1, 1, 1, 1 });

                }
                else
                {
                    mSound.PlayTipList(
                        new List<string>() { "animalnumonly_sound", "animalnumonly_sound", "number_sound", "animalnumonly_sound", "number_sound", "animalnumonly_sound" },
                        new List<string>() { "好厉害", "你只用了", mUI.fen.ToString(), "分", mUI.miao.ToString(), "秒" },
                        new List<float>() { 1, 1, 1, 1, 1, 1 });

                }
            }
        }
        else
        {
            if (mUI.fen == 0)
            {
                mSound.PlayTipList(
                    new List<string>() { "animalnumonly_sound", "animalnumonly_sound", "number_sound", "animalnumonly_sound", "animalnumonly_sound" },
                    new List<string>() { "好厉害", "你用了", mUI.miao.ToString(), "秒", "时间有点多哦" },
                    new List<float>() { 1, 1, 1, 1, 1 });
            }
            else 
            {
                if (mUI.miao == 0)
                {
                    mSound.PlayTipList(
                        new List<string>() { "animalnumonly_sound", "animalnumonly_sound", "number_sound", "animalnumonly_sound", "animalnumonly_sound" },
                        new List<string>() { "好厉害", "你用了", mUI.fen.ToString(), "分", "时间有点多哦" },
                        new List<float>() { 1, 1, 1, 1, 1 });

                }
                else
                {
                    mSound.PlayTipList(
                        new List<string>() { "animalnumonly_sound", "animalnumonly_sound", "number_sound", "animalnumonly_sound", "number_sound", "animalnumonly_sound", "animalnumonly_sound" },
                        new List<string>() { "好厉害", "你用了", mUI.fen.ToString(), "分", mUI.miao.ToString(), "秒", "时间有点多哦" },
                        new List<float>() { 1, 1, 1, 1, 1, 1, 1 });

                }
            }

        }

    }
    

    IEnumerator TStart()
    {
        if (null == mPlace)
        {
            mUI = gameObject.AddComponent<AnimalNumOnlyUI>();
            mUI.Init();

            mPlace = UguiMaker.newGameObject("mPlace", transform).AddComponent<AnimalNumOnlyPlace>();

            mUI.SetYun0Last();

        }

        yield return new WaitForSeconds(0.1f);

        SetGuanka(data_guanka);
        mPlace.Flush();
        mPlace.PlayGoUp();
        //mPlace.mRtran.anchoredPosition = data_place_pos;

        //mUI.Control(true);

        //yield return new WaitForSeconds(1);
        //mSound.PlayTipDefaultAb("玩法", 1, true);
        yield return new WaitForSeconds(1.5f);
        if (isFirstTime)
        {
            SoundManager.instance.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.05f);
            isFirstTime = false;
        }

        mUI.Reset();
        PlayTip();

        yield return new WaitForSeconds(1f);
        mUI.m_can_click_replay = true;


    }
    IEnumerator TReplayGuanka()
    {
        mPlace.StopCorrect();
        Vector3 speed = new Vector3(0, 0, 0);
        while (mPlace.mRtran.anchoredPosition3D.y > -650)
        {
            mPlace.mRtran.anchoredPosition3D += speed;
            yield return new WaitForSeconds(0.01f);
            speed.y -= 1;
        }
        

        transform.localScale = Vector3.one;
        transform.localEulerAngles = Vector3.zero;


        yield return new WaitForSeconds(0.5f);

        StartCoroutine("TStart");

    }


}
