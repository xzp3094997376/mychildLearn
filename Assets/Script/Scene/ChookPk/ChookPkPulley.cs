using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChookPkPulley : MonoBehaviour {

    Image mOK0, mOK1, mOK2;
    Image mDeng, mDengLight;
    Image mHualunLeft, mHualunRight;
    Image mShenziLeft, mShenziRight, mShenziMid;
    GameObject mOKEffect { get; set; }
    public ParticleSystem mMathcEffect_L, mMathcEffect_R;
    public RectTransform mLanziLeft, mLanziRight;
    public List<ChookPkChook> mLeftChooks = new List<ChookPkChook>();
    public List<ChookPkChook> mRightChooks = new List<ChookPkChook>();
    GuideHandCtl mHand { get; set; }

    int shengzi_mid = 200;
    int shengzi_down = 347;
    int shengzi_up = 53;

    void Awake()
    {

        temp_soft_poss = Common.PosSortByWidth(265, 5, -119);
        temp_heavy_poss = Common.PosSortByWidth(265, 3, -119);
        temp_canuse_pos = new Dictionary<string, bool>();
        temp_canuse_pos.Add("left-soft-0", true);
        temp_canuse_pos.Add("left-soft-1", true);
        temp_canuse_pos.Add("left-soft-2", true);
        temp_canuse_pos.Add("left-soft-3", true);
        temp_canuse_pos.Add("left-soft-4", true);
        temp_canuse_pos.Add("left-heavy-0", true);
        temp_canuse_pos.Add("left-heavy-1", true);
        temp_canuse_pos.Add("left-heavy-2", true);
        temp_canuse_pos.Add("right-soft-0", true);
        temp_canuse_pos.Add("right-soft-1", true);
        temp_canuse_pos.Add("right-soft-2", true);
        temp_canuse_pos.Add("right-soft-3", true);
        temp_canuse_pos.Add("right-soft-4", true);
        temp_canuse_pos.Add("right-heavy-0", true);
        temp_canuse_pos.Add("right-heavy-1", true);
        temp_canuse_pos.Add("right-heavy-2", true);
        

    }
    void Start ()
    {
        mShenziMid = UguiMaker.newImage("shenzi_mid", transform, "chookpk_sprite", "shengzi_mid", false);
        //mShenziMid.type = Image.Type.Tiled;
        mShenziMid.rectTransform.anchoredPosition = new Vector2(0, 0);
        //mShenziMid.rectTransform.sizeDelta = new Vector2(420, 24);

        

        GameObject left = UguiMaker.newGameObject("left", transform);
        GameObject right = UguiMaker.newGameObject("right", transform);
        left.transform.localPosition = new Vector3(-313, -23, 0);
        right.transform.localPosition = new Vector3(313, -23, 0);
        


        mShenziLeft = UguiMaker.newImage("shenzi_left", left.transform, "chookpk_sprite", "shengzi", false);
        mShenziRight = UguiMaker.newImage("shenzi_right", right.transform, "chookpk_sprite", "shengzi", false);
        mShenziLeft.rectTransform.pivot = new Vector2(0.5f, 1);
        mShenziRight.rectTransform.pivot = new Vector2(0.5f, 1);
        mShenziLeft.type = Image.Type.Tiled;
        mShenziRight.type = Image.Type.Tiled;
        mShenziLeft.rectTransform.anchoredPosition = new Vector2(0, 0);
        mShenziRight.rectTransform.anchoredPosition = new Vector2(0, 0);
        mShenziLeft.rectTransform.sizeDelta = new Vector2(6, shengzi_mid);
        mShenziRight.rectTransform.sizeDelta = new Vector2(6, shengzi_mid);


        mLanziLeft = UguiMaker.newGameObject("down_left", mShenziLeft.transform).GetComponent<RectTransform>();
        mLanziRight = UguiMaker.newGameObject("down_right", mShenziRight.transform).GetComponent<RectTransform>();
        mLanziLeft.anchorMin = new Vector2(0.5f, 0);
        mLanziLeft.anchorMax = new Vector2(0.5f, 0);
        mLanziRight.anchorMin = new Vector2(0.5f, 0);
        mLanziRight.anchorMax = new Vector2(0.5f, 0);
        mLanziLeft.anchoredPosition = Vector2.zero;
        mLanziRight.anchoredPosition = Vector2.zero;



        Image lanzi_left0 = UguiMaker.newImage("0", mLanziLeft.transform, "chookpk_sprite", "lanzi0", true);
        Image lanzi_right0 = UguiMaker.newImage("0", mLanziRight.transform, "chookpk_sprite", "lanzi0", true);
        lanzi_left0.rectTransform.pivot = new Vector2(0.5f, 1);
        lanzi_right0.rectTransform.pivot = new Vector2(0.5f, 1);
        lanzi_left0.rectTransform.anchoredPosition = new Vector2(-4, 6.16f);
        lanzi_right0.rectTransform.anchoredPosition = new Vector2(-4, 6.16f);
        lanzi_left0.rectTransform.sizeDelta = new Vector2(265, 130);
        lanzi_right0.rectTransform.sizeDelta = new Vector2(265, 130);
        Button lanzi_l = lanzi_left0.gameObject.AddComponent<Button>();
        Button lanzi_r = lanzi_right0.gameObject.AddComponent<Button>();
        lanzi_l.transition = Selectable.Transition.None;
        lanzi_r.transition = Selectable.Transition.None;
        BoxCollider boxl = lanzi_l.gameObject.AddComponent<BoxCollider>();
        BoxCollider boxr = lanzi_r.gameObject.AddComponent<BoxCollider>();
        boxl.size = new Vector3(256, 130, 1);
        boxr.size = new Vector3(256, 130, 1);
        boxl.center = new Vector3(0, -65, 0);
        boxr.center = new Vector3(0, -65, 0);
        //lanzi_l.onClick.AddListener(OnClkLeft);
        //lanzi_r.onClick.AddListener(OnClkRight);


        mHualunLeft = UguiMaker.newImage("lunzi_left", left.transform, "chookpk_sprite", "hualun", false);
        mHualunRight = UguiMaker.newImage("lunzi_right", right.transform, "chookpk_sprite", "hualun", false);
        mHualunLeft.rectTransform.anchoredPosition = new Vector2(29, 0);
        mHualunRight.rectTransform.anchoredPosition = new Vector2(-29, 0);
        //mHualunLeft.transform.localPosition = new Vector3();

        mDeng = UguiMaker.newImage("mDeng", transform, "chookpk_sprite", "deng_close", false);
        mDengLight = UguiMaker.newImage("mDengLight", transform, "chookpk_sprite", "deng_light", false);
        mDeng.rectTransform.anchoredPosition = new Vector2(0, -116);
        mDengLight.rectTransform.anchoredPosition = new Vector2(0, -116);
        mDengLight.gameObject.SetActive(false);


        mOK0 = UguiMaker.newImage("mOK0", transform, "chookpk_sprite", "btn_ok0", false);
        mOK1 = UguiMaker.newImage("mOK1", transform, "chookpk_sprite", "btn_ok1", false);
        mOK2 = UguiMaker.newImage("mOK2", transform, "chookpk_sprite", "btn_ok2-0", false);
        mOK1.gameObject.AddComponent<BoxCollider>().size = new Vector3(197, 108, 1);
        mOK1.transform.localEulerAngles = new Vector3(0, 0, 18);
        //mOK2.color = new Color32(237, 121, 72, 255);





        ChookPkCtl.instance.temp_start_count--;

    }


    //bool temp_ok_round = false;//ok按钮是否旋转
    //bool temp_ok_click = false;//ok按钮可点
    //float temp_angle_speed_l = 0;
    //float temp_angle_speed_r = 0;
    //float temp_angle_l = 0;
    //float temp_angle_r = 0;
    Transform temp_tran_lanzi = null;
    float temp_A_l = 0;//振幅
    float temp_A_r = 0;
    float temp_T_l = 0;//时间
    float temp_T_r = 0;//时间
    
    void Update ()
    {

        if (ChookPkCtl.instance.mState == ChookPkCtl.State.match_weight || ChookPkCtl.instance.mState == ChookPkCtl.State.select_weight)
        {
            if (Input.GetMouseButtonDown(0))
            {
                temp_tran_lanzi = null;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, 100);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject.transform.parent.parent.gameObject.name.Contains("shenzi_"))
                    {
                        temp_tran_lanzi = hit.collider.gameObject.transform.parent.parent;
                        //Debug.LogError("temp_tran_lanzi.gameObject.name=" + temp_tran_lanzi.gameObject.name);
                        break;
                    }
                }
            }
            else if (Input.GetMouseButton(0) && null != temp_tran_lanzi)
            {
                Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - temp_tran_lanzi.position;
                dir.z = 0;
                float angle = Vector3.Angle(Vector3.down, dir);
                if (angle > 40)
                    angle = 40;
                if(Vector3.Cross(dir, Vector3.down).z > 0)
                {
                    angle *= -1;
                }
                else
                {
                }
                temp_tran_lanzi.localEulerAngles = new Vector3(0, 0, angle);
                //Debug.LogError(temp_tran_lanzi.gameObject.name + " angle=" + angle);
            }

            if (Input.GetMouseButtonUp(0) && null != temp_tran_lanzi)
            {
                if(temp_tran_lanzi == mShenziLeft.transform)
                {
                    Vector3 angle180 = Common.Parse180(mShenziLeft.transform.localEulerAngles);
                    temp_A_l = Mathf.Abs(angle180.z);
                    if (0 < angle180.z)
                        temp_T_l = Mathf.PI * 0.5f;
                    else
                        temp_T_l = Mathf.PI * 1.5f;
                }
                else
                {
                    Vector3 angle180 = Common.Parse180(mShenziRight.transform.localEulerAngles);
                    temp_A_r = Mathf.Abs(angle180.z);
                    if (0 < angle180.z)
                        temp_T_r = Mathf.PI * 0.5f;
                    else
                        temp_T_r = Mathf.PI * 1.5f;
                }
                temp_tran_lanzi = null;
                //Debug.LogError("temp_tran_lanzi.gameObject.name=null");
            }

        }



        if (temp_tran_lanzi != mShenziLeft.transform)
        {
            mShenziLeft.transform.localEulerAngles = new Vector3(0, 0, temp_A_l * Mathf.Sin(temp_T_l));  
            //Debug.LogError((temp_A_l * Mathf.Sin(temp_T_l)) + " left=" + mShenziLeft.transform.localEulerAngles);
            temp_T_l += 0.1f;
            temp_A_l *= 0.99f;
        }
        if (temp_tran_lanzi != mShenziRight.transform)
        {
            mShenziRight.transform.localEulerAngles = new Vector3(0, 0, temp_A_r * Mathf.Sin(temp_T_r));
            //Debug.LogError((temp_A_r * Mathf.Sin(temp_T_r)) + " right=" + mShenziRight.transform.localEulerAngles);
            temp_T_r += 0.1f;
            temp_A_r *= 0.99f;
        }

        //mShenziRight.transform.localEulerAngles += new Vector3(0, 0, temp_angle_speed_r);


    }


    List<Vector3> temp_soft_poss = null;
    List<Vector3> temp_heavy_poss = null;
    Dictionary<string, bool> temp_canuse_pos = null;

    public Vector2 GetRandomPos(string flag, int chook_type)
    {
        if(null == temp_soft_poss)
        {
            temp_soft_poss = Common.PosSortByWidth(265, 5, -119);
            temp_heavy_poss = Common.PosSortByWidth(265, 3, -119);
            temp_canuse_pos = new Dictionary<string, bool>();
            temp_canuse_pos.Add("left-soft-0", true);
            temp_canuse_pos.Add("left-soft-1", true);
            temp_canuse_pos.Add("left-soft-2", true);
            temp_canuse_pos.Add("left-soft-3", true);
            temp_canuse_pos.Add("left-soft-4", true);
            temp_canuse_pos.Add("left-heavy-0", true);
            temp_canuse_pos.Add("left-heavy-1", true);
            temp_canuse_pos.Add("left-heavy-2", true);
            temp_canuse_pos.Add("right-soft-0", true);
            temp_canuse_pos.Add("right-soft-1", true);
            temp_canuse_pos.Add("right-soft-2", true);
            temp_canuse_pos.Add("right-soft-3", true);
            temp_canuse_pos.Add("right-soft-4", true);
            temp_canuse_pos.Add("right-heavy-0", true);
            temp_canuse_pos.Add("right-heavy-1", true);
            temp_canuse_pos.Add("right-heavy-2", true);

        }
        int index = 0;
        switch (flag)
        {
            case "left":
                if(chook_type == 2)
                {
                    for(int i = 0; i < mLeftChooks.Count; i++)
                    {
                        if (mLeftChooks[i].mType == 2)
                            index++;
                    }
                    temp_canuse_pos["left-soft-" + index] = false;
                    return temp_soft_poss[4 - index];
                }
                else
                {
                    for (int i = 0; i < mLeftChooks.Count; i++)
                    {
                        if (mLeftChooks[i].mType != 2)
                            index++;
                    }
                    temp_canuse_pos["left-heavy-" + index] = false;
                    return temp_heavy_poss[2 - index];
                }
            case "right":
                if (chook_type == 2)
                {
                    for (int i = 0; i < mRightChooks.Count; i++)
                    {
                        if (mRightChooks[i].mType == 2)
                            index++;
                    }
                    temp_canuse_pos["right-soft-" + index] = false;
                    return temp_soft_poss[index];
                }
                else
                {
                    for (int i = 0; i < mRightChooks.Count; i++)
                    {
                        if (mRightChooks[i].mType != 2)
                            index++;
                    }
                    temp_canuse_pos["right-heavy-" + index] = false;
                    return temp_heavy_poss[index];
                }
                //return new Vector2(Random.Range(0, 130), -119);
        }
        return Vector2.zero;
    }
    public Vector2 GetStandPos(string flag, ChookPkChook chook)
    {

        switch (flag)
        {
            case "left":
                if (chook.mType == 2)
                {
                    for(int i = 4; i >= 0; i--)
                    {
                        string key = "left-soft-" + i;
                        if(temp_canuse_pos[key])
                        {
                            temp_canuse_pos[key] = false;
                            chook.mPosInPulleyIndex = i;
                            return temp_soft_poss[i];
                        }
                    }
                }
                else
                {
                    for (int i = 2; i >= 0; i--)
                    {
                        string key = "left-heavy-" + i;
                        if (temp_canuse_pos[key])
                        {
                            temp_canuse_pos[key] = false;
                            chook.mPosInPulleyIndex = i;
                            return temp_heavy_poss[i];
                        }
                    }
                }
                break;


            case "right":
                if (chook.mType == 2)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        string key = "right-soft-" + i;
                        if (temp_canuse_pos[key])
                        {
                            temp_canuse_pos[key] = false;
                            chook.mPosInPulleyIndex = i;
                            return temp_soft_poss[i];
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        string key = "right-heavy-" + i;
                        if (temp_canuse_pos[key])
                        {
                            temp_canuse_pos[key] = false;
                            chook.mPosInPulleyIndex = i;
                            return temp_heavy_poss[i];
                        }
                    }
                }
                break;
        }
        return Vector2.zero;

    }
    public int GetPosIndex(string flag, int chook_type)
    {
        int index = 0;
        switch (flag)
        {
            case "left":
                if (chook_type == 2)
                {
                    for (int i = 0; i < mLeftChooks.Count; i++)
                    {
                        if (mLeftChooks[i].mType == 2)
                            index++;
                    }
                }
                else
                {
                    for (int i = 0; i < mLeftChooks.Count; i++)
                    {
                        if (mLeftChooks[i].mType != 2)
                            index++;
                    }
                }
                break;
            case "right":
                if (chook_type == 2)
                {
                    for (int i = 0; i < mRightChooks.Count; i++)
                    {
                        if (mRightChooks[i].mType == 2)
                            index++;
                    }
                }
                else
                {
                    for (int i = 0; i < mRightChooks.Count; i++)
                    {
                        if (mRightChooks[i].mType != 2)
                            index++;
                    }
                }
                break;
        }
        return index;
    }
    public void PlayHualun(Vector3 angle)
    {
        mHualunLeft.transform.localEulerAngles += angle;
        mHualunRight.transform.localEulerAngles += angle;
    }
    public void PlayOKBtn(Vector3 angle)
    {
        mOK1.transform.localEulerAngles += angle;
    }
    public void PlayOKBtnAnimation()
    {
        Global.instance.PlayBtnClickAnimation(mOK1.transform);
        Global.instance.PlayBtnClickAnimation(mOK2.transform);
        if(null != mHand)
        {
            mHand.gameObject.SetActive(false);
        }
    }
    public void PlayMatchEffect()
    {
        if(null == mMathcEffect_L)
        {
            mMathcEffect_L = ResManager.GetPrefab("chookpk_prefab", "jitui_star").GetComponent<ParticleSystem>();
            UguiMaker.InitGameObj(mMathcEffect_L.gameObject, transform, "match_effect", Vector3.zero, Vector3.one);

            mMathcEffect_R = ResManager.GetPrefab("chookpk_prefab", "jitui_star").GetComponent<ParticleSystem>();
            UguiMaker.InitGameObj(mMathcEffect_R.gameObject, transform, "match_effect", Vector3.zero, Vector3.one);
        }
        mMathcEffect_L.transform.position = mLanziLeft.transform.position;
        mMathcEffect_L.Emit(20);
        mMathcEffect_R.transform.position = mLanziRight.transform.position;
        mMathcEffect_R.Emit(20);

        ChookPkCtl.instance.mSound.PlayShort("chookpk_sound", "09-爆2");

    }
    public int GetWeight_L()
    {
        int result = 0;
        for(int i = 0; i < mLeftChooks.Count; i++)
        {
            result += mLeftChooks[i].mWeight;
        }
        return result;
    }
    public int GetWeight_R()
    {
        int result = 0;
        for (int i = 0; i < mRightChooks.Count; i++)
        {
            result += mRightChooks[i].mWeight;
        }
        return result;
    }


    public void DengOpen()
    {
        Debug.LogError("DengOpen()");
        ChookPkCtl.instance.mSound.PlayShort("chookpk_sound", "08-开关灯");
        mDengLight.gameObject.SetActive(true);
        mDeng.sprite = ResManager.GetSprite("chookpk_sprite", "deng_open");
    }
    public void DengClose()
    {
        Debug.LogError("DengClose");
        ChookPkCtl.instance.mSound.PlayShort("chookpk_sound", "08-开关灯");
        mDengLight.gameObject.SetActive(false);
        mDeng.sprite = ResManager.GetSprite("chookpk_sprite", "deng_close");
    }

    public void SetOKBtnType(bool show_correct)
    {
        if(show_correct)
        {
            mOK2.sprite = ResManager.GetSprite("chookpk_sprite", "btn_ok2");
            //mOK2.color = new Color32(195, 73, 73, 255);
            //if(null == mOKEffect)
            //{
            //    mOKEffect = ResManager.GetPrefab("chookpk_prefab", "btneffect");
            //    UguiMaker.InitGameObj(mOKEffect.gameObject, mOK0.transform, "mOKEffect", Vector3.zero, Vector3.one);
            //}
            //mOKEffect.gameObject.SetActive(true);
        }
        else
        {
            mOK2.sprite = ResManager.GetSprite("chookpk_sprite", "btn_ok2-0");
            //mOK2.color = new Color32(237, 121, 72, 255);
            //if(null != mOKEffect)
            //{
            //    mOKEffect.gameObject.SetActive(false);
            //}
        }
        mOK2.SetNativeSize();

    }
    public void SetAllChookFree()
    {
        for(int i = 0; i < mLeftChooks.Count; i++)
        {
            mLeftChooks[i].transform.SetParent(ChookPkCtl.instance.transform);
            mLeftChooks[i].gameObject.SetActive(false);
            mLeftChooks[i].SetBusy(false);
        }
        for (int i = 0; i < mRightChooks.Count; i++)
        {
            mRightChooks[i].transform.SetParent(ChookPkCtl.instance.transform);
            mRightChooks[i].gameObject.SetActive(false);
            mRightChooks[i].SetBusy(false);
        }
        CleanChook_L();
        CleanChook_R();

    }
    public void CleanChook_L()
    {
        mLeftChooks.Clear();
        temp_canuse_pos["left-soft-0"] = true;
        temp_canuse_pos["left-soft-1"] = true;
        temp_canuse_pos["left-soft-2"] = true;
        temp_canuse_pos["left-soft-3"] = true;
        temp_canuse_pos["left-soft-4"] = true;

        temp_canuse_pos["left-heavy-0"] = true;
        temp_canuse_pos["left-heavy-1"] = true;
        temp_canuse_pos["left-heavy-2"] = true;

    }
    public void CleanChook_R()
    {
        mRightChooks.Clear();
        temp_canuse_pos["right-soft-0"] = true;
        temp_canuse_pos["right-soft-1"] = true;
        temp_canuse_pos["right-soft-2"] = true;
        temp_canuse_pos["right-soft-3"] = true;
        temp_canuse_pos["right-soft-4"] = true;

        temp_canuse_pos["right-heavy-0"] = true;
        temp_canuse_pos["right-heavy-1"] = true;
        temp_canuse_pos["right-heavy-2"] = true;

    }
    public void PushIn_L(ChookPkChook chook)
    {
        //Debug.LogError("push in L");
        mLeftChooks.Add(chook);
        //让木板转
        //if (Mathf.Abs(temp_angle_l) > 0.1f)
        //{
        //    temp_angle_l *= 1.2f;
        //}
        //else
        //{
        //    temp_angle_l = 10;
        //}       
    }
    public void PushIn_R(ChookPkChook chook)
    {
        //Debug.LogError("push in R");
        mRightChooks.Add(chook);
        //让木板转
        //if (Mathf.Abs(temp_angle_r) > 0.1f)
        //{
        //    temp_angle_r *= 1.2f;
        //}
        //else
        //{
        //    temp_angle_r = 10;
        //}
    }
    public void PushOut(ChookPkChook chook)
    {
        string key = "";
        if (mLeftChooks.Contains(chook))
        {
            if (2 == chook.mType)
                key = "left-soft-" + chook.mPosInPulleyIndex;
            else
                key = "left-heavy-" + chook.mPosInPulleyIndex;
            temp_canuse_pos[key] = true;
            mLeftChooks.Remove(chook);
        }
        if(mRightChooks.Contains(chook))
        {
            if (2 == chook.mType)
                key = "right-soft-" + chook.mPosInPulleyIndex;
            else
                key = "right-heavy-" + chook.mPosInPulleyIndex;
            temp_canuse_pos[key] = true;
            mRightChooks.Remove(chook);
        }
    }
    public void FlushOKSprite()
    {
        Debug.Log("FlushOKSprite() guanka=" + ChookPkCtl.instance.mGuanka.guanka);
        if(null != mOK2)
        {
            if(1 == ChookPkCtl.instance.mGuanka.guanka)
            {
                mOK2.sprite = ResManager.GetSprite("chookpk_sprite", "btn_ok2-0");
                if (null != mOKEffect)
                {
                    mOKEffect.gameObject.SetActive(false);
                }
            }
            else
            {
                mOK2.sprite = ResManager.GetSprite("chookpk_sprite", "btn_ok2-1");
                if (null == mOKEffect)
                {
                    mOKEffect = ResManager.GetPrefab("chookpk_prefab", "btneffect");
                    UguiMaker.InitGameObj(mOKEffect.gameObject, mOK0.transform, "mOKEffect", Vector3.zero, Vector3.one);
                }
                mOKEffect.gameObject.SetActive(true);

                if (null == mHand)
                {
                    mHand = GuideHandCtl.Create(mOK2.transform);
                    mHand.GuideTipClick(0.8f, 0.7f, true, true, "hand1");
                    mHand.transform.localEulerAngles = new Vector3(0, 0, 45);
                    mHand.transform.localPosition = new Vector3(51, -40, 0);
                }

            }
            mOK2.SetNativeSize();
        }
    }

    public void Shake_L()
    {
        StopCoroutine("TShake_L");
        StartCoroutine("TShake_L");
    }
    public void Shake_R()
    {
        StopCoroutine("TShake_R");
        StartCoroutine("TShake_R");
    }
    IEnumerator TShake_L()
    {
        float b = Random.Range(2, 5);
        float t = 0;
        while(true)
        {
            mShenziLeft.transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(t) * b);
            t += 0.1f;
            b -= 0.02f;
            if (b < 0)
                break;
            yield return new WaitForSeconds(0.01f);

        }
        mShenziLeft.transform.localEulerAngles = Vector3.zero;
    }
    IEnumerator TShake_R()
    {
        float b = Random.Range(2, 5);
        float t = 0;
        while (true)
        {
            mShenziRight.transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(t) * b);
            t += 0.1f;
            b -= 0.02f;
            if (b < 0)
                break;
            yield return new WaitForSeconds(0.01f);

        }
        mShenziRight.transform.localEulerAngles = Vector3.zero;
    }


    System.Action<string> func_callback = null;
    public void PlayAnimation_L( System.Action<string> func)
    {
        func_callback = func;
        StartCoroutine(TPlayAnimation("left"));
    }
    public void PlayAnimation_R(System.Action<string> func)
    {
        func_callback = func;
        StartCoroutine(TPlayAnimation("right"));
    }
    IEnumerator TPlayAnimation(string side)
    {
        DengOpen();
        RectTransform down = null;
        RectTransform up = null;
        Vector3 angle = Vector3.zero;
        if(side.Equals("left"))
        {
            down = mShenziLeft.rectTransform;
            up = mShenziRight.rectTransform;
            angle = new Vector3(0, 0, 5);
        }
        else
        {
            down = mShenziRight.rectTransform;
            up = mShenziLeft.rectTransform;
            angle = new Vector3(0, 0, -5);
        }

        //落地
        float shenzi_speed = 3;
        ChookPkCtl.instance.mSound.PlayBg("chookpk_sound", "06-滑轮");
        //ChookPkCtl.instance.mSound.PlayShort("chookpk_sound", "game-tips7-1-鸡落地" + (Random.Range(0, 1000) % 2).ToString());
        while (down.sizeDelta.y < shengzi_down)
        {
            down.sizeDelta += new Vector2(0, shenzi_speed);
            up.sizeDelta -= new Vector2(0, shenzi_speed);
            PlayHualun(angle);
            yield return new WaitForSeconds(0.01f);
        }
        ChookPkCtl.instance.mSound.StopBg();
        down.sizeDelta = new Vector2(6, shengzi_down);
        up.sizeDelta = new Vector2(6, shengzi_up);
        yield return new WaitForSeconds(0.5f);

        ChookPkCtl.instance.mSound.PlayBg2("chookpk_sound", "05-鸡走");
        if (side.Equals("left"))
        {
            mShenziLeft.transform.localEulerAngles = Vector3.zero;
            for(int i = 0; i < mLeftChooks.Count; i++)
            {
                mLeftChooks[i].RunOut(800);
            }
            for(int i = 0; i < mRightChooks.Count; i++)
            {
                mRightChooks[i].Fly();
            }
        }
        else
        {
            mShenziRight.transform.localEulerAngles = Vector3.zero;
            for (int i = 0; i < mRightChooks.Count; i++)
            {
                mRightChooks[i].RunOut(-800);
            }
            for (int i = 0; i < mLeftChooks.Count; i++)
            {
                mLeftChooks[i].Fly();
            }
        }

        CleanChook_L();
        CleanChook_R();
        
        //变回平衡   
        yield return new WaitForSeconds(0.5f);
        ChookPkCtl.instance.mSound.PlayBg("chookpk_sound", "01-齿轮转动");
        while (down.sizeDelta.y > shengzi_mid)
        {
            down.sizeDelta -= new Vector2(0, shenzi_speed);
            up.sizeDelta += new Vector2(0, shenzi_speed);
            PlayHualun(-angle);
            PlayOKBtn(-angle * 0.5f);
            yield return new WaitForSeconds(0.01f);
        }
        ChookPkCtl.instance.mSound.StopBg();
        down.sizeDelta = new Vector2(6, shengzi_mid);
        up.sizeDelta = new Vector2(6, shengzi_mid);

        Shake_L();
        Shake_R();
        

        if (null != func_callback)
        {
            func_callback(side);
            func_callback = null;
        }
        DengClose();
        ChookPkCtl.instance.mSound.StopBg2();
    }


    System.Action func_callback_null = null;
    public void PlayAnimationCenter(System.Action func)
    {
        func_callback_null = func;
        StartCoroutine(TPlayAnimationCenter());
    }
    public void PlayAnimationCenter(string side, System.Action func)
    {
        func_callback_null = func;
        StartCoroutine(TPlayAnimationCenter(side));
    }
    IEnumerator TPlayAnimationCenter()
    {
        DengOpen();
        bool can_break = true;
        float shenzi_speed = 3f;
        ChookPkCtl.instance.mSound.PlayBg("chookpk_sound", "01-齿轮转动");
        while (true)
        {
            can_break = true;
            if (mShenziLeft.rectTransform.sizeDelta.y < shengzi_mid - shenzi_speed)
            {
                mShenziLeft.rectTransform.sizeDelta += new Vector2(0, shenzi_speed);
                mOK1.rectTransform.localEulerAngles -= new Vector3(0, 0, 5);
                can_break = false;
                PlayHualun(new Vector3(0, 0, 5));
            }
            else if (mShenziLeft.rectTransform.sizeDelta.y > shengzi_mid + shenzi_speed)
            {
                mShenziLeft.rectTransform.sizeDelta -= new Vector2(0, shenzi_speed);
                mOK1.rectTransform.localEulerAngles += new Vector3(0, 0, 5);
                can_break = false;
                PlayHualun(new Vector3(0, 0, -5));
            }

            if (mShenziRight.rectTransform.sizeDelta.y < shengzi_mid - shenzi_speed)
            {
                mShenziRight.rectTransform.sizeDelta += new Vector2(0, shenzi_speed);
                can_break = false;
            }
            else if (mShenziRight.rectTransform.sizeDelta.y > shengzi_mid + shenzi_speed)
            {
                mShenziRight.rectTransform.sizeDelta -= new Vector2(0, shenzi_speed);
                can_break = false;
            }

            yield return new WaitForSeconds(0.01f);

            if (can_break)
                break;
        }
        ChookPkCtl.instance.mSound.StopBg();

        mShenziLeft.rectTransform.sizeDelta = new Vector2(6, shengzi_mid);
        mShenziRight.rectTransform.sizeDelta = new Vector2(6, shengzi_mid);


        if (null != func_callback_null)
        {
            func_callback_null();
            func_callback_null = null;
        }
        DengClose();
    }
    IEnumerator TPlayAnimationCenter(string side)
    {

        ChookPkCtl.instance.mSound.PlayShort("chookpk_sound", "game-tips7-1-这边更轻");
        if (0 == Random.Range(0, 1000) % 2)
        {
        }
        else
        {
            //ChookPkCtl.instance.mSound.PlayShort("chookpk_sound", "game-tips7-1-感觉这些鸡快要掉下来");
        }

        DengOpen();
        RectTransform down = null;
        RectTransform up = null;
        Vector3 angle = Vector3.zero;
        if (side.Equals("left"))
        {
            down = mShenziLeft.rectTransform;
            up = mShenziRight.rectTransform;
            angle = new Vector3(0, 0, 5);
        }
        else
        {
            down = mShenziRight.rectTransform;
            up = mShenziLeft.rectTransform;
            angle = new Vector3(0, 0, 5);
        }

        //落地
        float shenzi_speed = 3;
        ChookPkCtl.instance.mSound.PlayBg("chookpk_sound", "06-滑轮");
        while (down.sizeDelta.y < shengzi_down)
        {
            down.sizeDelta += new Vector2(0, shenzi_speed);
            up.sizeDelta -= new Vector2(0, shenzi_speed);
            PlayHualun(angle);
            yield return new WaitForSeconds(0.01f);
        }
        ChookPkCtl.instance.mSound.StopBg();
        down.sizeDelta = new Vector2(6, shengzi_down);
        up.sizeDelta = new Vector2(6, shengzi_up);

        if (side.Equals("left"))
        {
            //StopCoroutine("TShake_L");
            mShenziLeft.transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            //StopCoroutine("TShake_R");
            mShenziRight.transform.localEulerAngles = Vector3.zero;
        }

        if (null != func_callback_null)
        {
            func_callback_null();
            func_callback_null = null;
        }

        DengClose();
    }



    public void OnClkLeft()
    {
        if(ChookPkCtl.instance.mState == ChookPkCtl.State.select_loading)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100);
        bool click_canfly = false;
        foreach (RaycastHit hit in hits)
        {
            ChookPkChook c = hit.collider.gameObject.GetComponent<ChookPkChook>();
            if (null != c)
            {
                if (c.mCanFly)
                {
                    click_canfly = true;
                    break;
                }
            }
        }
        if (!click_canfly)
            Shake_L();

        if(ChookPkCtl.instance.mState == ChookPkCtl.State.select_weight)
        {
            if(GetWeight_L() > GetWeight_R())
            {
                ChookPkCtl.instance.Callback_Select(true, "left");
            }
            else
            {
                ChookPkCtl.instance.Callback_Select(false, string.Empty);
            }
        }
    }
    public void OnClkRight()
    {
        if (ChookPkCtl.instance.mState == ChookPkCtl.State.select_loading)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100);
        bool click_canfly = false;
        foreach (RaycastHit hit in hits)
        {
            ChookPkChook c = hit.collider.gameObject.transform.parent.GetComponent<ChookPkChook>();
            if(null != c)
            {
                if(c.mCanFly)
                {
                    click_canfly = true;
                    break;
                }
            }
        }
        if(!click_canfly)
            Shake_R();

        if (ChookPkCtl.instance.mState == ChookPkCtl.State.select_weight)
        {
            if (GetWeight_L() < GetWeight_R())
            {
                ChookPkCtl.instance.Callback_Select(true, "right");
            }
            else
            {
                ChookPkCtl.instance.Callback_Select(false, string.Empty);
            }
        }

    }

    

}
