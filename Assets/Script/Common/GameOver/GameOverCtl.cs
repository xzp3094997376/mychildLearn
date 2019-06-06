using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameOverCtl : MonoBehaviour {
    static GameOverCtl instance = null;
    public static GameOverCtl GetInstance()
    {
        if(null == instance)
        {

            GameObject go = UguiMaker.newGameObject("GameOver", Global.instance.mCanvasTop);
            instance = go.AddComponent<GameOverCtl>();
            Canvas canvas = go.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 10;
            go.AddComponent<GraphicRaycaster>();
            go.layer = LayerMask.NameToLayer("UI");

            Image mbg = UguiMaker.newImage("overbg", go.transform, "public", "white");
            mbg.rectTransform.sizeDelta = new Vector2(1423, 800);
            mbg.color = new Color(0, 0, 0, 0.8f);
            
        }
        return instance;
    }


    private List<Image> mStars = null;
    private ParticleSystem mStartEffect{ get; set; }
    //礼花
    GameObject lihua_r_hong { get; set; }
    GameObject lihua_r_lan { get; set; }
    GameObject lihua_l_hong { get; set; }
    GameObject lihua_l_lan { get; set; }
    ParticleSystem mEffectLihua_R_Hong { get; set; }
    ParticleSystem mEffectLihua_R_Lan { get; set; }
    ParticleSystem mEffectLihua_L_Hong { get; set; }
    ParticleSystem mEffectLihua_L_Lan { get; set; }


    void Init()
    {
        mStars = new List<Image>();
        GameObject stargo = UguiMaker.newGameObject("star", transform);
        RectTransform rtstar = stargo.GetComponent<RectTransform>();
        for (int i = 1;i < 21; i++)
        {
            Image star = UguiMaker.newImage("star" + i, rtstar, "public", "game_over_star");
            star.rectTransform.sizeDelta = new Vector2(64, 60);
            star.transform.localScale = Vector3.one;
            star.enabled = false;
            mStars.Add(star);
        }


        //按钮
        Button button_play = UguiMaker.newButton("finishButton", transform, "public", "gameover_next");
        button_play.transform.localPosition = new Vector2(210, -290);
        button_play.onClick.AddListener( OnButtonClick);
        Button button_replay = UguiMaker.newButton("rePlayButton", transform, "public", "gameover_replay");
        button_replay.transform.localPosition = new Vector2(-190, -290);
        button_replay.onClick.AddListener( OnButtonClickRePlay);


        //星星特效
        GameObject obj = ResManager.GetPrefabInResources("prefab/Gameover/ParticleSystem");
        obj.transform.SetParent(transform);
        obj.transform.localScale = Vector3.one;
        mStartEffect = obj.GetComponent<ParticleSystem>();


        //礼花
        GameObject lihua = UguiMaker.newGameObject("lihua", transform);
        GameObject lihua_r = UguiMaker.newGameObject("right", lihua.transform);
        lihua_r_hong = UguiMaker.newGameObject("hong", lihua_r.transform);
        lihua_r_lan = UguiMaker.newGameObject("lan", lihua_r.transform);
        Image lihua_r_hong0 = UguiMaker.newImage("0", lihua_r_hong.transform, "public", "gameover_lipao2");
        Image lihua_r_hong1 = UguiMaker.newImage("1", lihua_r_hong.transform, "public", "gameover_lipao3");
        Image lihua_r_lan0 = UguiMaker.newImage("0", lihua_r_lan.transform, "public", "gameover_lipao0");
        Image lihua_r_lan1 = UguiMaker.newImage("1", lihua_r_lan.transform, "public", "gameover_lipao1");
        lihua_r.transform.localPosition = new Vector3(GlobalParam.screen_width * 0.5f + 30, GlobalParam.screen_height * -0.5f - 30, 0);
        lihua_r_hong0.rectTransform.pivot = new Vector2(1, 0);
        lihua_r_hong0.rectTransform.anchoredPosition = new Vector2(-31.2f, 160.9f);
        lihua_r_hong1.rectTransform.pivot = new Vector2(1, 0);
        lihua_r_hong1.rectTransform.anchoredPosition = Vector2.zero;
        lihua_r_lan0.rectTransform.pivot = new Vector2(1, 0);
        lihua_r_lan0.rectTransform.anchoredPosition = new Vector2(-120.6f, 52.1f);
        lihua_r_lan1.rectTransform.pivot = new Vector2(1, 0);
        lihua_r_lan1.rectTransform.anchoredPosition = Vector2.zero;

        GameObject lihua_l = UguiMaker.newGameObject("left", lihua.transform);
        lihua_l_hong = UguiMaker.newGameObject("hong", lihua_l.transform);
        lihua_l_lan = UguiMaker.newGameObject("lan", lihua_l.transform);
        Image lihua_l_hong0 = UguiMaker.newImage("0", lihua_l_hong.transform, "public", "gameover_lipao2");
        Image lihua_l_hong1 = UguiMaker.newImage("1", lihua_l_hong.transform, "public", "gameover_lipao3");
        Image lihua_l_lan0 = UguiMaker.newImage("0", lihua_l_lan.transform, "public", "gameover_lipao0");
        Image lihua_l_lan1 = UguiMaker.newImage("1", lihua_l_lan.transform, "public", "gameover_lipao1");
        lihua_l.transform.localPosition = new Vector3(GlobalParam.screen_width * -0.5f - 30, GlobalParam.screen_height * -0.5f - 30, 0);
        lihua_l.transform.localScale = new Vector3(-1, 1, 1);
        lihua_l_hong0.rectTransform.pivot = new Vector2(1, 0);
        lihua_l_hong0.rectTransform.anchoredPosition = new Vector2(-31.2f, 160.9f);
        lihua_l_hong1.rectTransform.pivot = new Vector2(1, 0);
        lihua_l_hong1.rectTransform.anchoredPosition = Vector2.zero;
        lihua_l_lan0.rectTransform.pivot = new Vector2(1, 0);
        lihua_l_lan0.rectTransform.anchoredPosition = new Vector2(-120.6f, 52.1f);
        lihua_l_lan1.rectTransform.pivot = new Vector2(1, 0);
        lihua_l_lan1.rectTransform.anchoredPosition = Vector2.zero;

        mEffectLihua_R_Hong = ResManager.GetPrefabInResources("prefab/Gameover/lihua/lihua_r_hong").GetComponent<ParticleSystem>();
        UguiMaker.InitGameObj(mEffectLihua_R_Hong.gameObject, lihua_r_hong.transform, "lihua_r_hong", new Vector3(-101, 207, 0), Vector3.one);
        mEffectLihua_R_Lan = ResManager.GetPrefabInResources("prefab/Gameover/lihua/lihua_r_lan").GetComponent<ParticleSystem>();
        UguiMaker.InitGameObj(mEffectLihua_R_Lan.gameObject, lihua_r_lan.transform, "lihua_r_lan", new Vector3(-179, 126, 0), Vector3.one);


        mEffectLihua_L_Hong = ResManager.GetPrefabInResources("prefab/Gameover/lihua/lihua_r_hong").GetComponent<ParticleSystem>();
        UguiMaker.InitGameObj(mEffectLihua_L_Hong.gameObject, lihua_l_hong.transform, "lihua_l_hong", new Vector3(-98, 206, 0), Vector3.one);
        mEffectLihua_L_Hong.transform.localEulerAngles = new Vector3( 300, 90, 270);
        mEffectLihua_L_Lan = ResManager.GetPrefabInResources("prefab/Gameover/lihua/lihua_r_lan").GetComponent<ParticleSystem>();
        UguiMaker.InitGameObj(mEffectLihua_L_Lan.gameObject, lihua_l_lan.transform, "lihua_l_lan", new Vector3(-199, 121, 0), Vector3.one);
        mEffectLihua_L_Lan.transform.localEulerAngles = new Vector3(327, 90, 270);

    }
    public void Close()
    {
        KbadyCtl.instance.HideSpine();
        gameObject.SetActive(false);
        AndroidDataCtl.DoAndroidFunc("stopListenAnswer");
    }



    System.Action callback_func = null;
    void OnButtonClickRePlay()
    {
        if(null != callback_func)
        {
            callback_func();
        }
        callback_func = null;
        Close();

    }
    void OnButtonClick()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        Application.Quit();
    }


    
    public void Show(int star_number, System.Action callback_replay)
    {
        KbadyCtl.Init();
        TopTitleCtl.instance.mSoundTipData.Clean();
        SoundManager.instance.PlayShort("胜利通关音乐");
        callback_func = callback_replay;
        gameObject.SetActive(true);

        if (null == mStars)
            Init();
        for (int i = 0; i < 20; i++)
            mStars[i].enabled = false;
        
        StartCoroutine(TShowStar(star_number));
        StartCoroutine(TPlayRihua());
        StartCoroutine(TPlayLihua());

        AndroidDataCtl.DoAndroidFunc("onCompleteOneGame");
        Debug.Log("安卓通信：发送打通游戏信号onCompleteOneGame");

    }
    IEnumerator TShowStar(int star_number)
    {
        KbadyCtl.instance.mRtranSpine.anchoredPosition = new Vector2(0, 67);
        KbadyCtl.instance.ShowSpine(new Vector3(0.6f, 0.6f));
        //KbadyCtl.instance.PlaySpine(kbady_enum.Encourage_1, true);
        KbadyCtl.instance.PlaySpineEncourage(true);

        List<Vector3> poss = Common.PosSortByWidth(85 * star_number, star_number, -151);

        for (int i = 0; i < star_number; i++)
        {
            mStars[i].rectTransform.anchoredPosition = poss[i];
            mStars[i].enabled = true;

            mStartEffect.transform.localPosition = poss[i];
            mStartEffect.Play();

            SoundManager.instance.PlayShort("gameover_star", 0.5f);
            for (float j = 0; j < 1f; j += 0.1f)
            {
                float p = Mathf.Sin(Mathf.PI * j) * 0.5f;
                mStars[i].rectTransform.localScale = Vector2.Lerp(Vector2.zero, Vector3.one, j) +  new Vector2(p, p);
                mStars[i].rectTransform.localEulerAngles = Vector3.Lerp( new Vector3(0, 0, 60), Vector3.zero, j);
                yield return new WaitForSeconds(0.01f);
            }
            mStars[i].rectTransform.localScale = Vector3.one;
            mStars[i].rectTransform.localEulerAngles = Vector3.zero;
        }
    }

    float temp_lihua_num = 4.5f;
    IEnumerator TPlayRihua()
    {
        Vector3 pos_hong_out = new Vector3(-15, 25, 0);
        Vector3 pos_hong_in = Vector3.zero;// new Vector3(4, -31, 0);
        Vector3 pos_lan_out = new Vector3(-18, 17, 0);
        Vector3 pos_lan_in = Vector3.zero;// new Vector3(54, -31, 0);
        Vector3 sacle0 = new Vector3(0.8f, 0.8f, 1);
        Vector3 scale1 = Vector3.one;

        while (true)
        {

            for(float i = 0; i < 1f; i += 0.08f)
            {
                lihua_r_hong.transform.localPosition = Vector3.Lerp(pos_hong_in, pos_hong_out, i);
                lihua_r_lan.transform.localPosition = Vector3.Lerp(pos_lan_out, pos_lan_in, i);
                lihua_r_hong.transform.localScale = Vector3.Lerp(sacle0, scale1, i);
                lihua_r_lan.transform.localScale = Vector3.Lerp(scale1, sacle0, i);
                yield return new WaitForSeconds(0.01f);
            }
            mEffectLihua_R_Hong.Emit((int)(5 * temp_lihua_num));
            yield return new WaitForSeconds(0.1f);

            for (float i = 0; i < 1f; i += 0.08f)
            {
                lihua_r_hong.transform.localPosition = Vector3.Lerp(pos_hong_out, pos_hong_in, i);
                lihua_r_lan.transform.localPosition = Vector3.Lerp(pos_lan_in, pos_lan_out, i);
                lihua_r_hong.transform.localScale = Vector3.Lerp(scale1, sacle0, i);
                lihua_r_lan.transform.localScale = Vector3.Lerp(sacle0, scale1, i);
                yield return new WaitForSeconds(0.01f);
            }
            mEffectLihua_R_Lan.Emit((int)(3 * temp_lihua_num));
            yield return new WaitForSeconds(0.1f);

        }

    }
    IEnumerator TPlayLihua()
    {
        Vector3 pos_hong_out = new Vector3(-15, 25, 0);
        Vector3 pos_hong_in = Vector3.zero;// new Vector3(4, -31, 0);
        Vector3 pos_lan_out = new Vector3(-18, 17, 0);
        Vector3 pos_lan_in = Vector3.zero;// new Vector3(54, -31, 0);
        Vector3 sacle0 = new Vector3(0.8f, 0.8f, 1);
        Vector3 scale1 = Vector3.one;

        while (true)
        {

            for (float i = 0; i < 1f; i += 0.08f)
            {
                lihua_l_hong.transform.localPosition = Vector3.Lerp(pos_hong_in, pos_hong_out, i);
                lihua_l_lan.transform.localPosition = Vector3.Lerp(pos_lan_out, pos_lan_in, i);
                lihua_l_hong.transform.localScale = Vector3.Lerp(sacle0, scale1, i);
                lihua_l_lan.transform.localScale = Vector3.Lerp(scale1, sacle0, i);
                yield return new WaitForSeconds(0.01f);
            }
            mEffectLihua_L_Hong.Emit((int)(5 * temp_lihua_num));
            yield return new WaitForSeconds(0.1f);

            for (float i = 0; i < 1f; i += 0.08f)
            {
                lihua_l_hong.transform.localPosition = Vector3.Lerp(pos_hong_out, pos_hong_in, i);
                lihua_l_lan.transform.localPosition = Vector3.Lerp(pos_lan_in, pos_lan_out, i);
                lihua_l_hong.transform.localScale = Vector3.Lerp(scale1, sacle0, i);
                lihua_l_lan.transform.localScale = Vector3.Lerp(sacle0, scale1, i);
                yield return new WaitForSeconds(0.01f);
            }
            mEffectLihua_L_Lan.Emit((int)(3 * temp_lihua_num));
            yield return new WaitForSeconds(0.1f);

        }

    }

}
