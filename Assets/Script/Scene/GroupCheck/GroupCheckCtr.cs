using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GroupCheckCtr : MonoBehaviour {
    public static GroupCheckCtr instance = null;
    public SoundManager mSound { get; set; }
    private GameObject colorGroup { get; set; }
    private List<Image> colorboxs = new List<Image>();
    private List<Items> items = new List<Items>();
    private List<Image> buttons = new List<Image>();
    private GameObject penGo { get; set; }
    //private Image mPrint { get; set; }
    private Image pen { get; set; }
    private Image subimt { get; set; }
    public Guanka mGuanka = new Guanka();
    private bool isPass { get; set; }
    private GameObject tuanGo { get; set; }
    private bool ischangeColor { get; set; }
    private ParticleSystem mOKBtn_Effect { get; set; }
    private bool isinited { get; set; }
    private Image currentTuAn { get; set; }
    private int currScore { get; set; }
    //关卡
    public class Guanka
    {
        public int guanka { get; set; }
        public int guanka_last { get; set; }
        public int currColor { get; set; }
        public int currTuAn { get; set; }
        public List<Color> ColorList { get; set; }
        public Color itemdeColor = new Color(222f / 256, 222f / 256, 222f / 256);
        public Vector3 penPos { get; set; }
        public List<ItemGrid> grids = new List<ItemGrid>();
        public int gridId { get; set; }
        public string childSprite { get; set; }
        public int okNum { get; set; }
       
        public Guanka()
        {
            guanka_last = 2;
            ColorList = getColorList();
        }
        public void Set(int _guanka)
        {
            guanka = _guanka;
            gridId = 0;
            currTuAn = -2;
            switch (_guanka)
            {
                case 1:
                    okNum = 6;
                    
                    break;
                case 2:
                    okNum = 0;//12
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }

        private List<Color> getColorList()
        {
            List<Color>  arr = new List<Color>();
            arr.Add(new Color(253f / 256, 53f / 256, 1f / 256));
            arr.Add(new Color(2f / 256, 233f / 256, 225f / 256));
            arr.Add(new Color(255f / 256, 255f / 256, 1f / 256));
            arr.Add(new Color(0, 185f / 256, 239f / 256));
            arr.Add(new Color(242f / 256, 158f / 256, 194f / 256));
            arr.Add(new Color(179f / 256, 87f / 256, 0));
            arr.Add(new Color(255f / 256, 171f / 256, 0));
            arr.Add(new Color(128f / 256, 128f / 256, 128f / 256));
            arr.Add(new Color(166f / 256, 89f / 256, 201f / 256));
            arr.Add(new Color(0, 0, 0));
            arr.Add(new Color(63f / 256, 203f / 256, 0));
            arr.Add(new Color(256f / 256, 256f / 256, 256f / 256));
            return arr;
        }
    }
    //所拼的图组
    public class ItemGrid
    {
        public static ItemGrid gSelect { get; set; }
        public List<Items> list { get; set; }
        public int mcolor { get; set; }
        public int mtuan { get; set; }
        public int mgridID { get; set; }
        public string rule { get; set; }
        public ItemGrid(int color,int id,int _tuan)
        {
            mgridID = id;
            mcolor = color;
            mtuan = _tuan;
            list = new List<Items>();
        }
        public void Add(Items item)
        {
            list.Add(item);
        }
        public void setResult(List<Items> _list)
        {
            if(list.Count <= 0)
            {
                rule = "";
                return;
            }
            Items startItem = null;
            for(int i = 0;i < _list.Count; i++)
            {
                Items item = _list[i];
                for (int j= 0;j< list.Count; j++)
                {
                    Items gridItem = list[j];
                    if (gridItem.mid == item.mid)
                    {
                        if (null == startItem)
                        {
                            startItem = list[j];
                        }
                        rule += (gridItem.mh - startItem.mh) + (gridItem.mv - startItem.mv).ToString();
                    }
                }
            }
            //Debug.Log("grid id : " + mgridID + ";rule : " + rule);
        }

    }

    void Awake()
    {
        mSound = gameObject.AddComponent<SoundManager>();
        instance = this;
    }
    // Use this for initialization
    void Start () {
        RawImage bg = UguiMaker.newRawImage("bg", transform, "groupcheck_texture", "bg", false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        StartCoroutine(TInit());
    }
    IEnumerator TInit()
    {
        yield return new WaitForSeconds(0.5f);
        Image itembg = UguiMaker.newImage("itembg", transform, "groupcheck_sprite", "sb_kuang_03");
        itembg.rectTransform.localPosition = new Vector3(0, -33, 0);
        reGame();
    }
    // Update is called once per frame
    float times = 0;
	void Update () {

        if (null != tuanGo && tuanGo.active)
        {
            if(null != currentTuAn)
            {
                currentTuAn.rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(times) * 10);
                times += 0.1f;

            }
        }
        if (isPass || !isinited) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            Items.gSelect = null;
            foreach (RaycastHit hit in hits)
            {
                GameObject com = hit.collider.gameObject;
                string[] strs = com.name.Split('_');
                if(strs[0] == "colorbox")
                {
                    int index = int.Parse(strs[1]);
                    if (index != mGuanka.currColor)
                    {
                        if (ischangeColor)
                        {
                            
                            if (index == 13)
                            {
                                mSound.PlayTip("groupcheck_sound", "click_clean", 1);
                            }else
                            {
                                mSound.PlayTip("groupcheck_sound", "click_color", 1);
                            }
                            setcolorBoxAlpha(index);
                            //ItemGrid.gSelect = null;切换颜色不换组
                            break;
                        }
                        /*利用来判断不能换颜色的情况下，可以点击删除按钮
                        else if(index == 13)
                        {
                            
                            setcolorBoxAlpha(index);
                            ischangeColor = true;
                            ItemGrid.gSelect = null;
                            break;
                        }
                        */
                        
                    }
                }
                if (strs[0] == "tuan" && ischangeColor)
                {
                    int index = int.Parse(strs[1]);
                    if (index != mGuanka.currTuAn)
                    {
                        mSound.PlayTip("groupcheck_sound", "click_color", 1);
                        setButtonAlpha(index);
                        //ItemGrid.gSelect = null;//切换图案换组设置
                        break;
                    }
                }
                if (com.name == "subimt")
                {
                    mSound.PlayShort("groupcheck_sound", "submit_down", 1);
                    subimt.sprite = ResManager.GetSprite("groupcheck_sprite", "sb_anniu2_33");
                    break;
                }

                Items item = hit.collider.gameObject.GetComponent<Items>();
                Items.gSelect = item;
                if(mGuanka.currColor < 13)
                {
                    if (strs[0] == "item" && !item.mstate)
                    {
                        setItemColor(com, item);
                        mSound.PlayShort("groupcheck_sound", "click_item", 1);
                    }
                    if (strs[0] == "item" && checkRule())//
                    {
                        item.setColor(mGuanka.currColor);
                        Items.gLSelect = item;
                        if (null == ItemGrid.gSelect)
                        {
                            ItemGrid grid = new ItemGrid(mGuanka.currColor, mGuanka.gridId, mGuanka.currTuAn);
                            ItemGrid.gSelect = grid;
                            mGuanka.grids.Add(grid);
                            mGuanka.gridId++;
                            ischangeColor = true;//满足是否可以切换图案或者颜色的条件
                        }
                        ItemGrid.gSelect.Add(item);
                        if (ItemGrid.gSelect.list.Count >= 4)
                        {
                            ItemGrid.gSelect.setResult(items);
                            ItemGrid.gSelect = null;
                            ischangeColor = true;
                        }
                        //mSound.PlayTip("groupcheck_sound", "click_item", 1);
                    }
                }else
                {
                    //TODO 清理某一组的图案
                    if(null != item)
                    {
                        if(item.mcolor != -1 && !item.isDemo)
                        {
                            mSound.PlayTip("groupcheck_sound", "click_clean", 1);
                            ItemGrid grid = null;
                            int startindex = 0;
                            if (mGuanka.guanka == 1) startindex = 1;

                            for (int i = startindex; i < mGuanka.grids.Count; i++)
                            {
                                List<Items> list = mGuanka.grids[i].list;
                                bool boo = false;
                                for (int j = 0; j < list.Count; j++)
                                {
                                    Items griditem = list[j];
                                    if (griditem.mid == item.mid)
                                    {
                                        grid = mGuanka.grids[i];
                                        boo = true;
                                        break;
                                    }
                                }
                                if (boo) break;
                            }
                            if (null != grid)
                            {
                                int leng = grid.list.Count;
                                Items agriditem = null;
                                for (int j = 0; j < leng; j++)
                                {
                                    agriditem = grid.list[j];
                                    agriditem.setDeColor();
                                }
                                if (ItemGrid.gSelect == grid)
                                {
                                    ItemGrid.gSelect = null;
                                }
                                mGuanka.grids.Remove(grid);
                            }
                            else
                            {
                                item.setDeColor();
                            }
                        }else
                        {
                            if (!item.isDemo)
                            {
                                string tipsname = "tips_color";
                                if (mGuanka.guanka == 2)
                                {
                                    //tipsname = "game-wrong4-2-2";
                                }
                                mSound.PlayTip("groupcheck_sound", tipsname, 1);
                            }
                        }
                        
                        //单个删除
                        /*
                       bool isallcleaned = true;
                       for (int j = 0; j < leng; j++)
                       {
                           Items cgriditem = grid.list[j];
                           if (cgriditem.mcolor != -1)
                           {
                               isallcleaned = false;
                               break;
                           }
                       }
                       if (isallcleaned)
                       {
                           mGuanka.grids.Remove(grid);
                       }

                       if (grid.list.Count <= 0)
                       {
                           mGuanka.grids.Remove(grid);
                       }
                       */
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                GameObject com = hit.collider.gameObject;
                if (com.name == "subimt")
                {
                    //TODO 检测完成多少组图形
                    int oknum = 0;
                    int startindex = 0;
                    if (mGuanka.guanka == 1) startindex = 1;
                    List<string> rules = new List<string>();
                    int index = 0;
                    //Debug.Log("制作多少组：" + mGuanka.grids.Count);
                    for (int i = startindex; i < mGuanka.grids.Count; i++)
                    {
                        List <Items> list = mGuanka.grids[i].list;
                        string rule = mGuanka.grids[i].rule;
                        if (list.Count == 4)
                        {
                            oknum++;
                            Items firstitem = mGuanka.grids[i].list[0];

                            if (!rules.Contains(rule))//是否已经存在该图形
                            {
                                rules.Add(mGuanka.grids[i].rule);
                                firstitem.showNum(rules.Count); 
                            }
                            else
                            {
                                firstitem.showNum(rules.IndexOf(rule) + 1);
                            }
                        }
                        index++;
                    }
                    //Debug.Log("完成多少组：" + rules.Count);
                    if(rules.Count >= mGuanka.okNum)
                    {
                        if(null == mOKBtn_Effect)
                        {
                            mOKBtn_Effect = ResManager.GetPrefab("effect_star0", "effect_star1").GetComponent<ParticleSystem>();
                            UguiMaker.InitGameObj(mOKBtn_Effect.gameObject, subimt.transform, "okbtn_effect", Vector3.zero, Vector3.one);
                        }
                        mSound.PlayShort("groupcheck_sound", "04-星星-02");
                        mOKBtn_Effect.Play();
                        if(mGuanka.guanka == 2)
                        {
                            currScore = rules.Count;
                            if (currScore > markedNum)
                            {
                                List<string> abList = new List<string>() { "groupcheck_sound", "groupcheck_sound", "groupcheck_sound", "groupcheck_sound", "groupcheck_sound", "groupcheck_sound", "groupcheck_sound", "groupcheck_sound", "groupcheck_sound" };
                                List<string> snList = new List<string>() { "additional-116（看谁拼得多）", currScore.ToString(), "additional-117（看谁拼得多）", "additional-118（看谁拼得多）", "additional-119（看谁拼得多）" ,(currScore - markedNum).ToString(), "danyuan_ge", "additional-121（看谁拼得多）" };
                                mSound.PlayTipList(abList, snList);// "groupcheck_sound", "game-wrong4-2-5", 1);
                                setMaxNum(currScore);
                                saveMaxNum();
                                nextGame(8);

                            }
                            else if(currScore < markedNum)
                            {
                                List<string> abList = new List<string>() { "groupcheck_sound", "groupcheck_sound", "groupcheck_sound", "groupcheck_sound", "groupcheck_sound", "groupcheck_sound", "groupcheck_sound", "groupcheck_sound", "groupcheck_sound" };
                                List<string> snList = new List<string>() { "additional-116（看谁拼得多）", currScore.ToString(), "additional-117（看谁拼得多）", "additional-118（看谁拼得多）", "additional-120（看谁拼得多）", (markedNum - currScore).ToString(), "danyuan_ge", "additional-122（看谁拼得多）"};
                                mSound.PlayTipList(abList, snList);
                                nextGame(8);
                            }
                            else
                            {
                                nextGame();
                            }
                        }else
                        {
                            nextGame();
                        }
                        
                    }else
                    {
                        if(mGuanka.guanka == 1)
                        {
                            mSound.PlayTip("groupcheck_sound", "game-wrong4-2-5-6", 1);//"game-wrong4-2-5"
                        }
                        else
                        {
                            mSound.PlayTip("groupcheck_sound", "game-wrong4-2-6", 1);
                        }
                        Invoke("InvokeOnClkBtnOK", 1.5f);
                    }
                    break;
                }
            }
        }

    }
    private void setItemColor(GameObject com,Items item)
    {
        if (mGuanka.guanka == 1)
        {
            if (mGuanka.currColor <= 12)
            {
                if (mGuanka.currColor < 12)
                {
                    Image image = com.GetComponent<Image>();
                    image.color = mGuanka.ColorList[mGuanka.currColor];
                }
                else
                {
                    item.addChild("sb_cai_5", new Color(1, 1, 1));
                }
            }
        }
        else
        {
            if (mGuanka.currColor < 12)
            {
                item.addChild(mGuanka.childSprite, mGuanka.ColorList[mGuanka.currColor], 2);
            }
            else
            {
                item.addChild("sb_cai_5", new Color(1, 1, 1), 1);
            }
        }
    }
    private void InvokeOnClkBtnOK()
    {
        mSound.PlayShort("groupcheck_sound", "submit_up", 1);
        subimt.sprite = ResManager.GetSprite("groupcheck_sprite", "sb_anniu1_33");
    }
    //规则检测
    private bool checkRule()
    {
        bool state = true;
        if (Items.gSelect.mstate)
        {
            //已经被选中了
            return false;
        }else
        {
            if(null != ItemGrid.gSelect)
            {
                if(ItemGrid.gSelect.list.Count >= 4){
                    //数据够4个了
                    return false;
                }else
                {
                    bool boo = false;
                    for(int i = 0;i < ItemGrid.gSelect.list.Count; i++)
                    {
                        Items item = ItemGrid.gSelect.list[i];
                        if (Items.gSelect.mh == item.mh && (Items.gSelect.mv == (item.mv + 1) || Items.gSelect.mv == (item.mv - 1)))
                        {
                            boo = true;
                            break;
                        }
                        if (Items.gSelect.mv == item.mv && (Items.gSelect.mh == (item.mh + 1) || Items.gSelect.mh == (item.mh - 1)))
                        {
                            boo = true;
                            break;
                        }
                        //对角线
                        if((Items.gSelect.mh == (item.mh + 1) && Items.gSelect.mv == (item.mv + 1)))
                        {
                            boo = true;
                            break;
                        }
                        if ((Items.gSelect.mh == (item.mh - 1) && Items.gSelect.mv == (item.mv - 1)))
                        {
                            boo = true;
                            break;
                        }
                        //反对角线
                        if ((Items.gSelect.mh == (item.mh + 1) && Items.gSelect.mv == (item.mv - 1)))
                        {
                            boo = true;
                            break;
                        }
                        if ((Items.gSelect.mh == (item.mh - 1) && Items.gSelect.mv == (item.mv + 1)))
                        {
                            boo = true;
                            break;
                        }
                    }
                    if (!boo)
                    {
                        Items.gSelect.playError();
                        mSound.PlayTip("groupcheck_sound", "game-wrong4-2-1", 1);
                        for (int i = 0; i < ItemGrid.gSelect.list.Count; i++)
                        {
                            Items item = ItemGrid.gSelect.list[i];
                            item.playError(false);
                        }
                            //与改组元素不相邻
                            return false;
                    }
                    boo = false;
                    for (int i = 0; i < mGuanka.grids.Count; i++)
                    {
                        bool sstate = false;
                        ItemGrid grid = mGuanka.grids[i];
                        if (grid.mgridID != ItemGrid.gSelect.mgridID)
                        {
                            for (int j = 0; j < grid.list.Count; j++)
                            {
                                Items item = grid.list[j];
                                if (Mathf.Abs(item.mh - Items.gSelect.mh) <= 1 && Mathf.Abs(item.mv - Items.gSelect.mv) <= 1)
                                {
                                    //只有第二关才会做图形判断
                                    bool isTux = true;
                                    if(mGuanka.guanka == 2)
                                    {
                                        if(mGuanka.currTuAn == item.mPhoto)
                                        {
                                            isTux = true;
                                        }else
                                        {
                                            //图形不同的条件，false表示颜色相同，图形不同则通过，true不判断图形，只判断颜色
                                            isTux = false;// false;
                                        }
                                    }
                                    if (item.mcolor != -1)//&& isTux 控制相邻的颜色是否一样 或者是已经图上颜色或者图案  mGuanka.currColor == item.mcolor || (用于判断颜色不是是同一种）
                                    {
                                        sstate = true;
                                        break;
                                    }
                                }
                            }
                            if (sstate)
                            {
                                boo = true;
                                break;
                            }
                        }
                    }
                    if (boo)
                    {
                        //与相邻组颜色相同
                        Items.gSelect.playError();
                        mSound.PlayTip("groupcheck_sound", "tips_jiange", 1);
                        return false;
                    }
                }
            }else
            {
                bool boo = true;
                for(int i = 0;i < items.Count; i++)
                {
                    Items item = items[i];
                    if(Mathf.Abs(item.mh - Items.gSelect.mh) <= 1 && Mathf.Abs(item.mv - Items.gSelect.mv) <= 1)
                    {
                        bool isTux = true;
                        if (mGuanka.guanka == 2)
                        {
                            if (mGuanka.currTuAn == item.mPhoto)
                            {
                                isTux = true;
                            }
                            else
                            {
                                isTux = false;
                            }

                        }
                        if (item.mcolor != -1)//&& isTux 不限制图形 控制相邻的颜色是否一样 或者是已经图上颜色或者图案 mGuanka.currColor == item.mcolor ||
                        {
                            boo = false;
                            break;
                        }
                    }
                }
                if (!boo)
                {
                    //相邻组元素颜色有相同
                    Items.gSelect.playError();
                    mSound.PlayTip("groupcheck_sound", "tips_jiange", 1);
                    return false;
                }
            }
        }
        return state;
    }
    //重置游戏
    private void reGame()
    {
        mGuanka.guanka = 1;
        setGameData();
    }
    private void nextGame(float delay = 8)
    {
        if(mGuanka.guanka == 1)
        {
            mSound.PlayTip("groupcheck_sound", "game-right4-2-3", 1);
        }
        isPass = true;
        StartCoroutine(TnextGame(delay));
    }
    //设置游戏数据
    private bool isPlayBged { get; set; }
    private void setGameData()
    {
        mGuanka.childSprite = "";
        if (mGuanka.guanka == 1)
        {
            isinited = false;
            TopTitleCtl.instance.Reset();
        }else
        {
            TopTitleCtl.instance.AddStar();
        }
        isPass = false;
        mGuanka.Set(mGuanka.guanka);
        mGuanka.grids.Clear();
        if(null != ItemGrid.gSelect)
        {
            ItemGrid.gSelect.list.Clear();
        }
        
        ischangeColor = true;
        if (null == colorGroup)
        {
            colorGroup = UguiMaker.newGameObject("colorGroup", transform);
            Vector3 startPos = new Vector3(-510, 250, 0);
            for (int i = 0; i < 12; i++)
            {
                Image colorbox = UguiMaker.newImage("colorbox_" + i, colorGroup.transform, "groupcheck_sprite", "sb_white_06");
                colorbox.rectTransform.localPosition = startPos + new Vector3(i % 2 * 80, -((int)i / 2) * 80, 0);
                colorbox.color = mGuanka.ColorList[i];
                colorboxs.Add(colorbox);
                BoxCollider box = colorbox.gameObject.AddComponent<BoxCollider>();
                box.size = new Vector3(70, 70, 0);
            }
            Image colorboxtu = UguiMaker.newImage("colorbox_12", colorGroup.transform, "groupcheck_sprite", "sb_xp_34");
            colorboxtu.rectTransform.localPosition = new Vector3(-508, -241, 0);
            colorboxs.Add(colorboxtu);
            BoxCollider boxtu = colorboxtu.gameObject.AddComponent<BoxCollider>();
            boxtu.size = new Vector3(70, 70, 0);

            Image colorboxca = UguiMaker.newImage("colorbox_13", colorGroup.transform, "groupcheck_sprite", "sb_xp_31");
            colorboxca.rectTransform.localPosition = new Vector3(-426, -241, 0);
            colorboxs.Add(colorboxca);
            BoxCollider boxtuca = colorboxca.gameObject.AddComponent<BoxCollider>();
            boxtuca.size = new Vector3(60, 90, 0);
            
            GameObject itemsGo = UguiMaker.newGameObject("itemsGo", transform);
            Vector3 startitemPos = new Vector3(-332f, 145.14f, 0);
            for (int i = 0; i < 105; i++)
            {
                Image img = UguiMaker.newImage("item_" + i, itemsGo.transform, "public", "white");
                Items item = img.gameObject.AddComponent<Items>();
                img.rectTransform.sizeDelta = new Vector2(50, 50);
                int h = ((int)i / 15);
                int v = i % 15;
                img.rectTransform.localPosition = startitemPos + new Vector3(v * 57.8f, -h * 57.8f, 0);
                img.color = mGuanka.itemdeColor;
                BoxCollider box = img.gameObject.AddComponent<BoxCollider>();
                box.size = new Vector3(50, 50, 0);
                item.setData(i,h, v);
                items.Add(item);
            }
            penGo = UguiMaker.newGameObject("penGo", transform);
            pen = UguiMaker.newImage("pen", penGo.transform, "groupcheck_sprite", "sb_dian_06");

            Image subimtshader = UguiMaker.newImage("subimtshader", transform, "groupcheck_sprite", "submitshader");
            subimtshader.rectTransform.localPosition = new Vector3(367, -323, 0);

            subimt = UguiMaker.newImage("subimt", transform, "groupcheck_sprite", "sb_anniu1_33");
            subimt.rectTransform.localPosition = new Vector3(356, -301, 0);
            BoxCollider sbox = subimt.gameObject.AddComponent<BoxCollider>();
            sbox.size = new Vector3(85, 120, 0);
        }
        else
        {
            for (int i = 0; i < 105; i++)
            {
                Items item = items[i];
                item.setDeColor();
            }
        }
        pen.enabled = false;
        for (int i = 0;i < 14; i++)
        {
            Image color = colorboxs[i];
            color.enabled = false;
        }
        subimt.sprite = ResManager.GetSprite("groupcheck_sprite", "sb_anniu1_33");

        Image caise = colorGroup.transform.Find("colorbox_12").gameObject.GetComponent<Image>();
        caise.enabled = true;
        BoxCollider box12 = caise.gameObject.GetComponent<BoxCollider>();
        box12.enabled = true;

        Image clean = colorGroup.transform.Find("colorbox_13").gameObject.GetComponent<Image>();
        clean.rectTransform.localEulerAngles = Vector3.zero;
        clean.rectTransform.localPosition = new Vector3(-426, -241, 0);
        if (mGuanka.guanka == 2)
        {
            caise.enabled = false;
            box12.enabled = false;
            clean.rectTransform.localEulerAngles = new Vector3(0, 0, 90);
            clean.rectTransform.localPosition = new Vector3(-474, -241, 0);
            if (null == tuanGo)
            {
                Vector3 startPos = new Vector3(-246, -320, 0);
                tuanGo = UguiMaker.newGameObject("tuanGo", transform);
                for(int i = 0;i < 5; i++)
                {
                    Image shadow = UguiMaker.newImage("shadow_" + i, tuanGo.transform, "groupcheck_sprite", "qwfz_shadow_03");
                    shadow.rectTransform.sizeDelta = new Vector2(50, 22);
                    Color black = Color.black;
                    black.a = 0.4f;
                    shadow.color = black;
                    shadow.rectTransform.localPosition = startPos + new Vector3(i * 90, -45, 0);
                    Image tuan = UguiMaker.newImage("tuan_" + i, tuanGo.transform, "groupcheck_sprite", "sb_yz_" + i);
                    tuan.rectTransform.localPosition = startPos + new Vector3(i * 90, 0, 0);
                    BoxCollider sbox = tuan.gameObject.AddComponent<BoxCollider>();
                    sbox.size = new Vector3(60, 80, 0);
                    tuan.transform.localScale = Vector3.zero;
                    buttons.Add(tuan);
                }
            }
            for (int i = 0; i < 5; i++)
            {
                buttons[i].transform.localScale = Vector3.zero;
                Transform tuan = tuanGo.transform.Find("shadow_" + i);
                tuan.gameObject.SetActive(false);
            }
            tuanGo.SetActive(true);
            mGuanka.childSprite = "sb_yin_0";
            StartCoroutine(TshowTuAn(0));
            //setButtonAlpha(0);
        }
        else
        {
            if (null != tuanGo)
            {
                tuanGo.SetActive(false);
            }
        }
        
        if(mGuanka.guanka == 1)
        {
            //setDemo();
        }
        for (int i = 0; i < 14; i++)
        {
            Image color = colorboxs[i];
            if (mGuanka.guanka == 1)
            {
                color.enabled = true;
            }
            else{
                if(i != 12)
                {
                    color.enabled = true;
                }
                else
                {
                    color.enabled = false;
                }
            }
            
            color.transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, 0.01f);
            color.transform.localScale = Vector3.one;
        }
        setcolorBoxAlpha(0);
        StartCoroutine(TplayHowToPlay());

        if (!isPlayBged)
        {
            mSound.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.1f);
            isPlayBged = true;
        }

        if (mGuanka.guanka == 2)
        {
            markedNum = PlayerPrefs.GetInt("score");
            setMaxNum(markedNum);
        }else
        {
            closeMarkMax();
        }

    }
    private int markedNum { get; set; }
    //设置最佳记录
    private GameObject Numgo { get; set; }
    private Image maxScorebg { get; set; }
    private void setMaxNum(int num)
    {
        if(null == Numgo)
        {
            maxScorebg = UguiMaker.newImage("maxScorebg", transform, "groupcheck_sprite", "maxScorebg");
            maxScorebg.rectTransform.localPosition = new Vector3(510, 275, 0);
            Numgo = UguiMaker.newGameObject("Numgo", transform);
            Numgo.transform.localPosition = new Vector3(510, 275, 0);
        }else
        {
            showMarkMax();
            for (int i = 0;i < Numgo.transform.childCount; i++)
            {
                GameObject go = Numgo.transform.GetChild(i).gameObject;
                GameObject.Destroy(go);
            }
        }
        int leng = (int)(num / 10) + 1;
        Vector3 startPos = Vector3.zero;
        List<int> list = new List<int>();
        if(leng > 1)
        {
            startPos = new Vector3(10, 10, 0);
            list.Add((int)(num / 10));
            list.Add((int)(num % 10));
        }else
        {
            startPos = new Vector3(24, 10, 0);
            list.Add((int)(num % 10));
        }
        for (int i = 0;i < leng; i++)
        {
            Image numImage = UguiMaker.newImage("numImage", Numgo.transform, "public", "default" + list[i].ToString());
            numImage.rectTransform.sizeDelta = new Vector2(20, 50);
            numImage.color = new Color(253f / 255, 102f / 255, 1f / 255);
            numImage.transform.localPosition = startPos + new Vector3(i * 20, 0, 0);
        }
        mSound.PlayShort("groupcheck_sound", "奖章解冻的特效声", 0.6f);
        PlayStarParticle(Numgo.transform.localPosition + new Vector3(24, 16, 0));
    }
    private ParticleSystem mStarparticle { get; set; }
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
    private void showMarkMax()
    {
        if (null != Numgo)
        {
            Numgo.active = true;
            maxScorebg.enabled = true;
        }
    }
    private void closeMarkMax()
    {
        if(null != Numgo)
        {
            Numgo.active = false;
            maxScorebg.enabled = false;
        }
    }
    private void saveMaxNum()
    {
        PlayerPrefs.SetInt("score", currScore);
    }
    IEnumerator TsetDemo()
    {
        List<int> list = Common.BreakRank(new List<int>() { 0, 1, 2, 15, 16});
        for (int i = 0; i < 4; i++)
        {
            items[list[i]].gameObject.GetComponent<Image>().color = mGuanka.ColorList[mGuanka.currColor];
            items[list[i]].setColor(mGuanka.currColor,true);
            Items.gLSelect = items[list[i]];
            if (null == ItemGrid.gSelect)
            {
                ItemGrid grid = new ItemGrid(mGuanka.currColor, mGuanka.gridId, mGuanka.currTuAn);
                ItemGrid.gSelect = grid;
                mGuanka.grids.Add(grid);
                mGuanka.gridId++;
                ischangeColor = true;//满足是否可以切换图案或者颜色的条件
            }
            ItemGrid.gSelect.Add(items[list[i]]);
            if (ItemGrid.gSelect.list.Count >= 4)
            {
                ItemGrid.gSelect.setResult(items);
                ItemGrid.gSelect = null;
                ischangeColor = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
        isinited = true;
    }
    private void setButtonAlpha(int index)
    {
        mGuanka.currTuAn = index;
        for (int i = 0; i < buttons.Count; i++)
        {
            Color clor = buttons[i].color;
            if (i == index)
            {
                clor.a = 1;
                buttons[i].rectTransform.localScale = Vector3.one * 1.3f;
                mGuanka.childSprite = "sb_yin_" + index;
                currentTuAn = buttons[i];
            }
            else
            {
                clor.a = 0.6f;
                buttons[i].rectTransform.localScale = Vector3.one;
                buttons[i].rectTransform.localEulerAngles = new Vector3(0, 0, 0);
            }
            buttons[i].color = clor;
        }
    }
    //设置单个颜色图片的透明度
    private void setcolorBoxAlpha(int index)
    {
        mGuanka.currColor = index;
        pen.enabled = true;
        for (int i = 0; i < colorboxs.Count; i++)
        {
            Color clor = colorboxs[i].color;
            if (i == index)
            {
                clor.a = 1;
                colorboxs[i].rectTransform.localScale = Vector3.one * 1.3f;
                if (index == 11)
                {
                    pen.sprite = ResManager.GetSprite("groupcheck_sprite", "sb_dian_08");
                }else
                {
                    pen.sprite = ResManager.GetSprite("groupcheck_sprite", "sb_dian_06");
                }
                if (index >= 12)
                {
                    if (index == 12)
                    {
                        penGo.SetActive(true);
                        if (mGuanka.guanka == 1)
                        {
                            mGuanka.childSprite = "sb_cai_5";
                            //mPrint.color = new Color(1, 1, 1, 1);
                            //mPrint.sprite = ResManager.GetSprite("groupcheck_sprite", "sb_pen2_12");
                            //pen.rectTransform.localPosition = colorboxs[i].rectTransform.localPosition;
                            //mGuanka.penPos = pen.rectTransform.localPosition;
                        }
                    }else
                    {
                        penGo.SetActive(false);
                    }
                    pen.rectTransform.localPosition = colorboxs[i].rectTransform.localPosition;
                    mGuanka.penPos = pen.rectTransform.localPosition;
                }
                else
                {
                    penGo.SetActive(true);
                    //设置笔的颜色
                    //mPrint.color = mGuanka.ColorList[mGuanka.currColor];
                    //mPrint.sprite = ResManager.GetSprite("groupcheck_sprite", "sb_pen1_12");
                    pen.rectTransform.localPosition = colorboxs[i].rectTransform.localPosition;
                    mGuanka.penPos = pen.rectTransform.localPosition;
                }
            }
            else
            {
                clor.a = 0.6f;
                colorboxs[i].rectTransform.localScale = Vector3.one;
            }
            colorboxs[i].color = clor;
        }
    }
    IEnumerator TplayHowToPlay()
    {
        float time = 0.01f;
        string soundname = "additional-115（看谁拼得多）";// "game-tips4-2-2";
        if (mGuanka.guanka == 1)
        {
            soundname = "game-tips4-2-1-6";// "game-tips4-2-1";
            time = 2f;
        }
        yield return new WaitForSeconds(time);
        mSound.PlayTip("groupcheck_sound", soundname, 1, true);
        if (mGuanka.guanka == 1)
        {
            StartCoroutine(TsetDemo());
        }else
        {
            isinited = true;
        }
    }
    IEnumerator TshowTuAn(int index)
    {
        for(int i = 0;i < buttons.Count; i++)
        {
            mSound.PlayShort("groupcheck_sound", "submit_up", 0.6f);
            Transform tuan = tuanGo.transform.Find("shadow_" + i);
            tuan.gameObject.SetActive(true);
            for (float j = 0; j < 1f; j += 0.1f)
            {
                buttons[i].transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, j);
                
                yield return new WaitForSeconds(0.01f);
            }
            buttons[i].transform.localScale = Vector3.one;
        }
        setButtonAlpha(index);
    }

    IEnumerator TnextGame(float delay)
    {
        yield return new WaitForSeconds(delay);

        mGuanka.guanka++;
        if(mGuanka.guanka > mGuanka.guanka_last)
        {
            TopTitleCtl.instance.AddStar();
            yield return new WaitForSeconds(1.5f);
            GameOverCtl.GetInstance().Show(mGuanka.guanka_last, reGame);

        }
        else
        {
            setGameData();
        } 
    }
}
