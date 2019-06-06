using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka7_Station : MonoBehaviour
{
    public static ShapeLogicGuanka7_Station gSelect { get; set; }
    public RectTransform mRtran { get; set; }
    Image mBg, mShape;
    List<Image> mPeople;
    BoxCollider mBox { get; set; }
    public AudioSource mCom { get; set; }
    public Vector3 mResetPos = Vector3.zero;

    List<Vector3> mResetPeoplePos = new List<Vector3>();

    public int mdata_shape_type = 0;
    public int mdata_people_num = 0;

    public bool mdata_is_question = false;
    public bool mdata_is_over = false;

    public void Init(int shape_type, int people_num)
    {
        mdata_shape_type = shape_type;
        mdata_people_num = people_num;

        mRtran = gameObject.GetComponent<RectTransform>();
        mBox = gameObject.AddComponent<BoxCollider>();
        mBox.size = new Vector3(100, 145.53f, 1);
        mBox.center = new Vector3(0, 16, 0);

        mBg = UguiMaker.newImage("mBg", transform, "shapelogic_sprite", "guanka7_stationbg0", false);
        mBg.rectTransform.sizeDelta = new Vector2(90, 110);
        mBg.type = Image.Type.Sliced;


        mShape = UguiMaker.newImage("mShape", transform, "shapelogic_sprite", "guanka7_station" + shape_type.ToString(), false);
        mShape.rectTransform.anchoredPosition = new Vector2(0, 28.4f);

        mPeople = new List<Image>();
        List<Vector3> poss = new List<Vector3>();//Common.PosSortByWidth(200, people_num, -37.2f);
        switch(people_num)
        {
            case 1:
                poss.Add(new Vector3(0, -27.2f, 0));
                break;
            case 2:
                poss.Add(new Vector3(-15, -27.2f, 0));
                poss.Add(new Vector3(15, -27.2f, 0));
                break;
            case 3:
                poss.Add(new Vector3(-30, -27.2f, 0));
                poss.Add(new Vector3(0, -27.2f, 0));
                poss.Add(new Vector3(30, -27.2f, 0));
                break;
            case 4:
                poss.Add(new Vector3(-15, -27.2f, 0));
                poss.Add(new Vector3(15, -27.2f, 0));
                poss.Add(new Vector3(-45, -27.2f, 0));
                poss.Add(new Vector3(45, -27.2f, 0));
                break;
            case 5:
                poss.Add(new Vector3(-30, -27.2f, 0));
                poss.Add(new Vector3(0, -27.2f, 0));
                poss.Add(new Vector3(30, -27.2f, 0));
                poss.Add(new Vector3(-60, -27.2f, 0));
                poss.Add(new Vector3(60, -27.2f, 0));
                break;

        }
        bool up = Random.Range(0, 1000) % 2 == 0;
        if (4 <= people_num)
            up = true;
        for(int i = 0; i < people_num; i++)
        {
            Image img = UguiMaker.newImage("mPeople" + i.ToString(), transform, "shapelogic_sprite", "guanka7_station_people", false);
            if(up)
            {
                img.rectTransform.anchoredPosition3D = poss[i] + new Vector3(0, 107.2f, 0);
            }
            else
            {
                img.rectTransform.anchoredPosition3D = poss[i];
            }
            mPeople.Add(img);
        }


        for (int i = 0; i < mPeople.Count; i++)
        {
            mResetPeoplePos.Add(mPeople[i].rectTransform.anchoredPosition3D);
        }

    }
    public void SetQuestion()
    {
        mdata_is_question = true;
    }
    public void SetAnswer()
    {
        mdata_is_question = false;
    }
    public void SetBoxEnable(bool _enable)
    {
        mBox.enabled = _enable;

    }
    public void SetResetPos(Vector3 pos)
    {
        mResetPos = pos;
    }
    public void ShowCorrect()
    {
        mShape.gameObject.SetActive(true);
        for (int i = 0; i < mPeople.Count; i++)
        {
            mPeople[i].gameObject.SetActive(true);
        }
        mBg.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka7_stationbg0");

    }
    public void UnSelect()
    {
        StopCoroutine("TSelect");

        for (int i = 0; i < mPeople.Count; i++)
        {
            mPeople[i].rectTransform.anchoredPosition3D = mResetPeoplePos[i];
        }
    }

    public void ShowAnswer()
    {

    }
    public void ShowQuestionDefault()
    {
        StartCoroutine(TShowQuestionDefault());
    }
    IEnumerator TShowQuestionDefault()
    {
        Vector3 dir_cen = (mRtran.anchoredPosition3D - Vector3.zero).normalized;

        Vector3 pos_bg0 = mBg.rectTransform.anchoredPosition3D;
        Vector3 pos_bg1 = pos_bg0 + dir_cen * 800;

        Vector3 pos_shape0 = mShape.rectTransform.anchoredPosition3D;
        Vector3 pos_shape1 = pos_shape0 + (dir_cen + mShape.rectTransform.anchoredPosition3D).normalized * 800;

        List<Vector3> pos_peoples0 = new List<Vector3>();
        List<Vector3> pos_peoples1 = new List<Vector3>();
        for(int i = 0; i < mPeople.Count; i++)
        {
            pos_peoples0.Add(mPeople[i].rectTransform.anchoredPosition3D);
            pos_peoples1.Add(pos_peoples0[i] + (dir_cen + mPeople[i].rectTransform.anchoredPosition3D).normalized * 800);
        }

        Vector3 angle0 = new Vector3(0, 0, -360);
        Vector3 angle1 = Vector3.zero;
        for(float i = 0; i < 1f; i += 0.04f)
        {
            Vector3 angle = Vector3.Lerp(angle0, angle1, i);
            mBg.rectTransform.anchoredPosition3D = Vector3.Lerp(pos_bg1, pos_bg0, i);
            mBg.rectTransform.localEulerAngles = angle;

            mShape.rectTransform.anchoredPosition3D = Vector3.Lerp(pos_shape1, pos_shape0, i);
            mShape.rectTransform.localEulerAngles = angle;

            for(int j = 0; j < mPeople.Count; j++)
            {
                mPeople[j].rectTransform.anchoredPosition3D = Vector3.Lerp(pos_peoples1[j], pos_peoples0[j], i);
                mPeople[j].rectTransform.localEulerAngles = angle;
            }

            yield return new WaitForSeconds(0.01f);
        }

        mBg.rectTransform.anchoredPosition3D = pos_bg0;
        mBg.rectTransform.localEulerAngles = Vector3.zero;
        mShape.rectTransform.anchoredPosition3D = pos_shape0;
        mShape.rectTransform.localEulerAngles = Vector3.zero;
        for (int j = 0; j < mPeople.Count; j++)
        {
            mPeople[j].rectTransform.anchoredPosition3D = pos_peoples0[j];
            mPeople[j].rectTransform.localEulerAngles = Vector3.zero;
        }

    }
    public void ShowQuestionPlace()
    {
        mShape.gameObject.SetActive(false);
        for(int i = 0; i < mPeople.Count; i++)
        {
            mPeople[i].gameObject.SetActive(false);
        }
        mBg.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka7_stationbg1");

        StartCoroutine(TShowQuestionPlace());
    }
    IEnumerator TShowQuestionPlace()
    {
        Vector3 dir_cen = (mRtran.anchoredPosition3D - Vector3.zero).normalized;

        Vector3 pos_bg0 = mBg.rectTransform.anchoredPosition3D;
        Vector3 pos_bg1 = pos_bg0 + dir_cen * 800;
        Vector3 angle0 = new Vector3(0, 0, -360);
        Vector3 angle1 = Vector3.zero;

        for (float i = 0; i < 1f; i += 0.04f)
        {
            Vector3 angle = Vector3.Lerp(angle0, angle1, i);
            mBg.rectTransform.anchoredPosition3D = Vector3.Lerp(pos_bg1, pos_bg0, i);
            mBg.rectTransform.localEulerAngles = angle;
            yield return new WaitForSeconds(0.01f);
        }

        mBg.rectTransform.anchoredPosition3D = pos_bg0;
        mBg.rectTransform.localEulerAngles = Vector3.zero;

        yield break;
       

    }


    public void ResetPos()
    {
        StopCoroutine("TShake");
        mRtran.localEulerAngles = Vector3.zero;
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


    public void Select()
    {
        if (null == mCom)
        {
            mCom = gameObject.AddComponent<AudioSource>();
            mCom.clip = ResManager.GetClip("shapelogic_sound", "脚步声");

        }
        //Debug.Log("Select");
        transform.SetAsLastSibling();
        StartCoroutine("TSelect");
    }
    IEnumerator TSelect()
    {
        //Debug.Log("TSelect");
        while (true)
        {
            for(int i = 0; i < mPeople.Count; i++)
            {
                mCom.Play();
                for (float j = 0; j < 1f; j += 0.1f)
                {
                    mPeople[i].rectTransform.anchoredPosition3D = mResetPeoplePos[i] + new Vector3(0, Mathf.Sin(Mathf.PI * j) * 30, 0);
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }
    }

    public void PlayError()
    {
        StartCoroutine("TPlayError");
    }
    IEnumerator TPlayError()
    {
        SetBoxEnable(false);
        Vector3 dir = Vector3.zero - mRtran.anchoredPosition3D;
        dir.x *= -1;
        dir.y *= 0.5f;
        Vector3 pos0 = mRtran.anchoredPosition3D + dir.normalized * 600;
        Vector3 pos1 = mRtran.anchoredPosition3D;
        Vector3 angle0 = new Vector3(0, 0, -360);
        for(float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos1, pos0, i) + new Vector3(0, Mathf.Sin(Mathf.PI * i) * 200, 0);
            mRtran.localEulerAngles = Vector3.Lerp(angle0, Vector3.zero, i);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.localEulerAngles = Vector3.zero;
        pos0 = mResetPos + new Vector3(0, -250, 0);
        for (float i = 0; i < 1f; i += 0.08f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, mResetPos, i) + new Vector3(0, Mathf.Sin(Mathf.PI * i) * 200, 0);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition3D = mResetPos;

        SetBoxEnable(true);


    }

    public void ShootOver()
    {
        mdata_is_over = true;
        StopAllCoroutines();
        StartCoroutine("TShootOver");
    }
    IEnumerator TShootOver()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("素材出去通用");
        Vector3 dir_cen = (mRtran.anchoredPosition3D - Vector3.zero).normalized;

        Vector3 pos_bg0 = mBg.rectTransform.anchoredPosition3D;
        Vector3 pos_bg1 = pos_bg0 + dir_cen * 800;

        Vector3 pos_shape0 = mShape.rectTransform.anchoredPosition3D;
        Vector3 pos_shape1 = pos_shape0 + (dir_cen + mShape.rectTransform.anchoredPosition3D).normalized * 800;

        List<Vector3> pos_peoples0 = new List<Vector3>();
        List<Vector3> pos_peoples1 = new List<Vector3>();
        for (int i = 0; i < mPeople.Count; i++)
        {
            pos_peoples0.Add(mPeople[i].rectTransform.anchoredPosition3D);
            pos_peoples1.Add(pos_peoples0[i] + (dir_cen + mPeople[i].rectTransform.anchoredPosition3D).normalized * 800);
        }

        Vector3 angle0 = new Vector3(0, 0, -360);
        Vector3 angle1 = Vector3.zero;
        for (float i = 0; i < 1f; i += 0.04f)
        {
            Vector3 angle = Vector3.Lerp(angle0, angle1, i);
            mBg.rectTransform.anchoredPosition3D = Vector3.Lerp(pos_bg0, pos_bg1, i);
            mBg.rectTransform.localEulerAngles = angle;

            mShape.rectTransform.anchoredPosition3D = Vector3.Lerp(pos_shape0, pos_shape1, i);
            mShape.rectTransform.localEulerAngles = angle;

            for (int j = 0; j < mPeople.Count; j++)
            {
                mPeople[j].rectTransform.anchoredPosition3D = Vector3.Lerp(pos_peoples0[j], pos_peoples1[j], i);
                mPeople[j].rectTransform.localEulerAngles = angle;
            }

            yield return new WaitForSeconds(0.01f);
        }

        gameObject.SetActive(false);

    }

}
