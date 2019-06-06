using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RegularOrderDefine
{

    public int[,] myMap = new int[8,6];
    /// <summary>
    /// 随机地图信息
    /// </summary>
    public void InitMapDataInfo(int _id)
    {
        switch (_id)
        {
            case 1:
                myMap = new int[8, 6]
                {
                    { 3,1,3,1,3,1 },
                    { 2,3,2,3,2,3 },
                    { 3,1,3,1,3,1 },
                    { 2,3,2,3,2,3 },
                    { 3,1,3,1,3,1 },
                    { 2,3,2,3,2,3 },
                    { 3,1,3,1,3,1 },
                    { 2,3,2,3,2,3 }
                };
                break;
            case 2:
                myMap = new int[8, 6]
                {
                    { 2,3,2,3,2,3 },
                    { 3,1,3,1,3,1 },
                    { 2,3,2,3,2,3 },
                    { 3,1,3,1,3,1 },
                    { 2,3,2,3,2,3 },
                    { 3,1,3,1,3,1 },
                    { 2,3,2,3,2,3 },
                    { 3,1,3,1,3,1 }
                };
                break;
            case 3:
                myMap = new int[8, 6]
                {
                    { 1,2,1,2,1,2 },
                    { 2,1,2,1,2,1 },
                    { 1,2,1,2,1,2 },
                    { 2,1,2,1,2,1 },
                    { 1,2,1,2,1,2 },
                    { 2,1,2,1,2,1 },
                    { 1,2,1,2,1,2 },
                    { 2,1,2,1,2,1 }
                };
                break;
            case 4:
                myMap = new int[8, 6]
                {
                    { 1,3,1,3,1,3 },
                    { 3,2,3,2,3,2 },
                    { 1,3,1,3,1,3 },
                    { 3,2,3,2,3,2 },
                    { 1,3,1,3,1,3 },
                    { 3,2,3,2,3,2 },
                    { 1,3,1,3,1,3 },
                    { 3,2,3,2,3,2 }
                };
                break;
            case 5:
                myMap = new int[8, 6]
                {
                    { 1,2,1,2,1,2 },
                    { 3,1,3,1,3,1 },
                    { 1,2,1,2,1,2 },
                    { 3,1,3,1,3,1 },
                    { 1,2,1,2,1,2 },
                    { 3,1,3,1,3,1 },
                    { 1,2,1,2,1,2 },
                    { 3,1,3,1,3,1 }
                };
                break;
            case 6:
                myMap = new int[8, 6]
                {
                    { 2,1,2,1,2,1 },
                    { 1,2,1,2,1,2 },
                    { 2,1,2,1,2,1 },
                    { 1,2,1,2,1,2 },
                    { 2,1,2,1,2,1 },
                    { 1,2,1,2,1,2 },
                    { 2,1,2,1,2,1 },
                    { 1,2,1,2,1,2 }
                };
                break;
            default:
                break;
        }
    }


    public List<Vector3> myAnserPos = new List<Vector3>();
    /// <summary>
    /// 初始化答案位置
    /// </summary>
    public void InitAnserPos()
    {
        myAnserPos.Clear();
        myAnserPos.Add(new Vector3(200f + offset, 260f, 0f));
        myAnserPos.Add(new Vector3(360f + offset, 148f, 0f));
        myAnserPos.Add(new Vector3(177f + offset, 115f, 0f));
        myAnserPos.Add(new Vector3(283f + offset, 45f, 0f));
        myAnserPos.Add(new Vector3(412f + offset, 19f, 0f));
        myAnserPos.Add(new Vector3(190f + offset, -57f, 0f));
        myAnserPos.Add(new Vector3(332f + offset, -111f, 0f));
        myAnserPos.Add(new Vector3(170f + offset, -190f, 0f));
        myAnserPos.Add(new Vector3(295f + offset, -253f, 0f));
        myAnserPos.Add(new Vector3(407f + offset, -211f, 0f));
        myAnserPos.Add(new Vector3(373f + offset, 269f, 0f));
    }

    private float offset = -50f;
}



public class RegularOrderPoint
{
    public int x = 0;
    public int y = 0;
    public RegularOrderPoint(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}
