using UnityEngine;
using System.Collections;

public enum SceneEnum
{
    SceneDraw = -2,
    None = -1,
    SceneMain = 0,

    //游戏列表
    SceneGames = 9,
    
    //郭振昌
    MonkeySortOut = 100,//猴子分一分
    BirthdayCake = 101,//生日蛋糕
    TvRoom = 102,//电视直播间
    ChookPk = 103,//滑轮室里的鸡
    AnimalParty = 104,//动物聚会
    GoodFriendCar = 105,//我们都是好朋友
    AnimalNumOnly = 106,//动物数独
    AnimalCanFly = 107,//会飞的动物
    PickPeach = 108,//摘桃子
    Fishing = 109,//钓小鱼
    ShapeLogic1 = 111,//图形推理1
    ShapeLogic2 = 112,//图形推理2
    ShapeLogic3 = 113,//图形推理3
    FindSameThing = 114,//找相同


    //刘越毅
    FunnyGroup = 200,//小朋友分组（趣味分组）
    AnimalSort = 201,//三只小怪兽
    FindHome = 202,//找找我住哪里
    GroupCheck = 203,//拼图形
    LearnTime = 204,//我会看钟表
    AnimalEquation = 205,//看图编算式
    ManyEquation = 206,//一图多解
    FindPosition = 207,//找座位
    AnimalNumberLogic = 208,//找座位
    OneAndMore = 209,//1和许多
    LookVase = 210,//看花瓶
    RabbitGoHome = 211,//小兔子回家
    FoodShape = 212,//食物上的图案
    LearnUpMiddleDown = 213,//食物上的图案


    #region//lv
    /// <summary>
    /// 动物回家
    /// </summary>
    AnimalClassification = 300,
    /// <summary>
    /// 动物比多少(统计)
    /// </summary>
    AnimalStatistics = 301,
    /// <summary>
    /// 海底奇遇(奇数偶数)  
    /// </summary>
    SingleAndDualNumber = 302,
    /// <summary>
    /// 小猪和花(规律排序) 
    /// </summary>
    RegularOrder = 303,
    /// <summary>
    /// 我是正方形(认识正方体)
    /// </summary>
    KnowCube = 304,
    /// <summary>
    /// 小动物转转转(空)
    /// </summary>
    AnimalRotate = 305,
    /// <summary>
    /// 学会看日历
    /// </summary>
    KnowCalendar = 306,
    /// <summary>
    /// 分小鱼
    /// </summary>
    CatchFish = 307,
    /// <summary>
    /// 排队来做操
    /// </summary>
    LineUpCtrl = 308,
    /// <summary>
    /// 动物的家
    /// </summary>
    AnimalsHome = 309,
    /// <summary>
    /// 我是小邮差
    /// </summary>
    LittlePostman = 310,
    /// <summary>
    /// 数字推理(大班量)
    /// </summary>
    NumberReasoning = 311,
    /// <summary>
    /// 规律迷宫
    /// </summary>
    RegularMaze = 312,
    /// <summary>
    /// 组合图形
    /// </summary>
    CombineGraphics = 313,
    /// <summary>
    /// 看谁飞得快(中班时)
    /// </summary>
    WhoFlyFast = 314,
    /// <summary>
    /// 认识圆形
    /// </summary>
    KnowCircular = 315,
    /// <summary>
    /// 认识个位十位
    /// </summary>
    KnowOneAndTen = 316,
    #endregion



    //谢展鹏
    LearnUpDown=400,//认识上下


}
