using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour {
    public static AnswerButton gSelect = null;
    public int mNum { get; set; }
    private Image buttonNumImg { get; set; }
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setData(int _num,int index)
    {
        if (null == buttonNumImg)
        {
            buttonNumImg = UguiMaker.newImage("num", transform, "funnygroup_sprite", "0");
        }
        mNum = _num;
        buttonNumImg.sprite = ResManager.GetSprite("funnygroup_sprite", mNum.ToString());

        Image button = transform.gameObject.GetComponent<Image>();// UguiMaker.newImage("button" + i, answerButtonGo.transform, "funnygroup_sprite", "qwfz_ka_" + 0);
        button.sprite = ResManager.GetSprite("funnygroup_sprite", "qwfz_ka_" + 0);
        buttonNumImg.enabled = true;
        StartCoroutine(TShow());
    }
    public void fanguolai()
    {
        StartCoroutine(Tfanye());
    }
    IEnumerator Tfanye()
    {
        Vector3 vec = new Vector3(0, 90, 0);
        Vector3 vecs = new Vector3(0, 0, 0);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            transform.localEulerAngles = Vector3.Lerp(Vector3.zero, vec, i);
            yield return new WaitForSeconds(0.01f);
        }
        buttonNumImg.enabled = false;
        transform.localEulerAngles = new Vector3(0,90,0);

        for (float i = 0; i < 1f; i += 0.1f)
        {
            transform.localEulerAngles = Vector3.Lerp(vec,Vector3.zero, i);
            yield return new WaitForSeconds(0.01f);
        }
        Image button = transform.gameObject.GetComponent<Image>();// UguiMaker.newImage("button" + i, answerButtonGo.transform, "funnygroup_sprite", "qwfz_ka_" + 0);
        button.sprite = ResManager.GetSprite("funnygroup_sprite", "qwfz_ka_" + 1);
    }
    IEnumerator TShow()
    {
        for (float i = 0; i < 1f; i += 0.05f)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = Vector3.one;
    }
}
