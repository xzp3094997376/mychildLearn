using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class WFF_RocketObj : MonoBehaviour
{

    public int nID = 0;
    public int nAnimalID = 0;
    public int nNum = 0;//排序id

    public float fSetX = 0f;


    private GameObject baseOBj;
    private Image rocketbg;
    private Image[] fires = new Image[3];
    private Image imgNum;//数字img
    float fScale = 0.85f;
    private Image imgInputTip;

    private GameObject mAnimal;
    private SkeletonGraphic mAnimalSpine;

    private BoxCollider2D mNumBox2D;
    private BoxCollider2D mBox2D;

    public float fUpDown = 1f;
    public float fFluy = 13f;
    private float fGetY = 0f;
    private float fIndex = 0;
    public float fFireTime = 0.04f;
    public bool bshake = false;

    float fStart = 0f;
    float fEnd = 0f;

    Vector3[] starPath = new Vector3[5];
    private ParticleSystem moveStar;

    private WhoFlyFastCtrl mCtrl;
    

    public void InitAwake(int _id)
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as WhoFlyFastCtrl;
        nID = _id;
        baseOBj = transform.Find("base").gameObject;
        rocketbg = baseOBj.transform.Find("rocketbg").GetComponent<Image>();
        rocketbg.sprite = ResManager.GetSprite("whoflyfast_sprite", "rocket" + nID);

        fires[0] = baseOBj.transform.Find("fire0").GetComponent<Image>();
        fires[1] = baseOBj.transform.Find("fire1").GetComponent<Image>();
        fires[2] = baseOBj.transform.Find("fire2").GetComponent<Image>();
        fires[0].sprite = ResManager.GetSprite("whoflyfast_sprite", "fire0");
        fires[1].sprite = ResManager.GetSprite("whoflyfast_sprite", "fire1");
        fires[2].sprite = ResManager.GetSprite("whoflyfast_sprite", "fire2");
        PlayFire();

        imgNum = UguiMaker.newImage("imgNum", baseOBj.transform, "number_slim", "0", false);
        imgNum.color = new Color(124f / 255, 27f / 255, 30f / 255, 1f);
        imgNum.transform.localPosition = new Vector3(-7f, -25f,0f);
        imgNum.transform.localScale = Vector3.one * fScale;

        mNumBox2D = imgNum.gameObject.AddComponent<BoxCollider2D>();
        mNumBox2D.size = new Vector2(120f, 120f);

        imgInputTip = UguiMaker.newGameObject("inputTip", baseOBj.transform).AddComponent<Image>();
        imgInputTip.raycastTarget = false;
        imgInputTip.color = Color.black;
        imgInputTip.transform.localPosition = new Vector3(-5f, -25f, 0f);
        imgInputTip.rectTransform.sizeDelta = new Vector2(4f, 50f);
        SetFlashInputTip();
        SetInputTipActive(false);

        mBox2D = gameObject.AddComponent<BoxCollider2D>();
        mBox2D.size = new Vector2(300f, 130f);
        mBox2D.offset = new Vector2(0f, -38f);

        transform.localScale = Vector3.one * 0.85f;

        fUpDown = UnityEngine.Random.Range(1f, 1.8f);
        fFluy = UnityEngine.Random.Range(12f, 18f);

        starPath[0] = new Vector3(-90f, 20f, 0f);
        starPath[1] = new Vector3(120f, 20f, 0f);
        starPath[2] = new Vector3(120f, -90f, 0f);
        starPath[3] = new Vector3(-90f, -90f, 0f);
        starPath[4] = new Vector3(-90f, 20f, 0f);

        moveStar = ResManager.GetPrefab("fx_movestar", "fx_movestar", transform).GetComponent<ParticleSystem>();
        moveStar.startSize = 0.07f;
        moveStar.Stop();
        moveStar.gameObject.SetActive(false);

        
    }

    public void PlayMoveStart()
    {
        mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("whoflyfast_sound", "startmove"));
        moveStar.transform.localPosition = starPath[0];
        moveStar.gameObject.SetActive(true);
        moveStar.Play();
        moveStar.transform.DOLocalPath(starPath, 2.5f).OnComplete(()=> 
        {
            moveStar.Stop();
            moveStar.gameObject.SetActive(false);
        });
    }

    public void ResetInfo()
    {
        SetState(true);
        StopPingpong();
        transform.localPosition = new Vector3(1000f, 0f, 0f);
        if (mAnimal != null)
        {
            GameObject.Destroy(mAnimal);
            mAnimal = null;
            mAnimalSpine = null;
        }
        nAnimalID = 0;
        InitStartData();
    }

    public void InitStartData()
    {
        nNum = 0;
        SetNumBox2DActive(true);
        SetNumberActive(false);
        SetBox2DActive(false);
        moveStar.Stop();
        moveStar.gameObject.SetActive(false);
        PlayAnimation("face_idle", true);
    }



    /// <summary>
    /// 输入光标显示/隐藏
    /// </summary>
    public void SetInputTipActive(bool _active)
    {
        imgInputTip.gameObject.SetActive(_active);
        if (nNum > 0)
            imgInputTip.transform.localPosition = new Vector3(16f, -25f, 0f);
        else
        { imgInputTip.transform.localPosition = new Vector3(-3f, -25f, 0f); }
    }
    /// <summary>
    /// 数字显示/隐藏
    /// </summary>
    public void SetNumberActive(bool _active)
    {
        imgNum.enabled = _active;
    }
    /// <summary>
    /// 碰撞体显示/隐藏
    /// </summary>
    public void SetNumBox2DActive(bool _active)
    {
        mNumBox2D.enabled = _active;
    }
    public void SetBox2DActive(bool _active)
    {
        mBox2D.enabled = _active;
    }
    //数字错误设置
    public void SetWrong()
    {
        nNum = 0;
        tweenImgnum1 = imgNum.transform.DOLocalMoveY(50f, 0.5f).OnComplete(()=> 
        { imgNum.transform.localPosition = new Vector3(-7f, -25f, 0f); });
        tweenImgnum2 = DOTween.To(() => imgNum.color, x => imgNum.color = x, new Color(124f / 255, 27f / 255, 30f / 255, 0f), 0.5f);
        PlayAnimation("face_sayno", false);
    }
    Tween tweenImgnum1 = null;
    Tween tweenImgnum2 = null;


    /// <summary>
    /// 设置航线 启动移动
    /// </summary>
    /// <param name="_fY"></param>
    public void SetData(int _animalID, float _fY, float _moveOutTime, bool _setPingpong)
    {
        bSetPingpong = _setPingpong;
        nAnimalID = _animalID;
        CreateAnimal(nAnimalID);

        transform.localPosition = new Vector3(1000f, _fY -37f, 0f);
        fStart = UnityEngine.Random.Range(-420f, 0f);
        fEnd = UnityEngine.Random.Range(50f, 400f);
        float fTime = _moveOutTime;

        transform.DOLocalMoveX(fStart, fTime).SetEase(Ease.Linear).OnComplete(()=> 
        {
            if (bSetPingpong)
            {
                float fPingpong = UnityEngine.Random.Range(3f, 6f);
                tweenPingpong = transform.DOLocalMoveX(fEnd, fPingpong).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            }
        });
    }

    public void DoPingpongMove()
    {
        fStart = UnityEngine.Random.Range(-420f, 0f);
        fEnd = UnityEngine.Random.Range(50f, 400f);
        transform.DOLocalMoveX(fStart, 2f).OnComplete(() =>
        {
            float fPingpong = UnityEngine.Random.Range(2f, 3f);
            tweenPingpong = transform.DOLocalMoveX(fEnd, fPingpong).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        });
    }

    /// <summary>
    /// 设置航线 启动移动level2
    /// </summary>
    public void SetData(int _animalID, float _fY, float _moveOutTime, float _to, bool _createAnimal = false)
    {
        bSetPingpong = false;
        if (_createAnimal)
        {
            nAnimalID = _animalID;
            CreateAnimal(nAnimalID);
        }
        transform.localPosition = new Vector3(1000f, _fY - 37f, 0f);
        fStart = _to;
        fEnd = UnityEngine.Random.Range(50f, 400f);
        float fTime = _moveOutTime;
        transform.DOLocalMoveX(fStart, fTime).SetEase(Ease.Linear);
    }
    bool bSetPingpong = false;
    Tween tweenPingpong = null;

    public Image GetImgNum()
    {
        return imgNum;
    }

    /// <summary>
    /// 停止pingpong移动
    /// </summary>
    public void StopPingpong()
    {
        if(tweenPingpong != null)
        {
            tweenPingpong.Pause();
        }
        SetState(false);
    }

    /// <summary>
    /// 设置排名数字
    /// </summary>
    /// <param name="_num"></param>
    public void SetNumber(int _num, bool _showInputTip = true)
    {
        if (tweenImgnum1 != null)
            tweenImgnum1.Pause();
        if (tweenImgnum2 != null)
            tweenImgnum2.Pause();

        imgNum.color = new Color(124f / 255, 27f / 255, 30f / 255, 1f);
        imgNum.transform.localPosition = new Vector3(-7f, -25f, 0f);
        nNum = _num;
        imgNum.sprite = ResManager.GetSprite("number_slim", _num.ToString());
        SetNumberActive(true);
        if (scaleTween != null)
            scaleTween.Pause();
        imgNum.transform.localScale = Vector3.one * fScale;
        scaleTween = imgNum.transform.DOScale(Vector3.one * fScale * 1.2f, 0.2f).OnComplete(() =>
        {
            scaleTween = imgNum.transform.DOScale(Vector3.one * fScale, 0.15f);
        });
        SetInputTipActive(_showInputTip);
    }
    Tween scaleTween = null;

    /// <summary>
    /// 平速/加速
    /// </summary>
    /// <param name="_fast"></param>
    public void SetState(bool _fast)
    {
        bshake = true;
        if (_fast)
        {
            //bshake = true;
            fFireTime = 0.015f;
        }
        else
        {
            //bshake = false;
            fFireTime = 0.04f;
        }
    }
    
    private void Update()
    {
        if (bshake)
        {
            fIndex += Time.deltaTime * fFluy;
            if (fIndex >= 4)
            { fIndex = 0; }
            fGetY = Mathf.Sin(Mathf.PI * fIndex) * fUpDown;

            baseOBj.transform.localPosition = Vector3.zero + new Vector3(0f, fGetY, 0f);
        }
    }
    
    /// <summary>
    /// 上下抖动
    /// </summary>
    public void PlayShake(bool _shake)
    {
        bshake = _shake;
    }
    /// <summary>
    /// 火焰动画
    /// </summary>
    public void PlayFire()
    {
        StartCoroutine(iePlayFire());
    }
    IEnumerator iePlayFire()
    {
        yield return new WaitForSeconds(fFireTime);
        HideFire(1);
        yield return new WaitForSeconds(fFireTime);
        HideFire(2);
        yield return new WaitForSeconds(fFireTime);
        HideFire(0);
        PlayFire();
    }
    private void HideFire(int _index)
    {
        for (int i = 0; i < fires.Length; i++)
        { fires[i].enabled = false; }
        fires[_index].enabled = true;
    }
    //create animal
    private void CreateAnimal(int _id)
    {
        string strName = MDefine.GetAnimalNameByID_EN(_id);
        mAnimal = ResManager.GetPrefab("aa_animal_person_prefab", strName, baseOBj.transform);
        mAnimalSpine = mAnimal.transform.GetChild(0).GetComponent<SkeletonGraphic>();
        mAnimal.transform.SetSiblingIndex(0);
        mAnimal.transform.localPosition = new Vector3(7f, -20f, 0f);
        mAnimal.transform.localScale = Vector3.one * 0.75f;

        PlayAnimation("face_idle", true);
    }

    private void SetFlashInputTip()
    {     
        imgInputTip.transform.DOScaleY(1f, 0.5f).OnComplete(() => 
        {
            imgInputTip.enabled = false;
            imgInputTip.transform.DOScaleY(1f, 0.5f).OnComplete(() => 
            {
                imgInputTip.enabled = true;
                SetFlashInputTip();
            });
        });
    }

    /// <summary>
    /// 放大效果
    /// </summary>
    public void DoScaleEffect()
    {
        mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("whoflyfast_sound", "setbig"));
        transform.SetSiblingIndex(6);
        transform.DOScale(Vector3.one * fScale * 1.2f, 0.25f).OnComplete(()=> 
        {
            transform.DOScale(Vector3.one * fScale, 0.2f);
        });
    }

    /// <summary>
    /// 移出场景
    /// </summary>
    public void RocketMoveOut()
    {
        SetState(true);
        float fGet = transform.localPosition.x - 1200f;
        transform.DOLocalMoveX(fGet, 4f).SetEase(Ease.Linear);
    }


    public void ShakeObj()
    {
        PlayAnimation("face_sayno", false);
        StartCoroutine(TScale());
    }
    IEnumerator TScale()
    {
        for (float j = 0; j < 1f; j += 0.05f)
        {
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * j) * 10);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localEulerAngles = Vector3.zero;
    }


    /// <summary>
    /// 播放动画 face_idle/face_sayno/face_sayyes/face_walk
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_loop"></param>
    public void PlayAnimation(string _name, bool _loop)
    {
        if (mAnimalSpine == null)
            return;
        StopCoroutine(ResetToIdle());  
        aanimation = mAnimalSpine.AnimationState.Data.SkeletonData.FindAnimation(_name);
        //Debug.Log(_name);
        if (aanimation != null)
            mAnimalSpine.AnimationState.SetAnimation(1, aanimation, _loop);

        if (aanimation != null && !_loop)
            StartCoroutine(ResetToIdle());
    }

    Spine.Animation aanimation = null;
    private IEnumerator ResetToIdle()
    {
        float ftime = aanimation.Duration;
        yield return new WaitForSeconds(ftime);
        PlayAnimation("face_idle", true);
    }


    public void DoLocalMoveX(float _to)
    {
        transform.DOLocalMoveX(_to, 0.5f);
    }

}
