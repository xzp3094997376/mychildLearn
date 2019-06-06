using UnityEngine;
using System.Collections;
using DG.Tweening;

public class KnowDropObj : MonoBehaviour
{

    protected BoxCollider2D mbox2D;
    protected Vector3 vStart;
    public float fScale = 1f;

    public virtual void InitAwake()
    { }
    public virtual void ResetInfos()
    { }


    public void SetStartPos(Vector3 _vpos)
    {
        vStart = _vpos;
    }
    public void Box2DActive(bool _active)
    {
        if (mbox2D != null)
        {
            mbox2D.enabled = _active;
        }
    }
    public Vector2 GetBoxSize()
    {
        if (mbox2D != null)
        { return mbox2D.size; }
        return new Vector2(100f, 100f);
    }
    public void DropSet()
    {
        transform.DOScale(Vector3.one * fScale * 1.3f, 0.2f);
    }
    public virtual void DropReset(System.Action _call = null)
    {
        transform.DOScale(Vector3.one * fScale, 0.2f).OnComplete(()=> 
        {
            if (_call != null)
            { _call(); }
        });
    }
    public void BlackToStart()
    {
        transform.DOLocalMove(vStart, 0.3f);
    }

    public void DoScale(Vector3 vScale, float _time)
    {
        transform.DOScale(vScale * fScale, _time);
    }
	
}
