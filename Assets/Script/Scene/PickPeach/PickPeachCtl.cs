using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickPeachCtl : BaseScene
{
    public static PickPeachCtl instance;

    public GameObject mScene { get; set; }
    public PickPeachGuanka1 mGuanka1 { get; set; }
    public PickPeachGuanka2 mGuanka2 { get; set; }
    public PickPeachGuanka3 mGuanka3 { get; set; }
    public PickPeachLanzi[] mLanzi = new PickPeachLanzi[3];
    public SoundManager mSound { get; set; }


    public int mdata_guanka = 1;
    public List<int> mdata_flower_num = null;

    void Awake()
    {
        instance = this;
        mSound = gameObject.AddComponent<SoundManager>();
        mSound.SetAbName("pickpeach_sound");

        mGuanka1 = gameObject.AddComponent<PickPeachGuanka1>();
        mGuanka2 = gameObject.AddComponent<PickPeachGuanka2>();
        mGuanka3 = gameObject.AddComponent<PickPeachGuanka3>();

        mLanzi[0] = UguiMaker.newGameObject("mLanzi1", transform).AddComponent<PickPeachLanzi>();
        mLanzi[1] = UguiMaker.newGameObject("mLanzi2", transform).AddComponent<PickPeachLanzi>();
        mLanzi[2] = UguiMaker.newGameObject("mLanzi3", transform).AddComponent<PickPeachLanzi>();

        mLanzi[0].Init();
        mLanzi[1].Init();
        mLanzi[2].Init();


        Image sky = UguiMaker.newImage("sky", transform, "public", "white", false);
        sky.rectTransform.sizeDelta = new Vector2(1423, 1809);
        sky.color = new Color32( 8, 210, 255, 255);
        sky.rectTransform.anchoredPosition = new Vector2(0, 300);

    }
    void Start ()
    {
        mSceneType = SceneEnum.GoodFriendCar;
        CallLoadFinishEvent();


        
        Reset();
        //TopTitleCtl.instance.MoveIn();


    }
    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.Q))
        {
            if(1 == mdata_guanka)
            {
                mGuanka1.EndGame();
                StartCoroutine(TGotoGuanka2());
            }
            else if(2 == mdata_guanka)
            {
                SetGuanka(3);
                mGuanka3.BeginGame();

            }
        }
	}

    public void Reset()
    {
        mGuanka2.Clean();

        TopTitleCtl.instance.Reset();
        SetGuanka(1);
        for(int i = 0; i < 3; i++)
        {
            mLanzi[i].SetFlowerNum(mdata_flower_num[i]);
            mLanzi[i].transform.localScale = Vector3.one;
            mLanzi[i].HidwShadow();
            mLanzi[i].Clean();
        }

        mScene = ResManager.GetPrefab("pickpeach_prefab", "scene");
        UguiMaker.InitGameObj(mScene, transform, "scene", Vector3.zero, Vector3.one);
        
        mGuanka1.BeginGame();

    }
    public void SetGuanka(int guanka)
    {
        mdata_guanka = guanka;
        switch(guanka)
        {
            case 1:
                mdata_flower_num = Common.GetMutexValue(1, 5, 3);
                break;
            case 2:
                break;
            case 3:
                break;
        }

    }
    public int GetFlowerNumBig()
    {
        int result = 0;
        for(int i = 0; i < mdata_flower_num.Count; i++)
        {
            if (mdata_flower_num[i] > result)
                result = mdata_flower_num[i];
        }
        return result;
    }
    public int GetFlowerNumSmall()
    {
        int result = 5;
        for (int i = 0; i < mdata_flower_num.Count; i++)
        {
            if (mdata_flower_num[i] < result)
                result = mdata_flower_num[i];
        }
        return result;
    }


    public bool callbackGuanka1_btnOk()
    {
        bool[] correct = new bool[] { false, false, false};
        correct[0] = mLanzi[0].Correct();
        correct[1] = mLanzi[1].Correct();
        correct[2] = mLanzi[2].Correct();

        if (correct[0] && correct[1] && correct[2])
        {
            //正确
            TopTitleCtl.instance.AddStar();
            mGuanka1.EndGame();
            StartCoroutine(TGotoGuanka2());
            mSound.PlayShortDefaultAb("欢呼0");
            return true;
        }
        else
        {
            //错误
            if (!correct[0])
            {
                mLanzi[0].Shake();
                mGuanka1.mAnimal[0].PlaySayNo();
            }
            else
            {
                mGuanka1.mAnimal[0].PlaySayYes();
            }
            if (!correct[1])
            {
                mLanzi[1].Shake();
                mGuanka1.mAnimal[1].PlaySayNo();
            }
            else
            {
                mGuanka1.mAnimal[1].PlaySayYes();
            }
            if (!correct[2])
            {
                mLanzi[2].Shake();
                mGuanka1.mAnimal[2].PlaySayNo();
            }
            else
            {
                mGuanka1.mAnimal[2].PlaySayYes();
            }
            mSound.PlayTipDefaultAb("tip不对不对你再仔细数数");
            return false;
        }

    }
    public void callbackGuanka2_over()
    {
        TopTitleCtl.instance.AddStar();
        SetGuanka(3);
        Invoke("Invoke_callbackGuanka2_over", 4.5f);
    }
    public void callbackGuanka3_over()
    {
        //mGuanka3.EndGame();
        TopTitleCtl.instance.AddStar();
        StartCoroutine(TGameOver());
    }

    public void Invoke_callbackGuanka2_over()
    {
        mGuanka3.BeginGame();

    }

    IEnumerator TGotoGuanka2()
    {
        yield return new WaitForSeconds(2.5f);

        mGuanka1.ShowBtnEnd(false);
        mGuanka1.mAnimal[0].MoveOut();
        mGuanka1.mAnimal[1].MoveOut();
        mGuanka1.mAnimal[2].MoveOut();



        yield return new WaitForSeconds(2);

        mSound.PlayShortDefaultAb("气球上升");
        Vector3 pos0 = mScene.transform.localPosition;
        Vector3 pos1 = new Vector3(0, -1450, 0);
        for(float i = 0; i < 1f; i += 0.01f)
        {
            mScene.transform.localPosition = Vector3.Lerp(pos0, pos1, i);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(mScene.gameObject);
        mScene = null;

        SetGuanka(2);
        mGuanka2.BeginGame();

    }
    IEnumerator TGameOver()
    {
        yield return new WaitForSeconds(2);
        GameOverCtl.GetInstance().Show(3, Reset);

    }
   

}
