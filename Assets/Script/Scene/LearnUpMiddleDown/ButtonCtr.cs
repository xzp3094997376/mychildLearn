using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class ButtonCtr : MonoBehaviour {
    public static ButtonCtr gSlect { get; set; }
    public int MID { get; set; }
    private GameObject LightContain { get; set; }
    private GameObject GoodsContain { get; set; }
    private List<Image> goodsList = new List<Image>();
    private List<Image> effectList = new List<Image>();
    //private Image effect { get; set; }
    //实际上表示层数，最上层为0
    public void setData(int id)
    {
        MID = id;
        if(null == LightContain)
        {
            LightContain = UguiMaker.newGameObject("LightContain", transform);
        }
        List<Vector3> list = GetLightPosList(MID);
        int leng = 18;
        if(null != list)
        {
            leng = list.Count;
        }
        for (int i = 0; i < leng; i++)
        {
            Image light = UguiMaker.newImage("light_" + i, LightContain.transform, "learnupmiddledown_sprite", "light_close");
            BoxCollider box = light.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(50, 50, 0);
            light.rectTransform.localPosition = list[i];
            Image effect = UguiMaker.newImage("effect", transform, "learnupmiddledown_sprite", "light_effect");
            effect.enabled = false;
            effect.rectTransform.localPosition = list[i];
            effectList.Add(effect);
        }
        /*
        if(null == effect)
        {
            effect = UguiMaker.newImage("effect", transform, "learnupmiddledown_sprite", "light_effect");
            effect.enabled = false;
        }
        */
    }
    public void OpenLight()
    {
        StartCoroutine(TOpenLight());
    }
    public void PlayLight()
    {
        StartCoroutine(TStartPlayLight());
    }
    public void cleanGoods()
    {
        for (int i = 0; i < GoodsContain.transform.childCount; i++)
        {
            GameObject.Destroy(GoodsContain.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < LightContain.transform.childCount; i++)
        {
            Image light = LightContain.transform.GetChild(i).gameObject.GetComponent<Image>();
            light.sprite = ResManager.GetSprite("learnupmiddledown_sprite", "light_close");
        }
    }
    public Vector3 setChild(GoodsCtr goods)
    {
        Vector3 pos = Vector3.zero;
        BoxCollider box = goods.gameObject.GetComponent<BoxCollider>();
        box.enabled = false;
        
        if(null == GoodsContain)
        {
            GoodsContain = UguiMaker.newGameObject("GoodsContain", transform);
        }
        goods.transform.parent = GoodsContain.transform;
        goods.transform.localScale = Vector3.one * 0.8f;

        if (GoodsContain.transform.childCount > 1)
        {
            Vector3 startPos = new Vector3(-60, 0, 0);
            int index = 0;
            for(int i = 0;i < GoodsContain.transform.childCount; i++)
            {
                Transform child = GoodsContain.transform.GetChild(i);
                if(child.name != "LightContain")
                {
                    pos = startPos + new Vector3(index * 150, 0, 0);
                    child.localPosition = pos;
                    index++;
                }
            }
        }else
        {
            pos = new Vector3(0, 0, 0);
            goods.transform.localPosition = pos;
        }
        return pos;
    }
    private int currIndex = 0;
    public string getGoodsName()
    {
        string outname = goodsList[indexList[currIndex]].gameObject.GetComponent<GoodsCtr>().mGoodsName;
        currIndex++;
        return outname;
    }
    private List<string> userList = new List<string>();
    private List<int> indexList = new List<int>();
    public void createGoods(List<string> list)
    {
        goodsList.Clear();
        userList.Clear();
        currIndex = 0;
        for (int i = 0; i < 3; i++)
        {
            string str = list[i];
            userList.Add(str);
        }
        userList = Common.BreakRank(userList);

        if (null == GoodsContain)
        {
            GoodsContain = UguiMaker.newGameObject("GoodsContain", transform);
        }
        Vector3 startPos = new Vector3(-150, 0, 0);
        for (int i = 0;i < userList.Count; i++)
        {
            Image goods = UguiMaker.newImage("goods_" + i, GoodsContain.transform, "learnupmiddledown_sprite", userList[i]);
            GoodsCtr goodsc = goods.gameObject.AddComponent<GoodsCtr>();
            BoxCollider box = goods.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(100, 100, 0);
            Vector3 pos = startPos + new Vector3(i * 150, 0, 0);
            goods.rectTransform.localPosition = pos;
            goods.rectTransform.localScale = Vector3.zero;
            goodsc.setDef(i, pos, userList[i], MID);
            goodsList.Add(goods);
            indexList.Add(i);
        }
        indexList = Common.BreakRank(indexList);
        StartCoroutine(TShowGoods());
    }
    IEnumerator TShowGoods()
    {
        //mSound.PlayShort("learnupmiddledown_sound", "素材出现通用音效");
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one * 0.8f;
        for (int i = 0; i < 3; i++)
        {
            LearnUpMiddleDownCtr.mTrans.playShowGoodsSound();
            Image goods = goodsList[i];
            GoodsCtr goodsc = goods.gameObject.GetComponent<GoodsCtr>();
            goodsc.ShowOut(0.8f);
            yield return new WaitForSeconds(0.4f);
        }
    }
    IEnumerator TOpenLight()
    {
        //yield return new WaitForSeconds(1f);
        for (int i = 0; i < LightContain.transform.childCount; i++)
        {
            Image light = LightContain.transform.GetChild(i).gameObject.GetComponent<Image>();
            //StartCoroutine(TPlayLight(i, light));
            light.sprite = ResManager.GetSprite("learnupmiddledown_sprite", "light_open");
            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator TStartPlayLight()
    {
        for (int i = 0; i < LightContain.transform.childCount; i++)
        {
            //Image light = LightContain.transform.GetChild(i).gameObject.GetComponent<Image>();
            Image effect = effectList[i];
            Color color = effect.color;
            color.a = 1;
            effect.color = color;
            effect.enabled = true;
            //StartCoroutine(TPlayLight(i, light));
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.02f);
        for (int i = 0; i < LightContain.transform.childCount; i++)
        {
            Image effect = effectList[i];
            StartCoroutine(TClose(effect));
        }
        
    }
    IEnumerator TClose(Image effect)
    {
        Color startColor = effect.color;
        startColor.a = 1;
        Color endColor = effect.color;
        endColor.a = 0;
        for (int j = 0; j < 4; j++)
        {
            effect.color = startColor;
            //light.sprite = ResManager.GetSprite("learnupmiddledown_sprite", spName);
            yield return new WaitForSeconds(0.2f);
            effect.color = endColor;
            //light.sprite = ResManager.GetSprite("learnupmiddledown_sprite", "light_3");
            yield return new WaitForSeconds(0.05f);
        }
        effect.color = startColor;
        yield return new WaitForSeconds(0.05f);

        for (float j = 0; j < 1f; j += 0.1f)
        {
            effect.color = Color.Lerp(startColor, endColor, j);
            yield return new WaitForSeconds(0.01f);
        }
        effect.color = endColor;
    }
    IEnumerator TPlayLight(int index, Image light)
    {
        light.enabled = true;
        Color color = Color.red;
        if(index % 2 == 1)
        {
            color = Color.green;
        }
        for (int j = 0; j < 10; j++)
        {
            light.color = color;
            //light.sprite = ResManager.GetSprite("learnupmiddledown_sprite", spName);
            yield return new WaitForSeconds(0.2f);
            light.color = Color.white;
            //light.sprite = ResManager.GetSprite("learnupmiddledown_sprite", "light_3");
            yield return new WaitForSeconds(0.05f);
        }
        
    }
    private List<Vector3> GetLightPosList(int type)
    {
        List<Vector3> list = null;
        switch (type)
        {
            case 0:
                list = new List<Vector3>(){
                    new Vector3( -347f, 10f, 0f),
                    new Vector3( -262f, 69f, 0f),
                    new Vector3( -161f, 105f, 0f),
                    new Vector3( -50f, 121f, 0f),
                    new Vector3( 42f, 123f, 0f),
                    new Vector3( 126f, 115f, 0f),
                    new Vector3( 218f, 92f, 0f),
                    new Vector3( 297f, 59f, 0f),
                    new Vector3( 372f, 2f, 0f),
                    new Vector3( 421f, -77f, 0f),
                    new Vector3( 322f, -80f, 0f),
                    new Vector3( 222f, -77f, 0f),
                    new Vector3( 115f, -77f, 0f),
                    new Vector3( 0f, -73f, 0f),
                    new Vector3( -115f, -69f, 0f),
                    new Vector3( -226f, -65f, 0f),
                    new Vector3( -320f, -63f, 0f),
                    new Vector3( -389f, -61f, 0f)
                };
                break;
            case 1:
                list = new List<Vector3>(){
                    new Vector3( -390f, 98f, 0f),
                    new Vector3( -320f, 98f, 0f),
                    new Vector3( -226f, 96f, 0f),
                    new Vector3( -115f, 92f, 0f),
                    new Vector3( 0f, 88f, 0f),
                    new Vector3( 0f, 88f, 0f),
                    new Vector3( 115f, 84f, 0f),
                    new Vector3( 223f, 84f, 0f),
                    new Vector3( 322f, 80f, 0f),
                    new Vector3( 420f, 82f, 0f),
                    new Vector3( 439f, 25f, 0f),
                    new Vector3( 447f, -29f, 0f),
                    new Vector3( 441f, -84f, 0f),
                    new Vector3( 322f, -86f, 0f),
                    new Vector3( 224f, -82f, 0f),
                    new Vector3( 117f, -78f, 0f),
                    new Vector3( 0f, -74f, 0f),
                    new Vector3( -117f, -74f, 0f),
                    new Vector3( -224f, -68f, 0f),
                    new Vector3( -324f, -68f, 0f),
                    new Vector3( -416f, -66f, 0f),
                    new Vector3( -422f, -12f, 0f),
                    new Vector3( -414f, 45f, 0f)
                };
                break;
            case 2:
                list = new List<Vector3>(){
                    new Vector3( -416f, 94f, 0f),
                    new Vector3( -324f, 92f, 0f),
                    new Vector3( -224f, 92f, 0f),
                    new Vector3( -115f, 86f, 0f),
                    new Vector3( 0f, 86f, 0f),
                    new Vector3( 119f, 82f, 0f),
                    new Vector3( 224f, 78f, 0f),
                    new Vector3( 320f, 74f, 0f),
                    new Vector3( 441f, 76f, 0f),
                    new Vector3( 422f, 18f, 0f),
                    new Vector3( 385f, -39f, 0f),
                    new Vector3( 334f, -80f, 0f),
                    new Vector3( 265f, -78f, 0f),
                    new Vector3( -178f, -70f, 0f),
                    new Vector3( -303f, -68f, 0f),
                    new Vector3( -363f, -31f, 0f),
                    new Vector3( -400f, 20f, 0f)
                };
                break;
        }
        return list;
    }
}
