using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EffectStartFlying : MonoBehaviour
{
    public RectTransform mRtran { get; set; }
    public Image mStar { get; set; }
    public Image[] mLine { get; set; }
    public Vector2[] mLineSize { get; set; }
    public int mdata_index = 0;
    public System.Action<int> callback = null;


    float[] m_line_speed = new float[3];

	// Use this for initialization
	public void Init (string star_abName, string star_Name, Color32 line_color, int callback_func_param = 0, System.Action<int> callback_func = null)
    {
        callback = callback_func;
        mdata_index = callback_func_param;

        mRtran = gameObject.GetComponent<RectTransform>();
        mLineSize = new Vector2[] { new Vector2(0, 10), new Vector2(0, 10), new Vector2(0, 10) };
        mLine = new Image[3];
        Vector3[] line_pos = new Vector3[] { new Vector3(0, 26, 0), new Vector3(0, -1, 0), new Vector3(0, -28.9f, 0)};

        for(int i = 0; i < 3; i++)
        {
            mLine[i] = UguiMaker.newImage("mLine" + i.ToString(), transform, "public", "white0", false);
            mLine[i].rectTransform.anchoredPosition3D = line_pos[i];
            mLine[i].rectTransform.pivot = new Vector2(1, 0.5f);
            mLine[i].type = Image.Type.Sliced;
            mLine[i].color = line_color;

        }
        mStar = UguiMaker.newImage("mStar", transform, star_abName, star_Name, false);
        mStar.rectTransform.sizeDelta = new Vector2(200, 200);



        m_line_speed[0] = 0.85f;// Random.Range(0.85f, 1f);
        m_line_speed[1] = 0.85f;//Random.Range(0.85f, 1f);
        m_line_speed[2] = 0.85f;//Random.Range(0.85f, 1f);

    }
	

	void Update () {
        if(null != mStar)
        {
            mStar.transform.localEulerAngles += new Vector3(0, 0, 10);
            for(int i = 0; i < mLineSize.Length; i++)
            {
                mLine[i].rectTransform.sizeDelta = mLineSize[i];
                mLineSize[i].x -= 35 * m_line_speed[0];
                if (mLineSize[i].x < 0)
                {
                    mLineSize[i].x = 0;
                }
            }
        }


    }

    public void Fly(Vector3 pos_begin, Vector3 pos_end, float speed)
    {
        StartCoroutine(TFly(pos_begin, pos_end, speed));
    }
    IEnumerator TFly(Vector3 pos_begin, Vector3 pos_end, float speed)
    {
        mRtran.anchoredPosition3D = pos_begin;
        float angle = Vector3.Angle( pos_end - pos_begin, Vector3.right);
        if(0 >  Vector3.Cross(pos_end - pos_begin, Vector3.right).z)
        {
            mRtran.localEulerAngles = new Vector3(0, 0, angle);
        }
        else
        {
            mRtran.localEulerAngles = new Vector3(0, 0, -angle);
        }

        float[] line_speed = new float[3];
        line_speed[0] = 1.8f;// Random.Range(1.75f, 1.82f);
        line_speed[1] = 1.81f;// Random.Range(1.75f, 1.82f);
        line_speed[2] = 1.8f;// Random.Range(1.75f, 1.82f);

        while (true)
        {
            Vector3 dir = (pos_end - mRtran.anchoredPosition3D).normalized * speed;
            dir.z = 0;
            mRtran.anchoredPosition3D += dir;

            for(int i = 0; i < 3; i++)
            {
                mLineSize[i].x += speed * line_speed[i];
            }

            yield return new WaitForSeconds(0.01f);

            if(Vector3.Distance(mRtran.anchoredPosition3D, pos_end) < speed)
            {
                break;
            }
        }
        mRtran.anchoredPosition3D = pos_end;
        //Debug.LogError(pos_end);

        bool line_result = true;
        while(true)
        {
            line_result = true;
            for(int i = 0; i < mLine.Length; i++)
            {
                if (mLine[i].rectTransform.sizeDelta.x > 10)
                    line_result = false;
            }
            if (line_result)
                break;

            yield return new WaitForSeconds(0.01f);

        }

        for (int i = 0; i < mLine.Length; i++)
            mLine[i].gameObject.SetActive(false);

        for (float i = 0; i < 1f; i += 0.05f)
        {
            float p = Mathf.Sin(Mathf.PI * i) + (1 - i);
            mRtran.localScale = new Vector3(p, p, 1);
            yield return new WaitForSeconds(0.01f);
        }

        if(null != callback)
        {
            callback(mdata_index);
        }

        Destroy(gameObject);
        


    }


}
