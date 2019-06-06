using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class PanZi2Ctr : MonoBehaviour {
    public static PanZi2Ctr gSelect { get; set; }
    public static Image gSelectbg { get; set; }
    private GameObject comtain { get; set; }
    private int mID { get; set; }
    private List<Image> mks = new List<Image>();
    private Dictionary<string, int> markIDs = new Dictionary<string, int>();
    private Dictionary<string, int> makeIDs = new Dictionary<string, int>();
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setData(int _id,int closeNum,int dir)
    {
        if (null == comtain)
        {
            comtain = UguiMaker.newGameObject("comtain", transform);
        }
        mID = _id;
        List<int> closeIDs = null;
        if (mID != 0)
        {
            closeIDs = Common.GetMutexValue(1, 6, closeNum);
        }
        for (int i = 1;i < 7; i++)
        {
            Image mkbg = UguiMaker.newImage("mkbg_" + i, comtain.transform, "foodshape_sprite", "mkbg");
            BoxCollider box = mkbg.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(90, 90);
            mkbg.transform.localPosition = getPos(i - 1, 60);
            Image mk = UguiMaker.newImage("mk_" + i, comtain.transform, "foodshape_sprite", "mk_" + i);
            mk.transform.localPosition = mkbg.transform.localPosition;
            if(null != closeIDs && closeIDs.Contains(i))
            {
                markIDs.Add("mk_" + i, i);
                mk.enabled = false;
            }else
            {
                box.enabled = false;
            }
            mks.Add(mk);
        }
        comtain.transform.localEulerAngles = new Vector3(0, 0, dir * mID * 60);
    }
    //mkName选择的mk，mkbgid盘子上空白ID（事件的驱动者）
    public void setMake(string mkName,string mkbgid)
    {
        Image mk = comtain.transform.Find("mk_" + mkbgid).gameObject.GetComponent<Image>();
        mk.sprite = ResManager.GetSprite("foodshape_sprite", mkName);
        mk.enabled = true;
        int id = int.Parse(mkName.Split('_')[1]);
        if (makeIDs.ContainsKey("mk_" + mkbgid))
        {
            makeIDs["mk_" + mkbgid] = id;
        }else
        {
            makeIDs.Add("mk_" + mkbgid, id);
        }
    }
    public bool checkFinish()
    {
        bool boo = true;
        foreach(string key in markIDs.Keys)
        {
            Image mk = comtain.transform.Find(key).gameObject.GetComponent<Image>();
            if (!mk.enabled)
            {
                boo = false;
            }else
            {
               
                if (makeIDs[key] != markIDs[key])
                {
                    boo = false;
                    StartCoroutine(TPlayErrEffect(mk));
                }
            }
        }
        return boo;
    }
    private Vector3 getPos(int _id, float disAngel)
    {
        float angle = ((_id * disAngel) / 180) * Mathf.PI;
        float vx = 0;
        float vy = 0;
        float r = 100;
        vx = Mathf.Sin(angle) * r;
        vy = Mathf.Cos(angle) * r;
        Vector3 pos = new Vector3(vx, vy, 0);
        return pos;
    }

    IEnumerator TPlayErrEffect(Image image)
    {
        for (float j = 0; j < 1f; j += 0.05f)
        {
            float p = Mathf.Sin(Mathf.PI * j) * 0.8f;

            image.transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * j) * 10);
            yield return new WaitForSeconds(0.01f);
        }
        image.transform.localEulerAngles = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.5f);
        image.enabled = false;
    }
}
