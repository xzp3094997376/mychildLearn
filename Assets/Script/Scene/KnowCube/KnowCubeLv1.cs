using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// level 1
/// </summary>
public class KnowCubeLv1 : MonoBehaviour
{
    private Vector3 vStartPos = new Vector3(0f,-35f,0f);
    private RawImage bg;

    private GameObject mLeft;
    private ActionCube mCube;
    private Vector3 vCube = new Vector3(0f, 0f, 0f);

    private GameObject mRight;
    private Image line1;
    private Image line2;
    private Image line3;
    private Image line4;
    private Vector3 vline1;
    private Vector3 vline2;
    private Vector3 vline3;
    private Vector3 vline4;
    private Button btntip1;
    private Button btntip2;
    private Button btntip3;

    private KnowInputBtn btn0Ctrl;
    private Button btn0;

    public InputNumObj mInputNumObj;

    private KnowInputBtn btn2Ctrl;
    private Button btn2;

    private MaskBtns maskbtn2;

    private Button btn3_1;
    private Button btn3_2;
    Image img31;
    Image img32;
    Vector3 vbtn32;

    private bool bInit = false;

    bool bOK0 = false;
    bool bOK2 = false;
    bool bOK3 = false;

    KnowCubeCtrl mCtrl;
    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as KnowCubeCtrl;
        transform.localPosition = vStartPos;

        bg = transform.Find("map1").GetComponent<RawImage>();
        bg.texture = ResManager.GetTexture("knowcube_texture", "map2");
        bg.rectTransform.sizeDelta = new Vector2(1339f, 721f);

        mLeft = UguiMaker.newGameObject("mLeft", transform);
        mLeft.transform.localPosition = new Vector3(-268f, 50f, 0f);

        //right
        mRight = transform.Find("mRight").gameObject;
        line1 = mRight.transform.Find("line1").GetComponent<Image>();
        line2 = mRight.transform.Find("line2").GetComponent<Image>();
        line3 = mRight.transform.Find("line3").GetComponent<Image>();
        line4 = mRight.transform.Find("line4").GetComponent<Image>();
        line1.sprite = ResManager.GetSprite("knowcube_sprite", "line");
        line2.sprite = ResManager.GetSprite("knowcube_sprite", "line");
        line3.sprite = ResManager.GetSprite("knowcube_sprite", "line");
        line4.sprite = ResManager.GetSprite("knowcube_sprite", "line");

        btntip1 = line1.transform.Find("btntip1").GetComponent<Button>();
        btntip2 = line2.transform.Find("btntip2").GetComponent<Button>();
        btntip3 = line3.transform.Find("btntip3").GetComponent<Button>();
        EventTriggerListener.Get(btntip1.gameObject).onClick = PlaySoundTipClick;
        EventTriggerListener.Get(btntip2.gameObject).onClick = PlaySoundTipClick;
        EventTriggerListener.Get(btntip3.gameObject).onClick = PlaySoundTipClick;

        vline1 = line1.transform.localPosition;
        vline2 = line2.transform.localPosition;
        vline3 = line3.transform.localPosition;
        vline4 = line4.transform.localPosition;

        btn0Ctrl = line1.transform.Find("btn0").gameObject.AddComponent<KnowInputBtn>();
        btn0Ctrl.InitAwake();
        btn0 = btn0Ctrl.transform.GetComponent<Button>();
        EventTriggerListener.Get(btn0.gameObject).onClick = ClickBtn0;

        InputInfoData infoda = new InputInfoData();
        infoda.strAlatsName = "knowcube_sprite";
        infoda.strPicBG = "zbgbg";
        infoda.strCellPicFirstName = "z";
        infoda.fNumScale = 0.73f;
        infoda.bgcolor = Color.white;
        infoda.color_blockBG = new Color(221f / 255, 177f / 255, 118f / 255, 1f);
        infoda.color_blockNum = new Color(155f / 255, 93f / 255, 33f / 255, 1f);
        infoda.color_blockSureStart = new Color(155f / 255, 93f / 255, 33f / 255, 1f);
        mInputNumObj = InputNumObj.Create(transform, infoda);
        mInputNumObj.transform.localPosition = new Vector3(370f, -12f, 0f);
        mInputNumObj.gameObject.SetActive(false);
        mInputNumObj.SetInputNumberCallBack(() => 
        {
            btn0Ctrl.SetBtnText(mInputNumObj.strInputNum,45);
            btn0Ctrl.btnflash.rectTransform.anchoredPosition = new Vector2(20f, 0f);
            btn0Ctrl.btnflash.gameObject.SetActive(true);
        });
        mInputNumObj.SetFinishInputCallBack(() =>
        {
            if (mInputNumObj.strInputNum.CompareTo("") == 0)
            {
                btn0Ctrl.SetBtnText("?", 45);
            }
            else
            {
                btn0Ctrl.btn0Txt.text = mInputNumObj.strInputNum;
                if (mInputNumObj.nInputNum != 6)
                {
                    btn0Ctrl.BtnShake("?", 45);

                    string failename = "game-tips4-1-13";
                    if (Random.value > 0.5f)
                    { failename = "game-tips4-1-13-a"; }
                    AudioClip cp = mCtrl.GetClip(failename);
                    mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp, 1f);
                }
                else
                {
                    bOK0 = true;
                    btn0Ctrl.btn0Img.sprite = ResManager.GetSprite("knowcube_sprite", "btn0_2");
                    btn0.enabled = false;
                    CheckAllOK();

                    AudioClip cp = mCtrl.GetClip("game-tips4-1-12");
                    mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp, 1f);
                }
            }
            btn0Ctrl.btnflash.rectTransform.anchoredPosition = Vector2.zero;
            btn0Ctrl.btnflash.gameObject.SetActive(false);
        });
        mInputNumObj.SetClearNumberCallBack(() => 
        {
            btn0Ctrl.SetBtnText("",45);
            btn0Ctrl.btnflash.rectTransform.anchoredPosition = Vector2.zero;
        });

        btn2Ctrl = line2.transform.Find("btn2").gameObject.AddComponent<KnowInputBtn>();
        btn2Ctrl.InitAwake();
        btn2 = btn2Ctrl.transform.GetComponent<Button>();
        EventTriggerListener.Get(btn2.gameObject).onClick = ClickBtn2;

        maskbtn2 = mRight.transform.Find("maskbtn2").gameObject.AddComponent<MaskBtns>();
        maskbtn2.InitAwake();
        maskbtn2.gameObject.SetActive(false);

        SetMaskBtn2CallBack();

        btn3_1 = line3.transform.Find("btn1").GetComponent<Button>();
        btn3_2 = line3.transform.Find("btn2").GetComponent<Button>();
        EventTriggerListener.Get(btn3_1.gameObject).onDown = ClickDownBtn3;
        EventTriggerListener.Get(btn3_2.gameObject).onDown = ClickDownBtn3;
        EventTriggerListener.Get(btn3_1.gameObject).onUp = ClickUpBtn3;
        EventTriggerListener.Get(btn3_2.gameObject).onUp = ClickUpBtn3;
        img31 = btn3_1.GetComponent<Image>();
        img32 = btn3_2.GetComponent<Image>();
        img31.sprite = ResManager.GetSprite("knowcube_sprite", "btn1_1");
        img32.sprite = ResManager.GetSprite("knowcube_sprite", "btn2_1");
        vbtn32 = img32.transform.localPosition;

        mClickBallTip = GuideHandCtl.Create(transform);

        mLeft.transform.localScale = Vector3.one * 1.3f;
        bInit = true;
    }

    public GameObject CreateSpineFace(Transform _tr,Vector3 _vpos)
    {
        GameObject mgo = ResManager.GetPrefab("knowcube_prefab", "face");
        mgo.transform.SetParent(_tr);
        mgo.transform.localScale = Vector3.one;
        mgo.transform.localPosition = _vpos;
        return mgo;
    }

    public void ResetInfos()
    {
        bOK0 = false;
        bOK2 = false;
        bOK3 = false;
        doonce = false;
        img31.sprite = ResManager.GetSprite("knowcube_sprite", "btn1_1");
        img32.sprite = ResManager.GetSprite("knowcube_sprite", "btn2_1");

        btn0Ctrl.ResetInfos();
        btn2Ctrl.ResetInfos();

        if (mCube != null)
        {
            if (mCube.gameObject != null)
                GameObject.Destroy(mCube.gameObject);
            mCube = null;
        }

        mRight.gameObject.SetActive(false);
        btn0.enabled = true;
        btn0Ctrl.SetBtnText("?", 45);
        btn2Ctrl.SetBtnText("?", 45);
        maskbtn2.strChooseObjName = "";
        maskbtn2.ChangeColor(new Color(1f, 1f, 1f, 1f));
        nQuaCount = 0;
    }

    public void SetData()
    {
        this.StopAllCoroutines();
        ResetInfos();

        //action Cube Create
        int ntype = 1;//Random.Range(1, 3);
        mCube = ResManager.GetPrefab("knowcube_prefab", "changecube" + ntype).GetComponent<ActionCube>();
        mCube.transform.SetParent(mLeft.transform);
        mCube.transform.localEulerAngles = new Vector3(-8f, -8f, 0f);
        mCube.transform.localScale = Vector3.one * 0.55f;
        mCube.transform.localPosition = vCube + new Vector3(-800f, 0f, 0f);
        mCube.InitAwake(ntype);
        mCube.SetFirstOverCallback(SetRightData);

        mCtrl.StartCoroutine(IEStartData01());
    }
    IEnumerator IEStartData01()
    {
        SceneMove(true);
        yield return new WaitForSeconds(1.1f);
        mCube.transform.DOLocalMove(vCube, 1f).OnComplete(()=> 
        {
            ShowClickTip(mCube.transform.position + new Vector3(0f, 0.1f, 0f));
        });
        //Debug.Log("我是正方体，你知道我有几个面吗？点我，看看会发生什么？");

        mCtrl.SetTipSound(IEPlayTipSound1());
        mCtrl.PlayTipSound();
    }



    void Update()
    {
        if (!bInit)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            #region//射线检测
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                ActionCubePanel com = hit.collider.gameObject.GetComponent<ActionCubePanel>();
                if (com != null && mCube != null)
                {
                    mCube.ChangeOpenClose();
                    mCtrl.PlayTheSortSound("clickblock");
                    StopClickTip();
                }
            }
            #endregion
        }
    }


    //right
    int nQuaCount = 0;
    bool doonce = false;
    public void SetRightData()
    {
        if (doonce)
            return;
        doonce = true;
        mRight.SetActive(true);
        line1.transform.localPosition = vline1 + new Vector3(800f, 0f, 0f);
        line2.transform.localPosition = vline2 + new Vector3(800f, 0f, 0f);
        line3.transform.localPosition = vline3 + new Vector3(800f, 0f, 0f);
        line4.transform.localPosition = vline4 + new Vector3(800f, 0f, 0f);

        ShowQuw();      
    }

    public void ShowQuw()
    {
        StartCoroutine(IEWaiteShow());
    }
    IEnumerator IEWaiteShow()
    {
        yield return new WaitForSeconds(1f);
        if (nQuaCount == 0)
        {
            PlayMoveOutSound();
            line1.transform.DOLocalMove(vline1, 0.3f);
        }
        if (nQuaCount == 1)
        {
            PlayMoveOutSound();
            line2.transform.DOLocalMove(vline2, 0.3f);
        }
        if (nQuaCount == 2)
        {
            PlayMoveOutSound();
            line3.transform.DOLocalMove(vline3, 0.3f);
            line4.transform.DOLocalMove(vline4, 0.3f);
        }
    }

    AudioClip theOutCP= null;
    public void PlayMoveOutSound()
    {
        if (theOutCP == null)
        { theOutCP = Resources.Load<AudioClip>("sound/素材出现通用音效"); }
        if (theOutCP != null)
        {
            AudioSource.PlayClipAtPoint(theOutCP, Camera.main.transform.position);
        }
    }

    private void ClickBtn0(GameObject _go)
    {
        if (bOK0)
            return;
        if (mInputNumObj.gameObject.activeSelf)
            return;
        mInputNumObj.ShowEffect();
        btn0Ctrl.SetBtnText("",45);
        btn0Ctrl.btnflash.rectTransform.anchoredPosition = Vector2.zero;
        btn0Ctrl.btnflash.gameObject.SetActive(true);
    }

    private void ClickBtn2(GameObject _go)
    {
        if (bOK2)
            return;
        if (maskbtn2.gameObject.activeSelf)
            return;

        AudioClip cp = Resources.Load<AudioClip>("sound/button_down");
        mCtrl.PlayTheSortSound(cp);

        btn2Ctrl.SetBtnText("");
        btn2Ctrl.btnflash.rectTransform.anchoredPosition = Vector2.zero;
        btn2Ctrl.btnflash.gameObject.SetActive(true);
        maskbtn2.ShowBtns();
    }
    private void SetMaskBtn2CallBack()
    {
        maskbtn2.SetCloseCB(() => 
        {
            btn2Ctrl.SetBtnText("?",45);
            btn2Ctrl.btnflash.rectTransform.anchoredPosition = Vector2.zero;
            btn2Ctrl.btnflash.gameObject.SetActive(false);
        });
        maskbtn2.SetChooseCB(() => 
        {
            btn2Ctrl.btnflash.gameObject.SetActive(false);
            if (maskbtn2.strChooseObjName.CompareTo("正方形") == 0)
            {
                bOK2 = true;
                btn2Ctrl.btn0Img.sprite = ResManager.GetSprite("knowcube_sprite", "btn0_2");
                btn2.enabled = false;
                btn2Ctrl.btn0Txt.text = maskbtn2.strChooseObjName;
                maskbtn2.gameObject.SetActive(false);
                CheckAllOK();
                AudioClip cp0 = mCtrl.GetClip("game-tips4-1-14");
                mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp0, 1f);
            }
            else
            {
                btn2Ctrl.btn0Txt.text = maskbtn2.strChooseObjName;
                btn2Ctrl.BtnShake("",30,()=> { btn2Ctrl.btnflash.gameObject.SetActive(true); });
                AudioClip cp0 = mCtrl.GetClip("game-tips4-1-16");
                mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp0, 1f);
            }
        });
    }

    private void ClickDownBtn3(GameObject _go)
    {
        if (bOK3)
            return;

        AudioClip cp = Resources.Load<AudioClip>("sound/button_down");
        mCtrl.PlayTheSortSound(cp);

        if (_go == btn3_1.gameObject)
        {
            img31.sprite = ResManager.GetSprite("knowcube_sprite", "btn1_2");
        }
        else if (_go == btn3_2.gameObject)
        {
            img32.sprite = ResManager.GetSprite("knowcube_sprite", "btn2_2");
        }
    }
    private void ClickUpBtn3(GameObject _go)
    {
        if (bOK3)
            return;

        if (_go == btn3_1.gameObject)
        {
            bOK3 = true;
            img31.sprite = ResManager.GetSprite("knowcube_sprite", "btn1_3");
            CheckAllOK();
            AudioClip cp0 = mCtrl.GetClip("game-tips4-1-17");
            mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp0, 1f);
        }
        else if (_go == btn3_2.gameObject)
        {
            img32.sprite = ResManager.GetSprite("knowcube_sprite", "btn2_3");
            Btn32Shake();
            AudioClip cp0 = mCtrl.GetClip("game-tips4-1-18");
            mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp0, 1f);
        }
    }
    private void Btn32Shake()
    {
        img32.transform.DOLocalMove(vbtn32 + new Vector3(15f, 0f, 0f), 0.1f).OnComplete(() =>
        {
            img32.transform.DOLocalMove(vbtn32 - new Vector3(15f, 0f, 0f), 0.2f).OnComplete(() =>
            {
                img32.transform.DOLocalMove(vbtn32, 0.1f).OnComplete(() =>
                {
                    img32.sprite = ResManager.GetSprite("knowcube_sprite", "btn2_1");
                });
            });
        });
    }

    /// <summary>
    /// 检测是否全部完成
    /// </summary>
    private void CheckAllOK()
    {
        nQuaCount++;
        ShowQuw();
        if (bOK0 && bOK2 && bOK3)
        {
            mCtrl.bLvPass = true;
            //Debug.Log("lv1 ok");
            StartCoroutine(IEToLevel2());
        }
    }
    IEnumerator IEToLevel2()
    {
        yield return new WaitForSeconds(2f);
        //Debug.Log("你发现了没？正方体有6个面，每个面都是一样大小的正方形");
        AudioClip cp0 = mCtrl.GetClip("game-tips4-1-3");
        mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp0, 1f);       
        yield return new WaitForSeconds(cp0.length + 0.1f);

        yield return new WaitForSeconds(1f);
        KnowCubeCtrl mctrl = SceneMgr.Instance.GetNowScene() as KnowCubeCtrl;
        mctrl.LevelPass();
    }

    public void SceneMove(bool _in)
    {
        if (_in)
        {
            transform.localPosition = new Vector3(0f, -800f, 0f);
            transform.DOLocalMove(vStartPos, 1f);
        }
        else
        {
            transform.DOLocalMove(new Vector3(0f, -800f, 0f), 1f);
        }
    }

    /// <summary>
    /// 点击文字播放问题语音
    /// </summary>
    /// <param name="_go"></param>
    private void PlaySoundTipClick(GameObject _go)
    {
        if (mCtrl.bLvPass)
            return;
        AudioClip cp = null;
        if (_go.name.CompareTo(btntip1.gameObject.name) == 0)
        {
            cp = mCtrl.GetClip("game-tips4-1-4");
        }
        else if (_go.name.CompareTo(btntip2.gameObject.name) == 0)
        {
            cp = mCtrl.GetClip("game-tips4-1-5");
        }
        else if (_go.name.CompareTo(btntip3.gameObject.name) == 0)
        {
            cp = mCtrl.GetClip("game-tips4-1-6");
        }
        if (cp != null)
            mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp, 1f);
    }


    /// <summary>
    /// click 指引
    /// </summary>
    private GuideHandCtl mClickBallTip;
    public void ShowClickTip(Vector3 _worldPos)
    {
        mClickBallTip.GuideTipClick(1.3f, 0.7f, true, true,"hand1");
        mClickBallTip.SetClickTipPos(_worldPos);
        mClickBallTip.SetClickTipOffsetPos(new Vector3(9f, -32f, 0f), 1f);
        mClickBallTip.transform.localPosition = mClickBallTip.transform.localPosition - new Vector3(0f, 50f, 0f);
    }
    public void StopClickTip()
    {
        mClickBallTip.StopClick();
    }



    //开始提示1
    public IEnumerator IEPlayTipSound1()
    {
        AudioClip cp0 = mCtrl.GetClip("game-tips4-1-1");
        mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp0, 1f);
        yield return new WaitForSeconds(cp0.length);
        AudioClip cp1 = mCtrl.GetClip("game-tips4-1-2");
        mCtrl.PlaySound(mCtrl.mKimiAudioSource, cp1, 1f);
    }

}


public class KnowInputBtn : MonoBehaviour
{

    public Image btn0Img;
    public Text btn0Txt;
    public Image btnflash;
    private Vector3 vbtn0;

    private float flashtime = 0.4f;

    bool bInit = false;

    public void InitAwake()
    {
        btn0Txt = transform.Find("Text").GetComponent<Text>();
        btnflash = transform.Find("flash").GetComponent<Image>();
        btnflash.gameObject.SetActive(false);
        vbtn0 = transform.localPosition;
        btn0Img = gameObject.GetComponent<Image>();
        btn0Img.sprite = ResManager.GetSprite("knowcube_sprite", "btn0_1");

        bInit = true;
    }

    public void BtnShake(string _setText,int fontsize,System.Action _callbacl = null)
    {
        btn0Img.sprite = ResManager.GetSprite("knowcube_sprite", "btn0_3");
        transform.DOLocalMove(vbtn0 + new Vector3(15f, 0f, 0f), 0.1f).OnComplete(() =>
        {
            btn0Img.sprite = ResManager.GetSprite("knowcube_sprite", "btn0_1");
            transform.DOLocalMove(vbtn0 - new Vector3(15f, 0f, 0f), 0.2f).OnComplete(() =>
            {
                btn0Img.sprite = ResManager.GetSprite("knowcube_sprite", "btn0_3");
                transform.DOLocalMove(vbtn0, 0.1f).OnComplete(() =>
                {
                    SetBtnText(_setText, fontsize);
                    btn0Img.sprite = ResManager.GetSprite("knowcube_sprite", "btn0_1");
                    if (_callbacl != null)
                        _callbacl();
                });
            });
        });
    }

    public void SetBtnText(string _txt,int fontsize = 30)
    {
        btn0Txt.text = _txt;
        btn0Txt.fontSize = fontsize; 

    }
    public void SetBtnFlashPox(Vector2 _vpos)
    {
        btnflash.rectTransform.anchoredPosition = _vpos;
    }


    public void ResetInfos()
    {
        btnflash.gameObject.SetActive(false);
        btn0Img.sprite = ResManager.GetSprite("knowcube_sprite", "btn0_1");
    }

    private void Update()
    {
        if (!bInit)
            return;
        flashtime -= Time.deltaTime;
        if (flashtime <= 0f)
        {
            btnflash.enabled = false;
            if (flashtime <= -0.4f)
            {
                btnflash.enabled = true;
                flashtime = 0.4f;
            }
        }
    }
}


public class MaskBtns : MonoBehaviour
{
    private GameObject btns;
    private Button btn0;
    private Button btn1;
    private Button btn2;
    private Button btn3;

    private System.Action mCloseCallback = null;
    private System.Action mChooseCallback = null;

    public void SetCloseCB(System.Action _cb)
    {
        mCloseCallback = _cb;
    }
    public void SetChooseCB(System.Action _cb)
    {
        mChooseCallback = _cb;
    }


    public string strChooseObjName = "";

    bool binit = false;
    public void InitAwake()
    {
        btns = transform.Find("btns").gameObject;
        btn0 = btns.transform.Find("cbtn0").GetComponent<Button>();
        btn1 = btns.transform.Find("cbtn1").GetComponent<Button>();
        btn2 = btns.transform.Find("cbtn2").GetComponent<Button>();
        btn3 = btns.transform.Find("cbtn3").GetComponent<Button>();
        EventTriggerListener.Get(btn0.gameObject).onClick = ClickBtn;
        EventTriggerListener.Get(btn1.gameObject).onClick = ClickBtn;
        EventTriggerListener.Get(btn2.gameObject).onClick = ClickBtn;
        EventTriggerListener.Get(btn3.gameObject).onClick = ClickBtn;

        Image img0 = btn0.GetComponent<Image>();
        Image img1 = btn1.GetComponent<Image>();
        Image img2 = btn2.GetComponent<Image>();
        Image img3 = btn3.GetComponent<Image>();
        img0.sprite = ResManager.GetSprite("knowcube_sprite", "btn0_1");
        img1.sprite = ResManager.GetSprite("knowcube_sprite", "btn0_1");
        img2.sprite = ResManager.GetSprite("knowcube_sprite", "btn0_1");
        img3.sprite = ResManager.GetSprite("knowcube_sprite", "btn0_1");
        binit = true;
    }

    private void ClickBtn(GameObject _go)
    {
        ChangeColor(new Color(1f, 1f, 1f, 1f));
        _go.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
        Text txtget = _go.transform.Find("Text").GetComponent<Text>();
        if (txtget != null)
            strChooseObjName = txtget.text;
        else
            strChooseObjName = "";
        if (mChooseCallback != null)
            mChooseCallback();
    }

    public void ChangeColor(Color _color)
    {
        btn0.GetComponent<Image>().color = _color;
        btn1.GetComponent<Image>().color = _color;
        btn2.GetComponent<Image>().color = _color;
        btn3.GetComponent<Image>().color = _color;
    }

    public void ShowBtns()
    {
        gameObject.SetActive(true);
        btns.transform.localPosition = new Vector3(0f, 230f, 0f);
        btns.transform.DOLocalMove(Vector3.zero, 0.3f);
    }

    private void Update()
    {
        if (!binit)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit2d = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit2d)
            {
                if (hit2d.collider.gameObject != btns.gameObject)
                {
                    gameObject.SetActive(false);
                    if (mCloseCallback != null)
                        mCloseCallback();
                }
            }
            else
            {
                gameObject.SetActive(false);
                if (mCloseCallback != null)
                    mCloseCallback();
            }

            AudioClip cp = Resources.Load<AudioClip>("sound/button_up");
            AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position);
        }
    }
}
