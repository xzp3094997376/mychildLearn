using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneMgr : MonoBehaviour
{

    private static SceneMgr _instance;
    public static SceneMgr Instance
    {
        get
        {
            if(null == _instance)
            {
                _instance = Global.instance.gameObject.AddComponent<SceneMgr>();

                Image mask = UguiMaker.newImage("blackpanel", Global.instance.mCanvasTop, "public", "white");
                mask.rectTransform.sizeDelta = new Vector2(1423, 800);
                mask.color = new Color(0, 0, 0, 0.5f);

                _instance.mBlackCtrl = mask.gameObject.AddComponent<MBlackPanelCtrl>();
                _instance.mBlackCtrl.mImage = mask;
                _instance.mBlackCtrl.gameObject.SetActive(false);


                //场景加载完成监听
                NotificationCenter.GetInstance().addEventListerner(UIDefineEvent.eSceneLoadFinish, _instance.SceneLoadFinish);

            }
            return _instance;
        }
    }

    private MBlackPanelCtrl mBlackCtrl;


    //当前的场景
    private static BaseScene mScene = null;
    public static void SetCurentScene(BaseScene curent_scene)
    {
        mScene = curent_scene;
    }

    /// <summary>
    /// 获取当前场景
    /// </summary>
    /// <returns>BaseScene</returns>
    public BaseScene GetNowScene()
    {
        if(null == mScene)
        {
            mScene = Global.instance.mCanvasScene.Find("scene").GetChild(0).GetComponent<BaseScene>();
        }
        return mScene;
    }
    
    #region//黑屏---
    /// <summary>
    /// 渐进黑屏
    /// </summary>
    /// <param name="_callback"></param>
    public void BlackIn(System.Action _callback = null)
    {
        mBlackCtrl.BlackIn(_callback);
    }
    /// <summary>
    /// 渐退黑屏
    /// </summary>
    /// <param name="_callback"></param>
    public void BlackOut(System.Action _callback = null)
    {
        mBlackCtrl.BlackOut(_callback);
    }
    public void BlackInOut(System.Action _callback = null)
    {
        mBlackCtrl.BlackInOut(_callback);
    }
    #endregion

    #region//load scene---
    /// <summary>
    /// 直接打开场景
    /// </summary>
    /// <param name="_strSceneName">prefab名字</param>
    public void OpenScene(string _strSceneName)
    {
        //友盟统计关卡打开次数
        //Debug.Log("友盟统计关卡打开次数：" + _strSceneName);
        //Umeng.GA.Event(_strSceneName);

        GameObject scene = ResManager.GetPrefabInResources("scene/" + _strSceneName);
        scene.SetActive(true);
        UguiMaker.InitGameObj(scene, Global.instance.mCanvasScene, _strSceneName, Vector3.zero, Vector3.one);
        if (mScene == null)
            SetCurentScene(scene.GetComponent<BaseScene>());
    }
    /// <summary>
    /// 黑屏方式打开场景
    /// </summary>
    /// <param name="_strSceneName">prefab名字</param>
    public void OpenSceneWithBlackScene(string _strSceneName)
    {
        BlackIn(() =>
        {
            OpenScene(_strSceneName);
            BlackOut();
        });
    }

    /// <summary>
    /// 直接打开场景
    /// </summary>
    public void OpenScene(SceneEnum _sceneType)
    {
        OpenScene(_sceneType.ToString());
    }
    public void OpenSceneWithBlackScene(SceneEnum _sceneType)
    {
        OpenSceneWithBlackScene(_sceneType.ToString());
    }
    #endregion



    /// <summary>
    /// 场景加载完成
    /// </summary>
    /// <param name="_notif"></param>
    private void SceneLoadFinish(M_Notification _notif)
    {    
        if (_notif != null)
        {
            BaseScene sender = _notif.sender as BaseScene;
            if (mScene != null && mScene.mSceneType == sender.mSceneType)
            {
                Debug.Log("same scene: " + mScene.mSceneType.ToString());
                return;
            }

            if (mScene != null)
                GameObject.Destroy(mScene.gameObject);
            mScene = null;

            mScene = sender;
            Debug.Log("Load: " + mScene.mSceneType.ToString());
            
        }
    }
    




}
