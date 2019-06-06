using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChookPkCar : MonoBehaviour {
    public string mFlag = "";
    public Vector2 m_pos_stand { get; set; }

    RectTransform mRtran { get; set; }
    public Image mCar;
    List<ChookPkChook> mChooks = new List<ChookPkChook>();
    public bool uping = false;

    void Start ()
    {
        mRtran = gameObject.GetComponent<RectTransform>();
        mRtran.sizeDelta = Vector2.zero;

        mCar = UguiMaker.newImage("car", transform, "chookpk_sprite", "car", true);
        mCar.rectTransform.pivot = new Vector2(1, 0.5f);
        mCar.rectTransform.anchoredPosition = new Vector2(-22, 0);

        Button btn = mCar.gameObject.AddComponent<Button>();
        btn.onClick.AddListener(CarUp);
        btn.transition = Selectable.Transition.None;




        switch (mFlag)
        {
            case "left":
                m_pos_stand = new Vector2(-649, -348);
                transform.localScale = new Vector3(-1, 1, 1);
                break;
            case "right":
                m_pos_stand = new Vector2(649, -348);
                //transform.localScale = new Vector3(-1, 1, 1);

                break;
        }
        mRtran.anchoredPosition = m_pos_stand;


        ChookPkCtl.instance.temp_start_count--;

    }
    public void AddChook(ChookPkChook chook)
    {
        if(!uping && !mChooks.Contains(chook))
        {
            mChooks.Add(chook);
        }

    }
    public Vector2 GetRandomPos()
    {
        return new Vector2(-44, -5);
        //return new Vector2( Random.Range(-156.1f, -58.6f), -3);
    }


    public void CarUp()
    {
        //Debug.LogError("CarUp " + mFlag);
        if (0 == mChooks.Count || uping)
        {
            return;
        }
        bool can_up = true;
        for(int i = 0; i < mChooks.Count; i++)
        {
            if(mChooks[i].mIsRuning)
            {
                can_up = false;
                break;
            }
        }
        if (can_up)
        {
            StartCoroutine(TCarUp());
        }

    }
    IEnumerator TCarUp()
    {
        uping = true;
        ChookPkCtl.instance.mSound.PlayShort("chookpk_sound", "弹簧");
        for (int i = 0; i < mChooks.Count; i++)
        {
            mChooks[i].JumpInPulley();
        }
        mChooks.Clear();

        float angle = 330;
        for(float i = 0; i < 1f; i += 0.3f)
        {
            mCar.transform.localEulerAngles = Vector3.Lerp( new Vector3(0, 0, 360), new Vector3(0, 0, angle), i);
            yield return new WaitForSeconds(0.01f);
        }
        mCar.transform.localEulerAngles = new Vector3(0, 0, angle);

        //yield return new WaitForSeconds(0.5f);


        for (float i = 0; i < 1f; i += 0.1f)
        {
            mCar.transform.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, angle), new Vector3(0, 0, 360), i);
            yield return new WaitForSeconds(0.01f);
        }
        mCar.transform.localEulerAngles = new Vector3(0, 0, 360);


        uping = false;
        yield break;

        /*
        uping = true;
        float end = 0;
        switch(mFlag)
        {
            case "left":
                Vector3 pos_l = ChookPkCtl.instance.transform.worldToLocalMatrix.MultiplyPoint(ChookPkCtl.instance.mPulley.mLanziLeft.transform.position);
                end = pos_l.y - 100;
                break;
            case "right":
                Vector3 pos_r = ChookPkCtl.instance.transform.worldToLocalMatrix.MultiplyPoint(ChookPkCtl.instance.mPulley.mLanziRight.transform.position);
                end = pos_r.y - 100;
                break;
        }
        while(mRtran.anchoredPosition.y < end)
        {
            mRtran.anchoredPosition += new Vector2(0, 13);
            yield return new WaitForSeconds(0.01f);
        }

        Vector3 world_jump_pos = transform.localToWorldMatrix.MultiplyPoint(new Vector3(-140, 3, 0));
        Vector3 pulley_jump_pos = Vector3.zero;
        switch (mFlag)
        {
            case "left":
                pulley_jump_pos = ChookPkCtl.instance.mPulley.mLanziLeft.worldToLocalMatrix.MultiplyPoint(world_jump_pos);
                break;
            case "right":
                pulley_jump_pos = ChookPkCtl.instance.mPulley.mLanziRight.worldToLocalMatrix.MultiplyPoint(world_jump_pos);
                break;
        }

        for(int i = 0; i < mChooks.Count; i++)
        {
            Vector3 pulley_end_pos = ChookPkCtl.instance.mPulley.GetRandomPos(mFlag);
            mChooks[i].JumpInPulley(pulley_jump_pos, pulley_end_pos);
        }
        mChooks.Clear();
        while (mRtran.anchoredPosition.y > m_pos_stand.y)
        {
            mRtran.anchoredPosition -= new Vector2(0, 13);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition = m_pos_stand;

        uping = false;
        */
    }

}
