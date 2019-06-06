using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneMainLevel2 : MonoBehaviour
{
    ScrollRectUpdate mScrollRectUpdate { get; set; }
    List<SceneMainScrollData> mData = new List<SceneMainScrollData>();

    void Awake()
    {
        for(int i = 0; i < 1; i++)
        {
            mData.Add(new SceneMainScrollData("集合与分类", new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData(string.Empty, new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData(string.Empty, new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData(string.Empty, new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData("数运算", new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData(string.Empty, new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData(string.Empty, new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData("量的比较", new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData(string.Empty, new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData("形状", new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData(string.Empty, new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData(string.Empty, new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData("时空", new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData(string.Empty, new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData(string.Empty, new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData(string.Empty, new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData("空间", new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));
            mData.Add(new SceneMainScrollData(string.Empty, new List<string>() { "MonkeySortOut", "MonkeySortOut", "MonkeySortOut" }, new List<string>() { "scroll", "scroll", "scroll" }));

        }






    }

	void Start ()
    {
        GameObject root = GameObject.Instantiate(Resources.Load("prefab/scene_main/level2/root_level2")) as GameObject;
        UguiMaker.InitGameObj(root, transform, "root_level1", Vector3.zero, Vector3.one);
        root.GetComponent<RectTransform>().sizeDelta = new Vector2(GlobalParam.screen_width * 0.8f, GlobalParam.screen_height);
        root.SetActive(true);

        mScrollRectUpdate = root.GetComponent<ScrollRectUpdate>();
        mScrollRectUpdate.RectUpdate(mData.Count);


    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public SceneMainScrollData GetData(int index)
    {
        if(index < mData.Count && 0 <= index)
        {
            return mData[index];
        }
        return null;
    }

}
