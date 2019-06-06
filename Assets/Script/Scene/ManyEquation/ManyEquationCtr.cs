using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ManyEquationCtr : MonoBehaviour {
    public SoundManager mSound { get; set; }
    public Guanka mGuanka = new Guanka();
    private InputNumObj inputobj { get; set; }
    private RawImage bg { get; set; }
    private GameObject currGo { get; set; }
    private GameObject ques1Go { get; set; }
    private GameObject animalGo { get; set; }
    private Image currchildImage { get; set; }
    private Image maxdemoimg { get; set; }
    private bool inpass { get; set; }
    private GameObject elGo { get; set; }

    public class Guanka
    {
        public int guanka { get; set; }
        public int guanka_last { get; set; }

        public int maxNum { get; set; }
        public int num1 { get; set; }
        public int num2 { get; set; }

        public string type { get; set; }

        public string scene { get; set; }

        public List<GameObject> spins = new List<GameObject>();

        public List<GameObject> quesGos = new List<GameObject>();

        public List<GameObject> timus = new List<GameObject>();

        public List<Vector3> timuPoss = new List<Vector3>();

        public List<Vector3> timuEndPoss = new List<Vector3>();

        public List<Vector3> animalPoss = new List<Vector3>();

        public List<Vector3> part1Data = new List<Vector3>();
        public List<Vector3> part2Data = new List<Vector3>();

        public List<ManyCar> part1Gos = new List<ManyCar>();
        public List<ManyCar> part2Gos = new List<ManyCar>();

        public string showSound { get; set; }

        public Vector3 questionPos { get; set; }
        public Vector3 inputPos { get; set; }

        public float scaleNum { get; set; }

        public List<Vector2> markedList { get; set; }//记录历史的两个num1，num2

        public Vector3 demoStartPos { get; set; }
        public Vector3 suanshiStartPos { get; set; }
        public Vector3 showPos { get; set; }

        public string danYuanSoundName { get; set; }
        public string currenType { get; set; }

        public bool isLigt { get; set; }
        public Guanka()
        {
            guanka_last = 2;
        }
        public void initGamedata()
        {
            maxNum = Common.GetMutexValue(5, 10);
            guanka_last = (int)maxNum / 2;
            num1 = 0;
            num2 = 0;
            markedList = new List<Vector2>();
        }
        public void Set(int _guanka)
        {
            guanka = _guanka;
            num1 = 0;
            num2 = 0;
            for (int i = 0; i < spins.Count; i++)
            {
                ManyEquationSpine spine = spins[i].GetComponent<ManyEquationSpine>();
                GameObject.Destroy(spine.gameObject);
            }
            for (int i = 0; i < quesGos.Count; i++)
            {
                GameObject go = quesGos[i];
                GameObject.Destroy(go);
            }
            quesGos.Clear();
            for (int i = 0; i < timus.Count; i++)
            {
                GameObject go = timus[i];
                GameObject.Destroy(go);
            }
            spins.Clear();
            timus.Clear();
            timuPoss.Clear();

            animalPoss.Clear();

            part1Gos.Clear();
            part2Gos.Clear();
            //题目播放算式的最终位置
            timuEndPoss.Add(new Vector3(150, -200, 0));
            timuEndPoss.Add(new Vector3(150, -130, 0));
            timuEndPoss.Add(new Vector3(150, -60, 0));
            timuEndPoss.Add(new Vector3(150, 10, 0));

            timuPoss.Add(new Vector3(-453, -202, 0));
            timuPoss.Add(new Vector3(-29, -202, 0));
            timuPoss.Add(new Vector3(-29, -311, 0));
            timuPoss.Add(new Vector3(380, -202, 0));
            timuPoss.Add(new Vector3(380, -311, 0));

            showSound = "素材出去通用";
            //案例的初始位置
            demoStartPos = new Vector3(0, -237, 0);
            //算式的初始位置位置
            questionPos = new Vector3(0, -237, 0);
            //输入框的位置
            inputPos = new Vector3(-55, -76, 0);
            isLigt = false;
            scaleNum = 0.25f;
            danYuanSoundName = "danyuan_zhi";
            if (maxNum == 5)
            {
                scene = "scene_1";
                questionPos = new Vector3(254, -10, 0);
                demoStartPos = new Vector3(181, 0, 0);

                showPos = questionPos;
                timuPoss.Clear();
                timuPoss.Add(new Vector3(-453, -202, 0));
                timuPoss.Add(new Vector3(-29, -202, 0));
                timuPoss.Add(new Vector3(-29, -311, 0));
                timuPoss.Add(new Vector3(380, -202, 0));
                timuPoss.Add(new Vector3(380, -311, 0));

                animalPoss.Add(new Vector3(456, 0, 0));
                animalPoss.Add(new Vector3(-448, -169, 0));
                animalPoss.Add(new Vector3(-173, -161, 0));
                animalPoss.Add(new Vector3(-256, 31, 0));
                animalPoss.Add(new Vector3(-6, 40, 0));
            }
            else if (maxNum == 6)
            {
                //scaleNum = 0.5f;
                scene = "scene_2";

                questionPos = new Vector3(254, -10, 0);
                demoStartPos = new Vector3(181, 0, 0);
                showPos = questionPos;

                timuPoss.Clear();
                timuPoss.Add(new Vector3(-453, -202, 0));
                timuPoss.Add(new Vector3(-29, -202, 0));
                timuPoss.Add(new Vector3(-29, -311, 0));
                timuPoss.Add(new Vector3(380, -202, 0));
                timuPoss.Add(new Vector3(380, -311, 0));

                animalPoss.Add(new Vector3(456, 0, 0));
                animalPoss.Add(new Vector3(-428, -184, 0));
                animalPoss.Add(new Vector3(-134, -89, 0));
                animalPoss.Add(new Vector3(-256, 31, 0));
                animalPoss.Add(new Vector3(-6, 101, 0));
                animalPoss.Add(new Vector3(216, 85, 0));

            }
            else if (maxNum == 7)
            {
                scene = "scene_3";
                showSound = "scene_3_ok";
                questionPos = new Vector3(80, -68, 0);
                demoStartPos = new Vector3(5, -20, 0);
                showPos = new Vector3(53, 121, 0);
                //大小，颜色，方向
                int dic = 0;//Common.GetMutexValue(0, 1);//随机一个方向

                if(0 == 0)//第一行有几个车子 == 0 第一行有4个车子//Common.GetMutexValue(0, 1)
                {
                    dic = 0;
                    part1Data = new List<Vector3>() { new Vector3(0, 0, dic), new Vector3(1, 1, dic), new Vector3(1, 1, dic), new Vector3(1, 1, dic) };
                    int sdic = 0;
                    if (dic == 0)
                    {
                        sdic = 1;
                    }
                    part2Data = new List<Vector3>() { new Vector3(1, 1, sdic), new Vector3(1, 1, sdic), new Vector3(1, 1, sdic) };

                    if (0 == 0)//判断另外一个黄色是在第几行 == 0 在第一行//Common.GetMutexValue(0, 1)
                    {
                        part1Data = new List<Vector3>() { new Vector3(0, 0, dic), new Vector3(1, 0, dic), new Vector3(1, 1, dic), new Vector3(1, 1, dic) };
                    }
                    else{
                        part2Data = Common.BreakRank(new List<Vector3>() { new Vector3(1, 1, sdic), new Vector3(1, 0, sdic), new Vector3(1, 1, sdic) });
                    }
                }
                else
                {
                    dic = 1;
                   
                    part1Data = Common.BreakRank(new List<Vector3>() { new Vector3(0, 0, dic), new Vector3(1, 1, dic), new Vector3(1, 1, dic)});
                    int sdic = 1;
                    if (dic == 1)
                    {
                        sdic = 0;
                    }
                    part2Data = Common.BreakRank(new List<Vector3>() { new Vector3(1, 1, sdic), new Vector3(1, 1, sdic), new Vector3(1, 1, sdic), new Vector3(1, 1, sdic) });

                    if (Common.GetMutexValue(0, 1) == 0)
                    {
                        part1Data = Common.BreakRank(new List<Vector3>() { new Vector3(0, 0, dic), new Vector3(1, 0, dic), new Vector3(1, 1, dic)});
                    }
                    else
                    {
                        part2Data = Common.BreakRank(new List<Vector3>() { new Vector3(1, 1, sdic), new Vector3(1, 1, sdic), new Vector3(1, 0, sdic), new Vector3(1, 1, sdic) });
                    }
                }
                timuPoss.Clear();
                timuPoss.Add(new Vector3(-453, -202, 0));
                timuPoss.Add(new Vector3(-29, -202, 0));
                timuPoss.Add(new Vector3(-29, -311, 0));
                timuPoss.Add(new Vector3(380, -202, 0));
                timuPoss.Add(new Vector3(380, -311, 0));
            }
            else if (maxNum == 8)
            {
                scene = "scene_4";
                isLigt = true;
                questionPos = new Vector3(150, -37, 0);
                demoStartPos = new Vector3(40, 11, 0);
                showPos = questionPos;
                inputPos = new Vector3(355, -33, 0);
                //timuEndPoss.Add(new Vector3(-455, -255, 0));
                //timuEndPoss.Add(new Vector3(230, -300, 0));
                //timuEndPoss.Add(new Vector3(230, -230, 0));
                //timuEndPoss.Add(new Vector3(230, -160, 0));
                //timuEndPoss.Add(new Vector3(230, -90, 0));

                animalPoss.Add(new Vector3(-169, -56, 0));
                animalPoss.Add(new Vector3(114, 38, 0));
                animalPoss.Add(new Vector3(-9, 141, 0));
                animalPoss.Add(new Vector3(477, -27, 0));
                animalPoss.Add(new Vector3(-292, 127, 0));
                animalPoss.Add(new Vector3(-451, -165, 0));
                animalPoss.Add(new Vector3(-514, 48, 0));
                animalPoss.Add(new Vector3(280, 110, 0));
            }
            else if (maxNum == 9)
            {
                scene = "scene_5";
                questionPos = new Vector3(95, -18, 0);
                demoStartPos = new Vector3(10, 10, 0);
                inputPos = new Vector3(-253, -130, 0);
                showPos = new Vector3(108, 78, 0);
                //timuPoss.Add(new Vector3(-455, -255, 0));
                //timuPoss.Add(new Vector3(52, -300, 0));
                //timuPoss.Add(new Vector3(52, -230, 0));
                //timuPoss.Add(new Vector3(52, -160, 0));
                //timuPoss.Add(new Vector3(52, -90, 0));

                animalPoss.Add(new Vector3(-508, 77, 0));
                animalPoss.Add(new Vector3(-237, 94, 0));
                animalPoss.Add(new Vector3(73, 117, 0));
                animalPoss.Add(new Vector3(322, 87, 0));
                animalPoss.Add(new Vector3(532, 78, 0));
                animalPoss.Add(new Vector3(-551, -138, 0));
                animalPoss.Add(new Vector3(-247, -126, 0));
                animalPoss.Add(new Vector3(288, -134, 0));
                animalPoss.Add(new Vector3(542, -130, 0));

                timuEndPoss.Clear();
                timuEndPoss.Add(new Vector3(66, -200, 0));
                timuEndPoss.Add(new Vector3(66, -130, 0));
                timuEndPoss.Add(new Vector3(66, -60, 0));
                timuEndPoss.Add(new Vector3(66, 10, 0));
            }
            else if (maxNum == 10)
            {
                scene = "scene_6";
                questionPos = new Vector3(110, -68, 0);
                demoStartPos = new Vector3(15, 0, 0);
                showPos = new Vector3(140, 20, 0);

                animalPoss.Add(new Vector3(148, 9, 0));
                animalPoss.Add(new Vector3(-8, 41, 0));
                animalPoss.Add(new Vector3(-513, 61, 0));
                animalPoss.Add(new Vector3(405, -210, 0));
                animalPoss.Add(new Vector3(-172, -33, 0));
                animalPoss.Add(new Vector3(-270, 53, 0));
                animalPoss.Add(new Vector3(-356, -83, 0));
                animalPoss.Add(new Vector3(384, -44, 0));
                animalPoss.Add(new Vector3(251, 60, 0));
                animalPoss.Add(new Vector3(-508, -207, 0));

                timuEndPoss.Clear();
                timuEndPoss.Add(new Vector3(60, -210, 0));
                timuEndPoss.Add(new Vector3(60, -140, 0));
                timuEndPoss.Add(new Vector3(60, -70, 0));
                timuEndPoss.Add(new Vector3(60, 0, 0));
            }

            switch (_guanka)
            {
                case 1:
                    type = "add";
                    break;
                case 2:
                    type = "drog";
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }

        }
        public Dictionary<string, int> getCarInfo(int num)
        {
            Dictionary<string,int> dic = new Dictionary<string, int>();
            if(num == 1)
            {
                dic.Add("size",0);
            }
            if (num == 2)
            {
                dic.Add("color", 0);
            }
            if (num == 3)
            {
                dic.Add("dic", 1);
            }
            if (num == 4)
            {
                dic.Add("dic", 0);
            }
            if (num == 5)
            {
                dic.Add("color", 1);
            }
            if (num == 6)
            {
                dic.Add("size", 1);
            }
            return dic;
        }
    }
    // Use this for initialization
    void Awake()
    {
        mSound = gameObject.AddComponent<SoundManager>();
    }
    private bool isPlayBged = false;
    void Start()
    {
        mGuanka.guanka = 1;
        mGuanka.initGamedata();
        mGuanka.Set(mGuanka.guanka);
        bg = UguiMaker.newRawImage("bg", transform, mGuanka.scene, false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        StartCoroutine(TInit());
        
        //initGame();
    }

    // Update is called once per frame
    private float scaleTime = 0;
    void Update()
    {

        if (inpass || (null != inputobj && inputobj.gameObject.active)) return;

        if (null != currGo && currGo.active)
        {
            scaleTime += 0.1f;
            if (null != currGo.transform.Find("maximg/wen"))
            {
                Image maxwen = currGo.transform.Find("maximg/wen").gameObject.GetComponent<Image>();
                if (maxwen.enabled)
                {
                    maxwen.rectTransform.localScale = Vector3.one * (0.8f + 0.2f * Mathf.Sin(scaleTime));
                }
            }

            Image num1wen = currGo.transform.Find("num1img/wen").gameObject.GetComponent<Image>();
            if (num1wen.enabled)
            {
                num1wen.rectTransform.localScale = Vector3.one * (0.8f + 0.2f * Mathf.Sin(scaleTime));
            }
            Image num2wen = currGo.transform.Find("num2img/wen").gameObject.GetComponent<Image>();
            if (num2wen.enabled)
            {
                num2wen.rectTransform.localScale = Vector3.one * (0.8f + 0.2f * Mathf.Sin(scaleTime));
            }

        }

        if (mGuanka.isLigt && null != elGo)
        {
            scaleTime += 0.1f;
            for (int i = 0;i < 2; i++)
            {
                Image light = elGo.transform.Find("lightGo_" + i + "/light").gameObject.GetComponent<Image>();
                Color color = light.color;
                color.a = (0.8f + 0.2f * Mathf.Sin(scaleTime));
                light.color = color;
            }
            
            
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            currGo = null;
            if (null != hits)
            {
                foreach (RaycastHit hit in hits)
                {
                    Image com = hit.collider.gameObject.GetComponent<Image>();
                    if (null != com && (com.name == "num1img" || com.name == "num2img" || com.name == "maximg"))
                    {
                        currGo = com.transform.parent.gameObject;
                        currchildImage = com;
                        showInput();
                        break;
                    }
                }
            }
        }

    }
    //显示输入框
    private void showInput()
    {
        if (inpass || (null != inputobj && inputobj.gameObject.active)) return;

        if (null == inputobj)
        {
            InputInfoData data = new InputInfoData();
            data.fNumScale = 3f;
            data.fscale = 0.3f;
            data.vBgSize = new Vector2(665, 745);//575);
            data.vCellSize = new Vector2(200, 166);
            data.vSpacing = new Vector2(10, 10);
            data.bgcolor = new Color32(252, 229, 194, 255);
            data.color_blockBG = new Color32(179, 138, 89, 255);
            data.color_blockNum = new Color32(202, 183, 155, 255);
            data.color_blockSureBG = new Color32(172, 123, 66, 255);
            data.color_blockSureStart = new Color32(202, 183, 155, 255);
            //data.mBlockIDList = new List<int>() { 7, 8, 9, 4, 5, 6, 1, 2, 3, 0 };
            data.strAlatsName = "manyequation_sprite";

            inputobj = InputNumObj.Create(transform, data);
            inputobj.nCountLimit = -1;
            Canvas canvas = inputobj.gameObject.AddComponent<Canvas>();
            inputobj.gameObject.layer = LayerMask.NameToLayer("UI");
            canvas.overrideSorting = true;
            canvas.sortingOrder = 11;
            inputobj.SetInputNumberCallBack(getNumfromInputNumObj);
            inputobj.SetClearNumberCallBack(CleanInputNum);
            //inputobj.SetFinishInputCallBack(FinishInputNum);
            inputobj.transform.localPosition = new Vector3(-55, -76, 0);
            GraphicRaycaster gry = inputobj.gameObject.AddComponent<GraphicRaycaster>();
            gry.ignoreReversedGraphics = true;
            
            mSound.PlayShort("manyequation_sound", "button_down");
            /*
            GridLayoutGroup grid = inputobj.gameObject.GetComponent<GridLayoutGroup>();
            grid.enabled = false;
            InputNumBlockObj blockObj = inputobj.GetInputNumBlockByIndex(9);
            float fGetLocalX = inputobj.GetInputNumBlockByIndex(7).transform.localPosition.x;
            blockObj.transform.localPosition = new Vector3(fGetLocalX, blockObj.transform.localPosition.y, 0f);
            */
        }
        /*
        if (currGo == ques1Go)
        {
            inputobj.transform.localPosition = new Vector3(353, currGo.transform.localPosition.y - 350, 0);
        }
        else
        {
            inputobj.transform.localPosition = new Vector3(353, currGo.transform.localPosition.y - 150, 0);
        }
        */
        inputobj.transform.localPosition = currGo.transform.localPosition +  currchildImage.transform.localPosition + new Vector3(0,-170,0) ;// mGuanka.inputPos;
        inputobj.ShowEffect();
    }
    ////结束输入
    //private void FinishInputNum()
    //{
    //    //Debug.Log("FinishInputNum inputobj.strInputNum : " + inputobj.strInputNum);
    //    string setNum = inputobj.strInputNum;
    //}
    //输入框清除数据
    private void CleanInputNum()
    {
        if (null != currchildImage)
        {
            Image child = currchildImage.transform.Find("num").gameObject.GetComponent<Image>();
            child.enabled = false;
            Image wen = currchildImage.transform.Find("wen").gameObject.GetComponent<Image>();
            wen.enabled = true;
        }
    }
    //获取选择的数字
    private void getNumfromInputNumObj()
    {
        string setNum = inputobj.strInputNum;
        if(setNum == "0")
        {
            setNum = "10";
        }
        inputobj.HideEffect();
        if (null != currchildImage)
        {
            Image child = currchildImage.transform.Find("num").gameObject.GetComponent<Image>();
            child.sprite = ResManager.GetSprite("manyequation_sprite", setNum);
            child.enabled = true;
            Image wen = currchildImage.transform.Find("wen").gameObject.GetComponent<Image>();
            wen.enabled = false;
        }
        Image maxchild = currGo.transform.Find("maximg/num").gameObject.GetComponent<Image>();
        Image num1child = currGo.transform.Find("num1img/num").gameObject.GetComponent<Image>();
        num1child.SetNativeSize();
        Image num2child = currGo.transform.Find("num2img/num").gameObject.GetComponent<Image>();
        num2child.SetNativeSize();

        Image currType = currGo.transform.Find("type").gameObject.GetComponent<Image>();

        if (currchildImage.name == "maximg")
        {
            if (setNum != mGuanka.maxNum.ToString())
            {
                StartCoroutine(TScale(currchildImage));
            }
            else
            {
                if (maxchild.enabled && num1child.enabled && num2child.enabled)
                {
                    checkFinishCurr(maxchild, num1child, num2child, currType);
                }
            }
        }
        else if (currchildImage.name == "num1img")
        {
            if (int.Parse(setNum) <  mGuanka.maxNum)
            {
                if (mGuanka.num1 != 0)
                {
                    if (setNum == mGuanka.num1.ToString() || setNum == mGuanka.num2.ToString())
                    {
                        if (isMark(num1child, num2child, currType,mGuanka.timus.Count))
                        {
                            StartCoroutine(TScale(currchildImage));
                        }
                        else
                        {
                            if (maxchild.enabled && num1child.enabled && num2child.enabled)
                            {
                                checkFinishCurr(maxchild, num1child, num2child, currType);
                            }
                        }
                    }
                    else
                    {
                        StartCoroutine(TScale(currchildImage));
                    }
                }
                else
                {
                    if(maxchild.enabled && num1child.enabled && num2child.enabled)
                    {
                        checkFinishCurr(maxchild, num1child, num2child, currType);
                    }else
                    {
                        int num1 = 0;
                        int num2 = 0;
                        if (num1child.enabled)
                        {
                            num1 = int.Parse(num1child.sprite.name);
                        }
                        if (num2child.enabled)
                        {
                            num2 = int.Parse(num2child.sprite.name);
                        }
                        checkMared(num1, num2);
                    }
                    
                }
            }
            else
            {
                StartCoroutine(TScale(currchildImage));
            }
        }
        else if (currchildImage.name == "num2img")
        {
            if (int.Parse(setNum) < mGuanka.maxNum)
            {
                if (mGuanka.num2 != 0)
                {
                    if (setNum == mGuanka.num1.ToString() || setNum == mGuanka.num2.ToString())
                    {
                        Debug.Log("mGuanka.timus.Count : " + mGuanka.timus.Count);
                        if (isMark(num1child, num2child, currType,mGuanka.timus.Count))
                        {
                            StartCoroutine(TScale(currchildImage));
                        }
                        else
                        {
                            if (maxchild.enabled && num1child.enabled && num2child.enabled)
                            {
                                checkFinishCurr(maxchild, num1child, num2child, currType);
                            }
                        }
                    }
                    else
                    {
                        StartCoroutine(TScale(currchildImage));
                    }
                }
                else
                {
                    if (maxchild.enabled && num1child.enabled && num2child.enabled)
                    {
                        checkFinishCurr(maxchild, num1child, num2child, currType);
                    }
                    else
                    {
                        int num1 = 0;
                        int num2 = 0;
                        if (num1child.enabled)
                        {
                            num1 = int.Parse(num1child.sprite.name);
                        }
                        if (num2child.enabled)
                        {
                            num2 = int.Parse(num2child.sprite.name);
                        }
                        checkMared(num1, num2);
                    }
                }
            }
            else
            {
                StartCoroutine(TScale(currchildImage));
            }
        }
    }
    //检测历史记录
    private void checkMared(int num1,int num2)
    {
        Debug.Log("checkMared curr num1 : " + num1 + ";num2 : " + num2);
        bool boo = false;
        for (int i = 0; i < mGuanka.markedList.Count; i++)
        {
            Vector2 vec2 = mGuanka.markedList[i];
            Debug.Log("checkMared num1 : " + vec2.x + ";num2 : " + vec2.y);
            if (num1 == vec2.x || num1 == vec2.y || num2 == vec2.x || num2 == vec2.y)
            {
                //TODO 错误逻辑
                boo = true;
                break;
            }
        }
        if (boo)
        {
            StartCoroutine(TScale(currchildImage));
        }
    }
    private void checkFinishCurr(Image maxchild, Image num1child, Image num2child,Image currType)
    {
        Debug.Log("checkFinishCurr");
        if (int.Parse(maxchild.sprite.name) == mGuanka.maxNum && (int.Parse(num1child.sprite.name) + int.Parse(num2child.sprite.name) == mGuanka.maxNum) && num1child.enabled && num2child.enabled && maxchild.enabled)
        {
            if (mGuanka.timus.Count <= 1)
            {

                if (mGuanka.timus.Count < 1)
                {
                    mGuanka.num1 = int.Parse(num1child.sprite.name);
                    mGuanka.num2 = int.Parse(num2child.sprite.name);
                    finishQues1(currGo,true);
                }
                else
                {
                    if ((int.Parse(num1child.sprite.name) == mGuanka.num1 || int.Parse(num1child.sprite.name) == mGuanka.num2) && (int.Parse(num2child.sprite.name) == mGuanka.num1 || int.Parse(num2child.sprite.name) == mGuanka.num2))
                    {
                        finishQues1(currGo);
                    }
                    else
                    {
                        playErrsound();
                        //TODO 错误逻辑
                    }
                }
            }
            else
            {
                bool boo = true;
                for (int i = 0; i < mGuanka.timus.Count; i++)
                {
                    Image olsChild1 = mGuanka.timus[i].transform.Find("num1img/num").gameObject.GetComponent<Image>();
                    Image olsChild2 = mGuanka.timus[i].transform.Find("num2img/num").gameObject.GetComponent<Image>();
                    Image oldType = mGuanka.timus[i].transform.Find("type").gameObject.GetComponent<Image>();
                    if (olsChild2.sprite.name == num2child.sprite.name && oldType.sprite.name == currType.sprite.name && olsChild2.sprite.name != olsChild1.sprite.name)
                    {
                        //TODO 错误逻辑
                        boo = false;
                        break;
                    }
                }
                if (boo)
                {
                    if ((int.Parse(num1child.sprite.name) == mGuanka.num1 || int.Parse(num1child.sprite.name) == mGuanka.num2) && (int.Parse(num2child.sprite.name) == mGuanka.num1 || int.Parse(num2child.sprite.name) == mGuanka.num2))
                    {
                        finishQues1(currGo);
                    }
                    else
                    {
                        //TODO 错误逻辑
                        playErrsound();
                    }

                }
                else
                {
                    playErrsound();
                }
            }
        }
        else
        {
            //TODO 错误逻辑
            playErrsound();

        }
        
    }
    private bool isMark(Image num1child, Image num2child, Image currType, int index)
    {
        bool state = false;
        for (int i = 1; i < mGuanka.timus.Count; i++)
        {
            Image olsChild2 = mGuanka.timus[i].transform.Find("num2img/num").gameObject.GetComponent<Image>();
            Image olsChild1 = mGuanka.timus[i].transform.Find("num1img/num").gameObject.GetComponent<Image>();
            Image oldType = mGuanka.timus[i].transform.Find("type").gameObject.GetComponent<Image>();
            /*
            
            /*
            Image oldType = mGuanka.timus[i].transform.FindChild("type").gameObject.GetComponent<Image>();
            if (oldType.sprite.name == currType.sprite.name && (olsChild2.sprite.name == num2child.sprite.name || olsChild1.sprite.name == num1child.sprite.name))
            {
                //TODO 错误逻辑
                state = true;
                break;
            }
            */

            if (index == 1 || index == 2)
            {
                Debug.Log("num1child.sprite.name : " + num1child.sprite.name + ";olsChild1.sprite.name : " + olsChild1.sprite.name);
                if (olsChild1.sprite.name == num1child.sprite.name && olsChild1.sprite.name != olsChild2.sprite.name)
                {
                    state = true;
                    break;
                }
            }
            else
            {
                if (olsChild2.sprite.name == num2child.sprite.name && olsChild1.sprite.name != olsChild2.sprite.name && oldType.sprite.name == currType.sprite.name)
                {
                    state = true;
                    break;
                }
            }
        }
        return state;
    }
    private void playErrsound()
    {
        playPrefabFail();
        StartCoroutine(TScale(currchildImage));
        /*
        Image maxchild = currGo.transform.FindChild("maximg/num").gameObject.GetComponent<Image>();
        Image num1child = currGo.transform.FindChild("num1img/num").gameObject.GetComponent<Image>();
        Image num2child = currGo.transform.FindChild("num2img/num").gameObject.GetComponent<Image>();

        if (num2child.enabled && num1child.enabled && maxchild.enabled)
        {
            playPrefabFail();
            mSound.PlayOnly("manyequation_sound", "sound_fail2", 5);
            if (currGo != ques1Go)
            {
                StartCoroutine(TScale(currGo.transform.FindChild("maximg").gameObject.GetComponent<Image>()));
            }
            StartCoroutine(TScale(currGo.transform.FindChild("num1img").gameObject.GetComponent<Image>()));
            StartCoroutine(TScale(currGo.transform.FindChild("num2img").gameObject.GetComponent<Image>()));
        }
        */
    }
    //播放成功后动物的动画
    private void playPrefabSuccse()
    {
        // Debug.Log("playPrefabSuccse mGuanka.spins.Count:" + mGuanka.spins.Count);
        for (int i = 0; i < mGuanka.spins.Count; i++)
        {
            ManyEquationSpine spine = mGuanka.spins[i].GetComponent<ManyEquationSpine>();
            spine.PlaySpineEffect("Correct");
        }
        mSound.PlayShort("manyequation_sound", "欢呼0", 0.3f);
    }
    private void playPrefabFail()
    {
        mSound.PlayOnly("manyequation_sound", "sound_fail2", 5);
        //Debug.Log("playPrefabFail mGuanka.spins.Count:" + mGuanka.spins.Count);
        for (int i = 0; i < mGuanka.spins.Count; i++)
        {
            ManyEquationSpine spine = mGuanka.spins[i].GetComponent<ManyEquationSpine>();
            spine.PlaySpineEffect("Mistake");

        }
    }
    private void finishQues1(GameObject go,bool isdemo = false)
    {
        //Debug.Log("完成第一个题目");
        mSound.StopOnly();
        go.transform.Find("num1img").gameObject.GetComponent<BoxCollider>().enabled = false;
        go.transform.Find("num2img").gameObject.GetComponent<BoxCollider>().enabled = false;
        if (go.transform.Find("maximg").gameObject.GetComponent<BoxCollider>())
        {
            go.transform.Find("maximg").gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        StartCoroutine(TSetQue(go, isdemo));
    }
    /*
    private void finishQues(GameObject go)
    {
        Debug.Log("完成一个题目");
        
        StartCoroutine(TSetQue(go));
    }
    */
    //重置游戏
    private void reGame()
    {
        mGuanka.guanka = 1;
        mGuanka.initGamedata();
        setGameData();
    }

    private void initGame()
    {
        setGameData(true);
    }
    //设置游戏数据
    private void setGameData(bool isInnit = false)
    {
        if (mGuanka.guanka == 1)
        {
            TopTitleCtl.instance.Reset(mGuanka.guanka_last);
        }
        else
        {
            TopTitleCtl.instance.AddStar();
        }
        if (!isInnit)
        {
            mGuanka.Set(mGuanka.guanka);
        }
        inpass = false;
        if (null != animalGo)
        {
            GameObject.Destroy(animalGo);
        }
        animalGo = UguiMaker.newGameObject("animalGo", transform);

        if(null != elGo)
        {
            GameObject.Destroy(elGo);
        }
        
        if (mGuanka.scene == "scene_3")
        {
            StartCoroutine(creteScene3());
        }else if(mGuanka.scene == "scene_2")
        {
            StartCoroutine(creteScene2());
        }
        else if (mGuanka.scene == "scene_6")
        {
            StartCoroutine(creteScene6());
        }
        else if (mGuanka.scene == "scene_5")
        {
            StartCoroutine(creteScene5());
        }
        else if (mGuanka.scene == "scene_4")
        {
            StartCoroutine(creteScene4());
        }
        else if (mGuanka.scene == "scene_1")
        {
            StartCoroutine(creteScene1());
        }

    }
    private void createQuestion()
    {
        if (null != ques1Go)
        {
            GameObject.Destroy(ques1Go);
        }

        //出题第一道题目
        ques1Go = UguiMaker.newGameObject("ques1Go", transform);
        float scaleNum = mGuanka.scaleNum;
        Image maximg = UguiMaker.newImage("maximg", ques1Go.transform, "manyequation_sprite", "maxNum");
        maximg.enabled = false;
        Image maxNumimg = UguiMaker.newImage("num", maximg.transform, "manyequation_sprite", mGuanka.maxNum.ToString());
        maxNumimg.color = Color.black;
        maxNumimg.rectTransform.localScale = Vector3.one * (scaleNum + 0.1f);

        Image fenimg = UguiMaker.newImage("type", ques1Go.transform, "manyequation_sprite", "fen");
        fenimg.transform.localPosition = new Vector3(0, -41, 0);


        Image num1img = UguiMaker.newImage("num1img", ques1Go.transform, "manyequation_sprite", "childNum");
        num1img.transform.localPosition = new Vector3(-77, -81, 0);
        BoxCollider num1box = num1img.gameObject.AddComponent<BoxCollider>();
        num1box.size = new Vector3(131, 131, 0);

        Image num1Numimg = UguiMaker.newImage("num", num1img.transform, "manyequation_sprite", mGuanka.num1.ToString());
        num1Numimg.rectTransform.localScale = Vector3.one * scaleNum;
        num1Numimg.color = Color.black;
        num1Numimg.enabled = false;
        Image wen1 = UguiMaker.newImage("wen", num1img.transform, "manyequation_sprite", "wen_red");
        wen1.rectTransform.localScale = Vector3.one * 0.8f;


        Image num2img = UguiMaker.newImage("num2img", ques1Go.transform, "manyequation_sprite", "childNum");
        num2img.transform.localPosition = new Vector3(77, -81, 0);
        BoxCollider num2box = num2img.gameObject.AddComponent<BoxCollider>();
        num2box.size = new Vector3(131, 131, 0);

        Image num2Numimg = UguiMaker.newImage("num", num2img.transform, "manyequation_sprite", mGuanka.num2.ToString());
        num2Numimg.rectTransform.localScale = Vector3.one * scaleNum;
        num2Numimg.color = Color.black;
        num2Numimg.enabled = false;
        Image wen2 = UguiMaker.newImage("wen", num2img.transform, "manyequation_sprite", "wen_red");
        wen2.rectTransform.localScale = Vector3.one * 0.8f;

        ques1Go.transform.localPosition = mGuanka.demoStartPos;
        ques1Go.transform.localScale = Vector3.one * 1.2f;
        currGo = ques1Go;
        mSound.PlayShort("manyequation_sound", "指针对位正确");
        mSound.PlayTip("manyequation_sound", "game-tips", 1, true);

        if (!isPlayBged)
        {
            StartCoroutine(DelayPlayBgSound());
        }
    }
    IEnumerator DelayPlayBgSound()
    {
        yield return new WaitForSeconds(2f);
        if (!isPlayBged)
        {
            mSound.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.1f);
            isPlayBged = true;
        }
    }
    IEnumerator creteScene1()
    {
        Vector3 startScale = Vector3.zero;
        Vector3 ensScale = Vector3.one * 0.9f;
        //创建动物
        for (int i = 0; i < mGuanka.maxNum; i++)
        {
            mSound.PlayShort("素材出现通用音效");
            int index = i + 1;
            ManyEquationSpine spine = ResManager.GetPrefab("manyequation_prefab", "scene_1_" + index).GetComponent<ManyEquationSpine>();
            spine.transform.parent = animalGo.transform;
            spine.transform.localPosition = mGuanka.animalPoss[i];
            //spine.transform.localScale = ;
            spine.PlaySpineEffect("Correct");
            mGuanka.spins.Add(spine.gameObject);
            for (float j = 0; j < 1f; j += 0.1f)
            {
                spine.transform.localScale = Vector3.Lerp(startScale, ensScale, j);
                yield return new WaitForSeconds(0.01f);
            }
        }
        createQuestion();
    }
    IEnumerator creteScene4()
    {
        elGo = UguiMaker.newGameObject("elGo", transform);
        List<Vector3> arr = new List<Vector3>() { new Vector3(-399, 158, 0), new Vector3(164, 173, 0) };
        Vector3 startPos = new Vector3(-519, -338, 0);
        for (int i = 0; i < 2; i++)
        {
            GameObject go = getLight(i, elGo.transform);
            go.transform.localPosition = startPos + new Vector3(1020 * i, 0, 0);
            if (i == 1)
            {
                go.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
            }
        }
        Vector3 startScale = Vector3.zero;
        Vector3 ensScale = Vector3.one * 0.5f;
        //创建动物
        for (int i = 0; i < mGuanka.maxNum; i++)
        {
            mSound.PlayShort("素材出现通用音效");
            int index = i + 1;
            ManyEquationSpine spine = ResManager.GetPrefab("manyequation_prefab", "scene_4_" + index).GetComponent<ManyEquationSpine>();
            spine.transform.parent = animalGo.transform;
            spine.transform.localPosition = mGuanka.animalPoss[i];
            //spine.transform.localScale = Vector3.one * 0.5f;
            //spine.PlaySpine("Idle", true);
            spine.PlaySpineEffect("Correct");
            mGuanka.spins.Add(spine.gameObject);

            for (float j = 0; j < 1f; j += 0.1f)
            {
                spine.transform.localScale = Vector3.Lerp(startScale, ensScale, j);
                yield return new WaitForSeconds(0.01f);
            }
        }
        createQuestion();
    }
    private GameObject getLight(int id,Transform parent)
    {
        GameObject go = UguiMaker.newGameObject("lightGo_" + id, parent);
        Image light = UguiMaker.newImage("light", go.transform, "manyequation_sprite", "scene_4_light_1");
        light.transform.localPosition = new Vector3(217, 316, 0);
        Image box = UguiMaker.newImage("box_" + id, go.transform, "manyequation_sprite", "scene_4_box");
        Image zhijia = UguiMaker.newImage("zhijia_" + id, go.transform, "manyequation_sprite", "scene_4_" + (id + 1));
        zhijia.transform.localPosition = new Vector3(0, -50, 0);
        return go;
    }
    IEnumerator creteScene5()
    {
        elGo = UguiMaker.newGameObject("elGo", transform);
        List<Vector3> arr = new List<Vector3>() { new Vector3(-399, 158, 0), new Vector3(164, 173, 0), new Vector3(-437, -102, 0), new Vector3(421, -37, 0) };
        mSound.PlayShort("素材出现通用音效");
        for (int i = 1; i < 5; i++)
        {
            Image tree = UguiMaker.newImage("tree", elGo.transform, "manyequation_sprite", "scene_5_" + i);
            tree.rectTransform.localPosition = arr[i-1];

        }
        yield return new WaitForSeconds(0.2f);

        Vector3 startScale = Vector3.zero;
        Vector3 ensScale = Vector3.one * 0.8f;
        //创建动物
        for (int i = 0; i < mGuanka.maxNum; i++)
        {
            mSound.PlayShort("素材出现通用音效");
            int index = i + 1;
            ManyEquationSpine spine = ResManager.GetPrefab("manyequation_prefab", "scene_5_" + index).GetComponent<ManyEquationSpine>();
            spine.transform.parent = animalGo.transform;
            spine.transform.localPosition = mGuanka.animalPoss[i];
            //spine.transform.localScale = Vector3.one * 0.8f;
            //spine.PlaySpine("Idle", true);
            mGuanka.spins.Add(spine.gameObject);
            spine.PlaySpineEffect("Correct");
            for (float j = 0; j < 1f; j += 0.1f)
            {
                spine.transform.localScale = Vector3.Lerp(startScale, ensScale, j);
                yield return new WaitForSeconds(0.01f);
            }
        }
        createQuestion();
    }
    //创建场景6
    IEnumerator creteScene6()
    {
        Vector3 startScale = Vector3.zero;
        Vector3 ensScale = Vector3.one * 0.8f;
        int leng = mGuanka.maxNum;
        //mSound.PlayShort("manyequation_sound", "button_down");
        //创建动物
        for (int i = 0; i < leng; i++)
        {
            int index = i + 1;
            ManyEquationSpine spine = ResManager.GetPrefab("manyequation_prefab", "scene_6_" + index).GetComponent<ManyEquationSpine>();
            spine.transform.localScale = Vector3.zero;
            spine.transform.parent = animalGo.transform;
            spine.transform.localPosition = mGuanka.animalPoss[i];
            yield return new WaitForSeconds(0.01f);
            mSound.PlayShort("素材出现通用音效");
            if (i == 3)
            {
                ensScale = Vector3.one * 0.9f;
            }
            else
            {
                ensScale = Vector3.one * 0.4f;
            }
            mGuanka.spins.Add(spine.gameObject);
            spine.PlaySpineEffect("Correct");
            for (float j = 0; j < 1f; j += 0.1f)
            {
                spine.transform.localScale = Vector3.Lerp(startScale, ensScale, j);
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(0.01f);
            //break;
        }
        //mSound.PlayShort("manyequation_sound", "素材出现通用音效");
        createQuestion();
    }
    //创建场景2
    IEnumerator creteScene2()
    {
        Vector3 startScale = Vector3.zero;
        Vector3 ensScale = Vector3.one * 0.8f;
        //创建动物
        for (int i = 0; i < mGuanka.maxNum; i++)
        {
            mSound.PlayShort("素材出现通用音效");
            int index = i + 1;
            ManyEquationSpine spine = ResManager.GetPrefab("manyequation_prefab", "scene_2_" + index).GetComponent<ManyEquationSpine>();
            spine.transform.parent = animalGo.transform;
            spine.transform.localPosition = mGuanka.animalPoss[i];
            //spine.transform.localScale = Vector3.one * 0.8f;
            //spine.PlaySpine("Idle", true);
            mGuanka.spins.Add(spine.gameObject);
            spine.PlaySpineEffect("Correct");
            for (float j = 0; j < 1f; j += 0.1f)
            {
                spine.transform.localScale = Vector3.Lerp(startScale, ensScale, j);
                yield return new WaitForSeconds(0.01f);
            }
        }
        createQuestion();
    }
    //创建场景3汽车
    IEnumerator creteScene3()
    {
        Vector3 startp1Pos = new Vector3(-445, 170, 0);
        for (int i = 0; i < mGuanka.part1Data.Count; i++)
        {
            Vector3 vec = mGuanka.part1Data[i];
            ManyCar car = UguiMaker.newGameObject("car_1_" + i, animalGo.transform).AddComponent<ManyCar>();
            float ofsetY = Common.GetMutexValue(10, 40);
            if (Common.GetMutexValue(0, 1) == 0)
            {
                ofsetY *= -1;
            }
            Vector3 pos = startp1Pos + new Vector3(300 * i, 0, 0);
            car.setData((int)vec.x, (int)vec.y, (int)vec.z, pos);
            mGuanka.part1Gos.Add(car);
        }
        mSound.PlayShort("manyequation_sound", mGuanka.showSound, 1);
        Vector3 startp2Pos = startp1Pos;
        startp2Pos.y = 0;
        for (int i = 0; i < mGuanka.part2Data.Count; i++)
        {
            Vector3 vec = mGuanka.part2Data[i];
            ManyCar car = UguiMaker.newGameObject("car_2_" + i, animalGo.transform).AddComponent<ManyCar>();
            float ofsetY = Common.GetMutexValue(10, 20);
            if (Common.GetMutexValue(0, 1) == 0)
            {
                ofsetY *= -1;
            }
            Vector3 pos = startp2Pos + new Vector3(300 * i, ofsetY, 0);
            car.setData((int)vec.x, (int)vec.y, (int)vec.z, pos);
            mGuanka.part2Gos.Add(car);
        }
        mSound.PlayShort("manyequation_sound", mGuanka.showSound, 1);
        yield return new WaitForSeconds(2f);
        createQuestion();
    }
    //设置算式
    private void setOneQue(GameObject go, Vector3 pos)
    {
        float starX = -200;
        float dis = 60;
        float scaleNum = 0.25f;
        Image num1img = UguiMaker.newImage("num1img", go.transform, "manyequation_sprite", "childNum");
        num1img.transform.localPosition = new Vector3(starX + dis * 0, 0, 0);
        BoxCollider num1box = num1img.gameObject.AddComponent<BoxCollider>();
        num1box.size = new Vector3(131, 131, 0);

        Image num1Numimg = UguiMaker.newImage("num", num1img.transform, "manyequation_sprite", mGuanka.num1.ToString());
        num1Numimg.rectTransform.localScale = Vector3.one * scaleNum;
        num1Numimg.color = Color.black;
        num1Numimg.enabled = false;

        Image wen1 = UguiMaker.newImage("wen", num1img.transform, "manyequation_sprite", "wen_red");
        wen1.rectTransform.localScale = Vector3.one * 0.8f;

        Image fenimg = UguiMaker.newImage("type", go.transform, "manyequation_sprite", "type_" + mGuanka.type);
        fenimg.transform.localPosition = new Vector3(starX + dis * 1, 0, 0);

        Image num2img = UguiMaker.newImage("num2img", go.transform, "manyequation_sprite", "childNum");
        num2img.transform.localPosition = new Vector3(starX + dis * 2, 0, 0);
        BoxCollider num2box = num2img.gameObject.AddComponent<BoxCollider>();
        num2box.size = new Vector3(131, 131, 0);

        Image num2Numimg = UguiMaker.newImage("num", num2img.transform, "manyequation_sprite", mGuanka.num2.ToString());
        num2Numimg.rectTransform.localScale = Vector3.one * scaleNum;
        num2Numimg.color = Color.black;
        num2Numimg.enabled = false;

        Image wen2 = UguiMaker.newImage("wen", num2img.transform, "manyequation_sprite", "wen_red");
        wen2.rectTransform.localScale = Vector3.one * 0.8f;

        Image deng = UguiMaker.newImage("deng", go.transform, "manyequation_sprite", "type_add_deng");//+ mGuanka.type + "_deng"
        deng.transform.localPosition = new Vector3(starX + dis * 3, 0, 0);

        Image maximg = UguiMaker.newImage("maximg", go.transform, "manyequation_sprite", "childNum");
        maximg.transform.localPosition = new Vector3(starX + dis * 4, 0, 0);
        BoxCollider maxbox = maximg.gameObject.AddComponent<BoxCollider>();
        maxbox.size = new Vector3(131, 131, 0);
        Image maxNumimg = UguiMaker.newImage("num", maximg.transform, "manyequation_sprite", mGuanka.maxNum.ToString());
        maxNumimg.color = Color.black;
        maxNumimg.rectTransform.localScale = Vector3.one * scaleNum;
        maxNumimg.enabled = false;
        Image wenmax = UguiMaker.newImage("wen", maximg.transform, "manyequation_sprite", "wen_red");
        wenmax.rectTransform.localScale = Vector3.one * 0.8f;

        go.transform.localPosition = pos;

        if (mGuanka.type != "add")
        {
            maximg.transform.localPosition = new Vector3(starX + dis * 0, 0, 0);
            num1img.transform.localPosition = new Vector3(starX + dis * 4, 0, 0);
        }
        mGuanka.quesGos.Add(go);
        go.transform.localScale = Vector3.one * 1.2f;
        currGo = go;
    }
    private void nextQuestion()
    {
        GameObject ques = UguiMaker.newGameObject("ques_" + mGuanka.timus.Count, transform);
        Vector3 startPos = mGuanka.questionPos;// new Vector3(79, -199, 0);
        setOneQue(ques, startPos);
        TShowQue(ques);
    }
    private List<string> getGoSoundList(GameObject go1, int index)
    {
        List<string> list;
        Image maxchild = go1.transform.Find("maximg/num").gameObject.GetComponent<Image>();
        string nummaxstr = maxchild.sprite.name;

        Image num1child = go1.transform.Find("num1img/num").gameObject.GetComponent<Image>();
        string num1str = num1child.sprite.name;

        Image num2child = go1.transform.Find("num2img/num").gameObject.GetComponent<Image>();
        string num2str = num2child.sprite.name;
        bool issame = false;
        if (num1str == num2str)
        {
            issame = true;
            num2str = num1str + "_1";
        }

        if (index == 0 || index == 1)
        {
            if(issame)
            {
                if (index == 0)
                {
                    list = new List<string>() {
                        mGuanka.scene + "_" + num1str,
                        "game-tips_f_add",
                        mGuanka.scene + "_" + num2str,
                        "game-tips_f_deng",
                        mGuanka.scene + "_" + mGuanka.maxNum
                    };
                }
                else
                {
                    list = new List<string>() {
                        mGuanka.scene + "_" + num2str,
                        "game-tips_f_add",
                        mGuanka.scene + "_" + num1str,
                        "game-tips_f_deng",
                        mGuanka.scene + "_" + mGuanka.maxNum
                    };
                }
            }else
            {
                list = new List<string>() {
                        mGuanka.scene + "_" + num1str,
                        "game-tips_f_add",
                        mGuanka.scene + "_" + num2str,
                        "game-tips_f_deng",
                        mGuanka.scene + "_" + mGuanka.maxNum
                    };
            }
            
            
        }
        else
        {
            if (issame)
            {
                if (index == 2)
                {
                    list = new List<string>() {
                        mGuanka.scene + "_" + mGuanka.maxNum,
                        "game-tips_f_minus",
                        mGuanka.scene + "_" + num2str,
                        "game-tips_f_deng",
                        mGuanka.scene + "_" + num1str
                    };
                }
                else
                {
                    list = new List<string>() {
                        mGuanka.scene + "_" + mGuanka.maxNum,
                        "game-tips_f_minus",
                        mGuanka.scene + "_" + num1str,
                        "game-tips_f_deng",
                        mGuanka.scene + "_" + num2str
                    };
                }
            }else
            {
                list = new List<string>() {
                        mGuanka.scene + "_" + mGuanka.maxNum,
                        "game-tips_f_minus",
                        mGuanka.scene + "_" + num2str,
                        "game-tips_f_deng",
                        mGuanka.scene + "_" + num1str
                    };
            }
            
            
        }

        return list;
    }
    private List<string> GetABnames()
    {
        List<string> list = new List<string>();
        for (int i = 0; i < 5; i++)
        {
            list.Add("manyequation_sound");
        }
        return list;
    }
    private List<float> getSoundValList()
    {
        float numval = 0.6f;
        List<float> vals = new List<float>()
        {
            1,
            1,
            1,
            1,
            1,
            
        };
        return vals;
    }

    IEnumerator TInit()
    {
        //mSound.PlayShort("manyequation_sound", "素材出现通用音效");
        yield return new WaitForSeconds(0.2f);
        initGame();
    }

    IEnumerator  TPayQuestionMove()
    {
        for(int i = 1;i < 5; i++)
        {
            GameObject go = mGuanka.spins[i];
            Vector3 startPos = go.transform.localPosition;
            Vector3 endPos = mGuanka.questionPos;
            for (float j = 0; j < 1f; j += 0.2f)
            {
                go.transform.localPosition = Vector3.Lerp(startPos, endPos, j);
                yield return new WaitForSeconds(0.01f);
            }
            go.transform.localPosition = endPos;

            Vector3 startScale = Vector3.one * 0.5f;
            Vector3 endScale = Vector3.one * 0.8f;
            //放大
            for (float j = 0; j < 1f; j += 0.2f)
            {
                go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
                yield return new WaitForSeconds(0.01f);
            }
            go.transform.localScale = endScale;
            //缩小
            for (float j = 0; j < 1f; j += 0.2f)
            {
                go.transform.localScale = Vector3.Lerp(endScale,startScale, j);
                yield return new WaitForSeconds(0.01f);
            }
            go.transform.localScale = startScale;

            startPos = endPos;
            endPos = mGuanka.timuEndPoss[i];
            for (float j = 0; j < 1f; j += 0.2f)
            {
                go.transform.localPosition = Vector3.Lerp(startPos, endPos, j);
                yield return new WaitForSeconds(0.01f);
            }
            go.transform.localPosition = endPos;

            yield return new WaitForSeconds(4);
        }
    }
    //抖动某个框
    IEnumerator TScale(Image go)
    {
        playPrefabFail();
        for (float j = 0; j < 1f; j += 0.05f)
        {
            go.transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * j) * 10);
            yield return new WaitForSeconds(0.01f);
        }
        go.transform.localEulerAngles = Vector3.zero;

        Image child = go.transform.Find("num").gameObject.GetComponent<Image>();
        child.enabled = false;
        Image wen = go.transform.Find("wen").gameObject.GetComponent<Image>();
        wen.enabled = true;
    }
    IEnumerator TNextGame()
    {
        inpass = true;
        yield return new WaitForSeconds(2f);
        mGuanka.guanka++;
        Debug.Log("下一关: mGuanka.guanka ：" + mGuanka.guanka + ";mGuanka.guanka_last : " + mGuanka.guanka_last);
        if (mGuanka.guanka > mGuanka.guanka_last)
        {
            TopTitleCtl.instance.AddStar();
            yield return new WaitForSeconds(2f);
            GameOverCtl.GetInstance().Show(mGuanka.guanka_last, reGame);
        }
        else
        {
            Debug.Log("过关……");
            setGameData();
        }
    }
   
    //设置一题最终的显示
    IEnumerator TSetQue(GameObject go,bool isdemo)
    {
        float deley = 2;
        if (isdemo)
        {
            mSound.PlayTipList(new List<string>() { "manyequation_sound", "manyequation_sound", "manyequation_sound", "manyequation_sound", "manyequation_sound","manyequation_sound", "manyequation_sound", "manyequation_sound", "manyequation_sound", "manyequation_sound" }, new List<string>() { mGuanka.maxNum.ToString(), "fencheng", mGuanka.num1.ToString(), "he", mGuanka.num2.ToString(), mGuanka.num1.ToString(), "he", mGuanka.num2.ToString(), "hecheng", mGuanka.maxNum.ToString() });
            yield return new WaitForSeconds(3f);
        }
        else
        {
            deley = 5;
            mSound.PlayTipList(
                GetABnames(),
                getGoSoundList(go, mGuanka.timus.Count - 1),
                getSoundValList()
                );
        }
        Vector3 startScale = Vector3.one * 1.2f;
        Vector3 endScale = Vector3.one * 1.4f;
        //放大
        for (float j = 0; j < 1f; j += 0.02f)
        {
            go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
            yield return new WaitForSeconds(0.01f);
        }
        go.transform.localScale = endScale;
        yield return new WaitForSeconds(1.5f);
        //缩小
        for (float j = 0; j < 1f; j += 0.02f)
        {
            go.transform.localScale = Vector3.Lerp(endScale, startScale, j);
            yield return new WaitForSeconds(0.01f);
        }
        go.transform.localScale = startScale;

        yield return new WaitForSeconds(deley);
        mSound.PlayShort("manyequation_sound", "指针对位正确", 1);

        Vector3 startPos = go.transform.localPosition;
        float endY = startPos.y;
        if(mGuanka.timus.Count == 0)
        {
            endY = startPos.y + 130;
        }
        Vector3 endPos = mGuanka.timuPoss[mGuanka.timus.Count] + new Vector3(33,0,0);//new Vector3(startPos.x, endY, 0);
         startScale = Vector3.one * 1.2f;
         endScale = Vector3.one; //image.rectTransform.localScale +
        for (float j = 0; j < 1f; j += 0.2f)
        {
            go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
            go.transform.localPosition = Vector3.Lerp(startPos, endPos, j);
            yield return new WaitForSeconds(0.01f);
        }
        go.transform.localScale = endScale;
        go.transform.localPosition = endPos;

        
        mGuanka.markedList.Add(new Vector2(mGuanka.num1, mGuanka.num2));
        mGuanka.timus.Add(go);

        playPrefabSuccse();
        /*
        if (go == ques1Go)
        {
            StartCoroutine(TShowQue(ques2Go));
        }
        else if (go == ques2Go)
        {
            StartCoroutine(TShowQue(ques3Go));
        }
        else
        {
            StartCoroutine(TNextGame());
        }
        */
        Debug.Log("mGuanka.timus.Count : " + mGuanka.timus.Count);
        if (mGuanka.timus.Count >= 5)
        {
            StartCoroutine(TNextGame());
        }
        else
        {
            if(mGuanka.timus.Count >= 3)
            {
                mGuanka.type = "drog";
            }
            else
            {
                mGuanka.type = "add";
            }
            //Debug.Log("TSetQue mGuanka.timus.Count : " + mGuanka.timus.Count);
            nextQuestion();
        }
        

    }
    //显示某一道算式题目
    IEnumerator TShowQue(GameObject go)
    {
        go.active = true;
        Vector3 endScale = Vector3.one;
        Vector3 startScale = Vector3.one * 0.5f; //image.rectTransform.localScale +
        for (float j = 0; j < 1f; j += 0.2f)
        {
            go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
            yield return new WaitForSeconds(0.01f);
        }
        go.transform.localScale = endScale;
    }
}
