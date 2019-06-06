using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ShiZiLu : MonoBehaviour {
    public static ShiZiLu gSlect { get; set; }
    private Image shizilu1 { get; set; }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void setData()
    {
        if(null == shizilu1)
        {
            shizilu1 = UguiMaker.newImage("shizilu1", transform, "rabbitgohome_sprite", "shizilu_2");
        }
    }
    public void mMouseDown()
    {
        if(null != shizilu1)
        {
            shizilu1.sprite = ResManager.GetSprite("rabbitgohome_sprite", "shizilu_1");
        }
        
    }
    public void mMouseUp()
    {
        if (null != shizilu1)
        {
            shizilu1.sprite = ResManager.GetSprite("rabbitgohome_sprite", "shizilu_2");
        }

    }
}
