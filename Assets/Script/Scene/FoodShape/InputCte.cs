using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class InputCte : MonoBehaviour {
    private Image bg;
    private GameObject contain { get; set; }
    private System.Action mBackFun { get; set; }
    public string chooseName { get; set; }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit2D)
            {
                if (hit2D.collider.gameObject != gameObject || null == hit2D.collider.gameObject)
                {
                    close();
                }
            }else
            {
                close();
            }
        }
    }
    public void setData(FoodShapeCtr.Guanka guanka, int itemsNum,System.Action backFun)
    {
        mBackFun = backFun;
        if (null == bg)
        {
            bg = UguiMaker.newImage("inputbg", transform, "foodshape_sprite", "mp_2_inputbg");
            BoxCollider2D box2d = gameObject.AddComponent<BoxCollider2D>();
            box2d.size = bg.rectTransform.sizeDelta;
        }else
        {
            if(guanka.guanka == 3 && itemsNum == 6)
            {
                bg.sprite = ResManager.GetSprite("foodshape_sprite", "mp_" + 2 + "_inputbg");
            }else
            {
                bg.sprite = ResManager.GetSprite("foodshape_sprite", "mp_" + guanka.guanka + "_inputbg");
            }
            
        }
        bg.SetNativeSize();
        GameObject.Destroy(contain);
        contain = UguiMaker.newGameObject("contain", transform);
        if (guanka.guanka == 3)
        {
            setMap3Data(guanka,itemsNum);
        }
        else
        {
            setMap2Data(itemsNum);
        }
        
        
    }
    private void setMap2Data(int itemsNum)
    {
        Vector3 startPos = new Vector3(-34, 63, 0);
        for (int i = 0; i < 6; i++)
        {
            int index = i + 1;
            Image mkbg = UguiMaker.newImage("mk_" + index, contain.transform, "foodshape_sprite", "mk_" + index);
            mkbg.transform.localScale = Vector3.one * 0.6f;
            BoxCollider box = mkbg.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(90, 90) * 0.6f;
            mkbg.transform.localPosition = startPos + new Vector3((i % 2) * 70, -(int)(i / 2) * 64, 0);
            Button btn = mkbg.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            EventTriggerListener.Get(btn.gameObject).onDown = BlockDown;
            EventTriggerListener.Get(btn.gameObject).onUp = BlockUp;
        }
    }
    private void BlockDown(GameObject _go)
    {
        //Debug.Log("BlockDown : " + _go.name);
        chooseName = _go.name;
        close();
    }
    private void BlockUp(GameObject _go)
    {
        
    }
    public void show()
    {
        chooseName = null;
        gameObject.active = true;
    }
    public void close()
    {
        mBackFun();
        gameObject.active = false;
    }
    private void setMap3Data(FoodShapeCtr.Guanka guanka,int itemsNum)
    {
        Vector3 startPos = new Vector3(-34, 63, 0);
        int disW = 70;
        int disH = 65;
        float scalNum = 0.15f;
        if(itemsNum == 4)
        {
            startPos = new Vector3(-41, 38, 0);
            disW = 80;
            disH = 80;
            scalNum = 0.2f;
        }

        List<float> angels = guanka.guanka3Angels3 ;
      
        if (itemsNum < 6)
        {
            angels = guanka.guanka3Angels4;
            /*
            for (int i = 1; i < 4; i++)
            {
                angels.Add(i * 90);
            }
            */
        }else
        {
            /*
            int index = 0;
            for (int i = 0; i < 7; i++)
            {
                if(i != guanka.answerIndex && index < 4)
                {
                    angels.Add(i * 90);
                    index++;
                }
                
            }
            */
        }
        /*
        Debug.Log("answerAngel : " + guanka.answerIndex * 90);
        angels.Add(guanka.answerIndex * 60);
        angels = Common.BreakRank(angels);
        */
        for (int i = 0; i < angels.Count; i++)
        {
            int index = i + 1;
            float currAngel = angels[i];
            Image mkbg = UguiMaker.newImage("sugbg_" + currAngel, contain.transform, "foodshape_sprite", "sugbg");
            mkbg.transform.localScale = Vector3.one * scalNum;
            BoxCollider box = mkbg.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(300, 300);
            mkbg.transform.localPosition = startPos + new Vector3((i % 2) * disW, -(int)(i / 2) * disH, 0);
            Button btn = mkbg.gameObject.AddComponent<Button>();
            EventTriggerListener.Get(btn.gameObject).onDown = BlockDown;
            EventTriggerListener.Get(btn.gameObject).onUp = BlockUp;
            mkbg.transform.localEulerAngles = new Vector3(0, 0, guanka.dir * currAngel);
        }
        if (itemsNum < 6) return;

        int itemIndex = 1;
        for (int i = 3; i < 6; i++)
        {
            Image mkbg = UguiMaker.newImage("item_" + itemIndex, contain.transform, "foodshape_sprite", "item_" + itemIndex);
            mkbg.transform.localScale = Vector3.one * 0.5f;
            BoxCollider box = mkbg.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(300, 300);
            mkbg.transform.localPosition = startPos + new Vector3((i % 2) * disW, -(int)(i / 2) * disH, 0);
            Button btn = mkbg.gameObject.AddComponent<Button>();
            EventTriggerListener.Get(btn.gameObject).onDown = BlockDown;
            EventTriggerListener.Get(btn.gameObject).onUp = BlockUp;
            itemIndex++;
        }
    }
}
