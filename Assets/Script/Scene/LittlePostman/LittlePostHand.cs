using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 手
/// </summary>
public class LittlePostHand : MonoBehaviour
{
    private Image hand0;
    private Image hand1;

    private Vector3 vShow = new Vector3(650f, -305f, 0f);
    private Vector3 vHide = new Vector3(900f, -305f, 0f);

    public LittlePostMsgObj postmsg;
    private LittlePostmanCtrl mCtrl;

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as LittlePostmanCtrl;

        hand0 = UguiMaker.newImage("hand0", transform, "littlepostman_sprite", "hand0", false);
        hand1 = UguiMaker.newImage("hand1", transform, "littlepostman_sprite", "hand1", false);

        hand0.rectTransform.anchoredPosition = new Vector2(-150f, 59f);
        hand1.rectTransform.anchoredPosition = new Vector2(-168f, 118f);

        transform.localScale = Vector3.one * 0.65f;
        transform.localPosition = vHide;
    }




    /// <summary>
    /// 拿到信件
    /// </summary>
    /// <param name="_postmsg"></param>
    public void GetPostMsg(LittlePostMsgObj _postmsg)
    {
        postmsg = _postmsg;
        postmsg.transform.SetParent(transform);
        postmsg.transform.localPosition = new Vector3(-247f, 155f, 0f);
        postmsg.transform.localEulerAngles = new Vector3(0f, 0f, 40f);
        postmsg.transform.SetSiblingIndex(1);
        postmsg.nState = 0;
        postmsg.Box2DActive(true);
    }

    /// <summary>
    /// 丢失信件
    /// </summary>
    public void LostPostMsg()
    {
        //postmsg.Box2DActive(false);
        postmsg.nState = 1;
        postmsg.transform.SetParent(mCtrl.transform);
        postmsg.HandLostMsgReset();
        postmsg = null;

        bshake = false;
        StopCoroutine(ieDoShake());
        transform.localEulerAngles = Vector3.zero;
        transform.DOLocalMove(vHide, 0.3f);
    }

    /// <summary>
    /// 显示手
    /// </summary>
    public void ShowHand()
    {
        mCtrl.SoundCtrl.PlaySortSound("littlepostman_sound", "伸手");
        transform.DOLocalMove(vShow, 0.3f).OnComplete(() =>
        {
            bshake = true;
            DoShake();
        });
    }

    /// <summary>
    /// shake hand
    /// </summary>
    public void DoShake()
    {
        StartCoroutine(ieDoShake());
    }
    bool bshake = false;
    IEnumerator ieDoShake()
    {
        yield return new WaitForSeconds(0.5f);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Sin(Mathf.PI * 4 * i) * 5f);
            yield return new WaitForSeconds(0.02f);
        }
        transform.localEulerAngles = Vector3.zero;
        yield return new WaitForSeconds(2.5f);
        if (bshake)
            DoShake();
    }
}
