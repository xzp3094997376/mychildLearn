using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimalNumOnlyBtn : MonoBehaviour
{
    public static AnimalNumOnlyBtn gSelect { get; set; }

    public RectTransform mRtran { get; set; }
    RectTransform root { get; set; }
    //Image mBg { get; set; }
    //Image mHead { get; set; }
    Image mHead2 { get; set; }
    SphereCollider mBox0 { get; set; }
    BoxCollider mBox1 { get; set; }

    public int m_id { get; set; }

	// Use this for initialization
	void Start () {
	
	}
	
    public void Init(int animal_id)
    {
        root = UguiMaker.newGameObject("root", transform).GetComponent<RectTransform>();
        mRtran = gameObject.GetComponent<RectTransform>();
        //mBg = UguiMaker.newImage("mBg", root, "animalnumonly_sprite", "animal_bg2", false);
        //mHead = UguiMaker.newImage("mHead", root, "aa_animal_head", animal_id.ToString(), false);
        mHead2 = UguiMaker.newImage("mHead2", root, "animalnumonly_sprite", "btnbg" + animal_id.ToString(), true);
        m_id = animal_id;
        //mBg.rectTransform.sizeDelta = new Vector2(75, 75);
        //mHead.rectTransform.sizeDelta = new Vector2(60, 60);
        mBox0 = root.gameObject.AddComponent<SphereCollider>();
        mBox0.radius = 36.5f;
        mBox1 = root.gameObject.AddComponent<BoxCollider>();
        mBox1.size = new Vector3(75, 75, 1);
        mBox0.enabled = false;
        mBox1.enabled = false;
        SetType(1);

    }

    public void SetType(int type)
    {
        //Debug.Log("SetType(" + type + ")");
        if (0 == type)
        {
            //mBg.sprite = ResManager.GetSprite("animalnumonly_sprite", "animal_bg0");
            mBox0.enabled = true;
            mBox1.enabled = false;
        }
        else
        {
            //mBg.sprite = ResManager.GetSprite("animalnumonly_sprite", "animal_bg2");
            mBox0.enabled = false;
            mBox1.enabled = true;

        }
    }
    
    public void Reset()
    {
        SetColor(new Color(1, 1, 1, 1));
        SetUnSelect();
    }
    public void SetSelect()
    {
        //Debug.Log("SetSelect()");
        //mBg.sprite = ResManager.GetSprite("animalnumonly_sprite", "animal_bg1");
    }
    public void SetUnSelect()
    {
        //Debug.Log("SetUnSelect()");
        //mBg.sprite = ResManager.GetSprite("animalnumonly_sprite", "animal_bg0");
    }

    public Color GetColor()
    {
        return mHead2.color;
    }
    public void SetColor(Color cor)
    {
        //mBg.color = cor;
        //mHead.color = cor;
        mHead2.color = cor;
    }

}
