using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 信件
/// </summary>
public class LittlePostMsgObj : MonoBehaviour
{
    public int nAnimalID = 1;
    /// <summary>
    /// 0在手上  1已经click  2在邮箱里
    /// </summary>
    public int nState = 0;
    private Image imgMsg;
    private Image imgpoint;
    private Image imgAnimalhead;
    private BoxCollider2D mbox2D;

    private float fscale = 0.75f;

    private GameObject xinTrigger;

    private LittlePostmanCtrl mCtrl;

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as LittlePostmanCtrl;

        imgMsg = UguiMaker.newImage("imgMsg", transform, "littlepostman_sprite", "xin0", false);
        imgpoint = UguiMaker.newImage("imgpoint", transform, "littlepostman_sprite", "xinpoint", false);
        imgpoint.rectTransform.sizeDelta = Vector2.one * 75f;
        imgAnimalhead = UguiMaker.newImage("animalhead", imgpoint.transform, "aa_animal_head", "1", false);
        imgAnimalhead.rectTransform.sizeDelta = Vector2.one * 65f;

        imgMsg.rectTransform.sizeDelta = imgMsg.rectTransform.sizeDelta * fscale;

        mbox2D = gameObject.AddComponent<BoxCollider2D>();
        mbox2D.size = imgMsg.rectTransform.sizeDelta;
        mbox2D.isTrigger = true;

        xinTrigger = UguiMaker.newGameObject("xinTrigger", transform);
        BoxCollider2D tribox2D = xinTrigger.AddComponent<BoxCollider2D>();
        tribox2D.size = new Vector2(100f, 10f);
        Rigidbody2D rig2D = xinTrigger.AddComponent<Rigidbody2D>();
        rig2D.isKinematic = true;
    }

    public void ResetInfos()
    {
        nAnimalID = 0;
        nState = 0;
        transform.localScale = Vector3.one;
        imgMsg.sprite = ResManager.GetSprite("littlepostman_sprite", "xin0");
        imgpoint.transform.localEulerAngles = Vector3.zero;
        imgpoint.transform.localPosition = Vector3.zero;
        imgpoint.transform.localScale = Vector3.one;
        Box2DActive(true);
    }

    /// <summary>
    /// 设置信件动物ID
    /// </summary>
    /// <param name="_id"></param>
    public void SetAnimalID(int _id)
    {
        nAnimalID = _id;
        imgAnimalhead.sprite = ResManager.GetSprite("aa_animal_head", nAnimalID.ToString());
    }

    /// <summary>
    /// 设置信件视觉状态
    /// </summary>
    /// <param name="_sendState"></param>
    public void SetXin(bool _sendState)
    {
        if (_sendState)
        {
            nState = 2;
            imgMsg.sprite = ResManager.GetSprite("littlepostman_sprite", "xin1");
            imgpoint.transform.localEulerAngles = new Vector3(40f, 0f, 0f);
            imgpoint.transform.localPosition = new Vector3(0f, -10f, 0f);
            imgpoint.transform.localScale = Vector3.one * 0.83f;
        }
        else
        {
            nState = 1;
            imgMsg.sprite = ResManager.GetSprite("littlepostman_sprite", "xin0");
            imgpoint.transform.localEulerAngles = Vector3.zero;
            imgpoint.transform.localPosition = Vector3.zero;
            imgpoint.transform.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// hand 丢失信件
    /// </summary>
    public void HandLostMsgReset()
    {
        mCtrl.SoundCtrl.PlaySortSound("littlepostman_sound", "取走信封");
        transform.DOLocalRotate(Vector3.zero, 0.2f);
    }

    public void Box2DActive(bool _active)
    {
        mbox2D.enabled = _active;
    }

    /// <summary>
    /// 信件box大小
    /// </summary>
    public Vector2 GetSize()
    {
        return mbox2D.size;
    }




  

}


