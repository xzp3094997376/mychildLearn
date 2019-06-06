using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;

public class AnimalRotateLevel3 : MonoBehaviour
{

    private AnimalRotateCtrl mCtrl;
    private bool bInit = false;

    public int nTwice = 1;
    public int nCount = 0;
    public int nToCount = 0;

    public List<AnimalRotateLv3Station> mStationList = new List<AnimalRotateLv3Station>();
    public List<AnimalRotateCell> mCellList = new List<AnimalRotateCell>();

    private Button mCheckBtn;
    private Image btnImage;

    public bool bTwicePass = false;

    private ParticleSystem mCheckOKFX;

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as AnimalRotateCtrl;

        mCheckBtn = UguiMaker.newButton("mcheckbtn", transform, "public", "checkup");
        mCheckBtn.transform.localPosition = new Vector3(0f, -200f, 0f);
        EventTriggerListener.Get(mCheckBtn.gameObject).onDown = CheckBtnDown;
        EventTriggerListener.Get(mCheckBtn.gameObject).onUp = CheckBtnUp;
        mCheckBtn.gameObject.SetActive(false);
        btnImage = mCheckBtn.GetComponent<Image>();
        mCheckBtn.transition = Selectable.Transition.None;

        mCheckOKFX = ResManager.GetPrefab("effect_okbtn", "okbtn_effect").GetComponent<ParticleSystem>();
        mCheckOKFX.transform.SetParent(mCheckBtn.transform);
        mCheckOKFX.transform.localPosition = Vector3.zero;
        mCheckOKFX.transform.localScale = Vector3.one;
        mCheckOKFX.Pause();
        mCheckOKFX.Stop();

        bInit = true;
    }

    /// <summary>
    /// reset infos
    /// </summary>
    public void ResetInfos()
    {
        for (int i = 0; i < mStationList.Count; i++)
        {
            if (mStationList[i].gameObject != null)
                GameObject.Destroy(mStationList[i].gameObject);
        }
        mStationList.Clear();

        for (int i = 0; i < mCellList.Count; i++)
        {
            if (mCellList[i].gameObject != null)
                GameObject.Destroy(mCellList[i].gameObject);
        }
        mCellList.Clear();

        nCount = 0;
        nToCount = 0;
        mCheckBtn.gameObject.SetActive(false);
        btnImage.sprite = ResManager.GetSprite("public", "checkup");

        bTwicePass = false;
    }

    private float fstationScale = 1.8f;
    private float fstationHeight = -10f;
    public void SetData()
    {
        ResetInfos();

        List<int> animalTypes = Common.GetIDList(1, 14, 14, -1);
        List<int> type0 = animalTypes.GetRange(0, 4);

        int nBoxType = Random.Range(0, 4);
        //第一个模板(固定不变)
        AnimalRotateLv3Station st0 = CreateAnimalRotateStation(nBoxType, transform);
        st0.transform.localScale = Vector3.one * fstationScale;
        st0.transform.localPosition = new Vector3(-300f, fstationHeight, 0f);
        for (int i = 0; i < type0.Count; i++)
        {
            AnimalRotateCell thecell = mCtrl.CreateAnimalRotateCell(type0[i], transform);
            thecell.SetLevel3();
            thecell.bCanDrop = false;
            thecell.SetRotateID(Random.Range(0, 4));
            st0.mCellStations[i].AddCell(thecell);
        }
        mStationList.Add(st0);


        if (nTwice == 1)
        {
            nToCount = 2;
        }
        else if (nTwice == 2)
        {
            nToCount = 3;
        }

        //已填好的
        List<int> unLostIndex = Common.GetIDList(0, 3, 4 - nToCount, -1);

        AnimalRotateLv3Station st1 = CreateAnimalRotateStation(nBoxType, transform);
        st1.transform.localScale = Vector3.one * fstationScale;
        st1.transform.localPosition = new Vector3(300f, fstationHeight, 0f);
        st1.CreateButton();
        for (int i = 0; i < type0.Count; i++)
        {
            AnimalRotateCell thecell = mCtrl.CreateAnimalRotateCell(type0[i], transform);
            thecell.SetLevel3();
            thecell.RotateTipPointActive(true);
            thecell.bInStation = false;
            if (unLostIndex.Contains(i))
            {
                //设置与左边的local旋转相同
                AnimalRotateCell mleftCell = st0.mCellStations[i].mcell;
                thecell.SetRotateID(mleftCell.nRotateID);

                thecell.bCanDrop = false;
                thecell.RotateTipPointActive(false);
                st1.mCellStations[i].AddCell(thecell);
            }
            else
                mCellList.Add(thecell);
        }            
        mStationList.Add(st1);

        //cell 排列
        mCellList = Common.BreakRank(mCellList);
        List<float> getIndex = Common.GetOrderList(nToCount, 250f);
        for (int i = 0; i < mCellList.Count; i++)
        {
            mCellList[i].transform.localPosition = new Vector3(getIndex[i], -300f, 0f);
            mCellList[i].SetRemeberPos(mCellList[i].transform.localPosition);
            mCellList[i].SetRotateID(Random.Range(1, AnimalRotateDefine.nRotateMaxType + 1));
            mCellList[i].transform.localScale = Vector3.one * 1.5f;
        }

        st1.SetRotateID(Random.Range(1, AnimalRotateDefine.nRotateMaxType + 1));

        if (nTwice == 1)
        {
            //设置顺时针(默认)
            //for (int i = 0; i < mStationList.Count; i++)
            //{ mStationList[i].nRotateDir = -1; }
            //for (int i = 0; i < mCellList.Count; i++)
            //{
            //    mCellList[i].nRotateDir = -1;
            //}
        }
        else if (nTwice == 2)
        {
            //设置逆时针
            for (int i = 0; i < mStationList.Count; i++)
            { mStationList[i].nRotateDir = 1; }
            for (int i = 0; i < mCellList.Count; i++)
            {
                mCellList[i].SetRotateDir(1);
            }
        }

    }


    public void ToNext()
    {
        nCount++;
        CheckPass();
    }
    public void CheckPass()
    {
        if (nCount >= nToCount && nToCount > 0)
        {
            if (!mCheckBtn.gameObject.activeSelf)
            {
                mCheckBtn.transform.localScale = Vector3.one * 0.001f;
                mCheckBtn.gameObject.SetActive(true);
                mCheckBtn.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);             
            }
        }
    }
    IEnumerator IENextTwice3()
    {
        //Debug.Log("level 3 pass");
        //mCtrl.KBabyShow();
        //mCtrl.bPlayOtherTip = true;
        AudioClip cp0 = mCtrl.mSoundCtrl.GetClip("animalrotate_sound", "game-right6-1-" + Random.Range(1, 3));
        mCtrl.mSoundCtrl.PlaySound(cp0, 1f);
        yield return new WaitForSeconds(cp0.length);
        //mCtrl.KBabyHide();
        yield return new WaitForSeconds(0.3f);

        if (nTwice > 2)
        {
            mCtrl.LevelCheckNext();
            nTwice = 1;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            SceneMove(false);
            yield return new WaitForSeconds(2f);
            SetData();
            SceneMove(true);
        }     
    }

    /// <summary>
    /// 隐藏check按钮
    /// </summary>
    private void CheckBtnHide()
    {
        if (nCount < nToCount)
        { mCheckBtn.gameObject.SetActive(false); }
    }
    private void CheckBtnDown(GameObject _go)
    {
        if (bTwicePass)
            return;
        btnImage.sprite = ResManager.GetSprite("public", "checkdown");
        mCtrl.mSoundCtrl.PlaySortSound("checkgamebtn_sound", "checkgamebtn_down");
    }
    private void CheckBtnUp(GameObject _go)
    {
        if (bTwicePass)
            return;
        if (mCtrl.CheckStationRotationIsSameByAngle(mStationList[0], mStationList[1]))
        {
            nTwice++;
            bTwicePass = true;
            PassReset();
            mCheckOKFX.Play();
            //Debug.Log("twice:" + nTwice);
             mCtrl.StartCoroutine(IENextTwice3());
            mCtrl.mSoundCtrl.PlaySortSound("animalrotate_sound", "按钮点击正确");
        }
        else
        {
            //Debug.Log("check faile");
            btnImage.sprite = ResManager.GetSprite("public", "checkup");
            //mCtrl.mSoundCtrl.PlaySortSound("animalrotate_sound", "按钮点击错误");
            AudioClip cp0 = ResManager.GetClip("animalrotate_sound", "game-wrong6-1-" + Random.Range(1, 3));
            mCtrl.mSoundCtrl.PlaySound(cp0, 1f);
        }
    }

    /// <summary>
    /// 完成重置信息
    /// </summary>
    private void PassReset()
    {
        mStationList[0].HideBtn();
        mStationList[1].HideBtn();
        mStationList[0].RotateTipPointActive(false);
        mStationList[1].RotateTipPointActive(false);
        for (int i = 0; i < mCellList.Count; i++)
        {
            mCellList[i].RotateTipPointActive(false);
        }
    }


    void Update ()
    {
        if (!bInit)
            return;
        if (bTwicePass)
            return;
        MUpdate();
    }

    Vector3 vstart;
    Vector3 temp_select_offset = Vector3.zero;
    AnimalRotateCell mSelect = null;
    private void MUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            #region//one
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hits != null)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    AnimalRotateCell com = hits[i].collider.gameObject.GetComponent<AnimalRotateCell>();
                    if (com != null && com.bCanDrop)
                    {
                        //先移出操作
                        if (com.bInStation)
                        {
                            AnimalRotateLv3CellStation cellst = com.transform.parent.GetComponent<AnimalRotateLv3CellStation>();
                            if (cellst != null)
                            { cellst.RemoveCell(); }
                            com.transform.SetParent(transform);
                            com.bInStation = false;
                            nCount--;
                            if (nCount <= 0)
                                nCount = 0;                   
                        }

                        mSelect = com;
                        mSelect.transform.SetSiblingIndex(20);
                        temp_select_offset = Common.getMouseLocalPos(transform) - mSelect.transform.localPosition;
                        vstart = mSelect.transform.localPosition;

                        mSelect.DropSet();

                        mCtrl.mSoundCtrl.PlaySortSound("animalrotate_sound", "点击角色转动");
                    }
                }
            }
            #endregion
        }
        else if (Input.GetMouseButton(0))
        {
            #region//two
            if (mSelect != null)
            {
                //拖动值限制
                Vector3 vInput = Common.getMouseLocalPos(transform) - temp_select_offset;
                float fX = vInput.x;
                float fY = vInput.y;
                Vector2 vsize = mSelect.GetSize();
                fX = Mathf.Clamp(fX, -GlobalParam.screen_width * 0.5f + vsize.x * 0.5f, GlobalParam.screen_width * 0.5f - vsize.x * 0.5f);
                fY = Mathf.Clamp(fY, -GlobalParam.screen_height * 0.5f + vsize.y * 0.5f, GlobalParam.screen_height * 0.5f - vsize.y * 0.5f);
                mSelect.transform.localPosition = new Vector3(fX, fY, 0f);
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//two
            if (mSelect != null)
            {
                bool bMatch = false;
                bool bHitObj = false;

                Vector3 vend = mSelect.transform.localPosition;
                float fdis = Vector3.Distance(vstart, vend);
                if (fdis <= 2.5f)
                {
                    int nrotatedir = -1;
                    if (nTwice == 2)
                    { nrotatedir = 1; }
                    mSelect.transform.localEulerAngles = mSelect.transform.localEulerAngles + new Vector3(0f, 0f, AnimalRotateDefine.fRotateIndex * nrotatedir);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hits != null)
                    {
                        for (int i = 0; i < hits.Length; i++)
                        {
                            AnimalRotateLv3CellStation com = hits[i].collider.gameObject.GetComponent<AnimalRotateLv3CellStation>();
                            if (com != null && !mSelect.bInStation && com.mcell == null)
                            {
                                bMatch = true;
                                com.AddCell(mSelect);
                                ToNext();
                                //mCtrl.mSoundCtrl.PlaySortSound("animalrotate_sound", "放入");
                                break;
                            }
                            else
                            {
                                if (hits[i].collider.gameObject != mSelect.gameObject)
                                {
                                    bHitObj = true;
                                }
                            }
                        }
                    }
                    //mSelect.transform.localPosition = vstart;
                }
                else
                {
                    RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hits != null)
                    {
                        for (int i = 0; i < hits.Length; i++)
                        {
                            AnimalRotateLv3CellStation com = hits[i].collider.gameObject.GetComponent<AnimalRotateLv3CellStation>();
                            if (com != null && !mSelect.bInStation && com.mcell == null)
                            {
                                bMatch = true;
                                com.AddCell(mSelect);
                                ToNext();
                                mCtrl.mSoundCtrl.PlaySortSound("animalrotate_sound", "放入");
                                break;
                            }
                            else
                            {
                                if (hits[i].collider.gameObject != mSelect.gameObject)
                                {
                                    bHitObj = true;
                                }
                            }
                        }
                    }
                }


                if (!bMatch && bHitObj)
                {
                    mSelect.MoveToRemeberPos();                  
                }
                else
                {
                    mSelect.SetRemeberPos(mSelect.transform.localPosition);
                    if (!bMatch)
                        mSelect.DropReset();
                }

                CheckBtnHide();

                mSelect = null;
            }
            #endregion
        }
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




    public AnimalRotateLv3Station CreateAnimalRotateStation(int _type, Transform _transform)
    {
        AnimalRotateLv3Station mstation = null;
        GameObject mgo = ResManager.GetPrefab("animalrotate_prefab", "type" + _type);
        mgo.transform.SetParent(_transform);
        mgo.transform.localScale = Vector3.one;
        mgo.transform.localPosition = Vector3.zero;
        mstation = mgo.AddComponent<AnimalRotateLv3Station>();
        mstation.InitAwake(_type);
        return mstation;
    }



    /// <summary>
    /// 提示语音
    /// </summary>
    /// <returns></returns>
    public IEnumerator iePlayTipSoundLv3()
    {
        yield return 1;
        AudioClip cp0 = mCtrl.mSoundCtrl.GetClip("animalrotate_sound", "game-tips6-1-3");
        mCtrl.mSoundCtrl.PlaySound(mCtrl.mSoundCtrl.mKimiAudioSource, cp0, 1f);
    }

}
