using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class KengItem : MonoBehaviour {
    private Image luobo { get; set; }
    public bool isP { get; set; }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void setLuobo()
    {
        isP = false;
        luobo = UguiMaker.newImage("luobo", transform, "rabbitgohome_sprite", "keng_luobo");
        luobo.rectTransform.localPosition = new Vector3(3, 30, 0);
    }
    public void closeLuobo()
    {
        isP = true;
        luobo.enabled = false;
    }
    public void showLuobo()
    {
        isP = false;
        luobo.enabled = true;
    }
}
