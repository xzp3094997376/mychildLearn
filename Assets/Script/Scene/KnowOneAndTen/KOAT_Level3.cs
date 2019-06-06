using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class KOAT_Level3 : KOAT_SceneBase
{

    private GameObject mObjRes;

    private int nToGameCount = 2;//总次数
    public int nGameCount = 0;//累积次数
    public List<int> mTargetList = new List<int>();//次数目标列表
    public int nTargetNum = 0;//目标数
    private int nLeft = 0;
    private int nRight = 0;

    public int mmRight = 0;
    public int mmLeft = 0;

    private bool bCanClick = false;

    public int nType = 1;//怪兽类型
    public bool bGuideOK = false;//演示是否完成
    private KOATHandClick mHandClick;

    private List<KOAT_BlockObj> mBallLeftList = new List<KOAT_BlockObj>();
    private List<KOAT_BlockObj> mBallRightList = new List<KOAT_BlockObj>();

    private KOAT_SuanShi mSuanShi;
    private InputNumObj mInputNumObj;

    public override void ResetInfos()
    {
        base.ResetInfos();

        nGameCount = 0;

        mTargetList.Clear();
        mTargetList = Common.GetIDList(11, 19, 2, 11);

        nTargetNum = 0;
        nLeft = 0;
        nRight = 0;
        bCanClick = false;

        bGuideOK = false;
        mHandClick = null;
        mSuanShi = null;
        mInputNumObj = null;
        mBallRightList.Clear();
        mBallLeftList.Clear();
    }

    /// <summary>
    /// 演示
    /// </summary>
    public void PlayStartGuide()
    {
        mHandClick = UguiMaker.newGameObject("handClick", transform).AddComponent<KOATHandClick>();
        mHandClick.transform.localPosition = new Vector3(0f, -800f, 0f);
        mHandClick.InitAwake();

        mCtrl.StartCoroutine(PlayGuideCtr());
    }
    private IEnumerator PlayGuideCtr()
    {
        mCtrl.bPlayOtherTip = true;

        List<float> findexList = Common.GetOrderList(10, 55f);
        float fTime = 0.2f;
        yield return new WaitForSeconds(0.1f);
        //十位数
        GameObject mGoLeft = UguiMaker.newGameObject("mLeftBlock", mObjRes.transform);
        mGoLeft.transform.localPosition = new Vector3(-160f, 0f, 0f);
        for (int i = 0; i < 10; i++)
        {
            GameObject mgo = UguiMaker.newGameObject("block", mGoLeft.transform);
            mgo.transform.localPosition = new Vector3(0f, 800f, 0f);
            KOAT_BlockObj ballCtrl = mgo.AddComponent<KOAT_BlockObj>();
            ballCtrl.InitAwake();
            ballCtrl.BlockActive(true);
            ballCtrl.transform.DOLocalMoveY(findexList[i], fTime);
            mBallLeftList.Add(ballCtrl);
            yield return new WaitForSeconds(fTime);
        }
        //个位数
        GameObject mGoRight = UguiMaker.newGameObject("mRightBlock", mObjRes.transform);
        mGoRight.transform.localPosition = new Vector3(160f, 0f, 0f);
        for (int i = 0; i < 10; i++)
        {
            GameObject mgo = UguiMaker.newGameObject("block", mGoRight.transform);
            mgo.transform.localPosition = new Vector3(0f, 800f, 0f);
            KOAT_BlockObj ballCtrl = mgo.AddComponent<KOAT_BlockObj>();
            ballCtrl.InitAwake();
            ballCtrl.transform.DOLocalMoveY(findexList[i], fTime);
            mBallRightList.Add(ballCtrl);
            yield return new WaitForSeconds(fTime);
        }

        //hand click 个位-----
        mHandClick.transform.DOMove(mBallRightList[0].transform.position, 0.2f);
        yield return new WaitForSeconds(0.2f);
        mHandClick.PlayClick();
        yield return new WaitForSeconds(0.2f);
        mBallRightList[0].ClickShowBlock(true);
        yield return new WaitForSeconds(0.3f);

        Debug.Log("语音:11可以分成10和1");
        yield return new WaitForSeconds(1f);
        //AudioClip cp0 = ResManager.GetClip("knowoneandten_sound", "十位上的数字是1表示10个小怪兽");
        //mCtrl.SoundCtrl.PlaySound(cp0, 1f);
        //yield return new WaitForSeconds(cp0.length);

        //出现题目
        mSuanShi.SceneMove(true, 0.5f);
        yield return new WaitForSeconds(0.51f);
        //小手点击...
        Vector3 vpos = mSuanShi.GetInputPos();
        mHandClick.transform.DOMove(vpos, 0.5f);
        yield return new WaitForSeconds(0.51f);
        mHandClick.PlayClick();
        yield return new WaitForSeconds(0.5f);
        //出现计算器...
        mInputNumObj.transform.position = mSuanShi.GetInputPos();
        mInputNumObj.ShowEffect();
        mInputNumObj.SetProtect(true);
        mInputNumObj.transform.DOLocalMove(new Vector3(0f, -175f, 0f), 0.3f);
        yield return new WaitForSeconds(0.3f);

        GameObject toGo = mInputNumObj.GetInputNumBlockByIndex(6).gameObject;
        mHandClick.transform.DOMove(toGo.transform.position, 0.5f);
        yield return new WaitForSeconds(0.51f);
        mHandClick.PlayClick();
        yield return new WaitForSeconds(0.2f);
        //输入数字1
        mSuanShi.SetNum1(nRight);
        mInputNumObj.SetProtect(false);
        mInputNumObj.HideEffect();
        yield return new WaitForSeconds(0.3f);
        mHandClick.transform.DOLocalMoveY(-600f, 0.5f);


        //语音:10加1等于11
        Debug.Log("语音:10加1等于11");
        yield return new WaitForSeconds(1f);

        mObjRes.transform.DOLocalMoveX(-1300f, 0.5f);
        //算式退出...
        yield return new WaitForSeconds(0.51f);

        Debug.Log("guide finish");
        bGuideOK = true;
        mCtrl.bPlayOtherTip = false;

        InitData();
    }



    public override void InitData()
    {
        base.InitData();

        if (mObjRes == null)
        {
            mObjRes = UguiMaker.newGameObject("mObjRes", transform);
        }
        mObjRes.transform.localPosition = Vector3.zero;
        Common.DestroyChilds(mObjRes.transform);

        nType = Random.Range(0, 3);
        //算式
        mSuanShi = UguiMaker.newGameObject("mSuanShi", mObjRes.transform).AddComponent<KOAT_SuanShi>();
        mSuanShi.InitAwake();
        //top
        Image imgTop = UguiMaker.newImage("imgTop", mObjRes.transform, "knowoneandten_sprite", "mblock0", false);
        imgTop.rectTransform.anchoredPosition = new Vector2(0f, 300f);
        miniImageNumber mtopNum = UguiMaker.newGameObject("mtopNum", imgTop.transform).AddComponent<miniImageNumber>();
        mtopNum.strABName = "knowoneandten_sprite";
        mtopNum.transform.localScale = Vector3.one * 0.3f;
        Image imgArrow0 = UguiMaker.newImage("imgArrow0", mObjRes.transform, "knowoneandten_sprite", "marrow0", false);
        imgArrow0.rectTransform.anchoredPosition = new Vector2(-90f, 280f);
        imgArrow0.transform.localScale = new Vector3(-1f, 1f, 1f);
        Image imgArrow1 = UguiMaker.newImage("imgArrow1", mObjRes.transform, "knowoneandten_sprite", "marrow0", false);
        imgArrow1.rectTransform.anchoredPosition = new Vector2(90f, 280f);
        //计算器
        InputInfoData infoda = new InputInfoData();
        infoda.strAlatsName = "knowoneandten_sprite";
        infoda.strPicBG = "zbgbg";
        infoda.strCellPicFirstName = "";
        infoda.fNumScale = 0.73f;
        infoda.bgcolor = Color.white;
        infoda.color_blockBG = new Color(221f / 255, 177f / 255, 118f / 255, 1f);
        infoda.color_blockNum = new Color(155f / 255, 93f / 255, 33f / 255, 1f);
        infoda.color_blockSureStart = new Color(155f / 255, 93f / 255, 33f / 255, 1f);
        infoda.vBgSize = new Vector2(690f, 620f);
        infoda.vCellSize = new Vector2(200f, 180f);
        infoda.vSpacing = new Vector2(10f, 10f);
        infoda.fscale = 0.3f;
        infoda.nConstraintCount = 3;
        mInputNumObj = InputNumObj.Create(transform, infoda);
        mInputNumObj.transform.localPosition = new Vector3(0f, -175f, 0f);
        mInputNumObj.gameObject.SetActive(false);

        //bGuideOK = true;
        if (!bGuideOK)
        {
            nTargetNum = 11;
            nLeft = 10;
            nRight = 1;           
            PlayStartGuide();
        }
        else
        {
            ResetResPos();
            nTargetNum = mTargetList[0];
            mTargetList.RemoveAt(0);
            nLeft = 10;
            nRight = nTargetNum % 10;
            mCtrl.StartCoroutine(ieStartGame());
        }

        mSuanShi.SetNum0(nLeft);
        mSuanShi.SetNum2(nTargetNum);
        mtopNum.SetNumber(nTargetNum);

    }


    /// <summary>
    /// 从重置资源位置
    /// </summary>
    private void ResetResPos()
    {
        mmLeft = 0;
        mmRight = 0;
        bCanClick = false;
        mBallLeftList.Clear();
        mBallRightList.Clear();
    }

    //开始
    IEnumerator ieStartGame()
    {
        mCtrl.bPlayOtherTip = true;

        List<float> findexList = Common.GetOrderList(10, 55f);
        float fTime = 0.2f;
        yield return new WaitForSeconds(0.1f);
        //十位数
        GameObject mGoLeft = UguiMaker.newGameObject("mLeftBlock", mObjRes.transform);
        mGoLeft.transform.localPosition = new Vector3(-160f, 0f, 0f);
        for (int i = 0; i < 10; i++)
        {
            GameObject mgo = UguiMaker.newGameObject("block", mGoLeft.transform);
            mgo.transform.localPosition = new Vector3(0f, 800f, 0f);
            KOAT_BlockObj ballCtrl = mgo.AddComponent<KOAT_BlockObj>();
            ballCtrl.InitAwake();
            ballCtrl.BlockActive(true);
            ballCtrl.transform.DOLocalMoveY(findexList[i], fTime);
            mBallLeftList.Add(ballCtrl);
            yield return new WaitForSeconds(fTime);
        }
        //个位数
        GameObject mGoRight = UguiMaker.newGameObject("mRightBlock", mObjRes.transform);
        mGoRight.transform.localPosition = new Vector3(160f, 0f, 0f);
        for (int i = 0; i < 10; i++)
        {
            GameObject mgo = UguiMaker.newGameObject("block", mGoRight.transform);
            mgo.transform.localPosition = new Vector3(0f, 800f, 0f);
            KOAT_BlockObj ballCtrl = mgo.AddComponent<KOAT_BlockObj>();
            ballCtrl.InitAwake();
            ballCtrl.ButtonActive(true);
            ballCtrl.transform.DOLocalMoveY(findexList[i], fTime);
            ballCtrl.SetClickCall(ClickCall);
            mBallRightList.Add(ballCtrl);
            yield return new WaitForSeconds(fTime);
        }

        bCanClick = true;
        mCtrl.PlayTipSound();
                
    }
    //算式题
    IEnumerator ieStartSuanShi()
    {
        mCtrl.bPlayOtherTip = true;

        //语音:X可以分成10和X1
        List<AudioClip> cpList = new List<AudioClip>();
        cpList.Add(ResManager.GetClip("number_sound", nTargetNum.ToString()));
        cpList.Add(ResManager.GetClip("knowoneandten_sound", "可以分成"));
        cpList.Add(ResManager.GetClip("number_sound", nLeft.ToString()));
        cpList.Add(ResManager.GetClip("knowoneandten_sound", "和"));
        cpList.Add(ResManager.GetClip("number_sound", nRight.ToString()));
        for (int i = 0; i < cpList.Count; i++)
        {
            mCtrl.SoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
        mCtrl.bPlayOtherTip = false;

        yield return new WaitForSeconds(0.1f);
        //move in
        float fInTime = 0.5f;
        mSuanShi.SceneMove(true, fInTime);
        yield return new WaitForSeconds(fInTime);

        mSuanShi.SetInputClickCall(ClickShowInputObj);
        mInputNumObj.SetInputNumberCallBack(InputFinishCall);

        bCanClick = true;
        mCtrl.PlayTipSound();
    }


    private void ClickCall(KOAT_BlockObj _obj)
    {
        if (!bCanClick)
            return;

        Transform objParent = _obj.transform.parent;
        if (objParent.gameObject.name.CompareTo("mRightBlock") == 0)
        {
            if (_obj.IsActiveBlock())
            {
                if (_obj == mBallRightList[mmRight - 1])
                {
                    mmRight--;
                    _obj.ClickShowBlock(false);
                }
            }
            else
            {
                if (_obj == mBallRightList[mmRight])
                {
                    mmRight++;
                    _obj.ClickShowBlock(true);
                    //第二阶段 算式
                    if (mmRight == nRight)
                    {
                        bCanClick = false;
                        mCtrl.StartCoroutine(ieStartSuanShi());
                    }
                }
            }
            mmRight = Mathf.Clamp(mmRight, 0, 10);
        }
    }

    /// <summary>
    /// 点击显示输入框
    /// </summary>
    private void ClickShowInputObj()
    {
        if (!mInputNumObj.gameObject.activeSelf)
        {
            mInputNumObj.transform.position = mSuanShi.GetInputPos();
            mInputNumObj.ShowEffect();
            mInputNumObj.transform.DOLocalMove(new Vector3(0f, -175f, 0f), 0.3f);
        }
        //Debug.Log("click");
    }

    private void InputFinishCall()
    {
        Debug.Log("输入了:" + mInputNumObj.strInputNum);
        int getNum = int.Parse(mInputNumObj.strInputNum);
        mSuanShi.SetNum1(getNum);

        if (getNum == nRight)
        {
            nGameCount++;
            Debug.Log("ok---");
            RePlayGame();
        }
        else
        {
            AudioClip cp0 = ResManager.GetClip("knowoneandten_sound", "呃有些地方涂得不对呢");
            mCtrl.SoundCtrl.PlaySound(cp0, 1f);

            mSuanShi.ShakeNumObj();
        }

        mInputNumObj.HideEffect();
    }



    //次数重玩
    public void RePlayGame()
    {
        bCanClick = false;
        mCtrl.StartCoroutine(ieRePlayGame());
    }
    IEnumerator ieRePlayGame()
    {
        mCtrl.bPlayOtherTip = true;
        //语音:10加X等于X1
        List<AudioClip> cpList = new List<AudioClip>();
        cpList.Add(ResManager.GetClip("number_sound", "10"));
        cpList.Add(ResManager.GetClip("knowoneandten_sound", "加"));
        cpList.Add(ResManager.GetClip("number_sound", nRight.ToString()));
        cpList.Add(ResManager.GetClip("knowoneandten_sound", "等于"));
        cpList.Add(ResManager.GetClip("number_sound", nTargetNum.ToString()));
        for (int i = 0; i < cpList.Count; i++)
        {
            mCtrl.SoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
        
        yield return new WaitForSeconds(0.1f);
        AudioClip cp0 = ResManager.GetClip("knowoneandten_sound", "做得很好");
        mCtrl.SoundCtrl.PlaySound(cp0, 1f);
        yield return new WaitForSeconds(cp0.length);
        mCtrl.bPlayOtherTip = false;
        if (nGameCount >= nToGameCount)
        {
            mCtrl.MLevelPass();
        }
        else
        {
            mObjRes.transform.DOLocalMoveX(-1400f, 0.5f);
            mCtrl.mRectBox.SceneMove(false, 0.3f);
            yield return new WaitForSeconds(0.52f);
            Common.DestroyChilds(mObjRes.transform);
            InitData();
        }
    }

    /// <summary>
    /// 提示语音
    /// </summary>
    public void PlaySoundTip()
    {
        //AudioClip cp0 = ResManager.GetClip("knowoneandten_sound", "仔细观察下面的数字然后涂出相应的小怪兽");
        //mCtrl.SoundCtrl.PlaySound(cp0, 1f);
    }

}
