using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 主场
/// </summary>
public class SceneMainCtrl : BaseScene
{
    public static SceneMainCtrl instance = null;

    public SceneMainLevel1 mLevel1 { get; set; }
    public SceneMainLevel2 mLevel2 { get; set; }

    public int mLevel = 1;

    void Awake()
    {
        instance = this;
        mLevel = GlobalParam.scene_main_level;
        
    }

    void Start()
    {
        GotoLevel(mLevel);

        mSceneType = SceneEnum.SceneMain;
        CallLoadFinishEvent();
    }
    void Update()
    {

    }

    public void GotoLevel(int level)
    {
        mLevel = level;
        if(1 == mLevel)
        {
            transform.Find("level2").gameObject.SetActive(false);

            if (null != mLevel1)
                mLevel1.gameObject.SetActive(true);
            else
            {
                mLevel1 = transform.Find("level1").gameObject.AddComponent<SceneMainLevel1>();
                mLevel1.gameObject.SetActive(true);
            }

        }
        else
        {
            transform.Find("level1").gameObject.SetActive(false);

            if (null != mLevel2)
                mLevel2.gameObject.SetActive(true);
            else
            {
                mLevel2 = transform.Find("level2").gameObject.AddComponent<SceneMainLevel2>();
                mLevel2.gameObject.SetActive(true);
            }

        }

    }

    void Click1(GameObject _go)
    {
        SceneMgr.Instance.OpenSceneWithBlackScene(SceneEnum.MonkeySortOut);
    }

    
    void OnDestroy()
    {
        if(this == instance)
        {
            instance = null;
        }
    }

}