using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// 计算机式输入框
/// </summary>
public class InputNumObj : MonoBehaviour
{
    /// <summary>
    /// 输入的数字文本
    /// </summary>
    public string strInputNum = "";
    /// <summary>
    /// 输入确认的数字
    /// </summary>
    public int nInputNum = 0;
    /// <summary>
    /// 取位数限制 -1无限制
    /// </summary>
    public int nCountLimit = 1;

    private bool bInit = false;
    private List<InputNumBlockObj> mBlocksList = new List<InputNumBlockObj>();
    
    /// <summary>
    /// 输入框信息
    /// </summary>
    public InputInfoData mInputInfoData = new InputInfoData();

    /// <summary>
    /// 取消ID
    /// </summary>
    private const int nResetID = 10;
    /// <summary>
    /// 确认ID
    /// </summary>
    private const int nSureID = 11;

    private System.Action mInpuFinishCallBack = null;
    public void SetFinishInputCallBack(System.Action _action)
    {
        mInpuFinishCallBack = _action;
    }
    private System.Action mInputNumCallBack = null;
    public void SetInputNumberCallBack(System.Action _action)
    {
        mInputNumCallBack = _action;
    }
    private System.Action mClearInputCallback = null;
    public void SetClearNumberCallBack(System.Action _action)
    {
        mClearInputCallback = _action;
    }

    private bool bProtect = false;

    public static InputNumObj Create(Transform _parent, InputInfoData _infoData)
    {
        GameObject obj = UguiMaker.newGameObject("inputNumObj", _parent);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        InputNumObj result = obj.AddComponent<InputNumObj>();
        //AudioClip cp0 = Resources.Load<AudioClip>("sound/button_down");
        //if (cp0 != null)
        //{
        //    AudioSource.PlayClipAtPoint(cp0, Camera.main.transform.position);
        //}
        result.InitData(_infoData);
        return result;
    }
    private void InitData(InputInfoData _infoData)
    {
        mInputInfoData = _infoData;
        transform.localScale = Vector3.one * mInputInfoData.fscale;

        Image bg = gameObject.AddComponent<Image>();
        bg.type = Image.Type.Sliced;
        bg.rectTransform.sizeDelta = mInputInfoData.vBgSize;
        bg.sprite = ResManager.GetSprite(mInputInfoData.strAlatsName, mInputInfoData.strPicBG);
        bg.color = mInputInfoData.bgcolor;

        BoxCollider2D box2d = gameObject.AddComponent<BoxCollider2D>();
        box2d.size = bg.rectTransform.sizeDelta;

        GridLayoutGroup grid = gameObject.AddComponent<GridLayoutGroup>();
        grid.cellSize = mInputInfoData.vCellSize;
        grid.childAlignment = TextAnchor.MiddleCenter;
        grid.constraintCount = mInputInfoData.nConstraintCount;
        grid.spacing = mInputInfoData.vSpacing;

        int ncreateCount = 3 * mInputInfoData.nConstraintCount;
        List<int> theIDList = mInputInfoData.mBlockIDList;
        for (int i = 0; i < theIDList.Count; i++)
        {
            if (i <= ncreateCount - 1)
            {
                int getID = theIDList[i];
                mBlocksList.Add(CreateBlock(getID));
            }
        }
        //mBlocksList.Add(CreateBlock(7));
        //mBlocksList.Add(CreateBlock(8));
        //mBlocksList.Add(CreateBlock(9));
        //mBlocksList.Add(CreateBlock(4));
        //mBlocksList.Add(CreateBlock(5));
        //mBlocksList.Add(CreateBlock(6));
        //mBlocksList.Add(CreateBlock(1));
        //mBlocksList.Add(CreateBlock(2));
        //mBlocksList.Add(CreateBlock(3));
        //if(mInputInfoData.nConstraintCount == 4)
        //{
        //    //取消键
        //    mBlocksList.Add(CreateBlock(nResetID));
        //    mBlocksList.Add(CreateBlock(0));
        //    //确认键
        //    mBlocksList.Add(CreateBlock(nSureID));
        //    mBlocksList[11].SetTipColor(mInputInfoData.color_blockSureStart);
        //}
        bInit = true;
    }

    private InputNumBlockObj CreateBlock(int _index)
    {
        GameObject gObj = UguiMaker.newGameObject("block" + _index, transform);
        InputNumBlockObj objCtrl = gObj.AddComponent<InputNumBlockObj>();
        objCtrl.InitAwake(_index, mInputInfoData);

        objCtrl.SetBGColor(mInputInfoData.color_blockBG);
        objCtrl.SetTipColor(mInputInfoData.color_blockNum);

        Button btn = objCtrl.gameObject.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        EventTriggerListener.Get(btn.gameObject).onDown = BlockDown;
        EventTriggerListener.Get(btn.gameObject).onUp = BlockUp;
        return objCtrl;
    }

    public InputNumBlockObj GetInputNumBlockByIndex(int _index)
    {
        if (mBlocksList.Count - 1 >= _index)
        {
            return mBlocksList[_index];
        }
        return null;
    }


    private void BlockDown(GameObject _go)
    {
        if (bProtect)
            return;
        InputNumBlockObj getobj = _go.GetComponent<InputNumBlockObj>();
        if (getobj == null)
            return;
        //取消键
        if (getobj.nID == nResetID)
        {
            if (strInputNum.CompareTo("") == 0)
                return;
            getobj.SetBGColor(mInputInfoData.color_blockNum);
            getobj.SetTipColor(mInputInfoData.color_blockBG);
        }
        else
        {
            getobj.SetBGColor(mInputInfoData.color_blockNum);
            getobj.SetTipColor(mInputInfoData.color_blockBG);
        }

        AudioClip cp0 = Resources.Load<AudioClip>("sound/inputnumclick");
        if (cp0 != null)
        {
            AudioSource.PlayClipAtPoint(cp0, Camera.main.transform.position);
        }
    }
    private void BlockUp(GameObject _go)
    {
        if (bProtect)
            return;
        InputNumBlockObj getobj = _go.GetComponent<InputNumBlockObj>();
        if (getobj == null)
            return;
   
        if (getobj.nID != nSureID)
        {
            getobj.SetBGColor(mInputInfoData.color_blockBG);
            getobj.SetTipColor(mInputInfoData.color_blockNum);
        }
        //0 - 9
        if (getobj.nID <= 9 && getobj.nID >= 0)
        {
            if (strInputNum.Length >= nCountLimit && nCountLimit != -1)
            {
                strInputNum = "";
            }
            string gettxt = strInputNum + getobj.nID;
            if (nCountLimit != -1)
            {
                int getcount = nCountLimit;
                getcount = Mathf.Clamp(getcount, 1, gettxt.Length);
                strInputNum = gettxt.Substring(0, getcount);
            }
            else
            {
                strInputNum = gettxt;
            }

            //确认键颜色
            if(mBlocksList.Count -1 == 11)
            {
                mBlocksList[11].SetTipColor(mInputInfoData.color_blockNum);
                mBlocksList[11].SetBGColor(mInputInfoData.color_blockSureBG);
            }
            
            if (mInputNumCallBack != null)
                mInputNumCallBack();
        }
        else if (getobj.nID == nResetID) //取消键
        {
            strInputNum = "";
            nInputNum = 0;
            mBlocksList[11].SetTipColor(mInputInfoData.color_blockSureStart);
            mBlocksList[11].SetBGColor(mInputInfoData.color_blockBG);
            if (mClearInputCallback != null)
            { mClearInputCallback(); }
        }
        else if(getobj.nID == nSureID) //确认键
        {
            if (int.TryParse(strInputNum, out nInputNum))
            { }
            else
            {
                nInputNum = 0;
            }
            mBlocksList[11].SetTipColor(mInputInfoData.color_blockSureStart);
            mBlocksList[11].SetBGColor(mInputInfoData.color_blockBG);

            if (strInputNum.CompareTo("") != 0)
                SetFinishInput();
        }      
        //Debug.Log(getobj.nID);
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

    /// <summary>
    /// 出现效果
    /// </summary>
    public void ShowEffect()
    {
        if (gameObject.activeSelf)
            return;
        bProtect = false;
        AudioClip cp0 = Resources.Load<AudioClip>("sound/button_down");
        if (cp0 != null)
        {
            AudioSource.PlayClipAtPoint(cp0, Camera.main.transform.position);
        }
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
        AudioClip cp0 = Resources.Load<AudioClip>("sound/button_up");
        if (cp0 != null)
        {
            AudioSource.PlayClipAtPoint(cp0, Camera.main.transform.position);
        }
        transform.DOScale(Vector3.one * 0.001f, 0.3f).OnComplete(()=> 
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

    /// <summary>
    /// 重置信息
    /// </summary>
    public void ResetInfos()
    {
        strInputNum = "";
        nInputNum = 0;
        for (int i = 0; i < mBlocksList.Count; i++)
        {
            mBlocksList[i].SetBGColor(mInputInfoData.color_blockBG);
            mBlocksList[i].SetTipColor(mInputInfoData.color_blockNum);
            if (i == 11)
            {
                mBlocksList[i].SetTipColor(mInputInfoData.color_blockSureStart);
            }
        }
    }

    /// <summary>
    /// 保护设置开/关
    /// </summary>
    /// <param name="_isProtect"></param>
    public void SetProtect(bool _isProtect)
    {
        bProtect = _isProtect;
    }

}


public class InputNumBlockObj : MonoBehaviour
{
    public int nID = 0;
    public Image mImageBG;
    public Image mBlock;

    public void InitAwake(int _id, InputInfoData _inputData)
    {
        nID = _id;
        mImageBG = gameObject.AddComponent<Image>();
        mImageBG.sprite = ResManager.GetSprite(_inputData.strAlatsName, _inputData.strPicBG);
        mImageBG.type = Image.Type.Sliced;
        mBlock = UguiMaker.newImage("tip", transform, _inputData.strAlatsName, _inputData.strCellPicFirstName + _id);
        mBlock.transform.localScale = Vector3.one * _inputData.fNumScale;
    }

    public void SetBGColor(Color _color)
    {
        mImageBG.color = _color;
    }
    public void SetTipColor(Color _color)
    {
        mBlock.color = _color;
    }
}

[System.Serializable]
public class InputInfoData
{
    //多少行？
    public int nConstraintCount = 4;
    //输入键id
    public List<int> mBlockIDList = new List<int>() { 7, 8, 9, 4, 5, 6, 1, 2, 3, 10, 0, 11 };
    public Vector2 vBgSize = new Vector2(665f, 625f);
    public Vector2 vCellSize = new Vector2(200f, 140f);
    public Vector2 vSpacing = new Vector2(5f, 5f);
    /// <summary>
    /// 整体缩放大小
    /// </summary>
    public float fscale = 0.45f;
    /// <summary>
    /// 字体缩放大小
    /// </summary>
    public float fNumScale = 4.5f;

    [Header("Color Change")]
    //block 背景颜色
    public Color color_blockBG = new Color(154f / 255, 216f / 255, 255f / 255, 1f);
    //block 数字颜色
    public Color color_blockNum = new Color(1f / 255, 96f / 255, 158f / 255, 1f);
    //block 输入数字后确认按钮颜色
    public Color color_blockSureBG = new Color(255f / 255, 179f / 255, 66f / 255, 1f);
    //block 确认按钮初始颜色
    public Color color_blockSureStart = new Color(255f / 255, 255f / 255, 255f / 255, 1f);
    //大背景颜色
    public Color bgcolor = new Color(255f / 255, 255f / 255, 255f / 255, 1f);
    [Header("Res Path")]
    //res图集名
    public string strAlatsName = "public";
    //背景图名
    public string strPicBG = "inputbg";
    //cell图前缀名
    public string strCellPicFirstName = "input";
}

