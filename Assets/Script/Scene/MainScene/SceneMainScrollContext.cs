using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SceneMainScrollContext : MonoBehaviour , IScrollRectFlush{
    public SceneMainScrollData mData { get; set; }
    public Text mText;
    public List<Image> mImage = new List<Image>();

    void Start () {
	
	}
	
    public void Flush(int index)
    {
        //Debug.Log("index=" + index + " " + gameObject.name);
        mData = SceneMainCtrl.instance.mLevel2.GetData(index);
        if(null != mData)
        {
            if(string.IsNullOrEmpty(mData.title))
            {
                if(mText.gameObject.activeSelf)
                    mText.gameObject.SetActive(false);
            }
            else 
            {
                if (!mText.gameObject.activeSelf)
                    mText.gameObject.SetActive(true);
                mText.text = mData.title;
            }
            for(int i = 0; i < mImage.Count; i++)
            {
                if(string.IsNullOrEmpty(mData.sprite_names[i]))
                {
                    if (mImage[i].gameObject.activeSelf)
                        mImage[i].gameObject.SetActive(false);
                }
                else
                {
                    if (!mImage[i].gameObject.activeSelf)
                        mImage[i].gameObject.SetActive(true);
                    mImage[i].sprite = ResManager.GetSprite("public", mData.sprite_names[i]);
                }
            }
        }

    }

    public void OnClkBtn(int index)
    {
        Debug.Log("onclk index=" + index);
        if(!string.IsNullOrEmpty(mData.game_names[index]))
        {
            SceneMgr.Instance.OpenSceneWithBlackScene(mData.game_names[index]);
        }
        else
        {
            Debug.LogError("没有这个场景:" + mData.game_names[index]);
        }

    }


}
