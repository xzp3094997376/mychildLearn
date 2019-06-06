using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FaileShakeCtrl : MonoBehaviour {

    Vector3 vstart;
	// Use this for initialization
	public void InitAwake () {
        vstart = transform.localPosition;
    }

    public void ShakeObj(System.Action _callback = null)
    {
        transform.DOLocalMove(vstart + new Vector3(15f, 0f, 0f), 0.1f).OnComplete(() =>
        {
            transform.DOLocalMove(vstart - new Vector3(15f, 0f, 0f), 0.2f).OnComplete(() =>
            {
                transform.DOLocalMove(vstart, 0.1f).OnComplete(() =>
                {
                    if (_callback != null)
                        _callback();
                });
            });
        });
    }

}
