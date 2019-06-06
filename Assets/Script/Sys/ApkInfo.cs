using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ApkInfo
{

    static Dictionary<SceneEnum, List<string>> mDicSceneAbPaths = null;

    //打开apk进入的第一个游戏
    public static SceneEnum g_begin_scene = SceneEnum.SingleAndDualNumber;

    public static LoadResourcesEnum g_load_res_type = LoadResourcesEnum.ResCopy;

    //版本标识
    public static int g_version_code_single = 1;

    public static string g_version_single//自动获取 V1.3.3.171205.01
    {
        get
        {
            return
                "V1.0.3." + 
                System.DateTime.Now.Year.ToString().Substring(2, 2) + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + System.DateTime.Now.Day.ToString().PadLeft(2, '0') + "." + 
                g_version_code_single.ToString().PadLeft(2, '0');
        }
    }


    //定义每个场景需要的assetbundle,到时打包单个apk的时候会根据这个选择资源一起打包
    static void InitDicSceneAbPaths()
    {
        mDicSceneAbPaths = new Dictionary<SceneEnum, List<string>>();

        //郭振昌
        #region 5-6岁 猴子分一分,启动3秒 ......
        mDicSceneAbPaths.Add(SceneEnum.MonkeySortOut, new List<string>() {
        "Assets/ResSprite/monkeysortout_sprite",
        "Assets/ResPrefab/monkeysortout_prefab",
        "Assets/ResSound/monkeysortout_sound",
        "Assets/ResSound/bgmusic_loop0",
        });
        #endregion
        #region 5-6岁 生日蛋糕 ......
        mDicSceneAbPaths.Add(SceneEnum.BirthdayCake, new List<string>() {
        "Assets/ResSprite/birthdaycake_sprite",
        "Assets/ResPrefab/birthdaycake_prefab",
        "Assets/ResPrefab/effect_okbtn",
        "Assets/ResSound/birthdaycake_sound",
        "Assets/ResSound/bgmusic_loop0",
        
        });
        #endregion
        #region 5-6岁 电视直播间(动物守恒) ......
        mDicSceneAbPaths.Add(SceneEnum.TvRoom, new List<string>() {
        "Assets/ResSprite/tvroom_sprite",
        "Assets/ResPrefab/tvroom_prefab",
        "Assets/ResSound/tvroom_sound",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResTexture/tvroom_texture",

        });
        #endregion
        #region 滑轮室里的鸡 ......
        mDicSceneAbPaths.Add(SceneEnum.ChookPk, new List<string>() {
        "Assets/ResSprite/chookpk_sprite",
        "Assets/ResSprite/number_color",
        "Assets/ResTexture/chookpk_texture",
        "Assets/ResPrefab/chookpk_prefab",
        "Assets/ResSound/chookpk_sound",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/number_sound",

        });
        #endregion
        #region 动物聚会 ......
        mDicSceneAbPaths.Add(SceneEnum.AnimalParty, new List<string>() {
        "Assets/ResSprite/animalparty_sprite",
        "Assets/ResPrefab/animalparty_prefab",
        "Assets/ResPrefab/animalhead_prefab",
        "Assets/ResPrefab/effect_lihua",
        "Assets/ResPrefab/effect_okbtn",
        "Assets/ResPrefab/effect_paopao0",
        "Assets/ResPrefab/effect_star0",

        "Assets/ResSound/animalparty_sound",
        "Assets/ResSound/number_sound",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResSound/aa_animal_sound",
        });
        #endregion
        #region 我们都是好朋友
        mDicSceneAbPaths.Add(SceneEnum.GoodFriendCar, new List<string>() {
        "Assets/ResSprite/goodfriendcar_sprite",
        "Assets/ResSprite/number_round",
        "Assets/ResTexture/goodfriendcar_texture",
        "Assets/ResPrefab/aa_animal_person_prefab",
        "Assets/ResSound/goodfriendcar_sound",
        "Assets/ResSound/number_sound",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResSound/aa_animal_sound",


        });
        #endregion
        #region 动物数独
        mDicSceneAbPaths.Add(SceneEnum.AnimalNumOnly, new List<string>() {
        "Assets/ResSprite/animalnumonly_sprite",
         "Assets/ResSprite/aa_animal_head",
        //"Assets/ResPrefab/animalnumonly_prefab",
        "Assets/ResPrefab/effect_yun0",
        "Assets/ResPrefab/aa_animal_env_prefab",
        "Assets/ResPrefab/animalhead_prefab",
        "Assets/ResSound/animalnumonly_sound",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResSound/number_sound",
        "Assets/ResSound/aa_animal_sound",
        
        });
        #endregion
        #region 会飞的动物
        mDicSceneAbPaths.Add(SceneEnum.AnimalCanFly, new List<string>() {
        "Assets/ResSprite/animalcanfly_sprite",
         "Assets/ResSprite/aa_animal_head",
         "Assets/ResSprite/number_slim",
         
        //"Assets/ResPrefab/animalcanfly_prefab",
        "Assets/ResPrefab/aa_animal_env_prefab",
        "Assets/ResPrefab/effect_okbtn",
        "Assets/ResPrefab/effect_shine0",
        "Assets/ResPrefab/effect_quanquan",
        "Assets/ResPrefab/animalhead_prefab",


        "Assets/ResSound/animalcanfly_sound",
        "Assets/ResSound/bgmusic_loop2",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResSound/number_sound",
        "Assets/ResSound/aa_animal_sound",
        
        //"Assets/ResTexture/animalcanfly_texture",

        });
        #endregion
        #region 摘桃子
        mDicSceneAbPaths.Add(SceneEnum.PickPeach, new List<string>() {
        "Assets/ResSprite/pickpeach_sprite",
         //"Assets/ResSprite/aa_animal_head",
         //"Assets/ResSprite/number_slim",
         
        "Assets/ResPrefab/pickpeach_prefab",
        "Assets/ResPrefab/aa_animal_person_prefab",
        "Assets/ResPrefab/effect_okbtn",
        "Assets/ResPrefab/effect_green_boom",
        //"Assets/ResPrefab/effect_quanquan",
        //"Assets/ResPrefab/animalhead_prefab",


        "Assets/ResSound/pickpeach_sound",
        "Assets/ResSound/aa_animal_sound",
        "Assets/ResSound/bgmusic_loop3",
        //"Assets/ResSound/number_sound",
        //"Assets/ResSound/aa_animal_sound",


        });


        #endregion
        #region 钓小鱼
        mDicSceneAbPaths.Add(SceneEnum.Fishing, new List<string>() {
        "Assets/ResSprite/fishing_sprite",
        //"Assets/ResSprite/aa_animal_head",
        //"Assets/ResSprite/number_slim",

        "Assets/ResPrefab/fishing_prefab",
        "Assets/ResPrefab/effect_paopao",
        "Assets/ResPrefab/aa_fish_prefab",
        //"Assets/ResPrefab/effect_green_boom",
        //"Assets/ResPrefab/effect_quanquan",
        //"Assets/ResPrefab/animalhead_prefab",

        //"Assets/ResTexture/fishing_texture",
        


        //"Assets/ResSound/pickpeach_sound",
        "Assets/ResSound/fishing_sound",
        "Assets/ResSound/bgmusic_loop3",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResSound/aa_animal_effect_sound",


        });


        #endregion
        #region 图形推理,启动3秒
        mDicSceneAbPaths.Add(SceneEnum.ShapeLogic1, new List<string>() {
        "Assets/ResSprite/shapelogic_sprite",
        "Assets/ResPrefab/effect_fan",
        "Assets/ResPrefab/effect_star0",
        "Assets/ResPrefab/effect_start3",
        "Assets/ResPrefab/effect_puke",
        "Assets/ResSound/shapelogic_sound",
        "Assets/ResSound/bgmusic_loop3",

        });
        mDicSceneAbPaths.Add(SceneEnum.ShapeLogic2, new List<string>() {
        "Assets/ResSprite/shapelogic_sprite",
        "Assets/ResPrefab/effect_fan",
        "Assets/ResPrefab/effect_star0",
        "Assets/ResPrefab/effect_start3",
        "Assets/ResPrefab/effect_puke",
        "Assets/ResSound/shapelogic_sound",
        "Assets/ResSound/bgmusic_loop3",

        });
        mDicSceneAbPaths.Add(SceneEnum.ShapeLogic3, new List<string>() {
        "Assets/ResSprite/shapelogic_sprite",
        "Assets/ResPrefab/effect_fan",
        "Assets/ResPrefab/effect_star0",
        "Assets/ResPrefab/effect_start3",
        "Assets/ResPrefab/effect_puke",
        "Assets/ResSound/shapelogic_sound",
        "Assets/ResSound/bgmusic_loop3",

        });


        #endregion
        #region 找相同,启动3秒
        mDicSceneAbPaths.Add(SceneEnum.FindSameThing, new List<string>() {
           "Assets/ResSprite/findsamething_sprite",
        //"Assets/ResSprite/aa_animal_head",
        //"Assets/ResSprite/number_slim",

        "Assets/ResPrefab/findsamething_prefab",
        //"Assets/ResPrefab/effect_fan",
        "Assets/ResPrefab/effect_star4",
        //"Assets/ResPrefab/effect_start3",
        //"Assets/ResPrefab/effect_puke",
        //"Assets/ResPrefab/animalhead_prefab",

        //"Assets/ResTexture/fishing_texture",
        


        "Assets/ResSound/findsamething_sound",
        "Assets/ResSound/bgmusic_loop1",
        //"Assets/ResSound/findsamething_sound",
        //"Assets/ResSound/aa_animal_name",
        //"Assets/ResSound/aa_animal_effect_sound",


        });


        #endregion


        //刘越毅
        #region //趣味分组 ......
        mDicSceneAbPaths.Add(SceneEnum.FunnyGroup, new List<string>() {
        "Assets/ResSprite/funnygroup_sprite",
        "Assets/ResPrefab/funnygroup_prefab",
        "Assets/ResTexture/funnygroup_texture",
        "Assets/ResSound/funnygroup_sound",
        "Assets/ResSound/bgmusic_loop3",
        });
        #endregion     
        #region 三只小怪兽 ......
        mDicSceneAbPaths.Add(SceneEnum.AnimalSort, new List<string>() {
        "Assets/ResSprite/animalsort_sprite",
        "Assets/ResPrefab/animalsort_prefab",
        "Assets/ResSound/animalsort_sound",
        "Assets/ResTexture/animalsort_texture",
        "Assets/ResSound/bgmusic_loop0",
        });
        #endregion
        #region 猜猜我住哪 ......
        mDicSceneAbPaths.Add(SceneEnum.FindHome, new List<string>() {
        "Assets/ResSprite/findhome_sprite",
        "Assets/ResPrefab/findhome_prefab",
        "Assets/ResSound/findhome_sound",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResSound/aa_animal_sound",
        });
        #endregion
        #region 看谁拼得多 ......
        mDicSceneAbPaths.Add(SceneEnum.GroupCheck, new List<string>() {
        "Assets/ResSprite/groupcheck_sprite",
        "Assets/ResPrefab/groupcheck_prefab",
        "Assets/ResPrefab/effect_star0",
        "Assets/ResPrefab/effect_star2",
        "Assets/ResSound/groupcheck_sound",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResTexture/groupcheck_texture",
        });
        #endregion
        #region 我会看钟表 ......
        mDicSceneAbPaths.Add(SceneEnum.LearnTime, new List<string>() {
        "Assets/ResSprite/learntime_sprite",
        "Assets/ResTexture/learntime_texture",
        "Assets/ResPrefab/learntime_prefab",
        "Assets/ResSound/learntime_sound",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResPrefab/effect_star2",
        });
        #endregion
        #region 看图编算式 ......
        mDicSceneAbPaths.Add(SceneEnum.AnimalEquation, new List<string>() {
        "Assets/ResSprite/animalequation_sprite",
        "Assets/ResPrefab/animalequation_prefab",
        "Assets/ResSound/animalequation_sound",
        "Assets/ResSound/aa_animal_effect_sound",
        "Assets/ResTexture/animalequation_texture",
        "Assets/ResSound/bgmusic_loop0",
        });
        #endregion
        #region 一图多解 ......
        mDicSceneAbPaths.Add(SceneEnum.ManyEquation, new List<string>() {
        "Assets/ResSprite/manyequation_sprite",
        "Assets/ResPrefab/manyequation_prefab",
        "Assets/ResSound/manyequation_sound",
        //"Assets/ResTexture/manyequation_texture",
        "Assets/ResSound/bgmusic_loop0",
        });
        #endregion
        #region 找座位 ......
        mDicSceneAbPaths.Add(SceneEnum.FindPosition, new List<string>() {
        "Assets/ResSprite/findposition_sprite",
        "Assets/ResPrefab/animalhead_prefab",
        "Assets/ResPrefab/findposition_prefab",
        "Assets/ResPrefab/effect_star2",
        "Assets/ResSound/findposition_sound",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResSound/aa_animal_effect_sound",
        "Assets/ResTexture/findposition_texture",
        "Assets/ResSound/bgmusic_loop4",
        });
        #endregion
        #region 数字动物 ......
        mDicSceneAbPaths.Add(SceneEnum.AnimalNumberLogic, new List<string>() {
        "Assets/ResSprite/animalnumberlogic_sprite",
        "Assets/ResPrefab/animalnumberlogic_prefab",
        "Assets/ResPrefab/aa_animal_person_prefab",
        "Assets/ResPrefab/effect_star2",
        "Assets/ResSound/animalnumberlogic_sound",
        "Assets/ResSound/aa_animal_effect_sound",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResTexture/animalnumberlogic_texture",
        "Assets/ResSound/bgmusic_loop0",
        });
        #endregion
        #region 看花瓶 ......
        mDicSceneAbPaths.Add(SceneEnum.LookVase, new List<string>() {
        "Assets/ResSprite/lookvase_sprite",
        "Assets/ResPrefab/lookvase_prefab",
        "Assets/ResPrefab/aa_animal_person_prefab",
        "Assets/ResPrefab/effect_star2",
        "Assets/ResPrefab/effect_star0",
        "Assets/ResSound/lookvase_sound",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResSound/aa_animal_effect_sound",
        "Assets/ResTexture/lookvase_texture",
        "Assets/ResSound/bgmusic_loop1",
        });
        #endregion
        #region 食物上的图案 ......
        mDicSceneAbPaths.Add(SceneEnum.FoodShape, new List<string>() {
        "Assets/ResSprite/foodshape_sprite",
        "Assets/ResPrefab/foodshape_prefab",
        "Assets/ResPrefab/aa_animal_person_prefab",
        "Assets/ResPrefab/effect_star2",
        "Assets/ResPrefab/effect_star0",
        "Assets/ResSound/foodshape_sound",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResSound/aa_animal_effect_sound",
        "Assets/ResTexture/foodshape_texture",
        "Assets/ResSound/bgmusic_loop3",
        });
        #endregion
        #region 小兔子回家 ......
        mDicSceneAbPaths.Add(SceneEnum.RabbitGoHome, new List<string>() {
        "Assets/ResSprite/rabbitgohome_sprite",
        "Assets/ResPrefab/rabbitgohome_prefab",
        "Assets/ResPrefab/aa_animal_person_prefab",
        "Assets/ResPrefab/effect_star2",
        "Assets/ResPrefab/effect_star0",
        "Assets/ResSound/rabbitgohome_sound",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResSound/aa_animal_effect_sound",
        "Assets/ResTexture/rabbitgohome_texture",
        "Assets/ResSound/bgmusic_loop3",
        
        });
        #endregion
        #region 认识上中下 ......
        mDicSceneAbPaths.Add(SceneEnum.LearnUpMiddleDown, new List<string>() {
        "Assets/ResSprite/learnupmiddledown_sprite",
        "Assets/ResPrefab/learnupmiddledown_prefab",
        "Assets/ResPrefab/effect_star2",
        "Assets/ResPrefab/effect_star0",
        "Assets/ResSound/learnupmiddledown_sound",
        "Assets/ResTexture/learnupmiddledown_texture",
        "Assets/ResSound/bgmusic_loop3",

        });
        #endregion
        #region 1和许多 ......
        mDicSceneAbPaths.Add(SceneEnum.OneAndMore, new List<string>() {
        "Assets/ResSprite/oneandmore_sprite",
        "Assets/ResPrefab/oneandmore_prefab",
        "Assets/ResSound/oneandmore_sound",
        "Assets/ResSound/aa_animal_effect_sound",
        "Assets/ResTexture/oneandmore_texture",
        "Assets/ResSound/bgmusic_loop3",
        });
        #endregion



        //Lv
        #region //动物分类 ......
        mDicSceneAbPaths.Add(SceneEnum.AnimalClassification, new List<string>() {
        "Assets/ResSprite/animalclass_sprite",
        "Assets/ResPrefab/animalclass_prefab",
        "Assets/ResSound/animalclass_sound",
        "Assets/ResTexture/animalclass_texture",
        "Assets/ResSound/bgmusic_loop0",
        });
        #endregion
        #region //分类与统计 ......
        mDicSceneAbPaths.Add(SceneEnum.AnimalStatistics, new List<string>() {
        "Assets/ResSprite/animalstatistics_sprite",
        "Assets/ResPrefab/animalstatistics_prefab",
        "Assets/ResPrefab/animalhead_prefab",
        "Assets/ResSound/animalstatistics_sound",
        "Assets/ResTexture/animalstatistics_texture",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/checkgamebtn_sound",
        "Assets/ResSound/aa_animal_sound"
        });
        #endregion
        #region //按规律排序 ......
        mDicSceneAbPaths.Add(SceneEnum.RegularOrder, new List<string>() {
        "Assets/ResSprite/regularorder_sprite",
        "Assets/ResTexture/regularorder_texture",
        "Assets/ResPrefab/regularorder_prefab",
        "Assets/ResPrefab/animalhead_prefab",
        "Assets/ResSound/regularorder_sound",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/aa_animal_sound",
        });
        #endregion
        #region //海底奇遇(奇数偶数) ......
        mDicSceneAbPaths.Add(SceneEnum.SingleAndDualNumber, new List<string>() {
        "Assets/ResSprite/singledualnum_sprite",
        "Assets/ResPrefab/singledualnum_prefab",
        "Assets/ResPrefab/aa_littlefish_prefab",
        "Assets/ResPrefab/effect_paopao",
        "Assets/ResSound/singledualnum_sound",
        "Assets/ResTexture/singledualnum_texture",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/number_sound",
        "Assets/ResSound/checkgamebtn_sound",
        });
        #endregion
        #region //我是正方体(认识正方体) ......
        mDicSceneAbPaths.Add(SceneEnum.KnowCube, new List<string>() {
        "Assets/ResSprite/knowcube_sprite",
        "Assets/ResPrefab/knowcube_prefab",
        "Assets/ResSound/knowcube_sound",
        "Assets/ResTexture/knowcube_texture",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/checkgamebtn_sound",
        "Assets/ResPrefab/oklight_prefab",
        "Assets/ResPrefab/effect_okbtn"
        });
        #endregion
        #region //动物转转转(空) ......
        mDicSceneAbPaths.Add(SceneEnum.AnimalRotate, new List<string>() {
        "Assets/ResSprite/animalrotate_sprite",
        "Assets/ResPrefab/animalrotate_prefab",
        "Assets/ResPrefab/animalhead_prefab",
        "Assets/ResPrefab/fx_animalrotate",
        "Assets/ResPrefab/fx_movestar",
        "Assets/ResPrefab/effect_okbtn",
        "Assets/ResSound/animalrotate_sound",
        "Assets/ResTexture/animalrotate_texture",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/checkgamebtn_sound"
        });
        #endregion
        #region //学会看日历(时) ......
        mDicSceneAbPaths.Add(SceneEnum.KnowCalendar, new List<string>() {
        "Assets/ResSprite/knowcalendar_sprite",
        "Assets/ResPrefab/knowcalendar_prefab",
        "Assets/ResSound/knowcalendar_sound",
        "Assets/ResTexture/knowcalendar_texture",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResPrefab/effect_star2",
        "Assets/ResSound/number_sound",
        });
        #endregion
        #region //分小鱼(数) ......
        mDicSceneAbPaths.Add(SceneEnum.CatchFish, new List<string>() {
        "Assets/ResSprite/catchfish_sprite",
        "Assets/ResPrefab/catchfish_prefab",
        "Assets/ResSound/catchfish_sound",
        "Assets/ResTexture/catchfish_texture",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/number_sound",
        "Assets/ResSound/checkgamebtn_sound",
        "Assets/ResPrefab/effect_okbtn",
        });
        #endregion
        #region //排队来做操(数) ......
        mDicSceneAbPaths.Add(SceneEnum.LineUpCtrl, new List<string>() {
        "Assets/ResSprite/lineupctrl_sprite",
        "Assets/ResPrefab/lineupctrl_prefab",
        "Assets/ResPrefab/aa_animal_person_prefab",
        "Assets/ResSound/lineupctrl_sound",
        "Assets/ResTexture/lineupctrl_texture",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResSound/aa_good_sound",
        "Assets/ResSound/aa_animal_sound",
        "Assets/ResSprite/number_slim",
        });
        #endregion
        #region //动物的家(数) ......
        mDicSceneAbPaths.Add(SceneEnum.AnimalsHome, new List<string>() {
        "Assets/ResSprite/animalshome_sprite",
        "Assets/ResPrefab/animalshome_prefab",
        "Assets/ResPrefab/animalhead_prefab",
        "Assets/ResPrefab/effect_okbtn",
        "Assets/ResSound/animalshome_sound",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/aa_animal_sound",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResSound/number_sound",
        "Assets/ResTexture/animalshome_texture",
        });
        #endregion
        #region //我是小邮差(空) ......
        mDicSceneAbPaths.Add(SceneEnum.LittlePostman, new List<string>() {
        "Assets/ResSprite/littlepostman_sprite",
        "Assets/ResSprite/aa_animal_head",
        "Assets/ResPrefab/littlepostman_prefab",
        "Assets/ResSound/littlepostman_sound",
        "Assets/ResSound/bgmusic_loop1",
        "Assets/ResSound/aa_animal_sound",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResSound/number_sound",
        "Assets/ResTexture/littlepostman_texture",
        });
        #endregion
        #region //数字推理(大班量) ......
        mDicSceneAbPaths.Add(SceneEnum.NumberReasoning, new List<string>() {
        "Assets/ResSprite/numberreasoning_sprite",
        "Assets/ResSprite/inputnumres_sprite",
        "Assets/ResPrefab/numberreasoning_prefab",
        "Assets/ResPrefab/aa_animal_person_prefab",
        "Assets/ResSound/numberreasoning_sound",
        "Assets/ResSound/bgmusic_loop2",
        "Assets/ResSound/aa_animal_sound",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResSound/number_sound",
        "Assets/ResSound/aa_good_sound",
        "Assets/ResTexture/numberreasoning_texture",
        });
        #endregion
        #region //规律迷宫(中班量) ......
        mDicSceneAbPaths.Add(SceneEnum.RegularMaze, new List<string>() {
        "Assets/ResSprite/regularmaze_sprite",
        "Assets/ResPrefab/regularmaze_prefab",
        "Assets/ResSound/regularmaze_sound",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/aa_animal_sound",
        //"Assets/ResSound/aa_animal_name",
        "Assets/ResSound/aa_good_sound",
        "Assets/ResTexture/regularmaze_texture",
        });
        #endregion
        #region //组合图形(中班形) ......
        mDicSceneAbPaths.Add(SceneEnum.CombineGraphics, new List<string>() {
        "Assets/ResSprite/combinegraphics_sprite",
        "Assets/ResPrefab/combinegraphics_prefab",
        "Assets/ResSound/combinegraphics_sound",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/aa_good_sound",
        "Assets/ResTexture/combinegraphics_texture",
        });
        #endregion
        #region //看谁飞得快(中班时)3s ......
        mDicSceneAbPaths.Add(SceneEnum.WhoFlyFast, new List<string>() {
        "Assets/ResSprite/whoflyfast_sprite",
        "Assets/ResSprite/number_slim",
        "Assets/ResPrefab/whoflyfast_prefab",
        "Assets/ResPrefab/aa_animal_person_prefab",
        "Assets/ResPrefab/fx_movestar",
        "Assets/ResSound/whoflyfast_sound",
        "Assets/ResSound/bgmusic_loop0",
        "Assets/ResSound/aa_animal_name",
        "Assets/ResTexture/whoflyfast_texture",
        });
        #endregion
        #region //认识圆形(小班形)3s ......
        mDicSceneAbPaths.Add(SceneEnum.KnowCircular, new List<string>() {
        "Assets/ResSprite/knowcircular_sprite",
        "Assets/ResPrefab/knowcircular_prefab",
        "Assets/ResPrefab/fx_movestar",
        "Assets/ResSound/knowcircular_sound",
        "Assets/ResSound/bgmusic_loop2",
        "Assets/ResTexture/knowcircular_texture",
        });
        #endregion
        #region //认识个位十位(幼小数)......
        mDicSceneAbPaths.Add(SceneEnum.KnowOneAndTen, new List<string>() {
        "Assets/ResSprite/knowoneandten_sprite",
        "Assets/ResSprite/number_slim",
        "Assets/ResPrefab/knowoneandten_prefab",
        "Assets/ResSound/knowoneandten_sound",
        "Assets/ResSound/bgmusic_loop3",
        "Assets/ResSound/number_sound",
        "Assets/ResTexture/knowoneandten_texture",
        });
        #endregion


        //谢展鹏
        #region //认识上下 ......
        mDicSceneAbPaths.Add(SceneEnum.LearnUpDown, new List<string>() {
        "Assets/ResSprite/learnupdown_sprite",
        //"Assets/ResPrefab/learnupdown_prefab",
      //  "Assets/ResSound/learnupdown_sound",
        //"Assets/ResTexture/learnupdown_texture",
     //   "Assets/ResSound/bgmusic_loop0",
        //"Assets/ResSound/number_sound",
        });
        #endregion




    }

    public static string GetSceenName(SceneEnum scene_enum)
    {
        switch (scene_enum)
        {
            //郭振昌
            case SceneEnum.MonkeySortOut:
                return "猴子分一分";
            case SceneEnum.BirthdayCake:
                return "生日蛋糕";
            case SceneEnum.TvRoom:
                return "电视直播间";
            case SceneEnum.ChookPk:
                return "滑轮室里的鸡";
            case SceneEnum.AnimalParty:
                return "动物聚会";
            case SceneEnum.GoodFriendCar:
                return "我们都是好朋友";
            case SceneEnum.AnimalNumOnly:
                return "动物数独";
            case SceneEnum.AnimalCanFly:
                return "会飞的动物";
            case SceneEnum.PickPeach:
                return "摘桃子";
            case SceneEnum.Fishing:
                return "钓小鱼";
            case SceneEnum.ShapeLogic1:
                return "图形推理一";
            case SceneEnum.ShapeLogic2:
                return "图形推理二";
            case SceneEnum.ShapeLogic3:
                return "图形推理三";
            case SceneEnum.FindSameThing:
                return "找相同";


            //刘越毅
            case SceneEnum.FunnyGroup:
                return "趣味分组";
            case SceneEnum.AnimalSort:
                return "三个小怪兽";
            case SceneEnum.FindHome:
                return "猜猜我住哪";
            case SceneEnum.GroupCheck:
                return "看谁拼得多";
            case SceneEnum.LearnTime:
                return "我会看钟表";
            case SceneEnum.AnimalEquation:
                return "看图编算式";
            case SceneEnum.ManyEquation:
                return "一图多解";
            case SceneEnum.FindPosition:
                return "找座位";
            case SceneEnum.AnimalNumberLogic:
                return "数字动物";
            case SceneEnum.LookVase:
                return "看花瓶";
            case SceneEnum.FoodShape:
                return "食物上的图案";
            case SceneEnum.RabbitGoHome:
                return "小兔子回家";
            case SceneEnum.LearnUpMiddleDown:
                return "认识上中下";
            case SceneEnum.OneAndMore:
                return "1和许多";


            //吕洋新
            case SceneEnum.AnimalClassification:
                return "动物回家";
            case SceneEnum.AnimalStatistics:
                return "动物比多少";
            case SceneEnum.SingleAndDualNumber:
                return "奇数和偶数";
            case SceneEnum.RegularOrder:
                return "小动物和花";
            case SceneEnum.KnowCube:
                return "我是正方体";
            case SceneEnum.AnimalRotate:
                return "动物转转转";
            case SceneEnum.KnowCalendar:
                return "学会看日历";
            case SceneEnum.CatchFish:
                return "分小鱼";
            case SceneEnum.LineUpCtrl:
                return "排队来做操";
            case SceneEnum.AnimalsHome:
                return "小动物的家";
            case SceneEnum.LittlePostman:
                return "我是小邮差";
            case SceneEnum.NumberReasoning:
                return "数字推理";
            case SceneEnum.RegularMaze:
                return "规律迷宫";
            case SceneEnum.CombineGraphics:
                return "组合图形";
            case SceneEnum.WhoFlyFast:
                return "看谁飞得快";
            case SceneEnum.KnowCircular:
                return "认识圆形";
            case SceneEnum.KnowOneAndTen:
                return "认识个位和十位";


            //谢展鹏
            case SceneEnum.LearnUpDown:
                return "认识上下";

            default:
                return "";

        }

    }

    //获取该游戏是否需要定义宏，例如生日蛋糕的吹蜡烛录音功能
    public static string GetGameDefine(SceneEnum scene_enum)
    {
        switch (scene_enum)
        {
            case SceneEnum.BirthdayCake:
                return "OPEN_MIC";
        }

        return string.Empty;
    }


















    //配置打包单个游戏的时候，的启动闪屏,存放路径Assets/ScreenShort
    public static string GetScreenShort(SceneEnum scene_enum)
    {
        string shoot_screen_name = GetSceenName(scene_enum);
        if(string.IsNullOrEmpty(shoot_screen_name))
        {
            return "生日蛋糕";
        }
        else
        {
            return shoot_screen_name;
        }
   
    }


    //获取该场景需要的ab
    public static List<string> GetSceneAbPaths(SceneEnum scene_enum)
    {
        if(null == mDicSceneAbPaths)
        {
            InitDicSceneAbPaths();
        }
        if (mDicSceneAbPaths.ContainsKey(scene_enum))
            return mDicSceneAbPaths[scene_enum];
        else
        {
            //Debug.LogError("没有配置" + scene_enum + "的ab path路径" );
            return new List<string>() { };
        }
    }


}
