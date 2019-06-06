using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class KnowCalendarQueslv2 : MonoBehaviour
{
    /// <summary>
    /// 0昨天 1今天 2后天
    /// </summary>
    public int nQuaID = 0;

    private Image quatxt;
    private Button quabtn0;
    private Button quabtn1;

    public ImageNumber mNum0;
    public Image mImg1;

    private Image mWenhao0;
    private Image mWenhao1;

    private KnowCalendarCtrl mCtrl;

    System.Action<KnowCalendarQueslv2> mClickBtn0CallBack = null;
    System.Action<KnowCalendarQueslv2> mClickBtn1CallBack = null;

    public bool bShakeFinish = true;

    public void InitAwake(int _quaID)
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as KnowCalendarCtrl;

        nQuaID = _quaID;
        quatxt = UguiMaker.newImage("quatxt", transform, "knowcalendar_sprite", "kc_daytxt" + nQuaID, false);
        quabtn0 = UguiMaker.newButton("btn0", transform, "knowcalendar_sprite", "kc_imgbg3");
        quabtn1 = UguiMaker.newButton("btn1", transform, "knowcalendar_sprite", "kc_imgbg3");

        quabtn0.transform.localPosition = new Vector3(-20f, 0f, 0f);
        quabtn1.transform.localPosition = new Vector3(245f, 0f, 0f);

        //img文字
        mNum0 = CreateNum(quabtn0.transform);
        mNum0.gameObject.SetActive(false);
        quabtn0.transition = Selectable.Transition.None;
        //日期设置回调
        mClickBtn0CallBack = mCtrl.mLevel2Ctrl.ClickCheckDay;
        EventTriggerListener.Get(quabtn0.gameObject).onClick = Btn0Click;

        //星期回调设置
        mImg1 = UguiMaker.newImage("mImg1", quabtn1.transform, "knowcalendar_sprite", "cn0", false);
        mImg1.color = new Color(162f / 255, 68f / 255, 8f / 255, 1f);
        mImg1.gameObject.SetActive(false);
        mClickBtn1CallBack = mCtrl.mLevel2Ctrl.ClickCheckWeek;
        EventTriggerListener.Get(quabtn1.gameObject).onClick = Btn1Click;


        mWenhao0 = UguiMaker.newImage("wenhao", quabtn0.transform, "knowcalendar_sprite", "kc_wenhao", false);
        mWenhao1 = UguiMaker.newImage("wenhao", quabtn1.transform, "knowcalendar_sprite", "kc_wenhao", false);
    }

    //创建img文字
    ImageNumber CreateNum(Transform _trans)
    {
        GameObject mgo = UguiMaker.newGameObject("imgNumber", _trans);
        Image num1 = UguiMaker.newImage("num1", mgo.transform, "knowcalendar_sprite", "kc_num0", false);
        Image num2 = UguiMaker.newImage("num2", mgo.transform, "knowcalendar_sprite", "kc_num0", false);
        ImageNumber imgNumber = mgo.AddComponent<ImageNumber>();
        imgNumber.strABName = "knowcalendar_sprite";
        imgNumber.strFirstPicName = "kc_num";
        imgNumber.fIndex = 2;
        imgNumber.InitAwake();
        imgNumber.SetNumber(0);
        imgNumber.SetNumColor(new Color(104f / 255, 13f / 255, 2f / 255, 1f));
        imgNumber.transform.localScale = Vector3.one * 1.2f;
        return imgNumber;
    }


    /// <summary>
    /// 日期点击
    /// </summary>
    private void Btn0Click(GameObject _go)
    {
        if (mClickBtn0CallBack != null)
        {
            mClickBtn0CallBack(this);
        }
    }
    /// <summary>
    /// 日期按钮事件关闭/开启
    /// </summary>
    /// <param name="_active"></param>
    public void Btn0Active(bool _active)
    {
        quabtn0.GetComponent<Image>().raycastTarget = _active;
    }
    /// <summary>
    /// 日期shake
    /// </summary>
    public void ShakeObj0()
    {
        bShakeFinish = false;
        float ft = 0.14f;
        mCtrl.mSoundCtrl.PlaySortSound("knowcalendar_sound", "drop_error");
        quabtn0.transform.DOLocalRotate(new Vector3(0f, 0f, -15f), ft).OnComplete(() =>
        {
            quabtn0.transform.DOLocalRotate(new Vector3(0f, 0f, 15f), ft * 2f).OnComplete(() =>
            {
                quabtn0.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), ft).OnComplete(() =>
                {
                    mNum0.gameObject.SetActive(false);
                    SetWenhao0Active(true);
                    bShakeFinish = true;
                });
            });
        });
    }





    /// <summary>
    /// 星期按钮点击
    /// </summary>
    /// <param name="_go"></param>
    private void Btn1Click(GameObject _go)
    {
        if (mClickBtn1CallBack != null)
        {
            mClickBtn1CallBack(this);
        }
    }
    /// <summary>
    /// 星期按钮事件关闭/开启
    /// </summary>
    /// <param name="_active"></param>
    public void Btn1Active(bool _active)
    {
        quabtn1.GetComponent<Image>().raycastTarget = _active;
    }
    /// <summary>
    /// 星期shake
    /// </summary>
    public void ShakeObj1()
    {
        bShakeFinish = false;
        float ft = 0.14f;
        mCtrl.mSoundCtrl.PlaySortSound("knowcalendar_sound", "drop_error");
        quabtn1.transform.DOLocalRotate(new Vector3(0f, 0f, -15f), ft).OnComplete(() =>
        {
            quabtn1.transform.DOLocalRotate(new Vector3(0f, 0f, 15f), ft * 2f).OnComplete(() =>
            {
                quabtn1.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), ft).OnComplete(() =>
                {
                    mImg1.gameObject.SetActive(false);
                    SetWenhao1Active(true);
                    bShakeFinish = true;
                });
            });
        });
    }
    /// <summary>
    /// set week sprite
    /// </summary>
    /// <param name="_id"></param>
    public void SetWeekSprite(int _id)
    {
        mImg1.sprite = ResManager.GetSprite("knowcalendar_sprite", "cn" + _id);
        mImg1.SetNativeSize();
    }



    public void SetWenhao0Active(bool _active)
    {
        mWenhao0.enabled = _active;
    }
    public void SetWenhao1Active(bool _active)
    {
        mWenhao1.enabled = _active;
    }

}




