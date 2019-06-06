using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
public class PositionCtr : MonoBehaviour {
    public static PositionCtr gSelect { get; set; }
    private Image bg { get; set; }
    private Image rolebg { get; set; }
    private Image numVImage { get; set; }
    private Image numHImage { get; set; }
    public int mNumH { get; set; }
    public int mNumV { get; set; }
    private ParticleSystem mStarparticle { get; set; }
    private GameObject animalGo { get; set; }
    private GameObject anGo { get; set; }
    // Use this for initialization
    void Start () {
        bg = UguiMaker.newImage("bg", transform, "findposition_sprite", "shafa");
    }
	public void showVNum(int num)
    {
        GameObject vgo = UguiMaker.newGameObject("vgo", transform);
        numVImage = UguiMaker.newImage("numVImage", vgo.transform, "findposition_sprite", "position_v");
        numVImage.transform.localPosition = new Vector3(0, -75, 0);
        Image numimage = UguiMaker.newImage("num", vgo.transform, "findposition_sprite", "num_" + num.ToString());
        numimage.transform.localPosition = new Vector3(-17, -75, 0);

        Canvas canvas = vgo.AddComponent<Canvas>();
        vgo.layer = LayerMask.NameToLayer("UI");
        canvas.overrideSorting = true;
        canvas.sortingOrder = 2;
    }
    public void showHNum(int num)
    {
        GameObject hgo = UguiMaker.newGameObject("hgo", transform);
        numHImage = UguiMaker.newImage("numHImage", hgo.transform, "findposition_sprite", "position_h");
        numHImage.transform.localPosition = new Vector3(-100, 20, 0);
        Image numimage = UguiMaker.newImage("num", hgo.transform, "findposition_sprite", "num_" + num.ToString());
        numimage.transform.localPosition = new Vector3(-100, 20, 0);
        Canvas canvas = hgo.AddComponent<Canvas>();
        hgo.layer = LayerMask.NameToLayer("UI");
        canvas.overrideSorting = true;
        canvas.sortingOrder = 2;
    }
    public void setData(int numH,int numV)
    {
        mNumH = numH;
        mNumV = numV;
        ShowOut();
    }
    public void setAnimal(string animalName)
    {
        if(null == rolebg)
        {
            anGo = UguiMaker.newGameObject("rolego", transform);
            rolebg = UguiMaker.newImage("rolebg", anGo.transform, "findposition_sprite", "photobg");
            rolebg.rectTransform.localScale = Vector3.one * 0.6f;
            rolebg.rectTransform.localPosition = new Vector3(0, 15, 0);
        }
        else
        {
            rolebg.enabled = true;
        }
        FindPositionSpine spin = ResManager.GetPrefab("animalhead_prefab", animalName).AddComponent<FindPositionSpine>();
        if(null == animalGo)
        {
            animalGo = UguiMaker.InitGameObj(spin.gameObject, anGo.transform, "animal", Vector3.zero, Vector3.one * 0.23f);
            animalGo.transform.localScale = Vector3.zero;
        }
        spin.transform.localPosition = new Vector3(0, -25, 0);
        spin.PlaySpine("Idle", true);

        
        Canvas canvas = anGo.GetComponent<Canvas>();
        if(null == canvas)
        {
            canvas = anGo.AddComponent<Canvas>();
        }
        anGo.layer = LayerMask.NameToLayer("UI");
        if(null != canvas)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = 2;
        }
        

        StartCoroutine(TToScale());
    }
    public void playStar()
    {

        if (null == mStarparticle)
        {
            GameObject obj1 = ResManager.GetPrefab("effect_star2", "effect_star2");
            obj1.transform.parent = anGo.transform;
            mStarparticle = obj1.GetComponent<ParticleSystem>();
            mStarparticle.transform.localPosition = new Vector3(0, 20, 0);
            mStarparticle.transform.localScale = Vector3.one;
        }
        if(null != mStarparticle)
        {
            mStarparticle.Play();
        }
        
    }
    public void cleanAnimal()
    {
        GameObject.Destroy(animalGo);
        animalGo = null;
        if(null != rolebg)
        {
            rolebg.enabled = false;
        }
        
    }

    IEnumerator TToScale()
    {
        Vector3 startPos = Vector3.zero;
        Vector3 endPos = Vector3.one * 0.23f;
        for (float i = 0; i < 1f; i += 0.1f)
        {
            animalGo.transform.localScale = Vector3.Lerp(startPos, endPos, i);
            yield return new WaitForSeconds(0.01f);
        }
        animalGo.transform.localScale = endPos;
    }
    public void ShowOut(float endScale = 1)
    {
        float ofset = 0.1f;
        transform.localScale = Vector3.one * 0.001f;
        transform.DOScale(Vector3.one * (endScale + ofset), 0.2f).OnComplete(() =>
        {
            transform.DOScale(Vector3.one * (endScale - (ofset- 0.1f)), 0.15f).OnComplete(() =>
            {
                transform.DOScale(Vector3.one * (endScale + (ofset - 0.1f)), 0.12f).OnComplete(() =>
                {
                    transform.DOScale(Vector3.one * endScale, (ofset - 0.1f));
                });
            });
        });
    }
}
