using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class LuoBoItem : MonoBehaviour {
    private Vector3 mDefPos { get; set; }
    private Transform mDefParent { get; set; }
    private Image luobo { get; set; }
    private bool isRun = false;
    // Use this for initialization
    void Start () {
	
	}
    int pic = 0;
	// Update is called once per frame
	void Update () {
        /*
        if (null != luobo && luobo.enabled)
        {
            pic++;
            if(pic % 10 == 0)
            {

            }
            int index = (pic % 3) + 1;
            
        }
        */
    }
    public void close()
    {
        if(null == luobo)
        {
            luobo = gameObject.GetComponent<Image>();
        }
        luobo.enabled = false;
        if (isRun)
        {
            StopCoroutine("TPlayCheng");
            isRun = false;
        }
        
    }
    public void show()
    {
        if (null == luobo)
        {
            luobo = gameObject.GetComponent<Image>();
        }
        luobo.enabled = true;
        if (!isRun)
        {
            StartCoroutine("TPlayCheng");
            isRun = true;
        }
        
    }
    List<int> list = new List<int>() { 1, 2, 3, 2, 1, 2,3 };
    IEnumerator TPlayCheng()
    {

        for(int i = 0;i < list.Count; i++)
        {
            luobo.sprite = ResManager.GetSprite("rabbitgohome_sprite", "luobo_" + list[i]);
            yield return new WaitForSeconds(0.1f);
        }
        /*
        while (true)
        {
            pic++;
            int index = (pic % 5);
            Debug.Log(index);
            luobo.sprite = ResManager.GetSprite("rabbitgohome_sprite", "luobo_" + list[index]);
            yield return new WaitForSeconds(0.1f);
        }
        */
    }
    /*
    public void setData(Vector3 defPos,Transform defParent)
    {
        mDefPos = defPos;
        mDefParent = defParent;
    }
    */
}
