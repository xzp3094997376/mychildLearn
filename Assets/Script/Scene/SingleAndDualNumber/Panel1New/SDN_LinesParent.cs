using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class SDN_LinesParent : MonoBehaviour
{

    public int nNumber = 0;
    public bool bFinish = false;
    public List<SDN_LinelineObj> mLinelineObjList = new List<SDN_LinelineObj>();

    private List<Image> mOKLineList = new List<Image>();

    private Image imgNum;
    private Image imgRang;

    private Image imgLine;
    private Image imgDrop;
    private Image imgStartPoint;

    private SingleAndDualNumCtrl mCtrl;

    public void InitAwake(int _num)
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as SingleAndDualNumCtrl;

        nNumber = _num;
        int count = nNumber / 2;

        List<float> findex = Common.GetOrderList(count, 120f);
        if (nNumber % 2 > 0)
        {
            findex = Common.GetOrderList(count +1, 150f);
        }
       
        int indexX = 0;       
        for (int i = 0; i < count; i++)
        {

            SDN_LinelineObj linelineCtrl0 = CreateLineline();
            linelineCtrl0.transform.localPosition = new Vector3(findex[i], 140f, 0f);
            mLinelineObjList.Add(linelineCtrl0);

            SDN_LinelineObj linelineCtrl1 = CreateLineline();
            linelineCtrl1.transform.localPosition = new Vector3(findex[i], -140f, 0f);
            mLinelineObjList.Add(linelineCtrl1);
            indexX++;
        }

        //单数处理
        if (nNumber % 2 > 0)
        {
            SDN_LinelineObj linelineCtrl0 = CreateLineline();
            linelineCtrl0.transform.localPosition = new Vector3(findex[indexX], 0f, 0f);
            mLinelineObjList.Add(linelineCtrl0);
        }

        Vector3 voffset = mLinelineObjList[mLinelineObjList.Count - 1].transform.localPosition;
        imgRang = UguiMaker.newImage("num", transform, "singledualnum_sprite", "rangwhite", false);
        imgRang.rectTransform.localPosition = voffset + new Vector3(190f, -200f, 0f);
        imgNum = UguiMaker.newImage("num", imgRang.transform, "singledualnum_sprite", _num.ToString(), false);
        imgNum.transform.localScale = Vector3.one * 0.5f;
        imgNum.color = new Color(247f / 255, 226f / 255, 61f / 255);
        if ((nNumber % 2 == 0) || (nNumber==1))
        {
            imgRang.rectTransform.localPosition = voffset + new Vector3(190f, 0f, 0f);
        }

        imgLine = UguiMaker.newGameObject("line", transform).AddComponent<Image>();
        imgLine.rectTransform.sizeDelta = new Vector2(10f, 10f);
        imgLine.transform.localPosition = Vector3.zero;
        imgDrop = UguiMaker.newImage("imgDrop", transform, "singledualnum_sprite", "rang1", false);
        imgDrop.rectTransform.sizeDelta = Vector2.one * 30f;
        imgStartPoint = UguiMaker.newImage("imgStartPoint", transform, "singledualnum_sprite", "rang1", false);
        imgStartPoint.rectTransform.sizeDelta = Vector2.one * 30f;

        imgLine.gameObject.SetActive(false);
        imgDrop.gameObject.SetActive(false);
        imgStartPoint.gameObject.SetActive(false);

        //编号化
        for (int i = 0; i < mLinelineObjList.Count; i++)
        {
            mLinelineObjList[i].nIndex = i;
            mLinelineObjList[i].SetUnLineUpIndexs(false);
        }
        if (nNumber % 2 > 0)
        {
            mLinelineObjList[mLinelineObjList.Count - 1].SetUnLineUpIndexs(true);
        }

        //GuideHand
        if (mLinelineObjList.Count >= 2)
        {
            mCtrl.GuideShow(transform, mLinelineObjList[0].transform.position, mLinelineObjList[1].transform.position);
        }
    }

    private SDN_LinelineObj CreateLineline()
    {
        GameObject mgo = UguiMaker.newGameObject("linelineObj", transform);
        SDN_LinelineObj linelineCtrl = mgo.AddComponent<SDN_LinelineObj>();
        linelineCtrl.InitAwake();
        return linelineCtrl;
    }


    /// <summary>
    /// 数字显示/隐藏
    /// </summary>
    /// <param name="_active"></param>
    public void NumberActive(bool _active)
    {
        imgRang.gameObject.SetActive(_active);
    }

    /// <summary>
    /// All lineObjs Box Hide
    /// </summary>
    public void LinesParentBoxHide()
    {
        for (int i = 0; i < mLinelineObjList.Count; i++)
        {
            mLinelineObjList[i].BoxActive(false);
        }
    }

    /// <summary>
    /// 全部连线完成检测
    /// </summary>
    public void CheckAllLineOver()
    {
        bool bAllOK = true;

        int checkLineCount = nNumber / 2;
        if (mOKLineList.Count >= checkLineCount)
        {
            bAllOK = true;
        }
        else
        {
            //Debug.Log("还差line");
            bAllOK = false;
        }

        if (bAllOK)
        {
            LinesParentBoxHide();

            bFinish = true;
            if (mLineOKCallback != null)
            {
                mLineOKCallback(this);
            }
        }
    }

    private SDN_LineOK_Delegate mLineOKCallback = null;
    public void SetLineOKCallBack(SDN_LineOK_Delegate m_lineOKCallback)
    {
        mLineOKCallback = m_lineOKCallback;
    }


    /// <summary>
    /// 指引
    /// </summary>
    public void SetGuideHand()
    {
        if (nNumber == 2)
        {
            //mLinelineObjList[0].CreateGuide();
        }
    }

    /// <summary>
    /// 创建一条已连好的线
    /// </summary>
    /// <param name="_v1">start</param>
    /// <param name="_v2">end</param>
    /// <returns></returns>
    public Image CreateOKLine(Vector3 _v1, Vector3 _v2)
    {
        Image iLine = UguiMaker.newGameObject("iline", transform).AddComponent<Image>();
        iLine.rectTransform.sizeDelta = new Vector2(10f, 10f);
        iLine.transform.localPosition = Vector3.zero;
        //长度
        float dis = Vector3.Distance(_v1, _v2);
        iLine.rectTransform.sizeDelta = new Vector2(dis, iLine.rectTransform.sizeDelta.y);
        iLine.rectTransform.anchoredPosition3D = (_v1 + _v2) * 0.5f;
        //旋转方向
        Vector3 dir = (_v2 - _v1).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(new Vector3(0f, 0f, targetAngle));
        iLine.rectTransform.localRotation = q;

        iLine.transform.SetSiblingIndex(0);
        return iLine;
    }




    SDN_LinelineObj mSelect = null;
    Vector3 vInput;
    Vector3 vStartDrop;
    void Update()
    {
        if (bFinish)
            return;
        //if (!bInit)
        //    return;

        if (Input.GetMouseButtonDown(0))
        {
            #region//stp1
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                SDN_LinelineObj com = hit.collider.gameObject.GetComponent<SDN_LinelineObj>();
                if (com != null)
                {
                    if (!com.bLineOK)
                    {
                        mSelect = com;

                        imgLine.rectTransform.sizeDelta = new Vector2(0, imgLine.rectTransform.sizeDelta.y);
                        imgDrop.transform.localPosition = com.transform.localPosition;
                        imgStartPoint.transform.localPosition = com.transform.localPosition;
                        imgLine.gameObject.SetActive(true);
                        imgDrop.gameObject.SetActive(true);
                        imgStartPoint.gameObject.SetActive(true);

                        RectTransform rt = mSelect.transform as RectTransform;
                        vStartDrop = rt.anchoredPosition3D;
                        mCtrl.PlayTheSortSound("dropline");

                        mSelect.transform.parent.SetSiblingIndex(20);

                        LineCtrol();
                        mCtrl.GuideHide();
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
                imgDrop.rectTransform.anchoredPosition3D = vInput;
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
                SDN_LinelineObj hitStation = null;
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hits != null)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        hitStation = hits[i].collider.gameObject.GetComponent<SDN_LinelineObj>();
                        if (hitStation != null && !hitStation.bLineOK)
                        {
                            if (hitStation != mSelect && hitStation.CheckCanLineUp(mSelect.nIndex))
                            {
                                bHitOK = true;
                                imgDrop.transform.localPosition = hitStation.transform.localPosition;
                                imgDrop.gameObject.SetActive(false);
                                LineCtrol();
                                mCtrl.MovePraSys(mSelect.transform.position, imgDrop.transform.position);
                                break;
                            }
                        }
                    }
                }
                
                if(bHitOK)
                {
                    mSelect.BoxActive(false);
                    if (hitStation != null)
                    {
                        hitStation.BoxActive(false);
                        //创建一条完成的线
                        Image okLine = CreateOKLine(mSelect.transform.localPosition, hitStation.transform.localPosition);
                        mOKLineList.Add(okLine);
                    }

                    imgLine.gameObject.SetActive(false);
                    imgDrop.gameObject.SetActive(false);
                    imgStartPoint.gameObject.SetActive(false);

                    CheckAllLineOver();
                }
                else
                {
                    imgDrop.transform.DOLocalMove(vStartDrop, 0.3f).OnUpdate(LineCtrol).OnComplete(() =>
                    {
                        imgLine.gameObject.SetActive(false);
                        imgDrop.gameObject.SetActive(false);
                        imgStartPoint.gameObject.SetActive(false);
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
        float dis = Vector3.Distance(vStartDrop, imgDrop.rectTransform.anchoredPosition3D);
        imgLine.rectTransform.sizeDelta = new Vector2(dis, imgLine.rectTransform.sizeDelta.y);
        imgLine.rectTransform.anchoredPosition3D = (vStartDrop + imgDrop.rectTransform.anchoredPosition3D) * 0.5f;
        //旋转方向
        Vector3 dir = (imgDrop.rectTransform.anchoredPosition3D - vStartDrop).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(new Vector3(0f, 0f, targetAngle));
        imgLine.rectTransform.localRotation = q;
    }





}
