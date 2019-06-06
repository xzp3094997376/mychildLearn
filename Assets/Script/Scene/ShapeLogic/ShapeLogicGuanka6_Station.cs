using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka6_Station : MonoBehaviour {
    public static ShapeLogicGuanka6_Station gSelect { get; set; }
    public RectTransform mRtran { get; set; }
    Image mBg { get; set; }
    BoxCollider mBox { get; set; }
    List<Image> mStars = new List<Image>();
    List<Vector3> mPoss = new List<Vector3>();
    public Vector3 mResetPos = Vector3.zero;

    Vector2 mdata_bg_size = new Vector2(75, 130);

    public int mdata_num = 0;
    public bool mdata_is_question = false;
    public bool mdata_out_side = false;
    public bool mdata_is_over = false;


    public void Init(int num, bool out_side)
    {
        mdata_num = num;
        mdata_out_side = out_side;

        mRtran = gameObject.GetComponent<RectTransform>();
        mBox = gameObject.AddComponent<BoxCollider>();
        mBox.size = new Vector3(100, 136, 1 );

        mBg = UguiMaker.newImage("mBg", transform, "shapelogic_sprite", "guanka6_frame", false);
        mBg.rectTransform.sizeDelta = new Vector2(90, 130);

        for (int i = 0; i < num; i++)
        {
            mStars.Add(UguiMaker.newImage("mStar" + i.ToString(), transform, "shapelogic_sprite", "guanka6_star", false));

        }

        List<Vector3> poss4_inside = new List<Vector3>() { new Vector3(25, 0, 0), new Vector3(-25, 0, 0), new Vector3(0, 45, 0), new Vector3(0, -45, 0) };
        List<Vector3> poss4_outside = new List<Vector3>() { new Vector3(56, 0, 0), new Vector3(-56, 0, 0), new Vector3(0, 75, 0), new Vector3(0, -75, 0) };

        mPoss = new List<Vector3>();
        switch (num)
        {
            case 5:
                {
                    mPoss.Add(Vector3.zero);
                    for (int i = 0; i < 4; i++)
                    {
                        mPoss.Add(poss4_outside[i]);
                    }
                }
                break;
            case 4:
                {
                    if(out_side)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            mPoss.Add(poss4_outside[i]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            mPoss.Add(poss4_inside[i]);
                        }
                    }
                }
                break;
            case 3:
                {
                    int index = Random.Range(0, 1000) % 4;
                    if (out_side)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if(i != index)
                            {
                                mPoss.Add(poss4_outside[i]);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if(i != index)
                            {
                                mPoss.Add(poss4_inside[i]);
                            }
                        }
                    }
                }
                break;
            case 2:
                {
                    List<int> indexs = Common.GetMutexValue(0, 4, 2);
                    if (out_side)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (i != indexs[0] && i != indexs[1])
                            {
                                mPoss.Add(poss4_outside[i]);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (i != indexs[0] && i != indexs[1])
                            {
                                mPoss.Add(poss4_inside[i]);
                            }
                        }
                    }
                }
                break;
            case 1:
                {
                    if (out_side)
                    {
                        mPoss.Add(poss4_outside[ Random.Range(0, 1000) % 4]);
                    }
                    else
                    {
                        mPoss.Add(Vector3.zero);
                    }
                }
                break;
            default:
                {
                    Debug.LogError("数据错误");
                }
                break;
        }


    }
    public void SetBgFrameWhite()
    {
        mBg.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka6_frame1");

    }
    public void SetBoxEnable(bool _enable)
    {
        mBox.enabled = _enable;

    }
    public void SetQuestion()
    {
        mdata_is_question = true;
    }
    public void SetAnswer()
    {
        mdata_is_question = false;
    }
    public void SetResetPos(Vector3 pos)
    {
        mResetPos = pos;
    }
    public void ShowStart(bool is_show)
    {
        for(int i = 0; i < mStars.Count; i++)
        {
            if (is_show)
            {
                mStars[i].transform.localScale = Vector3.one;
                mStars[i].rectTransform.anchoredPosition3D = mPoss[i];
            }
            else
                mStars[i].transform.localScale = Vector3.zero;
        }
        if(is_show)
        {
            mBg.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka6_frame");

        }
    }


    public void ShowByAnimtion()
    {
        StartCoroutine(TShowByAnimtion());
    }
    IEnumerator TShowByAnimtion()
    {
        mRtran.localEulerAngles = new Vector3(0, 0, -90);
        Vector3 pos0 = mResetPos + new Vector3(-1300, 0, 0);
        for(float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, mResetPos, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition3D = mResetPos;


        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, -90), Vector3.zero, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.localEulerAngles = Vector3.zero;

        yield return new WaitForSeconds(0.2f);

        if(!ShapeLogicCtl.instance.mSound.IsPlayingOnly())
            ShapeLogicCtl.instance.mSound.PlayOnly("shapelogic_sound", "星星闪烁2");

        float speed = 0.1f;
        Vector3 scale = new Vector3(1.3f, 1.3f, 1);
        if(mdata_num != 1)
        {
            for (float i = 0; i < 1f; i += speed)
            {
                mRtran.localScale = Vector3.Lerp(Vector3.one, scale, Mathf.Sin(Mathf.PI * 0.5f * i));
                yield return new WaitForSeconds(0.01f);
            }
            

            for (float i = 0; i < 1f; i += speed)
            {
                mRtran.localScale = Vector3.Lerp(Vector3.one, scale, Mathf.Sin(Mathf.PI * 0.5f * i + Mathf.PI * 0.5f));
                for (int j = 0; j < mdata_num; j++)
                {
                    mStars[j].rectTransform.anchoredPosition3D = Vector3.Lerp(Vector3.zero, mPoss[j], i);
                }
                yield return new WaitForSeconds(0.01f);
            }
            for (int j = 0; j < mdata_num; j++)
            {
                mStars[j].rectTransform.anchoredPosition3D = mPoss[j];
            }
            mRtran.localScale = Vector3.one;


        }
        


        yield break;
    }
    public void ShowByLittleAnimtion()
    {
        StartCoroutine(TShowByLittleAnimtion());
    }
    IEnumerator TShowByLittleAnimtion()
    {
        mRtran.localEulerAngles = new Vector3(0, 0, -90);
        Vector3 pos0 = mResetPos + new Vector3(-1300, 0, 0);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, mResetPos, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition3D = mResetPos;


        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, -90), Vector3.zero, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.localEulerAngles = Vector3.zero;

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
        transform.SetAsLastSibling();
        StartCoroutine("TSelect");
    }
    public void UnSelect()
    {
        StopCoroutine("TSelect");
        for (int i = 0; i < mStars.Count; i++)
        {
            mStars[i].transform.localEulerAngles = Vector3.zero;
        }
    }
    IEnumerator TSelect()
    {
        while(true)
        {
            for(int i = 0; i < mStars.Count; i++)
            {
                mStars[i].transform.localEulerAngles += new Vector3(0, 0, -10);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void PlayError()
    {
        StartCoroutine("TPlayError");
    }
    IEnumerator TPlayError()
    {
        SetBoxEnable(false);

        Vector3 pos0 = mRtran.anchoredPosition3D;
        Vector3 pos1 = pos0 + new Vector3(0, -500, 0);
        for(float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos1, i);
            mRtran.localEulerAngles += new Vector3(0, 0, -5);
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

        yield break;
    }

    public void PlayOver()
    {
        StartCoroutine(TPlayOver());
    }
    IEnumerator TPlayOver()
    {
        while (true)
        {
            for (int i = 0; i < mStars.Count; i++)
            {
                mStars[i].transform.localEulerAngles += new Vector3(0, 0, -10);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void ShootOver()
    {
        mdata_is_over = true;
        transform.SetAsLastSibling();
        StartCoroutine(TShootOver());
    }
    IEnumerator TShootOver()
    {
        List<Vector3> dir = new List<Vector3>();
        if(mdata_num == 1)
        {
            switch (Random.Range(0, 1000) % 4)
            {
                case 0:
                    dir.Add(Vector3.right * 30);
                    break;
                case 1:
                    dir.Add(Vector3.left * 30);
                    break;
                case 2:
                    dir.Add(Vector3.up * 30);
                    break;
                case 3:
                    dir.Add(Vector3.down * 30);
                    break;
            }

        }
        else if(mdata_num == 5)
        {
            switch (Random.Range(0, 1000) % 4)
            {
                case 0:
                    dir.Add(Vector3.right * 30);
                    break;
                case 1:
                    dir.Add(Vector3.left * 30);
                    break;
                case 2:
                    dir.Add(Vector3.up * 30);
                    break;
                case 3:
                    dir.Add(Vector3.down * 30);
                    break;
            }
            for (int i = 0; i < 4; i++)
            {
                dir.Add((mStars[i + 1].rectTransform.anchoredPosition3D - Vector3.zero).normalized * 30);
            }

        }
        else
        {
            for (int i = 0; i < mStars.Count; i++)
            {
                dir.Add((mStars[i].rectTransform.anchoredPosition3D - Vector3.zero).normalized * 30);
            }
        }
        ShapeLogicCtl.instance.mSound.PlayShort("素材出去通用");
        
        bool temp = false;
        while(true)
        {
            for(int i = 0; i < mStars.Count; i++)
            {
                mStars[i].rectTransform.anchoredPosition3D += dir[i];
            }
            mBg.rectTransform.localEulerAngles -= new Vector3(0, 0, 10);
            if(0.1f < mBg.rectTransform.localScale.x)
            {
                mBg.rectTransform.localScale = mBg.rectTransform.localScale * 0.9f;
            }
            else if(!temp)
            {
                ShapeLogicCtl.instance.mSound.PlayShortDefaultAb("星星闪烁2");
                temp = true;
                mBg.rectTransform.localScale = Vector3.zero;
                ParticleSystem effect = ResManager.GetPrefab("effect_star0", "effect_star2").GetComponent<ParticleSystem>();
                UguiMaker.InitGameObj(effect.gameObject, transform, "effect", Vector3.zero, Vector3.one);
                effect.Emit(1);

            }
            yield return new WaitForSeconds(0.01f);
            
        }

    }

}
