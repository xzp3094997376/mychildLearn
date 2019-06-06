using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class SingleDualNum_tap : MonoBehaviour
{

    private Image img;
    private BoxCollider2D box2D;

    public SDN_NineBlockStation mStation;

    public void InitAwake(SDN_NineBlockStation _station)
    {
        mStation = _station;

        img = transform.GetComponent<Image>();
        img.sprite = ResManager.GetSprite("singledualnum_sprite", "rang1");
        box2D = transform.GetComponent<BoxCollider2D>();
    }


    public void ShowOut()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.one * 0.001f;
        transform.DOScale(Vector3.one, 0.3f);
    }

    public void Hide()
    {
        transform.DOScale(Vector3.one * 0.001f, 0.3f);
    }

}
