using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class CFishNumObj : MonoBehaviour
{

    public int nNum = 0;
    public bool bUp = true;

    public bool bStation = false;


    private Image imgbg;
    private Image imgnum;

    private BoxCollider2D mbox2d;

    Vector3 vstart;

    public void InitAwake()
    {
        imgbg = UguiMaker.newImage("imgbg", transform, "catchfish_sprite", "rect0", false);
        imgnum = UguiMaker.newImage("imgnum", transform, "catchfish_sprite", "1", false);

        mbox2d = gameObject.AddComponent<BoxCollider2D>();
        mbox2d.size = imgbg.rectTransform.sizeDelta;

        vOldScale = transform.localScale;
    }

    /// <summary>
    /// 设置数字
    /// </summary>
    /// <param name="_num"></param>
    public void SetNumber(int _num)
    {
        nNum = _num;
        imgnum.sprite = ResManager.GetSprite("catchfish_sprite", nNum.ToString());
        imgnum.SetNativeSize();
    }

    /// <summary>
    /// 数字隐藏显示
    /// </summary>
    /// <param name="_active"></param>
    public void NumberActive(bool _active)
    {
        imgnum.enabled = _active;
    }

    public Vector2 GetSize()
    {
        return mbox2d.size;
    }

    public void SetStartPos(Vector3 _pos)
    {
        vstart = _pos;
    }

    public void BoxActive(bool _active)
    {
        mbox2d.enabled = _active;
    }

    public void SetScale(float _fscale)
    {
        transform.DOScale(Vector3.one * _fscale, 0.2f);
    }

    /// <summary>
    /// 数字移出效果
    /// </summary>
    public void RemoveOutEffect()
    {
        int _dir = 1;
        if (!bUp)
            _dir = -1;

        nNum = 0;
        float ftime = 0.32f;
        DOTween.To(() => imgnum.color, x => imgnum.color = x, new Color(1f, 1f, 1f, 0f), ftime);
        imgnum.transform.DOLocalMoveY(100 * _dir, ftime).OnComplete(() =>
        {
            imgnum.enabled = false;
            imgnum.color = new Color(1f, 1f, 1f, 1f);
            imgnum.rectTransform.anchoredPosition = Vector2.zero;
        });
    }

    /// <summary>
    /// 重置位置(重生)
    /// </summary>
    public void ReCreateNum()
    {       
        transform.localPosition = vstart;
        transform.localScale = Vector3.one * 0.001f;
        gameObject.SetActive(true);
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }


    Vector3 vOldScale;
    public void DoScaleEffect()
    {
        transform.SetSiblingIndex(5);
        transform.DOScale(Vector3.one * 1.25f, 0.2f).OnComplete(() =>
        {
            transform.DOScale(vOldScale, 0.2f);
        });
    }

}
