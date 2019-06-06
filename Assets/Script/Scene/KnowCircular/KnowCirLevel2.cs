using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class KnowCirLevel2 : MonoBehaviour
{
    public class KCirObjData
    {
        public int nType = 0;
        public int nIndex = 0;
        public KCirObjData(int _type, int _index)
        { nType = _type;nIndex = _index; }
    }

    KnowCircularCtrl mCtrl;

    public int nGameTimes = 0;//累积次数
    public int nToCount = 0;//达成拖对目标
    public int nCount = 0;//拖对次数

    private Image z_buzi;
    private Image z_zhangfeng;
    private GameObject mLanzi;
    private Image z_lanzidown;
    private GameObject mStationTrans;
    private Image z_lanziup;
    private BoxCollider2D mBox2D;

    public bool bCanDrop = false;
    public List<KCirObjLv2> mDropObjList = new List<KCirObjLv2>();//dropObj
    private List<KCirObjData> mDataList = new List<KCirObjData>();

    float fMinX = -370f;
    float fMaxX = 300f;
    float fMinY = -290f;
    float fMaxY = -185f;

    public List<int> yuanObjList = new List<int>();
    public List<int> otherObjList = new List<int>();

    public void InitAwake(KnowCircularCtrl _mctrl)
    {
        mCtrl = _mctrl;
        InitPosss();

        yuanObjList = Common.GetIDList(0, 5, 6, -1);
        otherObjList = Common.GetIDList(3, 11, 9, -1);

        z_buzi = UguiMaker.newImage("z_buzi", transform, "knowcircular_sprite", "z_buzi", false);
        z_buzi.transform.localPosition = new Vector3(-24f, -293f, 0f);

        z_zhangfeng = UguiMaker.newImage("z_zhangfeng", transform, "knowcircular_sprite", "z_zhangfeng", false);
        z_zhangfeng.transform.localPosition = new Vector3(-545f, -237f, 0f);

        mLanzi = UguiMaker.newGameObject("mLanzi", transform);
        mBox2D = mLanzi.AddComponent<BoxCollider2D>();
        mBox2D.size = new Vector2(220f, 220f);
        mBox2D.offset = new Vector2(0f, -26f);
        mLanzi.transform.localPosition = new Vector3(478f, -200f, 0f);
        z_lanziup = UguiMaker.newImage("z_lanziup", mLanzi.transform, "knowcircular_sprite", "z_lanziup", false);
        mStationTrans = UguiMaker.newGameObject("stationTrans", mLanzi.transform);
        z_lanzidown = UguiMaker.newImage("z_lanzidown", mLanzi.transform, "knowcircular_sprite", "z_lanzidown", false);

        SetDate();
    }

    public void ResetInfos()
    {
        nToCount = 0;
        nCount = 0;
        for (int i = 0; i < mDropObjList.Count; i++)
        {
            if (mDropObjList[i].gameObject != null)
                GameObject.Destroy(mDropObjList[i].gameObject);
        }
        mDropObjList.Clear();
        mDataList.Clear();
        bCanDrop = false;
    }

    public void SetDate()
    {
        ResetInfos();
        StartCoroutine(ieSetDate());
    }
    IEnumerator ieSetDate()
    {
        
        nToCount = 3;
        if (nGameTimes == 0)
        {
            //3个圆物品
            for (int i = 0; i < 3; i++)
            {
                int nindex = yuanObjList[i];
                mDataList.Add(new KCirObjData(0, nindex));
            }
            //4个其它物品
            for (int i = 0; i < 4; i++)
            {
                int nobjid = otherObjList[i];
                mDataList.Add(new KCirObjData(nobjid, 0));
            }
        }
        else
        {   
            //3个圆物品
            for (int i = 0; i < nToCount; i++)
            {
                int nindex = yuanObjList[i + 3];
                mDataList.Add(new KCirObjData(0, nindex));
            }
            //5个其它物品
            for (int i = 0; i < 5; i++)
            {
                int nobjid = otherObjList[i + 4];
                mDataList.Add(new KCirObjData(nobjid, 0));
            }
        }
               
        mDataList = Common.BreakRank(mDataList);
        mPosss = Common.BreakRank(mPosss);

        for (int i = 0; i < mDataList.Count; i++)
        {
            KCirObjLv2 spCtrl = UguiMaker.newGameObject("mObj" + mDataList[i].nType, transform).AddComponent<KCirObjLv2>();
            spCtrl.nType = mDataList[i].nType;
            spCtrl.nIndex = mDataList[i].nIndex;
            spCtrl.InitAwake();
            spCtrl.SetStartPos(mPosss[i]);
            spCtrl.transform.localPosition = mPosss[i];
            spCtrl.transform.localScale = Vector3.one * 0.001f;
            mDropObjList.Add(spCtrl);
            spCtrl.DoScale(Vector3.one, 0.2f);
            mCtrl.PlaySetBigSound() ;
            yield return new WaitForSeconds(0.2f);
        }

        OrderSetting();

        yield return new WaitForSeconds(0.1f);

        ShowGuideDrop();

        bCanDrop = true;
        mCtrl.PlayTipSound();
    }

    KCirObjLv2 mSelect;
    Vector3 temp_select_offset = Vector3.zero;
    private void Update()
    {
        if (!bCanDrop)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            #region//one
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            for (int i = 0; i < hits.Length; i++)
            {
                KCirObjLv2 com = hits[i].collider.gameObject.GetComponent<KCirObjLv2>();
                if (com != null)
                {
                    if (mSelect == null)
                    { mSelect = com; }
                    else if (com.transform.GetSiblingIndex() > mSelect.transform.GetSiblingIndex())
                    { mSelect = com; }
                }
            }
            if (mSelect != null)
            {
                mCtrl.GuideStop();
                mSelect.DropSet();
                RectTransform retf = mSelect.transform as RectTransform;
                temp_select_offset = Common.getMouseLocalPos(transform) - retf.anchoredPosition3D;
                mSelect.transform.SetSiblingIndex(50);
                mSelect.SetRemindPos();
                mSelect.YingActive(false);
                mCtrl.PlaySetBigSound();
            }
            #endregion
        }
        else if (Input.GetMouseButton(0))
        {
            #region//动物位置设置
            if (mSelect != null)
            {
                //拖动值限制
                Vector3 vInput = Common.getMouseLocalPos(transform) - temp_select_offset;
                float fX = vInput.x;
                float fY = vInput.y;
                Vector3 vsize = mSelect.GetBoxSize();
                fX = Mathf.Clamp(fX, -GlobalParam.screen_width * 0.5f + vsize.x * 0.5f, GlobalParam.screen_width * 0.5f - vsize.x * 0.5f);
                fY = Mathf.Clamp(fY, -GlobalParam.screen_height * 0.5f + vsize.y * 0.5f, GlobalParam.screen_height * 0.5f - vsize.y * 0.5f - 60f);
                mSelect.transform.localPosition = new Vector3(fX, fY, 0f);
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//...
            if (mSelect != null)
            {
                bool bMatch = false;
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                for (int i = 0; i < hits.Length; i++)
                {
                    GameObject hitObj = hits[i].collider.gameObject;
                    if (hitObj == mLanzi)
                    {
                        if (mSelect.nType == 0)
                        {
                            bMatch = true;
                            AddToLanzi(mSelect);
                            CheckLevelPass();
                            mCtrl.PlayObjSound(mSelect.nType);
                            mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "setok"));
                        }
                        else
                        {
                            mSelect.BackToRemindPos();
                            mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "setwrong"));
                        }
                        break;
                    }
                }

                if (!bMatch)
                {
                    mSelect.DropReset(() => { OrderSetting(); });
                    if (mSelect.transform.localPosition.y <= -130f)
                        mSelect.YingActive(true);
                }
            }
            mSelect = null;
            #endregion
        }
    }

    private void AddToLanzi(KCirObjLv2 _obj)
    {
        _obj.transform.SetParent(mLanzi.transform);
        _obj.DoScale(Vector3.one * 0.8f, 0.35f);
        _obj.Box2DActive(false);

        float fsetX = _obj.transform.localPosition.x;
        fsetX = Mathf.Clamp(fsetX, -42f, 42f);
        float fsetY = UnityEngine.Random.Range(-64f, -32f);
        _obj.transform.DOLocalMove(new Vector3(fsetX, mStationTrans.transform.localPosition.y + fsetY + 30f, 0f), 0.2f).OnComplete(() =>
        {
            _obj.transform.SetParent(mStationTrans.transform);
            _obj.transform.DOLocalMove(new Vector3(fsetX, fsetY, 0f), 0.2f);
        });
    }

    private void CheckLevelPass()
    {
        nCount++;
        if (nCount >= nToCount && nToCount > 0)
        {
            bCanDrop = false;
            nGameTimes++;
            mCtrl.StartCoroutine(ieCheckLevelPass());
        }
    }
    IEnumerator ieCheckLevelPass()
    {
        yield return new WaitForSeconds(1f);
        mCtrl.PlaySucSound();
        yield return new WaitForSeconds(3f);
        if (nGameTimes < 2)
        {
            //元素消失
            for (int i = 0; i < mDropObjList.Count; i++)
            {
                mDropObjList[i].DoScale(Vector3.one * 0.001f, 0.3f);
            }
            yield return new WaitForSeconds(1f);
            //replay
            SetDate();
        }
        else
        {
            //yield return new WaitForSeconds(1f);
            mCtrl.LevelCheckNext();
        }
    }

    /// <summary>
    /// 层设置
    /// </summary>
    public void OrderSetting()
    {
        int index = 0;
        for (int i = 0; i < mDropObjList.Count - 1; ++i)
        {
            if (!mDropObjList[i].bInStation)
            {
                index = i;
                for (int j = i + 1; j < mDropObjList.Count; ++j)
                {
                    if (mDropObjList[j].transform.position.y > mDropObjList[index].transform.position.y)
                        index = j;
                }
                KCirObjLv2 t = mDropObjList[index];
                mDropObjList[index] = mDropObjList[i];
                mDropObjList[i] = t;
                mDropObjList[i].transform.SetSiblingIndex(40);
            }
        }
        mDropObjList[mDropObjList.Count - 1].transform.SetSiblingIndex(40);
    }

    List<Vector3> mPosss = new List<Vector3>();
    public void InitPosss()
    {
        mPosss.Add(new Vector3(-394f, -300f, 0f));
        mPosss.Add(new Vector3(-253, -307, 0f));
        mPosss.Add(new Vector3(-105, -300f, 0f));
        mPosss.Add(new Vector3(23, -303f, 0f));
        mPosss.Add(new Vector3(168, -285, 0f));
        mPosss.Add(new Vector3(306, -305f, 0f));
        mPosss.Add(new Vector3(-296, -211, 0f));
        mPosss.Add(new Vector3(-179, -208, 0f));
        mPosss.Add(new Vector3(-52, -202, 0f));
        mPosss.Add(new Vector3(69, -217, 0f));
        mPosss.Add(new Vector3(191, -214, 0f));
        mPosss.Add(new Vector3(276, -214, 0f));
        mPosss.Add(new Vector3(88, -274, 0f));
        mPosss.Add(new Vector3(-183, -266, 0f));
        mPosss.Add(new Vector3(-54, -274, 0f));
        mPosss.Add(new Vector3(-316, -282, 0f));
    }

    bool bGuide = false;
    public void ShowGuideDrop()
    {
        if (bGuide)
            return;
        Vector3 vfrom = Vector3.zero;
        for (int i = 0; i < mDropObjList.Count; i++)
        {
            if (mDropObjList[i].nType == 0)
            {
                vfrom = mDropObjList[i].transform.position;
                break;
            }
        }
        Vector3 vto = mLanzi.transform.position;
        mCtrl.GuideShow(vfrom, vto);
        bGuide = true;
    }

}
