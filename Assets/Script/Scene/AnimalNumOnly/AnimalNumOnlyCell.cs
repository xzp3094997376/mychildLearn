using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Spine.Unity;

public class AnimalNumOnlyCell : MonoBehaviour
{
    public static AnimalNumOnlyCell gSelect { get; set; }

    public RectTransform mRtran { get; set; }
    Image mBg { get; set; }
    //Image mAnimal { get; set; }
    SkeletonGraphic mSpine { get; set; }
    Image mLock { get; set; }
    BoxCollider mBox { get; set; }

    public int data_correct_id = 0;
    public int data_id = 0;
    public int data_x = 0;
    public int data_y = 0;

    public void Init()
    {
        if(null == mRtran)
        {
            mRtran = gameObject.GetComponent<RectTransform>();
            mBg = UguiMaker.newImage("mBg", transform, "animalnumonly_sprite", "color0", false);
            mBg.type = Image.Type.Sliced;

            //mAnimal = UguiMaker.newImage("mAnimal", transform, "aa_animal_head", "1", false);
            //mAnimal.gameObject.SetActive(false);
            //mAnimal.rectTransform.sizeDelta = new Vector2(60, 60);

            mLock = UguiMaker.newImage("mLock", transform, "animalnumonly_sprite", "suo", false);
            mLock.rectTransform.anchoredPosition3D = new Vector3(-30, 30);

            mBox = gameObject.AddComponent<BoxCollider>();

        }

    }

    public void SetSize(int w, int h)
    {
        mBg.rectTransform.sizeDelta = new Vector2(w, h);
        mBox.size = new Vector3(w, h);
    }

    public void SetPos(Vector2 pos, int datax, int datay)
    {
        data_x = datax;
        data_y = datay;
        mRtran.anchoredPosition = pos;

    }

    public void SetAnimal(int animal_id)
    {
        //Debug.Log("animal_id=" + animal_id);
        data_id = animal_id;

        if (0 < animal_id)
        {
            if (null != mSpine)
            {
                Destroy(mSpine.gameObject);
            }
            mSpine = ResManager.GetPrefab("animalhead_prefab", MDefine.GetAnimalHeadResNameByID(animal_id)).GetComponent<SkeletonGraphic>();
            mSpine.raycastTarget = false;
            //mAnimal.gameObject.SetActive(true);
            //mAnimal.sprite = ResManager.GetSprite("aa_animal_head", animal_id.ToString());
            UguiMaker.InitGameObj(mSpine.gameObject, transform, MDefine.GetAnimalHeadResNameByID(animal_id), new Vector3(0, -31.92f), new Vector3(0.2f, 0.2f));

            mBg.sprite = ResManager.GetSprite("animalnumonly_sprite", "color" + AnimalNumOnlyCtl.instance.data_animal_color[animal_id]);

        }
        else
        {
            //mAnimal.gameObject.SetActive(false);
            if(null != mSpine)
                mSpine.gameObject.SetActive(false);
            mBg.sprite = ResManager.GetSprite("animalnumonly_sprite", "kong");

        }
        
    }

    public void SetCorrectId(int correct_id)
    {
        data_correct_id = correct_id;
    }
    
    public void SetBoxEnable(bool _enable)
    {
        mBox.enabled = _enable;
    }

    public void SetLock(int state)
    {
        switch (state)
        {
            case 0:
                mLock.gameObject.SetActive(false);
                break;
            case 1:
                mLock.gameObject.SetActive(true);
                mLock.sprite = ResManager.GetSprite("animalnumonly_sprite", "suo");
                mLock.SetNativeSize();
                break;
            case 2:
                mLock.gameObject.SetActive(false);
                mLock.sprite = ResManager.GetSprite("aa_animal_head", data_correct_id.ToString());
                mLock.rectTransform.sizeDelta = new Vector2(35, 35);
                //mLock.sprite = ResManager.GetSprite("animalnumonly_sprite", "tip");
                //mLock.SetNativeSize();
                break;
        }


    }

    public void PlaySpine()
    {
        if(null != mSpine)
        {
            mSpine.AnimationState.ClearTracks();
            mSpine.AnimationState.AddAnimation(1, "Click", false, 0);
            mSpine.AnimationState.AddAnimation(1, "Idle", true, 0);
        }
    }
    public void PlaySpineLoop()
    {
        if (null != mSpine)
        {
            mSpine.AnimationState.ClearTracks();
            mSpine.AnimationState.AddAnimation(1, "Click", true, 0);
        }
    }

    public void ShowError()
    {
        //Debug.Log("ShowError()");
        StartCoroutine("TShowError");
    }
    IEnumerator TShowError()
    {
        Vector3 scale0 = new Vector3(0.2f, 0.2f, 1);
        Vector3 scale1 = new Vector3(0.3f, 0.3f, 1);
        Vector3 pos0 = new Vector3(0, -31.92f, 0);
        Vector3 pos1 = new Vector3(0, -43.9f, 0);
        PlaySpine();

        for (float i = 0; i < 1f; i += 0.15f)
        {
            mSpine.transform.localScale = Vector3.Lerp(scale0, scale1, i);
            mSpine.rectTransform.anchoredPosition = Vector3.Lerp(pos0, pos1, i);
            yield return new WaitForSeconds(0.01f);
        }
        mSpine.transform.localScale = scale1;

        for(float i = 0; i < 1f; i += 0.05f)
        {
            mSpine.transform.localEulerAngles = new Vector3( 0, 0, 10 * Mathf.Sin(Mathf.PI * 4 * i));
            yield return new WaitForSeconds(0.01f);
        }
        mSpine.transform.localEulerAngles = Vector3.zero;

        for (float i = 0; i < 1f; i += 0.15f)
        {
            mSpine.transform.localScale = Vector3.Lerp( scale1, scale0, i);
            mSpine.rectTransform.anchoredPosition = Vector3.Lerp(pos1, pos0, i);
            yield return new WaitForSeconds(0.01f);
        }
        mSpine.transform.localScale = scale0;
        mSpine.rectTransform.anchoredPosition = pos0;

    }

}
