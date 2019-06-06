using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TopTitleCtl : MonoBehaviour
{
    public static TopTitleCtl instance { get; set; }

    
    List<Image> mStars = new List<Image>();
    RectTransform mRtran { get; set; }
    Text mTitle;
    GridLayoutGroup group;
    config_game mCurentGameConfig { get; set; }

    string ab_name = "public";
    string guanka_black = "title_star0";
    string guanka_curent = "title_star1";
    string guanka_show = "title_star2";

    public SoundTipData mSoundTipData { get; set; }




    void Awake()
    {
        instance = this;
        mTitle = transform.Find("title").GetComponent<Text>();
        mRtran = gameObject.GetComponent<RectTransform>();
        mSoundTipData = new SoundTipData();

    }
	void Start ()
    {
        Transform layout = transform.Find("star_layout");
        for (int i = 0; i < 20; i++)
        {
            Image img = UguiMaker.newImage(i.ToString(), layout, "public", "title_star0");
            img.transform.SetAsFirstSibling();
            img.gameObject.SetActive(false);
            mStars.Add(img);
        }
        GraphicRaycaster gry = gameObject.AddComponent<GraphicRaycaster>();
        gry.ignoreReversedGraphics = true;

        //Reset();


    }



    public void HideAllStar()// 隐藏star
    {
        for (int i = 0; i < mStars.Count; i++)
        {
            mStars[i].enabled = false;
        }
    }
    public void SetBgColor(Color bg_color)
    {
        transform.Find("bg").GetComponent<Image>().color = bg_color;
    }
    public void SetSpriteData(string _ab_name, string _guanka_black, string _guanka_curent, string _guanka_show)
    {
        ab_name = _ab_name;
        guanka_black = _guanka_black;
        guanka_curent = _guanka_curent;
        guanka_show = _guanka_show;
        //Debug.LogError("SetSpriteData(): _ab_name=" + _ab_name + " _guanka_black=" + _guanka_black + " _guanka_curent=" + _guanka_curent + " _guanka_show=" + _guanka_show);
    }
    public void SetStartLayout(Vector2 cell_size, Vector2 spacing, Vector3 pos)
    {
        if(null == group)
        {
            group = transform.Find("star_layout").GetComponent<GridLayoutGroup>();
        }
        group.cellSize = cell_size;
        group.spacing = spacing;
        group.GetComponent<RectTransform>().anchoredPosition3D = pos;

    }
    public void ResetSetStartLayout()
    {
        if (null == group)
        {
            group = transform.Find("star_layout").GetComponent<GridLayoutGroup>();
        }
        group.cellSize = new Vector2(30, 30);
        group.spacing = new Vector2(13.24f, 0);
        group.transform.localPosition = Vector3.zero;
    }
    public List<string> GetStarNames()
    {
        List<string> result = new List<string>();
        for(int i = 0; i < mStars.Count; i++)
        {
            if (!mStars[i].gameObject.activeSelf)
                break;
            result.Add(mStars[i].sprite.name);
            
        }
        return result;

    }
    public Vector3 GetStarPos(string sprite_name)
    {
        Vector3 result = new Vector3(9999, 0, 0);

        for (int i = 0; i < mStars.Count; i++)
        {
            if (!mStars[i].gameObject.activeSelf)
                break;
            if(mStars[i].sprite.name.Equals(sprite_name))
            {
                //return mStars[i].transform.position;
                return new Vector3(9999, 9999, 0);
            }
            else if(mStars[i].sprite.name.Equals(guanka_black) || mStars[i].sprite.name.Equals(guanka_curent))
            {
                if(result.x > mStars[i].transform.position.x)
                {
                    result = mStars[i].transform.position;
                }
            }
            //else if(result.x < -9998 && mStars[i].sprite.name.Contains("0"))
            //{
            //    result = mStars[i].transform.position;
            //}
        }
        return result;
    }



    public void Reset()
    {
        Reset(-1);
    }
    public void Reset(int all_guanka_number)
    {
        ResetData(all_guanka_number);
        MoveIn();

    }
    public void ResetNotMoveIn()
    {
        ResetData(-1);
    }
    void ResetData(int all_guanka_number)
    {
        mCurentGameConfig = FormManager.config_games[ApkInfo.g_begin_scene];
        if (-1 == all_guanka_number)
            all_guanka_number = mCurentGameConfig.m_all_guanka;

        mTitle.text = mCurentGameConfig.m_module + ">>" + mCurentGameConfig.m_game_name;

        int index = 0;
        mStars[index].gameObject.SetActive(true);
        mStars[index].sprite = ResManager.GetSprite(ab_name, guanka_curent);
        for (index++; index < all_guanka_number; index++)
        {
            mStars[index].gameObject.SetActive(true);
            mStars[index].sprite = ResManager.GetSprite(ab_name, guanka_black);
        }
        for (; index < mStars.Count; index++)
        {
            mStars[index].gameObject.SetActive(false);
        }

    }

    public void AddStar()
    {
        for(int i = 0; i < mStars.Count; i++)
        {
            if(mStars[i].gameObject.activeSelf && mStars[i].sprite.name.Equals(guanka_curent))
            {
                mStars[i].sprite = ResManager.GetSprite(ab_name, guanka_show);
                if(i + 1 < mStars.Count && mStars[i + 1].gameObject.activeSelf)
                {
                    SoundManager.instance.PlayShort("03-星星状态栏");
                    mStars[i + 1].sprite = ResManager.GetSprite(ab_name, guanka_curent);
                }
                else
                {
                    //AndroidDataCtl.DoAndroidFunc("onCompleteOneGame");
                    //Debug.Log("安卓通信：发送打通游戏信号");
                }
                AndroidDataCtl.DoAndroidFunc("completeOneLevel");
                SoundManager.instance.PlayShort("gameover_star", 0.5f);
                Debug.Log("安卓通信：发送通关信号completeOneLevel");
                break;
            }
        }
    }



    public void OnClkReturn()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        Application.Quit();
    }
    public void OnClkTipBtn()
    {
        
        switch (mSoundTipData.play_type)
        {
            case "tip":
                {
                    if (null != mSoundTipData.play_com)
                    {
                        mSoundTipData.play_com.PlayTip(mSoundTipData.ab_names[0], mSoundTipData.sound_names[0]);
                    }
                }
                break;
            case "list":
                {
                    if (null != mSoundTipData.play_com)
                    {
                        mSoundTipData.play_com.PlayTipList(mSoundTipData.ab_names, mSoundTipData.sound_names, null);
                    }
                }
                break;
            case "callback":
                {
                    if(null != mSoundTipData.callback_func)
                    {
                        mSoundTipData.callback_func();
                    }
                }
                break;
            default:
                {

                }
                break;

        }
        

    }



    public void MoveIn()
    {
        Debug.Log("TopTitleCtl.MoveIn()");
        if(mRtran.anchoredPosition.y > 1)
        {
            StartCoroutine(TMoveIn());
        }
    }
    IEnumerator TMoveIn()
    {
        Vector2 pos0 = mRtran.anchoredPosition;
        for(float i = 0; i < 1f; i += 0.03f)
        {
            mRtran.anchoredPosition = Vector2.Lerp(pos0, Vector2.zero, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition = Vector2.zero;

    }

    
    

}
