using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Wff_level1Ctrl : MonoBehaviour
{

    private WhoFlyFastCtrl mCtrl;

    private List<float> mMoveOutTimeList = new List<float>();
    AudioClip button_down;
    AudioClip button_up;

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as WhoFlyFastCtrl;

        button_down = Resources.Load<AudioClip>("sound/button_down");
        button_up = Resources.Load<AudioClip>("sound/button_up");

        float findex = 3f;
        for (int i = 0; i < 7; i++)
        {
            float getT = findex + 0.5f * i;
            mMoveOutTimeList.Add(getT);
        }
    }

    public void ResetInfos()
    {
        bOpenClick = true;
        bOpenCheck = false;
    }

    public void SetData()
    {
        ResetInfos();
        //checkBtn
        EventTriggerListener.Get(mCtrl.mBtnClick.gameObject).onDown = ClickDown;
        EventTriggerListener.Get(mCtrl.mBtnClick.gameObject).onUp = ClickUp;
        EventTriggerListener.Get(mCtrl.mBtnCheck.gameObject).onDown = CheckDown;
        EventTriggerListener.Get(mCtrl.mBtnCheck.gameObject).onUp = CheckUp;

        List<int> animalIDs = Common.GetIDList(1, 14, 5, 11);
        mCtrl.fLineYs = Common.BreakRank(mCtrl.fLineYs);
        mMoveOutTimeList = Common.BreakRank(mMoveOutTimeList);

        mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("whoflyfast_sound", "rocketin"));

        for (int i = 0; i < 5; i++)
        {
            mCtrl.mSceneRockets[i].SetData(animalIDs[i], mCtrl.fLineYs[i], mMoveOutTimeList[i], true);         
            mCtrl.mSceneRockets[i].SetBox2DActive(false);
            mCtrl.mSceneRockets[i].SetState(true);
            mCtrl.mScenePanels[i].SetState(true);
        }

        for (int i = 0; i < 5; i++)
        {
            mCtrl.mScenePanels[i].bMove = true;
        }

        mCtrl.StartCoroutine(ieWaiteSet());
    }
    IEnumerator ieWaiteSet()
    {
        yield return new WaitForSeconds(3f);
        mCtrl.mAudioRocketfly.Play();
        yield return new WaitForSeconds(3.1f);
        mCtrl.mBtnClick.transform.localScale = Vector3.one * 0.001f;
        mCtrl.mBtnClick.gameObject.SetActive(true);
        mCtrl.mBtnClick.transform.DOScale(Vector3.one, 0.2f);
        bOpenClick = true;
        //play tip sound
        mCtrl.PlayTipSound();
        //create guide
        Vector3 guidePos = mCtrl.mBtnClick.transform.position;
        mCtrl.GuideClick(guidePos, new Vector3(6f, -24f, 0f));
    }

    #region//Btn
    bool bOpenClick = false;
    //停止按钮按下
    private void ClickDown(GameObject _go)
    {
        if (mCtrl.nLevel != 1)
            return;
        if (mCtrl.bLvPass)
            return;
        if (!bOpenClick)
            return;
        mCtrl.SoundCtrl.PlaySortSound(button_down);
        mCtrl.mBtnClick.sprite = ResManager.GetSprite("whoflyfast_sprite", "play2");
        //停止点击按钮
        mCtrl.GuideStop();
    }
    //停止按钮弹起
    private void ClickUp(GameObject _go)
    {
        if (mCtrl.nLevel != 1)
            return;
        if (mCtrl.bLvPass)
            return;
        if (!bOpenClick)
            return;
        for (int i = 0; i < 5; i++)
        {
            mCtrl.mScenePanels[i].SetState(false);
            mCtrl.mSceneRockets[i].SetState(false);
            mCtrl.mSceneRockets[i].StopPingpong();
        }

        mCtrl.SetDirffientFast();

        mCtrl.mBtnClick.transform.DOScale(Vector3.one * 0.001f, 0.2f).OnComplete(() =>
        {
            mCtrl.mBtnClick.gameObject.SetActive(false);

            mCtrl.mBtnCheck.transform.localScale = Vector3.one * 0.001f;
            mCtrl.mBtnCheck.gameObject.SetActive(true);
            mCtrl.mBtnCheck.transform.DOScale(Vector3.one, 0.2f);
        });
        StartCoroutine(ieShowCheckBtn());
    }

    IEnumerator ieShowCheckBtn()
    {
        yield return new WaitForSeconds(0.5f);
        bOpenCheck = true;

        mCtrl.PlayTipSound();
        //点击火箭圆圈指引
        Vector3 guidePos = mCtrl.mSceneRockets[0].GetImgNum().transform.position;
        mCtrl.GuideClick(guidePos, new Vector3(0f, -30f, 0f));
    }


    bool bOpenCheck = false;
    //提交按钮按下
    private void CheckDown(GameObject _go)
    {
        if (mCtrl.nLevel != 1)
            return;
        if (mCtrl.bLvPass)
            return;
        if (!bOpenCheck)
            return;
        if (button_down == null)
        { button_down = Resources.Load<AudioClip>("sound/button_down"); }
        mCtrl.SoundCtrl.PlaySortSound(button_down);
        mCtrl.mBtnCheck.sprite = ResManager.GetSprite("whoflyfast_sprite", "yes2");
    }
    //提交按钮弹起
    private void CheckUp(GameObject _go)
    {
        if (mCtrl.nLevel != 1)
            return;
        if (mCtrl.bLvPass)
            return;
        if (!bOpenCheck)
            return;

        bool bAllSetNum = mCtrl.CheckIsAllSetNumber();
        if (!bAllSetNum)
        {
            //Debug.Log("还有一些小圆圈里面没有数字哦");
            PlayNotAllHaveNumber();
            mCtrl.mBtnCheck.sprite = ResManager.GetSprite("whoflyfast_sprite", "yes1");
            return;
        }

        bool bCheckOK = mCtrl.CheckIsOrderOK();
        if (bCheckOK)
        {
            for (int i=0;i<5;i++)
            {
                mCtrl.mSceneRockets[i].SetNumBox2DActive(false);
            }
            PlayLevel1SucEndSound();
        }
        else
        {
            //Debug.Log("这些火箭的快慢顺序不对哦/排在前面的比排在后面的要快一些呢");
            PlayWrongCheckSound();
            mCtrl.mBtnCheck.sprite = ResManager.GetSprite("whoflyfast_sprite", "yes1");
        }

        if (mCtrl.mSceneInputCtrl.gameObject.activeSelf)
        {
            mCtrl.mSceneInputCtrl.HideInputObj();
        }
    }
    #endregion

    WFF_RocketObj mSelect = null;
    void Update ()
    {
        if (!bOpenCheck)
            return;
        if (mCtrl.mSceneInputCtrl.gameObject.activeSelf)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            #region//one
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            for (int i = 0; i < hits.Length; i++)
            {
                WFF_RocketObj com = hits[i].collider.gameObject.transform.parent.parent.GetComponent<WFF_RocketObj>();
                if (com != null)
                {
                    if (mSelect == null)
                    {
                        mSelect = com;
                    }
                    else if (com.transform.GetSiblingIndex() > mSelect.transform.GetSiblingIndex())
                    {
                        mSelect = com;
                    }
                    if (mSelect != null)
                    {
                        mCtrl.GuideStop();
                    }
                }
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//two
            if (mSelect != null)
            {
                mSelect.transform.SetSiblingIndex(10);
                if (!mCtrl.mSceneInputCtrl.gameObject.activeSelf)
                {
                    mCtrl.mSceneInputCtrl.ShowInputObj(mSelect);
                }
                mSelect = null;
            }
            #endregion
        }
    }

    #region//Sound

    public IEnumerator iePlayTipSoundLv1()
    {
        List<AudioClip> cpList = new List<AudioClip>();
        yield return new WaitForSeconds(0.1f);
        if (!bOpenCheck)
        {
            cpList.Add(ResManager.GetClip("whoflyfast_sound", "点点停止按钮让这些火箭停下来吧"));
        }
        else
        {
            cpList.Add(ResManager.GetClip("whoflyfast_sound", "哪个火箭最快"));
            cpList.Add(ResManager.GetClip("whoflyfast_sound", "哪个火箭最慢"));
            cpList.Add(ResManager.GetClip("whoflyfast_sound", "点击火箭上的小圆圈给它们排序吧"));
        }
        for (int i = 0; i < cpList.Count; i++)
        {
            mCtrl.SoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
        if (!bOpenCheck)
        {
            yield return new WaitForSeconds(10f);
            mCtrl.PlayTipSound();
        }
    }
    /// <summary>
    /// 播放没有填完数字的语音
    /// </summary>
    public void PlayNotAllHaveNumber()
    {
        mCtrl.SoundCtrl.StopTipSound();
        AudioClip cp = ResManager.GetClip("whoflyfast_sound", "还有一些小圆圈里面没有数字哦");
        mCtrl.SoundCtrl.PlaySound(cp, 1f);
    }
    /// <summary>
    /// 播放排序错误语音
    /// </summary>
    public void PlayWrongCheckSound()
    {
        string strCp = "这些火箭的快慢顺序不对哦";
        if (UnityEngine.Random.value > 0.5f)
        { strCp = "排在前面的比排在后面的要快一些呢"; }
        mCtrl.SoundCtrl.StopTipSound();
        AudioClip cp = ResManager.GetClip("whoflyfast_sound", strCp);
        mCtrl.SoundCtrl.PlaySound(cp, 1f);
    }
    /// <summary>
    /// 播放第一关结束语音
    /// </summary>
    public void PlayLevel1SucEndSound()
    {
        mCtrl.bLvPass = true;
        mCtrl.bPlayOtherTip = true;
        mCtrl.StartCoroutine(iePlayLevel1SucEndSound());
    }
    IEnumerator iePlayLevel1SucEndSound()
    {
        mCtrl.SoundCtrl.StopTipSound();
        yield return new WaitForSeconds(0.1f);

        AudioClip cpGood = ResManager.GetClip("whoflyfast_sound", "哇哦做得很好");
        mCtrl.SoundCtrl.PlaySound(cpGood, 1f);
        yield return new WaitForSeconds(cpGood.length + 0.2f);

        //最快
        List<AudioClip> cpList1 = new List<AudioClip>();
        WFF_RocketObj rocket1 = mCtrl.mSceneRockets[4];
        rocket1.DoScaleEffect();
        rocket1.PlayAnimation("face_sayyes", true);
        string strAnimal1 = MDefine.GetAnimalNameByID_CH(rocket1.nAnimalID);
        cpList1.Add(ResManager.GetClip("aa_animal_name", strAnimal1));
        cpList1.Add(ResManager.GetClip("whoflyfast_sound", "的火箭最快"));
        cpList1.Add(ResManager.GetClip("whoflyfast_sound", "排在第1"));
        for (int i = 0; i < cpList1.Count; i++)
        {
            mCtrl.SoundCtrl.PlaySound(cpList1[i], 1f);
            yield return new WaitForSeconds(cpList1[i].length);
        }
        //最慢
        List<AudioClip> cpList5 = new List<AudioClip>();
        WFF_RocketObj rocket5 = mCtrl.mSceneRockets[0];
        rocket5.DoScaleEffect();
        rocket5.PlayAnimation("face_sayno", true);
        string strAnimal5 = MDefine.GetAnimalNameByID_CH(rocket5.nAnimalID);
        cpList5.Add(ResManager.GetClip("aa_animal_name", strAnimal5));
        cpList5.Add(ResManager.GetClip("whoflyfast_sound", "的火箭最慢"));
        cpList5.Add(ResManager.GetClip("whoflyfast_sound", "排在第5"));
        for (int i = 0; i < cpList5.Count; i++)
        {
            mCtrl.SoundCtrl.PlaySound(cpList5[i], 1f);
            yield return new WaitForSeconds(cpList5[i].length);
        }
        //第二
        List<AudioClip> cpList2 = new List<AudioClip>();
        WFF_RocketObj rocket2 = mCtrl.mSceneRockets[3];
        rocket2.DoScaleEffect();
        rocket2.PlayAnimation("face_sayyes", true);
        string strAnimal2 = MDefine.GetAnimalNameByID_CH(rocket2.nAnimalID);
        cpList2.Add(ResManager.GetClip("aa_animal_name", strAnimal2));
        cpList2.Add(ResManager.GetClip("whoflyfast_sound", "的火箭"));
        cpList2.Add(ResManager.GetClip("whoflyfast_sound", "排在第2"));
        for (int i = 0; i < cpList2.Count; i++)
        {
            mCtrl.SoundCtrl.PlaySound(cpList2[i], 1f);
            yield return new WaitForSeconds(cpList2[i].length);
        }
        //第三
        List<AudioClip> cpList3 = new List<AudioClip>();
        WFF_RocketObj rocket3 = mCtrl.mSceneRockets[2];
        rocket3.DoScaleEffect();
        rocket3.PlayAnimation("face_sayyes", true);
        string strAnimal3 = MDefine.GetAnimalNameByID_CH(rocket3.nAnimalID);
        cpList3.Add(ResManager.GetClip("aa_animal_name", strAnimal3));
        cpList3.Add(ResManager.GetClip("whoflyfast_sound", "的火箭"));
        cpList3.Add(ResManager.GetClip("whoflyfast_sound", "排在第3"));
        for (int i = 0; i < cpList3.Count; i++)
        {
            mCtrl.SoundCtrl.PlaySound(cpList3[i], 1f);
            yield return new WaitForSeconds(cpList3[i].length);
        }
        //第四
        List<AudioClip> cpList4 = new List<AudioClip>();
        WFF_RocketObj rocket4 = mCtrl.mSceneRockets[1];
        rocket4.DoScaleEffect();
        rocket4.PlayAnimation("face_sayyes", true);
        string strAnimal4 = MDefine.GetAnimalNameByID_CH(rocket4.nAnimalID);
        cpList4.Add(ResManager.GetClip("aa_animal_name", strAnimal4));
        cpList4.Add(ResManager.GetClip("whoflyfast_sound", "的火箭"));
        cpList4.Add(ResManager.GetClip("whoflyfast_sound", "排在第4"));
        for (int i = 0; i < cpList4.Count; i++)
        {
            mCtrl.SoundCtrl.PlaySound(cpList4[i], 1f);
            yield return new WaitForSeconds(cpList4[i].length);
        }

        yield return new WaitForSeconds(0.5f);

        mCtrl.mBtnCheck.transform.DOScale(Vector3.one * 0.001f, 0.2f).OnComplete(() =>
        {
            mCtrl.mBtnCheck.gameObject.SetActive(false);
        });

        mCtrl.LevelCheckNext();
    }

    #endregion

}
