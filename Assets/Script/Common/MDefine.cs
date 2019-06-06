using UnityEngine;
using System.Collections;

/// <summary>
/// 动物类型
/// </summary>
public enum MAnimalType
{
    none = 0,
    /// <summary>
    /// 熊
    /// </summary>
    bruin = 1,
    /// <summary>
    /// 鸡
    /// </summary>
    chook = 2,
    /// <summary>
    /// 牛
    /// </summary>
    cow = 3,
    /// <summary>
    /// 鸭
    /// </summary>
    duck = 4,
    /// <summary>
    /// 鹅
    /// </summary>
    goose = 5,
    /// <summary>
    /// 马
    /// </summary>
    horse = 6,
    /// <summary>
    /// 豹子
    /// </summary>
    leopard = 7,
    /// <summary>
    /// 猫头鹰
    /// </summary>
    owl = 8,
    /// <summary>
    /// 猪
    /// </summary>
    pig = 9,
    /// <summary>
    /// 鸽子
    /// </summary>
    pigeon = 10,
    /// <summary>
    /// 兔子
    /// </summary>
    rabbit = 11,
    /// <summary>
    /// 羊
    /// </summary>
    sheep = 12,
    /// <summary>
    /// 燕子
    /// </summary>
    swallow = 13,
    /// <summary>
    /// 老虎
    /// </summary>
    tiger = 14,
    /// 老虎
    /// </summary>
    monkey = 15
}


public class MDefine
{
    /// <summary>
    /// 取得动物名,动物prefab
    /// </summary>
    public static string GetAnimalNameByID_EN(int _id)
    {
        string getname = "";
        switch (_id)
        {
            case 1:
                getname = "bruin";
                break;
            case 2:
                getname = "chook";
                break;
            case 3:
                getname = "cow";
                break;
            case 4:
                getname = "duck";
                break;
            case 5:
                getname = "goose";
                break;
            case 6:
                getname = "horse";
                break;
            case 7:
                getname = "leopard";
                break;
            case 8:
                getname = "owl";
                break;
            case 9:
                getname = "pig";
                break;
            case 10:
                getname = "pigeon";
                break;
            case 11:
                getname = "rabbit";
                break;
            case 12:
                getname = "sheep";
                break;
            case 13:
                getname = "swallow";
                break;
            case 14:
                getname = "tiger";
                break;
            case 15:
                getname = "monkey";
                break;
        }
        return getname;
    }

    /// <summary>
    /// 取得动物头像名
    /// </summary>
    public static string GetAnimalHeadResNameByID(int _id)
    {
        string getname = "";
        switch (_id)
        {
            case 1:
                getname = "spine_bruin";
                break;
            case 2:
                getname = "spine_chook";
                break;
            case 3:
                getname = "spine_cow";
                break;
            case 4:
                getname = "spine_duck";
                break;
            case 5:
                getname = "spine_goose";
                break;
            case 6:
                getname = "spine_horse";
                break;
            case 7:
                getname = "spine_leopard";
                break;
            case 8:
                getname = "spine_owl";
                break;
            case 9:
                getname = "spine_pig";
                break;
            case 10:
                getname = "spine_pigeon";
                break;
            case 11:
                getname = "spine_rabbit";
                break;
            case 12:
                getname = "spine_sheep";
                break;
            case 13:
                getname = "spine_swallow";
                break;
            case 14:
                getname = "spine_tiger";
                break;
            case 15:
                getname = "spine_monkey";
                break;
        }
        return getname;
    }

    /// <summary>
    /// 取得动物名
    /// </summary>
    public static string GetAnimalNameByID_CH(int _id)
    {
        string getname = "";
        switch (_id)
        {
            case 1:
                getname = "熊";
                break;
            case 2:
                getname = "公鸡";
                break;
            case 3:
                getname = "牛";
                break;
            case 4:
                getname = "鸭子";
                break;
            case 5:
                getname = "鹅";
                break;
            case 6:
                getname = "马";
                break;
            case 7:
                getname = "豹子";
                break;
            case 8:
                getname = "猫头鹰";
                break;
            case 9:
                getname = "猪";
                break;
            case 10:
                getname = "鸽子";
                break;
            case 11:
                getname = "兔子";
                break;
            case 12:
                getname = "羊";
                break;
            case 13:
                getname = "燕子";
                break;
            case 14:
                getname = "老虎";
                break;
            case 15:
                getname = "猴子";
                break;
        }
        return getname;
    }
    /// <summary>
    /// 取得动物情绪音效名
    /// yes_no:yes表示成功；no表失败
    /// 
    /// </summary>
    public static string GetAnimalEffectSoundNameByID_CH(int _id,string yes_no)
    {
        return GetAnimalNameByID_CH(_id) + yes_no;
    }

}
