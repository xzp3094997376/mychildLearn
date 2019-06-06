using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KeyboardCtl : MonoBehaviour {
    enum KeyboardType {none, bottom, }
    static KeyboardCtl instance = null;
    public static KeyboardCtl GetInstance()
    {
        if(null == instance)
        {
            instance = UguiMaker.newGameObject("keyboard", Global.instance.mCanvasTop).AddComponent< KeyboardCtl>();
            instance.Init();
        }
        return instance;
    }

    List<RectTransform> keyboard_list = null;
    System.Action<int> callback_onclk = null;
    RectTransform mRtranKeyboard { get; set; }
    KeyboardType mType = KeyboardType.none;

    void Init()
    {
        Image image = gameObject.AddComponent<Image>();
        image.rectTransform.sizeDelta = new Vector2(1423, 800);
        image.color = new Color(1, 1, 1, 0);
        Button button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(clkhide);

        mRtranKeyboard = UguiMaker.newGameObject("keyboard", transform).GetComponent<RectTransform>();
        keyboard_list = new List<RectTransform>();

        for (int i = 0; i <= 9; i++)
        {
            Image img = UguiMaker.newImage(i.ToString(), mRtranKeyboard, "public", "keybord_btn");
            img.gameObject.AddComponent<Button>();
            img.rectTransform.sizeDelta = new Vector2(80, 80);

            Text tex = UguiMaker.newText("text", img.transform);
            tex.fontSize = 50;
            tex.rectTransform.sizeDelta = new Vector2(100, 100);
            tex.color = new Color32(38, 38, 54, 255);
            tex.alignment = TextAnchor.LowerCenter;

            keyboard_list.Add(img.gameObject.GetComponent<RectTransform>());

        }

        Image img_clean = UguiMaker.newImage("clean", mRtranKeyboard, "public", "keybord_clean");
        img_clean.gameObject.AddComponent<Button>();
        keyboard_list.Add(img_clean.gameObject.GetComponent<RectTransform>());
        img_clean.rectTransform.sizeDelta = new Vector2(80, 80);

        Image img_right = UguiMaker.newImage("right", mRtranKeyboard, "public", "keybord_right");
        img_right.gameObject.AddComponent<Button>();
        keyboard_list.Add(img_right.gameObject.GetComponent<RectTransform>());
        img_right.rectTransform.sizeDelta = new Vector2(80, 80);

    }


    public void ShowOnButtom(System.Action<int> callback)
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<Button>().onClick.AddListener(clkhide);
        callback_onclk = callback;

        mRtranKeyboard.sizeDelta = new Vector2(80 * 12, 70);
        List<Vector3> poss = Common.PosSortByWidth(82 * 12, 12, 0);
        for(int i = 0; i < 12; i++)
        {
            keyboard_list[i].anchoredPosition3D = poss[i];
        }
        StartCoroutine("TShowOnButtom");

        mType = KeyboardType.bottom;
    }
    IEnumerator TShowOnButtom()
    {
        Vector2 pos0 = new Vector2(0, -480);
        Vector3 pos1 = new Vector3(0, -400 + 45);
        for(float i = 0; i < 1f; i += 0.05f)
        {
            mRtranKeyboard.anchoredPosition = Vector2.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mRtranKeyboard.anchoredPosition = pos1;

    }
    IEnumerator THideOnButtom()
    {
        Vector2 pos0 = new Vector2(0, -480);
        Vector3 pos1 = new Vector3(0, -400 + 45);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtranKeyboard.anchoredPosition = Vector2.Lerp(pos1, pos0, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mRtranKeyboard.anchoredPosition = pos0;
        gameObject.SetActive(false);
    }

    public void clkhide()
    {
        switch (mType)
        {
            case KeyboardType.bottom:
                {
                    StopAllCoroutines();
                    StartCoroutine("THideOnButtom");
                }
                break;

        }
        mType = KeyboardType.none;

    }

    public void clk0()
    {
        if(null != callback_onclk)
        {
            callback_onclk(0);
        }
    }
    public void clk1()
    {
        if (null != callback_onclk)
        {
            callback_onclk(1);
        }
    }
    public void clk2()
    {
        if (null != callback_onclk)
        {
            callback_onclk(2);
        }
    }
    public void clk3()
    {
        if (null != callback_onclk)
        {
            callback_onclk(3);
        }
    }
    public void clk4()
    {
        if (null != callback_onclk)
        {
            callback_onclk(0);
        }
    }
    public void clk5()
    {
        if (null != callback_onclk)
        {
            callback_onclk(5);
        }
    }
    public void clk6()
    {
        if (null != callback_onclk)
        {
            callback_onclk(6);
        }
    }
    public void clk7()
    {
        if (null != callback_onclk)
        {
            callback_onclk(7);
        }
    }
    public void clk8()
    {
        if (null != callback_onclk)
        {
            callback_onclk(8);
        }
    }
    public void clk9()
    {
        if (null != callback_onclk)
        {
            callback_onclk(9);
        }
    }
    public void clk_right()
    {
        if (null != callback_onclk)
        {
            callback_onclk(-1);
        }
        StartCoroutine("THideOnButtom");
    }
    public void clk_clean()
    {
        if (null != callback_onclk)
        {
            callback_onclk(-2);
        }
        clkhide();
    }

}
