using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class KnowCubeLv3 : MonoBehaviour
{

    private Vector3 vStartPos = new Vector3(0f, -30f, 0f);
    private RawImage bg;

    private Transform poss;
    private List<Transform> mposList = new List<Transform>();

    private List<int> mDataList = new List<int>();

    private List<KnowCubeBigcube> mBigCubeList = new List<KnowCubeBigcube>();

    private KnowCubeBottle mBottle;
    public bool bCanDrop = false;

    private GameObject moklight;

    bool bInit = false;
    public KnowCubeCtrl mCtrl;

    private GuideHandCtl mGuideCtrl;

    public void InitAwake ()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as KnowCubeCtrl;

        transform.localPosition = vStartPos;
        bg = transform.Find("map2").GetComponent<RawImage>();
        bg.texture = ResManager.GetTexture("knowcube_texture", "map2");

        poss = transform.Find("poss");
        for (int i = 0; i < 5; i++)
        {
            Transform tr = poss.Find("pos" + i);
            mposList.Add(tr);
        }

        mBottle = transform.Find("bottle").gameObject.AddComponent<KnowCubeBottle>();
        mBottle.InitAwake(this);
        mBottle.transform.localScale = Vector3.one * 0.001f;

        moklight = ResManager.GetPrefab("oklight_prefab", "moklight");
        moklight.transform.SetParent(transform);
        moklight.transform.localScale = Vector3.one;
        moklight.transform.localPosition = new Vector3(211f, 30f, 0f);
        moklight.SetActive(false);

        mGuideCtrl = GuideHandCtl.Create(transform);
        bInit = true;
    }


    public void ResetInfos()
    {
        for (int i = 0; i < mBigCubeList.Count; i++)
        {
            if (mBigCubeList[i].gameObject != null)
            {
                GameObject.Destroy(mBigCubeList[i].gameObject);
            }
        }
        mBigCubeList.Clear();
        mBottle.ResetINfos();

        moklight.SetActive(false);
    }

    /// <summary>
    /// 设置信息
    /// </summary>
    public void SetData()
    {
        this.StopAllCoroutines();
        mDataList.Clear();
        mDataList = Common.GetIDList(0, 4, 2, -1);

        StartCoroutine(IEStartPanel3Data());
    }
    IEnumerator IEStartPanel3Data()
    {
        ResetInfos();

        SceneMove(true);
        yield return new WaitForSeconds(1.1f);
        mCtrl.PlayTipSound();
        //pos
        mposList = Common.BreakRank(mposList);

        //tip cube
        int getindex = mDataList[0];
        KnowCubeBigcube leftbigcube = CreateBigCube(getindex, 0, new Vector3(-460f, 0f, 0f));
        mBigCubeList.Add(leftbigcube);
        mDataList.RemoveAt(0);

        leftbigcube.transform.localPosition = new Vector3(-1000f, 0f, 0f);
        leftbigcube.transform.DOLocalMove(leftbigcube.vstart, 0.3f);
        yield return new WaitForSeconds(0.3f);

        //选项cube
        for (int i = 0; i < 5; i++)
        {
            KnowCubeBigcube bigcube = CreateBigCube(getindex, i + 1, mposList[i].localPosition);
            mBigCubeList.Add(bigcube);
            bigcube.transform.localScale = Vector3.one * 0.001f;
            bigcube.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            mCtrl.PlayTheSortSound("cubeshowout");
            yield return new WaitForSeconds(0.3f);
        }

        //bottle
        mBottle.transform.DOScale(Vector3.one, 0.3f);
        mCtrl.PlayTheSortSound("cubeshowout");

        if (mDataList.Count == 1)
        {
            mGuideCtrl.transform.SetSiblingIndex(20);
            ShowDropTip();
        }
        bCanDrop = true;
    }


    KnowCubeBigcube mSelect;
    Vector3 temp_select_offset = Vector3.zero;
    private void Update()
    {
        if (!bInit)
            return;

        if (!bCanDrop)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            #region//射线检测
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                KnowCubeBigcube com = hit.collider.gameObject.GetComponent<KnowCubeBigcube>();
                if (com != null)
                {
                    if (com.nIndex != 0)
                    {
                        temp_select_offset = Common.getMouseLocalPos(transform) - com.rectTransform.anchoredPosition3D;
                        com.transform.SetSiblingIndex(20);
                        mSelect = com;
                        mCtrl.PlayTheSortSound("cubeshowout");
                        StopDropTip();
                    }
                }
            }
            #endregion
        }
        else if (Input.GetMouseButton(0))
        {
            if (mSelect != null)
            {
                Vector3 vInput = Common.getMouseLocalPos(transform) - temp_select_offset;
                mSelect.rectTransform.anchoredPosition3D = vInput;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//two
            if (mSelect != null)
            {
                bool bHitIn = false;
                #region//射线检测
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hits != null)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].collider.transform.parent == mBottle.transform)
                        {
                            bHitIn = true;
                            mBottle.DropIn(mSelect);
                            break;
                        }
                    }
                }
                #endregion

                if (!bHitIn)
                {
                    mSelect.MoveToStart();
                }
                mSelect = null;
            }
            #endregion
        }
    }

    /// <summary>
    /// 组合完成
    /// </summary>
    public void CombineOK()
    {
        StartCoroutine(IEPanel3Finish());
    }
    IEnumerator IEPanel3Finish()
    {
        yield return new WaitForSeconds(0.3f);
        //其他de变小
        for (int i = 3; i < mBigCubeList.Count; i++)
        {
            if ((mBigCubeList[i].nIndex != 1) || (mBigCubeList[i].nIndex != 2) || (mBigCubeList[i].nIndex != 0))
            {
                mBigCubeList[i].transform.DOScale(Vector3.one * 0.001f, 0.5f);
            }
        }
        //瓶子move center
        mBottle.transform.DOLocalMoveY(-90f, 0.5f);  
        yield return new WaitForSeconds(1f);
        //特效...

        AudioClip cp0 = mCtrl.GetClip("zadditional-7-" + Random.Range(0, 2));
        mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp0, 1f);

        yield return new WaitForSeconds(0.6f);
        //正确de变大
        for (int i = 0; i < mBottle.mdropInList.Count; i++)
        {
            mBottle.mdropInList[i].transform.SetParent(transform);
            mBottle.mdropInList[i].transform.DOScale(Vector3.one,0.5f).SetEase(Ease.OutBack);
            mBottle.mdropInList[i].transform.SetSiblingIndex(3 + mBottle.mdropInList[i].nIndex);
        }
        moklight.gameObject.SetActive(true);
        moklight.transform.DOScale(Vector3.one, 0.3f);
        mCtrl.PlayTheSortSound("show_reward0");
        //瓶子消失
        mBottle.BottleHide();
        
        yield return new WaitForSeconds(3f);
        //end
        if (mDataList.Count <= 0)
        {
            //to finish game
            mCtrl.LevelPass();
        }
        else
        {
            //to next
            SceneMove(false);
            moklight.transform.DOScale(Vector3.one * 0.001f, 0.3f);
            yield return new WaitForSeconds(1.1f);
            moklight.gameObject.SetActive(false);
            StartCoroutine(IEStartPanel3Data());
        }
    }

    //创建cubes
    private KnowCubeBigcube CreateBigCube(int _type,int _index,Vector3 _pos)
    {
        GameObject mgo = ResManager.GetPrefab("knowcube_prefab", "bigcube" + _type + "_" + _index);
        mgo.transform.SetParent(transform);
        mgo.transform.localScale = Vector3.one;
        mgo.transform.localPosition = _pos;
        KnowCubeBigcube bigcubectrl = mgo.AddComponent<KnowCubeBigcube>();
        bigcubectrl.InitAwake(_type, _index);
        return bigcubectrl;
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

    
    public void ShowDropTip()
    {
        KnowCubeBigcube theFrom = mBigCubeList.Find((x) => { return x.nIndex == 1; });
        if (theFrom != null)
        {
            Vector3 vfrom = theFrom.GetCenter().position;
            Vector3 vto = mBottle.transform.position + new Vector3(0f,0.2f,0f);
            mGuideCtrl.GuideTipDrag(vfrom, vto, -1, 1f, "hand1");
            mGuideCtrl.SetDragDate(new Vector3(0f, -26f, 0f), 1f);
        }
    }
    public void StopDropTip()
    {
        mGuideCtrl.StopDrag();
    }


    //开始提示3
    public IEnumerator IEPlayTipSound3()
    {
        AudioClip cp0 = mCtrl.GetClip("zadditional-6");
        mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp0, 1f);
        yield return 1f;
    }


}
