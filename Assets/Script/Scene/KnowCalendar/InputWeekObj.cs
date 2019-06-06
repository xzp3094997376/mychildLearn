using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// 星期输入框
/// </summary>
public class InputWeekObj : MonoBehaviour
{

    public string strInputNum = "";
    public int nInputNum = 0;

    /// <summary>
    /// 输入框信息
    /// </summary>
    public InputInfoData mInputInfoData = new InputInfoData();

    private bool bProtect = false;

    private Image imgBG;
    List<Image> blockList = new List<Image>();
    private BoxCollider2D mbox2d;

    private bool bInit = false;
    public void InitAwake()
    {
        transform.localScale = Vector3.one * mInputInfoData.fscale;

        imgBG = gameObject.GetComponent<Image>();
        imgBG.sprite = ResManager.GetSprite(mInputInfoData.strAlatsName, mInputInfoData.strPicBG);
        imgBG.color = mInputInfoData.bgcolor;

        mbox2d = gameObject.AddComponent<BoxCollider2D>();
        mbox2d.size = imgBG.rectTransform.sizeDelta;
        mbox2d.isTrigger = true;

        for (int i = 0; i < 8; i++)
        {
            Image bl = transform.Find("block" + i).GetComponent<Image>();
            bl.sprite = ResManager.GetSprite(mInputInfoData.strAlatsName, mInputInfoData.strPicBG);
            bl.color = mInputInfoData.color_blockBG;
            Image txt = bl.transform.Find("txt").GetComponent<Image>();
            txt.sprite = ResManager.GetSprite(mInputInfoData.strAlatsName, mInputInfoData.strCellPicFirstName + i);
            txt.color = mInputInfoData.color_blockNum;

            EventTriggerListener.Get(bl.gameObject).onDown = BlockClickDown;
            EventTriggerListener.Get(bl.gameObject).onUp = BlockClickUp;

            blockList.Add(bl);
        }

        bInit = true;
    }


    private System.Action mInpuFinishCallBack = null;
    public void SetFinishInputWeekCallBack(System.Action _action)
    {
        mInpuFinishCallBack = _action;
    }
    private System.Action mInputWeekCallBack = null;
    public void SetInputWeekCallBack(System.Action _action)
    {
        mInputWeekCallBack = _action;
    }


    private void BlockClickDown(GameObject _go)
    {
        if (bProtect)
            return;
        Image imgobj = _go.GetComponent<Image>();
        Image imgtxt = _go.transform.Find("txt").GetComponent<Image>();

        if (_go.name.Contains("7"))
        {
            if (strInputNum.CompareTo("") == 0)
                return;
            imgobj.color = mInputInfoData.color_blockNum;
            imgtxt.color = mInputInfoData.color_blockBG;
        }
        else
        {
            imgobj.color = mInputInfoData.color_blockNum;
            imgtxt.color = mInputInfoData.color_blockBG;
        }
        PlayResSound("sound/inputnumclick");
    }
    private void BlockClickUp(GameObject _go)
    {
        if (bProtect)
            return;
        Image imgobj = _go.GetComponent<Image>();
        Image imgtxt = _go.transform.Find("txt").GetComponent<Image>();

        if (!_go.name.Contains("7"))
        {
            imgobj.color = mInputInfoData.color_blockBG;
            imgtxt.color = mInputInfoData.color_blockNum;

            strInputNum = GetStringID(_go.name);

            blockList[7].color = mInputInfoData.color_blockSureBG;
            blockList[7].transform.Find("txt").GetComponent<Image>().color = mInputInfoData.color_blockNum;

            if (mInputWeekCallBack != null)
                mInputWeekCallBack();
        }
        else
        {
            if (strInputNum.CompareTo("") == 0)
                return;

            imgobj.color = mInputInfoData.color_blockBG;
            imgtxt.color = mInputInfoData.color_blockNum;

            if (int.TryParse(strInputNum, out nInputNum))
            { }
            else
            {
                nInputNum = 0;
            }

            if (strInputNum.CompareTo("") != 0 && _go.name.Contains("7"))
            {
                SetFinishInput();
            }
        }
    }

    private void SetFinishInput()
    {
        HideEffect();
        if (int.TryParse(strInputNum, out nInputNum))
        { }
        else
        {
            nInputNum = 0;
        }
        if (mInpuFinishCallBack != null)
            mInpuFinishCallBack();
    }


    string GetStringID(string _str)
    {
        if (_str.Contains("0"))
            return "0";
        if (_str.Contains("1"))
            return "1";
        if (_str.Contains("2"))
            return "2";
        if (_str.Contains("3"))
            return "3";
        if (_str.Contains("4"))
            return "4";
        if (_str.Contains("5"))
            return "5";
        if (_str.Contains("6"))
            return "6";

        return "";
    }

    public void ResetInfos()
    {
        strInputNum = "";
        nInputNum = 0;
        blockList[7].color = mInputInfoData.color_blockBG;
        blockList[7].transform.Find("txt").GetComponent<Image>().color = mInputInfoData.color_blockSureStart;
    }

    /// <summary>
    /// 出现效果
    /// </summary>
    public void ShowEffect()
    {
        if (gameObject.activeSelf)
            return;
        bProtect = false;
        PlayResSound("sound/button_down");
        ResetInfos();

        gameObject.SetActive(true);
        strInputNum = "";
        nInputNum = 0;
        transform.localScale = Vector3.one * 0.001f;
        transform.DOScale(Vector3.one * mInputInfoData.fscale, 0.3f);
    }
    /// <summary>
    /// 隐藏效果
    /// </summary>
    public void HideEffect()
    {
        bProtect = true;
        PlayResSound("sound/button_up");
        transform.DOScale(Vector3.one * 0.001f, 0.3f).OnComplete(() =>
        {
            bProtect = false;
            gameObject.SetActive(false); });
    }


    private void Update()
    {
        if (!bInit)
            return;
        if (bProtect)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit2D)
            {
                if (hit2D.collider.gameObject != gameObject)
                {
                    HideEffect();
                    SetFinishInput();
                }
            }
            else
            {
                HideEffect();
                SetFinishInput();
            }
        }
    }


    private void PlayResSound(string _namePath)
    {
        AudioClip cp0 = Resources.Load<AudioClip>(_namePath);
        if (cp0 != null)
        {
            AudioSource.PlayClipAtPoint(cp0, Camera.main.transform.position);
        }
    }

}
