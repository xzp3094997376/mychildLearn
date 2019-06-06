using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class KOAT_RectBox : MonoBehaviour
{
    private miniImageNumber imgLeftNum;//十位数字
    private miniImageNumber imgRightNum;//个位数字
    private Image imgLeftBG;//十位数字bg
    private Image imgRightBG;//个位数字bg

    private Transform leftpos;
    private Transform rightpos;

    Vector3 vStart = new Vector3(0f, -340f, 0f);

    public void InitAwake()
    {

        imgLeftBG = transform.Find("mLeftbg").gameObject.GetComponent<Image>();
        imgRightBG = transform.Find("mRightbg").gameObject.GetComponent<Image>();
        leftpos = transform.Find("leftpos");
        rightpos = transform.Find("rightpos");

        imgLeftNum = UguiMaker.newGameObject("leftNum", transform).AddComponent<miniImageNumber>();
        imgLeftNum.strABName = "knowoneandten_sprite";
        imgLeftNum.transform.localPosition = leftpos.localPosition;
        imgLeftNum.transform.localScale = Vector3.one * 0.25f;

        imgRightNum = UguiMaker.newGameObject("rightNum", transform).AddComponent<miniImageNumber>();
        imgRightNum.strABName = "knowoneandten_sprite";
        imgRightNum.transform.localPosition = rightpos.localPosition;
        imgRightNum.transform.localScale = Vector3.one * 0.25f;

        SetOutSidePos();
    }

    public void SetLeftNum(int _num)
    {
        imgLeftNum.SetNumber(_num);
    }
    public void SetRightNum(int _num)
    {
        imgRightNum.SetNumber(_num);
    }

    public void SetOutSidePos()
    {
        transform.localPosition = vStart + new Vector3(0f, -400f, 0f);
    }

    public void SceneMove(bool _in, float _time)
    {
        if (_in)
        { transform.DOLocalMove(vStart, _time); }
        else
        {
            transform.DOLocalMove(vStart + new Vector3(0f, -400f, 0f), _time);
        }
    }


    /// <summary>
    /// 个位数字闪动
    /// </summary>
    /// <param name="_play"></param>
    public void RightNumFlash(bool _play)
    {
        if (_play)
        {
            RightNumFlash(false);
            iePlayRightFlash = ieRightNumFlash();
            StartCoroutine(iePlayRightFlash);
        }
        else
        {
            if (iePlayRightFlash != null)
            { StopCoroutine(iePlayRightFlash); }
            imgRightNum.gameObject.SetActive(true);
        }
    }
    IEnumerator iePlayRightFlash = null;
    IEnumerator ieRightNumFlash()
    {
        imgRightNum.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        imgRightNum.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        RightNumFlash(true);
    }

    /// <summary>
    /// 数字背景闪动
    /// </summary>
    /// <param name="_play"></param>
    public void NumBGFlash(bool _play)
    {
        if (_play)
        {
            NumBGFlash(false);
            iePlayNumBGFlash = ieNumBGFlash();
            StartCoroutine(iePlayNumBGFlash);
        }
        else
        {
            if (iePlayNumBGFlash != null)
            { StopCoroutine(iePlayNumBGFlash); }
            imgLeftBG.gameObject.SetActive(true);
            imgRightBG.gameObject.SetActive(true);
        }
    }
    IEnumerator iePlayNumBGFlash = null;
    IEnumerator ieNumBGFlash()
    {
        imgLeftBG.gameObject.SetActive(true);
        imgRightBG.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        imgLeftBG.gameObject.SetActive(false);
        imgRightBG.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        NumBGFlash(true);
    }

}


public class KOAT_SuanShi : MonoBehaviour
{

    private miniImageNumber mNum0;
    private miniImageNumber mNum1;
    private miniImageNumber mNum2;

    private Image fuhao0;
    private Image fuhao1;

    private Image imgInput;
    private Button btnInput;

    Vector3 vStart = new Vector3(0f, -340f, 0f);

    public void InitAwake()
    {
        fuhao0 = UguiMaker.newImage("fuhao0", transform, "knowoneandten_sprite", "fuhao0", false);
        fuhao1 = UguiMaker.newImage("fuhao1", transform, "knowoneandten_sprite", "fuhao1", false);
        fuhao0.transform.localPosition = new Vector3(-95f, 0f, 0f);
        fuhao1.transform.localPosition = new Vector3(95f, 0f, 0f);
        fuhao0.transform.localScale = Vector3.one * 1.7f;
        fuhao1.transform.localScale = Vector3.one * 1.7f;

        imgInput = UguiMaker.newImage("imgInput", transform, "knowoneandten_sprite", "mblock1");
        imgInput.rectTransform.sizeDelta = Vector3.one * 95f;

        mNum0 = UguiMaker.newGameObject("mNum0", transform).AddComponent<miniImageNumber>();
        mNum0.strABName = "knowoneandten_sprite";
        mNum0.transform.localPosition = new Vector3(-180f, 0f, 0f);
        mNum0.transform.localScale = Vector3.one * 0.4f;

        mNum1 = UguiMaker.newGameObject("mNum1", transform).AddComponent<miniImageNumber>();
        mNum1.strABName = "knowoneandten_sprite";
        mNum1.transform.localPosition = new Vector3(0f, 0f, 0f);
        mNum1.transform.localScale = Vector3.one * 0.4f;

        Image imgBtn = UguiMaker.newGameObject("imgBtn", transform).AddComponent<Image>();
        imgBtn.color = new Color(1f, 1f, 1f, 0f);
        imgBtn.rectTransform.sizeDelta = imgInput.rectTransform.sizeDelta;
        btnInput = imgBtn.gameObject.AddComponent<Button>();
        btnInput.transition = Selectable.Transition.None;
        btnInput.onClick.AddListener(BtnInputClick);

        mNum2 = UguiMaker.newGameObject("mNum2", transform).AddComponent<miniImageNumber>();
        mNum2.strABName = "knowoneandten_sprite";
        mNum2.transform.localPosition = new Vector3(180f, 0f, 0f);
        mNum2.transform.localScale = Vector3.one * 0.4f;

        SetOutSidePos();
    }


    public void SetNum0(int _num)
    {
        mNum0.SetNumber(_num);
    }
    public void SetNum1(int _num)
    {
        mNum1.SetNumber(_num);
    }
    public void SetNum2(int _num)
    {
        mNum2.SetNumber(_num);
    }


    public void SetOutSidePos()
    {
        transform.localPosition = vStart + new Vector3(0f, -400f, 0f);
    }

    public void SceneMove(bool _in, float _time)
    {
        if (_in)
        { transform.DOLocalMove(vStart, _time); }
        else
        {
            transform.DOLocalMove(vStart + new Vector3(0f, -400f, 0f), _time);
        }
    }

    /// <summary>
    /// get input world pos
    /// </summary>
    /// <returns></returns>
    public Vector3 GetInputPos()
    {
        return imgInput.transform.position;
    }


    public void SetInputClickCall(System.Action _call)
    {
        mInputCall = _call;
    }
    System.Action mInputCall;
    private void BtnInputClick()
    {
        if (mInputCall != null)
        {
            mInputCall();
        }
    }



    public void ShakeNumObj()
    {
        StartCoroutine(TScale());
    }
    IEnumerator TScale()
    {
        //if (mCtrl == null)
        //{ mCtrl = SceneMgr.Instance.GetNowScene() as LineUpCtrl; }
        //mCtrl.mSoundCtrl.PlaySortSound("lineupctrl_sound", "inputnum_error");
        for (float j = 0; j < 1f; j += 0.05f)
        {
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * j) * 10);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localEulerAngles = Vector3.zero;
        mNum1.DesNum();
    }



}
