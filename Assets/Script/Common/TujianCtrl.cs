using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// 图鉴
/// </summary>
public class TujianCtrl : MonoBehaviour
{

    private Button closebtn;

    private GameObject btnbase;

    private RectTransform IconsObj;
    private Scrollbar mScrollbar;
    private ScrollRect mScrollRect;
    private GridLayoutGroup mGrid;

    private RectTransform TipsObj;
    private Text tipText;
    private Image tipIcon;

    public List<int> mTujianIDList = new List<int>();
    private List<TujianIcon> mTujianDataList = new List<TujianIcon>();

    TujianIcon nowTujianIcon = null;

    public bool bTujianState = false;

    public static TujianCtrl Create(Transform _parent)
    {
        GameObject mload = Resources.Load("prefab/Tujian/Tujian") as GameObject;
        GameObject obj = GameObject.Instantiate(mload);
        obj.transform.SetParent(_parent);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        TujianCtrl result = obj.AddComponent<TujianCtrl>();
        result.InitAwake();
        return result;
    }
    private void InitAwake()
    {
        closebtn = transform.Find("closebtn").GetComponent<Button>();
        EventTriggerListener.Get(closebtn.gameObject).onClick = CloseBtnClick;

        btnbase = transform.Find("btnbase").gameObject;
        btnbase.SetActive(false);

        IconsObj = transform.Find("IconsObj").GetComponent<RectTransform>();
        mScrollbar = IconsObj.transform.Find("Scrollbar").GetComponent<Scrollbar>();
        mScrollRect = IconsObj.transform.Find("ScrollRect").GetComponent<ScrollRect>();
        mGrid = mScrollRect.transform.Find("Grid").GetComponent<GridLayoutGroup>();

        TipsObj = transform.Find("TipsObj").GetComponent<RectTransform>();
        tipText = TipsObj.Find("tipText").GetComponent<Text>();
        tipIcon = TipsObj.Find("tipIcon").GetComponent<Image>();

        gameObject.SetActive(false);
    }


    public void SetData(List<int> _tujianIDList)
    {
        float fHeight = mGrid.spacing.y;

        mTujianIDList = _tujianIDList;
        for (int i = 0; i < mTujianIDList.Count; i++)
        {
            int nkey = mTujianIDList[i];
            config_tujian tujiandata = FormManager.config_tujians[nkey];
            
            GameObject mgo = GameObject.Instantiate(btnbase) as GameObject;
            mgo.SetActive(true);
            mgo.transform.SetParent(mGrid.transform);
            mgo.transform.localPosition = Vector3.zero;
            mgo.transform.localScale = Vector3.one;
            TujianIcon icon = mgo.AddComponent<TujianIcon>();
            icon.InitAwake(this, tujiandata);
            mTujianDataList.Add(icon);

            fHeight += mGrid.cellSize.y;
            fHeight += mGrid.spacing.y;
        }
        mGrid.cellSize = new Vector2(150f, 100f);
        RectTransform rectTGrid = mGrid.transform as RectTransform;     
        rectTGrid.sizeDelta = new Vector2(mGrid.cellSize.x + mGrid.spacing.x, fHeight);
        rectTGrid.anchoredPosition = new Vector2(0f, -rectTGrid.sizeDelta.y * 0.5f);

        if (mTujianDataList.Count > 0)
        {
            nowTujianIcon = mTujianDataList[0];
            tipText.text = nowTujianIcon.mData.m_strTxt;
            tipIcon.sprite = nowTujianIcon.img.sprite;
        }
        mScrollbar.value = 1;
    }




    public void ShowTujian()
    {
        bTujianState = true;
        gameObject.SetActive(true);
        transform.localPosition = new Vector3(-1500f, 0f, 0f);
        transform.DOLocalMove(Vector3.zero, 0.5f);
    }
    public void HideTujian()
    {
        transform.DOLocalMove(new Vector3(1500f, 0f, 0f), 0.5f).OnComplete(()=> 
        {
            gameObject.SetActive(false);
            bTujianState = false;
        });
    }




    private void CloseBtnClick(GameObject _go)
    {
        HideTujian();
    }

    public void IconClick(GameObject _go)
    {
        TujianIcon theicon = _go.GetComponent<TujianIcon>();
        if (theicon != null && theicon != nowTujianIcon)
        {
            nowTujianIcon = theicon;
            tipText.text = nowTujianIcon.mData.m_strTxt;
            tipIcon.sprite = nowTujianIcon.img.sprite;
        }
    }
	
}


public class TujianIcon : MonoBehaviour
{
    public int nID = 0;
    public config_tujian mData;

    private Button btn;
    public Image img;
    public Text txt;
    private TujianCtrl mTujianCtrl;

    public void InitAwake(TujianCtrl _mctrl, config_tujian _data)
    {
        mTujianCtrl = _mctrl;
        mData = _data;

        btn = transform.GetComponent<Button>();
        EventTriggerListener.Get(gameObject).onClick = mTujianCtrl.IconClick;

        img = transform.GetComponent<Image>();
        txt = transform.Find("Text").GetComponent<Text>();
        txt.text = mData.m_strName;
    }


}
