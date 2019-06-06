using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka3_Flower : MonoBehaviour {
    public RectTransform mRtran { get; set; }
    Image mFlower { get; set; }
    Image mGan { get; set; }
    Image[] mYezi { get; set; }

    public int mdata_color_id { get; set; }

    bool temp_yezi = false;
    Vector3 temp_yezi_angle = Vector3.zero;
    public void Update()
    {
        if(null != mYezi)
        {
            if(temp_yezi)
            {
                temp_yezi_angle += new Vector3(0, 0, 0.2f);
                mYezi[0].transform.localEulerAngles = temp_yezi_angle;
                mYezi[1].transform.localEulerAngles = temp_yezi_angle * -1;
                if(temp_yezi_angle.z > 5)
                {
                    temp_yezi = false;
                }
            }
            else
            {
                temp_yezi_angle -= new Vector3(0, 0, 0.2f);
                mYezi[0].transform.localEulerAngles = temp_yezi_angle;
                mYezi[1].transform.localEulerAngles = temp_yezi_angle * -1;
                if (temp_yezi_angle.z < -5)
                {
                    temp_yezi = true;
                }

            }
        }
    }

    public void Init(int color_id)
    {
        mdata_color_id = color_id;

        mRtran = gameObject.GetComponent<RectTransform>();
        
    }
    public void EatFlower()
    {
        mFlower.gameObject.SetActive(false);
        Growdown();
    }

    public void Growup()
    {
        StartCoroutine("TGrowup");

    }
    IEnumerator TGrowup()
    {
        mGan = UguiMaker.newImage("mGan", transform, "public", "white0", false);
        mGan.type = Image.Type.Sliced;
        mGan.rectTransform.localEulerAngles = new Vector3(0, 0, 90);
        mGan.rectTransform.pivot = new Vector2(0, 0.5f);
        mGan.color = new Color32(0, 126, 6, 255);

        if(!ShapeLogicCtl.instance.mSound.IsPlayingOnly())
            ShapeLogicCtl.instance.mSound.PlayOnlyDefaultAb("树");

        Vector2 size0 = new Vector3(0, 8);
        Vector2 size1 = new Vector2(Random.Range(25, 50), 8);
        Vector3 size2 = new Vector3(Random.Range(size1.x + 50, size1.x + 60), 8);
        for(float i = 0; i < 1f; i += 0.1f)
        {
            mGan.rectTransform.sizeDelta = Vector2.Lerp(size0, size1, i);
            yield return new WaitForSeconds(0.01f);
        }
        mGan.rectTransform.sizeDelta = size1;

        mYezi = new Image[2];
        mYezi[0] = UguiMaker.newImage("yezi", transform, "shapelogic_sprite", "guanka3_yezi", false);
        mYezi[1] = UguiMaker.newImage("yezi", transform, "shapelogic_sprite", "guanka3_yezi", false);
        mYezi[1].rectTransform.localScale = new Vector3(-1, 1, 1);
        mYezi[0].rectTransform.pivot = new Vector2(0, 0);
        mYezi[1].rectTransform.pivot = new Vector2(0, 0);
        mYezi[0].rectTransform.anchoredPosition3D = new Vector3(0, size1.x);
        mYezi[1].rectTransform.anchoredPosition3D = new Vector3(0, size1.x);

        temp_yezi_angle = new Vector3(0, 0, Random.Range(-15f, 15f));
        mYezi[0].transform.localEulerAngles = temp_yezi_angle;
        mYezi[1].transform.localEulerAngles = temp_yezi_angle;

        for (float i = 0; i < 1f; i += 0.05f)
        {
            Vector3 scale0 = Vector3.Lerp(Vector3.zero, Vector3.one, i + Mathf.Sin(Mathf.PI * i) * 0.5f);
            Vector3 scale1 = new Vector3(scale0.x * -1, scale0.y, scale0.z);
            mYezi[0].transform.localScale = scale0;
            mYezi[1].transform.localScale = scale1;
            yield return new WaitForSeconds(0.01f);

        }

        if (!ShapeLogicCtl.instance.mSound.IsPlayingOnly())
            ShapeLogicCtl.instance.mSound.PlayOnlyDefaultAb("树");
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mGan.rectTransform.sizeDelta = Vector2.Lerp(size1, size2, i);
            yield return new WaitForSeconds(0.01f);
        }
        mGan.rectTransform.sizeDelta = size2;

        mFlower = UguiMaker.newImage("mFlower", transform, "shapelogic_sprite", "guanka3_color" + mdata_color_id.ToString(), false);
        mFlower.rectTransform.anchoredPosition = new Vector2(0, size2.x);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mFlower.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i + Mathf.Sin(Mathf.PI * i) * 0.5f);
            yield return new WaitForSeconds(0.01f);
        }
        mFlower.transform.localScale = Vector3.one;


        yield break;

    }

    public void Growdown()
    {
        StartCoroutine("TGrowdown");
    }
    IEnumerator TGrowdown()
    {
        yield return new WaitForSeconds(1);
        if(null != mFlower)
        {
            Destroy(mFlower.gameObject);
            mFlower = null;
        }


        for (float i = 0; i < 1f; i += 0.1f)
        {
            Vector3 scale0 = Vector3.Lerp(Vector3.one, Vector3.zero, i);
            Vector3 scale1 = new Vector3(scale0.x * -1, scale0.y, scale0.z);
            mYezi[0].transform.localScale = scale0;
            mYezi[1].transform.localScale = scale1;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(mYezi[0].gameObject);
        Destroy(mYezi[1].gameObject);
        mYezi = null;

        if (!ShapeLogicCtl.instance.mSound.IsPlayingOnly())
            ShapeLogicCtl.instance.mSound.PlayOnlyDefaultAb("树");
        Vector2 temp_size0 = mGan.rectTransform.sizeDelta;
        Vector2 temp_size1 = new Vector2(0, temp_size0.y);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mGan.rectTransform.sizeDelta = Vector2.Lerp(temp_size0, temp_size1, i);
            yield return new WaitForSeconds(0.01f);
        }
        mGan.rectTransform.sizeDelta = temp_size1;

        Destroy(gameObject);

    }



}
