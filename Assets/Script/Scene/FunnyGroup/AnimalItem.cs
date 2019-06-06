using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AnimalItem : MonoBehaviour {
    public float mAngel;// { get; set; }
    private bool isMoving { get; set; }
    private float defaultSpeed = 3f;
    private float mSpeed { get; set; }
    private float times { get; set; }
    private Vector3 mStartPos { get; set; }
    public bool isFix { get; set; }
    
	// Use this for initialization
	void Start () {
	
	}
    public void move(float angel)
    {
        isFix = false;
        mSpeed = 3;
        mAngel = angel;
        resetData();
        isMoving = true;
        
    }
    public void stop()
    {
        isMoving = false;
    }
    public void ShootIn()
    {
        StartCoroutine(TShootIn(true));
    }
    // Update is called once per frame
    void Update () {

        if (!isMoving) return;


        times += mSpeed;

        transform.localPosition = mStartPos + new Vector3(Mathf.Sin(mAngel) * times, Mathf.Cos(mAngel) * times, 0);

        if(transform.localPosition.x > GlobalParam.screen_width * 0.5f - 50)// || transform.localPosition.x < -GlobalParam.screen_width * 0.5f + 50 || transform.localPosition.y > GlobalParam.screen_height * 0.5f - 100 || transform.localPosition.y < -GlobalParam.screen_height * 0.5f + 50)
        {
            //transform.localPosition = transform.localPosition - new Vector3(100, 0, 0);
            mAngel += 90;//Common.GetRandValue(90,100);// FunnyGroupCtr.randomAngel();
            resetData();
            //mSpeed = -mSpeed;
        }else if(transform.localPosition.x < -GlobalParam.screen_width * 0.5f + 50)
        {
            //transform.localPosition = transform.localPosition + new Vector3(100,0,0);
            mAngel += 90;//Common.GetRandValue(90, 100);// FunnyGroupCtr.randomAngel();
            resetData();
            //mSpeed = -mSpeed;
        }else

        if (transform.localPosition.y > GlobalParam.screen_height * 0.5f - 100)
        {
            //transform.localPosition = transform.localPosition - new Vector3(0, 150, 0);
            mAngel += 90;// Common.GetRandValue(90, 100);// FunnyGroupCtr.randomAngel();
            resetData();
            //mSpeed = -mSpeed;
        }
        else if (transform.localPosition.y < -GlobalParam.screen_height * 0.5f + 50)
        {
            //transform.localPosition = transform.localPosition + new Vector3(0, 100, 0);
            mAngel += 90;//Common.GetRandValue(90, 100);// FunnyGroupCtr.randomAngel();
            resetData();
            //mSpeed = -mSpeed;
        }

    }
    private void resetData()
    {
        times = 0;
        mStartPos = transform.localPosition;
    }
    public void MoveTo(Vector3 pos)
    {
        StartCoroutine("TMoveTo", pos);
    }
    public static void cleanPosStat()
    {
        tempShootPos.Clear();
        temp_i.Clear();
    }
    IEnumerator TMoveTo(Vector3 pos)
    {
        Vector3 startPos = transform.localPosition;
        for (float i = 0; i < 1f; i += 0.05f)
        {
            transform.localPosition = Vector3.Lerp(startPos, pos, i);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = pos;
        resetData();
    }
    public static List<Vector3> tempShootPos = new List<Vector3>();
    public static Dictionary<AnimalItem, string> temp_i = new Dictionary<AnimalItem, string>();
    bool ShootStop(bool is_in, int pass, float i, bool isfirt = false)
    {
        if (!is_in) return false;

        if (0 == tempShootPos.Count)
        {
            return false;
        }
        else
        {
            if (FunnyGroupCtr.mD > Vector3.Distance(tempShootPos[tempShootPos.Count - 1], transform.localPosition))
            {
                tempShootPos.Add(transform.localPosition);
                if (temp_i.ContainsKey(this))
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
        Vector3 pos = tempShootPos[tempShootPos.Count - 2] + (transform.localPosition - tempShootPos[tempShootPos.Count - 2]).normalized * FunnyGroupCtr.mN;// 80;
        //(beg - end).normalized * 88;
        tempShootPos[tempShootPos.Count - 1] = pos;
        transform.localPosition = pos;
        //StartCoroutine(TSmoothPos(pos));
    }
    IEnumerator TSmoothPos(Vector3 endpos)
    {
        Vector3 pos0 = transform.localPosition;
        for (float i = 0; i < 1f; i += 0.2f)
        {
            transform.localPosition = Vector3.Lerp(pos0, endpos, i);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = endpos;
    }
    
    IEnumerator TShootIn(bool is_in)
    {
        
        float r = FunnyGroupCtr.mR;
        float i = 0;
        float speed = 0.05f;
        int pass = 0;
        float s = 0.05f;
        float s_add = 0f;
        Vector3 pos0 = new Vector3(-GlobalParam.screen_width * 0.5f, r, 0);
        Vector3 pos1 = new Vector3(0, r, 0);
        Vector3 pos2 = Vector3.zero;
        
        //直线pass=0
        if (is_in || (!is_in && pass <= 0))//pass<=0表示还没有经历过这段
        {
            //走
            for (; i < 1f; i += speed)
            {
                transform.localPosition = Vector3.Lerp(pos0, pos1, i);
                yield return new WaitForSeconds(0.01f);
            }

        }
        s += s_add;

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
                temp_i[this] = "4-1";
            }
            else
            {
                temp_i.Add(this, "4-1");
            }
        }
        s += s_add;

        resetData();
    }
}
