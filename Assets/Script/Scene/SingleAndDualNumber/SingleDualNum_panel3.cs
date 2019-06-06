using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class SingleDualNum_panel3 : MonoBehaviour
{
    public int nCount = 0;
    public int nToCount = 0;

    public SDN_Panel3Station mstation0;
    public SDN_Panel3Station mstation1;

    public List<SDN_Panel3Zhenzhu> mZhenzhuList = new List<SDN_Panel3Zhenzhu>();

    public bool bPanelOK = true;
    public SingleAndDualNumCtrl mCtrl;

    private GameObject mBtnArea;
    private bool bInit = false;

    private Image btnImg;
    private Button mCheckBtn;
    public void CheckBtnActive(bool _active)
    { mCheckBtn.gameObject.SetActive(_active); }

    private ParticleSystem mCheckOKFX;

    private bool bGuide3 = true;
    private miniImageNumber miniNumber;

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as SingleAndDualNumCtrl;

        mstation0 = transform.Find("station0").gameObject.AddComponent<SDN_Panel3Station>();
        mstation0.InitAwake(0);
        mstation1 = transform.Find("station1").gameObject.AddComponent<SDN_Panel3Station>();
        mstation1.InitAwake(1);
        //(语音需求)交换下位置...
        Vector3 vst0 = mstation0.transform.localPosition;
        mstation0.transform.localPosition = mstation1.transform.localPosition;
        mstation1.transform.localPosition = vst0;

        bPanelOK = true;

        mBtnArea = UguiMaker.newGameObject("btnArea",transform);
        mBtnArea.transform.localPosition = new Vector3(-368f, -340f, 0f);
        BoxCollider2D btnAreaBox = mBtnArea.AddComponent<BoxCollider2D>();
        btnAreaBox.size = new Vector2(560f, 150f);

        mCheckBtn = UguiMaker.newButton("checkbtn", transform, "public", "checkup");
        btnImg = mCheckBtn.GetComponent<Image>();
        btnImg.rectTransform.anchoredPosition = new Vector2(0f, -170f);
        EventTriggerListener.Get(mCheckBtn.gameObject).onDown = CheckBtnDown;
        EventTriggerListener.Get(mCheckBtn.gameObject).onUp = CheckBtnUp;

        mCheckOKFX = ResManager.GetPrefab("singledualnum_prefab", "checkok_fx").GetComponent<ParticleSystem>();
        mCheckOKFX.transform.SetParent(mCheckBtn.transform);
        mCheckOKFX.transform.localPosition = Vector3.zero;
        mCheckOKFX.transform.localScale = Vector3.one;
        mCheckOKFX.Pause();
        mCheckOKFX.Stop();

        miniNumber = UguiMaker.newGameObject("miniNumber", transform).AddComponent<miniImageNumber>();
        miniNumber.transform.localPosition = new Vector3(0f, 50f, 0f);
        miniNumber.transform.localScale = Vector3.one * 2.5f;
        miniNumber.strABName = "singledualnum_sprite";
        miniNumber.strFirstPicName = "";
        miniNumber.gameObject.SetActive(false);

        bInit = true;
    }


    public void ReplayReset()
    {
        bPanelOK = true;
        bGuide3 = true;
        ResetInfos();
        CheckBtnActive(false);
    }

    public void ResetInfos()
    {
        btnImg.sprite = ResManager.GetSprite("public", "checkup");

        for (int i = 0; i < mZhenzhuList.Count; i++)
        {
            if (mZhenzhuList[i].gameObject != null)
                GameObject.Destroy(mZhenzhuList[i].gameObject);
        }
        mZhenzhuList.Clear();

        mstation0.ResetInfos();
        mstation1.ResetInfos();
    }

    public void SetData()
    {
        ResetInfos();
        btnImg.gameObject.SetActive(false);
        mCtrl.bButtonActive = false;

        //set check id
        mstation0.nCheck = 0;
        mstation1.nCheck = 1;
        bGuide3 = false;
        if (bGuide3)
        {
            mstation0.gameObject.SetActive(false);
            mstation1.gameObject.SetActive(false);
            mCtrl.StartCoroutine(ieGuide3());
        }
        else
        {
            mCtrl.StartCoroutine(IECreateZhunzhu());
        }
    }
    AudioClip mmcp;
    IEnumerator IECreateZhunzhu()
    {
        mstation0.gameObject.SetActive(true);
        mstation1.gameObject.SetActive(true);

        nCount = 0;
        nToCount = 10;
        List<int> nids = Common.GetIDList(1, 50, nToCount, -1);
        for (int i = 0; i < nids.Count; i++)
        {
            int numberid = nids[i];
            GameObject imgzhenzhu = UguiMaker.newGameObject("zhenzhu" + numberid, transform);
            SDN_Panel3Zhenzhu zhenzhuctrl = imgzhenzhu.AddComponent<SDN_Panel3Zhenzhu>();
            zhenzhuctrl.InitAwake(numberid);
            zhenzhuctrl.ShowOut();
            zhenzhuctrl.PlayMove();
            mZhenzhuList.Add(zhenzhuctrl);

            if (mmcp == null)
            {
                mmcp = Resources.Load<AudioClip>("sound/素材出现通用音效");
            }
            mCtrl.PlayTheSortSound(mmcp);

            yield return new WaitForSeconds(0.1f);
        }
        
        //AudioClip cp = mCtrl.GetClip("z_manyzhenzhu");
        //mCtrl.PlayTheSortSound(cp);
        //yield return new WaitForSeconds(cp.length);
        mCtrl.bButtonActive = true;

        mCtrl.PlayTipSound();
    }


    IEnumerator ieGuide3()
    {
        yield return new WaitForSeconds(0.1f);

        mStopSound();
        AudioClip cp0 = ResManager.GetClip("singledualnum_sound", "偶数最后一个是");
        mCtrl.PlaySound(cp0, 1f);
        miniNumber.gameObject.SetActive(true);
        miniNumber.SetNumber(20);
        MiniNumberEffect();
        yield return new WaitForSeconds(3.5f);
        miniNumber.SetNumber(22);
        MiniNumberEffect();
        yield return new WaitForSeconds(1f);
        miniNumber.SetNumber(24);
        MiniNumberEffect();
        yield return new WaitForSeconds(1f);
        miniNumber.SetNumber(26);
        MiniNumberEffect();
        yield return new WaitForSeconds(0.7f);
        miniNumber.SetNumber(28);
        MiniNumberEffect();
        yield return new WaitForSeconds(1.5f);

        
        AudioClip cp1 = ResManager.GetClip("singledualnum_sound", "奇数最后一个是");
        mCtrl.PlaySound(cp1, 1f);
        miniNumber.gameObject.SetActive(true);
        miniNumber.SetNumber(21);
        MiniNumberEffect();
        yield return new WaitForSeconds(3.5f);
        miniNumber.SetNumber(23);
        MiniNumberEffect();
        yield return new WaitForSeconds(0.85f);
        miniNumber.SetNumber(25);
        MiniNumberEffect();
        yield return new WaitForSeconds(0.8f);
        miniNumber.SetNumber(27);
        MiniNumberEffect();
        yield return new WaitForSeconds(0.6f);
        miniNumber.SetNumber(29);
        MiniNumberEffect();
        yield return new WaitForSeconds(1.5f);

        bGuide3 = false;
        miniNumber.gameObject.SetActive(false);

        mCtrl.BtnsMove(true, 0.5f);
        mCtrl.StartCoroutine(IECreateZhunzhu());
    }

    /// <summary>
    /// 数字出现效果
    /// </summary>
    public void MiniNumberEffect()
    {
        //miniNumber.transform.localScale = Vector3.one * 0.001f;
        //miniNumber.transform.DOScale(Vector3.one * 2.5f, 0.2f);
    }


    

    Vector3 temp_select_offset = Vector3.zero;
    SDN_Panel3Zhenzhu mSelect = null;
    private void Update()
    {
        if (!bInit)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            #region//one
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hits != null)
            {
                SDN_Panel3Station _st = null;
                for (int i = 0; i < hits.Length; i++)
                {
                    GameObject mgo = hits[i].collider.gameObject;
                    if (mgo.name.CompareTo("mcollider") == 0)
                    {
                        _st = mgo.transform.parent.GetComponent<SDN_Panel3Station>();
                    }                
                    SDN_Panel3Zhenzhu com = mgo.GetComponent<SDN_Panel3Zhenzhu>();
                    if (com != null)
                    {
                        if (mSelect == null)
                        {
                            mSelect = com;
                        }
                        else if (com.transform.GetSiblingIndex() > mSelect.transform.GetSiblingIndex())
                        {
                            mSelect = com;
                        }
                        //优先选中在station里的
                        if (com.bInStation)
                        {
                            mSelect = com;
                        }
                    }
                }

                if (mSelect != null)
                {
                    if (_st != null)
                    {
                        if (mSelect.bInStation)
                        {
                            mstation0.RemoveZhenzhu(mSelect);
                            mstation1.RemoveZhenzhu(mSelect);

                            mSelect.StopMove();
                            mSelect.transform.SetParent(mCtrl.transform);
                            mSelect.transform.SetSiblingIndex(50);
                            mSelect.bInStation = false;
                            RectTransform retf = mSelect.transform as RectTransform;
                            temp_select_offset = Common.getMouseLocalPos(transform) - retf.anchoredPosition3D;
                            mSelect.DropSet();
                            mCtrl.PlayTheSortSound("点击拖拽泡泡");
                        }
                        else
                        {
                            mSelect = null;
                        }
                    }
                    else
                    {
                        mSelect.StopMove();
                        mSelect.transform.SetParent(mCtrl.transform);
                        mSelect.transform.SetSiblingIndex(50);
                        mSelect.bInStation = false;
                        RectTransform retf = mSelect.transform as RectTransform;
                        temp_select_offset = Common.getMouseLocalPos(transform) - retf.anchoredPosition3D;
                        mSelect.DropSet();
                        mCtrl.PlayTheSortSound("点击拖拽泡泡");
                    }                                                
                }
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
                fX = Mathf.Clamp(fX, -540f, 540);
                fY = Mathf.Clamp(fY, -220f , 280f);
                mSelect.transform.localPosition = new Vector3(fX, fY, 0f);
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//two
            if (mSelect != null)
            {
                //mSelect.DropReset();

                bool bMatch = false;
                SDN_Panel3Station _station = null;

                bool bHitBtnArea = false;

                #region//射线检测
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hits != null)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        GameObject hitGO = hits[i].collider.gameObject;
                        if (hitGO.name.CompareTo("mcollider") == 0)
                        {
                            _station = hitGO.transform.parent.GetComponent<SDN_Panel3Station>();
                            if (_station != null)
                            {
                                if (_station.AddZhenzhuNumber(mSelect))
                                {
                                    CheckPass();
                                    bMatch = true;
                                }
                            }                          
                        }
                        if (hitGO == mBtnArea)
                        {
                            bHitBtnArea = true;
                        }
                    }
                }
                #endregion

                if (!bMatch)
                {
                    mSelect.transform.SetParent(transform);
                    if (_station == null)
                    {
                        if (bHitBtnArea)
                            mSelect.MoveToRememberPos();
                        else
                        {
                            mSelect.SetRememberPos(mSelect.transform.localPosition);
                            mSelect.PlayMove();
                        }
                    }
                    else
                    {
                        mSelect.MoveToRememberPos();
                    }
                    mSelect.DropReset();
                }
 
                OrderSetting();

                mSelect = null;
            }
            #endregion
        }
    }


    private void CheckPass()
    {
        nCount++;
        if (nCount >= nToCount && nToCount > 0)
        {
            btnImg.transform.localScale = Vector3.one * 0.001f;
            btnImg.gameObject.SetActive(true);
            btnImg.transform.DOScale(Vector3.one, 0.2f);       
        }
    }
    
    private void CheckBtnDown(GameObject _go)
    {
        if (bPanelOK)
            return;
        btnImg.sprite = ResManager.GetSprite("public", "checkdown");
        AudioClip cp = ResManager.GetClip("checkgamebtn_sound", "checkgamebtn_down");
        if (cp != null)
            AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position);
    }
    private void CheckBtnUp(GameObject _go)
    {
        if (bPanelOK)
            return;
        
        bool bOk0 = mstation0.CheckStationIsOK();
        bool bOk1 = mstation1.CheckStationIsOK();

        if (bOk0 && bOk1)
        {
            mPlayCheckOK();
            mCheckOKFX.Play();
            bPanelOK = true;
            mCtrl.bButtonActive = false;
            mCtrl.StartCoroutine(IEPanel3Finish());
            mCtrl.PlayTheSortSound("按钮点击正确");
        }
        else
        {
            mstation0.RemoveUnOKZhenzhu();
            mstation1.RemoveUnOKZhenzhu();
            btnImg.gameObject.SetActive(false);

            OrderSetting();

            mPlayCheckFaliTip();
            mCtrl.PlayTheSortSound("按钮点击错误");
            btnImg.sprite = ResManager.GetSprite("public", "checkup");
        }
    }

    IEnumerator IEPanel3Finish()
    {
        yield return new WaitForSeconds(2.5f);
        mCtrl.MLevelPass();
    }


    /// <summary>
    /// 层设置
    /// </summary>
    public void OrderSetting()
    {
        mstation0.transform.SetSiblingIndex(30);
        mstation1.transform.SetSiblingIndex(30);
        //int index = 0;
        //for (int i = 0; i < mZhenzhuList.Count - 1; ++i)
        //{
        //    index = i;
        //    for (int j = i + 1; j < mZhenzhuList.Count; ++j)
        //    {
        //        if (mZhenzhuList[j].transform.position.y > mZhenzhuList[index].transform.position.y)
        //            index = j;
        //    }
        //    SDN_Panel3Zhenzhu t = mZhenzhuList[index];
        //    mZhenzhuList[index] = mZhenzhuList[i];
        //    mZhenzhuList[i] = t;
        //    mZhenzhuList[i].transform.SetSiblingIndex(40);
        //}
        //mZhenzhuList[mZhenzhuList.Count - 1].transform.SetSiblingIndex(40);
    }


    public void SceneMove(bool _in, float _time)
    {
        if (_in)
        {
            gameObject.SetActive(true);
            if (bPanelOK)
            {
                bPanelOK = false;
                SetData();
            }
            transform.localPosition = new Vector3(0f, 900f, 0f);
            transform.DOLocalMove(Vector3.zero, _time);
            if (!bGuide3)
            {
                mCtrl.BtnsMove(true, 0.5f);
            }
        }
        else
        {
            transform.DOLocalMove(new Vector3(0f, 900f, 0f), _time).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }

    public void ChangePanelResetInfos()
    {
        mStopSound();
    }

    /// <summary>
    /// 停止语音
    /// </summary>
    public void mStopSound()
    {
        mCtrl.StopTipSound();
    }

    /// <summary>
    /// 播放第3关提示音
    /// </summary>
    public void mPlayTipSound3()
    {
        mCtrl.SetTipSound(iePlaySoundLv3());
    }
    IEnumerator iePlaySoundLv3()
    {
        List<AudioClip> cpList = new List<AudioClip>();
        cpList.Add(mCtrl.GetClip("z_putinto0"));
        cpList.Add(mCtrl.GetClip("z_putinto1"));
        for (int i = 0; i < cpList.Count; i++)
        {
            mCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }      
    }

    /// <summary>
    /// 播放数字是奇数/偶数
    /// </summary>
    /// <param name="_num"></param>
    public void PlayNumSingleDual(int _num)
    {
        mCtrl.StopTipSound();
        mCtrl.SetTipSound(IEPlayNumSD(_num));
        mCtrl.PlayTipSound();
    }
    IEnumerator IEPlayNumSD(int _num)
    {
        List<AudioClip> cpList = new List<AudioClip>();
        cpList.Add(mCtrl.GetNumClip(_num));
        string strCp = "是奇数";
        if (_num % 2 == 0)
        { strCp = "是偶数"; }
        AudioClip cp1 = mCtrl.GetClip(strCp);
        cpList.Add(cp1);
        for (int i = 0; i < cpList.Count; i++)
        {
            mCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
    }

    /// <summary>
    /// 检测失败音
    /// </summary>
    public void mPlayCheckFaliTip()
    {
        mCtrl.StopTipSound();
        mCtrl.SetTipSound(iePlayCheckFaliTip());
        mCtrl.PlayTipSound();
    }
    IEnumerator iePlayCheckFaliTip()
    {
        List<AudioClip> cpList = new List<AudioClip>();
        cpList.Add(mCtrl.GetClip("z_checkfailetip0"));
        cpList.Add(mCtrl.GetClip("z_checkfailetip1"));
        for (int i = 0; i < cpList.Count; i++)
        {
            mCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
    }

    /// <summary>
    /// 成功语音音
    /// </summary>
    public void mPlayCheckOK()
    {
        mStopSound();
        AudioClip sucp = mCtrl.GetClip("z_checkok" + Random.Range(0, 2));
        mCtrl.PlaySound(sucp, 1f);
    }
   
}
