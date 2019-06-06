using UnityEngine;
using System.Collections;

/// <summary>
/// 角度类型
/// </summary>
public enum AnimalRotateType
{
    //angle0 = 0,
    //angle45 = 1,
    //angle90 = 2,
    //angle135 = 3,
    //angle180 = 4,
    //angle225 = 5,
    //angle270 = 6,
    //angle315 = 7
    angle0 = 0,
    angle90 = 1,
    angle180 = 2,
    angle270 = 3,
}



public class AnimalRotateDefine
{
    /// <summary>
    /// 一次旋转的角度
    /// </summary>
    public static float fRotateIndex = 90f;

    public static int nRotateMaxType = 3;

}
