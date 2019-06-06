using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Spine.Unity;

public enum kbady_enum
{
    Encourage_1,
    Encourage_2,
    Encourage_3,
    Encourage_4,
    Idle,
    Narrate,
    shuyohua,
}

public class KbadyCtl : MonoBehaviour {
    public static KbadyCtl instance;
    public static void Init()
    {
        if (null != instance)
            return;
        GameObject obj = ResManager.GetPrefabInResources("prefab/Kbady/Kbady");
    }

    public SkeletonGraphic mSpine;
    public RectTransform mRtranSpine;

    void Awake()
    {
        if (null != instance)
            Destroy(this);
        instance = this;
        transform.SetParent(Global.instance.mCanvasTop);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        mRtranSpine.localScale = Vector3.zero;

    }
    void Start()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 9;
        gameObject.AddComponent<GraphicRaycaster>();
        gameObject.layer = LayerMask.NameToLayer("UI");

        //mRtranSpine.gameObject.SetActive(false);
        //gameObject.SetActive(false);
    }
    //void Update()
    //{

    //    if (Input.GetKeyDown(KeyCode.B))
    //    {
    //        //mRtran.anchoredPosition = new Vector2(0, -150);
    //        //PlayBgEffect0(null);
    //        BgEffect1_Create(new Color32(255, 0, 255, 255));
    //        BgEffect1_Play();
    //    }
    //}


    public void ShowSpine(Vector3 end_scale)
    {
        StartCoroutine(TShowSpine(end_scale));
    }
    public void HideSpine(bool immediate = false)
    {
        if (immediate)
            mRtranSpine.gameObject.SetActive(false);
        else
            StartCoroutine("THideSpine");
    }
    IEnumerator TShowSpine(Vector3 end_scale)
    {
        mRtranSpine.localScale = Vector3.zero;
        mRtranSpine.gameObject.SetActive(true);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            float param = Mathf.Sin(Mathf.PI * i) * 0.1f;
            mRtranSpine.localScale = Vector2.Lerp(Vector2.zero, end_scale, i) + new Vector2(param, param);
            yield return new WaitForSeconds(0.01f);
        }
        mRtranSpine.localScale = end_scale;
    }
    IEnumerator THideSpine()
    {
        Vector2 scale = mRtranSpine.localScale;
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtranSpine.localScale = Vector2.Lerp(scale, Vector2.zero, i);
            yield return new WaitForSeconds(0.01f);
        }
        mRtranSpine.localScale = Vector3.zero;
        mRtranSpine.gameObject.SetActive(false);
    }


    public void PlaySpine(kbady_enum k_enum, bool loop)
    {
        mRtranSpine.gameObject.SetActive(true);
        mSpine.AnimationState.ClearTracks();

        mSpine.AnimationState.TimeScale = 1;
        mSpine.AnimationState.SetAnimation(1, k_enum.ToString(), loop);
        
    }
    public void PlaySpineEncourage(bool loop)
    {
        mRtranSpine.gameObject.SetActive(true);
        mSpine.AnimationState.TimeScale = 1;
        mSpine.AnimationState.ClearTracks();
        switch (Random.Range(0, 1000) % 4)
        {
            case 0:
                mSpine.AnimationState.SetAnimation(1, "Encourage_1", loop);
                break;
            case 1:
                mSpine.AnimationState.SetAnimation(1, "Encourage_2", loop);
                break;
            case 2:
                mSpine.AnimationState.SetAnimation(1, "Encourage_3", loop);
                break;
            case 3:
                mSpine.AnimationState.SetAnimation(1, "Encourage_4", loop);
                break;

        }
        

    }
    public void StopSpine()
    {
        mSpine.AnimationState.TimeScale = 0;
    }

    public void DestroyHelp()
    {
        if(mBgEffect1_Mask != null)
        {
            Destroy(mBgEffect1_Mask.gameObject);
            mBgEffect1_Mask = null;
        }
    }
    

    #region bg_effect1
    RectTransform mBgEffect1_Mask = null;
    public RectTransform BgEffect1_Create(Color32 color)
    {
        if(null == mBgEffect1_Mask)
        {
            GameObject obj = ResManager.GetPrefabInResources("prefab/Kbady/bg_effect1");
            obj.transform.SetParent(transform);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            mBgEffect1_Mask = obj.GetComponent<RectTransform>();
        }
        Material mat = mBgEffect1_Mask.GetComponent<RawImage>().material;
        mat.SetFloat("_Distance", 0);
        mat.SetColor("_Color", color);
        mBgEffect1_Mask.GetComponent<RawImage>().enabled = false;
        return mBgEffect1_Mask;
    }
    public void BgEffect1_Play()
    {
        StartCoroutine("TBgEffect1_Play");
    }
    public void BgEffect1_Stop()
    {
        StartCoroutine("TBgEffect1_Stop");
    }
    IEnumerator TBgEffect1_Play()
    {
        SoundManager.instance.PlayShort("08-蓝圈");
        mBgEffect1_Mask.GetComponent<RawImage>().enabled = true;
        Material mat = mBgEffect1_Mask.GetComponent<RawImage>().material;
        for (float i = 0; i < 1f; i += 0.02f)
        {
            //mBgEffect1_Mask.sizeDelta = Vector2.Lerp(Vector2.zero, new Vector2(1600, 1600), i);
            mat.SetFloat("_Distance", i);
            yield return new WaitForSeconds(0.01f);
        }
        mat.SetFloat("_Distance", 1);
    }
    IEnumerator TBgEffect1_Stop()
    {
        Material mat = mBgEffect1_Mask.GetComponent<RawImage>().material;
        for (float i = 0; i < 1f; i += 0.02f)
        {
            //mBgEffect1_Mask.sizeDelta = Vector2.Lerp(new Vector2(1600, 1600), Vector2.zero, i);
            mat.SetFloat("_Distance", 1 - i);
            yield return new WaitForSeconds(0.01f);
        }
        mat.SetFloat("_Distance", 0);
        Destroy(mBgEffect1_Mask.gameObject);
        mBgEffect1_Mask = null;

    }

    #endregion


}
