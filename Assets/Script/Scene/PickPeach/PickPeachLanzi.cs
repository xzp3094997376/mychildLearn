using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickPeachLanzi : MonoBehaviour {
    public static PickPeachLanzi gSelect = null;

    public Image mBg0, mBg1, mShadow;
    public RectTransform mRtran { get; set; }
    public RectTransform mPlace { get; set; }
    BoxCollider mBox { get; set; }

    List<Image> mFlower = new List<Image>();
    List<PickPeachTao> mTaos = new List<PickPeachTao>();
    List<Vector3> mTaoPos = new List<Vector3>() {
        new Vector3(21.5f, -25, 0),
        new Vector3(-23.3f, -24.26f, 0),
        new Vector3(34.1f, -5.3f, 0),
        new Vector3( 8.7f, -2.1f, 0),
        new Vector3(-35.1f, -5.6f, 0)
    };

    public int mdata_flower_number = 0;

    void Start()
    {
        mBox = gameObject.AddComponent<BoxCollider>();
        mBox.size = new Vector3(151.83f, 159.3f, 5);

    }

    public void Clean()
    {
        for(int i = 0; i < mTaos.Count; i++)
        {
            if(null != mTaos[i])
            {
                Destroy(mTaos[i].gameObject);
            }
        }
        mTaos.Clear();

    }
    public void Init()
    {
        if(mRtran == null)
        {
            mRtran = gameObject.GetComponent<RectTransform>();
            mBg0 = UguiMaker.newImage("mBg0", transform, "pickpeach_sprite", "lanzi0");
            mPlace = UguiMaker.newGameObject("mPlace", transform).GetComponent<RectTransform>();
            mBg1 = UguiMaker.newImage("mBg1", transform, "pickpeach_sprite", "lanzi1");

            for(int i = 0; i < 5; i++)
            {
                Image img = UguiMaker.newImage(i.ToString(), mBg1.transform, "pickpeach_sprite", "hua0", false);
                img.transform.localScale = new Vector3(0.7f, 0.7f, 1);
                mFlower.Add(img);
            }

        }

        mRtran.anchoredPosition = new Vector2(9999, 0);

    }
    public void SetFlowerNum(int flower_num)
    {
        mdata_flower_number = flower_num;
        List<Vector3> poss = null;
        switch (flower_num)
        {
            case 1:
                poss = new List<Vector3>() {
                    new Vector3(0, -62.4f, 0),
                };
                break;
            case 2:
                poss = new List<Vector3>() {
                    new Vector3(-16.6f, -60.7f, 0),
                    new Vector3(24.5f, -60.7f, 0)
                };
                break;
            case 3:
                poss = new List<Vector3>() {
                    new Vector3(-33.8f, -55.1f, 0),
                    new Vector3(0, -62.4f, 0),
                    new Vector3(33.4f, -58.4f, 0)
                };
                break;
            case 4:
                poss = new List<Vector3>()
                {
                    new Vector3(-37.7f, -54.2f, 0),
                    new Vector3(-11.8f, -61.4f, 0),
                    new Vector3(15.1f, -61.1f, 0),
                    new Vector3(41, -54.2f, 0)
                };
                break;
            default:
                poss = new List<Vector3>() {
                    new Vector3(-44.9f, -50.9f, 0),
                    new Vector3(-23.3f, -59.9f, 0),
                    new Vector3(0, -62.4f, 0),
                    new Vector3(24.1f, -61.3f, 0),
                    new Vector3(47.8f, -52.3f, 0)
                };
                break;
        }
        HideFlower();
        for(int i = 0; i < flower_num; i++)
        {
            mFlower[i].gameObject.SetActive(true);
            mFlower[i].rectTransform.anchoredPosition3D = poss[i];
            mFlower[i].sprite = ResManager.GetSprite("pickpeach_sprite", "hua0");
        }

    }
    public void SetBoxEnable(bool _enable)
    {
        mBox.enabled = _enable;
    }
    public void HideFlower()
    {
        for (int i = 0; i < 5; i++)
            mFlower[i].gameObject.SetActive(false);
    }

    /// <summary>
    /// 返回 桃子数 - 花朵数
    /// </summary>
    /// <returns></returns>
    public int GetResult()
    {
        return mTaos.Count - mdata_flower_number;
    }
    public bool Correct()
    {
        if( mdata_flower_number == mTaos.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ShowShadow()
    {
        if(null == mShadow)
        {
            mShadow = UguiMaker.newImage("mShadow", transform, "public", "shadow", false);
            mShadow.transform.SetAsFirstSibling();
            mShadow.transform.localEulerAngles = Vector3.zero;
            mShadow.rectTransform.anchoredPosition = new Vector2(0, -58.8f);
            mShadow.rectTransform.sizeDelta = new Vector2(124.2f, 64f);
            mShadow.color = new Color32(0, 0, 0, 30); 
        
        }
        mShadow.gameObject.SetActive(true);
        
    }
    public void HidwShadow()
    {
        if(null != mShadow)
        {
            mShadow.gameObject.SetActive(false);
        }
    }


    public bool PushTaozi(PickPeachTao tao)
    {
        if (mTaos.Count == 5)
            return false;

        mFlower[mTaos.Count].sprite = ResManager.GetSprite("pickpeach_sprite", "hua1");
        mTaos.Add(tao);

        tao.SetBoxEnable(false);
        tao.mRtran.SetParent(mPlace);
        tao.mRtran.SetAsLastSibling();
        tao.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        tao.mRtran.anchoredPosition = new Vector2(Random.Range(-30, 30), Random.Range(-15f, -8.7f));
        for(int i = 0; i < mTaos.Count; i++)
        {
            mTaos[mTaos.Count - 1 - i].mRtran.anchoredPosition3D = mTaoPos[i];
        }

        

        return true;

    }
    public void PopTaozi()
    {
        if (mTaos.Count == 0)
            return;
        mTaos[0].OutLanzi();
        mTaos.RemoveAt(0);
        mFlower[mTaos.Count].sprite = ResManager.GetSprite("pickpeach_sprite", "hua0");
        Global.instance.PlayBtnClickAnimation(transform);

    }

    public void Shake()
    {
        StopCoroutine("TShake");
        StartCoroutine("TShake");
    }
    IEnumerator TShake()
    {
        PickPeachCtl.instance.mSound.PlayShort( "按钮1");
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * i) * 8);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.localEulerAngles = Vector3.zero;
        
    }

}
