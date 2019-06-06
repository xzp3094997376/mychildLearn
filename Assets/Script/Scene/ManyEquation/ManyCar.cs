using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManyCar : MonoBehaviour {
    public int mSize { get; set; }
    public int mColor { get; set; }
    public int mDic { get; set; }
    private Image mCar { get; set; }
    private Image mlun1 { get; set; }
    private Image mlun2 { get; set; }
    private Vector3 mendPos { get; set; }
    private SoundManager mSound { get; set; }
    void Awake()
    {
        //mSound = gameObject.AddComponent<SoundManager>();
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void setData(int size,int color,int dic,Vector3 pos)
    {
        mSize = size;
        mColor = color;
        mDic = dic;
        mendPos = pos;

        string spriteName = "scene_3_" + mSize + "_" + mColor;
        mCar = UguiMaker.newImage(spriteName, transform, "manyequation_sprite", spriteName);

        mlun1 = UguiMaker.newImage(spriteName + "_lun1", transform, "manyequation_sprite", "scene_3_4");
        mlun1.rectTransform.localPosition = new Vector3(-81, -63, 0);
        if(mSize == 1)
        {
            mlun1.rectTransform.localPosition = new Vector3(-81, -46, 0);
        }

        mlun2 = UguiMaker.newImage(spriteName + "_lun2", transform, "manyequation_sprite", "scene_3_4");
        mlun2.rectTransform.localPosition = new Vector3(91, -63, 0);
        if (mSize == 1)
        {
            mlun2.rectTransform.localPosition = new Vector3(91, -46, 0);
        }

        if (mDic == 1)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        StartCoroutine(TMoveIn());
    }
    public void moveOut()
    {
        StartCoroutine(TMoveOut());
    }
    public void scale(Vector3 vec3)
    {
        int csize = (int)vec3.x;
        int ccolor = (int)vec3.y;
        int cdic = (int)vec3.z;
        if(mSize == csize || ccolor == mColor || cdic == mDic)
        {
            StartCoroutine(TScale());
        }
    }
    IEnumerator TScale()
    {
        Vector3 sc1 = Vector3.one;
        Vector3 sc2 = Vector3.one * 1.2f;
        for (float j = 0; j < 1f; j += 0.1f)
        {
            transform.localScale = Vector3.Lerp(sc1, sc2, j);

            yield return new WaitForSeconds(0.01f);
        }
        sc1 = Vector3.one * 1.2f;
        sc2 = Vector3.one;
        for (float j = 0; j < 1f; j += 0.1f)
        {
            transform.localScale = Vector3.Lerp(sc1, sc2, j);

            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = sc2;
    }
    IEnumerator TMoveOut()
    {
        Vector3 startPos = mendPos;
        Vector3 endPos = mendPos;
        if (mDic == 0)
        {
            endPos.x = 1000;
        }
        else
        {
            endPos.x = -1000;
        }
        for (float j = 0; j < 1f; j += 0.04f)
        {
            transform.localPosition = Vector3.Lerp(startPos, endPos, j);

            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = endPos;
    }
    IEnumerator TMoveIn()
    {
        Vector3 startPos = mendPos;
        if(mDic == 0)
        {
            startPos.x = -1000;
        }else
        {
            startPos.x = 1000;
        }
        for (float j = 0; j < 1f; j += 0.04f)
        {
            transform.localPosition = Vector3.Lerp(startPos, mendPos, j);

            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = mendPos;
    }
}
