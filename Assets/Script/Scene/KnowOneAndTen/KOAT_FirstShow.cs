using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class KOAT_FirstShow : KOAT_SceneBase
{

    private System.Action mFinishCall = null;//完成演示回调
    private List<KOAT_BallObj> mBallList = new List<KOAT_BallObj>();

    float fRotateSpeed = 0f;

    public int nType = 1;

    public override void ResetInfos()
    {
        base.ResetInfos();
        mBallList.Clear();
    }

    public override void InitData()
    {
        base.InitData();
        nType = Random.Range(0, 3);
        mCtrl.StartCoroutine(ieSetData());
    }
    IEnumerator ieSetData()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject mBalls0 = UguiMaker.newGameObject("mBalls0", transform);
        mBalls0.transform.localPosition = new Vector3(125f, 0f, 0f);

        List<float> findexList = Common.GetOrderList(10, 55f);
        int nCount = 0;
        float fTime = 0.4f;
        for (int i = 0; i < 10; i++)
        {
            GameObject mgo = UguiMaker.newGameObject("ball", mBalls0.transform);
            mgo.transform.localPosition = new Vector3(0f, 800f, 0f);
            KOAT_BallObj ballCtrl = mgo.AddComponent<KOAT_BallObj>();
            ballCtrl.InitAwake(nType);
            ballCtrl.transform.DOLocalMoveY(findexList[i], 0.3f);
            nCount++;
            mBallList.Add(ballCtrl);
            yield return new WaitForSeconds(fTime);
            mCtrl.mRectBox.SetRightNum(nCount);
        }

        mCtrl.mRectBox.RightNumFlash(true);
        yield return new WaitForSeconds(1f);

        //围成圆
        List<Vector3> getCirPosList = Common.GetCircleOrderPosList(10, 90f);
        for (int i = 0; i < 10; i++)
        {
            mBallList[i].transform.DOLocalMove(getCirPosList[i], 5f);
        }
        mBalls0.transform.DOLocalMove(new Vector3(-120f, -150f, 0f), 5f);

        fRotateSpeed = 0f;
        DOTween.To(() => fRotateSpeed, x => fRotateSpeed = x, 50f, 3f).OnUpdate(()=> 
        {
            mBalls0.transform.localEulerAngles += new Vector3(0f, 0f, fRotateSpeed * Time.deltaTime * 10f);
        });
        yield return new WaitForSeconds(3f);
        DOTween.To(() => fRotateSpeed, x => fRotateSpeed = x, 0f, 3f).OnUpdate(() =>
        {
            mBalls0.transform.localEulerAngles += new Vector3(0f, 0f, fRotateSpeed * Time.deltaTime * 10f);
        });
        yield return new WaitForSeconds(3f);

        mCtrl.mRectBox.RightNumFlash(false);
        mCtrl.mRectBox.SetLeftNum(1);
        mCtrl.mRectBox.SetRightNum(0);

        AudioClip cp0 = ResManager.GetClip("knowoneandten_sound", "十位上的数字是1表示数量10");
        mCtrl.SoundCtrl.PlaySound(cp0, 1f);
        yield return new WaitForSeconds(cp0.length);

        //------

        GameObject mBalls1 = UguiMaker.newGameObject("mBalls1", transform);
        mBalls1.transform.localPosition = new Vector3(125f, 0f, 0f);

        for (int i = 0; i < 8; i++)
        {
            GameObject mgo = UguiMaker.newGameObject("ball", mBalls1.transform);
            mgo.transform.localPosition = new Vector3(0f, 600f, 0f);
            KOAT_BallObj ballCtrl = mgo.AddComponent<KOAT_BallObj>();
            ballCtrl.InitAwake(nType);
            ballCtrl.transform.DOLocalMoveY(findexList[i], 0.3f);
            mBallList.Add(ballCtrl);
            //if (i == 0)
            //{
            //    AudioClip cp1 = ResManager.GetClip("knowoneandten_sound", "十位上的数字是1表示数量10");
            //    mCtrl.SoundCtrl.PlaySound(cp1, 1f);
            //    yield return new WaitForSeconds(cp1.length);
            //}
            //else
            //{
            //    yield return new WaitForSeconds(0.4f);
            //}
            yield return new WaitForSeconds(0.3f);
            mCtrl.mRectBox.SetRightNum(i + 1);
        }

        AudioClip cp2 = ResManager.GetClip("knowoneandten_sound", "现在个位上的数字是8表示数量8");
        mCtrl.SoundCtrl.PlaySound(cp2, 1f);
        yield return new WaitForSeconds(cp2.length + 0.3f);

        //底色一起闪动
        mCtrl.mRectBox.NumBGFlash(true);
        AudioClip cp3 = ResManager.GetClip("knowoneandten_sound", "十位的10加上个位的8等于18");
        mCtrl.SoundCtrl.PlaySound(cp3, 1f);
        yield return new WaitForSeconds(cp3.length + 0.5f);

        mCtrl.mRectBox.NumBGFlash(false);

        //元素消失
        mBalls0.transform.DOLocalMoveX(mBalls0.transform.localPosition.x - 1000f, 0.4f);
        mBalls1.transform.DOLocalMoveX(mBalls1.transform.localPosition.x - 1000f, 0.4f);
        mCtrl.mRectBox.SceneMove(false, 0.4f);
        yield return new WaitForSeconds(0.42f);

        if (mFinishCall != null)
        {
            mFinishCall();
        }

    }

    /// <summary>
    /// 设置演示完成回调
    /// </summary>
    /// <param name="_call"></param>
    public void SetFinishCall(System.Action _call)
    {
        mFinishCall = _call;
    }



}
