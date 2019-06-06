using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System;
using Spine.Unity;

public class KCirObjLv1 : KnowDropObj
{
    /// <summary>
    /// 类型 0圆形 1四方形 2三角形
    /// </summary>
    public int nType = 0;
    private SkeletonGraphic mSpine;
    private KnowCircularCtrl mCtrl;

    private Vector3 vSpineStart;

    Canvas mCanvas;

    public override void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as KnowCircularCtrl;
        mbox2D = gameObject.AddComponent<BoxCollider2D>();
        mbox2D.size = Vector2.one * 120f;

        //gameObject.layer = 5;
        //mCanvas = gameObject.AddComponent<Canvas>();
        //mCanvas.overrideSorting = true;
        //mCanvas.sortingOrder = 3;

        GameObject mgo = null;
        if (nType == 0)
        { mgo = GameObject.Instantiate(mCtrl.GetObjType0); }
        else if (nType == 1)
        { mgo = GameObject.Instantiate(mCtrl.GetObjType1); }
        else
        { mgo = GameObject.Instantiate(mCtrl.GetObjType2); }
        mgo.transform.SetParent(transform);
        mgo.SetActive(true);
        mSpine = mgo.GetComponent<SkeletonGraphic>();
        mSpine.transform.localScale = Vector3.one * 0.2f;
        mSpine.transform.localPosition = new Vector3(0f, -45f, 0f);
        if (nType == 0)
        { mSpine.transform.localPosition = new Vector3(0f, -55f, 0f); }
        else if (nType == 1)
        { mSpine.transform.localPosition = new Vector3(0f, -65f, 0f); }

        fScale = 1f;
        transform.localScale = Vector3.one * 0.001f;
        transform.DOScale(Vector3.one * fScale, 0.2f).SetEase(Ease.OutBack);

        vSpineStart = mSpine.transform.localPosition;

        fhehe = UnityEngine.Random.Range(5, 10f);
        binit = true;

        PlayFudongTween(true);
    }

    public bool bDropState = false;

    bool binit = false;
    float fhehe = 10f;
    private void Update()
    {
        if (!binit)
            return;  
        if (!mCtrl.mLevel1.bCanDrop)
            return;
        if (bDropState)
            return;
        if (mbox2D == null)
            return;
        if (!mbox2D.enabled)
            return;
        if (fhehe > 0)
        {
            fhehe -= Time.deltaTime;
            if (fhehe <= 0)
            {
                if (UnityEngine.Random.value > 0.5f)
                {
                    PlayAnimation("Idle_random_a", 2f);
                    mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "Idle_" + UnityEngine.Random.Range(1, 3)), 0.5f);
                }
                else
                {
                    PlayAnimation("Idle_random_b", 2f);
                    mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "Idle_t" + UnityEngine.Random.Range(1, 4)), 0.5f);
                }
                fhehe = UnityEngine.Random.Range(5, 10f);
            }
        }
    }


    /// <summary>
    /// 播放动画 
    /// </summary>
    /// <param name="_name">Click/Defeated/Idle/Idle_random_a/Idle_random_b/Succeed</param>
    /// <param name="_durTime">持续时间 小于等于0则一直循环播放</param>
    public void PlayAnimation(string _name, float _durTime)
    {
        fDurTime = _durTime;
        if (mSpine == null)
            return;
        if (ieRestoidle != null)
        {
            StopCoroutine(ieRestoidle);
        }
        aanimation = mSpine.AnimationState.Data.SkeletonData.FindAnimation(_name);
        if (aanimation != null)
            mSpine.AnimationState.SetAnimation(1, aanimation, true);
        if (aanimation != null && _durTime > 0)
        {
            ieRestoidle = ResetToIdle();
            StartCoroutine(ieRestoidle);
        }
    }
    Spine.Animation aanimation = null;
    float fDurTime = 2f;
    IEnumerator ieRestoidle;
    private IEnumerator ResetToIdle()
    {
        yield return new WaitForSeconds(fDurTime);
        PlayAnimation("Idle", -2f);
    }

    Vector3 vRemindPos;
    public void SetRemindPos(Vector3 _pos)
    {
        vRemindPos = _pos;
    }
    public void BackToRemindPos()
    {
        transform.DOLocalMove(vRemindPos, 0.2f);
    }

    public override void DropReset(Action _call = null)
    {
        SetOrderReander(3);
        transform.DOScale(Vector3.one * fScale, 0.2f).OnComplete(() =>
        {
            if (_call != null)
            { _call(); }
            PlayFudongTween(true);
        });
    }

    Tween fudongTween;
    public void PlayFudongTween(bool _play)
    {
        if (_play)
            fudongTween = mSpine.transform.DOLocalMoveY(vSpineStart.y + 8f, 1f).SetLoops(-1, LoopType.Yoyo);
        else
        {
            if (fudongTween != null)
            {
                fudongTween.Pause();
                mSpine.transform.localPosition = vSpineStart;
            }
        }
    }


    public void SetOK()
    {
        Box2DActive(false);
        PlayAnimation("Succeed", 0f);
        transform.DOScale(Vector3.one * 1.3f, 0.2f).SetEase(Ease.OutBack).OnComplete(()=> 
        {
            transform.DOScale(Vector3.one * 0.001f, 0.5f).SetDelay(2f);
        });
    }

    public void SetOrderReander(int _order)
    {
        if(mCanvas != null)
        mCanvas.sortingOrder = _order;
    }

}


public class KCirObjLv2 : KnowDropObj
{
    /// <summary>
    /// 类型 0圆形 1四方形 2三角形
    /// </summary>
    public int nType = 0;
    public int nIndex = 0;
    private Image imgObj;
    private Image imgYing;
    public bool bInStation = false;
    private Vector3 vRemindPos;

    public override void InitAwake()
    {
        imgYing = UguiMaker.newImage("imgObj", transform, "knowcircular_sprite", "mobj" + nType + "_" + nIndex + "ying", false);
        imgObj = UguiMaker.newImage("imgObj", transform, "knowcircular_sprite", "mobj" + nType + "_" + nIndex, false);
        
        mbox2D = gameObject.AddComponent<BoxCollider2D>();
        mbox2D.size = imgObj.rectTransform.sizeDelta;

        fScale = 1f;
        transform.localScale = Vector3.one * 0.001f;
        transform.DOScale(Vector3.one * fScale, 0.2f).SetEase(Ease.OutBack);
    }
    public void SetRemindPos()
    {
        vRemindPos = transform.localPosition;
    }
    public void BackToRemindPos()
    {
        transform.DOLocalMove(vRemindPos, 0.2f);
    }
    public void YingActive(bool _active)
    {
        imgYing.enabled = _active;
    }

}


public class KCirObjLv3 : KnowDropObj
{
    private Image imgbg;
    private Image imgRang;
    private Button btn;

    System.Action<KCirObjLv3> mClickCall;

    public override void InitAwake()
    {
        imgbg = gameObject.GetComponent<Image>();
        imgbg.color = new Color(1f, 1f, 1f, 0f);
        btn = gameObject.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        btn.onClick.AddListener(ClickCall);
        int nindex = UnityEngine.Random.Range(0, 3);
        imgRang = UguiMaker.newImage("imgRang", transform, "knowcircular_sprite", "draw0", false);
        imgRang.rectTransform.sizeDelta = imgbg.rectTransform.sizeDelta + Vector2.one * 20f;
        imgRang.enabled = false;
        imgRang.color = Color.blue;
        imgRang.type = Image.Type.Filled;
        imgRang.fillMethod = Image.FillMethod.Radial360;
        imgRang.fillAmount = 0;
        imgRang.fillOrigin = 2;
    }

    public void SetClickCall(System.Action<KCirObjLv3> _call)
    {
        mClickCall = _call;
    }

    private void ClickCall()
    {
        if (mClickCall != null)
        {
            mClickCall(this);
        }
    }

    public void ShowRang()
    {
        imgRang.enabled = true;
        imgbg.raycastTarget = false;
        DOTween.To(() => imgRang.fillAmount, x => imgRang.fillAmount = x, 1f, 0.5f);
    }
    public void HideRang()
    {
        DOTween.To(() => imgRang.color, x => imgRang.color = x, new Color(0f,0f,1f,0f), 0.5f);
    }
}
