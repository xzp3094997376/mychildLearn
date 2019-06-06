using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OneAndMoreCtr : MonoBehaviour {
    public Guanka mGuanka = new Guanka();
    public SoundManager mSound { get; set; }
    private RawImage bg { get; set; }
    private bool inpass { get; set; }
    private bool isInPraint { get; set; }
    public class Guanka
    {
        public int guanka { get; set; }
        public int guanka_last { get; set; }
        public List<GameObject> items = new List<GameObject>();
        public int childNum { get; set; }
        public Guanka()
        {
            guanka_last = 2;
        }
        public void Set(int _guanka)
        {
            guanka = _guanka;
            
            for(int i = 0;i < items.Count; i++)
            {
                GameObject.Destroy(items[i]);
            }
            items.Clear();

            childNum = Common.GetRandValue(5, 7);
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
    }
    void Awake()
    {
        mSound = gameObject.AddComponent<SoundManager>();
    }
    // Use this for initialization
    void Start () {
        bg = UguiMaker.newRawImage("bg", transform, "oneandmore_texture", "bg_1", false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        StartCoroutine(TInit());
    }
    IEnumerator TInit()
    {
        yield return new WaitForSeconds(0.5f);
        mSound.PlayBgAsync("bgmusic_loop3", "bgmusic_loop3", 0.1f);
        reGame();
    }
    // Update is called once per frame
    Vector3 temp_select_offset = new Vector3(0, 0, 0);
    GameObject currPen { get; set; }
    private Vector3 defPenPos = new Vector3(552, 264, 0);
    void Update()
    {
        if (inpass || isInPraint) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (null != hits)
            {
                OneAndMoreDuck.gSlect = null;
                currPen = null;
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit2D hit = hits[i];
                    GameObject conGo = hit.collider.gameObject;
                    Debug.Log("conGo : " + conGo.name);
                    OneAndMoreDuck com = conGo.GetComponent<OneAndMoreDuck>();
                    if(null != com)
                    {
                        OneAndMoreDuck.gSlect = com;
                        OneAndMoreDuck.gSlect.gameObject.transform.SetAsLastSibling();
                        temp_select_offset = com.gameObject.GetComponent<RectTransform>().anchoredPosition3D - Common.getMouseLocalPos(transform);
                        mSound.PlayShort("aa_animal_effect_sound", "鸭子yes");
                        com.Click();
                        break;
                    }else
                    {
                        OneAndMorePig pig = conGo.GetComponent<OneAndMorePig>();
                        if (null != pig && !pig.isPraint)
                        {
                            mSound.PlayShort("aa_animal_effect_sound", "猪yes"); 
                            pig.Click();
                        }
                    }
                    if(conGo.name == "pen")
                    {
                        currPen = conGo;
                        conGo.transform.SetAsLastSibling();
                        temp_select_offset = new Vector3(50,50,0);
                        mSound.PlayShort("button_down");
                        break;
                    }
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            if(null != OneAndMoreDuck.gSlect && !OneAndMoreDuck.gSlect.mIsSwiming)
            {
                OneAndMoreDuck.gSlect.gameObject.GetComponent<RectTransform>().anchoredPosition3D = Common.getMouseLocalPos(transform) + temp_select_offset;
            }else
            {
                if(null != currPen)
                {
                    currPen.GetComponent<RectTransform>().anchoredPosition3D = Common.getMouseLocalPos(transform) + temp_select_offset;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (null != OneAndMoreDuck.gSlect || null != currPen)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (null != hits)
                {
                    bool boo = false;
                    for (int i = 0; i < hits.Length; i++)
                    {
                        RaycastHit2D hit = hits[i];
                        GameObject com = hit.collider.gameObject;
                        if (null != com && com.name == "walterarrea" && !OneAndMoreDuck.gSlect.mIsSwiming)
                        {
                            OneAndMoreDuck.gSlect.swiming();
                            mSound.PlayShort("oneandmore_sound", "入水");
                            checkFinish();
                            boo = true;
                            break;
                        }else
                        {
                            OneAndMorePig pig = com.GetComponent<OneAndMorePig>();
                            if(null != pig)
                            {
                                if (pig.isPraint)
                                {
                                    mSound.PlayShort("aa_animal_effect_sound", "猪yes");
                                    isInPraint = true;
                                    boo = true;
                                    draw(pig);
                                    pig.Click();
                                }
                            }
                        }
                    }
                    if (!boo && null != OneAndMoreDuck.gSlect)
                    {
                        if (!OneAndMoreDuck.gSlect.mIsSwiming)
                        {
                            OneAndMoreDuck.gSlect.setDefPos();
                        }else
                        {
                            //OneAndMoreDuck.gSlect.Click();
                        }

                    }else
                    {
                        if(null != currPen)
                        {
                            mSound.PlayShort("button_up");
                            StartCoroutine(penBack());
                        }
                    }
                }
            }
            OneAndMoreDuck.gSlect = null;
        }
    }
    private void draw(OneAndMorePig pig)
    {
        pig.praint();
        StartCoroutine(TDraw(pig));
    }
    IEnumerator TDraw(OneAndMorePig pig)
    {
        pig.praint();
        Vector3 startPos = pig.transform.localPosition - new Vector3(40, 0, 0) + new Vector3(0,156,0);
        Vector3 endPos = pig.transform.localPosition + new Vector3(130, 0, 0) + new Vector3(0, 156, 0);
        /*
        for (float j = 0; j < 1f; j += 0.025f)
        {
            currPen.transform.localPosition = Vector3.Lerp(startPos, endPos, j);
            yield return new WaitForSeconds(0.01f);
        }
        */
        StartCoroutine(penBack());
        yield return new WaitForSeconds(0.5f);
        isInPraint = false;
        currPen = null;
        checkFinishPig();
    }
    IEnumerator penBack()
    {
        if (null != currPen) {
            Vector3 startPos = currPen.transform.localPosition;
            for (float j = 0; j < 1f; j += 0.2f)
            {
                currPen.transform.localPosition = Vector3.Lerp(startPos, defPenPos, j);
                yield return new WaitForSeconds(0.01f);
            }
            currPen.transform.localPosition = defPenPos;
        }
        
    }
    //检测猪
    private void checkFinishPig()
    {
        bool isFinish = true;
        for (int i = 0; i < mGuanka.items.Count; i++)
        {
            GameObject go = mGuanka.items[i];
            OneAndMorePig pig = go.GetComponent<OneAndMorePig>();
            if (pig.isPraint)//处在画画状态
            {
                isFinish = false;
                break;
            }
        }
        if (isFinish)
        {
            mSound.PlayTip("oneandmore_sound", "game-tips1-1-3-4", 1, true);
            StartCoroutine(TNextGame());
        }
    }
    //检测鸭子
    private void checkFinish()
    {
        bool isFinish = true;
        if(mGuanka.guanka == 1)
        {
            for (int i = 0; i < mGuanka.items.Count; i++)
            {
                GameObject go = mGuanka.items[i];
                OneAndMoreDuck duck = go.GetComponent<OneAndMoreDuck>();
                if (!duck.mIsSwiming)
                {
                    isFinish = false;
                    break;
                }
            }
        }

        if (isFinish)
        {
            mSound.PlayTip("oneandmore_sound", "game-tips1-1-3-2", 1, true);
            StartCoroutine(TNextGame());
        }
        
    }
    IEnumerator TNextGame()
    {
        inpass = true;
        yield return new WaitForSeconds(6);

        mGuanka.guanka++;
        if (mGuanka.guanka > mGuanka.guanka_last)
        {
            TopTitleCtl.instance.AddStar();
            yield return new WaitForSeconds(2f);
            GameOverCtl.GetInstance().Show(mGuanka.guanka_last, reGame);
        }
        else
        {
            Debug.Log("过关……");
            mGuanka.Set(mGuanka.guanka);
            setGameData();
        }
    }
    private void reGame()
    {
        mGuanka.Set(1);
        setGameData();
    }
    //岸边上小鸭子的坐标
    private List<Vector3> GetChildStartPoss()
    {
        List<Vector3> poss;
        if(mGuanka.guanka == 1)
        {
            poss =  new List<Vector3>() { new Vector3(-128, -363, 0), new Vector3(-334, -333, 0), new Vector3(-498, -262, 0), new Vector3(303, 172, 0), new Vector3(502, 22, 0), new Vector3(94, -352, 0), new Vector3(-528, 176, 0) };
            poss = Common.BreakRank(poss);
        }else
        {
            poss = new List<Vector3>() { new Vector3(-380, -211, 0), new Vector3(-395, -387, 0), new Vector3(-184, -317, 0), new Vector3(267, -226, 0), new Vector3(41, -365, 0), new Vector3(368, -383, 0), new Vector3(516, -249, 0) }; 
        }
        return poss;
    }
    //获取水池中的小鸭子结果坐标
    private List<Vector3> GetChildEndPoss()
    {
        List<Vector3> duckPoss = new List<Vector3>() { new Vector3(-390, 26, 0), new Vector3(49, 90, 0), new Vector3(-64, -53, 0), new Vector3(-348, -145, 0), new Vector3(-146, -198, 0), new Vector3(210, -75, 0), new Vector3(131, -202, 0) };
        if (mGuanka.guanka == 1)
        {
            duckPoss = Common.BreakRank(duckPoss);
        }
        return duckPoss;
    }

    //设置游戏数据
    private void setGameData()
    {
        inpass = false;
        if (null != duckGameGo)
        {
            duckGameGo.active = false;
        }
        if (null != pigGameGo)
        {
            pigGameGo.active = false;
            closePen();
        }
        if (mGuanka.guanka == 1)
        {
            bg.texture = ResManager.GetTexture("oneandmore_texture", "bg_1");
            bg.rectTransform.sizeDelta = new Vector2(1423, 800);
            TopTitleCtl.instance.Reset();
            CreateDuckGame();
        }
        else
        {
            bg.texture = ResManager.GetTexture("oneandmore_texture", "bg_2");
            bg.rectTransform.sizeDelta = new Vector2(1423, 800);
            TopTitleCtl.instance.AddStar();
            createPigGame();
        }
        
    }
    private GameObject duckGameGo { get; set; }
    GameObject duckMotherGo { get; set; }
    //创建鸭子游戏
    private void CreateDuckGame()
    {
        if(null == duckGameGo)
        {
            duckGameGo = UguiMaker.newGameObject("duckGameGo", transform);
            GameObject walterarrea = ResManager.GetPrefab("oneandmore_prefab", "walterarrea");
            walterarrea.name = "walterarrea";
            walterarrea.transform.parent = duckGameGo.transform;
            walterarrea.transform.localPosition = Vector3.zero;
            walterarrea.transform.localScale = Vector3.one;
            duckMotherGo = createDuck(duckGameGo.transform, new Vector3(-176, 46, 0), 1, true, "big");
        }else
        {
            duckGameGo.active = true;
        }
        
        List<Vector3> listPos = GetChildStartPoss();
        for (int i = 0; i < mGuanka.childNum; i++)
        {
            GameObject duckchildGo = createDuck(duckGameGo.transform, listPos[i], 1, false, "small");
            mGuanka.items.Add(duckchildGo);
        }
        mSound.PlayTip("oneandmore_sound", "game-tips1-1-3-1", 1,true);
    }
    private GameObject pigGameGo { get; set; }
    GameObject pigMotherGo { get; set; }
    //创建猪游戏
    private void createPigGame()
    {
        if (null == pigGameGo)
        {
            pigGameGo = UguiMaker.newGameObject("pigGameGo", transform);
            pigGameGo = createPig(transform, new Vector3(-15, -173, 0), 1, true, "big");
        }else
        {
            pigGameGo.active = true;
        }
            

        List<Vector3> listPos = GetChildStartPoss();
        for (int i = 0; i < mGuanka.childNum; i++)
        {
            GameObject pigchildGo = createPig(transform, listPos[i], 1, false, "small");
            mGuanka.items.Add(pigchildGo);
        }
        if(null == penColor)
        {
            penColor = UguiMaker.newImage("penColor", transform, "oneandmore_sprite", "pencolor");
            penColor.transform.localPosition = new Vector3(513, 121, 0);
            pen = UguiMaker.newImage("pen", transform, "oneandmore_sprite", "pen");
            pen.transform.localPosition = defPenPos;
            BoxCollider2D penbox = pen.gameObject.AddComponent<BoxCollider2D>();
            penbox.size = new Vector2(180, 150);
        }
        showPen();

        mSound.PlayTip("oneandmore_sound", "game-tips1-1-3-3", 1, true);
    }
    private Image penColor { get; set; }
    private Image pen { get; set; }
    private void closePen()
    {
        if(null != penColor)
        {
            penColor.enabled = false;
            pen.enabled = false;
        }
    }
    private void showPen()
    {
        if (null != penColor)
        {
            penColor.enabled = true;
            pen.enabled = true;
        }
    }
    //创建一个鸭子
    private GameObject createDuck(Transform _parent,Vector3 _pos, float _scaleNum,bool isInWalter,string _type)
    {
        GameObject go = UguiMaker.newGameObject("duckGo", _parent);
        OneAndMoreDuck duck = go.AddComponent<OneAndMoreDuck>();
        duck.setPos(_pos);
        string duckName = "duck";
        BoxCollider2D box = go.AddComponent<BoxCollider2D>();
        if (_type == "small")
        {
            duckName = "ducking";
            box.offset = new Vector2(0, 80);
            box.size = new Vector3(120, 120);
        }else
        {
            box.offset = new Vector2(0, 80);
            box.size = new Vector3(120, 120);
        }
        duck.createDuck(duckName,_scaleNum, isInWalter, _type);
        return go;
    }
    //创建一头猪
    private GameObject createPig(Transform _parent, Vector3 _pos, float _scaleNum, bool isPraint, string _type)
    {
        GameObject go = UguiMaker.newGameObject("pigGo", _parent);
        OneAndMorePig duck = go.AddComponent<OneAndMorePig>();
        duck.setPos(_pos);
        string duckName = "pig";
        BoxCollider2D box = go.AddComponent<BoxCollider2D>();
        box.offset = new Vector2(0, 80);
        box.size = new Vector3(190, 120);
        if (_type == "small")
        {
            duckName = "porket";
        }else
        {
            box.offset = new Vector2(0, 250);
            box.size = new Vector3(500, 300);
        }
        duck.createPig(duckName, _scaleNum, isPraint, _type);
        return go;
    }

}
