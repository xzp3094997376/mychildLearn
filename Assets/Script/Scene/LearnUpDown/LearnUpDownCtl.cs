using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LearnUpDownCtl : BaseScene
{
    SoundManager mSound;

    // Use this for initialization
    void Start ()
    {
        mSceneType = SceneEnum.LearnUpDown;
        CallLoadFinishEvent();


        //mSound = g

        StartCoroutine(TStart());

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator TStart()
    {
        //创建背景图
        Image bg = UguiMaker.newImage("bg", transform, "learnupdown_sprite", "bg0", false);
        //Image bg =  UGUI ResManager.GetSprite("learnupdown_sprite", "bg0");


        yield return new WaitForSeconds(0.2f);


        //TopTitleCtl.instance.Reset();
        
        //




    }


}
