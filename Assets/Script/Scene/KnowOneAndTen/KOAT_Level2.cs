using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class KOAT_Level2 : KOAT_SceneBase
{

    private GameObject mObjRes;

    private Image btnCheck;
    Vector3 vCheck = new Vector3(400f, -340f, 0f);

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

    private List<KOAT_BallObj> mBallLeftList = new List<KOAT_BallObj>();
    private List<KOAT_BallObj> mBallRightList = new List<KOAT_BallObj>();

    public override void ResetInfos()
    {
        base.ResetInfos();
        nGameCount = 0;

        mTargetList.Clear();
        mTargetList = Common.GetIDList(11, 19, 2, 15);

        nTargetNum = 0;
        nLeft = 0;
        nRight = 0;
        bCanClick = false;

        bGuideOK = false;
        mHandClick = null;
        mBallRightList.Clear();
        mBallLeftList.Clear();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha5))
    //    {
    //        mHandClick.PlayClick();
    //    }
    //}

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
        nTargetNum = 15;
        nLeft = 1;
        nRight = 5;
        mCtrl.mRectBox.SetLeftNum(nLeft);
        mCtrl.mRectBox.SetRightNum(nRight);
        //十位数
        GameObject mGoLeft = UguiMaker.newGameObject("mForRight", mObjRes.transform);
        mGoLeft.transform.localPosition = new Vector3(-120f, 0f, 0f);
        for (int i = 0; i < 9; i++)
        {
            GameObject mgo = UguiMaker.newGameObject("ball", mGoLeft.transform);
            mgo.transform.localPosition = new Vector3(0f, 800f, 0f);
            KOAT_BallObj ballCtrl = mgo.AddComponent<KOAT_BallObj>();
            ballCtrl.InitAwake(nType);
            ballCtrl.SpineActive(false);
            ballCtrl.transform.DOLocalMoveY(findexList[i], fTime);
            if (i < nLeft)
                mBallLeftList.Add(ballCtrl);
            yield return new WaitForSeconds(fTime);
        }
        //个位数
        GameObject mGoRight = UguiMaker.newGameObject("mBalls0", mObjRes.transform);
        mGoRight.transform.localPosition = new Vector3(125f, 0f, 0f);
        for (int i = 0; i < 9; i++)
        {
            GameObject mgo = UguiMaker.newGameObject("ball", mGoRight.transform);
            mgo.transform.localPosition = new Vector3(0f, 800f, 0f);
            KOAT_BallObj ballCtrl = mgo.AddComponent<KOAT_BallObj>();
            ballCtrl.InitAwake(nType);
            ballCtrl.SpineActive(false);
            ballCtrl.transform.DOLocalMoveY(findexList[i], fTime);
            if (i < nRight)
                mBallLeftList.Add(ballCtrl);
            yield return new WaitForSeconds(fTime);
        }
        //move in
        float fInTime = 0.5f;
        mCtrl.mRectBox.SceneMove(true, fInTime);
        yield return new WaitForSeconds(fInTime);

        //hand click 10位-----
        mHandClick.transform.DOMove(mBallLeftList[0].transform.position, 0.2f);
        yield return new WaitForSeconds(0.2f);
        mHandClick.PlayClick();
        yield return new WaitForSeconds(0.2f);
        mBallLeftList[0].ClickShowSpine();
        yield return new WaitForSeconds(0.3f);
        //语音:十位上的数字是1，表示10个小怪兽
        AudioClip cp0 = ResManager.GetClip("knowoneandten_sound", "十位上的数字是1表示10个小怪兽");
        mCtrl.SoundCtrl.PlaySound(cp0, 1f);
        yield return new WaitForSeconds(cp0.length);

        //hand click 个位-----
        for (int i = 1; i < 6; i++)
        {
            mHandClick.transform.DOMove(mBallLeftList[i].transform.position, 0.2f);
            yield return new WaitForSeconds(0.2f);
            mHandClick.PlayClick();
            yield return new WaitForSeconds(0.2f);
            mBallLeftList[i].ClickShowSpine();
            yield return new WaitForSeconds(0.3f);
        }
        mHandClick.transform.DOLocalMoveX(1300f, 1f);
        //语音:个位上的数字是5，表示5个小怪兽，合起来表示15个小怪兽
        AudioClip cp1 = ResManager.GetClip("knowoneandten_sound", "个位上的数字是5表示5个小怪兽");
        mCtrl.SoundCtrl.PlaySound(cp1, 1f);
        yield return new WaitForSeconds(cp1.length);

        mObjRes.transform.DOLocalMoveX(-1300f, 0.5f);
        mCtrl.mRectBox.SceneMove(false, 0.5f);
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
        //bGuideOK = true;
        if (!bGuideOK)
        {
            PlayStartGuide();
        }
        else
        {
            btnCheck = UguiMaker.newImage("btnCheck", mObjRes.transform, "knowoneandten_sprite", "yes1");
            EventTriggerListener.Get(btnCheck.gameObject).onDown = CheckClickDown;
            EventTriggerListener.Get(btnCheck.gameObject).onUp = CheckClickUp;

            ResetResPos();

            nTargetNum = mTargetList[0];
            mTargetList.RemoveAt(0);
            nLeft = nTargetNum / 10;
            nRight = nTargetNum % 10;

            mCtrl.mRectBox.SetLeftNum(nLeft);
            mCtrl.mRectBox.SetRightNum(nRight);

            mCtrl.StartCoroutine(ieStartGame());
        }
    }


    /// <summary>
    /// 从重置资源位置
    /// </summary>
    private void ResetResPos()
    {
        btnCheck.transform.localPosition = vCheck - new Vector3(0f, 200f, 0f);
        mCtrl.mRectBox.SetOutSidePos();

        btnCheck.sprite = ResManager.GetSprite("knowoneandten_sprite", "yes1");

        mmLeft = 0;
        mmRight = 0;
        bCanClick = false;
        mBallLeftList.Clear();
        mBallRightList.Clear();
    }

    //开始
    IEnumerator ieStartGame()
    {
        float fTime = 0.2f;
        List<float> findexList = Common.GetOrderList(10, 55f);
        //十位
        GameObject mForLeft = UguiMaker.newGameObject("mForLeft", mObjRes.transform);
        mForLeft.transform.localPosition = new Vector3(-120f, 0f, 0f);
        for (int i = 0; i < 9; i++)
        {
            GameObject mgo = UguiMaker.newGameObject("ball", mForLeft.transform);
            mgo.transform.localPosition = new Vector3(0f, 800f, 0f);
            KOAT_BallObj ballCtrl = mgo.AddComponent<KOAT_BallObj>();
            ballCtrl.InitAwake(nType);
            ballCtrl.SpineActive(false);
            ballCtrl.ButtonActive(true);
            ballCtrl.transform.DOLocalMoveY(findexList[i], fTime);
            ballCtrl.SetClickCall(ClickObjCall);
            mBallLeftList.Add(ballCtrl);
            yield return new WaitForSeconds(fTime);
        }
        //个位
        GameObject mGoRight = UguiMaker.newGameObject("mGoRight", mObjRes.transform);
        mGoRight.transform.localPosition = new Vector3(125f, 0f, 0f);    
        for (int i = 0; i < 9; i++)
        {
            GameObject mgo = UguiMaker.newGameObject("ball", mGoRight.transform);
            mgo.transform.localPosition = new Vector3(0f, 800f, 0f);
            KOAT_BallObj ballCtrl = mgo.AddComponent<KOAT_BallObj>();
            ballCtrl.InitAwake(nType);
            ballCtrl.SpineActive(false);
            ballCtrl.ButtonActive(true);
            ballCtrl.transform.DOLocalMoveY(findexList[i], fTime);
            ballCtrl.SetClickCall(ClickObjCall);
            mBallRightList.Add(ballCtrl);
            yield return new WaitForSeconds(fTime);
        }
        //move in
        float fInTime = 0.5f;
        btnCheck.transform.DOLocalMove(vCheck, fInTime);
        mCtrl.mRectBox.SceneMove(true, fInTime);
        yield return new WaitForSeconds(fInTime);

        bCanClick = true;
        if (nGameCount <= 0)
        {
            mCtrl.PlayTipSound();
        }
    }

    private void ClickObjCall(KOAT_BallObj _obj)
    {        
        Transform objParent = _obj.transform.parent;
        if (objParent.gameObject.name.CompareTo("mForLeft") == 0)
        {
            if (_obj.IsActiveSpine())
            {
                if (_obj == mBallLeftList[mmLeft - 1])
                {
                    mmLeft--;
                    _obj.ClickShowSpine(false);
                }
            }
            else
            {
                if (_obj == mBallLeftList[mmLeft])
                {
                    mmLeft++;
                    _obj.ClickShowSpine(true);
                }
            }
            mmLeft = Mathf.Clamp(mmLeft, 0, 10);
        }
        else if (objParent.gameObject.name.CompareTo("mGoRight") == 0)
        {
            if (_obj.IsActiveSpine())
            {
                if (_obj == mBallRightList[mmRight - 1])
                {
                    mmRight--;
                    _obj.ClickShowSpine(false);
                }
            }
            else
            {
                if (_obj == mBallRightList[mmRight])
                {
                    mmRight++;
                    _obj.ClickShowSpine(true);
                }
            }
            mmRight = Mathf.Clamp(mmRight, 0, 10);
        }
    }

    private void CheckClickUp(GameObject _go)
    {
        if (!bCanClick)
            return;
        if (_go == btnCheck.gameObject)
        {
            int nget = mmLeft * 10 + mmRight;
            if (nget == nTargetNum)
            {
                nGameCount++;
                Debug.Log("ok---");
                RePlayGame();
            }
            else
            {
                Debug.Log("wrong---");
                btnCheck.sprite = ResManager.GetSprite("knowoneandten_sprite", "yes1");
                AudioClip cp0 = ResManager.GetClip("knowoneandten_sound", "呃有些地方涂得不对呢");
                mCtrl.SoundCtrl.PlaySound(cp0, 1f);
            }
        }
    }
    private void CheckClickDown(GameObject _go)
    {
        if (!bCanClick)
            return;
        if (_go == btnCheck.gameObject)
        {
            btnCheck.sprite = ResManager.GetSprite("knowoneandten_sprite", "yes2");
        }
    }

    //次数重玩
    public void RePlayGame()
    {
        bCanClick = false;
        mCtrl.StartCoroutine(ieRePlayGame());
    }
    IEnumerator ieRePlayGame()
    {
        yield return new WaitForSeconds(0.1f);
        AudioClip cp0 = ResManager.GetClip("knowoneandten_sound", "做得很好");
        mCtrl.SoundCtrl.PlaySound(cp0, 1f);
        yield return new WaitForSeconds(cp0.length);
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
        AudioClip cp0 = ResManager.GetClip("knowoneandten_sound", "仔细观察下面的数字然后涂出相应的小怪兽");
        mCtrl.SoundCtrl.PlaySound(cp0, 1f);
    }


}
