using UnityEngine;
using System.Collections;

public class GlobalParam
{
    public static int screen_height { get { return 800; } }
    public static int screen_width { get { return (int)(800f * Screen.width / Screen.height); } }

    //延时原路返回时间
    public static float drag_delay_time = 1f;

    //原路返回加速度
    public static int drag_reset_aspeed = 3;

    //返回主界面时候是加载一级，还是二级，level=1，level=2
    public static int scene_main_level = 1;


}
