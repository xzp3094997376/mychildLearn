using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WindowCtr : MonoBehaviour {
    public int id { get; set; }
    public int id_animal { get; set; }
    private GameObject numGo { get; set; }
    public int mNumH { get; set; }
    public int mNumV { get; set; }
    private int currH = -1;
    private int currV = -1;
    public Image wenbg { get; set; }
    private Image wenred { get; set; }
    private Image wenblack { get; set; }
    private BoxCollider boxl { get; set; }
    private BoxCollider boxr { get; set; }
    public bool mState { get; set; }
    private Image window { get; set; }
    public GameObject photo { get; set; }
    private RoleSpine photoctr { get; set; }
    float temp_scale = 0.9f;// 0.6f;

    void Start () {
	
	}
	
    public void closeDoor(bool isplay = true)
    {
        window.sprite = ResManager.GetSprite("findhome_sprite", "window_close");
        window.rectTransform.sizeDelta = new Vector2(92, 100);
        photo.gameObject.SetActive(false);
        if (isplay)
        {
            FindHomeCtr.instance.playEffectSound("window_close");
        }
    }
    public void open()
    {
        window.sprite = ResManager.GetSprite("findhome_sprite", "window_open");
        window.rectTransform.sizeDelta = new Vector2(248, 212);
        //做渐渐长大的效果
        photo.gameObject.SetActive(true);
        StartCoroutine(TShowPhoto());
        //photoctr.PlaySpine("Idle", true);
    }
    public void setData(int _id,int h,int v)
    {
        id = _id;
        mNumH = h;
        mNumV = v;
        window = UguiMaker.newImage("window", transform, "findhome_sprite", "window_close");

        id_animal = FindHomeCtr.animalNames[id % 13];// Common.GetRandValue(0, 13);

        window.raycastTarget = false;

        photoctr = ResManager.GetPrefab("findhome_prefab", "animal" + id_animal).GetComponent<RoleSpine>();
        photo = UguiMaker.InitGameObj(photoctr.gameObject, transform, "animal", Vector3.zero, Vector3.one * 0.2f);
        photoctr.transform.localPosition = new Vector3(0, -35, 0);
        photoctr.PlaySpine("Idle", true);

        //photo = UguiMaker.newImage("photo", transform, "findhome_sprite", "animal" + id_animal);
       // photo.rectTransform.localPosition = new Vector3(0, -10, 0);
        //photo.rectTransform.sizeDelta = new Vector2(50, 50);



        float offset_y = -10f;
        numGo = UguiMaker.newGameObject("numGo", transform);



        wenbg = UguiMaker.newImage("bg", numGo.transform, "findhome_sprite", "kuang_2", false);
        wenbg.type = Image.Type.Tiled;
        wenbg.rectTransform.anchoredPosition = new Vector2(0, -69.98f + offset_y);
        wenbg.rectTransform.sizeDelta = new Vector2(125.5f, 40);
        wenbg.raycastTarget = false;


        wenred = UguiMaker.newImage("wenred", numGo.transform, "findhome_sprite", "num_black_" + h);
        wenred.rectTransform.anchoredPosition = new Vector2(-44.9f, -70 + offset_y);
        wenred.rectTransform.localScale = Vector3.one * temp_scale;
        wenred.raycastTarget = false;
        boxl = wenred.gameObject.AddComponent<BoxCollider>();
        boxl.size = new Vector3(71, 61, 1);
        boxl.center = new Vector3(11, 0, 0);

        Image num1 = UguiMaker.newImage("num1", numGo.transform, "findhome_sprite", "ceng");
        num1.rectTransform.anchoredPosition = new Vector2( -14.9f, -70 + offset_y);
        num1.rectTransform.localScale = Vector3.one * temp_scale;

        wenblack = UguiMaker.newImage("wenblack", numGo.transform, "findhome_sprite", "num_black_" + v);
        wenblack.rectTransform.anchoredPosition = new Vector2(13.5f, -70 + offset_y);
        wenblack.rectTransform.localScale = Vector3.one * temp_scale;
        wenblack.raycastTarget = false;
        boxr = wenblack.gameObject.AddComponent<BoxCollider>();
        boxr.size = new Vector3(67, 61, 1);
        boxr.center = new Vector3(16.2f, 0, 0);

        Image num2 = UguiMaker.newImage("num2", numGo.transform, "findhome_sprite", "hao");
        num2.rectTransform.anchoredPosition = new Vector2(40, -70 + offset_y);
        num2.rectTransform.localScale = Vector3.one * temp_scale;

        numGo.SetActive(false);
    }
    public void showNum(bool all,bool left,bool right)
    {
        if (all)
        {
            if (left)
            {
                wenred.sprite = ResManager.GetSprite("findhome_sprite", "num_black_" + mNumH);
                boxl.enabled = false;
            }
            else
            {
                wenred.sprite = ResManager.GetSprite("findhome_sprite", "wen_black");
                boxl.enabled = true;
            }
            if (right)
            {
                wenblack.sprite = ResManager.GetSprite("findhome_sprite", "num_black_" + mNumV);
                boxr.enabled = false;
            }
            else
            {
                wenblack.sprite = ResManager.GetSprite("findhome_sprite", "wen_black");
                boxr.enabled = true;
            }
        }else
        {
            wenred.sprite = ResManager.GetSprite("findhome_sprite", "wen_red");
            boxl.enabled = true;
            wenblack.sprite = ResManager.GetSprite("findhome_sprite", "wen_black");
            boxr.enabled = true;
        }
        wenred.SetNativeSize();
        wenblack.SetNativeSize();
        numGo.SetActive(true);

    }
    public void setState(bool _stae)
    {
        mState = _stae;
        if (!_stae)
        {
            StartCoroutine(TScale(wenred, "wen_black"));
        }
    }
    public void setInpudata(string numstr,string type)
    {

        if(type == "left")
        {
            if(numstr != "")
            {
                wenred.sprite = ResManager.GetSprite("findhome_sprite", "num_black_" + numstr);
                currH = int.Parse(numstr);
                boxl.enabled = false;
                //FindHomeCtr.instance.playEffectSound("choose_ok");
            }else
            {
                wenred.sprite = ResManager.GetSprite("findhome_sprite", "wen_red");
                currH = -1;
                boxl.enabled = true;
                //FindHomeCtr.instance.playEffectSound("choose_error");
            }
            

        }else if(type == "right")
        {
            if (numstr != "")
            {
                wenblack.sprite = ResManager.GetSprite("findhome_sprite", "num_black_" + numstr);
                currV = int.Parse(numstr);
                boxr.enabled = false;
                //FindHomeCtr.instance.playEffectSound("choose_ok");
            }
            else
            {
                currV = -1;
                wenblack.sprite = ResManager.GetSprite("findhome_sprite", "wen_black");
                boxr.enabled = true;
                //FindHomeCtr.instance.playEffectSound("choose_error");
            }
            
        }
    }
    public void setWenBgCorrect()//设置问题背景
    {
        wenbg.sprite = ResManager.GetSprite("findhome_sprite", "kuang_3");
    }
    public void setWenBgCorrectde()//设置问题背景
    {
        wenbg.sprite = ResManager.GetSprite("findhome_sprite", "kuang_2");
    }
    public bool checkFinish()
    {
        bool boo = true;
        if(currH != mNumH)
        {
            boo = false;
            if(currH != -1)
            {
                FindHomeCtr.instance.playEffectSound("choose_error");
                StartCoroutine(TScale(wenred, "wen_black"));
                currH = -1;
                boxl.enabled = true;
                return false;
            }
        }

        if (currV != mNumV)
        {
            boo = false;
            if (currV != -1)
            {
                FindHomeCtr.instance.playEffectSound("choose_error");
                StartCoroutine(TScale(wenblack, "wen_black"));
                currV = -1;
                boxr.enabled = true;
                return false;
            }
        }
        setState(true);
        FindHomeCtr.instance.playEffectSound("choose_ok");
        return boo;
    }
    public void close()
    {
        currV = -1;
        currH = -1;
        setState(false);
        numGo.SetActive(false);
    }
    IEnumerator TShowPhoto()
    {
        for (float j = 0; j < 1f; j += 0.05f)
        {
            photo.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 0.2f, j);
            yield return new WaitForSeconds(0.01f);
        }
        photo.transform.localScale = Vector3.one * 0.2f;
    }
    IEnumerator TScale(Image img,string spriteName)
    {
        wenbg.sprite = ResManager.GetSprite("findhome_sprite", "kuang_1");
        for (float j = 0; j < 1f; j += 0.05f)
        {
            float p = Mathf.Sin(Mathf.PI * j) * 0.8f;
            //img.rectTransform.localScale = Vector2.Lerp(Vector2.zero, Vector3.one * temp_scale, j) + new Vector2(p, p);
            
            numGo.transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * j) * 10);
            yield return new WaitForSeconds(0.01f);
        }
        //img.rectTransform.localScale = Vector3.one * temp_scale;
        img.sprite = ResManager.GetSprite("findhome_sprite", spriteName);
        numGo.transform.localEulerAngles = Vector3.zero;
        wenbg.sprite = ResManager.GetSprite("findhome_sprite", "kuang_2");

    }
}
