using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AnimalPartyCtl : BaseScene
{
    public class Guanka
    {
        public int guanka = 1;

        public Guanka()
        {

        }
        public void Set(int _guanka)
        {
            guanka = _guanka;

        }
    }

    public static AnimalPartyCtl instance = null;


    public Guanka mGuanka { get; set; }
    public SoundManager mSound { get; set; }
    public AnimalPartyGuanka1 mGuankaCtl1 { get; set; }
    public AnimalPartyGuanka2 mGuankaCtl2 { get; set; }
    public AnimalPartyGuanka3 mGuankaCtl3 { get; set; }

    public RectTransform mLine1 { get; set; }
    public RectTransform mLine2 { get; set; }
    bool isFirstTime = true;

    void OnDestroy()
    {
        if (instance == this)
            instance = null;

        AnimalPartyBalloon.gSelect = null;
        AnimalPartyHead.gSelect = null;

    }
    void Awake()
    {
        instance = this;
        mGuanka = new Guanka();
        mSound = gameObject.AddComponent<SoundManager>();

    }
    void Start()
    {
        

        mLine1 = UguiMaker.newGameObject("mLine1", transform).GetComponent<RectTransform>();
        Image line0 = UguiMaker.newImage("line0", mLine1, "animalparty_sprite", "line0", false);
        //Image line1 = UguiMaker.newImage("line1", mLine1, "animalparty_sprite", "line1", true);
        line0.rectTransform.sizeDelta = new Vector2(1423, 128);
        //line1.gameObject.AddComponent<Button>().onClick.AddListener(NextGuanka);

        mLine2 = UguiMaker.newGameObject("mLine2", transform).GetComponent<RectTransform>();
        line0 = UguiMaker.newImage("line0", mLine2, "animalparty_sprite", "line0", false);
        //line1 = UguiMaker.newImage("line1", mLine2, "animalparty_sprite", "line1", true);
        line0.rectTransform.sizeDelta = new Vector2(1423, 128);
        //line1.gameObject.AddComponent<Button>().onClick.AddListener(NextGuanka);

        //mLine1.anchoredPosition = new Vector2(0, -800);
        mLine2.anchoredPosition = new Vector2(0, -800);

        mSceneType = SceneEnum.AnimalParty;
        CallLoadFinishEvent();

        //mSound.PlayTip("animalparty_sound", "game-name6-2");
        
        StartCoroutine(TStart());

    }

    public void InitGuanka1(Vector3 pos)
    {
        if (null == mGuankaCtl1)
        {
            mGuankaCtl1 = UguiMaker.newGameObject("guanka1", transform).AddComponent<AnimalPartyGuanka1>();
        }
        mGuankaCtl1.Init();
        mGuankaCtl1.GetComponent<RectTransform>().anchoredPosition3D = pos;

    }
    public void InitGuanka2(Vector3 pos)
    {
        if (null == mGuankaCtl2)
        {
            mGuankaCtl2 = UguiMaker.newGameObject("guanka2", transform).AddComponent<AnimalPartyGuanka2>();
        }
        mGuankaCtl2.Init();
        mGuankaCtl2.GetComponent<RectTransform>().anchoredPosition3D = pos;

    }
    public void InitGuanka3(Vector3 pos)
    {
        if (null == mGuankaCtl3)
        {
            mGuankaCtl3 = UguiMaker.newGameObject("guanka3", transform).AddComponent<AnimalPartyGuanka3>();
        }
        mGuankaCtl3.Init();
        mGuankaCtl3.GetComponent<RectTransform>().anchoredPosition3D = pos;

    }

    public void Reset()
    {
        StartCoroutine("TReset");
    }
    public void FlushPos()
    {
        if(null != mLine1)
        {
            mLine1.anchoredPosition = mGuankaCtl1.mRtran.anchoredPosition + new Vector2(0, -812);
        }
        if(null != mGuankaCtl2)
        {
            mGuankaCtl2.mRtran.anchoredPosition = mLine1.anchoredPosition + new Vector2(0, -400 - 12);
        }
        if(null != mLine2 && null != mGuankaCtl2)
        {
            mLine2.anchoredPosition = mGuankaCtl2.mRtran.anchoredPosition + new Vector2(0, -400 - 12);
        }
        if(null != mGuankaCtl3)
        {
            mGuankaCtl3.mRtran.anchoredPosition = mLine2.anchoredPosition + new Vector2(0, -400 - 12);
        }


    }
    public void NextGuanka()
    {
        Debug.Log("NextGuanka()");
        TopTitleCtl.instance.AddStar();
        switch (mGuanka.guanka)
        {
            case 1:
                {
                    if(mGuankaCtl1.cur_guanka == 2)
                    {
                        StartCoroutine("TGoto1_2");
                    }
                    else
                    {
                        StartCoroutine("TGoto2");
                    }
                }
                break;
            case 2:
                {
                    StartCoroutine("TGoto3");
                }
                break;
            case 3:
                break;
        }

        
    }


    IEnumerator TStart()
    {
        TopTitleCtl.instance.Reset();
        mGuanka.Set(1);
        InitGuanka1(new Vector3(0, -400));
        yield return new WaitForSeconds(1f);
        TopTitleCtl.instance.MoveIn();

        mLine1.SetAsLastSibling();
        mLine2.SetAsLastSibling();
        for (float i = 0; i < 1f; i += 0.02f)
        {
            mGuankaCtl1.mRtran.anchoredPosition = Vector2.Lerp(new Vector2(0, -400), new Vector2(0, 400), i);
            FlushPos();
            yield return new WaitForSeconds(0.01f);
        }
        mGuankaCtl1.mRtran.anchoredPosition = new Vector2(0, 400);
        FlushPos();
        mGuankaCtl1.PlayGuanka();

        if(isFirstTime)
        {
            SoundManager.instance.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.1f);
            isFirstTime = false;
        }
    }
    IEnumerator TGoto1_2()
    {
        for (float i = 0; i < 1f; i += 0.02f)
        {
            mGuankaCtl1.mRtran.anchoredPosition = Vector2.Lerp(new Vector2(0, 400), new Vector2(0, -145), i);
            FlushPos();
            yield return new WaitForSeconds(0.01f);
        }
        mGuankaCtl1.mRtran.anchoredPosition = new Vector2(0, -145);
        FlushPos();

    }
    IEnumerator TGoto2()
    {
        mGuanka.Set(2);
        InitGuanka2(new Vector3(0, -800, 0));

        //yield return new WaitForSeconds(2f);

        mLine1.SetAsLastSibling();
        mLine2.SetAsLastSibling();
        for (float i = 0; i < 1f; i += 0.015f)
        {
            mGuankaCtl1.mRtran.anchoredPosition = Vector2.Lerp(new Vector2(0, -145), new Vector2(0, 1200 + 24), i);
            FlushPos();
            yield return new WaitForSeconds(0.01f);
        }
        mGuankaCtl1.mRtran.anchoredPosition = new Vector2(0, 1200 + 24);
        FlushPos();
        mGuankaCtl2.PlayTip();

    }
    IEnumerator TGoto3()
    {
        mGuanka.Set(3);
        InitGuanka3(new Vector3(0, -400, 0));

        //yield return new WaitForSeconds(2f);

        mLine1.SetAsLastSibling();
        mLine2.SetAsLastSibling();
        for (float i = 0; i < 1f; i += 0.02f)
        {
            mGuankaCtl1.mRtran.anchoredPosition = Vector2.Lerp(new Vector2(0, 1200 + 24), new Vector2(0, 2000 + 48), i);
            FlushPos();
            yield return new WaitForSeconds(0.01f);
        }
        mGuankaCtl1.mRtran.anchoredPosition = new Vector2(0, 2000 + 48);
        FlushPos();
        mGuankaCtl3.PlayTip();


    }
    IEnumerator TReset()
    {
        TopTitleCtl.instance.mSoundTipData.Clean();
        mLine1.SetAsLastSibling();
        mLine2.SetAsLastSibling();
        for (float i = 0; i < 1f; i += 0.02f)
        {
            mGuankaCtl1.mRtran.anchoredPosition = Vector2.Lerp(new Vector2(0, 2000 + 48), new Vector2(0, -400), i);
            FlushPos();
            yield return new WaitForSeconds(0.01f);
        }
        mGuankaCtl1.mRtran.anchoredPosition = new Vector2(0, -400);
        FlushPos();
        yield return new WaitForSeconds(1);
        StartCoroutine("TStart");

    }




}
