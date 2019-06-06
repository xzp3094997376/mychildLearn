using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class KnowCircularShip : MonoBehaviour
{

    private Image ship_zhijia;
    KCJiaZiLight line0;
    KCJiaZiLight line1;
    KCJiaZiLight line2;

    private Transform ship_bodyTrans;
    private Image ship_body;
    private Image[] imgPoints = new Image[12];
    public SkeletonGraphic spine_kbaby;
    private BoxCollider2D mbox2D;

    private Image ship_flg;

    public List<KCirObjLv1> mInDropObjList = new List<KCirObjLv1>();
    private List<Vector3> mPoss = new List<Vector3>();
    private GameObject mGOIn;

    private Tween mShakeTween;

    AudioSource mAudioSource;

    ParticleSystem msys;

    Canvas mCanvas;

    public void InitAwake()
    {
        //gameObject.layer = 5;
        //mCanvas = gameObject.AddComponent<Canvas>();
        //mCanvas.overrideSorting = true;
        //mCanvas.sortingOrder = 2;

        ship_zhijia = transform.Find("ship_zhijia").GetComponent<Image>();
        ship_zhijia.sprite = ResManager.GetSprite("knowcircular_sprite", "ship_zhijia");
        KCJiaZiLight line0 = ship_zhijia.transform.Find("line0").gameObject.AddComponent<KCJiaZiLight>();
        KCJiaZiLight line1 = ship_zhijia.transform.Find("line1").gameObject.AddComponent<KCJiaZiLight>();
        KCJiaZiLight line2 = ship_zhijia.transform.Find("line2").gameObject.AddComponent<KCJiaZiLight>();
        line0.InitAwake(8);
        line1.InitAwake(8);
        line2.InitAwake(7);

        ship_bodyTrans = transform.Find("ship_bodyTrans");
        ship_body = ship_bodyTrans.transform.Find("ship_body").GetComponent<Image>();
        ship_body.sprite = ResManager.GetSprite("knowcircular_sprite", "ship_body");

        List<Vector2> vpointList = new List<Vector2>();
        vpointList.Add(new Vector2(-2.7f, -65.6f));
        vpointList.Add(new Vector2(36.1f, -76.6f));
        vpointList.Add(new Vector2(63.7f, -104f));
        vpointList.Add(new Vector2(74, -143));
        vpointList.Add(new Vector2(63.5f, -180f));
        vpointList.Add(new Vector2(35.4f, -208.7f));
        vpointList.Add(new Vector2(-3f, -219.7f));
        vpointList.Add(new Vector2(-40.8f, -209.1f));
        vpointList.Add(new Vector2(-69.5f, -181.4f));
        vpointList.Add(new Vector2(-79.4f, -142.4f));
        vpointList.Add(new Vector2(-70.2f, -103.1f));
        vpointList.Add(new Vector2(-41.5f, -75.8f));
        for (int i = 0; i < imgPoints.Length; i++)
        {
            imgPoints[i] = UguiMaker.newImage("imgpoint" + i, ship_body.transform, "knowcircular_sprite", "ship_cle0", false);
            imgPoints[i].transform.localPosition = vpointList[i];
            imgPoints[i].transform.localScale = Vector3.one * 0.52f;
        }

        spine_kbaby = ship_bodyTrans.transform.Find("spine_kbaby").GetComponent<SkeletonGraphic>();
        spine_kbaby.transform.localPosition = new Vector3(5f, -315f, 0f);
        spine_kbaby.transform.localScale = Vector3.one * 0.45f;
        mbox2D = ship_body.GetComponent<BoxCollider2D>();
        mbox2D.offset = new Vector2(0f, -140f);
        mbox2D.size = Vector2.one * 180f;
        spine_kbaby.transform.SetSiblingIndex(0);

        ship_flg = transform.Find("ship_flg").GetComponent<Image>();
        ship_flg.sprite = ResManager.GetSprite("knowcircular_sprite", "ship_flg");

        mGOIn = UguiMaker.newGameObject("mGOIn", ship_bodyTrans);
        mGOIn.transform.localPosition = new Vector3(17f, -395f, 0f);
        mGOIn.transform.localEulerAngles = Vector3.zero;
        for (int i = 0; i < 8; i++)
        {
            mPoss.Add(new Vector3(160f + i * 50f, 0f, 0f));
        }

        mAudioSource = gameObject.AddComponent<AudioSource>();
        mAudioSource.playOnAwake = false;
        mAudioSource.loop = false;
        mAudioSource.volume = 0.6f;
        mAudioSource.clip = ResManager.GetClip("knowcircular_sound", "shipshake");

        ship_bodyTrans.transform.localEulerAngles = Vector3.zero;

        mTo = UguiMaker.newGameObject("mto", transform);
        mTo.transform.localPosition = new Vector3(16f, 160f, 0f);

        msys = ResManager.GetPrefab("knowcircular_prefab", "kcstar", mTo.transform).GetComponent<ParticleSystem>();

        ResetInfos();
    }


    private void PlayShipSound(bool _play)
    {
        //if (_play)
        //{
        //    iePlay = iePlayShipSound();
        //    StartCoroutine(iePlay);
        //}
        //else
        //{
        //    if (iePlay != null)
        //        StopCoroutine(iePlay);
        //    mAudioSource.Stop();
        //}
    }
    //IEnumerator iePlay = null;
    //IEnumerator iePlayShipSound()
    //{
    //    yield return new WaitForSeconds(2f);
    //    mAudioSource.Play();
    //    PlayShipSound(true);
    //}


    GameObject mTo;
    public Vector3 GetDropTo()
    {
        return mTo.transform.position;
    }

    /// <summary>
    /// 摇摆Active
    /// </summary>
    /// <param name="_active"></param>
    public void ShakeLRActive(bool _active)
    {
        if (_active)
        {           
            ship_bodyTrans.transform.DORotate(new Vector3(0f, 0f, 15f), 1f).OnComplete(() =>
            {
                if (mShakeTween != null)
                {
                    mShakeTween.Restart();
                    PlayShipSound(_active);
                }
                else
                {
                    mShakeTween = ship_bodyTrans.transform.DORotate(new Vector3(0f, 0f, -15f), 2f).SetLoops(-1, LoopType.Yoyo);                  
                }
            });
        }
        else
        {
            if (mShakeTween != null)
            { mShakeTween.Pause(); }
            ship_bodyTrans.transform.DORotate(new Vector3(0f, 0f, 0f), 1f);
            PlayShipSound(_active);
        }
    }

    public void ResetInfos()
    {
        mInDropObjList.Clear();
        mPoss = Common.BreakRank(mPoss);
        for (int i = 0; i < imgPoints.Length; i++)
        {
            imgPoints[i].enabled = false;
        }
        PlayKBabyAnimation("Idle", 0f);
    }


    public void SetCirInShip(KCirObjLv1 _obj)
    {
        mInDropObjList.Add(_obj);
        _obj.transform.SetParent(mGOIn.transform);
        _obj.transform.localPosition = Vector3.zero;
        _obj.SetOK();
        ShakeObj();
        msys.Play();
        PlayShipLight(5);
    }
    private int nToCpp = 5;
    public void PlayShipLight(int _count)
    {
        nToCpp = _count;
        nncount = 0;
        PlayLight();
    }
    private int nncount = 0;
    private void PlayLight()
    {
        StartCoroutine(iePlayShipLight());
    }
    IEnumerator iePlayShipLight()
    {
        for (int i = 0; i < imgPoints.Length; i++)
        {
            if (i % 2 == 0)
                imgPoints[i].enabled = true;
            else
                imgPoints[i].enabled = false;
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < imgPoints.Length; i++)
        {
            if (i % 2 == 0)
                imgPoints[i].enabled = false;
            else
                imgPoints[i].enabled = true;
        }
        yield return new WaitForSeconds(0.2f);
        nncount++;
        if (nncount < nToCpp)
        {
            PlayLight();
        }
        else
        {
            for (int i = 0; i < imgPoints.Length; i++)
            {
                imgPoints[i].enabled = false;
            }
        }
    }


    /// <summary>
    /// 播放KBaby动画 
    /// </summary>
    /// <param name="_name">Defeated/Idle/Succeed</param>
    /// <param name="_durTime">持续时间 小于等于0则一直循环播放</param>
    public void PlayKBabyAnimation(string _name, float _durTime)
    {
        fDurTime = _durTime;
        if (spine_kbaby == null)
            return;
        StopCoroutine(ResetToIdle());
        aanimation = spine_kbaby.AnimationState.Data.SkeletonData.FindAnimation(_name);
        if (aanimation != null)
            spine_kbaby.AnimationState.SetAnimation(1, aanimation, true);
        if (aanimation != null && fDurTime > 0)
            StartCoroutine(ResetToIdle());
    }
    Spine.Animation aanimation = null;
    float fDurTime = 2f;
    private IEnumerator ResetToIdle()
    {
        yield return new WaitForSeconds(fDurTime);
        PlayKBabyAnimation("Idle", -2f);
    }


    public void ShakeObj()
    {
        StartCoroutine(TScale());
    }
    IEnumerator TScale()
    {
        for (float j = 0; j < 1f; j += 0.05f)
        {
            ship_bodyTrans.transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 2 * j) * 3 * Mathf.Sin(Mathf.PI * j));
            yield return new WaitForSeconds(0.01f);
        }
        ship_bodyTrans.transform.localEulerAngles = Vector3.zero;
    }



    void PlayJiaZiLine()
    {

    }


}
