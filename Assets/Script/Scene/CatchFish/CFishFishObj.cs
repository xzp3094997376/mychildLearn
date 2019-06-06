using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

/// <summary>
/// 鱼Obj
/// </summary>
public class CFishFishObj : MonoBehaviour
{

    public int nFishType = 0;

    /// <summary>
    /// 鱼所在的station(0底下water  1leftBox  2rightBox)
    /// </summary>
    public int nInStation = 0;

    private Image imgfish;
    private BoxCollider2D mbox2d;

    private Tween fishMoveTween = null;
    private int nDir = -1;
    private float fMinX = -100f;
    private float fMax = 100f;
    private float fMoveMinTime = 9f;
    private float fMoveMaxTime = 15f;

    private SkeletonGraphic mspine;

    public void InitAwake(int _type)
    {
        nFishType = _type;
        imgfish = UguiMaker.newImage("imgfish", transform, "catchfish_sprite", "fish" + nFishType, false);
        imgfish.enabled = false;

        mspine = ResManager.GetPrefab("catchfish_prefab", "fish" + nFishType).GetComponent<SkeletonGraphic>();
        mspine.transform.SetParent(transform);
        mspine.transform.localScale = Vector3.one;
        mspine.transform.localPosition = new Vector3(0f, -70f, 0f);
        PlayAnimation("Idle");

        mbox2d = gameObject.AddComponent<BoxCollider2D>();
        mbox2d.size = imgfish.rectTransform.sizeDelta;
    }

    public Vector2 GetSize()
    {
        return mbox2d.size;
    }

    public void BoxActive(bool _active)
    {
        mbox2d.enabled = _active;
    }

    public void SetFishSize(float _fscale)
    {
        Vector3 vscale = transform.localScale;
        transform.localScale = vscale * _fscale;
    }

    /// <summary>
    /// 设置鱼移动x值限制
    /// </summary>
    public void SetMoveLimiteX(float _min, float _max)
    {
        fMinX = _min;
        fMax = _max;
    }

    public void SetMoveLimiteTime(float _min, float _max)
    {
        fMoveMaxTime = _max;
        fMoveMinTime = _min;
    }
    
    public void DoFishMove()
    {
        if (nDir > 0)
        {
            Vector3 vscale = transform.localScale;
            float fdir = vscale.x;
            transform.localScale = new Vector3(Mathf.Abs(fdir), vscale.y, vscale.z);
            fishMoveTween = transform.DOLocalMoveX(fMax, Random.Range(fMoveMinTime, fMoveMaxTime)).SetEase(Ease.Linear).OnComplete(()=> 
            {
                nDir = -1;
                DoFishMove();
            });
        }
        else
        {
            Vector3 vscale = transform.localScale;
            float fdir = vscale.x;
            transform.localScale = new Vector3(Mathf.Abs(fdir) * -1, vscale.y, vscale.z);
            fishMoveTween = transform.DOLocalMoveX(fMinX, Random.Range(fMoveMinTime, fMoveMaxTime)).SetEase(Ease.Linear).OnComplete(()=> 
            {
                nDir = 1;
                DoFishMove();
            });
        }
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        if (fishMoveTween != null)
            fishMoveTween.Pause();
    }

    /// <summary>
    /// 设置开始方向(-1left, 1right)
    /// </summary>
    /// <param name="_dir"></param>
    public void SetDirction(int _dir)
    {
        nDir = _dir;
    }
    /// <summary>
    /// 设置开始随机方向
    /// </summary>
    public void SetRandomDirction()
    {
        if (Random.value > 0.5f)
        { SetDirction(1); }
        else
        { SetDirction(-1); }
    }

    /// <summary>
    /// 播放动画 Idle / Struggle
    /// </summary>
    /// <param name="_name"></param>
    public void PlayAnimation(string _name)
    {
        mspine.AnimationState.SetAnimation(0, _name, true);
    }

    public void StopPlayAnimal()
    {
        PlayAnimation("Idle");
        StartCoroutine(IEStopAnimation());
    }
    IEnumerator IEStopAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        mspine.AnimationState.TimeScale = 0f;
    }

    /// <summary>
    /// 进场
    /// </summary>
    public void SceneMoveIn()
    {
        BoxActive(false);
        transform.DOScale(Vector3.one, 0.25f).OnComplete(()=> 
        {
            SetRandomDirction();
            SetMoveLimiteX(-550f, 550f);
            DoFishMove();
            BoxActive(true);
        });
    }

    #region//Guide hand click
    GuideHandCtl mGuideCtrl;
    public void CreateGuideObj()
    {
        mGuideCtrl = GuideHandCtl.Create(transform);
        mGuideCtrl.GuideTipClick(0.8f, 0.7f, true, true, "hand1");
        mGuideCtrl.SetClickTipOffsetPos(new Vector3(8f, -25f, 0f));
    }
    public void StopGuideHand()
    {
        if (mGuideCtrl != null)
        {
            if (mGuideCtrl.gameObject != null)
                GameObject.Destroy(mGuideCtrl.gameObject);

            if (mnextGuideCallback != null)
                mnextGuideCallback();

            mGuideCtrl = null;
        }
    }
    System.Action mnextGuideCallback = null;
    public void SetNextGuideCallback(System.Action _callback)
    {
        mnextGuideCallback = _callback;
    }
    #endregion

}
