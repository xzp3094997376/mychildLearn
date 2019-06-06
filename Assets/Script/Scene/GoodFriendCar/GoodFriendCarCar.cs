using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GoodFriendCarCar : MonoBehaviour {
    public RectTransform mRtran { get; set; }
    
    public int data_id { get; set; }
    
    Image mShadow { get; set; }
    Image mBg0 { get; set; }
    Image mBg1 { get; set; }
    Image mLunzi0 { get; set; }
    Image mLunzi1 { get; set; }
    RectTransform mSitRtran { get; set; }

    List<GoodFriendCarAnimal> mAnimals = new List<GoodFriendCarAnimal>();

    static AudioSource mComAudio { get; set; }

    void OnDestroy()
    {
        mComAudio = null;
    }

    public void SetData(int car_id)
    {
        data_id = car_id;
        
        if(null == mRtran)
        {
            mRtran = gameObject.GetComponent<RectTransform>();
            mShadow = UguiMaker.newImage("mShadow", transform, "goodfriendcar_sprite", "car_shadow", false);
            mBg0 = UguiMaker.newImage("mBg0", transform, "goodfriendcar_sprite", "car" + car_id + "_0", false);
            mSitRtran = UguiMaker.newGameObject("mSitRtran", transform).GetComponent<RectTransform>();
            mBg1 = UguiMaker.newImage("mBg1", transform, "goodfriendcar_sprite", "car" + car_id + "_1", false);
            mLunzi0 = UguiMaker.newImage("mLunzi0", transform, "goodfriendcar_sprite", "car" + car_id + "_2", false);
            mLunzi1 = UguiMaker.newImage("mLunzi1", transform, "goodfriendcar_sprite", "car" + car_id + "_2", false);

            mShadow.rectTransform.pivot = new Vector2(0.5f, 0);
            mBg0.rectTransform.pivot = new Vector2(0.5f, 0);
            mBg1.rectTransform.pivot = new Vector2(0.5f, 0);
            mBg0.rectTransform.anchoredPosition = new Vector2(0, 57);
            mBg1.rectTransform.anchoredPosition = Vector2.zero;
            mShadow.rectTransform.anchoredPosition = new Vector2(-21.9f, -23.47f);
            mShadow.color = new Color32(255, 255, 255, 132);
            mLunzi0.rectTransform.anchoredPosition = new Vector2(153, 0);
            mLunzi1.rectTransform.anchoredPosition = new Vector2(-153, 0);

        }



    }

    public void FlushLunzi()
    {
        mLunzi0.transform.localEulerAngles += new Vector3(0, 0, 10);
        mLunzi1.transform.localEulerAngles += new Vector3(0, 0, 10);
        if(null == mComAudio)
        {
            mComAudio = gameObject.AddComponent<AudioSource>();
            mComAudio.clip = ResManager.GetClip("goodfriendcar_sound", "car_run");
        }
        if(!mComAudio.isPlaying)
        {
            mComAudio.Play();
        }
    }
    public void SetAnimalBox(bool _enable)
    {
        for(int i = 0; i < mAnimals.Count; i++)
        {
            mAnimals[i].SetBoxEnable(_enable);
        }
    }
    public void SitOutCar(GoodFriendCarAnimal anim)
    {
        if(mAnimals.Contains(anim))
        {
            mAnimals.Remove(anim);
        }
    }
    public Vector2 SitInCar(GoodFriendCarAnimal anim)
    {
        anim.transform.SetParent(mSitRtran);
        mAnimals.Add(anim);
        switch (mSitRtran.childCount)
        {
            case 1:
                return new Vector2(-150, 46);
            case 2:
                return new Vector2(0, 46);
            case 3:
                return new Vector2(150, 46);
        }
        return Vector2.zero;
    }
    public bool Check()
    {
        if (mAnimals.Count != 3)
            return false;

        int num0 = -1;
        int num1 = -1;
        int num2 = -1;
        for(int i = 0; i < mAnimals.Count; i++)
        {
            
            if( -100 > mAnimals[i].mRtran.anchoredPosition.x )
            {
                num0 = mAnimals[i].data_number;
            }
            else if(100 < mAnimals[i].mRtran.anchoredPosition.x)
            {
                num2 = mAnimals[i].data_number;
            }
            else
            {
                num1 = mAnimals[i].data_number;
            }
        }
        if(num0 == num1 - 1 && num2 == num1 + 1)
        {
            return true;
        }

        //string msg = "";
        //msg += mAnimals[0].data_number + " ";
        //msg += mAnimals[1].data_number + " ";
        //msg += mAnimals[2].data_number + " ";
        //Debug.Log(msg);

        return false;
    }
    public void CarGoOut()
    {

        StartCoroutine("TCarGoOut");

    }
         
    
    public GoodFriendCarAnimal PopAnimal()
    {
        if (0 == mAnimals.Count)
            return null;
        else
        {
            GoodFriendCarAnimal animal = mAnimals[0];
            mAnimals.Remove(animal);
            return animal;
        }
    }
    public void PopAllAnimal()
    {
        if (0 == mAnimals.Count)
            return;
        else
        {
            mAnimals.Clear();
        }

    }

    IEnumerator TCarGoOut()
    {
        for(int i = 0; i < mAnimals.Count; i++)
        {
            mAnimals[i].SetSpine(new List<string>() { "face_sayyes" });
        }
        Vector3 pos0 = mRtran.anchoredPosition3D;
        Vector3 pos1 = pos0 + new Vector3(1423, 0);
        for(float i = 0; i < 1f; i += 0.01f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * (i - 1)) + 1);
            FlushLunzi();
            yield return new WaitForSeconds(0.01f);
        }

        for(int i= 0; i < mAnimals.Count; i++)
        {
            GoodFriendCarCtl.instance.ResetAnimal(mAnimals[i]);
        }
        PopAllAnimal();

    }


}
