using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class KnowCubeLv2 : MonoBehaviour
{

    public class SqObjData
    {
        public int nType = 0;
        public int nID = 0;
        public SqObjData(int _type, int _id)
        {
            nType = _type;
            nID = _id;
        }
    }

    private Vector3 vStartPos = new Vector3(0f, -35f, 0f);
    private RawImage bg;

    private Image checkbtn;
    private FaileShakeCtrl checkbtnshake;

    private Transform mpos;
    private List<Transform> vposList = new List<Transform>();

    //正方体
    private List<SqObjData> mSqobjDataList1 = new List<SqObjData>();
    //其他几何体
    private List<SqObjData> mSqobjDataList2 = new List<SqObjData>();
    private List<KnowCubeTheObj> mSqobjList = new List<KnowCubeTheObj>();

    public int nCount = 0;
    public int nToCount = 0;

    //private ParticleSystem particleSystem;
    bool bthispass = false;


    private ParticleSystem mCheckOKFX;


    KnowCubeCtrl mCtrl;
    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as KnowCubeCtrl;

        transform.localPosition = vStartPos;
        bg = transform.Find("map1").GetComponent<RawImage>();
        bg.texture = ResManager.GetTexture("knowcube_texture", "map2");
        bg.rectTransform.sizeDelta = new Vector2(1339f, 721f);

        mpos = transform.Find("mpos");
        for (int i = 0; i < 19; i++)
        {
            Transform tra = mpos.Find("pos" + i);
            vposList.Add(tra);
        }

        checkbtn = transform.Find("checkbtn").GetComponent<Image>();
        checkbtn.sprite = ResManager.GetSprite("public", "checkup");
        checkbtn.SetNativeSize();
        EventTriggerListener.Get(checkbtn.gameObject).onUp = CheckIsFinish;
        EventTriggerListener.Get(checkbtn.gameObject).onDown = CheckIsDown;
        checkbtnshake = checkbtn.gameObject.AddComponent<FaileShakeCtrl>();
        checkbtnshake.InitAwake();

        mCheckOKFX = ResManager.GetPrefab("knowcube_prefab", "cubefx").GetComponent<ParticleSystem>();
        mCheckOKFX.transform.SetParent(checkbtn.transform);
        mCheckOKFX.transform.localPosition = Vector3.zero;
        mCheckOKFX.transform.localScale = Vector3.one;
        mCheckOKFX.Pause();
        mCheckOKFX.Stop();
    }

    public void GetDatas()
    {
        mSqobjDataList1.Clear();
        mSqobjDataList1.Add(new SqObjData(1, 1));
        mSqobjDataList1.Add(new SqObjData(1, 2));
        mSqobjDataList1.Add(new SqObjData(1, 3));
        mSqobjDataList1.Add(new SqObjData(1, 7));
        mSqobjDataList1.Add(new SqObjData(1, 8));

        mSqobjDataList2.Clear();
        mSqobjDataList2.Add(new SqObjData(2, 1));
        mSqobjDataList2.Add(new SqObjData(2, 2));
        mSqobjDataList2.Add(new SqObjData(2, 3));
        mSqobjDataList2.Add(new SqObjData(2, 4));
        mSqobjDataList2.Add(new SqObjData(2, 5));

        mSqobjDataList2.Add(new SqObjData(3, 1));
        mSqobjDataList2.Add(new SqObjData(3, 2));
        mSqobjDataList2.Add(new SqObjData(3, 3));
        mSqobjDataList2.Add(new SqObjData(3, 4));
        mSqobjDataList2.Add(new SqObjData(3, 5));
        mSqobjDataList2.Add(new SqObjData(3, 6));

        mSqobjDataList2.Add(new SqObjData(4, 1));
        mSqobjDataList2.Add(new SqObjData(4, 2));

        mSqobjDataList2.Add(new SqObjData(5, 1));
        mSqobjDataList2.Add(new SqObjData(5, 2));
        mSqobjDataList2.Add(new SqObjData(5, 3));
        mSqobjDataList2.Add(new SqObjData(5, 4));
        mSqobjDataList2.Add(new SqObjData(5, 5));

        mSqobjDataList2.Add(new SqObjData(6, 1));
        mSqobjDataList2.Add(new SqObjData(6, 2));
        mSqobjDataList2.Add(new SqObjData(6, 3));
        //mSqobjDataList2.Add(new SqObjData(6, 4));
        mSqobjDataList2.Add(new SqObjData(6, 5));

        mSqobjDataList1 = Common.BreakRank(mSqobjDataList1);
        mSqobjDataList2 = Common.BreakRank(mSqobjDataList2);
    }


    public void ResetInfos()
    {
        for (int i = 0; i < mSqobjList.Count; i++)
        {
            if (mSqobjList[i].gameObject != null)
            {
                GameObject.Destroy(mSqobjList[i].gameObject);
            }
        }
        mSqobjList.Clear();
    }

    public void SetData()
    {
        this.StopAllCoroutines();
        ResetInfos();
        GetDatas();
        ToSetTimes();
        SceneMove(true);
    }
    private void ToSetTimes()
    {
        checkbtn.sprite = ResManager.GetSprite("public", "checkup");
        //clear
        for (int i = 0; i < mSqobjList.Count; i++)
        {
            if (mSqobjList[i].gameObject != null)
            {
                GameObject.Destroy(mSqobjList[i].gameObject);
            }
        }
        mSqobjList.Clear();
        nCount = 0;
        nToCount = 5;
        bthispass = false;
        //create正方形
        for (int i = 0; i < mSqobjDataList1.Count; i++)
        {
            KnowCubeTheObj sqObj0 = CreateSqObj(mSqobjDataList1[i]);
            mSqobjList.Add(sqObj0);
        }
        //create其他 8-10
        int otherCount = 10;//UnityEngine.Random.Range(8, 11);
        for (int i = 0; i < otherCount; i++)
        {
            KnowCubeTheObj sqObj1 = CreateSqObj(mSqobjDataList2[i]);
            mSqobjList.Add(sqObj1);
        }

        //位置
        //mSqobjList = Common.BreakRank(mSqobjList);
        vposList = Common.BreakRank(vposList);
        for (int i = 0; i < mSqobjList.Count; i++)
        {
            mSqobjList[i].transform.localPosition = vposList[i].localPosition;
            mSqobjList[i].transform.localScale = Vector3.one * 0.001f;
        }
        StartCoroutine(IEStartData());
    }
    IEnumerator IEStartData()
    {
        yield return new WaitForSeconds(1.1f);
        mCtrl.PlayTipSound();
        //Debug.Log("找找看吧,那些是正方体?");
        for (int i = 0; i < mSqobjList.Count; i++)
        {
            mSqobjList[i].transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            mCtrl.PlayTheSortSound("cubeshowout");
            yield return new WaitForSeconds(0.3f);
        }
    }




    /// <summary>
    /// 创建一个几何体
    /// </summary>
    public KnowCubeTheObj CreateSqObj(SqObjData _sqdata)
    {
        GameObject mgo = UguiMaker.newGameObject("sqobj" + _sqdata.nType + "_" + _sqdata.nID, transform);
        mgo.transform.localScale = Vector3.one;
        KnowCubeTheObj sqobjctrl = mgo.AddComponent<KnowCubeTheObj>();
        sqobjctrl.InitAwake(this, _sqdata.nType, _sqdata.nID);
        return sqobjctrl;
    }

    //点击几何体
    public void ClickObjUp(GameObject _go)
    {
        if (bthispass)
            return;

        _go.transform.DOScale(Vector3.one * 1.2f, 0.13f).OnComplete(()=> 
        {
            _go.transform.DOScale(Vector3.one, 0.13f);
        });

        mCtrl.PlayTheSortSound("clickblock");

        KnowCubeTheObj sqobjctrl = _go.GetComponent<KnowCubeTheObj>();
        if (sqobjctrl != null)
        {
            if (!sqobjctrl.bHit)
            {
                sqobjctrl.bHit = true;
                sqobjctrl.ShowRang();
                if (sqobjctrl.nType == 1)
                {
                    nCount++;
                }
            }
            else
            {
                sqobjctrl.bHit = false;
                sqobjctrl.HideRang();
                if (sqobjctrl.nType == 1)
                {
                    nCount--;
                }
            }
        }
    }

    private void CheckIsDown(GameObject _go)
    {
        if (bthispass)
        { return; }
        checkbtn.sprite = ResManager.GetSprite("public", "checkdown");

        AudioClip cp = ResManager.GetClip("checkgamebtn_sound", "checkgamebtn_down");
        if (cp != null)
            AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position);
    }
    //确认点击
    private void CheckIsFinish(GameObject _go)
    {
        if (bthispass)
        { return; }

        AudioClip cp = ResManager.GetClip("checkgamebtn_sound", "checkgamebtn_up");
        if (cp != null)
            AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position);

        if (CheckIsAllSqu() && nCount >= nToCount && nToCount > 0)
        {
            bthispass = true;
            mCheckOKFX.Play();
            StartCoroutine(IEToNext());
            mCtrl.PlayTheSortSound("show_reward0");
        }
        else
        {
            for (int i = 0; i < mSqobjList.Count; i++)
            {
                mSqobjList[i].HideRang();
            }
            nCount = 0;

            checkbtnshake.ShakeObj(()=> 
            {
                checkbtn.sprite = ResManager.GetSprite("public", "checkup");
            });

            string cp0name = "game-tips4-1-16";
            if (Random.value > 0.5f)
            {
                cp0name = "game-tips4-1-18";
            }
            AudioClip cp0 = mCtrl.GetClip(cp0name);
            mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp0, 1f);
        }
    }

    IEnumerator IEToNext()
    {
        yield return new WaitForSeconds(0.5f);
        //特效播放
        for (int i = 0; i < mSqobjList.Count; i++)
        {
            if (mSqobjList[i].nType == 1)
            {
                PlayFX(mSqobjList[i].transform.position);
                yield return new WaitForSeconds(0.25f);
            }
        }
        yield return new WaitForSeconds(1.5f);

        string cp0name = "game-tips4-1-15";
        if (Random.value > 0.5f)
        {
            cp0name = "game-tips4-1-17";
        }
        AudioClip cp0 = mCtrl.GetClip(cp0name);
        mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp0, 1f);
        yield return new WaitForSeconds(cp0.length);
  
        KnowCubeCtrl mctrl = SceneMgr.Instance.GetNowScene() as KnowCubeCtrl;
        mctrl.LevelPass();
    }


    //检测选中的obj是否全为squ
    bool CheckIsAllSqu()
    {
        bool reto = true;
        for (int i = 0; i < mSqobjList.Count; i++)
        {
            if (mSqobjList[i].bHit && mSqobjList[i].nType != 1)
            {
                return false;
            }
        }
        return reto;
    }


    public void SceneMove(bool _in)
    {
        if (_in)
        {
            transform.localPosition = new Vector3(0f, -800f, 0f);
            transform.DOLocalMove(vStartPos, 1f);
        }
        else
        {
            transform.DOLocalMove(new Vector3(0f, -800f, 0f), 1f);
        }
    }

    /// <summary>
    /// 播放特效
    /// </summary>
    /// <param name="vpos"></param>
    private void PlayFX(Vector3 worldPos)
    {
        //particleSystem.transform.position = worldPos;
        //particleSystem.Play();
    }


    //开始提示2
    public IEnumerator IEPlayTipSound2()
    {
        AudioClip cp0 = mCtrl.GetClip("zadditional-3");
        mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp0, 1f);
        yield return 1f;
    }

}

/// <summary>
/// 几何体
/// </summary>
public class KnowCubeTheObj : MonoBehaviour
{
    /// <summary>
    /// 类型
    /// </summary>
    public int nType = 0;
    /// <summary>
    /// id
    /// </summary>
    public int nID = 0;

    public bool bHit = false;

    private Image img;
    private Button btn;

    private Image imgRang;

    public void InitAwake(KnowCubeLv2 _lv2Ctrl,int _type,int _id)
    {
        nType = _type;
        nID = _id;
        //Debug.Log(nType + "_" + nID);
        img = gameObject.AddComponent<Image>();
        img.sprite = ResManager.GetSprite("knowcube_sprite", "spobj" + nType + "_" + nID);
        img.SetNativeSize();
        btn = gameObject.AddComponent<Button>();
        EventTriggerListener.Get(gameObject).onUp = _lv2Ctrl.ClickObjUp;

        imgRang = UguiMaker.newImage("rang", transform, "knowcube_sprite", "yuanquan", false);
        imgRang.rectTransform.anchoredPosition = Vector2.zero;
        imgRang.rectTransform.localScale = Vector3.one;
        imgRang.rectTransform.sizeDelta = new Vector2(140f, 140f);
        imgRang.enabled = false;
    }

    public void ShowRang()
    {
        imgRang.enabled = true;
    }
    public void HideRang()
    {
        imgRang.enabled = false;
        bHit = false;
    }

}
