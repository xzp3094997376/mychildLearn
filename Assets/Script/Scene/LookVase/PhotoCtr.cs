using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PhotoCtr : MonoBehaviour {
    private Image praintImage { get; set; }
    private Image xuxianImage { get; set; }
    private string mResultType { get; set; }
    public string mcurrType { get; set; }
    public bool isMark = false;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void setData(string resultType)
    {
        mResultType = resultType;
        if(null == xuxianImage)
        {
            xuxianImage = UguiMaker.newImage("xuxian", transform, "lookvase_sprite", "xuxian");
            xuxianImage.transform.localPosition = new Vector3(0, -175, 0);
        }
        if (null != praintImage)
        {
            praintImage.enabled = false;
        }
    }
    public void praint(string type)
    {
        if(null == praintImage)
        {
            praintImage = UguiMaker.newImage("praint", transform, "lookvase_sprite", "praint_" + type + "_result");
            praintImage.transform.localPosition = new Vector3(0, -175, 0);
        }
        else
        {
            praintImage.sprite = ResManager.GetSprite("lookvase_sprite", "praint_" + type + "_result");
        }
        praintImage.enabled = true;
        mcurrType = type;
        praintImage.color = new Color(7 / 255f, 14 / 255f, 158 / 255f);
        praintImage.SetNativeSize();
        isMark = true;
    }
    public bool checkFix()
    {
        bool boo = true;
        if(mcurrType != mResultType)
        {
            boo = false;
        }

        
        if (!boo)
        {
            StartCoroutine(TClosePraint());
        }
        return boo;
    }

    IEnumerator TClosePraint()
    {
        for (float j = 0; j < 1f; j += 0.05f)
        {
            float p = Mathf.Sin(Mathf.PI * j) * 0.8f;
            praintImage.transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * j) * 10);
            yield return new WaitForSeconds(0.01f);
        }
        praintImage.transform.localEulerAngles = new Vector3(0, 0, 0);

        praintImage.enabled = false;
        isMark = false;
        mcurrType = null;
    }
    public void ShowOut(Image bg, Image vasephoto)
    {
        StartCoroutine(TShowOut(bg,vasephoto));
    }
    IEnumerator TShowOut(Image bg,Image vasephoto)
    {
        Vector3 startScalebg = Vector3.zero;
        Vector3 endScalebg = Vector3.one * 1.2f;
        Vector3 endScalep = Vector3.one * 0.8f;
        Vector3 endScalel = Vector3.one;
        for (float j = 0; j < 1f; j += 0.1f)
        {
            bg.rectTransform.localScale = Vector3.Lerp(startScalebg, endScalebg, j);
            gameObject.transform.localScale = Vector3.Lerp(startScalebg, endScalep, j);
            yield return new WaitForSeconds(0.01f);
        }
        bg.rectTransform.localScale = endScalebg;
        gameObject.transform.localScale = endScalep;
    }
}
