using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

/// <summary>
/// 珍珠数字
/// </summary>
public class SDN_Panel3Zhenzhu : MonoBehaviour
{

    public miniImageNumber imgNumber;
    /// <summary>
    /// 是否在贝壳里
    /// </summary>
    public bool bInStation = false;
    
    private BoxCollider2D mbox2d;
    private Image mFish;
    SkeletonGraphic fishSpine;

    public int setInPointIndex = 0;

    private Vector3 vRemeberpos;

    private Tween fishMoveTween = null;
    private int nDir = -1;
    private float fMoveMinTime = 8f;
    private float fMoveMaxTime = 12f;
    private float fStartTimeMove = 8f;

    private float fMinX = -550f;
    private float fMaxX = 550f;

    private SingleAndDualNumCtrl mCtrl;

    public void InitAwake(int _num)
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as SingleAndDualNumCtrl;

        int nfishID = Random.Range(0, 4);
        mFish = UguiMaker.newImage("fish", transform, "singledualnum_sprite", "flash" + nfishID, false);

        GameObject mgofish = GameObject.Instantiate(mCtrl.GetFishResByID(nfishID)) as GameObject;
        mgofish.SetActive(true);
        fishSpine = mgofish.GetComponent<SkeletonGraphic>();
        fishSpine.transform.SetParent(mFish.transform);
        fishSpine.transform.localScale = Vector3.one * 1.2f;
        fishSpine.transform.localPosition = new Vector3(-20f, -80f, 0f);
        mFish.enabled = false;

        if (Random.value > 0.5f)
        { nDir = 1; }
        else
        { nDir = -1; }

        mbox2d = gameObject.AddComponent<BoxCollider2D>();
        mbox2d.size = mFish.rectTransform.sizeDelta;
        mbox2d.isTrigger = true;
        
        transform.localPosition = new Vector3(Random.Range(-410f, 410f), Random.Range(-220f, 280f), 0f);

        GameObject mgo = UguiMaker.newGameObject("imgNumber", transform);
        mgo.transform.localScale = Vector3.one * 0.35f;
        imgNumber = mgo.AddComponent<miniImageNumber>();
        imgNumber.strABName = "singledualnum_sprite";
        imgNumber.strFirstPicName = "";
        imgNumber.SetNumColor(new Color(219f / 255, 112f / 255, 2f / 255, 1f));
        imgNumber.SetNumber(_num);

        imgNumber.CreateImgNumBG("singledualnum_sprite", "rang1", Vector2.one * 250f, new Color(1f, 1f, 1f, 210f / 255));

        fStartTimeMove = Random.Range(fMoveMinTime, fMoveMaxTime);
    }

    private void CreateNum(string _name,Transform _trans)
    {
        GameObject num1GO = new GameObject(_name);
        num1GO.transform.SetParent(_trans);
        num1GO.transform.localScale = Vector3.one;
        num1GO.transform.localPosition = Vector3.zero;
        Image imgnum = num1GO.AddComponent<Image>();
        imgnum.color = new Color(210f / 255, 117f / 255, 7f / 255, 1f);
    }



    public void BoxActive(bool _active)
    {
        mbox2d.enabled = _active;
    }

    public void SetRememberPos(Vector3 _pos)
    {
        vRemeberpos = _pos;
    }

    Tween theTween = null;
    public void StopScaleTween()
    {
        if (theTween != null)
        {
            theTween.Pause();
        }
    }

    public void MoveToRememberPos()
    {
        //transform.DOLocalMove(vRemeberpos, 0.3f).OnComplete(()=> 
        //{
        //    PlayMove();
        //});
        PlayMove();
    }

    public Vector2 GetSize()
    {
        return mbox2d.size;
    }

    public void DropSet()
    {
        theTween = transform.DOScale(Vector3.one * 1.3f, 0.2f);
        PlayFishAction("Click");
    }
    public void DropReset()
    {
        theTween = transform.DOScale(Vector3.one, 0.2f);
        PlayFishAction("Idle");
    }

    public void ShowOut()
    {
        transform.SetSiblingIndex(0);
        transform.localScale = Vector3.one * 0.001f;
        theTween = transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        PlayFishAction("Idle");
    }

    /// <summary>
    /// 播放鱼动作
    /// </summary>
    /// <param name="_name">Click/Idle</param>
    public void PlayFishAction(string _name)
    {
        fishSpine.AnimationState.SetAnimation(1, _name, true);
    }


    /// <summary>
    /// 匹配错误弹回
    /// </summary>
    public void FaileMoveBack()
    {
        BoxActive(false);
        transform.DOScale(Vector3.one, 0.6f);
        StartCoroutine(IEToyMoveBack());
    }
    IEnumerator IEToyMoveBack()
    {
        Vector3 vfrom = transform.localPosition;
        //vRemaidLocalPos = vPath[2];
        for (float i = 0; i < 1f; i += 0.04f)
        {
            transform.localPosition = Vector3.Lerp(vfrom, vRemeberpos, i) + new Vector3(0f, 200f, 0f) * Mathf.Sin(Mathf.PI * i);
            yield return new WaitForSeconds(0.02f * Mathf.Sin(Mathf.PI * i));
        }
        BoxActive(true);
        PlayMove();
    }


    float ftime = 8;
    public void DoFishMove()
    {
        ftime = CuaTime();
        PlayFishAction("Idle");
        if (nDir > 0)//右
        {
            imgNumber.transform.localPosition = new Vector3(20f, 0f, 0f);
            mFish.transform.localScale = new Vector3(-1, 1f, 1f);
            fishMoveTween = transform.DOLocalMoveX(fMaxX, ftime).SetEase(Ease.Linear).OnComplete(() =>
            {
                nDir = -1;
                DoFishMove();
            });
        }
        else//左
        {
            imgNumber.transform.localPosition = new Vector3(-20f, 0f, 0f);
            mFish.transform.localScale = Vector3.one;
            fishMoveTween = transform.DOLocalMoveX(fMinX, ftime).SetEase(Ease.Linear).OnComplete(() =>
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
    public void PlayMove()
    {
        DoFishMove();
    }

    //计算move时间
    private float CuaTime()
    {
        float oldTime = fStartTimeMove;
        float disToRight = fMaxX - transform.localPosition.x;
        if (nDir <0)
        { disToRight = 1000f - disToRight; }
        float newTime = oldTime * Mathf.Abs(disToRight) / 1000f;
        //Debug.Log("dis:" + disToRight + "  time:" + newTime);
        return newTime;
    }

}
