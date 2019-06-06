using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka4_Station : MonoBehaviour {
    public static ShapeLogicGuanka4_Station gSelect { get; set; }

    public RectTransform mRtran { get; set; }
    Image mMachine0, mMachine1, mMachine2;
    Image mBg;
    Image mOver0, mOver1;
    Image[] mPicture;
    BoxCollider mBox { get; set; }
    Vector3 mResetPos = Vector3.zero;

    public List<int> mdata_ids = new List<int>();

    public bool mdata_can_show = true;
    public bool mdata_is_over = false;



    void Start () {
	
	}
	
    public void InitQuestion(List<int> ids)
    {
        mdata_is_over = false;
        mdata_ids = ids;
        mRtran = gameObject.GetComponent<RectTransform>();
        mBox = gameObject.AddComponent<BoxCollider>();
        mBox.size = new Vector3(216.71f, 300, 1);

    }
    public void InitAnswer(List<int> ids)
    {
        mdata_is_over = false;
        mdata_ids = ids;
        mRtran = gameObject.GetComponent<RectTransform>();
        mBox = gameObject.AddComponent<BoxCollider>();
        mBox = gameObject.AddComponent<BoxCollider>();
        mBox.size = new Vector3(216.71f, 300, 1);
        SetCanShow(true);
        InitBg(ids);
        mRtran.localScale = new Vector3(0.85f, 0.85f, 1);
    }
    public void InitBg(List<int> ids)
    {
        mdata_ids = ids;
        List<Vector2> poss = new List<Vector2>() {
            new Vector2(-56, 82), new Vector2(56, 82),
            new Vector2(-56, 0), new Vector2(56, 2.8f),
            new Vector2(-56, -81.74f), new Vector2(56, -81.74f),

        };
        if (null == mBg)
        {
            mBg = UguiMaker.newImage("mBg", transform, "shapelogic_sprite", "guanka4_stationbg", false);
            mBg.transform.SetSiblingIndex(1);
            mBg.transform.localScale = new Vector3(0.9f, 0.9f, 1);
        }
        if(mdata_can_show)
        {
            if (null == mPicture)
            {
                mPicture = new Image[6];
                for (int i = 0; i < mdata_ids.Count; i++)
                {
                    mPicture[i] = UguiMaker.newImage("mPicture" + i.ToString(), mBg.transform, "shapelogic_sprite", "guanka4_type" + ids[i].ToString(), false);
                    mPicture[i].rectTransform.anchoredPosition = poss[i];
                }
            }
            else
            {
                for (int i = 0; i < mdata_ids.Count; i++)
                {
                    mPicture[i].sprite = ResManager.GetSprite("shapelogic_sprite", "guanka4_type" + ids[i].ToString());
                }
            }


        }

    }
    public void SetCanShow(bool can_show)
    {
        mdata_can_show = can_show;
    }
    public void SetBoxEnable(bool _enable)
    {
        mBox.enabled = _enable;
    }
    public void SetResetPos(Vector3 reset_pos)
    {
        mResetPos = reset_pos;
    }
    public void Select()
    {
        mRtran.localScale = Vector3.one;
        transform.SetAsLastSibling();
        Shake();
    }
    public void UnSelect()
    {
        StopCoroutine("TShake");
        mRtran.localEulerAngles = Vector3.zero;
        mRtran.localScale = new Vector3(0.85f, 0.85f, 1);

    }

    public void PlayInitQuestion()
    {
        StartCoroutine("TPlayInitQuestion");
    }
    IEnumerator TPlayInitQuestion()
    {
        float machine_speed = 0.022f;
        Vector2 pos_up = new Vector3(0, 160, 0);
        Vector2 pos_down = new Vector3(0, -160, 0);

        mMachine0 = UguiMaker.newImage("mMachine0", transform, "shapelogic_sprite", "guanka4_machine0", false);
        GameObject line0 = UguiMaker.newGameObject("line0", transform);
        GameObject line1 = UguiMaker.newGameObject("line1", transform);
        line0.transform.localPosition = pos_up;
        line1.transform.localPosition = pos_down;
        mMachine1 = UguiMaker.newImage("mMachine1", transform, "shapelogic_sprite", "guanka4_machine1", false);
        mMachine2 = UguiMaker.newImage("mMachine2", transform, "shapelogic_sprite", "guanka4_machine2", false);
        mMachine0.color = new Color32(255, 255, 255, 0);
        mMachine2.rectTransform.anchoredPosition3D = pos_up;

        mRtran.localScale = Vector3.one; for (float i = 0; i < 1f; i += 0.08f)
        {
            float p = Mathf.Sin(Mathf.PI * i) * 0.5f;
            mRtran.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i) + new Vector3(p, p, 0);
            yield return new WaitForSeconds(0.01f);
        }
        for(float i = 0; i < 1f; i += 0.05f)
        {
            mMachine0.color = Color32.Lerp(new Color32(255, 255, 255, 0), Color.white, i);
            yield return new WaitForSeconds(0.01f);
        }
        mMachine0.color = Color.white;

        float line_width = 6;
        int index_up = 0;
        int index_down = 0;
        int line_num = 6;
        List<Image> lines0 = new List<Image>();
        List<Vector3> poss = Common.PosSortByWidth(250, line_num, 0);


        for(int i = 0; i < line_num; i++)
        {
            Image l = UguiMaker.newImage(i.ToString(), line0.transform, "public", "white", false);
            l.type = Image.Type.Sliced;
            l.rectTransform.localEulerAngles = new Vector3(0, 0, -90);
            l.rectTransform.pivot = new Vector2(0, 0.5f);
            l.rectTransform.anchoredPosition3D = poss[i];
            if(0 == Random.Range(0, 1000) % 2)
            {
                l.color = new Color32(255, 84, 150, 255);
            }
            else
            {
                l.color = new Color32(54, 241, 255, 255);
            }

            lines0.Add(l);

        }
        if(!ShapeLogicCtl.instance.mSound.IsPlayingOnly())
        {
            ShapeLogicCtl.instance.mSound.PlayOnly("shapelogic_sound", "机器人移动短");
        }
        for(float i = 0; i < 1f; i += machine_speed)
        {
            mMachine2.rectTransform.anchoredPosition = Vector2.Lerp(pos_up, pos_down, Mathf.Sin(Mathf.PI * 0.5f * i));
            for(int j = 0; j < lines0.Count; j++)
            {
                lines0[j].rectTransform.sizeDelta = new Vector2(pos_up.y - mMachine2.rectTransform.anchoredPosition.y, line_width);
            }
            yield return new WaitForSeconds(0.01f);
        }
        //
        line_num = 5;
        List<Image> lines1 = new List<Image>();
        poss = Common.PosSortByWidth(210, line_num, 0);
        for (int i = 0; i < line_num; i++)
        {
            Image l = UguiMaker.newImage(i.ToString(), line1.transform, "public", "white", false);
            l.type = Image.Type.Sliced;
            l.rectTransform.localEulerAngles = new Vector3(0, 0, 90);
            l.rectTransform.pivot = new Vector2(0, 0.5f);
            l.rectTransform.anchoredPosition3D = poss[i];
            if (0 == Random.Range(0, 1000) % 2)
            {
                l.color = new Color32(255, 84, 150, 255);
            }
            else
            {
                l.color = new Color32(54, 241, 255, 255);
            }

            lines1.Add(l);

        }
        if (!ShapeLogicCtl.instance.mSound.IsPlayingOnly())
        {
            ShapeLogicCtl.instance.mSound.PlayOnly("shapelogic_sound", "机器人移动短");
        }
        for (float i = 0; i < 1f; i += machine_speed)
        {
            mMachine2.rectTransform.anchoredPosition = Vector2.Lerp(pos_down, pos_up, Mathf.Sin(Mathf.PI * 0.5f * i));
            for (int j = 0; j < lines1.Count; j++)
            {
                lines1[j].rectTransform.sizeDelta = new Vector2(mMachine2.rectTransform.anchoredPosition.y - pos_down.y, line_width);
            }
            yield return new WaitForSeconds(0.01f);
        }
        //
        //yield break;
        index_up = 6;
        line_num = 10;
        poss = Common.PosSortByWidth(210, line_num, 0);
        for (int i = 0; i < line_num; i++)
        {
            Image l = UguiMaker.newImage(i.ToString(), line0.transform, "public", "white", false);
            l.type = Image.Type.Sliced;
            l.rectTransform.localEulerAngles = new Vector3(0, 0, -90);
            l.rectTransform.pivot = new Vector2(0, 0.5f);
            l.rectTransform.anchoredPosition3D = poss[i];
            if (0 == Random.Range(0, 1000) % 2)
            {
                l.color = new Color32(255, 84, 150, 255);
            }
            else
            {
                l.color = new Color32(54, 241, 255, 255);
            }
            //l.color = new Color(0, 0, 0, 1); 

            lines0.Add(l);

        }
        if (!ShapeLogicCtl.instance.mSound.IsPlayingOnly())
        {
            ShapeLogicCtl.instance.mSound.PlayOnly("shapelogic_sound", "机器人移动短");
        }
        for (float i = 0; i < 1f; i += machine_speed)
        {
            mMachine2.rectTransform.anchoredPosition = Vector2.Lerp(pos_up, pos_down, Mathf.Sin(Mathf.PI * 0.5f * i));
            for (int j = index_up; j < lines0.Count; j++)
            {
                lines0[j].rectTransform.sizeDelta = new Vector2(pos_up.y - mMachine2.rectTransform.anchoredPosition.y, line_width);
            }
            yield return new WaitForSeconds(0.01f);
        }
        //
        index_down = 5;
        line_num = 20;
        poss = Common.PosSortByWidth(210, line_num, 0);
        for (int i = 0; i < line_num; i++)
        {
            Image l = UguiMaker.newImage(i.ToString(), line1.transform, "public", "white", false);
            l.type = Image.Type.Sliced;
            l.rectTransform.localEulerAngles = new Vector3(0, 0, 90);
            l.rectTransform.pivot = new Vector2(0, 0.5f);
            l.rectTransform.anchoredPosition3D = poss[i];
            if (0 == Random.Range(0, 1000) % 2)
            {
                l.color = new Color32(255, 84, 150, 255);
            }
            else
            {
                l.color = new Color32(54, 241, 255, 255);
            }
            //l.color = new Color(0, 0, 0, 1);

            lines1.Add(l);

        }
        if (!ShapeLogicCtl.instance.mSound.IsPlayingOnly())
        {
            ShapeLogicCtl.instance.mSound.PlayOnly("shapelogic_sound", "机器人移动短");
        }
        for (float i = 0; i < 1f; i += machine_speed)
        {
            mMachine2.rectTransform.anchoredPosition = Vector2.Lerp(pos_down, pos_up, Mathf.Sin(Mathf.PI * 0.5f * i));
            for (int j = index_down; j < lines1.Count; j++)
            {
                lines1[j].rectTransform.sizeDelta = new Vector2(mMachine2.rectTransform.anchoredPosition.y - pos_down.y, line_width);
            }
            yield return new WaitForSeconds(0.01f);
        }
        InitBg(mdata_ids);

        for(int i = 0; i < lines0.Count; i++)
        {
            lines0[i].transform.SetParent(line1.transform);
            lines0[i].transform.SetAsFirstSibling();
            lines0[i].gameObject.name = "up";
            lines0[i].rectTransform.localEulerAngles = new Vector3(0, 0, 90);
            lines0[i].rectTransform.anchoredPosition3D = new Vector3(lines0[i].rectTransform.anchoredPosition3D.x, 0, 0);
        }
        //yield break;
        if (!ShapeLogicCtl.instance.mSound.IsPlayingOnly())
        {
            ShapeLogicCtl.instance.mSound.PlayOnly("shapelogic_sound", "机器人移动短");
        }
        for (float i = 0; i < 1f; i += machine_speed)
        {
            mMachine2.rectTransform.anchoredPosition = Vector2.Lerp(pos_up, pos_down, Mathf.Sin(Mathf.PI * 0.5f * i));
            for (int j = 0; j < lines0.Count; j++)
            {
                lines0[j].rectTransform.sizeDelta = new Vector2( mMachine2.rectTransform.anchoredPosition.y - pos_down.y, line_width);
            }
            for (int j = 0; j < lines1.Count; j++)
            {
                lines1[j].rectTransform.sizeDelta = new Vector2(mMachine2.rectTransform.anchoredPosition.y - pos_down.y, line_width);
            }
            yield return new WaitForSeconds(0.01f);
        }

        Destroy(line0.gameObject);
        Destroy(line1.gameObject);
        lines0.Clear();
        lines1.Clear();

        mMachine2.transform.SetParent(mMachine1.transform);
        //mMachine1.transform.SetSiblingIndex(1);
        Vector3 temp_pos = mMachine1.rectTransform.anchoredPosition3D;
        for(float i = 0; i < 1f; i += 0.05f)
        {
            Vector3 pos = Vector3.Lerp(temp_pos, new Vector3(0, -600, 0), Mathf.Sin(Mathf.PI * 0.5f * i));
            mMachine1.rectTransform.anchoredPosition3D = pos;
            mMachine0.rectTransform.anchoredPosition3D = pos;

            yield return new WaitForSeconds(0.01f);
        }
        Destroy(mMachine0.gameObject);
        Destroy(mMachine2.gameObject);
        Destroy(mMachine1.gameObject);
        mMachine0 = null;
        mMachine1 = null;
        mMachine2 = null;

        ShapeLogicCtl.instance.mGuankaCtl4.callback_ShowAnswer();

        ResetPos();
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

    public void PlayError()
    {
        StartCoroutine("TPlayError");
    }
    IEnumerator TPlayError()
    {
        SetBoxEnable(false);
        while(true)
        {
            mRtran.anchoredPosition3D += new Vector3(0, 40, 0);
            if( 500 < mRtran.anchoredPosition3D.y)
            {
                break;
            }
            yield return new WaitForSeconds(0.01f);
            
        }

        Vector3 pos0 = new Vector3(mResetPos.x, -500, 0);
        for (float i = 0; i < 1f; i += 0.05f)
        {

            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, mResetPos, i) + new Vector3(0, Mathf.Sin(Mathf.PI * i) * 200, 0);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition3D = mResetPos;

        SetBoxEnable(true);
    }

    public void Shake()
    {
        StartCoroutine("TShake");
    }
    IEnumerator TShake()
    {
        float p = 0;
        while (true)
        {
            mRtran.localEulerAngles = new Vector3(0, 0, Mathf.Sin(p) * 3 + 30);
            p += 0.2f;
            yield return new WaitForSeconds(0.01f);
        }


    }

    public void ShowOver()
    {
        StartCoroutine("TShowOver");
    }
    IEnumerator TShowOver()
    {
        mOver0 = UguiMaker.newImage("mOver0", transform, "shapelogic_sprite", "guanka4_over0", false);
        Mask mask = UguiMaker.newImage("Mask", transform, "public", "white", false).gameObject.AddComponent<Mask>();
        mask.showMaskGraphic = false;
        mask.rectTransform.sizeDelta = new Vector2(262, 353.1f);
        mBg.transform.SetParent(mask.transform);
        mOver1 = UguiMaker.newImage("mOver1", transform, "shapelogic_sprite", "guanka4_over1", false);
        mOver0.transform.SetAsFirstSibling();
        mOver1.transform.SetAsLastSibling();
        mOver0.rectTransform.anchoredPosition = new Vector2(0, 172);
        mOver1.rectTransform.anchoredPosition = new Vector2(0, 172);
        for(float i = 0; i < 1f; i += 0.08f)
        {
            mOver0.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(0, 38), new Vector2(269, 38), i);
            mOver1.rectTransform.sizeDelta = mOver0.rectTransform.sizeDelta;

            yield return new WaitForSeconds(0.01f);
        }
        mOver0.rectTransform.sizeDelta = new Vector2(269, 38);
        mOver1.rectTransform.sizeDelta = mOver0.rectTransform.sizeDelta;
    }

    public void ShootOver()
    {
        mdata_is_over = true;
        StartCoroutine("TShootOver");
    }
    IEnumerator TShootOver()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("素材出去通用");
        while(true)
        {
            mBg.rectTransform.anchoredPosition3D += new Vector3(0, 40, 0);
            yield return new WaitForSeconds(0.01f);
            if(mBg.rectTransform.anchoredPosition3D.y > 500)
            {
                break;
            }
        }

        for (float i = 0; i < 1f; i += 0.1f)
        {
            mOver0.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(269, 38), new Vector2(0, 38), i);
            mOver1.rectTransform.sizeDelta = mOver0.rectTransform.sizeDelta;

            yield return new WaitForSeconds(0.01f);
        }
        Destroy(mOver0.gameObject);
        Destroy(mOver1.gameObject);
        mOver0 = null;
        mOver1 = null;

    }
    

}
