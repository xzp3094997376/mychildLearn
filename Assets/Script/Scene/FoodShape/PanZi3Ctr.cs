using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PanZi3Ctr : MonoBehaviour {
    public static Image gSelect { get; set; }
    public static Image gSelectItem { get; set; }
    private GameObject contain { get; set; }
    private GameObject itemContain { get; set; }
    private BoxCollider bgbox { get; set; }
    public float mAngel = 0;
    private float mLineAngel { get; set; }
    private Image sugbg { get; set; }
    public bool isHasBg { get; set; }
    private bool isCreateItem { get; set; }
    private List<Image> items = new List<Image>();
    private Dictionary<GameObject, float> ErrDic = new Dictionary<GameObject, float>();
    private int mDir { get; set; }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void setData(int _id,float _angel,bool ischoose,int dir)
    {
        mDir = dir;
        if (null == contain)
        {
            contain = UguiMaker.newGameObject("contain", transform);
        }
        mAngel = _angel;
        //Debug.Log("mAngel : " + mAngel);
        //Debug.Log("mAngel : " + mAngel);
        if (!ischoose)
        {
            Image fenge = UguiMaker.newImage("fenge", contain.transform, "foodshape_sprite", "fenge");
            fenge.transform.localScale = Vector3.one * 0.9f;
            List<Vector3> poss = new List<Vector3>() { new Vector3(-52, 32, 0), new Vector3(65, 40, 0), new Vector3(0, -74, 0) };
            for (int i = 0; i < 3; i++)
            {
                int index = i + 1;
                Image sug = UguiMaker.newImage("sug_" + index, contain.transform, "foodshape_sprite", "sug_" + index);
                sug.transform.localScale = Vector3.one * 0.9f;
                sug.transform.localEulerAngles = new Vector3(0, 0, i * 120);
            }
            contain.transform.localEulerAngles = new Vector3(0, 0, dir * mAngel);
            isCreateItem = true;
        }
        else
        {
            gameObject.name = "panzi3";
            isHasBg = false;
            bgbox = gameObject.AddComponent<BoxCollider>();
            bgbox.size = new Vector3(280, 280, 0);
            isCreateItem = false;
        }
    }
    public void setLine(float _lineAngel)
    {
        mLineAngel = _lineAngel;
        if (null == contain)
        {
            contain = UguiMaker.newGameObject("contain", transform);
            
        }
        if (null == sugbg)
        {
            sugbg = UguiMaker.newImage("sugbg", contain.transform, "foodshape_sprite", "sugbg");
            sugbg.transform.localScale = Vector3.one * 0.9f;
        }
        if (!isHasBg)
        {
            isHasBg = true;
        }
        bgbox.enabled = false;
        if(null != itemContain)
        {
            GameObject.Destroy(itemContain);
            
        }
        cleanItem();
        for (int i = 0; i < 3; i++)
        {
            int index = i + 1;
            Image sug = UguiMaker.newImage("sug_" + index, contain.transform, "foodshape_sprite", "sug_" + index);
            sug.transform.localScale = Vector3.one * 0.9f;
            sug.transform.localEulerAngles = new Vector3(0, 0, i * 120);
            BoxCollider areaBox = sug.gameObject.AddComponent<BoxCollider>();
            areaBox.size = new Vector3(100, 100, 0);
            areaBox.center = new Vector3(-84, 4, 0);
            Color color = sug.color;
            color.a = 0f;
            sug.color = color;
            items.Add(sug);
        }
        isCreateItem = true;
        contain.transform.localEulerAngles = new Vector3(0, 0,  -mLineAngel);

        
    }
    private void customStopAllCoroutines()
    {
        StopAllCoroutines();
        for (int i = 0; i < items.Count; i++)
        {
            Image item = items[i];
            int index = int.Parse(item.name.Split('_')[1]);
            item.transform.localEulerAngles = new Vector3(0, 0, 120 * (index - 1));
        }
    }
    private void cleanItem()
    {
        for (int i = 0; i < items.Count; i++)
        {
            GameObject.Destroy(items[i].gameObject);
        }
        items.Clear();
    }
    public void setItem(string itemName)
    {
        string sugName = "sug_" + itemName.Split('_')[1];
        //Debug.Log("itemName: " + sugName);
        if(null != gSelectItem)
        {
            gSelectItem.sprite = ResManager.GetSprite("foodshape_sprite", sugName);
            int index = int.Parse(itemName.Split('_')[1]);
            Color color = gSelectItem.color;
            color.a = 1f;
            gSelectItem.color = color;
        }
    }
    private void resetErrAngel()
    {
        foreach(GameObject go in ErrDic.Keys)
        {
            go.transform.localEulerAngles = new Vector3(0, 0, ErrDic[go]);
        }
    }
    int mindex = 0;
    public bool checkFinish()
    {
        StopAllCoroutines();
        resetErrAngel();
        ErrDic.Clear();
        mindex++;
        bool state = true;
        //Debug.Log("mindex : " + mindex + ";mLineAngel: " + mLineAngel + ";mAngel : " + mAngel);
        //Debug.Log("(int)mLineAngel != (int)mAngel : " + ((int)mLineAngel != (int)mAngel));
        if ((int)mLineAngel != (int)mAngel)
        {
            StartCoroutine(TplayErr(gameObject,transform));
            state =  false;
        }
        else
        {
            bool isallShow = true;
            for (int i = 0; i < items.Count; i++)
            {
                Image sug = items[i];
                Color color = sug.color;
                if (color.a == 0)
                {
                    isallShow = false;
                    break;
                }
            }
            //Debug.Log("isallShow : " + isallShow);
            if (isallShow)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    Image sug = items[i];
                    if (sug.sprite.name != "sug_" + (1+i))
                    {
                        state = false;
                        StartCoroutine(TplayErr(sug.gameObject,false));
                    }
                }
            }
            else
            {
                state = false;
                Image sug1 = items[0];
                Image sug2 = items[1];
                Image sug3 = items[2];
                if (sug1.color.a == 1 && sug2.color.a == 1 && sug1.sprite.name == "sug_1" && sug2.sprite.name == "sug_2")
                {
                    state = true;
                }
                else
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        Image sug = items[i];
                        if (sug.sprite.name != "sug_" + (i + 1))
                        {
                            StartCoroutine(TplayErr(sug.gameObject, false));
                        }
                    }
                }
            }
        }
        
        return state;
    }
    
    IEnumerator TplayErr(GameObject go,bool isclean)
    {
        float cuangel = go.transform.localEulerAngles.z;
        ErrDic.Add(go, cuangel);
        for (float j = 0; j < 1f; j += 0.05f)
        {
            float p = Mathf.Sin(Mathf.PI * j) * 0.8f;

            go.transform.localEulerAngles = new Vector3(0, 0, cuangel + Mathf.Sin(Mathf.PI * 6 * j) * 10);
            yield return new WaitForSeconds(0.01f);
        }
        go.transform.localEulerAngles = new Vector3(0, 0, cuangel);
        if (isclean)
        {
            clean();
        }else
        {
            Color color = go.gameObject.GetComponent<Image>().color;
            color.a = 0f;
            go.gameObject.GetComponent<Image>().color = color;
        }
        
    }
    private void clean()
    {
        GameObject.Destroy(contain);
        isCreateItem = false;
        isHasBg = false;
        items.Clear();
    }
}
