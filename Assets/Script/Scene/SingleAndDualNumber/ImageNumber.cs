using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ImageNumber : MonoBehaviour
{

    public int nNumber = 0;

    public string strABName = "singledualnum_sprite";
    public string strFirstPicName = "greennum";

    private Image num1;
    private Image num2;

    public float fIndex = 10f;


    public Image GetNum1()
    {
        return num1;
    }
    public Image GetNum2()
    {
        return num2;
    }

    public void InitAwake()
    {
        num1 = transform.Find("num1").GetComponent<Image>();     
        num2 = transform.Find("num2").GetComponent<Image>();
        SetNumber(0);
    }


    public void SetNumber(int _num)
    {
        num1.enabled = false;
        num2.enabled = false;

        nNumber = _num;
        if (_num < 10)
        {
            num1.enabled = true;
            num1.rectTransform.anchoredPosition = Vector2.zero;
            int nA = _num;
            num1.sprite = ResManager.GetSprite(strABName, strFirstPicName + nA.ToString());
        }
        else
        {
            num1.enabled = true;
            num2.enabled = true;
            int nA = _num / 10;
            int nB = _num % 10;
            num1.sprite = ResManager.GetSprite(strABName, strFirstPicName + nA.ToString());
            num2.sprite = ResManager.GetSprite(strABName, strFirstPicName + nB.ToString());
        }

        num1.SetNativeSize();
        num2.SetNativeSize();

        if (_num >= 10)
        {
            Vector2 vgetL = num1.rectTransform.sizeDelta;
            Vector2 vgetR = num2.rectTransform.sizeDelta;
            num1.rectTransform.anchoredPosition = new Vector2(-vgetL.x * 0.5f - fIndex * 0.5f, 0f);
            num2.rectTransform.anchoredPosition = new Vector2(vgetR.x * 0.5f + fIndex * 0.5f, 0f);
        }

    }


    public void SetNumColor(Color _color)
    {
        num1.color = _color;
        num2.color = _color;
    }
}
