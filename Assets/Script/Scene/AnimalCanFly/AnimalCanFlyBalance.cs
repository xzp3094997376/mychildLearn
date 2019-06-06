using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Spine.Unity;

public class AnimalCanFlyBalance : MonoBehaviour {
    public static AnimalCanFlyBalance gSelect = null;

    public Image mBan0, mBan1, mBan2;
    public RectTransform mRtran, mSitRight, mSitLeft;
    List<GameObject> mSpineLeft = new List<GameObject>();
    List<GameObject> mSpineRight = new List<GameObject>();
    BoxCollider mBox { get; set; }
    BoxCollider mBoxLeft { get; set; }
    BoxCollider mBoxRight { get; set; }
    ParticleSystem mEffect { get; set; }
    AudioSource mAudio { get; set; }

    public string mdata_side { get; set; }
    public float mdata_sub { get; set; }

    void Awake()
    {
        mBan0 = UguiMaker.newImage("mBan0", transform, "animalcanfly_sprite", "ban0", false);
        mBan1 = UguiMaker.newImage("mBan1", transform, "animalcanfly_sprite", "ban1", false);
        mBan2 = UguiMaker.newImage("mBan2", transform, "animalcanfly_sprite", "ban2", false);
        mSitRight = UguiMaker.newGameObject("mSitRight", mBan1.transform).GetComponent<RectTransform>();
        mSitLeft = UguiMaker.newGameObject("mSitLeft", mBan1.transform).GetComponent<RectTransform>();
        mRtran = gameObject.GetComponent<RectTransform>();

        mSitRight.anchoredPosition = new Vector2(110, 26);
        mSitLeft.anchoredPosition = new Vector2(-110, 26);

        mBox = gameObject.AddComponent<BoxCollider>();
        mBox.size = new Vector3(369.25f, 77, 1);
        mBox.center = new Vector3(-6, -10, 0);


        mBoxRight = mSitRight.gameObject.AddComponent<BoxCollider>();
        mBoxRight.size = new Vector3(147, 72, 1);
        mBoxRight.center = new Vector3(0, 36, 0);


        mBoxLeft = mSitLeft.gameObject.AddComponent<BoxCollider>();
        mBoxLeft.size = new Vector3(-147, 72, 1);
        mBoxLeft.center = new Vector3(0, 36, 0);

    }
	void Start () {
        switch(mdata_side)
        {
            case "left":
                mRtran.anchoredPosition = new Vector2(-387, 4.8f);
                break;
            case "right":
                mRtran.anchoredPosition = new Vector2(390, 4.8f);
                break;
        }
        mAudio = gameObject.AddComponent<AudioSource>();
        mAudio.clip = ResManager.GetClip("animalcanfly_sound", "跷跷板");
        mAudio.loop = true;


    }
	
    public void ResetShakeSub()
    {
        mdata_sub = 0.07f + 0.001f * (Random.Range(0, 1000) % 5);
        if (null != mEffect && mEffect.isPlaying)
        {
            mEffect.Stop();
            mAudio.Stop();
        }
    }
    public void PlayEffect()
    {
        if (null == mEffect)
        {
            mEffect = ResManager.GetPrefab("effect_quanquan", "effect_quanquan").GetComponent<ParticleSystem>();
            UguiMaker.InitGameObj(mEffect.gameObject, transform, "mEffect", Vector3.zero, Vector3.one);
        }
        if(!mEffect.isPlaying)
        {
            mEffect.Play();
            mAudio.Play();
        }
    }
    
    public void PlayAnimal(string left_right)
    {
        switch (left_right)
        {
            case "left":
                for (int i = 0; i < mSpineLeft.Count; i++)
                {
                    SkeletonGraphic spine = mSpineLeft[i].transform.Find("spine").GetComponent<SkeletonGraphic>();
                    spine.AnimationState.ClearTracks();
                    spine.AnimationState.AddAnimation(1, "Click", false, 0);
                    spine.AnimationState.AddAnimation(1, "Idle", true, 0);
                }
                break;
            case "right":
                for (int i = 0; i < mSpineRight.Count; i++)
                {
                    SkeletonGraphic spine = mSpineRight[i].transform.Find("spine").GetComponent<SkeletonGraphic>();
                    spine.AnimationState.ClearTracks();
                    spine.AnimationState.AddAnimation(1, "Click", false, 0);
                    spine.AnimationState.AddAnimation(1, "Idle", true, 0);
                }
                break;
        }
    }
    
    public void Show(int animal_left_id, int animal_right_id, int num_left, int num_right)
    {
        StopAllCoroutines();
        StartCoroutine(TShow(animal_left_id, animal_right_id, num_left, num_right));
        StartCoroutine("TShake");
        mBoxLeft.enabled = true;
        mBoxRight.enabled = true;
    }
    IEnumerator TShow(int animal_left_id, int animal_right_id, int num_left, int num_right)
    {
        if (0 < mSpineLeft.Count)
        {
            for (int i = 0; i < mSpineLeft.Count; i++)
            {
                Destroy(mSpineLeft[i].gameObject);
            }
            for (int i = 0; i < mSpineRight.Count; i++)
            {
                Destroy(mSpineRight[i].gameObject);
            }
            mSpineLeft.Clear();
            mSpineRight.Clear();
        }

        List<Vector3> poss0 = Common.PosSortByWidth(100, num_left, -12);
        List<Vector3> poss1 = Common.PosSortByWidth(100, num_right, -12);
        Vector3 scale0 = new Vector3(0.5f, 0.5f, 1);
        for (int i = 0; i < num_left; i++)
        {
            GameObject obj = ResManager.GetPrefab("aa_animal_env_prefab", MDefine.GetAnimalNameByID_EN(animal_left_id));
            obj.transform.Find("spine").GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(1, "Idle", true);
            obj.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);

            UguiMaker.InitGameObj(obj, mSitLeft, i.ToString(), poss0[i], Vector3.zero);
            obj.transform.SetAsFirstSibling();

            mSpineLeft.Add(obj);
        }
        for (int i = 0; i < num_right; i++)
        {
            GameObject obj = ResManager.GetPrefab("aa_animal_env_prefab", MDefine.GetAnimalNameByID_EN(animal_right_id));
            obj.transform.Find("spine").GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(1, "Idle", true);
            obj.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);

            UguiMaker.InitGameObj(obj, mSitRight, i.ToString(), poss1[i], Vector3.zero);
            mSpineRight.Add(obj);
        }

        AnimalPrefabData data_left = mSpineLeft[0].GetComponent<AnimalPrefabData>();
        AnimalPrefabData data_right = mSpineRight[0].GetComponent<AnimalPrefabData>();
        //Debug.LogError(data_left.m_body_side + "  " + data_right.m_body_side);
        for (float i = 0; i < 1; i += 0.06f)
        {
            Vector3 scale1 = Vector3.Lerp(Vector3.zero, scale0, Mathf.Sin(Mathf.PI * 0.5f * i));
            Vector3 scale2 = Vector3.Lerp(Vector3.zero, scale0, Mathf.Sin(Mathf.PI * 0.5f * i));
            scale1.x = scale1.x * data_left.m_face_side;// / data_left.m_body_side;
            scale2.x = scale2.x * data_right.m_face_side * -1;// / data_right.m_body_side;
            scale1.y = Mathf.Abs(scale1.x);
            scale2.y = Mathf.Abs(scale2.x);

            for (int j = 0; j < mSpineLeft.Count; j++)
            {
                mSpineLeft[j].transform.localScale = scale1;
            }
            for (int j = 0; j < mSpineRight.Count; j++)
            {
                mSpineRight[j].transform.localScale = scale2;
            }
            yield return new WaitForSeconds(0.01f);
        }


    }
    IEnumerator TShake()
    {

        float p = 0;
        ResetShakeSub();
        while(true)
        {
            mBan1.rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(p) * 5);
            p += mdata_sub;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Fly()
    {
        mBoxLeft.enabled = false;
        mBoxRight.enabled = false;
        StartCoroutine(TFly());

    }
    IEnumerator TFly()
    {
        List<RectTransform> san_animal = new List<RectTransform>();
        List<Image> san = new List<Image>();
        List<float> speed = new List<float>();
        for (int i = 0; i < mSpineLeft.Count; i++)
        {
            Image s = UguiMaker.newImage("san", mSpineLeft[i].transform, "animalcanfly_sprite", "san_one", false);
            s.transform.SetAsFirstSibling();
            s.rectTransform.pivot = new Vector2(0.5f, 0);
            s.rectTransform.anchoredPosition = new Vector2(0, -32);
            s.rectTransform.localScale = new Vector3(3, 3, 1);
            speed.Add(Random.Range(5f, 10f));
            san_animal.Add(s.transform.parent.GetComponent<RectTransform>());
            san.Add(s);
        }
        for (int i = 0; i < mSpineRight.Count; i++)
        {
            Image s = UguiMaker.newImage("san", mSpineRight[i].transform, "animalcanfly_sprite", "san_one", false);
            s.transform.SetAsFirstSibling();
            s.rectTransform.pivot = new Vector2(0.5f, 0);
            s.rectTransform.anchoredPosition = new Vector2(0, -32);
            s.rectTransform.localScale = Vector3.zero;
            speed.Add(Random.Range(5f, 8f));
            san_animal.Add(s.transform.parent.GetComponent<RectTransform>());
            san.Add(s);
        }
        AnimalCanFlyCtl.instance.mSound.PlayShortDefaultAb("降落伞");
        for (float i = 0; i < 1f; i += 0.06f)
        {
            Vector3 scale = Vector3.Lerp(Vector3.zero, new Vector3(3, 3, 1), i);
            for(int j = 0; j < san.Count; j++)
            {
                san[j].rectTransform.localScale = scale;
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int j = 0; j < san_animal.Count; j++)
        {
            san_animal[j].SetParent(transform);
        }
        int count = 0;
        while(true)
        {
            count = 0;
            for (int j = 0; j < san_animal.Count; j++)
            {
                san_animal[j].anchoredPosition += new Vector2(0, speed[j]);
                if(san_animal[j].anchoredPosition.y > 800)
                {
                    count++;
                }
            }
            if(count == san.Count)
            {
                break;
            }
            yield return new WaitForSeconds(0.01f);

        }


    }

    
    

}
