using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

/// <summary>
/// 小怪兽
/// </summary>
public class KOAT_BallObj : MonoBehaviour
{
    public int nType = 0;
    private Image imgbg;
    private SkeletonGraphic mspine;
    bool bInit = false;

    float fhehe = 10f;

    private Button imgBtn;
    private System.Action<KOAT_BallObj> mClickCall = null;

    public void InitAwake(int _type)
    {
        nType = _type;
        imgbg = UguiMaker.newImage("imgbg", transform, "knowoneandten_sprite", "mobj" + nType, false);
        mspine = ResManager.GetPrefab("knowoneandten_prefab", "monster" + _type, transform).GetComponent<SkeletonGraphic>();
        mspine.transform.localPosition = new Vector3(0f, -28f, 0f);
        
        int nrandom = Random.Range(1, 4);
        if (nrandom == 1)
        {
            PlayAnimation("Idle_random1", -1f);
        }
        else if (nrandom == 2)
        { PlayAnimation("Idle_random2", -1f); }

        Image imgs = UguiMaker.newGameObject("imgbtn", transform).AddComponent<Image>();
        imgs.rectTransform.sizeDelta = imgbg.rectTransform.sizeDelta;
        imgs.color = new Color(1f, 1f, 1f, 0f);
        imgBtn = imgs.gameObject.AddComponent<Button>();
        imgBtn.transition = Selectable.Transition.None;
        imgBtn.onClick.AddListener(ButtonClick);

        ButtonActive(false);
        bInit = true;
    }

    /// <summary>
    /// 灰色背景显示
    /// </summary>
    /// <param name="_active"></param>
    public void BGActive(bool _active)
    {
        imgbg.enabled = _active;
    }
    /// <summary>
    /// spine显示/隐藏
    /// </summary>
    /// <param name="_active"></param>
    public void SpineActive(bool _active)
    {
        mspine.gameObject.SetActive(_active);
        imgbg.enabled = !_active;
    }
    //按钮触发事件active
    public void ButtonActive(bool _active)
    {
        imgBtn.gameObject.SetActive(_active);
    }
    /// <summary>
    /// 设置点击回调
    /// </summary>
    /// <param name="_call"></param>
    public void SetClickCall(System.Action<KOAT_BallObj> _call)
    {
        mClickCall = _call;
    }

    //点击call
    private void ButtonClick()
    {
        if (mClickCall != null)
        { mClickCall(this); }
    }

    /// <summary>
    /// 是否在显示spine
    /// </summary>
    /// <returns></returns>
    public bool IsActiveSpine()
    {
        return mspine.gameObject.activeSelf;
    }

    public void ClickShowSpine(bool _active = true)
    {
        //if (bIsChange)
        //    return;
        bIsChange = true;
        if (_active)
        {
            SpineActive(true);
            transform.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, 0.15f);
                bIsChange = false;
            });
        }
        else
        {
            SpineActive(false);
            transform.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, 0.15f);
                bIsChange = false;
            });
        }
    }
    bool bIsChange = false;


    void Update()
    {
        if (!bInit)
            return;

        transform.rotation = Quaternion.identity;

        if (mspine.gameObject.activeSelf)
        {
            if (fhehe > 0)
            {
                fhehe -= Time.deltaTime;
                if (fhehe <= 0)
                {
                    if (UnityEngine.Random.value > 0.5f)
                    {
                        PlayAnimation("Idle_random1", 2f);
                        //mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "Idle_" + UnityEngine.Random.Range(1, 3)), 0.5f);
                    }
                    else
                    {
                        PlayAnimation("Idle_random2", 2f);
                        //mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "Idle_t" + UnityEngine.Random.Range(1, 4)), 0.5f);
                    }
                    fhehe = UnityEngine.Random.Range(3, 7f);
                }
            }
        }
    }


    /// <summary>
    /// 播放动画 
    /// </summary>
    /// <param name="_name">Idle/Idle_random1/Idle_random2</param>
    /// <param name="_durTime">持续时间 小于等于0则一直循环播放</param>
    public void PlayAnimation(string _name, float _durTime)
    {
        fDurTime = _durTime;
        if (mspine == null)
            return;
        if (ieRestoidle != null)
        {
            StopCoroutine(ieRestoidle);
        }
        aanimation = mspine.AnimationState.Data.SkeletonData.FindAnimation(_name);
        if (aanimation != null)
            mspine.AnimationState.SetAnimation(1, aanimation, true);
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



}


/// <summary>
/// 手点击
/// </summary>
public class KOATHandClick : MonoBehaviour
{
    private Image handup;
    private Image handdown;
    private Image click;
    private GameObject goOffset;

    public void InitAwake()
    {
        goOffset = UguiMaker.newGameObject("offsetGO", transform);
        goOffset.transform.localPosition = new Vector3(34f, -24f, 0f);

        click = UguiMaker.newImage("click", goOffset.transform, "knowoneandten_sprite", "click", false);
        click.rectTransform.anchoredPosition = new Vector2(-40f, 20f);

        handup = UguiMaker.newImage("handup", goOffset.transform, "knowoneandten_sprite", "handup", false);
        handdown = UguiMaker.newImage("handdown", goOffset.transform, "knowoneandten_sprite", "handdown", false);
        

        ResetInfos();
    }

    private void ResetInfos()
    {
        handup.enabled = true;
        handdown.enabled = false;
        click.enabled = false;
    }

    Tween tween1;
    Tween tween2;
    public void PlayClick()
    {

        if (tween1 != null)
            tween1.Pause();
        if (tween2 != null)
            tween2.Pause();

        ResetInfos();

        tween1 = transform.DOScale(Vector3.one, 0.2f).OnComplete(() => 
        {
            handup.enabled = false;
            handdown.enabled = true;
            click.enabled = true;
            click.transform.localScale = Vector3.one * 0.001f;
            tween2 = click.transform.DOScale(Vector3.one * 2f, 0.2f).OnComplete(()=> 
            {
                ResetInfos();
            });
        });
    }

}


/// <summary>
/// 小方块
/// </summary>
public class KOAT_BlockObj : MonoBehaviour
{

    private Image imgRect0;
    private Image imgRect1;
    private Button imgBtn;

    private System.Action<KOAT_BlockObj> mClickCall = null;

    float fScale = 0.8f;

    public void InitAwake()
    {
        imgRect0 = UguiMaker.newImage("imgRect0", transform, "knowoneandten_sprite", "mblock1", false);
        imgRect1 = UguiMaker.newImage("imgRect1", transform, "knowoneandten_sprite", "mblock0", false);
        
        Image imgs = UguiMaker.newGameObject("imgbtn", transform).AddComponent<Image>();
        imgs.rectTransform.sizeDelta = imgRect0.rectTransform.sizeDelta;
        imgs.color = new Color(1f, 1f, 1f, 0f);
        imgBtn = imgs.gameObject.AddComponent<Button>();
        imgBtn.transition = Selectable.Transition.None;
        imgBtn.onClick.AddListener(ButtonClick);

        ButtonActive(false);
        BlockActive(false);

        transform.localScale = Vector3.one * fScale;
    }

    /// <summary>
    /// block显示/隐藏
    /// </summary>
    /// <param name="_active"></param>
    public void BlockActive(bool _active)
    {
        imgRect0.enabled = !_active;
        imgRect1.enabled = _active;
    }
    //按钮触发事件active
    public void ButtonActive(bool _active)
    {
        imgBtn.gameObject.SetActive(_active);
    }
    /// <summary>
    /// 设置点击回调
    /// </summary>
    /// <param name="_call"></param>
    public void SetClickCall(System.Action<KOAT_BlockObj> _call)
    {
        mClickCall = _call;
    }

    //点击call
    private void ButtonClick()
    {
        if (mClickCall != null)
        { mClickCall(this); }
    }

    /// <summary>
    /// 是否在显示block
    /// </summary>
    /// <returns></returns>
    public bool IsActiveBlock()
    {
        return imgRect1.enabled;
    }

    public void ClickShowBlock(bool _active = true)
    {
        if (_active)
        {
            BlockActive(true);
            transform.DOScale(Vector3.one * 1.2f * fScale, 0.2f).OnComplete(() =>
            {
                transform.DOScale(Vector3.one * fScale, 0.15f);
            });
        }
        else
        {
            BlockActive(false);
            transform.DOScale(Vector3.one * 1.2f * fScale, 0.2f).OnComplete(() =>
            {
                transform.DOScale(Vector3.one * fScale, 0.15f);
            });
        }
    }

}