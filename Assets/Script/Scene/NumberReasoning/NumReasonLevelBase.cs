using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NumReasonLevelBase : MonoBehaviour
{
    public int nToCount = 0;
    public int nCount = 0;

    protected NumberReasoningCtrl mCtrl;
    protected RawImage imgBG;
    protected bool bInit = false;

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as NumberReasoningCtrl;
        bInit = true;
    }
    /// <summary>
    /// 重置信息
    /// </summary>
    public virtual void ResetInfos()
    {
        nToCount = 0;
        nCount = 0;
        if (imgBG != null)
        {
            if (imgBG.gameObject != null)
                GameObject.Destroy(imgBG.gameObject);
            imgBG = null;
        }
    }
    /// <summary>
    /// 设置信息
    /// </summary>
    public virtual void SetData() { }
    /// <summary>
    /// 输入数字
    /// </summary>
    public virtual void InputNumbCallback() { }
    /// <summary>
    /// 确认输入
    /// </summary>
    public virtual void InputNumFinishCallback() { }
    /// <summary>
    /// 清除数字
    /// </summary>
    public virtual void InputClearCallback() { }
    /// <summary>
    /// 移出界面并清除信息
    /// </summary>
    public virtual void MoveOutAndReset() { }
    /// <summary>
    /// 玩法提示语音
    /// </summary>
    public virtual void PlayTipSound() { }

}
