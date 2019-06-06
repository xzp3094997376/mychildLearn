using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class KnowCalendarLostDay : MonoBehaviour
{

    public int nDay;

    private Image img0;
    private GameObject imgNumberObj;
    public ImageNumber imgNumber;

    private BoxCollider2D mbox2D;

    private Vector3 vRemenber;

    public void InitAwake(int _day)
    {
        nDay = _day;

        img0 = gameObject.AddComponent<Image>();
        img0.type = Image.Type.Sliced;
        img0.sprite = ResManager.GetSprite("public", "inputbg");
        img0.rectTransform.sizeDelta = new Vector2(85f, 72f);

        mbox2D = gameObject.AddComponent<BoxCollider2D>();
        mbox2D.size = new Vector2(85f, 72f);

        imgNumberObj = UguiMaker.newGameObject("imgNumber", transform);
        UguiMaker.newImage("num1", imgNumberObj.transform, "knowcalendar_sprite", "kc_num0", false);
        UguiMaker.newImage("num2", imgNumberObj.transform, "knowcalendar_sprite", "kc_num0", false);
        imgNumber = imgNumberObj.AddComponent<ImageNumber>();
        imgNumber.strABName = "knowcalendar_sprite";
        imgNumber.strFirstPicName = "kc_num";
        imgNumber.fIndex = 2;
        imgNumber.InitAwake();
        imgNumber.SetNumber(_day);
        imgNumber.SetNumColor(new Color(104f / 255, 13f / 255, 2f / 255, 1f));
        imgNumber.transform.localScale = Vector3.one * 1f;

        gameObject.name = "lostday" + _day;
    }


    public Vector2 GetSize()
    {
        return mbox2D.size;
    }

    public void SetRemenberPos(Vector3 _vpos)
    {
        vRemenber = _vpos;
    }

    public void MoveToRemenberPos()
    {
        transform.DOLocalMove(vRemenber, 0.3f);
    }

    public void DoScale(float _sc,float _time)
    {
        transform.DOScale(Vector3.one * _sc, _time);
    }
}
