using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 小手袋
/// </summary>
public class ShapeLogicGuanka1 : MonoBehaviour {

    Image mBg;
    Image[] mBgTiao = null;
    Image mBgAnswer;
    List<ShapeLogicGuanka1_Station> mStationQuestion;

    public ParticleSystem mEffect { get; set; }

    bool temp_isover = false;

	void Start () {
	
	}
    void Update()
    {
        if(temp_isover)
        {
            UpdateOver();
        }
        else
        {
            UpdatePlay();
        }
    }
	void UpdatePlay ()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach(RaycastHit h in hits)
            {
                ShapeLogicGuanka1_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka1_Station>();
                if(null != com  && !com.mdata_is_question)
                {
                    ShapeLogicGuanka1_Station.gSelect = com;
                    ShapeLogicGuanka1_Station.gSelect.Select();
                    if(null == mEffect)
                    {
                        mEffect = ResManager.GetPrefab("effect_fan", "fan0").GetComponent<ParticleSystem>();
                    }
                    mEffect.gameObject.SetActive(true);
                    mEffect.transform.SetParent(ShapeLogicGuanka1_Station.gSelect.transform);
                    mEffect.transform.localScale = Vector3.one;
                    mEffect.Play();
                    mEffect.transform.localPosition = Vector3.zero;
                    ShapeLogicCtl.instance.mSound.PlayOnlyDefaultAb("风");
                    ShapeLogicCtl.instance.mSound.PlayShort("select0");

                    break;
                }
            }
        }
        else if(Input.GetMouseButton(0))
        {
            if(null != ShapeLogicGuanka1_Station.gSelect)
            {
                Vector3 pos = Common.getMouseLocalPos(transform);
                pos.z = 0;
                ShapeLogicGuanka1_Station.gSelect.mRtran.anchoredPosition3D = pos;


            }
            
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if(null != ShapeLogicGuanka1_Station.gSelect)
            {
                ShapeLogicCtl.instance.mSound.StopOnly();
                RaycastHit[] hits = Common.getMouseRayHits();
                ShapeLogicGuanka1_Station com = null;
                bool result = false;
                foreach (RaycastHit h in hits)
                {
                    com = h.collider.gameObject.GetComponent<ShapeLogicGuanka1_Station>();
                    if (null != com && com.mdata_is_question)
                    {
                        result = true;
                        break;
                    }
                }

                if(result)
                {
                    if (ShapeLogicGuanka1_Station.gSelect.mdata_index == 1)
                    {
                        //回答正确
                        ShapeLogicGuanka1_Station.gSelect.gameObject.SetActive(false);
                        com.SetPackSprite("guanka1_answer1");
                        Debug.LogError("回答正确");
                        ShapeLogicCtl.instance.mSound.PlayShort("select1");
                        StartCoroutine("TOver");
                    }
                    else
                    {
                        //回答错误
                        ShapeLogicGuanka1_Station.gSelect.PlayError();
                        mEffect.gameObject.SetActive(false);
                        ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("掉下来");
                        Debug.LogError("回答错误");
                    
                    }

                }
                else
                {
                    ShapeLogicGuanka1_Station.gSelect.ResetPos();
                    mEffect.gameObject.SetActive(false);
                    ShapeLogicCtl.instance.mSound.PlayShort("错误");
                }


            }
            ShapeLogicGuanka1_Station.gSelect = null;
        }
	
	}
    void UpdateOver()
    {
        if(Input.GetMouseButtonUp(0))
        {
            RaycastHit[] hits = Common.getMouseRayHits();
            foreach (RaycastHit h in hits)
            {
                ShapeLogicGuanka1_Station com = h.collider.gameObject.GetComponent<ShapeLogicGuanka1_Station>();
                if (null != com)
                {
                    com.Scale2Zero();
                    com.SetBoxEnable(false);
                    ShapeLogicCtl.instance.mSound.PlayShort("按钮2");

                    break;
                }
            }

        }
    }

    int temp_count = 0;
    public void callback_over()
    {
        temp_count++;
        if(4 == temp_count)
        {
            //ShapeLogicCtl.instance.
            EndGame();
            ShapeLogicCtl.instance.GameNext();
        }

    }

    public void BeginGame()
    {
        temp_count = 0;
        gameObject.SetActive(true);
        transform.localScale = Vector3.one;
        StartCoroutine(TBeginGame());
    }
    public void EndGame()
    {
        transform.SetAsLastSibling();
        StartCoroutine(TEndGame());
    }
    IEnumerator TBeginGame()
    {
        temp_isover = false;
        mBg = UguiMaker.newImage("mBg0", transform, "public", "white", false);
        mBg.color = new Color32(169, 237, 243, 255);
        mBg.rectTransform.sizeDelta = new Vector2(1423, 800);

        List<Vector3> poss = Common.PosSortByHeight(850, 5, 0);
        mBgTiao = new Image[5];
        for (int i = 0; i < 5; i++)
        {
            Image bg = UguiMaker.newImage("mBg" + i, mBg.transform, "shapelogic_sprite", "guanka2_bg", false);
            bg.rectTransform.localEulerAngles = new Vector3(0, 0, 0);
            bg.rectTransform.anchoredPosition3D = poss[i];
            bg.rectTransform.sizeDelta = new Vector2(1300, 131);
            bg.type = Image.Type.Tiled;
            bg.color = new Color32(255, 255, 255, 128);

            mBgTiao[i] = bg;

        }



        yield return new WaitForSeconds(1);
        ShapeLogicCtl.instance.mSound.PlayOnlyDefaultAb("风");
        poss = Common.PosSortByWidth(1200, 4, 280);
        mStationQuestion = new List<ShapeLogicGuanka1_Station>();
        for(int i = 0; i < 4; i++)
        {
            ShapeLogicGuanka1_Station station = UguiMaker.newGameObject("question" + i, transform).AddComponent<ShapeLogicGuanka1_Station>();
            mStationQuestion.Add(station);
            station.Init(true, i);
            //station.mRtran.anchoredPosition3D = poss[i];
            if( i < 2)
            {
                station.Fly(new Vector3(-800, 100, 0), poss[i], 0.015f);
            }
            else
            {
                station.Fly(new Vector3(800, 100, 0), poss[i], 0.015f);
            }
            station.SetCanUpDown(true);
            station.SetCanFly(true);
            if(3 == i)
            {
                //最后一个是用来填空
                station.SetBoxEnable(true);
            }
            else
            {
                station.SetBoxEnable(false);
            }
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(2f);
        mBgAnswer = UguiMaker.newImage("mBgAnswer", transform, "shapelogic_sprite", "guanka1_bg1", false);
        mBgAnswer.transform.SetSiblingIndex(1);
        mBgAnswer.type = Image.Type.Sliced;
        //mBgAnswer.rectTransform.sizeDelta = new Vector2(1060, 360);
        mBgAnswer.rectTransform.anchoredPosition3D = new Vector3(0, -200);
        for(float i = 0; i < 1f; i += 0.05f)
        {
            mBgAnswer.rectTransform.sizeDelta = Vector3.Lerp(new Vector3( 1060, 0), new Vector3(1060, 316), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mBgAnswer.rectTransform.sizeDelta = new Vector3(1060, 316);
        
        poss = Common.PosSortByWidth(mBgAnswer.rectTransform.sizeDelta.x, 4, -74.7f);
        poss = Common.BreakRank<Vector3>(poss);
        for (int i = 0; i < 4; i++)
        {
            ShapeLogicGuanka1_Station station = UguiMaker.newGameObject("answer" + i, transform).AddComponent<ShapeLogicGuanka1_Station>();
            mStationQuestion.Add(station);
            station.Init(false, i);
            station.mRtran.anchoredPosition3D = poss[i];
            station.Fly(poss[i] - new Vector3(0, 300, 0), poss[i], 0.05f);
            station.SetCanFly(false);
            station.SetCanUpDown(false);
            station.SetBoxEnable(true);
            station.SetResetPos(poss[i]);
        }



        ShapeLogicCtl.instance.mSound.PlayTipListDefaultAb(
            new List<string>() { "1观察长方形里面颜色的变化规律", "拖动正确的图案放到空白的长方形里" },
            new List<float>() { 1, 1 }, true);
            
    }
    IEnumerator TEndGame()
    {
        for(int i = 0; i < mStationQuestion.Count; i++)
        {
            Destroy(mStationQuestion[i].gameObject);
        }


        //yield break;
        Color bg_color0 = mBg.color;
        Color bg_color1 = mBg.color;
        
        bg_color1.a = 0;
        for (float j = 0; j < 1f; j += 0.03f)
        {
            Color cor = Color.Lerp(new Color(1, 1, 1, 0.5f), new Color(1, 1, 1, 0), j);
            for (int i = 0; i < mBgTiao.Length; i++)
            {
                mBgTiao[i].color = cor;
            }
            mBg.color = Color.Lerp(bg_color0, bg_color1, j);
            yield return new WaitForSeconds(0.01f);
        }

        //for (int i = 0; i < mBgTiao.Length; i++)
        //{
        //    Destroy(mBgTiao[i].gameObject);
        //}
        //for (float i = 0; i < 1f; i += 0.05f)
        //{
        //    transform.localScale = Vector3.Lerp(Vector3.one, Vector2.zero, i);
        //    yield return new WaitForSeconds(0.01f);
        //}

        //yield break;
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        gameObject.SetActive(false);
        mBg = null;
        mEffect = null;
        mBgAnswer = null;
        mStationQuestion.Clear();

        gameObject.SetActive(false);

    }
    IEnumerator TOver()
    {
        for(int i = 0; i < mStationQuestion.Count; i++)
        {
            mStationQuestion[i].SetBoxEnable(false);
        }
        yield return new WaitForSeconds(0.8f);
        ShapeLogicCtl.instance.mSound.PlayShort("胜利通关音乐", 1);
        for (float i = 0; i < 1f; i += 0.06f)
        {
            mBgAnswer.rectTransform.sizeDelta = Vector3.Lerp( new Vector3(1060, 316), new Vector3(1060, 0), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mBgAnswer.rectTransform.sizeDelta = new Vector3(1060, 0);

        for(int i = 0; i < mStationQuestion.Count; i++)
        {
            if(!mStationQuestion[i].mdata_is_question )
            {
                if(mStationQuestion[i].gameObject.activeSelf)
                {
                    mStationQuestion[i].KillPlayStar();
                }
                mStationQuestion.RemoveAt(i);
                i--;
            }
            else
            {
                
            }
        }

        yield return new WaitForSeconds(1);

        temp_isover = true;
        for (int i = 0; i < mStationQuestion.Count; i++)
        {
            mStationQuestion[i].Fly(mStationQuestion[i].mRtran.anchoredPosition3D, mStationQuestion[i].mRtran.anchoredPosition3D - new Vector3(0, 150, 0), 0.04f);
            mStationQuestion[i].SetBoxEnable(true);
        }








    }

}
