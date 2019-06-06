using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class KnowCirLevel1 : MonoBehaviour
{
    KnowCircularCtrl mCtrl;
    KnowCircularShip mShip;

    public int nGameTimes = 0;//累积次数
    public int nToCount = 0;//达成拖对目标
    public int nCount = 0;//拖对次数

    public bool bCanDrop = false;
    private List<Vector3> mResPos = new List<Vector3>();//位置
    public List<KCirObjLv1> mDropObjList = new List<KCirObjLv1>();//dropObj

    private KCYinhua kcYanhua0;
    private KCYinhua kcYanhua1;
    private KCYinhua kcYanhua2;

    private void SetPos()
    {
        mResPos.Add(new Vector3(-528f, 133f, 0f));
        mResPos.Add(new Vector3(-540f, -22f, 0f));
        mResPos.Add(new Vector3(-382f, 185, 0f));
        mResPos.Add(new Vector3(-245f, 256, 0f));
        mResPos.Add(new Vector3(214, 240, 0f));
        mResPos.Add(new Vector3(479, 227, 0f));
        mResPos.Add(new Vector3(505, 42, 0f));
        mResPos.Add(new Vector3(518, -180, 0f));
        mResPos.Add(new Vector3(375, -311, 0f));
        mResPos.Add(new Vector3(142, -311, 0f));
        mResPos.Add(new Vector3(-118, -324, 0f));
        mResPos.Add(new Vector3(-370, -329, 0f));
        mResPos.Add(new Vector3(-517, -234, 0f));//12
        //for (int i = 0; i < mResPos.Count; i++)
        //{
        //    UnityEngine.UI.Image mgo = UguiMaker.newGameObject("pos", transform).AddComponent<UnityEngine.UI.Image>();
        //    mgo.rectTransform.sizeDelta = Vector2.one * 100f;
        //    mgo.transform.localPosition = mResPos[i];
        //}
    }

    public void InitAwake(KnowCircularCtrl _mctrl)
    {
        mCtrl = _mctrl;     
        SetPos();

        //kcYanhua0 = ResManager.GetPrefab("knowcircular_prefab", "yanhua1", transform).gameObject.AddComponent<KCYinhua>();
        //kcYanhua1 = ResManager.GetPrefab("knowcircular_prefab", "yanhua0", transform).gameObject.AddComponent<KCYinhua>();
        //kcYanhua2 = ResManager.GetPrefab("knowcircular_prefab", "yanhua2", transform).gameObject.AddComponent<KCYinhua>();
        kcYanhua0 = UguiMaker.newImage("yanhua0", transform, "knowcircular_sprite", "yinhua0", false).gameObject.AddComponent<KCYinhua>();
        kcYanhua1 = UguiMaker.newImage("yanhua1", transform, "knowcircular_sprite", "yinhua1", false).gameObject.AddComponent<KCYinhua>();
        kcYanhua2 = UguiMaker.newImage("yanhua2", transform, "knowcircular_sprite", "yinhua0", false).gameObject.AddComponent<KCYinhua>();
        kcYanhua0.transform.localPosition = new Vector3(94f, 190f, 0f);
        kcYanhua1.transform.localPosition = new Vector3(-434f, 136f, 0f);
        kcYanhua2.transform.localPosition = new Vector3(482f, 218f, 0f);
        kcYanhua0.InitAwake();
        kcYanhua1.InitAwake();
        kcYanhua2.InitAwake();
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
        mResPos = Common.BreakRank(mResPos);
        bCanDrop = false;

        if (mShip != null)
        { mShip.ResetInfos(); }

        //kcYanhua1.gameObject.SetActive(false);
        //kcYanhua2.gameObject.SetActive(false);
        //kcYanhua0.gameObject.SetActive(false);
    }

    public void SetDate()
    {
        ResetInfos();
        StartCoroutine(ieSetDate());
    }
    IEnumerator ieSetDate()
    {
        if (mShip == null)
        {
            mShip = ResManager.GetPrefab("knowcircular_prefab", "ship", transform).AddComponent<KnowCircularShip>();
            mShip.transform.localPosition = new Vector3(-30f, -1090f, 0f);
            mShip.InitAwake();
            mShip.transform.DOLocalMoveY(-290f, 1f);
            yield return new WaitForSeconds(1f);
        }
        else
        {
            mShip.ShakeLRActive(false);
        }

        int allCount = UnityEngine.Random.Range(8, 13);
        List<int> mTypeList = new List<int>();
        //至少要有3个圆
        mTypeList.Add(0);
        mTypeList.Add(0);
        mTypeList.Add(0);
        allCount -= 3;
        //至少要有2四方形
        mTypeList.Add(1);
        mTypeList.Add(1);
        allCount -= 2;
        //至少要有2三角形
        mTypeList.Add(2);
        mTypeList.Add(2);
        allCount -= 2;
        //剩余随机
        for (int i = 0; i < allCount; i++)
        {
            mTypeList.Add(UnityEngine.Random.Range(0, 3));
        }

        for (int i = 0; i < mTypeList.Count; i++)
        {
            if (mTypeList[i] == 0)
            {
                nToCount++;
            }
            KCirObjLv1 spCtrl = UguiMaker.newGameObject("spine" + mTypeList[i],transform).AddComponent<KCirObjLv1>();
            spCtrl.transform.localPosition = mResPos[i];
            spCtrl.SetStartPos(mResPos[i]);
            spCtrl.nType = mTypeList[i];
            spCtrl.InitAwake();
            mDropObjList.Add(spCtrl);
            mCtrl.PlaySetBigSound();
            yield return new WaitForSeconds(0.2f);
        }

        ShowGuideDrop();

        bCanDrop = true;
        mCtrl.PlayTipSound();
    }

    KCirObjLv1 mSelect;
    Vector3 temp_select_offset = Vector3.zero;
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha8))
        //{
        //    kcYanhua0.nCount = 3;
        //    kcYanhua1.nCount = 3;
        //    kcYanhua2.nCount = 3;
        //    kcYanhua0.PlayYinhua();
        //    kcYanhua1.PlayYinhua();
        //    kcYanhua2.PlayYinhua();
        //}
        if (!bCanDrop)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            #region//one
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            for (int i = 0; i < hits.Length; i++)
            {
                KCirObjLv1 com = hits[i].collider.gameObject.GetComponent<KCirObjLv1>();
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
                mSelect.bDropState = true;
                mSelect.DropSet();
                mSelect.SetOrderReander(4);
                mSelect.PlayFudongTween(false);
                RectTransform retf = mSelect.transform as RectTransform;
                temp_select_offset = Common.getMouseLocalPos(transform) - retf.anchoredPosition3D;
                mSelect.transform.SetSiblingIndex(50);
                mSelect.SetRemindPos(mSelect.transform.localPosition);
                mSelect.PlayAnimation("Click", -1f);
                mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "Click_" + UnityEngine.Random.Range(1, 4)));
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
            if (mSelect != null)
            {
                mCtrl.GuideStop();

                bool bMatch0 = false;
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                for (int i = 0; i < hits.Length; i++)
                {
                    GameObject hitObj = hits[i].collider.gameObject;
                    if (hitObj.name.CompareTo("ship_body") == 0)
                    {
                        bMatch0 = true;
                        if (mSelect.nType == 0)
                        {
                            mSelect.SetOrderReander(3);
                            mShip.SetCirInShip(mSelect);
                            mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "setok1"));
                            CheckLevelPass();
                            mCtrl.PlayObjSound(mSelect.nType);
                            mShip.PlayKBabyAnimation("Succeed", 1f);
                            mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "Succeed_" + UnityEngine.Random.Range(1, 3)));
                        }
                        else
                        {
                            mSelect.DropReset();
                            mShip.PlayKBabyAnimation("Defeated", 2f);
                            mSelect.PlayAnimation("Defeated", 2f);                        
                            mSelect.BackToRemindPos();                          
                            mCtrl.PlayObjSound(mSelect.nType);
                            mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "Defeated_" + UnityEngine.Random.Range(1, 4)));
                        }
                        break;
                    }
                }

                if (!bMatch0)
                {
                    mSelect.PlayAnimation("Idle", 0f);
                    mSelect.DropReset();
                }
                mSelect.bDropState = false;
                
            }
            mSelect = null;
        }
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
        yield return new WaitForSeconds(1.1f);
        mShip.ShakeLRActive(true);
        mCtrl.StartCoroutine(iePlayYinhua());
        mShip.PlayKBabyAnimation("Succeed", -1f);
        mShip.PlayShipLight(30);
        yield return new WaitForSeconds(2f);
        mCtrl.PlaySucSound();     
        yield return new WaitForSeconds(4f);
        if (nGameTimes < 2)
        {
            //元素消失
            for (int i = 0; i < mDropObjList.Count; i++)
            {
                mDropObjList[i].DoScale(Vector3.one * 0.001f, 0.3f);
            }
            yield return new WaitForSeconds(0.5f);
            //replay
            SetDate();
        }
        else
        {
            mShip.ShakeLRActive(false);
            //yield return new WaitForSeconds(1f);
            mCtrl.LevelCheckNext();
        }
    }

    IEnumerator iePlayYinhua()
    {
        kcYanhua0.gameObject.SetActive(true);
        kcYanhua0.PlayYinhua();
        yield return new WaitForSeconds(0.25f);
        kcYanhua1.gameObject.SetActive(true);
        kcYanhua1.PlayYinhua();
        yield return new WaitForSeconds(0.25f);
        kcYanhua2.gameObject.SetActive(true);
        kcYanhua2.PlayYinhua();
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
        Vector3 vto = mShip.GetDropTo();
        mCtrl.GuideShow(vfrom, vto);
        bGuide = true;
    }    
}
