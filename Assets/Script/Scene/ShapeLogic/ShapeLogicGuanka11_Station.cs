using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka11_Station : MonoBehaviour
{
    public RectTransform mRtran { get; set; }
    public Image mFrame, mBg;
    public Image mDoor, mWindow, mTop, mQizi;
    public BoxCollider mBox;
    //public ParticleSystem mEffect { get; set; }

    Vector3 mScale = new Vector3(0.9f, 0.9f, 1);
    public bool mdata_is_over = false;

    public int mdata_door_type = 0;
    public int mdata_window_type = 0;
    public int mdata_top_side = 0;
    public int mdata_qizi_type = 0;

    Button btn_door_left, btn_door_right;
    Button btn_window_left, btn_window_right;
    Button btn_top_left, btn_top_right;
    Button btn_qizi_left, btn_qizi_right;

    public void Init(int door_type, int window_type, int top_side, int qizi_type)
    {
        mRtran = gameObject.GetComponent<RectTransform>();
        mRtran.localScale = mScale;

        mdata_door_type = door_type;
        mdata_window_type = window_type;
        mdata_top_side = top_side;
        mdata_qizi_type = qizi_type;

        if (null == mBg)
        {

            mTop = UguiMaker.newImage("mTop", transform, "shapelogic_sprite", "guanka11_station2-0", false);
            mQizi = UguiMaker.newImage("mQizi", mTop.transform, "shapelogic_sprite", "guanka11_station3-" + qizi_type.ToString(), false);
            mBg = UguiMaker.newImage("mBg", transform, "shapelogic_sprite", "guanka11_station_bg", false);
            mDoor = UguiMaker.newImage("mDoor", transform, "shapelogic_sprite", "guanka11_station0-" + door_type.ToString(), false);
            mWindow = UguiMaker.newImage("mWindow", transform, "shapelogic_sprite", "guanka11_station1-" + window_type.ToString(), false);

            mBg.type = Image.Type.Sliced;
            mBg.rectTransform.sizeDelta = new Vector2(110, 150);
            mWindow.rectTransform.anchoredPosition = new Vector2(0, 33.5f);

        }

    }
    public void InitAnswer()
    {
        btn_door_left = UguiMaker.newButton("door_left", transform, "shapelogic_sprite", "guanka11_station_btn1");
        btn_door_right = UguiMaker.newButton("door_right", transform, "shapelogic_sprite", "guanka11_station_btn");
        btn_door_left.image.rectTransform.anchoredPosition = new Vector2(-90, -35);
        btn_door_right.image.rectTransform.anchoredPosition = new Vector2(90, -35);
        btn_door_left.onClick.AddListener(delegate () { OnClkBtnDoor(1); });
        btn_door_right.onClick.AddListener(delegate () { OnClkBtnDoor(-1); });


        btn_window_left = UguiMaker.newButton("window_left", transform, "shapelogic_sprite", "guanka11_station_btn1");
        btn_window_right = UguiMaker.newButton("window_right", transform, "shapelogic_sprite", "guanka11_station_btn");
        btn_window_left.image.rectTransform.anchoredPosition = new Vector2(-90, 33);
        btn_window_right.image.rectTransform.anchoredPosition = new Vector2(90, 33);
        btn_window_left.onClick.AddListener(delegate () { OnClkBtnWindow(-1); });
        btn_window_right.onClick.AddListener(delegate () { OnClkBtnWindow(1); });


        btn_top_left = UguiMaker.newButton("top_left", transform, "shapelogic_sprite", "guanka11_station_btn1");
        btn_top_right = UguiMaker.newButton("top_right", transform, "shapelogic_sprite", "guanka11_station_btn");
        btn_top_left.image.rectTransform.anchoredPosition = new Vector2(-90, 97);
        btn_top_right.image.rectTransform.anchoredPosition = new Vector2(90, 97);
        btn_top_left.onClick.AddListener(delegate () { OnClkBtnTop(-1); });
        btn_top_right.onClick.AddListener(delegate () { OnClkBtnTop(1); });



        btn_qizi_left = UguiMaker.newButton("qizi_left", transform, "shapelogic_sprite", "guanka11_station_btn1");
        btn_qizi_right = UguiMaker.newButton("qizi_right", transform, "shapelogic_sprite", "guanka11_station_btn");
        btn_qizi_left.image.rectTransform.anchoredPosition = new Vector2(-90, 143);
        btn_qizi_right.image.rectTransform.anchoredPosition = new Vector2(90, 143);
        btn_qizi_left.onClick.AddListener(delegate () { OnClkBtnQizi(-1); });
        btn_qizi_right.onClick.AddListener(delegate () { OnClkBtnQizi(1); });

        //StartCoroutine("TInitAnswer");
    }
    public void InitFrameBg()
    {
        mFrame = UguiMaker.newImage("mFrame", transform, "shapelogic_sprite", "guanka11_frame", false);
        mFrame.transform.SetAsFirstSibling();
        mFrame.type = Image.Type.Sliced;
        mFrame.rectTransform.sizeDelta = new Vector2(210, 264);
        mFrame.rectTransform.anchoredPosition = new Vector2(0, 45);

    }
    public void SetBoxEnable(bool _enable)
    {
        if (null == mBox)
        {
            mBox = gameObject.AddComponent<BoxCollider>();
            mBox.size = new Vector3(124, 250, 1);
            mBox.center = new Vector3(0, 45, 0);
        }
        mBox.enabled = _enable;

    }
    

    public void ReflushUI()
    {
        switch(mdata_door_type)
        {
            case 0:
                mDoor.rectTransform.anchoredPosition = new Vector2(29.4f, -34.5f);
                break;
            case 1:
                mDoor.rectTransform.anchoredPosition = new Vector2(0, -34.5f);
                break;
            case 2:
                mDoor.rectTransform.anchoredPosition = new Vector2(-29.4f, -34.5f);
                break;

        }
        switch(mdata_qizi_type)
        {
            case 0:
                mQizi.rectTransform.anchoredPosition = new Vector2(6.7f, 46.4f);
                break;
            case 1:
                mQizi.rectTransform.anchoredPosition = new Vector2(10.83f, 46.4f);
                break;
            case 2:
                mQizi.rectTransform.anchoredPosition = new Vector2(-11.25f, 47.75f);
                break;
        }
        mTop.rectTransform.anchoredPosition = new Vector2(mdata_top_side * 40.6f, 95.12f);
        mWindow.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka11_station1-" + mdata_window_type.ToString());
        mQizi.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka11_station3-" + mdata_qizi_type.ToString());
        mDoor.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka11_station0-" + mdata_door_type.ToString());


    }
    public void Play()
    {
        StartCoroutine("TPlay");
    }
    IEnumerator TPlay()
    {
        if(null != mFrame)
        {
            Destroy(mFrame.gameObject);
        }
        Vector3 pos0 = mTop.rectTransform.anchoredPosition3D;
        float p = 0;
        while (true)
        {
            mTop.rectTransform.anchoredPosition3D = pos0 + new Vector3(0, Mathf.Abs( Mathf.Sin(p)) * -20, 0);
            p += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        
    }



    public void OnClkBtnDoor(int move)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        //Debug.Log(move);
        if (0 > move)
            Global.instance.PlayBtnClickAnimation(btn_door_left.transform);
        else
            Global.instance.PlayBtnClickAnimation(btn_door_right.transform);
        mdata_door_type += move;
        if (0 > mdata_door_type)
            mdata_door_type = 2;
        if (2 < mdata_door_type)
            mdata_door_type = 0;

        ReflushUI();

        ShapeLogicCtl.instance.mGuankaCtl11.mStation[ShapeLogicCtl.instance.mGuankaCtl11.mdata_answer_index].mdata_door_type = mdata_door_type;
        ShapeLogicCtl.instance.mGuankaCtl11.mStation[ShapeLogicCtl.instance.mGuankaCtl11.mdata_answer_index].ReflushUI();

    }
    public void OnClkBtnWindow(int move)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        Debug.Log(move);
        if (0 > move)
            Global.instance.PlayBtnClickAnimation(btn_window_left.transform);
        else
            Global.instance.PlayBtnClickAnimation(btn_window_right.transform);

        mdata_window_type += move;
        if (0 > mdata_window_type)
            mdata_window_type = 2;
        if (2 < mdata_window_type)
            mdata_window_type = 0;

        ReflushUI();
        ShapeLogicCtl.instance.mGuankaCtl11.mStation[ShapeLogicCtl.instance.mGuankaCtl11.mdata_answer_index].mdata_window_type = mdata_window_type;
        ShapeLogicCtl.instance.mGuankaCtl11.mStation[ShapeLogicCtl.instance.mGuankaCtl11.mdata_answer_index].ReflushUI();


    }
    public void OnClkBtnTop(int move)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        Debug.Log(move);
        if (0 > move)
            Global.instance.PlayBtnClickAnimation(btn_top_left.transform);
        else
            Global.instance.PlayBtnClickAnimation(btn_top_right.transform);

        mdata_top_side += move;
        if (-1 > mdata_top_side)
            mdata_top_side = 1;
        if (1 < mdata_top_side)
            mdata_top_side = -1;
        
        ReflushUI();
        ShapeLogicCtl.instance.mGuankaCtl11.mStation[ShapeLogicCtl.instance.mGuankaCtl11.mdata_answer_index].mdata_top_side = mdata_top_side;
        ShapeLogicCtl.instance.mGuankaCtl11.mStation[ShapeLogicCtl.instance.mGuankaCtl11.mdata_answer_index].ReflushUI();


    }
    public void OnClkBtnQizi(int move)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        Debug.Log(move);
        if (0 > move)
            Global.instance.PlayBtnClickAnimation(btn_qizi_left.transform);
        else
            Global.instance.PlayBtnClickAnimation(btn_qizi_right.transform);

        mdata_qizi_type += move;
        if (0 > mdata_qizi_type)
            mdata_qizi_type = 2;
        if (2 < mdata_qizi_type)
            mdata_qizi_type = 0;

        ReflushUI();
        ShapeLogicCtl.instance.mGuankaCtl11.mStation[ShapeLogicCtl.instance.mGuankaCtl11.mdata_answer_index].mdata_qizi_type = mdata_qizi_type;
        ShapeLogicCtl.instance.mGuankaCtl11.mStation[ShapeLogicCtl.instance.mGuankaCtl11.mdata_answer_index].ReflushUI();


    }

    

    public void PlayError()
    {
        StartCoroutine("TPlayError");
    }
    IEnumerator TPlayError()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("错误");
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * i) * 5);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.localEulerAngles = Vector3.zero;


    }


    public void PlayShoot()
    {
        mdata_is_over = true;
        StartCoroutine("TPlayShoot");
    }
    IEnumerator TPlayShoot()
    {
        Image mask = UguiMaker.newImage("mask", transform.parent, "public", "white", false);
        mask.rectTransform.sizeDelta = new Vector2(160, 225);
        mask.rectTransform.anchorMin = new Vector2(0.5f, 0);
        mask.rectTransform.anchorMax = new Vector2(0.5f, 0);
        mask.rectTransform.pivot = new Vector2(0.5f, 0);
        mask.rectTransform.anchoredPosition3D = mRtran.anchoredPosition3D + new Vector3(0, -68, 0);
        transform.SetParent(mask.transform);
        mRtran.anchoredPosition = new Vector2(0, 68);
        mask.gameObject.AddComponent<Mask>().showMaskGraphic = false;

        Image green_line = UguiMaker.newImage("green_line", mask.transform, "public", "white", false);
        green_line.rectTransform.anchorMin = new Vector2(0.5f, 1);
        green_line.rectTransform.anchorMax = new Vector2(0.5f, 1);
        green_line.rectTransform.sizeDelta = new Vector2(140, 4);
        green_line.rectTransform.anchoredPosition = new Vector2(0, -2);
        green_line.color = new Color(0, 1, 0, 1);


        AudioSource sound = gameObject.AddComponent<AudioSource>();
        sound.clip = ResManager.GetClip("shapelogic_sound", "电流平均");
        sound.loop = true;
        sound.Play();

        for (float i = 0; i < 1f; i += 0.02f)
        {
            mask.rectTransform.sizeDelta = new Vector2(160, 225 * (1 - i));
            yield return new WaitForSeconds(0.01f);
        }

        sound.Stop();

    }

}
