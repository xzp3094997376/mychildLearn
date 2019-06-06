using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class AnimalRotateLevel1 : MonoBehaviour
{
    private AnimalRotateCtrl mCtrl;
    //private bool bInit = false;

    public List<AnimalRotateStation> mStationList = new List<AnimalRotateStation>();


    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as AnimalRotateCtrl;
        //bInit = true;
    }

    /// <summary>
    /// reset infos
    /// </summary>
    public void ResetInfos()
    {
        for (int i=0;i<mStationList.Count;i++)
        {
            if (mStationList[i].gameObject != null)
                GameObject.Destroy(mStationList[i].gameObject);
        }
        mStationList.Clear();
    }

    private float fstationScale = 1.8f;
    private float fstationHeight = -50f;
    public void SetData()
    {
        ResetInfos();

        //动物头像id
        List<int> cellList0 = Common.GetIDList(1, 14, 4, -1);
        //station id
        List<int> typeList0 = Common.GetIDList(0, 3, 2, -1);
        for (int i = 0; i < typeList0.Count; i++)
        {
            AnimalRotateStation mst = mCtrl.CreateAnimalRotateStation(typeList0[0], cellList0.ToArray(), transform);
            mStationList.Add(mst);
        }
        //位置
        mStationList[0].transform.localPosition = new Vector3(-260f, fstationHeight, 0f);
        mStationList[1].transform.localPosition = new Vector3(260f, fstationHeight, 0f);
        mStationList[0].transform.localScale = Vector3.one * fstationScale;
        mStationList[1].transform.localScale = Vector3.one * fstationScale;
        mStationList[0].SetRememberPos(mStationList[0].transform.localPosition);
        mStationList[1].SetRememberPos(mStationList[1].transform.localPosition);

        mStationList[0].ButtonEnbal(false);
        for (int i = 0; i < 4; i++)
        {
            mStationList[0].mCells[i].ButtonEnbal(false);
        }

        //set station rotate
        int rotateid = Random.Range(1, AnimalRotateDefine.nRotateMaxType + 1);
        mStationList[1].SetRotateID(rotateid);
        mStationList[1].SetClickCallBack(CheckCallBack);
        //set cells rotate
        //List<int> cellsRotateIds = Common.GetIDList(1, 7, 4, -1);
        for (int i = 0; i < 4; i++)
        {
            int cellrotateid = Random.Range(1, AnimalRotateDefine.nRotateMaxType + 1);//cellsRotateIds[i];
            mStationList[1].SetCellRotateID(i, cellrotateid);
            mStationList[1].mCells[i].SetClickCallBack(CheckCallBack);
        }
    }

    /// <summary>
    /// 点击回调
    /// </summary>
    public void CheckCallBack()
    {
        bool ispass = false;
        ispass = mCtrl.CheckStationRotationIsSame(mStationList[0], mStationList[1]);
        if (ispass)
        {
            mStationList[1].PassReset();
            mStationList[1].ButtonEnbal(false);
            for (int i = 0; i < 4; i++)
            {
                mStationList[1].mCells[i].ButtonEnbal(false);
            }

            mCtrl.StartCoroutine(IEPassLv1());
        }
        else
        { }     
    }
    IEnumerator IEPassLv1()
    {
        yield return new WaitForSeconds(0.3f);
        //Debug.Log("level 1 pass");
        //mCtrl.KBabyShow();
        //mCtrl.bPlayOtherTip = true;
        AudioClip cp0 = mCtrl.mSoundCtrl.GetClip("animalrotate_sound", "game-right6-1-" + Random.Range(1, 3));
        mCtrl.mSoundCtrl.PlaySound(mCtrl.mSoundCtrl.mKimiAudioSource, cp0, 1f);
        yield return new WaitForSeconds(cp0.length);
        //mCtrl.KBabyHide();
        yield return new WaitForSeconds(0.3f);
        mCtrl.LevelCheckNext();
    }


    /// <summary>
    /// scene move
    /// </summary>
    /// <param name="_in"></param>
    public void SceneMove(bool _in)
    {
        if (_in)
        {
            transform.localPosition = new Vector3(-1200f, 0f, 0f);
            transform.DOLocalMove(Vector3.zero, 1f);
        }
        else
        {
            transform.DOLocalMove(new Vector3(1200f,0f,0f), 1f);
        }
    }

    /// <summary>
    /// 提示语音
    /// </summary>
    /// <returns></returns>
    public IEnumerator iePlayTipSoundLv1()
    {
        yield return 1;
        AudioClip cp0 = mCtrl.mSoundCtrl.GetClip("animalrotate_sound", "game-tips6-1-1");
        mCtrl.mSoundCtrl.PlaySound(mCtrl.mSoundCtrl.mKimiAudioSource, cp0, 1f);
    }



}
