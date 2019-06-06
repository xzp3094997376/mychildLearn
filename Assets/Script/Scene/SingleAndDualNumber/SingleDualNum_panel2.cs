using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class SingleDualNum_panel2 : MonoBehaviour
{
    private SingleAndDualNumCtrl mCtrl;

    private SingleDualNum_MiniMap miniMap;

    private SDN_NineBlockStation mStation0;
    private SDN_NineBlockStation mStation1;
    private SDN_NineBlockStation mStation2;
    private SDN_NineBlockStation mStation3;

    //private bool bCanDrawLine = false;

    /// <summary>
    /// 页面是否完成 if完成 则从新initdata
    /// </summary>
    public bool bPanelOK = true;
    //下一阶段 连线
    private bool bNextState = false;

    private Image mDrop;
    private Image lineTip;
    private SingleDualNum_tap tap0;
    private SingleDualNum_tap tap1;
    private SingleDualNum_tap tap2;
    private SingleDualNum_tap tap3;

    bool bIsSingleNum = false;
    int spineID = 1;
    public bool bTipSoundOver = false;


    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as SingleAndDualNumCtrl;
        miniMap = new SingleDualNum_MiniMap();

        mStation0 = transform.Find("station0").gameObject.AddComponent<SDN_NineBlockStation>();
        mStation0.InitAwake();
        mStation0.SetFinishDrawLine(DrawLineFinish);

        mStation1 = transform.Find("station1").gameObject.AddComponent<SDN_NineBlockStation>();
        mStation2 = transform.Find("station2").gameObject.AddComponent<SDN_NineBlockStation>();
        mStation3 = transform.Find("station3").gameObject.AddComponent<SDN_NineBlockStation>();
        mStation1.InitAwake();
        mStation2.InitAwake();
        mStation3.InitAwake();
        mStation1.transform.localScale = Vector3.one * 0.9f;
        mStation2.transform.localScale = Vector3.one * 0.9f;
        mStation3.transform.localScale = Vector3.one * 0.9f;

        mDrop = transform.Find("mDrop").GetComponent<Image>();
        mDrop.sprite = ResManager.GetSprite("singledualnum_sprite", "rang1");
        mDrop.gameObject.SetActive(false);

        lineTip = transform.Find("lineTip").GetComponent<Image>();
        lineTip.gameObject.SetActive(false);

        tap0 = transform.Find("tap0").gameObject.AddComponent<SingleDualNum_tap>();
        tap1 = transform.Find("tap1").gameObject.AddComponent<SingleDualNum_tap>();
        tap2 = transform.Find("tap2").gameObject.AddComponent<SingleDualNum_tap>();
        tap3 = transform.Find("tap3").gameObject.AddComponent<SingleDualNum_tap>();
        tap0.InitAwake(mStation0);
        tap1.InitAwake(mStation1);
        tap2.InitAwake(mStation2);
        tap3.InitAwake(mStation3);
        tap0.gameObject.SetActive(false);
        tap1.gameObject.SetActive(false);
        tap2.gameObject.SetActive(false);
        tap3.gameObject.SetActive(false);

        bPanelOK = true;
    }

   
    SingleDualNum_tap mSelect;
    Vector3 vInput;
    Vector3 vStartDrop;
    void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    InitData();
        //}

        //if (!bTipSoundOver)
        //    return;
        //if (!bCanDrawLine)
        //    return;

        //LineCtrol();

        if (Input.GetMouseButtonDown(0))
        {
            #region//stp1
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hits != null)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    SingleDualNum_tap com = hits[i].collider.gameObject.GetComponent<SingleDualNum_tap>();
                    if (com != null)
                    {
                        mSelect = com;
                        lineTip.gameObject.SetActive(true);
                        lineTip.rectTransform.sizeDelta = new Vector2(0, lineTip.rectTransform.sizeDelta.y);
                        mDrop.transform.localPosition = com.transform.localPosition;
                        mDrop.gameObject.SetActive(true);

                        RectTransform rt = mSelect.transform as RectTransform;
                        vStartDrop = rt.anchoredPosition3D;
                        mCtrl.PlayTheSortSound("dropline");
                        StopPlayTipSound();
                        break;
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

                LineCtrol();
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//stp3
            if (mSelect != null)
            {
                bool bHitOK = false;
                SingleDualNum_tap hitStation = null;
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hits != null)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        hitStation = hits[i].collider.gameObject.GetComponent<SingleDualNum_tap>();
                        if (hitStation != null && hitStation != mSelect)
                        {
                            if (hitStation.mStation.nMapID == mSelect.mStation.nMapID)
                            {
                                bHitOK = true;
                                mDrop.transform.localPosition = hitStation.transform.localPosition;
                                mDrop.gameObject.SetActive(false);
                                LineCtrol();
                                //bCanDrawLine = false;

                                mCtrl.MovePraSys(mSelect.transform.position, mDrop.transform.position);
                                Panel2Finish();
                                break;
                            }                          
                        }
                    }
                }

                if (!bHitOK)
                {
                    mDrop.transform.DOLocalMove(vStartDrop, 0.3f).OnUpdate(LineCtrol).OnComplete(() => 
                    {
                        lineTip.gameObject.SetActive(false);
                        mDrop.gameObject.SetActive(false);
                    });
                    mCtrl.PlayTheSortSound("lineback");           
                }

                mSelect = null;
            }
            #endregion          
        }      
    }
    private void LineCtrol()
    {
        //长度
        float dis = Vector3.Distance(vStartDrop, mDrop.rectTransform.anchoredPosition3D);
        lineTip.rectTransform.sizeDelta = new Vector2(dis, lineTip.rectTransform.sizeDelta.y);
        lineTip.rectTransform.anchoredPosition3D = (vStartDrop + mDrop.rectTransform.anchoredPosition3D) * 0.5f;
        //旋转方向
        Vector3 dir = (mDrop.rectTransform.anchoredPosition3D - vStartDrop).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(new Vector3(0f, 0f, targetAngle));
        lineTip.rectTransform.localRotation = q;
    }


    public void ReplayReset()
    {
        bPanelOK = true;
        ResetInfos();
    }

    public void ResetInfos()
    {
        //bCanDrawLine = false;
        lineTip.gameObject.SetActive(false);
        lineTip.rectTransform.sizeDelta = new Vector2(0, lineTip.rectTransform.sizeDelta.y);
        mDrop.gameObject.SetActive(false);

        tap0.gameObject.SetActive(false);
        tap1.gameObject.SetActive(false);
        tap2.gameObject.SetActive(false);
        tap3.gameObject.SetActive(false);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void InitData()
    {
        if (bPanelOK)
            return;

        bTipSoundOver = false;
        ResetInfos();
        bNextState = false;

        mStation0.transform.localScale = Vector3.one * 1.8f;
        mStation0.transform.localPosition = new Vector3(0f, 20f, 0f);

        mStation1.transform.localPosition = mStation1.vStart + new Vector3(-1300f, 0f, 0f);
        mStation2.transform.localPosition = mStation2.vStart + new Vector3(-1300f, 0f, 0f);
        mStation3.transform.localPosition = mStation3.vStart + new Vector3(-1300f, 0f, 0f);

        List<int> dadatalist = new List<int>() { 1, 3, 5, 7, 9 };
        bIsSingleNum = true;
        if (Random.value >0.5f)
        {
            dadatalist = new List<int>() { 2, 4, 6, 8, 10 };
            bIsSingleNum = false;
        }

        spineID = Random.Range(1, 4);

        //随机出3个地图
        List<int> mapIDList = Common.GetIDList(1, 10, 3, -1);
        //设置1
        List<int> getMap = miniMap.GetMap(mapIDList[0]);
        mStation0.nMapID = mapIDList[0];
        mStation0.SetData(spineID, getMap, dadatalist);
        //打乱
        List<int> mapIDBrakeList = Common.BreakRank(mapIDList);
        //设置2
        List<int> getMap1 = miniMap.GetMap(mapIDBrakeList[0]);
        mStation1.nMapID = mapIDBrakeList[0];
        mStation1.SetData(Random.Range(1,4), getMap1, dadatalist);
        mStation1.ToDrawLines();
        List<int> getMap2 = miniMap.GetMap(mapIDBrakeList[1]);
        mStation2.nMapID = mapIDBrakeList[1];
        mStation2.SetData(Random.Range(1, 4), getMap2, dadatalist);
        mStation2.ToDrawLines();
        List<int> getMap3 = miniMap.GetMap(mapIDBrakeList[2]);
        mStation3.nMapID = mapIDBrakeList[2];
        mStation3.SetData(Random.Range(1, 4), getMap3, dadatalist);
        mStation3.ToDrawLines();

        mCtrl.PlayTipSound();

        mCtrl.bButtonActive = true;
        bTipSoundOver = true;
    }
    /// <summary>
    /// 播放提示1
    /// </summary>
    public void PlayTipSoundPanel2()
    {
        mCtrl.SetTipSound(IEPlayTipSound2());
    }
    /// <summary>
    /// 停止播放提示
    /// </summary>
    public void StopPlayTipSound()
    {
        mCtrl.StopTipSound();
    }
    IEnumerator IEPlayTipSound2()
    {
        List<AudioClip> cplist = new List<AudioClip>();
        if (!bNextState)
        {
            if (bIsSingleNum)
            {
                cplist.Add(mCtrl.GetClip("game-tips2-4-4"));
            }
            else
            {
                cplist.Add(mCtrl.GetClip("game-tips2-4-5"));
            }
            //sound spine type
            cplist.Add(mCtrl.GetClip("game-tips2-4-" + (5 + spineID).ToString()));
            //sound lineup
            cplist.Add(mCtrl.GetClip("game-tips2-4-9"));
            for (int i = 0; i < cplist.Count; i++)
            {
                float fdetime = cplist[i].length;
                mCtrl.PlaySound(cplist[i], 1f);
                yield return new WaitForSeconds(fdetime);
            }
        }
        else
        {
            AudioClip soundTip0 = mCtrl.GetClip("game-tips2-4-10");
            mCtrl.PlaySound(soundTip0, 1f);
            yield return new WaitForSeconds(soundTip0.length);
        }
         
    }



    /// <summary>
    /// 画线完成回调
    /// </summary>
    private void DrawLineFinish()
    {
        //Debug.Log("draw line finish");
        mCtrl.bButtonActive = false;
        mCtrl.StartCoroutine(IEPanelWaite());
    }
    IEnumerator IEPanelWaite()
    {
        bNextState = true;
        AudioClip soundTip0 = mCtrl.GetClip("game-tips2-4-10");
        mCtrl.PlaySound(soundTip0, 1f);

        yield return new WaitForSeconds(1f);
        mStation0.transform.DOLocalMove(new Vector3(0f, 200f, 0f), 0.5f);
        mStation0.transform.DOScale(Vector3.one * 0.9f, 0.5f);

        mStation1.MoveToStart(0.5f);
        mStation2.MoveToStart(0.5f);
        mStation3.MoveToStart(0.5f);

        yield return new WaitForSeconds(0.6f);
        tap0.ShowOut();
        tap1.ShowOut();
        tap2.ShowOut();
        tap3.ShowOut();

        mCtrl.bButtonActive = true;
        yield return new WaitForSeconds(4f);
        //bCanDrawLine = true;
        mCtrl.bButtonActive = true;
    }

    /// <summary>
    /// 页面完成to panel3
    /// </summary>
    public void Panel2Finish()
    {
        mCtrl.PlayTheSortSound("linesuc");
        bPanelOK = true;
        mCtrl.bButtonActive = false;
        mCtrl.StartCoroutine(IEGoToPanel3());
    }
    IEnumerator IEGoToPanel3()
    {
        yield return new WaitForSeconds(2f);
        mCtrl.ChangePanel(2, 3);
        mCtrl.bButtonActive = true;
    }


    public void SceneMove(bool _in, float _time)
    {
        if (_in)
        {
            gameObject.SetActive(true);
            if (bPanelOK)
            {
                bPanelOK = false;
                InitData();
            }
            transform.localPosition = new Vector3(0f, 900f, 0f);
            transform.DOLocalMove(Vector3.zero, _time);

            mCtrl.BtnsMove(_in, _time);
        }
        else
        {
            transform.DOLocalMove(new Vector3(0f, 900f, 0f), _time).OnComplete(()=> 
            {
                gameObject.SetActive(false);
            });
        }      
    }


    public void ChangePanelResetInfos()
    {
        StopPlayTipSound();
    }

}
