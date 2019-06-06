using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Spine.Unity;

public class AnimalPartyHead : MonoBehaviour
{
    public static AnimalPartyHead gSelect { get; set; }

    public RectTransform mRtran { get; set; }
    public Image mHeadbg { get; set; }
    public BoxCollider mBox { get; set; }
    public SkeletonGraphic mSpine { get; set; }
    public ParticleSystem mEffect0 { get; set; }
    public ParticleSystem mEffect1 { get; set; }
    public AudioSource mAudio { get; set; }

    public int mAnimalId { get; set; }
    public int mFruitId { get; set; }
    public Vector3 mResetPos = Vector3.zero;




    public void SetData(int animal_id, int fruit_id, Vector3 pos)
    {
        if (null == mRtran)
        {
            mRtran = gameObject.GetComponent<RectTransform>();
            mHeadbg = UguiMaker.newImage("bg", transform, "animalparty_sprite", "headbg", false);
            mBox = gameObject.AddComponent<BoxCollider>();
            mBox.size = new Vector3(179, 179, 5);
            mAudio = gameObject.AddComponent<AudioSource>();
            mEffect0 = UguiMaker.newParticle("effect_star0", transform, Vector3.zero, Vector3.one, "effect_star0", "effect_star0");
            mEffect1 = UguiMaker.newParticle("effect_star0", transform, Vector3.zero, Vector3.one, "effect_star0", "effect_star1");

        }

        if (null != mSpine)
        {
            Destroy(mSpine.gameObject);
            mSpine = null;
        }



        mAnimalId = animal_id;
        mFruitId = fruit_id;
        mResetPos = pos;

        mRtran.anchoredPosition3D = mResetPos;

        mSpine = ResManager.GetPrefab("animalhead_prefab", MDefine.GetAnimalHeadResNameByID(mAnimalId)).GetComponent<SkeletonGraphic>();
        mSpine.transform.SetParent(transform);
        mSpine.rectTransform.anchoredPosition3D = new Vector3(0, -73.13f, 0);
        mSpine.transform.localScale = new Vector3(0.4f, 0.4f, 1);
        mSpine.AnimationState.SetAnimation(1, "Idle", true);

        mAudio.clip = ResManager.GetClip("aa_animal_sound", MDefine.GetAnimalNameByID_CH(mAnimalId) + "0");
        mAudio.loop = false;
        mAudio.volume = 1f;

        SetBoxEnable(true);

    }
    public void SetBoxEnable(bool _enable)
    {
        mBox.enabled = _enable;
    }
    public void PlayClick()
    {
        if (null != mSpine)
        {
            mSpine.AnimationState.ClearTracks();
            mSpine.AnimationState.SetAnimation(2, "Click", true);
        }
    }
    public void PlayIdle()
    {
        if (null != mSpine)
        {
            mSpine.AnimationState.ClearTracks();
            mSpine.AnimationState.SetAnimation(1, "Idle", true);
        }
    }
    public void PlaySound()
    {
        mAudio.Play();
    }

    int temp_select_state = 0;//0缩小，1保持不变，2放大至圈圈大小，3复位, 4结束
    public void Select()
    {
        temp_select_state = 0;
        StopCoroutine("TSelect");
        StartCoroutine("TSelect");
    }
    public void Select2Place()
    {
        temp_select_state = 2;
    }
    public void Select2Reset()
    {
        temp_select_state = 3;
    }
    IEnumerator TSelect()
    {
        PlayClick();
        mEffect0.Play();
        PlaySound();

        while (0 == temp_select_state)
        {
            transform.localScale -= new Vector3(0.06f, 0.06f, 0);
            if (transform.localScale.x < 0.6f)
                temp_select_state = 1;
            yield return new WaitForSeconds(0.01f);
        }

        while(1 == temp_select_state)
        {
            yield return new WaitForSeconds(0.1f);
        }

        while(2 == temp_select_state)
        {
            transform.localScale += new Vector3(0.08f, 0.08f, 0);
            if (transform.localScale.x > 0.73f)
            {
                transform.localScale = new Vector3(0.73f, 0.73f, 1);
                temp_select_state = 4;

                //mAudio.clip = ResManager.GetClip("aa_animal_sound", MDefine.GetAnimalNameByID(mAnimalId) + "1");
                //mAudio.loop = true;
                //mAudio.volume = 0.5f;
                //mAudio.Play();

                mEffect1.Play();

            }
            yield return new WaitForSeconds(0.01f);
        }

        while(3 == temp_select_state)
        {
            mRtran.anchoredPosition3D += (mResetPos - mRtran.anchoredPosition3D).normalized * 40;
            if(transform.localScale.x < 1f)
            {
                transform.localScale += new Vector3(0.06f, 0.06f, 0);
            }

            if(Vector3.Distance(mRtran.anchoredPosition3D, mResetPos) < 45)
            {
                transform.localScale = Vector3.one;
                mRtran.anchoredPosition3D = mResetPos;
                temp_select_state = 4;
            }
            yield return new WaitForSeconds(0.01f);
        }

        PlayIdle();
        mEffect0.Stop();
    }

}
