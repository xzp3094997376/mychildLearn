using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeLogicGuanka8_Station : MonoBehaviour
{
    public static ShapeLogicGuanka8_Station gSelect { get; set; }
    public RectTransform mRtran { get; set; }
    public RectTransform mRtranMid { get; set; }
    public RectTransform mRtranDown { get; set; }
    Image mQuestionBg { get; set; }
    Image mLeft;
    Image[] mMid;
    Image[] mDown;
    RectTransform mRootMid;

    BoxCollider mBox { get; set; }

    public int mdata_left = -2;
    public int mdata_mid = -2;
    public int mdata_down = -2;
    int temp_left = -2;
    int temp_mid = -2;
    int temp_down = -2;

    public bool mdata_is_question = false;
    public bool mdata_is_over = false;

    public void Init(int left, int mid, int down)
    {
        mdata_left = left;
        mdata_mid = mid;
        mdata_down = down;

        mRtran = gameObject.GetComponent<RectTransform>();
        mRootMid = UguiMaker.newGameObject("mRootMid", transform).GetComponent<RectTransform>();
        mRtranMid = UguiMaker.newGameObject("mRtranMid", mRootMid).GetComponent<RectTransform>();
        mRtranDown = UguiMaker.newGameObject("mRtranDown", transform).GetComponent<RectTransform>();
        mRtranDown.anchoredPosition = new Vector2(0, -98);

        mLeft = UguiMaker.newImage("mLeft", mRootMid, "shapelogic_sprite", "guanka8_station0-0", false);
        mLeft.rectTransform.anchoredPosition = new Vector2(-138, 0);


        mMid = new Image[6];
        mMid[0] = UguiMaker.newImage("mMid0", mRtranMid, "shapelogic_sprite", "guanka8_station0-0", false);
        mMid[1] = UguiMaker.newImage("mMid1", mRtranMid, "shapelogic_sprite", "guanka8_station0-0", false);
        mMid[2] = UguiMaker.newImage("mMid2", mRtranMid, "shapelogic_sprite", "guanka8_station0-0", false);
        mMid[3] = UguiMaker.newImage("mMid3", mRtranMid, "shapelogic_sprite", "guanka8_station0-0", false);
        mMid[4] = UguiMaker.newImage("mMid4", mRtranMid, "shapelogic_sprite", "guanka8_station0-0", false);
        mMid[5] = UguiMaker.newImage("mMid5", mRtranMid, "shapelogic_sprite", "guanka8_station0-0", false);
        mMid[0].type = Image.Type.Sliced;

        mDown = new Image[4];
        mDown[0] = UguiMaker.newImage("mDown0", mRtranDown, "shapelogic_sprite", "guanka8_station1-0", false);
        mDown[1] = UguiMaker.newImage("mDown1", mRtranDown, "shapelogic_sprite", "guanka8_station1-0", false);
        mDown[2] = UguiMaker.newImage("mDown2", mRtranDown, "shapelogic_sprite", "guanka8_station1-0", false);
        mDown[3] = UguiMaker.newImage("mDown3", mRtranDown, "shapelogic_sprite", "guanka8_station1-0", false);

        mBox = gameObject.AddComponent<BoxCollider>();
        mBox.size = new Vector3(271, 206, 1);
        mBox.center = new Vector3(-27, -28, 0);

        SetLeft(left);
        SetMid(mid);
        SetDown(down);

    }
    public void SetBoxEnable(bool _enable)
    {
        mBox.enabled = _enable;

    }
    public void SetLeft(int left)
    {
        mLeft.sprite = ResManager.GetSprite("shapelogic_sprite", "guanka8_station0-" + left);
    }
    public void SetMid(int mid)
    {
        mMid[0].sprite = ResManager.GetSprite("shapelogic_sprite", "guanka8_station2-0");
        mMid[0].rectTransform.sizeDelta = new Vector2(216, 146);
        switch (mid)
        {
            case 0:
                mMid[1].gameObject.SetActive(true);
                mMid[1].sprite = ResManager.GetSprite("shapelogic_sprite", "guanka8_station3-1");
                mMid[1].type = Image.Type.Sliced;
                mMid[1].color = Color.white;
                mMid[1].rectTransform.anchoredPosition = new Vector2(-73.4f, -44f);
                mMid[1].rectTransform.sizeDelta = new Vector2(68.6f, 55f);
                for (int i = 2; i < 6; i++)
                {
                    mMid[i].gameObject.SetActive(true);
                    mMid[i].sprite = ResManager.GetSprite("public", "white");
                    mMid[i].type = Image.Type.Sliced;
                    mMid[i].color = new Color32(116, 25, 174, 255);
                }
                mMid[2].rectTransform.sizeDelta = new Vector2(4, 140f);
                mMid[3].rectTransform.sizeDelta = new Vector2(4, 140f);
                mMid[2].rectTransform.anchoredPosition = new Vector2(-40, 0);
                mMid[3].rectTransform.anchoredPosition = new Vector2(40, 0);
                mMid[4].rectTransform.sizeDelta = new Vector2(65.56f, 4);
                mMid[5].rectTransform.sizeDelta = new Vector2(65.56f, 4);
                mMid[4].rectTransform.anchoredPosition = new Vector2(73, -17);
                mMid[5].rectTransform.anchoredPosition = new Vector2(-73, -17);

                break;
            case 1:
                mMid[1].gameObject.SetActive(true);
                mMid[1].sprite = ResManager.GetSprite("shapelogic_sprite", "guanka8_station3-0");
                mMid[1].type = Image.Type.Simple;
                mMid[1].color = Color.white;
                mMid[1].SetNativeSize();
                mMid[1].rectTransform.anchoredPosition = new Vector2(-0, -28.2f);

                for (int i = 2; i < 6; i++)
                {
                    mMid[i].gameObject.SetActive(false);

                }
                break;
            case 2:
                for (int i = 1; i < 5; i++)
                {
                    mMid[i].gameObject.SetActive(true);
                    mMid[i].sprite = ResManager.GetSprite("public", "white");
                    mMid[i].type = Image.Type.Sliced;
                    mMid[i].color = new Color32(116, 25, 174, 255);
                }
                mMid[5].gameObject.SetActive(false);
                mMid[1].rectTransform.sizeDelta = new Vector2(210, 4);
                mMid[2].rectTransform.sizeDelta = new Vector2(210, 4);
                mMid[1].rectTransform.anchoredPosition = new Vector2(0, 30);
                mMid[2].rectTransform.anchoredPosition = new Vector2(0, -30);
                mMid[3].rectTransform.sizeDelta = new Vector2(4, 60);
                mMid[4].rectTransform.sizeDelta = new Vector2(4, 60);
                mMid[3].rectTransform.anchoredPosition = new Vector2(-40, 0);
                mMid[4].rectTransform.anchoredPosition = new Vector2(40, 0);

                break;
        }

    }
    public void SetDown(int down)
    {
        List<Vector2> poss = new List<Vector2>();
        switch (down)
        {
            case 0:
                poss.Add(new Vector2(-27, 0));
                poss.Add(new Vector2(27, 0));
                break;
            case 1:
                poss.Add(new Vector2(-54, 0));
                poss.Add(new Vector2(0, 0));
                poss.Add(new Vector2(54, 0));
                break;
            case 2:
                poss.Add(new Vector2(-27, 0));
                poss.Add(new Vector2(27, 0));
                poss.Add(new Vector2(-81, 0));
                poss.Add(new Vector2(81, 0));
                break;
        }
        for(int i = 0; i < 4; i++)
        {
            if(i < poss.Count)
            {
                mDown[i].gameObject.SetActive(true);
                mDown[i].rectTransform.anchoredPosition = poss[i];

            }
            else
            {
                mDown[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowLeft(int type)
    {
        temp_left = type;
        StopCoroutine("TShowLeft");
        StartCoroutine("TShowLeft");
    }
    public void ShowMid(int type)
    {
        temp_mid = type;
        StopCoroutine("TShowMid");
        StartCoroutine("TShowMid");

    }
    public void ShowDown(int type)
    {
        temp_down = type;
        StopCoroutine("TShowDown");
        StartCoroutine("TShowDown");

    }
    float temp_show_speed = 0.2f;
    IEnumerator TShowLeft()
    {
        if(mLeft.transform.localScale.x > 0.5f)
        {
            for (float i = 0; i < 1f; i += temp_show_speed)
            {
                mLeft.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
                mLeft.transform.localEulerAngles = Vector3.Lerp( Vector3.zero, new Vector3(0, 0, -180), i);
                yield return new WaitForSeconds(0.01f);
            }
        }
        SetLeft(temp_left);
        for (float i = 0; i < 1f; i += temp_show_speed)
        {
            mLeft.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i);
            mLeft.transform.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 180), Vector3.zero, i);
            yield return new WaitForSeconds(0.01f);
        }

        mLeft.transform.localScale = Vector3.one;
        mLeft.transform.localEulerAngles = Vector3.zero;

    }
    IEnumerator TShowMid()
    {
        if (mRtranMid.transform.localScale.x > 0.5f)
        {
            for (float i = 0; i < 1f; i += temp_show_speed)
            {
                mRtranMid.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
                mRtranMid.localEulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, -180), i);
                yield return new WaitForSeconds(0.01f);
            }
        }
        SetMid(temp_mid);
        for (float i = 0; i < 1f; i += temp_show_speed)
        {
            mRtranMid.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i);
            mRtranMid.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 180), Vector3.zero, i);
            yield return new WaitForSeconds(0.01f);
        }

        mRtranMid.localScale = Vector3.one;
        mRtranMid.localEulerAngles = Vector3.zero;

    }
    IEnumerator TShowDown()
    {
        if (mRtranDown.transform.localScale.x > 0.5f)
        {
            for (float i = 0; i < 1f; i += temp_show_speed)
            {
                mRtranDown.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
                mRtranDown.transform.localEulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, -180), i);
                yield return new WaitForSeconds(0.01f);
            }
        }
        SetDown(temp_down);
        for (float i = 0; i < 1f; i += temp_show_speed)
        {
            mRtranDown.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i);
            mRtranDown.transform.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 180), Vector3.zero, i);
            yield return new WaitForSeconds(0.01f);
        }

        mRtranDown.transform.localScale = Vector3.one;
        mRtranDown.transform.localEulerAngles = Vector3.zero;

    }

    public void ShowQuestionBg(float delay)
    {
        mLeft.transform.localScale = Vector3.zero;
        mRtranMid.localScale = Vector3.zero;
        mRtranDown.transform.localScale = Vector3.zero;
        

        StartCoroutine(TShowQuestionBg(delay));
    }
    IEnumerator TShowQuestionBg(float delay)
    {
        yield return new WaitForSeconds(delay);
        mQuestionBg = UguiMaker.newImage("mQuestionBg", transform, "shapelogic_sprite", "guanka8_station2-1", false);
        mQuestionBg.type = Image.Type.Sliced;
        mQuestionBg.transform.SetAsFirstSibling();
        mQuestionBg.rectTransform.anchoredPosition = new Vector2(-28, -26);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mQuestionBg.rectTransform.sizeDelta = Vector2.Lerp(Vector2.zero, new Vector2(304, 228), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void HideQuestionBg()
    {
        StartCoroutine("THideQuestionBg");
    }
    IEnumerator THideQuestionBg()
    {
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mQuestionBg.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(304, 228), Vector2.zero, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(mQuestionBg.gameObject);
        mQuestionBg = null;
    }

    public void Run()
    {
        StopCoroutine("TRun");
        StartCoroutine("TRun");
    }
    IEnumerator TRun()
    {
        while(true)
        {
            for(float i = 0; i < 1f; i += 0.02f)
            {
                mRootMid.anchoredPosition = new Vector2(0, Mathf.Sin(Mathf.PI * 2 * i) * 3 - 5);
                mRootMid.localEulerAngles = new Vector3(0, 0, Mathf.Cos(Mathf.PI * 2 * i) * 1);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    public void GotoPos(Vector3 pos, float delay)
    {
        StartCoroutine(TGotoPos(pos, delay));
    }
    IEnumerator TGotoPos(Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 pos0 = mRtran.anchoredPosition3D;
        for(float i = 0; i < 1f; i += 0.015f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition3D = pos;
        StopCoroutine("TRun");
        mRootMid.anchoredPosition = Vector3.zero;
        mRootMid.localEulerAngles = Vector3.zero;

    }
    
    

    public void PlayError()
    {
        ShapeLogicCtl.instance.mSound.PlayShort("错误");
        StartCoroutine("TPlayError");
    }
    IEnumerator TPlayError()
    {
        for(float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * i) * 5);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.localEulerAngles = Vector3.zero;

        
    }

    public void PlayOver()
    {
        StartCoroutine(TPlayOver());
    }
    IEnumerator TPlayOver()
    {
        while (true)
        {
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
        AudioSource sound = gameObject.AddComponent<AudioSource>();
        sound.clip = ResManager.GetClip("shapelogic_sound", "car_run");
        sound.loop = true;
        sound.Play();
        
        Vector3 pos0 = mRtran.anchoredPosition3D;
        Vector3 pos1 = pos0 + new Vector3(-1423, 0, 0);
        for (float i = 0; i < 1f; i += 0.01f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i - Mathf.PI * 0.5f) + 1);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition3D = pos1;
        sound.Stop();

    }


}
