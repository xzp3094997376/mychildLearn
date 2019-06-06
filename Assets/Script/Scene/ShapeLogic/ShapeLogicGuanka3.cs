using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka3 : MonoBehaviour {

    Image mBg0 { get; set; }
    Image[] mBgTiao { get; set; }
    Image mGround { get; set; }
    Image mBgAnswer { get; set; }
    RectTransform mGroundBack { get; set; }
    RectTransform mGroundFront0 { get; set; }
    RectTransform mGroundFront1 { get; set; }

    List<ShapeLogicGuanka3_Station> mStationQuestion;
    List<ShapeLogicGuanka3_Station> mStationAnswer;

    List<List<int>> mdata_question_color_id { get; set; }//上面每个礼品袋中分别的花朵颜色序列
    List<Vector3> mdata_question_poss { get; set; }
    int mdata_answer_index = 0;
    int temp_updata_state = 0;

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        switch (temp_updata_state)
        {
            case 1:
                UpdatePlay();
                break;
            case 2:
                UpdateOver();
                break;

        }


    }
    void UpdatePlay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach (RaycastHit h in hits)
            {
                ShapeLogicGuanka3_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka3_Station>();
                if (null != com && !com.mdata_is_question)
                {
                    ShapeLogicGuanka3_Station.gSelect = com;
                    ShapeLogicGuanka3_Station.gSelect.Select();

                    ShapeLogicCtl.instance.mSound.PlayOnlyDefaultAb("星星闪烁2");
                    ShapeLogicCtl.instance.mSound.PlayShort("select0");


                    break;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (null != ShapeLogicGuanka3_Station.gSelect)
            {
                Vector3 pos = Common.getMouseLocalPos(transform);
                pos.z = 0;
                ShapeLogicGuanka3_Station.gSelect.mRtran.anchoredPosition3D = pos;


            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (null != ShapeLogicGuanka3_Station.gSelect)
            {
                RaycastHit[] hits = Common.getMouseRayHits();
                ShapeLogicGuanka3_Station com = null;
                bool result = false;
                foreach (RaycastHit h in hits)
                {
                    com = h.collider.gameObject.GetComponent<ShapeLogicGuanka3_Station>();
                    if (null != com && com.mdata_is_question)
                    {
                        result = true;
                        break;
                    }
                }

                if (result)
                {
                    bool is_correct = true;
                    for(int i = 0; i < ShapeLogicGuanka3_Station.gSelect.color_ids.Count; i++)
                    {
                        if(ShapeLogicGuanka3_Station.gSelect.color_ids[i] != mdata_question_color_id[mdata_answer_index][i])
                        {
                            is_correct = false;
                            break;
                        }
                    }
                    if (is_correct)
                    {
                        ShapeLogicCtl.instance.mSound.PlayShort("select1");
                        //回答正确
                        ShapeLogicGuanka3_Station.gSelect.gameObject.SetActive(false);
                        com.CorrectShowFlower();
                        //Debug.LogError("回答正确");
                        StartCoroutine("TOver");
                    }
                    else
                    {
                        ShapeLogicCtl.instance.mSound.PlayShort("错误");
                        //回答错误
                        ShapeLogicGuanka3_Station.gSelect.PlayError();
                        //mEffect.gameObject.SetActive(false);
                        //Debug.LogError("回答错误");

                    }

                }
                else
                {
                    ShapeLogicCtl.instance.mSound.PlayShort("错误");
                    ShapeLogicGuanka3_Station.gSelect.ResetPos();
                    //mEffect.gameObject.SetActive(false);
                }


            }
            ShapeLogicGuanka3_Station.gSelect = null;
        }


    }
    void UpdateOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach (RaycastHit h in hits)
            {
                ShapeLogicGuanka3_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka3_Station>();
                if (null != com)
                {
                    //com.Scale2Zero();
                    com.SetBoxEnable(false);
                    com.OnClkOver();
                    bool is_over = true;
                    for(int i = 0; i < mStationQuestion.Count; i++)
                    {
                        if(!mStationQuestion[i].mdata_is_over)
                        {
                            is_over = false;
                            break;
                        }
                    }
                    if(is_over)
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
        //数据
        mdata_question_color_id = new List<List<int>>();
        List<int> temp = new List<int>() { 0, 1, 2, 3};
        temp = Common.BreakRank<int>(temp);
        for(int i = 0; i < 4; i++)
        {
            List<int> list = new List<int>();
            for(int j = 0; j < 4; j++)
            {
                int index = j - i;
                if(index < 0)
                {
                    index = index + temp.Count;
                }
                list.Add(temp[index]);
            }
            mdata_question_color_id.Add(list);
        }

        mdata_question_poss = Common.PosSortByWidth(1300, 4, 310);
        mdata_answer_index = Random.Range(0, 1000) % 4;

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
            bg.rectTransform.sizeDelta = new Vector2(1300, 262);
            bg.type = Image.Type.Tiled;
            bg.color = new Color32(255, 255, 255, 255);

            mBgTiao[i] = bg;

        }

        //yield return new WaitForSeconds(1);

        //土地
        mGroundBack = UguiMaker.newGameObject("mGroundBack", transform).GetComponent<RectTransform>();

        mGround = UguiMaker.newImage("mBg2", transform, "shapelogic_sprite", "guanka3_bg2", false);
        mGround.rectTransform.sizeDelta = new Vector2(1423, 132);
        mGround.type = Image.Type.Tiled;
        mGround.color = new Color32(255, 255, 255, 255);
        mGround.rectTransform.pivot = new Vector2(0.5f, 0);

        //ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("树");

        Vector3 pos0 = new Vector3(0, -532);
        Vector3 pos1 = new Vector3(0, -400);
        for (float i = 0; i < 1f; i += 0.06f)
        {
            mGround.rectTransform.anchoredPosition3D = Vector3.Lerp(pos0, pos1, i);
            yield return new WaitForSeconds(0.01f);
        }
        mGround.rectTransform.anchoredPosition3D = pos1;

        mGroundFront0 = UguiMaker.newGameObject("mGroundFront0", transform).GetComponent<RectTransform>();
        mGroundFront1 = UguiMaker.newGameObject("mGroundFront1", transform).GetComponent<RectTransform>();

        //花
        List<Vector3> flower_offset = new List<Vector3>()
        {
            new Vector3(-80, -640, 0),
            new Vector3(40, -640, 0),
            new Vector3(-35, -690, 0),
            new Vector3(95, -690, 0),
        };

        //ShapeLogicCtl.instance.mSound.PlaySoundList(new List<string>() { "shapelogic_sound", "shapelogic_sound" }, new List<string>() { "树", "树"});


        mStationQuestion = new List<ShapeLogicGuanka3_Station>();
        for(int i = 0; i < mdata_question_poss.Count; i++)
        {
            ShapeLogicGuanka3_Station station = UguiMaker.newGameObject("station" + i.ToString(), mGroundBack.transform).AddComponent<ShapeLogicGuanka3_Station>();
            station.InitQuestion(mdata_question_color_id[i]);
            station.mRtran.anchoredPosition3D = mdata_question_poss[i] + new Vector3(0, -600, 0);
            mStationQuestion.Add(station);
        }

        for (int i = 0; i < mdata_question_color_id.Count; i++)
        {
            List<int> color_ids = mdata_question_color_id[i];
            for(int j = 0; j < color_ids.Count; j++)
            {
                ShapeLogicGuanka3_Flower flower = UguiMaker.newGameObject("flower", mGroundFront1).AddComponent<ShapeLogicGuanka3_Flower>();
                flower.Init(color_ids[j]);
                flower.mRtran.anchoredPosition3D = mdata_question_poss[i] + flower_offset[j];
                flower.Growup();
                mStationQuestion[i].AddJumpEatFlower(flower);

            }

        }

        yield return new WaitForSeconds(1.7f);
        //问题出来
        for(int i = 0; i < mdata_question_color_id.Count; i++)
        {
            if(i != mdata_answer_index)
            {
                mStationQuestion[i].Jump(mGroundBack, mGroundFront0, mGroundFront1, mdata_question_poss[i], true);
                mStationQuestion[i].SetBoxEnable(false);
            }
            else
            {
                mStationQuestion[i].Jump(mGroundBack, mGroundFront0, mGroundFront1, mdata_question_poss[i], false);
                mStationQuestion[i].SetBoxEnable(true);
            }
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(4f);

        mBgAnswer = UguiMaker.newImage("mBgAnswer", transform, "shapelogic_sprite", "guanka3_bg3", false);
        mBgAnswer.type = Image.Type.Sliced;
        mBgAnswer.rectTransform.sizeDelta = new Vector2(1200, 340);

        List<List<int>> list_answer_id = new List<List<int>>();
        List<int> correct_color_id = mdata_question_color_id[mdata_answer_index];
        List<int> rand = Common.GetMutexValue(0, 5, 3);
        //第一个正确的
        list_answer_id.Add(new List<int>() { correct_color_id [0], correct_color_id [1], correct_color_id [2], correct_color_id [3]});
        //后面的是干扰, 任意两个交换视
        for (int i = 0; i < 3; i++)
        {
            switch (rand[i])
            {
                case 0:
                    list_answer_id.Add(new List<int>() { correct_color_id[1], correct_color_id[0], correct_color_id[2], correct_color_id[3] });
                    break;
                case 1:
                    list_answer_id.Add(new List<int>() { correct_color_id[2], correct_color_id[1], correct_color_id[0], correct_color_id[3] });
                    break;
                case 2:
                    list_answer_id.Add(new List<int>() { correct_color_id[3], correct_color_id[1], correct_color_id[2], correct_color_id[0] });
                    break;
                case 3:
                    list_answer_id.Add(new List<int>() { correct_color_id[0], correct_color_id[2], correct_color_id[1], correct_color_id[3] });
                    break;
                case 4:
                    list_answer_id.Add(new List<int>() { correct_color_id[0], correct_color_id[3], correct_color_id[2], correct_color_id[1] });
                    break;
                case 5:
                    list_answer_id.Add(new List<int>() { correct_color_id[0], correct_color_id[1], correct_color_id[3], correct_color_id[2] });
                    break;
            }
        }


        mStationAnswer = new List<ShapeLogicGuanka3_Station>();
        poss = Common.PosSortByWidth(1200, 4, 140);
        for(int i = 0; i < 4; i++)
        {
            ShapeLogicGuanka3_Station station = UguiMaker.newGameObject("station_answer" + i.ToString(), mBgAnswer.transform).AddComponent<ShapeLogicGuanka3_Station>();
            int index = Random.Range(0, 1000) % list_answer_id.Count;
            station.InitAnswer(list_answer_id[index]);
            station.mRtran.anchoredPosition3D = poss[i];

            list_answer_id.RemoveAt(index);
            mStationAnswer.Add(station);
        }




        pos0 = new Vector3(0, -600, 0);
        pos1 = new Vector3(0, -200, 0);
        Vector3 temp_pos = mGround.rectTransform.anchoredPosition3D;
        Vector3 temp_pos1 = temp_pos + new Vector3(0, -150, 0);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mGround.rectTransform.anchoredPosition3D = Vector3.Lerp(temp_pos, temp_pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
            mBgAnswer.rectTransform.anchoredPosition3D = Vector3.Lerp(pos0, pos1, i);
            yield return new WaitForSeconds(0.01f);
        }
        mBgAnswer.rectTransform.anchoredPosition3D = pos1;

        for (int i = 0; i < 4; i++)
        {
            mStationAnswer[i].OpenMouse_ShowFlower();
            mStationAnswer[i].transform.SetParent(transform);
            mStationAnswer[i].transform.SetAsLastSibling();
            mStationAnswer[i].SetResetPos(mStationAnswer[i].mRtran.anchoredPosition3D);
        }

        ShapeLogicCtl.instance.mSound.PlayTipListDefaultAb(
            new List<string>() { "3观察长方形里面小花颜色的变化规律", "拖动正确的图案放到空白的长方形里" },
            new List<float>() { 1, 1 }, true);

        yield return new WaitForSeconds(1);
        temp_updata_state = 1;

    }
    IEnumerator TEndGame()
    {
        for(int i = 0; i < mStationQuestion.Count; i++)
        {
            mStationQuestion[i].transform.SetParent(mGround.transform);
        }
        
        Vector3 temp_pos = mGround.rectTransform.anchoredPosition3D;
        Vector3 temp_pos1 = temp_pos + new Vector3(0, -320, 0);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mGround.rectTransform.anchoredPosition3D = Vector3.Lerp(temp_pos, temp_pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

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

    }
    IEnumerator TOver()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("胜利通关音乐", 1);

        for (int i = 0; i < mStationAnswer.Count; i++)
        {
            mStationAnswer[i].SetBoxEnable(false);
            mStationAnswer[i].transform.SetParent(mBgAnswer.rectTransform);
        }
        for (int i = 0; i < mStationQuestion.Count; i++)
        {
            mStationQuestion[i].SetBoxEnable(false);
        }


        Vector3 pos0 = new Vector3(0, -600, 0);
        Vector3 pos1 = new Vector3(0, -200, 0);
        Vector3 temp_pos = mGround.rectTransform.anchoredPosition3D;
        Vector3 temp_pos1 = temp_pos + new Vector3(0, 150, 0);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mGround.rectTransform.anchoredPosition3D = Vector3.Lerp(temp_pos, temp_pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
            mBgAnswer.rectTransform.anchoredPosition3D = Vector3.Lerp(pos1, pos0, i);
            yield return new WaitForSeconds(0.01f);
        }
        mBgAnswer.rectTransform.anchoredPosition3D = pos0;


        for (int i = 0; i < mStationQuestion.Count; i++)
        {
            mStationQuestion[i].Falldown();
        }

        temp_updata_state = 2;

        

        yield break;
    }

}

