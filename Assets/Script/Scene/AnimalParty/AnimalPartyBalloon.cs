using UnityEngine;
using System.Collections;
using Spine.Unity;

public class AnimalPartyBalloon : MonoBehaviour {
    public static AnimalPartyBalloon gSelect = null;

    public int mId { get; set; }
    public bool mTPlaySakeing
    {
        get; set;
    }
    public Vector3 mResetPos = Vector3.zero;
    public SkeletonGraphic mSpine { get; set; }
    public RectTransform mRtran { get; set; }
    public BoxCollider mBox { get; set; }
    public ParticleSystem mEffectLiHua { get; set; }

    public void Init(int id, Vector3 end_pos)
    {
        mId = id;
        if(null == mSpine)
        {
            transform.localScale = new Vector3(0.65f, 0.65f, 1);

            mRtran = gameObject.GetComponent<RectTransform>();
            mSpine = ResManager.GetPrefab("animalparty_prefab", "balloon").GetComponent<SkeletonGraphic>();
            UguiMaker.InitGameObj(mSpine.gameObject, transform, "balloon" + id, Vector3.zero, Vector3.one);

            mBox = gameObject.AddComponent<BoxCollider>();
            mBox.center = new Vector3(0, 235, 0);
            mBox.size = new Vector3(117, 459, 1);

            mEffectLiHua = UguiMaker.newParticle("effect_lihua", transform, new Vector3(0, 200, 0), Vector3.one, "effect_lihua", "effect_lihua0");

        }
        mSpine.gameObject.SetActive(true);
        mSpine.AnimationState.SetAnimation(1, "Balloon_" + id, true);
        mSpine.timeScale = Random.Range(0.8f, 1.2f);

        mResetPos = end_pos;

        //BoxEnable(true);

    }
    public void BoxEnable(bool _enable)
    {
        mBox.enabled = _enable;
    }
    public void Select()
    {
        StopAllCoroutines();
    }
    public void UnSelect()
    {
        StopAllCoroutines();
        StartCoroutine("TUnSelect");
    }
    public void SetMoveState()
    {
        BoxEnable(false);
        StopAllCoroutines();
        mSpine.gameObject.SetActive(true);
    }

    public void PlayLiHua()
    {
        BoxEnable(false);
        mSpine.gameObject.SetActive(false);
        mEffectLiHua.Play();
        AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "气球爆炸", 1);
    }
    public void PlayFlyOut()
    {
        StopAllCoroutines();
        StartCoroutine("TPlayFlyOut");
    }
    public void PlayFlyUp()
    {
        StartCoroutine("TPlayFlyUp");

    }
    public void PlaySake()
    {
        AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "点击气球");
        if(!mTPlaySakeing)
            StartCoroutine("TPlaySake");

    }
    public void StayInSky()
    {
        StopAllCoroutines();
        StartCoroutine("TStayInSky");
    }
    IEnumerator TPlayFlyOut()
    {
        //yield return new WaitForSeconds(0.5f);
        Vector3 pos_beg = mRtran.anchoredPosition3D;
        Vector3 pos_end = mRtran.anchoredPosition3D;
        pos_end.y = 565;
        float speed = Random.Range(0.01f, 0.01f);
        for (float i = 0; i < 1f; i += speed)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos_beg, pos_end, i);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition3D = pos_end;
        
    }
    IEnumerator TPlayFlyUp()
    {
        BoxEnable(false);
        Vector3 pos_beg = mResetPos + new Vector3(0, 800, 0);
        float speed = Random.Range(0.006f, 0.01f);
        for (float i = 0; i < 1f; i += speed)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos_beg, mResetPos, i);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition3D = mResetPos;

        BoxEnable(true);
        StartCoroutine("TStayInSky");
    }
    IEnumerator TUnSelect()
    {
        //Vector3 pos_beg = mRtran.anchoredPosition3D;
        Vector3 pos_end = mRtran.anchoredPosition3D + new Vector3(0, Random.Range(50, 100), 0);
        if(pos_end.y > -400)
        {
            pos_end.y = -400;
        }

        while(true)
        {
            if(mRtran.anchoredPosition3D.y < pos_end.y)
            {
                mRtran.anchoredPosition3D += new Vector3(0, 5, 0);
            }
            else if(mRtran.anchoredPosition3D.y > pos_end.y + 5)
            {
                mRtran.anchoredPosition3D += new Vector3(0, -5, 0);
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        
        mResetPos = mRtran.anchoredPosition3D;
        StartCoroutine("TStayInSky");


    }
    IEnumerator TStayInSky()
    {
        float p = 0;
        while(true)
        {
            mRtran.anchoredPosition = mResetPos + new Vector3(0, Mathf.Sin(p) * 20, 0);
            p += Random.Range(0, 0.15f);
            yield return new WaitForSeconds(0.005f);
        }
    }
    IEnumerator TPlaySake()
    {
        mTPlaySakeing = true;
        Vector3 reset = mResetPos;
        float a = 50;
        for (float i = 0; i < 1f; i += 0.02f)
        {
            mResetPos = reset + new Vector3(Mathf.Sin(Mathf.PI * 2 * i) * a, 0, 0);
            a -= 1;
            yield return new WaitForSeconds(0.005f);
        }
        mResetPos = reset;
        mTPlaySakeing = false;
    }


}
