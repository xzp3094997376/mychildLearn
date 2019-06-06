using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class AnimalStatistics_BallCtrl : MonoBehaviour
{

    public int nID = 0;

    private RectTransform tanghuangline;
    private Image linesp;
    private Image tanghuangtop;

    private Image ball;
    public Image BallImage { get { return ball; } }
    private Image num1;
    private Image num2;

    //开始点
    private Vector3 vStart;
    //是否丢失
    public bool bBallLose = false;
    private bool bInit = false;

    private AnimalStatisticsCtrl mCtrl;


    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as AnimalStatisticsCtrl;

        tanghuangline = transform.Find("tanghuangline") as RectTransform;

        linesp = tanghuangline.Find("linesp").GetComponent<Image>();
        linesp.sprite = ResManager.GetSprite("animalstatistics_sprite", "tanghuangline1");
        linesp.type = Image.Type.Simple;
        linesp.rectTransform.sizeDelta = new Vector2(linesp.rectTransform.sizeDelta.x, 64f);
        linesp.rectTransform.anchoredPosition = new Vector2(0f, -32f);

        tanghuangtop = transform.Find("tanghuangtop").GetComponent<Image>();
        tanghuangtop.sprite = ResManager.GetSprite("animalstatistics_sprite", "tanghuangtop");

        ball = transform.Find("ball").GetComponent<Image>();
        num1 = ball.transform.Find("num1").GetComponent<Image>();
        num2 = ball.transform.Find("num2").GetComponent<Image>();
        ball.rectTransform.localScale = Vector3.one * 1.3f;

        num1.color = new Color(1f, 1f, 1f, 1f);
        num2.color = new Color(1f, 1f, 1f, 1f);

        vStart = new Vector3(0f, -120f, 0f);
        ball.rectTransform.anchoredPosition3D = vStart;

        bInit = true;
    }

    public void SetNumber(int _num)
    {
        nID = _num;
        num1.enabled = false;
        num2.enabled = false;
        if (_num < 10)
        {
            num1.enabled = true;
            num1.rectTransform.anchoredPosition = Vector2.zero;
            int nA = _num;
            num1.sprite = ResManager.GetSprite("animalstatistics_sprite", nA.ToString());
        }
        else
        {
            num1.enabled = true;
            num2.enabled = true;
            num1.rectTransform.anchoredPosition = new Vector2(-15f, 0f);
            num2.rectTransform.anchoredPosition = new Vector2(15f, 0f);
            int nA = _num / 10;
            int nB = _num % 10;
            num1.sprite = ResManager.GetSprite("animalstatistics_sprite", nA.ToString());
            num2.sprite = ResManager.GetSprite("animalstatistics_sprite", nB.ToString());
        }

        num1.SetNativeSize();
        num2.SetNativeSize();

        num1.transform.localScale = Vector3.one * 0.55f;
        num2.transform.localScale = Vector3.one * 0.55f;
    }

    public void SetBallBG(int _type)
    {
        ball.sprite = ResManager.GetSprite("animalstatistics_sprite", "num" + _type);
    }

    Vector3 temp_select_offset = Vector3.zero;
    Image mSelect = null;
    void Update()
    {
        if (!bInit)
            return;

        if (!mCtrl.bGameReady)
            return;

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    SetNumber(nID);
        //}

        if (!bBallLose)
        {
            if (Input.GetMouseButtonDown(0))
            {
                #region//one
                RaycastHit2D hits = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hits)
                {
                    if (hits.collider.gameObject == ball.gameObject)
                    {
                        mSelect = ball;
                        temp_select_offset = Common.getMouseLocalPos(transform) - ball.rectTransform.anchoredPosition3D;
                        transform.SetSiblingIndex(10);
                        mCtrl.PlaySortSound("sound_lineup");
                        mCtrl.GuideHandStopLv1();
                    }
                }
                #endregion
            }
            else if (Input.GetMouseButton(0))
            {
                if (mSelect != null)
                {
                    Vector3 vInput = Common.getMouseLocalPos(transform) - temp_select_offset;
                    mSelect.rectTransform.anchoredPosition3D = vInput;

                    BallCtrol();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                #region//two
                if (mSelect != null)
                {
                    bool bCheckok = false;
                    #region//射线检测
                    RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hits != null)
                    {
                        AnimalStatistics_TipNum theTipNum = null;
                        for (int i = 0; i < hits.Length; i++)
                        {
                            theTipNum = hits[i].collider.gameObject.GetComponent<AnimalStatistics_TipNum>();
                            if (theTipNum != null && theTipNum.nNum == nID)
                            {
                                bCheckok = true;
                                SetHitTipNum(theTipNum);
                                break;
                            }
                        }
                    }
                    #endregion

                    SetBallBack(bCheckok);

                    mSelect = null;
                }
                #endregion
            }
        }

        //BallCtrol();
    }

    private void BallCtrol()
    {
        //长度
        float dis = Vector3.Distance(ball.rectTransform.anchoredPosition3D, tanghuangline.anchoredPosition3D);
        linesp.rectTransform.sizeDelta = new Vector2(linesp.rectTransform.sizeDelta.x, dis);
        linesp.rectTransform.anchoredPosition3D = (tanghuangline.anchoredPosition3D + ball.rectTransform.anchoredPosition3D) * 0.5f;

        //旋转方向
        Vector3 dir = (ball.rectTransform.anchoredPosition3D - tanghuangline.anchoredPosition3D).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(new Vector3(0f, 0f, targetAngle + 90f));
        linesp.rectTransform.localRotation = q;
    }

    private void SetBallBack(bool _checkOK, System.Action _action = null)
    {
        if (_checkOK)
            mCtrl.PlaySortSound("sound_balldropok");
        else
            mCtrl.PlaySortSound("sound_lineback");
        ball.transform.DOLocalMove(vStart, 0.5f).SetEase(Ease.OutBack).OnUpdate(BallCtrol).OnComplete(()=> 
        {
            if (_action != null)
            { _action(); }
        });      
    }

    private void SetHitTipNum(AnimalStatistics_TipNum _tipnum)
    {
        ball.enabled = false;
        num1.enabled = false;
        num2.enabled = false;
        linesp.sprite = ResManager.GetSprite("animalstatistics_sprite", "tanghuangline");
        bBallLose = true;
        _tipnum.ShowNumber();
        _tipnum.SetBGSprite(ball.sprite);
        _tipnum.transform.DOScale(Vector3.one * 1.6f, 0.2f).OnComplete(() => 
        {
            _tipnum.transform.DOScale(Vector3.one * 1.3f, 0.2f).SetEase(Ease.InBack);
        });

        AnimalStatistics_DataObj dataobj = _tipnum.transform.parent.GetComponent<AnimalStatistics_DataObj>();
        if (dataobj != null)
        {
            //动物头像变大
            dataobj.SetAnimalHeadBigEffect();
        }

        mCtrl.MLevelPass();
    }

    public void ResetInfos()
    {
        bBallLose = false;
        ball.enabled = true;
        linesp.sprite = ResManager.GetSprite("animalstatistics_sprite", "tanghuangline1");
    }

}
