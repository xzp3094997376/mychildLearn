using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Spine.Unity;

public class ItemSpineCtr : MonoBehaviour {
    public static ItemSpineCtr gSelect = null;
    public SkeletonGraphic mSpine { get; set; }
    public RectTransform mrtran { get; set; }
    public int mNum { get; set; }
    public Image mImage { get; set; }
    public string mAnimalName { get; set; }
    public bool isEvent { get; set; }
    public string type { get; set; }
    private Vector3 mdefaultPos { get; set; }
    public int id { get; set; }
    public bool isFix { get; set; }
    private string roleType = "";
    private Image shadow { get; set; }
    // Use this for initialization

    void Start () {
       
        //mSpine.Skeleton.
    }
    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = Vector3.zero;
        if (transform.position.y - transform.parent.position.y >= 0)
        {
            if (roleType != "zheng")
            {
                roleType = "zheng";
                //transform.Rotate(0, 0, 0);
            }
            transform.Rotate(0, 0, 0);
        }
        else
        {
            if (roleType != "fan")
            {
                roleType = "fan";
                //transform.Rotate(0, 180, 0);
            }
            transform.Rotate(0, 180, 0);
        }
        
    }
    public void setData(string _mAnimalName,bool _isEvent = false,string  _type = "",int _i = 0)
    {
        type = _type;
        mAnimalName = _mAnimalName;
        isEvent = _isEvent;
        id = _i;
    }
    public void show()
    {
        gameObject.SetActive(true);
        StartCoroutine("TsetPos");
    }
    IEnumerator TsetPos()
    {
        Vector3 scale0 = new Vector3(0.5f, 0.5f, 1);
        Vector3 scale1 = new Vector3(1, 1, 1);
        for (float j = 0; j < 1f; j += 0.1f)
        {
            //transform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, , Vector3.one * 0.4f, Mathf.Sin(j));
            float p = Mathf.Sin(Mathf.PI * j) * 0.25f;
            transform.localScale = Vector3.Lerp(scale0, scale1, j) + new Vector3(p, p, 0);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = scale1;
        if (null == mSpine)
        {
            mrtran = GetComponent<RectTransform>();
            mSpine = transform.Find("spine").GetComponent<SkeletonGraphic>();
        }
        mSpine.AnimationState.SetAnimation(1, "Idle", true);

    }
    public void closeAnimal()
    {
        if(null == mImage)
        {
            mImage = UguiMaker.newImage("image", transform, "animalsort_sprite", "xuxian");
            mImage.rectTransform.localPosition = new Vector3(0, 212, 0);
        }
        mImage.enabled = true;
        if (null != mSpine)
        {
            mSpine.enabled = false;
        }
    }
    public void showAnimal()
    {
        if (null != mImage)
        {
            mImage.enabled = false;
        }
        if (null != mSpine)
        {
            mSpine.enabled = true;
        }
    }
    public void PlaySpine(string name, bool isloop)
    {
        if(null == mSpine)
        {
            mrtran = GetComponent<RectTransform>();
            mSpine = transform.Find("spine").GetComponent<SkeletonGraphic>();
            mSpine.timeScale = 0.6f;
        }
        if (isloop)
        {
            mSpine.AnimationState.SetAnimation(1, name, isloop);
        }
        else
        {
            mSpine.AnimationState.ClearTracks();
            mSpine.AnimationState.AddAnimation(1, name, false, 0);
            mSpine.AnimationState.AddAnimation(1, "Idle", true, 0);//animalsort:Idle_1;classify:Idle

        }

    }

    public static void cleanRoadData()
    {
        tempShootPos.Clear();
        temp_i.Clear();
    }

    private Vector3 getPos(int _id)
    {

        float disAngel = 360 / FunnyGroupCtr.instance.mGuanka.allRole;
        float angle = ((360 - _id * disAngel) / 180) * Mathf.PI;
        float vx = 0;
        float vy = 0;
        vx = Mathf.Sin(angle) * FunnyGroupCtr.mR;
        vy = Mathf.Cos(angle) * FunnyGroupCtr.mR;
        Vector3 pos = new Vector3(vx, vy, 0);
        return pos;
    }
    public void ShootIn(int _id)
    {
        id = _id;
         //Vector3 pos = getPos(_id);

        StartCoroutine(TShootIn(true));//TShootIn
    }
    public void MoveTo(Vector3 pos, bool isback = false)
    {
        StartCoroutine(TMoveTo(pos, isback));
    }
    IEnumerator TMoveTo(Vector3 pos,bool isback = false)
    {
        Vector3 startPos = transform.localPosition;
        for (float i = 0; i < 1f; i += 0.05f)
        {
            transform.localPosition = Vector3.Lerp(startPos, pos, i);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = pos;

        if (isback)
        {
            PlaySpine("Click", true);
        }else
        {
            PlaySpine("Idle", false);
        }
    }

    private void resetData()
    {
        mdefaultPos = transform.localPosition;
    }
    
    //归位设置
    public void setDefaultPos()
    {
        MoveTo(mdefaultPos, true);
    }
    public void Select()
    {
        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }

    public void RemovePosState()
    {
        tempShootPos.RemoveAt(tempShootPos.Count - 1);
        //temp_i.Remove(this);
    }
    
    static List<Vector3> tempShootPos = new List<Vector3>();
    static Dictionary<ItemSpineCtr, string> temp_i = new Dictionary<ItemSpineCtr, string>();
    bool ShootStop(bool is_in, int pass, float i)
    {
        if (!is_in) return false;
        if (0 == tempShootPos.Count)
        {
            return false;
        }
        else
        {
            if(FunnyGroupCtr.mD > Vector3.Distance( tempShootPos[tempShootPos.Count - 1], transform.localPosition))
            {
                tempShootPos.Add(transform.localPosition);
                if(temp_i.ContainsKey(this))
                {
                    temp_i[this] = pass + "-" + i;
                }
                else
                {
                    temp_i.Add(this, pass + "-" + i);
                }
                return true;
            }
        }
        return false;
    }
    void SetShootEndPos()
    {
        Vector3 pos = tempShootPos[tempShootPos.Count - 2] + (transform.localPosition - tempShootPos[tempShootPos.Count - 2]).normalized * FunnyGroupCtr.mN;
            //(beg - end).normalized * 88;
        tempShootPos[tempShootPos.Count - 1] = pos;
        resetData();
        //transform.localPosition = getPos(id);
        StartCoroutine(TSmoothPos(getPos(id)));
    }

    IEnumerator TEnterTO(Vector3 enPos)
    {
        float r = FunnyGroupCtr.mR;
        float i = 0;
        Vector3 pos0 = new Vector3(-GlobalParam.screen_width * 0.5f, 0, 0);
        Vector3 pos1 = new Vector3(0, 0, 0);
        /*
        for (; i < 1f; i += 0.03f)
        {
            transform.localPosition = Vector3.Lerp(pos0, pos1, i);
            yield return new WaitForSeconds(0.01f);
        }
        */
        //pos0 = pos1;
        pos1 = enPos;
        Debug.Log("enPos : " + enPos);
        i = 0;
        for (; i < 1f; i += 0.02f)
        {
            transform.localPosition = Vector3.Lerp(pos0, pos1, i);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = enPos;

        resetData();
    }

    IEnumerator TShootIn(bool is_in)
    {
        float r = FunnyGroupCtr.mR;
        float i = 0;
        float speed = 0.05f;
        int pass = 0;
        float s = FunnyGroupCtr.mS;//0.05f;
        float s_add = 0f;
        Vector3 pos0 = new Vector3(-GlobalParam.screen_width * 0.5f, r, 0);
        Vector3 pos1 = new Vector3(0, r, 0);
        Vector3 pos2 = Vector3.zero;

        for (; i < 1f; i += 0.05f)
        {
            transform.localPosition = Vector3.Lerp(pos0, pos1, i);
            yield return new WaitForSeconds(0.01f);
        }

        //s += s_add;

        //走半圆环pass=1
        pos0 = pos1;
        pos1 = new Vector3(0, -r, 0);
        if (is_in || (!is_in && pass <= 1))
        {
            //初始值
            if (is_in || pass != 1)
            {
                i = 0;
                speed = s;
            }
            else
            {
                string[] str = temp_i[this].Split('-');
                pass = int.Parse(str[0]);
                i = float.Parse(str[1]);
                speed = s;// / (1 - i);
            }
            //走
            for (; i < 1f; i += speed)
            {
                transform.localPosition = pos2 + new Vector3(
                    r * Mathf.Sin(Mathf.PI * i),
                    r * Mathf.Cos(Mathf.PI * i),
                    0
                    );
                if (ShootStop(is_in, 1, i))
                {
                    SetShootEndPos();
                    yield break;
                }
                yield return new WaitForSeconds(0.01f);

            }
        }
        s += s_add;


        //走后半圆环pass=2
        pos0 = pos1;
        pos1 = new Vector3(-r, 0, 0);
        if (is_in || (!is_in && pass <= 1))
        {
            //初始值
            if (is_in || pass != 2)
            {
                i = 0;
                speed = s;
            }
            else
            {
                string[] str = temp_i[this].Split('-');
                pass = int.Parse(str[0]);
                i = float.Parse(str[1]);
                speed = s;// / (1 - i);
            }
            //走
            for (; i < 1f; i += speed)
            {
                transform.localPosition = pos2 + new Vector3(
                    r * -Mathf.Sin(Mathf.PI * i),
                    r * -Mathf.Cos(Mathf.PI * i),
                    0
                    );
                if (ShootStop(is_in, 2, i))
                {
                    SetShootEndPos();
                    yield break;
                }
                yield return new WaitForSeconds(0.01f);

            }
            tempShootPos.Add(transform.localPosition);
            if (temp_i.ContainsKey(this))
            {
                temp_i[this] = "2-1";
            }
            else
            {
                temp_i.Add(this, "2-1");
            }
        }
        s += s_add;
        resetData();
    }
    ///*
    IEnumerator TSmoothPos(Vector3 endpos)
    {
        Vector3 pos0 = transform.localPosition;
        for(float i = 0; i < 1f; i += 0.2f)
        {
            transform.localPosition = Vector3.Lerp(pos0, endpos, i);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = endpos;
        resetData();
    }
   // */

}
