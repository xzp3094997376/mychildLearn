using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class AnimalsHomeClock : MonoBehaviour
{

    private Text mtext;
    private SkeletonGraphic spine;

    private int nTime = 20;
    private int nCount = 0;

    private AudioClip cp0;
    private AnimalsHomeCtrl mCtrl;
    private System.Action mRunTimeCallback = null;

    private Vector3 vStart = new Vector3(480f, -30f, 0f);

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as AnimalsHomeCtrl;
        transform.localPosition = vStart;

        spine = ResManager.GetPrefab("animalshome_prefab", "mclock").GetComponent<SkeletonGraphic>();
        spine.transform.SetParent(transform);
        spine.transform.localScale = Vector3.one;
        spine.transform.localPosition = new Vector3(4f, -77f, 0f);

        mtext = UguiMaker.newText("text", transform);
        mtext.raycastTarget = false;
        mtext.rectTransform.sizeDelta = new Vector2(90, 90);
        mtext.alignment = TextAnchor.MiddleCenter;
        mtext.text = "20";
        mtext.fontSize = 60;
        mtext.color = Color.red;
        mtext.font = Font.CreateDynamicFontFromOSFont("FZSEJW", 60);

        cp0 = Resources.Load<AudioClip>("sound/倒计时");
    }


    public void SetTime()
    {
        nTime = 60;
        mtext.text = nTime.ToString();
    }

    public void StopTimeRun()
    {
        if (ieTheTimeRun != null)
            mCtrl.StopCoroutine(ieTheTimeRun);
    }

    public void StartTimeRun(System.Action _callback = null)
    {
        mRunTimeCallback = _callback;

        nCount = nTime;
        mtext.text = nCount.ToString();

        if (ieTheTimeRun != null)
            mCtrl.StopCoroutine(ieTheTimeRun);

        ieTheTimeRun = ieTimeRun();
        mCtrl.StartCoroutine(ieTheTimeRun);
    }
    IEnumerator ieTheTimeRun = null;
    IEnumerator ieTimeRun()
    {
        for (int i = 0; i < nTime; i++)
        {
            yield return new WaitForSeconds(1f);
            nCount--;
            mtext.text = nCount.ToString();
            if (nCount <= 5)
            {
                mCtrl.MSoundCtrl.PlaySortSound(cp0);
                spine.AnimationState.SetAnimation(2, "Click", false);
            }
            if (nCount <=0)
            {
                //Debug.Log("time run over");
                if (mRunTimeCallback != null)
                {
                    mRunTimeCallback();
                }
            }
        }
    }

    /// <summary>
    /// 视觉切换时设置位置大小
    /// </summary>
    /// <param name="_vtarget"></param>
    public void SetToPosScale(Vector3 _vtarget)
    {
        if (gameObject.activeSelf)
        {
            Vector3 vclockpos = _vtarget + new Vector3(93f, 0f, 0f);
            transform.DOScale(Vector3.one * 0.23f, 0.2f);
            transform.DOLocalMove(vclockpos, 0.2f);
        }
    }
    /// <summary>
    /// 视觉还原
    /// </summary>
    public void ResetToBack()
    {
        if (gameObject.activeSelf)
        {
            transform.DOScale(Vector3.one, 0.2f);
            transform.DOLocalMove(vStart, 0.2f);
        }
    }

}
