using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleDualNum_MiniMap
{

    private List<int> miniMap = new List<int>();

    public List<int> GetMap(int _id)
    {
        #region//map
        switch (_id)
        {
            case 1:
                //miniMap = new List<int>()
                //{
                //    0, 1, 2,
                //    0, 4, 3,
                //    0, 5, 0
                //};
                miniMap = new List<int>() { 1, 2, 5, 4, 7 };
                break;
            case 2:
                //miniMap = new List<int>()
                //{
                //    0, 5, 0,
                //    0, 0, 4,
                //    1, 2, 3
                //};
                miniMap = new List<int>() { 6, 7, 8, 5, 2 };
                break;
            case 3:
                //miniMap = new List<int>()
                //{
                //    0, 5, 0,
                //    1, 4, 3,
                //    0, 2, 0
                //};
                miniMap = new List<int>() { 3, 7, 5, 4, 2 };
                break;
            case 4:
                //miniMap = new List<int>()
                //{
                //    0, 0, 1,
                //    3, 2, 0,
                //    0, 4, 5
                //};
                miniMap = new List<int>() { 2, 4, 3, 7, 8 };
                break;
            case 5:
                //miniMap = new List<int>()
                //{
                //    1, 0, 0,
                //    2, 3, 0,
                //    0, 4, 5
                //};
                miniMap = new List<int>() { 0, 3, 4, 7, 8 };
                break;
            case 6:
                //miniMap = new List<int>()
                //{
                //    0, 0, 1,
                //    0, 3, 2,
                //    5, 4, 0
                //};
                miniMap = new List<int>() { 2, 5, 4, 7, 6 };
                break;
            case 7:
                //miniMap = new List<int>()
                //{
                //    0, 0, 1,
                //    0, 2, 0,
                //    5, 4, 3
                //};
                miniMap = new List<int>() { 2, 4, 8, 7, 6 };
                break;
            case 8:
                //miniMap = new List<int>()
                //{
                //    5, 4, 3,
                //    0, 2, 0,
                //    0, 1, 0
                //};
                miniMap = new List<int>() { 7, 4, 2, 1, 0 };
                break;
            case 9:
                //miniMap = new List<int>()
                //{
                //    0, 2, 3,
                //    1, 0, 4,
                //    0, 0, 5
                //};
                miniMap = new List<int>() { 3, 1, 2, 5, 8 };
                break;
            case 10:
                //miniMap = new List<int>()
                //{
                //    1, 3, 0,
                //    2, 0, 4,
                //    0, 5, 0
                //};
                miniMap = new List<int>() { 0, 3, 1, 5, 7 };
                break;
        }
        #endregion
        return miniMap;
    }

}
