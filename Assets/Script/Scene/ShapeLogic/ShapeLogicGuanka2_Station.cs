using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections;

public class ShapeLogicGuanka2_Station : MonoBehaviour
{

    public static ShapeLogicGuanka2_Station gSelect = null;

    public RectTransform mRtran { get; set; }
    Image mImage { get; set; }
    BoxCollider mBox { get; set; }
    Vector3 mResetPos { get; set; }


    public int mdata_id { get; set; }

    public void Init(int id)
    {
        mdata_id = id;

        mRtran = gameObject.GetComponent<RectTransform>();
        mImage = UguiMaker.newImage("mImage", transform, "shapelogic_sprite", "guanka2_" + id.ToString(), false);
        mImage.rectTransform.localEulerAngles = new Vector3(0, 0, 90);
        mImage.rectTransform.pivot = new Vector2(1, 0.5f);
        mImage.rectTransform.anchoredPosition3D = Vector3.zero;

        if(null == mBox)
        {
            mBox = gameObject.AddComponent<BoxCollider>();
            mBox.size = new Vector3(240, 200, 1);
            mBox.center = new Vector3(0, -100, 0);
        }



    }

    public void SetBoxEnable(bool _enable)
    {
        mBox.enabled = _enable;

    }
    public void Select()
    {
        transform.SetAsLastSibling();
        mRtran.localScale = Vector3.one;
        StartCoroutine("TShake");

    }
    public void UnSelect()
    {
        StopCoroutine("TShake");
        mRtran.localEulerAngles = Vector3.zero;
        mRtran.localScale = new Vector3(0.9f, 0.9f, 1);
    }

    public void ShowByScale()
    {
        StartCoroutine("TShowByScale");

    }
    IEnumerator TShowByScale()
    {
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(Mathf.PI * i) + i);
            yield return new WaitForSeconds(0.01f);

        }
        mRtran.localScale = Vector3.one;

    }

    public void ShowByUp()
    {
        mRtran.localScale = new Vector3(0.9f, 0.9f, 1);
        StartCoroutine("TShowByUp");

    }
    IEnumerator TShowByUp()
    {
        Vector3 pos0 = mRtran.anchoredPosition3D;
        Vector3 pos1 = pos0 - new Vector3(0, 300, 0);
        for (float i = 0; i < 1f; i += 0.04f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos1, pos0, i) + new Vector3(0, Mathf.Sin(Mathf.PI * i) * 250, 0);
            yield return new WaitForSeconds(0.01f);

        }
        mRtran.anchoredPosition3D = pos0;
        mResetPos = pos0;


    }

    public void Shake()
    {
        StartCoroutine("TShake");
    }
    IEnumerator TShake()
    {
        float p = 0;
        while(true)
        {
            mRtran.localEulerAngles = new Vector3(0, 0, Mathf.Sin(p) * 3);
            p += 0.2f;
            yield return new WaitForSeconds(0.01f);
        }


    }


    public void ResetPos()
    {
        StartCoroutine("TResetPos");
    }
    IEnumerator TResetPos()
    {
        SetBoxEnable(false);

        while (40 < Vector3.Distance(mResetPos, mRtran.anchoredPosition3D))
        {
            Vector3 dir = (mResetPos - mRtran.anchoredPosition3D).normalized * 40;
            dir.z = 0;
            mRtran.anchoredPosition3D += dir;

            yield return new WaitForSeconds(0.01f);

        }

        mRtran.anchoredPosition3D = mResetPos;
        SetBoxEnable(true);

    }

    public void PlayError()
    {
        StartCoroutine("TPlayError");

    }
    IEnumerator TPlayError()
    {

        float speed = 0;
        while (true)
        {
            mRtran.anchoredPosition3D += new Vector3(0, -speed, 0);

            yield return new WaitForSeconds(0.01f);

            speed += 1f;
            if (mRtran.anchoredPosition3D.y < -500)
            {
                break;
            }
        }

        Vector3 pos0 = mResetPos;
        Vector3 pos1 = pos0 - new Vector3(0, 300, 0);
        for (float i = 0; i < 1f; i += 0.04f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos1, pos0, i) + new Vector3(0, Mathf.Sin(Mathf.PI * i) * 250, 0);
            yield return new WaitForSeconds(0.01f);

        }
        mRtran.anchoredPosition3D = pos0;
        mResetPos = pos0;

    }

    public void PutInBox()
    {
        StartCoroutine("TPutInBox");
    }
    IEnumerator TPutInBox()
    {
        SetBoxEnable(false);
        StopCoroutine("TShake");
        Vector3 pos0 = mRtran.anchoredPosition3D;
        Vector3 pos1 = new Vector3( Random.Range(-143, 143), -192, 0);
        for(float i = 0; i < 1f; i += 0.04f)
        {
            mRtran.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.5f, 0.5f, 1), i);
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos1, i) + new Vector3(0, Mathf.Sin(Mathf.PI * i) * 50, 0);
            yield return new WaitForSeconds(0.01f);

        }
        Destroy(gameObject);


    }

}
