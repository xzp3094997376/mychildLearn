using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class KengRabbitGoHomeCtr : MonoBehaviour
{
    public static KengRabbitGoHomeCtr mtransform { get; set; }
    
    public static bool isWarking { get; set; }
    public static bool isBacking { get; set; }
    public Guanka mGuanka = new Guanka();
    public MapInfo mapData = new MapInfo();
    public SoundManager mSound { get; set; }
    private RawImage bg { get; set; }
    private Image mDoor { get; set; }
    private Image mPupDoor { get; set; }
    private ParticleSystem mEffectLuobo { get; set; }

    private RabbitGoHomeSpine rabbitSpine { get; set; }
    private ShiZiLu lastShizilu { get; set; }
    private bool inpass { get; set; }
    private bool isFull { get; set; }
    private List<Vector3> raod11 { get; set; }
    private List<Vector3> raod12 { get; set; }
    private List<Vector3> raod13 { get; set; }
    private List<Vector3> raod21 { get; set; }
    private List<Vector3> raod22 { get; set; }
    private List<Vector3> raod31 { get; set; }
    private List<Vector3> raod32 { get; set; }
    private List<Vector3> raod42 { get; set; }
    private List<RaGrid> gridList1 = new List<RaGrid>();
    private List<RaGrid> gridList2 = new List<RaGrid>();
    private List<RaGrid> gridList3 = new List<RaGrid>();
    private List<RaGrid> gridList4 = new List<RaGrid>();
    private List<RaGrid> gridList5 = new List<RaGrid>();

    private GameObject GameContain { get; set; }

    public class Guanka
    {
        public int guanka { get; set; }
        public int guanka_last { get; set; }
        public int okRoad { get; set; }
        public int times = 0;
        public Dictionary<int, List<int>> numDic = new Dictionary<int, List<int>>();//每一部分创建的萝卜个数
        public Dictionary<List<Vector3>, bool> commDic = new Dictionary<List<Vector3>, bool>();//共用的地点
        //public Dictionary<List<Vector3>, float> scaleDic = new Dictionary<List<Vector3>, float>();//每一部分萝卜的角度
        public Dictionary<string, Vector3> shiziluPosList = new Dictionary<string, Vector3>();
        public Dictionary<string, float> shiziluScaleList = new Dictionary<string, float>(); 
        public Dictionary<string, ShiZiLu> shiziluDic = new Dictionary<string, ShiZiLu>();
        public int partNum = 0;
        List<Vector3> oklist = new List<Vector3>();
        List<Vector3> errlist = new List<Vector3>();
        //public bool isFirst { get; set; }
        public string lastshizilu { get; set; }
        public Vector3 homePos { get; set; }
        public bool isFinish { get; set; }
        public int map { get; set; }
        List<int> okList { get; set; }
        public Guanka()
        {
            guanka_last = 2;
        }
        public void Set(int _guanka,MapInfo _mapdata)
        {
            guanka = _guanka;
            map = _mapdata.getMap(guanka + "_" + times);
            homePos = _mapdata.getHomePos(map);
            okList = _mapdata.getOkRoadList(map, _guanka);
            resetData(_guanka, _mapdata);

            switch (_guanka)
            {
                case 1:
                    partNum = 2;
                    break;
                case 2:
                    partNum = 3;
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
            
        }
        public void resetData(int _guanka,MapInfo _mapdata)
        {
            numDic.Clear();
            oklist.Clear();
            errlist.Clear();
            shiziluDic.Clear();
            map = _mapdata.getMap(guanka + "_" + times);
            shiziluPosList.Clear();
            shiziluScaleList.Clear();
            shiziluPosList = _mapdata.GetShiZiLuPosDic(map);
            shiziluScaleList = _mapdata.GetShiZiLuScaleDic(map);
            lastshizilu = _mapdata.getLastShiZiLuName(map);
            okRoad = okList[times];
            //Debug.Log("okRoad : " + okRoad);
        }
        /*
        public void cleanmarked()
        {
            numDic.Clear();
            oklist.Clear();
            errlist.Clear();
            shiziluDic.Clear();
        }
        */
    }
    //设置每一个路径对应的萝卜数量
    private Dictionary<List<Vector3>, int> partLuoboNumDic = new Dictionary<List<Vector3>, int>();
    private void setRoadNumList(int road)
    {
        List<int> nums = new List<int>();
        List<List<Vector3>> luoboPosList = GetLuoboPosList(road,mGuanka.map);
        int length = luoboPosList.Count + 1;

        if (mGuanka.guanka > 1)
        {
            List<Vector3> list1 = null;
            List<Vector3> list2 = null;
            List<Vector3> list3 = null;
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            if (road == mGuanka.okRoad)
            {
                list1 = luoboPosList[0];
                list2 = luoboPosList[1];
                list3 = luoboPosList[2];
                int leng1 = list1.Count + 1;
                int leng2 = list2.Count + 1;
                int leng3 = list3.Count + 1;
                List<Vector3> stampVexs = new List<Vector3>();
                for(int x = 1; x < leng1; x++)
                {
                    for (int y = 1; y < leng2; y++)
                    {
                        for (int z = 1; z < leng3; z++)
                        {
                            if(x + y + z == 7)
                            {
                                stampVexs.Add(new Vector3(x, y, z));
                            }
                        }
                    }
                }
                Vector3 vec = stampVexs[Random.Range(0, 1000) % stampVexs.Count];
                num1 = (int)vec.x;
                num2 = (int)vec.y;
                num3 = (int)vec.z;

            }
            else
            {
                if (luoboPosList.Count > 2)
                {
                    list1 = luoboPosList[0];
                    list2 = luoboPosList[1];
                    list3 = luoboPosList[2];

                    int leng1 = list1.Count + 1;
                    int leng2 = list2.Count + 1;
                    int leng3 = list3.Count + 1;
                    List<Vector3> stampVexs = new List<Vector3>();
                    for (int x = 0; x < leng1; x++)
                    {
                        for (int y = 0; y < leng2; y++)
                        {
                            for (int z = 0; z < leng3; z++)
                            {
                                if (x + y + z != 7)
                                {
                                    stampVexs.Add(new Vector3(x, y, z));
                                }
                            }
                        }
                    }
                    Vector3 vec = stampVexs[Random.Range(0, 1000) % stampVexs.Count];
                    num1 = (int)vec.x;
                    num2 = (int)vec.y;
                    num3 = (int)vec.z;
                }else
                {
                    list1 = luoboPosList[0];
                    list2 = luoboPosList[1];
                    List<int> temp = new List<int>();
                    for (int i = 0; i < list1.Count; i++)
                    {
                        int temp1 = i + 1;
                        int temp2 = 7 - temp1;
                        if (temp2 > 0 && temp2 <= list2.Count)
                        {
                            temp.Add(temp1);
                        }
                    }
                    num1 = temp[Random.Range(0, 1000) % temp.Count];
                    num2 = 7 - num1;
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
            //Debug.Log("road:" + road  + ";mGuanka.okRoad :" + mGuanka.okRoad);
            List<Vector3> list1 = luoboPosList[0];
            List<Vector3> list2 = luoboPosList[1];
            int num1 = Common.GetRandValue(1, list1.Count);
            int num2 = Common.GetRandValue(1, list2.Count);
           
            if (road == mGuanka.okRoad)
            {
                List<int> temp = new List<int>();
                for(int i = 0; i < list1.Count; i++)
                {
                    int temp1 = i + 1;
                    int temp2 = 7 - temp1;
                    if(temp2 > 0 && temp2 <= list2.Count)
                    {
                        temp.Add(temp1);
                    }
                }
                num1 = temp[Random.Range(0, 1000) % temp.Count];
                num2 = 7 - num1;
                //Debug.Log("road:" + road + ";num1 : " + num1 + ";num2 : " + num2 + ";mGuanka.okRoad :" + mGuanka.okRoad);
            }
            else
            {
                List<int> temp = new List<int>();
                for (int i = 0; i < list1.Count; i++)
                {
                    int temp1 = i + 1;
                    int temp2 = 7 - temp1;
                    if (temp2 > 0 && temp2 <= list2.Count)
                    {
                        temp.Add(temp1);
                    }
                }
                num1 = temp[Random.Range(0, 1000) % temp.Count];
                num2 = 7 - num1;
                //Debug.Log("road:" + road + ";num1 : " + num1 + ";num2 : " + num2 + ";mGuanka.okRoad : no");
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
    }
    void Awake()
    {
        mSound = gameObject.AddComponent<SoundManager>();
        mtransform = this;
        
    }
    // Use this for initialization
    void Start()
    {
        
        string bgName = "bg_" + Common.GetRandValue(2, 3);
        //map = 1;// Common.GetRandValue(1, 3);
        bg = UguiMaker.newRawImage("bg", transform, "rabbitgohome_texture", bgName, false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        mSound.PlayBgAsync("bgmusic_loop3", "bgmusic_loop3", 0.3f);

        if (null == GameContain)
        {
            GameContain = UguiMaker.newGameObject("GameContain", transform);
        }

        setGameGuanka(1);
        resetLuoboData();
        setGameData(true);
    }
    /*
    IEnumerator TCreateView()
    {
        yield return new WaitForSeconds(0.001f);

        List<string> roadPartList = mapData.getPartName(mGuanka.map);
        for (int i = 0; i < roadPartList.Count; i++)
        {
            setRoadPartList(roadPartList[i]);
        }
        List<string> comPartList = mapData.getComPartName(mGuanka.map);
        for (int i = 0; i < comPartList.Count; i++)
        {
            setComPartList(comPartList[i]);
        }
        setGameGuanka(1);
    }
    */
    //这只所有段落的数据(坐标）
    private void setRoadPartList(string namestr)
    {
        int map = mGuanka.map;
        switch (namestr)
        {
            case "11":
                raod11 = mapData.GetLuoboPosList(map, "11");
                break;
            case "12":
                raod12 = mapData.GetLuoboPosList(map, "12");
                break;
            case "13":
                raod13 = mapData.GetLuoboPosList(map, "13");
                break;
            case "21":
                raod21 = mapData.GetLuoboPosList(map, "21");
                break;
            case "22":
                raod22 = mapData.GetLuoboPosList(map, "22");
                break;
            case "31":
                raod31 = mapData.GetLuoboPosList(map, "31");
                break;
            case "32":
                raod32 = mapData.GetLuoboPosList(map, "32");
                break;
            case "42":
                raod42 = mapData.GetLuoboPosList(map, "42");
                break;
        }
    }
    //设置那些是公共部分
    private void setComPartList(string namestr)
    {
        switch (namestr)
        {
            case "21":
                mGuanka.commDic.Add(raod21, false);
                break;
            case "22":
                mGuanka.commDic.Add(raod22, false);
                break;
            case "13":
                mGuanka.commDic.Add(raod13, false);
                break;
            case "31":
                mGuanka.commDic.Add(raod31, false);
                break;
            case "32":
                mGuanka.commDic.Add(raod32, false);
                break;
            case "42":
                mGuanka.commDic.Add(raod42, false);
                break;
        }
    }
    // Update is called once per frame
    private Image line = null;
    private Image startImage = null;
    List<Vector3> roadPoss = new List<Vector3>();
    private Dictionary<int, int> Eventdic = new Dictionary<int, int>();//<路线，事件节点>
    void Update()
    {
        if (isWarking || inpass) return;
        //Debug.Log( "isWarking=" + isWarking + "  " + inpass);

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("GetMouseButtonDown");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            Eventdic.Clear();
            if (null != hits)
            {
                startImage = null;
                line = null;
                bool isHitGrid = false;
                ShiZiLu shizilu = null;
                ShiZiLu.gSlect = null;
                RaGrid.gSlect = null;
                foreach (RaycastHit hit in hits)
                {
                    RaGrid com = hit.collider.gameObject.GetComponent<RaGrid>();
                    /*
                    Image posImage = hit.collider.gameObject.GetComponent<Image>();
                    if(null != posImage)
                    {
                        if (posImage.name == "endPoint")
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
                            if (arr.Length > 1 && arr[1] != "gezi")
                            {
                                posImage.color = Color.black;
                                roadPoss.Add(posImage.transform.localPosition);
                            }

                        }
                    }
                    //*/
                    if (null != com && !com.isPath)
                    {
                        ///*
                        string[] arr = com.name.Split('_');
                        if (arr.Length > 1 && arr[1] == "gezi")
                        {
                            RaGrid.gSlect = com;
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
                            
                        }
                        isHitGrid = true;
                        //*/
                    }
                    ///*
                    Image exit = hit.collider.gameObject.GetComponent<Image>();
                    if(exit == endPoint && mGuanka.isFinish)
                    {
                        List<Vector3> list = new List<Vector3>();
                        list.Add(mGuanka.homePos);
                        OpenDoor();
                        rabbitSpine.MoveTo(mapData.getIntoHomePos(mGuanka.map),true);
                        mSound.PlayTip("rabbitgohome_sound", "game-tips2-2-4-3");
                        stopExitT();
                    }
                    //*/
                }
                ///*
                //Debug.Log("isHitGrid : " + isHitGrid + ";isFull : " + isFull);
                if (isHitGrid && !isFull)
                {
                    StopCoroutine("TPlayDemo");
                    Vector3 pos  = RaGrid.gSlect.transform.localPosition;
                    ShiZiLu shiziluC = null;
                    foreach (string _key in mGuanka.shiziluPosList.Keys)
                    {
                        if(mGuanka.shiziluPosList[_key] == pos)
                        {
                            shiziluC = mGuanka.shiziluDic[_key];
                            shiziluC.mMouseDown();
                            break;
                        }
                    }
                }
                else
                {
                    foreach (RaycastHit hit in hits)
                    {
                        shizilu = hit.collider.gameObject.GetComponent<ShiZiLu>();
                        if(null != shizilu)
                        {
                            //Debug.Log("shizilu.gameObject.name : " + shizilu.gameObject.name + ";currShiZiluName : " + currShiZiluName);
                        }
                        
                        if (null != shizilu && shizilu.gameObject.name != currShiZiluName)
                        {
                            ShiZiLu.gSlect = shizilu;
                            ShiZiLu.gSlect.mMouseDown();
                        }
                        
                    }
                }
                //*/
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
            if(null != RaGrid.gSlect)
            {
                Vector3 pos = RaGrid.gSlect.transform.localPosition;
                string key = null;
                foreach (string _key in mGuanka.shiziluPosList.Keys)
                {
                    if (mGuanka.shiziluPosList[_key] == pos)
                    {
                        key = _key;
                        ShiZiLu shiziluC = mGuanka.shiziluDic[key];
                        shiziluC.mMouseUp();
                        break;
                    }
                }
                //Debug.Log("isFull : " + isFull);
                if (Eventdic.Count > 0 && !isFull)
                {
                    playClickRoadSound();
                    EventGo();
                }
            }else
            {
                if (null != ShiZiLu.gSlect)
                {
                    ShiZiLu.gSlect.mMouseUp();
                    //Debug.Log("ShiZiLu.gSlect.name : " + ShiZiLu.gSlect.name);
                    playClickRoadSound();
                    EventBack(ShiZiLu.gSlect.gameObject);
                }
            }
            RaGrid.gSlect = null;
            ShiZiLu.gSlect = null;
            startImage = null;
            line = null;
        }
    }
    public  string currShiZiluName = ""; 
    private void ClickBtnDown(GameObject go)
    {
        //EventBack(go);
    }
    private void stopExitT()
    {
        StopCoroutine("TShowExit");
        endPoint.transform.localScale = Vector3.one;
    }
    
    //往回走
    private void EventBack(GameObject go)
    {
        //Debug.Log("EventBack : " + go.name + ";time : " + Time.time);
        string[] arr = go.name.Split('_');
        int length = arr.Length;
        bool isInOpen = false;
        bool isContainCurrRoad = false;
        //Debug.Log("currRoad : " + currRoad);
        for (int i = 0; i < openRoads.Count; i++)
        {
            int roadid = openRoads[i];
            //Debug.Log("openRoads : " + roadid);
            bool isInShizilu = false;
            for (int j = 1; j < arr.Length; j++)
            {
                if(arr[j] != "end")
                {
                    int shiziluId = int.Parse(arr[j]);
                    if (shiziluId == roadid)
                    {
                        isInShizilu = true;
                        //break;
                    }
                    if (shiziluId == currRoad)
                    {
                        isContainCurrRoad = true;
                    }
                }
                
            }
            if (isInShizilu && isContainCurrRoad)
            {
                isInOpen = true;
                break;
            }
        }

        if (!isInOpen) return;

        stopExitT();
        //Debug.Log("backing : ");
        isBacking = true;
        Vector3 endPos = go.transform.localPosition;
        List<RaGrid> grids = getGrids(currRoad.ToString());
        int endNode = 0;
        for (int j = 0; j < grids.Count; j++)
        {
            RaGrid grid = grids[j];
            if(grid.transform.localPosition == endPos)
            {
                endNode = j;
                break;
            }
        }
        List<Vector3> outlist = new List<Vector3>();
        for (int i = currId; i > endNode -1; i--)
        {
            RaGrid grid = grids[i];
            grid.isPath = false;
            outlist.Add(grid.transform.localPosition);
        }
        
        
        List<int> roadIds = new List<int>();
        for(int i= 1;i < length; i++)
        {
            if (arr[i] != "end")
            {
                int roadid = int.Parse(arr[i]);
                if (openRoads.Contains(roadid))
                {
                    //Debug.Log("backroadid : " + roadid);
                    List<RaGrid> currgrids = getGrids(roadid.ToString());
                    for (int j = 0; j < currgrids.Count; j++)
                    {
                        RaGrid grid = currgrids[j];
                        if (j <= endNode)
                        {
                            //grid.isPath = true;
                        }
                        else
                        {
                            grid.isPath = false;
                        }
                        BoxCollider box = grid.gameObject.GetComponent<BoxCollider>();
                        box.enabled = true;

                    }
                }
            }
            
        }
        //Debug.Log("EventBack currRoad : " + currRoad + ";endNode : " + endNode + ";currId : " + currId + ";outlist.count : " + outlist.Count);
        if (null != outlist && outlist.Count > 0)
        {
            currId = endNode;
            rabbitSpine.MoveTo(outlist, false);
            isFull = false;
            mGuanka.isFinish = false;
        }
    }
    //兔子碰到十字路后回调
    public void setOpenRoadList(GameObject shizilu)
    {
        currShiZiluName = shizilu.name;
        string[] arr = shizilu.name.Split('_');
        //Debug.Log("shizilu.name : " + shizilu.name + ";openRoads.count : " + openRoads.Count);
        if(shizilu.name == mapData.getFirstShiZiLuName(mGuanka.map))
        {
            rabbitSpine.removeContainChild();
            addShowNum(0);

        }
        int roadNum = mapData.GetMapRoadNum(mGuanka.map);
        int maxLeng = roadNum + 1;
        if(openRoads.Count <= 0)
        {
            List<int> stampShiziluIds = new List<int>();
            for (int i = 1; i < arr.Length; i++)
            {
                string str = arr[i];
                if (str != "end")
                {
                    int id = int.Parse(str);
                    if (id != 0)
                    {
                        stampShiziluIds.Add(id);
                    }

                }
            }
            if (stampShiziluIds.Count < roadNum)
            {
                for (int i = 1; i < arr.Length; i++)
                {
                    int road = int.Parse(arr[i]);
                    if (road != 0)
                    {
                        openRoads.Add(road);
                        List<RaGrid> currgrids = getGrids(road.ToString());
                        int shiziluNodeIndex = getshiziluNode(currgrids, shizilu.transform.localPosition);
                        for (int j = 0; j < currgrids.Count; j++)
                        {
                            RaGrid grid = currgrids[j];

                            if (j <= shiziluNodeIndex)
                            {
                                grid.isPath = true;
                            }
                            else
                            {
                                grid.isPath = false;
                                Vector3 pos = grid.transform.localPosition;
                                if (isInShiZiLu(pos))
                                {
                                    BoxCollider geziBox = grid.gameObject.GetComponent<BoxCollider>();
                                    geziBox.isTrigger = true;
                                    geziBox.enabled = true;
                                    //Debug.Log("grid pos " + grid.name + " ; geziBox.enabled : " + geziBox.enabled);
                                }
                            }
                        }
                    }
                }
            }else if(stampShiziluIds.Count == roadNum)
            {
                resetDataStete();
                for (int i = 1; i < maxLeng; i++)
                {
                    List<RaGrid> currgrids = getGrids(i.ToString());
                    for (int j = 0; j < currgrids.Count; j++)
                    {
                        RaGrid grid = currgrids[j];
                        if(null != grid)
                        {
                            BoxCollider box = grid.gameObject.GetComponent<BoxCollider>();
                            if (box.isTrigger)
                            {
                                box.enabled = true;
                            }
                            else
                            {
                                box.enabled = false;
                            }
                            grid.isPath = false;
                        }
                        
                    }
                }
            }

        }else
        {
            List<int> stampOpen = new List<int>();
            List<int> stampShiziluIds = new List<int>();
            for (int i = 1; i < arr.Length; i++)
            {
                string str = arr[i];
                if(str != "end")
                {
                    int id = int.Parse(str);
                    if(id != 0)
                    {
                        stampShiziluIds.Add(id);
                    }

                }
            }
            if (stampShiziluIds.Count < roadNum)
            {
               
                for (int i = 1; i < arr.Length; i++)
                {
                    int road = int.Parse(arr[i]);
                    if (road != 0 && openRoads.Contains(road))
                    {
                        List<RaGrid> currgrids = getGrids(road.ToString());
                        int shiziluNodeIndex = getshiziluNode(currgrids, shizilu.transform.localPosition);
                        //Debug.Log("shiziluNodeIndex " + shiziluNodeIndex + " ; road : " + road);
                        for (int j = 0; j < currgrids.Count; j++)
                        {
                            RaGrid grid = currgrids[j];

                            if (j <= shiziluNodeIndex)
                            {
                                grid.isPath = true;
                            }
                            else
                            {
                                grid.isPath = false;
                                Vector3 pos = grid.transform.localPosition;
                                if (isInShiZiLu(pos))
                                {
                                    BoxCollider geziBox = grid.gameObject.GetComponent<BoxCollider>();
                                    geziBox.isTrigger = true;
                                    geziBox.enabled = true;
                                    //Debug.Log("grid pos " + grid.name + " ; geziBox.enabled : " + geziBox.enabled);
                                }
                            }
                        }
                        stampOpen.Add(road);
                    }
                }
            }
            else if (stampShiziluIds.Count == roadNum && isBacking)
            {
                ///*
                resetDataStete();
                for (int i = 1; i < maxLeng; i++)
                {
                    List<RaGrid> currgrids = getGrids(i.ToString());
                    for (int j = 0; j < currgrids.Count; j++)
                    {
                        RaGrid grid = currgrids[j];
                        BoxCollider box = grid.gameObject.GetComponent<BoxCollider>();
                        if (box.isTrigger)
                        {
                            box.enabled = true;
                        }
                        else
                        {
                            box.enabled = false;
                        }
                        grid.isPath = false;
                    }
                }
                //*/
            }
        }
        

        if (shizilu.name == mGuanka.lastshizilu)
        {
            
            if(rabbitSpine.luoboNum == 7)
            {
                mGuanka.isFinish = true;
                StartCoroutine("TShowExit");
            }
            else
            {
                ErrRoad();
            }
            
        }
        else
        {
            lastShizilu = shizilu.GetComponent<ShiZiLu>();
            //Debug.Log("lastShizilu.name : " + lastShizilu.name);
        }
        //Debug.Log("setOpenRoadList : " + shiziluName);
    }
    private int getshiziluNode(List<RaGrid> currgrids,Vector3 pos)
    {
        int index = 0;
        for (int j = 0; j < currgrids.Count; j++)
        {
            RaGrid grid = currgrids[j];

            if (grid.transform.localPosition == pos)
            {
                index = j;
            }
        }
        return index;
    }
    private void resetDataStete()
    {
        inpass = false;
        isFull = false;
        mGuanka.isFinish = false;
        openRoads.Clear();
        currId = 0;
        currRoad = 0;
        lastShizilu = null;
        stopExitT();
    }
    //设置满格
    public void Full()
    {
        isFull = true;
        currId = rabbitSpine.passId;
        currShiZiluName = "";
    }
    //设置不满格
    public void noFull()
    {
        isFull = false;
        mGuanka.isFinish = false;
    }
    //播放拔萝卜
    public void playGetLuoboSound()
    {
        mSound.PlayShort("rabbitgohome_sound", "拔萝卜");
    }
    //播放丢去萝卜
    public void playPushLuoboSound()
    {
        mSound.PlayShort("rabbitgohome_sound", "萝卜放回地");
    }
    //播放点击路径
    public void playClickRoadSound()
    {
        mSound.StopTip();
        mSound.PlayShort("rabbitgohome_sound", "点击路径");
    }
    //播放走路
    public void playClickWalkSound()
    {
        mSound.PlayOnly("rabbitgohome_sound", "走路");
    }
    public void playStopWalkSound()
    {
        mSound.StopOnly();
    }
    public int currRoad = 0;
    //点击路径往前冲
    private void EventGo()
    {
        isBacking = false;
        List<Vector3> posList = null;
        //Debug.Log("EventGo : ");

        int leng = mapData.GetMapRoadNum(mGuanka.map) + 1;
        ///*
        for (int i = 1; i < leng; i++)
        {
            List<RaGrid> grids = getGrids(i.ToString());
            for (int j = 0; j < grids.Count; j++)
            {
                RaGrid grid = grids[j];
                grid.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
        //*/
        int stampCurrRoad = 0;
        int stampCurrNode = 0;
        int eventNode = 0;
        foreach (int road in Eventdic.Keys)
        {
            int node = Eventdic[road];
            
            List<RaGrid> grids = getGrids(road.ToString());
            //Debug.Log("road : " + road + ";node : " + node + ";grids.Count : " + grids.Count);
            int startIndex = 0;
            for (int j = 0; j < grids.Count; j++)
            {
                RaGrid grid = grids[j];
                if (!grid.isPath)
                {
                    
                    startIndex = j;
                    break;
                }
            }
            int shizilunum = 0;
            int length = node + 1;
            int mubiaoNum = 2;
            if (startIndex > 0)
            {
                mubiaoNum = 1;
            }
            for (int j = startIndex; j < length; j++)
            {
                RaGrid grid = grids[j];
                Vector3 pos = grid.transform.localPosition;
                if (mGuanka.shiziluPosList.ContainsValue(pos))
                {
                    shizilunum++;
                }
            }
            //Debug.Log("road : " + road + ";shizilunum : " + shizilunum + ";startIndex : " + startIndex);
            
            List<Vector3> oneList = getGridsPos(road, node);
            if (shizilunum == mubiaoNum)
            {
                posList = oneList;
                stampCurrRoad = road;
                stampCurrNode = node;
                break;
            }
        }
        //Debug.Log("currRoad : " + currRoad + ";posList.Count : " + posList + ";currId : " + currId + ";stampCurrNode : " + stampCurrNode);
        if (null != posList && posList.Count > 0)
        {
            currRoad = stampCurrRoad;
            currId = stampCurrNode;
            rabbitSpine.MoveTo(posList, false);
            DesGuide();
        }

    }
    //清空节点列表
    private void cleanGrids(string roadStr)
    {
        switch (roadStr)
        {
            case "1":
                gridList1.Clear();
                break;
            case "2":
                gridList2.Clear();
                break;
            case "3":
                gridList3.Clear();
                break;
            case "4":
                gridList4.Clear();
                break;
            case "5":
                gridList5.Clear();
                break;
        }
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
    public void setPassId(Vector3 pos)
    {
        if (currRoad == 0) return;
        List<RaGrid> grids = getGrids(currRoad.ToString());
        for (int j = 0; j < grids.Count; j++)
        {
            RaGrid grid = grids[j];
           if(grid.transform.localPosition == pos)
            {
                rabbitSpine.passId = j;
                break;
            }
        }
    }
    public int currId = 0;//该路上的索引
    private List<int> openRoads = new List<int>();//存储可行路线
    //获取往下走的节点
    private List<Vector3> getGridsPos(int road, int EventIndex)
    {

        List<RaGrid> grids = getGrids(road.ToString());
        int leng = EventIndex + 1;
        //Debug.Log("getGridsPos EventIndex : " + EventIndex + ";road : " + road);
        List<RaGrid> stampGrids = new List<RaGrid>();
        List<Vector3> outlist = new List<Vector3>();
        
        if(currId < EventIndex)
        {
            for (int i = currId; i < leng; i++)
            {
                RaGrid grid = grids[i];
                if (!grid.isPath)
                {
                    outlist.Add(grid.transform.localPosition);
                    stampGrids.Add(grid);
                }
            }
        }
        for (int j = 0; j < grids.Count; j++)
        {
            RaGrid grid = grids[j];
            BoxCollider box = grid.gameObject.GetComponent<BoxCollider>();
            if (box.isTrigger)
            {
                box.enabled = true;
            }
        }
        return outlist;
    }
    //选择错误的路段
    public void ErrRoad()
    {
        mSound.PlayTip("rabbitgohome_sound", "game-tips2-2-4-4");
        StartCoroutine(TPlayShizilu(lastShizilu));
    }
    IEnumerator TPlayDemo()
    {
        yield return new WaitForSeconds(7f);
        for (int i = 0; i < demoList.Count; i++)
        {
            ShiZiLu shizilu = demoList[i];
            StartCoroutine(TPlayShizilu(shizilu,4));
            
        }
    }
    //闪烁十字路
    IEnumerator TPlayShizilu(ShiZiLu shizilu,int times = 10)
    {
        if(null != shizilu)
        {
            for(int i = 0;i < times; i++)
            {
                shizilu.mMouseDown();
                yield return new WaitForSeconds(0.1f);
                shizilu.mMouseUp();
                yield return new WaitForSeconds(0.1f);
                shizilu.mMouseDown();
                yield return new WaitForSeconds(0.1f);
                shizilu.mMouseUp();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    IEnumerator TShowExit()
    {
        while (true)
        {
            /*
            for (int i = 0; i < 10; i++)
            {
                endPoint.transform.localScale = Vector3.one * 1.1f;
                yield return new WaitForSeconds(0.5f);
                endPoint.transform.localScale = Vector3.one;
                yield return new WaitForSeconds(0.5f);
                endPoint.transform.localScale = Vector3.one * 1.1f;
                yield return new WaitForSeconds(0.5f);
                endPoint.transform.localScale = Vector3.one;
                yield return new WaitForSeconds(0.1f);
            }
            */
            for (float j = 0; j < 1f; j += 0.1f)
            {
                endPoint.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.3f, j);
                yield return new WaitForSeconds(0.01f);
            }
            for (float j = 0; j < 1f; j += 0.1f)
            {
                endPoint.rectTransform.localScale = Vector3.Lerp(Vector3.one * 1.3f, Vector3.one, j);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    //重玩
    private void reGame()
    {
        setGameGuanka(1);
        resetLuoboData();
        setGameData(true);
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
        mGuanka.Set(guanka, mapData);
       
    }
    //重置背景和萝卜块的数据
    private void resetLuoboData()
    {
        int leng = mapData.GetMapRoadNum(mGuanka.map) + 1;
        //创建行走轨迹
        if (null != nodeGo)
        {
            GameObject.Destroy(nodeGo);

        }
        nodeGo = UguiMaker.newGameObject("nodeGo", GameContain.transform);
        for (int i = 1; i < leng; i++)
        {
            cleanGrids(i.ToString());
            createRoad(i, nodeGo.transform, mapData.GetRoadPosList(mGuanka.map, i), getGrid(i));
        }

        bg.texture = ResManager.GetTexture("rabbitgohome_texture", "bg_" + mGuanka.map);

        List<string> roadPartList = mapData.getPartName(mGuanka.map);
        for (int i = 0; i < roadPartList.Count; i++)
        {
            setRoadPartList(roadPartList[i]);
        }
        List<string> comPartList = mapData.getComPartName(mGuanka.map);
        for (int i = 0; i < comPartList.Count; i++)
        {
            setComPartList(comPartList[i]);
        }
        partLuoboNumDic.Clear();
        setRoadNumList(mGuanka.okRoad);

        for (int i = 1; i < leng; i++)
        {
            if (i != mGuanka.okRoad)
            {
                setRoadNumList(i);
            }
        }
    }
    /*
    private void resetGuankanData()
    {
        partLuoboNumDic.Clear();
        setRoadNumList(mGuanka.okRoad);
        int leng = mapData.GetMapRoadNum(mGuanka.map) + 1;
        for (int i = 1; i < leng; i++)
        {
            if (i != mGuanka.okRoad)
            {
                setRoadNumList(i);
            }
        }
    }
    */
    //设置下一关或者是再来一次
    public void NextGame()
    {
        inpass = true;
        if (isFinish())
        {
            StartCoroutine(TNextGame());
        }else
        {
            StartCoroutine(TCreatePass());
        }
        
    }
    
    private GameObject shiziluGo { get; set; }
    private GameObject luoboGo { get; set; }
    private GameObject nodeGo { get; set; }
    private GameObject pickGo { get; set; }
    private Image endPoint { get; set; }
    private List<LuoBoItem> showLuobos = new List<LuoBoItem>();
    private List<ShiZiLu> demoList = new List<ShiZiLu>();
    private ShiZiLu demoShizilu = null;
    //设置游戏数据
    private void setGameData(bool isInit)
    {
        isOver = false;
        ///*
        if (null == endPoint)
        {
            endPoint = UguiMaker.newImage("endPoint", GameContain.transform, "rabbitgohome_sprite", "Exit");
            endPoint.transform.localScale = Vector3.one;
            endPoint.transform.localPosition = new Vector3(330, -318, 0);
            BoxCollider box = endPoint.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(50, 50, 0);
            endPoint.transform.localEulerAngles = new Vector3(0, 0, -120);
        }
        endPoint.transform.localPosition = mapData.getExitPos(mGuanka.map);// 

        resetDataStete();
        CloseDoor();
        Dictionary<List<Vector3>, bool> buffer = new Dictionary<List<Vector3>, bool>();
        foreach (List<Vector3> list in mGuanka.commDic.Keys)
        {
            buffer.Add(list, false);
        }
        mGuanka.commDic = buffer;

        if (null != shiziluGo)
        {
            GameObject.Destroy(shiziluGo);
            demoShizilu = null;
        }
        shiziluGo = UguiMaker.newGameObject("shiziluGo", GameContain.transform);

        foreach (string key in mGuanka.shiziluPosList.Keys)
        {
            GameObject go = UguiMaker.newGameObject("shizilu_" + key, shiziluGo.transform);
            EventTriggerListener.Get(go).onDown = ClickBtnDown;
            ShiZiLu shizilu = go.AddComponent<ShiZiLu>();
            shizilu.setData();
            go.transform.localPosition = mGuanka.shiziluPosList[key];
            go.transform.localScale = Vector3.one * mGuanka.shiziluScaleList[key];
            BoxCollider shizilubox = go.AddComponent<BoxCollider>();
            shizilubox.size = new Vector3(80, 80);
            mGuanka.shiziluDic.Add(key, shizilu);
            if (mapData.isInDemo(mGuanka.map, key))
            {
                string[] arr = key.Split('_');
                for(int i = 0;i < arr.Length; i++)
                {
                    int id = int.Parse(arr[i]);
                    if(id == mGuanka.okRoad)
                    {
                        demoShizilu = shizilu;
                        break;
                    }
                }
                
            }
        }
        if (null != luoboGo)
        {
            GameObject.Destroy(luoboGo);
        }
        luoboGo = UguiMaker.newGameObject("luoboGo", GameContain.transform);
        currId = 0;
        checkRoadNum();
        int leng = mapData.GetMapRoadNum(mGuanka.map) + 1;
        // /创建萝卜
        for (int i = 1; i < leng; i++)
        {
            createRoadLuobo(i);
        }

        for (int i = 1; i < leng; i++)
        {
            resetAllGrid(getGrid(i));
        }

        if (null == pickGo)
        {
            pickGo = UguiMaker.newGameObject("pickGo", GameContain.transform);
            Image pickbg = UguiMaker.newImage("pickbg", pickGo.transform, "rabbitgohome_sprite", "pickbg");
            pickbg.type = Image.Type.Sliced;
            pickbg.rectTransform.sizeDelta = new Vector2(500, 80);
            pickGo.transform.localPosition = new Vector3(-377, -350, 0);
            Vector3 startPos = new Vector3(-197, 4, 0);
            for(int i = 0;i < 7; i++)
            {
                Image luoboState = UguiMaker.newImage("State_luobo_" + i, pickGo.transform, "rabbitgohome_sprite", "state_luobo");
                luoboState.transform.localPosition = startPos + new Vector3(i * 65, 0, 0);
                Image luoboshow = UguiMaker.newImage("show_luobo_" + i, pickGo.transform, "rabbitgohome_sprite", "luobo_1");
                LuoBoItem item = luoboshow.gameObject.AddComponent<LuoBoItem>();
                luoboshow.transform.localPosition = luoboState.transform.localPosition + new Vector3(-3,2,0);
                luoboshow.transform.localScale = Vector3.one * 0.7f;
                luoboshow.transform.localEulerAngles = new Vector3(0, 0, -15);
                item.close();
                showLuobos.Add(item);
            }
        }else
        {
            addShowNum(0);
        }
        if(null == mDoor)
        {
            mDoor = UguiMaker.newImage("door", GameContain.transform, "rabbitgohome_sprite", "door_close");
            mDoor.transform.localPosition = new Vector3(570, -334, 0);
        }
        if (null == rabbitSpine)
        {
            rabbitSpine = ResManager.GetPrefab("rabbitgohome_prefab", "rabbitgohome").GetComponent<RabbitGoHomeSpine>();
            GameObject ribbitGo = UguiMaker.InitGameObj(rabbitSpine.gameObject, GameContain.transform, "rabbit", new Vector3(-496, 73, 0), Vector3.one * 0.2f);
            BoxCollider spineBox = rabbitSpine.gameObject.AddComponent<BoxCollider>();
            spineBox.isTrigger = true;
            Rigidbody rig = rabbitSpine.gameObject.AddComponent<Rigidbody>();
            rig.isKinematic = true;
            spineBox.size = new Vector3(200, 200, 0);
            spineBox.center = new Vector3(0, 200, 0);


            Image shader = UguiMaker.newImage("shader", rabbitSpine.transform, "rabbitgohome_sprite", "shader");
            shader.transform.localScale = Vector3.one * 4f;
            shader.transform.localPosition = new Vector3(0, 80, 0);
            shader.transform.SetAsFirstSibling();

        }
        else
        {
            rabbitSpine.luoboNum = 0;
            rabbitSpine.transform.localScale = Vector3.one * 0.2f;
            rabbitSpine.removeContainChild();
        }
        rabbitSpine.transform.localPosition = mapData.getRabbitPos(mGuanka.map);// new Vector3(-558f, 160f, 0f);

        BoxCollider endBox = endPoint.gameObject.GetComponent<BoxCollider>();
        endBox.enabled = true;

        if(null == mPupDoor)
        {
            mPupDoor = UguiMaker.newImage("doorPup", GameContain.transform, "rabbitgohome_sprite", "doorPup");
            mPupDoor.transform.localPosition = new Vector3(634, -304, 0);
        }
        else
        {
            mPupDoor.enabled = false;
        }

        rabbitSpine.transform.SetAsLastSibling();
        mPupDoor.transform.SetAsLastSibling();

        rabbitSpine.PlaySpine("Idle", true);
        isWarking = false;

        if (isInit)
        {
            PlayDiscTips();
            //StartCoroutine("TPlayDemo");
            GuideClick(demoShizilu.transform.position, new Vector3(6,-26,0));
        }
    }
    private void PlayDiscTips()
    {
        mSound.PlayTip("rabbitgohome_sound", "game-tips2-2-4-1", 1, true);
    }
    //开门
    public void OpenDoor()
    {
        mSound.PlayShort("rabbitgohome_sound", "窗户关上的音效");
        mDoor.sprite = ResManager.GetSprite("rabbitgohome_sprite", "door_open");
        mPupDoor.enabled = true;
    }
    //关门
    public void CloseDoor(bool finish = false)
    {
        if(null != mDoor)
        {
            mDoor.sprite = ResManager.GetSprite("rabbitgohome_sprite", "door_close");
        }
        if (finish)
        {
            mSound.PlayShort("rabbitgohome_sound", "窗户关上的音效");
        }
        

    }
    //获取该路线的所有节点列表
    private List<RaGrid> getGrid(int road)
    {
        List<RaGrid> grids = null;
        switch (road)
        {
            case 1:
                grids = gridList1;
                break;
            case 2:
                grids = gridList2;
                break;
            case 3:
                grids = gridList3;
                break;
            case 4:
                grids = gridList4;
                break;
            case 5:
                grids = gridList5;
                break;
        }
        return grids;
    }
    //显示获取的萝卜
    public void addShowNum(int num)
    {
        for(int i = 0;i < 7; i++)
        {
            if(i < num)
            {
                showLuobos[i].show();
            }else
            {
                showLuobos[i].close();// = false;
            }
        }
        
    }
    public void finishCurrMove(int luoboNum)
    {
        if (luoboNum > 7)
        {
            mSound.PlayTip("rabbitgohome_sound", "tips_err_up");
            StartCoroutine(TPlayShizilu(lastShizilu));
            //TODO 提示已经超过预计的萝卜数量
        }
    }
    
    //检测不是正确路径的数量
    private void checkRoadNum()
    {
        List<List<Vector3>> oklist = GetLuoboPosList(mGuanka.okRoad, mGuanka.map);//
        int leng = mapData.GetMapRoadNum(mGuanka.map) + 1;
        List<List<Vector3>> changedList = new List<List<Vector3>>();
        List<int> checkedList = new List<int>();
        for (int i = 1;i < leng; i++)
        {
            if(i != mGuanka.okRoad)
            {
                int currRoandNum = 0;
                List<List<Vector3>> list = GetLuoboPosList(i, mGuanka.map);
                for(int j = 0;j < list.Count; j++)
                {
                    List<Vector3> poss = list[j];
                    if (partLuoboNumDic.ContainsKey(poss))
                    {
                        //Debug.Log("part  : " + j + ";currRoandNum : " + partLuoboNumDic[poss]);
                        currRoandNum += partLuoboNumDic[poss];
                    }
                }
                //Debug.Log("road : " + i + ";currRoandNum : " + currRoandNum);
                if (currRoandNum == 7)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        List<Vector3> poss = list[j];

                        if (!oklist.Contains(poss) && !changedList.Contains(poss) && !checkedList.Contains(getPartInRoad(poss)))
                        {
                            if (partLuoboNumDic[poss] > 1)
                            {
                                partLuoboNumDic[poss] -= 1;
                            }
                            else
                            {
                                partLuoboNumDic[poss] += 1;
                            }
                            changedList.Add(poss);
                            break;
                        }
                    }
                }
                checkedList.Add(i);
            }
        }
    }
    private int getPartInRoad(List<Vector3> _poss)
    {
        int outId = 0;
        int leng = mapData.GetMapRoadNum(mGuanka.map) + 1;
        for (int i = 1; i < leng; i++)
        {
            if (i != mGuanka.okRoad)
            {
                List<List<Vector3>> list = GetLuoboPosList(i, mGuanka.map);
                for (int j = 0; j < list.Count; j++)
                {
                    List<Vector3> poss = list[j];
                    if (_poss == poss)
                    {
                        outId = i;
                        break;
                    }
                }
            }
        }
        return outId;
    }
    //创建每条路上多少个萝卜
    private float luobosacle = 0.8f;
    private void createRoadLuobo(int road)
    {
        //Debug.Log("road : " + road + ";okroad : " + mGuanka.okRoad);
        List<List<Vector3>> list = GetLuoboPosList(road, mGuanka.map);//
        List<List<Vector3>> oklist = GetLuoboPosList(mGuanka.okRoad, mGuanka.map);//
        int leng = mGuanka.partNum;
        //Debug.Log("leng : " + leng + ";okroad : " + mGuanka.okRoad);
        if (mGuanka.guanka == 2)
        {
            leng = list.Count;
        }
        for (int i = 0; i < leng; i++)
        {
            List<Vector3> posss = list[i];
            if (road == mGuanka.okRoad)
            {
                if (mGuanka.commDic.ContainsKey(posss))
                {
                    if (!mGuanka.commDic[posss])
                    {
                        int len = partLuoboNumDic[posss];// NumList[i];
                        for (int j = 0; j < len; j++)
                        {
                            createKeng(road, i, j, posss);
                        }
                        mGuanka.commDic[posss] = true;
                    }
                }
                else
                {
                    int len = partLuoboNumDic[posss];// NumList[i];
                    for (int j = 0; j < len; j++)
                    {
                        createKeng(road, i, j, posss);
                    }

                }
            }
            else
            {
                if (!oklist.Contains(posss))
                {
                    if (mGuanka.commDic.Count > 0 && mGuanka.commDic.ContainsKey(posss))
                    {
                        if (!mGuanka.commDic[posss])
                        {
                            int len = partLuoboNumDic[posss];//NumList[i];
                            for (int j = 0; j < len; j++)
                            {
                                createKeng(road, i, j, posss);
                            }
                            mGuanka.commDic[posss] = true;
                        }
                    }
                    else
                    {
                        int len = partLuoboNumDic[posss];//NumList[i];
                        for (int j = 0; j < len; j++)
                        {
                            createKeng(road, i, j, posss);
                        }
                    }
                }
            }


        }
    }
    //创建一个坑
    private void createKeng(int road,int part,int index, List<Vector3> posss)
    {
        string kengName = mapData.getkengName(mGuanka.map);
        Image luobo = UguiMaker.newImage("luobo_" + road + "_" + part + "_" + index, luoboGo.transform, "rabbitgohome_sprite", kengName);
        KengItem kengItem = luobo.gameObject.AddComponent<KengItem>();
        kengItem.setLuobo();
        luobo.transform.localScale = Vector3.one * luobosacle;
        luobo.transform.localEulerAngles = new Vector3(0, 0, 0);
        luobo.transform.localPosition = posss[index];
        BoxCollider nodeBox = luobo.gameObject.AddComponent<BoxCollider>();
        nodeBox.isTrigger = true;
        LuoBoItem item = luobo.gameObject.AddComponent<LuoBoItem>();
        nodeBox.size = new Vector3(80, 80, 0);
        nodeBox.center = new Vector3(0, 15, 0);
    }
    //重置每一天路的状态
    private void resetAllGrid(List<RaGrid> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            RaGrid grid = list[i];
            grid.isPath = false;
            BoxCollider nodeBox = grid.gameObject.GetComponent<BoxCollider>();
            nodeBox.enabled = true;
        }
    }
    //创建路径
    private void createRoad(int roadIndex, Transform parent, List<Vector3> poss, List<RaGrid> list)
    {
        float gezisize = 50f;
        //Debug.Log("createRoad : ");
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
            geziBox.enabled = false;
            if (isInShiZiLu(pos))//isInShiZiLu(pos)......................................................................调试坐标
            {
                geziBox.isTrigger = true;
                geziBox.enabled = true;
            }
            list.Add(grid);
        }
    }
    private bool isInShiZiLu(Vector3 _pos)
    {
        bool boo = false;
        foreach (string key in mGuanka.shiziluPosList.Keys)
        {
            Vector3 pos = mGuanka.shiziluPosList[key];
            if (pos == _pos)
            {
                boo = true;
                break;
            }
        }
        return boo;
    }
    private List<List<Vector3>> GetLuoboPosList(int road,int map)
    {
        List<List<Vector3>> list = new List<List<Vector3>>();
        if(map == 1 || map == 2)
        {
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
        }else
        {
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
                    list.Add(raod13);
                    break;
                case 3:
                    list.Add(raod21);
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
        }
        
        return list;
    }
    GameObject passGo { get; set; }
    List<Image> passLuobo = new List<Image>();
    IEnumerator TCreatePass()
    {
        KbadyCtl.Init();
        RectTransform mask = KbadyCtl.instance.BgEffect1_Create(new Color32(255, 185, 0, 255));
        KbadyCtl.instance.BgEffect1_Play();
        //yield return new WaitForSeconds(0.5f);
        //StartCoroutine("");
        bool is_create = false;
        Image img = UguiMaker.newImage("luobo", mask.transform, "rabbitgohome_sprite", "luobo");
        img.rectTransform.localEulerAngles = new Vector3(0, 0, 48);
        for(float i = 0; i < 1f; i += 0.01f)
        {
            if(!is_create && i > 0.7f )
            {
                is_create = true;
                StartCoroutine(TNextGame());
            }
            img.rectTransform.anchoredPosition3D = new Vector3(0, Mathf.Abs(Mathf.Sin(Mathf.PI * 4 * i)) * 100, 0);
            yield return new WaitForSeconds(0.01f);
        }


        //yield return new WaitForSeconds(2);

        KbadyCtl.instance.BgEffect1_Stop();
        yield return new WaitForSeconds(0.5f);
        if (!isOver)
        {
            PlayDiscTips();
        }
        
    }
    private bool isOver = false;
    private bool isFinish()
    {
        int stampTime = mGuanka.times;
        int stampGuanka = mGuanka.guanka;
        stampTime += 1;
        bool boo = false;
        if (stampTime >= 2)
        {
            stampGuanka += 1;
            if (stampGuanka > mGuanka.guanka_last)
            {
                boo = true;
            }
        }
        return boo;
    }
    IEnumerator TNextGame()
    {
        //yield return new WaitForSeconds(1f);
        mGuanka.times++;
        //Debug.Log("mGuanka.times : " + mGuanka.times);
        if (mGuanka.times >= 2)
        {
            mGuanka.guanka++;
            mGuanka.times = 0;
            if (mGuanka.guanka > mGuanka.guanka_last)
            {
                isOver = true;
                TopTitleCtl.instance.AddStar();
                yield return new WaitForSeconds(2f);
                GameOverCtl.GetInstance().Show(mGuanka.guanka_last, reGame);
            }
            else
            {
                //显示过度
                //StartCoroutine(TCreatePass());

                //yield return new WaitForSeconds(1f);
                setGameGuanka(mGuanka.guanka);
                resetLuoboData();
                setGameData(false);
            }

        }
        else
        {
            //显示过度
            //StartCoroutine(TCreatePass());
            //yield return new WaitForSeconds(1f);
            mGuanka.resetData(mGuanka.guanka,mapData);
            resetLuoboData();
            setGameData(false);

        }
    }
    GuideHandCtl mClickGuide;
    public void GuideClick(Vector3 _worldPos, Vector3 _localOffset)
    {
        if (mClickGuide == null)
        {
            mClickGuide = GuideHandCtl.Create(transform);
        }
        mClickGuide.transform.SetSiblingIndex(60);
        mClickGuide.GuideTipClick(0.8f, 0.7f, true, true, "hand1");
        mClickGuide.SetClickTipPos(_worldPos);
        mClickGuide.SetClickTipOffsetPos(_localOffset);
    }
    /// <summary>
    /// 停止guide click
    /// </summary>
    public void GuideStop()
    {
        if (mClickGuide != null)
        {
            mClickGuide.StopClick();
        }
    }
    public void DesGuide()
    {
        if (mClickGuide != null)
        {
            if (mClickGuide.gameObject != null)
                GameObject.Destroy(mClickGuide.gameObject);
            mClickGuide = null;
        }
    }

}
