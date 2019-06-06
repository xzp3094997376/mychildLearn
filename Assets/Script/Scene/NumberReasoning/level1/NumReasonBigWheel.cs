using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 摩天轮
/// </summary>
public class NumReasonBigWheel : MonoBehaviour
{

    private bool bWheelRun = true;

    public float fSpeed = 5f;
    private Image img_jiazi;
    private Image img_wheel;
   

    private Transform[] mposs = new Transform[8];
    private NumReasonStation[] mStations = new NumReasonStation[8];

    private List<NumReasonStation> mStationList = new List<NumReasonStation>();

    public List<int> mLeftIDs = new List<int>();
    public List<int> mRightIDs = new List<int>();

    private bool bInit = false;

    AudioSource mAudioSource;

    public void InitAwake()
    {

        mAudioSource = gameObject.AddComponent<AudioSource>();
        mAudioSource.volume = 0.4f;
        mAudioSource.playOnAwake = false;
        mAudioSource.loop = true;
        mAudioSource.clip = ResManager.GetClip("numberreasoning_sound", "摩天轮转动");
        mAudioSource.Play();

        transform.localPosition = new Vector3(0f, -400f, 0f);
        transform.localScale = Vector3.one * 0.93f;

        img_jiazi = transform.Find("img_jiazi").GetComponent<Image>();
        img_wheel = transform.Find("img_wheel").GetComponent<Image>();

        img_jiazi.sprite = ResManager.GetSprite("numberreasoning_sprite", "mtl_jiazi");
        img_wheel.sprite = ResManager.GetSprite("numberreasoning_sprite", "mtl_quan");

        mLeftIDs = Common.GetIDList(1, 10, 8, -1);
        mRightIDs = Common.GetIDList(1, 10, 8, -1);

        for (int i = 0; i < mposs.Length; i++)
        {
            mposs[i] = img_wheel.transform.Find("mpos" + i);
            GameObject mgo = ResManager.GetPrefab("numberreasoning_prefab", "mstation", mposs[i]);
            mStations[i] = mgo.AddComponent<NumReasonStation>();
            mStations[i].InitAwake(mLeftIDs[i], mRightIDs[i]);

            mStationList.Add(mStations[i]);
        }

        bInit = true;
    }

    /// <summary>
    /// 设置空缺数字
    /// </summary>
    /// <param name="_count">个数</param>
    public void SetLostNumObj(int _count)
    {
        mStationList = Common.BreakRank(mStationList);
        for (int i = 0; i < mStationList.Count; i++)
        {
            if (i <= (_count - 1))
            {
                mStationList[i].SetTheLostNumObj();
            }
        }
    }

    /// <summary>
    /// 设置摩天轮转动/停止
    /// </summary>
    /// <param name="_run"></param>
    public void SetBigWheelRun(bool _run)
    {
        bWheelRun = _run;
        if (_run)
        {
            mAudioSource.Play();
        }
        else
        {
            mAudioSource.Stop();
        }
    }


    void Update ()
    {
        if (!bInit)
            return;

        if (bWheelRun)
            img_wheel.transform.localEulerAngles = img_wheel.transform.localEulerAngles + new Vector3(0f, 0f, Time.deltaTime * fSpeed);

	}
}
