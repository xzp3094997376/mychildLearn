using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Spine.Unity;

public class AnimalCanFlyCalculate : MonoBehaviour {
    RectTransform mRtran { get; set; }
    Image mNum0, mHead0, mEqual, mNumBg, mNum1, mHead1, mOK;
    Image mUp, mDown;
    SkeletonGraphic mAnimalHead0, mAnimalHead1;
    AudioSource mAudio0, mAudio1;
    ParticleSystem mEffect { get; set; }
    
    bool temp_lock = false;
    int temp_onclick = 0;//0没，1上，2下, 3mOk
    int temp_count = 0;//记录时间

    Sprite[] mBtnUp;
    Sprite[] mBtnDown;

    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100))
            {
                if(hit.collider.gameObject.name.Equals("mOK"))
                {
                    temp_onclick = 3;
                    mOK.sprite = ResManager.GetSprite("animalcanfly_sprite", "btn_down");
                    AnimalCanFlyCtl.instance.mSound.PlayShort("button_down");
                    
                }
                else if (hit.collider.gameObject.name.Equals("mUp"))
                {
                    temp_onclick = 1;
                    mUp.transform.localScale = new Vector3(0.8f, 0.8f, 1);
                    mUp.sprite = mBtnUp[1];
                    OnClkBtnUp();
                }
                else if (hit.collider.gameObject.name.Equals("mDown"))
                {
                    temp_onclick = 2;
                    mDown.transform.localScale = new Vector3(0.8f, 0.8f, 1);
                    mDown.sprite = mBtnDown[1];
                    OnclkBtnDown();
                }
                temp_count = 0;
            }
        }

        if (0 != temp_onclick && Input.GetMouseButton(0))
        {
            temp_count++;
            if(20 < temp_count)
            {
                if(temp_count % 5 == 0)
                {
                    switch (temp_onclick)
                    {
                        case 0:
                            break;
                        case 1:
                            OnClkBtnUp();
                            break;
                        case 2:
                            OnclkBtnDown();
                            break;
                        case 3:
                            break;
                    }
                }
            }

        }

        if (0 != temp_onclick && Input.GetMouseButtonUp(0))
        {

            switch (temp_onclick)
            {
                case 0:
                    break;
                case 1:
                    mUp.transform.localScale = Vector3.one;
                    mUp.sprite = mBtnUp[0];
                    break;
                case 2:
                    mDown.transform.localScale = Vector3.one;
                    mDown.sprite = mBtnDown[0];
                    break;
                case 3:
                    mOK.sprite = ResManager.GetSprite("animalcanfly_sprite", "btn_up");
                    AnimalCanFlyCtl.instance.mSound.PlayShort("button_up");
                    break;
            }


            if (!temp_lock)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (temp_onclick == 3 && hit.collider.gameObject.name.Equals("mOK"))
                    {
                        temp_onclick = 3;
                        AnimalCanFlyCtl.instance.Callback_OK();
                    }
                    else if (temp_onclick == 1 && hit.collider.gameObject.name.Equals("mUp"))
                    {
                        temp_onclick = 1;
                    }
                    else if (temp_onclick == 2 && hit.collider.gameObject.name.Equals("mDown"))
                    {
                        temp_onclick = 2;
                    }
                }
            }

            temp_onclick = 0;

        }

    }

    public void Init()
    {
        mBtnUp = new Sprite[2];
        mBtnDown = new Sprite[2];
        mBtnUp[0] = ResManager.GetSprite("animalcanfly_sprite", "btn_up0");
        mBtnUp[1] = ResManager.GetSprite("animalcanfly_sprite", "btn_up1");
        mBtnDown[0] = ResManager.GetSprite("animalcanfly_sprite", "btn_down0");
        mBtnDown[1] = ResManager.GetSprite("animalcanfly_sprite", "btn_down1");


        mNum0 = UguiMaker.newImage("mNun0", transform, "number_slim", "0", false);
        mHead0 = UguiMaker.newImage("mHead0", transform, "aa_animal_head", "1", true);
        mEqual = UguiMaker.newImage("mEqual", transform, "number_slim", "=", false);
        mNumBg = UguiMaker.newImage("mNumBg", transform, "animalcanfly_sprite", "kuang", false);
        mNum1 = UguiMaker.newImage("mNum1", transform, "number_slim", "0", false);
        mHead1 = UguiMaker.newImage("mHead1", transform, "aa_animal_head", "1", true);
        mOK = UguiMaker.newImage("mOK", transform, "animalcanfly_sprite", "btn_up", false);
        mUp = UguiMaker.newImage("mUp", transform, "animalcanfly_sprite", "btn_up0", false);
        mDown = UguiMaker.newImage("mDown", transform, "animalcanfly_sprite", "btn_down0", false);

        mHead0.color = new Color(1, 1, 1, 0);
        mHead1.color = new Color(1, 1, 1, 0);

        mHead0.gameObject.AddComponent<Button>().onClick.AddListener(OnclkAnimal0);
        mHead1.gameObject.AddComponent<Button>().onClick.AddListener(OnclkAnimal1);

        //mUp = UguiMaker.newButton("mUp", transform, "animalcanfly_sprite", "up");
        //mDown = UguiMaker.newButton("mDown", transform, "animalcanfly_sprite", "down");
        //mUp.onClick.AddListener(OnClkBtnUp);
        //mDown.onClick.AddListener(OnclkBtnDown);
        
        mNum0.rectTransform.anchoredPosition = new Vector2(-230.72f, 0);
        mHead0.rectTransform.anchoredPosition = new Vector2(-143.5f, 0);
        mEqual.rectTransform.anchoredPosition = new Vector2(-45.7f, 0);
        mNumBg.rectTransform.anchoredPosition = new Vector2(37.7f, 0);
        mNum1.rectTransform.anchoredPosition = new Vector2(37.7f, 0);
        mHead1.rectTransform.anchoredPosition = new Vector2(132.8f, 0);
        mUp.rectTransform.anchoredPosition = new Vector2(229, 0);
        mDown.rectTransform.anchoredPosition = new Vector2(305, 0);
        mOK.rectTransform.anchoredPosition = new Vector2(418, 0);


        BoxCollider box = mOK.gameObject.AddComponent<BoxCollider>();
        box.center = new Vector3(0, 7.46f, 0);
        box.size = new Vector3(80, 114, 1);

        box = mUp.gameObject.AddComponent<BoxCollider>();
        box.size = new Vector3(mOK.rectTransform.sizeDelta.x, mOK.rectTransform.sizeDelta.y, 1);//new Vector3(46, 60, 1);

        box = mDown.gameObject.AddComponent<BoxCollider>();
        box.size = new Vector3(mDown.rectTransform.sizeDelta.x, mDown.rectTransform.sizeDelta.y, 1);//new Vector3(46, 60, 1);

        mDown.gameObject.SetActive(false);

        mRtran = gameObject.GetComponent<RectTransform>();
        mRtran.anchoredPosition = new Vector2(-98, -3000);

        mAudio0 = gameObject.AddComponent<AudioSource>();
        mAudio1 = gameObject.AddComponent<AudioSource>();

    }
    public void Flush(int num0, int animal_id0, int num1, int animal_id1)
    {
        mNum0.sprite = ResManager.GetSprite("number_slim", num0.ToString());
        mHead0.sprite = ResManager.GetSprite("aa_animal_head", animal_id0.ToString());
        mNum1.sprite = ResManager.GetSprite("number_slim", num1.ToString());
        mHead1.sprite = ResManager.GetSprite("aa_animal_head", animal_id1.ToString());

        if(null != mAnimalHead0)
        {
            Destroy(mAnimalHead0.gameObject);
        }
        if(null != mAnimalHead1)
        {
            Destroy(mAnimalHead1.gameObject);
        }

        mAnimalHead0 = ResManager.GetPrefab("animalhead_prefab", MDefine.GetAnimalHeadResNameByID(animal_id0)).GetComponent<SkeletonGraphic>();
        mAnimalHead1 = ResManager.GetPrefab("animalhead_prefab", MDefine.GetAnimalHeadResNameByID(animal_id1)).GetComponent<SkeletonGraphic>();
        UguiMaker.InitGameObj(mAnimalHead0.gameObject, mHead0.transform, "spine", new Vector3(0, -49, 0), new Vector3(0.3f, 0.3f, 1));
        UguiMaker.InitGameObj(mAnimalHead1.gameObject, mHead1.transform, "spine", new Vector3(0, -49, 0), new Vector3(0.3f, 0.3f, 1));
        mAnimalHead0.raycastTarget = false;
        mAnimalHead1.raycastTarget = false;
        mAudio0.clip = ResManager.GetClip("aa_animal_sound", MDefine.GetAnimalNameByID_CH(AnimalCanFlyCtl.instance.mdata_animal_id0) + "1");
        mAudio1.clip = ResManager.GetClip("aa_animal_sound", MDefine.GetAnimalNameByID_CH(AnimalCanFlyCtl.instance.mdata_animal_id2) + "1");

    }
    public void Flush(int num1)
    {
        mNum1.sprite = ResManager.GetSprite("number_slim", num1.ToString());
        Global.instance.PlayBtnClickAnimation(mNum1.transform);

    }
    public void FlushBtnUpDown()
    {
        if (19 == AnimalCanFlyCtl.instance.mdata_animal_num1)
        {
            mUp.gameObject.SetActive(false);
        }
        else
        {
            mUp.gameObject.SetActive(true);
        }

        if (0 == AnimalCanFlyCtl.instance.mdata_animal_num1)
        {
            mDown.gameObject.SetActive(false);
        }
        else
        {
            mDown.gameObject.SetActive(true);
        }

    }
    public void SetLock(bool islock)
    {
        temp_lock = islock;
    }

    public void OnClkBtnUp()
    {
        AnimalCanFlyCtl.instance.mSound.PlayShort("按钮-增加减少");
        Global.instance.PlayBtnClickAnimation(mUp.transform);
        if (temp_lock)
            return;

        if (18 == AnimalCanFlyCtl.instance.Callbalc_Up())
        {
            mUp.gameObject.SetActive(false);
        }
        else
        {
            mDown.gameObject.SetActive(true);
        }

    }
    public void OnclkBtnDown()
    {
        AnimalCanFlyCtl.instance.mSound.PlayShort("按钮-增加减少");
        Global.instance.PlayBtnClickAnimation(mDown.transform);
        if (temp_lock)
            return;

        if (0 == AnimalCanFlyCtl.instance.Callback_Down())
        {
            mDown.gameObject.SetActive(false);
        }
        else
        {
            mUp.gameObject.SetActive(true);
        }
    }
    public void OnclkAnimal0()
    {
        Global.instance.PlayBtnClickAnimation(mHead0.transform);
        mAnimalHead0.AnimationState.ClearTracks();
        mAnimalHead0.AnimationState.AddAnimation(1, "Click", false, 0);
        mAnimalHead0.AnimationState.AddAnimation(1, "Click", false, 0);
        mAnimalHead0.AnimationState.AddAnimation(1, "Click", false, 0);
        mAnimalHead0.AnimationState.AddAnimation(1, "Click", false, 0);
        mAnimalHead0.AnimationState.AddAnimation(1, "Idle", true, 0);
        if (!mAudio0.isPlaying)
            mAudio0.Play();

    }
    public void OnclkAnimal1()
    {
        Global.instance.PlayBtnClickAnimation(mHead1.transform);
        mAnimalHead1.AnimationState.ClearTracks();
        mAnimalHead1.AnimationState.AddAnimation(1, "Click", false, 0);
        mAnimalHead1.AnimationState.AddAnimation(1, "Click", false, 0);
        mAnimalHead1.AnimationState.AddAnimation(1, "Click", false, 0);
        mAnimalHead1.AnimationState.AddAnimation(1, "Click", false, 0);
        mAnimalHead1.AnimationState.AddAnimation(1, "Idle", true, 0);
        if (!mAudio1.isPlaying)
            mAudio1.Play();
    }


    public void Show()
    {
        mUp.gameObject.SetActive(true);
        mDown.gameObject.SetActive(false);
        StartCoroutine(TShow());
    }
    IEnumerator TShow()
    {
        Vector3 pos0 = new Vector3(-98, -171f);
        Vector3 pos1 = new Vector3(-98, -283f);


        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.anchoredPosition = Vector2.Lerp(pos1, pos0, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition = pos0;
        

    }

    public void Hide()
    {
        StartCoroutine(THide());
    }
    IEnumerator THide()
    {
        Vector3 pos0 = new Vector3(-98, -171f);
        Vector3 pos1 = new Vector3(-98, -283f);

        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.anchoredPosition = Vector2.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition = pos1;

    }

    public void PlayCorrect()
    {
        temp_lock = true;
        if (null == mEffect)
        {
            mEffect = ResManager.GetPrefab("effect_okbtn", "okbtn_effect").GetComponent<ParticleSystem>();
            UguiMaker.InitGameObj(mEffect.gameObject, mOK.transform, "mEffect", Vector3.zero, Vector3.one);

        }
        mEffect.Play();

    }
    public void PlayError()
    {
        StopCoroutine("TPlayError");
        StartCoroutine("TPlayError");
    }
    IEnumerator TPlayError()
    {
        Vector3 pos0 = new Vector3(-98, -171f);

        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.anchoredPosition3D = pos0 + new Vector3(Mathf.Sin(Mathf.PI * 2 * 2 * i) * 20, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition3D = pos0;

    }

}
