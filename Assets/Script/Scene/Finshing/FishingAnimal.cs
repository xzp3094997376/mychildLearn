using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Spine.Unity;

public class FishingAnimal : MonoBehaviour
{
    public int mdata_index = -1;
    public int mdata_fishing_id = -1;
    public bool mdata_isover = false;
   

    public RectTransform mRtran { get; set; }
    public RectTransform mRtranFish { get; set; }
    public SkeletonGraphic mSpine { get; set; }
    public SkeletonGraphic mFishSpine { get; set; }
    public Image mFishImage { get; set; }
    public Image mTong { get; set; }
    public Image mYuGan { get; set; }
    public Button mButton { get; set; }
    public Button mButtonYugan { get; set; }


    public List<Vector3> mAnchors = null;
    List<Image> mLines = new List<Image>();
    List<Vector2> mLineSizes = new List<Vector2>();

    Vector3 mOffsetFish = Vector3.zero;


    int line_width = 4;

    public void Init (int index, int fishing_id, Vector3 animal_pos, Vector3 fish_pos, List<Vector3> line_anchor_pos)
    {
        mdata_index = index;
        mdata_fishing_id = fishing_id;
        mdata_isover = false;



        if(null == mRtran)
        {
            mRtran = gameObject.GetComponent<RectTransform>();
            switch (index)
            {
                case 0:
                    mTong = UguiMaker.newImage("tong", transform, "fishing_sprite", "tong0", false);
                    mSpine = ResManager.GetPrefab("fishing_prefab", "chook").GetComponent<SkeletonGraphic>();
                    mYuGan = UguiMaker.newImage("yugan", transform, "fishing_sprite", "yugan0", false);
                    break;
                case 1:
                    mTong = UguiMaker.newImage("tong", transform, "fishing_sprite", "tong1", false);
                    mSpine = ResManager.GetPrefab("fishing_prefab", "goose").GetComponent<SkeletonGraphic>();
                    mYuGan = UguiMaker.newImage("yugan", transform, "fishing_sprite", "yugan1", false);
                    break;
                case 2:
                    mTong = UguiMaker.newImage("tong", transform, "fishing_sprite", "tong2", false);
                    mSpine = ResManager.GetPrefab("fishing_prefab", "pig").GetComponent<SkeletonGraphic>();
                    mYuGan = UguiMaker.newImage("yugan", transform, "fishing_sprite", "yugan0", false);
                    break;
                default:
                    mTong = UguiMaker.newImage("tong", transform, "fishing_sprite", "tong3", false);
                    mSpine = ResManager.GetPrefab("fishing_prefab", "pigeon").GetComponent<SkeletonGraphic>();
                    mYuGan = UguiMaker.newImage("yugan", transform, "fishing_sprite", "yugan1", false);
                    break;

            }
        }

        if(null != mRtranFish)
        {
            Destroy(mRtranFish.gameObject);
            mRtranFish = null;
            mFishSpine = null;
        }
        

        UguiMaker.InitGameObj(mSpine.gameObject, transform, "spine", animal_pos, Vector3.one);
        mTong.rectTransform.anchoredPosition3D = mSpine.rectTransform.anchoredPosition3D + new Vector3(110, 70, 0);


        mAnchors = line_anchor_pos;
        for (int i = 0; i < mLines.Count; i++)
        {
            if (null != mLines[i])
            {
                Destroy(mLines[i].gameObject);
            }
        }
        mLines.Clear();
        mLineSizes.Clear();




        bool is_shuxian = true;
        for (int i = 1; i < mAnchors.Count; i++)
        {
            Image image = UguiMaker.newImage(i.ToString(), transform, "public", "white", false);
            mLines.Add(image);
            if (is_shuxian)
            {
                float sub = mAnchors[i].y - mAnchors[i - 1].y;
                if (sub > 0)
                {
                    image.rectTransform.pivot = new Vector2(0.5f, 0f);
                    image.rectTransform.sizeDelta = new Vector2(line_width, 0);
                    image.rectTransform.anchoredPosition3D = mAnchors[i - 1];
                    mLineSizes.Add(new Vector2(line_width, sub));
                }
                else
                {
                    image.rectTransform.pivot = new Vector2(0.5f, 1f);
                    image.rectTransform.sizeDelta = new Vector2(line_width, 0);
                    image.rectTransform.anchoredPosition3D = mAnchors[i - 1];
                    mLineSizes.Add(new Vector2(line_width, -sub));
                }
            }
            else
            {
                float sub = mAnchors[i].x - mAnchors[i - 1].x;
                if (sub > 0)
                {
                    image.rectTransform.pivot = new Vector2(0f, 0.5f);
                    image.rectTransform.sizeDelta = new Vector2(0, line_width);
                    image.rectTransform.anchoredPosition3D = mAnchors[i - 1];
                    mLineSizes.Add(new Vector2(sub, line_width));
                }
                else
                {
                    image.rectTransform.pivot = new Vector2(1f, 0.5f);
                    image.rectTransform.sizeDelta = new Vector2(0, line_width);
                    image.rectTransform.anchoredPosition3D = mAnchors[i - 1];
                    mLineSizes.Add(new Vector2(-sub, line_width));

                }
            }
            is_shuxian = !is_shuxian;

        }

        mYuGan.rectTransform.anchoredPosition3D = mAnchors[0];
        mYuGan.rectTransform.SetAsLastSibling();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayError()
    {
        Global.instance.PlayAngleAnimation(mRtranFish, Vector3.zero);
    }
    public void PlayYugan()
    {
        mYuGan.rectTransform.localEulerAngles += new Vector3(0, 0, -5);
    }

    public void Shoot()
    {
        StartCoroutine(TShoot());
    }
    IEnumerator TShoot()
    {

        FishingCtl.instance.mSound.PlayShortDefaultAb("收线", 0.2f);
        for (int i = 0; i < mLines.Count; i++)
        {
            if (0 == i % 2)
            {
                while (true)
                {
                    mLines[i].rectTransform.sizeDelta += new Vector2(0, 50);
                    if (mLines[i].rectTransform.sizeDelta.y > mLineSizes[i].y)
                    {
                        mLines[i].rectTransform.sizeDelta = mLineSizes[i];
                        break;
                    }
                    yield return new WaitForSeconds(0.01f);
                }
            }
            else
            {
                //横线
                while (true)
                {
                    mLines[i].rectTransform.sizeDelta += new Vector2(50, 0);
                    if (mLines[i].rectTransform.sizeDelta.x > mLineSizes[i].x)
                    {
                        mLines[i].rectTransform.sizeDelta = mLineSizes[i];
                        break;
                    }
                    yield return new WaitForSeconds(0.01f);
                }

            }
        }

        //出现鱼
        if (null != mFishSpine)
        {
            Destroy(mFishSpine.gameObject);
            mFishSpine = null;
        }
        switch (mdata_fishing_id)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                if (null == mFishImage)
                {
                    mFishImage = UguiMaker.newImage("fish" + mdata_fishing_id, transform, "fishing_sprite", "laji0", false);
                }
                mFishImage.sprite = ResManager.GetSprite("fishing_sprite", "laji" + mdata_fishing_id);
                mFishImage.SetNativeSize();
                mRtranFish = mFishImage.rectTransform;
                break;

            default:
                if (null != mFishImage)
                {
                    Destroy(mFishImage.gameObject);
                    mFishImage = null;
                }
                mRtranFish = ResManager.GetPrefab("aa_fish_prefab", "mouse_left" + (mdata_fishing_id - 4).ToString()).GetComponent<RectTransform>();
                mFishSpine = mRtranFish.Find("spine").GetComponent<SkeletonGraphic>();
                UguiMaker.InitGameObj(mRtranFish.gameObject, transform, "spine" + mdata_fishing_id, Vector3.zero, Vector3.one);


            break;
        }
        switch (mdata_fishing_id)
        {
            case 0:
                mOffsetFish = new Vector3(26.9f, 0, 0);
                break;
            case 1:
                mOffsetFish = new Vector3(5.3f, 0, 0);
                break;
            case 2:
                mOffsetFish = new Vector3(30.2f, 0, 0);
                break;
            case 3:
                mOffsetFish = new Vector3(23.7f, 0, 0);
                break;
            case 4:
                mOffsetFish = new Vector3(59.8f, 0, 0);
                break;
            case 5:
                mOffsetFish = new Vector3(73.8f, -68, 0);
                break;
            case 6:
                mOffsetFish = new Vector3(58, -61.5f, 0);
                break;
            case 7:
                mOffsetFish = new Vector3(73.8f, -61.81f, 0);
                break;
            case 8:
                mOffsetFish = new Vector3(60.6f, -70.7f, 0);
                break;
        }
        //Debug.Log(mdata_index + "  " + mAnchors[mAnchors.Count - 1]);

        if (null == mButton)
        {
            mButton = UguiMaker.newButton("mButton", transform, "public", "white");
            mButton.image.rectTransform.sizeDelta = new Vector2(120, 130);
            mButton.image.color = new Color(1, 1, 1, 0);
            mButton.onClick.AddListener(OnClkBtn);
            mButton.transition = Selectable.Transition.None;
        }
        mButton.gameObject.SetActive(true);

        FishingCtl.instance.mSound.PlayShort("素材出现通用音效");
        if (mdata_fishing_id <= 4)
        {
            mFishImage.rectTransform.anchoredPosition3D = mAnchors[mAnchors.Count - 1] + mOffsetFish;
            mFishImage.rectTransform.localEulerAngles = Vector3.zero;
            mButton.image.rectTransform.anchoredPosition3D = mFishImage.rectTransform.anchoredPosition3D;
            for (float i = 0; i < 1f; i += 0.04f)
            {
                float p = Mathf.Sin(Mathf.PI * i) * 0.5f;
                mFishImage.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i) + new Vector3(p, p, 0);
                yield return new WaitForSeconds(0.01f);
            }
            mFishImage.rectTransform.localScale = Vector3.one;

        }
        else
        {
            mRtranFish.anchoredPosition3D = mAnchors[mAnchors.Count - 1];// + mOffsetFish;
            mButton.image.rectTransform.anchoredPosition3D = mRtranFish.anchoredPosition3D + new Vector3(65, 0, 0);
            Vector3 scale = Vector3.one;
            for (float i = 0f; i < 1f; i += 0.04f)
            {
                float p = Mathf.Sin(Mathf.PI * i) * 0.5f;
                mRtranFish.localScale = Vector3.Lerp(Vector3.zero, scale, i) + new Vector3(p, p, 0);
                yield return new WaitForSeconds(0.01f);
            }
            mRtranFish.localScale = scale;

        }
        

    }

    public void GetFish()
    {
        mButton.gameObject.SetActive(false);
        StartCoroutine(TGetFish());
    }
    IEnumerator TGetFish()
    {
        for(int i = 0; i < mLines.Count; i++)
        {
            mLines[i].color = new Color32(0, 255, 0, 255);
        }

        int speed = 10;
        float speed_angle = 10;
        Vector3 scale_sub = Vector3.zero;// new Vector3(-0.02f, -0.02f, 0);
        if(null != mFishSpine)
        {
            mFishSpine.AnimationState.SetAnimation(0, "Struggle", true);
        }

        FishingCtl.instance.mSound.PlayShortDefaultAb("收线");
        while (mLines != null && mLines.Count > 0)
        {
            int index = mLines.Count - 1;
            Vector2 size = mLineSizes[index];
            Image image = mLines[index];
            int angle_stop_flag = 0;
            if(image.rectTransform.pivot.x > 0.7f)
            {
                while(image.rectTransform.sizeDelta.x > speed)
                {
                    image.rectTransform.sizeDelta -= new Vector2(speed, 0);
                    mRtranFish.anchoredPosition3D = image.rectTransform.anchoredPosition3D + new Vector3(-image.rectTransform.sizeDelta.x, 0, 0);// + mOffsetFish;
                    if(mRtranFish.localScale.x > 0.6f)
                    {
                        mRtranFish.localScale += scale_sub;
                    }

                    if(0 <= angle_stop_flag)
                    {
                        if (mRtranFish.localEulerAngles.z < 180)
                        {
                            mRtranFish.localEulerAngles += new Vector3(0, 0, speed_angle);
                            if(0 != angle_stop_flag && 1 != angle_stop_flag)
                            {
                                angle_stop_flag = -1;
                            }
                            else
                            {
                                angle_stop_flag = 1;
                            }
                        }
                        else
                        {
                            mRtranFish.localEulerAngles -= new Vector3(0, 0, speed_angle);
                            if (0 != angle_stop_flag && 2 != angle_stop_flag)
                            {
                                angle_stop_flag = -1;
                            }
                            else
                            {
                                angle_stop_flag = 2;
                            }
                        }

                    }

                    PlayYugan();
                    yield return new WaitForSeconds(0.01f);
                }
            }
            else if(image.rectTransform.pivot.x < 0.3f)
            {
                while (image.rectTransform.sizeDelta.x > speed)
                {
                    image.rectTransform.sizeDelta -= new Vector2(speed, 0);
                    mRtranFish.anchoredPosition3D = image.rectTransform.anchoredPosition3D + new Vector3(image.rectTransform.sizeDelta.x, 0, 0);// + mOffsetFish;
                    if (mRtranFish.localScale.x > 0.6f)
                    {
                        mRtranFish.localScale += scale_sub;
                    }


                    if (0 <= angle_stop_flag)
                    {
                        if (mRtranFish.localEulerAngles.z < 180)
                        {
                            mRtranFish.localEulerAngles -= new Vector3(0, 0, speed_angle);
                            if (0 != angle_stop_flag && 1 != angle_stop_flag)
                            {
                                angle_stop_flag = -1;
                            }
                            else
                            {
                                angle_stop_flag = 1;
                            }
                        }
                        else
                        {
                            mRtranFish.localEulerAngles += new Vector3(0, 0, speed_angle);
                            if (0 != angle_stop_flag && 2 != angle_stop_flag)
                            {
                                angle_stop_flag = -1;
                            }
                            else
                            {
                                angle_stop_flag = 2;
                            }
                        }

                    }
                    PlayYugan();
                    yield return new WaitForSeconds(0.01f);


                }


            }
            else if(image.rectTransform.pivot.y > 0.7f)
            {
                while (image.rectTransform.sizeDelta.y > speed)
                {
                    image.rectTransform.sizeDelta -= new Vector2(0, speed);
                    mRtranFish.anchoredPosition3D = image.rectTransform.anchoredPosition3D + new Vector3(0, -image.rectTransform.sizeDelta.y, 0);// + mOffsetFish;
                    if (mRtranFish.localScale.x > 0.6f)
                    {
                        mRtranFish.localScale += scale_sub;
                    }

                    if (0 <= angle_stop_flag)
                    {
                        if (mRtranFish.localEulerAngles.z < 270 && mRtranFish.localEulerAngles.z > 90)
                        {
                            mRtranFish.localEulerAngles += new Vector3(0, 0, speed_angle);
                            if (0 != angle_stop_flag && 1 != angle_stop_flag)
                            {
                                angle_stop_flag = -1;
                            }
                            else
                            {
                                angle_stop_flag = 1;
                            }
                        }
                        else
                        {
                            mRtranFish.localEulerAngles -= new Vector3(0, 0, speed_angle);
                            if (0 != angle_stop_flag && 2 != angle_stop_flag)
                            {
                                angle_stop_flag = -1;
                            }
                            else
                            {
                                angle_stop_flag = 2;
                            }

                        }

                    }

                    PlayYugan();
                    yield return new WaitForSeconds(0.01f);
                }
            }
            else if(image.rectTransform.pivot.y < 0.3f)
            {
                while (image.rectTransform.sizeDelta.y > speed)
                {
                    image.rectTransform.sizeDelta -= new Vector2(0, speed);
                    mRtranFish.anchoredPosition3D = image.rectTransform.anchoredPosition3D + new Vector3(0, image.rectTransform.sizeDelta.y, 0);// + mOffsetFish;
                    if (mRtranFish.localScale.x > 0.6f)
                    {
                        mRtranFish.localScale += scale_sub;
                    }

                    if (0 <= angle_stop_flag)
                    {
                        if (mRtranFish.localEulerAngles.z > 90 && mRtranFish.localEulerAngles.z < 270)
                        {
                            mRtranFish.localEulerAngles -= new Vector3(0, 0, speed_angle);
                            if (0 != angle_stop_flag && 1 != angle_stop_flag)
                            {
                                angle_stop_flag = -1;
                            }
                            else
                            {
                                angle_stop_flag = 1;
                            }
                        }
                        else
                        {
                            mRtranFish.localEulerAngles += new Vector3(0, 0, speed_angle);
                            if (0 != angle_stop_flag && 2 != angle_stop_flag)
                            {
                                angle_stop_flag = -1;
                            }
                            else
                            {
                                angle_stop_flag = 2;
                            }
                        }

                    }

                    PlayYugan();
                    yield return new WaitForSeconds(0.01f);
                }

            }
            else
            {
                Debug.LogError("错误数据 gamename=" + gameObject.name + " image.name=" + image.gameObject.name);
                yield break;
            }

            Destroy(image.gameObject);
            mLines.RemoveAt(index);
            mLineSizes.RemoveAt(index);
        }

        mLines.Clear();
        mLineSizes.Clear();
        mYuGan.rectTransform.localEulerAngles = Vector3.zero;

        if (null == mButtonYugan)
        {
            mButtonYugan = UguiMaker.newButton("mButtonYugan", mYuGan.transform, "public", "white");
            mButtonYugan.onClick.AddListener(OnClkPutFishInTong);
            mButtonYugan.image.rectTransform.anchoredPosition3D = new Vector3(-10, -64, 0);
            mButtonYugan.image.rectTransform.sizeDelta = new Vector2(109, 168);
            mButtonYugan.image.color = new Color(1, 1, 1, 0);
        }
        mButtonYugan.gameObject.SetActive(true);

    }

    IEnumerator TPutFishInTong()
    {
        switch (mdata_index)
        {
            case 0:
                FishingCtl.instance.mSound.PlayShort("aa_animal_effect_sound", "公鸡yes");
                break;
            case 1:
                FishingCtl.instance.mSound.PlayShort("aa_animal_effect_sound", "鹅yes");
                break;
            case 2:
                FishingCtl.instance.mSound.PlayShort("aa_animal_effect_sound", "猪yes");
                break;
            default:
                FishingCtl.instance.mSound.PlayShort("aa_animal_effect_sound", "鸽子yes");
                break;

        }

        FishingCtl.instance.mSound.PlayShortDefaultAb("鱼进桶");
        Vector3 pos0 = mRtranFish.anchoredPosition3D;
        Vector3 pos1 = Vector3.zero;
        if(null != mFishSpine)
        {
            pos1 = mTong.rectTransform.anchoredPosition3D + new Vector3(0, 50, 0);
        }
        else
        {
            pos1 = mTong.rectTransform.anchoredPosition3D + new Vector3(0, 30, 0);
        }

        Vector3 scale0 = mRtranFish.localScale;

        bool is_set_first = false;
        for(float i = 0; i < 1f; i += 0.03f)
        {
            mRtranFish.anchoredPosition3D = Vector3.Lerp(pos0, pos1, i) + new Vector3(0, Mathf.Sin(Mathf.PI * i) * 100);
            mRtranFish.localScale = Vector3.Lerp(scale0, new Vector3(0.5f,0.5f, 1), i);
            if (i > 0.5f && !is_set_first)
            {
                is_set_first = true;
                mRtranFish.SetAsFirstSibling();
            }
            yield return new WaitForSeconds(0.01f);
        }
        mdata_isover = true;
        FishingCtl.instance.callback_Finish_PutFish();

    }


    public void OnClkBtn()
    {
        //Debug.Log(gameObject.name);
        FishingCtl.instance.callbackOnClick(this);
        Global.instance.PlayBtnClickAnimation(mRtranFish);

    }
    public void OnClkPutFishInTong()
    {
        mButtonYugan.gameObject.SetActive(false);
        StartCoroutine(TPutFishInTong());

    }


}
