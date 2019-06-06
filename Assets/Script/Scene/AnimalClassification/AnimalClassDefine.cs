using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 动物类型
/// </summary>
public enum AnimalClassType
{
    None = 0,
    /// <summary>
    /// 牛
    /// </summary>
    Cow = 1,
    /// <summary>
    /// 燕子
    /// </summary>
    Swallow = 2,
    /// <summary>
    /// 熊
    /// </summary>
    Bear = 3,
    /// <summary>
    /// 鸡
    /// </summary>
    Chicken = 4,
    /// <summary>
    /// 羊
    /// </summary>
    Sheep = 5,
    /// <summary>
    /// 老虎
    /// </summary>
    Tiger = 6,
    /// <summary>
    /// 猫头鹰
    /// </summary>
    Owl = 7,
    /// <summary>
    /// 马
    /// </summary>
    Horse = 8,
    /// <summary>
    /// 鸽子
    /// </summary>
    Dove = 9,
    /// <summary>
    /// 鸭子
    /// </summary>
    Duck = 10,
    /// <summary>
    /// 兔子
    /// </summary>
    Rabbit = 11,
    /// <summary>
    /// 豹子
    /// </summary>
    Leopard = 12,
    /// <summary>
    /// 鹅
    /// </summary>
    Geese = 13,
    /// <summary>
    /// 猪
    /// </summary>
    Pig = 14
}


/// <summary>
/// 属性类型
/// </summary>
public enum AnimalValueType
{
    None = 0,
    HaveWings = 1,
    NoWings = 2,

    HaveAngle = 3,
    NoAngle = 4,

    /// <summary>
    /// 可以游泳
    /// </summary>
    CanSwim = 5,
    /// <summary>
    /// 不可以游泳
    /// </summary>
    CantSwim = 6,

    /// <summary>
    /// 可以飞
    /// </summary>
    CanFly = 7,
    /// <summary>
    /// 不可以飞
    /// </summary>
    CantFly = 8,

    /// <summary>
    /// 正面
    /// </summary>
    ZhengMian = 9,
    /// <summary>
    /// 侧面
    /// </summary>
    CeMian = 10,
    

    /// <summary>
    /// 在天空
    /// </summary>
    OnSky = 20,
    /// <summary>
    /// 在地上
    /// </summary>
    OnGround = 21,
    /// <summary>
    /// 在水里
    /// </summary>
    OnWater = 22,

    /// <summary>
    /// 家禽
    /// </summary>
    Poultry = 30,
    /// <summary>
    /// 家畜
    /// </summary>
    Livestock = 31,
    /// <summary>
    /// 飞禽
    /// </summary>
    Birds = 32,
    /// <summary>
    /// 野兽
    /// </summary>
    Beast = 33
}


public class AnimalClassDefine
{
    public Dictionary<int, List<int>> mDicDataInfos = new Dictionary<int, List<int>>();

    public void InitDicDataInfos()
    {
        //第一关
        List<int> animals1 = new List<int>() { 2, 4, 7, 9, 10, 13 };
        List<int> animals2 = new List<int>() { 1, 3, 5, 6, 8, 11, 12, 14 };

        List<int> animals3 = new List<int>() { 1, 5 };
        List<int> animals4 = new List<int>() { 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

        List<int> animals5 = new List<int>() { 10, 13 };
        List<int> animals6 = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 12, 14 };

        List<int> animals7 = new List<int>() { 2, 7, 9};
        List<int> animals8 = new List<int>() { 1, 3, 4, 5, 6, 8, 11, 12,13, 14 };

        List<int> animals9 = new List<int>() { 1, 3, 5, 6, 7, 11, 12, 14 };
        List<int> animals10 = new List<int>() { 2, 4, 8, 9, 10, 13 };


        //第二关 鸭子/鹅 陆地/天空
        List<int> animals20 = new List<int>() { 2, 7, 9 };

        List<int> animals21 = new List<int>() { 1, 3, 4, 5, 6, 8, 11, 12, 14 };
        List<int> animals22 = new List<int>() { 10, 13 };

        List<int> animals23 = new List<int>() { 1, 3, 4, 5, 6, 8, 10, 11, 12, 14 };
        List<int> animals24 = new List<int>() { 13 };

        List<int> animals25 = new List<int>() { 1, 3, 4, 5, 6, 8, 11, 12, 13, 14 };
        List<int> animals26 = new List<int>() { 10 };

        //第三关
        List<int> animals30 = new List<int>() { 4, 9, 10, 13 };
        List<int> animals31 = new List<int>() { 1, 5, 8, 11, 14 };
        List<int> animals32 = new List<int>() { 2, 7 };
        List<int> animals33 = new List<int>() { 3, 6, 12 };

        List<int> animals34 = new List<int>() { 4, 10, 13 };
        List<int> animals35 = new List<int>() { 2, 7, 9 };

        mDicDataInfos.Add(1, animals1);
        mDicDataInfos.Add(2, animals2);
        mDicDataInfos.Add(3, animals3);
        mDicDataInfos.Add(4, animals4);
        mDicDataInfos.Add(5, animals5);
        mDicDataInfos.Add(6, animals6);
        mDicDataInfos.Add(7, animals7);
        mDicDataInfos.Add(8, animals8);
        mDicDataInfos.Add(9, animals9);
        mDicDataInfos.Add(10, animals10);

        mDicDataInfos.Add(20, animals20);
        mDicDataInfos.Add(21, animals21);
        mDicDataInfos.Add(22, animals22);
        mDicDataInfos.Add(23, animals23);
        mDicDataInfos.Add(24, animals24);
        mDicDataInfos.Add(25, animals25);
        mDicDataInfos.Add(26, animals26);

        mDicDataInfos.Add(30, animals30);
        mDicDataInfos.Add(31, animals31);
        mDicDataInfos.Add(32, animals32);
        mDicDataInfos.Add(33, animals33);
        mDicDataInfos.Add(34, animals34);
        mDicDataInfos.Add(35, animals35);
    }
}



/// <summary>
/// station达成条件信息
/// </summary>
public class AnimalStationValue
{
    public int nID = 0;
    public AnimalValueType mValueType = AnimalValueType.None;
    public AnimalClass_Station mStation = null;
    public List<AnimalClass_Animal> mAnimalList = new List<AnimalClass_Animal>();
    public string strTipText = "";
    public string GetSoundName()
    {
        string strsound = "";
        int typeid = (int)mValueType;
        switch (typeid)
        {
            #region//lv 1
            case 1:
                strsound = "sound_suc_chibang";
                strTipText = "tag_1_2";
                break;
            case 2:
                strsound = "sound_suc_chibang";
                strTipText = "tag_2_2";
                break;
            case 3:
                strsound = "sound_suc_jiao";
                strTipText = "tag_3_1";
                break;
            case 4:
                strsound = "sound_suc_jiao";
                strTipText = "tag_4_1";
                break;
            case 5:
                strsound = "sound_suc_swim";
                strTipText = "tag_5_1";
                break;
            case 6:
                strsound = "sound_suc_swim";
                strTipText = "tag_6_1";
                break;
            case 7:
                strsound = "sound_suc_fly";
                strTipText = "tag_7_1";
                break;
            case 8:
                strsound = "sound_suc_fly";
                strTipText = "tag_8_1";
                break;
            case 9:
                strsound = "sound_suc_face";
                strTipText = "tag_9_1";
                break;
            case 10:
                strsound = "sound_suc_face";
                strTipText = "tag_10_1";
                break;
            #endregion

            #region//lv 2
            case 20:
                strsound = "sound_suc_lv2";
                strTipText = "tag_20_1";
                break;
            case 21:
                strsound = "sound_suc_lv2";
                strTipText = "tag_21_1";
                break;
            case 22:
                strsound = "sound_suc_lv2";
                strTipText = "tag_22_1";
                break;

            case 23:
                strsound = "sound_suc_lv2";
                strTipText = "tag_21_1";
                break;
            case 24:
                strsound = "sound_suc_lv2";
                strTipText = "tag_22_1";
                break;

            case 25:
                strsound = "sound_suc_lv2";
                strTipText = "tag_21_1";
                break;
            case 26:
                strsound = "sound_suc_lv2";
                strTipText = "tag_22_1";
                break;
            #endregion

            #region//lv 3
            case 30:
                strsound = "sound_suc_lv3";
                strTipText = "tag_30_1";//"家禽";
                break;
            case 31:
                strsound = "sound_suc_lv3";
                strTipText = "tag_31_1";//"家畜";
                break;
            case 32:
                strsound = "sound_suc_lv3";
                strTipText = "tag_32_1";//"飞禽";
                break;
            case 33:
                strsound = "sound_suc_lv3";
                strTipText = "tag_33_1";//"野兽";
                break;
            case 34:
                strsound = "sound_suc_lv3";
                strTipText = "tag_30_1";//"家禽";
                break;
            case 35:
                strsound = "sound_suc_lv3";
                strTipText = "tag_32_1";//"飞禽";
                break;
            #endregion
        }
        return strsound;
    }
}

/// <summary>
/// station 限制点
/// </summary>
[System.Serializable]
public class LimiteVetect
{
    public float fLeft = -100f;
    public float fRight = 100f;
    public float fTop = 100f;
    public float fDown = -100f;
    public LimiteVetect(float l,float r,float t,float d)
    {
        fLeft = l;
        fRight = r;
        fTop = t;
        fDown = d;
    }
}


