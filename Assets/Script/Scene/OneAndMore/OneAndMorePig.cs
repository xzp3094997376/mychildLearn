using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spine.Unity;

public class OneAndMorePig : MonoBehaviour {
    public SkeletonGraphic mSpine { get; set; }
    private GameObject prefab { get; set; }
    public bool mIsInWalter { get; set; }
    public string mType { get; set; }
    private Image waltershader { get; set; }
    private Image landshader { get; set; }
    private string IdleName { get; set; }
    private Image mask { get; set; }
    public bool isPraint { get; set; }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void setPos(Vector3 pos)
    {
        transform.localPosition = pos;
    }
    //创建鸭子
    public void createPig(string prefabName, float _scaleNum, bool ispraint, string _type)
    {
        mIsInWalter = ispraint;
        mType = _type;
        IdleName = "Idle";
        if (null == landshader)
        {
            landshader = UguiMaker.newImage("landshader", transform, "oneandmore_sprite", "shadow");
            if(mType == "small")
            {
                landshader.transform.localPosition = new Vector3(0, 15, 0);
            }else
            {
                landshader.rectTransform.sizeDelta = new Vector2(500, 50);
                landshader.transform.localPosition = new Vector3(0, 30, 0);
            }
        }
        prefab = ResManager.GetPrefab("oneandmore_prefab", prefabName);
        prefab.transform.parent = transform;
        prefab.transform.localScale = Vector3.one * _scaleNum;
        prefab.transform.localPosition = Vector3.zero;
        if (mType == "small")
        {
            isPraint = true;
            mask = UguiMaker.newImage("pigMask", transform, "public", "white");
            mask.rectTransform.pivot = new Vector2(1, 0.5f);
            Image pigdemo = UguiMaker.newImage("pigdemo", mask.transform, "oneandmore_sprite", "pig_praint");
            pigdemo.rectTransform.anchorMin = new Vector2(1, 0.5f);
            pigdemo.rectTransform.anchorMax = new Vector2(1, 0.5f);
            pigdemo.transform.localPosition = new Vector3(-130, -1, 0);
            pigdemo.rectTransform.sizeDelta = new Vector2(232, 146);
            mask.transform.localPosition = new Vector3(124, 87, 0);
            mask.rectTransform.sizeDelta = new Vector2(250, 150);
            Mask mk = mask.gameObject.AddComponent<Mask>();
            mk.showMaskGraphic = false;
            stopPrefab();
            //prefab.gameObject.active = false;
            //praint();
        }else
        {
            isPraint = false;
            PlaySpine(IdleName, true);
        }
    }
    public void Click()
    {
        if (!isPraint)
        {
            PlaySpine("Click", true, true);
        }
    }
    public void praint()
    {
        mask.rectTransform.sizeDelta = new Vector2(0, 150);
        isPraint = false;
        PlaySpine(IdleName, true);
        //StartCoroutine(TPraint());
    }
    IEnumerator TPraint()
    {
        float startWidth = 250;
        for (float j = 0; j < 1f; j += 0.02f)
        {
            startWidth -= 5;
            mask.rectTransform.sizeDelta = new Vector2(startWidth, 150);
            yield return new WaitForSeconds(0.01f);
        }
        isPraint = false;
        PlaySpine(IdleName, true);
    }
    private void stopPrefab()
    {
        if (null == mSpine)
        {
            mSpine = prefab.transform.Find("spine").GetComponent<SkeletonGraphic>();
        }
        mSpine.timeScale = 0;
    }
    //播放动画
    public void PlaySpine(string name, bool isloop, bool idDelay = false)
    {
        if (null == mSpine)
        {
            mSpine = prefab.transform.Find("spine").GetComponent<SkeletonGraphic>();
        }
        mSpine.timeScale = 1;
        mSpine.AnimationState.ClearTracks();
        mSpine.AnimationState.SetAnimation(2, name, isloop);
        if (idDelay)
        {
            StartCoroutine(TDelay());
        }
    }
    IEnumerator TDelay()
    {
        yield return new WaitForSeconds(1);
        PlaySpine(IdleName, true);
    }
}
