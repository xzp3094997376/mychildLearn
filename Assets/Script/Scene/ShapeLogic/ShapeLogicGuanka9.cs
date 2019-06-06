using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka9 : MonoBehaviour
{
    Button mOK;
    Image mBg0;
    Image mSelectHead, mSelectBody, mSelectTail, mSelectTailType;//mSelectEar, mSelectEye, mSelectMao
    RectTransform mRtranHead, mRtranBody, mRtranTail, mRtranTailType;//mRtranEar,, mRtranMao , mRtranEye
    ParticleSystem mEffectOK;

    List<ShapeLogicGuanka9_Station> mStation;

    int mdata_answer_index;
    int mdata_head = -1;
    int mdata_body = -1;
    int mdata_tail = -1;
    int mdata_tail_type = -1;


    int temp_updata_state = 0;
    void Start () {
	
	}

    void Update()
    {
        switch (temp_updata_state)
        {
            case 1:
                UpdateOver();
                break;

        }

    }
    void UpdateOver()
    {


        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach (RaycastHit h in hits)
            {
                ShapeLogicGuanka9_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka9_Station>();
                if (null != com)
                {
                    com.SetBoxEnable(false);
                    com.PlayShoot();
                    bool is_over = true;
                    for (int i = 0; i < mStation.Count; i++)
                    {
                        if (!mStation[i].mdata_is_over)
                        {
                            is_over = false;
                            break;
                        }
                    }
                    if (is_over)
                    {
                        Invoke("EndGame", 1f);
                    }
                    break;
                }
            }

        }

    }

    public void BeginGame()
    {
        gameObject.SetActive(true);
        StartCoroutine(TBeginGame());
    }
    public void EndGame()
    {
        StartCoroutine(TEndGame());
    }
    IEnumerator TBeginGame()
    {
        temp_updata_state = 0;
        mdata_head = -1;
        mdata_body = -1;
        mdata_tail = -1;
        mdata_tail_type = -1;

        //背景
        mBg0 = UguiMaker.newImage("mBg0", transform, "public", "white", false);
        mBg0.color = new Color32(255, 160, 207, 255);
        mBg0.rectTransform.sizeDelta = new Vector2(1423, 800);

        //动物
        mdata_answer_index = Random.Range(0, 1000) % 9;
        mStation = new List<ShapeLogicGuanka9_Station>();
        int[,] temp_head = Common.GetArrayMuteId(3);
        int[,] temp_body = Common.GetArrayMuteId(3);
        int[,] temp_tail_f = Common.GetArrayMuteId(3);
        int[,] temp_tail_type = Common.GetArrayMuteId(3);


        List<Vector3> poss = Common.PosSortByWidth(700, 3, 200);
        for(int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                int index = j * 3 + i;
                
                mStation.Add(UguiMaker.newGameObject("station", transform).AddComponent<ShapeLogicGuanka9_Station>());
                mStation[index].Init( temp_head[j, i],  temp_body[j, i], temp_tail_f[j,i], temp_tail_type[j, i]);
                mStation[index].mRtran.anchoredPosition3D = poss[i] + new Vector3(-230, j * -240, 0);
                if(index == mdata_answer_index)
                {
                    mStation[index].InitAnswer();
                }
                else
                {
                    mStation[index].ReflushUI_PlayTail(false);
                    mStation[index].Jump(j % 2 != 0);

                }

            }

        }

        mRtranHead = UguiMaker.newGameObject("mRtranHead", transform).GetComponent<RectTransform>();
        mRtranBody = UguiMaker.newGameObject("mRtranBody", transform).GetComponent<RectTransform>();
        mRtranTail = UguiMaker.newGameObject("mRtranTail", transform).GetComponent<RectTransform>();
        mRtranTailType = UguiMaker.newGameObject("mRtranTailType", transform).GetComponent<RectTransform>();
        mRtranHead.anchoredPosition = new Vector2(1000, 260.63f);
        mRtranBody.anchoredPosition = new Vector2(1000, 63.1f);
        mRtranTail.anchoredPosition = new Vector2(1000, -124.2f);
        mRtranTailType.anchoredPosition = new Vector2(1000, -245.5f);

        
        Button btn_head0 = UguiMaker.newButton("btn_head0", mRtranHead, "shapelogic_sprite", "guanka9_head0");
        Button btn_head1 = UguiMaker.newButton("btn_head1", mRtranHead, "shapelogic_sprite", "guanka9_head1");
        Button btn_head2 = UguiMaker.newButton("btn_head2", mRtranHead, "shapelogic_sprite", "guanka9_head2");
        Button btn_body0 = UguiMaker.newButton("btn_body0", mRtranBody, "shapelogic_sprite", "guanka9_body0");
        Button btn_body1 = UguiMaker.newButton("btn_body1", mRtranBody, "shapelogic_sprite", "guanka9_body1");
        Button btn_body2 = UguiMaker.newButton("btn_body2", mRtranBody, "shapelogic_sprite", "guanka9_body2");
        Button btn_tail0 = UguiMaker.newButton("btn_tail0", mRtranTail, "shapelogic_sprite", "guanka9_tail_f0");
        Button btn_tail1 = UguiMaker.newButton("btn_tail1", mRtranTail, "shapelogic_sprite", "guanka9_tail_f1");
        Button btn_tail2 = UguiMaker.newButton("btn_tail1", mRtranTail, "shapelogic_sprite", "guanka9_tail_f2");
        Button btn_tail_type0 = UguiMaker.newButton("btn_tail_type0", mRtranTailType, "shapelogic_sprite", "guanka9_tail0");
        Button btn_tail_type1 = UguiMaker.newButton("btn_tail_type1", mRtranTailType, "shapelogic_sprite", "guanka9_tail1");
        Button btn_tail_type2 = UguiMaker.newButton("btn_tail_type2", mRtranTailType, "shapelogic_sprite", "guanka9_tail2");
        btn_head0.onClick.AddListener(ClkHead0);
        btn_head1.onClick.AddListener(ClkHead1);
        btn_head2.onClick.AddListener(ClkHead2);
        btn_body0.onClick.AddListener(ClkBody0);
        btn_body1.onClick.AddListener(ClkBody1);
        btn_body2.onClick.AddListener(ClkBody2);
        btn_tail0.onClick.AddListener(ClkTail0);
        btn_tail1.onClick.AddListener(ClkTail1);
        btn_tail2.onClick.AddListener(ClkTail2);
        btn_tail_type0.onClick.AddListener(ClkTailType0);
        btn_tail_type1.onClick.AddListener(ClkTailType1);
        btn_tail_type2.onClick.AddListener(ClkTailType2);

        poss = Common.PosSortByWidth(500, 3, 0);
        poss = Common.PosSortByWidth(450, 3, 0);
        btn_head0.image.rectTransform.anchoredPosition = poss[0];
        btn_head1.image.rectTransform.anchoredPosition = poss[1];
        btn_head2.image.rectTransform.anchoredPosition = poss[2];
        poss = Common.PosSortByWidth(300, 3, 0);
        poss = Common.PosSortByWidth(450, 3, 0);
        poss = Common.PosSortByWidth(450, 3, 0);
        btn_body0.image.rectTransform.anchoredPosition = poss[0];
        btn_body1.image.rectTransform.anchoredPosition = poss[1];
        btn_body2.image.rectTransform.anchoredPosition = poss[2];
        poss = Common.PosSortByWidth(400, 3, 0);
        btn_tail0.image.rectTransform.anchoredPosition = poss[0];
        btn_tail1.image.rectTransform.anchoredPosition = poss[1];
        btn_tail2.image.rectTransform.anchoredPosition = poss[2];
        poss = Common.PosSortByWidth(250, 3, 0);
        btn_tail_type0.image.rectTransform.anchoredPosition = poss[0];
        btn_tail_type1.image.rectTransform.anchoredPosition = poss[1];
        btn_tail_type2.image.rectTransform.anchoredPosition = poss[2];


        mSelectHead = UguiMaker.newImage("mSelectHead", mRtranHead, "shapelogic_sprite", "guanka8_select", false);
        mSelectBody = UguiMaker.newImage("mSelectBody", mRtranBody, "shapelogic_sprite", "guanka8_select", false);
        mSelectTail = UguiMaker.newImage("mSelectTail", mRtranTail, "shapelogic_sprite", "guanka8_select", false);
        mSelectTailType = UguiMaker.newImage("mSelectTailType", mRtranTailType, "shapelogic_sprite", "guanka8_select", false);


        mSelectHead.type = Image.Type.Sliced;
        mSelectBody.type = Image.Type.Sliced;
        mSelectTail.type = Image.Type.Sliced;
        mSelectTailType.type = Image.Type.Sliced;

        mSelectHead.gameObject.SetActive(false);
        mSelectBody.gameObject.SetActive(false);
        mSelectTail.gameObject.SetActive(false);
        mSelectTailType.gameObject.SetActive(false);

        yield return new WaitForSeconds(4);
        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mRtranHead.anchoredPosition = Vector2.Lerp(new Vector2(1000, 190), new Vector2(370, 260.63f), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mRtranHead.anchoredPosition = new Vector2(370, 260.63f);
        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mRtranBody.anchoredPosition = Vector2.Lerp(new Vector2(1000, -75), new Vector2(370, 63.1f), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        mRtranBody.anchoredPosition = new Vector2(370, 63.1f);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mRtranTail.anchoredPosition = Vector2.Lerp(new Vector2(1000, -194), new Vector2(370, -124.2f), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        mRtranTail.anchoredPosition = new Vector2(370, -124.2f);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mRtranTailType.anchoredPosition = Vector2.Lerp(new Vector2(1000, -266), new Vector2(370, -245.5f), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mRtranTailType.anchoredPosition = new Vector2(370, -245.5f);


        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        mOK = UguiMaker.newButton("mOK", transform, "shapelogic_sprite", "btn_up");
        mOK.onClick.AddListener(OnClkOK);
        mOK.transition = Selectable.Transition.None;
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mOK.image.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(353.62f, -460), new Vector2(353.62f, -343.87f), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mOK.image.rectTransform.anchoredPosition = new Vector2(353.62f, -343.87f);


        ShapeLogicCtl.instance.mSound.PlayTipListDefaultAb(
            new List<string>() { "9观察小动物们的特征和排列顺序", "选择合适的小图案组成正确的大图案吧" },
            new List<float>() { 1, 1 }, true);


    }
    IEnumerator TEndGame()
    {
        yield return new WaitForSeconds(2f);
        ShapeLogicCtl.instance.GameNext();
        Color bg_color0 = mBg0.color;
        Color bg_color1 = mBg0.color;
        bg_color1.a = 0;

        for (float j = 0; j < 1f; j += 0.03f)
        {
            Color cor = Color.Lerp(new Color(1, 1, 1, 0.5f), new Color(1, 1, 1, 0), j);
            mBg0.color = Color.Lerp(bg_color0, bg_color1, j);
            yield return new WaitForSeconds(0.01f);
        }

        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        gameObject.SetActive(false);

    }
    IEnumerator TOver()
    {
        //ShapeLogicCtl.instance.DestroyLine();
        yield return new WaitForSeconds(0.8f);
        ShapeLogicCtl.instance.mSound.PlayShort("胜利通关音乐", 1);
        temp_updata_state = 1;
        for (int i = 0; i < mStation.Count; i++)
        {
            mStation[i].SetBoxEnable(true);
            mStation[i].PlayCorrect();
        }
        
        //Vector3 pos0 = mRtranEar.anchoredPosition3D;
        Vector3 pos1 = mRtranHead.anchoredPosition3D;
        //Vector3 pos2 = mRtranEye.anchoredPosition3D;
        Vector3 pos3 = mRtranBody.anchoredPosition3D;
        Vector3 pos4 = mRtranTail.anchoredPosition3D;
        Vector3 pos5 = mRtranTailType.anchoredPosition3D;
        //Vector3 pos6 = mRtranMao.anchoredPosition3D;

        //Vector3 _pos0 = mRtranEar.anchoredPosition3D + new Vector3(600, 0, 0);
        Vector3 _pos1 = mRtranHead.anchoredPosition3D + new Vector3(600, 0, 0);
        //Vector3 _pos2 = mRtranEye.anchoredPosition3D + new Vector3(600, 0, 0);
        Vector3 _pos3 = mRtranBody.anchoredPosition3D + new Vector3(600, 0, 0);
        Vector3 _pos4 = mRtranTail.anchoredPosition3D + new Vector3(600, 0, 0);
        Vector3 _pos5 = mRtranTailType.anchoredPosition3D + new Vector3(600, 0, 0);
        //Vector3 _pos6 = mRtranMao.anchoredPosition3D + new Vector3(600, 0, 0);


        for (float i = 0; i < 1f; i += 0.05f)
        {
            //mRtranEar.anchoredPosition = Vector2.Lerp(pos0, _pos0, Mathf.Sin(Mathf.PI * 0.5f * i));
            mRtranHead.anchoredPosition = Vector2.Lerp(pos1, _pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
            //mRtranEye.anchoredPosition = Vector2.Lerp(pos2, _pos2, Mathf.Sin(Mathf.PI * 0.5f * i));
            mRtranBody.anchoredPosition = Vector2.Lerp(pos3, _pos3, Mathf.Sin(Mathf.PI * 0.5f * i));
            mRtranTail.anchoredPosition = Vector2.Lerp(pos4, _pos4, Mathf.Sin(Mathf.PI * 0.5f * i));
            mRtranTailType.anchoredPosition = Vector2.Lerp(pos5, _pos5, Mathf.Sin(Mathf.PI * 0.5f * i));
            //mRtranMao.anchoredPosition = Vector2.Lerp(pos6, _pos6, Mathf.Sin(Mathf.PI * 0.5f * i));

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1f);
        for (float i = 0; i < 1f; i += 0.08f)
        {
            mOK.image.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(353.62f, -350), new Vector2(353.62f, -460), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

    }


    public void OnClkOK()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("button_down");
        mOK.image.sprite = ResManager.GetSprite("shapelogic_sprite", "btn_down");
        mOK.enabled = false;

        bool correct = true;
         if (mdata_head == -1||
             mdata_body == -1||
             mdata_tail == -1||
             mdata_tail_type == -1)
        {
            correct = false;
        }

         if(!mStation[mdata_answer_index].GetCorrect())
        {
            correct = false;
        }



        if (correct)
        {
            ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("04-星星-02");
            Debug.Log("回答正确!");
            mEffectOK = ResManager.GetPrefab("effect_star0", "effect_star1").GetComponent<ParticleSystem>();
            UguiMaker.InitGameObj(mEffectOK.gameObject, transform, "effect_ok", mOK.image.rectTransform.anchoredPosition3D, Vector3.one);
            mEffectOK.Play();
            StartCoroutine("TOver");

        }
        else
        {
            mStation[mdata_answer_index].PlayError();
            Debug.Log("回答错误!");
            Invoke("InvokeOnClkOk", 0.5f);
        }
    }
    void InvokeOnClkOk()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("button_up");
        mOK.image.sprite = ResManager.GetSprite("shapelogic_sprite", "btn_up");
        mOK.enabled = true;

    }

    //public void ClkEar(int type)
    //{
    //    ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
    //    mdata_ear = type;
    //    mSelectEar.gameObject.SetActive(true);
    //    switch (type)
    //    {
    //        case 0:
    //            mSelectEar.rectTransform.anchoredPosition = new Vector2(-167.89f, 1.1f);
    //            mSelectEar.rectTransform.sizeDelta = new Vector2(141, 95);
    //            break;
    //        case 1:
    //            mSelectEar.rectTransform.anchoredPosition = new Vector2(-0.3f, 1.1f);
    //            mSelectEar.rectTransform.sizeDelta = new Vector2(141, 95);
    //            break;
    //        case 2:
    //            mSelectEar.rectTransform.anchoredPosition = new Vector2(166, 1.1f);
    //            mSelectEar.rectTransform.sizeDelta = new Vector2(141, 95);
    //            break;
    //        case 3:
    //            mSelectEar.rectTransform.anchoredPosition = new Vector2(187.3f, 1.1f);
    //            mSelectEar.rectTransform.sizeDelta = new Vector2(141, 95);
    //            break;
    //    }
    //    mStation[mdata_answer_index].SetEar(type, mdata_head);

    //}
    //public void ClkEar0()
    //{
    //    ClkEar(0);
    //}
    //public void ClkEar1()
    //{
    //    ClkEar(1);
    //}
    //public void ClkEar2()
    //{
    //    ClkEar(2);
    //}
    //public void ClkEar3()
    //{
    //    ClkEar(3);
    //}

    public void ClkHead(int type)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        mdata_head = type;
        mSelectHead.gameObject.SetActive(true);
        switch (type)
        {
            case 0:
                mSelectHead.rectTransform.anchoredPosition = new Vector2(-150, -5.6f);
                mSelectHead.rectTransform.sizeDelta = new Vector2(159.4f, 121.16f);
                break;
            case 1:
                mSelectHead.rectTransform.anchoredPosition = new Vector2(-0.1f, -5.6f);
                mSelectHead.rectTransform.sizeDelta = new Vector2(159.4f, 121.16f);
                break;
            case 2:
                mSelectHead.rectTransform.anchoredPosition = new Vector2(149.9f, -5.6f);
                mSelectHead.rectTransform.sizeDelta = new Vector2(159.4f, 121.16f);
                break;
        }
        mStation[mdata_answer_index].SetHead(type);

    }
    public void ClkHead0()
    {
        ClkHead(0);
    }
    public void ClkHead1()
    {
        ClkHead(1);
    }
    public void ClkHead2()
    {
        ClkHead(2);
    }


    //public void ClkEye(int type)
    //{
    //    ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
    //    mdata_eye = type;
    //    mSelectEye.gameObject.SetActive(true);
    //    switch (type)
    //    {
    //        case 0:
    //            mSelectEye.rectTransform.anchoredPosition = new Vector2(-100, 0);
    //            mSelectEye.rectTransform.sizeDelta = new Vector2(105.7f, 84.8f);
    //            break;
    //        case 1:
    //            mSelectEye.rectTransform.anchoredPosition = new Vector2(0, 0);
    //            mSelectEye.rectTransform.sizeDelta = new Vector2(105.7f, 84.8f);
    //            break;
    //        case 2:
    //            mSelectEye.rectTransform.anchoredPosition = new Vector2(100, 0);
    //            mSelectEye.rectTransform.sizeDelta = new Vector2(105.7f, 84.8f);
    //            break;
    //    }
    //    mStation[mdata_answer_index].SetEye(type, mdata_head);
    //}
    //public void ClkEye0()
    //{
    //    ClkEye(0);
    //}
    //public void ClkEye1()
    //{
    //    ClkEye(1);
    //}
    //public void ClkEye2()
    //{
    //    ClkEye(2);
    //}

    //public void ClkMao(int type)
    //{
    //    ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
    //    mdata_mao = type;
    //    mSelectMao.gameObject.SetActive(true);
    //    switch (type)
    //    {
    //        case 0:
    //            mSelectMao.rectTransform.anchoredPosition = new Vector2(-152.49f, 2.7f);
    //            mSelectMao.rectTransform.sizeDelta = new Vector2(156.3f, 86.4f);
    //            break;
    //        case 1:
    //            mSelectMao.rectTransform.anchoredPosition = new Vector2(0, 2.7f);
    //            mSelectMao.rectTransform.sizeDelta = new Vector2(156.3f, 86.4f);
    //            break;
    //        case 2:
    //            mSelectMao.rectTransform.anchoredPosition = new Vector2(147.63f, 2.7f);
    //            mSelectMao.rectTransform.sizeDelta = new Vector2(156.3f, 86.4f);
    //            break;
    //    }
    //    mStation[mdata_answer_index].SetMao(type, mdata_head);
    //}
    //public void ClkMao0()
    //{
    //    ClkMao(0);
    //}
    //public void ClkMao1()
    //{
    //    ClkMao(1);
    //}
    //public void ClkMao2()
    //{
    //    ClkMao(2);
    //}

    public void ClkBody(int type)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        mdata_body = type;
        mSelectBody.gameObject.SetActive(true);
        switch (type)
        {
            case 0:
                mSelectBody.rectTransform.anchoredPosition = new Vector2(-150.46f, 2.1f);
                mSelectBody.rectTransform.sizeDelta = new Vector2(161.6f, 190.8f);
                break;
            case 1:
                mSelectBody.rectTransform.anchoredPosition = new Vector2(0, 2.1f);
                mSelectBody.rectTransform.sizeDelta = new Vector2(161.6f, 190.8f);
                break;
            case 2:
                mSelectBody.rectTransform.anchoredPosition = new Vector2(150.46f, 2.1f);
                mSelectBody.rectTransform.sizeDelta = new Vector2(161.6f, 190.8f);
                break;
        }
        mStation[mdata_answer_index].SetBody(type);
    }
    public void ClkBody0()
    {
        ClkBody(0);
    }
    public void ClkBody1()
    {
        ClkBody(1);
    }
    public void ClkBody2()
    {
        ClkBody(2);
    }

    public void ClkTail(int type)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        mdata_tail = type;
        mSelectTail.gameObject.SetActive(true);
        switch (type)
        {
            case 0:
                mSelectTail.rectTransform.anchoredPosition = new Vector2(-132.3f, -0.99f);
                mSelectTail.rectTransform.sizeDelta = new Vector2(154.17f, 91f);
                break;
            case 1:
                mSelectTail.rectTransform.anchoredPosition = new Vector2(-1.3f, -0.99f);
                mSelectTail.rectTransform.sizeDelta = new Vector2(154.17f, 91f);
                break;
            case 2:
                mSelectTail.rectTransform.anchoredPosition = new Vector2(134.82f, -0.99f);
                mSelectTail.rectTransform.sizeDelta = new Vector2(154.17f, 91f);
                break;
        }
        mStation[mdata_answer_index].SetTail(type);
    }
    public void ClkTail0()
    {
        ClkTail(0);
    }
    public void ClkTail1()
    {
        ClkTail(1);
    }
    public void ClkTail2()
    {
        ClkTail(2);
    }

    public void ClkTailType(int type)
    {
        ShapeLogicCtl.instance.mSound.PlayShort("inputnumclick");
        mdata_tail_type = type;
        mSelectTailType.gameObject.SetActive(true);
        switch (type)
        {
            case 0:
                mSelectTailType.rectTransform.anchoredPosition = new Vector2(-94.21f, 0);
                mSelectTailType.rectTransform.sizeDelta = new Vector2(80, 80);
                break;
            case 1:
                mSelectTailType.rectTransform.anchoredPosition = new Vector2(0, 0);
                mSelectTailType.rectTransform.sizeDelta = new Vector2(80, 80);
                break;
            case 2:
                mSelectTailType.rectTransform.anchoredPosition = new Vector2(83.29f, 0);
                mSelectTailType.rectTransform.sizeDelta = new Vector2(80, 80);
                break;
        }
        mStation[mdata_answer_index].SetTailType(type);
    }
    public void ClkTailType0()
    {
        ClkTailType(0);
    }
    public void ClkTailType1()
    {
        ClkTailType(1);
    }
    public void ClkTailType2()
    {
        ClkTailType(2);
    }


}
