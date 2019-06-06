using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka4 : MonoBehaviour {
    Image mBg { get; set; }
    Image mBgAnswer { get; set; }
    ParticleSystem mEffectBg { get; set; }
    GameObject mLayer10 { get; set; }
    ParticleSystem mEffect { get; set; }

    Dictionary<int, List<int>> mdata_dic_rule;//分别存了3层的规律
    List<List<int>> mdata_questions_ids;
    List<List<int>> mdata_answers_ids;
    int mdata_answer_index = 0;
    int temp_updata_state = 0;

    List<ShapeLogicGuanka4_Station> mStationQuestion;
    List<ShapeLogicGuanka4_Station> mStationAnswer;


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


    }
    void UpdatePlay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach (RaycastHit h in hits)
            {
                ShapeLogicGuanka4_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka4_Station>();
                if (null != com && com.mdata_can_show)
                {
                    ShapeLogicGuanka4_Station.gSelect = com;
                    ShapeLogicGuanka4_Station.gSelect.Select();
                    if (null == mEffect)
                    {
                        mEffect = ResManager.GetPrefab("effect_star0", "effect_star0").GetComponent<ParticleSystem>();
                    }
                    mEffect.gameObject.SetActive(true);
                    mEffect.transform.SetParent(ShapeLogicGuanka4_Station.gSelect.transform);
                    mEffect.transform.localScale = Vector3.one;
                    mEffect.Play();
                    mEffect.transform.localPosition = Vector3.zero;

                    ShapeLogicCtl.instance.mSound.PlayOnlyDefaultAb("星星闪烁2");
                    ShapeLogicCtl.instance.mSound.PlayShort("select0");

                    break;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (null != ShapeLogicGuanka4_Station.gSelect)
            {
                Vector3 pos = Common.getMouseLocalPos(transform);
                pos.z = 0;
                ShapeLogicGuanka4_Station.gSelect.mRtran.anchoredPosition3D = pos;


            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (null != ShapeLogicGuanka4_Station.gSelect)
            {
                RaycastHit[] hits = Common.getMouseRayHits();
                ShapeLogicGuanka4_Station com = null;
                bool result = false;
                foreach (RaycastHit h in hits)
                {
                    com = h.collider.gameObject.GetComponent<ShapeLogicGuanka4_Station>();
                    if (null != com && !com.mdata_can_show)
                    {
                        result = true;
                        break;
                    }
                }

                if (result)
                {
                    bool is_correct = true;
                    for (int i = 0; i < ShapeLogicGuanka4_Station.gSelect.mdata_ids.Count; i++)
                    {
                        if (ShapeLogicGuanka4_Station.gSelect.mdata_ids[i] !=  mdata_answers_ids[mdata_answer_index][i])
                        {
                            is_correct = false;
                            break;
                        }
                    }
                    if (is_correct)
                    {
                        ShapeLogicCtl.instance.mSound.PlayShort("select1");
                        //回答正确
                        ShapeLogicGuanka4_Station.gSelect.gameObject.SetActive(false);
                        com.SetCanShow(true);
                        com.InitBg(mdata_answers_ids[mdata_answer_index]);
                        Debug.LogError("回答正确");
                        StartCoroutine("TOver");
                    }
                    else
                    {
                        ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("掉下来");
                        //回答错误
                        ShapeLogicGuanka4_Station.gSelect.PlayError();
                        ShapeLogicGuanka4_Station.gSelect.UnSelect();
                        Debug.LogError("回答错误");

                    }

                }
                else
                {
                    ShapeLogicCtl.instance.mSound.PlayShort("错误");
                    ShapeLogicGuanka4_Station.gSelect.ResetPos();
                    ShapeLogicGuanka4_Station.gSelect.UnSelect();
                }

                mEffect.gameObject.SetActive(false);

            }
            ShapeLogicGuanka4_Station.gSelect = null;
        }


    }
    void UpdateOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach (RaycastHit h in hits)
            {
                ShapeLogicGuanka4_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka4_Station>();
                if (null != com)
                {
                    //com.Scale2Zero();
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
        is_show_answer = false;
        //规则
        mdata_dic_rule = new Dictionary<int, List<int>>();
        mdata_dic_rule.Add(0, new List<int>() { 0, 5, 4, 1, 2, 5, 4, 3 });
        mdata_dic_rule.Add(1, new List<int>() { 0, 3, 2, 1, 1, 2, 3, 0 });
        mdata_dic_rule.Add(2, new List<int>() { 5, 0, 1, 4, 5, 2, 3, 4 });

        mdata_answer_index = Random.Range(0, 1000) % 4;

        mdata_questions_ids = new List<List<int>>();
        List<int> data_combine = Common.BreakRank<int>(new List<int>() { 0, 1, 2 });
        for(int i = 0; i < 4; i++)
        {
            List<int> ids = new List<int>();
            for (int j = 0; j < data_combine.Count; j++)
            {
                List<int> rule = mdata_dic_rule[data_combine[j]];
                ids.Add(rule[i * 2]);
                ids.Add(rule[i * 2 + 1]);
            }
            mdata_questions_ids.Add(ids);
        }

        //问题
        mdata_answers_ids = new List<List<int>>();
        List<int> correct_ids = mdata_questions_ids[mdata_answer_index];
        for(int i = 0; i < 4; i++)
        {
            List<int> data = new List<int>();
            for(int j = 0; j < correct_ids.Count; j++)
            {
                data.Add(correct_ids[j]);
            }
            mdata_answers_ids.Add(data);
        }

        //答案
        //int correct_index = Random.Range(0, 1000) % 4;
        List<int> error_indexs = Common.GetMutexValue(0, 5, 4);
        for(int i = 0; i < 4; i++)
        {
            if (mdata_answer_index == i)
                continue;
            int error_index = error_indexs[i];
            int correct_id = mdata_answers_ids[i][error_index];
            while(true)
            {
                int temp_id = Random.Range(0, 1000) % 7;
                if(temp_id != correct_id)
                {
                    mdata_answers_ids[i][error_index] = temp_id;
                    break;
                }
            }
        }



        //背景
        mBg = UguiMaker.newImage("mBg0", transform, "public", "white", false);
        mBg.color = new Color32(254, 198, 65, 255);
        mBg.rectTransform.sizeDelta = new Vector2(1423, 800);
        mEffectBg = ResManager.GetPrefab("effect_start3", "star_bg").GetComponent<ParticleSystem>();
        UguiMaker.InitGameObj(mEffectBg.gameObject, transform, "mEffetBg", Vector3.zero, Vector3.one);
        mLayer10 = UguiMaker.newGameObject("mLayer10", transform).gameObject;
        mLayer10.layer = LayerMask.NameToLayer("UI");
        Canvas canvas = mLayer10.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 10;

        //出来问题
        mStationQuestion = new List<ShapeLogicGuanka4_Station>();
        List<Vector3> poss = Common.PosSortByWidth(1280, 4, 0);
        List<Vector3> poss1 = Common.PosSortByWidth(1100, 4, 150);
        for(int i = 0; i < 4; i++)
        {
            ShapeLogicGuanka4_Station station = UguiMaker.newGameObject("question" + i.ToString(), mLayer10.transform).AddComponent<ShapeLogicGuanka4_Station>();
            station.InitQuestion(mdata_questions_ids[i]);
            station.mRtran.anchoredPosition3D = poss[i];
            if (i == mdata_answer_index)
                station.SetCanShow(false);
            else
                station.SetCanShow(true);
            station.PlayInitQuestion();
            station.SetResetPos(poss1[i]);
            mStationQuestion.Add(station);
        }

        ShapeLogicCtl.instance.mSound.PlayTipListDefaultAb(
            new List<string>() { "4观察长方形里面颜色和形状的变化规律", "拖动正确的图案放到空白的长方形里" },
            new List<float>() { 1, 1 }, true);

        yield break;
    }
    IEnumerator TEndGame()
    {
        //yield break;

        ShapeLogicCtl.instance.GameNext();
        Destroy(mEffectBg.gameObject);
        mEffectBg = null;

        Color bg_color0 = mBg.color;
        Color bg_color1 = mBg.color;
        bg_color1.a = 0;

        for (float j = 0; j < 1f; j += 0.03f)
        {
            mBg.color = Color.Lerp(bg_color0, bg_color1, j);
            yield return new WaitForSeconds(0.01f);
        }

        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        gameObject.SetActive(false);

        mBg = null;
        mBgAnswer = null;
        mEffectBg = null;
        mLayer10 = null;
        mdata_dic_rule.Clear();
        mdata_questions_ids.Clear();
        mdata_answers_ids.Clear();
        mdata_answer_index = 0;
        temp_updata_state = 0;
        mStationQuestion.Clear();
        mStationAnswer.Clear();

    }

    bool is_show_answer = false;
    public void callback_ShowAnswer()
    {
        if (is_show_answer)
            return;
        is_show_answer = true;
        StartCoroutine("TShowAnswer");
    }
    IEnumerator TShowAnswer()
    {

        mBgAnswer = UguiMaker.newImage("mBgAnswer", mLayer10.transform, "shapelogic_sprite", "guanka1_bg1", false);
        mBgAnswer.transform.SetSiblingIndex(1);
        mBgAnswer.type = Image.Type.Sliced;
        mBgAnswer.rectTransform.sizeDelta = new Vector2(1060, 360);

        List<Vector3> poss = Common.PosSortByWidth(1060, 4, 0);
        mStationAnswer = new List<ShapeLogicGuanka4_Station>();
        for (int i = 0; i < 4; i++)
        {
            ShapeLogicGuanka4_Station station = UguiMaker.newGameObject("answer" + i.ToString(), mBgAnswer.transform).AddComponent<ShapeLogicGuanka4_Station>();
            station.InitAnswer(mdata_answers_ids[i]);
            station.mRtran.anchoredPosition3D = poss[i];
            mStationAnswer.Add(station);
        }
        
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mBgAnswer.rectTransform.anchoredPosition3D = Vector3.Lerp(new Vector3(0, -600), new Vector3(0, -200), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

        for (int i = 0; i < 4; i++)
        {
            mStationAnswer[i].transform.SetParent(mLayer10.transform);
            mStationAnswer[i].SetResetPos(mStationAnswer[i].mRtran.anchoredPosition3D);
            mStationAnswer[i].SetBoxEnable(true);
            mStationQuestion[i].SetBoxEnable(!mStationQuestion[i].mdata_can_show);
            
        }



        temp_updata_state = 1;

    }

    IEnumerator TOver()
    {
        for (int i = 0; i < mStationQuestion.Count; i++)
        {
            mStationQuestion[i].SetBoxEnable(false);
        }
        for (int i = 0; i < mStationAnswer.Count; i++)
        {
            mStationAnswer[i].SetBoxEnable(false);
        }
        yield return new WaitForSeconds(0.8f);
        ShapeLogicCtl.instance.mSound.PlayShort("胜利通关音乐", 1);
        temp_updata_state = 0;

        for (int i = 0; i < 4; i++)
        {
            mStationAnswer[i].transform.SetParent(mBgAnswer.transform);

        }

        for (float i = 0; i < 1f; i += 0.05f)
        {
            mBgAnswer.rectTransform.anchoredPosition3D = Vector3.Lerp(new Vector3(0, -200), new Vector3(0, -600), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

        List<Vector3> poss0 = Common.PosSortByWidth(1100, 4, 150);
        List<Vector3> poss1 = Common.PosSortByWidth(1200, 4, -50);

        for (float i = 0; i < 1f; i += 0.05f)
        {
            for(int j = 0; j < 4; j++)
            {
                mStationQuestion[j].mRtran.anchoredPosition3D = Vector3.Lerp(poss0[j], poss1[j], i);
            }
            yield return new WaitForSeconds(0.01f);
        }

        for(int i = 0; i < 4; i++)
        {
            mStationQuestion[i].ShowOver();
            mStationQuestion[i].SetBoxEnable(true);
        }

        temp_updata_state = 2;
        yield break;
    }

}
