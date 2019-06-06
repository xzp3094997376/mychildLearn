using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShapeLogicGuanka1_Station : MonoBehaviour
{
    public static ShapeLogicGuanka1_Station gSelect = null;

    public bool mdata_is_question { get; set; }
    public int mdata_index { get; set; }

    public RectTransform mRtran { get; set; }
    Image mAnchor { get; set; }
    Image mPack { get; set; }
    BoxCollider mBox { get; set; }
    Vector3 mResetPos = Vector3.zero;

    bool can_fly = false;
    bool can_up_down = false;
    float flyp = 0;
    void Update()
    {
        if(can_fly && null != mAnchor)
        {
            mAnchor.transform.localEulerAngles += new Vector3(0, 0, 20);
            if(can_up_down)
            {
                mAnchor.rectTransform.anchoredPosition3D = new Vector3(0, Mathf.Sin(flyp) * 8, 0);
                mPack.rectTransform.anchoredPosition3D = mAnchor.rectTransform.anchoredPosition3D;
                flyp += 0.05f;
            }

        }


    }

    public void Init(bool is_question, int index)
    {
        transform.localScale = new Vector3(0.8f, 0.8f, 1);

        mdata_is_question = is_question;
        mdata_index = index;

        mRtran = gameObject.GetComponent<RectTransform>();

        if (is_question)
        {
            mPack = UguiMaker.newImage("mPack", transform, "shapelogic_sprite", "guanka1_question" + index, false);
        }
        else
        {
            mPack = UguiMaker.newImage("mPack", transform, "shapelogic_sprite", "guanka1_answer" + index, false);

        }
        mPack.rectTransform.pivot = new Vector2(0.5f, 1f);
        mPack.rectTransform.anchoredPosition3D = new Vector3(0, 10.82f, 0);
        //mPack.transform.localScale = Vector3.zero;
        mAnchor = UguiMaker.newImage("mAnchor", transform, "shapelogic_sprite", "guanka1_anchor", false);

        mBox = gameObject.AddComponent<BoxCollider>();
        mBox.center = new Vector3(0, -148.76f, 0);
        mBox.size = new Vector3(214.4f, 333.85f, 1);

        //StartCoroutine(TQuestionShow());

    }
    public void SetCanFly(bool canFly)
    {
        can_fly = canFly;
        if(can_fly)
        {
            flyp = 0;
        }
    }
    public void SetCanUpDown(bool canUpDown)
    {
        can_up_down = canUpDown;
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
        gSelect.SetCanFly(true);
        gSelect.transform.SetAsLastSibling();
        mAnchor.transform.localScale = new Vector3(2, 2, 1);

    }
    public void SetPackSprite(string sprite_name)
    {
        mPack.sprite = ResManager.GetSprite("shapelogic_sprite", sprite_name);
    }


    public void Fly(Vector3 begin_pos, Vector3 end_pos, float speed)
    {
        StartCoroutine(TFly(begin_pos, end_pos, speed));
    }
    IEnumerator TFly(Vector3 begin_pos, Vector3 end_pos, float speed)
    {
        for(float i = 0; i < 1f; i += speed)
        {

            mRtran.anchoredPosition3D = Vector3.Lerp(begin_pos, end_pos, i);// + new Vector3(0, Mathf.Sin(Mathf.PI * 4 * i) * 50, 0);
            yield return new WaitForSeconds(0.01f);
        }
        if(!can_up_down)
        {
            can_fly = false;
            mRtran.anchoredPosition3D = end_pos;
        }

    }

    public void ResetPos()
    {
        StartCoroutine("TResetPos");
    }
    IEnumerator TResetPos()
    {
        SetBoxEnable(false);

        while(40 < Vector3.Distance(mResetPos, mRtran.anchoredPosition3D))
        {
            Vector3 dir = (mResetPos - mRtran.anchoredPosition3D).normalized * 40;
            dir.z = 0;
            mRtran.anchoredPosition3D += dir;

            yield return new WaitForSeconds(0.01f);

        }

        mRtran.anchoredPosition3D = mResetPos;
        SetBoxEnable(true);
        SetCanFly(false);
        mAnchor.transform.localEulerAngles = Vector3.zero;
        mAnchor.transform.localScale = Vector3.one;

    }

    public void PlayError()
    {
        StartCoroutine(TPlayError());
    }
    IEnumerator TPlayError()
    {
        SetBoxEnable(false);
        SetCanFly(true);
        mAnchor.transform.localScale = Vector3.one;

        float speed = 0;
        while(true)
        {
            mAnchor.rectTransform.anchoredPosition3D += new Vector3(0, 10, 0);
            mPack.rectTransform.anchoredPosition3D += new Vector3(0, -speed, 0);
            yield return new WaitForSeconds(0.01f);

            speed += 1f;
            if(mAnchor.rectTransform.anchoredPosition3D.y + mRtran.anchoredPosition3D.y > 420 && mPack.rectTransform.anchoredPosition3D.y + mRtran.anchoredPosition3D.y < -500 )
            {
                break;
            }
        }

        //yield return new WaitForSeconds(1f);

        SetCanFly(false);
        mAnchor.transform.localEulerAngles = Vector3.zero;
        mAnchor.rectTransform.anchoredPosition3D = Vector3.zero;
        mPack.rectTransform.anchoredPosition3D = Vector3.zero;
        Fly(mResetPos + new Vector3(0, -300, 0), mResetPos, 0.03f);




        SetBoxEnable(true);

    }

    public void KillPlayStar()
    {
        StartCoroutine("TKillPlayStar");
    }
    IEnumerator TKillPlayStar()
    {
        Vector3 pos0 = mRtran.anchoredPosition3D;
        Vector3 pos1 = pos0 + new Vector3(0, -150, 0);
        for(float i = 0; i < 1f; i += 0.08f)
        {
            Vector3 scale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
            mAnchor.transform.localScale = scale;
            mPack.transform.localScale = scale;
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos1, i);
            yield return new WaitForSeconds(0.01f);

        }
        mAnchor.transform.localScale = Vector3.zero;
        mPack.transform.localScale = Vector3.zero;
        GameObject obj = ResManager.GetPrefab("effect_star0", "effect_star1");
        UguiMaker.InitGameObj(obj, transform, "effect_star0", Vector3.zero, Vector3.one);
        obj.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(3);
        Destroy(gameObject);

        

    }

    public void Scale2Zero()
    {
        StartCoroutine(TScale2Zero());
    }
    IEnumerator TScale2Zero()
    {
        Vector3 pos0 = mRtran.anchoredPosition3D;
        Vector3 pos1 = pos0 + new Vector3(0, -100, 0);
        for(float i = 0; i < 1f; i += 0.05f)
        {
            mPack.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, Mathf.Sin(Mathf.PI * 0.5f * i));
            mAnchor.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(2, 2, 1), Mathf.Sin(Mathf.PI * 0.5f * i));
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos1, i);
            yield return new WaitForSeconds(0.01f);

        }

        Vector3 pos2 = Vector3.zero;
        switch (Random.Range(0, 1000) % 4)
        {
            case 0:
                {
                    pos2.x = -800;
                    pos2.y = Random.Range(-600, 600);
                }
                break;
            case 1:
                {
                    pos2.x = 800;
                    pos2.y = Random.Range(-600, 600);

                }
                break;
            case 2:
                {
                    pos2.y = 600;
                    pos2.x = Random.Range(-800, 800);
                }
                break;
            case 3:
                {
                    pos2.y = -600;
                    pos2.x = Random.Range(-800, 800);
                }
                break;

        }
        //for (float i = 0; i < 1f; i += 0.05f)
        //{
        //    mRtran.anchoredPosition3D = Vector3.Lerp(pos1, pos2, i);
        //    yield return new WaitForSeconds(0.01f);

        //}

        yield return new WaitForSeconds(0.3f);
        while(20 < Vector3.Distance(mRtran.anchoredPosition3D, pos2))
        {
            Vector3 dir = (pos2 - mRtran.anchoredPosition3D).normalized;
            mRtran.anchoredPosition3D += dir * 20;

            yield return new WaitForSeconds(0.01f);
        }

        ShapeLogicCtl.instance.mGuankaCtl1.callback_over();



    }


}
