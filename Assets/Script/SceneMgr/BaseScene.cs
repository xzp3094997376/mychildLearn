using UnityEngine;
using System.Collections;

public class BaseScene : MonoBehaviour
{
    private SceneEnum _sceneType = SceneEnum.None;
    public SceneEnum mSceneType
    {
        set { _sceneType = value; }
        get { return _sceneType; }
    }

    public virtual void CorrectSet()
    { }
    public virtual void ErrorSet()
    { }

    public virtual void SceneSucceed()
    { }
    public virtual void SceneFail()
    { }

    /// <summary>
    /// 加载场景完成call
    /// </summary>
    public void CallLoadFinishEvent()
    {
        //友盟统计关卡打开次数
        Debug.Log("友盟统计关卡打开次数：" + ApkInfo.g_begin_scene.ToString());
        Umeng.GA.Event(ApkInfo.g_begin_scene.ToString());

        M_Notification notif = new M_Notification(this, mSceneType.ToString());
        NotificationCenter.GetInstance().dispatchEvent(UIDefineEvent.eSceneLoadFinish, notif);

        //TopTitleCtl.instance.Reset();

    }

}
