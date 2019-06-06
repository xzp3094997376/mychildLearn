using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka2 : MonoBehaviour {
    Image mBg { get; set; }
    Image mBgAnswer { get; set; }
    Image bg_box0, bg_box1;
    GameObject mLayer10 { get; set; }
    ParticleSystem mEffectBg { get; set; }
    ParticleSystem mEffectSelect { get; set; }

    List<ShapeLogicGuanka2_Station> mPackQuestion { get; set; }
    List<ShapeLogicGuanka2_Station> mPackAnswer { get; set; }
    List<int> mdata_question_pack_id = null;
    List<int> mdata_anwser_pack_id = null;
    int mdata_answer_id = 0;

    int temp_updata_state = 0;
    List<Vector3> mdata_question_poss = Common.PosSortByWidth(1100, 4, 250);


    void Start () {
	
	}
	void Update ()
    {
        if(1 == temp_updata_state)
        {
            UpdateRunning();
        }
        else if(2 == temp_updata_state)
        {
            UpdateOver();
        }
    }
    void UpdateRunning()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach (RaycastHit h in hits)
            {
                ShapeLogicGuanka2_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka2_Station>();
                if (null != com && com.mdata_id != 10)
                {
                    ShapeLogicGuanka2_Station.gSelect = com;
                    ShapeLogicGuanka2_Station.gSelect.Select();
                    if (null == mEffectSelect)
                    {
                        mEffectSelect = ResManager.GetPrefab("effect_star0", "effect_star0").GetComponent<ParticleSystem>();
                    }
                    mEffectSelect.gameObject.SetActive(true);
                    mEffectSelect.transform.SetParent(ShapeLogicGuanka2_Station.gSelect.transform);
                    mEffectSelect.transform.localScale = Vector3.one;
                    mEffectSelect.Play();
                    mEffectSelect.transform.localPosition = new Vector3(0, -109);

                    ShapeLogicCtl.instance.mSound.PlayOnlyDefaultAb("星星闪烁2");
                    ShapeLogicCtl.instance.mSound.PlayShort("select0");

                    break;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (null != ShapeLogicGuanka2_Station.gSelect)
            {
                Vector3 pos = Common.getMouseLocalPos(transform);
                pos.z = 0;
                ShapeLogicGuanka2_Station.gSelect.mRtran.anchoredPosition3D = pos;// + new Vector3(0, -80, 0);


            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (null != ShapeLogicGuanka2_Station.gSelect)
            {
                mEffectSelect.transform.SetParent(transform);
                mEffectSelect.gameObject.SetActive(false);

                RaycastHit[] hits = Common.getMouseRayHits();
                ShapeLogicGuanka2_Station com = null;
                foreach (RaycastHit h in hits)
                {
                    ShapeLogicGuanka2_Station temp = h.collider.gameObject.GetComponent<ShapeLogicGuanka2_Station>();
                    if(null != temp && temp.mdata_id == 10)
                    {
                        com = temp;
                        break;
                    }

                }

                if(null != com)
                {
                    if(mdata_answer_id == ShapeLogicGuanka2_Station.gSelect.mdata_id)
                    {
                        ShapeLogicCtl.instance.mSound.PlayShort("select1");
                        //正确
                        ShapeLogicGuanka2_Station.gSelect.gameObject.SetActive(false);
                        com.Init(mdata_answer_id);
                        for(int i = 0; i < 4; i++)
                        {
                            mPackQuestion[i].Shake();
                            mPackQuestion[i].SetBoxEnable(false);
                            
                        }
                        for (int i = 0; i < mPackAnswer.Count; i++)
                        {
                            mPackAnswer[i].SetBoxEnable(false);
                        }
                        StartCoroutine("TOver");

                    }
                    else
                    {
                        //错误
                        ShapeLogicGuanka2_Station.gSelect.UnSelect();
                        ShapeLogicGuanka2_Station.gSelect.PlayError();
                        ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("掉下来");

                    }

                }
                else
                {
                    //原路返回
                    ShapeLogicGuanka2_Station.gSelect.UnSelect();
                    ShapeLogicGuanka2_Station.gSelect.ResetPos();
                    ShapeLogicCtl.instance.mSound.PlayShort("错误");
                }




            }
            ShapeLogicGuanka2_Station.gSelect = null;
        }

    }
    void UpdateOver()
    {
        if (Input.GetMouseButtonUp(0))
        {

            RaycastHit[] hits = Common.getMouseRayHits();
            ShapeLogicGuanka2_Station com = null;
            foreach (RaycastHit h in hits)
            {
                com = h.collider.gameObject.GetComponent<ShapeLogicGuanka2_Station>();
                if (null != com)
                {
                    break;
                }

            }

            if (null != com)
            {
                com.PutInBox();
                mPackQuestion.Remove(com);
                com.transform.SetSiblingIndex(com.transform.parent.childCount - 2);
                ShapeLogicCtl.instance.mSound.PlayShort("按钮2");
                if (mPackQuestion.Count == 0)
                {
                    EndGame();
                }
            }



        }
    }

    public void callback_StarEnd(int index)
    {
        ShapeLogicGuanka2_Station pack = UguiMaker.newGameObject("pack" + mdata_question_pack_id[index].ToString(), mLayer10.transform).AddComponent<ShapeLogicGuanka2_Station>();
        if(mdata_answer_id == mdata_question_pack_id[index])
        {
            pack.Init(10);
            pack.SetBoxEnable(true);
        }
        else
        {
            pack.Init(mdata_question_pack_id[index]);
            pack.SetBoxEnable(false);
            //pack.Shake();
        }
        pack.mRtran.anchoredPosition3D = mdata_question_poss[index];
        pack.ShowByScale();
        ShapeLogicCtl.instance.mSound.PlayShort("素材出现通用音效");
        mPackQuestion.Add(pack);
    }
    public void GameOverReset()
    {
        StartCoroutine("TGameOverReset");
    }

    public void BeginGame()
    {

        gameObject.SetActive(true);
        StartCoroutine(TBeginGame());
    }
    public void EndGame()
    {
        //Debug.LogError("结束第二关");
        StartCoroutine(TEndGame());
    }
    IEnumerator TBeginGame()
    {
        //回答id
        mdata_answer_id = Random.Range(0, 1000) % 2 + 2;
        mdata_anwser_pack_id = new List<int>() { 4, 5, 6, mdata_answer_id};
        mdata_anwser_pack_id = Common.BreakRank<int>(mdata_anwser_pack_id);
        //问题id
        switch (Random.Range(0, 1000) % 8)
        {
            case 0:
                mdata_question_pack_id = new List<int>() { 0, 1, 2, 3};
                break;
            case 1:
                mdata_question_pack_id = new List<int>() { 1, 0, 2, 3 };
                break;
            case 2:
                mdata_question_pack_id = new List<int>() { 0, 1, 3, 2 };
                break;
            case 3:
                mdata_question_pack_id = new List<int>() { 1, 0, 3, 2 };
                break;
            case 4:
                mdata_question_pack_id = new List<int>() { 2, 3, 0, 1};
                break;
            case 5:
                mdata_question_pack_id = new List<int>() { 2, 3, 1, 0 };
                break;
            case 6:
                mdata_question_pack_id = new List<int>() { 3, 2, 0, 1 };
                break;
            case 7:
                mdata_question_pack_id = new List<int>() {  3, 2, 1, 0 };
                break;
        }

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

        mPackQuestion = new List<ShapeLogicGuanka2_Station>();
        mPackAnswer = new List<ShapeLogicGuanka2_Station>();

        yield return new WaitForSeconds(1);                                                       
        //Debug.Log("fly");
        List<Vector3> poss = Common.PosSortByWidth(1100, 4, 150);
        for(int i = 0; i < 4; i++)
        {
            EffectStartFlying fly_start = UguiMaker.newGameObject("Star" + i.ToString(), mLayer10.transform).AddComponent<EffectStartFlying>();
            fly_start.Init("shapelogic_sprite", "guanka2_star", new Color32(0, 153, 68, 255), i, callback_StarEnd);
            fly_start.Fly(poss[i] - new Vector3(500, 700, 0), poss[i], 30);
            ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("飞", 0.5f);

            yield return new WaitForSeconds(0.1f);
        }

        mBgAnswer = UguiMaker.newImage("mBgAnswer", mLayer10.transform, "shapelogic_sprite", "guanka1_bg1", false);
        mBgAnswer.transform.SetSiblingIndex(1);
        mBgAnswer.type = Image.Type.Sliced;
        mBgAnswer.rectTransform.sizeDelta = new Vector2(1200, 300);
        for (float i = 0; i < 1f; i += 0.04f)
        {
            mBgAnswer.rectTransform.anchoredPosition3D = Vector3.Lerp(new Vector3(0, -900), new Vector3(0, -415), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mBgAnswer.rectTransform.anchoredPosition3D = new Vector3(0, -415);

        poss = Common.PosSortByWidth(1100, 4, -159);
        for (int i = 0; i < mdata_anwser_pack_id.Count; i++)
        {
            ShapeLogicGuanka2_Station pack = UguiMaker.newGameObject("pack" + mdata_anwser_pack_id[i].ToString(), mLayer10.transform).AddComponent<ShapeLogicGuanka2_Station>();

            pack.Init(mdata_anwser_pack_id[i]);

            pack.mRtran.anchoredPosition3D = poss[i];
            pack.ShowByUp();

            pack.SetBoxEnable(true);

            mPackAnswer.Add(pack);

            yield return new WaitForSeconds(0.2f);
        }


        ShapeLogicCtl.instance.mSound.PlayTipListDefaultAb(
            new List<string>() { "2Error观察1和2的变化规律", "拖动正确的图案放到空白的长方形里" },
            new List<float>() { 1, 1 }, true);

        temp_updata_state = 1;

    }
    IEnumerator TEndGame()
    {
        TopTitleCtl.instance.AddStar();
        yield return new WaitForSeconds(1f);
        GameOverCtl.GetInstance().Show(3, GameOverReset);

        //for (float i = 0; i < 1f; i += 0.06f)
        //{
        //    Vector2 size = Vector2.Lerp(new Vector2(534, 138), new Vector2(0, 138), i);
        //    bg_box0.rectTransform.sizeDelta = size;
        //    bg_box1.rectTransform.sizeDelta = size;
        //    yield return new WaitForSeconds(0.01f);
        //}
        //Destroy(bg_box0.gameObject);
        //Destroy(bg_box1.gameObject);
        //bg_box0 = null;
        //bg_box1 = null;

        //Destroy(mEffectBg.gameObject);
        //Destroy(mEffectSelect.gameObject);
        //mEffectSelect = null;
        //mEffectBg = null;

        //ShapeLogicCtl.instance.GameNext();

        //Color bg_color0 = mBg.color;
        //Color bg_color1 = mBg.color;
        //bg_color1.a = 0;
        //for (float j = 0; j < 1f; j += 0.03f)
        //{
        //    mBg.color = Color.Lerp(bg_color0, bg_color1, j);
        //    yield return new WaitForSeconds(0.01f);
        //}

        //foreach (Transform t in transform)
        //{
        //    Destroy(t.gameObject);
        //}
        //gameObject.SetActive(false);
        //mBg = null;
        //mBgAnswer = null;
        //mPackQuestion.Clear();
        //mPackAnswer.Clear();

        //gameObject.SetActive(false);

    }
    IEnumerator TOver()
    {
        yield return new WaitForSeconds(0.8f);
        //ShapeLogicCtl.instance.mSound.PlayShort("胜利通关音乐", 1);
        for (int i = 0; i < mPackAnswer.Count; i++)
        {
            mPackAnswer[i].transform.SetParent(mBgAnswer.transform);
        }

        Vector3 pos0 = mBgAnswer.rectTransform.anchoredPosition3D;
        Vector3 pos1 = pos0 - new Vector3(0, 500, 0);
        

        for(float i = 0; i < 1f; i += 0.07f)
        {
            mBgAnswer.rectTransform.anchoredPosition3D = Vector3.Lerp(pos0, pos1, i);
            for(int j = 0; j < mPackQuestion.Count; j++)
            {
                mPackQuestion[j].mRtran.anchoredPosition3D = Vector3.Lerp( mdata_question_poss[j], mdata_question_poss[j] + new Vector3(0, -50, 0), i);
            }
            yield return new WaitForSeconds(0.01f);
        }

        //List<Vector3> poss = Common.PosSortByWidth(142)
        bg_box0 = UguiMaker.newImage("bg_box", mLayer10.transform, "shapelogic_sprite", "guanka2_box", false);
        bg_box0.type = Image.Type.Sliced;
        bg_box0.rectTransform.sizeDelta = new Vector2(0, 138);
        bg_box0.transform.SetAsFirstSibling();
        bg_box0.rectTransform.anchoredPosition3D = new Vector3(0, -254, 0);


        bg_box1 = UguiMaker.newImage("bg_box", mLayer10.transform, "shapelogic_sprite", "guanka2_box1", false);
        bg_box1.type = Image.Type.Sliced;
        bg_box1.rectTransform.sizeDelta = new Vector2(0, 138);
        bg_box1.transform.SetAsLastSibling();
        bg_box1.rectTransform.anchoredPosition3D = new Vector3(0, -254, 0);

        for(float i = 0; i < 1f; i += 0.05f)
        {
            Vector2 size = Vector2.Lerp(new Vector2(0, 138), new Vector2(534, 138), i) + new Vector2(Mathf.Sin(Mathf.PI * i) * 250, 0);
            bg_box0.rectTransform.sizeDelta = size;
            bg_box1.rectTransform.sizeDelta = size;
            yield return new WaitForSeconds(0.01f);
        }

        for (int j = 0; j < mPackQuestion.Count; j++)
        {
            mPackQuestion[j].SetBoxEnable(true);
        }
        temp_updata_state = 2;

        yield break;
    }
    IEnumerator TGameOverReset()
    {
        for (float i = 0; i < 1f; i += 0.06f)
        {
            Vector2 size = Vector2.Lerp(new Vector2(534, 138), new Vector2(0, 138), i);
            bg_box0.rectTransform.sizeDelta = size;
            bg_box1.rectTransform.sizeDelta = size;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(bg_box0.gameObject);
        Destroy(bg_box1.gameObject);
        bg_box0 = null;
        bg_box1 = null;

        Destroy(mEffectBg.gameObject);
        Destroy(mEffectSelect.gameObject);
        mEffectSelect = null;
        mEffectBg = null;

        ShapeLogicCtl.instance.Reset();

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
        mPackQuestion.Clear();
        mPackAnswer.Clear();

        gameObject.SetActive(false);

    }




}
