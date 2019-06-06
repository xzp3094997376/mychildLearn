using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenDebug : MonoBehaviour {


    static Text gText { get; set; }
    static string gmsg = "";

	void Start () {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject top = GameObject.Find("Canvas/top");

        transform.SetParent(canvas.transform);
        transform.SetSiblingIndex(top.transform.GetSiblingIndex() + 1);
        transform.localScale = Vector3.one;
        gameObject.layer = LayerMask.NameToLayer("UI");

        gameObject.name = "ScreenDebug";
        RectTransform rt = gameObject.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 1);
        rt.anchorMax = new Vector2(0.5f, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.anchoredPosition3D = Vector3.zero;
        rt.sizeDelta = new Vector2(GlobalParam.screen_width - 20, 0);

        ContentSizeFitter fit = gameObject.AddComponent<ContentSizeFitter>();
        fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        gText = gameObject.AddComponent<Text>();
        gText.font = Global.instance.mFontHwakang;
        //gText.font = Global.instance.mfo
        gText.color = new Color(1, 0, 0, 1);
        gText.fontSize = 30;

        gmsg = "";

        gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;

        Log("ScreenDebug初始化完成!");

    }

    public static void Log(string msg)
    {
        if (null == gText)
            return;
        gmsg = gmsg + "\n" + msg;
        if(gText.rectTransform.sizeDelta.y > GlobalParam.screen_height - 50)
        {
            //int begin = 0;
            int index = 0;
            for (int i = 0; i < 3; i++)
            {
                index = gmsg.IndexOf('\n', index + 1);
            }
            gmsg = gmsg.Substring(index);

        }
        gText.text = gmsg;

    }
	

}
