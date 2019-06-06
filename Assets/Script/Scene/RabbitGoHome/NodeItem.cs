using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NodeItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void setItems(int num)
    {
        Vector3 startPos = getStartPos(num);
        for(int i = 0;i < num; i++)
        {
            Image endPoint = UguiMaker.newImage("luobo_" + (i + 1), transform, "rabbitgohome_sprite", "luobo");
            endPoint.transform.localScale = Vector3.one * 0.6f;
            endPoint.transform.localPosition = startPos + new Vector3(i % 3 * 50, -(int)(i / 3) * 50, 0);
        }
    }
    private Vector3 getStartPos(int childNum)
    {
        Vector3 pos = Vector3.zero;
        switch (childNum)
        {
            case 2:
                break;
            case 6:
                pos = new Vector3(-43, 28, 0);
                break;
            case 1:
            default:
                break;
        }
        return pos;
    }

}
