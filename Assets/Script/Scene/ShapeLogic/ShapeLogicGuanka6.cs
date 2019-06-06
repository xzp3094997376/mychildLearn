using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka6 : MonoBehaviour
{
    Image mBg0 { get; set; }
    Image mBgAnswer;
    Image[] mBgTiao { get; set; }

    List<ShapeLogicGuanka6_Station> mStationsQuestion;
    List<ShapeLogicGuanka6_Station> mStationsAnswer;

    int mdata_answer_index = 0;//选那个当问题
    int temp_updata_state = 0;

    // Use this for initialization
    void Start () {
	
	}

    void Update()
    {
        switch (temp_updata_state)
        {
            case 1:
                UpdatePlay();
                break;
            case 2:
                UpdateOver();
                break;

        }
        if(null != mBgTiao)
        {
            for(int i = 0; i < mBgTiao.Length; i++)
            {
                mBgTiao[i].rectTransform.anchoredPosition3D += new Vector3(0.5f, 0, 0);
                if (mBgTiao[i].rectTransform.anchoredPosition3D.x > 1707)
                {
                    mBgTiao[i].rectTransform.anchoredPosition3D = new Vector3(0, mBgTiao[i].rectTransform.anchoredPosition3D.y, 0);
                }
            }
        }

    }
    void UpdatePlay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach (RaycastHit h in hits)
            {
                ShapeLogicGuanka6_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka6_Station>();
                if (null != com && !com.mdata_is_question)
                {
                    ShapeLogicGuanka6_Station.gSelect = com;
                    ShapeLogicGuanka6_Station.gSelect.Select();

                    ShapeLogicCtl.instance.mSound.PlayOnlyDefaultAb("星星闪烁2");
                    ShapeLogicCtl.instance.mSound.PlayShort("select0");

                    break;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (null != ShapeLogicGuanka6_Station.gSelect)
            {
                Vector3 pos = Common.getMouseLocalPos(transform);
                pos.z = 0;
                ShapeLogicGuanka6_Station.gSelect.mRtran.anchoredPosition3D = pos;


            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (null != ShapeLogicGuanka6_Station.gSelect)
            {
                RaycastHit[] hits = Common.getMouseRayHits();
                ShapeLogicGuanka6_Station com = null;
                foreach (RaycastHit h in hits)
                {
                    ShapeLogicGuanka6_Station temp = h.collider.gameObject.GetComponent<ShapeLogicGuanka6_Station>();
                    if (null != temp && temp.mdata_is_question)
                    {
                        com = temp;
                        break;
                    }
                }

                if (null != com)
                {
                    bool is_correct = com.mdata_num == ShapeLogicGuanka6_Station.gSelect.mdata_num;
                    if (is_correct)
                    {
                        ShapeLogicCtl.instance.mSound.PlayShort("select1");
                        //回答正确
                        ShapeLogicGuanka6_Station.gSelect.gameObject.SetActive(false);
                        com.ShowStart(true);
                        Debug.LogError("回答正确");
                        StartCoroutine("TOver");
                    }
                    else
                    {
                        ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("掉下来");
                        //回答错误
                        ShapeLogicGuanka6_Station.gSelect.PlayError();
                        ShapeLogicGuanka6_Station.gSelect.UnSelect();
                        Debug.LogError("回答错误");

                    }

                }
                else
                {
                    ShapeLogicCtl.instance.mSound.PlayShort("错误");
                    ShapeLogicGuanka6_Station.gSelect.ResetPos();
                    ShapeLogicGuanka6_Station.gSelect.UnSelect();
                }
                

            }
            ShapeLogicGuanka6_Station.gSelect = null;
        }


    }
    void UpdateOver()
    {
        
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach (RaycastHit h in hits)
            {
                ShapeLogicGuanka6_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka6_Station>();
                if (null != com)
                {
                    com.SetBoxEnable(false);
                    com.ShootOver();
                    bool is_over = true;
                    for (int i = 0; i < mStationsQuestion.Count; i++)
                    {
                        if (!mStationsQuestion[i].mdata_is_over)
                        {
                            is_over = false;
                            break;
                        }
                    }
                    if (is_over)
                    {
                        Invoke("EndGame", 1f);
                    }
                    break;
                }
            }

        }
        
    }

    public void BeginGame()
    {
        gameObject.SetActive(true);
        StartCoroutine(TBeginGame());
    }
    public void EndGame()
    {
        StartCoroutine(TEndGame());
    }
    IEnumerator TBeginGame()
    {
        temp_updata_state = 0;
        bool temp_out_side = true;//记录左上方第一个的星星在外面还是里面
        mdata_answer_index = Random.Range(0, 1000) % 3 + 6;

        //背景
        mBg0 = UguiMaker.newImage("mBg0", transform, "public", "white", false);
        mBg0.color = new Color32(184, 226, 32, 255);
        mBg0.rectTransform.sizeDelta = new Vector2(1423, 800);
        List<Vector3> poss = Common.PosSortByHeight(1050, 4, 0);
        mBgTiao = new Image[4];
        for (int i = 0; i < mBgTiao.Length; i++)
        {
            Image bg = UguiMaker.newImage("mBg" + i, mBg0.transform, "shapelogic_sprite", "guanka3_bg1", false);
            bg.rectTransform.localEulerAngles = new Vector3(0, 0, 0);
            bg.rectTransform.anchoredPosition3D = poss[i];
            bg.rectTransform.sizeDelta = new Vector2(5000, 262);
            bg.type = Image.Type.Tiled;
            bg.color = new Color32(255, 255, 255, 255);
            mBgTiao[i] = bg;
        }
        //ShapeLogicCtl.instance.CreateLine(1100, mBg0.transform, new List<Vector2>() { new Vector2(0, 161), new Vector2(0, -22) });
        
        //初始化
        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        mStationsQuestion = new List<ShapeLogicGuanka6_Station>();
        List<int> temp = new List<int>() { 5, 4, 3, 4, 3, 2, 3, 2, 1};
        poss = Common.PosSortByWidth(1200, 3, 250);
        for (int i = 0; i < 9; i++)
        {
            ShapeLogicGuanka6_Station station = UguiMaker.newGameObject("station" + i.ToString(), transform).AddComponent<ShapeLogicGuanka6_Station>();
            mStationsQuestion.Add(station);
            station.Init(temp[i], temp_out_side);
            station.SetQuestion();
            station.SetResetPos(poss[i % 3] + new Vector3(0, -180 * ( i / 3), 0));
            if (i == mdata_answer_index)
            {
                station.SetBoxEnable(true);
                station.SetBgFrameWhite();
                station.ShowByLittleAnimtion();
                station.ShowStart(false);
            }
            else
            {
                station.SetBoxEnable(false);
                station.ShowByAnimtion();

            }

            temp_out_side = !temp_out_side;

        }

        yield return new WaitForSeconds(2);

        
        //答案
        mBgAnswer = UguiMaker.newImage("mBgAnswer", transform, "shapelogic_sprite", "guanka6_bg", false);
        mBgAnswer.type = Image.Type.Sliced;
        mBgAnswer.rectTransform.sizeDelta = new Vector2(1038, 196);
        poss = Common.PosSortByWidth(1000, 5, 0);
        mStationsAnswer = new List<ShapeLogicGuanka6_Station>();
        for (int i = 0; i < 5; i++)
        {
            ShapeLogicGuanka6_Station station = UguiMaker.newGameObject("station_answer" + i.ToString(), mBgAnswer.transform).AddComponent<ShapeLogicGuanka6_Station>();
            mStationsAnswer.Add(station);
            if (i == mStationsQuestion[mdata_answer_index].mdata_num)
            {
                station.Init(i + 1, mStationsQuestion[mdata_answer_index].mdata_out_side);
            }
            else
            {
                station.Init(i + 1, Random.Range(0, 1000) % 2 == 0);
            }

            station.SetAnswer();
            station.SetBoxEnable(false);
            station.ShowStart(true);
            station.mRtran.anchoredPosition3D = poss[i];

        }


        for (float i = 0; i < 1f; i += 0.04f)
        {
            mBgAnswer.rectTransform.anchoredPosition3D = Vector3.Lerp(new Vector3(0, -511), new Vector3(0, -296), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mBgAnswer.rectTransform.anchoredPosition3D = new Vector3(0, -296);

        for(int i = 0; i < mStationsAnswer.Count; i++)
        {
            mStationsAnswer[i].SetBoxEnable(true);
            mStationsAnswer[i].transform.SetParent(transform);
            mStationsAnswer[i].SetResetPos(mStationsAnswer[i].mRtran.anchoredPosition3D);
        }


        ShapeLogicCtl.instance.mSound.PlayTipListDefaultAb(
            new List<string>() { "6观察长方形里面和外面五角星的变化规律", "拖动正确的图案放到空白的长方形里" },
            new List<float>() { 1, 1 }, true);

        temp_updata_state = 1;
        
    }
    IEnumerator TEndGame()
    {
        yield return new WaitForSeconds(1.5f);
        ShapeLogicCtl.instance.GameNext();
        Color bg_color0 = mBg0.color;
        Color bg_color1 = mBg0.color;
        bg_color1.a = 0;

        for (float j = 0; j < 1f; j += 0.03f)
        {
            Color cor = Color.Lerp(new Color(1, 1, 1, 0.5f), new Color(1, 1, 1, 0), j);
            for (int i = 0; i < mBgTiao.Length; i++)
            {
                mBgTiao[i].color = cor;
            }
            mBg0.color = Color.Lerp(bg_color0, bg_color1, j);
            yield return new WaitForSeconds(0.01f);
        }

        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        gameObject.SetActive(false);
        mBg0 = null;
        mBgAnswer = null;
        mBgTiao = null;
        mStationsAnswer.Clear();
        mStationsQuestion.Clear();

        temp_updata_state = 0;

    }
    IEnumerator TOver()
    {
        //ShapeLogicCtl.instance.DestroyLine();
        yield return new WaitForSeconds(0.8f);
        ShapeLogicCtl.instance.mSound.PlayShort("胜利通关音乐", 1);
        temp_updata_state = 0;
        for (int i = 0; i < mStationsAnswer.Count; i++)
        {
            mStationsAnswer[i].transform.SetParent(mBgAnswer.transform);
        }


        List<Vector3> poss0 = new List<Vector3>();
        List<Vector3> poss1 = new List<Vector3>();
        float[] y = new float[] { 250, 0, -250 };
        for(int i = 0; i < mStationsQuestion.Count; i++)
        {
            poss0.Add(mStationsQuestion[i].mRtran.anchoredPosition3D);
            poss1.Add(new Vector3(poss0[i].x, y[i / 3], 0));

            mStationsQuestion[i].PlayOver();
        }

        for (float i = 0; i < 1f; i += 0.08f)
        {
            mBgAnswer.rectTransform.anchoredPosition3D = Vector3.Lerp(new Vector3(0, -296), new Vector3(0, -511), Mathf.Sin(Mathf.PI * 0.5f * i));
            for(int j = 0; j < mStationsQuestion.Count; j++)
            {
                mStationsQuestion[j].mRtran.anchoredPosition = Vector3.Lerp(poss0[j], poss1[j], i);
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int j = 0; j < mStationsQuestion.Count; j++)
        {
            mStationsQuestion[j].mRtran.anchoredPosition = poss1[j];
            mStationsQuestion[j].SetBoxEnable(true);
        }
        mBgAnswer.rectTransform.anchoredPosition3D = new Vector3(0, -511);

        temp_updata_state = 2;
    }

}
