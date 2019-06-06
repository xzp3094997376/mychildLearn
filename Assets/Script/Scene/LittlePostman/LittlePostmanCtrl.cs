using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LittlePostmanCtrl : BaseScene
{
    public int nLevel = 1;
    public const int nLevels = 4;
    public bool bLvPass = false;


    private RawImage imgBG;
    private Image imgPostBoxpanel;
    private PlaySoundController mSoundCtrl;
    public PlaySoundController SoundCtrl { get { return mSoundCtrl; } }

    private GameObject mPostBoxsTrans;
    //信箱s
    private PostBoxCtrl[] mPostBoxs = new PostBoxCtrl[8];
    //信件
    private LittlePostMsgObj postMsgObj;
    //手
    private LittlePostHand postHand;
    /// <summary>
    /// 当前信碰到的postbox
    /// </summary>
    public PostBoxCtrl hitPostBoxNow;

    /// <summary>
    /// 答案层(前两个分别作第一关的target 和第三关的target)
    /// </summary>
    public List<int> mAnswerList = new List<int>();


    //参考层
    int targetCengId = 0;
    PostBoxCtrl targetBox = null;
    //答案层
    int answerCengID = 0;
    PostBoxCtrl answerBox = null;


    void Awake()
    {
        mSceneType = SceneEnum.LittlePostman;
        CallLoadFinishEvent();

        imgBG = UguiMaker.newRawImage("bg", transform, "littlepostman_texture", "littlepostman", false);
        imgBG.rectTransform.sizeDelta = new Vector2(1280f, 800f);
    }

    // Use this for initialization
    void Start ()
    {
        mPostBoxsTrans = UguiMaker.newGameObject("postboxs", transform);
        mPostBoxsTrans.transform.localPosition = new Vector3(0f, 48f, 0f);
        mPostBoxsTrans.transform.localScale = Vector3.one * 1.2f;
        imgPostBoxpanel = UguiMaker.newImage("postpanel", mPostBoxsTrans.transform, "littlepostman_sprite", "postpanel", false);

        mSoundCtrl = gameObject.AddComponent<PlaySoundController>();
        mSoundCtrl.InitAwake();
        StartCoroutine(ieCreatePostBox());
    }
    IEnumerator ieCreatePostBox()
    {
        yield return new WaitForSeconds(0.1f);
        mSoundCtrl.SetDelayLoadBGClip(1f);
        mSoundCtrl.PlayBGSound1("bgmusic_loop1", "bgmusic_loop1", 0.2f);

        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);
        

        List<int> animalIDList = Common.GetIDList(1, 14, 8, -1);
        List<float> fIndexList = Common.GetOrderList(8, 72f);
        for (int i = 0; i < mPostBoxs.Length; i++)
        {
            PostBoxCtrl postbox = ResManager.GetPrefab("littlepostman_prefab", "postbox").AddComponent<PostBoxCtrl>();
            postbox.transform.SetParent(mPostBoxsTrans.transform);
            postbox.transform.localScale = Vector3.one;
            postbox.transform.localPosition = new Vector3(-30f, fIndexList[i] - 55f, 0f);
            postbox.InitAwake(i + 1);
            postbox.nAnimalID = animalIDList[i];
            mPostBoxs[i] = postbox;
            yield return new WaitForSeconds(0.1f);
        }

        postMsgObj = UguiMaker.newGameObject("postMsgObj", transform).AddComponent<LittlePostMsgObj>();
        postMsgObj.InitAwake();

        postHand = UguiMaker.newGameObject("postHand", transform).AddComponent<LittlePostHand>();
        postHand.InitAwake();

        mAnswerList = Common.GetIDList(1, 8, 6, -1);
        InitLevelData();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    //InitLevelData();
        //    postMsgObj.SetXin(true);
        //}
        if (bLvPass)
            return;
        MUpdate();
    }


    /// <summary>
    /// 重置信息
    /// </summary>
    public void ResetInfos()
    {
        postMsgObj.ResetInfos();
    }

    /// <summary>
    /// 初始化关卡
    /// </summary>
    public void InitLevelData()
    {
        ResetInfos();
        bLvPass = false;
        bPlayOtherTip = false;

        //参考层
        targetCengId = 0;
        targetBox = null;
        //答案层
        answerCengID = 0;
        answerBox = null;
        
        if (nLevel == 1)
        {
            targetCengId = mAnswerList[0];
            targetBox = mPostBoxs[targetCengId - 1];

            answerCengID = mAnswerList[2];
            answerBox = mPostBoxs[answerCengID - 1];
        }
        else if (nLevel == 2)
        {
            targetCengId = mAnswerList[2];
            targetBox = mPostBoxs[targetCengId - 1];

            answerCengID = mAnswerList[3];
            answerBox = mPostBoxs[answerCengID - 1];
        }
        else if (nLevel == 3)
        {
            targetCengId = mAnswerList[1];
            targetBox = mPostBoxs[targetCengId - 1];

            answerCengID = mAnswerList[4];
            answerBox = mPostBoxs[answerCengID - 1];
        }
        else
        {
            targetCengId = mAnswerList[4];
            targetBox = mPostBoxs[targetCengId - 1];

            answerCengID = mAnswerList[5];
            answerBox = mPostBoxs[answerCengID - 1];
        }

        //设置信属于哪个动物
        postMsgObj.SetAnimalID(answerBox.nAnimalID);
        postHand.GetPostMsg(postMsgObj);
        postHand.ShowHand();


        PlayTipSound();

        
    }

    /// <summary>
    /// 进场/退场
    /// </summary>
    /// <param name="_in"></param>
    public void SceneMove(bool _in)
    {
    }

    /// <summary>
    /// 下一关
    /// </summary>
    public void LevelCheckNext()
    {
        bLvPass = true;
        StartCoroutine(IETOver());
    }
    IEnumerator IETOver()
    {
        mSoundCtrl.StopTipSound();
        bPlayOtherTip = true;
        yield return new WaitForSeconds(0.5f);

        List<AudioClip> cpList = new List<AudioClip>();
        cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s喔噢谢谢你吧"));
        string animalName = MDefine.GetAnimalNameByID_CH(answerBox.nAnimalID);
        cpList.Add(mSoundCtrl.GetClip("aa_animal_name", animalName));
        cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s把他的信放到信箱里"));
        for (int i = 0; i < cpList.Count; i++)
        {
            mSoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }

        TopTitleCtl.instance.AddStar();
        nLevel++;
        if (nLevel > nLevels)
        {
            //结算
            GameOverCtl.GetInstance().Show(4, RePlayGame);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            SceneMove(false);
            yield return new WaitForSeconds(1.1f);
            InitLevelData();
        }
    }
    /// <summary>
    /// 重玩
    /// </summary>
    private void RePlayGame()
    {
        nLevel = 1;
        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);
        //重新设置动物的信箱
        List<int> animalIDList = Common.GetIDList(1, 14, 8, -1);
        for (int i = 0; i < mPostBoxs.Length; i++)
        {
            mPostBoxs[i].ReplayReset(animalIDList[i]);
        }
        //重新设置答案
        mAnswerList = Common.GetIDList(1, 8, 6, -1);
        InitLevelData();
    }

    private LittlePostMsgObj mSelect;
    private Vector3 temp_select_offset;
    private void MUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            #region//one
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hits != null)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    LittlePostMsgObj com = hits[i].collider.gameObject.GetComponent<LittlePostMsgObj>();
                    if (com != null && com == postMsgObj)
                    {
                        mSelect = com;
                        temp_select_offset = Common.getMouseLocalPos(transform) - mSelect.transform.localPosition;
                    }
                }
            }
            #endregion
        }
        else if (Input.GetMouseButton(0) && mSelect != null)
        {
            #region//two
            if (mSelect.nState == 1)
            {
                Vector3 vInput = Common.getMouseLocalPos(transform) - temp_select_offset;
                float fX = vInput.x;
                float fY = vInput.y;
                Vector3 vsize = mSelect.GetSize();
                fX = Mathf.Clamp(fX, -GlobalParam.screen_width * 0.5f + vsize.x * 0.5f, GlobalParam.screen_width * 0.5f - vsize.x * 0.5f);
                fY = Mathf.Clamp(fY, -GlobalParam.screen_height * 0.5f + vsize.y * 0.5f, GlobalParam.screen_height * 0.5f - vsize.y * 0.5f);
                mSelect.transform.localPosition = new Vector3(fX, fY, 0f);
            }
            #endregion
        }
        else if(Input.GetMouseButtonUp(0) && mSelect != null)
        {
            #region//three
            if (mSelect.nState == 0)
            {
                postHand.LostPostMsg();
            }
            else if (mSelect.nState == 1)
            {
                if (hitPostBoxNow != null && hitPostBoxNow.bPostBoxActive)
                {
                    if (hitPostBoxNow.nAnimalID == mSelect.nAnimalID)
                    {                      
                        hitPostBoxNow.PlayXinSetIn(mSelect, true);
                    }
                    else
                    {
                        hitPostBoxNow.PlayXinSetIn(mSelect, false);
                    }
                }
            }
            mSelect = null;
            #endregion
        }

    }

    /// <summary>
    /// 取得动物的邮箱
    /// </summary>
    /// <param name="_animalID"></param>
    /// <returns></returns>
    public PostBoxCtrl GetPostBoxObjByAnimalID(int _animalID)
    {
        for (int i = 0; i < mPostBoxs.Length; i++)
        {
            if (mPostBoxs[i].nAnimalID == _animalID)
            {
                return mPostBoxs[i];
            }
        }
        return null;
    }


    #region//sound ctrl
    public bool bPlayOtherTip = false;
    public void PlayTipSound()
    {
        if (bLvPass)
            return;
        if (bPlayOtherTip)
            return;

        mSoundCtrl.PlayTipSound(iePlayTipSound());
    }

    //玩法提示语音
    IEnumerator iePlayTipSound()
    {
        yield return new WaitForSeconds(0.1f);
        //A的信箱往上数4层就是8号信箱？猜猜A的信箱是几号信箱？帮A把信件放在他的信箱中吧！
        //B的信箱比A高2层，请你帮B把信件放在他的信箱中吧！
        //C的信箱比10层低5层，请你帮C把信件放在他的信箱中吧！
        //D的信箱比C层高5层，请你帮D把信件放在他的信箱中吧！

        List<AudioClip> cpList = new List<AudioClip>();

        int targetAnimalID = targetBox.nAnimalID;
        int answerAnimalID = answerBox.nAnimalID;
        //string strDebug = "";
        string animalName = MDefine.GetAnimalNameByID_CH(answerAnimalID);
        string strTarAnimalName = MDefine.GetAnimalNameByID_CH(targetAnimalID);
        //strDebug = animalName + "的信箱";

        cpList.Add(mSoundCtrl.GetClip("aa_animal_name", animalName));

        if (nLevel == 1)
        {
            //strDebug = strDebug + "往";
            int chaValue = targetCengId - answerCengID;
            if (chaValue > 0)
            {
                //strDebug = strDebug + "上数";
                cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s的信箱往上数"));
            }
            else
            {
                //strDebug = strDebug + "下数";
                cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s的信箱往下数"));
            }
            //strDebug = strDebug + Mathf.Abs(chaValue) + "层就是" + targetCengId + "号信箱,猜猜" + animalName + "的信箱是几号信箱？帮" + animalName + "把信件放在他的信箱中吧";

            int nCha = Mathf.Abs(chaValue);
            cpList.Add(mSoundCtrl.GetClip("number_sound", nCha.ToString()));
            cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s层"));
            cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s就是"));
            cpList.Add(mSoundCtrl.GetClip("number_sound", targetCengId.ToString()));
            cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s号信箱"));
            cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s猜猜"));
            cpList.Add(mSoundCtrl.GetClip("aa_animal_name", animalName));
            cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s的信箱是几号信箱"));
            cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s帮他把信件放在他的信箱中吧"));
        }
        else
        {
            cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s的信箱"));
            cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s比"));
            if (nLevel == 3)
            {
                //strDebug = strDebug + "比" + targetBox.nCeng;
                cpList.Add(mSoundCtrl.GetClip("number_sound", targetBox.nCeng.ToString()));
                cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s层"));
            }
            else
            {
                //strDebug = strDebug + "比" + strTarAnimalName;
                cpList.Add(mSoundCtrl.GetClip("aa_animal_name", strTarAnimalName));
            }
            
            int chaValue = targetCengId - answerCengID;
            if (chaValue < 0)
            {
                //strDebug = strDebug + "高";
                cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s高"));
            }
            else
            {
                //strDebug = strDebug + "低";
                cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s低"));
            }
            //strDebug = strDebug + Mathf.Abs(chaValue) + "层,请你帮" + animalName + "把信件放在他的信箱中吧";

            int nCha = Mathf.Abs(chaValue);
            cpList.Add(mSoundCtrl.GetClip("number_sound", nCha.ToString()));
            cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s层"));
            cpList.Add(mSoundCtrl.GetClip("littlepostman_sound", "s帮他把信件放在他的信箱中吧"));
        }
        //Debug.Log(strDebug);

        for (int i = 0; i < cpList.Count; i++)
        {
            mSoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
    }

    /// <summary>
    /// 播放失败提示语音
    /// </summary>
    public void PlayFaileTipSound()
    {
        mSoundCtrl.StopTipSound();
        string strClip = "s就差一点点了";
        if(Random.value >0.5f)
            strClip = "s你一定能填对";
        AudioClip cp = mSoundCtrl.GetClip("littlepostman_sound", strClip);
        mSoundCtrl.PlaySound(cp, 1f);
    }

    
    #endregion



}
