using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Spine.Unity;

public class SpineCtr : MonoBehaviour {
    public static SpineCtr gSelect = null;
    public static SpineCtr gCSelect = null;
    public SkeletonGraphic mSpine { get; set; }
    public RectTransform mrtran { get; set; }
    public int mNum { get; set; }
    public Image mImage { get; set; }
    public string mAnimalName { get; set; }
    public bool isEvent { get; set; }
    public string type { get; set; }
    private Vector3 mdefaultPos { get; set; }
    public int id { get; set; }
    // Use this for initialization

    void Start () {
       
        //mSpine.Skeleton.
    }
    // Update is called once per frame
    void Update()
    {

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
        }
        if (isloop)
        {
            mSpine.AnimationState.SetAnimation(1, name, isloop);
        }
        else
        {
            mSpine.AnimationState.ClearTracks();
            mSpine.AnimationState.AddAnimation(1, name, false, 0);
            mSpine.AnimationState.AddAnimation(1, "Idle_1", true, 0);//animalsort:Idle_1;classify:Idle

        }

    }

    public static void cleanRoadData()
    {
        tempShootPos.Clear();
        temp_i.Clear();
    }
    public void pushToRaod()
    {
        StartCoroutine(TShootIn(true));
    }
    public void PopOut()
    {
        StartCoroutine(TShootIn(false));
    }
    //设置初始位置
    public void setDefaultPosData(Vector3 _dePos)
    {
        mdefaultPos = _dePos;

    }
    //归位设置
    public void setDefaultPos()
    {
        StartCoroutine(TsetDefaultPos());
    }

    IEnumerator TsetDefaultPos()
    {
        if(null != SpineCtr.gCSelect)
        {
            if(SpineCtr.gCSelect.type != "answer")
            {
                for (float j = 0; j < 1f; j += 0.1f)
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, mdefaultPos, j);
                    yield return new WaitForSeconds(0.01f);
                }
                transform.localPosition = mdefaultPos;
                Select();
                SpineCtr.gCSelect.gameObject.SetActive(true);
            }
            else
            {
                Select();
                SpineCtr.gCSelect.Select();
            }

        }else
        {
            Select();
        }
        AnimalSortCtr.opState = true;
        
    }
    //
    public void onChoose()
    {
        StartCoroutine(TonChoose());
    }
    //自动缩放
    public void Scale()
    {
        StartCoroutine("TScale");
    }

    public void Select()
    {
        //Debug.Log("Destroy");
        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }

    public void RemovePosState()
    {
        tempShootPos.RemoveAt(tempShootPos.Count - 1);
        //temp_i.Remove(this);
    }
    
    static List<Vector3> tempShootPos = new List<Vector3>();
    static Dictionary<SpineCtr, string> temp_i = new Dictionary<SpineCtr, string>();
    bool ShootStop(bool is_in, int pass, float i)
    {
        if (!is_in) return false;
        if (0 == tempShootPos.Count)
        {
            return false;
        }
        else
        {
            if(95 > Vector3.Distance( tempShootPos[tempShootPos.Count - 1], transform.localPosition))
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
    void SetShootEndPos(float r = 80)
    {
        Vector3 pos = tempShootPos[tempShootPos.Count - 2] + (transform.localPosition - tempShootPos[tempShootPos.Count - 2]).normalized * r;
            //(beg - end).normalized * 88;
        tempShootPos[tempShootPos.Count - 1] = pos;
        StartCoroutine(TSmoothPos(pos));
    }
    IEnumerator TShootIn(bool is_in)
    {
        Vector3 pos0 = new Vector3(-15, -147.39f, 0);
        Vector3 pos1 = new Vector3(-221, -147.39f, 0);
        Vector3 pos2 = Vector3.zero;
        float r = 85;
        float i = 0;
        float speed = 0.05f;
        int pass = 0;
        float s = 0.05f;
        float s_add = 0f;

        if (!is_in)
        {
            string[] str = temp_i[this].Split('-');
            pass = int.Parse(str[0]);
            i = float.Parse(str[1]);
        }

        //直线pass=0
        if (is_in || (!is_in && pass <= 0))//pass<=0表示还没有经历过这段
        {
            //初始值
            if (is_in || pass != 0)//在出去的情况下，但且仅当 储存的pass与当前pass相等时，才不会从头开始走
            {
                i = 0;
                speed = s;
            }
            else
            {
                string[] str = temp_i[this].Split('-');
                pass = int.Parse(str[0]);
                i = float.Parse(str[1]);
                speed = s;// 0.05f / (1 - i);
            }
            //走
            for (; i < 1f; i += speed)
            {
                transform.localPosition = Vector3.Lerp(pos0, pos1, i);
                if (ShootStop(is_in, 0, i))
                {
                    SetShootEndPos(r);
                    yield break;
                }
                yield return new WaitForSeconds(0.01f);
            }

        }
        s += s_add;

        //走半圆环pass=1
        pos0 = pos1;
        pos1 = new Vector3(-221, 34, 0);
        pos2 = (pos0 + pos1) * 0.5f;
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
                    r * -Mathf.Sin(Mathf.PI * i),
                    r * -Mathf.Cos(Mathf.PI * i),
                    0
                    );
                if (ShootStop(is_in, 1, i))
                {
                    SetShootEndPos(r);
                    yield break;
                }
                yield return new WaitForSeconds(0.01f);

            }
        }
        s += s_add;

        //直线pass=2
        pos0 = pos1;
        pos1 = new Vector3(279, 30, 0);
        if (is_in || (!is_in && pass <= 2))
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
                transform.localPosition = Vector3.Lerp(pos0, pos1, i);
                if (ShootStop(is_in, 2, i))
                {
                    SetShootEndPos(r);
                    yield break;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        s += s_add;

        //走半圆pass=3
        pos0 = pos1;
        pos1 = new Vector3(278, 210, 0);
        pos2 = (pos0 + pos1) * 0.5f;
        r = 103;
        if (is_in || (!is_in && pass <= 3))
        {
            //初始值
            if (is_in || pass != 3)
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
                transform.localPosition = pos2 + new Vector3(r * Mathf.Sin(Mathf.PI * i),r * -Mathf.Cos(Mathf.PI * i),0);
                if (ShootStop(is_in, 3, i))
                {
                    SetShootEndPos(70);
                    yield break;
                }
                yield return new WaitForSeconds(0.01f);

            }
        }
        s += s_add;

        //直线pass=4
        pos0 = pos1;
        pos1 = new Vector3(-310, 215, 0);
        if (is_in || (!is_in && pass <= 4))
        {
            //初始值
            if (is_in || pass != 4)
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
                transform.localPosition = Vector3.Lerp(pos0, pos1, i);
                if (ShootStop(is_in, 4, i))
                {
                    SetShootEndPos();
                    yield break;
                }
                yield return new WaitForSeconds(0.01f);
            }
            if (is_in)
            {
                tempShootPos.Add(transform.localPosition);
                
                if (temp_i.ContainsKey(this))
                {
                    temp_i[this] = "4-1";
                }
                else
                {
                    temp_i.Add(this, "4-1");
                }

            }
        }
        s += s_add;

        //直线出去
        if (!is_in)
        {
            pos0 = pos1;
            pos1 = new Vector3(-288, 215, 0);
            for (i = 0; i < 1f; i += 0.1f)
            {
                transform.localPosition = Vector3.Lerp(pos0, pos1, i);
                yield return new WaitForSeconds(0.01f);
            }
            Destroy(gameObject);
        }
        

    }
    IEnumerator TSmoothPos(Vector3 endpos)
    {
        Vector3 pos0 = transform.localPosition;
        for(float i = 0; i < 1f; i += 0.2f)
        {
            transform.localPosition = Vector3.Lerp(pos0, endpos, i);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = endpos;
    }

    IEnumerator TonChoose()
    {
        Vector3 endscale = AnimalSortCtr.spine_scale + new Vector3(0.2f, 0.2f, 0);
        for (float j = 0; j < 1f; j += 0.1f)
        {
            float p = Mathf.Sin(Mathf.PI * j) * 0.5f;
            transform.localScale = Vector2.Lerp(AnimalSortCtr.spine_scale, endscale, j);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = endscale;
    }

    IEnumerator TScale()
    {
        for (float j = 0; j < 1f; j += 0.1f)
        {
            float p = Mathf.Sin(Mathf.PI * j) * 0.5f;
            transform.localScale = Vector2.Lerp(AnimalSortCtr.spine_scale, AnimalSortCtr.spine_scale + new Vector3(0.04f, 0.04f,0), j);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = AnimalSortCtr.spine_scale;
    }

}
