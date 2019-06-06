using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PostBoxCtrl : MonoBehaviour
{
    /// <summary>
    /// 层ID
    /// </summary>
    public int nCeng = 0;
    public int nAnimalID = 0;
    /// <summary>
    /// 是否激活状态
    /// </summary>
    public bool bPostBoxActive = false;

    private Image imgbg;
    private Image yellowRect;
    private Image slider;   
    private Image imgtip;

    private Image numbg;
    private Image imgNum;
    private Image imgAnimalhead;
    private Image imgpick;

    private BoxCollider2D mbox2D;

    float fsliderStart = 60f;
    float fsliderEnd = 370f;
    float fimgTipStart = -125f;
    float fimgTipEnd = 180f;

    private LittlePostmanCtrl mCtrl;

    AudioSource maudiosource;

    public void InitAwake(int _cengID)
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as LittlePostmanCtrl;
        nCeng = _cengID;

        maudiosource = gameObject.AddComponent<AudioSource>();

        imgbg = transform.Find("imgbg").GetComponent<Image>();
        slider = transform.Find("slider").GetComponent<Image>();
        numbg = transform.Find("numbg").GetComponent<Image>();
        imgtip = transform.Find("imgtip").GetComponent<Image>();

        imgbg.sprite = ResManager.GetSprite("littlepostman_sprite", "redrect2");
        slider.sprite = ResManager.GetSprite("littlepostman_sprite", "redrect1");
        numbg.sprite = ResManager.GetSprite("littlepostman_sprite", "sque");
        imgtip.sprite = ResManager.GetSprite("littlepostman_sprite", "redrect0");

        imgAnimalhead = UguiMaker.newImage("animalhead", transform, "aa_animal_head", "1", false);
        imgAnimalhead.transform.localScale = Vector3.one * 0.47f;
        imgAnimalhead.transform.localPosition = new Vector3(30f, 0f, 0f);

        imgNum = UguiMaker.newImage("imgnum", numbg.transform, "public", "default" + _cengID);
        imgNum.color = Color.red;
        imgNum.transform.localScale = Vector3.one * 0.36f;

        imgpick = UguiMaker.newImage("imgpick", transform, "littlepostman_sprite", "pick");
        imgpick.transform.localPosition = new Vector3(226f, 10f, 0f);

        yellowRect = UguiMaker.newImage("yellowRect", transform, "littlepostman_sprite", "rect");
        yellowRect.transform.localPosition = new Vector3(34f, 0f, 0f);
        yellowRect.rectTransform.sizeDelta = new Vector2(294f, 56f);
        yellowRect.transform.SetSiblingIndex(1);

        mbox2D = gameObject.AddComponent<BoxCollider2D>();
        mbox2D.size = new Vector2(280, 45f);
        mbox2D.offset = new Vector2(34f, 0f);
        mbox2D.isTrigger = true;

        gameObject.name = "postBox" + nCeng;
        ResetStart();
    }

    /// <summary>
    /// box2d 碰撞体开/关
    /// </summary>
    /// <param name="_active"></param>
    public void Box2DActive(bool _active)
    {
        mbox2D.enabled = _active;
    }

    /// <summary>
    /// 重玩设置信息
    /// </summary>
    /// <param name="_animalID"></param>
    public void ReplayReset(int _animalID)
    {
        OpenPostBox();
        nAnimalID = _animalID;
        PickActive(false);
        YellowRectActive(false);
        AnimalHeadActive(false);
        Box2DActive(true);
        bPostBoxActive = false;
    }

    /// <summary>
    /// 初始open
    /// </summary>
    public void ResetStart()
    {
        imgtip.transform.localPosition = new Vector3(fimgTipStart, 0f, 0f);
        slider.rectTransform.sizeDelta = new Vector2(fsliderStart, slider.rectTransform.sizeDelta.y);
        PickActive(false);
        YellowRectActive(false);
        AnimalHeadActive(false);
        Box2DActive(true);
    }

    /// <summary>
    /// 打开邮箱
    /// </summary>
    public void OpenPostBox(System.Action _callbcak = null)
    {
        imgtip.transform.DOLocalMove(new Vector3(fimgTipStart, 0f, 0f), 0.3f).OnComplete(()=> 
        {
            if (_callbcak != null)
            { _callbcak(); }
        });
        DOTween.To(() => slider.rectTransform.sizeDelta, x => slider.rectTransform.sizeDelta = x, new Vector2(fsliderStart, slider.rectTransform.sizeDelta.y), 0.3f);
    }
    /// <summary>
    /// 关闭邮箱
    /// </summary>
    public void ClosePostBox(System.Action _callbcak = null)
    {
        imgtip.transform.DOLocalMove(new Vector3(fimgTipEnd, 0f, 0f), 0.3f).OnComplete(() =>
        {
            if (_callbcak != null)
            { _callbcak(); }
        }); ;
        DOTween.To(() => slider.rectTransform.sizeDelta, x => slider.rectTransform.sizeDelta = x, new Vector2(fsliderEnd, slider.rectTransform.sizeDelta.y), 0.3f);
    }

    /// <summary>
    /// yellow框显示/隐藏
    /// </summary>
    /// <param name="_active"></param>
    public void YellowRectActive(bool _active)
    {
        yellowRect.gameObject.SetActive(_active);
    }
    /// <summary>
    /// 错误提示显示/隐藏
    /// </summary>
    /// <param name="_active"></param>
    public void PickActive(bool _active)
    {
        imgpick.gameObject.SetActive(_active);
    }
    /// <summary>
    /// 播放错误提示
    /// </summary>
    public void PlayPick()
    {
        StartCoroutine(iePlayPick());
    }
    IEnumerator iePlayPick()
    {
        PickActive(true);
        yield return new WaitForSeconds(0.3f);
        PickActive(false);
        //yield return new WaitForSeconds(0.2f);
        //PickActive(true);
        //yield return new WaitForSeconds(0.2f);
        //PickActive(false);
    }

    /// <summary>
    /// 把信放入信箱
    /// </summary>
    /// <param name="_xinObj"></param>
    /// <param name="_isOK"></param>
    public void PlayXinSetIn(LittlePostMsgObj _xinObj, bool _isOK)
    {
        //清空hitPostBox
        mCtrl.hitPostBoxNow = null;
        bPostBoxActive = false;
        if (_isOK)
        {
            //关闭碰撞体
            Box2DActive(false);          
        }
        //Debug.Log("ok--- set in");
        YellowRectActive(false);
        transform.SetSiblingIndex(10);    
        _xinObj.Box2DActive(false);
        StartCoroutine(iePlayXinSetIn(_xinObj, _isOK));
    }
    IEnumerator iePlayXinSetIn(LittlePostMsgObj _xinObj, bool _isOK)
    {
        mCtrl.SoundCtrl.PlaySortSound("littlepostman_sound", "放信封");
        _xinObj.transform.SetParent(transform);
        _xinObj.transform.SetSiblingIndex(2);
        _xinObj.SetXin(true);
        _xinObj.transform.DOScale(Vector3.one * 0.45f, 0.3f);
        _xinObj.transform.DOLocalMove(new Vector3(30f, 7f, 0f), 0.3f);
        yield return new WaitForSeconds(0.31f);

        ClosePostBox();
        if (_isOK)
        { mCtrl.SoundCtrl.PlaySortSound("littlepostman_sound", "关门"); }
        else
        { mCtrl.SoundCtrl.PlaySortSound("littlepostman_sound", "关门错误"); }
        yield return new WaitForSeconds(0.31f);

        SetAnimalHead(_xinObj.nAnimalID);
        AnimalHeadActive(true);
        PlayPick();

        if (_isOK)
        {
            //Debug.Log("---is ok---");
            mCtrl.LevelCheckNext();
        }
        else
        {          
            //失败语音
            mCtrl.PlayFaileTipSound();
            yield return new WaitForSeconds(0.6f);
            AnimalHeadActive(false);
            OpenPostBox();
            yield return new WaitForSeconds(0.31f);
            mCtrl.SoundCtrl.PlaySortSound("littlepostman_sound", "错误弹出");
            _xinObj.transform.SetParent(mCtrl.transform);
            _xinObj.SetXin(false);
            _xinObj.transform.DOLocalMoveX(350f, 0.2f);
            _xinObj.transform.DOScale(Vector3.one, 0.2f);
            yield return new WaitForSeconds(0.2f);
            _xinObj.Box2DActive(true);
        }
        

    }


    private void PlaySortSound(string _strAB, string _name, float _volume)
    {
        AudioClip cp = mCtrl.SoundCtrl.GetClip(_strAB, _name);
        maudiosource.loop = false;
        maudiosource.volume = 1f;
        maudiosource.clip = cp;
        maudiosource.Play();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject);
        if (collision.gameObject.name.CompareTo("xinTrigger") == 0)
        {          
            if (!yellowRect.gameObject.activeSelf)
            {
                //mCtrl.SoundCtrl.PlaySortSound("littlepostman_sound", "黄色框", 0.2f);
                PlaySortSound("littlepostman_sound", "黄色框", 0.5f);
                mCtrl.hitPostBoxNow = this;
                bPostBoxActive = true;
                YellowRectActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject);
        if (collision.gameObject.name.CompareTo("xinTrigger") == 0)
        {        
            if (yellowRect.gameObject.activeSelf)
            {
                bPostBoxActive = false;
                YellowRectActive(false);
            }
        }
    }


    /// <summary>
    /// 动物头像显示/隐藏
    /// </summary>
    /// <param name="_active"></param>
    public void AnimalHeadActive(bool _active)
    {
        imgAnimalhead.gameObject.SetActive(_active);
    }
    /// <summary>
    /// 设置动物邮箱头像
    /// </summary>
    /// <param name="_id"></param>
    public void SetAnimalHead(int _id)
    {
        Sprite msp = ResManager.GetSprite("aa_animal_head", _id.ToString());
        imgAnimalhead.sprite = msp;
        imgAnimalhead.SetNativeSize();
    }

}
