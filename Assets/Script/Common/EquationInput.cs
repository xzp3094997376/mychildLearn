using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class EquationInput : MonoBehaviour {
    public string strInputNum { get; set; }
    private string mWho { get; set; }
    private bool mIsDub { get; set; }
    // Use this for initialization
    void Start () {
	
	}
    
    private System.Action mInputNumCallBack = null;
    public void SetInputNumberCallBack(System.Action _action)
    {
        mInputNumCallBack = _action;
    }
    private System.Action mCleanCallBack = null;
    public void SetClearNumberCallBack(System.Action _action)
    {
        mCleanCallBack = _action;
    }
    private System.Action mFinishCallBack = null;
    public void SetFinishInputCallBack(System.Action _action)
    {
        mFinishCallBack = _action;
    }
    private Color getBgColor()
    {
        Color color = new Color(1, 1, 1, 1);
        switch (mWho)
        {
            case "animalnumberlogic":
                color = new Color(208f / 255, 247f / 255, 255f / 255, 1);
                break;
        }
        return color;
    }
    private Color getNumBgColor()
    {
        Color color = new Color(227f / 255, 181f / 255, 122f / 255, 1);
        switch (mWho)
        {
            case "animalnumberlogic":
                color = new Color(0f / 255, 198f / 255, 216f / 255, 1);
                break;
        }
        return color;
    }
    private Color getNumColor()
    {
        Color color = new Color(152f / 255, 91f / 255, 30f / 255, 1);
        switch (mWho)
        {
            case "animalnumberlogic":
                color = new Color(3f / 255, 105f / 255, 206f / 255, 1);
                break;
        }
        return color;
    }
    private Color getItemDownColor()
    {
        Color color = new Color(215f / 255, 155f / 255, 82f / 255);
        switch (mWho)
        {
            case "animalnumberlogic":
                color = new Color(66f / 255, 209f / 255, 239f / 255, 1);
                break;
        }
        return color;
    }
    private string getLastMid()
    {
        string mid = "10";
        switch (mWho)
        {
            case "animalnumberlogic":
                mid = "10";
                break;
            case "learntime":
                mid = "0";
                break;
        }
        return mid;
    }
    public void init(string spName,int cul = 3,bool isdub = false)
    {
        float bgH = 260;
        if(cul == 4)
        {
            bgH = 335;
        }
        mWho = spName;
        mIsDub = isdub;
        Image bg = UguiMaker.newImage("bg", transform, spName + "_sprite", "inputbg");
        bg.rectTransform.sizeDelta = new Vector2(300, bgH);
        bg.color = getBgColor();

        BoxCollider box = gameObject.AddComponent<BoxCollider>();
        box.size = new Vector3(300, bgH);
        float startY = 73;
        float startX = -85;
        if (cul == 4)
        {
            startY = 111;
        }
        Vector3 startPos = new Vector3(startX, startY, 0);
        int disH = 85;
        int disV = 75;
        Color bgColor = getNumBgColor();
        Color numColor = getNumColor();
        for (int i = 0;i < 9; i++)
        {
            int index = i + 1;
            GameObject itemGo = UguiMaker.newGameObject("item_" + index, transform);
            Image bgitem = UguiMaker.newImage("bg", itemGo.transform, spName + "_sprite", "inputItemBg");
            bgitem.rectTransform.sizeDelta = new Vector2(disH - 5, disV - 5);
            bgitem.color = bgColor;
            itemGo.transform.localPosition = startPos + new Vector3((i % 3) * disH, -((int)(i / 3) * disV), 0);
            Image numImage = UguiMaker.newImage("num", itemGo.transform, spName + "_sprite", "input" + index);
            numImage.rectTransform.localScale = Vector3.one * 0.28f;
            numImage.color = numColor;
            EventTriggerListener.Get(itemGo).onDown = BlockDown;
            EventTriggerListener.Get(itemGo).onUp = BlockUp;
        }
        if(cul > 3)
        {
            string mid = getLastMid();
            List<string> list = new List<string>() { "clean", mid, "close" };
            int index = 0;
            for(int i = 9;i < 12; i++)
            {
                int cIndex = i + 1;
                string str = list[index];
                GameObject itemGo = UguiMaker.newGameObject("item_" + str, transform);
                Image bgitem = UguiMaker.newImage("bg", itemGo.transform, spName + "_sprite", "inputItemBg");
                bgitem.rectTransform.sizeDelta = new Vector2(disH - 5, disV - 5);
                bgitem.color = bgColor;
                itemGo.transform.localPosition = startPos + new Vector3((i % 3) * disH, -((int)(i / 3) * disV), 0);
                Image numImage = UguiMaker.newImage("num", itemGo.transform, spName + "_sprite", "input" + str);
                float scaleNu = 0.28f;
                if (str != mid)
                {
                    scaleNu = 1;
                }
                numImage.rectTransform.localScale = Vector3.one * scaleNu;
                numImage.color = numColor;
                EventTriggerListener.Get(itemGo).onDown = BlockDown;
                EventTriggerListener.Get(itemGo).onUp = BlockUp;
                index++;
            }
        }
       gameObject.active = false;
    }
    private void setItembgDownColor(GameObject go)
    {
        if (go.name == "item_clean")
        {
            strInputNum = "100000";
        }
        else if (go.name == "item_close")
        {
            strInputNum = "10000";
        }
        else
        {
            if (mIsDub)
            {
                strInputNum += go.name.Split('_')[1];

                if(strInputNum.Length > 2)
                {
                    strInputNum = strInputNum.Substring(1,1);
                }
            }
            else
            {
                strInputNum = go.name.Split('_')[1];
            }
        }
        Image bgitem = go.transform.Find("bg").gameObject.GetComponent<Image>();
        bgitem.color = getItemDownColor();// new Color(215f / 255, 155f / 255, 82f / 255);
    }
    private void setItembgUpColor(GameObject go)
    {
        Image bgitem = go.transform.Find("bg").gameObject.GetComponent<Image>();
        bgitem.color = getNumBgColor();// new Color(227f / 255, 181f / 255, 122f / 255);
    }
    private void BlockDown(GameObject go)
    {
        setItembgDownColor(go);
    }
    private void BlockUp(GameObject go)
    {
        setItembgUpColor(go);
        if(null != mInputNumCallBack)
        {
            if(strInputNum == "10000")
            {
                HideEffect();

            }else if(strInputNum == "100000")
            {
                if(null != mCleanCallBack)
                {
                    strInputNum = "";
                    mCleanCallBack();
                }
            }
            else
            {
                mInputNumCallBack();
            }
            
        }
    }
    private bool isInShow { get; set; }
    //显示
    public void ShowEffect()
    {
        if (gameObject.active || isInShow) return;

        strInputNum = "";
        gameObject.SetActive(true);
        isInShow = true;
        transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
        {
        });

    }
    //隐藏
    public void HideEffect()
    {
        if (null != mFinishCallBack)
        {
            mFinishCallBack();
        }
        transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            isInShow = false;
            gameObject.SetActive(false);
        });
    }
    // Update is called once per frame
    void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            //Debug.Log("hits : " + hits);
            if (null != hits)
            {
                if(hits.Length > 0)
                {
                    foreach (RaycastHit hit in hits)
                    {
                        GameObject com = hit.collider.gameObject;
                        Debug.Log("com : " + com.name);
                        if (null != com)
                        {
                            if (com != gameObject)
                            {
                                HideEffect();
                            }

                        }
                        else
                        {
                            HideEffect();
                        }
                    }
                }else
                {
                    HideEffect();
                }
                
            }else
            {
                HideEffect();
            }
        }
    }
}
