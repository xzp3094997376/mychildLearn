using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class SingleDualNum_panel1 : MonoBehaviour
{
    /// <summary>
    /// 数字
    /// </summary>
    public int nNum = 0;
    public int nFinishCount = 0;

    private Transform top;
    private SingleDualNum_beike[] beikes = new SingleDualNum_beike[10];

    private Transform center;
    private Transform balls;
    private Image yellowballBase;
    //private Image mText;

    private RectTransform hand;

    private Image xuxian;
    private Image lineSp;
    private Image imageStart;
    private Image imageEnd;

    private Image imageDrop;
    private BoxCollider2D dropBox2D;

    List<SingleDualNum_yellowball> mYellowBallList = new List<SingleDualNum_yellowball>();


    Vector3 vInput = Vector3.zero;
    Image mSelect = null;
    public bool bDropOK = false;

    SingleAndDualNumCtrl mCtrl;

    /// <summary>
    /// 引导是否完成
    /// </summary>
    public bool bGuideFinish = false;


    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as SingleAndDualNumCtrl;

        top = transform.Find("top");
        for (int i = 0; i < 10; i++)
        {
            int index = i + 1;
            beikes[i] = top.Find("beike" + index).gameObject.AddComponent<SingleDualNum_beike>();
            beikes[i].InitStart(index);
        }

        center = transform.Find("center");

        balls = center.Find("balls");
        yellowballBase = center.Find("yellowballBase").GetComponent<Image>();
        yellowballBase.sprite = ResManager.GetSprite("singledualnum_sprite", "yellowrang");
        yellowballBase.gameObject.SetActive(false);

        //mText = center.FindChild("Text").GetComponent<Image>();
        //mText.gameObject.SetActive(false);

        hand = center.Find("hand") as RectTransform;
        Image handImg = hand.Find("Image").GetComponent<Image>();
        handImg.sprite = ResManager.GetSprite("singledualnum_sprite", "hand");
        handImg.SetNativeSize();
        hand.gameObject.SetActive(false);

        xuxian = center.Find("xuxian").GetComponent<Image>();
        xuxian.sprite = ResManager.GetSprite("singledualnum_sprite", "xuxian");

        lineSp = center.Find("dropline").GetComponent<Image>();
        lineSp.rectTransform.sizeDelta = new Vector2(0.01f, 8f);

        imageDrop = center.Find("imageDrop").GetComponent<Image>();
        imageDrop.sprite = ResManager.GetSprite("singledualnum_sprite", "rang1");
        imageDrop.rectTransform.sizeDelta = new Vector2(75f, 75f) * 0.5f;
        dropBox2D = imageDrop.GetComponent<BoxCollider2D>();
        dropBox2D.size = new Vector2(200f, 200f);

        imageStart = center.Find("imageStart").GetComponent<Image>();
        imageStart.sprite = ResManager.GetSprite("singledualnum_sprite", "rang1");
        imageStart.rectTransform.sizeDelta = new Vector2(75f, 75f) * 0.5f;

        imageEnd = center.Find("imageEnd").GetComponent<Image>();
        imageEnd.sprite = ResManager.GetSprite("singledualnum_sprite", "rang1");
        imageEnd.rectTransform.sizeDelta = new Vector2(75f, 75f) * 0.5f;
        BoxCollider2D dropendBox = imageEnd.GetComponent<BoxCollider2D>();
        dropendBox.size = new Vector2(200f, 200f);

        vInput = imageStart.rectTransform.anchoredPosition3D;

        transform.localPosition = new Vector3(0f, 900f, 0f);
        bGuideFinish = false;


        xuxian.color = new Color(1f, 1f, 1f, 0f);
        lineSp.color = new Color(1f, 1f, 1f, 0f);
        imageStart.color = new Color(1f, 1f, 1f, 0f);
        imageEnd.color = new Color(1f, 1f, 1f, 0f);
        imageDrop.color = new Color(1f, 1f, 1f, 0f);
    }

    //line reset
    private void ResetLine()
    {
        lineSp.rectTransform.anchoredPosition = imageStart.rectTransform.anchoredPosition;
        lineSp.rectTransform.sizeDelta = new Vector2(0.01f, lineSp.rectTransform.sizeDelta.y);
    }
    
    void Update()
    {
        if (bDropOK)
            return;
        if (bGuide)
        {
            LineCtrol();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            #region//stp1
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                if (hit.collider.gameObject == imageDrop.gameObject)
                {
                    mSelect = imageDrop;
                    vInput = imageDrop.rectTransform.anchoredPosition3D;
                    hand.localPosition = imageStart.transform.localPosition;
                    YellowBallBoxActive(true);
                    mCtrl.PlayTheSortSound("dropline");
                }
            }
            #endregion
        }
        else if (Input.GetMouseButton(0))
        {
            #region//stp2
            if (mSelect != null)
            {
                vInput = Common.getMouseLocalPos(transform);
                float fX = vInput.x;
                float fY = imageStart.rectTransform.anchoredPosition.y;
                if (fX < -500)
                    fX = -500f;
                vInput = new Vector3(fX, fY, 0f);
                mSelect.rectTransform.anchoredPosition3D = vInput;

                LineCtrol();
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//stp3
            if (mSelect != null)
            {
                #region//射线检测
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hits != null)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].collider.gameObject == imageEnd.gameObject)
                        {
                            SetDropOK();
                            break;
                        }
                    }
                }
                #endregion

                if (!bDropOK)
                {
                    imageDrop.rectTransform.DOLocalMove(imageStart.rectTransform.localPosition, 0.3f).OnUpdate(LineCtrol);
                    YellowBallBoxActive(false);
                    mCtrl.PlayTheSortSound("lineback");
                }

                mSelect = null;
            }
            #endregion          
        }     
    }
    private void LineCtrol()
    {
        hand.localPosition = imageDrop.rectTransform.localPosition;
        //长度
        float dis = Vector3.Distance(imageStart.rectTransform.anchoredPosition3D, imageDrop.rectTransform.anchoredPosition3D);
        lineSp.rectTransform.sizeDelta = new Vector2(dis, lineSp.rectTransform.sizeDelta.y);
        lineSp.rectTransform.anchoredPosition3D = (imageDrop.rectTransform.anchoredPosition3D + imageStart.rectTransform.anchoredPosition3D) * 0.5f;
        //旋转方向
        Vector3 dir = (imageDrop.rectTransform.anchoredPosition3D - imageStart.rectTransform.anchoredPosition3D).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(new Vector3(0f, 0f, targetAngle));
        lineSp.rectTransform.localRotation = q;
    }

    public bool bPlayNumSound = false;
    /// <summary>
    /// 拖线完成
    /// </summary>
    private void SetDropOK()
    {
        mCtrl.bButtonActive = false;
        bDropOK = true;
        vInput = imageEnd.rectTransform.anchoredPosition3D;
        imageDrop.rectTransform.anchoredPosition3D = vInput;
        LineCtrol();
        mCtrl.PlayTheSortSound("linesuc");
        //粒子move
        mCtrl.MovePraSys(imageStart.transform.position, imageEnd.transform.position,1f);

        beikes[nNum - 1].SetState(3);
        bPlayNumSound = true;
        mCtrl.StartCoroutine(IEToFinishNumber());
    }
    IEnumerator IEToFinishNumber()
    {
        yield return new WaitForSeconds(0.5f);
        List<AudioClip> aclist = new List<AudioClip>();
        aclist.Add(mCtrl.GetClip("ice"));
        aclist.Add(mCtrl.GetNumClip(nNum));       
        if (IsSingleNumber())
        {
            //Debug.LogError(nNum + " 是奇数");
            aclist.Add(mCtrl.GetClip("game-tips2-4-1"));
        }
        else
        {
            //Debug.LogError(nNum + " 是偶数");
            aclist.Add(mCtrl.GetClip("game-tips2-4-2"));
        }
        for (int i = 0; i < aclist.Count; i++)
        {
            float actime = aclist[i].length;
            mCtrl.PlaySound(aclist[i], 1f);
            yield return new WaitForSeconds(actime + 0.1f);
        }
        yield return new WaitForSeconds(1f);

        nFinishCount++;
        SingleDualNum_beike nextBeike = GetNextBeike();

        //自动下一个数
        int nextNum = nNum + 1;
        if (nFinishCount < 10)
        {          
            SetNumber(nextBeike.nNum);
            mCtrl.bButtonActive = true;
        }
        else
        {
            //to panel2
            mCtrl.ChangePanel(1, 2);
            mCtrl.bButtonActive = true;
        }

        bPlayNumSound = false;
    }

    /// <summary>
    /// 重置信息
    /// </summary>
    public void ResetInfos()
    {
        //mText.enabled = true;
        vInput = imageStart.rectTransform.anchoredPosition3D;
        imageDrop.rectTransform.localPosition = vInput;
        bDropOK = false;
        for (int i = 0; i < mYellowBallList.Count; i++)
        {
            if (mYellowBallList[i].gameObject != null)
                GameObject.Destroy(mYellowBallList[i].gameObject);
        }
        mYellowBallList.Clear();

        for (int i = 0; i < beikes.Length; i++)
        {
            beikes[i].ResetInfos();
        }
    }

    /// <summary>
    /// 初始化数字
    /// </summary>
    public void SetNumber(int _number)
    {
        ResetInfos();
        ResetLine();

        nNum = _number;
        beikes[_number - 1].SetState(2);

        //mText.gameObject.SetActive(true);
        //mText.transform.localScale = Vector3.one * 0.001f;
        //mText.transform.DOScale(Vector3.one, 0.3f);
        //mText.sprite = ResManager.GetSprite("singledualnum_sprite", _number.ToString());
        //mText.SetNativeSize();

        List<Vector3> posList = GetPoss(_number);
        for (int i=0;i<nNum;i++)
        {
            SingleDualNum_yellowball yeball = CreateYellowBall(yellowballBase, balls.transform, posList[i]);
            yeball.SetBoxActive(false);
            yeball.EffectEnter();
            mYellowBallList.Add(yeball);
        }
    }

    /// <summary>
    /// 碰撞体开/关
    /// </summary>
    /// <param name="_active"></param>
    private void YellowBallBoxActive(bool _active)
    {
        if (IsSingleNumber())
        {
            mYellowBallList[mYellowBallList.Count - 1].SetBoxActive(_active);
        }
    }

    /// <summary>
    /// 判断是否为奇数
    /// </summary>
    private bool IsSingleNumber()
    {
        return nNum % 2 == 1;
    }

    
    /// <summary>
    /// 引导操作
    /// </summary>
    public void PlayGuide(System.Action _Callback = null)
    {
        //清除
        ResetInfos();

        mCtrl.StopCoroutine("IENextGuide1");

        guideFinishCallback = _Callback;
        bGuide = true;
        //mText.enabled = false;

        imageDrop.rectTransform.localPosition = imageStart.rectTransform.localPosition;
        hand.gameObject.SetActive(false);
     
        mCtrl.StartCoroutine(IENextGuide1());
    }
    public bool bGuide = false;
    private System.Action guideFinishCallback = null;
    bool bGameFirst = true;
    IEnumerator IENextGuide1()
    {
        if (bGameFirst)
        {
            bGameFirst = false;
        }
        yield return new WaitForSeconds(0.1f);
        AudioClip gamename1 = mCtrl.GetClip("game-tips2-4-3");
        mCtrl.PlaySound(gamename1, 1f);
        yield return new WaitForSeconds(gamename1.length + 0.1f);

        //一点出现 移动中间
        SingleDualNum_yellowball ball1 = CreateYellowBall(yellowballBase, balls.transform, new Vector3(1000f, 0f, 0f));
        mYellowBallList.Add(ball1);
        ball1.transform.DOLocalMove(Vector3.zero, 1f);
        mCtrl.PlayTheSortSound("yellowpointout");
        yield return new WaitForSeconds(1.1f);
        //小鱼出现
        SingleDualNum_yellowball ball2 = CreateYellowBall(yellowballBase, balls.transform, new Vector3(-150f, 0f, 0f));
        mYellowBallList.Add(ball2);
        ball2.rectTransform.localScale = Vector3.one * 0.001f;
        ball2.transform.DOScale(Vector3.one, 0.3f);
        mCtrl.PlayTheSortSound("yellowpointout");
        yield return new WaitForSeconds(0.3f);
        SingleDualNum_yellowball ball3 = CreateYellowBall(yellowballBase, balls.transform, new Vector3(150f, 0f, 0f));     
        mYellowBallList.Add(ball3);       
        ball3.rectTransform.localScale = Vector3.one * 0.001f;      
        ball3.transform.DOScale(Vector3.one, 0.3f);
        mCtrl.PlayTheSortSound("yellowpointout");
        yield return new WaitForSeconds(0.4f);

        //左右白点出现
        DOTween.To(() => imageStart.color, x => imageStart.color = x, new Color(1f, 1f, 1f, 1f), 0.25f);
        DOTween.To(() => imageEnd.color, x => imageEnd.color = x, new Color(1f, 1f, 1f, 1f), 0.25f);
        yield return new WaitForSeconds(0.25f);
        //虚线出现
        DOTween.To(() => xuxian.color, x => xuxian.color = x, new Color(1f, 1f, 1f, 1f), 0.25f);
        yield return new WaitForSeconds(0.25f);

        lineSp.color = new Color(1f, 1f, 1f, 1f);    
        imageDrop.color = new Color(1f, 1f, 1f, 1f);
        
        //手出现
        hand.gameObject.SetActive(true);
        hand.localPosition = imageStart.rectTransform.localPosition;
        hand.localScale = Vector3.one * 0.001f;
        hand.DOScale(Vector3.one, 0.3f);
        yield return new WaitForSeconds(0.4f);
        //手移动
        imageDrop.rectTransform.DOLocalMove(imageEnd.rectTransform.localPosition, 2f).SetEase(Ease.Linear);
        mCtrl.PlayTheSortSound("dropline");
        yield return new WaitForSeconds(2.1f);
        mCtrl.PlayTheSortSound("linesuc");
        mCtrl.MovePraSys(imageStart.transform.position, imageEnd.transform.position, 1f);
        //手变小消失
        hand.DOScale(Vector3.one * 0.001f, 0.3f).OnComplete(()=> 
        {
            hand.gameObject.SetActive(false);     
        });
        yield return new WaitForSeconds(2f);
        bGuide = false;
        if (guideFinishCallback != null)
            guideFinishCallback();

        Debug.Log("guide finish");
    }


    /// <summary>
    /// 创建一个image
    /// </summary>
    public Image CreateImage(Image _baseImage, Transform _parent,Vector3 _localPos)
    {
        Image ball1 = GameObject.Instantiate(_baseImage) as Image;
        ball1.gameObject.SetActive(true);
        ball1.rectTransform.SetParent(_parent);
        ball1.rectTransform.localScale = Vector3.one;
        ball1.rectTransform.localPosition = _localPos;
        return ball1;
    }
    /// <summary>
    /// 创建一个yellow ball
    /// </summary>
    public SingleDualNum_yellowball CreateYellowBall(Image _baseImage, Transform _parent, Vector3 _localPos)
    {
        Image img = CreateImage(_baseImage, _parent, _localPos);
        SingleDualNum_yellowball yeBall = img.gameObject.AddComponent<SingleDualNum_yellowball>();
        yeBall.InitAwake();
        return yeBall;
    }

    /// <summary>
    /// yellow ball 位置
    /// </summary>
    public List<Vector3> GetPoss(int _number)
    {
        List<Vector3> vposlist = new List<Vector3>();

        int col = _number / 2;
        int col1 = _number % 2;
        int cols = col + col1;

        List<float> fGetXList = GetOrderList(cols, 120f);

        for (int i = 0; i < col; i++)
        {
            Vector3 getUP = new Vector3(fGetXList[i], 70f);
            Vector3 getDown = new Vector3(fGetXList[i], -70f);
            vposlist.Add(getUP);
            vposlist.Add(getDown);
        }
        if (col1 > 0)
        {
            Vector3 getp = new Vector3(fGetXList[fGetXList.Count - 1], 0f);
            vposlist.Add(getp);
        }
        return vposlist;
    }
    /// <summary>
    /// 分段
    /// </summary>
    public List<float> GetOrderList(float count, float width)
    {
        List<float> list = new List<float>();
        if (count <= 1f)
        {
            list.Add(0);
            return list;
        }
        float start = width * ((count + 1) / 2f);
        for (float i = 1; i < count + 1; i++)
        {
            float x = width * i - start;

            list.Add(x);
        }
        return list;
    }



    public void SceneMove(bool _in,float _time)
    {
        if (_in)
        {
            gameObject.SetActive(true);
            if (nFinishCount >= 10)
            {
                for (int i = 0; i < beikes.Length; i++)
                {
                    beikes[i].bFinish = false;
                }
                nFinishCount = 0;
                SetNumber(1);
            }
            transform.localPosition = new Vector3(0f, 900f, 0f);
            transform.DOLocalMove(Vector3.zero, _time).OnComplete(()=> 
            {
                //如果引导没完成,先完成引导
                PlayGuideAction();
            });
        }
        else
        {
            transform.DOLocalMove(new Vector3(0f, 900f, 0f), _time).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }       
    }


    //如果引导没完成,先完成引导
    private void PlayGuideAction()
    {
        if (!bGuideFinish)
        {
            PlayGuide(() =>
            {
                bGuideFinish = true;
                SetNumber(1);
                mCtrl.BtnsMove(true, 0.5f);
                mCtrl.bButtonActive = true;
            });
        }
        else
        {
            mCtrl.BtnsMove(true, 0.5f);
        }
    }

    /// <summary>
    /// 自动跳转 顺获取一个没完成的
    /// </summary>
    /// <returns></returns>
    public SingleDualNum_beike GetNextBeike()
    {
        for (int i=0;i<beikes.Length;i++)
        {
            if (beikes[i].bFinish == false)
            {
                return beikes[i];
            }
        }
        return null;
    }

}
