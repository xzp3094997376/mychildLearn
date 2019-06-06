using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class RabbitGoHomeCtr : MonoBehaviour {
    public static RabbitGoHomeCtr mtransform { get; set; }
    public static bool isWarking { get; set; }
    public Guanka mGuanka = new Guanka();
    public SoundManager mSound { get; set; }
    private RawImage bg { get; set; }
    private RabbitGoHomeSpine rabbitSpine { get; set; }
    private bool inpass { get; set; }
    private List<Vector3> MraodPoss1 = new List<Vector3>(){
        new Vector3( -380f, 96f, 0f),
        new Vector3( -318f, 150f, 0f),
        new Vector3( -265f, 217f, 0f),
        new Vector3( -204f, 271f, 0f),
        new Vector3( -129f, 300f, 0f),
        new Vector3( -43f, 306f, 0f),
        new Vector3( 32f, 292f, 0f),
        new Vector3( 99f, 268f, 0f),
        new Vector3( 174f, 255f, 0f),
        new Vector3( 252f, 260f, 0f),
        new Vector3( 335f, 276f, 0f),
        new Vector3( 418f, 284f, 0f),
        new Vector3( 499f, 260f, 0f),
        new Vector3( 525f, 180f, 0f),
        new Vector3( 504f, 91f, 0f),
        new Vector3( 426f, 24f, 0f),
        new Vector3( 348f, -19f, 0f),
        new Vector3( 276f, -70f, 0f),
        new Vector3( 223f, -144f, 0f),
        new Vector3( 225f, -212f, 0f),
        new Vector3( 292f, -284f, 0f),
        new Vector3( 367f, -335f, 0f)
    };
    private List<Vector3> MraodPoss2 = new List<Vector3>()
    {
        new Vector3( -380f, 96f, 0f),
        new Vector3( -318f, 150f, 0f),
        new Vector3( -225f, 150f, 0f),
        new Vector3( -150f, 110f, 0f),
        new Vector3( -78f, 72f, 0f),
        new Vector3( -3f, 43f, 0f),
        new Vector3( 67f, 40f, 0f),
        new Vector3( 137f, 13f, 0f),
        new Vector3( 193f, -48f, 0f),
        new Vector3( 223f, -144f, 0f),
        new Vector3( 225f, -212f, 0f),
        new Vector3( 292f, -284f, 0f),
        new Vector3( 367f, -335f, 0f)
    };
    private List<Vector3> MraodPoss3 = new List<Vector3>(){
        new Vector3( -480f, -21f, 0f),
        new Vector3( -472f, -104f, 0f),
        new Vector3( -458f, -182f, 0f),
        new Vector3( -391f, -209f, 0f),
        new Vector3( -311f, -220f, 0f),
        new Vector3( -241f, -230f, 0f),
        new Vector3( -163f, -249f, 0f),
        new Vector3( -99f, -292f, 0f),
        new Vector3( -32f, -330f, 0f),
        new Vector3( 43f, -335f, 0f),
        new Vector3( 110f, -303f, 0f),
        new Vector3( 174f, -255f, 0f),
       new Vector3( 225f, -212f, 0f),
        new Vector3( 292f, -284f, 0f),
        new Vector3( 367f, -335f, 0f)
    };
    private List<Vector3> MraodPoss4 = new List<Vector3>(){
        new Vector3( -380f, 96f, 0f),
        new Vector3( -318f, 150f, 0f),
        new Vector3( -225f, 150f, 0f),
        new Vector3( -150f, 110f, 0f),
        new Vector3( -78f, 72f, 0f),
        new Vector3( -48f, -16f, 0f),
        new Vector3( -54f, -99f, 0f),
        new Vector3( -115f, -161f, 0f),
         new Vector3( -163f, -249f, 0f),
        new Vector3( -99f, -292f, 0f),
        new Vector3( -32f, -330f, 0f),
        new Vector3( 43f, -335f, 0f),
        new Vector3( 110f, -303f, 0f),
        new Vector3( 174f, -255f, 0f),
       new Vector3( 225f, -212f, 0f),
        new Vector3( 292f, -284f, 0f),
        new Vector3( 367f, -335f, 0f)
    };
    private List<Vector3> MraodPoss5 = new List<Vector3>(){
        new Vector3( -480f, -21f, 0f),
        new Vector3( -472f, -104f, 0f),
        new Vector3( -458f, -182f, 0f),
        new Vector3( -391f, -209f, 0f),
        new Vector3( -311f, -220f, 0f),
        new Vector3( -241f, -230f, 0f),
        new Vector3( -163f, -249f, 0f),
        new Vector3( -115f, -161f, 0f),
        new Vector3( -54f, -99f, 0f),
        new Vector3( -48f, -16f, 0f),
        new Vector3( -3f, 43f, 0f),
        new Vector3( 67f, 40f, 0f),
        new Vector3( 137f, 13f, 0f),
        new Vector3( 193f, -48f, 0f),
        new Vector3( 223f, -144f, 0f),
        new Vector3( 225f, -212f, 0f),
        new Vector3( 292f, -284f, 0f),
        new Vector3( 367f, -335f, 0f)
    };

    private List<Vector3> raod11 = new List<Vector3>(){
        new Vector3( -204f, 282f, 0f),
        new Vector3( -136f, 305f, 0f),
        new Vector3( -66f, 318f, 0f),
        new Vector3( 1f, 314f, 0f),
        new Vector3( 64f, 295f, 0f),
        new Vector3( 122f, 274f, 0f),
    };
    private List<Vector3> raod12 = new List<Vector3>(){
        new Vector3( 253f, 280f, 0f),
        new Vector3( 316f, 287f, 0f),
        new Vector3( 382f, 294f, 0f),
        new Vector3( 449f, 297f, 0f),
        new Vector3( 501f, 275f, 0f),
        new Vector3( 539f, 231f, 0f),
    };
    private List<Vector3> raod13 = new List<Vector3>(){
        new Vector3( 374f, 1f, 0f),
        new Vector3( 436f, 17f, 0f),
        new Vector3( 477f, 82f, 0f),
        new Vector3( 539f, 96f, 0f),
        new Vector3( 519f, 150f, 0f),
        new Vector3( 562f, 196f, 0f),
    };

    private List<Vector3> raod21 = new List<Vector3>(){
        new Vector3( -251f, 130f, 0f),
        new Vector3( -204f, 111f, 0f),
        new Vector3( -142f, 131f, 0f),
        new Vector3( -93f, 121f, 0f),
        new Vector3( -142f, 131f, 0f),
    };
    private List<Vector3> raod22 = new List<Vector3>(){
        new Vector3( 72f, 40f, 0f),
        new Vector3( 131f, 17f, 0f),
        new Vector3( 164f, -27f, 0f),
        new Vector3( 198f, 22f, 0f),
    };
    private List<Vector3> raod31 = new List<Vector3>(){
        new Vector3( -477f, -10f, 0f),
        new Vector3( -484f, -77f, 0f),
        new Vector3( -501f, -139f, 0f),
        new Vector3( -460f, -181f, 0f),
        new Vector3( -395f, -205f, 0f),
        new Vector3( -331f, -226f, 0f),
    };
    private List<Vector3> raod32 = new List<Vector3>(){
        new Vector3( -83f, -304f, 0f),
        new Vector3( -38f, -326f, 0f),
        new Vector3( 14f, -335f, 0f),
        new Vector3( 74f, -332f, 0f),
        new Vector3( 146f, -295f, 0f),
        new Vector3( 113f, -261f, 0f),
    };
    private List<Vector3> raod42 = new List<Vector3>(){
        new Vector3( -45f, -10f, 0f),
        new Vector3( -12f, -52f, 0f),
        new Vector3( -81f, -88f, 0f),
        new Vector3( -56f, -136f, 0f),
    };
    private List<RaGrid> gridList1 = new List<RaGrid>();
    private List<RaGrid> gridList2 = new List<RaGrid>();
    private List<RaGrid> gridList3 = new List<RaGrid>();
    private List<RaGrid> gridList4 = new List<RaGrid>();
    private List<RaGrid> gridList5 = new List<RaGrid>();

    public class Guanka
    {
        public int guanka { get; set; }
        public int guanka_last { get; set; }
        public int okRoad { get; set; }
        public int times = 0;
        public Dictionary<int, List<int>> numDic = new Dictionary<int, List<int>>();//每一部分创建的萝卜个数
        public Dictionary<List<Vector3>, bool> commDic = new Dictionary<List<Vector3>, bool>();//共用的地点
        public Dictionary<List<Vector3>, float> scaleDic = new Dictionary<List<Vector3>, float>();//每一部分萝卜的角度
        public int partNum = 0;
        List<Vector3> oklist = new List<Vector3>();
        List<Vector3> errlist = new List<Vector3>();
        public bool isFirst { get; set; }
        public Guanka()
        {
            guanka_last = 2;
        }
        public void Set(int _guanka)
        {
            guanka = _guanka;
            //Debug.Log("okRoad : " + okRoad);
            numDic.Clear();
            oklist.Clear();
            errlist.Clear();
            switch (_guanka)
            {
                case 1:
                    partNum = 2;
                    /*
                    for (int i = 1;i < 7; i++)
                    {
                        for (int j = 1; j < 7; j++)
                        {
                            Vector3 vec = new Vector3(i, j, 0);
                            if (i + j == 7)
                            {
                                oklist.Add(vec);
                            }else
                            {
                                errlist.Add(vec);
                            }
                        }
                    }
                    */
                    okRoad = Common.BreakRank(new List<int>() { 1,2,3})[times];
                    break;
                case 2:
                    partNum = 3;
                    /*
                    for (int i = 1; i < 7; i++)
                    {
                        for (int j = 1; j < 7; j++)
                        {
                            for (int z = 1; z < 7; z++)
                            {
                                Vector3 vec = new Vector3(i, j, z);
                                if (i + j + z == 7)
                                {
                                    oklist.Add(vec);
                                }
                                else
                                {
                                    errlist.Add(vec);
                                }
                            }
                        }
                    }
                    */
                    okRoad = Common.BreakRank(new List<int>() { 1, 4, 5 })[times];
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
            Debug.Log("okRoad : " + okRoad);
            //oklist = Common.BreakRank(oklist);
            //errlist = Common.BreakRank(errlist);
            /*
            for(int i = 0;i < 1; i++)
            {
                List<int> list = new List<int>() { 4, 4, 6 };//new List<int>() { (int)oklist[i].x, (int)oklist[i].y, (int)oklist[i].z };
                numDic.Add(okRoad, list);
            }
            for (int i = 0; i < 5; i++)
            {
                List<int> list = new List<int>() { 4, 4, 6 };//new List<int>() { (int)errlist[i].x, (int)errlist[i].y, (int)errlist[i].z };
                int road = i + 1;
                if(road != okRoad)
                {
                    numDic.Add(road, list);
                }
            }
            */

        }
    }
    private Dictionary<List<Vector3>, int> partLuoboNumDic = new Dictionary<List<Vector3>, int>();
    private void setRoadNumList(int road)
    {
        
        List<int> nums = new List<int>();
        List<List<Vector3>> luoboPosList = GetLuoboPosList(road);
        int length = luoboPosList.Count + 1;
        
        if (mGuanka.guanka > 1)
        {
            List<Vector3> list1 = null;
            List<Vector3> list2 = null;
            List<Vector3> list3 = null;
            int num1 = Common.GetRandValue(3, length);
            int num2 = Common.GetRandValue(3, length);
            int num3 = Common.GetRandValue(3, length);
            if(road == mGuanka.okRoad)
            {
                list1 = luoboPosList[0];
                list2 = luoboPosList[1];
                list3 = luoboPosList[2];
                num1 = Random.Range(0, 1000) % 5 + 1;
                num2 = Random.Range(0, 7 - num1) % (7 - num1 - 1) + 1;
                num3 = 7 - num1 - num2;
               while(list1.Count < num1 || list1.Count < num2 || list3.Count < num3)
                {
                    num1 = Random.Range(0, 1000) % 5 + 1;
                    num2 = Random.Range(0, 7 - num1) % (7 - num1 - 1) + 1;
                    num3 = 7 - num1 - num2;
                }
            }else
            {
                if(luoboPosList.Count > 2)
                {
                    list1 = luoboPosList[0];
                    list2 = luoboPosList[1];
                    list3 = luoboPosList[2];
                    
                    while(num1 + num2 + num3 == 7)
                    {
                        num1 = Random.Range(0, 1000) % 4 + 1;
                        num2 = Random.Range(0, 6 - num1) % (6 - num1 - 1) + 1;
                        num3 = 6 - num1 - num2;
                    }
                }else
                {
                    list1 = luoboPosList[0];
                    list2 = luoboPosList[1];
                    if (road == 2)
                    {
                        if (Random.Range(0, 1000) % 2 == 0)
                        {
                            num1 = 4;
                            num2 = 3;
                        }
                        else
                        {
                            num1 = 3;
                            num2 = 4;
                        }
                    }
                    else
                    {
                        num1 = Random.Range(3, 1000) % 4 + 1;
                        num2 = Random.Range(3, 1000) % 4 + 1;
                        while (num1 + num2 == 7)
                        {
                            num1 = Random.Range(0, 1000) % 4 + 1;
                            num2 = Random.Range(0, 1000) % 4 + 1;
                        }
                    }
                }
               
            }
            if (!partLuoboNumDic.ContainsKey(list1))
            {
                partLuoboNumDic.Add(list1, num1);
            }
            if (!partLuoboNumDic.ContainsKey(list2))
            {
                partLuoboNumDic.Add(list2, num2);
            }
            if (null != list3 && !partLuoboNumDic.ContainsKey(list3))
            {
                partLuoboNumDic.Add(list3, num3);
            }
            //Debug.Log("road:" + road + ";num1 : " + num1 + ";num2 : " + num2 + ";num3 : " + num3);
        }
        else
        {
            List<Vector3> list1 = luoboPosList[0];
            List<Vector3> list2 = luoboPosList[1];
            int num1 = Common.GetRandValue(3, length);
            int num2 = Common.GetRandValue(3, length);
            if (road == mGuanka.okRoad)
            {
                if (road == 2)
                {
                    if(Random.Range(0, 1000) % 2 == 0)
                    {
                        num1 = 4;
                        num2 = 3;
                    }else
                    {
                        num1 = 3;
                        num2 = 4;
                    }
                }else
                {
                    num1 = Random.Range(0, 1000) % 6 + 1;
                    num2 = 7 - num1;
                }
            }
            else
            {
                num1 =  Random.Range(3, 1000) % 4 + 1;
                num2 = Random.Range(3, 1000) % 4 + 1;
                while(num1 + num2 == 7)
                {
                    num1 = Random.Range(0, 1000) % 4 + 1;
                    num2 = Random.Range(0, 1000) % 4 + 1;
                }
            }

            if (!partLuoboNumDic.ContainsKey(list1))
            {
                partLuoboNumDic.Add(list1, num1);
            }
            if (!partLuoboNumDic.ContainsKey(list2))
            {
                partLuoboNumDic.Add(list2, num2);
            }
            
        }
        
        /*
        List<Vector3> oklist = new List<Vector3>();
        List<Vector3> errlist = new List<Vector3>();
        for (int i = 0;i < luoboPosList.Count; i++)
        {
            List<Vector3> list = luoboPosList[i];
            int leng = list.Count + 1;
            Debug.Log("list.Count : " + list.Count);
            if (mGuanka.guanka == 1)
            {
                for (int n = 1; n < leng; n++)
                {
                    for (int j = 1; j < leng; j++)
                    {
                        Vector3 vec = new Vector3(n, j, 0);
                        if (n + j == 7)
                        {
                            oklist.Add(vec);
                        }
                        else
                        {
                            errlist.Add(vec);
                        }
                    }
                }
               
            }else
            {
                for (int n = 1; n < leng; n++)
                {
                    for (int j = 1; j < leng; j++)
                    {
                        for (int z = 1; z < leng; z++)
                        {
                            Vector3 vec = new Vector3(n, j, z);
                            if (n + j + z == 7)
                            {
                                oklist.Add(vec);
                            }
                            else
                            {
                                errlist.Add(vec);
                            }
                        }

                    }
                }
                
            }
            oklist = Common.BreakRank(oklist);
            errlist = Common.BreakRank(errlist);
            List<int> listuse = new List<int>() { (int)oklist[0].x, (int)oklist[0].y, (int)oklist[0].z };
            List<int> listuse2 = new List<int>() { (int)errlist[0].x, (int)errlist[0].y, (int)errlist[0].z };
        }
        
        if (road == mGuanka.okRoad)
        {
            List<int> listuse = new List<int>() { (int)oklist[0].x, (int)oklist[0].y, (int)oklist[0].z };
            mGuanka.numDic.Add(road, listuse);
        }
        else
        {
            //Debug.Log("setRoadNumList road : " + road);
            List<int> listuse2 = new List<int>() { (int)errlist[0].x, (int)errlist[0].y, (int)errlist[0].z };
            mGuanka.numDic.Add(road, listuse2);
        }
        */
    }
    void Awake()
    {
        mSound = gameObject.AddComponent<SoundManager>();
        mtransform = this;
    }
    // Use this for initialization
    void Start()
    {
        bg = UguiMaker.newRawImage("bg", transform, "rabbitgohome_texture", "bg", false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        mSound.PlayBgAsync("bgmusic_loop3", "bgmusic_loop3", 0.3f);

        mGuanka.commDic.Add(raod21, false);
        mGuanka.commDic.Add(raod22, false);
        mGuanka.commDic.Add(raod31, false);
        mGuanka.commDic.Add(raod32, false);
        mGuanka.commDic.Add(raod42, false);

        //mGuanka.scaleDic.Add(raod13, -60);
        //mGuanka.scaleDic.Add(raod22, 150);
        //mGuanka.scaleDic.Add(raod31, 150);
        //mGuanka.scaleDic.Add(raod32, 180);
        //mGuanka.scaleDic.Add(raod42, -60);

        setGameGuanka(1);
    }

    // Update is called once per frame
    private Image line = null;
    private Image startImage = null;
    List<Vector3> roadPoss = new List<Vector3>();
    private Dictionary<int, int> Eventdic = new Dictionary<int, int>();//<路线，事件节点>
    void Update () {

        if (isWarking || inpass) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("GetMouseButtonDown");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            Eventdic.Clear();
            if (null != hits)
            {
                startImage = null;
                line = null;
                foreach (RaycastHit hit in hits)
                {
                    Image com = hit.collider.gameObject.GetComponent<Image>();
                    if(null != com)
                    {
                        //画线
                        /*
                        startImage = com;
                        line = UguiMaker.newGameObject("line", transform).AddComponent<Image>();
                        line.sprite = ResManager.GetSprite("rabbitgohome_sprite", "rwaline");
                        line.rectTransform.sizeDelta = new Vector2(5, 50);
                        line.type = Image.Type.Tiled;
                        line.rectTransform.pivot = new Vector2(0, 0.5f);
                        line.transform.localPosition = com.transform.localPosition;
                        */

                        /*
                        if(com.name == "endPoint")
                        {
                            string str = "new List<Vector3>(){\n";
                            for (int i = 0;i < roadPoss.Count;i++)
                            {
                                Vector3 pos = roadPoss[i];
                                str += "new Vector3( " + pos.x + "f, " + pos.y + "f, " + pos.z + "f),\n";
                            }
                            str += "};";
                            Debug.LogError(str);
                        }else
                        {
                            string[] arr = com.name.Split('_');
                            if (arr.Length > 1 && arr[1] != "gezi")
                            {
                                com.color = Color.black;
                                roadPoss.Add(com.transform.localPosition);
                            }
                                
                        }
                        //*/

                        ///*
                        string[] arr = com.name.Split('_');
                        if(arr.Length > 1 && arr[1] == "gezi")
                        {
                            Vector3 endPos = com.transform.localPosition;
                            string roadIndex = arr[0];
                            string gridIndex = com.name.Split('_')[2];
                            if (!Eventdic.ContainsKey(int.Parse(roadIndex)))
                            {
                                Eventdic.Add(int.Parse(roadIndex), int.Parse(gridIndex));
                            }
                            else
                            {
                                Eventdic[int.Parse(roadIndex)] = int.Parse(gridIndex);
                            }
                            //Invoke("EventGo", 0.5f);
                            EventGo();
                        }
                        //*/
                        
                    }
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (null != startImage)
            {
                Vector3 pos = Common.getMouseLocalPos(transform);
                float dis = Vector3.Distance(startImage.transform.localPosition, pos);
                line.rectTransform.sizeDelta = new Vector2(dis, 50);
                float angle = Mathf.Atan2(pos.y - startImage.transform.localPosition.y, pos.x - startImage.transform.localPosition.x) * (180 / Mathf.PI);
                line.rectTransform.localEulerAngles = new Vector3(0, 0, angle);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            if (null != hits)
            {
                Image com = null;
                foreach (RaycastHit hit in hits)
                {
                    com = hit.collider.gameObject.GetComponent<Image>();
                }

                if (null != startImage && null != line && null != com)
                {
                    float dis = Vector3.Distance(com.transform.localPosition, startImage.transform.localPosition);
                    line.rectTransform.sizeDelta = new Vector2(dis, 50);
                    float angle = Mathf.Atan2(com.transform.localPosition.y - startImage.transform.localPosition.y, com.transform.localPosition.x - startImage.transform.localPosition.x) * (180 / Mathf.PI);
                }
            }
            startImage = null;
            line = null;
        }
    }
    private int currRoad { get; set; }
    //点击路径延迟执行
    private void EventGo()
    {
        List<Vector3> posList = null;
        //Debug.Log("EventGo Eventdic.Count : " + Eventdic.Count);
        if (Eventdic.Count > 1)
        {
            //Debug.Log("isAllNull(dic) : " + isAllNull(Eventdic));
            for (int i = 1; i < 6; i++)
            {
                List<RaGrid> grids = getGrids(i.ToString());
                for (int j = 0; j < grids.Count; j++)
                {
                    RaGrid grid = grids[j];
                    grid.gameObject.GetComponent<BoxCollider>().enabled = false;
                }
            }
                
            foreach (int road in Eventdic.Keys)
            {
                int node = Eventdic[road];
                List<RaGrid> grids = getGrids(road.ToString());
                posList = getGridsPos(grids, node, mGuanka.isFirst);
                for (int j = 0; j < grids.Count; j++)
                {
                    RaGrid grid = grids[j];
                    grid.gameObject.GetComponent<BoxCollider>().enabled = true;
                }

            }
        }
        else
        {
            for (int i = 1; i < 6; i++)
            {
                List<RaGrid> grids = getGrids(i.ToString());
                for (int j = 0; j < grids.Count; j++)
                {
                    RaGrid grid = grids[j];
                    grid.gameObject.GetComponent<BoxCollider>().enabled = false;
                }
            }
            foreach (int road in Eventdic.Keys)
            {
                int node = Eventdic[road];
                List<RaGrid> grids = getGrids(road.ToString());
                posList = getGridsPos(grids, node, mGuanka.isFirst);
                for (int j = 0; j < grids.Count; j++)
                {
                    RaGrid grid = grids[j];
                    grid.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
            }
            
        }
        //Debug.Log("posList.Count : " + posList.Count);
        if(null != posList && posList.Count > 0)
        {
            //rabbitSpine.MoveTo(posList);
        }
        
    }
    //获取有用的字典
    private Dictionary<int, List<RaGrid>> getUseDic(Dictionary<int, List<RaGrid>> dic)
    {
        Dictionary<int, List<RaGrid>> outdic = new Dictionary<int, List<RaGrid>>();
        foreach (int key in dic.Keys)
        {
            bool ispath = false;
            List<RaGrid> grids = dic[key];
            for (int i = 0; i < grids.Count; i++)
            {
                RaGrid grid = grids[i];
                if (grid.isPath)
                {
                    ispath = true;
                }
            }
            if (ispath)
            {
                outdic.Add(key, grids);
            }
        }
        return outdic;
    }
    //判断是否重叠的部分都是没有走过的
    private bool isAllNull(Dictionary<int,int> dic)
    {
        bool boo = true;
        foreach(int road in dic.Keys)
        {
            bool ispath = false;
            int node = dic[road];
            List<RaGrid> grids = getGrids(road.ToString());
            for(int i = 0;i< grids.Count; i++)
            {
                RaGrid grid = grids[i];
                if (grid.isPath)
                {
                    ispath = true;
                    break;
                }
            }
            if (ispath)
            {
                boo = false;
                break;
            }
        }
        return boo;
    }
    //索取某个路上的节点列表
    private List<RaGrid> getGrids(string roadStr)
    {
        List<RaGrid> grids = null;
        switch (roadStr)
        {
            case "1":
                grids = gridList1;
                break;
            case "2":
                grids = gridList2;
                break;
            case "3":
                grids = gridList3;
                break;
            case "4":
                grids = gridList4;
                break;
            case "5":
                grids = gridList5;
                break;
        }
        return grids;
    }
    private List<Vector3> getGridsPos(List<RaGrid> list,int EventIndex,bool isFirst)
    {
        int limitNum = 5;
        if (!isFirst)
        {
            limitNum = 8;
        }
        int leng = EventIndex + 1;
        //Debug.Log("getGridsPos EventIndex : " + EventIndex);
        List<RaGrid> stampGrids = new List<RaGrid>();
        List<Vector3> outlist = new List<Vector3>();
        for (int i = 0; i < leng; i++)
        {
            RaGrid grid = list[i];
            //Debug.Log("getGridsPos i : " + i + ";grid.isPath : " + grid.isPath);
            if (!grid.isPath)
            {
                outlist.Add(grid.transform.localPosition);
                stampGrids.Add(grid);
            }
        }
        if(stampGrids.Count <= 6)
        {
            for (int i = 0; i < stampGrids.Count; i++)
            {
                RaGrid grid = stampGrids[i];
                //grid.isPath = true;
            }
        }
        else
        {
            outlist.Clear();
        }
        if (mGuanka.isFirst)
        {
            mGuanka.isFirst = false;
        }
        return outlist;
    }
    //选择错误的路段
    public void ErrRoad()
    {
        setGameData();
    }
    //重玩
    private void reGame()
    {
        setGameGuanka(1);
    }
    //增加关卡
    private void setGameGuanka(int guanka)
    {
        if (guanka == 1)
        {
            TopTitleCtl.instance.Reset();
        }
        else
        {
            TopTitleCtl.instance.AddStar();
        }
        resetGuankanData(guanka);
        setGameData();
    }
    private void resetGuankanData(int guanka)
    {
        mGuanka.Set(guanka);
        partLuoboNumDic.Clear();
        setRoadNumList(mGuanka.okRoad);
        for (int i = 1; i < 6; i++)
        {
            if (i != mGuanka.okRoad)
            {
                setRoadNumList(i);
            }
        }
    }
    //设置下一关或者是再来一次
    public void NextGame()
    {
        inpass = true;
        StartCoroutine(TNextGame());
    }
    private GameObject luoboGo { get; set; }
    private GameObject nodeGo { get; set; }
    private Image endPoint { get; set; }
    //设置游戏数据
    private void setGameData()
    {
        ///*
        if(null == endPoint)
        {
            endPoint = UguiMaker.newImage("endPoint", transform, "rabbitgohome_sprite", "Exit");
            endPoint.transform.localScale = Vector3.one;
            endPoint.transform.localPosition = new Vector3(388, -351, 0);
            BoxCollider box = endPoint.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(50, 50, 0);
            endPoint.transform.localEulerAngles = new Vector3(0,0,-120);
        }
       inpass = false;
        mGuanka.isFirst = true;
        Dictionary<List<Vector3>, bool> buffer = new Dictionary<List<Vector3>, bool>();
        foreach (List<Vector3> list in mGuanka.commDic.Keys)
        {
            buffer.Add(list, false);
        }
        mGuanka.commDic = buffer;

        if (null != luoboGo)
        {
            GameObject.Destroy(luoboGo);
        }
        luoboGo = UguiMaker.newGameObject("luoboGo", transform);
       
       // /*
        for (int i = 1;i < 6; i++)
        {
           
            //Debug.Log("road " + i + "listvec : " + mGuanka.numDic[i].Count);
            createRoadLuobo(i);//mGuanka.numDic[i]
        }
        //*/
        //setRoadNumList(1);
        //createRoadLuobo(1, mGuanka.numDic[1]);

        if (null == nodeGo)
        {
            nodeGo = UguiMaker.newGameObject("nodeGo", transform);
            createRoad(1, nodeGo.transform, MraodPoss1, gridList1);
            createRoad(2, nodeGo.transform, MraodPoss2, gridList2);
            createRoad(3, nodeGo.transform, MraodPoss3, gridList3);
            createRoad(4, nodeGo.transform, MraodPoss4, gridList4);
            createRoad(5, nodeGo.transform, MraodPoss5, gridList5);
        }else
        {
            resetAllGrid(gridList1);
            resetAllGrid(gridList2);
            resetAllGrid(gridList3);
            resetAllGrid(gridList4);
            resetAllGrid(gridList5);
        }
        /*
        Image bgezi = UguiMaker.newImage("gezi_b", nodeGo.transform, "public", "white");
        bgezi.rectTransform.sizeDelta = new Vector2(50, 50);
        Vector3 bpos = new Vector3(-560, 145, 0);// + new Vector3(i * (gezisize + 1), -j * (gezisize + 1), 0);
        bgezi.transform.localPosition = bpos;
        Color bcolor = bgezi.color;
        //color.a = 0f;
        bgezi.color = bcolor;
        BoxCollider bgeziBox = bgezi.gameObject.AddComponent<BoxCollider>();
        bgeziBox.isTrigger = false;
        bgeziBox.size = new Vector3(50, 50, 0);
        //*/

        

        if(null == rabbitSpine)
        {
            rabbitSpine = ResManager.GetPrefab("rabbitgohome_prefab", "rabbitgohome").GetComponent<RabbitGoHomeSpine>();
            GameObject ribbitGo = UguiMaker.InitGameObj(rabbitSpine.gameObject, transform, "rabbit", new Vector3(-496, 73, 0), Vector3.one * 0.2f);
            Canvas canvas = ribbitGo.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 5;
            BoxCollider spineBox = rabbitSpine.gameObject.AddComponent<BoxCollider>();
            spineBox.isTrigger = true;
            Rigidbody rig = rabbitSpine.gameObject.AddComponent<Rigidbody>();
            rig.isKinematic = true;
            spineBox.size = new Vector3(200, 200, 0);
            spineBox.center = new Vector3(0, 150, 0);
        }else
        {
            rabbitSpine.transform.localPosition = new Vector3(-496, 73, 0);
            rabbitSpine.luoboNum = 0;
        }
        BoxCollider endBox = endPoint.gameObject.GetComponent<BoxCollider>();
        endBox.enabled = true;

        rabbitSpine.PlaySpine("Idle", true);
        isWarking = false;
    }
    //创建每条路上多少个萝卜
    private float luobosacle = 0.6f;
    private void createRoadLuobo(int road)//List<int> NumList
    {
        //Debug.Log("road : " + road + ";okroad : " + mGuanka.okRoad);
        List<List<Vector3>> list = GetLuoboPosList(road);//
        List<List<Vector3>> oklist = GetLuoboPosList(mGuanka.okRoad);//
        int leng = mGuanka.partNum;
        //Debug.Log("leng : " + leng + ";okroad : " + mGuanka.okRoad);
        if (mGuanka.guanka == 2)
        {
            leng = list.Count;
        }
        for (int i = 0; i < leng; i++)
        {
            List<Vector3> posss = list[i];
            float AngelNum = 0;
            if (mGuanka.scaleDic.ContainsKey(posss))
            {
                AngelNum = mGuanka.scaleDic[posss];
            }
            
            if (road == mGuanka.okRoad)
            {
                if (mGuanka.commDic.ContainsKey(posss))
                {
                    if (!mGuanka.commDic[posss])
                    {
                        int len = partLuoboNumDic[posss];// NumList[i];
                        for (int j = 0; j < len; j++)
                        {
                            Image luobo = UguiMaker.newImage("luobo_" + road + "_" + i + "_" + j, luoboGo.transform, "rabbitgohome_sprite", "luobo");
                            luobo.transform.localScale = Vector3.one * luobosacle;
                            luobo.transform.localEulerAngles = new Vector3(0,0, AngelNum);
                            luobo.transform.localPosition = posss[j];
                            BoxCollider nodeBox = luobo.gameObject.AddComponent<BoxCollider>();
                            nodeBox.isTrigger = true;
                            LuoBoItem item = luobo.gameObject.AddComponent<LuoBoItem>();
                            nodeBox.size = new Vector3(100, 100, 0);
                        }
                        mGuanka.commDic[posss] = true;
                    }
                }
                else
                {
                    int len = partLuoboNumDic[posss];// NumList[i];
                    for (int j = 0; j < len; j++)
                    {
                        Image luobo = UguiMaker.newImage("luobo_" + road + "_" + i + "_" + j, luoboGo.transform, "rabbitgohome_sprite", "luobo");
                        luobo.transform.localScale = Vector3.one * luobosacle;
                        luobo.transform.localEulerAngles = new Vector3(0, 0, AngelNum);
                        luobo.transform.localPosition = posss[j];
                        BoxCollider nodeBox = luobo.gameObject.AddComponent<BoxCollider>();
                        nodeBox.isTrigger = true;
                        nodeBox.size = new Vector3(100, 100, 0);
                    }

                }
            }
            else
            {
                if (!oklist.Contains(posss))
                {
                    if (mGuanka.commDic.ContainsKey(posss))
                    {
                        if (!mGuanka.commDic[posss])
                        {
                            int len = partLuoboNumDic[posss];//NumList[i];
                            for (int j = 0; j < len; j++)
                            {
                                Image luobo = UguiMaker.newImage("luobo_" + road + "_" + i + "_" + j, luoboGo.transform, "rabbitgohome_sprite", "luobo");
                                luobo.transform.localScale = Vector3.one * luobosacle;
                                luobo.transform.localEulerAngles = new Vector3(0, 0, AngelNum);
                                luobo.transform.localPosition = posss[j];
                                BoxCollider nodeBox = luobo.gameObject.AddComponent<BoxCollider>();
                                nodeBox.isTrigger = true;
                                nodeBox.size = new Vector3(100, 100, 0);
                            }
                            mGuanka.commDic[posss] = true;
                        }
                    }
                    else
                    {
                        int len = partLuoboNumDic[posss];//NumList[i];
                        for (int j = 0; j < len; j++)
                        {
                            Image luobo = UguiMaker.newImage("luobo_" + road + "_" + i + "_" + j, luoboGo.transform, "rabbitgohome_sprite", "luobo");
                            luobo.transform.localScale = Vector3.one * luobosacle;
                            luobo.transform.localEulerAngles = new Vector3(0, 0, AngelNum);
                            luobo.transform.localPosition = posss[j];
                            BoxCollider nodeBox = luobo.gameObject.AddComponent<BoxCollider>();
                            nodeBox.isTrigger = true;
                            nodeBox.size = new Vector3(100, 100, 0);
                        }

                    }
                }
            }
            
            
        }
    }
    private void resetAllGrid(List<RaGrid> list)
    {
        for(int i = 0;i < list.Count; i++)
        {
            RaGrid grid = list[i];
            grid.isPath = false;
            BoxCollider nodeBox = grid.gameObject.GetComponent<BoxCollider>();
            nodeBox.enabled = true;
        }
    }
    //private List<RaGrid> allcreateGrid = new List<RaGrid>();
    private void createRoad(int roadIndex,Transform parent,List<Vector3> poss,List<RaGrid> list)
    {
        float gezisize = 50f;
        for (int i = 0; i < poss.Count; i++)
        {
            Image gezi = UguiMaker.newImage(roadIndex + "_gezi_" + i, parent, "public", "white");
            RaGrid grid = gezi.gameObject.AddComponent<RaGrid>();
            grid.isPath = false;
            grid.mID = i;
            gezi.rectTransform.sizeDelta = new Vector2(gezisize, gezisize);
            Vector3 pos = poss[i];
            gezi.transform.localPosition = pos;
            Color color = gezi.color;
            color.a = 0f;
            gezi.color = color;
            BoxCollider geziBox = gezi.gameObject.AddComponent<BoxCollider>();
            geziBox.size = new Vector3(gezisize + 20, gezisize + 50, 0);
            geziBox.isTrigger = false;
            list.Add(grid);
            //allcreateGrid.Add(grid);
        }
    }

    private List<List<Vector3>> GetLuoboPosList(int road)
    {
        List<List<Vector3>> list = new List<List<Vector3>>();
        switch (road)
        {
            case 1:
                list.Add(raod11);
                list.Add(raod12);
                list.Add(raod13);
                break;
            case 2:
                list.Add(raod21);
                list.Add(raod22);
                break;
            case 3:
                list.Add(raod31);
                list.Add(raod32);
                break;
            case 4:
                list.Add(raod21);
                list.Add(raod42);
                list.Add(raod32);
                break;
            case 5:
                list.Add(raod31);
                list.Add(raod42);
                list.Add(raod22);
                break;
        }
        return list;
    }

    IEnumerator TNextGame()
    {
        yield return new WaitForSeconds(2f);
        mGuanka.times++;
        Debug.Log("mGuanka.times : " + mGuanka.times);
        if (mGuanka.times >= 2)
        {
            mGuanka.guanka++;
            mGuanka.times = 0;
            if (mGuanka.guanka > mGuanka.guanka_last)
            {
                TopTitleCtl.instance.AddStar();
                yield return new WaitForSeconds(2f);
                GameOverCtl.GetInstance().Show(mGuanka.guanka_last, reGame);
            }
            else
            {
                setGameGuanka(mGuanka.guanka);
            }
            
        }
        else
        {
            resetGuankanData(mGuanka.guanka);
            setGameData();
        }
    }

}
