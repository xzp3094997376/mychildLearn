using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class AnimalRotateLevel2 : MonoBehaviour
{

    private AnimalRotateCtrl mCtrl;
    private bool bInit = false;

    private GridLayoutGroup gridTop;
    private GridLayoutGroup gridDown;

    public int nTwice = 1;
    public int nCount = 0;
    public int nToCount = 0;

    public List<AnimalRotateDropLineST> mStationList = new List<AnimalRotateDropLineST>();
    public List<AnimalRotateDropPointST> mDropLinePointList = new List<AnimalRotateDropPointST>();
    public List<Image> mLineList = new List<Image>();  

    private Image mDrop;
    private Image mLine;

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as AnimalRotateCtrl;

        gridTop = UguiMaker.newGameObject("gridTop", transform).AddComponent<GridLayoutGroup>();
        gridTop.transform.localPosition = new Vector3(0f, 160f, 0f);
        gridTop.cellSize = new Vector2(300f, 300f);
        gridTop.spacing = new Vector2(100f, 110f);
        gridTop.childAlignment = TextAnchor.MiddleCenter;
        gridTop.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridTop.constraintCount = 1;

        gridDown = UguiMaker.newGameObject("gridDown", transform).AddComponent<GridLayoutGroup>();
        gridDown.transform.localPosition = new Vector3(0f, -250f, 0f);
        gridDown.cellSize = new Vector2(300f, 300f);
        gridDown.spacing = new Vector2(100f, 110f);
        gridDown.childAlignment = TextAnchor.MiddleCenter;
        gridDown.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridDown.constraintCount = 1;

        mLine = UguiMaker.newGameObject("mline", transform).AddComponent<Image>();
        mLine.rectTransform.sizeDelta = new Vector2(8f, 8f);
        mLine.gameObject.SetActive(false);

        mDrop = UguiMaker.newImage("mdrop", transform, "animalrotate_sprite", "mpoint", false);
        mDrop.rectTransform.sizeDelta = new Vector2(40f, 40f);
        mDrop.gameObject.SetActive(false);

        gridTop.enabled = false;
        gridDown.enabled = false;

        bInit = true;
    }

    public void ResetInfos()
    {
        for (int i = 0; i < mStationList.Count; i++)
        {
            if (mStationList[i].gameObject != null)
                GameObject.Destroy(mStationList[i].gameObject);
        }
        mStationList.Clear();
        for (int i = 0; i < mLineList.Count; i++)
        {
            if (mLineList[i].gameObject != null)
                GameObject.Destroy(mLineList[i].gameObject);
        }
        mLineList.Clear();
        for (int i = 0; i < mDropLinePointList.Count; i++)
        {
            if (mDropLinePointList[i].gameObject != null)
                GameObject.Destroy(mDropLinePointList[i].gameObject);
        }
        mDropLinePointList.Clear();
        nCount = 0;
        nToCount = 0;
    }


    public void SetData()
    {
        ResetInfos();

        mLine.transform.SetSiblingIndex(5);

        List<AnimalRotateDropLineST> mListTop = new List<AnimalRotateDropLineST>();
        List<AnimalRotateDropLineST> mListDown = new List<AnimalRotateDropLineST>();

        List<int> animalTypes = Common.GetIDList(1, 14, 14, -1);
        int nBoxType = Random.Range(0, 4);
        if (nBoxType == 1)
            nBoxType++;
        if (nTwice == 1)
        {
            List<int> type0 = animalTypes.GetRange(0, 4);
            AnimalRotateDropLineST mst0 = CreateDropStation(nBoxType, type0);
            AnimalRotateDropLineST mst1 = CreateDropStation(nBoxType, type0);
            mst0.SetBigRotate(0);
            mst1.SetBigRotate(Random.Range(1, AnimalRotateDefine.nRotateMaxType + 1));
            mListTop.Add(mst0);
            mListDown.Add(mst1);

            List<int> type1 = animalTypes.GetRange(0, 4);
            int hehe = type1[1];
            type1[1] = type1[0];
            type1[0] = hehe;
            AnimalRotateDropLineST mst2 = CreateDropStation(nBoxType, type1);
            AnimalRotateDropLineST mst3 = CreateDropStation(nBoxType, type1);
            mst2.SetBigRotate(0);
            mst3.SetBigRotate(Random.Range(1, AnimalRotateDefine.nRotateMaxType + 1));
            mListTop.Add(mst2);
            mListDown.Add(mst3);

            nToCount = 2;
        }
        else
        {
            List<int> type0 = animalTypes.GetRange(0, 4);
            AnimalRotateDropLineST mst0 = CreateDropStation(nBoxType, type0);
            AnimalRotateDropLineST mst1 = CreateDropStation(nBoxType, type0);
            mst0.SetBigRotate(0);
            mst1.SetBigRotate(Random.Range(1, AnimalRotateDefine.nRotateMaxType + 1));
            mListTop.Add(mst0);
            mListDown.Add(mst1);

            List<int> type1 = animalTypes.GetRange(0, 4);
            int hehe = type1[1];
            type1[1] = type1[0];
            type1[0] = hehe;
            AnimalRotateDropLineST mst2 = CreateDropStation(nBoxType, type1);
            AnimalRotateDropLineST mst3 = CreateDropStation(nBoxType, type1);
            mst2.SetBigRotate(0);
            mst3.SetBigRotate(Random.Range(1, AnimalRotateDefine.nRotateMaxType + 1));
            mListTop.Add(mst2);
            mListDown.Add(mst3);

            List<int> type2 = animalTypes.GetRange(0, 4);
            int hehe2 = type2[2];
            type2[2] = type2[3];
            type2[3] = hehe2;
            AnimalRotateDropLineST mst4 = CreateDropStation(nBoxType, type2);
            AnimalRotateDropLineST mst5 = CreateDropStation(nBoxType, type2);
            mst4.SetBigRotate(0);
            mst5.SetBigRotate(Random.Range(1, AnimalRotateDefine.nRotateMaxType + 1));
            mListTop.Add(mst4);
            mListDown.Add(mst5);

            nToCount = 3;
        }

        mListTop = Common.BreakRank(mListTop);
        mListDown = Common.BreakRank(mListDown);

        List<float> findexpos = Common.GetOrderList(mListTop.Count, 400f); 

        //创建拖动点
        for (int i = 0; i < mListTop.Count; i++)
        {
            mListTop[i].transform.SetParent(gridTop.transform);
            mListTop[i].transform.localPosition = new Vector3(findexpos[i], 0f, 0f);
            mListTop[i].CreateDropPoint(new Vector3(0f, -150f, 0f));
            AnimalRotateDropPointST droppointCtrl = CreateDropLinePoint(mListTop[i].mDropPoint.transform.position, mListTop[i]);
            droppointCtrl.transform.localPosition = droppointCtrl.transform.localPosition;
            mDropLinePointList.Add(droppointCtrl);
        }
        for (int i = 0; i < mListDown.Count; i++)
        {
            mListDown[i].transform.SetParent(gridDown.transform);
            mListDown[i].transform.localPosition = new Vector3(findexpos[i], 0f, 0f);
            mListDown[i].CreateDropPoint(new Vector3(0f, 150f, 0f));
            AnimalRotateDropPointST droppointCtrl = CreateDropLinePoint(mListDown[i].mDropPoint.transform.position, mListDown[i]);
            mDropLinePointList.Add(droppointCtrl);
        }

        mStationList.AddRange(mListTop);
        mStationList.AddRange(mListDown);

        for (int i=0;i<mStationList.Count;i++)
        {
            //动物随机旋转
            mStationList[i].SetRandomRotateInfo();
        }

        
        mDrop.transform.SetSiblingIndex(20);
    }

    /// <summary>
    /// 统计数据
    /// </summary>
    public void ToNext()
    {
        nCount++;
        if (nCount >= nToCount && nToCount > 0)
        {
            nTwice++;
            mCtrl.StartCoroutine(IEToNextTwice2());           
        }
    }
    IEnumerator IEToNextTwice2()
    {
        //Debug.Log("level 1 pass");
        AudioClip cp0 = mCtrl.mSoundCtrl.GetClip("animalrotate_sound", "goodgood" + Random.Range(0, 5));
        mCtrl.mSoundCtrl.PlaySound(mCtrl.mSoundCtrl.mKimiAudioSource, cp0, 1f);
        yield return new WaitForSeconds(cp0.length);
        //mCtrl.KBabyHide();
        yield return new WaitForSeconds(0.3f);
        if (nTwice <= 2)
        {
            yield return new WaitForSeconds(0.2f);
            SceneMove(false);
            yield return new WaitForSeconds(1.1f);
            SetData();
            SceneMove(true);
        }
        else
        {
            Debug.Log("level2 pass");
            mCtrl.LevelCheckNext();
            nTwice = 1;
        }
    }



    void Update ()
    {
        if (!bInit)
            return;
        MUpdate();
    }

    AnimalRotateDropLineST mSelect;
    Vector3 vInput;
    Vector3 vStartDrop;
    private void MUpdate()
    {
        LineCtrol();
        if (Input.GetMouseButtonDown(0))
        {
            #region//stp1
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hits != null)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    GameObject hitGO = hits[i].collider.gameObject;
                    if (hitGO.name.CompareTo("droppoint") == 0)
                    {
                        AnimalRotateDropPointST mmpointSt = hitGO.GetComponent<AnimalRotateDropPointST>();
                        AnimalRotateDropLineST com = mmpointSt.mDropLineST;
                        if (com != null && !com.bDropOK)
                        {
                            mSelect = com;
                            mLine.gameObject.SetActive(true);
                            mLine.rectTransform.sizeDelta = new Vector2(0, mLine.rectTransform.sizeDelta.y);
                            mDrop.transform.position = hitGO.transform.position;
                            mDrop.gameObject.SetActive(true);
                            vStartDrop = mDrop.transform.localPosition;// mDrop.rectTransform.anchoredPosition3D;

                            DropTipPointMoveReset();

                            mCtrl.mSoundCtrl.PlaySortSound("animalrotate_sound", "dropline");
                            //StopPlayTipSound();
                            break;
                        }
                    }
                }
            }
            #endregion
        }
        else if (Input.GetMouseButton(0))
        {
            #region//stp2
            if (mSelect != null)
            {
                vInput = Common.getMouseLocalPos(transform);
                mDrop.rectTransform.anchoredPosition3D = vInput;
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//stp3
            if (mSelect != null)
            {
                bool bHitOK = false;
                AnimalRotateDropLineST hitStation = null;
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hits != null)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        AnimalRotateDropPointST mmpointSt = hits[i].collider.transform.GetComponent<AnimalRotateDropPointST>();
                        if (mmpointSt != null)
                        {
                            hitStation = mmpointSt.mDropLineST;
                            if (hitStation != null && hitStation != mSelect)
                            {
                                bool isSameAnimalID = mCtrl.CheckStationCellsIsSame(mSelect.mstation, hitStation.mstation);
                                if (isSameAnimalID)
                                {
                                    bHitOK = true;
                                    mSelect.bDropOK = true;
                                    hitStation.bDropOK = true;

                                    mDrop.transform.position = hitStation.mDropPoint.transform.position;
                                    CreateLine(vStartDrop, mDrop.transform.localPosition);
                                    mDrop.gameObject.SetActive(false);
                                    mLine.gameObject.SetActive(false);
                                    LineCtrol();

                                    mCtrl.mSoundCtrl.PlaySortSound("animalrotate_sound", "linesuc");
                                    mCtrl.MovePraSys(mSelect.mDropPoint.transform.position, mDrop.transform.position);

                                    ToNext();
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!bHitOK)
                {
                    theDropPointTipMove = mDrop.transform.DOLocalMove(vStartDrop, 0.5f).OnComplete(() =>
                    {
                        mLine.gameObject.SetActive(false);
                        mDrop.gameObject.SetActive(false);
                    });
                    mCtrl.mSoundCtrl.PlaySortSound("animalrotate_sound", "lineback");
                }

                mSelect = null;
            }
            #endregion          
        }
    }

    Tween theDropPointTipMove = null;
    void DropTipPointMoveReset()
    {
        if (theDropPointTipMove != null)
            theDropPointTipMove.Pause();
    }


    private void LineCtrol()
    {
        //长度
        float dis = Vector3.Distance(vStartDrop, mDrop.rectTransform.anchoredPosition3D);
        mLine.rectTransform.sizeDelta = new Vector2(10f, dis);
        mLine.rectTransform.anchoredPosition3D = (vStartDrop + mDrop.rectTransform.anchoredPosition3D) * 0.5f;
        //旋转方向
        Vector3 dir = (mDrop.rectTransform.anchoredPosition3D - vStartDrop).normalized;
        mLine.transform.up = dir;
    }

    /// <summary>
    /// 创建一条线
    /// </summary>
    /// <param name="vstart"></param>
    /// <param name="vend"></param>
    public void CreateLine(Vector3 vstart,Vector3 vend)
    {
        Image sline = UguiMaker.newGameObject("line", transform).AddComponent<Image>();
        sline.transform.SetSiblingIndex(0);
        //长度
        float dis = Vector3.Distance(vstart, vend);
        sline.rectTransform.sizeDelta = new Vector2(8f, dis);
        sline.rectTransform.anchoredPosition3D = (vstart + vend) * 0.5f;
        //旋转方向
        Vector3 dir = (vend - vStartDrop).normalized;
        sline.transform.up = dir;
        sline.transform.SetSiblingIndex(20);
        mLineList.Add(sline);
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
            transform.DOLocalMove(new Vector3(1200f, 0f, 0f), 1f);
        }
    }

    /// <summary>
    /// 创建drop station
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_cellIDs"></param>
    /// <returns></returns>
    public AnimalRotateDropLineST CreateDropStation(int _type, int[] _cellIDs)
    {
        GameObject mgo = UguiMaker.newGameObject("mDropStation", transform);
        AnimalRotateDropLineST dropST = mgo.AddComponent<AnimalRotateDropLineST>();
        AnimalRotateStation _st = mCtrl.CreateAnimalRotateStation(_type, _cellIDs, mgo.transform);
        dropST.InitAwake(_st);

        dropST.transform.localScale = Vector3.one;
        return dropST;
    }
    public AnimalRotateDropLineST CreateDropStation(int _boxType, List<int> _type)
    {
        AnimalRotateDropLineST _ret = null;
        _ret = CreateDropStation(_boxType, _type.ToArray());
        return _ret;
    }

    /// <summary>
    /// 创建连线点
    /// </summary>
    /// <param name="_vpos"></param>
    /// <param name="_droplineST"></param>
    /// <returns></returns>
    public AnimalRotateDropPointST CreateDropLinePoint(Vector3 _vpos, AnimalRotateDropLineST _droplineST)
    {
        GameObject mgo = UguiMaker.newGameObject("droppoint", transform);
        mgo.transform.position = _vpos;
        Image mpoint = mgo.AddComponent<Image>();
        mpoint.rectTransform.sizeDelta = new Vector2(30f, 30f);
        mpoint.sprite = ResManager.GetSprite("animalrotate_sprite", "mpoint");
        BoxCollider2D cbox = mgo.AddComponent<BoxCollider2D>();
        cbox.size = new Vector2(50f,50f);
        AnimalRotateDropPointST pointSt = mgo.AddComponent<AnimalRotateDropPointST>();
        pointSt.mDropPoint = mpoint;
        pointSt.mDropLineST = _droplineST;
        return pointSt;
    }


    /// <summary>
    /// 提示语音
    /// </summary>
    /// <returns></returns>
    public IEnumerator iePlayTipSoundLv2()
    {
        yield return 1;
        AudioClip cp0 = mCtrl.mSoundCtrl.GetClip("animalrotate_sound", "game-tips6-1-2");
        mCtrl.mSoundCtrl.PlaySound(mCtrl.mSoundCtrl.mKimiAudioSource, cp0, 1f);
    }

}
