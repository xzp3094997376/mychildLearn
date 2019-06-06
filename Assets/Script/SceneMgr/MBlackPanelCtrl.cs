using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 黑屏控制器
/// </summary>
public class MBlackPanelCtrl : MonoBehaviour
{

    public Image mImage;
    public float fTime = 1.5f;

    //public void InitAwake()
    //{
    //    mImage = gameObject.GetComponent<Image>();
    //    mImage.color = new Color(0f, 0f, 0f, 0f);
    //    gameObject.SetActive(false);
    //}


    public void BlackIn(System.Action _callback = null)
    {
        gameObject.SetActive(true);
        mImage.color = new Color(0f, 0f, 0f, 0f);
        ColorSetAlpth(1, () =>
        {
            if (_callback != null)
                _callback();
        });       
    }

    public void BlackOut(System.Action _callback = null)
    {
        ColorSetAlpth(0, () =>
        {
            if (_callback != null)
                _callback();
            gameObject.SetActive(false);
        });     
    }

    public void BlackInOut(System.Action _callback = null)
    {
        BlackIn(() => 
        {
            BlackOut(_callback);
        });
    }


    private void ColorSetAlpth(float _toAlpth, System.Action _callback)
    {
        DOTween.To(() => mImage.color, x => mImage.color = x, new Color(0f, 0f, 0f,_toAlpth), fTime).OnComplete(() => 
        {
            if (_callback != null)
                _callback();
        });
    }

}
