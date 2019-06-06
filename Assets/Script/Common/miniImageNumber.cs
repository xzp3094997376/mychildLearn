using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// mini数字(image拼凑)
/// </summary>
public class miniImageNumber : MonoBehaviour
{

    public int nNumber = 0;
    public string strABName = "";
    public string strFirstPicName = "";
    private GameObject mNumGO;
    private List<Image> mImageList = new List<Image>();

    private Color mColor = Color.white;

    //private Image imgNumBG;

    public void SetNumber(int _num)
    {
        nNumber = _num;

        if (mNumGO == null)
        {
            mNumGO = UguiMaker.newGameObject("nums", transform);
        }

        Common.DestroyChilds(mNumGO.transform);
        mImageList.Clear();

        string strNum = _num.ToString();
        string[] strNums = FormatString(strNum);

        float fX = 0f;

        for (int i = 0; i < strNums.Length; i++)
        {
            Image img = UguiMaker.newImage(strNums[i], mNumGO.transform, strABName, strFirstPicName + strNums[i], false);
            mImageList.Add(img);
            img.color = mColor;
            float fhalf = img.rectTransform.sizeDelta.x * 0.5f;
            img.rectTransform.anchoredPosition = new Vector2(fX + fhalf, 0f);

            fX += img.rectTransform.sizeDelta.x;
        }
        //if (mImageList.Count > 1)
        mNumGO.transform.localPosition = new Vector3(-fX * 0.5f, 0f, 0f);
    }


    public void SetNumColor(Color _color)
    {
        mColor = _color;
        for (int i = 0; i < mImageList.Count; i++)
        {
            mImageList[i].color = _color;
        }
    }

    string[] FormatString(string s)
    {
        string[] ret = new string[s.Length];
        for (int i = 0; i < ret.Length; i++)
        {
            ret[i] = s.Substring(i, 1);
        }
        return ret;
    }

    public Image GetMaxRight()
    {
        if (mImageList.Count > 0)
        {
            return mImageList[mImageList.Count - 1];
        }
        return null;
    }


    public void CreateImgNumBG(string _altas, string _strName, Vector2 _size, Color _color)
    {
        Image imgnumbg = UguiMaker.newImage("numbg", mNumGO.transform, _altas, _strName, false);
        imgnumbg.rectTransform.sizeDelta = _size;
        float fIndexmm = 0f;
        for (int i = 0; i < mImageList.Count; i++)
        {
            fIndexmm += mImageList[i].rectTransform.sizeDelta.x;
        }
        imgnumbg.rectTransform.anchoredPosition = new Vector2(fIndexmm * 0.5f, 0f);
        imgnumbg.transform.SetSiblingIndex(0);
        imgnumbg.color = _color;
    }

    public void DesNum()
    {
        Common.DestroyChilds(mNumGO.transform);
        mImageList.Clear();
        nNumber = 0;
    }

}
