using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class LearnUpMiddleDownCtr : MonoBehaviour {
    public Guanka mGuanka = new Guanka();
    public static LearnUpMiddleDownCtr mTrans { get; set; }
    public SoundManager mSound { get; set; }
    private RawImage bg { get; set; }
    private bool inpass { get; set; }
    private Image effect { get; set; }
    private Image effect1 { get; set; }
    private Image effect2 { get; set; }
    private ParticleSystem mStarparticle { get; set; }
    private List<GoodsCtr> goodsImages = new List<GoodsCtr>();
    private List<ButtonCtr> buttonList = new List<ButtonCtr>();
    private GameObject buttonContain { get; set; }
    private GameObject goodsContain { get; set; }
    public class Guanka
    {
        public int guanka { get; set; }
        public int guanka_last { get; set; }
        public List<string> goodsList = new List<string>();
        public int currType { get; set; }
        public string currGoodsName { get; set; }
        public List<int> buttonTypeList = new List<int>() { 0, 1, 2 };
        public List<int> commandTypelist { get; set; }

        public Guanka()
        {
            guanka_last = 2;
        }
        public void Set(int _guanka)
        {
            guanka = _guanka;
            goodsList.Clear();
            List<string> stampList = Common.BreakRank(new List<string>() { "bingbangqiu", "gangbi", "kuzi", "maozi", "zuqiu", "qianbi", "shoutao", "shu", "xiangbica", "xiezi", "yifu", "yumaoqiu", "zhentou" });
            for(int i = 0;i < 5; i++)
            {
                goodsList.Add(stampList[i]);
            }
            commandTypelist = Common.BreakRank(new List<int>() { 0,1,2,3,4});
            buttonTypeList = Common.BreakRank(buttonTypeList);
            currType = 0;
            switch (_guanka)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }
        public int getFindId()
        {
            return commandTypelist[buttonTypeList[currType]];
        }
    }
    void Awake()
    {
        mSound = gameObject.AddComponent<SoundManager>();
    }
    // Use this for initialization
    void Start () {
        bg = UguiMaker.newRawImage("bg", transform, "learnupmiddledown_texture", "bg", false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        mSound.PlayBgAsync("bgmusic_loop3", "bgmusic_loop3", 0.1f);
        StartCoroutine(TInitAS());
        mTrans = this;


    }
    List<Vector3> roadPoss = new List<Vector3>();
    // Update is called once per frame
    Vector3 temp_select_offset = Vector3.zero;
	void Update () {

        if (inpass) return;
        //鼠标按下
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            if (null != hits)
            {
                GoodsCtr.gSlect = null;
                ButtonCtr.gSlect = null;
                foreach (RaycastHit hit in hits)
                {
                    GoodsCtr com = hit.collider.gameObject.GetComponent<GoodsCtr>();
                    if(null != com)
                    {
                        GoodsCtr.gSlect = com;
                        GoodsCtr.gSlect.PoP();
                        mSound.StopTip();
                        mSound.PlayShort("learnupmiddledown_sound", "checkgamebtn_up");
                        temp_select_offset = GoodsCtr.gSlect.gameObject.GetComponent<RectTransform>().anchoredPosition3D - Common.getMouseLocalPos(transform);
                        if(mGuanka.guanka == 2)
                        {
                            Canvas canvasinput = com.gameObject.GetComponent<Canvas>();
                            if(null == canvasinput)
                            {
                                
                                canvasinput = com.gameObject.AddComponent<Canvas>();
                            }
                            com.gameObject.layer = LayerMask.NameToLayer("UI");
                            canvasinput.overrideSorting = true;
                            canvasinput.sortingOrder = 4;
                            ButtonCtr.gSlect = com.transform.parent.parent.gameObject.GetComponent<ButtonCtr>();
                            ButtonCtr.gSlect.transform.SetAsLastSibling();
                        }
                        break;
                    }

                    /*
                    Image posImage = hit.collider.gameObject.GetComponent<Image>();
                    if (null != posImage && posImage.name == "OutPoint")
                    {
                        //StartCoroutine(TPlayEffct());
                    }
                    
                    Image posImage = hit.collider.gameObject.GetComponent<Image>();
                    if (posImage.name == "OutPoint")
                    {
                        string str = "new List<Vector3>(){\n";
                        for (int i = 0; i < roadPoss.Count; i++)
                        {
                            Vector3 pos = roadPoss[i];
                            str += "new Vector3( " + pos.x + "f, " + pos.y + "f, " + pos.z + "f),\n";
                        }
                        str += "};";
                        Debug.LogError(str);
                    }
                    else
                    {
                        Debug.Log(posImage.name);
                        string[] arr = posImage.name.Split('_');
                        if (arr.Length > 1 && arr[0] == "light")
                        {
                            posImage.color = Color.black;
                            roadPoss.Add(posImage.transform.localPosition);
                        }

                    }
                    //*/
                }
            }
        }
        //鼠标滑动
        if (Input.GetMouseButton(0))
        {
            if (null != GoodsCtr.gSlect)
            {
                GoodsCtr.gSlect.gameObject.GetComponent<RectTransform>().anchoredPosition3D = Common.getMouseLocalPos(transform) + temp_select_offset;
                /*
                if (mGuanka.guanka == 2)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(ray);
                    GameObject hitGo = null;
                    if(null != hits)
                    {
                        foreach (RaycastHit hit in hits)
                        {
                            hitGo = hit.collider.gameObject;
                            if (null != hitGo && hitGo == goodsContain)
                            {
                                if(GoodsCtr.gSlect.transform.parent != goodsContain.transform)
                                {
                                    GoodsCtr.gSlect.transform.parent = goodsContain.transform;
                                    GoodsCtr.gSlect.transform.SetAsLastSibling();
                                }
                            }
                        }
                    }
                    
                }
                //*/
            }
        }
        //鼠标抬起
        if (Input.GetMouseButtonUp(0))
        {
            if (null != GoodsCtr.gSlect)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray);
                bool isGuanka2Hit = false;
                if (null != hits)
                {
                    ButtonCtr com = null;
                    GameObject hitGo = null;
                    foreach (RaycastHit hit in hits)
                    {
                        if(mGuanka.guanka == 1)
                        {
                            com = hit.collider.gameObject.GetComponent<ButtonCtr>();
                            if (null != com)
                            {
                                if ((GoodsCtr.gSlect.mID == mGuanka.getFindId() && com.MID == mGuanka.buttonTypeList[mGuanka.currType]) || (isLastType && GoodsCtr.gSlect.mID == mGuanka.commandTypelist[3] && com.MID == mGuanka.buttonTypeList[mGuanka.currType]))
                                {
                                    inpass = true;
                                    mSound.PlayShort("learnupmiddledown_sound", "奖章解冻的特效声");
                                    Vector3 pos = com.setChild(GoodsCtr.gSlect);
                                    com.transform.SetAsLastSibling();
                                    com.PlayLight();
                                    PlayStarParticle(com.transform.localPosition + pos);
                                    StartCoroutine(TNextCommand());
                                }
                                else
                                {
                                    mSound.PlayShort("learnupmiddledown_sound", "指针反弹");
                                    string errName = "game-encourage2-4-1";
                                    if(Random.Range(0, 1000) % 3 == 0)
                                    {
                                        errName = "game-tips2-6-1-9";
                                    }else if(Random.Range(0,1000) % 3 == 1)
                                    {
                                        errName = "game-encourage2-4-2";
                                    }
                                    mSound.PlayTip("learnupmiddledown_sound", errName);
                                    GoodsCtr.gSlect.MovetoDef();
                                }
                            }
                        }else //第二关
                        {
                            Canvas canvasinput = GoodsCtr.gSlect.gameObject.GetComponent<Canvas>();
                            if (null != canvasinput)
                            {
                                GameObject.Destroy(canvasinput);
                            }
                            hitGo = hit.collider.gameObject;
                            if(null != hitGo && hitGo.name.Split('_')[0] == "gezi")
                            {
                                isGuanka2Hit = true;
                                //Debug.Log("GoodsCtr.gSlect.mGoodsName : " + GoodsCtr.gSlect.mGoodsName + ";mGuanka.currGoodsName : " + mGuanka.currGoodsName + ";GoodsCtr.gSlect.mType : " + GoodsCtr.gSlect.mType + ";mGuanka.currType : " + mGuanka.buttonTypeList[mGuanka.currType]);
                                if (GoodsCtr.gSlect.mGoodsName == mGuanka.currGoodsName && GoodsCtr.gSlect.mType == mGuanka.buttonTypeList[mGuanka.currType])
                                {
                                    inpass = true;
                                    mSound.PlayShort("learnupmiddledown_sound", "奖章解冻的特效声");
                                    ButtonCtr button = ButtonCtr.gSlect;
                                    BoxCollider buttonbox = ButtonCtr.gSlect.gameObject.GetComponent<BoxCollider>();
                                    buttonbox.enabled = false;
                                    button.PlayLight();
                                    Vector3 pos = hitGo.transform.localPosition;
                                    setGoodsToContain(GoodsCtr.gSlect, hitGo.name, pos);
                                    PlayStarParticle(pos);
                                    StartCoroutine(TNextCommand());
                                }else
                                {
                                    Debug.Log("who back : hit");
                                    mSound.PlayShort("learnupmiddledown_sound", "指针反弹");
                                    mSound.PlayTip("learnupmiddledown_sound", "game-tips2-6-1-9");
                                    GoodsCtr.gSlect.MovetoDef();
                                }
                                break;
                            }
                        }
                        
                    }
                }

                if(mGuanka.guanka == 2)
                {
                    if (!isGuanka2Hit)
                    {
                        Debug.Log("who back : not hit");
                        mSound.PlayShort("learnupmiddledown_sound", "指针反弹");
                        mSound.PlayTip("learnupmiddledown_sound", "game-tips2-6-1-9");
                        GoodsCtr.gSlect.MovetoDef();
                    }
                   
                }
            }
            GoodsCtr.gSlect = null;
            ButtonCtr.gSlect = null;
        }
    }
    Dictionary<string, bool> containStateDic = new Dictionary<string, bool>();
    Dictionary<string, GoodsCtr> goodsDic = new Dictionary<string, GoodsCtr>();
    //设置白布上面的物件
    private Vector3 setGoodsToContain(GoodsCtr goods,string geziName,Vector3 _pos)
    {
        Vector3 outPos = new Vector3(0, -324, 0);
        goods.gameObject.GetComponent<BoxCollider>().enabled = false;
        goods.transform.parent = goodsContain.transform;
        goods.transform.localScale = Vector3.one;
        /*
        Vector3 startPos = Vector3.zero;
        switch (goodsContain.transform.childCount)
        {
            case 1:
                startPos = new Vector3(0, -324, 0);
                break;
            case 2:
                startPos = new Vector3(-100, -324, 0);
                break;
            case 3:
                startPos = new Vector3(-200, -324, 0);
                break;
            case 4:
                startPos = new Vector3(-300, -324, 0);
                break;
        }
        for(int i = 0;i < goodsContain.transform.childCount; i++)
        {
            Transform _goods = goodsContain.transform.GetChild(i);
            Vector3 pos = startPos + new Vector3(i * 200, 0, 0);
            _goods.localPosition = pos;
            outPos = pos;
        }
        */
        goods.transform.localPosition = _pos;
        string[] arr = geziName.Split('_');
        int index = int.Parse(arr[1]);
        if (containStateDic[geziName])
        {
            bool ismoveToLeft = false;
            for(int i = index - 1;i >= 0; i--)
            {
                string key = "gezi_" + i;
                if (!containStateDic[key])
                {
                    ismoveToLeft = true;
                    break;
                }
            }
            bool isFind = false;
            Debug.Log("ismoveToLeft : " + ismoveToLeft);
            if (ismoveToLeft)
            {
                for (int i = 0; i <= index; i++)
                {
                    string key = "gezi_" + i;
                    Debug.Log("get containStateDic key : " + key);
                    if (!containStateDic[key])
                    {
                        isFind = true;
                        continue;
                    }
                    if (isFind)
                    {
                        string upKey = "gezi_" + (i - 1);
                        GoodsCtr findgoods = goodsDic[key];
                        Transform gezitra = goodsContain.transform.Find(upKey);
                        //findgoods.transform.localPosition = gezitra.transform.localPosition;
                        StartCoroutine(TMoveToPos(findgoods, gezitra.transform.localPosition));
                        goodsDic[upKey] = findgoods;
                        containStateDic[key] = false;
                        containStateDic[upKey] = true;
                    }
                }
            }else
            {
                for (int i = 3; i >= index; i--)
                {
                    string key = "gezi_" + i;
                    if (!containStateDic[key])
                    {
                        isFind = true;
                        continue;
                    }
                    if (isFind)
                    {
                        string upKey = "gezi_" + (i + 1);
                        GoodsCtr findgoods = goodsDic[key];
                        Transform gezitra = goodsContain.transform.Find(upKey);
                        //findgoods.transform.localPosition = gezitra.transform.localPosition;
                        StartCoroutine(TMoveToPos(findgoods, gezitra.transform.localPosition));
                        goodsDic[upKey] = findgoods;
                        containStateDic[key] = false;
                        containStateDic[upKey] = true;
                    }
                }
            }
        }
        containStateDic[geziName] = true;
        goodsDic[geziName] = goods;
        return outPos;
    }
    IEnumerator TMoveToPos(GoodsCtr go,Vector3 endPos)
    {
        Vector3 startPos = go.transform.localPosition;
        for (float j = 0; j < 1f; j += 0.2f)
        {
            go.transform.localPosition = Vector3.Lerp(startPos, endPos, j);
            yield return new WaitForSeconds(0.01f);
        }
        go.transform.localPosition = endPos;
    }
    private void reGame()
    {
        setGameData(1, false);
    }
    //设置游戏数据
    private void setGameData(int guanka,bool isInit)
    {
        if (guanka == 1)
        {
            TopTitleCtl.instance.Reset();
        }
        else
        {
            TopTitleCtl.instance.AddStar();
        }
        mGuanka.Set(guanka);
        inpass = false;
        isLastType = false;
        /*
        Image OutPoint = UguiMaker.newImage("OutPoint", transform, "public", "white");
        OutPoint.rectTransform.sizeDelta = new Vector2(50, 50);
        OutPoint.rectTransform.localPosition = new Vector3(-542, 0, 0);
        BoxCollider outbox = OutPoint.gameObject.AddComponent<BoxCollider>();
        outbox.size = new Vector3(50, 50);
        */
        if (isInit)
        {
            if(null == buttonContain)
            {
                buttonContain = UguiMaker.newGameObject("buttonContain", transform);
            }
            Vector3 startPos = new Vector3(0, 152f, 0);
            for (int i = 0; i < 3; i++)
            {
                Image PosButtom = UguiMaker.newImage("button_" + i, buttonContain.transform, "public", "white");
                BoxCollider box = PosButtom.gameObject.AddComponent<BoxCollider>();
                box.size = new Vector3(800, 120, 0);
                ButtonCtr button = PosButtom.gameObject.AddComponent<ButtonCtr>();
                button.setData(i);
                Color color = PosButtom.color;
                color.a = 0;
                PosButtom.color = color;
                PosButtom.rectTransform.sizeDelta = new Vector2(800, 120);
                PosButtom.rectTransform.localPosition = startPos + new Vector3(0, -i * 160, 0);
                EventTriggerListener.Get(PosButtom.gameObject).onUp = ClickBtnDown;
                buttonList.Add(button);
            }
        }else
        {
            for (int i = 0; i < 3; i++)
            {
                ButtonCtr button = buttonList[i];
                if(mGuanka.guanka == 1)
                {
                    BoxCollider box = button.gameObject.GetComponent<BoxCollider>();
                    box.enabled = true;
                }
                button.cleanGoods();
                button.transform.SetAsLastSibling();
            }
        }
        if(null == goodsContain)
        {
            goodsContain = UguiMaker.newGameObject("goodsContain", transform);
            //BoxCollider goodsContainBox = goodsContain.AddComponent<BoxCollider>();
            //goodsContainBox.size = new Vector3(900, 120, 0);
            //goodsContainBox.center = new Vector3(0, -320, 0);
        }
        else
        {
            for (int i = 0; i < goodsContain.transform.childCount; i++)
            {
                GameObject.Destroy(goodsContain.transform.GetChild(i).gameObject);
            }
        }
        if (mGuanka.guanka == 2)
        {
            Vector3 startPos = new Vector3(-277, -324, 0);
            for (int i = 0; i < 4; i++)
            {
                string key = "gezi_" + i;
                Image gezi = UguiMaker.newImage(key, goodsContain.transform, "public", "white");
                gezi.rectTransform.sizeDelta = new Vector2(160, 200);
                Color color = gezi.color;
                color.a = 0;
                gezi.color = color;
                gezi.rectTransform.localPosition = startPos + new Vector3(i * 200, 0, 0);
                BoxCollider geziBox = gezi.gameObject.AddComponent<BoxCollider>();
                geziBox.size = new Vector3(200, 200, 0);
                if (containStateDic.ContainsKey(key))
                {
                    containStateDic[key] = false;
                }else
                {
                    containStateDic.Add(key, false);
                }
                Debug.Log("add containStateDic key : " + key);
            }
        }
        goodsImages.Clear();
        if (mGuanka.guanka == 1)
        {
            Vector3 startGoods = new Vector3(-304, -320, 0);
            for (int i = 0; i < mGuanka.goodsList.Count; i++)
            {
                Image goods = UguiMaker.newImage("goods_" + i, goodsContain.transform, "learnupmiddledown_sprite", mGuanka.goodsList[i]);
                
                GoodsCtr goodsc = goods.gameObject.AddComponent<GoodsCtr>();
                BoxCollider box = goods.gameObject.AddComponent<BoxCollider>();
                box.size = new Vector3(100, 100, 0);
                Vector3 pos = startGoods + new Vector3(i * 175, 0, 0);
                goods.rectTransform.localPosition = pos;
                goods.rectTransform.localScale = Vector3.zero;
                goodsc.setDef(i, pos, mGuanka.goodsList[i]);
                goodsImages.Add(goodsc);
            }
        }
        if (null == effect)
        {
            effect = UguiMaker.newImage("effect", transform, "learnupmiddledown_sprite", "effect_1");
            effect.rectTransform.localPosition = new Vector3(504,206, 0);
            effect.rectTransform.localScale = Vector3.zero;

            effect1 = UguiMaker.newImage("effect1", transform, "learnupmiddledown_sprite", "effect");
            effect1.rectTransform.localPosition = new Vector3(-504, -206, 0);
            effect1.rectTransform.localScale = Vector3.zero;

            effect2 = UguiMaker.newImage("effect2", transform, "learnupmiddledown_sprite", "effect_2");
            effect2.rectTransform.localPosition = new Vector3(-504, 206, 0);
            effect2.rectTransform.localScale = Vector3.zero;
        }
        StartCoroutine(TOpenLight());
    }
    IEnumerator TDeleyStart()
    {
        yield return new WaitForSeconds(1f);
        startGame();
    }
    IEnumerator TOpenLight()
    {
        yield return new WaitForSeconds(1f);
        mSound.PlayShort("learnupmiddledown_sound", "stareffect");
        for (int i = 0; i < buttonList.Count; i++)
        {
            ButtonCtr button = buttonList[i];
            button.OpenLight();
        }
        yield return new WaitForSeconds(2f);
        startGame();
    }
    //开始显示物件
    private void startGame()
    {
        if (mGuanka.guanka == 1)
        {
            StartCoroutine(TShowGoods());
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                ButtonCtr button = buttonList[i];
                button.createGoods(mGuanka.goodsList);
            }
            setCommand2();
        }
    }
    public void playShowGoodsSound()
    {
        mSound.PlayShort("learnupmiddledown_sound", "素材出现通用音效");
    }
    //播放特效
    private void PlayStarParticle(Vector3 pos)
    {
        if (null == mStarparticle)
        {
            GameObject obj1 = ResManager.GetPrefab("effect_star2", "effect_star2");
            obj1.transform.parent = transform;
            mStarparticle = obj1.GetComponent<ParticleSystem>();
            mStarparticle.transform.localPosition = Vector3.zero;
            mStarparticle.transform.localScale = Vector3.one;
        }
        mStarparticle.transform.localPosition = pos;
        mStarparticle.Play();
    }
    private void ClickBtnDown(GameObject go)
    {
        if (null != GoodsCtr.gSlect && mGuanka.guanka == 2) return;
        mSound.StopTip();
        switch (go.name)
        {
            case "button_0":
                mSound.PlayOnly("learnupmiddledown_sound", "game-tips2-6-1-1");
                break;
            case "button_1":
                mSound.PlayOnly("learnupmiddledown_sound", "game-tips2-6-1-2");
                break;
            case "button_2":
                mSound.PlayOnly("learnupmiddledown_sound", "game-tips2-6-1-3");
                break;
        }
        ButtonCtr button = go.GetComponent<ButtonCtr>();
        button.transform.SetAsLastSibling();
        button.PlayLight();
    }
    IEnumerator TShowGoods()
    {
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;
        for (int i = 0;i < goodsImages.Count; i++)
        {
            GoodsCtr goods = goodsImages[i];
            mSound.PlayShort("learnupmiddledown_sound", "素材出现通用音效");
            goods.ShowOut();
            yield return new WaitForSeconds(0.4f);
            //goods.transform.DOScale(Vector3.one * 1.2f, 0.8f).SetLoops(3, LoopType.Yoyo);
            //goods.transform.localScale = endScale;
        }
        setCommand();
    }
    //设置关卡2命令
    private void setCommand2()
    {
        ButtonCtr button = buttonList[mGuanka.buttonTypeList[mGuanka.currType]];
        
        mGuanka.currGoodsName = button.getGoodsName(); //mGuanka.goodsList[Common.GetRandValue(0,2)];
        //Debug.Log()
        List<string> abNames = new List<string>() { "learnupmiddledown_sound", "learnupmiddledown_sound", "learnupmiddledown_sound", "learnupmiddledown_sound", "learnupmiddledown_sound" };
        List<string> soundNames = new List<string>() { "game-tips2-6-1-4", "game-tips2-6-1-5", getTypeSoundName(mGuanka.buttonTypeList[mGuanka.currType]), getSoundName(mGuanka.currGoodsName), "game-tips2-6-1-13" };
        mSound.PlayTipList(abNames, soundNames, true);
        inpass = false;
    }
    //设置关卡1命令
    private void setCommand()
    {
        string command = mGuanka.goodsList[mGuanka.getFindId()];
        if (isLastType)
        {
            command = mGuanka.goodsList[mGuanka.commandTypelist[3]];
        }
        List<string> abNames = new List<string>() { "learnupmiddledown_sound", "learnupmiddledown_sound", "learnupmiddledown_sound", "learnupmiddledown_sound" };
        List<string> soundNames = new List<string>() { "game-tips2-6-1-4", "game-tips2-6-1-5", getSoundName(command), getTypeSoundName(mGuanka.buttonTypeList[mGuanka.currType]) };
        mSound.PlayTipList(abNames, soundNames, true);
        inpass = false;
    }
    bool isLastType { get; set; }
    IEnumerator TNextCommand()
    {
        yield return new WaitForSeconds(2);
        if (isLastType)
        {
            StartCoroutine(TPlayEffct());
        }
        else
        {
            mGuanka.currType++;
            if (mGuanka.currType > 2)
            {
                mGuanka.currType = Common.GetRandValue(0, 2);
                isLastType = true;
            }
            if(mGuanka.guanka == 1)
            {
                setCommand();
            }else
            {
                setCommand2();
            }
            
        }
    }
    //延迟初始化Game
    IEnumerator TInitAS()
    {
        yield return new WaitForSeconds(1f);
        setGameData(1, true);
    }
    IEnumerator TPlayEffct()
    {
        inpass = true;
        mSound.PlayShort("learnupmiddledown_sound", "stareffect");
        mSound.PlayOnly("learnupmiddledown_sound", "game-tips2-6-1-14");
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;
        for (float i = 0; i < 1f; i += 0.04f)
        {
            effect.rectTransform.localScale = Vector3.Lerp(startScale, endScale, Mathf.Sin((Mathf.PI / 4) * i * 2));
            yield return new WaitForSeconds(0.01f);
            effect1.rectTransform.localScale = Vector3.Lerp(startScale, endScale, Mathf.Sin((Mathf.PI / 4) * i * 2));
            yield return new WaitForSeconds(0.01f);
            effect2.rectTransform.localScale = Vector3.Lerp(startScale, endScale, Mathf.Sin((Mathf.PI / 4) * i * 2));
            yield return new WaitForSeconds(0.01f);
        }
        Color startColor = effect.color;
        Color endColor = effect.color;
        startColor.a = 1;
        endColor.a = 0;
        for (float i = 0; i < 1f; i += 0.05f)
        {
            effect.color = Color.Lerp(startColor, endColor, Mathf.Sin((Mathf.PI / 4) * i * 2));
            yield return new WaitForSeconds(0.01f);
            effect1.color = Color.Lerp(startColor, endColor, Mathf.Sin((Mathf.PI / 4) * i * 2));
            yield return new WaitForSeconds(0.01f);
            effect2.color = Color.Lerp(startColor, endColor, Mathf.Sin((Mathf.PI / 4) * i * 2));
            yield return new WaitForSeconds(0.01f);
        }
        effect.color = endColor;
        effect.rectTransform.localScale = startScale;
        effect.color = startColor;

        effect1.color = endColor;
        effect1.rectTransform.localScale = startScale;
        effect1.color = startColor;

        effect2.color = endColor;
        effect2.rectTransform.localScale = startScale;
        effect2.color = startColor;

        yield return new WaitForSeconds(0.5f);

        mGuanka.guanka++;
        if (mGuanka.guanka > mGuanka.guanka_last)
        {
            TopTitleCtl.instance.AddStar();
            //yield return new WaitForSeconds(5f);
            GameOverCtl.GetInstance().Show(mGuanka.guanka_last, reGame);
        }else
        {
            setGameData(mGuanka.guanka,false);
        }
    }
    private string getTypeSoundName(int type)
    {
        string buttonType = null;
        if(mGuanka.guanka == 1)
        {
            buttonType = "game-tips2-6-1-" + (6 + type);
        }else
        {
            buttonType = "game-tips2-6-1-" + (10 + type);
        }
        return buttonType;
    }
    private string getSoundName(string str)
    {
        string outstr = null;
        switch (str)
        {
            case "bingbangqiu":
                outstr = "乒乓球拍";
                break;
            case "gangbi":
                outstr = "钢笔";
                break;
            case "kuzi":
                outstr = "裤子";
                break;
            case "maozi":
                outstr = "帽子";
                break;
            case "zuqiu":
                outstr = "皮球";
                break;
            case "qianbi":
                outstr = "铅笔";
                break;
            case "shoutao":
                outstr = "手套";
                break;
            case "shu":
                outstr = "书";
                break;
            case "xiangbica":
                outstr = "橡皮擦";
                break;
            case "xiezi":
                outstr = "鞋子";
                break;
            case "yifu":
                outstr = "衣服";
                break;
            case "yumaoqiu":
                outstr = "羽毛球";
                break;
            case "zhentou":
                outstr = "枕头";
                break;
        }
        return outstr;
    }
}
