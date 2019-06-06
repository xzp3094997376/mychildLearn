using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class KOAT_Level1 : KOAT_SceneBase
{

    private GameObject mObjRes;

    private Image btnUp0;//右加
    private Image btnDown0;//右减
    private Image btnCheck;
    private Image btnUp1;//左加
    private Image btnDown1;//左减

    Vector3 vUp0 = new Vector3(315f, -315f, 0f);
    Vector3 vDown0 = new Vector3(315f, -365f, 0f);
    Vector3 vCheck = new Vector3(400f, -340f, 0f);
    Vector3 vUp1 = new Vector3(-315f, -315f, 0f);
    Vector3 vDown1 = new Vector3(-315f, -365f, 0f);

    private int nToGameCount = 4;//总次数
    public int nGameCount = 0;//累积次数
    public List<int> mTargetList = new List<int>();//次数目标列表
    public int nTargetNum = 0;//目标数
    public int nLeft = 0;
    public int nRight = 0;

    private int mmRight = 0;
    private int mmLeft = 0;

    private bool bCanClick = false;

    public int nType = 1;

    public override void ResetInfos()
    {
        base.ResetInfos();
        nGameCount = 0;

        mTargetList.Clear();
        mTargetList = Common.GetIDList(11, 19, 4, -1);

        nTargetNum = 0;
        nLeft = 0;
        nRight = 0;
        bCanClick = false;
    }

    public override void InitData()
    {
        base.InitData();

        nType = Random.Range(0, 3);

        if (mObjRes == null)
        {
            mObjRes = UguiMaker.newGameObject("mObjRes", transform);
        }
        mObjRes.transform.localPosition = Vector3.zero;

        btnUp0 = UguiMaker.newImage("btnUp", mObjRes.transform, "knowoneandten_sprite", "marrow1");
        btnDown0 = UguiMaker.newImage("btnDown", mObjRes.transform, "knowoneandten_sprite", "marrow2");
        btnCheck = UguiMaker.newImage("btnCheck", mObjRes.transform, "knowoneandten_sprite", "yes1");
        btnUp1 = UguiMaker.newImage("btnUp1", mObjRes.transform, "knowoneandten_sprite", "marrow1");
        btnDown1 = UguiMaker.newImage("btnDown1", mObjRes.transform, "knowoneandten_sprite", "marrow2");

        EventTriggerListener.Get(btnUp0.gameObject).onDown = ClickDown;
        EventTriggerListener.Get(btnUp0.gameObject).onUp = ClickUp;

        EventTriggerListener.Get(btnDown0.gameObject).onDown = ClickDown;
        EventTriggerListener.Get(btnDown0.gameObject).onUp = ClickUp;

        EventTriggerListener.Get(btnCheck.gameObject).onDown = ClickDown;
        EventTriggerListener.Get(btnCheck.gameObject).onUp = ClickUp;

        EventTriggerListener.Get(btnUp1.gameObject).onDown = ClickDown;
        EventTriggerListener.Get(btnUp1.gameObject).onUp = ClickUp;

        EventTriggerListener.Get(btnDown1.gameObject).onDown = ClickDown;
        EventTriggerListener.Get(btnDown1.gameObject).onUp = ClickUp;

        ResetResPos();

        mCtrl.mRectBox.SetLeftNum(0);
        mCtrl.mRectBox.SetRightNum(0);

        nTargetNum = mTargetList[0];
        mTargetList.RemoveAt(0);

        mCtrl.StartCoroutine(ieStartGame());
    }

    /// <summary>
    /// 从重置资源位置
    /// </summary>
    private void ResetResPos()
    {
        btnUp0.transform.localPosition = vUp0 - new Vector3(0f, 200f, 0f);
        btnDown0.transform.localPosition = vDown0 - new Vector3(0f, 200f, 0f);
        btnCheck.transform.localPosition = vCheck - new Vector3(0f, 200f, 0f);
        btnUp1.transform.localPosition = vUp1 - new Vector3(0f, 200f, 0f);
        btnDown1.transform.localPosition = vDown1 - new Vector3(0f, 200f, 0f);
        mCtrl.mRectBox.SetOutSidePos();

        btnCheck.sprite = ResManager.GetSprite("knowoneandten_sprite", "yes1");

        mmLeft = 0;
        mmRight = 0;
        bCanClick = false;
    }

    //开始
    IEnumerator ieStartGame()
    {
        List<float> findexList = Common.GetOrderList(10, 55f);

        nLeft = nTargetNum / 10;
        nRight = nTargetNum % 10;

        GameObject mGoRight = UguiMaker.newGameObject("mBalls0", mObjRes.transform);
        mGoRight.transform.localPosition = new Vector3(125f, 0f, 0f);
       
        float fTime = 0.2f;
        for (int i = 0; i < nRight; i++)
        {
            GameObject mgo = UguiMaker.newGameObject("ball", mGoRight.transform);
            mgo.transform.localPosition = new Vector3(0f, 800f, 0f);
            KOAT_BallObj ballCtrl = mgo.AddComponent<KOAT_BallObj>();
            ballCtrl.InitAwake(nType);
            ballCtrl.transform.DOLocalMoveY(findexList[i], fTime);
            yield return new WaitForSeconds(fTime);
        }

        if (nGameCount < 2) //前2次
        {
            List<Vector3> getCirPosList = Common.GetCircleOrderPosList(10, 90f);

            GameObject mGoLeft0 = UguiMaker.newGameObject("mGoLeft0", mObjRes.transform);
            mGoLeft0.transform.localPosition = new Vector3(-120f, -150f + 800f, 0f);

            for (int i = 0; i < 10; i++)
            {
                GameObject mgo = UguiMaker.newGameObject("ball", mGoLeft0.transform);
                mgo.transform.localPosition = new Vector3(0f, 800f, 0f);
                KOAT_BallObj ballCtrl = mgo.AddComponent<KOAT_BallObj>();
                ballCtrl.InitAwake(nType);
                ballCtrl.transform.localPosition = getCirPosList[i];
            }
            mGoLeft0.transform.DOLocalMoveY(-150f, fTime);

            if (nLeft == 2) // >=20
            {
                yield return new WaitForSeconds(fTime);

                GameObject mGoLeft1 = UguiMaker.newGameObject("mGoLeft1", mObjRes.transform);
                mGoLeft1.transform.localPosition = new Vector3(-120f, 100f + 800f, 0f);

                for (int i = 0; i < 10; i++)
                {
                    GameObject mgo = UguiMaker.newGameObject("ball", mGoLeft1.transform);
                    mgo.transform.localPosition = new Vector3(0f, 800f, 0f);
                    KOAT_BallObj ballCtrl = mgo.AddComponent<KOAT_BallObj>();
                    ballCtrl.InitAwake(nType);
                    ballCtrl.transform.localPosition = getCirPosList[i];
                }
                mGoLeft1.transform.DOLocalMoveY(100f, fTime);
            }
        }
        else //后两次
        {
            GameObject mForRight = UguiMaker.newGameObject("mForRight", mObjRes.transform);
            mForRight.transform.localPosition = new Vector3(-120f, 0f, 0f);

            for (int i = 0; i < nLeft; i++)
            {
                GameObject mgo = UguiMaker.newGameObject("ball", mForRight.transform);
                mgo.transform.localPosition = new Vector3(0f, 800f, 0f);
                KOAT_BallObj ballCtrl = mgo.AddComponent<KOAT_BallObj>();
                ballCtrl.InitAwake(nType);
                ballCtrl.transform.DOLocalMoveY(findexList[i], fTime);
                yield return new WaitForSeconds(fTime);
            }
        }

        float fInTime = 0.5f;

        btnUp0.transform.DOLocalMove(vUp0, fInTime);
        btnDown0.transform.DOLocalMove(vDown0, fInTime);
        btnCheck.transform.DOLocalMove(vCheck, fInTime);
        btnUp1.transform.DOLocalMove(vUp1, fInTime);
        btnDown1.transform.DOLocalMove(vDown1, fInTime);
        mCtrl.mRectBox.SceneMove(true, fInTime);
        yield return new WaitForSeconds(fInTime);

        bCanClick = true;
        if (nGameCount <= 0)
        {
            mCtrl.PlayTipSound();
        }
    }

    private void ClickUp(GameObject _go)
    {
        if (!bCanClick)
            return;
        _go.transform.DOScale(Vector3.one, 0.15f);
        if (_go == btnUp0.gameObject)
        {
            mmRight++;
            mmRight = Mathf.Clamp(mmRight, 0, 9);
            mCtrl.mRectBox.SetRightNum(mmRight);
        }
        else if (_go == btnDown0.gameObject)
        {
            mmRight--;
            mmRight = Mathf.Clamp(mmRight, 0, 9);
            mCtrl.mRectBox.SetRightNum(mmRight);
        }
        else if (_go == btnUp1.gameObject)
        {
            mmLeft++;
            mmLeft = Mathf.Clamp(mmLeft, 0, 9);
            mCtrl.mRectBox.SetLeftNum(mmLeft);
        }
        else if (_go == btnDown1.gameObject)
        {
            mmLeft--;
            mmLeft = Mathf.Clamp(mmLeft, 0, 9);
            mCtrl.mRectBox.SetLeftNum(mmLeft);
        }
        else if (_go == btnCheck.gameObject)
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
                AudioClip cp0 = ResManager.GetClip("knowoneandten_sound", "这些数字不对哦你再仔细看看");
                mCtrl.SoundCtrl.PlaySound(cp0, 1f);
            }
        }
    }
    private void ClickDown(GameObject _go)
    {
        if (!bCanClick)
            return;
        if (_go == btnUp0.gameObject)
        {
            _go.transform.DOScale(Vector3.one * 1.2f, 0.15f);
        }
        else if (_go == btnDown0.gameObject)
        {
            _go.transform.DOScale(Vector3.one * 1.2f, 0.15f);
        }
        else if (_go == btnUp1.gameObject)
        {
            _go.transform.DOScale(Vector3.one * 1.2f, 0.15f);
        }
        else if (_go == btnDown1.gameObject)
        {
            _go.transform.DOScale(Vector3.one * 1.2f, 0.15f);
        }
        else if (_go == btnCheck.gameObject)
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
        if (nGameCount >= 4)
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
        AudioClip cp0 = ResManager.GetClip("knowoneandten_sound", "仔细观察下面的小怪物点击小三角形");
        mCtrl.SoundCtrl.PlaySound(cp0, 1f);
    }


}
