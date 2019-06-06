using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class KnowCalendarQuesPanellv2 : MonoBehaviour
{

    private Image imgBG;

    KnowCalendarQueslv2 mQua0;
    KnowCalendarQueslv2 mQua1;
    KnowCalendarQueslv2 mQua2;

    Vector3 vQua0 = new Vector3(-45f, 75f, 0f);
    Vector3 vQua1 = new Vector3(-45f, 0f, 0f);
    Vector3 vQua2 = new Vector3(-45f, -75f, 0f);

    public void InitAwake()
    {
        imgBG = UguiMaker.newImage("imgbg", transform, "knowcalendar_sprite", "kc_imgbg1", false);

        mQua0 = UguiMaker.newGameObject("qua0", transform).AddComponent<KnowCalendarQueslv2>();
        mQua1 = UguiMaker.newGameObject("qua1", transform).AddComponent<KnowCalendarQueslv2>();
        mQua2 = UguiMaker.newGameObject("qua2", transform).AddComponent<KnowCalendarQueslv2>();
        mQua0.InitAwake(0);
        mQua1.InitAwake(1);
        mQua2.InitAwake(2);

        mQua0.transform.localPosition = vQua0 + new Vector3(800f, 0f, 0f);
        mQua1.transform.localPosition = new Vector3(-45f, 0f, 0f);
        mQua2.transform.localPosition = vQua2 + new Vector3(800f, 0f, 0f);


    }

    /// <summary>
    /// 问题显示
    /// </summary>
    /// <param name="_id"></param>
    public void ShowQua(int _id)
    {
        if (_id == 0)
        {
            mQua0.transform.DOLocalMove(vQua0, 0.5f);
        }
        else if (_id == 1)
        { mQua1.transform.DOLocalMove(vQua1, 0.5f); }
        else
        { mQua2.transform.DOLocalMove(vQua2, 0.5f); }
        
    }


}
