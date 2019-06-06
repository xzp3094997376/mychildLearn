using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka7 : MonoBehaviour
{
    Image mBg0 { get; set; }
    Image mBgAnswer;
    Image[] mBgTiao { get; set; }
    List<ShapeLogicGuanka7_Station> mStationQuestion;
    List<ShapeLogicGuanka7_Station> mStationAnswer;
    
    int mdata_answer_index = 0;
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
        //if (null != mBgTiao)
        //{
        //    for (int i = 0; i < mBgTiao.Length; i++)
        //    {
        //        mBgTiao[i].rectTransform.anchoredPosition3D += new Vector3(0.5f, 0, 0);
        //        if (mBgTiao[i].rectTransform.anchoredPosition3D.x > 1707)
        //        {
        //            mBgTiao[i].rectTransform.anchoredPosition3D = new Vector3(0, mBgTiao[i].rectTransform.anchoredPosition3D.y, 0);
        //        }
        //    }
        //}

    }
    void UpdatePlay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach (RaycastHit h in hits)
            {
                ShapeLogicGuanka7_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka7_Station>();
                if (null != com && !com.mdata_is_question)
                {
                    ShapeLogicGuanka7_Station.gSelect = com;
                    ShapeLogicGuanka7_Station.gSelect.Select();
                    ShapeLogicCtl.instance.mSound.PlayShort("select0");

                    break;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (null != ShapeLogicGuanka7_Station.gSelect)
            {
                Vector3 pos = Common.getMouseLocalPos(transform);
                pos.z = 0;
                ShapeLogicGuanka7_Station.gSelect.mRtran.anchoredPosition3D = pos;


            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (null != ShapeLogicGuanka7_Station.gSelect)
            {
                RaycastHit[] hits = Common.getMouseRayHits();
                ShapeLogicGuanka7_Station com = null;
                foreach (RaycastHit h in hits)
                {
                    ShapeLogicGuanka7_Station temp = h.collider.gameObject.GetComponent<ShapeLogicGuanka7_Station>();
                    if (null != temp && temp.mdata_is_question)
                    {
                        com = temp;
                        break;
                    }
                }

                if (null != com)
                {
                    bool is_correct = true;
                    if (com.mdata_people_num != ShapeLogicGuanka7_Station.gSelect.mdata_people_num)
                        is_correct = false;
                    if (com.mdata_shape_type != ShapeLogicGuanka7_Station.gSelect.mdata_shape_type)
                        is_correct = false;

                    if (is_correct)
                    {
                        ShapeLogicCtl.instance.mSound.PlayShort("select1");
                        //回答正确
                        ShapeLogicGuanka7_Station.gSelect.gameObject.SetActive(false);
                        com.ShowCorrect();
                        Debug.LogError("回答正确");
                        StartCoroutine("TOver");
                    }
                    else
                    {
                        ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("掉下来");
                        //回答错误
                        ShapeLogicGuanka7_Station.gSelect.PlayError();
                        ShapeLogicGuanka7_Station.gSelect.UnSelect();
                        Debug.LogError("回答错误");

                    }

                }
                else
                {
                    ShapeLogicCtl.instance.mSound.PlayShort("错误");
                    ShapeLogicGuanka7_Station.gSelect.ResetPos();
                    ShapeLogicGuanka7_Station.gSelect.UnSelect();
                }


            }
            ShapeLogicGuanka7_Station.gSelect = null;
        }


    }
    void UpdateOver()
    {

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach (RaycastHit h in hits)
            {
                ShapeLogicGuanka7_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka7_Station>();
                if (null != com)
                {
                    com.SetBoxEnable(false);
                    com.ShootOver();
                    bool is_over = true;
                    for (int i = 0; i < mStationQuestion.Count; i++)
                    {
                        if (!mStationQuestion[i].mdata_is_over)
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
        //背景
        mBg0 = UguiMaker.newImage("mBg0", transform, "public", "white", false);
        mBg0.color = new Color32(254, 214, 30, 255);
        mBg0.rectTransform.sizeDelta = new Vector2(1423, 800);
        List<Vector3> poss = Common.PosSortByWidth(1423, 8, 0);
        mBgTiao = new Image[8];
        for (int i = 0; i < mBgTiao.Length; i++)
        {
            Image bg = UguiMaker.newImage("mBg" + i, mBg0.transform, "shapelogic_sprite", "guanka7_bg", false);
            bg.rectTransform.localEulerAngles = new Vector3(0, 0, 0);
            bg.rectTransform.anchoredPosition3D = poss[i];
            bg.rectTransform.sizeDelta = new Vector2(99, 5000);
            bg.type = Image.Type.Tiled;
            bg.color = new Color32(255, 255, 255, 255);

            mBgTiao[i] = bg;

        }
        //ShapeLogicCtl.instance.CreateLine(1100, mBg0.transform, new List<Vector2>() { new Vector2(0, 166.2f), new Vector2(0, -16.8f) });



        //问题

        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        mStationQuestion = new List<ShapeLogicGuanka7_Station>();
        mdata_answer_index = Random.Range(0, 1000) % 3 + 6;
        poss = Common.PosSortByWidth(1200, 3, 230);
        
        List<int> data_hang0 = new List<int>() { 1, 2, 1};
        List<int> data_hang1 = new List<int>() { 1, 3, 2};
        List<int> data_hang2 = new List<int>() { 2, 5, 3};

        List<int> temp_random_heng = Common.BreakRank<int>(new List<int>() { 0, 1, 2});
        int[] value_index = new int[3];
        value_index[0] = data_hang0[0];
        value_index[1] = data_hang0[1];
        value_index[2] = data_hang0[2];
        data_hang0[0] = value_index[temp_random_heng[0]];
        data_hang0[1] = value_index[temp_random_heng[1]];
        data_hang0[2] = value_index[temp_random_heng[2]];

        value_index[0] = data_hang1[0];
        value_index[1] = data_hang1[1];
        value_index[2] = data_hang1[2];
        data_hang1[0] = value_index[temp_random_heng[0]];
        data_hang1[1] = value_index[temp_random_heng[1]];
        data_hang1[2] = value_index[temp_random_heng[2]];

        value_index[0] = data_hang2[0];
        value_index[1] = data_hang2[1];
        value_index[2] = data_hang2[2];
        data_hang2[0] = value_index[temp_random_heng[0]];
        data_hang2[1] = value_index[temp_random_heng[1]];
        data_hang2[2] = value_index[temp_random_heng[2]];

        List<List<int>> data_heng_num = new List<List<int>>();
        temp_random_heng = Common.BreakRank<int>(temp_random_heng);
        for(int i = 0; i < 3; i++)
        {
            switch(temp_random_heng[i])
            {
                case 0:
                    data_heng_num.Add(data_hang0);
                    break;

                case 1:
                    data_heng_num.Add(data_hang1);
                    break;

                case 2:
                    data_heng_num.Add(data_hang2);
                    break;

            }
        }

        //图形随机
        int[,] shape_random = Common.GetArrayMuteId(3);


        //第一行
        mStationQuestion.Add(UguiMaker.newGameObject("question0", transform).AddComponent<ShapeLogicGuanka7_Station>());
        mStationQuestion.Add(UguiMaker.newGameObject("question1", transform).AddComponent<ShapeLogicGuanka7_Station>());
        mStationQuestion.Add(UguiMaker.newGameObject("question2", transform).AddComponent<ShapeLogicGuanka7_Station>());
        for(int i = 0; i < 3; i++)
        {
            mStationQuestion[i].Init(shape_random[0,i], data_heng_num[0][i]);
            mStationQuestion[i].mRtran.anchoredPosition3D = poss[i];
        }
        //第二行
        mStationQuestion.Add(UguiMaker.newGameObject("question3", transform).AddComponent<ShapeLogicGuanka7_Station>());
        mStationQuestion.Add(UguiMaker.newGameObject("question4", transform).AddComponent<ShapeLogicGuanka7_Station>());
        mStationQuestion.Add(UguiMaker.newGameObject("question5", transform).AddComponent<ShapeLogicGuanka7_Station>());
        for (int i = 0; i < 3; i++)
        {
            mStationQuestion[i + 3].Init(shape_random[1, i], data_heng_num[1][i]);
            mStationQuestion[i + 3].mRtran.anchoredPosition3D = poss[i] + new Vector3(0, -180, 0);
        }
        //第三行
        mStationQuestion.Add(UguiMaker.newGameObject("question6", transform).AddComponent<ShapeLogicGuanka7_Station>());
        mStationQuestion.Add(UguiMaker.newGameObject("question7", transform).AddComponent<ShapeLogicGuanka7_Station>());
        mStationQuestion.Add(UguiMaker.newGameObject("question8", transform).AddComponent<ShapeLogicGuanka7_Station>());
        for (int i = 0; i < 3; i++)
        {
            mStationQuestion[i + 6].Init(shape_random[2, i], data_heng_num[2][i]);
            mStationQuestion[i + 6].mRtran.anchoredPosition3D = poss[i] + new Vector3(0, -360, 0);
        }
        for(int i = 0; i < mStationQuestion.Count; i++)
        {
            mStationQuestion[i].SetQuestion();
            if (i == mdata_answer_index)
            {
                mStationQuestion[i].ShowQuestionPlace();
                mStationQuestion[i].SetBoxEnable(true);
            }
            else
            {
                mStationQuestion[i].ShowQuestionDefault();
                mStationQuestion[i].SetBoxEnable(false);

            }
        }


        //答案
        yield return new WaitForSeconds(1);
        mBgAnswer = UguiMaker.newImage("mBgAnswer", transform, "shapelogic_sprite", "guanka6_bg", false);
        mBgAnswer.type = Image.Type.Sliced;
        mBgAnswer.rectTransform.sizeDelta = new Vector2(1038, 196);
        poss = Common.PosSortByWidth(1000, 5, -25.27f);
        mStationAnswer = new List<ShapeLogicGuanka7_Station>();

        int correct_num = mStationQuestion[mdata_answer_index].mdata_people_num;
        int correct_shape = mStationQuestion[mdata_answer_index].mdata_shape_type;
        List<int> temp_rand_index = Common.BreakRank(new List<int>() { 0, 1, 2, 3, 4 });

        List<int> temp_rand_data = new List<int>() { 12, 13, 21, 31, 23, 32, 22, 33};//十位表示个数，个位表示图形，1表示不变，2表示减一，3表示加一
        List<int> temp_rand_data_index = Common.GetMutexValue(0, temp_rand_data.Count - 1, 4);
        List<int> temp_nums = new List<int>() { correct_num };
        List<int> temp_shapes = new List<int>() { correct_shape};
        for(int i = 0; i < temp_rand_data_index.Count; i++)
        {
            int data_rand = temp_rand_data[temp_rand_data_index[i]];
            int num = correct_num;
            int shape = correct_shape;

            switch (data_rand / 10)
            {
                case 1:
                    break;
                case 2:
                    num--;
                    if (num == 0)
                        num = 3;
                    break;
                case 3:
                    num++;
                    if (num == 6)
                        num = 3;
                    break;
            }

            switch (data_rand % 10)
            {
                case 1:
                    break;
                case 2:
                    shape--;
                    if (-1 == shape)
                        shape = 2;
                    break;
                case 3:
                    shape++;
                    if (3 == shape)
                        shape = 0;
                    break;
            }

            temp_nums.Add(num);
            temp_shapes.Add(shape);

        }

        /*
        List<int> temp_nums = new List<int>() { correct_num, correct_num, correct_num, correct_num, correct_num };
        List<int> temp_shapes = new List<int>() { correct_shape, correct_shape, correct_shape, correct_shape, correct_shape };
        for(int i = 1; i < 5; i++)
        {
            int temp = Random.Range(0, 1000) % 3;

            int num = correct_num;
            while(num == correct_num)
            {
                num = Random.Range(0, 1000) % 4 + 1;
            }

            int shape = correct_shape;
            while(shape == correct_shape)
            {
                shape = Random.Range(0, 1000) % 3;
            }

            switch (temp)
            {
                case 0://改变数量
                    temp_nums[i] = num;
                    break;
                case 1://改变形状
                    temp_shapes[i] = shape;
                    break;
                case 2://都改变
                    temp_nums[i] = num;
                    temp_shapes[i] = shape;
                    break;
            }
        }
        */


        for (int i = 0; i < 5; i++)
        {
            mStationAnswer.Add(UguiMaker.newGameObject("station_answer" + i.ToString(), mBgAnswer.transform).AddComponent<ShapeLogicGuanka7_Station>());
            mStationAnswer[i].Init(temp_shapes[temp_rand_index[i]], temp_nums[temp_rand_index[i]]);
            mStationAnswer[i].SetAnswer();
            mStationAnswer[i].SetBoxEnable(true);
            mStationAnswer[i].mRtran.anchoredPosition3D = poss[i];

        }


        for (float i = 0; i < 1f; i += 0.04f)
        {
            mBgAnswer.rectTransform.anchoredPosition3D = Vector3.Lerp(new Vector3(0, -511), new Vector3(0, -296), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mBgAnswer.rectTransform.anchoredPosition3D = new Vector3(0, -296);

        for (int i = 0; i < mStationAnswer.Count; i++)
        {
            mStationAnswer[i].SetBoxEnable(true);
            mStationAnswer[i].transform.SetParent(transform);
            mStationAnswer[i].SetResetPos(mStationAnswer[i].mRtran.anchoredPosition3D);
        }

        ShapeLogicCtl.instance.mSound.PlayTipListDefaultAb(
            new List<string>() { "7观察每横排长方形里面和上面小人儿的变化规律", "拖动正确的图案放到空白的长方形里" },
            new List<float>() { 1, 1 }, true);

        temp_updata_state = 1;
    }
    IEnumerator TEndGame()
    {
        yield return new WaitForSeconds(0.8f);
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
        mStationAnswer.Clear();
        mStationQuestion.Clear();

        temp_updata_state = 0;
    }
    IEnumerator TOver()
    {
        //ShapeLogicCtl.instance.DestroyLine();
        yield return new WaitForSeconds(0.8f);
        ShapeLogicCtl.instance.mSound.PlayShort("胜利通关音乐", 1);
        List<Vector3> poss0 = new List<Vector3>();
        List<Vector3> poss1 = new List<Vector3>();
        for (int i = 0; i < mStationAnswer.Count; i++)
        {
            mStationAnswer[i].SetBoxEnable(false);
        }
        for (int i = 0; i < mStationQuestion.Count; i++)
        {
            mStationQuestion[i].SetBoxEnable(false);
            poss0.Add(mStationQuestion[i].mRtran.anchoredPosition3D);
            poss1.Add(poss0[i] + new Vector3(0, (i / 3 + 1) * -40, 0));
        }

        for (int i = 0; i < mStationAnswer.Count; i++)
        {
            mStationAnswer[i].transform.SetParent(mBgAnswer.transform);
        }


        for (float i = 0; i < 1f; i += 0.04f)
        {
            mBgAnswer.rectTransform.anchoredPosition3D = Vector3.Lerp( new Vector3(0, -296), new Vector3(0, -511), Mathf.Sin(Mathf.PI * 0.5f * i));
            for(int j = 0; j < mStationQuestion.Count; j++)
            {
                mStationQuestion[j].mRtran.anchoredPosition3D = Vector3.Lerp(poss0[j], poss1[j], i);
            }
            yield return new WaitForSeconds(0.01f);
        }
        mBgAnswer.rectTransform.anchoredPosition3D = new Vector3(0, -511);

        for (int i = 0; i < mStationQuestion.Count; i++)
        {
            mStationQuestion[i].SetBoxEnable(true);
            mStationQuestion[i].Select();
            mStationQuestion[i].mCom.volume = 0.2f;
        }
        temp_updata_state = 2;
        

    }


}
