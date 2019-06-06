using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneMainLevel1 : MonoBehaviour {
    Button mBtnLesson { get; set; }//全部课程
    Button mBtnDrill { get; set; }//专项训练

	void Start ()
    {
        GameObject root = GameObject.Instantiate(Resources.Load("prefab/scene_main/level1/root_level1")) as GameObject;
        UguiMaker.InitGameObj(root, transform, "root_level1", Vector3.zero, Vector3.one);
        mBtnLesson = root.transform.Find("lesson").GetComponent<Button>();
        mBtnDrill = root.transform.Find("drill").GetComponent<Button>();
        mBtnLesson.onClick.AddListener(OnClkLesson);
        mBtnDrill.onClick.AddListener(OnClkDrill);
        root.SetActive(true);

    }
    IEnumerator TStart()
    {
        mBtnLesson = UguiMaker.newButton("lesson", transform, "public", "gameover_replay");
        mBtnDrill = UguiMaker.newButton("drill", transform, "public", "gameover_next");
        mBtnLesson.onClick.AddListener(OnClkLesson);
        mBtnDrill.onClick.AddListener(OnClkDrill);

        Vector2 pos_beg1 = new Vector2(GlobalParam.screen_width * -0.5f - 200, 0);
        Vector2 pos_beg2 = new Vector2(GlobalParam.screen_width * 0.5f + 200, 0);
        Vector2 pos_end1 = new Vector2(GlobalParam.screen_width * -0.25f, 0);
        Vector2 pos_end2 = new Vector2(GlobalParam.screen_width * 0.25f, 0);

        int step = 10;
        
        yield return new WaitForSeconds(0.01f);

    }


    public void OnClkLesson()
    {
        Debug.Log("OnClkLesson()");
        SceneMainCtrl.instance.GotoLevel(2);
    }

    public void OnClkDrill()
    {
        Debug.Log("OnClkDrill()");

    }

}
