using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spine.Unity;

public class OneAndMoreDuck : MonoBehaviour {
    public static OneAndMoreDuck gSlect { get; set; }
    public SkeletonGraphic mSpine { get; set; }
    private GameObject prefab { get; set; }
    public bool mIsInWalter { get; set; }
    public string mType { get; set; }
    private Image waltershader { get; set; }
    private Image landshader { get; set; }
    private Image walter { get; set; }
    public bool mIsSwiming { get; set; }
    private Vector3 defPos { get; set; }
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    private float num = 0;
    private float ofset = 3;
    private string IdleName { get; set; }
	void Update () {
        if (!mIsSwiming) return;

        num += 0.1f;
        prefab.transform.localPosition = new Vector3(0, ofset * Mathf.Sin(num), 0);
        waltershader.transform.localPosition = new Vector3(ofsetX, ofsetY, 0) + new Vector3(0, ofset * Mathf.Sin(num), 0);

    }
    public void setPos(Vector3 pos)
    {
        defPos = pos;
        transform.localPosition = pos;
    }
    //创建鸭子
    public void createDuck(string prefabName,float _scaleNum,bool isInWalter,string _type)
    {
        mIsInWalter = isInWalter;
        mType = _type;
        if(_type == "small")
        {
            IdleName = "Idle_1";
        }else
        {
            IdleName = "Idle";
        }
        if (null == landshader)
        {
            landshader = UguiMaker.newImage("landshader", transform, "oneandmore_sprite", "shadow");
        }
        prefab = ResManager.GetPrefab("oneandmore_prefab", prefabName);
        prefab.transform.parent = transform;
        prefab.transform.localScale = Vector3.one * _scaleNum;
        prefab.transform.localPosition = Vector3.zero;
        PlaySpine(IdleName, true);
        if (mIsInWalter)
        {
            swiming();
        }
    }
    public void Click()
    {
        if (mIsSwiming)
        {
            if(mType == "small"){
                PlaySpine("Click_2", true, true);
            }
            else
            {
                PlaySpine("Click", true, true);
            }

        }else
        {
            PlaySpine("Click_1", true, true);
        }
    }
    private float ofsetY = 30;
    private float ofsetX = 0;
    //游泳
    public void swiming()
    {
        if (mIsSwiming) return;

        
        if(null != landshader)
        {
            landshader.enabled = false;
        }
        if (mType == "small")
        {
            IdleName = "Idle_2";
            ofsetY = 20;
            ofsetX = 5;
        }
        if (null == waltershader)
        {
            waltershader = UguiMaker.newImage("waltershader", transform, "oneandmore_sprite", "duck_" + mType + "_shader");
            waltershader.transform.localPosition = new Vector3(ofsetX, ofsetY, 0);
        }
        if (null == walter)
        {
            walter = UguiMaker.newImage("walter", transform, "oneandmore_sprite", "walter_" + mType + "_1");
            walter.transform.localPosition = new Vector3(0, 15, 0);
        }
        
        PlaySpine(IdleName, true);
        mIsSwiming = true;
        StartCoroutine(TPlayWalter());
    }
    //播放动画
    public void PlaySpine(string name, bool isloop, bool idDelay = false)
    {
        if (null == mSpine)
        {
            mSpine = prefab.transform.Find("spine").GetComponent<SkeletonGraphic>();
        }
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
    public void setDefPos()
    {
        StartCoroutine(TSetDefPos());
    }
    IEnumerator TSetDefPos()
    {
        Vector3 startPos = transform.localPosition;
        for (float j = 0; j < 1f; j += 0.2f)
        {
            transform.localPosition = Vector3.Lerp(startPos, defPos, j);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = defPos;
    }
    IEnumerator TPlayWalter()
    {
        int[] arr = new int[] { 1, 2, 3 };
        Color startColor = walter.color;
        Color endColor = walter.color;
        endColor.a = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            walter.color = startColor;
            for (int i = 0;i < arr.Length; i++)
            {
                walter.sprite = ResManager.GetSprite("oneandmore_sprite", "walter_" + mType + "_" + arr[i]);
                walter.SetNativeSize();
                yield return new WaitForSeconds(0.3f);
            }
            for (float j = 0; j < 1f; j += 0.02f)
            {
                walter.color = Color.Lerp(startColor, endColor, j);
                yield return new WaitForSeconds(0.01f);
            }
            walter.color = endColor;
        }
        

    }
}
