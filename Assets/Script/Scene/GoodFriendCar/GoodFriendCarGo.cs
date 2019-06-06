using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoodFriendCarGo : MonoBehaviour {
    public static GoodFriendCarGo gSelect { get; set; }

    public RectTransform mRtran { get; set; }
    Image mMask { get; set; }
    Image mFrame { get; set; }
    Image[] mImages { get; set; }
    BoxCollider mBox { get; set; }
    Vector3[] mPoss { get; set; }
    Vector3[] mPossMove { get; set; }
    int mdata_index { get; set; }

	void Start () {
        mPoss = new Vector3[] {
            new Vector2(-56, 0),
            new Vector2(0, 0),
            new Vector2(56, 0)
        };
        mPossMove = new Vector3[] {
            new Vector2(-120, 0),
            new Vector2(0, 0),
            new Vector2(120, 0)
        };

        mRtran = gameObject.GetComponent<RectTransform>();
        
        mMask = UguiMaker.newImage("Mask", transform, "public", "white", false);
        mMask.rectTransform.sizeDelta = new Vector2(56 * 3, 100);
        mMask.gameObject.AddComponent<Mask>().showMaskGraphic = false;

        //mFrame = UguiMaker.newImage("Mask", transform, "goodfriendcar_sprite", "frame", false);
        //mFrame.type = Image.Type.Sliced;

        mImages = new Image[3];
        mImages[0] = UguiMaker.newImage("image0", mMask.transform, "goodfriendcar_sprite", "go_cell");
        mImages[1] = UguiMaker.newImage("image1", mMask.transform, "goodfriendcar_sprite", "go_cell");
        mImages[2] = UguiMaker.newImage("image2", mMask.transform, "goodfriendcar_sprite", "go_cell");
        mImages[0].rectTransform.anchoredPosition = mPoss[0];
        mImages[1].rectTransform.anchoredPosition = mPoss[1];
        mImages[2].rectTransform.anchoredPosition = mPoss[2];
        mImages[0].color = new Color32(196, 255, 129, 255);
        mImages[1].color = new Color32(196, 255, 129, 255);
        mImages[2].color = new Color32(196, 255, 129, 255);
        mBox = gameObject.AddComponent<BoxCollider>();
        mBox.size = new Vector3(56 * 3, 60, 1);

        
        gameObject.SetActive(false);

    }
    public void Init(int index)
    {
        mdata_index = index;
    }
	
    public void Hide()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);

    }
    public void Show()
    {
        gameObject.SetActive(true);
        gameObject.transform.localScale = Vector3.one;
        mBox.enabled = true;
        switch(mdata_index)
        {
            case 0:
                mRtran.anchoredPosition = new Vector2(-16.58f, 98);
                break;
            case 1:
                mRtran.anchoredPosition = new Vector2(-16.58f, -104);
                break;
            case 2:
                mRtran.anchoredPosition = new Vector2(-16.58f, -301);
                break;
        }

        mImages[0].color = new Color32(196, 255, 129, 255);
        mImages[1].color = new Color32(196, 255, 129, 255);
        mImages[2].color = new Color32(196, 255, 129, 255);
        //mImages[0].rectTransform.anchoredPosition = new Vector2(-56, 0);
        //mImages[1].rectTransform.anchoredPosition = new Vector2(0, 0);
        //mImages[2].rectTransform.anchoredPosition = new Vector2(56, 0);

        StopAllCoroutines();
        StartCoroutine("TShow");
    }
    IEnumerator TShow()
    {
        float a = 0f; ;
        float b = 0.33f;
        float c = 0.66f;
        float add = 0.02f;
        while (true)
        {
            if(a > 1)
            {
                a -= 1;
            }
            if(b > 1)
            {
                b -= 1;
            }
            if(c > 1)
            {
                c -= 1;
            }
            mImages[0].rectTransform.anchoredPosition = Vector3.Lerp(mPossMove[0], mPossMove[2], a);
            mImages[1].rectTransform.anchoredPosition = Vector3.Lerp(mPossMove[0], mPossMove[2], b);
            mImages[2].rectTransform.anchoredPosition = Vector3.Lerp(mPossMove[0], mPossMove[2], c);

            //mImages[0].rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, a);
            //mImages[1].rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, b);
            //mImages[2].rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, c);

            //mImages[0].color = Color32.Lerp(new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 0), a);
            //mImages[1].color = Color32.Lerp(new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 0), b);
            //mImages[2].color = Color32.Lerp(new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 0), c);

            a += add;
            b += add;
            c += add;
            yield return new WaitForSeconds(0.01f);
        }
        

        //yield break;
        //float add = 0.06f;
        //while (true)
        //{
        //    mImages[0].rectTransform.anchoredPosition = mPoss[0];
        //    mImages[1].rectTransform.anchoredPosition = mPoss[0];
        //    mImages[2].rectTransform.anchoredPosition = mPoss[0];
        //    float a = -1f;
        //    float b = -0.5f;
        //    float c = 0;
        //    for(float i = 0; i < 2; i += add)
        //    {
        //        mImages[0].rectTransform.anchoredPosition3D = Vector3.Lerp(mPoss[0], mPoss[2], a);
        //        mImages[1].rectTransform.anchoredPosition3D = Vector3.Lerp(mPoss[0], mPoss[2], b);
        //        mImages[2].rectTransform.anchoredPosition3D = Vector3.Lerp(mPoss[0], mPoss[2], c);
        //        a += add;
        //        b += add;
        //        c += add;
        //        yield return new WaitForSeconds(0.01f);
        //    }

        //}

        yield break;
        mImages[0].gameObject.SetActive(false);
        mImages[1].gameObject.SetActive(false);
        mImages[2].gameObject.SetActive(false);

        while (true)
        {
            for(int i = 0; i <= 3; i++)
            {
                switch(i)
                {
                    case 0:
                        mImages[0].gameObject.SetActive(true);
                        break;
                    case 1:
                        mImages[1].gameObject.SetActive(true);
                        break;
                    case 2:
                        mImages[2].gameObject.SetActive(true);
                        break;
                    case 3:
                        mImages[0].gameObject.SetActive(false);
                        mImages[1].gameObject.SetActive(false);
                        mImages[2].gameObject.SetActive(false);
                        break;
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public void OnClickDown()
    {
        StopAllCoroutines();
        mImages[0].gameObject.SetActive(true);
        mImages[1].gameObject.SetActive(true);
        mImages[2].gameObject.SetActive(true);
        mImages[0].rectTransform.anchoredPosition = mPoss[0];
        mImages[1].rectTransform.anchoredPosition = mPoss[1];
        mImages[2].rectTransform.anchoredPosition = mPoss[2];
        gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1);

    }

    public void Shoot()
    {
        StopAllCoroutines();
        StartCoroutine("TShoot");
    }
    IEnumerator TShoot()
    {
        mBox.enabled = false;
        gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        Vector3 pos0 = mRtran.anchoredPosition3D;
        Vector3 pos1 = pos0 + new Vector3(800, 0, 0);
        Vector3 speed = new Vector3(0, 0, 0);
        while(mRtran.anchoredPosition3D.x < pos1.x)
        {
            mRtran.anchoredPosition3D += speed;
            speed.x += 2f;
            yield return new WaitForSeconds(0.01f);
        }

        GoodFriendCarCtl.instance.mGuanka2.callback_CarShootBegin(mdata_index);

    }

}
