using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AnimalGrids : MonoBehaviour {
    public static AnimalGrids gSelect { get; set; }
    public List<SpineCtr> animals { get; set; }
    private GridLayoutGroup mLayout { get; set; }
    public int mNumf { get; set; }
    public int mNums { get; set; }
    public string mAnimalNamef { get; set; }
    public string mAnimalNames { get; set; }
    public bool isAnswer { get; set; }
    private Vector3 mdePos { get; set; }
    // Use this for initialization
    void Start () {
	
	}
    // Update is called once per frame
    float times = 0;
    bool isShow { get; set; }
    void Update()
    {
        if (!isShow) return;

        times += 0.1f;
        transform.localPosition = mdePos + new Vector3(0, 10 * Mathf.Sin(times), 0);
    }
    public void playSpine(string idle,bool isLoop)
    {
        for (int i = 0; i < animals.Count; i++)
        {
            SpineCtr animalCtr = animals[i];
            animalCtr.PlaySpine(idle, isLoop);
        }
    }

    public void setData(AnimalSortCtr.Guanka guanka, int _numf, int _nums,bool _isAnswer,int _id)
    {
        mNumf = _numf;
        mNums = _nums;
        isAnswer = _isAnswer;
        times = _id * 30;
        ///*
        if (null == mLayout)
        {
            mLayout = gameObject.AddComponent<GridLayoutGroup>();
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(mNumf + mNums * 100, 100);//()
            mLayout.cellSize = new Vector2(55, 50);//60
            mLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
            mLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
            mLayout.childAlignment = TextAnchor.MiddleCenter;
            mLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            mLayout.constraintCount = 1;
            BoxCollider box = gameObject.AddComponent<BoxCollider>();
            box.size = new Vector2((mNumf + mNums) * 60, 100);
            box.center = new Vector3(0,30,0);
        }
        //*/
       
        animals = new List<SpineCtr>();
        float scaleNum = 0.12f;
        mAnimalNamef = guanka.animals[1];
        for (int i = 0; i < mNumf; i++)
        {
            SpineCtr animalCtrf = ResManager.GetPrefab("animalsort_prefab", mAnimalNamef).GetComponent<SpineCtr>();
            animalCtrf.setData(mAnimalNamef);
            animals.Add(animalCtrf);
        }
        mAnimalNames = guanka.animals[2];
        for (int i = 0; i < mNums; i++)
        {
            SpineCtr animalCtrs = ResManager.GetPrefab("animalsort_prefab", mAnimalNames).GetComponent<SpineCtr>();
            animalCtrs.setData(mAnimalNames);
            animals.Add(animalCtrs);
        }
        float startX = -animals.Count * 70;//100
        Vector3 startPos = new Vector3(startX, 0, 0);
        for (int i = 0; i < animals.Count; i++)
        {
            SpineCtr animalCtr = animals[i];
            GameObject go = UguiMaker.InitGameObj(animalCtr.gameObject, transform, "animal" + i, Vector3.zero, Vector3.one * scaleNum);
            animalCtr.PlaySpine("Idle_2", false);
        }

    }

    public void setDefaultPos(Vector3 _defPos)
    {
        mdePos = _defPos;
        gameObject.transform.localPosition = _defPos;
        isShow = true;
    }
    public void backToDefaultPos()
    {
        toSmall();
        StartCoroutine("TSetDefPos");
    }
    public void Close()
    {
        StartCoroutine("TClose");
    }
    public void cleanAnimal()
    {
        for (int i = 0; i < animals.Count; i++)
        {
            SpineCtr animalCtrs = animals[i];
            GameObject.Destroy(animalCtrs.gameObject);
        }
    }
    public void toBiger()
    {
        isShow = false;
        StartCoroutine("TtoBiger");
    }
    public void toSmall()
    {
        StartCoroutine("TtoSmall");
    }
    public void Select()
    {

    }

    private float bigger = 1.8f;
    IEnumerator TtoBiger()
    {
        for (float j = 0; j < 1f; j += 0.2f)
        {
            gameObject.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.8f, 1.8f, 0), j);
            yield return new WaitForSeconds(0.01f);
        }
        gameObject.transform.localScale = new Vector3(bigger, bigger, 0);
    }

    IEnumerator TtoSmall()
    {
        for (float j = 0; j < 1f; j += 0.2f)
        {
            gameObject.transform.localScale = Vector3.Lerp(new Vector3(bigger, bigger, 0),Vector3.one, j);
            yield return new WaitForSeconds(0.01f);
        }
        gameObject.transform.localScale = Vector3.one;
    }

    IEnumerator TSetDefPos()
    {
        Vector3 pos = gameObject.transform.localPosition;
        for (float j = 0; j < 1f; j += 0.2f)
        {
            gameObject.transform.localPosition = Vector3.Lerp(pos, mdePos, j);
            yield return new WaitForSeconds(0.01f);
        }
        gameObject.transform.localPosition = mdePos;
       transform.SetSiblingIndex(0);
       AnimalSortCtr.opState = true;
        isShow = true;
    }
    
    IEnumerator TClose()
    {
        gameObject.transform.localPosition = new Vector3(77, -110, 0);
        for (float j = 0; j < 1f; j += 0.2f)
        {
            gameObject.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, j);
            yield return new WaitForSeconds(0.01f);
        }
        gameObject.transform.localScale = Vector3.zero;

        gameObject.transform.localPosition = mdePos;
    }
   
}
