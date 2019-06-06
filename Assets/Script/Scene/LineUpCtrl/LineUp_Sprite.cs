using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class LineUp_Sprite : MonoBehaviour
{

    public string strRes = "";

    private Image imgbg;
    private Image imgWenhao;
    private BoxCollider2D mbox2D;

    public void InitAwake(string _strRes, bool _isQuetion)
    {
        imgbg = gameObject.AddComponent<Image>();
        imgbg.raycastTarget = false;

        imgbg.sprite = ResManager.GetSprite("lineupctrl_sprite", _strRes);
        imgbg.SetNativeSize();

        if (_isQuetion)
        {
            imgWenhao = UguiMaker.newImage("wenhao", transform, "lineupctrl_sprite", "wenhao", false);

            mbox2D = gameObject.AddComponent<BoxCollider2D>();
            mbox2D.size = imgbg.rectTransform.sizeDelta;
        }
    }


    public void SetNumber(int _id)
    {
        if (imgWenhao != null)
        {
            imgWenhao.color = new Color(40f / 255, 131f / 255, 16f / 255, 1f);
            imgWenhao.sprite = ResManager.GetSprite("number_slim", _id.ToString());
            imgWenhao.SetNativeSize();
            imgWenhao.rectTransform.sizeDelta = imgWenhao.rectTransform.sizeDelta * 0.85f;
        }
    }


    public void ResetInfo()
    {
        BoxActive(false);
        WenHaoActive(true);
    }

    public void WenHaoActive(bool _active)
    {
        if (imgWenhao != null)
        {
            imgWenhao.enabled = _active;
        }
    }
    public void BoxActive(bool _active)
    {
        if (mbox2D != null)
        {
            mbox2D.enabled = _active;
        }
    }

    public void ShakeObj()
    {
        StartCoroutine(TScale());
    }
    IEnumerator TScale()
    {
        if (mCtrl == null)
        { mCtrl = SceneMgr.Instance.GetNowScene() as LineUpCtrl; }
        mCtrl.mSoundCtrl.PlaySortSound("lineupctrl_sound", "inputnum_error");

        StopTip();

        for (float j = 0; j < 1f; j += 0.05f)
        {
            //float p = Mathf.Sin(Mathf.PI * j) * 0.8f;
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * j) * 10);
            yield return new WaitForSeconds(0.01f);
        }
        imgWenhao.sprite = ResManager.GetSprite("lineupctrl_sprite", "wenhao");
        imgWenhao.SetNativeSize();
        imgWenhao.color = new Color(1f, 1f, 1f, 1f);
        transform.localEulerAngles = Vector3.zero;

        PlayTip();
    }

    LineUpCtrl mCtrl;



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
