using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using Spine.Unity;

public class GoodFriendCarAnimal : MonoBehaviour
{
    public static GoodFriendCarAnimal gSelect { get; set; }

    public RectTransform mRtran { get; set; }
    SkeletonGraphic mSpine { get; set; }
    GameObject mSpineParent { get; set; }
    public Image mImageNumberBg { get; set; }
    Image mImageNumber { get; set; }
    Image mShadow { get; set; }
    Image mStar { get; set; }
    public BoxCollider mBox { get; set; }
    Vector2 mResetPos = Vector3.zero;
    //Image mTextBg { get; set; }
    //Text mText { get; set; }

    public int data_animal_id = 0;
    public int data_number = 0;
    
	public void SetDate(int mdefine_id)
    {
        string animal_name = MDefine.GetAnimalNameByID_EN(mdefine_id);// ((MAnimalType)Enum.Parse(typeof(MAnimalType), mdefine_id.ToString())).ToString();

        if(mdefine_id != data_animal_id)
        {
            if (null != mSpineParent)
                Destroy(mSpineParent);
            mSpineParent = ResManager.GetPrefab("aa_animal_person_prefab", animal_name);
            mSpine = mSpineParent.transform.Find("spine").GetComponent<SkeletonGraphic>();
            UguiMaker.InitGameObj(mSpineParent, transform, "mSpine", Vector3.zero, Vector3.one);
        }
        data_animal_id = mdefine_id;
        


        if (null == mRtran)
        {
            mRtran = gameObject.GetComponent<RectTransform>();

            mShadow = UguiMaker.newImage("mShadow", transform, "goodfriendcar_sprite", "shadow");
            mShadow.rectTransform.anchoredPosition = new Vector2(0, 12);
            mShadow.rectTransform.sizeDelta = new Vector2(100, 100);
            mShadow.color = new Color(0, 0, 0, 0.2f);

            //mImage = UguiMaker.newImage("mImage", transform, "goodfriendcar_sprite", animal_name);
            //mImage.rectTransform.pivot = new Vector2(0.5f, 0);
            //mImage.rectTransform.sizeDelta = new Vector2(150, 175);


            mImageNumberBg = UguiMaker.newImage("mImageNumberBg", transform, "goodfriendcar_sprite", "numbg");
            mImageNumber = UguiMaker.newImage("mImageNumber", transform, "goodfriendcar_sprite", "numbg");
            mImageNumber.color = new Color32(85, 34, 17, 255);
            mBox = gameObject.AddComponent<BoxCollider>();

            mStar = UguiMaker.newImage("mStar", transform, "goodfriendcar_sprite", "star");
            mStar.rectTransform.anchoredPosition3D = new Vector3(0, 100, 0);
            mStar.gameObject.SetActive(false);
            

        }

        mSpineParent.transform.SetSiblingIndex(mShadow.transform.GetSiblingIndex());
        SetSpine(new List<string>() { "face_idle" });

        //mImage.sprite = ResManager.GetSprite("goodfriendcar_sprite", animal_name);
        //mImage.rectTransform.anchoredPosition3D = Vector3.zero;



        gameObject.name = mdefine_id.ToString();


        switch (mdefine_id)
        {
            case 1:
                mImageNumberBg.rectTransform.anchoredPosition = new Vector2(0, 45);
                mImageNumberBg.transform.localScale = Vector3.one;
                break;
            case 2:
                mImageNumberBg.rectTransform.anchoredPosition = new Vector2(-8.11f, 52.53f);
                break;
            case 3:
                mImageNumberBg.rectTransform.anchoredPosition = new Vector2(1.9f, 61f);
                mImageNumberBg.transform.localScale = Vector3.one;
                break;
            case 4:
                mImageNumberBg.rectTransform.anchoredPosition = new Vector2(-6.9f, 50.1f);
                mImageNumberBg.transform.localScale = Vector3.one;
                break;
            case 5:
                mImageNumberBg.rectTransform.anchoredPosition = new Vector2(-10.1f, 63.3f);
                mImageNumberBg.transform.localScale = Vector3.one;
                break;
            case 6:
                mImageNumberBg.rectTransform.anchoredPosition = new Vector2(-0.4f, 53.2f);
                mImageNumberBg.transform.localScale = Vector3.one;
                break;
            case 7:
                mImageNumberBg.rectTransform.anchoredPosition = new Vector2(-0.4f, 72);
                mImageNumberBg.transform.localScale = Vector3.one;
                break;
            case 8:
                mImageNumberBg.rectTransform.anchoredPosition = new Vector2(-5, 36);
                mImageNumberBg.transform.localScale = Vector3.one;
                break;
            case 9:
                mImageNumberBg.rectTransform.anchoredPosition = new Vector2(-2.3f, 53);
                mImageNumberBg.transform.localScale = Vector3.one;
                break;
            case 10:
                mImageNumberBg.rectTransform.anchoredPosition = new Vector2(-4, 51);
                mImageNumberBg.transform.localScale = Vector3.one;
                break;
            case 11:
                mImageNumberBg.rectTransform.anchoredPosition = new Vector2(-2.4f, 30.5f);
                mImageNumberBg.transform.localScale = Vector3.one;
                //mImageNumberBg.transform.localScale = new Vector3(0.7f, 0.7f, 1);
                break;
            case 12:
                mImageNumberBg.rectTransform.anchoredPosition = new Vector2(1, 54);
                mImageNumberBg.transform.localScale = Vector3.one;
                //mImageNumberBg.transform.localScale = new Vector3(0.7f, 0.7f, 1);
                break;
            case 13:
                mImageNumberBg.rectTransform.anchoredPosition = new Vector2(-5.5f, 54f);
                mImageNumberBg.transform.localScale = Vector3.one;
                break;
            case 14:
                mImageNumberBg.rectTransform.anchoredPosition = new Vector2(0, 61.7f);
                mImageNumberBg.transform.localScale = Vector3.one;
                break;
        }
        mImageNumber.rectTransform.anchoredPosition = mImageNumberBg.rectTransform.anchoredPosition;
        mImageNumber.transform.localScale = mImageNumberBg.transform.localScale;

        float scale = mImageNumberBg.transform.localScale.x;
        mBox.size = new Vector3(150 * scale, 175 * scale, 1);
        mBox.center = new Vector3(0, mBox.size.y * 0.5f);
        

    }
    public void SetNumber(int number)
    {
        data_number = number;
        mImageNumber.sprite = ResManager.GetSprite("number_round", number.ToString());
    }
    public void SetResetPos(Vector2 reset_pos)
    {
        mResetPos = reset_pos;
    }
    public void SetBoxEnable(bool _enable)
    {
        mBox.enabled = _enable;
    }
    public void ShowShadow(bool is_show)
    {
        mShadow.gameObject.SetActive(is_show);
    }
    public void SetSpine(List<string> anims)
    {
        mSpine.AnimationState.ClearTracks();
        for (int i = 0; i < anims.Count - 1; i++)
        {
            mSpine.AnimationState.AddAnimation(0, anims[i], false, 0);
        }
        mSpine.AnimationState.AddAnimation(0, anims[anims.Count - 1], true, 0);
    }
    public void PlayVoice()
    {
        GoodFriendCarCtl.instance.mSound.PlayShort("aa_animal_sound", MDefine.GetAnimalNameByID_CH(data_animal_id) + "1");
    }
         

    /*
    public Vector3 GetNumberPos()
    {
        return mRtran.anchoredPosition3D + mImageNumberBg.rectTransform.anchoredPosition3D;
    }
    */

    public void Select()
    {
        transform.SetAsLastSibling();
        StopCoroutine("TSelect");
        StopCoroutine("TUnSelect");
        StartCoroutine("TSelect");
    }
    public void UnSelect()
    {
        mStar.gameObject.SetActive(false);
        StopCoroutine("TSelect");
        StopCoroutine("TUnSelect");
        StartCoroutine("TUnSelect");

    }
    IEnumerator TSelect()
    {
        //for(float i = 0; i < 1f; i += 0.05f)
        //{
        //    transform.localEulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, 45), i);
        //    yield return new WaitForSeconds(0.01f);
        //}
        mStar.gameObject.SetActive(true);
        float p = 0;
        float psin = 0;
        float pcos = 0;
        float pscale = 0;
        while(true)
        {
            psin = Mathf.Sin(p);
            pcos = Mathf.Cos(p);

            transform.localEulerAngles = new Vector3(0, 0, psin * 20);

            mStar.rectTransform.anchoredPosition = new Vector2(
             psin * 80,
             pcos * 10 + 160
             );
            pscale = -pcos * 0.1f + 0.6f;
            mStar.transform.localScale = new Vector3(pscale, pscale, 1);
            p += 0.3f;
            
            yield return new WaitForSeconds(0.01f);
        }

    }
    IEnumerator TUnSelect()
    {
        transform.localEulerAngles = Vector3.zero;
        
        while(true)
        {
            mRtran.anchoredPosition = (mResetPos - mRtran.anchoredPosition).normalized * 40 + mRtran.anchoredPosition;

            yield return new WaitForSeconds(0.01f);

            if(Vector2.Distance(mResetPos, mRtran.anchoredPosition) < 40)
            {
                break;
            }
        }
        mRtran.anchoredPosition = mResetPos;

    }

    public void ShowStar()
    {
        mStar.gameObject.SetActive(true);
        StartCoroutine("TShowStar");
    }
    public void HideStar()
    {
        mStar.gameObject.SetActive(false);
        StopCoroutine("TShowStar");
    }
    IEnumerator TShowStar()
    {
        float p = 0;
        float p1 = 0;
        while(true)
        {
            mStar.rectTransform.anchoredPosition = new Vector2(
                Mathf.Sin(p) * 50,
                Mathf.Cos(p) * 30 + 150
                );
            p1 = -Mathf.Cos(p) * 0.2f + 0.5f;
            mStar.transform.localScale = new Vector3(p1, p1, 1);
            p += 0.25f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    


}
