using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GridType
{
    Normal,//正常
    Obstacle,//障碍物
    Start,//起点
    End//终点
}

//为了格子排序 需要继承IComparable接口实现排序
public class MapGrid : IComparable//排序接口
{
    public int x;//记录坐标
    public int y;

    public int f;//总消耗
    public int g;//当前点到起点的消耗
    public int h;//当前点到终点的消耗


    public GridType type;//格子类型
    public MapGrid fatherNode;//父节点


    //排序
    public int CompareTo(object obj)     //排序比较方法 ICloneable的方法
    {
        //升序排序
        MapGrid grid = (MapGrid)obj;
        if (this.f < grid.f)
        {
            return -1;                    //升序
        }
        if (this.f > grid.f)
        {
            return 1;                    //降序
        }
        return 0;
    }

}
public class AStar : MonoBehaviour
{
    public SoundManager mSound { get; set; }
    private RawImage bg { get; set; }
    private RabbitGoHomeSpine rabbitSpine { get; set; }
    //格子大小
    public int row = 25;
    public int col = 24;
    public int size = 50;                //格子大小

    public MapGrid[,] grids;            //格子数组

    public ArrayList openList;            //开启列表
    public ArrayList closeList;            //结束列表

    private List<int> nodeState = new List<int>()
    {
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,1,1,1,1,1,1,1,0,
        0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,
        0,0,0,0,0,1,1,1,9,0,0,0,0,0,0,0,0,0,0,0,0,1,0,
        0,0,1,1,1,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,0,
        0,0,1,1,1,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,1,1,0,
        0,0,1,1,0,0,0,0,0,0,1,1,0,0,1,1,1,1,1,0,0,0,0,
        0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,1,1,1,0,0,0,0,0,
        0,0,1,1,0,0,0,0,0,0,1,0,0,0,0,1,1,0,0,0,0,0,0,
        0,0,1,1,0,0,0,0,0,0,1,0,0,0,0,1,1,0,0,0,0,0,0,
        0,0,1,1,1,1,1,1,1,1,0,0,0,0,1,1,1,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,1,1,1,0,0,0,1,1,1,1,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,1,1,1,0,0,0,
    };

    //开始,结束点位置
    private int xStart = 5;
    private int yStart = 2;

    private int xEnd = 12;
    private int yEnd = 17;
    private Stack<string> fatherNodeLocation;

    void Init()
    {
        grids = new MapGrid[row, col];    //初始化数组
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                grids[i, j] = new MapGrid();
                grids[i, j].x = i;
                grids[i, j].y = j;        //初始化格子,记录格子坐标
            }
        }
        grids[xStart, yStart].type = GridType.Start;
        grids[xStart, yStart].h = Manhattan(xStart, yStart);    //起点的 h 值

        grids[xEnd, yEnd].type = GridType.End;                    //结束点
        fatherNodeLocation = new Stack<string>();
        
        //生成障碍物
        for (int i = 0; i < nodeState.Count; i++)
        {
            int h = i % col;
            int v = (int)(i / col);
            Debug.Log("h : " + h + ";v : " + v);
            if(nodeState[i] < 1)
            {
                grids[v, h].type = GridType.Obstacle;
            }
        }
        
        openList = new ArrayList();
        openList.Add(grids[xStart, yStart]);
        closeList = new ArrayList();
        //createMap();
    }
    private void createMap()
    {
        Vector3 startPos = new Vector3(-640, 400, 0);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Color color = Color.yellow;
                if (grids[i, j].type == GridType.Start)
                {
                    color = Color.green;
                }
                else if (grids[i, j].type == GridType.End)
                {
                    color = Color.black;
                }
                else if (grids[i, j].type == GridType.Obstacle)    //障碍颜色
                {
                    color = Color.red;
                }
                else if (closeList.Contains(grids[i, j]))        //关闭列表颜色  如果当前点包含在closList里
                {
                    color = Color.yellow;
                }
                else { color = Color.gray; }

                GUI.backgroundColor = color;
                GUI.Button(new Rect(j * size, i * size, size, size), FGH(grids[i, j]));
                //Image grid = UguiMaker.newImage("node", transform, "public", "white");
                //grid.rectTransform.sizeDelta = new Vector2(size, size);
                //grid.rectTransform.localPosition = startPos + new Vector3(j * size, -i * size, 0);
                //color.a = 0.5f;
                //grid.color = color;
            }
        }
    }
    int Manhattan(int x, int y)                    //计算算法中的 h
    {
        return (int)(Mathf.Abs(xEnd - x) + Mathf.Abs(yEnd - y)) * 10;
    }

    void Awake()
    {
        mSound = gameObject.AddComponent<SoundManager>();
    }
    // Use this for initialization
    void Start()
    {
        bg = UguiMaker.newRawImage("bg", transform, "rabbitgohome_texture", "bg", false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        mSound.PlayBgAsync("bgmusic_loop3", "bgmusic_loop3", 0.3f);
        row = 14;
        col = 23;
        size = 40;

        if (null == rabbitSpine)
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
        }
        else
        {
            rabbitSpine.transform.localPosition = new Vector3(-496, 73, 0);
            rabbitSpine.luoboNum = 0;
        }
        rabbitSpine.PlaySpine("Idle", true);
        Init();
    }
    private bool isCreate = false;
    void DrawGrid()
    {
        //if(isCreate) return;

        Debug.Log("DrawGrid : ");
        //*
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Color color = Color.yellow;
                if (grids[i, j].type == GridType.Start)
                {
                    color = Color.green;
                }
                else if (grids[i, j].type == GridType.End)
                {
                    color = Color.black;
                }
                else if (grids[i, j].type == GridType.Obstacle)    //障碍颜色
                {
                    color = Color.red;
                }
                else if (closeList.Contains(grids[i, j]))        //关闭列表颜色  如果当前点包含在closList里
                {
                    color = Color.yellow;
                }
                else { color = Color.gray; }

                GUI.backgroundColor = color;
                GUI.Button(new Rect(j * size, i * size, size, size), FGH(grids[i, j]));
            }
        }
        isCreate = true;
        //*/
    }

    //每个格子显示的内容
    string FGH(MapGrid grid)
    {
        string str = "F" + grid.f + "\n";
        //str += "G" + grid.g + "\n";
        //str += "H" + grid.h + "\n";
        str += "(" + (grid.x + 1) + "," + (grid.y + 1) + ")";
        return "";
    }
    void OnGUI()
    {
        DrawGrid();
        for (int i = 0; i < openList.Count; i++)
        {
            //生成一个空行,存放开启数组
            GUI.Button(new Rect(i * size, (row + 1) * size, size, size), FGH((MapGrid)openList[i]));
        }
        //生成一个空行,存放关闭数组
        for (int j = 0; j < closeList.Count; j++)
        {
            GUI.Button(new Rect(j * size, (row + 2) * size, size, size), FGH((MapGrid)closeList[j]));
        }

        if (GUI.Button(new Rect(col * size, size, size, size), "next"))
        {
            string result = NextStep();//点击到下一步
            if(result == "Finded")
            {
                Debug.Log("openList.count : " + openList.Count);
                for (int i = 0; i < openList.Count; i++)
                {
                    MapGrid grid = (MapGrid)openList[i];
                    Debug.Log("grid.x : " + grid.x + ";grid.y : " + grid.y);
            }
            }
        }
    }

    string NextStep()
    {
        if (openList.Count == 0)                //没有可走的点
        {
            print("Over !");
            return "Over";
        }
        MapGrid grid = (MapGrid)openList[0];    //取出openList数组中的第一个点

        if (grid.type == GridType.End)            //找到终点
        {
            print("Find");
            ShowFatherNode(grid);        //找节点//打印路线
            return "Finded";
        }

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (!(i == 0 && j == 0))
                {
                    int x = grid.x + i;
                    int y = grid.y + j;
                    //x,y不超过边界,不是障碍物,不在closList里面
                    if (x >= 0 && x < row && y >= 0 && y < col && grids[x, y].type != GridType.Obstacle && !closeList.Contains(grids[x, y]))
                    {


                        //到起点的消耗
                        int g = grid.g + (int)(Mathf.Sqrt((Mathf.Abs(i) + Mathf.Abs(j))) * 10);
                        if (grids[x, y].g == 0 || grids[x, y].g > g)
                        {
                            grids[x, y].g = g;
                            grids[x, y].fatherNode = grid;        //更新父节点
                        }
                        //到终点的消耗
                        grids[x, y].h = Manhattan(x, y);
                        grids[x, y].f = grids[x, y].g + grids[x, y].h;
                        if (!openList.Contains(grids[x, y]))
                        {
                            openList.Add(grids[x, y]);            //如果没有则加入到openlist
                        }
                        openList.Sort();                        //排序
                    }
                }
            }
        }
        //添加到关闭数组
        closeList.Add(grid);
        //从open数组删除
        openList.Remove(grid);
        return "Finding";
    }


    //回溯法 递归父节点
    void ShowFatherNode(MapGrid grid)
    {
        if (grid.fatherNode != null)
        {
            //print(grid.fatherNode.x + "," + grid.fatherNode.y);
            string str = grid.fatherNode.x + "," + grid.fatherNode.y;
            fatherNodeLocation.Push(str);
            ShowFatherNode(grid.fatherNode);
        }
        /*
        if (fatherNodeLocation.Count != 0)
        {
            print(fatherNodeLocation.Pop());
        }
        */
    }


}