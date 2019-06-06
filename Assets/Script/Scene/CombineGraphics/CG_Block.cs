using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class CG_Block : MonoBehaviour
{
    public string strName = "";
    public int nIndex = 0;
    public int nID = 0;

    public bool bOK = false;
    private Image imgBlock;
    private BoxCollider2D mBox2D;

    private Vector3 vStart;

    public void InitAwake(string _name, int _index, int _id)
    {
        strName = _name;
        nIndex = _index;
        nID = _id;
        imgBlock = gameObject.AddComponent<Image>();
        imgBlock.sprite = ResManager.GetSprite("combinegraphics_sprite", strName + nIndex.ToString());
        imgBlock.SetNativeSize();

        mBox2D = gameObject.AddComponent<BoxCollider2D>();
        float fx = imgBlock.rectTransform.sizeDelta.x;
        float fy = imgBlock.rectTransform.sizeDelta.y;
        if (fx < 50f)
            fx = 50f;
        if (fy < 50f)
            fy = 50f;
        mBox2D.size = new Vector2(fx, fy);

    }

    public void SetStartPos(Vector3 _vpos)
    {
        vStart = _vpos;
    }

    public void BoxActive(bool _active)
    {
        mBox2D.enabled = _active;
    }

    public void SetColor(Color _color)
    {
        //imgBlock.color = _color;
    }

    public Vector2 GetBoxSize()
    {
        return mBox2D.size;
    }

    public void BlackToStart()
    {
        transform.DOLocalMove(vStart, 0.25f);
        transform.DOScale(Vector3.one * 1.3f, 0.25f);
    }

    public void HideByColor()
    {
        DOTween.To(() => imgBlock.color, x => imgBlock.color = x, new Color(1f, 1f, 1f, 0f), 1f);
    }

}
