using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SDN_NineBlockNum : MonoBehaviour
{
    public int nNum = 0;

    private Image bg;
    private Image imgNum;

    private BoxCollider2D box2D;

    public RectTransform rectTransform;
    SDN_NineBlockStation mstation;

    public void InitAwake()
    {
        rectTransform = transform as RectTransform;
        bg = gameObject.GetComponent<Image>();
        bg.sprite = ResManager.GetSprite("singledualnum_sprite", "rang2");

        imgNum = UguiMaker.newImage("imgNum", transform, "singledualnum_sprite", "0");
        imgNum.transform.localPosition = Vector3.zero;
        imgNum.transform.localScale = Vector3.one * 0.2f;

        box2D = gameObject.GetComponent<BoxCollider2D>();
        if(box2D == null)
            box2D = gameObject.AddComponent<BoxCollider2D>();
        box2D.size = bg.rectTransform.sizeDelta;

        mstation = transform.parent.GetComponent<SDN_NineBlockStation>();

        ResetInfos();
    }

    public void SetNumber(int _num)
    {
        imgNum.enabled = true;
        nNum = _num;
        imgNum.sprite = ResManager.GetSprite("singledualnum_sprite", _num.ToString());
        if (_num % 2 == 0)
        {
            imgNum.color = new Color(51f / 255, 130f / 255, 16f / 255);
        }
        else
        {
            imgNum.color = new Color(171f / 255, 91f / 255, 6f / 255);
        }
        imgNum.SetNativeSize();
    }

    public void BoxActive(bool _active)
    {
        box2D.enabled = _active;
    }

    public void ResetInfos()
    {
        imgNum.enabled = false;
        BoxActive(true);
    }

    /// <summary>
    /// 隐藏数字
    /// </summary>
    public void HideNumImage()
    {
        imgNum.enabled = false;
        BoxActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mstation.HitCheck(this);
    }

}
