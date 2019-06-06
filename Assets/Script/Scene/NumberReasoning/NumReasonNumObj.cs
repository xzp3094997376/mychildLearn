using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 数字Obj
/// </summary>
public class NumReasonNumObj : MonoBehaviour
{
    public int nID = 0;
    public int nNumber = 0;
    public int nCheckNum = 0;

    private Image imgWenhao;
    private BoxCollider2D mbox2D;
    public miniImageNumber GetMiniNumber { get { return miniNumber; } }
    private miniImageNumber miniNumber;

    public void InitAwake(int _id, int _num)
    {
        nID = _id;
        nNumber = _num;
        imgWenhao = UguiMaker.newImage("imgnum", transform, "numberreasoning_sprite", "wenhao", false);
        WenHaoActive(false);

        miniNumber = UguiMaker.newGameObject("miniNumber", transform).AddComponent<miniImageNumber>();
        miniNumber.strABName = "inputnumres_sprite";
        miniNumber.transform.localScale = Vector3.one * 0.3f;
        miniNumber.SetNumColor(new Color(105f / 255, 13f / 255, 2f / 255, 1f));
        miniNumber.SetNumber(nNumber);
        

        if (_id == 1)
        {
            imgWenhao.transform.localPosition = new Vector3(8f, -5f, 0f);
            miniNumber.transform.localPosition = new Vector3(8f, -5f, 0f);
        }
        if (_id == 2)
        {
            imgWenhao.transform.localPosition = new Vector3(-8f, -5f, 0f);
            miniNumber.transform.localPosition = new Vector3(-8f, -5f, 0f);
        }
        mbox2D = gameObject.AddComponent<BoxCollider2D>();
        RectTransform rts = transform as RectTransform;
        mbox2D.size = rts.sizeDelta;

        Box2DActive(false);     
    }

    /// <summary>
    /// 碰撞器显示/隐藏
    /// </summary>
    /// <param name="_active"></param>
    public void Box2DActive(bool _active)
    {
        mbox2D.enabled = _active;
    }
    /// <summary>
    /// 问号显示/隐藏
    /// </summary>
    /// <param name="_active"></param>
    public void WenHaoActive(bool _active)
    {
        imgWenhao.gameObject.SetActive(_active);
        if (_active)
        {
            PlayTip();
        }
    }
    /// <summary>
    /// mini数字显示/隐藏
    /// </summary>
    /// <param name="_active"></param>
    public void MiniNumberActive(bool _active)
    {
        miniNumber.gameObject.SetActive(_active);
    }

    public void SetBoxSize(Vector2 _size)
    {
        mbox2D.size = _size;
    }

    /// <summary>
    /// 设置数字
    /// </summary>
    /// <param name="_id"></param>
    public void SetNumber(int _id)
    {
        miniNumber.SetNumber(_id);
        nCheckNum = _id;
    }

    /// <summary>
    /// 检测是否匹配
    /// </summary>
    /// <returns></returns>
    public bool CheckIsOK()
    {
        if (nNumber == nCheckNum)
        {
            Box2DActive(false);
            return true;
        }
        else
        {
            ShakeObj();
            return false;
        }
    }



    public void ShakeObj()
    {
        StartCoroutine(TScale());
    }
    IEnumerator TScale()
    {
        //if (mCtrl == null)
        //{ mCtrl = SceneMgr.Instance.GetNowScene() as LineUpCtrl; }
        //mCtrl.mSoundCtrl.PlaySortSound("lineupctrl_sound", "inputnum_error");

        StopTip();

        for (float j = 0; j < 1f; j += 0.05f)
        {
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * j) * 10);
            yield return new WaitForSeconds(0.01f);
        }
        WenHaoActive(true);
        transform.localEulerAngles = Vector3.zero;

        PlayTip();
    }


    public void PlayTip()
    {
        if (tiptween == null)
            tiptween = imgWenhao.transform.DOScale(Vector3.one * 1.2f, 0.6f).SetLoops(-1, LoopType.Yoyo);
        else
            tiptween.Play();
    }
    public void StopTip()
    {
        if (tiptween != null)
            tiptween.Pause();
        imgWenhao.transform.localScale = Vector3.one;
    }
    private Tween tiptween = null;

}
