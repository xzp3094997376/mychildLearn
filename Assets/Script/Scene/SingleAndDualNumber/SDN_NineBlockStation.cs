using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class SDN_NineBlockStation : MonoBehaviour
{

    public int nMapID = 0;

    private List<int> mapList = new List<int>();
    private List<SDN_NineBlockNum> numberList = new List<SDN_NineBlockNum>();

    private Image rangSp;
    private Image mDrop;
    private Image lineTip;
    private bool bInit = false;

    [HideInInspector]
    public Vector3 vStart;

    private SingleAndDualNumCtrl mmCtrl;

    private System.Action mDrawLineCallback = null;
    /// <summary>
    /// 设置划线完成回调
    /// </summary>
    /// <param name="_callback"></param>
    public void SetFinishDrawLine(System.Action _callback)
    {
        mDrawLineCallback = _callback;
    }

    public void InitAwake()
    {
        mmCtrl = SceneMgr.Instance.GetNowScene() as SingleAndDualNumCtrl;
        vStart = transform.localPosition;

        for (int i=0;i<9;i++)
        {
            Transform tra = transform.Find("Image" + i);
            SDN_NineBlockNum numObj = tra.gameObject.AddComponent<SDN_NineBlockNum>();
            numObj.InitAwake();
            numberList.Add(numObj);
        }

        rangSp = transform.Find("rangSp").GetComponent<Image>();
        rangSp.sprite = ResManager.GetSprite("singledualnum_sprite", "rang1");

        mDrop = transform.Find("mDrop").GetComponent<Image>();
        mDrop.sprite = ResManager.GetSprite("singledualnum_sprite", "mbtn1");
        mDrop.gameObject.SetActive(false);
        mDrop.enabled = false;

        lineTip = transform.Find("lineTip").GetComponent<Image>();
        lineTip.gameObject.SetActive(false);

        bInit = true;
    }

    /// <summary>
    /// 设置地图
    /// </summary>
    /// <param name="map"></param>
    public void SetData(int _spineID ,List<int> map, List<int> _numlist)
    {
        ResetInfos();

        CreateSpine(_spineID);

        mapList = map;

        List<int> numData = _numlist;//new List<int>() { 1, 3, 5, 7, 9 };
        for (int i=0;i<map.Count;i++)
        {
            int index = map[i];
            numberList[index].SetNumber(numData[i]);
        }
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void ResetInfos()
    {
        for (int i = 0; i < numberList.Count; i++)
        {
            numberList[i].ResetInfos();
        }
        for (int i = 0; i < linelineList.Count; i++)
        {
            if (linelineList[i].gameObject != null)
                GameObject.Destroy(linelineList[i].gameObject);
        }
        linelineList.Clear();
        lineOK = false;
        nCountLine = 0;
        if (spineObj != null)
        { GameObject.Destroy(spineObj); }
    }


    private SDN_NineBlockNum mSelect;
    private Vector3 vInput;
    void Update ()
    {
        if (!bInit)
            return;

        if (lineOK)
            return;

        if (mapList.Count <= 0)
            return;

        //if (!mmCtrl.panel2.bTipSoundOver)
        //    return;

        if (Input.GetMouseButtonDown(0))
        {
            #region//stp1
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hits != null)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    SDN_NineBlockNum com = hits[i].collider.gameObject.GetComponent<SDN_NineBlockNum>();
                    if (com != null && com == numberList[mapList[0]])
                    {
                        mSelect = com;
                        lineTip.gameObject.SetActive(true);
                        lineTip.rectTransform.sizeDelta = new Vector2(0, lineTip.rectTransform.sizeDelta.y);
                        mDrop.transform.localPosition = com.transform.localPosition;
                        mDrop.gameObject.SetActive(true);

                        mmCtrl.PlayTheSortSound("dropline");
                        mmCtrl.panel2.StopPlayTipSound();
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
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//stp3
            if (mSelect != null)
            {
                lineTip.gameObject.SetActive(false);
                mDrop.gameObject.SetActive(false);
                mSelect = null;
            }
            #endregion          
        }

        if (mapList.Count > 0)
            LineCtrol();
    }
    private void LineCtrol()
    {
        SDN_NineBlockNum startNumber = numberList[mapList[0]];
        //长度
        float dis = Vector3.Distance(startNumber.rectTransform.anchoredPosition3D, mDrop.rectTransform.anchoredPosition3D);
        lineTip.rectTransform.sizeDelta = new Vector2(dis, lineTip.rectTransform.sizeDelta.y);
        lineTip.rectTransform.anchoredPosition3D = (startNumber.rectTransform.anchoredPosition3D + mDrop.rectTransform.anchoredPosition3D) * 0.5f;
        //旋转方向
        Vector3 dir = (mDrop.rectTransform.anchoredPosition3D - startNumber.rectTransform.anchoredPosition3D).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(new Vector3(0f, 0f, targetAngle));
        lineTip.rectTransform.localRotation = q;
    }


    public bool lineOK = false;
    int nCountLine = 0;
    /// <summary>
    /// 碰撞检测划线
    /// </summary>
    public void HitCheck(SDN_NineBlockNum _numCtrl)
    {
        if (mapList.Count <= 0)
            return;
        if (nCountLine > 4)
            return;
        if (_numCtrl == numberList[mapList[1]])
        {
            CreateLine(numberList[mapList[0]].rectTransform.anchoredPosition3D, _numCtrl.rectTransform.anchoredPosition3D);
            mapList.RemoveAt(0);
            nCountLine++;
            mmCtrl.PlayTheSortSound("linesuc");
            if (nCountLine >= 4)
            {
                lineOK = true;
                lineTip.gameObject.SetActive(false);
                mDrop.gameObject.SetActive(false);
                if (mDrawLineCallback != null)
                    mDrawLineCallback();
            }
        }
    }


    private List<Image> linelineList = new List<Image>();
    /// <summary>
    /// 生成线条
    /// </summary>
    private Image CreateLine(Vector3 _from,Vector3 _to)
    {
        Image newLine = GameObject.Instantiate(lineTip) as Image;
        newLine.gameObject.SetActive(true);
        newLine.gameObject.name = "lineline";
        newLine.transform.SetParent(transform);
        newLine.transform.localPosition = Vector3.zero;
        newLine.transform.SetSiblingIndex(3);
        newLine.transform.localScale = Vector3.one;

        //长度
        float dis = Vector3.Distance(_from, _to);
        newLine.rectTransform.sizeDelta = new Vector2(dis, newLine.rectTransform.sizeDelta.y);
        newLine.rectTransform.anchoredPosition3D = (_from + _to) * 0.5f;
        //旋转方向
        Vector3 dir = (_to - _from).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(new Vector3(0f, 0f, targetAngle));
        newLine.rectTransform.localRotation = q;

        linelineList.Add(newLine);
        return newLine;
    }

    /// <summary>
    /// 全部画好线
    /// </summary>
    public void ToDrawLines()
    {
        lineOK = true;
        //隐藏全部数字
        for (int i = 0; i < numberList.Count; i++)
        {
            numberList[i].HideNumImage();
        }
        //画线
        for (int i = 0; i < 4; i++)
        {
            Vector3 vfrom = numberList[mapList[i]].rectTransform.anchoredPosition3D;
            Vector3 vTo = numberList[mapList[i+1]].rectTransform.anchoredPosition3D;
            CreateLine(vfrom, vTo);
        }
    }

    /// <summary>
    /// 返回原位置
    /// </summary>
    /// <param name="_time"></param>
    public void MoveToStart(float _time)
    {
        transform.DOLocalMove(vStart, _time);
    }


    private GameObject spineObj;
    public void CreateSpine(int _spine)
    {
        int type = _spine;//Random.Range(1, 4);
        spineObj = ResManager.GetPrefab("singledualnum_prefab", "spine" + type);
        spineObj.transform.SetParent(transform);
        spineObj.transform.SetSiblingIndex(0);
        spineObj.transform.localPosition = Vector3.zero;
        spineObj.transform.localScale = Vector3.one;
    }

}
