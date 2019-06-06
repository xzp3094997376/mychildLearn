using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickPeachGuanka2 : MonoBehaviour {

    public RectTransform mGuanka2Root { get; set; }
    public RectTransform mCaihong { get; set; }
    public PickPeachSelectOption mSelectBig { get; set; }
    public PickPeachSelectOption mSelectSmall { get; set; }

    int mdata_small_guanka_index = 0;//0找最多，1找最少

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (2 != PickPeachCtl.instance.mdata_guanka)
            return;

        if(Input.GetMouseButtonUp(0))
        {
            PickPeachLanzi lanzi = null;
            RaycastHit[] hits = Common.getMouseRayHits();
            if (null != hits)
            {
                foreach (RaycastHit hit in hits)
                {
                    lanzi = hit.collider.gameObject.GetComponent<PickPeachLanzi>();
                    if (null != lanzi)
                        break;
                }
            }
            if (null != lanzi)
            {
                if(0 == mdata_small_guanka_index)
                {
                    if(lanzi.mdata_flower_number == PickPeachCtl.instance.GetFlowerNumBig())
                    {
                        //选对
                        mSelectBig.SetBoxEnable(false);
                        mSelectBig.transform.SetParent(lanzi.transform);
                        mSelectBig.mRtran.anchoredPosition = new Vector2(-45, 55.3f);
                        mSelectBig.mRtran.localEulerAngles = new Vector3(0, 0, 40.51f);
                        //mSelectBig.mRtran.localScale = new Vector3(0.4f, 0.4f, 1);
                        mSelectBig.Show();
                        mSelectBig.PlayEffect();
                        mdata_small_guanka_index = 1;

                        PickPeachCtl.instance.mSound.StopTip();

                        PickPeachCtl.instance.mSound.PlayShort("按钮点击正确");
                        if(0 == Random.Range(0, 1000) % 2)
                        {
                            PickPeachCtl.instance.mSound.PlayTipListDefaultAb(new List<string>() { "tip你找到数量最多的篮子", "tip点一点", "tip那个篮子里面的桃子最少" }, new List<float>() { 1, 1, 1 }, true);
                            //PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip你找到数量最多的篮子");
                        }
                        else
                        {
                            PickPeachCtl.instance.mSound.PlayTipListDefaultAb(new List<string>() { "tip你找到桃子数量最多的篮子", "tip点一点", "tip那个篮子里面的桃子最少" }, new List<float>() { 1, 1, 1 }, true);
                            //PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip你找到桃子数量最多的篮子");
                        }
                        //Global.instance.PlayBtnClickAnimation(lanzi.transform);

                        //Invoke("InitNext", 5);

                    }
                    else
                    {
                        if (0 == Random.Range(0, 1000) % 2)
                            PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip数清楚才能更快找出最多最少");
                        else
                            PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip有点不太对你再仔细数数");

                        lanzi.Shake();
                    }
                }
                else if(1 == mdata_small_guanka_index)
                {
                    if (lanzi.mdata_flower_number == PickPeachCtl.instance.GetFlowerNumSmall())
                    {
                        mSelectSmall.SetBoxEnable(false);
                        mSelectSmall.transform.SetParent(lanzi.transform);
                        mSelectSmall.mRtran.anchoredPosition = new Vector2(-45, 55.3f);
                        mSelectSmall.mRtran.localEulerAngles = new Vector3(0, 0, 40.51f);
                        //mSelectSmall.mRtran.localScale = new Vector3(0.4f, 0.4f, 1);
                        mSelectSmall.Show();
                        mSelectSmall.PlayEffect();
                        mdata_small_guanka_index = 1;
                        //Global.instance.PlayBtnClickAnimation(lanzi.transform);

                        PickPeachCtl.instance.mSound.PlayShort("按钮点击正确");
                        PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip这个篮子里的桃子最少");

                        if (!mSelectBig.GetBoxEnable() && !mSelectSmall.GetBoxEnable())
                        {
                            //游戏结束进入第三关
                            PickPeachCtl.instance.callbackGuanka2_over();
                        }

                    }
                    else
                    {
                        lanzi.Shake();
                    }

                }
                else
                {
                    Debug.LogError("不可能");
                }
                
            }
            else
            {
            }
            PickPeachSelectOption.gSelect = null;
            

        }
     
    }

    public void Clean()
    {
        if(null != mGuanka2Root)
        {
            Destroy(mGuanka2Root.gameObject);
        }
        if(null != mSelectBig)
        {
            Destroy(mSelectBig.gameObject);
        }
        if (null != mSelectSmall)
        {
            Destroy(mSelectSmall.gameObject);
        }
        mGuanka2Root = null;
        mCaihong = null;
        mSelectSmall = null;

    }
    //public void InitNext()
    //{
    //    //初始化找最少。。。。
    //    PickPeachCtl.instance.mSound.PlayTipListDefaultAb(new List<string>() { "tip点一点", "tip那个篮子里面的桃子最少" }, new List<float>() { 1, 1 }, true);

    //}

    //彩虹半径是1833
    public void BeginGame()
    {
        if(mGuanka2Root == null)
        {
            mGuanka2Root = ResManager.GetPrefab("pickpeach_prefab", "guanka2").GetComponent<RectTransform>();
            UguiMaker.InitGameObj(mGuanka2Root.gameObject, transform, "guanka2root", Vector3.zero, Vector3.one);

            mCaihong = mGuanka2Root.transform.Find("caihong").GetComponent<RectTransform>();

            mSelectBig = UguiMaker.newImage("mSelectBig", mGuanka2Root, "pickpeach_sprite", "tag_big", false).gameObject.AddComponent<PickPeachSelectOption>();
            mSelectSmall = UguiMaker.newImage("mSelectSmall", mGuanka2Root, "pickpeach_sprite", "tag_small", false).gameObject.AddComponent<PickPeachSelectOption>();

        }
        //mSelectBig.Init(PickPeachCtl.instance.GetFlowerNumBig());
        //mSelectSmall.Init(PickPeachCtl.instance.GetFlowerNumSmall());
        mGuanka2Root.anchoredPosition3D = new Vector3(0, 1000, 0);
        mCaihong.localEulerAngles = new Vector3(0, 0, 50);
        mSelectBig.transform.localScale = Vector3.zero;
        mSelectSmall.transform.localScale = Vector3.zero;

        float[] posx = new float[] { Common.gWidth * -0.25f, 0, Common.gWidth * 0.25f};
        for (int i = 0; i < 3; i++)
        {
            PickPeachCtl.instance.mLanzi[i].transform.SetParent(mCaihong.transform);
            PickPeachCtl.instance.mLanzi[i].transform.localScale = new Vector3(1.5f, 1.5f, 1);
            PickPeachCtl.instance.mLanzi[i].transform.localEulerAngles = Vector3.zero;
            PickPeachCtl.instance.mLanzi[i].mRtran.anchoredPosition = new Vector3(
                posx[i], Mathf.Sqrt(1833 * 1833 - posx[i] * posx[i]) + 70, 0);
            PickPeachCtl.instance.mLanzi[i].SetBoxEnable(true);
            PickPeachCtl.instance.mLanzi[i].ShowShadow();

            PickPeachCtl.instance.mLanzi[i].HideFlower();
            //PickPeachCtl.instance.mLanzi[i].

        }

        mdata_small_guanka_index = 0;

        StartCoroutine(TBeginGame());
    }
    IEnumerator TBeginGame()
    {
        //yield return new WaitForSeconds(4.5f);//等待画面离开桃树

        //场景从上而将
        for (float i = 0; i < 1f; i += 0.02f)
        {
            mGuanka2Root.anchoredPosition3D = Vector3.Lerp(new Vector3(0, 1000, 0), Vector3.zero, i);
            yield return new WaitForSeconds(0.01f);
        }
        mGuanka2Root.anchoredPosition3D = Vector3.zero;

        PickPeachCtl.instance.mSound.PlayShortDefaultAb("动画里发光音效氛围渲染");
        yield return new WaitForSeconds(0.5f);
        for (float i = 0; i < 1f; i += 0.04f)
        {
            mCaihong.localEulerAngles = Vector3.Lerp( new Vector3(0, 0, 50), Vector3.zero, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mCaihong.localEulerAngles = Vector3.zero;

        //mSelectBig.mResetPos = new Vector2(Common.gWidth * -0.22f, -286);
        //mSelectSmall.mResetPos = new Vector2(Common.gWidth * 0.22f, -286);
        //mSelectBig.Show();
        //mSelectSmall.Show();

        PickPeachCtl.instance.mSound.PlayTipListDefaultAb(new List<string>() { "tip点一点", "tip那个篮子里面的桃子最多" }, new List<float>() { 1, 1 }, true);
        
    }


}
