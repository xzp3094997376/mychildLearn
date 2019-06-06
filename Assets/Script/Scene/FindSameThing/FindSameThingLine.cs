using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FindSameThingLine : MonoBehaviour {
    public static FindSameThingLine gSelect;

    public RectTransform mRtran, mRtranLine;
    public Image mHead, mTail;
    public List<Image> mLines = new List<Image>();

    Vector3 mEndToPos = Vector3.zero;
    int line_num = 40;//要是4的倍数
    int line_width = 25;
    int[] line_angle = new int[] { -1, 1, 1, -1};


    void Start () {
	
	}
	
	// Update is called once per frame
	public void UpdateLine (Vector3 to_pos)
    {
        float dis = Vector3.Distance(mRtran.anchoredPosition3D, to_pos);
        float dis_sub = dis / line_num;
        float dis_h = Mathf.Sqrt(line_width * line_width - dis_sub * dis_sub);

        Vector3 dir = to_pos - mRtran.anchoredPosition3D;
        float angle = Vector3.Angle( dir, Vector3.right);
        if(Vector3.Cross(dir, Vector3.right).z > 0)
        {
            mRtran.localEulerAngles = new Vector3(0, 0, -angle);
        }
        else
        {
            mRtran.localEulerAngles = new Vector3(0, 0, angle);
        }

        if (dis_sub > 24)
            return;

        angle = Vector3.Angle(new Vector3(dis_sub, dis_h, 0), Vector3.right);
        //Debug.Log(dis_sub + "  " + angle);
        for(int i = 0; i < mLines.Count; i++)
        {
            mLines[i].rectTransform.eulerAngles = new Vector3(0, 0, angle * line_angle[i % 4] + mRtran.eulerAngles.z);
        }

        mEndToPos = to_pos;





    }

    public void Init()
    {
        if (null == mRtran)
        {
            mRtran = gameObject.GetComponent<RectTransform>();
            mRtranLine = UguiMaker.newGameObject("mRtranLine", transform).GetComponent<RectTransform>();
            mHead = UguiMaker.newImage("mHead", transform, "findsamething_sprite", "line_anchor", false);
            mTail = UguiMaker.newImage("mTail", transform, "findsamething_sprite", "line_anchor", false);
            mRtranLine.sizeDelta = Vector2.zero;
            //mRtranLine.localScale = new Vector3(1, 0.5f, 1);

            for (int i = 0; i < line_num; i++)
            {
                if(i == 0)
                    mLines.Add( UguiMaker.newImage(i.ToString(), mRtranLine, "findsamething_sprite", "line", false));
                else
                    mLines.Add(UguiMaker.newImage(i.ToString(), mLines[i - 1].rectTransform, "findsamething_sprite", "line", false));

                //mLines[i].rectTransform.sizeDelta = new Vector2(line_width, 9);
                mLines[i].rectTransform.pivot = new Vector2(0, 0.5f);
                mLines[i].rectTransform.anchorMin = new Vector2(1, 0.5f);
                mLines[i].rectTransform.anchorMax = new Vector2(1, 0.5f);
                //mLines[i].rectTransform.anchoredPosition3D = new Vector3(0, 7, 0);


                Image anchor = UguiMaker.newImage("anchor", mLines[i].transform, "findsamething_sprite", "line_anchor1", false);
                anchor.rectTransform.anchorMin = new Vector2(0, 0.5f);
                anchor.rectTransform.anchorMax = new Vector2(0, 0.5f);
                anchor.rectTransform.anchoredPosition = Vector2.zero;
                //anchor.color = new Color(1, 1, 1, 0);

            }
            mTail.transform.SetParent(mLines[mLines.Count - 1].transform);
            mTail.rectTransform.anchorMin = new Vector2(1, 0.5f);
            mTail.rectTransform.anchorMax = new Vector2(1, 0.5f);
            mTail.rectTransform.anchoredPosition = Vector2.zero;

        }


    }

    public void ToError()
    {
        StartCoroutine("TToError");
    }
    IEnumerator TToError()
    {
        Vector3 speed = (mRtran.anchoredPosition3D - mEndToPos).normalized * 50;

        while (true)
        {
            UpdateLine(mEndToPos + speed);
            yield return new WaitForSeconds(0.01f);
            if (50 > Vector3.Distance(mRtran.anchoredPosition3D, mEndToPos))
                break;

        }

        //yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        FindSameThingCtl.instance.callback_PlayError(this);
        
    }

}
