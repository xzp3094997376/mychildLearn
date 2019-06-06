using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CG_DataInfos
{
    /// <summary>
    /// 物品名称
    /// </summary>
    public string strName = "";
    /// <summary>
    /// blocks 索引ID
    /// </summary>
    public List<int> mBlockIndexList = new List<int>();
    /// <summary>
    /// blocks ID(有相同ID)
    /// </summary>
    public List<int> mBlockIDList = new List<int>();

    public List<int> mWrongIDList = new List<int>();
    /// <summary>
    /// 允许偏移量
    /// </summary>
    public float fCheckMaxDis = 35f;
    public float fStartX = 500f;
    /// <summary>
    /// 间距
    /// </summary>
    public float fJianju = 30f;
    public void SetObjDate(int _objId)
    {
        fJianju = 30f;
        fStartX = 500f;
        mWrongIDList = new List<int>() { 0, 1 };
        switch (_objId)
        {
            case 1:
                strName = "ship";
                fCheckMaxDis = 35f;
                fJianju = 60f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5 };
                mBlockIDList = new List<int>() { 0, 1, 2, 3, 4, 5 };
                break;
            case 2:
                strName = "home";
                fCheckMaxDis = 30f;
                fJianju = 5f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
                mBlockIDList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };           
                break;
            case 3:
                strName = "flower";
                fCheckMaxDis = 30f;
                fJianju = 35f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8};
                mBlockIDList = new List<int>() { 0, 1, 2, 3, 4, 4, 4, 4, 4 };
                break;
            case 4:
                strName = "car";
                fCheckMaxDis = 30f;
                fJianju = 5f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6 };
                mBlockIDList = new List<int>() { 0, 1, 2, 2, 2, 3, 3 };
                break;
            case 5:
                strName = "dragonfly";
                fCheckMaxDis = 30f;
                fJianju = 5f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
                mBlockIDList = new List<int>() { 0, 1, 2, 2, 3, 4, 4, 3 };
                break;
            case 6:
                strName = "train";
                fCheckMaxDis = 30f;
                fJianju = 30f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
                mBlockIDList = new List<int>() { 0, 1, 2, 3, 4, 5, 5, 6 };
                break;
            case 7:
                strName = "fish";
                fCheckMaxDis = 30f;
                fJianju = 35f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6 };
                mBlockIDList = new List<int>() { 0, 1, 1, 3, 4, 4, 4 };
                break;
            case 8:
                strName = "snowman";
                fCheckMaxDis = 30f;
                fJianju = 30f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
                mBlockIDList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 6 };
                break;
            case 9:
                strName = "robot";
                fCheckMaxDis = 30f;
                fJianju = 2f;
                fStartX = 570f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                mBlockIDList = new List<int>()    { 0, 1, 2, 3, 4, 5, 5, 5, 5, 6,  6,  6,  6,  7,  7 };
                break;
            case 10:
                strName = "horse";
                fCheckMaxDis = 30f;
                fJianju = 1f;
                fStartX = 585f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
                mBlockIDList = new List<int>()    { 0, 1, 2, 3, 4, 5, 6, 6, 6, 7,  7,  7,  7,  8 };
                break;
            case 11://兔
                strName = "rabbit";
                fCheckMaxDis = 30f;
                fJianju = 5f;
                fStartX = 550f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
                mBlockIDList = new List<int>()    { 0, 1, 2, 3, 4, 4, 5, 5, 6, 6,  6,  7 };
                break;
            case 12://猫头鹰
                strName = "owl";
                fCheckMaxDis = 30f;
                fJianju = 10f;
                fStartX = 550f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                mBlockIDList = new List<int>()    { 0, 1, 2, 3, 3, 4, 5, 6, 6, 7, 8 };
                break;
            case 13://飞机
                strName = "plane";
                fCheckMaxDis = 30f;
                fJianju = 1f;
                fStartX = 580f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
                mBlockIDList = new List<int>()    { 0, 1, 2, 3, 4, 4, 5, 6, 7, 7,  8,  8 };
                break;
            case 14://羊
                strName = "sheep";
                fCheckMaxDis = 30f;
                fJianju = 1f;
                fStartX = 580f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
                mBlockIDList = new List<int>()    { 0, 1, 2, 3, 4, 5, 6, 6, 7, 7,  8,  9 };
                break;
            case 15://猪
                strName = "pig";
                fCheckMaxDis = 30f;
                fJianju = 1f;
                fStartX = 580f;
                mBlockIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
                mBlockIDList = new List<int>()    { 0, 1, 2, 2, 3, 4, 4, 5, 6, 7,  7,  8 };
                break;
            default:
                break;
        }
        fCheckMaxDis = 35f;
    }
    


}
