using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class PanZiCtr : MonoBehaviour {
    public static PanZiCtr gSelect { get; set; }
    private GameObject comtain { get; set; }
    private int mID { get; set; }
    private int mFixID { get; set; }
    private bool isRotation { get; set; }
    private bool isStopRotation { get; set; }
    private float endAngel { get; set; }
    private ParticleSystem mStarparticle { get; set; }
    private int mdir = 0;
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    public float angel = 0;
	void Update () {
    }
    public void setData(int _id, int fixID, int dir)
    {
        if(null == comtain)
        {
            comtain = UguiMaker.newGameObject("comtain", transform);
            Image toolpan = UguiMaker.newImage("pis_" + _id, comtain.transform, "foodshape_sprite", "pan_1");
        }
        mID = _id;
        if (mID != fixID)
        {
            angel = 7 * 45 * dir;
        }
        else
        {
            angel = mID * 45 * dir;
        }
        mdir = dir;
        mFixID = fixID;
        StartCoroutine(TRooot(dir));
    }
    public void rotation()
    {
        angel = 0;
        isRotation = true;
        isStopRotation = true;
    }
    /*
    public void stopRotation(int fixID,int dir)
    {
        if(mID != fixID)
        {
            angel = 7 * 45 * dir;
        }
        else
        {
            angel = mID * 45 * dir;
        }
        mFixID = fixID;
        isRotation = false;

    }
    */
    public bool checkFix()
    {
        bool boo = true;
        if(mFixID == -1)
        {
            boo = false;
        }
        return boo;
    }
    public void playOkEffect()
    {
        if (null == mStarparticle)
        {
            GameObject obj1 = ResManager.GetPrefab("effect_star2", "effect_star2");
            obj1.transform.parent = transform;
            mStarparticle = obj1.GetComponent<ParticleSystem>();
            mStarparticle.transform.localPosition = Vector3.zero;
            mStarparticle.transform.localScale = Vector3.one;
        }
        mStarparticle.Play();
    }

    public void playErrEffect()
    {
        StartCoroutine(TPlayErrEffect());
    }
    IEnumerator TPlayErrEffect()
    {
        float cuangel = comtain.transform.localEulerAngles.z;
        for (float j = 0; j < 1f; j += 0.05f)
        {
            float p = Mathf.Sin(Mathf.PI * j) * 0.8f;

            comtain.transform.localEulerAngles = new Vector3(0, 0, cuangel + Mathf.Sin(Mathf.PI * 6 * j) * 10);
            yield return new WaitForSeconds(0.01f);
        }
        comtain.transform.localEulerAngles = new Vector3(0, 0, mdir * angel);
    }
    IEnumerator TRooot(int dir)
    {
        float time = 0;
        Vector3 temp0 = Vector3.zero;
        Vector3 temp1 = new Vector3(0, 0, 360 + angel) * dir;
        for(float i = 0; i < 1f; i += 0.01f)
        {
            comtain.transform.localEulerAngles = Vector3.Lerp(temp0, temp1, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        
    }
}
