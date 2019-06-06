using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Items : MonoBehaviour {
    public static Items gSelect = null;
    public static Items gLSelect = null;
    public int mid { get; set; }
    public bool mstate { get; set; }
    public int mh { get; set; }
    public int mv { get; set; }
    public int mcolor = -1;//颜色索引值
    public int mPhoto = -1;
    private Image photo { get; set; }
    private Image numimg { get; set; }
    private Image bgImage { get; set; }
    public bool isDemo { get; set; }
    private Image currentEffectImage { get; set; }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void setData(int id,int h,int v)
    {
        mid = id;
        mh = h;
        mv = v;
        if(null == bgImage)
        {
            bgImage = gameObject.GetComponent<Image>();
        }
    }
    public void addChild(string sprite_name,Color _color,int map = 1)
    {
        if(null == photo)
        {
            photo = UguiMaker.newImage("photo", transform, "groupcheck_sprite", sprite_name);
        }else
        {
            photo.sprite = ResManager.GetSprite("groupcheck_sprite", sprite_name);
        }
        if(map == 2)
        {
            photo.color = _color;
        }else
        {
            photo.color = new Color(1f,1f,1f,1f);
        }
        string[] strs = sprite_name.Split('_');
        mPhoto = int.Parse(strs[strs.Length - 1]);
        photo.enabled = true;
    }
    public void showNum(int num)
    {
        if(null == numimg)
        {
            numimg = UguiMaker.newImage("num", transform, "groupcheck_sprite", "num_" + num);
            Canvas canvas = numimg.gameObject.AddComponent<Canvas>();
            numimg.gameObject.layer = LayerMask.NameToLayer("UI");
            canvas.overrideSorting = true;
            canvas.sortingOrder = 2;
        }
        else
        {
            numimg.sprite = ResManager.GetSprite("groupcheck_sprite", "num_" + num);
        }

        numimg.enabled = true;
    }
    //设置颜色
    public void setColor(int colorIndex,bool _isDemo = false)
    {
        mcolor = colorIndex;
        isDemo = _isDemo;
        setState(true);
    }
    //设置状态
    public void setState(bool state)
    {
        mstate = state;
    }
    //设置默认颜色
    public void setDeColor()
    {
        bgImage = gameObject.GetComponent<Image>();
        bgImage.color = GroupCheckCtr.instance.mGuanka.itemdeColor;
        if(null != photo)
        {
            photo.enabled = false;
        }
        if (null != numimg)
        {
            numimg.enabled = false;
        }
        mcolor = -1;
        mPhoto = -1;
        mstate = false;
        isDemo = false;
    }

    public void playError(bool isreset = true)
    {
        if(null == bgImage)
        {
            bgImage = gameObject.GetComponent<Image>();
        }
        if (null != photo && photo.enabled)
        {
            currentEffectImage = photo;
        }else
        {
            currentEffectImage = bgImage;
        }
        StartCoroutine(TPlayError(isreset));
    }
    IEnumerator TPlayError(bool isreset)
    {
        Color startcolor = Color.red;
        if (mcolor != 12)
        {
            if(mcolor != -1)
            {
                startcolor = GroupCheckCtr.instance.mGuanka.ColorList[mcolor];
            }
        }
        
        for (float j = 0; j < 1f; j += 0.1f)
        {
            float lerp = Mathf.PingPong(Time.time, j) / j;
            currentEffectImage.color = Color.Lerp(startcolor, GroupCheckCtr.instance.mGuanka.itemdeColor, lerp);
            yield return new WaitForSeconds(0.01f);
        }
        if (null != photo && photo.enabled && mcolor == 12)
        {
            currentEffectImage.color = new Color(1, 1, 1, 1) ;
        }
        else
        {
            currentEffectImage.color = startcolor;
        }
            
        if (isreset)
        {
            setDeColor();
        }
    }
}
