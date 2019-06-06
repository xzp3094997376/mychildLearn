using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GoodsCtr : MonoBehaviour {
    public static GoodsCtr gSlect { get; set; }
    private Transform MDefParent { get; set; }
    private Vector3 MDefPos { get; set; }
    public int mID { get; set; }
    public string mGoodsName { get; set; }
    public int mType { get; set; }

    public void setDef(int id,Vector3 defPos,string goodsName,int road = 100)
    {
        mID = id;
        MDefPos = defPos;
        MDefParent = transform.parent;
        mGoodsName = goodsName;
        mType = road;
    }
    public void PoP()
    {
        gameObject.transform.SetAsLastSibling();
    }
    public void MovetoDef()
    {
        StartCoroutine("TMovetoDef");
    }
    IEnumerator TMovetoDef()
    {
        Vector3 startPos = transform.localPosition;
        for (float j = 0; j < 1f; j += 0.1f)
        {
            transform.localPosition = Vector3.Lerp(startPos, MDefPos, j);
            yield return new WaitForSeconds(0.001f);
        }
        transform.localPosition = MDefPos;
        transform.parent = MDefParent;
    }

    public void ShowOut(float endScale = 1)
    {
        transform.localScale = Vector3.one * 0.001f;
        transform.DOScale(Vector3.one * (endScale + 0.2f), 0.2f).OnComplete(() =>
        {
            transform.DOScale(Vector3.one * (endScale - 0.1f), 0.15f).OnComplete(() =>
            {
                transform.DOScale(Vector3.one * (endScale + 0.1f), 0.12f).OnComplete(() =>
                {
                    transform.DOScale(Vector3.one * endScale, 0.1f);
                });
            });
        });
    }
}
